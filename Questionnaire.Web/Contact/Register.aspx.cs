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

public partial class Contact_Register : PageBase
{
    // BUG10092009
    private int QuestionnaireID
    {
        get { return ( int )ViewState[ "QuestionnaireID" ]; }
        set { ViewState[ "QuestionnaireID" ] = value; }
    }

    // Differents Modes du Formulaire
    private enum Mode
    {
        Complet = 1,        // Email et Telephone
        CompletEmail,       // Email et pas Telephone
        CompletTelephone,   // Telephone et pas email
        Email,              // Seulement Email
        Telephone           // Seulement Telehpone
    }

    private Mode ModeFormulaire
    {
        get { return ( Mode )ViewState[ "ModeFormulaire" ]; }
        set { ViewState[ "ModeFormulaire" ] = value; }
    }

    private void FormulaireEnMode( Mode mode )
    {
        switch ( mode )
        {
            case Mode.Complet:
                break;
            case Mode.CompletEmail:
                TrNumeroTelephone.Visible = false;
                break;
            case Mode.CompletTelephone:
                TrEmail.Visible = false;
                break;
            case Mode.Email:
                TrCivilite.Visible = false;
                TrNom.Visible = false;
                TrPrenom.Visible = false;
                TrSociete.Visible = false;
                //TrEmail.Visible = false;
                TrNumeroTelephone.Visible = false;
                break;
            case Mode.Telephone:
                TrCivilite.Visible = false;
                TrNom.Visible = false;
                TrPrenom.Visible = false;
                TrSociete.Visible = false;
                TrEmail.Visible = false;
                //TrNumeroTelephone.Visible = false;
                break;
        }
    }

