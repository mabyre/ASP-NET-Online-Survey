/*
** Gestion d'un list de RadioButton disperses sur une Page
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
    public class RadioButtonExtender
    {
        public RadioButtonExtender()
        {
        }
        
        // Collection de RadioButton
        private ArrayList Collection = new ArrayList();

        // Ajouter un RadioButton a gerer
        public void Add( RadioButton radioButton )
        {
            radioButton.CheckedChanged += new EventHandler( _CheckedChanged );
            Collection.Add( radioButton );
        }
        
        public void Remove( RadioButton radioButton )
        {
            // Supression de l'évènement sur le bouton supprimé
            radioButton.CheckedChanged -= new EventHandler( _CheckedChanged );
            
            // On l'enlève de la collection
            Collection.Remove( radioButton );
        }

        public void RemoveAll()
        {
            Collection.Clear();
        }
        
        // Une modification a été détecté
        private void _CheckedChanged( object sender, EventArgs e )
        {
            foreach ( RadioButton radioButton in Collection )
            {
                radioButton.Checked = false;
            }

            // Puis on coche le bon RadioButton
            (( RadioButton )sender).Checked = true;
        }
    }
}
