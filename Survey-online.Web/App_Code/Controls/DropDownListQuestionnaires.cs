/*
** On distingue 2 types d'objets DDL Questionnaire pour l'autre ya un item "Tous les questionnaires"
** afin que l'admin puisse tout visualiser mais en utilisation "normale" c'est super chiant a gerer
** donc l'autre n'est utilise que pour WebContent
** 
** Celle-ci se charge automatiquement et c'est la presence de SessionSate.MembreInfo qui distingue
** l'Admin des autres membres
**
** Aller chercher les Questionnaires lies a un Membre
** S'il n'y pas de Membre choisi selectionner toutes les Questionnaires
** Afficher le tout dans une DropDownList
**
** Observation curieuse : Si deux questionnaires on la même description ce qui est le cas surtout
** depuis que je peux copier des questionnaire lorsqu'on choisi l'un ou l'autre il n'y a pas
** d'evenement selected index change de genere pour la DDL
** d'ou l'idee de rajouter :CodeAcces sinon on est perdu !!!
** 
** Attention on aurait pu se servir de ListItem( string text, string value ) pour cacher le code ou l'id
** mais bon si l'evt est pas declenche c'est la daube de toute facon
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
    public class DropDownListQuestionnaires : DropDownList 
    {
        protected static string _defaultText = "Pas de Questionnaire";
        public string DefaultText
        {
            get { return _defaultText; }
            set { _defaultText = value; }
        }

        protected string _selected = null;
        public string SelectedQuestionnaire
        {
            get 
            {
                EnsureChildControls(); 

                if ( this.SelectedIndex == -1 )
                    return _defaultText;

                return SelectedItem.Value.ToString(); 
            }
        }

        // Retrouver facilement le CodeAcces d'un Questionnaire
        private static ArrayList _CodeAcces = new ArrayList();
        public int CodeAcces
        {
            get
            {
                return ( int )_CodeAcces[ SelectedIndex ];
            }
        }

        // Retrouver facilement l'ID d'un Questionnaire
        private static ArrayList _QuestionnaireID = new ArrayList();
        public int QuestionnaireID
        {
            get
            {
                return ( int )_QuestionnaireID[ SelectedIndex ];
            }
        }

        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
            Load();
        }

        override protected void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack && Visible )
            {
                EnsureChildControls();
                Load();
            }
        }

        // Charger le composant a partir de la Base de Donnees
        public void Load()
        {
            Items.Clear();
            _CodeAcces.Clear();
            _QuestionnaireID.Clear();

            QuestionnaireCollection qc = SessionState.Questionnaires;
            foreach ( Questionnaire q in qc )
            {
                ListItem li = new ListItem( q.Description + ":" + q.CodeAcces );
                Items.Add( li );
                _CodeAcces.Add( q.CodeAcces );
                _QuestionnaireID.Add( q.QuestionnaireID );
            }

            if ( Items.Count <= 0 )
                Items.Add( new ListItem( _defaultText ) );

            if ( SessionState.Questionnaire != null ) // Selectionne par l'utilisateur
            {
                ListItem item = Items.FindByValue( SessionState.Questionnaire.Description + ":" + SessionState.Questionnaire.CodeAcces );
                if ( item != null )
                    item.Selected = true; // Positionne SelectedIndex
            }
            else
            {
                this.SelectedIndex = 0;
            }
        }

        /* Constructeur */
        public DropDownListQuestionnaires() : base() 
        {
            //CssClass = "";
        }
    }
}
