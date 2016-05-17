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
using Sql.Data;
using Sql.Web.Data;
using System.IO;
using TraceReporter;

public partial class Contact_MemberDelete : PageBase
{
    // BUG10092009 static Guid MembreGUID = Guid.Empty;

    protected void Page_Load( object sender, EventArgs e )
    {
        if ( IsPostBack == false )
        {
            if ( Request.QueryString[ "MembreGUID" ] == null )
            {
                Response.Redirect( "~/Member/Manage.aspx" );
            }
            else
            {
                Cache[ "MembreGUID" ] = new Guid( Request.QueryString[ "MembreGUID" ] );
                MembershipUser user = Membership.GetUser( ( Guid )Cache[ "MembreGUID" ] );
                MemberInfo member = MemberInfo.Get( ( Guid )Cache[ "MembreGUID" ] );
                ValidationMessage.Text += "Suppression du Membre : " + member.Nom + " " + member.Prenom + " " + user.Email + "<br />";
                ValidationMessage.Text += "-- Nom d'utilisateur : " + user.UserName + "<br /><br />";
                
                Cache[ "Questionnaires" ] = QuestionnaireCollection.GetQuestionnaireMembre( ( Guid )Cache[ "MembreGUID" ] );
                foreach ( Questionnaire questionnaire in ( QuestionnaireCollection )Cache[ "Questionnaires" ] )
                {
                    int QuestionnaireID = questionnaire.QuestionnaireID;
                    ValidationMessage.Text += "Suppression du Questionnaire : " + questionnaire.Description + ":" + questionnaire.CodeAcces + " questions : ";
                    PollQuestionCollection questions = PollQuestionCollection.GetByQuestionnaire( QuestionnaireID );
                    ValidationMessage.Text += questions.Count + "<br />";
                    foreach ( PollQuestion question in questions )
                    {
                        ValidationMessage.Text += "-- Question : " + question.Question + "<br />";
                        PollAnswerCollection reponses = PollAnswerCollection.GetByPollQuestionID( question.PollQuestionId );
                        foreach ( PollAnswer reponse in reponses )
                        {
                            ValidationMessage.Text += "---- Réponse : " + reponse.Answer + "<br />";
                            int nbVotes = PollVoteCollection.NumberOfVotesByAnswer( reponse.PollAnswerId );
                            ValidationMessage.Text += "------ Votes : " + nbVotes.ToString() + "<br />";
                        }
                    }

                    PersonneCollection personnes = PersonneCollection.GetQuestionnaire( QuestionnaireID );
                    ValidationMessage.Text += "</br>";
                    ValidationMessage.Text += "Suppression des contacts : " + personnes.Count + "<br />";
                    foreach ( Personne p in personnes )
                    {
                        ValidationMessage.Text += p.Nom + " " + p.Prenom + " " + p.EmailBureau + " " + p.Societe + "<br />";
                    }
                    ValidationMessage.Text += "</br>";

                    WebContentCollection webContents = WebContentCollection.GetWebContents( member.NomUtilisateur, questionnaire.CodeAcces.ToString() );
                    ValidationMessage.Text += "Suppression des contenus web pour le Questionnaire : " + webContents.Count + "<br />";
                    foreach ( WebContent wc in webContents )
                    {
                        ValidationMessage.Text += wc.Section + " " + wc.Utilisateur + " " + wc.Visualisateur + "<br />";
                    }
                    ValidationMessage.Text += "</br>";

                    Cache[ "Scores" ] = ScoreCollection.GetScoreQuestionnaire( QuestionnaireID );
                    ValidationMessage.Text += "Suppression des scores : " + ( ( ScoreCollection )Cache[ "Scores" ] ).Count + "<br />";
                    ValidationMessage.Text += "</br>";
                }
                ValidationMessage.Text += "</br>";

                Cache[ "WebContentsToutLeMonde" ] = WebContentCollection.GetWebContents( member.NomUtilisateur, WebContent.ToutLeMonde );
                ValidationMessage.Text += "Suppression des contenus web pour Tout le Monde : " + ( ( WebContentCollection )Cache[ "WebContentsToutLeMonde" ] ).Count + "<br />";
                foreach ( WebContent wc in ( WebContentCollection )Cache[ "WebContentsToutLeMonde" ] )
                {
                    ValidationMessage.Text += "-- " + wc.Section + " " + wc.Utilisateur + " " + wc.Visualisateur + "<br />";
                }
                ValidationMessage.Text += "</br>";

                ValidationMessage.Text += "Suppression des styles web :<br />";
                string dirStyleWeb = Server.MapPath( "~/App_Data/StyleWeb/" + user.UserName );
                if ( Directory.Exists( dirStyleWeb ) )
                {
                    List<Fichier> fichiers = Tools.GetAllFichiers( dirStyleWeb );
                    if ( fichiers.Count <= 0 )
                    {
                        ValidationMessage.Text += "-- Pas de style web<br />";
                    }
                    else
                    {
                        foreach ( Fichier f in fichiers )
                        {
                            string nomFichier = Tools.GetFileNameWithoutExtension( f.Nom );
                            ValidationMessage.Text += "-- " + nomFichier + "<br />";
                        }
                    }
                }
                else
                {
                    ValidationMessage.Text += "-- Pas de style web<br />";
                }
                ValidationMessage.Text += "</br>";

                ValidationMessage.Text += "Suppression du fichier de paramètres : <br />";
                string fileName = Server.MapPath( "~/App_Data/MemberSettings/" + user.UserName + ".xml" );
                if ( File.Exists( fileName ) )
                {
                    string nomFichier = Tools.GetFileNameWithoutExtension( fileName );
                    ValidationMessage.Text += "-- " + nomFichier + "<br />";
                }
                else
                {
                    ValidationMessage.Text += "-- Pas de paramètres<br />";
                }
                ValidationMessage.Text += "</br>";

                ValidationMessage.Text += "Suppression du Serveur SMTP :<br />";
                SmtpServer stmpServeur = SmtpServer.Get( member.MembreGUID );
                if ( stmpServeur != null )
                {
                    ValidationMessage.Text += "-- Email : " + stmpServeur.Email + "<br />";
                    ValidationMessage.Text += "-- UserName : " + stmpServeur.UserName + "<br />";
                    ValidationMessage.Text += "-- PassWord : " + stmpServeur.UserPassWord + "<br />";
                    ValidationMessage.Text += "-- ServerName : " + stmpServeur.ServerName + "<br />";
                }
                else
                {
                    ValidationMessage.Text += "-- Pas de serveur SMTP<br />";
                }

                ValidationMessage.Visible = true;
            }
        }
    }

