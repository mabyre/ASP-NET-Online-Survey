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

public partial class Member_Logout : PageBase
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( IsPostBack == false )
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            // a ne surtout pas faire, comme on vient de la page logout pour se relogguer
            // apres login on est redirigee vers logout !
            // FormsAuthentication.RedirectToLoginPage();

            // Il faut rediriger vers une autre page sinon c'est la page Member/logout.aspx qui reste en memoire
            // et cela ne marche pas !
            Response.Redirect( "~/Default.aspx" );
        }
    }
}
