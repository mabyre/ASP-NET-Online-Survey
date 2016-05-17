//
// Dans MSDN :
// Lorsque la propriété DataKeyNames est définie, le contrôle GridView crée 
// automatiquement un objet DataKey pour chaque ligne du contrôle à l'aide de la ou 
// des valeurs du ou des champs spécifiés. Les objets DataKey sont ensuite ajoutés à la 
// collection DataKeys du contrôle. Normalement, la propriété DataKeys est utilisée pour 
// récupérer l'objet DataKey pour une ligne de données spécifique du contrôle GridView. 
// Toutefois, si vous devez simplement récupérer l'objet DataKey de la ligne actuellement 
// sélectionnée, vous pouvez utiliser la propriété SelectedDataKey comme un raccourci. 
//
// Il se trouve que ce n'etait pas le cas ... longue histoire
//
// Attention a ne pas trop en faire. Il ne faut pas par exemple vouloir configurer 
// SqlDataSourceQuestion.SelectCommand dans le C# et vouloir editer
// Retrouver l'ID de la ligne selectionne par l'utilisateur, l'exemple se trouve dans :
// GridViewQuestion_RowCommand
// 

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

partial class Test_Gridview : System.Web.UI.Page
{
    static int columnCommandField = 0;
    static int columnSociete = 6;
    static int columnCreationDate = 7;

    public static int QuestionRankMax
    {
        get
        {
            if ( HttpContext.Current.Session[ "QuestionRankMax" ] != null )
                return ( int )( HttpContext.Current.Session[ "QuestionRankMax" ] );
            return 0;
        }

        set { HttpContext.Current.Session[ "QuestionRankMax" ] = value; }
    }

    public string FilterBool( bool b )
    {
        return b == true ? "Vrai" : "";
    }

    protected void Page_Load( object sender, System.EventArgs e )
    {
        ValidationMessage.Text = "";

        // Choisir le premier Questionnaire a la place de l'utilisateur
        if ( IsPostBack == false )
        {
            if ( SessionState.Questionnaire == null && SessionState.Questionnaires.Count > 0 )
            {
                SessionState.Questionnaire = SessionState.Questionnaires[ 0 ];
            }
        }


        if ( !IsPostBack )
        {

            if ( Request.QueryString[ "QuestionnaireID" ] != null )
            {
                int questionnaireID = int.Parse( Request.QueryString[ "QuestionnaireID" ] );
            }

            // Si c'est l'admin et qu'il choisi un Questionnaire
            if (      User.IsInRole( "Administrateur" ) 
                 && ( Request.QueryString[ "QuestionnaireID" ] != null || SessionState.Questionnaire != null )
               )
            {
                DetailsView1.Visible = true;
            }
            else // S'il n'y a pas de Questionnaire choisie on ne sait pas ajouter une Question
            {
                DetailsView1.Visible = false;
            }

            if ( SessionState.Questionnaire != null )
                BuildDataList();

            SessionState.Questionnaire = SessionState.Questionnaires.FindByID( DropDownListQuestionnaire.QuestionnaireID );
            HiddenFieldQuestionnaireID.Value = SessionState.Questionnaire.QuestionnaireID.ToString();
        }

        Trace.Warn( "IsPostBack == true" );
    }

    private void GridViewForAdmin()
    {
        GridViewQuestion.Columns[ columnCommandField ].Visible = false;
        SqlDataSourceQuestion.SelectCommand = "SELECT * FROM PollQuestions ORDER BY Rank";
    }

    private void BuildDataList()
    {
        if ( SessionState.Questionnaire != null )
        {
            PollQuestionCollection pollQuestions = PollQuestionCollection.GetByQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
            QuestionRankMax = pollQuestions.MaxRank();

            DataListQuestion.DataSource = pollQuestions;
            DataListQuestion.DataBind();

            // Trouver les reponses
            foreach ( DataListItem dli in DataListQuestion.Items )
            {
                DataList dl = new DataList();
                dl = ( DataList )dli.FindControl( "DataListReponse" );

                HiddenField hf = new HiddenField();
                hf = ( HiddenField )dli.FindControl( "PollQuestionId" );
                Guid pollQuestionId = new Guid( hf.Value );
                PollAnswerCollection answers = PollAnswerCollection.GetByPollQuestionID( pollQuestionId );

                dl.DataSource = answers;
                dl.DataBind();
            }
        }

        Trace.Warn( "BuildDataList" );
    }

