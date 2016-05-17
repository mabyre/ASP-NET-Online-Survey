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
using System.Drawing;

public partial class PageTest_RadioButtonList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       // RadioButtonList1.Items
       // RadioButtonList1.RepeatDirection = RepeatDirection.Horizontal

        if ( Page.IsPostBack == false )
        {
            // Add data to the fontNameList control.
            ListItemCollection names = new ListItemCollection();
            foreach ( FontFamily oneFontFamily in FontFamily.Families )
            {
                names.Add( oneFontFamily.Name );
            }
            DropDownListFont.DataSource = names;
            DropDownListFont.DataBind();
            ListItem li = new ListItem();
        }

        int i = 123;
    }

    protected void TextBoxRepeatColums_TextChanged( object sender, EventArgs e )
    {
        RadioButtonList1.RepeatColumns = int.Parse( TextBoxRepeatColums.Text );
    }
    protected void DropDownList1_SelectedIndexChanged( object sender, EventArgs e )
    {
        if ( DropDownList1.SelectedValue == "Horizontal" )
            RadioButtonList1.RepeatDirection = RepeatDirection.Horizontal;
        else
            RadioButtonList1.RepeatDirection = RepeatDirection.Vertical;

    }
    protected void DropDownListFont_SelectedIndexChanged( object sender, EventArgs e )
    {
        RadioButtonList1.Font.Name = DropDownListFont.SelectedValue;
        RadioButtonListStyle1.Font.Name = DropDownListFont.SelectedValue;
        Label1.Font.Name = DropDownListFont.SelectedValue;
        Style style = new Style();
        style.Font.Name = DropDownListFont.SelectedValue;
        RadioButtonList1.ApplyStyle( style );
        DivRadioButtonList.Style.Add( "font-family", DropDownListFont.SelectedValue );
    }

    protected void DropDownListLayout_SelectedIndexChanged( object sender, EventArgs e )
    {
        if ( DropDownListLayout.SelectedValue == "Flow" )
        {
            RadioButtonList1.RepeatLayout = RepeatLayout.Flow;
        }
        else
        {
            RadioButtonList1.RepeatLayout = RepeatLayout.Table;
        }
    }

    protected void CheckBoxUnderline_CheckedChanged( object sender, EventArgs e )
    {
        RadioButtonList1.Font.Underline = CheckBoxUnderline.Checked;
        RadioButtonListStyle1.Font.Underline = CheckBoxUnderline.Checked;
        Label1.Font.Underline = CheckBoxUnderline.Checked;
    }
}
