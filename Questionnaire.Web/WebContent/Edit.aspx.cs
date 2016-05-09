using System;
using System.IO;
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
using BusinessObject;

public partial class WebContent_Edit : PageBase
{
    private static string BaseFileDirectory = VirtualPathUtility.ToAbsolute( "~/UserFiles/" );

    private void Page_Load( object sender, System.EventArgs e )
    {
        if ( Page.IsPostBack == false )
        {
            if ( WebContent.CanEdit() == false )
            {
                string msg = "Vous n'avez pas les droits pour éditer ce contenu.";
                Response.Redirect( Tools.PageErreurPath + msg );
            }

            // Pourquoi il y a ce code ici ?
            SessionState.WebContent = null;

            // Depuis l'interface graphique, l'admin et le membres ne peuvent voir et 
            // editer que les pages "Tout le monde"
            if ( Request.QueryString[ "sectionname" ] != null )
            {
                string section = Request.QueryString[ "sectionname" ].ToString();
                string utilisateur = WebContent.GetUtilisateur();
                SessionState.WebContent = WebContent.GetWebContent( section, utilisateur, WebContent.ToutLeMonde );
                if ( SessionState.WebContent == null )
                {
                    FCKeditor1.Value = "<p></p>";
                }
                else
                {
                    FCKeditor1.Value = SessionState.WebContent.SectionContent;
                }
            }
            
            if ( Request.QueryString[ "id" ] != null )
            {
                // Edition d'une section
                int webContentID = int.Parse( Request.QueryString[ "id" ] );
                SessionState.WebContent = WebContent.GetWebContent( webContentID );
                if ( SessionState.WebContent == null )
                {
                    FCKeditor1.Value = "<p></p>";
                }
                else
                {
                    FCKeditor1.Value = SessionState.WebContent.SectionContent;
                }
            }

            // Page Supprimee
            if ( SessionState.WebContent == null && ( Request.QueryString[ "sectionname" ] != null || Request.QueryString[ "id" ] != null ) )
            {
                PanelCommande.Visible = false;
                PanelPageInexistante.Visible = true;
                LabelPageInexistante.Text = "Page supprimée";
                DropDownListQuestionnaire.Visible = false;
            }

            // Page existante
            if ( SessionState.WebContent != null )
            {
                // Selection de la Section 
                DropDownListWebContentSection.SelectedSection = SessionState.WebContent.Section;

                // Selection du membre
                // BUG20100210
                if ( User.IsInRole( "Administrateur" ) )
                {
                    // Selection du membre si l'admin n'est pas en train de gerer ses pages
                    if ( SessionState.WebContent.Utilisateur != WebContent.Admin )
                    {
                        MemberInfo membre = MemberInfo.GetMemberInfo( SessionState.WebContent.Utilisateur );
                        string selectedMembre = membre.Nom + "/" + membre.Prenom + "/" + membre.Societe; // Respecter le format de la DDL
                        DropDownListMembre.SelectedMembre = selectedMembre;
                    }
                }

                // Selection de la liste des questionnaires
                // BUG20100210
                DropDownListQuestionnaire.SelectedMembreGUID = SessionState.MemberInfo.MembreGUID;
                if ( User.IsInRole( "Administrateur" ) )
                {
                    // Selection du membre si l'admin n'est pas en train de gerer ses pages
                    if ( SessionState.WebContent.Utilisateur != WebContent.Admin )
                    {
                        MemberInfo membre = MemberInfo.GetMemberInfo( SessionState.WebContent.Utilisateur );
                        DropDownListQuestionnaire.SelectedMembreGUID = membre.MembreGUID;
                    }
                }

                // Selection du Visualiseur
                if ( SessionState.WebContent.Visualisateur != WebContent.ToutLeMonde )
                {
                    int code = int.Parse( SessionState.WebContent.Visualisateur );
                    Questionnaire q = SessionState.Questionnaires.FindByCodeAcces( code );
                    DropDownListQuestionnaire.SelectedQuestionnaire = q.Description + ":" + q.CodeAcces;
                }
                else
                {
                    DropDownListQuestionnaire.SelectedQuestionnaire = WebContent.ToutLeMonde;
                }
            }

            // Panel
            if ( User.IsInRole( "Administrateur" ) )
            {
                PanelAdmin.Visible = true;
            }
            else
            {
                PanelAdmin.Visible = false;
            }
        }

        if ( Request.QueryString[ "sectionname" ] != null || Request.QueryString[ "id" ] != null )
        {
            ButtonSupprimer.Visible = true;
            ButtonSauver.Text = "Sauver";
            ButtonSauver.Click += new EventHandler( ButtonSauver_Click );
        }
        else
        {
            ButtonSupprimer.Visible = false;
            ButtonSauver.Text = "Créer";
            ButtonSauver.Click += new EventHandler( ButtonCreer_Click );
        }

        MessageValider();
    }

