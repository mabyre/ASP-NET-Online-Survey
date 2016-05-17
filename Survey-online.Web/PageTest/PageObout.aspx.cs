using System;
using System.Collections;
using System.Configuration;
using System.Data;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using OboutInc.ColorPicker;

public partial class PageTest_PageObout : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( IsPostBack == false )
        {
            picker.InitialColor = color.Style[ "background-color" ] = previous.Style[ "background-color" ] = "#FFFFFF";
        }
    }

    public void Color_CallBack( object sender, ColorPostBackEventArgs e )
    {
        color.Style[ "background-color" ] = e.Color;
        previous.Style[ "background-color" ] = e.PreviousColor;
    }
}
