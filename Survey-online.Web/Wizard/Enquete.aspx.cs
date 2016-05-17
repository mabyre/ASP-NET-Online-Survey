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

public partial class Wizard_Enquete : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( Page.IsPostBack == false )
        {
            LabelNombreQuestionnaires.Text += SessionState.Questionnaires.Count.ToString();

            ButtonRedigerEmail.Text = "Editez l'email\r\naux interviewés";
            ButtonEditerPageAccueil.Text = "Editez\r\nla page d'accueil";
            ButtonImporter.Text = "Importez la liste\r\ndes Interviewés";
            ButtonEvoyerEmail.Text = "Envoyez les emails\r\naux Interviewés";
            ButtonStatistiques.Text = "Dépouillez\r\nles statistiques";
        }
    }

    protected void ButtonImporter_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Contact/Manage.aspx", true );
    }

    protected void ButtonRedigerEmail_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/WebContent/Edit.aspx?sectionname=CorpsEmail" );
    }

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

    protected void ButtonConseilInterpretation_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Poll/QuestionnaireStatAll.aspx" );
    }    
}
