
//
// Issu du travail sur les couleurs pour l'editeur de StyleWeb
//

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Web.Caching;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Security.Permissions;
using System.Drawing;

// List de toutes les Couleurs connues + systeme
public class KnownColors : ArrayList
{
    public KnownColors()
    {
        System.Array colorsArray = Enum.GetValues( typeof( KnownColor ) );
        KnownColor[] allColors = new KnownColor[ colorsArray.Length ];
        Array.Copy( colorsArray, allColors, colorsArray.Length );
        this.Capacity = colorsArray.Length;
        for ( int i = 0;i < allColors.Length;i++ )
        {
            this.Add( allColors[ i ].ToString() );
        }
    }
}

public class ColorsList : ArrayList
{
    public ColorsList()
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
                catch
                {
                }
            }
        }
    }
}

public class Colors
{
    public static string ToHtml( Color couleur )
    {
        string sR = Convert.ToString( couleur.R, 16 ).ToUpper();
        string sG = Convert.ToString( couleur.G, 16 ).ToUpper();
        string sB = Convert.ToString( couleur.B, 16 ).ToUpper();
        string html = "#" + sR.PadLeft( 2, '0' ) + sG.PadLeft( 2, '0' ) + sB.PadLeft( 2, '0' );
        return html;
    }
}