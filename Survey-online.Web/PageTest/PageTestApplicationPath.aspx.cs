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

public partial class PageTest_PageTestApplicationPath : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        // Comment trouve le path root du site web ?
        string appRelativeCurrentExecutionFilePath = Request.AppRelativeCurrentExecutionFilePath;
        string absoluteUri = Request.Url.AbsoluteUri;
        string siteWebUri = "";
        appRelativeCurrentExecutionFilePath = appRelativeCurrentExecutionFilePath.Substring( 1 );
        siteWebUri = absoluteUri.Substring( 0, absoluteUri.Length - appRelativeCurrentExecutionFilePath.Length );

        string SiteWebInOneLine = Request.Url.AbsoluteUri.Substring( 0, Request.Url.AbsoluteUri.Length - Request.AppRelativeCurrentExecutionFilePath.Substring( 1 ).Length );
        LabelInfos.Text = SiteWebInOneLine;
    }
}
