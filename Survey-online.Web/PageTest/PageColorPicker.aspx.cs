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
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

public partial class PageTest_PageStyle : System.Web.UI.Page
{
    protected override void OnPreRenderComplete( EventArgs e )
    {
        TextBoxMessage.Text = ColorTranslator.ToHtml( ColorPicker.SelectedColor );
        TextBoxColor.Text = ColorTranslator.ToHtml( ColorPicker.SelectedColor );
        base.OnPreRenderComplete( e );
    }

    protected void Page_Load( object sender, System.EventArgs e )
    {
        if ( Page.IsPostBack == false )
        {
        }
    }

}
