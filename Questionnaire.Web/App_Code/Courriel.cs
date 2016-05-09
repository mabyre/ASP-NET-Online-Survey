using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Data;
using System.Configuration;
using Sql.Web.Data;
using System.Threading;
using BusinessObject;

/// <summary>
/// Description resumee de Courriel
/// Finalement tres specifique a l'application, on ne peut pas generaliser
/// </summary>
public class Courriel
{
	public Courriel()
	{
	}

    // Envoyer l'email a l'interviewe (Personne)
    public static string EnvoyerEmailQuestionnaire( MailAddress mailTo, string civilite, string nom, string prenom, string societe, string code, string sujet, string guid )
    {
        string status = "";
        SmtpServer smtpServer = SmtpServer.Get( SessionState.MemberInfo.MembreGUID );
        if ( smtpServer == null )
        {
            // COR04112009
            string msg = "Pas de serveur SMTP configuré pour cet utilisateur : " + SessionState.MemberInfo.NomUtilisateur + "<br/>";
            msg += "Pour envoyer des emails à vos interviewés, vous devez configurer un compte d'emails :<br/><br/>";
            msg += "<span class=\"SpanHyperLinkStyle\">";
            msg += "<a href=\"Admin/Page/SettingsSmtp.aspx\" class=\"HyperLinkStyle\" >Configurez votre serveur d'emails</a>";
            msg += "</span>";
            //HttpContext.Current.Response.Redirect( Tools.PageErreurPath + msg );
            Tools.PageValidation( msg );
        }

        string utilisateur = WebContent.GetUtilisateur();

        //
        // Il y a t-il une Page Email pour ce Questionnaire
        //
        WebContent webContent = null;
        webContent = WebContent.GetWebContent( "CorpsEmail", utilisateur, code );
        
        // Sinon il y a-til un email pour Tout Le Monde
        if ( webContent == null )
        {
            webContent = WebContent.GetWebContent( "CorpsEmail", utilisateur, WebContent.ToutLeMonde );
        }
        
        // Et Sinon c'est une erreur
        if ( webContent == null )
        {
            string msg = "Pas de section CorpsEmail configuré pour cet utilisateur : " + utilisateur;
            msg += "<br/>Demandez à l'administrateur";
            Tools.PageValidation( msg );
        }

        //
        // Deux moyens de contacter l'interviewer
        // Soit par le %%LIEN%%
        // Soit par %%ADRESSE_EMAIL%% ET %%CODE_ACCES%% ET %%LOG%%
        //
        LogonInterviewe logon = new LogonInterviewe( webContent.SectionContent );
        if ( logon.Message != "" )
        {
            Tools.PageValidation( logon.Message );
        }

        MailMessage mail = new MailMessage();
        mail.From = new MailAddress( smtpServer.Email, smtpServer.UserName );
        mail.To.Add( mailTo );
        mail.Subject = smtpServer.EmailSubject + " " + sujet;

        string body = webContent.SectionContent;
        body = body.Replace( "%%CIVILITE%%", civilite );
        body = body.Replace( "%%NOM%%", nom );
        body = body.Replace( "%%PRENOM%%", prenom );
        body = body.Replace( "%%SOCIETE%%", societe );
        if ( logon.Automatique )
        {
            string lien = Utils.WebSiteUri.ToLower() + "/contact/login.aspx?guid=" + guid;
            body = body.Replace( "%%LIEN%%", lien );
        }
        if ( logon.AdresseEmailEtCode )
        {
            string log = Utils.WebSiteUri.ToLower() + "/contact/login.aspx";
            body = body.Replace( "%%ADRESSE_EMAIL%%", mailTo.Address );
            body = body.Replace( "%%CODE_ACCES%%", code );
            body = body.Replace( "%%LOG%%", log );
        }
        mail.Body = body;
        mail.IsBodyHtml = true; // A voir ...

        SmtpClient smtp = new SmtpClient( smtpServer.ServerName );
        smtp.Credentials = new System.Net.NetworkCredential( smtpServer.UserName, smtpServer.UserPassWord );
        smtp.EnableSsl = smtpServer.EnableSSL;
        smtp.Port = smtpServer.ServerPort;

        try
        {
            smtp.Send( mail );
            status = mailTo.Address + " message envoyé";
            //OnEmailSent( mail ); on veut renvoyer status, on ne peut pas deleguer
        }
        catch ( SmtpException ex )
        {
            status = "Connexion impossible.<br>";
            status += ex.Message;
            //OnEmailFailed( mail ); on veut renvoyer status, on ne peut pas deleguer
        }
        finally
        {
            // Remove the pointer to the message object so the GC can close the thread.
            mail.Dispose();
            mail = null;
        }
        return status;
    }