    protected void Page_Load( object sender, System.EventArgs e )
    {
        ValidationMessage.Text = "";

        if ( IsPostBack == false )
        {
            if ( Global.SettingsXml.EnregistrerContactAnonyme )
            {
                // Mode par defaut du Formulaire
                ModeFormulaire = Mode.CompletEmail; // Compatibilite ascendante ...

                if ( Request.QueryString[ "mod" ] != null )
                {
                    int mode = (int)Mode.CompletEmail;
                    try
                    {
                        mode = int.Parse( Request.QueryString[ "mod" ] );
                    }
                    catch
                    {
                        ValidationMessage.Text = "Mode du formulaire incorrecte";
                        ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                        ValidationMessage.Visible = true;
                        return;
                    }
                    switch ( mode )
                    {
                        case 1:
                            ModeFormulaire = Mode.Complet;
                            break;
                        case 2:
                            ModeFormulaire = Mode.CompletEmail;
                            break;
                        case 3:
                            ModeFormulaire = Mode.CompletTelephone;
                            break;
                        case 4:
                            ModeFormulaire = Mode.Email;
                            break;
                        case 5:
                            ModeFormulaire = Mode.Telephone;
                            break;
                    }
                }

                // Mise en mode du formulaire
                FormulaireEnMode( ModeFormulaire ); 

                if ( Request.QueryString[ "uid" ] != null && Request.QueryString[ "qid" ] != null )
                {
                    LabelTitre.Text = "Enregistrement";
                    Guid membreGUID = Guid.Empty;

                    // Attention tout le monde peut acceder ici 
                    // donc mettre n'importe quoi dans uid et qid
                    try
                    {
                        membreGUID = new Guid( Request.QueryString[ "uid" ].ToString() );
                    }
                    catch
                    {
                        ValidationMessage.Text = "Pas de Questionnaire";
                        ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                        ValidationMessage.Visible = true;
                        return;
                    }

                    MemberInfo membre = MemberInfo.GetMemberInfo( membreGUID );
                    if ( membre == null )
                    {
                        ValidationMessage.Text = "Pas de Questionnaire";
                        ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                        ValidationMessage.Visible = true;
                    }

                    try
                    {
                        QuestionnaireID = int.Parse( Request.QueryString[ "qid" ] );
                    }
                    catch
                    {
                        ValidationMessage.Text = "Pas de Questionnaire";
                        ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                        ValidationMessage.Visible = true;
                        return;
                    }

                    Questionnaire questionnaire = Questionnaire.GetQuestionnaire( QuestionnaireID );
                    if ( questionnaire == null )
                    {
                        ValidationMessage.Text = "Pas de Questionnaire";
                        ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                        ValidationMessage.Visible = true;
                    }
                    else
                    {
                        if ( questionnaire.Bloque )
                        {
                            Tools.PageValidation( "Le questionnaire \"" + questionnaire.Description + "\" est clôturé." );
                        }
                    }

                    if ( questionnaire != null && membre != null )
                    {
                        // Ameliorer le referencement
                        Page.Title = "Répondre au questionnaire : " + questionnaire.Description;

                        if ( questionnaire.MembreGUID != membre.MembreGUID )
                        {
                            ValidationMessage.Text = "Pas de Questionnaire";
                            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                            ValidationMessage.Visible = true;
                        }
                        else
                        {
                            if ( questionnaire.Anonyme == false )
                            {
                                LabelTitre.Text = "Pas d'enregistrement possible";
                                ValidationMessage.Text = "Demandez à l'intervieweur";
                                ValidationMessage.Visible = true;
                            }
                            else // questionnaire.Anonyme == true
                            {
                                LabelQuestionnaire.Text = questionnaire.Description;
                                PanelEnregistrement.Visible = true;
                                PanelBoutonEnregistrement.Visible = true;
                                ButtonEnregistrer.Visible = true;

                                if ( User.Identity.IsAuthenticated == false )
                                {
                                    // Anonyme et Anonymat
                                    if ( questionnaire.Anonymat == true )
                                    {
                                        // L'interviewe a devant lui la page d'enregistrement vide 
                                        // avec seulement le bouton repondre
                                        PanelEnregistrement.Visible = false;
                                        ButtonRepondre.Visible = true;
                                        ButtonEnregistrer.Visible = false;
                                        LabelTitre.Visible = false;
                                    }
                                }

                                // Pour que l'utilisateur non authentifie voit les bonnes page,
                                // il faut que SessionState.Personne ait la bonne valeur pour 
                                // la fonction WebContent.GetUtilisateur() 
                                if ( SessionState.Personne == null )
                                {
                                    SessionState.Personne = new Personne();
                                    SessionState.Personne.CodeAcces = questionnaire.CodeAcces;
                                    SessionState.Personne.QuestionnaireID = questionnaire.QuestionnaireID;
                                }
                                else
                                {
                                    SessionState.Personne.CodeAcces = questionnaire.CodeAcces;
                                    SessionState.Personne.QuestionnaireID = questionnaire.QuestionnaireID;
                                }
                            }
                        }
                    }
                }
                else
                {
                    LabelTitre.Text = "Pas de questionnaire";
                    ValidationMessage.Text = "Demandez à l'administrateur";
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                    ValidationMessage.Visible = true;
                }
            }
            else
            {
                LabelTitre.Text = "Pas d'enregistrement possible";
                ValidationMessage.Text = "Demandez à l'administrateur";
                ValidationMessage.Visible = true;
            }
        }

        InititializeCaptcha();

        // Pour un meilleur referencement, attention ici SessionState.Questionnaire n'est pas defini !
        base.AddMetaContentType();
        base.AddMetaTag( "description", "Inviation à répondre à un questionnaire" );
        base.AddMetaTag( "keywords", "questionnaire, statistique, enquêtes, enquete, en ligne" );
        base.AddMetaTag( "revisit-after", "10 days" );
        base.AddMetaTag( "author", "Sodevlog" );
        base.AddMetaTag( "copyright", "Sodevlog" );
    }

