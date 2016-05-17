//
// Utilisation de OnLoadComplete pour recharger Reponses apres un DetailsView1_ItemInserting
// et avant le GridView1_RowDataBound
// 
// Difficulte : on veut faire une sorte de BoundField cache pour "attraper" le TypeReponse
// qui est dans la DropDownList et n'est pas facile a recuperer.
// Seulement si on met Visible="false" le champ e.NewValues[ "TypeReponse" ] est null !!
// lors de GridView1_RowUpdating()
//
// Solution : GridView1.DataKeys[ e.RowIndex ].Value.ToString()
//

#region Using
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Sql.Web.Data;
using UserControl.Web.Controls;
#endregion

partial class Poll_Answers : PageBase
{
    static int columnDropDownListTypeReponse = 1;

    public Guid PollQuestionGUID
    {
        get
        {
            if ( ViewState[ "PollId" ] == null )
            {
                return Guid.Empty;
            }
            return ( Guid )ViewState[ "PollId" ];
        }
        set { ViewState[ "PollId" ] = value; }
    }

    private static ArrayList TextAlign()
    {
        ArrayList al = new ArrayList();
        al.Add( "Droite" );  
        al.Add( "Gauche" ); 
        return al;
    }

    private static ArrayList VerticalHorizontal()
    {
        ArrayList al = new ArrayList();
        al.Add( "Vertical" );
        al.Add( "Horizontal" );
        return al;
    }

    public static int ReponseRankMax
    {
        get
        {
            if ( HttpContext.Current.Session[ "ReponseRankMax" ] != null )
                return ( int )( HttpContext.Current.Session[ "ReponseRankMax" ] );
            return 0;
        }

        set { HttpContext.Current.Session[ "ReponseRankMax" ] = value; }
    }

    // Les champs AlignLeft et Horizontal sont sauves dans la permiere reponse 
    private static PollAnswer Reponse0
    {
        get
        {
            return ( PollAnswer )( HttpContext.Current.Session[ "Reponse0" ] );
        }

        set { HttpContext.Current.Session[ "Reponse0" ] = value; }
    }

    public PollAnswerCollection Reponses
    {
        get { return ( PollAnswerCollection )ViewState[ "Reponses" ]; }
        set { ViewState[ "Reponses" ] = value; }
    }

