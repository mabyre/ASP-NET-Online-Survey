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
// Pour l'instant on abandonne le mode print dans Excel de merde le formulaire à trop de
// liens pour etre utilise avec Excel de daube

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

partial class Poll_Manage : PageBase
{
    static int columnCommandField = 0;
    static int columnChoixMultipleMin = 6;
    static int columnChoixMultipleMax = 7;
    static int columnInstruction = 9;
    static int columnMessage = 10;
    static int columnSociete = 14;
    static int columnCreationDate = 15;
    static int columnAlignementQuestion = 16;
    static int columnAlignementReponse = 17;

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

    // Formulaire en mode print ou excel
    public bool FormulaireEnModePrint
    {
        get 
        {         
            if ( Request.QueryString[ "print" ] != null || Request.QueryString[ "excel" ] != null )
            {
                return true;
            }
            return false;
        }
    }

    // Formulaire en mode excel
    public bool FormulaireEnModeExcel
    {
        get
        {
            if ( Request.QueryString[ "excel" ] != null )
            {
                return true;
            }
            return false;
        }
    }

    protected override void OnPreInit( EventArgs e )
    {
        base.OnPreInit( e );

        if ( Request.QueryString[ "print" ] != null || Request.QueryString[ "excel" ] != null )
        {
            // MasterPageFile ne peut etre modifiee que dans OnPreInit()
            Page.MasterPageFile = "~/Print.Master";

            if ( Request.QueryString[ "excel" ] != null )
            {
                // AME15112009
                // La propriete Theme ne peut etre modifiee que dans OnPreInit()
                // On cherche a annuler le theme sinon les feuilles de styles sont presentes
                // dans le document telecharge par le client et lorsqu'il tente d'ouvir
                // son document avec Excel ce con d'Excel ne les trouves pas evidemment car elles
                // sont sur le serveur !!
                Page.Theme = "";
            }
        }
    }

    protected void Page_Load( object sender, System.EventArgs e )
    {
        ValidationMessage.Text = "";
        LabelValidationMessage.CssClass = "LabelValidationMessageStyle";

        // Choisir le premier Questionnaire a la place de l'utilisateur
        if ( IsPostBack == false )
        {
            if ( SessionState.Questionnaire == null && SessionState.Questionnaires.Count > 0 )
            {
                SessionState.Questionnaire = SessionState.Questionnaires[ 0 ];
            }
        }

        if ( IsPostBack == false )
        {
            if ( Request.QueryString[ "QuestionnaireID" ] != null )
            {
                int questionnaireID = int.Parse( Request.QueryString[ "QuestionnaireID" ] );
                SessionState.Questionnaire = SessionState.Questionnaires.FindByID( questionnaireID );
            }

            if ( ( User.IsInRole( "Administrateur" ) || User.IsInRole( "Client" ) ) && SessionState.Questionnaire != null )
            {
                DetailsView1.Visible = true;
            }
            else // S'il n'y a pas de Questionnaire choisie on ne sait pas ajouter une Question
            {
                DetailsView1.Visible = false;
            }

            if ( SessionState.Questionnaire != null ) // Il se peut qu'il n'y ait pas de questionnaire au depart de l'application
            {
                BuildDataList();
                HiddenFieldQuestionnaireID.Value = SessionState.Questionnaire.QuestionnaireID.ToString();
            }

            CheckBoxInstruction.Checked = SessionState.CheckBox[ "CheckBoxInstruction" ];
            GridViewQuestion.Columns[ columnInstruction ].Visible = CheckBoxInstruction.Checked;
            CheckBoxMessage.Checked = SessionState.CheckBox[ "CheckBoxMessage" ];
            GridViewQuestion.Columns[ columnMessage ].Visible = CheckBoxMessage.Checked;
            CheckBoxSociete.Checked = SessionState.CheckBox[ "CheckBoxSociete" ];
            GridViewQuestion.Columns[ columnSociete ].Visible = CheckBoxSociete.Checked;
            CheckBoxDate.Checked = SessionState.CheckBox[ "CheckBoxDate" ];
            GridViewQuestion.Columns[ columnCreationDate ].Visible = CheckBoxDate.Checked;
            CheckBoxAlignement.Checked = SessionState.CheckBox[ "CheckBoxAlignementQuestionReponse" ];
            GridViewQuestion.Columns[ columnAlignementQuestion ].Visible = CheckBoxAlignement.Checked;
            GridViewQuestion.Columns[ columnAlignementReponse ].Visible = CheckBoxAlignement.Checked;
            
            CheckBoxChoixMultipleMinMax.Checked = SessionState.CheckBox[ "CheckBoxChoixMultipleMinMax" ];
            GridViewQuestion.Columns[ columnChoixMultipleMin ].Visible = CheckBoxChoixMultipleMinMax.Checked;
            GridViewQuestion.Columns[ columnChoixMultipleMax ].Visible = CheckBoxChoixMultipleMinMax.Checked;
        }

        FormulaireEnModeImpression();

        Trace.Warn( "Page_Load : IsPostBack == true" );
    }

