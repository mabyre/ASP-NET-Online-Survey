/*
** On retrouve le meme genre dans Poll/Manage mais bon on va pas faire un Control !
** Ce formulaire a sa specificite il se comporte differemment en fonction de l'utilisateur
** qui est client ou non
** 
** L'utilisateur non client n'a pas acces a la modification du Questionnaire
**
** -------------------------------------------------------------------------------------------
** Le comportement de ce formulaire est tout bonnement incroyable !
** -------------------------------------------------------------------------------------------
** Lorsqu'on modifie les données : le texte d'une question par exemple
** pas besoin de faire BuildDataList() ni rien d'autre les donnees se rafraichissent toute seules !
** j'ai ajoute un bouton DefaultButton qui doit etre visible mais dont la taille est 0 pour
** recuperer les evts sinon on est redirige vers une autre page !...
**
** J'ai compris quand il se passe un TextChanged de la Question le texte est change
** et la question est bien mise a jour cela suffit pour que les donnes soient en accord
** avec ce que voit l'utilisateur.
**
** Donc quand on change une donnee par un TextBox
** il suffit de faire une mise a jour dans la BD dans le TextBox_TextChanged
**
** Quand le TextBox sert a mettre a jour une un autre objet graphique
** il suffit de faire la maj de la BD puis de l'objet graphique
**
** Lorsque l'ordre des objets est modifie, il faut ajouter un RebuildDataList()
** 
** Lorsque les donnes sont modifiees par un bouton c'est le TextChanged qui maj la BD
** et dans le ButtonClick il suffit de mettre a jour l'objet graphique
**
** C'est la seul moyen pour que MaintainScrollPositionOnPostback soit efficace
**
*/

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
using TraceReporter;
#endregion

partial class Poll_List : PageBase
{
    private bool BoutonVisible()
    {
        return User.IsInRole( "Client" ) == true || User.IsInRole( "Administrateur" ) == true;
    }

    // Pour la pagination de la DataList
    private int CurrentPage 
    {
        get
        {
            if ( Session[ "CurrentPage" ] == null )
            {
                Session[ "CurrentPage" ] = 0;
            }
            return ( int )Session[ "CurrentPage" ];
        }
        set { Session[ "CurrentPage" ] = value; }
    }

    protected void Page_Load( object sender, System.EventArgs e )
    {
        Reporter.Trace( "Page_Load" );

        if ( IsPostBack == false )
        {
            if ( Request.QueryString[ "QuestionnaireID" ] != null )
            {
                int questionnaireID = int.Parse( Request.QueryString[ "QuestionnaireID" ] );
                SessionState.Questionnaire = SessionState.Questionnaires.FindByID( questionnaireID );
            }

            // Choisir le premier Questionnaire a la place de l'utilisateur
            if ( SessionState.Questionnaire == null && SessionState.Questionnaires.Count > 0 )
            {
                SessionState.Questionnaire = SessionState.Questionnaires[ 0 ];
            }

            if ( SessionState.Questionnaire != null )
            {
                LabelValider.Visible = SessionState.Questionnaire.Valider;
                LabelFin.Visible = SessionState.Questionnaire.Fin;
                LabelBloque.Visible = SessionState.Questionnaire.Bloque;

                SessionState.Questions = PollQuestionCollection.GetByQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
                SessionState.Reponses = PollAnswerCollection.GetAll();
                BuildDataList();
            }

            RolloverButtonProgrammer.Visible = BoutonVisible();
            RolloverButtonAjouterQuestion.Visible = BoutonVisible();
        }

        // COR210920090002 c'est vicieux mais comme on ne peut rien faire sur UrlReferrer
        // de merde dont tous les elements sont en lecture seul !! 
        // je ne vois que rendre invisible l'ancre
        if ( Request.UrlReferrer.Fragment == "#BasDePage" )
        {
            AncreBasDePage.Visible = false;
        }

        Page.Form.DefaultButton = DefaultButton.UniqueID; // Pour donner le focus
    }

    private void RebuildDataList()
    {
        SessionState.Questions = PollQuestionCollection.GetByQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
        SessionState.Reponses = PollAnswerCollection.GetAll();
        BuildDataList();
    }

    private int nombrePage()
    {
        int taillePage = int.Parse( SessionState.MemberSettings.TaillePageQuestions );
        int nombrePage = SessionState.Questions.Count / taillePage; // nombre de page entieres
        if ( SessionState.Questions.Count % taillePage != 0 )
        {
            nombrePage += 1; // page non entiere
        }
        return nombrePage;
    }