    protected void Page_Load( object sender, System.EventArgs e )
    {
        ValidationMessage.Text = "";

        if ( !User.IsInRole( "Administrateur" ) && !User.IsInRole( "Client" ) )
        {
            GridView1.Columns[ 0 ].Visible = false; // Edit delete column
            DetailsView1.Visible = false;
        }

        if ( Request[ "PollId" ] == null )
        {
            if ( SessionState.Questionnaire != null )
            {
                SessionState.Questions = PollQuestionCollection.GetByQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
                if ( SessionState.Questions.Count > 0 )
                {
                    PollQuestionGUID = SessionState.Questions[ 0 ].PollQuestionId;

                    // Il faut prevenir le SqlDataSource que le parametre n'est pas dans la Query ...
                    // <asp:QueryStringParameter Name="pollId" QueryStringField="PollId" />
                    //SqlDataSource1.SelectParameters.Add( "pollId", PollQuestionGUID.ToString() );
                    //SqlDataSource1.DataBind()
                    // Cela ne fonctionne pas donc on fait :
                    Response.Redirect( "~/Poll/Answers.aspx?PollId=" + PollQuestionGUID.ToString() );
                }
            }
        }
        else
        {
            try
            {
                PollQuestionGUID = new Guid( Request[ "PollId" ] );
            }
            catch
            {
                Response.Redirect( "~/Poll/Manage.aspx", true );
            }
        }

        if ( PollQuestionGUID == Guid.Empty )
        {
            Response.Redirect( "~/Poll/Manage.aspx", true );
        }

        if ( SessionState.Questionnaire != null )
        {
            HyperLinkQuestionnaire.Text = SessionState.Questionnaire.Description;
        }
        PollQuestion question = PollQuestion.GetQuestion( PollQuestionGUID );
        HyperLinkRank.Text = question.Rank.ToString();
        HyperLinkRank.NavigateUrl = "~/Poll/Questionnaire.aspx?PollQuestionId=" + question.PollQuestionId.ToString() + "&t=1";
        HyperLinkModifierQuestion.NavigateUrl = "~/Wizard/Question.aspx?PollQuestionId=" + question.PollQuestionId.ToString();
        LabelQuestion.Text = " - " + question.Question;
        LabelObligatoire.Visible = question.Obligatoire;
        LabelChoixMultiple.Visible = question.ChoixMultiple;
        LabelChoixMultipleMinMax.Visible = question.ChoixMultiple && ( question.ChoixMultipleMin > 0 ) && ( question.ChoixMultipleMax > 0 );
        LabelChoixMultipleMinMax.Text = "(" + question.ChoixMultipleMin + "/" + question.ChoixMultipleMax + ")";
        LabelChoixSimple.Visible = ( question.ChoixMultiple == false );
        LabelFin.Visible = question.Fin;
        LabelInstruction.Text = question.Instruction;
        LabelMessage.Text = question.Message;

        Reponses = PollAnswerCollection.GetByPollQuestionID( PollQuestionGUID );
        ReponseRankMax = Reponses.MaxRank();
        if ( Reponses.Count > 0 )
        {
            Reponse0 = Reponses[ 0 ];
        }

        if ( Page.IsPostBack == false )
        {
            DropDownListTextAlign.DataSource = TextAlign();
            DropDownListTextAlign.DataBind();
            if ( Reponses.Count > 0 )
            {
                DropDownListTextAlign.SelectedValue = Reponses[ 0 ].AlignLeft == true ? ( string )TextAlign()[ 1 ] : ( string )TextAlign()[ 0 ];
            }

            DropDownListVerticalHorizontal.DataSource = VerticalHorizontal();
            DropDownListVerticalHorizontal.DataBind();
            if ( Reponses.Count > 0 )
            {
                DropDownListVerticalHorizontal.SelectedValue = Reponses[ 0 ].Horizontal == true ? ( string )VerticalHorizontal()[ 1 ] : ( string )VerticalHorizontal()[ 0 ];
            }
        }
    }

    protected override void OnLoadComplete( EventArgs e )
    {
        Trace.Warn( "OnLoadComplete" );
        Reponses = PollAnswerCollection.GetByPollQuestionID( PollQuestionGUID );
    }

