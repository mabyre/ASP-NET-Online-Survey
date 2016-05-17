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

public partial class Page_MemberEdit : PageBase
{
    // BUG10092009 static Guid MembreGUID = Guid.Empty;
    private Guid MembreGUID
    {
        get
        {
            if ( ViewState[ "MembreGUID" ] != null )
                return ( Guid )( ViewState[ "MembreGUID" ] );

            return Guid.Empty;
        }

        set
        {
            ViewState[ "MembreGUID" ] = value;
        }
    }

    protected void Page_Load( object sender, System.EventArgs e )
    {
        if ( IsPostBack == false )
        {
            if ( Request.QueryString[ "MembreGUID" ] != null )
            {
                MembreGUID = new Guid( Request.QueryString[ "MembreGUID" ] );
            }
            else
            {
                if ( SessionState.MemberInfo == null )
                {
                    Response.Redirect( "~/Member/Manage.aspx" );
                }
                MembreGUID = SessionState.MemberInfo.MembreGUID;
            }

            MemberInfo member = MemberInfo.Get( MembreGUID );
            MembershipUser user = Membership.GetUser( MembreGUID );

            TextBoxUserName.Text = user.UserName;
            TextBoxUserName.Enabled = false; // On ne peut pas changer de UserName !
            TextBoxEmail.Text = user.Email;

            TextBoxMotDePasse.Text = member.MotDePasse;
            TextBoxMotDePasse.Enabled = false; // Ici on ne change pas le mot de passe
            TextBoxNom.Text = member.Nom;
            TextBoxPrenom.Text = member.Prenom;
            TextBoxAdresse.Text = member.Adresse;
            TextBoxTelephone.Text = member.Telephone;
            TextBoxSociete.Text = member.Societe;

            LabelOnline.Text = user.IsOnline.ToString();
            if ( user.IsOnline )
            {
                LabelOnline.CssClass = "LabelGreenStyle";
            }
            CheckBoxUserIsApproved.Checked = user.IsApproved;
            LabelIsApproved.Text = user.IsApproved.ToString();
            if ( user.IsApproved == false ) 
            {
                LabelIsApproved.CssClass = "LabelRedStyle";
            }

            LabelIsLockedOut.Text = user.IsLockedOut.ToString();
            if ( user.IsLockedOut )
            {
                LabelIsLockedOut.CssClass = "LabelRedStyle";
            }

            LabelCreationDate.Text = user.CreationDate.ToString();
            LabelDateCreation.Text = user.CreationDate.ToString();
            LabelLastLoginDate.Text = user.LastLoginDate.ToString();
            LabelDerniereConnexion.Text = user.LastLoginDate.ToString();
            LastLockoutDate.Text = user.LastLockoutDate.ToString();
            LabelActivityDate.Text = user.LastActivityDate.ToString();
            LabelLastPasswordChangedDate.Text = user.LastPasswordChangedDate.ToString();
            LabelChangementMotPasse.Text = user.LastPasswordChangedDate.ToString();

            // Gestion des roles specifiques a l'application
            CheckBoxDebloquerClient.Checked = Roles.IsUserInRole( user.UserName, "Client" );
            CheckBoxAdministrateur.Checked = Roles.IsUserInRole( user.UserName, "Administrateur" );
            CheckBoxClientVerrouille.Checked = user.IsLockedOut;
            CheckBoxMembreDecouverte.Checked = Roles.IsUserInRole( user.UserName, "Découverte" );
            if ( User.IsInRole( "Administrateur" ) )
            {
                PanelInfosMembre.Visible = true;
                PanelDebloquerClient.Visible = Global.SettingsXml.DebloquerClient;
            }
            else
            {
                // Seul un administrateur peut supprimer un membre
                // Il y a une autre protection : la page Member/Delete n'est pas accessible
                //TdButtonSupprimer.Visible = false;
                ButtonSupprimer.ToolTip = "Visualisez votre compte avant suppression, attention vous n'aurez plus accès à cette plateforme.";
            }

            // Recalculer les Objets du membre
            SessionState.Limitations = null;

            // Nombre d'objets de du membre
            LabelLimiteQuestionnaires.Text = SessionState.Limitations.Questionnaires.ToString();
            LabelLimiteQuestions.Text = SessionState.Limitations.Questions.ToString();
            LabelLimiteInterviewes.Text = SessionState.Limitations.Interviewes.ToString();
            LabelRepondant.Text = SessionState.Limitations.Reponses.ToString();

            // Nombre d'objets de l'abonnement
            TextBoxAbonneLimiteQuestionnaires.Text = member.LimiteQuestionnaires.ToString();
            TextBoxAbonneLimiteQuestions.Text = member.LimiteQuestions.ToString();
            TextBoxAbonneLimiteInterviewes.Text = member.LimiteInterviewes.ToString();
            TextBoxAbonneLimiteReponses.Text = member.LimiteReponses.ToString();
            TextBoxDateFin.Text = member.DateFinAbonnement.ToShortDateString();

            if ( User.IsInRole( "Découverte" ) || User.IsInRole( "Administrateur" ) )
            {
                // Limites actuelles du compte 'Abonne'
                PanelLimiteAbonne.Visible = true;
                LabelLimiteCompteClientQuestionnaires.Text = Global.SettingsXml.AbonneLimiteQuestionnaires.ToString();
                LabelLimiteCompteClientQuestions.Text = Global.SettingsXml.AbonneLimiteQuestions.ToString();
                LabelLimiteCompteClientInterviewes.Text = Global.SettingsXml.AbonneLimiteInterviewes.ToString();
                LabelLimiteCompteClientReponses.Text = Global.SettingsXml.AbonneLimiteReponses.ToString();
            }

            if ( User.IsInRole( "Administrateur" ) )
            {
                LabelTitreAbonnement.Text = user.UserName;

                PanelMembreLimitePourAdmin.Visible = true;
                PanelMembreLimitePourAdmin.GroupingText += "'" + user.UserName + "'";
                Limitation limites = new Limitation( MembreGUID );
                LabelLimiteQuestionnairesPourAdmin.Text = limites.Questionnaires.ToString();
                LabelLimiteQuestionsPourAdmin.Text = limites.Questions.ToString();
                LabelLimiteInterviewesPourAdmin.Text = limites.Interviewes.ToString();
                LabelLimiteReponsePourAdmin.Text = limites.Reponses.ToString();

                TrBoutonAbonne.Visible = true;
                TextBoxAbonneLimiteQuestionnaires.Enabled = true;
                TextBoxAbonneLimiteQuestionnaires.CssClass = "TextBoxRegisterStyle";

                TextBoxAbonneLimiteQuestions.Enabled = true;
                TextBoxAbonneLimiteQuestions.CssClass = "TextBoxRegisterStyle";

                TextBoxAbonneLimiteInterviewes.Enabled = true;
                TextBoxAbonneLimiteInterviewes.CssClass = "TextBoxRegisterStyle";

                TextBoxAbonneLimiteReponses.Enabled = true;
                TextBoxAbonneLimiteReponses.CssClass = "TextBoxRegisterStyle";

                TextBoxDateFin.Enabled = true;
                TextBoxDateFin.CssClass = "TextBoxRegisterStyle";
            }

            ValiderMessage();
        }
    }

