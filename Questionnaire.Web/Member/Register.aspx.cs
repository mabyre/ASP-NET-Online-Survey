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

public partial class Member_Register : PageBase
{
    protected void Page_Load( object sender, EventArgs e )
    {
        ValidationMessage.Visible = false;

        if ( IsPostBack == false )
        {
            CustomValidatorPassWord.Text = "Minimum : " + Membership.MinRequiredPasswordLength;
        }

        if ( User.Identity.IsAuthenticated )
        {
            ButtonRetour.Visible = true;

            // Verifier que l'utilisateur est approuve car SiteMapPath presente des liens vers lequels
            // un utilisateur enregistre mais non approuve peut naviguer
            MembershipUser user = Membership.GetUser( User.Identity.Name.Trim() );
            if ( user.IsApproved == false )
            {
                string message = "Votre compte d'utilisateur n'est pas approuvé.<br/>";
                if ( Global.SettingsXml.MembreApprouveParEmail )
                {
                    message += "Dans l'email que vous avez reçu, vous devez cliquer sur le lien pour valider votre compte.<br/>";
                }
                FormsAuthentication.SignOut();
                Response.Redirect( Tools.PageErreurPath + message );
            }
        }

        InititializeCaptcha();
    }

    // Bon on ne sait pas trop pourquoi ca eut marché
    // mais aujourd'hui un gros bug de merde fait 
    // penser qu'on ne peut pas utiliser SessionSate
    // on obtient un comportement heratique de l'appli
    // pour l'utilisateur qui n'est pas encore enregistre
    void ValiderMessage()
    {
        if ( ValidationMessage.Text != "" )
        {
            ValidationMessage.Visible = true;
        }

        //if ( SessionState.ValidationMessage != null )
        //{
        //    ValidationMessage.Text = SessionState.ValidationMessage;
        //    ValidationMessage.Visible = true;
        //    SessionState.ValidationMessage = null;
        //}
        //else
        //{
        //    ValidationMessage.Visible = false;
        //}
    }

    protected void CheckBoxValidate( object source, ServerValidateEventArgs args )
    {
        args.IsValid = ( CheckBoxConditionGenerales.Checked == true );
    }

