using System;
using System.Text;
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
using System.Globalization;

//
// C'est vraiment une grosse merde cette authentification Windows
// J'ai trouve un autre moyen que dans le SiteVitrine le Response.Redirect avec err=name
// me faisait vraiement chier.
// Ici on laisse l'utilisateur s'authentifier avec n'importe quoi
// on intercepte le UserName.Text pour le mettre au "vrai" nom d'utilisateur et on a
// bien modifie User.Identity.UserName ce que je n'arrivais pas a faire avant !!
//

public partial class Page_Login : PageBase
{
    protected void Page_Load( object sender, EventArgs e )
    {
        string culture = CultureInfo.CurrentCulture.Name;

        if ( User.Identity.IsAuthenticated == false )
        {
            // Donner le focus au bouton
            // Rappel AnonymousTemplate n'existe que si l'utilisateur n'est pas authentifie !
            Login LoginControl = ( Login )LoginViewAuthentification.FindControl( "LoginControl" );
            RolloverButton LoginButton = ( RolloverButton )LoginControl.FindControl( "LoginButton" );
            Page.Form.DefaultButton = LoginButton.UniqueID;

            // BUG100920090003 Pour en finir avec toutes ces merdes d'authentification
            TextBox UserName = ( TextBox )LoginControl.FindControl( "UserName" );
            TextBox Password = ( TextBox )LoginControl.FindControl( "Password" );
            bool userIsValide = Membership.ValidateUser( UserName.Text, Password.Text );
            if ( userIsValide )
            {
                MembershipUser user = Membership.GetUser( UserName.Text );
                if ( Membership.GetUser( user.ProviderUserKey ).UserName != UserName.Text )
                {
                    // Remettre le "vrai" nom d'utilisateur et User.Identity.Name aura cette valeur
                    UserName.Text = Membership.GetUser( user.ProviderUserKey ).UserName;
                }
            }
        }

        if ( IsPostBack == false )
        {
            if ( User.Identity.IsAuthenticated )
            {
                Label LabelUserInRoles = ( Label )LoginViewAuthentification.FindControl( "LabelUserInRoles" );
                LabelUserInRoles.Text = "";
                string[] roles = Roles.GetRolesForUser();
                if ( roles.Length >= 0 )
                {
                    foreach ( string r in roles )
                    {
                        LabelUserInRoles.Text += r;
                        if ( roles.Length > 1 )
                            LabelUserInRoles.Text += " | ";
                    }
                }
                
                // 
                // Envoit de l'email de connexion d'un utilisateur a l'admin
                //
                if ( Global.SettingsXml.MembreConnexionPrevenir && ( User.IsInRole( "Administrateur" ) == false ) )
                {
                    // On ne sait pas empecher l'utilisateur de mettre des espace dans son nom d'utilisateur
                    // ces espaces se retrouvnet dans User.Identity.Name et donc petage NULL REF
                    // donc ici on met un Trim()
                    MemberInfo membre = MemberInfo.GetMemberInfo( User.Identity.Name.Trim() );
                    MembershipUser user = Membership.GetUser( User.Identity.Name.Trim() );

                    string sujetEmail2 = "Connexion d'un Membre sur le site : " + Global.SettingsXml.SiteNom;
                    string bodyEmail2 = "";

                    bodyEmail2 += "Nom d'utilisateur : " + membre.NomUtilisateur + "<br/>";
                    bodyEmail2 += "Nom : " + membre.Nom + "<br/>";
                    bodyEmail2 += "Prénom : " + membre.Prenom + "<br/>";
                    bodyEmail2 += "Société : " + membre.Societe + "<br/>";
                    bodyEmail2 += "Téléphone : " + membre.Telephone + "<br/>";
                    bodyEmail2 += "Adresse : " + membre.Adresse + "<br/>";
                    bodyEmail2 += "Email : " + user.Email + "<br/>";
                    bodyEmail2 += "Fin abonnement : " + membre.DateFinAbonnement.ToShortDateString() + "<br/>";
                    if ( user.IsApproved == false )
                    {
                        bodyEmail2 += "<br>Cet utilisateur n'est pas approuvé.<br/>";
                    }
                    if ( user.IsLockedOut )
                    {
                        bodyEmail2 += "<br>Cet utilisateur est vérouillé.<br/>";
                    }
                    bodyEmail2 += "<br/>Accès à l'application :<br/>" + string.Format( "<a href=\"{0}\" >{1}</a>", Utils.WebSiteUri, Utils.WebSiteUri ) + "<br/>";

                    MemberInfo admin = MemberInfo.GetMemberInfo( "admin" );
                    MembershipUser userAdmin = Membership.GetUser( admin.MembreGUID );

                    Courriel.EnvoyerEmailToAssynchrone( admin.MembreGUID, userAdmin.Email, sujetEmail2, bodyEmail2 );
                }

                Response.Redirect( "~/Wizard/Accueil.aspx" );
            }
        }
    }

