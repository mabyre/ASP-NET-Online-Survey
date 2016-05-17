#region Using

using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using SettingXmlProvider;
using Sql.Web.Data;
using System.Net;

#endregion

public partial class Admin_Pages_SettingsSmtp : PageBase
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( IsPostBack == false )
        {
            if ( User.IsInRole( "Administrateur" ) == false )
            {
                SmtpServer serveur = SmtpServer.Get( SessionState.MemberInfo.MembreGUID );
                if ( serveur != null )
                {
                    TextBoxUserName.Text = serveur.UserName;
                    TextBoxPassWord.Text = serveur.UserPassWord;
                    TextBoxServerName.Text = serveur.ServerName;
                    TextBoxServerPort.Text = serveur.ServerPort.ToString();
                    TextBoxEmail.Text = serveur.Email;
                    TextBoxEmailSubject.Text = serveur.EmailSubject;
                    CheckBoxEnableSSL.Checked = serveur.EnableSSL;
                    // FCKeditor1.Value = serveur.EmailBody; ne sert plus c'est une WebContent CorpsEmail
                }
                else
                {
                    TextBoxServerPort.Text = "25";
                }
            }
            else
            {
                PanelAdmin.Visible = true;
                //TextBoxUserName.Enabled = false;
            }
        }

        MessageValidation( string.Empty );
        Page.MaintainScrollPositionOnPostBack = true;
    }

    // message == string.Empty on efface
    // message != "nonvide" on cumule
    // if ( message == "nonvide" ) on affiche
    void MessageValidation( string message )
    {
        if ( message != string.Empty )
        {
            ValidationMessage.Text += message;
            PanelMessageValidation.Visible = true;
        }
        else
        {
            ValidationMessage.Text = "";
            PanelMessageValidation.Visible = false;
        }
    }

    bool ValiderFormulaire()
    {
        if ( TextBoxEmail.Text.Trim() == "" )
        {
            MessageValidation( "Email vide.<br/>" );
        }
        if ( Strings.IsValideEmail( TextBoxEmail.Text ) == false )
        {
            MessageValidation( "Adresse E-mail n'est pas valide<br/>" );
        }
        if ( TextBoxUserName.Text.Trim() == "" )
        {
            MessageValidation( "Nom d'utilisateur est vide.<br/>" );
        }
        if ( TextBoxServerName.Text.Trim() == "" )
        {
            MessageValidation( "Nom du serveur est vide.<br/>" );
        }
        if ( TextBoxPassWord.Text.Trim() == "" )
        {
            MessageValidation( "Mot de passe est vide.<br/>" );
        }
        if ( TextBoxServerPort.Text.Trim() == "" )
        {
            MessageValidation( "Le port serveur est vide.<br/>" );
        }

        return ValidationMessage.Text == string.Empty;
    }

    protected void ButtonTestSmtp_Click( object sender, EventArgs e )
    {
        if ( ValiderFormulaire() == false )
        {
            return;
        }

        MailMessage mail = new MailMessage();
        mail.From = new MailAddress( TextBoxEmail.Text, TextBoxUserName.Text );
        mail.To.Add( mail.From );
        mail.Subject = "Email de test " + TextBoxUserName.Text;
        mail.Body = "Message de test.";
        mail.IsBodyHtml = true;

        SmtpClient smtp = new SmtpClient( TextBoxServerName.Text );
        smtp.Credentials = new NetworkCredential( TextBoxUserName.Text, TextBoxPassWord.Text );
        smtp.EnableSsl = CheckBoxEnableSSL.Checked;
        smtp.Port = int.Parse( TextBoxServerPort.Text );

        try
        {
            smtp.Send( mail );
            LabelSmtpStatus.Text = "Test réussi.";
            LabelSmtpStatus.Style.Add( HtmlTextWriterStyle.Color, "green" );
        }
        catch ( Exception ex )
        {
            LabelSmtpStatus.Text = "Connexion impossible.<br>";
            LabelSmtpStatus.Style.Add( HtmlTextWriterStyle.Color, "red" );
            MessageValidation( ex.Message );
        }
    }

    public void ButtonSave_OnClick( object sender, EventArgs e )
    {
        string user = DropDownListMembre.SelectedValue.ToString();
        if ( ValiderFormulaire() == false || user == "-1" )
        {
            return;
        }

        SmtpServer serveur = new SmtpServer();
        Guid membreGUID = new Guid();
        if ( User.IsInRole( "Administrateur" ) )
        {
            string[] userName = user.Split( '/' );
            MemberInfo member = MemberInfo.GetMemberInfo( userName[ 0 ], userName[ 1 ] );
            serveur = SmtpServer.Get( member.MembreGUID );
            membreGUID = member.MembreGUID;
        }
        else
        {
            serveur = SmtpServer.Get( SessionState.MemberInfo.MembreGUID );
            membreGUID = SessionState.MemberInfo.MembreGUID;
        }

        if ( serveur == null ) // C'est une creation
        {
            serveur = new SmtpServer();
            serveur.UserGUID = membreGUID;
            serveur.UserName = TextBoxUserName.Text;
            serveur.UserPassWord = TextBoxPassWord.Text;
            serveur.ServerName = TextBoxServerName.Text;
            try
            {
                int port = int.Parse( TextBoxServerPort.Text );
                serveur.ServerPort = port;
            }
            catch 
            {
                ValidationMessage.Text = "Le numéro de port est un entier";
                serveur.ServerPort = 25;
            }
            serveur.Email = TextBoxEmail.Text;
            serveur.EmailSubject = TextBoxEmailSubject.Text;
            serveur.EmailBody = ""; //FCKeditor1.Value; ne sert plus c'est une WebContent CorpsEmail
            serveur.EnableSSL = CheckBoxEnableSSL.Checked;
            int status = SmtpServer.Create( serveur );
        }
        else // C'est une mise a jour
        {
            serveur.UserName = TextBoxUserName.Text;
            serveur.UserPassWord = TextBoxPassWord.Text;
            serveur.ServerName = TextBoxServerName.Text;
            try
            {
                int port = int.Parse( TextBoxServerPort.Text );
                serveur.ServerPort = port;
            }
            catch
            {
                ValidationMessage.Text = "Le numéro de port est un entier";
                serveur.ServerPort = 25;
            }
            serveur.Email = TextBoxEmail.Text;
            serveur.EmailSubject = TextBoxEmailSubject.Text;
            serveur.EmailBody = ""; //FCKeditor1.Value; ne sert plus c'est une WebContent CorpsEmail
            serveur.EnableSSL = CheckBoxEnableSSL.Checked;
            SmtpServer.Update( serveur );
        }
    }

    // Copier le serveur SMTP de l'intervieweur pour l'utilisateur
    public void ButtonCopier_OnClick( object sender, EventArgs e )
    {
        SmtpServer serveur = new SmtpServer();
        Guid membreGUID = new Guid();
        MemberInfo member = MemberInfo.GetMemberInfo( "Intervieweur" );
        serveur = SmtpServer.Get( member.MembreGUID );
        membreGUID = member.MembreGUID;

        if ( serveur == null ) // pas de serveur SMTP de l'Intervieweur
        {
            ValidationMessage.Text = "Il n'y pas de serveur SMTP de test configuré";
        }
        else 
        {
            TextBoxUserName.Text = serveur.UserName;
            TextBoxPassWord.Text = serveur.UserPassWord;
            TextBoxServerName.Text = serveur.ServerName;
            TextBoxServerPort.Text = serveur.ServerPort.ToString();
            TextBoxEmail.Text = serveur.Email;
            TextBoxEmailSubject.Text = serveur.EmailSubject;
            CheckBoxEnableSSL.Checked = serveur.EnableSSL;
            ValidationMessage.Text = "Cliquez sur \"Sauver\" pour conserver ce serveur SMTP";
        }
        PanelMessageValidation.Visible = true;
    }

    protected void DropDownListMembre_SelectedIndexChanged( object sender, EventArgs e )
    {
        string user = DropDownListMembre.SelectedValue.ToString();
        if ( user != "-1" )
        {
            string[] userName = user.Split( '/' );
            MemberInfo member = MemberInfo.GetMemberInfo( userName[ 0 ], userName[ 1 ] );
            SmtpServer serveur = SmtpServer.Get( member.MembreGUID );
            if ( serveur != null )
            {
                TextBoxUserName.Text = serveur.UserName;
                TextBoxPassWord.Text = serveur.UserPassWord;
                TextBoxServerName.Text = serveur.ServerName;
                TextBoxServerPort.Text = serveur.ServerPort.ToString();
                TextBoxEmail.Text = serveur.Email;
                TextBoxEmailSubject.Text = serveur.EmailSubject;
                CheckBoxEnableSSL.Checked = serveur.EnableSSL;
            }
            else if ( CheckBoxCopier.Checked == false )
            {
                TextBoxUserName.Text = string.Empty;
                TextBoxPassWord.Text = string.Empty;
                TextBoxServerName.Text = string.Empty;
                TextBoxServerPort.Text = string.Empty;
                TextBoxEmail.Text = string.Empty;
                TextBoxEmailSubject.Text = string.Empty;
                CheckBoxEnableSSL.Checked = false;
            }
        }
    }
}