/*
** Pour l'admin : toutes les sections
** Pour l'utilisateur : que les sections qui existent pour lui
** Afficher le tout dans une DropDownList
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
    public class SectionExclues : ArrayList
    {
        // Facilite pour l'Admin, section colorees
        public static ArrayList AdminSections()
        {
            ArrayList al = new ArrayList();
            al.Add( "PageAide" );
            al.Add( "PageAideAdmin" );
            al.Add( "PageLogin" );
            al.Add( "PageLoginContact" );
            al.Add( "PageAuthentifie" );
            // BUG171020090002 al.Add( "PageEnregistrement" );
            return al;
        }
    }

    public class DropDownListWebContentSection : DropDownList
    {
        string _selected = null;

        string _defaultText = "Selectionner une Section";
        public string Default
        {
            get { return _defaultText; }
            set { _defaultText = value; }
        }

        private void ConstruireItems()
        {
            Items.Clear();

            SectionExclues exclure = new SectionExclues();
            WebContentCollection wcc = new WebContentCollection();
            if ( HttpContext.Current.User.IsInRole( "Administrateur" ) )
            {
                wcc = WebContentCollection.GetAll();
            }
            else
            {
                wcc = WebContentCollection.GetAllByUser( HttpContext.Current.User.Identity.Name );
            }

            foreach ( WebContent wc in wcc )
            {
                ListItem li = new ListItem( wc.Section );
                if ( Items.Contains( li ) == false )
                {
                    Items.Add( li );
                }
            }
        }
        
        public string SelectedSection
        {
            get 
            {
                EnsureChildControls(); 

                if ( this.SelectedIndex == -1 )
                    return "-1";

                return SelectedItem.Value.ToString(); 
            }
            set 
            { 
                _selected = value.ToString(); 
            }
        }

        // Forcer le composant a effectuer une mise a jour des items
        // a partir de la Base de Donnees si celle-ci a ete modifie
        public void Reload()
        {
            ConstruireItems();

            // Add first item
            Items.Insert( 0, new ListItem( _defaultText, "-1" ) );

            // Selecte default
            ListItem item = Items.FindByValue( _selected );
            if ( item != null )
                item.Selected = true;
        }

        override protected void OnLoad( EventArgs e ) 
        {
            base.OnLoad( e );
            
            if ( !Page.IsPostBack && Visible ) 
            {
                EnsureChildControls();

                ConstruireItems();

                // Add first item
                Items.Insert( 0, new ListItem( _defaultText, "-1" ) );

                // Selecte default
                ListItem item = Items.FindByValue( _selected );
                if ( item != null )
                    item.Selected = true;
            }
        }

        /* Constructeur */
        public DropDownListWebContentSection() : base() 
        {
            //CssClass = "PasEncoreDefinie";
        }
    }
}
