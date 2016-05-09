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
using Sql.Web.Data;

public partial class PageTest_PageTestAjax : System.Web.UI.Page
{
    private string _Text = "";
    public string Text
    {
        get { return _Text; }
        set { _Text = value; }
    }

    private string _Date = Tools.DateTimeNull.ToShortDateString();
    public string Date
    {
        get { return _Date; }
        set { _Date = value; }
    }

    protected void Page_Load( object sender, EventArgs e )
    {
        // Ce code ne gene pas mais ne sert a rien
        CultureInfo culture = CultureInfo.CreateSpecificCulture( "fr-FR" );
        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetDynamicContent( string contextKey )
    {
        return default( string );
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

    protected void ButtonOk_Click( object sender, EventArgs e )
    {
        //string date = TextBoxDate.Date.ToString();

        //TextBoxDate textBoxDate = ( TextBoxDate )LoadControl( "~/UserControl/TextBoxDate.ascx" );
        //textBoxDate.Text = "AAA";

        //Label label = ( Label )textBoxDate.FindControl( "LabelTextBoxDate" );
        //StyleWeb.ApplyStyleWeb( "ReponseTextuelleLabel", TypeStyleWeb.Label, label );

        //TextBox textboxD = ( TextBox )textBoxDate.FindControl( "TextBoxDateText" );
        //StyleWeb.ApplyStyleWeb( "ReponseTextuelleTextBox", TypeStyleWeb.TextBox, textboxD );

        int i = 123;
    }

    protected void ButtonLoadTextBoxDate_Click( object sender, EventArgs e )
    {
        TextBoxDate textBoxDate = ( TextBoxDate )LoadControl( "~/UserControl/TextBoxDate.ascx" );
        textBoxDate.Label = "Coucou c'est moi : ";
        PanelTextBoxDate.Controls.Add( textBoxDate );
    }

}
