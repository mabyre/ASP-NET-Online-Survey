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

public partial class PageTest_PageTestSubstitution : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Display the current date and time in the label.
        // Output caching applies to this section of the page.
        CachedDateLabel.Text = DateTime.Now.ToString();
    }

    // The Substitution control calls this method to retrieve
    // the current date and time. This section of the page
    // is exempt from output caching. 
    public static string GetCurrentDateTime( HttpContext context )
    {
        return DateTime.Now.ToString();
    }


}
