using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

/// <summary>
/// This code is totally free and open. You may reuse it in any way you wish.
/// </summary>

public partial class ColorPicker : System.Web.UI.UserControl, IPostBackEventHandler
{
    public enum Size
    {
        Large = 256,
        Medium = 128,
        Small = 64
    }

    public enum Resolution
    {
        Fine = 32,
        Medium = 16,
        Coarse = 4
    }

    private Resolution Fineness
    {
        get
        {
            if ( ViewState[ "Fineness" ] == null )
            {
                ViewState[ "Fineness" ] = Resolution.Coarse;
            }
            return ( Resolution )ViewState[ "Fineness" ];
        }
        set { ViewState[ "Fineness" ] = value; }
    }

    private Size BoardSize
    {
        get
        {
            if ( ViewState[ "BoardSize" ] == null )
            {
                ViewState[ "BoardSize" ] = Size.Small;
            }
            return ( Size )ViewState[ "BoardSize" ];
        }
        set { ViewState[ "BoardSize" ] = value; }
    }


    protected override void Render( HtmlTextWriter writer )
    {
        Trace.Warn( "ColorPicker:Render()" );
        drawTable();
        base.Render( writer );
    }

    protected override void OnInit( EventArgs e )
    {
        if ( Page.IsPostBack == false )
        {
            DropDownListResolution.SelectedValue = Fineness.ToString();
            DropDownListSize.SelectedValue = BoardSize.ToString();
        }
    }

    private void drawTable()
    {
        tblShow.Width = ( int )BoardSize;
        tblPicker.Width = ( int )BoardSize;
        int increment = ( ( int )BoardSize / ( int )Fineness );

        cellPal1.Attributes.Add( "onClick", Page.ClientScript.GetPostBackEventReference( this, "pal1" ) );
        cellPal2.Attributes.Add( "onClick", Page.ClientScript.GetPostBackEventReference( this, "pal2" ) );
        cellPal3.Attributes.Add( "onClick", Page.ClientScript.GetPostBackEventReference( this, "pal3" ) );
        cellPal4.Attributes.Add( "onClick", Page.ClientScript.GetPostBackEventReference( this, "pal4" ) );
        cellPal5.Attributes.Add( "onClick", Page.ClientScript.GetPostBackEventReference( this, "pal5" ) );

        string tmpCol;

        for ( int x = 0;x <= ( int )BoardSize;x += increment )
        {
            TableRow tr = new TableRow();
            for ( int y = 0;y <= ( int )BoardSize;y += increment )
            {
                tmpCol = getColor( x, y );
                TableCell tc = new TableCell();
                tc.Attributes.Add( "bgcolor", tmpCol );
                tc.Attributes.Add( "width", increment.ToString() );
                tc.Attributes.Add( "height", increment.ToString() );
                tc.Attributes.Add( "onclick", Page.ClientScript.GetPostBackEventReference( this, tmpCol ) );
                tr.Cells.Add( tc );
            }

            tblPicker.Rows.Add( tr );
        }
    }

    private string Palette
    {
        get { return ( string )ViewState[ "Palette" ]; }
        set { ViewState[ "Palette" ] = value; }
    }

    public Color SelectedColor
    {
        get
        {
            if ( ViewState[ "SelectedColor" ] == null )
            {
                ViewState[ "SelectedColor" ] = Color.White;
            }
            return ( Color )ViewState[ "SelectedColor" ];
        }
        set 
        { 
            ViewState[ "SelectedColor" ] = value;
            cellPreview.BackColor = value;
            cellPreview.ForeColor = Color.FromArgb( ~cellPreview.BackColor.ToArgb() );
            cellPreview.Text = Colors.ToHtml( value );
        }
    }

    private string getColor( int x, int y )
    {
        int xmod = ( int )( ( 255F / ( float )BoardSize ) * ( float )x );
        int ymod = ( int )( ( 255F / ( float )BoardSize ) * ( float )y );

        int r = 0, g = 0, b = 0;

        switch ( Palette )
        {
            case "pal1":
                r = xmod;
                g = ymod;
                break;
            case "pal2":
                g = xmod;
                b = ymod;
                break;
            case "pal3":
                r = xmod;
                b = ymod;
                break;
            case "pal4":
                r = ymod;
                g = xmod;
                b = ymod;
                break;
            case "pal5":
                r = ymod;
                g = ymod;
                b = ymod;
                break;
            default:
                r = xmod;
                g = ymod;
                break;
        }

        return ColorTranslator.ToHtml( Color.FromArgb( r, g, b ) );
    }

    public void RaisePostBackEvent( string eventArgument )
    {
        Trace.Warn( "ColorPicker:RaisePostBackEvent" );
        switch ( eventArgument )
        {
            case "pal1":
            case "pal2":
            case "pal3":
            case "pal4":
            case "pal5":
                Palette = eventArgument;
                break;
            default:
                try
                {
                    cellPreview.BackColor = ColorTranslator.FromHtml( eventArgument );
                    cellPreview.ForeColor = Color.FromArgb( ~cellPreview.BackColor.ToArgb() );
                    this.SelectedColor = cellPreview.BackColor;
                    cellPreview.Text = eventArgument;
                }
                catch ( Exception )
                {

                }
                break;
        }

    }

    protected void DropDownListResolution_SelectedIndexChanged( object sender, EventArgs e )
    {
        switch ( DropDownListResolution.SelectedValue )
        {
            case "Fine":
                Fineness = Resolution.Fine;
                break;

            case "Medium":
                Fineness = Resolution.Medium;
                break;

            case "Coarse":
                Fineness = Resolution.Coarse;
                break;
        }
    }

    protected void DropDownListSize_SelectedIndexChanged( object sender, EventArgs e )
    {
        switch ( DropDownListSize.SelectedValue )
        {
            case "Large":
                BoardSize = Size.Large;
                break;
            case "Medium":
                BoardSize = Size.Medium;
                break;
            case "Small":
                BoardSize = Size.Small;
                break;
        }
    }
}