    protected void ButtonEnregistrer_Click( object sender, EventArgs e )
    {
        // Est-ce un Robo ?
        if ( IsCaptchaValid == false || Page.IsValid == false )
            Response.Redirect( Tools.PageErreurPath + "IP:" + Request.UserHostAddress.ToString() + "<br/>Coming from:" + Request.UrlReferrer.ToString() + "<br/>Is Robot");

        ValidationMessage.Text = "";
        ValidationMessage.CssClass = "LabelValidationMessageStyle";

        // Validation du Formulaire suivant son mode
        bool nomPrenomSociete = 
            ModeFormulaire == Mode.Complet || 
            ModeFormulaire == Mode.CompletEmail || 
            ModeFormulaire == Mode.CompletTelephone;
        if ( nomPrenomSociete )
        {
            if ( TextBoxNom.Text.Trim().Length == 0 )
            {
                ValidationMessage.Text += "Entrer un Nom<br/>";
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            }
            if ( TextBoxPrenom.Text.Trim().Length == 0 )
            {
                ValidationMessage.Text += "Entrer un Prénom<br/>";
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            }
            if ( Global.SettingsXml.EnregistrerContactAvecSociete && TextBoxSociete.Text.Trim().Length == 0 )
            {
                ValidationMessage.Text += "Entrer une Société<br/>";
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            }
        }

        if ( ( nomPrenomSociete || ModeFormulaire == Mode.Email ) 
            && ( ModeFormulaire == Mode.Telephone ) == false
            && ( ModeFormulaire == Mode.CompletTelephone ) == false )
        {
            if ( TextBoxEmail.Text.Trim().Length == 0 )
            {
                ValidationMessage.Text += "Entrer un E-mail<br/>";
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            }
            else
            {
                if ( Strings.IsValideEmail( TextBoxEmail.Text ) == false )
                {
                    ValidationMessage.Text += "Ce n'est pas un email valide<br/>";
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                }
            }
        }

        if ( ( nomPrenomSociete || ModeFormulaire == Mode.Telephone ) 
            && ( ModeFormulaire == Mode.Email ) == false
            && ( ModeFormulaire == Mode.CompletEmail ) == false )
        {
            if ( TextBoxTelephone.Text.Trim().Length == 0 )
            {
                ValidationMessage.Text += "Entrer un Numéro de téléphone<br/>";
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            }
            else
            {
                if ( Strings.IsValideTelephone( TextBoxTelephone.Text ) == false )
                {
                    ValidationMessage.Text += "Ce numéro de téléphone n'est pas valide<br/>";
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                }
            }
        }

        if ( ValidationMessage.Text != "" )
        {
            ValidationMessage.Visible = true;
            return;
        }

        Questionnaire questionnaire = Questionnaire.GetQuestionnaire( QuestionnaireID );
        
        Personne personne = new Personne();
        personne.Civilite = TextBoxCivilite.Text;
        personne.Nom = TextBoxNom.Text;
        personne.Prenom = TextBoxPrenom.Text;
        personne.Societe = TextBoxSociete.Text;
        personne.EmailBureau = TextBoxEmail.Text;
        personne.TelephonePerso = TextBoxTelephone.Text;
        personne.CodeAcces = questionnaire.CodeAcces;
        personne.EmailEnvois = 0;
        personne.QuestionnaireID = questionnaire.QuestionnaireID;
        personne.PersonneGUID = Guid.NewGuid();

        // Trouver si l'utilisateur de ce questionnaire est limite
        Limitation limitation = new Limitation( questionnaire.MembreGUID );
        if ( limitation.LimitesInterviewes )
        {
            Response.Redirect( Tools.PageErreurPath + "Désolé, la limite du nombre d'Interviewés pour ce questionnaire est atteinte.", true );
        }

        string message = string.Empty;
        int retCode = Personne.Create( personne, true, ref message );
        if ( retCode == 1 )
        {
            ValidationMessage.Text += "Vous êtes enregistré pour répondre au Questionnaire : " + questionnaire.Description + "<br/>";
            SessionState.Personne = personne;
            ButtonRepondre.Visible = true;
            ButtonEnregistrer.Visible = false;

            TextBoxCivilite.Enabled = false;
            TextBoxNom.Enabled = false;
            TextBoxPrenom.Enabled = false;
            TextBoxSociete.Enabled = false;
            TextBoxEmail.Enabled = false;
            TextBoxTelephone.Enabled = false;
        }
        else if ( retCode == 2 )
        {
            ValidationMessage.Text = message;
            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
        }

        ValidationMessage.Visible = true;
    }