    void ValiderMessage()
    {
        if ( SessionState.ValidationMessage != null )
        {
            ValidationMessage.Text = SessionState.ValidationMessage;
            ValidationMessage.Visible = true;
            SessionState.ValidationMessage = null;
        }
        else
        {
            ValidationMessage.Visible = false;
        }
    }

    protected void ButtonChangePassWord_Click( object sender, EventArgs e )
    {
        if ( MembreGUID != Guid.Empty )
        {
            MemberInfo member = MemberInfo.Get( MembreGUID );
            MembershipUser user = Membership.GetUser( MembreGUID );

            try
            {
                if ( user.ChangePassword( TextBoxMotDePasse.Text, TextBoxNouveauMotDePasse.Text ) )
                {
                    SessionState.ValidationMessage += "Mot de passe modifié avec succès.<br/>";
                    member.MotDePasse = TextBoxNouveauMotDePasse.Text;
                    int retCode = MemberInfo.Update( member );
                    if ( retCode == 1 )
                    {
                        // Membre mis à jour correctement
                        TextBoxMotDePasse.Text = TextBoxNouveauMotDePasse.Text;
                    }
                    else
                    {
                        SessionState.ValidationMessage += "Erreur sur la mise à jour du Membre.<br/>";
                        ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                    }
                }
                else
                {
                    SessionState.ValidationMessage = "Changement de mot de passe raté, essayez encore.<br/>";
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                }
            }
            catch ( Exception ex )
            {
                SessionState.ValidationMessage = "Erreur exception : " + Server.HtmlEncode( ex.Message ) + "<br/>Essayez encore.<br/>";
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            }

        }

        ValiderMessage();
    }