    void MessageValider()
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

    protected void DropDownListMembre_SelectedIndexChanged( object sender, EventArgs e )
    {
        if ( DropDownListMembre.SelectedIndex != 0 )
        {
            Guid membreGUID = ( Guid )DropDownListMembre.MembreGUID[ DropDownListMembre.SelectedIndex - 1 ];
            DropDownListQuestionnaire.SelectedQuestionnaire = "-1";
            DropDownListQuestionnaire.SelectedMembreGUID = membreGUID;
        }
    }

    protected void DropDownListWebContentSection_SelectedIndexChanged( object sender, EventArgs e )
    {
    }

    protected void ButtonSauver_Click( object sender, System.EventArgs e )
    {
        if ( DropDownListWebContentSection.SelectedSection == "-1" )
        {
            SessionState.ValidationMessage += "Sélectionnez une Section.<br/>";
        }
        if ( !WebContent.CanEdit() )
        {
            SessionState.ValidationMessage += "Vous n'avez pas les droits pour éditer cette section.<br/>";
        }
        if ( SessionState.ValidationMessage != null )
        {
            Response.Redirect( Request.RawUrl );
        }

        string newContenu = "";
        string oldContenu = "";
        if ( FCKeditor1.Value == "" )
        {
            newContenu = "<p></p>";
        }
        else
        {
            newContenu = FCKeditor1.Value;
        }

        oldContenu = SessionState.WebContent.SectionContent;
        SessionState.WebContent.SectionContent = newContenu;
        SessionState.WebContent.Section = DropDownListWebContentSection.SelectedSection;

        if ( DropDownListWebContentSection.SelectedSection == "CorpsEmail" )
        {
            LogonInterviewe logon = new LogonInterviewe( newContenu );
            if ( logon.Message != "" )
            {
                ValidationMessage.Text = logon.Message;
                ValidationMessage.Visible = true;
                return;
            }
        }

        if ( User.IsInRole( "Administrateur" ) )
        {
            if ( DropDownListMembre.SelectedMembre != "-1" )
            {
                // Admin sauve la page pour lui meme
                if ( DropDownListMembre.SelectedMembre == "-1" )
                {
                    SessionState.WebContent.Utilisateur = WebContent.Admin;
                }
                else // Trouver l'utilisateur
                {
                    Guid membreGuid = ( Guid )DropDownListMembre.MembreGUID[ DropDownListMembre.SelectedIndex - 1 ];
                    MemberInfo membre = MemberInfo.Get( membreGuid );
                    SessionState.WebContent.Utilisateur = membre.NomUtilisateur;
                }
            }
        }

        string codeAccess = "";
        if ( DropDownListQuestionnaire.SelectedQuestionnaire == "-1" ) // "-1" valeur mise par le composant
        {
            codeAccess = WebContent.ToutLeMonde;
        }
        else
        {
            codeAccess = DropDownListQuestionnaire.SelectedCodeAcces;
        }
        SessionState.WebContent.Visualisateur = codeAccess;

        int status = WebContent.Update( SessionState.WebContent );
        if ( status == 2 ) // n'existe pas
        {
            // l'utilisateur a changer le visualisateur d'une page existante
            SessionState.ValidationMessage += "La Page n'existait pas.<br/>";
            int statusCreate = WebContent.Create( SessionState.WebContent );
            if ( statusCreate != 1 )
            {
                SessionState.ValidationMessage += "Erreur à la Creation de cette Page.<br/>";
            }
            else
            {
                SessionState.ValidationMessage += "Elle est créée avec succès.<br />";
            }

            // Create a le bon gout de mettre a jour l'id du nouvel objet cree
            Response.Redirect( "~/WebContent/Edit.aspx?id=" + SessionState.WebContent.WebContentID );
        }
        if ( status == 0 )
        {
            SessionState.ValidationMessage += "Erreur serveur à la mise à jour de la Page.<br/>";
            Response.Redirect( Request.RawUrl );
        }

        // Update
        string adrIP = Request.ServerVariables[ "REMOTE_ADDR" ];
        if ( Global.SettingsXml.EnvoyerMiseAjour == true )
        {
            Courriel.EnvoyerMiseAJour( oldContenu, newContenu, "Mise à jour", SessionState.WebContent.Section, adrIP );
        }

        if ( Request[ "ReturnURL" ] != null )
        {
            Response.Redirect( Request[ "ReturnURL" ].ToString() );
        }
        else
        {
            Response.Redirect( "~/WebContent/Manage.aspx" );
        }
    }

