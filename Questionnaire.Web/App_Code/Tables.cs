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

/// <summary>
/// Creation de Table dynamiquement
/// </summary>
public class Tables
{
    public Tables()
    {
        //
        // TODO : ajoutez ici la logique du constructeur
        //
    }

    public static Table CreateTableForLabel( string label, string cellCssClass, string labelCssClass )
    {
        Table table = new Table();
        TableCell cell = new TableCell();
        TableRow row = new TableRow();

        cell.Text = string.Format( "<label class=\"{0}\">{1}</label>", labelCssClass, label );
        cell.CssClass = cellCssClass;

        row.Cells.Add( cell );
        table.Rows.Add( row );
        return table;
    }

    public static void CreateRowForLabel( ref Table table, string label, string cellCssClass, string labelCssClass )
    {
        TableCell cell = new TableCell();
        TableRow row = new TableRow();

        cell.Text = string.Format( "<label class=\"{0}\">{1}</label>", labelCssClass, label );
        cell.CssClass = cellCssClass;

        row.Cells.Add( cell );
        table.Rows.Add( row );
    }
}