    protected void ButtonAbonner_Click( object sender, EventArgs e )
    {
        // Ajouter le nombre d'objets de l'abonnement fixe par l'admin
        int limite = int.Parse( Global.SettingsXml.AbonneLimiteQuestionnaires );
        TextBoxAbonneLimiteQuestionnaires.Text = limite.ToString();
        TextBoxAbonneLimiteQuestionnaires.CssClass = "TextBoxRegisterBlueStyle";

        limite = int.Parse( Global.SettingsXml.AbonneLimiteQuestions );
        TextBoxAbonneLimiteQuestions.Text = limite.ToString();
        TextBoxAbonneLimiteQuestions.CssClass = "TextBoxRegisterBlueStyle";

        limite = int.Parse( Global.SettingsXml.AbonneLimiteInterviewes );
        TextBoxAbonneLimiteInterviewes.Text = limite.ToString();
        TextBoxAbonneLimiteInterviewes.CssClass = "TextBoxRegisterBlueStyle";

        limite = int.Parse( Global.SettingsXml.AbonneLimiteReponses );
        TextBoxAbonneLimiteReponses.Text = limite.ToString();
        TextBoxAbonneLimiteReponses.CssClass = "TextBoxRegisterBlueStyle";

        CheckBoxMembreDecouverte.Checked = false;
        CheckBoxMembreDecouverte_CheckedChanged( sender, e );
        ButtonSave_Click( sender, e );
    }

    protected void ButtonSave_Click( object sender, EventArgs e )
    {
        if ( MembreGUID != Guid.Empty )
        {
            MemberInfo member = MemberInfo.Get( MembreGUID );
            MembershipUser user = Membership.GetUser( MembreGUID );

            //user.UserName = TextBoxUserName.Text; IMPOSSIBLE
            user.Email = TextBoxEmail.Text;
            Membership.UpdateUser( user );
           
            // Informations personnelles
            member.Nom = TextBoxNom.Text;
            member.Prenom = TextBoxPrenom.Text;
            member.Adresse = TextBoxAdresse.Text;
            member.Telephone = TextBoxTelephone.Text;
            member.Societe = TextBoxSociete.Text;

            // Abonnement
            int limite = 0;
            try 
            { 
                limite = int.Parse( TextBoxAbonneLimiteQuestionnaires.Text );
                member.LimiteQuestionnaires = limite;
            }
            catch 
            {
                SessionState.ValidationMessage += "Erreur sur l'abonnement en Questionnaires.<br/>";
                TextBoxAbonneLimiteQuestionnaires.Text = member.LimiteQuestionnaires.ToString();
            }

            try 
            { 
                limite = int.Parse( TextBoxAbonneLimiteQuestions.Text );
                member.LimiteQuestions = limite;
            }
            catch 
            {
                SessionState.ValidationMessage += "Erreur sur l'abonnement en Questions.<br/>";
                TextBoxAbonneLimiteQuestions.Text = member.LimiteQuestions.ToString();
            }

            try 
            { 
                limite = int.Parse( TextBoxAbonneLimiteInterviewes.Text );
                member.LimiteInterviewes = limite;
            }
            catch 
            {
                SessionState.ValidationMessage += "Erreur sur l'abonnement en Interviewés.<br/>";
                TextBoxAbonneLimiteInterviewes.Text = member.LimiteInterviewes.ToString();
            }

            try 
            { 
                limite = int.Parse( TextBoxAbonneLimiteReponses.Text );
                member.LimiteReponses = limite;
            }
            catch 
            {
                SessionState.ValidationMessage += "Erreur sur l'abonnement en Réponses.<br/>";
                TextBoxAbonneLimiteReponses.Text = member.LimiteReponses.ToString();
            }

            DateTime dt = DateTime.Now;
            try
            {
                dt = DateTime.Parse( TextBoxDateFin.Text );
                member.DateFinAbonnement = dt;
            }
            catch
            {
                SessionState.ValidationMessage += "Erreur sur l'abonnement en Date de Fin.<br/>";
                TextBoxDateFin.Text = member.DateFinAbonnement.ToShortDateString();
            }

            //------------------ 
            // Update MemberInfo
            //------------------ 
            int retCode = MemberInfo.Update( member );
            if ( retCode == 1 )
            {
                SessionState.ValidationMessage += "Membre mis à jour correctement.<br/>";
            }
            else
            {
                SessionState.ValidationMessage += "Erreur sur la mise à jour du Membre.<br/>";
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            }
        }

        ValiderMessage();
    }

