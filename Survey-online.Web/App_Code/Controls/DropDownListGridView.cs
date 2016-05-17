/*
** Lorsqu'une DropDownList est dans une GridView par exemple
** recuperer une Valeur associee a la DropDownList
**
*/

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Sql.Web.Data;

namespace UserControl.Web.Controls
{
    public class DropDownListGridView : DropDownList
    {
        public string Valeur
        {
            get { return ( string )ViewState[ "DropDownListGridViewValeur" ]; }
            set { ViewState[ "DropDownListGridViewValeur" ] = value; }
        }
    }
}
