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

public partial class Contact_Login : PageBase
{
    // Differents Modes du Formulaire
    private enum Mode
    {
        AdresseEmail = 1,
        Telephone
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
            case Mode.AdresseEmail:
                TrNumeroTelephone.Visible = false;
                break;
            case Mode.Telephone:
                TrAdresseCourielle.Visible = false;
                break;
        }
    }

    protected void Page_Load( object sender, EventArgs e )
    {
        // Un truc bisard : un bas du Page_Load ca marche pas ?
        InititializeCaptcha();

        if ( IsPostBack == false )
        {
            // Mode par defaut du Formulaire
            ModeFormulaire = Mode.AdresseEmail;

            if ( Request.QueryString[ "mod" ] != null )
            {
                int mode = ( int )Mode.AdresseEmail;
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
                        ModeFormulaire = Mode.AdresseEmail;
                        break;
                    case 2:
                        ModeFormulaire = Mode.Telephone;
                        break;
                }
            }

            // Mise en mode du formulaire
            FormulaireEnMode( ModeFormulaire ); 

            bool authentifie = false;

            // Authentification automatique de l'interviewe
            if ( Request[ "guid" ] != null )
            {
                string guid = Request[ "guid" ].ToString();
                PersonneCollection personnes = PersonneCollection.GetAll();
                foreach ( Personne p in personnes )
                {
                    if ( p.PersonneGUID == new Guid( guid )  )
                    {
                        authentifie = true;
                        SessionState.Personne = p;
                        break;
                    }
                }

                // BUG02122009
                if ( authentifie == false && User.Identity.IsAuthenticated == false )
                {
                    SessionState.ValidationMessage = "Désolé mais votre ticket pour répondre à ce questionnaire n'est pas valable.";
                    ValidationMessage.Text = SessionState.ValidationMessage;
                    ValidationMessage.Visible = true;
                    WebContentLoginContact.Visible = false;
                    PanelLoginContact.Visible = false;
                    return;
                }
            }

            if ( authentifie == true )
            {
                // Entree dans l'application
                //FormsAuthentication.SignOut(); // Deconnecter un eventuel utilisateur 
                // NON on peut pas sinon la case "Memoriser ma connexion" ne sert plus a rien
                Response.Redirect( "~/Accueil.aspx" );
            }
        }

        ValiderMessage();
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

    protected void LoginButton_Click( object sender, System.EventArgs e )
    {
        // Est-ce un Robo ?
        if ( Page.IsValid == false || IsCaptchaValid == false )
            Response.Redirect( Tools.PageErreurPath + "IP:" + Request.UserHostAddress.ToString() + "<br/>Coming from:" + Request.UrlReferrer.ToString() + "<br/>Is Robot" );

        // Annulation des variables de Session precedentes
        Session.Clear();
        bool courrielleOK = false;
        bool telephoneOK = false;
        bool codeOK = false;

        if ( Strings.IsValideEmail( TextBoxAdresseCourrielle.Text ) && ModeFormulaire == Mode.AdresseEmail )
        {
            PersonneCollection personnes = PersonneCollection.GetAll();
            foreach ( Personne p in personnes )
            {
                if ( p.EmailBureau == TextBoxAdresseCourrielle.Text.Trim() )
                {
                    courrielleOK = true;
                }
                if ( courrielleOK && p.CodeAcces.ToString() == TextBoxCodeAcces.Text.Trim() )
                {
                    codeOK = true;
                    SessionState.Personne = p;
                    break;
                }
            }
        }

        if ( Strings.IsValideTelephone( TextBoxTelephone.Text ) && ModeFormulaire == Mode.Telephone )
        {
            PersonneCollection personnes = PersonneCollection.GetAll();
            foreach ( Personne p in personnes )
            {
                if ( p.TelephonePerso == TextBoxTelephone.Text.Trim() )
                {
                    telephoneOK = true;
                }
                if ( telephoneOK && p.CodeAcces.ToString() == TextBoxCodeAcces.Text.Trim() )
                {
                    codeOK = true;
                    SessionState.Personne = p;
                    break;
                }
            }
        }

        if ( ( courrielleOK || telephoneOK ) && codeOK )
        {
            FormsAuthentication.SignOut(); // Deconnecter un eventuel utilisateur
            Response.Redirect( "~/Accueil.aspx" );
        }
        else
        {
            SessionState.ValidationMessage = "Désolé mais nous n'avons pas pu vous authentifier.";
            if ( courrielleOK == false && ModeFormulaire == Mode.AdresseEmail )
            {
                SessionState.ValidationMessage += "<br/>L'Adresse courrielle n'est pas valide.";
            }
            if ( telephoneOK == false && ModeFormulaire == Mode.Telephone )
            {
                SessionState.ValidationMessage += "<br/>Le Numéro de téléphone n'est pas valide.";
            }
            if ( codeOK == false )
            {
                SessionState.ValidationMessage += "<br/>Le Code d'accès n'est pas valide.";
            }
            ValiderMessage();
            return;
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
                return Request.Form[ "captcha" ].Substring( 0, 36 ) == ViewState[ "captchavalue" ].ToString();
            }
            return false;
        }
    }

    #endregion
}
