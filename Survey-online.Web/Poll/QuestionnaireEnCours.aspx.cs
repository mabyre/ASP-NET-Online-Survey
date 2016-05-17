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

partial class Poll_QuestionnaireEnCours : PageBase
{
    protected void Page_Load( object sender, System.EventArgs e )
    {
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
            // C'est un interviewe qui accede ici il ne possede pas de MemberInfo
            if ( Request.QueryString[ "QuestionnaireID" ] != null && SessionState.Personne != null && SessionState.Personne.CodeAcces != 0 )
            {
                int questionnaireID = int.Parse( Request.QueryString[ "QuestionnaireID" ] );
                SessionState.Questionnaire = Questionnaire.GetQuestionnaire( questionnaireID );
                // Verifier qu'il y a un questionnaire
                if ( SessionState.Questionnaire == null )
                {
                    Response.Redirect( Tools.PageErreurPath + "Il n'y a pas de Questionnaire." );
                }
                // Verifier que c'est la bonne Personne
                if ( SessionState.Personne.QuestionnaireID != questionnaireID )
                {
                    Response.Redirect( Tools.PageErreurPath + "Il n'y a pas de Questionnaire." );
                }
            }

            BuildDataList();
            LabelDataListVide.Visible = DataListQuestion.Items.Count == 0;
            RolloverLinkRetour.Visible = SessionState.Questions != null;

            if ( Request.QueryString[ "fin" ] != null )
            {
                ButtonValiderQuestionnaire.Visible = true;
                RolloverLinkRetour.Visible = false;
            }
            else
            {
                ButtonValiderQuestionnaire.Visible =
                       ( SessionState.Questionnaire.Fin == false )
                    && ( SessionState.VotesEnCours.Count > 0 );
            }

            if ( Request.QueryString[ "valider" ] != null )
            {
                if ( Request.QueryString[ "valider" ].ToString() == "2" )
                {
                    RolloverLinkRetour.Visible = false;
                    ButtonValiderQuestionnaire.Visible = true;
                    ButtonValiderQuestionnaire.Text = "Terminer";
                    ButtonValiderQuestionnaire.ToolTip = "Terminer le Questionnaire";
                }
                else
                {
                    ButtonValiderQuestionnaire.Visible = false;
                }
                ValidationMessage.Text += "Vos réponses sont correctement enregistrés (" + Session[ "_VotesEnCoursCount" ].ToString() + ").";
                ValidationMessage.Visible = true;
            }
        }
    }

    private void BuildDataList()
    {
        DataListQuestion.DataSource = SessionState.QuestionsEnCours;
        DataListQuestion.DataBind();

        // Trouver les Reponses et les Votes
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

            // Rechercher dans les Votes deja effectues
            if ( answers.Count > 0 && SessionState.Votes != null )
            {
                int indexReponse = 0; // Trouver le bon LabelVote dans DataListReponse
                foreach ( PollAnswer answer in answers )
                {
                    PollVoteCollection pvc = SessionState.Votes.FindByAnswerID( answer.PollAnswerId );
                    if ( pvc.Count >= 1 ) // surement un seul mais bon ...
                    {
                        Label lbl = ( Label )dl.Controls[ indexReponse ].FindControl( "LabelVote" );
                        lbl.Text = "X";
                        lbl.CssClass = "LabelQuestionEnCoursVoteDejaFaitStyle";

                        if ( TypeReponse.EstTextuelle( answer.TypeReponse ) )
                        {
                            Label lblVoteTexte = ( Label )dl.Controls[ indexReponse ].FindControl( "LabelVoteTexte" );
                            lblVoteTexte.Text = pvc[ 0 ].Vote;
                        }
                    }
                    indexReponse += 1;
                }
            }

            // Rechercher dans les Votes en Cours
            if ( answers.Count > 0 && SessionState.VotesEnCours != null )
            {
                int indexReponse = 0; // Trouver le bon LabelVote dans DataListReponse
                foreach ( PollAnswer answer in answers )
                {
                    PollVoteCollection pvc = SessionState.VotesEnCours.FindByAnswerID( answer.PollAnswerId );
                    if ( pvc.Count >= 1 ) // surement un seul mais ...
                    {
                        Label lbl = ( Label )dl.Controls[ indexReponse ].FindControl( "LabelVote" );
                        lbl.Text = "X";

                        if ( TypeReponse.EstTextuelle( answer.TypeReponse ) )
                        {
                            Label lblVoteTexte = ( Label )dl.Controls[ indexReponse ].FindControl( "LabelVoteTexte" );
                            lblVoteTexte.Text = pvc[ 0 ].Vote;
                        }
                    }
                    indexReponse += 1;
                }
            }
        }
    }

    protected void ButtonValiderQuestionnaire_Click( object sender, EventArgs e )
    {
        if ( Request.QueryString[ "valider" ] != null )
        {
            if ( Request.QueryString[ "valider" ].ToString() == "2" )
            {
                Page.Response.Redirect( "~/Poll/Termine.Aspx", true );
            }
        }

        if ( SessionState.QuestionsEnCours.Count <= 0 )
        {
            ValidationMessage.Text += "Pas de questions en cours.";
            ValidationMessage.Visible = true;
            return;
        }

        if ( SessionState.VotesEnCours.Count <= 0 )
        {
            // L'interviewe a deja clique sur le bouton
            if ( ValidationMessage.Text == "Pas de réponses en cours." )
            {
                Page.Response.Redirect( "~/Poll/Termine.Aspx", true );
            }

            ValidationMessage.Text += "Pas de réponses en cours.";
            ValidationMessage.Visible = true;
            return;
        }

        int statusGlobal = 0;
        Session[ "_VotesEnCoursCount" ] = SessionState.VotesEnCours.Count;
        foreach ( PollQuestion question in SessionState.QuestionsEnCours )
        {
            PollAnswerCollection answers = PollAnswerCollection.GetByPollQuestionID( question.PollQuestionId );
            foreach ( PollAnswer answer in answers )
            {
                PollVoteCollection pvc = SessionState.VotesEnCours.FindByAnswerID( answer.PollAnswerId );
                if ( pvc.Count > 0 )
                {
                    foreach ( PollVote vote in pvc )
                    {
                        if ( HttpContext.Current.User.Identity.IsAuthenticated == false )
                        {
                            Limitation limitation = new Limitation( SessionState.Questionnaire.MembreGUID );
                            if ( limitation.LimitesReponses )
                            {
                                Context.Response.Redirect( Tools.PageErreurPath + "Désolé mais le nombre de réponses pour ce questionnaire est atteinte.", true );
                            }

                            int status = PollVote.Create( vote );
                            if ( status != 1 )
                            {
                                statusGlobal += 1;
                            }
                        }
                        SessionState.Votes.Add( vote );
                        SessionState.VotesEnCours.Remove( vote );
                    }
                }
            }
        }

        if ( statusGlobal != 0 )
        {
            ValidationMessage.Text += "Erreur à la création des votes : " + statusGlobal.ToString();
            ValidationMessage.Visible = true;
            return;
        }

        if ( Request.QueryString[ "fin" ] != null )
        {
            Page.Response.Redirect( "~/Poll/QuestionnaireEnCours.aspx?valider=2", true );
        } 
        Page.Response.Redirect( "~/Poll/QuestionnaireEnCours.aspx?valider=1", true );
    }
}