    protected void ButtonCreer_Click( object sender, System.EventArgs e )
    {
        if ( DropDownListWebContentSection.SelectedSection == "-1" )
        {
            SessionState.ValidationMessage += "Sélectionnez une Section.<br/>";
        }
        if ( !WebContent.CanEdit() )
        {
            SessionState.ValidationMessage += "Vous n'avez pas les droits pour éditer cette section.<br/>";
        }
        if ( SessionState.ValidationMessage != null )
        {
            Response.Redirect( Request.RawUrl );
        }

        WebContent Contenu = new WebContent();
        Contenu.Section = DropDownListWebContentSection.SelectedSection;
        Contenu.SectionContent = FCKeditor1.Value;

        if ( User.IsInRole( "Administrateur" ) )
        {
            // Admin cree la page pour lui meme
            if ( DropDownListMembre.SelectedMembre == "-1" )
            {
                Contenu.Utilisateur = WebContent.Admin;
            }
            else // Trouver l'utilisateur
            {
                Guid membreGuid = ( Guid )DropDownListMembre.MembreGUID[ DropDownListMembre.SelectedIndex - 1 ];
                MemberInfo membre = MemberInfo.Get( membreGuid );
                Contenu.Utilisateur = membre.NomUtilisateur;
            }
        }
        else
        {
            Contenu.Utilisateur = SessionState.MemberInfo.NomUtilisateur;
        }

        string codeAccess = "";
        if ( DropDownListQuestionnaire.SelectedQuestionnaire == "-1" ) // "-1" valeur mise par le composant
        {
            codeAccess = WebContent.ToutLeMonde;
        }
        else
        {
            codeAccess = DropDownListQuestionnaire.SelectedCodeAcces;
        }
        Contenu.Visualisateur = codeAccess;

        int status = WebContent.Create( Contenu );
        if ( status == 2 )
        {
            SessionState.ValidationMessage += "Erreur cette Page existe déjà.<br/>";
            Response.Redirect( Request.RawUrl );
        }
        if ( status != 1 )
        {
            SessionState.ValidationMessage += "Erreur à la Creation de cette Page.<br/>";
            Response.Redirect( Request.RawUrl );
        }

        Response.Redirect( "~/WebContent/Manage.aspx" );
    }

    protected void ButtonUploadImage_Click( object sender, EventArgs e )
    {
        if ( txtUploadImage.FileName == "" )
        {
            SessionState.ValidationMessage = "Choisir une image";
            Server.Transfer( Request.RawUrl );
        }
        else
        {
            string filePath = Upload( txtUploadImage );

            // Pour que l'image s'affiche dans un email envoye
            if ( SessionState.WebContent.Section == "CorpsEmail" )
            {
                filePath = "http://" + Request.Url.Authority + filePath;
            }

            string imgage = string.Format( "<img src=\"{0}\" alt=\"{1}\" />", filePath, txtUploadImage.FileName );

            FCKeditor1.Value += imgage;
        }
    }

