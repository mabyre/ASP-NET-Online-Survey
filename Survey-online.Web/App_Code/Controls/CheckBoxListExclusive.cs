/*
** Transforme SelectedIndex en TrueSelectedIndex qui represente le vrai choix utilisateur
** Attention : Il faut appeler les fonction de la class de base apres avoir construit les objets
** sinon ils n'ont pas les bonnes valeurs dans la page.
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
    public class CheckBoxListExclusive : CheckBoxList
    {
        public CheckBoxListExclusive() : base() 
        {
        }

        static int _TrueSelectedIndex = 0;
        public int TrueSelectedIndex
        {
            get { return _TrueSelectedIndex; }
            set { _TrueSelectedIndex = value; }
        }

        public string TrueSelectedValue
        {
            get { return this.Items[ _TrueSelectedIndex ].ToString(); }
        }

        private void BuildObjectItems()
        {
            for ( int i = 0;i < this.Items.Count;i++ )
            {
                ListItem li = this.Items[ i ];
                if ( li.Selected && ( li.Value != this.Items[ TrueSelectedIndex ].Value ) )
                {
                    this.Items[ TrueSelectedIndex ].Selected = false;
                    TrueSelectedIndex = i;
                }
            }

            this.SelectedIndex = this.TrueSelectedIndex;
        }

        override protected void OnLoad( EventArgs e ) 
        {
            if ( Visible )
            {
                EnsureChildControls();
                if ( ! Page.IsPostBack )
                {
                    BuildObjectItems();
                }
            }

            base.OnLoad( e );
        }

        protected override void OnSelectedIndexChanged( EventArgs e )
        {
            BuildObjectItems();
            base.OnSelectedIndexChanged( e );
        }
    }
}