    // Attention ici c'est un interviewe non authentifie qui veut envoyer un email
    // En plus si on l'envoit de facon assychrone dans une nouvelle thread SessionState a disparu 
    // c'est le plantage assure
    public static void EnvoyerEmailNouvelleReponse( Guid smtpMembreGUID, string sujet, string body )
    {
        string status = "";
        //SmtpServer smtpServer = SmtpServer.Get( SessionState.Questionnaire.MembreGUID );
        SmtpServer smtpServer = SmtpServer.Get( smtpMembreGUID );
        if ( smtpServer == null )
        {
            //string msg = "Pas de serveur SMTP configuré pour cet utilisateur : " + SessionState.MemberInfo.NomUtilisateur;
            return;            
        }

        MailAddress mailTo = new MailAddress( smtpServer.Email, smtpServer.UserName );
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress( smtpServer.Email, smtpServer.UserName );
        mail.To.Add( mailTo );
        mail.Subject = sujet;
        mail.Body = body;
        mail.IsBodyHtml = true; // A voir ...

        SmtpClient smtp = new SmtpClient( smtpServer.ServerName );
        smtp.Credentials = new System.Net.NetworkCredential( smtpServer.UserName, smtpServer.UserPassWord );
        smtp.EnableSsl = smtpServer.EnableSSL;
        smtp.Port = smtpServer.ServerPort;

        try
        {
            smtp.Send( mail );
            OnEmailSent( mail );
        }
        catch ( SmtpException ex )
        {
            OnEmailFailed( mail );
        }
        finally // Pourquoi pas ?
        {
            mail.Dispose();
            mail = null;
        }
    }

    //  
    public static void EnvoyerEmailTo( Guid smtpMembreGUID, string email, string sujet, string body )
    {
        string status = "";
        SmtpServer smtpServer = SmtpServer.Get( smtpMembreGUID );
        if ( smtpServer == null )
        {
            return;
        }

        MailAddress mailTo = new MailAddress( email );
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress( smtpServer.Email, smtpServer.UserName );
        mail.To.Add( mailTo );
        mail.Subject = sujet;
        mail.Body = body;
        mail.IsBodyHtml = true; // A voir ...

        SmtpClient smtp = new SmtpClient( smtpServer.ServerName );
        smtp.Credentials = new System.Net.NetworkCredential( smtpServer.UserName, smtpServer.UserPassWord );
        smtp.EnableSsl = smtpServer.EnableSSL;
        smtp.Port = smtpServer.ServerPort;

        try
        {
            smtp.Send( mail );
            OnEmailSent( mail );
        }
        catch ( SmtpException ex )
        {
            OnEmailFailed( mail );
        }
        finally // Pourquoi pas ?
        {
            mail.Dispose();
            mail = null;
        }
    }

    public static void EnvoyerMiseAJour( string origional, string nouveau, string sujet, string objet, string IPAddress )
    {
        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

        mail.To.Add( Global.SettingsXml.AdresseWebmaster );
        mail.Subject = Global.SettingsXml.SiteNom + " - " + sujet;

        mail.Body += "Date : " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
        mail.Body += "<br />";
        mail.Body += "Section : " + objet;
        mail.Body += "<br />";
        mail.Body += "IP Address : " + IPAddress;
        mail.Body += "<br />";
        mail.Body += "Contenu original : " + origional;
        mail.Body += "<br />";
        mail.Body += "Nouveau contenu : " + nouveau;

        mail.IsBodyHtml = true;

        SmtpClient smtp = new System.Net.Mail.SmtpClient();
        //smtp.Credentials = new System.Net.NetworkCredential( smtpServer.UserName, smtpServer.UserPassWord );
        //smtp.EnableSsl = smtpServer.EnableSSL;
        smtp.Send( mail );
    }

    #region Send Email Assynchrones

    public static void EnvoyerEmailNouvelleReponseAssynchrone( Guid smtpMembreGUID, string sujet, string body )
    {
        ThreadPool.QueueUserWorkItem( delegate { Courriel.EnvoyerEmailNouvelleReponse( smtpMembreGUID, sujet, body ); } );
    }

    public static void EnvoyerEmailToAssynchrone( Guid smtpMembreGUID, string email, string sujet, string body )
    {
        ThreadPool.QueueUserWorkItem( delegate { Courriel.EnvoyerEmailTo( smtpMembreGUID, email, sujet, body ); } );
    }

    /// <summary>
    /// Occurs after an e-mail has been sent. The sender is the MailMessage object.
    /// </summary>
    public static event EventHandler<EventArgs> EmailSent;
    private static void OnEmailSent( MailMessage message )
    {
        if ( EmailSent != null )
        {
            EmailSent( message, new EventArgs() );
        }
    }

    /// <summary>
    /// Occurs after an e-mail has been sent. The sender is the MailMessage object.
    /// </summary>
    public static event EventHandler<EventArgs> EmailFailed;
    private static void OnEmailFailed( MailMessage message )
    {
        if ( EmailFailed != null )
        {
            EmailFailed( message, new EventArgs() );
        }
    }

    #endregion
}