    // Se declenche quand on clique sur les boutons edit/delete/update/cancel
    protected void GridViewQuestion_RowCommand( object sender, GridViewCommandEventArgs e )
    {
        if ( e.CommandName == "Edit" )
        {
            // Trouver la Question selectionnee par l'utilisateur
            int index = Convert.ToInt32( e.CommandArgument );

            GridView gv = ( GridView )e.CommandSource;
            string q = gv.DataKeys[ index ].Value.ToString();

            PollQuestionCollection questions = PollQuestionCollection.GetByQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
            SessionState.Question = questions.FindByPollQuestionID( new Guid( q ) ); 
        }

        Trace.Warn( string.Format( "GridViewQuestion_RowCommand CommandName : {0}", e.CommandName ) );
    }

    protected void GridViewQuestion_RowUpdating( object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e )
    {
        Trace.Warn( "GridViewQuestion_RowUpdating" );

        if ( e.NewValues[ "QuestionObligatoire" ] != null )
        {
            e.NewValues[ "QuestionObligatoire" ] = Tools.StringToBoolean( e.NewValues[ "QuestionObligatoire" ].ToString() );
            e.NewValues[ "QuestionObligatoire" ] = Tools.StringBooleanFalseToNull( e.NewValues[ "QuestionObligatoire" ].ToString() );
        }

        if ( e.NewValues[ "QuestionFin" ] != null )
        {
            e.NewValues[ "QuestionFin" ] = Tools.StringToBoolean( e.NewValues[ "QuestionFin" ].ToString() );
            e.NewValues[ "QuestionFin" ] = Tools.StringBooleanFalseToNull( e.NewValues[ "QuestionFin" ].ToString() );
        }

        if ( e.NewValues[ "Question" ] == null )
        {
            e.NewValues[ "Question" ] = "Question ?";
        }

        // Les colonnes sont cachees et donc elles n'ont pas de valeurs !
        if ( CheckBoxSociete.Checked )
        {
            if ( e.NewValues[ "Societe" ] == null )
            {
                e.NewValues[ "Societe" ] = e.OldValues[ "Societe" ];
            }
        }
        else
        {
            e.NewValues[ "Societe" ] = SessionState.Question.Societe;
        }

        if ( CheckBoxDate.Checked )
        {
            if ( e.NewValues[ "CreationDate" ] == null )
            {
                e.NewValues[ "CreationDate" ] = e.OldValues[ "CreationDate" ];
            }
        }
        else
        {
            e.NewValues[ "CreationDate" ] = SessionState.Question.CreationDate;
        }

        if ( e.NewValues[ "Instruction" ] != null )
        {
            if ( Instruction.Valide( e.NewValues[ "Instruction" ].ToString() ) == Instruction.Type.Null )
                e.NewValues[ "Instruction" ] = null;
        }
    }

    protected void GridViewQuestion_RowUpdated( object sender, GridViewUpdatedEventArgs e )
    {
        Trace.Warn( "GridViewQuestion_RowUpdated" );
        BuildDataList();
    }

    protected void DetailsView1_ItemInserting( object sender, System.Web.UI.WebControls.DetailsViewInsertEventArgs e )
    {
        Trace.Warn( "DetailsView1_ItemInserting" );

        // L'utilisateur n'a pas fait de choix c'est la valeur de la checkbox
        if ( ( bool )e.Values[ "QuestionFin" ] == false )
        {
            e.Values[ "QuestionFin" ] = null; // c'est plus joli
        }
        if ( ( bool )e.Values[ "QuestionObligatoire" ] == false )
        {
            e.Values[ "QuestionObligatoire" ] = null;
        }

        if ( e.Values[ "PollQuestionID" ] == null )
        {
            e.Values[ "PollQuestionID" ] = Guid.NewGuid();
        }

        e.Values[ "QuestionnaireID " ] = SessionState.Questionnaire.QuestionnaireID;

        if ( e.Values[ "Societe" ] == null )
        {
            e.Values[ "Societe" ] = "Societe ?";
        }

        if ( e.Values[ "Rank" ] == null )
        {
            e.Values[ "Rank" ] = QuestionRankMax + 1;
        }

        if ( e.Values[ "Instruction" ] != null )
        {
            if ( Instruction.Valide( e.Values[ "Instruction" ].ToString() ) == Instruction.Type.Null )
                e.Values[ "Instruction" ] = null;
        }

        if ( e.Values[ "CreationDate" ] == null )
        {
            e.Values[ "CreationDate" ] = DateTime.Now;
        }

        if ( e.Values[ "MembreGUID" ] == null )
        {
            e.Values[ "MembreGUID" ] = ( Guid )Membership.GetUser().ProviderUserKey;
        }
    }

