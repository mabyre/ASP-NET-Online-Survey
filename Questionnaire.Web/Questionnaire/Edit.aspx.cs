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

public partial class Questionnaire_Edit : PageBase
{
    // BUG10092009
    private int QuestionnaireID
    {
        get { return ( int )ViewState[ "QuestionnaireID" ]; }
        set { ViewState[ "QuestionnaireID" ] = value; }
    }

    protected void Page_Load( object sender, System.EventArgs e )
    {
        if ( IsPostBack == false )
        {
            // Edition d'un Questionnaire existant
            if ( Request.QueryString[ "QuestionnaireID" ] != null || Request.QueryString[ "Edition" ] != null )
            {
                Questionnaire questionnaire = new Questionnaire();

                if ( Request.QueryString[ "Edition" ] != null )
                {
                    // Choisir le premier Questionnaire a la place de l'utilisateur
                    if ( SessionState.Questionnaire == null && SessionState.Questionnaires.Count > 0 )
                    {
                        SessionState.Questionnaire = SessionState.Questionnaires[ 0 ];
                    }
                    if ( SessionState.Questionnaire == null )
                    {
                        // Formulaire en mode creation
                        Response.Redirect( "~/Questionnaire/Edit.aspx" );
                    }

                    QuestionnaireID = SessionState.Questionnaire.QuestionnaireID;
                    questionnaire = Questionnaire.GetQuestionnaire( QuestionnaireID );
                }

                if ( Request.QueryString[ "QuestionnaireID" ] != null )
                {
                    QuestionnaireID = int.Parse( Request.QueryString[ "QuestionnaireID" ] );
                    questionnaire = Questionnaire.GetQuestionnaire( QuestionnaireID );
                    SessionState.Questionnaire = questionnaire;
                }

                LabelTitre.Text = "Editer un Questionnaire";
                ButtonSave.Text = "Sauver";
                ButtonSave.ToolTip = "Sauvegarder les modifications";

                MemberInfo membre = MemberInfo.Get( questionnaire.MembreGUID );
                HiddenFieldMembreGUID.Value = membre.MembreGUID.ToString();
                string lien = "";

                // Invitation vers le formulaire Contact/Register.aspx Mode.Complet "1"
                HyperLinkInvitationEnregistrementComplet.NavigateUrl = "~/Contact/Register.aspx?uid=" + membre.MembreGUID + "&qid=" + questionnaire.QuestionnaireID.ToString() + "&mod=1";
                HyperLinkInvitationEnregistrementComplet.Visible = true; // Global.SettingsXml.EnregistrerContactAnonyme;
                lien = Utils.WebSiteUriWhithOutQuery.ToLower() + "/contact/register.aspx?uid=" + membre.MembreGUID + "&qid=" + questionnaire.QuestionnaireID.ToString() + "&mod=1";
                TextBoxInvitationEnregistrementComplet.Text = string.Format( "<a href=\"{0}\" target=\"_blank\">{1}</a>", lien, "Accès au Questionnaire" );

                // Invitation vers le formulaire Contact/Register.aspx Mode.CompletEmail mais pas de Telephone "2" mode par defaut
                HyperLinkInvitationEnregistrementCompletEmail.NavigateUrl = "~/Contact/Register.aspx?uid=" + membre.MembreGUID + "&qid=" + questionnaire.QuestionnaireID.ToString();
                HyperLinkInvitationEnregistrementCompletEmail.Visible = true; // Global.SettingsXml.EnregistrerContactAnonyme;
                lien = Utils.WebSiteUriWhithOutQuery.ToLower() + "/contact/register.aspx?uid=" + membre.MembreGUID + "&qid=" + questionnaire.QuestionnaireID.ToString();
                TextBoxInvitationEnregistrementCompletEmail.Text = string.Format( "<a href=\"{0}\" target=\"_blank\">{1}</a>", lien, "Accès au Questionnaire" );

                // Invitation vers le formulaire Contact/Register.aspx Mode.Email "4"
                HyperLinkInvitationEnregistrementEmail.NavigateUrl = "~/Contact/Register.aspx?uid=" + membre.MembreGUID + "&qid=" + questionnaire.QuestionnaireID.ToString() + "&mod=4";
                HyperLinkInvitationEnregistrementEmail.Visible = true; // Global.SettingsXml.EnregistrerContactAnonyme;
                lien = Utils.WebSiteUriWhithOutQuery.ToLower() + "/contact/register.aspx?uid=" + membre.MembreGUID + "&qid=" + questionnaire.QuestionnaireID.ToString() + "&mod=4";
                TextBoxInvitationEnregistrementEmail.Text = string.Format( "<a href=\"{0}\" target=\"_blank\">{1}</a>", lien, "Accès au Questionnaire" );

                // Invitation vers le formulaire Contact/Register.aspx Mode.Telephone "5"
                HyperLinkInvitationEnregistrementTelephone.NavigateUrl = "~/Contact/Register.aspx?uid=" + membre.MembreGUID + "&qid=" + questionnaire.QuestionnaireID.ToString() + "&mod=5";
                HyperLinkInvitationEnregistrementTelephone.Visible = true; // Global.SettingsXml.EnregistrerContactAnonyme;
                lien = Utils.WebSiteUriWhithOutQuery.ToLower() + "/contact/register.aspx?uid=" + membre.MembreGUID + "&qid=" + questionnaire.QuestionnaireID.ToString() + "&mod=5";
                TextBoxInvitationEnregistrementTelephone.Text = string.Format( "<a href=\"{0}\" target=\"_blank\">{1}</a>", lien, "Accès au Questionnaire" );

                // Inviation vers le formulaire authentification Contact/Login.aspx
                HyperLinkInvitationEmailCodeAcces.NavigateUrl = "~/Contact/Login.aspx"; //?guid=" + membre.MembreGUID;
                HyperLinkInvitationEmailCodeAcces.Visible = true; // Global.SettingsXml.EnregistrerContactAnonyme;
                lien = Utils.WebSiteUriWhithOutQuery.ToLower() + "/contact/login.aspx";
                TextBoxInvitationEmailCodeAcces.Text = string.Format( "<a href=\"{0}\" target=\"_blank\">{1}</a>", lien, "Accès au Questionnaire" );

                // Inviation vers le formulaire authentificationContact/Login.aspx Mode.Telephone "2"
                HyperLinkInvitationTelephoneCodeAcces.NavigateUrl = "~/Contact/Login.aspx?mod=2"; //?guid=" + membre.MembreGUID;
                HyperLinkInvitationTelephoneCodeAcces.Visible = true; // Global.SettingsXml.EnregistrerContactAnonyme;
                lien = Utils.WebSiteUriWhithOutQuery.ToLower() + "/contact/login.aspx?mod=2";
                TextBoxInvitationTelehponeCodeAcces.Text = string.Format( "<a href=\"{0}\" target=\"_blank\">{1}</a>", lien, "Accès au Questionnaire" );
                
                // Lien vers les statistiques du questionnaire en cas de publication
                HyperLinkPublication.NavigateUrl = "~/Questionnaire/Publication.aspx?CodeAcces=" + questionnaire.CodeAcces.ToString();
                lien = Utils.WebSiteUriWhithOutQuery.ToLower() + "/questionnaire/publication.aspx?codeacces=" + questionnaire.CodeAcces.ToString();
                TextBoxPublication.Text = string.Format( "<a href=\"{0}\" target=\"_blank\">{1}</a>", lien, "Accès au Statistiques" );

                TextBoxDescription.Text = questionnaire.Description;
                CheckBoxValider.Checked = questionnaire.Valider;
                CheckBoxFin.Checked = questionnaire.Fin;
                CheckBoxAnonyme.Checked = questionnaire.Anonyme;
                CheckBoxBloque.Checked = questionnaire.Bloque;
                CheckBoxPublierQuestionnaire.Checked = questionnaire.Publier;
                CheckBoxAfficherCompteur.Checked = questionnaire.Compteur;
                LabelDateCreation.Text = questionnaire.DateCreation.ToString();
                LabelCodeAcces.Text = questionnaire.CodeAcces.ToString();
                LabelNom.Text = membre.Nom;
                LabePrenom.Text = membre.Prenom;
                LabelSociete.Text = membre.Societe;
                DropDownListMembre.SelectedMembre = membre.Nom + "/" + membre.Prenom + "/" + membre.Societe;
                LabelMembre.Text = membre.Nom + "/" + membre.Prenom + "/" + membre.Societe;

                HtmlInputCheckBox checkBox = ( HtmlInputCheckBox )PopupLabelAnonymat.FindControl( "CheckBoxToggleLabel" );
                checkBox.Checked = questionnaire.Anonymat;
                checkBox.Disabled = questionnaire.Anonymat;

                TrVoletCodeAcces.Visible = true;
                PanelModeEnregistrement.Visible = true;
                PanelInviationFormulaireEnregistrement.Visible = true;
                PanelInviationFormulaireAuthentification.Visible = true;
                PanelPublication.Visible = true;
                PanelCloture.Visible = true;
                PanelCompteurQuestion.Visible = true;
                ImageButtonExpandQuestion.ImageUrl = "~/Images/collapse.jpg";
            }
            // Creation d'un nouveau Questionnaire
            else
            {
                LabelTitre.Text = "Créer un Questionnaire";
                QuestionnaireID = 0;
                HiddenFieldMembreGUID.Value = "";
                ButtonCopier.Visible = false;
                ButtonSupprimer.Visible = false;
                ButtonSave.Text = "Créer";
                ButtonSave.ToolTip = "Créer le Questionnaire";

                TrVoletCodeAcces.Visible = false;
                PanelPublication.Visible = false;
                PanelCompteurQuestion.Visible = false;
                PanelModeEnregistrement.Visible = false;
                PanelInviationFormulaireEnregistrement.Visible = false;
                PanelInviationFormulaireAuthentification.Visible = false;
                PanelCloture.Visible = false;

                // On ne peut pas ajouter des Questions
                ButtonAjouterQuestion.Visible = false;

                // Creation d'un questionnaire par un Client
                if ( User.IsInRole( "Client" ) )
                {
                    HiddenFieldMembreGUID.Value = SessionState.MemberInfo.MembreGUID.ToString();
                    MemberInfo membre = SessionState.MemberInfo;
                    LabelMembre.Text = membre.Nom + "/" + membre.Prenom + "/" + membre.Societe;

                    // Afficher une heure approximative de creation
                    LabelDateCreation.Text = DateTime.Now.ToString();

                    ArrayList codes = QuestionnaireDAL.GetCodeAccessAll();
                    LabelCodeAcces.Text = Tools.CalculCodeAcces( membre.MembreID, codes ).ToString();

                    LabelNom.Text = membre.Nom;
                    LabePrenom.Text = membre.Prenom;
                    LabelSociete.Text = membre.Societe;
                }
            }

            TrVoletListQuestionnaire.Visible = false;
            ValidationMessage.Text = "";
        }

        if ( User.IsInRole( "Administrateur" ) )
        {
            PanelAdmin.Visible = true;
            // Detourner Bloque pour cloture du questionnaire
            //LabelBloque.Visible = true;
            //CheckBoxBloque.Visible = true;

            // Pour Creer un Questionnaire, l'Admin doit choisir un Membre
            if ( QuestionnaireID == 0 )
            {
                DropDownListMembre.AutoPostBack = true;
                DropDownListMembre.SelectedIndexChanged += new EventHandler( DropDownListMembre_SelectedIndexChanged );
            }
        }

        TableLienEnregistrement.Visible = CheckBoxAnonyme.Checked;
        TableLienPublication.Visible = CheckBoxPublierQuestionnaire.Checked;

        SqlDataSourceMembreQuestionnaire.SelectCommand = SelectCommand();
    }

