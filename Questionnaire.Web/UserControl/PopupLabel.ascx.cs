
//
// Un Label qui s'affiche lorsque l'on coche la CheckBox
// C'est plus simple que dans PopupTextBox 
// on applique le script directement au Label 
// mais j'ai as envie de modifier PopupTextBox 
// ca ete trop dur a vaider
// 

#region Using
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
using System.Text;
#endregion

public partial class PopupLabel : System.Web.UI.UserControl
{
    private string _labelCheckBox = "Texte devant la CheckBox";
    public string LabelCheckBox
    {
        get { return _labelCheckBox; }
        set { _labelCheckBox = value; }
    }

    private string _LabelChecboxWidth = "";
    public string LabelChecboxWidth
    {
        get { return _LabelChecboxWidth; }
        set { _LabelChecboxWidth = value; }
    }

    private string _CssClassLabelCheckBox = "";
    public string CssClassLabelCheckBox
    {
        get { return _CssClassLabelCheckBox; }
        set { _CssClassLabelCheckBox = value; }
    }

    private string _labelText = "Texte du Label";
    public string LabelText
    {
        get { return _labelText; }
        set { _labelText = value; }
    }


    private string _labelWidth = "";
    public string LabelWidth
    {
        get { return _labelWidth; }
        set { _labelWidth = value; }
    }

    private string _CssClassLabel = "";
    public string CssClassLabel
    {
        get { return _CssClassLabel; }
        set { _CssClassLabel = value; }
    }


    protected override void Render( HtmlTextWriter writer )
    {
        Response.Write( "<!-- Render PopupTextBox -->" );
        Response.Write( Script() );
        base.Render( writer );
    }

    protected void Page_Load( object sender, EventArgs e )
    {
        LabelChecbox.Text = LabelCheckBox;
        LabelChecbox.Width = new Unit( LabelChecboxWidth );
        LabelChecbox.CssClass = CssClassLabelCheckBox;
        PopupLabelText.Text = LabelText;
        PopupLabelText.CssClass = CssClassLabel;
        TdPopupLabel.Width = LabelWidth;
        CheckBoxToggleLabel.Attributes.Add( "onclick", "javascript:void(ToggleTextBox" + PopupLabelText.ClientID.ToString() + "());" );
    }

    private string Script()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine( "<script type=\"text/javascript\">" );
        sb.AppendLine( "function ToggleTextBox" + PopupLabelText.ClientID.ToString() + "()" );
        sb.AppendLine( "{" );
        sb.AppendLine( "var element = document.getElementById(\"" + PopupLabelText.ClientID.ToString() + "\");" );
        sb.AppendLine( "if (element.style.display == \"none\")" );
        sb.AppendLine( "element.style.display = \"block\";" );
        sb.AppendLine( "else" );
        sb.AppendLine( "element.style.display = \"none\";" );
        sb.AppendLine( "}" );
        sb.AppendLine( "</script>" );
        return sb.ToString();
    }
}
