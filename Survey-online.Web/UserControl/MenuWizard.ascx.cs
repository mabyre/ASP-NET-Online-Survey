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

public partial class UserControl_MenuWizard : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ( Page.IsPostBack == false )
        {
            if ( Request.Browser.Type.Contains( "InternetExplorer" ) )
            {
                // Je ne sais pas mettre /r/n dans le .ascx !
                // BRY_19052016
                //ButtonCreerQuestionnaire.Text = "Créez\r\nun questionnaire";
                //ButtonEditerPageAccueil.Text = "Editez la page d'accueil\r\nde vos interviewés";
                //ButtonAjouterQuestion.Text = "Ajoutez\r\ndes Questions";
                //ButtonViusaliser.Text = "Visualisez\r\nvos questionnaires";
                //ButtonRedigerEmail.Text = "Editez l'email\r\naux interviewés";
                //ButtonEditerPageAccueil.Text = "Editez\r\nla page d'accueil";
                //ButtonImporter.Text = "Importez la liste\r\ndes Interviewés";
                //ButtonEvoyerEmail.Text = "Envoyez les emails\r\naux Interviewés";
                //ButtonStatistiques.Text = "Dépouillez\r\nles statistiques";
            }

            // Controler la largeur de la colonne qui contient le Menu
            // this 
            // parent MenuWizard
            // parent LoginView
            HtmlTableCell htmlCell = ( HtmlTableCell )this.Parent.Parent.Parent.FindControl( "TdMasterPageColonneGauche" );
            htmlCell.Width = "150px";
        }
    }

    protected void ButtonCreerQuestionnaire_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Wizard/Questionnaire.aspx", true );
    }

    protected void ButtonAjouterQuestion_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Wizard/Question.aspx", true );
    }

    protected void ButtonModifier_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Poll/List.aspx", true );
    }

    protected void ButtonImporter_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Contact/Manage.aspx", true );
    }

    protected void ButtonRedigerEmail_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/WebContent/Edit.aspx?sectionname=CorpsEmail" );
    }

    // 5
    protected void ButtonEditerPageAccueil_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/WebContent/Edit.aspx?sectionname=PageAccueil" );
    }

    protected void ButtonEvoyerEmail_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Contact/Email.aspx" );
    }

    protected void ButtonStatistiques_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Poll/QuestionnaireStatAll.aspx" );
    }
}
