//
// Cela control ne sert pas a grand chose mais plutot que de rajouter des butons a toutes
// les pages il suffit de l'ajouter ici ...
//

#region Using
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
#endregion

public partial class UserControl_MenuContexte : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ( Page.IsPostBack == false )
        {
            if ( Request.Browser.Type.Contains( "InternetExplorer" ) )
            {
                ButtonWizardQuestion.Text = "Ajoutez\r\ndes Questions";
                ButtonPollList.Text = "Visualisez\r\nle Questionnaire";
                ButtonPollQuestionnaire.Text = "Testez\r\nle Questionnaire";
                ButtonWizardQuestionEnchainee.Text = "Enchainez\r\nles Questions";
            }

            string pageNom = Page.AppRelativeVirtualPath;
            switch ( pageNom )
            {
                case "~/Wizard/Accueil.aspx" :
                    PanelAccueil.Visible = false;
                    break;
                case "~/Wizard/Questionnaire.aspx" :
                    PanelWizardQuestion.Visible = true;
                    break;
                case "~/Questionnaire/Manage.aspx":
                case "~/Poll/List.aspx":
                    PanelWizardQuestion.Visible = true;
                    PanelPollQuestionnaire.Visible = true;
                    break;
                case "~/Wizard/Question.aspx" :
                    PanelPollList.Visible = true;
                    PanelPollQuestionnaire.Visible = true;
                    PanelWizardQuestion.Visible = true; // AME25092010
                    PanelWizardQuestionEnchainee.Visible = true;
                    break;
                case "~/Poll/Questionnaire.aspx" :
                    PanelWizardQuestion.Visible = true;
                    PanelPollList.Visible = true;
                    break;
                case "~/Wizard/QuestionEnchainee.aspx" :
                case "~/Poll/QuestionnaireStatAll.aspx" :
                case "~/Questionnaire/Copier.aspx":
                case "~/Poll/Answers.aspx":
                    PanelPollList.Visible = true;
                    break;
                case "~/Contact/Email.aspx" :
                    PanelStatistiques.Visible = true;
                    break;
                case "~/Poll/Manage.aspx" :
                case "~/StyleWeb/Edit.aspx" :
                case "~/Score/Manage.aspx":
                    PanelPollQuestionnaire.Visible = true;
                    PanelPollList.Visible = true;
                    break;
                case "~/Questionnaire/Edit.aspx":
                    PanelPollList.Visible = true;
                    break;

            }
        }
    }

    protected void ButtonStatistiques_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Poll/QuestionnaireStatAll.aspx", true );
    }

    protected void ButtonRetourAccueil_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Wizard/Accueil.aspx", true );
    }

    protected void ButtonWizardQuestion_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Wizard/Question.aspx", true );
    }

    protected void ButtonPollList_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Poll/List.aspx", true );
    }

    protected void ButtonPollQuestionnaire_Click( object sender, EventArgs e )
    {
        if ( SessionState.Questionnaire != null )
        {
            Response.Redirect( "~/Poll/Questionnaire.aspx?QuestionnaireID=" + SessionState.Questionnaire.QuestionnaireID.ToString(), true );
        }
        Tools.PageValidation( "Vous devez d'abord sélectionner un questionnaire." );
    }

    protected void ButtonWizardQuestionEnchainee_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Wizard/QuestionEnchainee.aspx", true );
    }
}