    protected void ButtonRepondre_Click( object sender, EventArgs e )
    {
        // Est-ce un Robo ?
        if ( IsCaptchaValid == false || Page.IsValid == false )
            Response.Redirect( Tools.PageErreurPath + "IP:" + Request.UserHostAddress.ToString() + "<br/>Coming from:" + Request.UrlReferrer.ToString() + "<br/>Is Robot" );

        Questionnaire questionnaire = Questionnaire.GetQuestionnaire( QuestionnaireID );
        // Anonyme et Anonyma on enregistre un anonyme
        if ( questionnaire.Anonyme && questionnaire.Anonymat )
        {
            PersonneCollection personnes = PersonneCollection.GetQuestionnaire( questionnaire.QuestionnaireID );

            Personne anonyme = new Personne();
            // PREMIERE IDEE MAUVAISE anonyme.ID_Personne = 0; // distinguer un utilisateur anonymat complet d'un contact enregistre
            anonyme.PersonneGUID = Guid.NewGuid();
            anonyme.QuestionnaireID = questionnaire.QuestionnaireID;
            // BUG20112009 
            //anonyme.Nom = "Anonyme" + ( personnes.Count + 1 ).ToString();
            //anonyme.EmailBureau = "Anonyme" + ( personnes.Count + 1 ).ToString();
            //anonyme.Prenom = "Anonyme" + ( personnes.Count + 1 ).ToString();
            anonyme.Nom = "";
            anonyme.EmailBureau = anonyme.PersonneGUID.ToString() + "@a.fr"; //BUG23072010
            anonyme.Prenom = "";
            anonyme.CodeAcces = questionnaire.CodeAcces;
            anonyme.EmailEnvois = 0;

            // Trouver si l'utilisateur de ce questionnaire est limite
            Limitation limitation = new Limitation( questionnaire.MembreGUID );
            if ( limitation.LimitesInterviewes )
            {
                Response.Redirect( Tools.PageErreurPath + "Désolé, la limite du nombre d'Interviewés pour ce questionnaire est atteinte.", true );
            }

            // Enregistrement de l'interviewe avec anonymat complet
            string message = string.Empty;
            int retCode = Personne.Create( anonyme, true, ref message );
            if ( retCode == 1 )
            {
                SessionState.Personne = anonyme;
            }
            else if ( retCode == 2 )
            {
                ValidationMessage.Text += message;
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            }
        }

        if ( SessionState.Personne == null )
        {
            ValidationMessage.Text += "Vous n'êtes pas enregistrez";
            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            ValidationMessage.Visible = true;
        }
        else
        {
            // Regarder si il y a au moins une Question pour ce Questionnaire        
            SessionState.Questions = PollQuestionCollection.GetByQuestionnaire( SessionState.Personne.QuestionnaireID );
            if ( SessionState.Questions.Count > 0 )
            {
                Response.Redirect( "~/Poll/Questionnaire.aspx?QuestionnaireID=" + SessionState.Personne.QuestionnaireID.ToString(), true );
            }
            else
            {
                Response.Redirect( Tools.PageErreurPath + "Désolé mais il n'y a pas de Questions pour ce Questionnaire." );
            }
        }
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
                return Request.Form[ "captcha" ] == ViewState[ "captchavalue" ].ToString();
            }

            return false;
        }
    }

    #endregion
}
