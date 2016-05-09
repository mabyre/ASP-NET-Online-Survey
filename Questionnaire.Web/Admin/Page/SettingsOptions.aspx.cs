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
using Sql.Web.Data;

#endregion

public partial class Admin_Pages_SettingsOptions : PageBase
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( IsPostBack == false )
        {
            CheckBoxPrevenirNouvelleReponse.Checked = SessionState.MemberSettings.PrevenirNouvelleReponse;
            TextBoxBoutonQuestionSuivanteTexte.Text = SessionState.MemberSettings.BoutonQuestionSuivanteTexte;
            TextBoxBoutonQuestionSuivanteAlt.Text = SessionState.MemberSettings.BoutonQuestionSuivanteAlt;
            TextBoxTaillePage.Text = SessionState.MemberSettings.TaillePageQuestions;
        }
    }

    protected void CheckBoxPrevenirNouvelleReponse_CheckedChanged( object sender, EventArgs e )
    {
        SessionState.MemberSettings.PrevenirNouvelleReponse = CheckBoxPrevenirNouvelleReponse.Checked;
        SessionState.MemberSettings.Update();
    }

    protected void TextBoxBoutonQuestionSuivanteTexte_TextChanged( object sender, EventArgs e )
    {
        if ( TextBoxBoutonQuestionSuivanteTexte.Text.Trim() != "" )
        {
            SessionState.MemberSettings.BoutonQuestionSuivanteTexte = TextBoxBoutonQuestionSuivanteTexte.Text;
            SessionState.MemberSettings.Update();
        }
        else
        {
            TextBoxBoutonQuestionSuivanteTexte.Text = SessionState.MemberSettings.BoutonQuestionSuivanteTexte;
        }
    }

    protected void TextBoxBoutonQuestionSuivanteAlt_TextChanged( object sender, EventArgs e )
    {
        if ( TextBoxBoutonQuestionSuivanteAlt.Text.Trim() != "" )
        {
            SessionState.MemberSettings.BoutonQuestionSuivanteAlt = TextBoxBoutonQuestionSuivanteAlt.Text;
            SessionState.MemberSettings.Update();
        }
        else
        {
            TextBoxBoutonQuestionSuivanteAlt.Text = SessionState.MemberSettings.BoutonQuestionSuivanteAlt;
        }
    }

    protected void TextBoxTaillePage_TextChanged( object sender, EventArgs e )
    {
        if ( TextBoxTaillePage.Text.Trim() != "" )
        {
            int taillePage = int.Parse( SessionState.MemberSettings.TaillePageQuestions );
            try
            {
                taillePage = int.Parse( TextBoxTaillePage.Text );
                if ( taillePage >= 1 && taillePage <= 100 )
                {
                    SessionState.MemberSettings.TaillePageQuestions = taillePage.ToString();
                    SessionState.MemberSettings.Update();
                }
            }
            catch { }
        }
        TextBoxTaillePage.Text = SessionState.MemberSettings.TaillePageQuestions;
    }
}