    private void FormulaireEnModeImpression()
    {
        if ( Request.QueryString[ "print" ] != null || Request.QueryString[ "excel" ] != null )
        {
            LabelTitre.Text = SessionState.Questionnaire.Description;
            PanelDropDownListQuestionnaires.Visible = false;
            PanelBoutonControl.Visible = false;
            PanelCopierQuestionReponse.Visible = false;
            PanelDetailsView1.Visible = false;
            PanelAide.Visible = false; // COR02082010
            PanelTitre.Visible = false; // COR02082010
            if ( Request.QueryString[ "print" ] == "2" )
            {
                LabelTitre.Text = "";
                PanelGridView.Visible = false;
                DetailsView1.Visible = false;
                GridViewQuestion.Visible = false;
            }
            ImageButtonPrint.Visible = false;
            ImageButtonPrint1.Visible = false;
            ImageButtonExcel.Visible = false;
            RolloverButtonTestez.Visible = false;
            TrBoutonRetour.Visible = true;
        }
        if ( Request.QueryString[ "excel" ] != null )
        {
            // Comme le mode print==2
            LabelTitre.Text = "";
            PanelGridView.Visible = false;
            DetailsView1.Visible = false;
            GridViewQuestion.Visible = false;
            PanelAide.Visible = false; // COR02082010
            PanelTitre.Visible = false; // COR02082010

            //Response.ContentType = "application/word"; // Ne fonctionne pas ??!!!!!!!
            Response.ContentType = "application/vnd.ms-excel"; // Set the content type to Excel
            Response.Charset = ""; // Remove the charset from the Content-Type header
            Page.EnableViewState = false; // Sinon Excel de merde ne sait pas lire le fichier genere !!!
            //UpdateProgress2.Visible = false; // incroyable mais ce con d'excel voit l'UpdatePanel
            //UpdateProgress1.Visible = false;
        }
    }

    private void GridViewForAdmin()
    {
        GridViewQuestion.Columns[ columnCommandField ].Visible = false;
        SqlDataSourceQuestion.SelectCommand = "SELECT * FROM PollQuestions ORDER BY Rank";
    }

