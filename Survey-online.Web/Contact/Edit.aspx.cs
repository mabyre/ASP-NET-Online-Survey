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

public partial class Contact_Edit : PageBase
{
    private int PersonneID
    {
        get { return ( int )ViewState[ "PersonneID" ]; }
        set { ViewState[ "PersonneID" ] = value; }
    }

    protected void Page_Load( object sender, System.EventArgs e )
    {
        ValidationMessage.Text = "";

        if ( IsPostBack == false )
        {
            // Choisir le premier Questionnaire a la place de l'utilisateur
            if ( SessionState.Questionnaire == null && SessionState.Questionnaires.Count > 0 )
            {
                SessionState.Questionnaire = SessionState.Questionnaires[ 0 ];
            }

            if ( Request.QueryString[ "PersonneID" ] != null )
            {
                LabelTitre.Text = "Edition d'un Interviewé";
                PersonneID = int.Parse( Request.QueryString[ "PersonneID" ] );
                Personne personne = Personne.Get( PersonneID );
                TextBoxCivilite.Text = personne.Civilite;
                TextBoxNom.Text = personne.Nom;
                TextBoxPrenom.Text = personne.Prenom;
                TextBoxSociete.Text = personne.Societe;
                TextBoxEmail.Text = personne.EmailBureau;
                TextBoxTelephone.Text = personne.TelephonePerso;
            }
            else
            {
                LabelTitre.Text = "Création d'un Interviewé";
                PersonneID = 0;
                if ( SessionState.Questionnaire == null )
                {
                    ButtonSave.Visible = false;
                    ValidationMessage.Text = "Choisir un Questionnaire";
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                    ValidationMessage.Visible = true;
                }
                else
                {
                    BloquerQuestionnaire( SessionState.Questionnaire.Bloque );
                }
            }
            LabelQuestionnaire.Text = SessionState.Questionnaire.Description + " / Code d'accès : " + SessionState.Questionnaire.CodeAcces.ToString();
        }
    }

    private void BloquerQuestionnaire( bool bloque )
    {
        if ( bloque )
        {
            Tools.PageValidation( "Le questionnaire \"" + SessionState.Questionnaire.Description + "\" est clôturé." );
        }
    }

    protected void ButtonSave_Click( object sender, EventArgs e )
    {
        ValidationMessage.Text = "";
        ValidationMessage.CssClass = "LabelValidationMessageStyle";

        // BUG0809090002
        //if ( TextBoxNom.Text.Trim().Length == 0 )
        //{
        //    ValidationMessage.Text += "Entrer un Nom<br/>";
        //    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
        //}
        // Le prenom n'est pas obligatoire, peut etre vide
        //if ( TextBoxPrenom.Text.Trim().Length == 0 )
        //{
        //    ValidationMessage.Text += "Entrer un Prénom<br/>";
        //    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
        //}
        if ( Global.SettingsXml.EnregistrerContactAvecSociete && TextBoxSociete.Text.Trim().Length == 0 )
        {
            ValidationMessage.Text += "Entrer une Société<br/>";
            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
        }
        // AME16062010
        TextBoxEmail.Text = TextBoxEmail.Text.Trim();
        TextBoxTelephone.Text = TextBoxTelephone.Text.Trim();
        if ( TextBoxEmail.Text.Length == 0 && TextBoxTelephone.Text.Length == 0 )
        {
            ValidationMessage.Text += "Entrer un E-mail ou un Numéro de Téléphone<br/>";
            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
        }
        else
        {
            if ( TextBoxTelephone.Text.Length != 0 )
            {
                if ( Strings.IsValideTelephone( TextBoxTelephone.Text ) == false )
                {
                    ValidationMessage.Text += "Ce numéro de téléphone n'est pas valide<br/>";
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                }
            }
            if ( TextBoxEmail.Text.Length != 0 )
            {
                if ( Strings.IsValideEmail( TextBoxEmail.Text ) == false )
                {
                    ValidationMessage.Text += "Ce n'est pas un email valide<br/>";
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                }
            }
        }
        // AME26062010
        //if ( TextBoxCodeAcces.Text.Trim().Length == 0 )
        //{
        //    ValidationMessage.Text += "Entrer un Code d'accès<br/>";
        //    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
        //}

        if ( ValidationMessage.Text != "" )
        {
            ValidationMessage.Visible = true;
            return;
        }
        
        // C'est une mise a jour
        if ( PersonneID != 0 )
        {
            Personne personne = Personne.Get( PersonneID );
            personne.EmailEnvois = 0;
            personne.Civilite = TextBoxCivilite.Text;
            personne.Prenom = TextBoxPrenom.Text;
            personne.Nom = TextBoxNom.Text;
            personne.Societe = TextBoxSociete.Text;
            personne.EmailBureau = TextBoxEmail.Text;
            personne.TelephonePerso = TextBoxTelephone.Text;
            string message = string.Empty;
            int retCode = Personne.Update( personne, ref message );
            ValidationMessage.Text += message;
            if ( retCode != 0 )
            {
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            }
        }
        else // C'est une creation
        {
            //BRY00020100209
            if ( SessionState.Limitations.LimitesInterviewesAtteinte( 1 ) )
            {
                Tools.PageValidation( "La limite du nombre d'Interviewés : " + SessionState.Limitations.NombreInterviewes + " est atteinte.<br/>Contactez l'administrateur." );
            }

            Personne personne = new Personne();
            personne.Civilite = TextBoxCivilite.Text;
            personne.Nom = TextBoxNom.Text;
            personne.Prenom = TextBoxPrenom.Text;
            personne.Societe = TextBoxSociete.Text;
            personne.EmailBureau = TextBoxEmail.Text;
            personne.TelephonePerso = TextBoxTelephone.Text;
            personne.CodeAcces = SessionState.Questionnaire.CodeAcces;
            personne.QuestionnaireID = SessionState.Questionnaire.QuestionnaireID;
            personne.PersonneGUID = Guid.NewGuid();

            string message = string.Empty;
            int retCode = Personne.Create( personne, true, ref message );
            if ( retCode == 1 )
            {
                ValidationMessage.Text += message;
                SessionState.Limitations.AjouterInterviewes( 1 );
            }
            else
            {
                ValidationMessage.Text += message;
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            }
        }

        ValidationMessage.Visible = true;
    }

    protected void ButtonCancel_Click( object sender, EventArgs e )
    {
        if ( Request.QueryString[ "Email" ] != null )
        {
            Response.Redirect( "~/Contact/Email.aspx" );
        }
        else
        {
            Response.Redirect( "~/Contact/Manage.aspx" );
        }
    }
}
