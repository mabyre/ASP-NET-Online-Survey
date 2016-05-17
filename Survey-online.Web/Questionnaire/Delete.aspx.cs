using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Sql.Web.Data;
using TraceReporter;

public partial class Questionnaire_Edit : PageBase
{
    protected void Page_Load( object sender, System.EventArgs e )
    {
        if ( IsPostBack == false )
        {
            if ( Request.QueryString[ "QuestionnaireID" ] != null )
            {
                Cache[ "QuestionnaireID" ] = int.Parse( Request.QueryString[ "QuestionnaireID" ] );
                Questionnaire questionnaire = Questionnaire.GetQuestionnaire( (int)Cache[ "QuestionnaireID" ] );

                Reporter.Trace( "Questionnaire lecture" );

                ValidationMessage.Text += "Suppression du Questionnaire : " + questionnaire.Description + ":" + questionnaire.CodeAcces +" questions : ";
                Cache[ "Questions" ] = PollQuestionCollection.GetByQuestionnaire( ( int )Cache[ "QuestionnaireID" ] );
                ValidationMessage.Text += ((PollQuestionCollection)Cache[ "Questions" ]).Count + "<br />";
                foreach ( PollQuestion question in ( PollQuestionCollection )Cache[ "Questions" ] )
                {
                    ValidationMessage.Text += "-- Question : " + question.Question + "<br />";
                    PollAnswerCollection reponses = PollAnswerCollection.GetByPollQuestionID( question.PollQuestionId );
                    foreach ( PollAnswer reponse in reponses )
                    {
                        ValidationMessage.Text += "---- Réponse : " + reponse.Answer + "<br />";
                        int nbVotes = PollVoteCollection.NumberOfVotesByAnswer( reponse.PollAnswerId );
                        ValidationMessage.Text += "----- Votes : " + nbVotes.ToString() + "<br />";
                    }
                }

                Cache[ "Personnes" ] = PersonneCollection.GetQuestionnaire( ( int )Cache[ "QuestionnaireID" ] );
                ValidationMessage.Text += "</br>";
                ValidationMessage.Text += "Suppression des contacts : " + (( PersonneCollection )Cache[ "Personnes" ]).Count + "<br />";
                foreach ( Personne p in ( PersonneCollection )Cache[ "Personnes" ] )
                {
                    ValidationMessage.Text += p.Nom + " " + p.Prenom + " " + p.EmailBureau + " " + p.Societe + "<br />";
                }

                MemberInfo membre = MemberInfo.GetMemberInfo( questionnaire.MembreGUID );
                Cache[ "WebContents" ]  = WebContentCollection.GetWebContents( membre.NomUtilisateur, questionnaire.CodeAcces.ToString() );
                ValidationMessage.Text += "</br>";
                ValidationMessage.Text += "Suppression des contenus web : " + ( ( WebContentCollection )Cache[ "WebContents" ] ).Count + "<br />";
                foreach ( WebContent wc in ( WebContentCollection )Cache[ "WebContents" ] )
                {
                    ValidationMessage.Text += wc.Section + " " + wc.Utilisateur + " " + wc.Visualisateur + "<br />";
                }

                Cache[ "Scores" ] = ScoreCollection.GetScoreQuestionnaire( ( int )Cache[ "QuestionnaireID" ] );
                ValidationMessage.Text += "</br>";
                ValidationMessage.Text += "Suppression des scores : " + ( ( ScoreCollection )Cache[ "Scores" ] ).Count + "<br />";

                ValidationMessage.Visible = true;
            }
        }
    }

