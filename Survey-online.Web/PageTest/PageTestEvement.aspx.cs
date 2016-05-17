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

public partial class PageTest_PageTestWebUserControlTestEvement : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        Trace.Warn( "Page_Load" );
        if ( IsPostBack == false )
        {
            Trace.Warn( "Page_Load : IsPostBack == false" );
        }
    }

    protected override void OnLoad( EventArgs e )
    {
        Trace.Warn( "OnLoad" );
        base.OnLoad( e );
        Trace.Warn( "base.OnLoad" );
    }

    protected override void OnInit( EventArgs e )
    {
        Trace.Warn( "OnInit" );
        if ( IsPostBack == false )
        {
            Trace.Warn( "OnInit : IsPostBack == false" );
        }
        else
        {
            Trace.Warn( "OnInit : IsPostBack == true" );
        }
        base.OnInit( e );
        Trace.Warn( "base.OnInit" );
    }

    protected override void OnPreRender( EventArgs e )
    {
        Trace.Warn( "OnPreRender" );
        base.OnPreRender( e );
        Trace.Warn( "base.OnPreRender" );
    }

    protected override void OnUnload( EventArgs e )
    {
        base.OnUnload( e );
        //Trace.Warn( "OnUnload" );
    }

    protected override void Render( HtmlTextWriter writer )
    {
        Trace.Warn( "Render" );
        base.Render( writer );
        Trace.Warn( "base.Render" );
    }

    protected override void RenderChildren( HtmlTextWriter writer )
    {
        Trace.Warn( "RenderChildren" );
        base.RenderChildren( writer );
        Trace.Warn( "base.RenderChildren" );
    }


    protected override void CreateChildControls()
    {
        Trace.Warn( "CreateChildControls" );
        base.CreateChildControls();
    }

    protected void ButtonOk_Click( object sender, EventArgs e )
    {
        Trace.Warn( "ButtonOk_Click()" );
    }
}
