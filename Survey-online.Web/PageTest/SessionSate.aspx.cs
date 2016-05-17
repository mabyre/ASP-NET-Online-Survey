using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Web.Configuration;


public partial class PageTest_SessionSate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpSessionState httpss = HttpContext.Current.Session;

        
        SessionStateSection sss = new SessionStateSection();

        LabelSessionSate.Text += "SqlCommandTimeout : " + sss.Mode + "<br>";
        LabelSessionSate.Text += "CookieName : " + sss.CookieName + "<br>";
        LabelSessionSate.Text += "Cookieless : " + sss.Cookieless + "<br>";
        LabelSessionSate.Text += "<br>";
        LabelSessionSate.Text += "SqlCommandTimeout : " + sss.SqlCommandTimeout + "<br>";
        LabelSessionSate.Text += "StateNetworkTimeout : " + sss.StateNetworkTimeout + "<br>";
        LabelSessionSate.Text += "Timeout : " + sss.Timeout + "<br>";


        LabelSessionSate.Text += "<br>";
        LabelSessionSate.Text += "<br>";

        // Get the Web application configuration object.
        System.Configuration.Configuration configuration =
          System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration( null );

        // Get the section related object.
        System.Web.Configuration.SessionStateSection sessSS = ( SessionStateSection ) configuration.GetSection( "system.web/sessionState" );

        LabelSessionSate.Text += "SqlCommandTimeout : " + sessSS.Mode + "<br>";
        LabelSessionSate.Text += "CookieName : " + sessSS.CookieName + "<br>";
        LabelSessionSate.Text += "Cookieless : " + sessSS.Cookieless + "<br>";
        LabelSessionSate.Text += "<br>";
        LabelSessionSate.Text += "SqlCommandTimeout : " + sessSS.SqlCommandTimeout + "<br>";
        LabelSessionSate.Text += "StateNetworkTimeout : " + sessSS.StateNetworkTimeout + "<br>";
        LabelSessionSate.Text += "Timeout : " + sessSS.Timeout + "<br>";

    }
}
