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
using AjaxControlToolkit;

public partial class Default_master : System.Web.UI.MasterPage
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( IsPostBack == false )
        {
            PanelMenu.Visible = Page.User.Identity.IsAuthenticated;
            if ( Page.User.Identity.IsAuthenticated )
            {
                PanelMenu.Visible = SessionState.MemberSettings.MenuExpert;
                TdMasterPageColonneGauche.Visible = SessionState.MemberSettings.MenuColonne;
                PanelBarreNavigation.Visible = SessionState.MemberSettings.BarreNavigation;
                UpdatePanelToujoursVisible.Visible = SessionState.MemberSettings.MenuToujoursVisible;
                Utils.AlwaysVisibleControlExtenderPosition( ref AlwaysVisibleControlExtender );
            }
            else
            {
                PanelBarreNavigation.Visible = false;
                TdMasterPageColonneGauche.Visible = false;
                TdMasterPageLoginView.Visible = false;
            }
        }
    }
}
