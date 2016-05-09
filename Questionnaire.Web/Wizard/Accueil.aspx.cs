using System;
using System.Collections;
using System.Configuration;
using System.Data;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using Sql.Web.Data;
using AjaxControlToolkit;

public partial class Wizard_Accueil : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( Page.IsPostBack == false )
        {
            // Prenom Nom de l'utilisateur
            if ( User.Identity.IsAuthenticated )
            {
                if ( User.IsInRole( "Administrateur" ) )
                {
                    LabelUtilisateur.Text = "Administrateur ";
                }
                if ( SessionState.MemberInfo != null )
                {
                    LabelUtilisateur.Text += SessionState.MemberInfo.Prenom + " " + SessionState.MemberInfo.Nom + " ";
                }

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
            else
            {
                try
                {
                    Response.Redirect( "~/Member/Login.aspx" );
                }
                catch ( Exception ex )
                {
                    string msg = ex.Message;
                }
            }

            // Recalculer les Objets du membre
            SessionState.Limitations = null;
            LabelNombreQuestionnaires.Text += SessionState.Limitations.Questionnaires.ToString();

            if ( User.IsInRole( "Découverte" ) )
            {
                PanelMembreAbonnement.GroupingText = "Membre Découverte :";
                PanelMembreAbonnement.CssClass = "PanelMembreDecouverteStyle";
                TableMembreAbonnement.Attributes.Add( "class", "TableMembreDecouverteStyle" );
                TdTableMembreAbonnementHead1.Attributes.Add( "class", "TdTableMembreAbonnementHead1Style" );
                LabelAbonnementHead1.Text = "Limitations";
                TdTableMembreAbonnementHead2.Attributes.Add( "class", "TdTableMembreAbonnementHead2Style" );
                LabelAbonnementHead2.Text = "Vos Objets";
            }
            else
            {
                PanelMembreAbonnement.GroupingText = "Membre Abonné :";
                PanelMembreAbonnement.CssClass = "PanelMembreAbonneStyle";
                TableMembreAbonnement.Attributes.Add( "class", "TableMembreAbonneStyle" );
                TdTableMembreAbonnementHead1.Attributes.Add( "class", "TdTableMembreAbonnementHead1Style" );
                LabelAbonnementHead1.Text = "Votre Abonnement";
                TdTableMembreAbonnementHead2.Attributes.Add( "class", "TdTableMembreAbonnementHead2Style" );
                LabelAbonnementHead2.Text = "Vos Objets";
                LabelDateFinAbonnement.Text = SessionState.MemberInfo.DateFinAbonnement.ToShortDateString();
            }

            LabelDateFinAbonnement.Text = SessionState.MemberInfo.DateFinAbonnement.ToShortDateString();
            DateTime now = DateTime.Now;
            if ( now < SessionState.MemberInfo.DateFinAbonnement )
            {
            }
            else
            {
                LabelDateFinAbonnement.CssClass = "BoundFieldDateRedStyle";
                // Dernier jour d'Abonnement
                if ( now < SessionState.MemberInfo.DateFinAbonnement.AddDays( 1 ) )
                {
                    LabelDateFinAbonnement.CssClass = "BoundFieldDateOrangeStyle";
                }
            }
            TimeSpan ts = SessionState.MemberInfo.DateFinAbonnement - DateTime.Now;
            LabelDateFinAbonnement.ToolTip = "Fin d'abonnement dans : " + ts.Days.ToString() + " jours";

            LabelLimiteQuestionnaires.Text += SessionState.Limitations.NombreQuestionnaires.ToString();
            LabelLimiteQuestions.Text += SessionState.Limitations.NombreQuestions.ToString();
            LabelLimiteInterviewes.Text += SessionState.Limitations.NombreInterviewes.ToString();
            LabelLimiteReponses.Text += SessionState.Limitations.NombreReponses.ToString();

            LabelQuestionnaires.Text = SessionState.Limitations.Questionnaires.ToString();
            LabelQuestions.Text = SessionState.Limitations.Questions.ToString();
            LabelInterviewes.Text = SessionState.Limitations.Interviewes.ToString();
            LabelReponses.Text = SessionState.Limitations.Reponses.ToString();

            if ( Request.Browser.Type.Contains( "InternetExplorer" ) )
            {
                // Je ne sais pas mettre /r/n dans le .ascx !
                ButtonCreerQuestionnaire.Text = "Créez\r\nun Questionnaire";
                ButtonStatistiques.Text = "Dépouillez\r\nles statistiques";
                ButtonRedigerEmail.Text = "Editez l'email\r\naux Interviewés";
                ButtonEditerPageAccueil.Text = "Editez\r\nla Page d'accueil";
                ButtonImporter.Text = "Importez la liste\r\ndes Interviewés";
                ButtonEvoyerEmail.Text = "Envoyez l'email\r\naux Interviewés";
                ButtonStatistiques.Text = "Dépouillez\r\nles statistiques";
                ButtonAjouterQuestion.Text = "Ajoutez\r\ndes Questions";
                ButtonAjouterQuestionEnchainee.Text = "Ajoutez Questions\r\nEnchaînées";
                ButtonQuestionnaireList.Text = "Visualisez vos\r\nQuestionnaires";
            }

            CheckBoxAideContextuelle.Checked = SessionState.MemberSettings.AfficherAide;
            CheckBoxAideEnLigne.Checked = SessionState.MemberSettings.AfficherAideEnLigne;
            CheckBoxMenuExpert.Checked = SessionState.MemberSettings.MenuExpert;
            CheckBoxMenuColonne.Checked = SessionState.MemberSettings.MenuColonne;
            CheckBoxBarreNavigation.Checked = SessionState.MemberSettings.BarreNavigation;
            CheckBoxMenuToujoursVisible.Checked = SessionState.MemberSettings.MenuToujoursVisible;
            DropDownListMenuToujoursVisiblePosition.SelectedValue = SessionState.MemberSettings.MenuToujoursVisiblePosition;

            PanelControlNavigation.Visible = SessionState.BooleanSate[ "ImageButtonExpandNavigation" ];
            if ( PanelControlNavigation.Visible )
            {
                ImageButtonExpandNavigation.ImageUrl = "~/Images/collapse.jpg";
            }
            else
            {
                ImageButtonExpandNavigation.ImageUrl = "~/Images/expand.jpg";
            }
        }
    }

    protected void ButtonQuestionnaireList_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Poll/List.aspx" );
    }

    protected void ButtonAjouterQuestionEnchainee_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Wizard/QuestionEnchainee.aspx", true );
    }

    protected void ButtonRedigerEmail_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/WebContent/Edit.aspx?sectionname=CorpsEmail" );
    }

    protected void ButtonEditerPageAccueil_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/WebContent/Edit.aspx?sectionname=PageAccueil" );
    }

    protected void ButtonQuestionnaireModifier_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Wizard/QuestionnaireModifier.aspx", true );
    }

    protected void ButtonCreerQuestionnaire_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Wizard/Questionnaire.aspx", true );
    }

    protected void ButtonAjouterQuestion_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Wizard/Question.aspx", true );
    }

    protected void ButtonViusaliser_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Poll/List.aspx", true );
    }

    protected void ButtonModifier_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Poll/List.aspx", true );
    }

    protected void ButtonImporter_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Contact/Manage.aspx", true );
    }

    protected void ButtonEvoyerEmail_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Contact/Email.aspx" );
    }

    protected void ButtonStatistiques_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Poll/QuestionnaireStatAll.aspx" );
    }

    protected void ImageButtonExpandNavigation_Click( object sender, ImageClickEventArgs e )
    {
        PanelControlNavigation.Visible = PanelControlNavigation.Visible == false;
        SessionState.BooleanSate[ "ImageButtonExpandNavigation" ] = PanelControlNavigation.Visible;
        if ( PanelControlNavigation.Visible )
        {
            ImageButtonExpandNavigation.ImageUrl = "~/Images/collapse.jpg";
        }
        else
        {
            ImageButtonExpandNavigation.ImageUrl = "~/Images/expand.jpg";
        }
    }

    // Curieux comme code on devrait utiliser simplement SessionState.MemberSettings
    // mais ca marche je touche pas
    protected void CheckBoxAideEnLigne_CheckedChanged( object sender, EventArgs e )
    {
        MemberSettings membreSettings = SessionState.MemberSettings;
        membreSettings.AfficherAideEnLigne = CheckBoxAideEnLigne.Checked;
        membreSettings.Update();
        SessionState.MemberSettings = null;
        Response.Redirect( Request.RawUrl );
    }

    protected void CheckBoxAideContextuelle_CheckedChanged( object sender, EventArgs e )
    {
        MemberSettings membreSettings = SessionState.MemberSettings;
        membreSettings.AfficherAide = CheckBoxAideContextuelle.Checked;
        membreSettings.Update();
        SessionState.MemberSettings = null;
        Response.Redirect( Request.RawUrl );
    }

    protected void CheckBoxMenuExpert_CheckedChanged( object sender, EventArgs e )
    {
        MemberSettings membreSettings = SessionState.MemberSettings;
        membreSettings.MenuExpert = CheckBoxMenuExpert.Checked;
        membreSettings.Update();
        SessionState.MemberSettings = null;
        Response.Redirect( Request.RawUrl );
    }

    protected void CheckBoxMenuColonne_CheckedChanged( object sender, EventArgs e )
    {
        MemberSettings membreSettings = SessionState.MemberSettings;
        membreSettings.MenuColonne = CheckBoxMenuColonne.Checked;
        membreSettings.Update();
        SessionState.MemberSettings = null;
        Response.Redirect( Request.RawUrl );
    }

    protected void CheckBoxBarreNavigation_CheckedChanged( object sender, EventArgs e )
    {
        MemberSettings membreSettings = SessionState.MemberSettings;
        membreSettings.BarreNavigation = CheckBoxBarreNavigation.Checked;
        membreSettings.Update();
        SessionState.MemberSettings = null;
        Response.Redirect( Request.RawUrl );
    }

    protected void CheckBoxMenuToujoursVisible_CheckedChanged( object sender, EventArgs e )
    {
        MemberSettings membreSettings = SessionState.MemberSettings;
        membreSettings.MenuToujoursVisible = CheckBoxMenuToujoursVisible.Checked;
        membreSettings.Update();
        SessionState.MemberSettings = null;
        Response.Redirect( Request.RawUrl );
    }

    protected void DropDownListMenuToujoursVisiblePosition_SelectedIndexChanged( object sender, EventArgs e )
    {
        string position = "";
        position = DropDownListMenuToujoursVisiblePosition.SelectedValue;
        MemberSettings membreSettings = SessionState.MemberSettings;
        membreSettings.MenuToujoursVisiblePosition = position;
        membreSettings.Update();
        SessionState.MemberSettings = null;
        Response.Redirect( Request.RawUrl );
    }

    protected void ButtonParDefaut_Click( object sender, EventArgs e )
    {
        SessionState.MemberSettings.AfficherAideEnLigne = false;
        SessionState.MemberSettings.AfficherAide = false;
        SessionState.MemberSettings.BarreNavigation = true;
        SessionState.MemberSettings.MenuColonne = true;
        SessionState.MemberSettings.MenuExpert = true;
        SessionState.MemberSettings.MenuToujoursVisible = true;
        SessionState.MemberSettings.MenuToujoursVisiblePosition = "HD";
        SessionState.MemberSettings.Update();
        Response.Redirect( Request.RawUrl );
    }
}
