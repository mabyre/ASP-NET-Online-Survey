/*
** Aller touts les Membres
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
    public class DropDownListMembre : DropDownList
    {
        string _selected = null;

        string _defaultText = "Pour l'Admin";
        public string Default
        {
            get { return _defaultText; }
            set { _defaultText = value; }
        }

        // Permet de retrouver facilement le Guid d'un Membre
        public static ArrayList _MembreGUID = new ArrayList();
        public ArrayList MembreGUID
        {
            get { return _MembreGUID; }
            set { _MembreGUID = value; }
        }

        private void ConstruireItems()
        {
            Items.Clear();
            _MembreGUID.Clear();

            MemberInfoCollection mic = MemberInfoCollection.GetAll();
            foreach ( MemberInfo mi in mic )
            {
                ListItem li = new ListItem( mi.Nom + "/" + mi.Prenom + "/" + mi.Societe );
                Items.Add( li );
                _MembreGUID.Add( mi.MembreGUID );
            }
        }
        
        public string SelectedMembre
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
        public DropDownListMembre() : base() 
        {
            CssClass = "PasEncoreDefinie";
        }
    }
}