    private void BuildDataList()
    {
        Trace.Warn( "BuildDataList" );

        if ( SessionState.Questionnaire != null )
        {
            PollQuestionCollection pollQuestions = PollQuestionCollection.GetByQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
            QuestionRankMax = pollQuestions.MaxRank();

            DropDownListCopierAPartirDe.Items.Clear();
            DropDownListCopierAPartirDe.Items.Add( "" ); 
            foreach ( PollQuestion pq in pollQuestions )
            {
                DropDownListCopierAPartirDe.Items.Add( pq.Rank.ToString() );
            }

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
    }

    // Se declenche quand on clique sur les boutons edit/delete/update/cancel
    protected void GridViewQuestion_RowCommand( object sender, GridViewCommandEventArgs e )
    {
        Trace.Warn( "GridViewQuestion_RowCommand" );

        if ( e.CommandName == "Edit" )
        {
            // Trouver la Question selectionnee par l'utilisateur pour remplir les colonnes cachees lors du save
            int index = Convert.ToInt32( e.CommandArgument );

            GridView gv = ( GridView )e.CommandSource;
            string q = gv.DataKeys[ index ].Value.ToString();

            PollQuestionCollection questions = PollQuestionCollection.GetByQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
            SessionState.Question = questions.FindByPollQuestionID( new Guid( q ) );
        }

        if ( e.CommandName == "Delete" )
        {
            int index = Convert.ToInt32( e.CommandArgument );

            GridView gv = ( GridView )e.CommandSource;

            Guid questionGuid = new Guid( gv.DataKeys[ index ].Value.ToString() );
            int status = PollQuestion.Delete( questionGuid );
            SessionState.Limitations.SupprimerQuestion();
        }

        Trace.Warn( string.Format( "GridViewQuestion_RowCommand CommandName : {0}", e.CommandName ) );
    }

    protected void GridViewQuestion_RowUpdating( object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e )
    {
        Trace.Warn( "GridViewQuestion_RowUpdating" );

        if ( e.NewValues[ "QuestionObligatoire" ] != null )
        {
            e.NewValues[ "QuestionObligatoire" ] = Tools.StringToBoolean( e.NewValues[ "QuestionObligatoire" ].ToString() );
            if ( e.NewValues[ "QuestionObligatoire" ] != null )
                e.NewValues[ "QuestionObligatoire" ] = Tools.StringBooleanFalseToNull( e.NewValues[ "QuestionObligatoire" ].ToString() );
        }

        if ( e.NewValues[ "QuestionFin" ] != null )
        {
            e.NewValues[ "QuestionFin" ] = Tools.StringToBoolean( e.NewValues[ "QuestionFin" ].ToString() );
            if ( e.NewValues[ "QuestionFin" ] != null )
                e.NewValues[ "QuestionFin" ] = Tools.StringBooleanFalseToNull( e.NewValues[ "QuestionFin" ].ToString() );
        }

        if ( e.NewValues[ "ChoixMultiple" ] != null )
        {
            e.NewValues[ "ChoixMultiple" ] = Tools.StringToBoolean( e.NewValues[ "ChoixMultiple" ].ToString() );
            if ( e.NewValues[ "ChoixMultiple" ] != null )
                e.NewValues[ "ChoixMultiple" ] = Tools.StringBooleanFalseToNull( e.NewValues[ "ChoixMultiple" ].ToString() );
        }

        if ( e.NewValues[ "Question" ] == null )
        {
            e.NewValues[ "Question" ] = "Question ?";
        }

        /*
        ** Traitement des colonnes cachees
        ** -------------------------------
        ** si la colonne est cachee e.NewValues == e.OldValues == null
        ** il faut trouver la valeur a mettre e.NewValues autrement
        */
        if ( CheckBoxInstruction.Checked == false )
        {
            e.NewValues[ "Instruction" ] = SessionState.Question.Instruction;
        }
        else
        {
            if ( e.NewValues[ "Instruction" ] != null )
            {
                if ( Instruction.Valide( e.NewValues[ "Instruction" ].ToString(), e.NewValues[ "ChoixMultiple" ] == null ) == false )
                    e.NewValues[ "Instruction" ] = e.OldValues[ "Instruction" ];
            }
        }

        if ( CheckBoxMessage.Checked == false )
        {
            e.NewValues[ "Message" ] = SessionState.Question.Message;
        }

        if ( CheckBoxSociete.Checked == false )
        {
            e.NewValues[ "Societe" ] = SessionState.Question.Societe;
        }

        if ( CheckBoxDate.Checked == false )
        {
            e.NewValues[ "CreationDate" ] = SessionState.Question.CreationDate;
        }
        else
        {
            if ( e.NewValues[ "CreationDate" ] != null )
            {
                try
                {
                    DateTime dt = DateTime.Parse( e.NewValues[ "CreationDate" ].ToString() );
                }
                catch
                {
                    e.NewValues[ "CreationDate" ] = e.OldValues[ "CreationDate" ];
                }
            }
        }

        int choixMin = 0; // BUG19062010
        int choixMax = 0;
        if ( CheckBoxChoixMultipleMinMax.Checked == false )
        {
            e.NewValues[ "ChoixMultipleMin" ] = SessionState.Question.ChoixMultipleMin;
        }
        else
        {
            if ( e.NewValues[ "ChoixMultipleMin" ] != null )
            {
                try
                {
                    choixMin = int.Parse( e.NewValues[ "ChoixMultipleMin" ].ToString() );
                }
                catch
                {
                    e.NewValues[ "ChoixMultipleMin" ] = e.OldValues[ "ChoixMultipleMin" ];
                }
            }
        }

        if ( CheckBoxChoixMultipleMinMax.Checked == false )
        {
            e.NewValues[ "ChoixMultipleMax" ] = SessionState.Question.ChoixMultipleMax;
        }
        else
        {
            if ( e.NewValues[ "ChoixMultipleMax" ] != null )
            {
                try
                {
                    choixMax = int.Parse( e.NewValues[ "ChoixMultipleMax" ].ToString() );
                }
                catch
                {
                    e.NewValues[ "ChoixMultipleMax" ] = e.OldValues[ "ChoixMultipleMax" ];
                }
            }
        }

        if ( ( choixMin > choixMax ) || ( choixMin < 0 ) || ( choixMax < 0 ) )
        {
            e.NewValues[ "ChoixMultipleMin" ] = e.OldValues[ "ChoixMultipleMin" ];
            e.NewValues[ "ChoixMultipleMax" ] = e.OldValues[ "ChoixMultipleMax" ];
            ValidationMessage.Text = "Choix Min est supérieur à Choix Max";
            ValidationMessage.Visible = true;
        }

        //if ( CheckBoxAlignement.Checked == false )
        //{
        //    e.NewValues[ "CreationDate" ] = 
        //}
    }

    protected void GridViewQuestion_RowUpdated( object sender, GridViewUpdatedEventArgs e )
    {
        Trace.Warn( "GridViewQuestion_RowUpdated" );
        //SqlDataSourceQuestion.DataBind();
        BuildDataList();
    }

    // Mettre une valeur a Null c'est l'effacer
    protected void DetailsView1_ItemInserting( object sender, System.Web.UI.WebControls.DetailsViewInsertEventArgs e )
    {
        Trace.Warn( "DetailsView1_ItemInserting" );

        // L'utilisateur n'a pas fait de choix ... false est la valeur de la checkbox par defaut il n'y a pas de null pour un bool !
        if ( ( bool )e.Values[ "QuestionFin" ] == false )
        {
            e.Values[ "QuestionFin" ] = null; // c'est plus joli
        }
        if ( ( bool )e.Values[ "QuestionObligatoire" ] == false )
        {
            e.Values[ "QuestionObligatoire" ] = null;
        }

        if ( ( bool )e.Values[ "ChoixMultiple" ] == false )
        {
            e.Values[ "ChoixMultiple" ] = null;
        }

        int choixMin = 0; // BUG19062010
        int choixMax = 0;
        try
        {
            choixMin = int.Parse( e.Values[ "ChoixMultipleMin" ].ToString() );
        }
        catch
        {
            e.Values[ "ChoixMultipleMin" ] = null;
        }
        try
        {
            choixMax = int.Parse( e.Values[ "ChoixMultipleMax" ].ToString() );
        }
        catch
        {
            e.Values[ "ChoixMultipleMax" ] = null;
        }
        if ( ( choixMin > choixMax ) || ( choixMin < 0 ) || ( choixMax < 0 ) )
        {
            e.Values[ "ChoixMultipleMin" ] = null;
            e.Values[ "ChoixMultipleMax" ] = null;
            ValidationMessage.Text = "Erreur : Choix Min est supérieur à Choix Max";
            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            ValidationMessage.Visible = true;
        }

        if ( ( bool )e.Values[ "MessageHaut" ] == false )
        {
            e.Values[ "MessageHaut" ] = null;
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

        // Copier les reponses de la question de rang choisie par l'utilisateur
        // dans la nouvelle question
        if ( DropDownListCopierAPartirDe.SelectedValue != "" )
        {
            // Le GUID de la nouvelle question cree
            Guid pollQuestionId = new Guid( e.Values[ "PollQuestionID" ].ToString() );

            int questionRank = 0;
            try 
            {
                questionRank = int.Parse( DropDownListCopierAPartirDe.SelectedValue );
            }
            catch
            {
                LabelValidationMessage.Text = "Erreur : le rang n'est pas un entier";
                LabelValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                return;
            }

            PollQuestionCollection questions = PollQuestionCollection.GetByQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
            PollQuestion laQuestion = null;
            foreach ( PollQuestion question in questions )
            {
                if ( question.Rank == questionRank )
                {
                    laQuestion = new PollQuestion();
                    laQuestion = question;
                    break;
                }
            }

            if ( laQuestion == null )
            {
                LabelValidationMessage.Text = "Pas de Question de ce rang";
                LabelValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                return;
            }

            PollAnswerCollection reponses = PollAnswerCollection.GetByPollQuestionID( laQuestion.PollQuestionId );
            foreach ( PollAnswer reponse in reponses )
            {
                PollAnswer a = new PollAnswer();
                a.PollQuestionId = pollQuestionId;
                a.Answer = reponse.Answer;
                a.TypeReponse = reponse.TypeReponse;
                a.Width = reponse.Width;
                a.Rows = reponse.Rows;
                a.Rank = reponse.Rank;
                a.Score = reponse.Score;

                int status = PollAnswer.Create( a );
                if ( status == 0 )
                {
                    LabelValidationMessage.Text = "Réponses copiées correctement";
                }
                else
                {
                    LabelValidationMessage.Text = "Erreur à la création des réponses";
                    LabelValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                }
            }
        }

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

        if ( DropDownListQuestionnaire.SelectedValue == DropDownListQuestionnaire.DefaultText )
        {
            SessionState.Questionnaire = null;
            //HiddenFieldQuestionnaireID.Value = "*"; ne marche pas bien sur
            GridViewForAdmin();
            DetailsView1.Visible = false;
        }
        else
        {
            int questionnaireID = DropDownListQuestionnaire.QuestionnaireID;
            HiddenFieldQuestionnaireID.Value = questionnaireID.ToString();
            SessionState.Questionnaire = Questionnaire.GetQuestionnaire( questionnaireID );
            DetailsView1.Visible = true;
            GridViewQuestion.Columns[ columnCommandField ].Visible = true;
            FormulaireEnModeImpression();
        }

        BuildDataList();
    }

    protected void CheckBoxDate_CheckedChanged( object sender, EventArgs e )
    {
        Trace.Warn( "CheckBoxDate_CheckedChanged" );

        SessionState.CheckBox[ "CheckBoxDate" ] = CheckBoxDate.Checked;
        GridViewQuestion.Columns[ columnCreationDate ].Visible = CheckBoxDate.Checked;
        if ( DropDownListQuestionnaire.SelectedValue == DropDownListQuestionnaire.DefaultText )
        {
            GridViewForAdmin();
        }
    }

    protected void CheckBoxInstruction_CheckedChanged( object sender, EventArgs e )
    {
        SessionState.CheckBox[ "CheckBoxInstruction" ] = CheckBoxInstruction.Checked;
        GridViewQuestion.Columns[ columnInstruction ].Visible = CheckBoxInstruction.Checked;
        if ( DropDownListQuestionnaire.SelectedValue == DropDownListQuestionnaire.DefaultText )
        {
            GridViewForAdmin();
        }
    }

    protected void CheckBoxChoixMultipleMinMax_CheckedChanged( object sender, EventArgs e )
    {
        SessionState.CheckBox[ "CheckBoxChoixMultipleMinMax" ] = CheckBoxChoixMultipleMinMax.Checked;
        GridViewQuestion.Columns[ columnChoixMultipleMin ].Visible = CheckBoxChoixMultipleMinMax.Checked;
        GridViewQuestion.Columns[ columnChoixMultipleMax ].Visible = CheckBoxChoixMultipleMinMax.Checked; 
        if ( DropDownListQuestionnaire.SelectedValue == DropDownListQuestionnaire.DefaultText )
        {
            GridViewForAdmin();
        }
    }

    protected void CheckBoxMessage_CheckedChanged( object sender, EventArgs e )
    {
        SessionState.CheckBox[ "CheckBoxMessage" ] = CheckBoxMessage.Checked;
        GridViewQuestion.Columns[ columnMessage ].Visible = CheckBoxMessage.Checked;
        if ( DropDownListQuestionnaire.SelectedValue == DropDownListQuestionnaire.DefaultText )
        {
            GridViewForAdmin();
        }
    }

    protected void CheckBoxAlignement_CheckedChanged( object sender, EventArgs e )
    {
        SessionState.CheckBox[ "CheckBoxAlignementQuestionReponse" ] = CheckBoxAlignement.Checked;
        GridViewQuestion.Columns[ columnAlignementQuestion ].Visible = CheckBoxAlignement.Checked;
        GridViewQuestion.Columns[ columnAlignementReponse ].Visible = CheckBoxAlignement.Checked;
        if ( DropDownListQuestionnaire.SelectedValue == DropDownListQuestionnaire.DefaultText )
        {
            GridViewForAdmin();
        }
    }

    protected void CheckBoxSociete_CheckedChanged( object sender, EventArgs e )
    {
        SessionState.CheckBox[ "CheckBoxSociete" ] = CheckBoxSociete.Checked;
        GridViewQuestion.Columns[ columnSociete ].Visible = CheckBoxSociete.Checked;
        if ( DropDownListQuestionnaire.SelectedValue == DropDownListQuestionnaire.DefaultText )
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

    protected void DropDownListAlignementQuestion_SelectedIndexChanged( object sender, EventArgs e )
    {
        Trace.Warn( "DropDownListAlignementQuestion_SelectedIndexChanged" );

        DropDownListGridView ddl = ( DropDownListGridView )sender;
        int indexe = int.Parse( ddl.Valeur );
        Guid questionGuid = new Guid( GridViewQuestion.DataKeys[ indexe ].Value.ToString() );
        PollQuestion question = PollQuestion.GetQuestion( questionGuid );
        question.AlignementQuestion = ddl.SelectedValue;
        PollQuestion.UpdateAlignementQuestion( question );
    }

    protected void DropDownListAlignementReponse_SelectedIndexChanged( object sender, EventArgs e )
    {
        DropDownListGridView ddl = ( DropDownListGridView )sender;
        int indexe = int.Parse( ddl.Valeur );
        Guid questionGuid = new Guid( GridViewQuestion.DataKeys[ indexe ].Value.ToString() );
        PollQuestion question = PollQuestion.GetQuestion( questionGuid );
        question.AlignementReponse = ddl.SelectedValue;
        PollQuestion.UpdateAlignementReponse( question );
    }

    // GridViewQuestion_RowDataBound est appellee pour chaque ligne lors de la construction
    // de la GridView
    // Attention : le fait d'utiliser une deuxieme fois la DDL, introduit un mauvais comportement
    // Valeur est à "" lors du SelectedIndexChanged !
    protected void ComputeAlignementColumn()
    {
        Trace.Warn( "ComputeAlignementColumn" );

        if ( GridViewQuestion.Rows.Count > 0 )
        {
            int indexRow = GridViewQuestion.Rows.Count - 1;

            Guid questionGuid = new Guid( GridViewQuestion.DataKeys[ indexRow ].Value.ToString() );
            PollQuestion question = PollQuestion.GetQuestion( questionGuid );

            DropDownListGridView ddlQ = ( DropDownListGridView )GridViewQuestion.Rows[ indexRow ].FindControl( "DropDownListGridViewAlignementQuestion" );
            ddlQ.DataSource = PollQuestion.TypeAlignement();
            ddlQ.DataBind();
            ddlQ.Valeur = indexRow.ToString();
            if ( question.AlignementQuestion != "" )
            {
                ddlQ.SelectedValue = question.AlignementQuestion;
            }

            DropDownListGridView ddlR = ( DropDownListGridView )GridViewQuestion.Rows[ indexRow ].FindControl( "DropDownListGridViewAlignementReponse" );
            ddlR.DataSource = PollQuestion.TypeAlignement();
            ddlR.DataBind();
            ddlR.Valeur = indexRow.ToString();
            if ( question.AlignementQuestion != "" )
            {
                ddlR.SelectedValue = question.AlignementReponse;
            }
        }
    }

    protected void GridViewQuestion_Load( object sender, EventArgs e )
    {
        Trace.Warn( "GridViewQuestion_Load" );
    }

    protected void GridViewQuestion_RowDeleted( object sender, GridViewDeletedEventArgs e )
    {
        BuildDataList();
    }

    protected void ButtonRangPlusUn_Click( object sender, EventArgs e )
    {
        int indexRow = 0;
        DataKeyArray dka = GridViewQuestion.DataKeys;
        foreach ( GridViewRow r in GridViewQuestion.Rows )
        {
            CheckBox cb = ( CheckBox )GridViewQuestion.Rows[ indexRow ].FindControl( "CheckBoxRangPlusMoinsUn" );
            Guid questionGuid = new Guid( dka[ indexRow ].Value.ToString() );
            if ( cb.Checked )
            {
                PollQuestion question = PollQuestion.GetQuestion( questionGuid );
                question.Rank += 1;
                PollQuestion.UpdateRank( question );
            }

            indexRow += 1;
        }
        //Response.Redirect( Request.RawUrl ); surtout pas ici !!
        HiddenFieldQuestionnaireID.Value = SessionState.Questionnaire.QuestionnaireID.ToString();
        GridViewQuestion.DataBind();
        BuildDataList();
    }

    protected void ButtonRangMoinsUn_Click( object sender, EventArgs e )
    {
        int indexRow = 0;
        DataKeyArray dka = GridViewQuestion.DataKeys;
        foreach ( GridViewRow r in GridViewQuestion.Rows )
        {
            CheckBox cb = ( CheckBox )GridViewQuestion.Rows[ indexRow ].FindControl( "CheckBoxRangPlusMoinsUn" );
            Guid questionGuid = new Guid( dka[ indexRow ].Value.ToString() );
            if ( cb.Checked )
            {
                PollQuestion question = PollQuestion.GetQuestion( questionGuid );
                question.Rank -= 1;
                PollQuestion.UpdateRank( question );
            }

            indexRow += 1;
        }
        //Response.Redirect( Request.RawUrl ); surtout pas ici !!
        HiddenFieldQuestionnaireID.Value = SessionState.Questionnaire.QuestionnaireID.ToString();
        GridViewQuestion.DataBind();
        BuildDataList();
    }

    protected void GridViewQuestion_RowDataBound( object sender, GridViewRowEventArgs e )
    {
        Trace.Warn( "GridViewQuestion_RowDataBound" );
        ComputeAlignementColumn();
    }

    protected void ImageButtonPrint_Click( object sender, ImageClickEventArgs e )
    {
        Response.Redirect( "~/Poll/Manage.aspx?print=1" );
    }

    protected void ImageButtonPrint1_Click( object sender, ImageClickEventArgs e )
    {
        Response.Redirect( "~/Poll/Manage.aspx?print=2" );
    }

    protected void ImageButtonExcel_Click( object sender, ImageClickEventArgs e )
    {
        Response.Redirect( "~/Poll/Manage.aspx?excel=1" );
    }
}

