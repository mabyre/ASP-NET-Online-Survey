/*
** Aller chercher les Questionnaires lies a un Membre
** S'il n'y pas de Membre choisi selectionner toutes les Questionnaires
** Afficher le tout dans une DropDownList
** 
** BUG20100210 Patacaisse avec le code d'accès, j'ai pu vérifier dans l'appli en prod
** des utilisateurs se sont échangé des pages, il existe egalement des pages
** pour des utilisateurs qui ont été supprimés !
** Je retire static et je modifie la stratégie pour CodeAccess
** Cela me laisse songeur pour les autres DropDownList qui sont encore avec des 
** statics ...
*/

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Sql.Web.Data;

namespace UserControl.Web.Controls
{
    public class DropDownListQuestionnaire : DropDownList 
    {
        protected string _defaultText = "Tous les Questionnaires";
        protected string _selected = null;
        protected Guid _selectedMembreGUID = Guid.Empty;

        public string DefaultText 
        {
            get { return _defaultText; }
            set { _defaultText = value; }
        }
        
        public string SelectedQuestionnaire
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

        public Guid SelectedMembreGUID
        {
            get { return _selectedMembreGUID; }
            set 
            { 
                _selectedMembreGUID = value;
                this.Reload();
            }
        }

        // Retrouver le CodeAcces du Questionnaire selectionné
        // BUG20100210 : public static ArrayList _CodeAcces = new ArrayList();
        // Format de la DropDownList : le code d'accès, c'est les 4 derniers digits
        public string SelectedCodeAcces
        {
            get 
            {
                return SelectedQuestionnaire.Substring( SelectedQuestionnaire.Length - 4 );
            }
        }

        private void ConstruireItems()
        {
            Items.Clear();

            QuestionnaireCollection qc = new QuestionnaireCollection();
            if ( _selectedMembreGUID != Guid.Empty )
            {
                qc = QuestionnaireCollection.GetQuestionnaireMembre( _selectedMembreGUID );
            }
            else
            {
                qc = QuestionnaireCollection.GetAll();
            }
            foreach ( Questionnaire q in qc )
            {
                ListItem li = new ListItem( q.Description + ":" + q.CodeAcces );
                Items.Add( li );
            }
        }

        // Forcer le composant a effectuer une mise a jour des items
        // a partir de la Base de Donnees
        public void Reload()
        {
            ConstruireItems();
            
            // Add first item
            Items.Insert( 0, new ListItem( _defaultText, "-1" ) );

            // L'utilisateur n'a pas fait de choix
            // La liste est remplie
            // Et un Membre a ete choisi
            if ( _selected == null && Items.Count > 1 && _selectedMembreGUID != Guid.Empty )
                this.SelectedIndex = 1; // Selectionner le premier Questionnaire de la Liste

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

                // L'utilisateur n'a pas fait de choix
                // La liste est remplie
                // Et un Membre a ete choisi
                if ( _selected == null && Items.Count > 1 && _selectedMembreGUID != Guid.Empty )
                    this.SelectedIndex = 1; // Selectionner le premier Questionnaire de la Liste

                // Selecte default
                ListItem item = Items.FindByValue( _selected );
                if ( item != null )
                    item.Selected = true;
            }
        }

        /* Constructeur */
        public DropDownListQuestionnaire() : base() 
        {
            //CssClass = "";
        }
    }
}
