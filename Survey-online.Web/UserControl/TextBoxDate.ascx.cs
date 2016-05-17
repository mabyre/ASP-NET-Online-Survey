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

public partial class TextBoxDate : System.Web.UI.UserControl
{
    private string _Label = "";
    public string Label
    {
        get { return _Label; }
        set { _Label = value; }
    }

    private string _Date = Tools.DateTimeNull.ToShortDateString();
    public string Date
    {
        get { return _Date; }
        set { _Date = value; }
    }

    private bool _Enabled = true;
    public bool Enabled
    {
        get { return _Enabled; }
        set { _Enabled = value; }
    }

    protected void Page_Load( object sender, EventArgs e )
    {
        if ( Label != string.Empty )
        {
            LabelTextBoxDate.Text = Label;
        }
        TextBoxDateText.Enabled = Enabled;
    }

    protected void TextBoxDateText_TextChanged( object sender, EventArgs e )
    {
        try
        {
            Date = DateTime.Parse( TextBoxDateText.Text ).ToShortDateString();
        }
        catch
        {
            Date = Tools.DateTimeNull.ToShortDateString();
        }
    }
}