    protected void ButtonEnregistrer_Click( object sender, System.EventArgs e )
    {
        // Est-ce un Robo ?
        if ( IsCaptchaValid == false || Page.IsValid == false )
            Response.Redirect( Tools.PageErreurPath + "IP:" + Request.UserHostAddress.ToString() + "<br/>Coming from:" + Request.UrlReferrer.ToString() + "<br/>Is Robot" );

        ValidationMessage.Text = "";

        MemberInfo membre = new MemberInfo();
        MembershipCreateStatus memberCreateStatus = MembershipCreateStatus.InvalidAnswer;
        try
        {
            Boolean isAppouved = false;
            if ( User.IsInRole( "Administrateur" ) )
            {
                isAppouved = true;
            }
            else
            {
                isAppouved = Global.SettingsXml.MembreApprouve && ( Global.SettingsXml.MembreApprouveParEmail == false );
            }

            Membership.CreateUser
            ( 
                TextBoxNomUtilisateur.Text,
                TextBoxPassWord.Text, 
                TextBoxEmail.Text, 
                TextBoxQuestion.Text, 
                TextBoxAnswer.Text,
                isAppouved,
                out memberCreateStatus 
            );

            switch ( memberCreateStatus )
            {
                case MembershipCreateStatus.InvalidPassword :
                    ValidationMessage.Text = "Mot de passe non valide.<br/>";
                    break;
                case MembershipCreateStatus.DuplicateUserName :
                    ValidationMessage.Text = "Erreur : Cet utilisateur existe déjà.<br/>";
                    break;

                case MembershipCreateStatus.Success :

                    MembershipUser user = Membership.GetUser( TextBoxNomUtilisateur.Text );
                    ValidationMessage.Text += "Création de l'utilisateur avec succès.<br/>";

                    membre.MembreGUID = ( Guid )user.ProviderUserKey;
                    membre.NomUtilisateur = user.UserName;
                    membre.MotDePasse = TextBoxPassWord.Text;
                    membre.Nom = TextBoxLastName.Text;
                    membre.Prenom = TextBoxFisrtName.Text;
                    membre.Societe = TextBoxSociete.Text;
                    membre.Telephone = TextBoxTelephone.Text;
                    membre.Adresse = TextBoxAdresse.Text;
                    membre.LimiteQuestionnaires = int.Parse( Global.SettingsXml.GratuitLimiteQuestionnaires );
                    membre.LimiteQuestions = int.Parse( Global.SettingsXml.GratuitLimiteQuestions );
                    membre.LimiteInterviewes = int.Parse( Global.SettingsXml.GratuitLimiteInterviewes );
                    membre.LimiteReponses = int.Parse( Global.SettingsXml.GratuitLimiteReponses );
                    membre.DateFinAbonnement = DateTime.Now.AddMonths( 1 ); 

                    int status = MemberInfo.Create( membre );
                    if ( status != 1 )
                    {
                        ValidationMessage.Text += "Erreur de création des informations utilisateurs.<br/>";
                        Membership.DeleteUser( user.UserName );
                        ValidationMessage.Text += "Suppression de l'utilisateur.<br/>";
                    }
                    
                    // Ajouter l'utilisateur dans le role Client
                    Roles.AddUserToRole
                    (
                        TextBoxNomUtilisateur.Text,
                        "Client"
                    );

                    // L'utilisateur est gratuit
                    Roles.AddUserToRole
                    (
                        TextBoxNomUtilisateur.Text,
                        "Découverte"
                    );

                    // Si ce n'est pas l'admin qui cree un compte
                    if ( User.IsInRole( "Administrateur" ) == false )
                    {
                        FormsAuthentication.SetAuthCookie( user.UserName, false );
                    }

                    break;
                default :
                    ValidationMessage.Text = "Erreur : MembershipCreateStatus non traité.";
                    break;
            }
        }
        catch ( Exception ex )
        {
            ValidationMessage.Text += "Erreur exception : " + ex.Message;
        }

        if ( memberCreateStatus == MembershipCreateStatus.Success )
        {
            //
            // Copier les Sections de l'admin pour le nouvel utilisateur
            //
            copierWebContent( membre.NomUtilisateur );

            // Copier le questionnaire de l'Intervieweur pour le nouveau membre
            string message = "";
            int code = int.Parse( Global.SettingsXml.CodeAccesQuestionnaireExemple );
            Questionnaire questionnaire = Questionnaire.GetByCodeAcces( code );
            if ( questionnaire != null )
            {
                message = QuestionnaireCopier.CopierQuestionnaire( questionnaire.QuestionnaireID, membre.MembreGUID, 0 );
                ValidationMessage.Text += "Copie du questionnaire d'exemple.<br/>";
            }

            //
            // Envoyer l'email de registration au nouvel utilisateur
            // si ce n'est pas un Administrateur qui enregistre un nouvel utilisateur
            //
            if ( HttpContext.Current.User.IsInRole( "Administrateur" ) == false )
            {
                string sujetEmail = "Vous êtes enregistré sur le site : " + Global.SettingsXml.SiteNom;
                string bodyEmail = "Voici vos informations d'enregistrement à conserver :<br/>";
                bodyEmail += "<br/>";
                bodyEmail += "Nom d'utilisateur : " + membre.NomUtilisateur + "<br/>";
                bodyEmail += "Mot de passe : " + membre.MotDePasse + "<br/>";
                bodyEmail += "Nom : " + membre.Nom + "<br/>";
                bodyEmail += "Prénom : " + membre.Prenom + "<br/>";
                bodyEmail += "Société : " + membre.Societe + "<br/>";
                bodyEmail += "Téléphone : " + membre.Telephone + "<br/>";
                bodyEmail += "Adresse : " + membre.Adresse + "<br/>";
                bodyEmail += "Email : " + TextBoxEmail.Text + "<br/>";

                if ( Global.SettingsXml.MembreApprouve == false && Global.SettingsXml.MembreApprouveParEmail == false )
                {
                    bodyEmail += "<br/><b>Votre compte d'utilisateur est en attente d'approbation.</b><br/>";
                    ValidationMessage.Text += "Votre compte d'utilisateur est en attente d'approbation.<br/>";
                }

                if ( Global.SettingsXml.MembreApprouveParEmail == true )
                {
                    bodyEmail += "<br/><b>Cliquez sur le lien suivant pour valider votre enregistrement :</b><br/>";
                    string lien = Utils.WebSiteUri.ToLower() + "/member/approuve.aspx?guid=" + membre.MembreGUID.ToString();
                    bodyEmail += string.Format( "<a href=\"{0}\" >{1}</a>", lien, "Validez votre enregistrement" ) + "<br/>";
                    ValidationMessage.Text += "Vous allez recevoir un email pour valider votre enregistrement.<br/>";
                }

                bodyEmail += "<br/>Lien d'accès à l'application :<br/>" + string.Format( "<a href=\"{0}\" >{1}</a>", Utils.WebSiteUri, Utils.WebSiteUri ) + "<br/>";

                MemberInfo member = MemberInfo.GetMemberInfo( "admin" );
                Courriel.EnvoyerEmailToAssynchrone( member.MembreGUID, TextBoxEmail.Text, sujetEmail, bodyEmail );
                ValidationMessage.Text += "Informations de connexion envoyées à l'adresse : " + TextBoxEmail.Text + "<br/>";
            }

            //
            // Envoyer l'email a l'administrateur
            //
            if ( Global.SettingsXml.MembrePrevenir )
            {
                string sujetEmail2 = "Enregistrement d'un nouvel utilisateur sur le site : " + Global.SettingsXml.SiteNom;
                string bodyEmail2 = "";

                bodyEmail2 += "Nom d'utilisateur : " + membre.NomUtilisateur + "<br/>";
                bodyEmail2 += "Mot de passe : " + membre.MotDePasse + "<br/>";
                bodyEmail2 += "Nom : " + membre.Nom + "<br/>";
                bodyEmail2 += "Prénom : " + membre.Prenom + "<br/>";
                bodyEmail2 += "Société : " + membre.Societe + "<br/>";
                bodyEmail2 += "Téléphone : " + membre.Telephone + "<br/>";
                bodyEmail2 += "Adresse : " + membre.Adresse + "<br/>";
                bodyEmail2 += "Email : " + TextBoxEmail.Text + "<br/>";

                if ( Global.SettingsXml.MembreApprouve )
                {
                    if ( Global.SettingsXml.MembreApprouveParEmail )
                    {
                        bodyEmail2 += "<br/>Cet utilisateur est en attente d'approbation par email.<br/>";
                    }
                    else
                    {
                        bodyEmail2 += "<br/>Cet utilisateur est approuvé.<br/>";
                    }
                }
                else
                {
                    bodyEmail2 += "<br/>Cet utilisateur est en attente d'approbation.<br/>";
                }

                bodyEmail2 += "<br/>Accès à l'application :<br/>" + string.Format( "<a href=\"{0}\" >{1}</a>", Utils.WebSiteUri, Utils.WebSiteUri ) + "<br/>";

                MemberInfo member = MemberInfo.GetMemberInfo( "admin" );
                MembershipUser user = Membership.GetUser( member.MembreGUID );

                Courriel.EnvoyerEmailToAssynchrone( member.MembreGUID, user.Email, sujetEmail2, bodyEmail2 );
                ValidationMessage.Text += "Email d'enregistrement envoyé à l'administrateur.<br/>";
            }
        }

        ValiderMessage();

        // Vider le formulaire pour que l'utilisateur ne s'enregitre pas plusieurs fois facilement
        TextBoxNomUtilisateur.Text = "";
        TextBoxQuestion.Text = "";
        TextBoxAnswer.Text = "";
        TextBoxLastName.Text = "";
        TextBoxFisrtName.Text = "";
        TextBoxEmail.Text = "";
        TextBoxSociete.Text = "";
        TextBoxAdresse.Text = "";
        TextBoxTelephone.Text = "";
        CheckBoxConditionGenerales.Checked = false;
    }

