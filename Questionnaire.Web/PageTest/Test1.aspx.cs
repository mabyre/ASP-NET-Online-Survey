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

public partial class PageTest_Test1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        TextBoxMaTextBox.Width = new Unit( 350 );
        TextBoxMaTextBox.TextMode = TextBoxMode.MultiLine;
        TextBoxMaTextBox.Rows = 10;

    }

    protected void BulletedList1_Click( object sender, BulletedListEventArgs e )
    {
        int tte = 1;
    }
    protected void BulletedList1_TextChanged( object sender, EventArgs e )
    {
        int tte = 1;

    }
    protected void RadioButtonList1_TextChanged( object sender, EventArgs e )
    {

        int tte = 1;
    }
}
