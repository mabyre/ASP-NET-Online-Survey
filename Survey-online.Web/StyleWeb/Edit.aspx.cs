using System;
using System.Collections;
using System.Collections.Generic;
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
using System.ComponentModel;
using System.Reflection;
using StyleWebData;
using Sql.Web.Data;
using UserControl.Web.Controls;

public partial class StyleWeb_Edit : System.Web.UI.Page
{
    // Passer le Texte a afficher
    private string Texte
    {
        get
        {
            string sw = ( string )HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ];
            if ( sw == null )
            {
                return "Le texte du control";
            }
            else
            {
                return sw;
            }
        }
        set { HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = value; }
    }

    private StyleWeb styleWebObjet
    {
        get
        {
            if ( ViewState[ "StyleWebObjet" ] == null )
            {
                return null;
            }
            return ( StyleWeb )ViewState[ "StyleWebObjet" ];
        }
        set { ViewState[ "StyleWebObjet" ] = value; }
    }

    // BUG10092009 private static Style primaryStyle = new Style();
    // on ne peut pas utiliser ViewState[] car Style n'est pas marqué comme sérialisable !!!
    static Style _primaryStyle = new Style();
    private Style primaryStyle
    {
        get { return _primaryStyle; }
        set { _primaryStyle = value; }
    }

    // BUG10092009 private static string returnUrl = "";
    private string returnUrl
    {
        get
        {
            if ( ViewState[ "returnUrl" ] == null )
            {
                ViewState[ "returnUrl" ] = "";
            }
            return ( string )ViewState[ "returnUrl" ];
        }
        set { ViewState[ "returnUrl" ] = value; }
    }

    protected override void OnPreRenderComplete( EventArgs e )
    {
        Trace.Warn( "Page:OnPreRenderComplete" );

        Color couleur1 = ColorPickerBackColor.SelectedColor;
        TextBoxBackColor.Text = ColorTranslator.ToHtml( couleur1 );
        primaryStyle.BackColor = couleur1;

        Color couleur2 = ColorPickerBorderColor.SelectedColor;
        TextBoxBorderColor.Text = ColorTranslator.ToHtml( couleur2 );
        primaryStyle.BorderColor = couleur2;

        Color couleur3 = ColorPickerForegroundColor.SelectedColor;
        TextBoxForegroundColor.Text = ColorTranslator.ToHtml( couleur3 );
        primaryStyle.ForeColor = couleur3;

        applyStyleOnPreRenderComplete();

        base.OnPreRenderComplete( e );
    }

    protected void Page_Load( object sender, System.EventArgs e )
    {
        if ( Page.IsPostBack == false )
        {
            LabelTitre.Text = "Edition d'un StyleWeb";
            if ( Request.QueryString[ "Style" ] != null && Request.QueryString[ "Type" ] != null )
            {
                string styleNom = Request.QueryString[ "Style" ].ToString();
                string type = Request.QueryString[ "Type" ].ToString();
                if ( type == TypeStyleWeb.Table )
                {
                    PanelTypeTable.Visible = true;
                }
                string membre = HttpContext.Current.User.Identity.Name;
                styleWebObjet = XmlStyleWebProvider.GetStyleWeb( membre, styleNom, type );
                primaryStyle = StyleWeb.StyleWebToStyle( styleWebObjet );
                LabelTitre.Text += " : " + styleNom + "&nbsp;: " + type;
                LabelApplicable.Visible = styleWebObjet.Applicable == false;
            }

            if ( Request.QueryString[ "ReturnUrl" ] != null )
            {
                returnUrl = Request.QueryString[ "ReturnUrl" ].ToString();
            }

            TextBoxPadding.Text = styleWebObjet.Padding.ToString();
            TextBoxSpacing.Text = styleWebObjet.Spacing.ToString();
            TextBoxWidth.Text = /*styleWebObjet.Width == 0 ? "" :*/ styleWebObjet.Width.ToString();
            TextBoxHeight.Text = /*styleWebObjet.Height == 0 ? "" :*/ styleWebObjet.Height.ToString();
            TextBoxBorderWidth.Text = styleWebObjet.BorderWidth == 0 ? "" : styleWebObjet.BorderWidth.ToString();
            TextBoxFontSize.Text = styleWebObjet.FontSize.ToString();

            ColorsList colors = new ColorsList();

            DropDownListBorderColor.DataSource = colors;
            DropDownListBorderColor.DataBind();
            if ( styleWebObjet.BorderColor != "none" )
            {
                TextBoxBorderColor.Text = styleWebObjet.BorderColor;
                DropDownListBorderColor.SelectedValue = ColorTranslator.FromHtml( styleWebObjet.BorderColor ).Name;
                ColorPickerBorderColor.SelectedColor = ColorTranslator.FromHtml( styleWebObjet.BorderColor );
            }

            DropDownListBackColor.DataSource = colors;
            DropDownListBackColor.DataBind();
            if ( styleWebObjet.BackColor != "none" )
            {
                TextBoxBackColor.Text = styleWebObjet.BackColor;
                DropDownListBackColor.SelectedValue = ColorTranslator.FromHtml( styleWebObjet.BackColor ).Name;
                ColorPickerBackColor.SelectedColor = ColorTranslator.FromHtml( styleWebObjet.BackColor );
            }

            DropDownListForegroundColor.DataSource = colors;
            DropDownListForegroundColor.DataBind();
            if ( styleWebObjet.ForeColor != "none" )
            {
                TextBoxForegroundColor.Text = styleWebObjet.ForeColor;
                DropDownListForegroundColor.SelectedValue = ColorTranslator.FromHtml( styleWebObjet.ForeColor ).Name;
                ColorPickerForegroundColor.SelectedColor = ColorTranslator.FromHtml( styleWebObjet.ForeColor );
            }

            // Add data to the borderStyleList control.
            ListItemCollection styles = new ListItemCollection();
            Type styleType = typeof( BorderStyle );
            foreach ( string s in Enum.GetNames( styleType ) )
            {
                styles.Add( s );
            }
            DropDownListBorderStyle.DataSource = styles;
            DropDownListBorderStyle.DataBind();
            DropDownListBorderStyle.SelectedIndex = styleWebObjet.BorderStyle;

            // Add data to the borderWidthList control.
            ListItemCollection widths = new ListItemCollection();
            for ( int i = 0;i < 46;i++ )
            {
                widths.Add( i.ToString() + "px" );
            }
            DropDownListBorderWidthList.DataSource = widths;
            DropDownListBorderWidthList.DataBind();

            // Add data to the fontNameList control.
            ListItemCollection names = new ListItemCollection();
            foreach ( FontFamily oneFontFamily in FontFamily.Families )
            {
                names.Add( oneFontFamily.Name );
            }
            DropDownListFontName.DataSource = names;
            DropDownListFontName.DataBind();

            // ca ne marche surement pas
            ListItem li = new ListItem( styleWebObjet.FontName );
            if ( DropDownListFontName.Items.Contains( li ) )
            {
                DropDownListFontName.SelectedValue = styleWebObjet.FontName;
            }

            // Add data to the fontSizeList control.
            ListItemCollection fontSizes = new ListItemCollection();
            fontSizes.Add( "Small" );
            fontSizes.Add( "Medium" );
            fontSizes.Add( "Large" );
            fontSizes.Add( "8pt" );
            fontSizes.Add( "10pt" );
            fontSizes.Add( "12pt" );
            fontSizes.Add( "14pt" );
            fontSizes.Add( "16pt" );
            fontSizes.Add( "18pt" );
            fontSizes.Add( "20pt" );
            fontSizes.Add( "24pt" );
            fontSizes.Add( "48pt" );
            DropDownListFontSize.DataSource = fontSizes;
            DropDownListFontSize.DataBind();

            // Font Style
            ListItemCollection stylesF = new ListItemCollection();
            Type styleTypeF = typeof( FontStyle );
            foreach ( string s in Enum.GetNames( styleTypeF ) )
            {
                stylesF.Add( s );
            }
            stylesF.Add( "Overline" ); // qui n'est pas dans FontStyle ???!!
            CheckBoxListFontStyle.DataSource = stylesF;
            CheckBoxListFontStyle.DataBind();

            CheckBoxListFontStyle.Items[ 1 ].Selected = styleWebObjet.Bold;
            CheckBoxListFontStyle.Items[ 2 ].Selected = styleWebObjet.Italic;
            CheckBoxListFontStyle.Items[ 3 ].Selected = styleWebObjet.Underline;
            CheckBoxListFontStyle.Items[ 4 ].Selected = styleWebObjet.Strikeout;
            CheckBoxListFontStyle.Items[ 5 ].Selected = styleWebObjet.Overline;
        }

        // Construire l'objet a chaque fois
        switch ( styleWebObjet.Type )
        {
            case "Label":
                Label lbl = new Label();
                lbl.ID = "ObjetID";
                lbl.Text = Texte;
                PanelObjet.Controls.Add( lbl );
                break;
            case "TextBox":
                TextBox txb = new TextBox();
                txb.ID = "ObjetID";
                txb.Text = Texte;
                PanelObjet.Controls.Add( txb );
                break;
            case "RadioButtonList":
                RadioButtonListStyle rbl = new RadioButtonListStyle();
                rbl.ID = "ObjetID";
                rbl.Items.Add( "article 1" );
                rbl.Items.Add( "article 2" );
                rbl.Items.Add( "article 3" );
                PanelObjet.Controls.Add( rbl );
                break;
            case "CheckBoxList":
                CheckBoxListStyle cbl = new CheckBoxListStyle();
                cbl.ID = "ObjetID";
                cbl.Items.Add( "article 1" );
                cbl.Items.Add( "article 2" );
                cbl.Items.Add( "article 3" );
                PanelObjet.Controls.Add( cbl );
                break;
            case "Table":
                Table tbl = new Table();
                if ( IsPostBack == false )
                {
                    int padding = 0;
                    try
                    {
                        padding = int.Parse( styleWebObjet.Padding );
                        tbl.CellPadding = padding;
                    }
                    catch
                    {
                    }
                    int spacing = 0;
                    try
                    {
                        spacing = int.Parse( styleWebObjet.Padding );
                        tbl.CellSpacing = spacing;
                    }
                    catch
                    {
                    }
                }
                else
                {
                    int padding = 0;
                    try
                    {
                        padding = int.Parse( TextBoxPadding.Text );
                        tbl.CellPadding = padding;
                    }
                    catch
                    {
                    }
                    int spacing = 0;
                    try
                    {
                        spacing = int.Parse( TextBoxSpacing.Text );
                        tbl.CellSpacing = spacing;
                    }
                    catch
                    {
                    }
                }
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.Controls.Add( new LiteralControl( Texte ) );
                row.Cells.Add( cell );
                tbl.Rows.Add( row );
                this.Controls.Add( tbl );
                tbl.ID = "ObjetID";
                PanelObjet.Controls.Add( tbl );
                break;
        }

        Page.Form.DefaultButton = ButtonWidthOk.UniqueID; // Pour donner le focus
    }

    private void applyStyleOnPreRenderComplete()
    {
        WebControl wc = ( WebControl )PanelObjet.FindControl( "ObjetID" );
        wc.ApplyStyle( primaryStyle );

        // On dirait comme un BUG ...
        // si primaryStyle.Width.IsEmpty alors wc.Width n'est pas affecte 
        // et conserve son ancienne valeur !! 
        // un comble non ?
        if ( primaryStyle.Width.IsEmpty )
        {
            wc.Width = new Unit();
        }
        if ( primaryStyle.Height.IsEmpty )
        {
            wc.Height = new Unit();
        }
        if ( primaryStyle.Font.Size.IsEmpty )
        {
            wc.Font.Size = new FontUnit();
        }
    }

    protected void ButtonWidthOk_Click( object sender, EventArgs e )
    {
        string width = TextBoxWidth.Text.Trim();

        // "" equivalent a "vide"
        if ( width == "" )
        {
            primaryStyle.Width = new Unit();
        }
        else
        {
            try
            {
                primaryStyle.Width = new Unit( width );
            }
            catch
            {
            }
        }
    }

    protected void TextBoxWidth_TextChanged( object sender, EventArgs e )
    {
        ButtonWidthOk_Click( sender, e );
        ButtonWidthOk.Click -= new EventHandler( ButtonWidthOk_Click );
    }

    protected void ButtonHeightOk_Click( object sender, EventArgs e )
    {
        string height = TextBoxHeight.Text.Trim();

        // "" equivalent a "vide"
        if ( height == "" )
        {
            primaryStyle.Height = new Unit();
        }
        else
        {
            try
            {
                primaryStyle.Height = new Unit( int.Parse( height ) );
            }
            catch
            {
            }
        }
    }

    protected void ButtonPaddingOk_Click( object sender, EventArgs e )
    {
        int padding = 0;
        try
        {
            padding = int.Parse( TextBoxPadding.Text.Trim() );
            styleWebObjet.Padding = TextBoxPadding.Text.Trim();
        }
        catch
        {
        }
    }

    protected void ButtonSpacingOk_Click( object sender, EventArgs e )
    {
        int spacing = 0;
        try
        {
            spacing = int.Parse( TextBoxSpacing.Text.Trim() );
            styleWebObjet.Spacing = TextBoxSpacing.Text.Trim();
        }
        catch
        {
        }
    }

    protected void TextBoxHeight_TextChanged( object sender, EventArgs e )
    {
        ButtonHeightOk_Click( sender, e );
        ButtonHeightOk.Click -= new EventHandler( ButtonHeightOk_Click );
    }

    public void DropDownListBorderColor_IndexChanged( object sender, System.EventArgs e )
    {
        primaryStyle.BorderColor = Color.FromName( DropDownListBorderColor.SelectedItem.Text );
        TextBoxBorderColor.Text = ColorTranslator.ToHtml( primaryStyle.BorderColor );
        ColorPickerBorderColor.SelectedColor = primaryStyle.BorderColor;
    }

    protected void TextBoxBorderColor_TextChanged( object sender, EventArgs e )
    {
        try
        {
            Color couleur = ColorTranslator.FromHtml( TextBoxBorderColor.Text );
            primaryStyle.BorderColor = couleur;
            ColorPickerBorderColor.SelectedColor = couleur;
        }
        catch
        {
        }
    }

    protected void ButtonBorderColorOk_Click( object sender, EventArgs e )
    {
        try
        {
            Color couleur = ColorTranslator.FromHtml( TextBoxBorderColor.Text );
            primaryStyle.BorderColor = couleur;
            ColorPickerBorderColor.SelectedColor = couleur;
        }
        catch
        {
        }
    }

    public void DropDownListBackColor_IndexChanged( object sender, System.EventArgs e )
    {
        primaryStyle.BackColor = Color.FromName( DropDownListBackColor.SelectedItem.Text );
        TextBoxBackColor.Text = ColorTranslator.ToHtml( primaryStyle.BackColor );
        ColorPickerBackColor.SelectedColor = primaryStyle.BackColor;
    }

    protected void TextBoxBackColor_TextChanged( object sender, EventArgs e )
    {
        try
        {
            Color couleur = ColorTranslator.FromHtml( TextBoxBackColor.Text );
            primaryStyle.BackColor = couleur;
            ColorPickerBackColor.SelectedColor = couleur;
        }
        catch
        {
        }
    }

    protected void ButtonBackColorOk_Click( object sender, EventArgs e )
    {
        try
        {
            Color couleur = ColorTranslator.FromHtml( TextBoxBackColor.Text );
            primaryStyle.BackColor = couleur;
            ColorPickerBackColor.SelectedColor = couleur;
        }
        catch
        {
        }
    }

    public void DropDownListForegroundColor_IndexChanged( object sender, System.EventArgs e )
    {
        primaryStyle.ForeColor = Color.FromName( DropDownListForegroundColor.SelectedItem.Text );
        TextBoxForegroundColor.Text = ColorTranslator.ToHtml( primaryStyle.ForeColor );
        ColorPickerForegroundColor.SelectedColor = primaryStyle.ForeColor;
    }

    protected void TextBoxForegroundColor_TextChanged( object sender, EventArgs e )
    {
        try
        {
            Color couleur = ColorTranslator.FromHtml( TextBoxForegroundColor.Text );
            primaryStyle.ForeColor = couleur;
            ColorPickerForegroundColor.SelectedColor = couleur;
        }
        catch
        {
        }
    }

    protected void ButtonForegroundColorOk_Click( object sender, EventArgs e )
    {
        try
        {
            Color couleur = ColorTranslator.FromHtml( TextBoxForegroundColor.Text );
            primaryStyle.ForeColor = couleur;
            ColorPickerForegroundColor.SelectedColor = couleur;
        }
        catch
        {
        }
    }

    public void DropDownListBorderStyle_IndexChanged( object sender, System.EventArgs e )
    {
        primaryStyle.BorderStyle = ( BorderStyle )Enum.Parse( typeof( BorderStyle ), DropDownListBorderStyle.SelectedItem.Text );
    }

    public void DropDownListBorderWidthList_SelectedIndexChanged( object sender, System.EventArgs e )
    {
        primaryStyle.BorderWidth = Unit.Parse( DropDownListBorderWidthList.SelectedItem.Text );
    }

    public void DropDownListFontName_SelectedIndexChanged( object sender, System.EventArgs e )
    {
        primaryStyle.Font.Name = DropDownListFontName.SelectedItem.Text;
    }

    public void DropDownListFontSize_SelectedIndexChanged( object sender, System.EventArgs e )
    {
        primaryStyle.Font.Size = FontUnit.Parse( DropDownListFontSize.SelectedItem.Text );
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
                        primaryStyle.Font.Bold = true;
                        break;
                    case "Underline":
                        primaryStyle.Font.Underline = true;
                        break;
                    case "Strikeout":
                        primaryStyle.Font.Strikeout = true;
                        break;
                    case "Italic":
                        primaryStyle.Font.Italic = true;
                        break;
                    case "Overline":
                        primaryStyle.Font.Overline = true;
                        break;
                }
            }
            else
            {
                string s = li.Value;
                switch ( s )
                {
                    case "Bold":
                        primaryStyle.Font.Bold = false;
                        break;
                    case "Underline":
                        primaryStyle.Font.Underline = false;
                        break;
                    case "Strikeout":
                        primaryStyle.Font.Strikeout = false;
                        break;
                    case "Italic":
                        primaryStyle.Font.Italic = false;
                        break;
                    case "Overline":
                        primaryStyle.Font.Overline = false;
                        break;
                }
            }
        }
    }

    protected void ButtonBorderWidthOk_Click( object sender, EventArgs e )
    {
        try
        {
            primaryStyle.BorderWidth = Unit.Parse( TextBoxBorderWidth.Text );
        }
        catch
        {
        }
    }

    protected void TextBoxBorderWidth_TextChanged( object sender, EventArgs e )
    {
        try
        {
            primaryStyle.BorderWidth = Unit.Parse( TextBoxBorderWidth.Text );
        }
        catch
        {
        }
    }

    protected void ButtonFontSizeOk_Click( object sender, EventArgs e )
    {
        ButtonHeightOk_Click( sender, e );
        ButtonHeightOk.Click -= new EventHandler( ButtonHeightOk_Click );

        if ( TextBoxFontSize.Text.Trim() == "" || TextBoxFontSize.Text.Trim() == "0" )
        {
            primaryStyle.Font.Size = new FontUnit();
        }
        else
        {
            try
            {
                primaryStyle.Font.Size = FontUnit.Parse( TextBoxFontSize.Text );
            }
            catch
            {
            }
        }
    }

    protected void TextBoxFontSize_TextChanged( object sender, EventArgs e )
    {
        ButtonFontSizeOk_Click( sender, e );
        ButtonFontSizeOk.Click -= new EventHandler( ButtonFontSizeOk_Click );
    }

    protected void ButtonSauver_Click( object sender, EventArgs e )
    {
        StyleWeb.StyleToStyleWeb( styleWebObjet, primaryStyle );
        string membre = HttpContext.Current.User.Identity.Name;
        styleWebObjet.Applicable = true;

        XmlStyleWebProvider.Update( membre, styleWebObjet );

        Response.Redirect( returnUrl );
    }

    protected void ButtonSupprimer_Click( object sender, EventArgs e )
    {
        styleWebObjet.Applicable = false;
        string membre = HttpContext.Current.User.Identity.Name;

        XmlStyleWebProvider.Update( membre, styleWebObjet );

        Response.Redirect( returnUrl );
    }    

    protected void ButtonRetour_Click( object sender, EventArgs e )
    {
        Response.Redirect( returnUrl );
    }
}