    private bool modePage()
    {
        if ( nombrePage() > 1 )
        {
            return true;
        }
        CurrentPage = 0; // raz si on sort du mode page
        return false;
    }

    private void BuildDataList()
    {
        Reporter.Trace( "BuildDataList" );

        // AME19102010 Mode Pagination - Il y a t-il plusieurs pages ?
        if ( modePage() )
        {
            // AME19102010 - Mettre ou remettre les boutons des pager en fonction de CurrentPage
            if ( CurrentPage == 0 ) // premiere page
            {
                ButtonPagePrecedenteHaut.Visible = false;
                ButtonPagePrecedenteBas.Visible = false;
                ButtonPageSuivanteHaut.Visible = true;
                ButtonPageSuivanteBas.Visible = true;
            }
            if ( CurrentPage > 0 )
            {
                ButtonPagePrecedenteHaut.Visible = true;
                ButtonPagePrecedenteBas.Visible = true;
                ButtonPageSuivanteHaut.Visible = true;
                ButtonPageSuivanteBas.Visible = true;
            }
            if ( nombrePage() > 0 && CurrentPage + 1 >= nombrePage() ) // derniere page
            {
                CurrentPage = nombrePage() - 1; // si on a change la pagination CurrentPage ne peut pas depasser nombrePage() - 1 
                ButtonPagePrecedenteHaut.Visible = true;
                ButtonPagePrecedenteBas.Visible = true;
                ButtonPageSuivanteHaut.Visible = false;
                ButtonPageSuivanteBas.Visible = false;
            }

            PagedDataSource pagedDataSource = new PagedDataSource();
            pagedDataSource.AllowPaging = true;
            pagedDataSource.DataSource = SessionState.Questions;
            pagedDataSource.PageSize = int.Parse( SessionState.MemberSettings.TaillePageQuestions );
            pagedDataSource.CurrentPageIndex = CurrentPage;

            LabelPageCouranteHaut.Text = ( CurrentPage + 1 ).ToString() + "/" + nombrePage().ToString();
            LabelPageCouranteBas.Text = LabelPageCouranteHaut.Text;
            LabelPageCouranteHaut.ToolTip = "Page courante/Nombre de pages - taille d'une page : " + SessionState.MemberSettings.TaillePageQuestions;
            LabelPageCouranteBas.ToolTip = LabelPageCouranteHaut.ToolTip;

            PanelPagerHaut.Visible = true;
            PanelPagerBas.Visible = true;

            DataListQuestion.DataSource = pagedDataSource;
        }
        else
        {
            PanelPagerHaut.Visible = false;
            PanelPagerBas.Visible = false;
            DataListQuestion.DataSource = SessionState.Questions;
        }
        DataListQuestion.DataBind();

        // Trouver toutes les reponses
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

            DropDownList ddlTypReponse = ( DropDownList )dli.FindControl( "DropDownListTypeReponse" );
            ddlTypReponse.DataSource = TypeReponse.List();
            ddlTypReponse.DataBind();
            ddlTypReponse.Items.Insert( 0, new ListItem( "Type de Réponse", "-1" ) );
            
            // Par defaut on propose toujours le type de reponse "Choix"
            ddlTypReponse.SelectedValue = TypeReponse.Choix;
        }
    }

    protected void DropDownListQuestionnaire_SelectedIndexChanged( object sender, EventArgs e )
    {
        SessionState.Questionnaire = SessionState.Questionnaires.FindByID( DropDownListQuestionnaire.QuestionnaireID );
        SessionState.Questions = PollQuestionCollection.GetByQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
        SessionState.Reponses = PollAnswerCollection.GetAll();
        LabelValider.Visible = SessionState.Questionnaire.Valider;
        LabelFin.Visible = SessionState.Questionnaire.Fin;
        LabelBloque.Visible = SessionState.Questionnaire.Bloque;
        BuildDataList();
    }

    protected void ImageButtonPrint_Click( object sender, ImageClickEventArgs e )
    {
        Response.Redirect( "~/Poll/Manage.aspx?print=2" );
    }

    protected void ButtonRangPlusMoins_Click( object sender, EventArgs e )
    {
        Button button = ( Button )sender;

        string srang = TextRangMoinsPlusHaut.Text;
        if ( button.ID == "ButtonRangPlusBas" || button.ID == "ButtonRangMoinsBas" )
        {
            srang = TextRangMoinsPlusBas.Text;
        }

        int irang = 1;
        try
        {
            irang = int.Parse( srang );
            if ( irang < 1 || irang > 1000 )
            {
                irang = 1;
                if ( button.ID == "ButtonRangPlusBas" || button.ID == "ButtonRangMoinsBas" )
                {
                    TextRangMoinsPlusBas.Text = "1";
                }
                else
                {
                    TextRangMoinsPlusHaut.Text = "1";
                }
                return;
            }
        }
        catch
        {
            if ( button.ID == "ButtonRangPlusBas" || button.ID == "ButtonRangMoinsBas" )
            {
                TextRangMoinsPlusBas.Text = "1";
            }
            else
            {
                TextRangMoinsPlusHaut.Text = "1";
            }
            return;
        }

        // Si c'est un bouton Moins on inverse le rang
        if ( button.ID == "ButtonRangMoinsHaut" || button.ID == "ButtonRangMoinsBas" )
        {
            irang = -irang;
        }

        foreach ( DataListItem dli in DataListQuestion.Items )
        {
            HiddenField hf = new HiddenField();
            hf = ( HiddenField )dli.FindControl( "PollQuestionId" );
            Guid pollQuestionId = new Guid( hf.Value );

            CheckBox cb = ( CheckBox )dli.FindControl( "CheckBoxRangPlusMoins" );
            if ( cb.Checked )
            {
                PollQuestion question = PollQuestion.GetQuestion( pollQuestionId );
                question.Rank += irang;
                PollQuestion.UpdateRank( question );
            }

            DataList dlr = new DataList();
            dlr = ( DataList )dli.FindControl( "DataListReponse" );

            foreach ( DataListItem dlir in dlr.Items )
            {
                HiddenField hfr = new HiddenField();
                hfr = ( HiddenField )dlir.FindControl( "PollAnswerId" );
                Guid pollAnswerId = new Guid( hfr.Value );

                CheckBox cbr = ( CheckBox )dlir.FindControl( "CheckBoxRangPlusMoins" );
                if ( cbr.Checked )
                {
                    PollAnswer reponse = SessionState.Reponses.FindByPollAnswerID( pollAnswerId );
                    reponse.Rank += irang;
                    PollAnswer.Update( reponse );
                }
            }
        }

        RebuildDataList();
    }

    protected void ButtonPagePrecedente_Click( object sender, EventArgs e )
    {
        if ( CurrentPage != 0 )
        {
            CurrentPage -= 1;
            BuildDataList();
        }
    }

    protected void ButtonPageSuivante_Click( object sender, EventArgs e )
    {
        if ( CurrentPage + 1 < nombrePage() )
        {
            CurrentPage += 1;
            BuildDataList();
        }
    }

    #region MiseAJourQuestion

    protected void TextBoxQuestion_TextChanged( object sender, EventArgs e )
    {
        Reporter.Trace( "TextBoxQuestion_TextChanged" );
        BloquerQuestionnaire( SessionState.Questionnaire.Bloque );

        try
        {
            TextBox textBox = ( TextBox )sender;
            HiddenField hf = new HiddenField();
            hf = ( HiddenField )textBox.Parent.FindControl( "PollQuestionId" );
            Guid pollQuestionId = new Guid( hf.Value );
            PollQuestion question = SessionState.Questions.FindByPollQuestionID( pollQuestionId );
            
            // Mise a jour de la Question
            if ( textBox.Text.Trim() != string.Empty )
            {
                question.Question = textBox.Text.Trim();
                PollQuestion.Update( question );
            }
            else // Suppression de la Question des Reponses et des Votes associes
            {
                int status = 0;
                PollAnswerCollection reponses = PollAnswerCollection.GetByPollQuestionID( question.PollQuestionId );
                foreach ( PollAnswer reponse in reponses )
                {
                    PollVoteCollection votes = PollVoteCollection.GetVotes( reponse.PollAnswerId );
                    foreach ( PollVote vote in votes )
                    {
                        status += PollVote.Delete( vote.VoteId );
                    }
                    status += PollAnswer.Delete( reponse.PollAnswerId );
                }
                status += PollQuestion.Delete( question.PollQuestionId );
                SessionState.Limitations.SupprimerQuestion();
                RebuildDataList();
            }
        }
        catch
        {
        }
    }

    private void BloquerQuestionnaire( bool bloque )
    {
        if ( bloque )
        {
            Tools.PageValidation( "Le questionnaire \"" + SessionState.Questionnaire.Description + "\" est clôturé." );
        }
    }

    protected void TestBoxRank_TextChanged( object sender, EventArgs e )
    {
        Reporter.Trace( "TestBoxRank_TextChanged" );
        BloquerQuestionnaire( SessionState.Questionnaire.Bloque );

        try
        {
            TextBox textBoxRank = ( TextBox )sender;
            int rank = int.Parse( textBoxRank.Text );
            HiddenField hf = new HiddenField();
            hf = ( HiddenField )textBoxRank.Parent.FindControl( "PollQuestionId" );
            Guid pollQuestionId = new Guid( hf.Value );
            PollQuestion question = SessionState.Questions.FindByPollQuestionID( pollQuestionId );
            question.Rank = rank;
            PollQuestion.UpdateRank( question );
        }
        catch
        {
        }

        RebuildDataList();
    }

    // Ajouter des Reponses a la Question
    protected void TextBoxReponse_TextChanged( object sender, EventArgs e )
    {
        Reporter.Trace( "TextBoxReponse_TextChanged" );
        BloquerQuestionnaire( SessionState.Questionnaire.Bloque );

        try
        {
            TextBox textBox = ( TextBox )sender;
            DropDownList ddlTypeReponse = ( DropDownList )textBox.Parent.FindControl( "DropDownListTypeReponse" );
            if ( textBox.Text.Trim() != "" && ddlTypeReponse.SelectedValue != "-1" )
            {
                HiddenField hf = new HiddenField();
                hf = ( HiddenField )textBox.Parent.FindControl( "PollQuestionId" );
                Guid pollQuestionId = new Guid( hf.Value );
                PollQuestion question = SessionState.Questions.FindByPollQuestionID( pollQuestionId );
                PollAnswerCollection reponses = PollAnswerCollection.GetByPollQuestionID( pollQuestionId );

                CheckBox cbxObligatoire = ( CheckBox )textBox.Parent.FindControl( "CheckBoxObligatoire" );

                if ( textBox.Text.Trim().Contains( ";" ) )
                {
                    int rank = reponses.MaxRank() + 1; // ajouter les reponses a la suite des reponses existantes
                    string[] reponsesSplit = textBox.Text.Trim().Split( ';' );
                    foreach ( string rep in reponsesSplit )
                    {
                        PollAnswer reponse = new PollAnswer( rep.Trim() );
                        reponse.PollQuestionId = question.PollQuestionId;
                        reponse.TypeReponse = ddlTypeReponse.SelectedValue;
                        reponse.Obligatoire = cbxObligatoire.Checked;
                        reponse.Rank = rank;

                        int status = PollAnswer.Create( reponse );
                        rank += 1;
                    }
                }
                else
                {
                    PollAnswer reponse = new PollAnswer();
                    reponse.PollQuestionId = question.PollQuestionId;
                    reponse.Answer = textBox.Text.Trim();
                    reponse.TypeReponse = ddlTypeReponse.SelectedValue;
                    //                reponse.Width = reponse.Width;
                    //                reponse.Rows = reponse.Rows;
                    //                reponse.AlignLeft = reponse.AlignLeft;
                    //                reponse.Horizontal = reponse.Horizontal;
                    reponse.Obligatoire = cbxObligatoire.Checked;
                    reponse.Rank = reponses.MaxRank() + 1; // ajouter la reponse a la suite des reponses existantes

                    int status = PollAnswer.Create( reponse );
                }

                RebuildDataList();
            }

            if ( ddlTypeReponse.SelectedValue == "-1" )
            {
                textBox.CssClass = "TextBoxListUserQuestionInstructionRedStyle";
                textBox.Text = "Choisir un Type de réponse";
            }
        }
        catch
        {
        }
    }

    protected void TextBoxInstruction_TextChanged( object sender, EventArgs e )
    {
        BloquerQuestionnaire( SessionState.Questionnaire.Bloque );

        try
        {
            TextBox textBox = ( TextBox )sender;

            HiddenField hf = new HiddenField();
            hf = ( HiddenField )textBox.Parent.FindControl( "PollQuestionId" );
            Guid pollQuestionId = new Guid( hf.Value );
            PollQuestion question = SessionState.Questions.FindByPollQuestionID( pollQuestionId );

            if ( textBox.Text.Trim() != string.Empty )
            {
                if ( Instruction.Valide( textBox.Text.Trim(), question.ChoixMultiple == false ) )
                {
                    question.Instruction = textBox.Text.Trim();
                    textBox.CssClass = "TextBoxListUserQuestionInstructionStyle";
                    PollQuestion.Update( question );
                }
                else
                {
                    textBox.CssClass = "TextBoxListUserQuestionInstructionRedStyle";
                    textBox.Text = "Instruction non valide";
                }
            }
            else
            {
                question.Instruction = string.Empty;
                PollQuestion.Update( question );
            }

            // Pallier a un bug graphique voir List.aspx.cs a <!-- BUG GRAPHIQUE
            if ( textBox.Text.Trim() != string.Empty )
                textBox.Width = new Unit( "100%" );
            else
                textBox.Width = new Unit( 130 );

        }
        catch
        {
        }
    }

    protected void TextBoxMessageUtilisateur_TextChanged( object sender, EventArgs e )
    {
        Reporter.Trace( "TextBoxMessageUtilisateur_TextChanged" );
        BloquerQuestionnaire( SessionState.Questionnaire.Bloque );

        try
        {
            TextBox textBox = ( TextBox )sender;

            HiddenField hf = new HiddenField();
            hf = ( HiddenField )textBox.Parent.FindControl( "PollQuestionId" );
            Guid pollQuestionId = new Guid( hf.Value );
            PollQuestion question = SessionState.Questions.FindByPollQuestionID( pollQuestionId );
            question.Message = textBox.Text.Trim();
            PollQuestion.Update( question );

            // Pallier a un bug graphique voir List.aspx.cs a <!-- BUG GRAPHIQUE
            if ( textBox.Text.Trim() != string.Empty )
                textBox.Width = new Unit( "100%" );
            else
                textBox.Width = new Unit( 180 );
        }
        catch
        {
        }
    }

    #endregion

    protected void TestBoxReponseRank_TextChanged( object sender, EventArgs e )
    {
        Reporter.Trace( "TestBoxReponseRank_TextChanged" );
        BloquerQuestionnaire( SessionState.Questionnaire.Bloque );

        try
        {
            TextBox textBoxRank = ( TextBox )sender;
            int rank = int.Parse( textBoxRank.Text );
            HiddenField hf = new HiddenField();
            hf = ( HiddenField )textBoxRank.Parent.FindControl( "PollAnswerId" );
            Guid pollAnswerId = new Guid( hf.Value );
            PollAnswer reponse = SessionState.Reponses.FindByPollAnswerID( pollAnswerId );
            reponse.Rank = rank;
            PollAnswer.Update( reponse );
        }
        catch
        {
        }

        RebuildDataList();
    }

    protected void TextBoxScore_TextChanged( object sender, EventArgs e )
    {
        Reporter.Trace( "TextBoxScore_TextChanged" );

        try
        {
            TextBox textBox = ( TextBox )sender;
            int score = int.Parse( textBox.Text );
            if ( score > 0 )
            {
                HiddenField hf = new HiddenField();
                hf = ( HiddenField )textBox.Parent.FindControl( "PollAnswerId" );
                Guid pollAnswerId = new Guid( hf.Value );
                PollAnswer reponse = SessionState.Reponses.FindByPollAnswerID( pollAnswerId );
                reponse.Score = score;
                PollAnswer.Update( reponse );
            }
        }
        catch
        {
        }
    }

    protected void TextBoxDataListReponse_TextChanged( object sender, EventArgs e )
    {
        Reporter.Trace( "TextBoxDataListReponse_TextChanged" );
        BloquerQuestionnaire( SessionState.Questionnaire.Bloque );

        try
        {
            TextBox textBox = ( TextBox )sender;

            HiddenField hf = new HiddenField();
            hf = ( HiddenField )textBox.Parent.FindControl( "PollAnswerId" );
            Guid pollAnswerId = new Guid( hf.Value );
            PollAnswer reponse = SessionState.Reponses.FindByPollAnswerID( pollAnswerId );

            // Mise a jour de la Reponse
            if ( textBox.Text.Trim() != string.Empty )
            {
                reponse.Answer = textBox.Text.Trim();
                PollAnswer.Update( reponse );
            }
            else // Suppression de la Reponse et des Votes associes
            {
                int status = 0;
                PollVoteCollection votes = PollVoteCollection.GetVotes( reponse.PollAnswerId );
                foreach ( PollVote vote in votes )
                {
                    status += PollVote.Delete( vote.VoteId );
                }
                status += PollAnswer.Delete( reponse.PollAnswerId );
                RebuildDataList();
            }
        }
        catch
        {
        }
    }

    #region SautPage

    protected void ImageButtonInsererSautPage_Click( object sender, EventArgs e )
    {
        Reporter.Trace( "ImageButtonInsererSautPage_Click" );

        ImageButton imageButton = (ImageButton)sender;
        Panel panelSautPageEdit = ( Panel )imageButton.Parent.FindControl( "PanelSautPageEdit" );
        Panel panelSautPage = ( Panel )imageButton.Parent.FindControl( "PanelSautPage" );
        panelSautPageEdit.Visible = panelSautPageEdit.Visible == true ? false : true;
        
        // Premier coup : panelSautPage.Visible == true 
        // equivalent a Question possede un Saut de page
        if ( panelSautPage.Visible )
        {
            panelSautPage.Visible = false;
        }
        else // Trouver si la Question possede ou non un Saut de page si true l'afficher
        {
            HiddenField hf = new HiddenField();
            hf = ( HiddenField )imageButton.Parent.FindControl( "PollQuestionId" );
            Guid pollQuestionId = new Guid( hf.Value );
            PollQuestion question = SessionState.Questions.FindByPollQuestionID( pollQuestionId );
            panelSautPage.Visible = !string.IsNullOrEmpty( question.SautPage );
        }
    }

    protected void TextBoxSautPage_TextChanged( object sender, EventArgs e )
    {
        Reporter.Trace( "TextBoxSautPage_TextChanged" );
        BloquerQuestionnaire( SessionState.Questionnaire.Bloque );

        TextBox textSautPage = ( TextBox )sender;
        if ( textSautPage.Text.Trim() != "" )
        {
            try
            {
                HiddenField hf = new HiddenField();
                hf = ( HiddenField )textSautPage.Parent.FindControl( "PollQuestionId" );
                Guid pollQuestionId = new Guid( hf.Value );
                PollQuestion question = SessionState.Questions.FindByPollQuestionID( pollQuestionId );
                question.SautPage = textSautPage.Text.Trim();
                int status = PollQuestion.UpdateSautPage( question );
            }
            catch
            {
            }
        }
    }

    protected void ButtonSautPageOk_Click( object sender, EventArgs e )
    {
        Reporter.Trace( "ButtonSautPageOk_Click" );

        Button button = ( Button )sender;
        Panel panelSautPageEdit = ( Panel )button.Parent.FindControl( "PanelSautPageEdit" );
        Panel panelSautPage = ( Panel )button.Parent.FindControl( "PanelSautPage" );
        panelSautPageEdit.Visible = false;

        // Rafraichir l'objet modifie
        try
        {
            HiddenField hf = new HiddenField();
            hf = ( HiddenField )button.Parent.FindControl( "PollQuestionId" );
            Guid pollQuestionId = new Guid( hf.Value );
            PollQuestion question = SessionState.Questions.FindByPollQuestionID( pollQuestionId );

            Label LabelSautPage = ( Label )panelSautPage.FindControl( "LabelSautPage" );
            LabelSautPage.Text = question.SautPage;
            panelSautPage.Visible = string.IsNullOrEmpty( question.SautPage ) == false;
        }
        catch
        {
        }
    }

    protected void ImageButtonSautPageSupprimer_Click( object sender, EventArgs e )
    {
        Reporter.Trace( "ImageButtonSautPageSupprimer_Click" );

        ImageButton imageButton = ( ImageButton )sender;

        try
        {
            HiddenField hf = new HiddenField();
            hf = ( HiddenField )imageButton.Parent.FindControl( "PollQuestionId" );
            Guid pollQuestionId = new Guid( hf.Value );
            PollQuestion question = SessionState.Questions.FindByPollQuestionID( pollQuestionId );
            question.SautPage = null;
            int status = PollQuestion.UpdateSautPage( question );
        }
        catch
        {
        }

        Panel panelSautPageEdit = ( Panel )imageButton.Parent.FindControl( "PanelSautPageEdit" );
        panelSautPageEdit.Visible = false;

        // Rafraichir l'objet modifie
        try
        {
            Panel panelSautPage = ( Panel )imageButton.Parent.FindControl( "PanelSautPage" );
            Label LabelSautPage = ( Label )panelSautPage.FindControl( "LabelSautPage" );
            LabelSautPage.Text = string.Empty;
        }
        catch
        {
        }
    }

    #endregion

    #region Tableau

    protected void ImageButtonInsererTableau_Click( object sender, EventArgs e )
    {
        Reporter.Trace( "ImageButtonInsererTableau_Click" );

        ImageButton imageButton = ( ImageButton )sender;
        Panel panelTableauEdit = ( Panel )imageButton.Parent.FindControl( "PanelTableauEdit" );
        Panel panelTableau = ( Panel )imageButton.Parent.FindControl( "PanelTableau" );
        panelTableauEdit.Visible = panelTableauEdit.Visible == true ? false : true;

        // Premier coup : panelTableau.Visible == true 
        // equivalent a Question possede un Tableau
        if ( panelTableau.Visible )
        {
            panelTableau.Visible = false;
        }
        else // Trouver si la Question possede ou non un Tableau si true l'afficher
        {
            HiddenField hf = new HiddenField();
            hf = ( HiddenField )imageButton.Parent.FindControl( "PollQuestionId" );
            Guid pollQuestionId = new Guid( hf.Value );
            PollQuestion question = SessionState.Questions.FindByPollQuestionID( pollQuestionId );
            panelTableau.Visible = !string.IsNullOrEmpty( question.Tableau );
        }
    }

    protected void TextBoxTableau_TextChanged( object sender, EventArgs e )
    {
        Reporter.Trace( "TextBoxTableau_TextChanged" );
        BloquerQuestionnaire( SessionState.Questionnaire.Bloque );

        TextBox text = ( TextBox )sender;
        if ( text.Text.Trim() != "" )
        {
            try
            {
                HiddenField hf = new HiddenField();
                hf = ( HiddenField )text.Parent.FindControl( "PollQuestionId" );
                Guid pollQuestionId = new Guid( hf.Value );
                PollQuestion question = SessionState.Questions.FindByPollQuestionID( pollQuestionId );
                question.Tableau = text.Text.Trim();
                int status = PollQuestion.UpdateTableau( question );
            }
            catch
            {
            }
        }

        Response.Redirect( Request.RawUrl );
    }

    protected void ButtonTableauOk_Click( object sender, EventArgs e )
    {
        Reporter.Trace( "ButtonTableauOk_Click" );

        Button button = ( Button )sender;
        Panel panelTableauEdit = ( Panel )button.Parent.FindControl( "PanelTableauEdit" );
        Panel panelTableau = ( Panel )button.Parent.FindControl( "PanelTableau" );
        panelTableauEdit.Visible = false;

        // Rafraichir l'objet modifie
        try
        {
            HiddenField hf = new HiddenField();
            hf = ( HiddenField )button.Parent.FindControl( "PollQuestionId" );
            Guid pollQuestionId = new Guid( hf.Value );
            PollQuestion question = SessionState.Questions.FindByPollQuestionID( pollQuestionId );

            Label LabelTableau = ( Label )panelTableau.FindControl( "LabelTableau" );
            LabelTableau.Text = question.Tableau;
            panelTableau.Visible = string.IsNullOrEmpty( question.Tableau ) == false;
        }
        catch
        {
        }
    }

    protected void ButtonTableauClassementOk_Click( object sender, EventArgs e )
    {
        Reporter.Trace( "ButtonTableauClassementOk_Click" );

        Button button = ( Button )sender;

        try
        {
            HiddenField hf = new HiddenField();
            hf = ( HiddenField )button.Parent.FindControl( "PollQuestionId" );
            Guid pollQuestionId = new Guid( hf.Value );
            PollQuestion question = SessionState.Questions.FindByPollQuestionID( pollQuestionId );
            if ( question.Tableau.Contains( Tableau.Classement ) )
            {
                question.Tableau = question.Tableau.Substring( 0, question.Tableau.Length - Tableau.Classement.Length );
            }
            else
            {
                question.Tableau = question.Tableau + Tableau.Classement;
            }
            int status = PollQuestion.UpdateTableau( question );
        }
        catch
        {
        }

        Panel panelTableauEdit = ( Panel )button.Parent.FindControl( "PanelTableauEdit" );
        Panel panelTableau = ( Panel )button.Parent.FindControl( "PanelTableau" );
        panelTableauEdit.Visible = false;
        panelTableau.Visible = true;

        // Rafraichir les objets modifies
        try
        {
            HiddenField hf = new HiddenField();
            hf = ( HiddenField )button.Parent.FindControl( "PollQuestionId" );
            Guid pollQuestionId = new Guid( hf.Value );
            PollQuestion question = SessionState.Questions.FindByPollQuestionID( pollQuestionId );

            Label LabelTableau = ( Label )panelTableau.FindControl( "LabelTableau" );
            LabelTableau.Text = question.Tableau;

            // Et la TextBox car on ne passera pas par TextBoxTableau_TextChanged comme avec ButtonTableauOk_Click
            TextBox TextBox = ( TextBox )panelTableauEdit.FindControl( "TextBoxTableauEdit" );
            TextBox.Text = question.Tableau;
        }
        catch
        {
        }
    }

    protected void ButtonTableauFinOk_Click( object sender, EventArgs e )
    {
        Reporter.Trace( "ButtonTableauFinOk_Click" );

        Button button = ( Button )sender;

        try
        {
            HiddenField hf = new HiddenField();
            hf = ( HiddenField )button.Parent.FindControl( "PollQuestionId" );
            Guid pollQuestionId = new Guid( hf.Value );
            PollQuestion question = SessionState.Questions.FindByPollQuestionID( pollQuestionId );
            question.Tableau = Tableau.Fin;
            int status = PollQuestion.UpdateTableau( question );
        }
        catch
        {
        }

        Panel panelTableauEdit = ( Panel )button.Parent.FindControl( "PanelTableauEdit" );
        Panel panelTableau = ( Panel )button.Parent.FindControl( "PanelTableau" );
        panelTableauEdit.Visible = false;
        panelTableau.Visible = true;

        // Rafraichir l'objet modifie
        try
        {
            HiddenField hf = new HiddenField();
            hf = ( HiddenField )button.Parent.FindControl( "PollQuestionId" );
            Guid pollQuestionId = new Guid( hf.Value );
            PollQuestion question = SessionState.Questions.FindByPollQuestionID( pollQuestionId );

            Label LabelTableau = ( Label )panelTableau.FindControl( "LabelTableau" );
            LabelTableau.Text = question.Tableau;
        }
        catch
        {
        }
    }

    protected void ImageButtonTableauSupprimer_Click( object sender, EventArgs e )
    {
        Reporter.Trace( "ImageButtonTableauSupprimer_Click" );

        ImageButton imageButton = ( ImageButton )sender;

        // Mettre a jour la BD
        try
        {
            HiddenField hf = new HiddenField();
            hf = ( HiddenField )imageButton.Parent.FindControl( "PollQuestionId" );
            Guid pollQuestionId = new Guid( hf.Value );
            PollQuestion question = SessionState.Questions.FindByPollQuestionID( pollQuestionId );
            question.Tableau = null;
            int status = PollQuestion.UpdateTableau( question );
        }
        catch
        {
        }

        Panel panelTableauEdit = ( Panel )imageButton.Parent.FindControl( "PanelTableauEdit" );
        panelTableauEdit.Visible = false;

        // Rafraichir l'objet modifie
        try
        {
            HiddenField hf = new HiddenField();
            hf = ( HiddenField )imageButton.Parent.FindControl( "PollQuestionId" );
            Guid pollQuestionId = new Guid( hf.Value );
            PollQuestion question = SessionState.Questions.FindByPollQuestionID( pollQuestionId );

            Panel panelTableau = ( Panel )imageButton.Parent.FindControl( "PanelTableau" );
            Label LabelTableau = ( Label )panelTableau.FindControl( "LabelTableau" );
            LabelTableau.Text = string.Empty;
            panelTableau.Visible = false;
        }
        catch
        {
        }
    }

    #endregion

    protected void RolloverButtonTestez_Click( object sender, EventArgs e )
    {
        if ( SessionState.Questionnaire != null )
        {
            Response.Redirect( "~/Poll/Questionnaire.aspx?QuestionnaireID=" + SessionState.Questionnaire.QuestionnaireID.ToString(), true );
        }
    }

    protected void RolloverButtonProgrammer_Click( object sender, EventArgs e )
    {
        if ( SessionState.Questionnaire != null )
        {
            Response.Redirect( "~/Poll/Manage.aspx", true );
        }
    }

    protected void RolloverButtonAjouterQuestion_Click( object sender, EventArgs e )
    {
        if ( SessionState.Questionnaire != null )
        {
            Response.Redirect( "~/Wizard/Question.aspx", true );
        }
    }

    // Consommer les evts en trop
    protected void DefaultButton_Click( object sender, EventArgs e )
    {
        Reporter.Trace( "DefaultButton_Click" );
    }
}

