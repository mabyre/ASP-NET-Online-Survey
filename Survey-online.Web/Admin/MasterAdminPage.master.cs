using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class MasterPage_Admin : System.Web.UI.MasterPage
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated == false )
        {
            Response.Redirect( "~/" );
        }
    }

    protected void LoginStatusButton_LoggingOut( object sender, LoginCancelEventArgs e )
    {
        FormsAuthentication.SignOut();
        Response.Redirect( "~/", true );
    }
}