    protected void DetailsView1_ItemInserting( object sender, System.Web.UI.WebControls.DetailsViewInsertEventArgs e )
    {
        Trace.Warn( "DetailsView1_ItemInserting" );

        if ( e.Values[ "PollQuestionId" ] == null )
        {
            e.Values[ "PollQuestionId" ] = PollQuestionGUID;
        }

        if ( e.Values[ "PollAnswerId" ] == null )
        {
            e.Values[ "PollAnswerId" ] = Guid.NewGuid();
        }

        // Recuperer la valeur de DropDownListTypeReponse
        DropDownList ddlTypeReponse = ( DropDownList )DetailsView1.Rows[ columnDropDownListTypeReponse ].FindControl( "DropDownListTypeReponse" );
        string valeur = ddlTypeReponse.SelectedItem.Text;
        e.Values[ "TypeReponse" ] = valeur;

        if ( TypeReponse.EstTextuelle( (string) e.Values[ "TypeReponse" ] ) == false )
        {
            if ( e.Values[ "Width" ] != null )
            {
                e.Values[ "Width" ] = null;
                ValidationMessage.Text += "La largeur ne concerne que les Réponses textuelles<br/>";
                ValidationMessage.Visible = true;
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            }
            if ( e.Values[ "Rows" ] != null )
            {
                e.Values[ "Rows" ] = null;
                ValidationMessage.Text += "Le nombre de lignes ne concerne que les Réponses textuelles<br/>";
                ValidationMessage.Visible = true;
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            }
            if ( ( bool )e.Values[ "Obligatoire" ] == true )
            {
                e.Values[ "Obligatoire" ] = null;
                ValidationMessage.Text += "Obligatoire ici ne concerne que les Réponses textuelles<br/>";
                ValidationMessage.Visible = true;
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            }
        }
        else
        {
            if ( e.Values[ "Width" ] != null )
            {
                try
                {
                    int i = int.Parse( e.Values[ "Width" ].ToString() );
                    if ( i < int.Parse( Global.SettingsXml.ReponseTextuelleLargeurMin ) )
                    {
                        e.Values[ "Width" ] = Global.SettingsXml.ReponseTextuelleLargeurMin;
                    }
                    if ( i > int.Parse( Global.SettingsXml.ReponseTextuelleLargeurMax ) )
                    {
                        e.Values[ "Width" ] = Global.SettingsXml.ReponseTextuelleLargeurMax;
                    }
                }
                catch
                {
                    e.Values[ "Width" ] = null;
                    ValidationMessage.Text += "Largeur est un entier<br/>";
                    ValidationMessage.Visible = true;
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                }
            }

            if ( e.Values[ "Rows" ] != null )
            {
                try
                {
                    int i = int.Parse( e.Values[ "Rows" ].ToString() );
                    if ( i > int.Parse( Global.SettingsXml.ReponseTextuelleLigneMax ) )
                    {
                        e.Values[ "Rows" ] = Global.SettingsXml.ReponseTextuelleLigneMax;
                    }
                }
                catch
                {
                    e.Values[ "Rows" ] = null;
                    ValidationMessage.Text += "Lignes est un entier<br/>";
                    ValidationMessage.Visible = true;
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                }
            }
        }

        // BUG20100407
        // Faire comme dans ButtonReponseTextuelleOk_Click
        if ( e.Values[ "Answer" ] == null && TypeReponse.EstTextuelle( ( string )e.Values[ "TypeReponse" ] ) )
        {
            e.Values[ "Answer" ] = " "; // reponse textuelle vide
        }
        // Si elle est encore nulle c'est qu'elle n'est pas textuelle alors on met une chaine "Réponse ?"
        if ( e.Values[ "Answer" ] == null )
        {
            e.Values[ "Answer" ] = "Réponse ?";
        }
        //if ( string.IsNullOrEmpty( e.Values[ "Answer" ].ToString() ) )
        //{
        //    e.Values[ "Answer" ] = "Réponse ?";
        //}

        if ( e.Values[ "Rank" ] == null )
        {
            e.Values[ "Rank" ] = ReponseRankMax + 1;
        }
        else
        {
            try
            {
                int i = int.Parse( e.Values[ "Rank" ].ToString() );
            }
            catch
            {
                ValidationMessage.Text += "Rang est un entier<br/>";
                ValidationMessage.Visible = true;
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                e.Values[ "Rank" ] = ReponseRankMax + 1;
            }
        }

        if ( e.Values[ "Score" ] != null )
        {
            try
            {
                int i = int.Parse( e.Values[ "Score" ].ToString() );
                if ( i <= 0 )
                {
                    ValidationMessage.Text += "Score est un entier positif<br/>";
                    ValidationMessage.Visible = true;
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                    e.Values[ "Rank" ] = null;
                }
            }
            catch
            {
                ValidationMessage.Text += "Score est un entier<br/>";
                ValidationMessage.Visible = true;
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                e.Values[ "Rank" ] = null;
            }
        }

        if ( e.Values[ "Obligatoire" ] != null )
        {
            if ( ( bool )e.Values[ "Obligatoire" ] == false )
            {
                e.Values[ "Obligatoire" ] = null; // effacer
            }
        }
    }

