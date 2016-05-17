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
using System.Globalization;
using System.Threading;

public partial class PageTest_PageTestAjax : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        CultureInfo culture = CultureInfo.CreateSpecificCulture( "fr-FR" );
        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;


    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetDynamicContent( string contextKey )
    {
        return default( string );
    }

    protected void ButtonOk_Click( object sender, EventArgs e )
    {
        string date = TextBoxDate.Date.ToString();

        int i = 123;
    }
}
