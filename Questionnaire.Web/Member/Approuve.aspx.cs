using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Sql.Web.Data;

public partial class Member_Approuve : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( IsPostBack == false )
        {
            if ( Request[ "guid" ] == null )
            {
                ValidationMessage.Text = "Erreur";

                // DEBUG obtenir le guid d'un membre
                //MembershipUser user = Membership.GetUser( User.Identity.Name );
                //Guid MembreGUID = new Guid( user.ProviderUserKey.ToString() );
                //string guid = MembreGUID.ToString();
            }
            else
            {
                Guid MembreGUID = new Guid( Request.QueryString[ "guid" ] );
                MembershipUser user = Membership.GetUser( MembreGUID );
                if ( user == null )
                {
                    ValidationMessage.Text = "Erreur, vous n'êtes pas membre.";
                }
                else
                {
                    MemberSettings memberSettings = MemberSettings.GetMemberSettings( user.UserName );
                    
                    // L'utilisateur a deja ete approuve, precaution pour qu'un petit malin qui serait
                    // desapprouve par l'admin retrouve l'email d'approbation et reclique dessus
                    if ( memberSettings.Approuve == true )
                    {
                        ValidationMessage.Text = "Vous êtes déjà approuvé, contactez l'administrateur.";
                    }
                    else
                    {
                        memberSettings.Approuve = true;
                        memberSettings.Update( user.UserName, memberSettings );
                        user.IsApproved = true;
                        Membership.UpdateUser( user );
                        ButtonLogin.Visible = true;
                        ValidationMessage.Text += "Votre enregistrement est maintenant validé.<br/>";
                        ValidationMessage.Text += "Vous pouvez vous connecter à l'application.<br/>";

                        //
                        // Envoyer l'email pour prevenir l'administrateur de l'approbation d'un utilisateur
                        //
                        if ( Global.SettingsXml.MembrePrevenir )
                        {
                            string sujetEmail2 = "Approbation par le formulaire d'un nouvel utilisateur sur le site : " + Global.SettingsXml.SiteNom;
                            string bodyEmail2 = "";

                            MemberInfo membre = MemberInfo.Get( MembreGUID );
                            bodyEmail2 += "Nom d'utilisateur : " + membre.NomUtilisateur + "<br/>";
                            bodyEmail2 += "Mot de passe : " + membre.MotDePasse + "<br/>";
                            bodyEmail2 += "Nom : " + membre.Nom + "<br/>";
                            bodyEmail2 += "Prénom : " + membre.Prenom + "<br/>";
                            bodyEmail2 += "Société : " + membre.Societe + "<br/>";
                            bodyEmail2 += "Téléphone : " + membre.Telephone + "<br/>";
                            bodyEmail2 += "Adresse : " + membre.Adresse + "<br/>";
                            bodyEmail2 += "Email : " + user.Email + "<br/>";

                            bodyEmail2 += "<br/>Accès à l'application :<br/>" + string.Format( "<a href=\"{0}\" >{1}</a>", Utils.WebSiteUri, Utils.WebSiteUri ) + "<br/>";

                            MemberInfo member = MemberInfo.GetMemberInfo( "admin" );
                            MembershipUser admin = Membership.GetUser( member.MembreGUID );

                            Courriel.EnvoyerEmailToAssynchrone( member.MembreGUID, admin.Email, sujetEmail2, bodyEmail2 );
                        }
                    }
                }
            }
        }

        ValiderMessage();
    }

    void ValiderMessage()
    {
        if ( ValidationMessage.Text != "" )
        {
            ValidationMessage.Visible = true;
        }
    }

    protected void ButtonLogin_Click( object sender, EventArgs e )
    {
        Response.Redirect( FormsAuthentication.DefaultUrl );
    }
}

