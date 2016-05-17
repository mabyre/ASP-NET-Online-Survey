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

public partial class Wizard_Questionnaire : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( Page.IsPostBack == false )
        {
            // Affcher l'aide contextuelle
            CollapsiblePanelExtenderAide2.Collapsed = !SessionState.MemberSettings.AfficherAide;
            CollapsiblePanelExtenderAide1.Collapsed = !SessionState.MemberSettings.AfficherAide;
        }
    }

    protected void ButtonCreerQuestionnaire_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Questionnaire/Edit.aspx" );
    }

    protected void ButtonRetourAccueil_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Wizard/Accueil.aspx", true );
    }
}