    protected void GridView1_RowUpdating( object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e )
    {
        Trace.Warn( "GridView1_RowUpdating" );

        // Recuperer la Reponse cliquee
        Guid answerGuid = new Guid( GridView1.DataKeys[ e.RowIndex ].Value.ToString() );
        PollAnswer reponse = Reponses.FindByPollAnswerID( answerGuid );

        e.NewValues[ "TypeReponse" ] = reponse.TypeReponse;

        // BUG20100407
        // Faire comme dans ButtonReponseTextuelleOk_Click
        if ( e.NewValues[ "Answer" ] == null && TypeReponse.EstTextuelle( ( string )e.NewValues[ "TypeReponse" ] ) )
        {
            e.NewValues[ "Answer" ] = " "; // reponse textuelle vide
        }
        // Si elle est encore nulle c'est qu'elle n'est pas textuelle alors on met une chaine "Réponse ?"
        if ( e.NewValues[ "Answer" ] == null )
        {
            e.NewValues[ "Answer" ] = "Réponse ?";
        }
        //if ( string.IsNullOrEmpty( e.NewValues[ "Answer" ].ToString() ) )
        //{
        //    e.NewValues[ "Answer" ] = "Réponse ?";
        //}

        if ( TypeReponse.EstTextuelle( (string) e.NewValues[ "TypeReponse" ] ) )
        {
            if ( e.NewValues[ "Width" ] != null )
            {
                try
                {
                    int i = int.Parse( e.NewValues[ "Width" ].ToString() );
                    if ( i < int.Parse( Global.SettingsXml.ReponseTextuelleLargeurMin ) )
                    {
                        e.NewValues[ "Width" ] = Global.SettingsXml.ReponseTextuelleLargeurMin;
                    }
                    if ( i > int.Parse( Global.SettingsXml.ReponseTextuelleLargeurMax ) )
                    {
                        e.NewValues[ "Width" ] = Global.SettingsXml.ReponseTextuelleLargeurMax;
                    }
                }
                catch
                {
                    ValidationMessage.Text += "Largeur est un entier<br/>";
                    ValidationMessage.Visible = true;
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                    e.NewValues[ "Width" ] = null;
                }
            }

            if ( e.NewValues[ "Rows" ] != null )
            {
                try
                {
                    int i = int.Parse( e.NewValues[ "Rows" ].ToString() );
                    if ( i > int.Parse( Global.SettingsXml.ReponseTextuelleLigneMax ) )
                    {
                        e.NewValues[ "Rows" ] = Global.SettingsXml.ReponseTextuelleLigneMax;
                    }
                }
                catch
                {
                    ValidationMessage.Text += "Lignes est un entier<br/>";
                    ValidationMessage.Visible = true;
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                    e.NewValues[ "Rows" ] = null;
                }
            }
        }
        else // Ce n'est pas une reponse textuelle
        {
            e.NewValues[ "Rows" ] = null;
            e.NewValues[ "Width" ] = null;
            e.NewValues[ "Obligatoire" ] = null;
        }

        if ( e.NewValues[ "Rank" ] == null )
        {
            e.NewValues[ "Rank" ] = 10;
        }
        else
        {
            try
            {
                int i = int.Parse( e.NewValues[ "Rank" ].ToString() );
            }
            catch
            {
                ValidationMessage.Text += "Rang est un entier<br/>";
                ValidationMessage.Visible = true;
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                e.NewValues[ "Rank" ] = 10;
            }
        }

        if ( e.NewValues[ "Score" ] != null )
        {
            try
            {
                int i = int.Parse( e.NewValues[ "Score" ].ToString() );
                if ( i <= 0 )
                {
                    ValidationMessage.Text += "Score est un entier positif<br/>";
                    ValidationMessage.Visible = true;
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                    e.NewValues[ "Score" ] = null;
                }
            }
            catch
            {
                ValidationMessage.Text += "Score est un entier<br/>";
                ValidationMessage.Visible = true;
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                e.NewValues[ "Score" ] = null;
            }
        }
    }

    protected void GridView1_RowCommand( object sender, GridViewCommandEventArgs e )
    {
        if ( e.CommandName == "Delete" )
        {
            int index = Convert.ToInt32( e.CommandArgument );
            GridView gv = ( GridView )e.CommandSource;

            Guid answerGuid = new Guid( gv.DataKeys[ index ].Value.ToString() );
            int status = PollAnswer.Delete( answerGuid );
        }

    }