    protected void ButtonCancel_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Member/Manage.aspx" );
    }

    protected void ButtonSupprimer_Click( object sender, EventArgs e )
    {
        if ( MembreGUID == Guid.Empty )
        {
            SessionState.ValidationMessage += "Choisir un membre à supprimer.<br/>";
            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
        }
        else
        {
            Response.Redirect( "~/Member/Delete.aspx?MembreGUID=" + MembreGUID.ToString() );
        }

        ValiderMessage();
    }

    protected void CheckBoxUserIsApproved_CheckedChanged( object sender, EventArgs e )
    {
        MembershipUser user = Membership.GetUser( MembreGUID );
        if ( CheckBoxUserIsApproved.Checked == true )
        {
            user.IsApproved = true;
            SessionState.ValidationMessage += "Utilisateur approuvé.<br/>";
        }
        else
        {
            user.IsApproved = false;
            SessionState.ValidationMessage += "Utilisateur non approuvé.<br/>";
        }
        Membership.UpdateUser( user );

        ValiderMessage();
    }    

    protected void CheckBoxDebloquerClient_CheckedChanged( object sender, EventArgs e )
    {
        MembershipUser user = Membership.GetUser( MembreGUID );

        if ( CheckBoxDebloquerClient.Checked == true )
        {
            if ( Roles.IsUserInRole( user.UserName, "Client" ) )
            {
                SessionState.ValidationMessage = "Utilisateur déjà dans le rôle \"Client\".";
            }
            else
            {
                Roles.AddUserToRole
                (
                    user.UserName,
                    "Client"
                );
                SessionState.ValidationMessage = "Utilisateur ajouté au rôle \"Client\".";
            }
        }
        else
        {
            if ( Roles.IsUserInRole( user.UserName, "Client" ) )
            {
                Roles.RemoveUserFromRole
                (
                    user.UserName,
                    "Client"
                );
                SessionState.ValidationMessage = "Utilisateur retiré du rôle \"Client\".";
            }
            else
            {
                SessionState.ValidationMessage = "Utilisateur n'est pas dans le rôle \"Client\".";
            }
        }

        ValiderMessage();
    }

    protected void CheckBoxAdministrateur_CheckedChanged( object sender, EventArgs e )
    {
        MembershipUser user = Membership.GetUser( MembreGUID );

        if ( CheckBoxAdministrateur.Checked == true )
        {
            if ( Roles.IsUserInRole( user.UserName, "Administrateur" ) )
            {
                SessionState.ValidationMessage = "Utilisateur déjà dans le rôle \"Administrateur\".";
            }
            else
            {
                Roles.AddUserToRole
                (
                    user.UserName,
                    "Administrateur"
                );
                SessionState.ValidationMessage = "Utilisateur ajouté au rôle \"Administrateur\".";
            }
        }
        else
        {
            if ( Roles.IsUserInRole( user.UserName, "Administrateur" ) )
            {
                Roles.RemoveUserFromRole
                (
                    user.UserName,
                    "Administrateur"
                );
                SessionState.ValidationMessage = "Utilisateur retiré du rôle \"Administrateur\".";
            }
            else
            {
                SessionState.ValidationMessage = "Utilisateur n'est pas dans le rôle \"Administrateur\".";
            }
        }

        ValiderMessage();
    }

    protected void CheckBoxClientVerrouille_CheckedChanged( object sender, EventArgs e )
    {
        MembershipUser user = Membership.GetUser( MembreGUID );

        if ( CheckBoxClientVerrouille.Checked == false )
        {
            user.UnlockUser();
            SessionState.ValidationMessage = "Utilisateur dévérouillé.";
        }
        else
        {
            SessionState.ValidationMessage = "Utilisateur est vérouillé.";
        }

        ValiderMessage();
    }

    protected void CheckBoxMembreDecouverte_CheckedChanged( object sender, EventArgs e )
    {
        MembershipUser user = Membership.GetUser( MembreGUID );

        if ( CheckBoxMembreDecouverte.Checked == true )
        {
            if ( Roles.IsUserInRole( user.UserName, "Découverte" ) )
            {
                SessionState.ValidationMessage = "Utilisateur déjà dans le rôle \"Découverte\".";
            }
            else
            {
                Roles.AddUserToRole
                (
                    user.UserName,
                    "Découverte"
                );
                SessionState.ValidationMessage = "Utilisateur ajouté au rôle \"Découverte\".";
            }
        }
        else
        {
            if ( Roles.IsUserInRole( user.UserName, "Découverte" ) )
            {
                Roles.RemoveUserFromRole
                (
                    user.UserName,
                    "Découverte"
                );
                SessionState.ValidationMessage = "Utilisateur retiré du rôle \"Découverte\".";
            }
            else
            {
                SessionState.ValidationMessage = "Utilisateur n'est pas dans le rôle \"Découverte\".";
            }
        }

        ValiderMessage();
    }
}
