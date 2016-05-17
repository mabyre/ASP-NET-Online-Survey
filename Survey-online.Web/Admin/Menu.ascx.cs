using System;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Admin_menu : System.Web.UI.UserControl
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( !Page.IsCallback )
            BindMenu();
    }

    private void BindMenu()
    {
        foreach ( SiteMapNode adminNode in SiteMap.Providers[ "AdminSiteMap" ].RootNode.ChildNodes )
        {
            bool isAccessibleToUser = adminNode.IsAccessibleToUser( HttpContext.Current );
            if ( isAccessibleToUser )
            {
                HtmlAnchor a = new HtmlAnchor();
                a.HRef = adminNode.Url;
                a.InnerHtml = "<span>" + Translate( adminNode.Title ) + "</span>";//"<span>" + Translate(info.Name.Replace(".aspx", string.Empty)) + "</span>";
                if ( Request.RawUrl.EndsWith( adminNode.Url, StringComparison.OrdinalIgnoreCase ) )
                    a.Attributes[ "class" ] = "current";
                HtmlGenericControl li = new HtmlGenericControl( "li" );
                li.Controls.Add( a );
                ulMenu.Controls.Add( li );
            }
        }
    }

    public void AddItem( string text, string url )
    {
        HtmlAnchor a = new HtmlAnchor();
        a.InnerHtml = "<span>" + text + "</span>";
        a.HRef = url;

        HtmlGenericControl li = new HtmlGenericControl( "li" );
        li.Controls.Add( a );
        ulMenu.Controls.Add( li );
    }

    public string Translate( string text )
    {
        try
        {
            return this.GetGlobalResourceObject( "labels", text ).ToString();
        }
        catch ( NullReferenceException )
        {
            return text;
        }
    }

}