    private string SelectCommand()
    {
        if ( HiddenFieldMembreGUID.Value == "" )
            return "";

        string sql = string.Format( "SELECT Description, CodeAcces FROM Questionnaire WHERE MembreGUID = '{0}'", HiddenFieldMembreGUID.Value );
        return sql;
    }

    private void BloquerQuestionnaire( bool bloque )
    {
        if ( bloque )
        {
            PanelQuestionnaireBloque.Visible = true;
            PanelQuestionnaire.Visible = false;
            ButtonSave.Visible = false;
            ButtonCopier.Visible = false;
            ButtonSupprimer.Visible = false;
        }
    }

    protected void ButtonSave_Click( object sender, EventArgs e )
    {
        ValidationMessage.Text = "";
        ValidationMessage.CssClass = "LabelValidationMessageStyle";

        if ( TextBoxDescription.Text.Trim().Length == 0 )
        {
            ValidationMessage.Text += "Entrer le Nom du Questionnaire<br/>";
            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
        }

        if (    LabelNom.Text.Trim().Length == 0
             || LabePrenom.Text.Trim().Length == 0
             || LabelSociete.Text.Trim().Length == 0
            )
        {
            ValidationMessage.Text += "Choisir un membre<br/>";
            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
        }

        if ( ValidationMessage.Text != "" )
        {
            ValidationMessage.Visible = true;
            return;
        }

        // C'est une mise a jour
        if ( QuestionnaireID != 0 )
        {
            Questionnaire questionnaire = Questionnaire.GetQuestionnaire( QuestionnaireID );
            questionnaire.Description = TextBoxDescription.Text;
            questionnaire.Valider = CheckBoxValider.Checked;
            questionnaire.Fin = CheckBoxFin.Checked;
            questionnaire.Anonyme = CheckBoxAnonyme.Checked;
            HtmlInputCheckBox checkBox = (HtmlInputCheckBox)PopupLabelAnonymat.FindControl( "CheckBoxToggleLabel" );
            if ( checkBox.Checked )
            {
                questionnaire.Anonymat = checkBox.Checked;
                checkBox.Disabled = true;
            }
            questionnaire.Bloque = CheckBoxBloque.Checked;
            questionnaire.Publier = CheckBoxPublierQuestionnaire.Checked;
            questionnaire.Compteur = CheckBoxAfficherCompteur.Checked;

            MemberInfo membre = MemberInfo.GetMemberInfo( LabelNom.Text, LabePrenom.Text );
            questionnaire.MembreGUID = membre.MembreGUID;

            int status = Questionnaire.Update( questionnaire );
            if ( status == 1 )
            {
                ValidationMessage.Text += "Questionnaire mis à jour correctement.<br/>";
            }
            else if ( status == 2 )
            {
                ValidationMessage.Text += "Erreur sur la mise à jour du Questionnaire, il n'existe pas.<br/>";
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            }
            else
            {
                ValidationMessage.Text += "Erreur sur la mise à jour du Questionnaire.<br/>";
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            }

            // Forcer les Questionnaires a se recharger depuis la Base de Donnees
            SessionState.Questionnaires = null;
            // BUG10112009
            //SessionState.Questionnaire = null;
            SessionState.Questionnaire = questionnaire;
        }
        else // C'est une creation
        {
            Questionnaire questionnaire = new Questionnaire();
            questionnaire.Description = TextBoxDescription.Text;
            questionnaire.Style = "Défaut.xml";
            questionnaire.Valider = CheckBoxValider.Checked;
            questionnaire.Fin = CheckBoxFin.Checked;
            questionnaire.Anonyme = CheckBoxAnonyme.Checked;
            HtmlInputCheckBox checkBox = ( HtmlInputCheckBox )PopupLabelAnonymat.FindControl( "CheckBoxToggleLabel" );
            if ( checkBox.Checked )
            {
                questionnaire.Anonymat = checkBox.Checked;
                checkBox.Disabled = true;
            }
            questionnaire.Bloque = CheckBoxBloque.Checked;
            questionnaire.Publier = CheckBoxPublierQuestionnaire.Checked;
            questionnaire.DateCreation = DateTime.Now;
            questionnaire.MembreGUID = new Guid( HiddenFieldMembreGUID.Value );
            questionnaire.CodeAcces = int.Parse( LabelCodeAcces.Text );
            questionnaire.Compteur = CheckBoxAfficherCompteur.Checked;

            // Tester les limitations avant d'ajouter le questionnaire
            if ( SessionState.Limitations.LimiteQuestionnaires )
            {
                Tools.PageValidation( "La limite du nombre de Questionnaires : " + SessionState.Limitations.NombreQuestionnaires + " est atteinte.<br/>Contactez l'administrateur." );
            }

            int status = Questionnaire.Create( questionnaire );
            if ( status == 1 )
            {
                ValidationMessage.Text += "Questionnaire créé correctement.<br/>";

                // Maintenant on peut ajouter des Questions
                ButtonAjouterQuestion.Visible = true;
                ButtonSave.Visible = false;

                // Prendre en compte le nouveau Questionnaire
                SessionState.Questionnaires.Add( questionnaire );
                SessionState.Questionnaire = questionnaire;
                SessionState.Limitations.AjouterQuestionnaire();
            }
            else if ( status == 2 )
            {
                ValidationMessage.Text += "Le Questionnaire existe déjà.<br>";
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            }
            else
            {
                ValidationMessage.Text += "Erreur sur la création du Questionnaire<br/>";
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            }
        }

        ValidationMessage.Visible = true;
    }

