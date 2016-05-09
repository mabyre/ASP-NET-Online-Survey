
//
// On ne peut pas faire TextBoxText.Visible = false sinon l'objet n'existe pas dans la page
// et ToggleTextBox() plante !
// 
// L' objet <asp:TextBox> ne possede pas la propriete style="display:none"
// donc on l'applique au Td
//
// On est oblige d'injecter le script pour particulariser le nom de la fonction :
// function ToggleTextBox...() sinon si plusieur control PopupTextBox.ascx sont sur 
// la meme page, toutes les case a cocher execute la meme fonction ToggleTextBox() 
// et n'agissent que sur le dernier TdTextBox
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

public partial class PopupTextBox : System.Web.UI.UserControl
{
    private string _labelCheckBox = "";
    public string LabelCheckBox
    {
        get { return _labelCheckBox; }
        set { _labelCheckBox = value; }
    }

    private string _textBoxWidth = "";
    public string TextBoxWidth
    {
        get { return _textBoxWidth; }
        set { _textBoxWidth = value; }
    }

    private int _textBoxRows = 0;
    public int TextBoxRows
    {
        get { return _textBoxRows; }
        set { _textBoxRows = value; }
    }

    protected override void Render( HtmlTextWriter writer )
    {
        Response.Write( "<!-- Render PopupTextBox -->" );
        Response.Write( Script() );
        base.Render( writer );
    }

    protected void Page_Load( object sender, EventArgs e )
    {
        //if ( IsPostBack == false )
        //{
            LabelChecbox.Text = LabelCheckBox;
            TextBoxText.Width = new Unit( TextBoxWidth );
            if ( TextBoxRows > 1 )
            {
                TextBoxText.Rows = TextBoxRows;
                TextBoxText.TextMode = TextBoxMode.MultiLine;
                
                // pas utile ? c'est difficile question de choix ...
                //Unit tdLabelChecboxHeight = new Unit( TdLabelChecbox.Height );
                //int height = ( int )tdLabelChecboxHeight.Value * TextBoxRows;
                //TdLabelChecbox.Height = height.ToString() + "px";
            }

            CheckBoxToggleTextBox.Attributes.Add( "onclick", "javascript:void(ToggleTextBox" + TdTextBox.ClientID.ToString() + "());" );
        //}
    }

    private string Script()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine( "<script type=\"text/javascript\">" );
        sb.AppendLine( "function ToggleTextBox" + TdTextBox.ClientID.ToString() + "()" );
        sb.AppendLine( "{" );
        sb.AppendLine( "var element = document.getElementById(\"" + TdTextBox.ClientID.ToString() + "\");" );
        sb.AppendLine( "if (element.style.display == \"none\")" );
        sb.AppendLine( "element.style.display = \"block\";" );
        sb.AppendLine( "else" );
        sb.AppendLine( "element.style.display = \"none\";" );
        sb.AppendLine( "}" );
        sb.AppendLine( "</script>" );
        return sb.ToString();
    }
}