    protected void ButtonCancel_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Member/Manage.aspx" );
    }

    protected void ButtonSupprimer_Click( object sender, EventArgs e )
    {
        if ( Cache[ "MembreGUID" ] == null || ( Guid )Cache[ "MembreGUID" ] == Guid.Empty )
        {
            ValidationMessage.Text += "<br/>Choisir un membre à supprimer.<br/>";
            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            ValidationMessage.Visible = true;
        }
        else
        {
            int status = 0;
            int statusGlobal = 0;

            ValidationMessage.Text += "<br />-----------------------------------------------------<br />";
            ValidationMessage.Text += " Début de la Suppression du Membre <br />";
            ValidationMessage.Text += "-----------------------------------------------------<br />";

            Reporter.Trace( "ButtonSupprimer_Click début" );

            MemberInfo member = MemberInfo.Get( ( Guid )Cache[ "MembreGUID" ] );

            foreach ( Questionnaire questionnaire in (QuestionnaireCollection)Cache[ "Questionnaires" ] )
            {
                int QuestionnaireID = questionnaire.QuestionnaireID;

                ValidationMessage.Text += "--- Suppression du Questionnaire : " + questionnaire.Description + ":" + questionnaire.CodeAcces + " questions : ";
                PollQuestionCollection questions = PollQuestionCollection.GetByQuestionnaire( QuestionnaireID );
                ValidationMessage.Text += questions.Count + "<br />";
                
                foreach ( PollQuestion question in questions )
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
                            ValidationMessage.Text += "      status : " + status.ToString() + "<br />";
                        }
                        ValidationMessage.Text += "---- Suppression de la Réponse : " + reponse.Answer + "<br />";
                        status = PollAnswer.Delete( reponse.PollAnswerId );
                        statusGlobal = statusGlobal + status;
                        ValidationMessage.Text += "     status : " + status.ToString() + "<br />";
                    }
                    ValidationMessage.Text += "---  Suppression de la Question : " + question.Question + "<br />";
                    status = PollQuestion.Delete( question.PollQuestionId );
                    SessionState.Limitations.SupprimerQuestion();
                    statusGlobal = statusGlobal + status;
                    ValidationMessage.Text += "    status : " + status.ToString() + "<br />";
                }

                PersonneCollection personnes = PersonneCollection.GetQuestionnaire( QuestionnaireID );
                ValidationMessage.Text += "</br>";
                ValidationMessage.Text += "Suppression des contacts : " + personnes.Count + "<br />";
                foreach ( Personne p in personnes )
                {
                    ValidationMessage.Text += p.Nom + " " + p.Prenom + " " + p.EmailBureau + " " + p.Societe + "<br />";
                    status = Personne.Delete( p.ID_Personne );
                    statusGlobal = statusGlobal + status;
                    ValidationMessage.Text += "status : " + status.ToString() + "<br />";
                }
                SessionState.Limitations.SupprimerInterviewes( personnes.Count );

                WebContentCollection webContents = WebContentCollection.GetWebContents( member.NomUtilisateur, questionnaire.CodeAcces.ToString() );
                ValidationMessage.Text += "</br>";
                ValidationMessage.Text += "Suppression des contenus web pour le Questionnaire : " + webContents.Count + "<br />";
                foreach ( WebContent wc in webContents )
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
            }// Fin de foreach ( Questionnaire questionnaire in Questionnaires )

            Reporter.Trace( "foreach ( Questionnaire questionnaire in Questionnaires ) fin" );

            ValidationMessage.Text += "Suppression des contenus web pour Tout Le Monde : " + ( ( WebContentCollection )Cache[ "WebContentsToutLeMonde" ] ).Count + "<br />";
            foreach ( WebContent wc in ( WebContentCollection )Cache[ "WebContentsToutLeMonde" ] )
            {
                ValidationMessage.Text += wc.Section + " " + wc.Utilisateur + " " + wc.Visualisateur + "<br />";
                status = WebContent.Delete( wc.WebContentID );
                statusGlobal = statusGlobal + status;
                ValidationMessage.Text += "status : " + status.ToString() + "<br />";
            }

            MembershipUser user = Membership.GetUser( ( Guid )Cache[ "MembreGUID" ] );

            Reporter.Trace( "Directory.Exists( dirStyleWeb ) début" );

            ValidationMessage.Text += "Suppression des styles web :<br />";
            string dirStyleWeb = Server.MapPath( "~/App_Data/StyleWeb/" + user.UserName );
            if ( Directory.Exists( dirStyleWeb ) )
            {
                List<Fichier> fichiers = Tools.GetAllFichiers( dirStyleWeb );
                if ( fichiers.Count <= 0 )
                {
                    ValidationMessage.Text += "-- Pas de style web<br />";
                }
                else
                {
                    foreach ( Fichier f in fichiers )
                    {
                        try
                        {
                            File.Delete( f.Nom );
                            string nomFichier = Tools.GetFileNameWithoutExtension( f.Nom );
                            ValidationMessage.Text += "-- fichier supprimé : " + nomFichier + "<br />";
                        }
                        catch
                        {
                            string nomFichier = Tools.GetFileNameWithoutExtension( f.Nom );
                            ValidationMessage.Text += "-- ERREUR à la suppression du fichier : " + nomFichier + "<br />";
                        }
                    }
                }
                ValidationMessage.Text += "Suppression du répertoire : " + user.UserName;
                try
                {
                    Directory.Delete( dirStyleWeb );
                }
                catch ( Exception ex )
                {
                    ValidationMessage.Text += " Erreur : " + ex.Message + "</br>";
                }
                ValidationMessage.Text += "</br>";
            }
            else
            {
                ValidationMessage.Text += "-- Pas de style web<br />";
            }
            ValidationMessage.Text += "</br>";

            Reporter.Trace( "Directory.Exists( dirStyleWeb ) fin" );

            ValidationMessage.Text += "Suppression du fichier de paramètres :<br />";
            string fileName = Server.MapPath( "~/App_Data/MemberSettings/" + user.UserName + ".xml" );
            if ( File.Exists( fileName ) )
            {
                File.Delete( fileName );
                string nomFichier = Tools.GetFileNameWithoutExtension( fileName );
                ValidationMessage.Text += "-- fichier supprimé : " + nomFichier + "<br />";

            }
            else
            {
                string nomFichier = Tools.GetFileNameWithoutExtension( fileName );
                ValidationMessage.Text += "-- ERREUR à la suppression du fichier : " + nomFichier + "<br />";
            }
            ValidationMessage.Text += "</br>";

            Reporter.Trace( "SmtpServer.Get( member.MembreGUID ) début" );

            SmtpServer stmpServeur = SmtpServer.Get( member.MembreGUID );
            if ( stmpServeur != null )
            {
                ValidationMessage.Text += "Suppression du Serveur SMTP :" + stmpServeur.ServerName + "<br />";
                status = SmtpServer.Delete( stmpServeur.SmtpServerID );
                statusGlobal = statusGlobal + status;
                ValidationMessage.Text += "status : " + status.ToString() + "<br />";
            }

            Reporter.Trace( "SmtpServer.Get( member.MembreGUID ) fin" );

            ValidationMessage.Text += "Suppression du Membre : " + member.Nom + " " + member.Prenom + " " + user.Email + " " + user.UserName + "<br />";
            status = MemberInfo.Delete( ( Guid )Cache[ "MembreGUID" ] );
            statusGlobal = statusGlobal + status;
            ValidationMessage.Text += "status : " + status.ToString() + "<br />";
            ValidationMessage.Text += "Suppression de l'Utilisateur : " + user.UserName + "<br />";

            bool ok = Membership.DeleteUser( user.UserName, true );
            if ( ok )
                status = 0;
            else
                status = 1;
            ValidationMessage.Text += "status : " + status.ToString() + "<br />";
            ValidationMessage.Text += "<br />status global : " + statusGlobal.ToString() + "<br />";

            ValidationMessage.Visible = true;

            // Forcer les Questionnaires a se recharger depuis la Base de Donnees
            SessionState.Questionnaires = null;
            SessionState.Questionnaire = null;
            Cache[ "MembreGUID" ] = Guid.Empty;

            // Si c'est un membre qui supprime sont compte
            if ( User.IsInRole( "Administrateur" ) == false )
            {
                // Message de suppression d'un membre a l'admin
                string sujetEmail2 = "Suppression d'un Membre sur le site : " + Global.SettingsXml.SiteNom;
                string bodyEmail2 = "";

                bodyEmail2 += "Nom d'utilisateur : " + SessionState.MemberInfo.NomUtilisateur + "<br/>";
                bodyEmail2 += "Nom : " + SessionState.MemberInfo.Nom + "<br/>";
                bodyEmail2 += "Prénom : " + SessionState.MemberInfo.Prenom + "<br/>";
                bodyEmail2 += "Société : " + SessionState.MemberInfo.Societe + "<br/>";
                bodyEmail2 += "Téléphone : " + SessionState.MemberInfo.Telephone + "<br/>";
                bodyEmail2 += "Adresse : " + SessionState.MemberInfo.Adresse + "<br/>";
                bodyEmail2 += "Email : " + user.Email + "<br/>";
                bodyEmail2 += "<br/>Accès à l'application :<br/>" + string.Format( "<a href=\"{0}\" >{1}</a>", Utils.WebSiteUri, Utils.WebSiteUri ) + "<br/>";

                MemberInfo admin = MemberInfo.GetMemberInfo( "admin" );
                MembershipUser userAdmin = Membership.GetUser( admin.MembreGUID );
                Courriel.EnvoyerEmailToAssynchrone( admin.MembreGUID, userAdmin.Email, sujetEmail2, bodyEmail2 );

                // Deconnecter l'utilisateur
                FormsAuthentication.SignOut();
                HttpContext.Current.Session.Abandon();
                Response.Redirect( "~/Member/Login.aspx" );
            }
        }
    }
}