    protected void DetailsView1_ItemInserted( object sender, DetailsViewInsertedEventArgs e )
    {
        Trace.Warn( "DetailsView1_ItemInserted" );
        BuildDataList();
    }

    protected void DetailsView1_ItemDeleted( object sender, DetailsViewDeletedEventArgs e )
    {
        Trace.Warn( "DetailsView1_ItemDeleted" );
        BuildDataList();
    }

    protected void DropDownListQuestionnaire_SelectedIndexChanged( object sender, EventArgs e )
    {
        Trace.Warn( "DropDownListQuestionnaire_SelectedIndexChanged" );

        if ( DropDownListQuestionnaire.SelectedValue == "Tous les Questionnaires" )
        {
            SessionState.Questionnaire = null;
            //HiddenFieldQuestionnaireID.Value = "*"; ne marche pas bien sur
            GridViewForAdmin();
            DetailsView1.Visible = false;
        }
        else
        {
            SessionState.Questionnaire = Questionnaire.GetQuestionnaire( DropDownListQuestionnaire.QuestionnaireID );
            HiddenFieldQuestionnaireID.Value = SessionState.Questionnaires.FindByID( DropDownListQuestionnaire.QuestionnaireID).ToString();
            DetailsView1.Visible = true;
            GridViewQuestion.Columns[ columnCommandField ].Visible = true;
        }

        BuildDataList();
    }

    protected void ButtonListe_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Poll/List.aspx?QuestionnaireID=" + SessionState.Questionnaire.QuestionnaireID.ToString() );
    }

    
    protected void CheckBoxDate_CheckedChanged( object sender, EventArgs e )
    {
        GridViewQuestion.Columns[ columnCreationDate ].Visible = CheckBoxDate.Checked;

        if ( DropDownListQuestionnaire.SelectedValue == "Tous les Questionnaires" )
        {
            GridViewForAdmin();
        }
    }

    protected void CheckBoxSociete_CheckedChanged( object sender, EventArgs e )
    {
        GridViewQuestion.Columns[ columnSociete ].Visible = CheckBoxSociete.Checked;
        
        if ( DropDownListQuestionnaire.SelectedValue == "Tous les Questionnaires" )
        {
            GridViewForAdmin();
        }            
    }

    protected void RolloverButtonTestez_Click( object sender, EventArgs e )
    {
        if ( SessionState.Questionnaire != null )
        {
            Response.Redirect( "~/Poll/Questionnaire.aspx?QuestionnaireID=" + SessionState.Questionnaire.QuestionnaireID.ToString(), true );
        }
    }

    protected void SqlDataSourceQuestion_Updating( object sender, SqlDataSourceCommandEventArgs e )
    {
        Trace.Warn( string.Format( "SqlDataSourceQuestion_Updating CommandText : {0}", e.Command.CommandText ) );
        Trace.Warn( string.Format( "SqlDataSourceQuestion_Updating Cancel : {0}", e.Cancel.ToString() ) );
    }

    protected void GridViewQuestion_SelectedIndexChanged( object sender, EventArgs e )
    {
        if ( GridViewQuestion.SelectedDataKey != null )
            Trace.Write( string.Format( "GridViewQuestion_SelectedIndexChanged SelectedDataKey : {0}", GridViewQuestion.SelectedDataKey.Value.ToString() ) );
        else
            Trace.Write( string.Format( "GridViewQuestion_SelectedIndexChanged SelectedDataKey : NULL" ) );
    }
}