    protected void LoginButton_Click( object sender, System.EventArgs e )
    {
        // Annulation des variables de Session precedentes
        Session.Clear();

        Login LoginControl = ( Login )LoginViewAuthentification.FindControl( "LoginControl" );
        TextBox UserName = ( TextBox )LoginControl.FindControl( "UserName" );
        TextBox Password = ( TextBox )LoginControl.FindControl( "Password" );
        Literal FailureText = ( Literal )LoginControl.FindControl( "FailureText" );

        Label LabelMessageUtilisateur = ( Label )LoginViewAuthentification.FindControl( "LabelMessageUtilisateur" );

        MembershipUser user = Membership.GetUser( UserName.Text );
        if ( user != null )
        {
            if ( user.IsApproved == false )
            {
                LabelMessageUtilisateur.Text = "Votre compte n'est pas approuvé, contactez l'administrateur.";
                LabelMessageUtilisateur.CssClass = "LabelRedStyle";
                LabelMessageUtilisateur.Visible = true;
                FailureText.Visible = false; // On peut pas changer le texte mais ca ca marche !
            }

            if ( user.IsLockedOut )
            {
                LabelMessageUtilisateur.Text = "Votre compte est vérouillé, contactez l'administrateur.";
                LabelMessageUtilisateur.CssClass = "LabelRedStyle";
                LabelMessageUtilisateur.Visible = true;
                //FailureText.Visible = false;
            }
        }

        //
        // Cela ne fonctionne pas sur le serveur
        //

        //if ( Global.SettingsXml.MembreConnexionPrevenir && ( User.IsInRole( "Administrateur" ) == false ) )
        //{
        //    // On ne sait pas empecher l'utilisateur de mettre des espace dans son nom d'utilisateur
        //    // ces espaces se retrouvnet dans User.Identity.Name et donc petage NULL REF
        //    // donc ici on met un Trim()
        //    MemberInfo membre = MemberInfo.GetMemberInfo( User.Identity.Name.Trim() );
        //    //MembershipUser user = Membership.GetUser( User.Identity.Name.Trim() );

        //    string sujetEmail2 = "Connexion d'un Membre sur le site : " + Global.SettingsXml.SiteNom;
        //    string bodyEmail2 = "";

        //    bodyEmail2 += "Nom d'utilisateur : " + membre.NomUtilisateur + "<br/>";
        //    bodyEmail2 += "Nom : " + membre.Nom + "<br/>";
        //    bodyEmail2 += "Prénom : " + membre.Prenom + "<br/>";
        //    bodyEmail2 += "Société : " + membre.Societe + "<br/>";
        //    bodyEmail2 += "Téléphone : " + membre.Telephone + "<br/>";
        //    bodyEmail2 += "Adresse : " + membre.Adresse + "<br/>";
        //    bodyEmail2 += "Email : " + user.Email + "<br/>";
        //    if ( user.IsApproved == false )
        //    {
        //        bodyEmail2 += "<br>Cet utilisateur n'est pas approuvé.<br/>";
        //    }
        //    if ( user.IsLockedOut )
        //    {
        //        bodyEmail2 += "<br>Cet utilisateur est vérouillé.<br/>";
        //    }
        //    bodyEmail2 += "<br/>Accès à l'application :<br/>" + string.Format( "<a href=\"{0}\" >{1}</a>", Utils.WebSiteUri, Utils.WebSiteUri ) + "<br/>";

        //    MemberInfo admin = MemberInfo.GetMemberInfo( "admin" );
        //    MembershipUser userAdmin = Membership.GetUser( admin.MembreGUID );

        //    Courriel.EnvoyerEmailToAssynchrone( admin.MembreGUID, userAdmin.Email, sujetEmail2, bodyEmail2 );
        //}

    }
}
