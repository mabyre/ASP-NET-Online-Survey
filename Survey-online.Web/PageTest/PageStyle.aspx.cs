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
    // List de toutes les Couleurs connues
    private class KnownColors : ArrayList
    {
        public KnownColors()
        {
            System.Array colorsArray = Enum.GetValues( typeof( KnownColor ) );
            KnownColor[] allColors = new KnownColor[ colorsArray.Length ];
            Array.Copy( colorsArray, allColors, colorsArray.Length );
            this.Capacity = colorsArray.Length;
            for ( int i = 0;i < allColors.Length;i++ )
            {
                this.Add( /*Color.FromName(*/ allColors[ i ].ToString() /*)*/ );
            }
        }
    }

    private class Colors : ArrayList
    {
        public Colors()
        {
            Type oT = System.Drawing.Color.Beige.GetType();
            System.Reflection.PropertyInfo[] oProperties = oT.GetProperties();

            foreach ( PropertyInfo oP in oProperties )
            {
                if ( oP.CanRead )
                {
                    try
                    {
                        Color color = ( Color )oP.GetValue( oT, null );
                        this.Add( color.Name );
                    }
                    catch ( Exception ex )
                    {
                    }
                }
            }
        }
    }

    private Style primaryStyle = new Style();

    protected override void OnPreRenderComplete( EventArgs e )
    {
        Trace.Warn( "Page:OnPreRenderComplete" );
        TextBoxBackColor.Text = ColorTranslator.ToHtml( ColorPickerBackColor.SelectedColor );

        Color color = ColorPickerBackColor.SelectedColor;
        primaryStyle.BackColor = color;
        applyStyle();

        base.OnPreRenderComplete( e );
    }

    // Afficher toutes les couleurs dans une jolie table
    private void DrawColors()
    {
        // Obtention du Type Color suivant une couleur au hazard
        Type oT = System.Drawing.Color.Beige.GetType();
        
        // Obtention des propriétés du type dans un tableau
        System.Reflection.PropertyInfo[] oProperties = oT.GetProperties();
        //System.Reflection.PropertyInfo oP;
        
        //Color[] ColorCollection;
        foreach ( PropertyInfo oP in oProperties )
        {
            if ( oP.CanRead )
            {
                try
                {
                    TableRow Row = new TableRow();
                    Color Color = ( Color )oP.GetValue( oT, null );
                    TableCell CelluleCouleur = new TableCell();
                    CelluleCouleur.BackColor = Color;
                    CelluleCouleur.Width = Unit.Pixel( 200 );
                    TableCell CelluleNom = new TableCell();
                    CelluleNom.Text = Color.Name;
                    CelluleNom.Width = Unit.Pixel( 200 );
                    Row.Cells.Add( CelluleCouleur );
                    Row.Cells.Add( CelluleNom );
                    TableCouleurs.Rows.Add( Row );
                }
                catch ( Exception ex )
                {
                }
            }
        }
    }

    protected void Page_Load( object sender, System.EventArgs e )
    {
        if ( Page.IsPostBack == false )
        {
            DrawColors();

            // Add data to the borderColorList, 
            // backColorList, and foreColorList controls.
            //ListItemCollection colors = new ListItemCollection();
            //colors.Add( Color.Black.Name );
            //colors.Add( Color.Blue.Name );
            //colors.Add( Color.Green.Name );
            //colors.Add( Color.Orange.Name );
            //colors.Add( Color.Purple.Name );
            //colors.Add( Color.Red.Name );
            //colors.Add( Color.White.Name );
            //colors.Add( Color.Yellow.Name );
            Colors colors = new Colors();

            DropDownListBorderColor.DataSource = colors;
            DropDownListBorderColor.DataBind();
            DropDownListBackColor.DataSource = colors;
            DropDownListBackColor.DataBind();
            DropDownListForegroundColor.DataSource = colors;
            DropDownListForegroundColor.DataBind();

            // Add data to the borderStyleList control.
            ListItemCollection styles = new ListItemCollection();
            Type styleType = typeof( BorderStyle );
            foreach ( string s in Enum.GetNames( styleType ) )
            {
                styles.Add( s );
            }
            borderStyleList.DataSource = styles;
            borderStyleList.DataBind();

            // Add data to the borderWidthList control.
            ListItemCollection widths = new ListItemCollection();
            for ( int i = 0;i < 46;i++ )
            {
                widths.Add( i.ToString() + "px" );
            }
            borderWidthList.DataSource = widths;
            borderWidthList.DataBind();

            // Add data to the fontNameList control.
            ListItemCollection names = new ListItemCollection();
            names.Add( "Arial" );
            names.Add( "Courier" );
            names.Add( "Garamond" );
            names.Add( "Time New Roman" );
            names.Add( "Verdana" );
            fontNameList.DataSource = names;
            fontNameList.DataBind();

            // Add data to the fontSizeList control.
            ListItemCollection fontSizes = new ListItemCollection();
            fontSizes.Add( "Small" );
            fontSizes.Add( "Medium" );
            fontSizes.Add( "Large" );
            fontSizes.Add( "10pt" );
            fontSizes.Add( "14pt" );
            fontSizes.Add( "20pt" );
            fontSizeList.DataSource = fontSizes;
            fontSizeList.DataBind();

            // Font Style
            ListItemCollection stylesF = new ListItemCollection();
            Type styleTypeF = typeof( FontStyle );
            foreach ( string s in Enum.GetNames( styleTypeF ) )
            {
                stylesF.Add( s );
            }
            DropDownListStyle.DataSource = stylesF;
            DropDownListStyle.DataBind();
            CheckBoxListFontStyle.DataSource = stylesF;
            CheckBoxListFontStyle.DataBind();

            // Set primaryStyle as the style for each control.
            applyStyle();
        }

        string pageIsCallBack = "";
        if ( Page.IsCallback )
        {
            pageIsCallBack = "Page.IsCallback == true";
        }
        else
        {
            pageIsCallBack = "Page.IsCallback == false";
        }
        string pageIsPostBack = "";
        if ( Page.IsPostBack )
        {
            pageIsPostBack = "Page.IsPostBack == true";
        }
        else
        {
            pageIsPostBack = "Page.IsPostBack == false";
        }

        Trace.Warn( "Page_Load : " + pageIsCallBack );
        Trace.Warn( "Page_Load : " + pageIsPostBack );
    }

    private void applyStyle()
    {
        Label1.ApplyStyle( primaryStyle );
        ListBox1.ApplyStyle( primaryStyle );
        Button1.ApplyStyle( primaryStyle );
        Table1.ApplyStyle( primaryStyle );
        TextBox1.ApplyStyle( primaryStyle );
    }

    public void ChangeBorderColor( object sender, System.EventArgs e )
    {
        primaryStyle.BorderColor = Color.FromName( DropDownListBorderColor.SelectedItem.Text );
        applyStyle();
    }

    public void ChangeBackColor( object sender, System.EventArgs e )
    {
        primaryStyle.BackColor = Color.FromName( DropDownListBackColor.SelectedItem.Text );
        applyStyle();
    }

    public void ChangeForeColor( object sender, System.EventArgs e )
    {
        primaryStyle.ForeColor = Color.FromName( DropDownListForegroundColor.SelectedItem.Text );
        applyStyle();
    }

    public void ChangeBorderStyle( object sender, System.EventArgs e )
    {
        primaryStyle.BorderStyle = ( BorderStyle )Enum.Parse( typeof( BorderStyle ),  borderStyleList.SelectedItem.Text );
        applyStyle();
    }

    public void ChangeBorderWidth( object sender, System.EventArgs e )
    {
        primaryStyle.BorderWidth = Unit.Parse( borderWidthList.SelectedItem.Text );
        applyStyle();
    }

    public void ChangeFont( object sender, System.EventArgs e )
    {
        primaryStyle.Font.Name = fontNameList.SelectedItem.Text;
        applyStyle();
    }

    public void ChangeFontSize( object sender, System.EventArgs e )
    {
        primaryStyle.Font.Size = FontUnit.Parse( fontSizeList.SelectedItem.Text );
        applyStyle();
    }

    protected void DropDownListStyle_SelectedIndexChanged( object sender, EventArgs e )
    {
        Label1.Font.Bold = DropDownListStyle.SelectedItem.Text == "Bold";
        Label1.Font.Underline = DropDownListStyle.SelectedItem.Text == "Underline";
        Label1.Font.Strikeout = DropDownListStyle.SelectedItem.Text == "Strikeout";
        applyStyle();
    }

    protected void CheckBoxListFontStyle_SelectedIndexChanged( object sender, EventArgs e )
    {
        foreach ( ListItem li in CheckBoxListFontStyle.Items )
        {
            if ( li.Selected )
            {
                string s = li.Value;
                switch ( s )
                {
                    case "Bold":
                        Label1.Font.Bold = true;
                        break;
                    case "Underline":
                        Label1.Font.Underline = true;
                        break;
                    case "Strikeout":
                        Label1.Font.Strikeout = true;
                        break;
                    case "Italic":
                        Label1.Font.Italic = true;
                        break;
                }
            }
            else
            {
                string s = li.Value;
                switch ( s )
                {
                    case "Bold":
                        Label1.Font.Bold = false;
                        break;
                    case "Underline":
                        Label1.Font.Underline = false;
                        break;
                    case "Strikeout":
                        Label1.Font.Strikeout = false;
                        break;
                    case "Italic":
                        Label1.Font.Italic = false;
                        break;
                }
            }
        }
    }

    protected void ButtonPaletteCouleurs_Click( object sender, EventArgs e )
    {
        foreach ( Color c in TypeDescriptor.GetConverter( typeof( Color ) ).GetStandardValues() )
        {
            LabelColors.Text += TypeDescriptor.GetConverter( c ).ConvertToString( c ) + "<br />";
        }
    }
}