    protected void ButtonAjouterQuestion_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Wizard/Question.aspx" );
    }

    protected void ButtonCopier_Click( object sender, EventArgs e )
    {
        MemberInfo membre = new MemberInfo();

        // Pour l'Admin uniquement, copier un Questionnaire pour un autre Membre
        if ( DropDownListMembre.SelectedMembre != "-1" && DropDownListMembre.SelectedMembre != LabelMembre.Text )
        {
            string selected = DropDownListMembre.SelectedMembre;
            string[] sel = selected.Split( '/' );
            string nom = sel[ 0 ];
            string prenom = sel[ 1 ];

            membre = MemberInfo.GetMemberInfo( nom, prenom );
        }
        else
        {
            membre = MemberInfo.GetMemberInfo( LabelNom.Text, LabePrenom.Text );
        }

        ArrayList codes = QuestionnaireDAL.GetCodeAccessAll();
        string codeAcces = Tools.CalculCodeAcces( membre.MembreID, codes ).ToString();

        Response.Redirect( "~/Questionnaire/Copier.aspx?QuestionnaireID=" + QuestionnaireID.ToString() + "&MembreGUID=" + membre.MembreGUID.ToString() + "&CodeAcces=" + codeAcces );
    }
    
    protected void ButtonSupprimer_Click( object sender, EventArgs e )
    {
        if ( QuestionnaireID == 0 )
        {
            ValidationMessage.Text += "Choisir un questionnaire à supprimer.<br/>";
            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            ValidationMessage.Visible = true;
        }
        else
        {
            Response.Redirect( "~/Questionnaire/Delete.aspx?QuestionnaireID=" + QuestionnaireID.ToString() );
        }
    }


    protected void DropDownListMembre_SelectedIndexChanged( object sender, EventArgs e )
    {
        if ( DropDownListMembre.SelectedIndex != 0 )
        {
            string selected = DropDownListMembre.SelectedMembre;
            string[] sel = selected.Split( '/' );
            string nom = sel[ 0 ];
            string prenom = sel[ 1 ];
            MemberInfo membre = MemberInfo.GetMemberInfo( nom, prenom );

            LabelMembre.Text = membre.Nom + "/" + membre.Prenom + "/" + membre.Societe;
            LabelNom.Text = membre.Nom;
            LabePrenom.Text = membre.Prenom;
            LabelSociete.Text = membre.Societe;

            ArrayList codes = QuestionnaireDAL.GetCodeAccessAll();
            string codeAcces = Tools.CalculCodeAcces( membre.MembreID, codes ).ToString();
            HiddenFieldMembreGUID.Value = membre.MembreGUID.ToString();

            LabelCodeAcces.Text = codeAcces;

            LabelDateCreation.Text = DateTime.Now.ToString();

            SqlDataSourceMembreQuestionnaire.SelectCommand = SelectCommand();
            SqlDataSourceMembreQuestionnaire.DataBind();
        }
    }

    protected void ImageButtonExpandListQuestionnaire_Click( object sender, ImageClickEventArgs e )
    {
        TrVoletListQuestionnaire.Visible = TrVoletListQuestionnaire.Visible == false;
        if ( TrVoletListQuestionnaire.Visible )
        {
            ImageButtonExpandListQuestionnaire.ImageUrl = "~/Images/collapse.jpg";
        }
        else
        {
            ImageButtonExpandListQuestionnaire.ImageUrl = "~/Images/expand.jpg";
        }
    }
    
    protected void ImageButtonExpandQuestion_Click( object sender, ImageClickEventArgs e )
    {
        TrVoletCodeAcces.Visible = TrVoletCodeAcces.Visible == false;
        PanelModeEnregistrement.Visible = PanelModeEnregistrement.Visible == false;
        PanelInviationFormulaireEnregistrement.Visible = PanelInviationFormulaireEnregistrement.Visible == false;
        PanelInviationFormulaireAuthentification.Visible = PanelInviationFormulaireAuthentification.Visible == false;
        PanelPublication.Visible = PanelPublication.Visible == false;
        PanelCloture.Visible = PanelCloture.Visible == false;
        PanelCompteurQuestion.Visible = PanelCompteurQuestion.Visible == false;
        if ( TrVoletCodeAcces.Visible )
        {
            ImageButtonExpandQuestion.ImageUrl = "~/Images/collapse.jpg";
        }
        else
        {
            ImageButtonExpandQuestion.ImageUrl = "~/Images/expand.jpg";
        }
    }

    protected void ImageButtonLinkAuthentification_Click( object sender, ImageClickEventArgs e )
    {
        TableTableLienAuthentification.Visible = TableTableLienAuthentification.Visible == false;
        if ( TableTableLienAuthentification.Visible )
        {
            ImageButtonLinkAuthentification.ImageUrl = "~/Images/collapse.jpg";
        }
        else
        {
            ImageButtonLinkAuthentification.ImageUrl = "~/Images/expand.jpg";
        }
    }

    protected void CheckBoxAnonyme_CheckedChanged( object sender, EventArgs e )
    {
        TableLienEnregistrement.Visible = CheckBoxAnonyme.Checked;
    }

    protected void CheckBoxPublierQuestionnaire_CheckedChanged( object sender, EventArgs e )
    {
        TableLienPublication.Visible = CheckBoxPublierQuestionnaire.Checked;
    }
}