    protected void ComputeTypeReponseColumn()
    {
        Trace.Warn( "ComputeAlignementColumn" );

        if ( GridView1.Rows.Count > 0 )
        {
            int indexRow = GridView1.Rows.Count - 1;

            Guid reponseGuid = new Guid( GridView1.DataKeys[ indexRow ].Value.ToString() );
            PollAnswer reponse = Reponses.FindByPollAnswerID( reponseGuid );
            DropDownListGridView ddlQ = ( DropDownListGridView )GridView1.Rows[ indexRow ].FindControl( "DropDownListTypeReponse" );
            ddlQ.DataSource = TypeReponse.List();
            ddlQ.DataBind();
            ddlQ.Valeur = indexRow.ToString();
            ddlQ.SelectedValue = reponse.TypeReponse;
        }
    }

    protected void ButtonRangPlusUn_Click( object sender, EventArgs e )
    {
        int indexRow = 0;
        DataKeyArray dka = GridView1.DataKeys;
        foreach ( GridViewRow r in GridView1.Rows )
        {
            CheckBox cb = ( CheckBox )GridView1.Rows[ indexRow ].FindControl( "CheckBoxRangPlusMoinsUn" );
            Guid reponseGuid = new Guid( dka[ indexRow ].Value.ToString() );
            if ( cb.Checked )
            {
                PollAnswer reponse = Reponses.FindByPollAnswerID( reponseGuid );
                reponse.Rank += 1;
                PollAnswer.Update( reponse );
            }

            indexRow += 1;
        }
        GridView1.DataBind();
    }

    protected void ButtonRangMoinsUn_Click( object sender, EventArgs e )
    {
        int indexRow = 0;
        DataKeyArray dka = GridView1.DataKeys;
        foreach ( GridViewRow r in GridView1.Rows )
        {
            CheckBox cb = ( CheckBox )GridView1.Rows[ indexRow ].FindControl( "CheckBoxRangPlusMoinsUn" );
            Guid reponseGuid = new Guid( dka[ indexRow ].Value.ToString() );
            if ( cb.Checked )
            {
                PollAnswer reponse = Reponses.FindByPollAnswerID( reponseGuid );
                reponse.Rank -= 1;
                PollAnswer.Update( reponse );
            }

            indexRow += 1;
        }
        GridView1.DataBind();
    }

    protected void GridView1_RowDataBound( object sender, GridViewRowEventArgs e )
    {
        ComputeTypeReponseColumn();
    }

    protected void DropDownListTextAlign_SelectedIndexChanged( object sender, EventArgs e )
    {
        string selectedValue = DropDownListTextAlign.SelectedValue;
        if ( Reponse0 != null )
        {
            switch ( selectedValue )
            {
                case "Gauche":
                    Reponse0.AlignLeft = true;
                    break;
                case "Droite":
                    Reponse0.AlignLeft = false;
                    break;
            }
            PollAnswer.UpdateAlignLeft( Reponse0 );
        }
    }

    protected void DropDownListVerticalHorizontal_SelectedIndexChanged( object sender, EventArgs e )
    {
        string selectedValue = DropDownListVerticalHorizontal.SelectedValue;
        if ( Reponse0 != null )
        {
            switch ( selectedValue )
            {
                case "Vertical":
                    Reponse0.Horizontal = false;
                    break;
                case "Horizontal":
                    Reponse0.Horizontal = true;
                    break;
            }
            PollAnswer.UpdateVerticalHorizontal( Reponse0 );
        }
    }

    // C'est un peu lourd de faire un PollAnswer.Update de toute la reponse ...
    // mais j'ai la fleme d'ecrire une procedure stockee de plus !
    protected void DropDownListTypeReponse_SelectedIndexChanged( object sender, EventArgs e )
    {
        DropDownListGridView ddl = ( DropDownListGridView )sender;
        int indexe = int.Parse( ddl.Valeur );
        Guid reponseGuid = new Guid( GridView1.DataKeys[ indexe ].Value.ToString() );
        PollAnswer reponse = Reponses.FindByPollAnswerID( reponseGuid );
        reponse.TypeReponse = ddl.SelectedValue;
        PollAnswer.Update( reponse );
    }
}

