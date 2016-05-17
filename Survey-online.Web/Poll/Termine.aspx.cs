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

public partial class Poll_Termine : PageBase
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( IsPostBack == false )
        {
            ScoreCollection scores = ScoreCollection.GetScoreQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
            if ( scores.Count > 0 )
            {
                PanelScore.Visible = true;

                int scoreTotal = 0;
                int scoreInterviewe = 0;
                foreach ( PollQuestion question in SessionState.Questions )
                {
                    PollAnswerCollection answers = PollAnswerCollection.GetByPollQuestionID( question.PollQuestionId );
                    foreach ( PollAnswer answer in answers )
                    {
                        PollVoteCollection pvc = SessionState.Votes.FindByAnswerID( answer.PollAnswerId );
                        if ( pvc.Count > 0 )
                        {
                            foreach ( PollVote vote in pvc )
                            {
                                scoreInterviewe += answer.Score;
                            }
                        }
                        scoreTotal += answer.Score;
                    }
                }

                LabelResultat.Text = "Vous avez un score de " + scoreInterviewe.ToString() + " sur un total de " + scoreTotal.ToString();
                foreach ( Score score in scores )
                {
                    if ( scoreInterviewe >= score.ScoreMin && scoreInterviewe <= score.ScoreMax )
                    {
                        LabelScoreTexte.Text += score.ScoreTexte;
                    }
                }
            }

            // Ne pas envoyer d'email quand c'est un intervieweur ou l'admin 
            if ( User.Identity.IsAuthenticated == false )
            {
                MemberInfo membre = MemberInfo.Get( SessionState.Questionnaire.MembreGUID );
                MemberSettings memberSettings = MemberSettings.GetMemberSettings( membre.NomUtilisateur );
                if ( memberSettings.PrevenirNouvelleReponse )
                {
                    string sujetEmail = "Nouvelle réponse au questionnaire " + SessionState.Questionnaire.Description;
                    string bodyEmail = "";
                    if ( SessionState.Questionnaire.Anonymat == false )
                    {
                        bodyEmail += "Nom : " + SessionState.Personne.Nom + "<br/>";
                        bodyEmail += "Prénom : " + SessionState.Personne.Prenom + "<br/>";
                        bodyEmail += "Email : " + SessionState.Personne.EmailBureau + "<br/>";
                        bodyEmail += "Téléphone : " + SessionState.Personne.TelephonePerso + "<br/>";
                    }
                    Limitation limitations = new Limitation( membre.MembreGUID );
                    bodyEmail += "<br/>Abonnement crédits réponses : " + ( limitations.NombreReponses - limitations.Reponses ).ToString() + "<br/>";
                    bodyEmail += "<br/>Accès à l'application :<br/>" + string.Format( "<a href=\"{0}\" >{1}</a>", Utils.WebSiteUri, Utils.WebSiteUri ) + "<br/>";

                    //Courriel.EnvoyerEmailNouvelleReponse( sujetEmail, bodyEmail );
                    Guid smtpMembreGUID = SessionState.Questionnaire.MembreGUID;
                    Courriel.EnvoyerEmailNouvelleReponseAssynchrone( smtpMembreGUID, sujetEmail, bodyEmail );
                }
            }
        }
    }
}