    private void copierWebContent( string utilisateur )
    {
        // BUG171020090002
        //string[] sections = { "CorpsEmail", "PageAccueil", "PageQuestionnaire", "PageTermine" };
        string[] sections = { "CorpsEmail", "PageAccueil", "PageEnregistrement", "PageQuestionnaire", "PageTermine" };
        string admin = "admin";

        foreach ( string section in sections )
        {
            WebContent ContenuAdmin = WebContent.GetWebContent( section, admin, WebContent.ToutLeMonde );
            WebContent Contenu = new WebContent();
            Contenu.Section = ContenuAdmin.Section;
            Contenu.Utilisateur = utilisateur;
            Contenu.Visualisateur = WebContent.ToutLeMonde;
            Contenu.SectionContent = ContenuAdmin.SectionContent;

            int status = WebContent.Create( Contenu );
            if ( status == 2 )
            {
                ValidationMessage.Text += "Erreur cette Page existe déjà .<br/>";
            }
            if ( status != 1 )
            {
                ValidationMessage.Text += "Erreur à la Creation de cette Page.<br/>";
            }
        }
    }

    protected void ButtonRetour_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Member/Manage.aspx" );
    }

    #region CAPTCHA

    /// <summary> 
    /// Initializes the captcha and registers the JavaScript 
    /// </summary> 
    private void InititializeCaptcha()
    {
        if ( ViewState[ "captchavalue" ] == null )
        {
            ViewState[ "captchavalue" ] = Guid.NewGuid().ToString();
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine( "function SetCaptcha(){" );
        sb.AppendLine( "var form = document.getElementById('" + Page.Form.ClientID + "');" );
        sb.AppendLine( "var el = document.createElement('input');" );
        sb.AppendLine( "el.type = 'hidden';" );
        sb.AppendLine( "el.name = 'captcha';" );
        sb.AppendLine( "el.value = '" + ViewState[ "captchavalue" ] + "';" );
        sb.AppendLine( "form.appendChild(el);}" );

        Page.ClientScript.RegisterClientScriptBlock( GetType(), "captchascript", sb.ToString(), true );
        Page.ClientScript.RegisterOnSubmitStatement( GetType(), "captchayo", "SetCaptcha()" );
    }

    /// <summary> 
    /// Gets whether or not the user is human 
    /// </summary> 
    private bool IsCaptchaValid
    {
        get
        {
            if ( ViewState[ "captchavalue" ] != null )
            {
                return Request.Form[ "captcha" ].Substring( 0, 36) == ViewState[ "captchavalue" ].ToString();
            }

            return false;
        }
    }

    #endregion
}
