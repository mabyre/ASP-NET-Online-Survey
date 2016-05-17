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

public partial class PageTest_WebUserControlTestEvenements : System.Web.UI.UserControl
{
    protected override void OnLoad( EventArgs e )
    {
        Response.Write( "<!-- OnLoad -->" );
        base.OnLoad( e );
        Response.Write( "<!-- base.OnLoad -->" );
    }

    protected override void OnInit( EventArgs e )
    {
        Response.Write( "<!-- OnInit -->" );
        base.OnInit( e );
        Response.Write( "<!-- base.OnInit -->" );
    }
    
    protected override void OnPreRender( EventArgs e )
    {
        Response.Write( "<!-- OnPreRender -->" );
        base.OnPreRender( e );
        Response.Write( "<!-- base.OnPreRender -->" );
    }

    protected override void OnUnload( EventArgs e )
    {
        base.OnUnload( e );
        //Response.Write( "<!-- OnUnload -->" );
    }

    protected override void Render( HtmlTextWriter writer )
    {
        Response.Write( "<!-- Render -->" );
        base.Render( writer );
        Response.Write( "<!-- base.Render -->" );
    }

    protected override void RenderChildren( HtmlTextWriter writer )
    {
        Response.Write( "<!-- RenderChildren -->" );
        base.RenderChildren( writer );
        Response.Write( "<!-- base.RenderChildren -->" );
    }

    //protected override void RenderControl( HtmlTextWriter writer )
    //{
    //    base.RenderControl( writer );
    //    Response.Write( "<!-- RenderChildren -->" );
    //}

    protected override void TrackViewState()
    {
        Response.Write( "<!-- TrackViewState -->" );
        base.TrackViewState();
        Response.Write( "<!-- base.TrackViewState -->" );
    }
    
    protected void Page_Load( object sender, EventArgs e )
    {
        Response.Write( "<!-- Page Load -->" );
        if ( IsPostBack == false )
        {
            Response.Write( "<!-- IsPostBack == false -->" );
        }
    }

    protected void ButtonOk_Click( object sender, EventArgs e )
    {
        Response.Write( "<!-- ButtonOk_Click -->" );
    }
}