    protected void ButtonCancel_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Questionnaire/Manage.aspx" );
    }

    protected void ButtonSupprimer_Click( object sender, EventArgs e )
    {
        if ( (int)Cache[ "QuestionnaireID" ] == 0 )
        {
            ValidationMessage.Text += "<br/>Choisir un questionnaire à supprimer.<br/>";
            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            ValidationMessage.Visible = true;
        }
        else
        {
            int status = 0;
            int statusGlobal = 0;

            Questionnaire questionnaire = Questionnaire.GetQuestionnaire( ( int )Cache[ "QuestionnaireID" ] );

            Reporter.Trace( "Questionnaire delete" );

            ValidationMessage.Text += "<br />-----------------------------------------------------<br />";
            ValidationMessage.Text += " Début de la Suppression du Questionnaire <br />";
            ValidationMessage.Text += "-----------------------------------------------------<br />";

            foreach ( PollQuestion question in ( PollQuestionCollection )Cache[ "Questions" ] )
            {
                PollAnswerCollection reponses = PollAnswerCollection.GetByPollQuestionID( question.PollQuestionId );
                foreach ( PollAnswer reponse in reponses )
                {
                    PollVoteCollection votes = PollVoteCollection.GetVotes( reponse.PollAnswerId );
                    int nbVotes = PollVoteCollection.NumberOfVotesByAnswer( reponse.PollAnswerId );
                    ValidationMessage.Text += "----- Suppression des votes : " + nbVotes.ToString() + "<br />";
                    foreach ( PollVote vote in votes )
                    {
                        status = PollVote.Delete( vote.VoteId );
                        statusGlobal = statusGlobal + status;
                        ValidationMessage.Text += "     status : " + status.ToString() + "<br />";
                    }
                    ValidationMessage.Text += "---- Suppression de la Réponse : " + reponse.Answer + "<br />";
                    status = PollAnswer.Delete( reponse.PollAnswerId );
                    statusGlobal = statusGlobal + status;
                    ValidationMessage.Text += "     status : " + status.ToString() + "<br />";
                }
                ValidationMessage.Text += "--- Suppression de la Question : " + question.Question + "<br />";
                status = PollQuestion.Delete( question.PollQuestionId );
                statusGlobal = statusGlobal + status;
                SessionState.Limitations.SupprimerQuestion();
                ValidationMessage.Text += "    status : " + status.ToString() + "<br />";
            }

            Reporter.Trace( "Questionnaire delete fin" );

            //PersonneCollection personnes = PersonneCollection.GetQuestionnaire( ( int )Cache[ "QuestionnaireID" ] );
            ValidationMessage.Text += "</br>";
            ValidationMessage.Text += "Suppression des contacts : " + (( PersonneCollection )Cache[ "Personnes" ]).Count + "<br />";
            foreach ( Personne p in ( PersonneCollection )Cache[ "Personnes" ] )
            {
                ValidationMessage.Text += p.Nom + " " + p.Prenom + " " + p.EmailBureau + " " + p.Societe + "<br />";
                status = Personne.Delete( p.ID_Personne );
                statusGlobal = statusGlobal + status;
                ValidationMessage.Text += "status : " + status.ToString() + "<br />";
            }
            SessionState.Limitations.SupprimerInterviewes( (( PersonneCollection )Cache[ "Personnes" ]).Count );

            MemberInfo membre = MemberInfo.GetMemberInfo( questionnaire.MembreGUID );
            //WebContentCollection webContents = WebContentCollection.GetWebContents( membre.NomUtilisateur, questionnaire.CodeAcces.ToString() );
            ValidationMessage.Text += "</br>";
            ValidationMessage.Text += "Suppression des contenus web : " + (( WebContentCollection )Cache[ "WebContents" ]).Count + "<br />";
            foreach ( WebContent wc in ( WebContentCollection )Cache[ "WebContents" ] )
            {
                ValidationMessage.Text += wc.Section + " " + wc.Utilisateur + " " + wc.Visualisateur + "<br />";
                status = WebContent.Delete( wc.WebContentID );
                statusGlobal = statusGlobal + status;
                ValidationMessage.Text += "status : " + status.ToString() + "<br />";
            }

            ValidationMessage.Text += "</br>";
            ValidationMessage.Text += "Suppression des scores : " + ( ( ScoreCollection )Cache[ "Scores" ] ).Count + "<br />";
            foreach ( Score s in ( ScoreCollection )Cache[ "Scores" ] )
            {
                status = Score.Delete( s.ScoreID );
                statusGlobal = statusGlobal + status;
            }

            ValidationMessage.Text += "Suppression du Questionnaire : " + questionnaire.Description + "<br />";
            status = Questionnaire.Delete( questionnaire.QuestionnaireID );
            statusGlobal = statusGlobal + status;
            SessionState.Limitations.SupprimerQuestionnaire();
            ValidationMessage.Text += "status : " + status.ToString() + "<br />";
            ValidationMessage.Text += "<br />status global : " + statusGlobal.ToString() + "<br />";
            ValidationMessage.Visible = true;

            // Forcer les Questionnaires a se recharger depuis la Base de Donnees
            SessionState.Questionnaires = null;
            SessionState.Questionnaire = null;
            Cache[ "QuestionnaireID" ] = 0; // fermer le formulaire

            // On ne supprime qu'une fois !
            ButtonSupprimer.Visible = false;
        }
    }
}