    protected void ButtonUploadFile_Click( object sender, EventArgs e )
    {
        if ( txtUploadFile.FileName == "" )
        {
            SessionState.ValidationMessage = "Choisir un fichier";
            Server.Transfer( Request.RawUrl );
        }
        else
        {
            string filePath = Upload( txtUploadFile );
            string text = txtUploadFile.FileName + " (" + SizeFormat( txtUploadFile.FileBytes.Length, "N" ) + ")";
            string aref = string.Format( "<p><a href=\"{0}\" >{1}</a></p>", filePath, text );

            FCKeditor1.Value += aref;
        }
    }

    private string Upload( FileUpload control )
    {
        if ( SessionState.WebContent == null )
        {
            SessionState.ValidationMessage += "La Page n'est pas encore créée.<br/>";
            SessionState.ValidationMessage += "Vous devez d'abord créer la page avant de télécharger un fichier.<br/>";
            Server.Transfer( Request.RawUrl );
        }

        string dir = BaseFileDirectory + SessionState.WebContent.Section
            + "/" + SessionState.WebContent.Utilisateur
            + "/" + SessionState.WebContent.Visualisateur
            + "/";

        string directory = Server.MapPath( dir );
        if ( !Directory.Exists( directory ) )
        {
            try
            {
                Directory.CreateDirectory( directory );
            }
            catch ( Exception ex )
            {
                string msg = "Problème avec la création du répertoire dans WebContent/Edit.aspx.<br/>";
                msg += "Erreur : " + ex.Message;
                Response.Redirect( Tools.PageErreurPath + msg );
            }
        }

        control.PostedFile.SaveAs( directory + control.FileName );
        return dir + control.FileName;
    }

    protected void ButtonSupprimer_Click( object sender, EventArgs e )
    {
        if ( SessionState.WebContent == null )
        {
            SessionState.ValidationMessage += "Erreur la Section est nulle.<br/>";
        }
        // Verifie sir l'utilisateur n'est pas en train de supprimer le dernier WebContent de la Section
        // BUG00020100214
        WebContentCollection webc = WebContentCollection.GetWebContents( SessionState.MemberInfo.NomUtilisateur, SessionState.WebContent.Section, true );
        if ( webc.Count <= 1 )
        {
            SessionState.ValidationMessage += "Vous ne pouvez pas supprimer la dernière page de la section : " + SessionState.WebContent.Section + "<br/>";
        }
        if ( SessionState.ValidationMessage != null )
        {
            Response.Redirect( Request.RawUrl );
        }

        int status = WebContent.Delete( SessionState.WebContent.WebContentID );
        if ( status != 0 )
        {
            SessionState.ValidationMessage += "Erreur à la Suppression de cette Page.<br/>";
            Response.Redirect( Request.RawUrl );
        }
        SessionState.ValidationMessage += "Suppression de " + SessionState.WebContent.Section + " de " + SessionState.WebContent.Utilisateur + " pour " + SessionState.WebContent.Visualisateur + "<br/>";
        Response.Redirect( Request.RawUrl );
    }

    protected void ButtonRetour_Click( object sender, EventArgs e )
    {
        if ( Request.QueryString[ "ReturnURL" ] != null )
        {
            string returnURL = Request.QueryString[ "ReturnURL" ].ToString();
            Response.Redirect( returnURL );   
        }
        Response.Redirect( "~/WebContent/Manage.aspx" );      
    }

    private string SizeFormat( float size, string formatString )
    {
        if ( size < 1024 )
            return size.ToString( formatString ) + " bytes";

        if ( size < Math.Pow( 1024, 2 ) )
            return ( size / 1024 ).ToString( formatString ) + " kb";

        if ( size < Math.Pow( 1024, 3 ) )
            return ( size / Math.Pow( 1024, 2 ) ).ToString( formatString ) + " mb";

        if ( size < Math.Pow( 1024, 4 ) )
            return ( size / Math.Pow( 1024, 3 ) ).ToString( formatString ) + " gb";

        return size.ToString( formatString );
    }
}
