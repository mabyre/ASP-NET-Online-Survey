#region Using

using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using SettingXmlProvider;

#endregion

public partial class Admin_Pages_Settings : PageBase
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( !IsPostBack )
        {
            BindSettings();

            // Informations en lecture uniquement
            string directory = Server.MapPath( VirtualPathUtility.ToAbsolute( "~/UserFiles" ) );
            long size = RepertoireInfo.GetTaille( directory );
            LabelTailleUserFiles.Text = Tools.FileSizeFormat( size, "N" );

            directory = Server.MapPath( VirtualPathUtility.ToAbsolute( "~/MemberDataFiles" ) );
            size = RepertoireInfo.GetTaille( directory );
            LabelTailleMemberDataFiles.Text = Tools.FileSizeFormat( size, "N" );

            LabelUtilisateursConnecte.Text = Application[ "ActiveUsers" ].ToString();
        }

        Page.MaintainScrollPositionOnPostBack = true;
    }


    private void BindSettings()
    {
        TextBoxSiteNom.Text = Global.SettingsXml.SiteNom;
        TextBoxSiteSlogan.Text = Global.SettingsXml.SiteSlogan;
        TextBoxAdresse.Text = Global.SettingsXml.SiteAddress;
        TextBoxCopyright.Text = Global.SettingsXml.SiteCopyright;

        // Parametres
        TextBoxVirtualPath.Text = Global.SettingsXml.VirtualPath;
        TextBoxRepeatColumns.Text = Global.SettingsXml.RadioButtonListQuestionnaireRepeatColumn;
        CheckBoxLogUser.Checked = Global.SettingsXml.LogUser;
        CheckBoxContactAvecSociete.Checked = Global.SettingsXml.EnregistrerContactAvecSociete;
        CheckBoxContactAnonyme.Checked = Global.SettingsXml.EnregistrerContactAnonyme;

        // Parametres Membre
        CheckBoxDebloquerClient.Checked = Global.SettingsXml.DebloquerClient;
        CheckBoxNouveauMembrePrevenir.Checked = Global.SettingsXml.MembrePrevenir;
        CheckBoxConnexionMembrePrevenir.Checked = Global.SettingsXml.MembreConnexionPrevenir;
        CheckBoxNouveauMembreApprouve.Checked = Global.SettingsXml.MembreApprouve;
        CheckBoxNouveauMembreApprouveParEmail.Checked = Global.SettingsXml.MembreApprouveParEmail;
        TextBoxCodeAccesQuestionnaireExemple.Text = Global.SettingsXml.CodeAccesQuestionnaireExemple;

        // Reponses textuelles
        TextBoxLargeurMin.Text = Global.SettingsXml.ReponseTextuelleLargeurMin;
        TextBoxLargeurMax.Text = Global.SettingsXml.ReponseTextuelleLargeurMax;
        TextBoxLigneMax.Text = Global.SettingsXml.ReponseTextuelleLigneMax;

        // Limitation du nombre de contact visible par page
        TextBoxContactsParPageMin.Text = Global.SettingsXml.ContactsParPageMin;
        TextBoxContactsParPageMax.Text = Global.SettingsXml.ContactsParPageMax;
        TextBoxContactsParPageCourant.Text = Global.SettingsXml.ContactsParPageCourant;

        // Labels
        TextBoxBoutonQuestion.Text = Global.SettingsXml.LabelBoutonQuestion;

        // Limitations de l'utilisateur Decouverte
        TextBoxGratuitLimiteQuestionnaires.Text = Global.SettingsXml.GratuitLimiteQuestionnaires;
        TextBoxGratuitLimiteQuestions.Text = Global.SettingsXml.GratuitLimiteQuestions;
        TextBoxGratuitLimiteInterviewes.Text = Global.SettingsXml.GratuitLimiteInterviewes;
        TextBoxGratuitLimiteReponses.Text = Global.SettingsXml.GratuitLimiteReponses;

        // Limitations de l'utilisateur Abonne
        TextBoxAbonneLimiteQuestionnaires.Text = Global.SettingsXml.AbonneLimiteQuestionnaires;
        TextBoxAbonneLimiteQuestions.Text = Global.SettingsXml.AbonneLimiteQuestions;
        TextBoxAbonneLimiteInterviewes.Text = Global.SettingsXml.AbonneLimiteInterviewes;
        TextBoxAbonneLimiteReponses.Text = Global.SettingsXml.AbonneLimiteReponses;

        // Limitation des Imports de la liste des interviewes
        TextBoxMaxImportsInterviewes.Text = Global.SettingsXml.LimitationImportsInterviewes;
    }

    public void ButtonSave_OnClick( object sender, EventArgs e )
    {
        SettingXml sxml = new SettingXml();

        sxml.SiteNom = TextBoxSiteNom.Text;
        sxml.SiteSlogan = TextBoxSiteSlogan.Text;
        sxml.SiteAddress = TextBoxAdresse.Text;
        sxml.SiteCopyright = TextBoxCopyright.Text;
        sxml.VirtualPath = TextBoxVirtualPath.Text;
        sxml.LabelBoutonQuestion = TextBoxBoutonQuestion.Text;
        sxml.DebloquerClient = CheckBoxDebloquerClient.Checked;
        sxml.MembrePrevenir = CheckBoxNouveauMembrePrevenir.Checked;
        sxml.MembreConnexionPrevenir = CheckBoxConnexionMembrePrevenir.Checked;
        sxml.MembreApprouve = CheckBoxNouveauMembreApprouve.Checked;
        sxml.MembreApprouveParEmail = CheckBoxNouveauMembreApprouveParEmail.Checked;
        sxml.LogUser = CheckBoxLogUser.Checked;
        sxml.EnregistrerContactAvecSociete = CheckBoxContactAvecSociete.Checked;
        sxml.EnregistrerContactAnonyme = CheckBoxContactAnonyme.Checked;

        try
        {
            int code = int.Parse( TextBoxCodeAccesQuestionnaireExemple.Text );
            sxml.CodeAccesQuestionnaireExemple = TextBoxCodeAccesQuestionnaireExemple.Text;
        }
        catch
        {

        }
        

        try
        {
            int nbQuestionsParColonne = int.Parse( TextBoxRepeatColumns.Text );
            sxml.RadioButtonListQuestionnaireRepeatColumn = nbQuestionsParColonne.ToString();
        }
        catch
        {

        }

        try
        {
            int largeurMin = int.Parse( TextBoxLargeurMin.Text );
            sxml.ReponseTextuelleLargeurMin = largeurMin.ToString();
        }
        catch
        {

        }

        try
        {
            int i = int.Parse( TextBoxContactsParPageMin.Text );
            sxml.ContactsParPageMin = i.ToString();
        }
        catch
        {

        }

        try
        {
            int i = int.Parse( TextBoxContactsParPageMax.Text );
            sxml.ContactsParPageMax = i.ToString();
        }
        catch
        {

        }

        try
        {
            int i = int.Parse( TextBoxContactsParPageCourant.Text );
            sxml.ContactsParPageCourant = i.ToString();
        }
        catch
        {

        }

        try
        {
            int i = int.Parse( TextBoxMaxImportsInterviewes.Text );
            sxml.LimitationImportsInterviewes = i.ToString();
        }
        catch
        {

        }

        try
        {
            int largeurMax = int.Parse( TextBoxLargeurMax.Text );
            sxml.ReponseTextuelleLargeurMax = largeurMax.ToString();
        }
        catch
        {

        }

        try
        {
            int ligneMax = int.Parse( TextBoxLigneMax.Text );
            sxml.ReponseTextuelleLigneMax = ligneMax.ToString();
        }
        catch
        {

        }

        // Limitations de l'utilisateur Decouverte
        try { int i = int.Parse( TextBoxGratuitLimiteQuestionnaires.Text ); sxml.GratuitLimiteQuestionnaires = i.ToString(); }
        catch { }
        try { int i = int.Parse( TextBoxGratuitLimiteQuestions.Text ); sxml.GratuitLimiteQuestions = i.ToString(); }
        catch { }
        try { int i = int.Parse( TextBoxGratuitLimiteInterviewes.Text ); sxml.GratuitLimiteInterviewes = i.ToString(); }
        catch { }
        try { int i = int.Parse( TextBoxGratuitLimiteReponses.Text ); sxml.GratuitLimiteReponses = i.ToString(); }
        catch { }

        // Limitations de l'utilisateur Abonne
        try { int i = int.Parse( TextBoxAbonneLimiteQuestionnaires.Text ); sxml.AbonneLimiteQuestionnaires = i.ToString(); }
        catch { }
        try { int i = int.Parse( TextBoxAbonneLimiteQuestions.Text ); sxml.AbonneLimiteQuestions = i.ToString(); }
        catch { }
        try { int i = int.Parse( TextBoxAbonneLimiteInterviewes.Text ); sxml.AbonneLimiteInterviewes = i.ToString(); }
        catch { }
        try { int i = int.Parse( TextBoxAbonneLimiteReponses.Text ); sxml.AbonneLimiteReponses = i.ToString(); }
        catch { }

        //
        // Sauvegarde Generale
        // 
        sxml.Save( sxml );

        Global.SettingsXml = sxml.Reload();
        Response.Redirect( Request.RawUrl, true );
    }
}