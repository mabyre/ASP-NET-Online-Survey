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

public partial class Controls_LoginBanner : System.Web.UI.UserControl
{
    protected void Page_Load( object sender, System.EventArgs e )
    {
        if ( Page.User.Identity.IsAuthenticated == false )
        {
            string link = "~/Member/Login.aspx?ReturnURL=";
            string url = Request.Url.ToString();
            link += System.Web.HttpUtility.UrlEncode( url );
            HyperLink hlLogin = ( HyperLink )LoginView1.FindControl( "HyperLinkLogin" );
            if ( hlLogin != null )
            {
                hlLogin.NavigateUrl = link;
            }
        }
        else
        {
            Label LabelUserInRoles = ( Label )LoginView1.FindControl( "LabelUserInRoles" );
            if ( LabelUserInRoles != null )
            {
                LabelUserInRoles.Text = "";
                string[] roles = Roles.GetRolesForUser();
                if ( roles.Length != 0 )
                {
                    foreach ( string r in roles )
                    {
                        LabelUserInRoles.Text += " | " + r;
                    }
                }
            }
        }
    }

    protected void LinkButtonLogout_Click( object sender, System.EventArgs e )
    {
        FormsAuthentication.SignOut();
        Session.Clear();
        Session.Abandon();
        
        Response.Redirect( "~/Default.aspx" );
    }

    [Obsolete()]
    private string GetFileName( string str )
    {
        string st;
        int count;
        st = str.Remove( 0, 1 );
        count = st.IndexOf( '/' );
        st = st.Remove( 0, count );
        st = "~" + st;
        return st;
    }
}
