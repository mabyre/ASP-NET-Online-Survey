/*
**
*/
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

public partial class Control_MenuPanel: System.Web.UI.UserControl
{
    protected void Page_Load( object sender, EventArgs e )
    {
        /* Compatibilite avec Google Chrome */
        if ( Request.UserAgent.IndexOf( "AppleWebKit" ) > 0 )
        {
            Request.Browser.Adapters.Clear();
        } 

        if ( Page.IsPostBack == false )
        {
            if ( MenuPanel.SelectedItem != null )
            {
                string aaa = MenuPanel.SelectedItem.Selected.ToString();
            }

            if ( Page.User.Identity.IsAuthenticated )
            {
                MenuPanel.DataSource = SiteMap.Providers[ "MenuPanelSiteMap" ].RootNode.ChildNodes;
                MenuPanel.DataBind();
            }
        }
    }
}
