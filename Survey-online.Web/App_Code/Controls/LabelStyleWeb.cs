/*
** Label avec un StyleWeb
** C'est pas au point ca merde je sais pas pourquoi mais je m'en vais ne pas l'utiliser !!!!!!!!!!
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
using System.Drawing;
using Sql.Web.Data;
using StyleWebData;

namespace UserControl.Web.Controls
{
    public class LabelStyleWeb : Label
    {
        string _StyleWeb;
        public string StyleWeb
        {
            get { return _StyleWeb; }
            set { _StyleWeb = value; }
        }

        private void Construire()
        {
            string membre = HttpContext.Current.User.Identity.Name;
            StyleWeb sw = XmlStyleWebProvider.GetStyleWeb( membre, StyleWeb, TypeStyleWeb.Label );
            Style st = Sql.Web.Data.StyleWeb.StyleWebToStyle( sw );
            this.ApplyStyle( st );
        }
        
        override protected void OnLoad( EventArgs e ) 
        {
            base.OnLoad( e );
            
            if ( Page.IsPostBack == false && Visible ) 
            {
                EnsureChildControls();
                Construire();
            }
        }

        /* Constructeurs */
        public LabelStyleWeb() : base()
        {
        }

        public LabelStyleWeb( string nom ) : base() 
        {
            _StyleWeb = nom;
        }
    }
}
