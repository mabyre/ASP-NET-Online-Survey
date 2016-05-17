
#region Using

using System;
using System.Web;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Xml;
using System.Data;
using System.ComponentModel;
using System.Net.Mail;
using System.Drawing;
using MemberSettingsData;
using System.Web.UI.WebControls;
using Sql.Web.Data;

#endregion

namespace Sql.Web.Data
{
    [Serializable()]
    public class MemberSettings
    {
        #region Constructor

        public MemberSettings()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Ce membre est Approuve, ce flag doit toujours etre a "false" dans MemberSettings/intervieweur.xml
        /// </summary>
        public bool _Approuve;
        public bool Approuve
        {
            get { return _Approuve; }
            set { _Approuve = value; }
        }

        /// <summary>
        /// Presence ou non du "menu expert"
        /// </summary>
        public bool _MenuExpert;
        public bool MenuExpert
        {
            get { return _MenuExpert; }
            set { _MenuExpert = value; }
        }

        /// <summary>
        /// Presence ou non du "menu en colonne a gauche"
        /// </summary>
        public bool _MenuColonne;
        public bool MenuColonne
        {
            get { return _MenuColonne; }
            set { _MenuColonne = value; }
        }

        /// <summary>
        /// Afficher l'aide contextuelle
        /// </summary>
        public bool _AfficherAide;
        public bool AfficherAide
        {
            get { return _AfficherAide; }
            set { _AfficherAide = value; }
        }

        /// <summary>
        /// Afficher l'aide en ligne
        /// </summary>
        public bool _AfficherAideEnLigne;
        public bool AfficherAideEnLigne
        {
            get { return _AfficherAideEnLigne; }
            set { _AfficherAideEnLigne = value; }
        }

        /// <summary>
        /// Afficher la barre de navigation
        /// </summary>
        public bool _BarreNavigation;
        public bool BarreNavigation
        {
            get { return _BarreNavigation; }
            set { _BarreNavigation = value; }
        }

        /// <summary>
        /// Afficher le MenuToujoursVisible
        /// </summary>
        public bool _MenuToujoursVisible;
        public bool MenuToujoursVisible
        {
            get { return _MenuToujoursVisible; }
            set { _MenuToujoursVisible = value; }
        }

        /// <summary>
        /// Position du MenuToujoursVisible H M B, G C D
        /// </summary>
        public string _MenuToujoursVisiblePosition;
        public string MenuToujoursVisiblePosition
        {
            get { return _MenuToujoursVisiblePosition; }
            set { _MenuToujoursVisiblePosition = value; }
        }

        /// <summary>
        /// Prevenir l'intervieweur d'une nouvelle reponse d'un interviewe
        /// </summary>
        public bool _PrevenirNouvelleReponse;
        public bool PrevenirNouvelleReponse
        {
            get { return _PrevenirNouvelleReponse; }
            set { _PrevenirNouvelleReponse = value; }
        }

        /// <summary>
        /// Texte du Bouton Question suivante
        /// </summary>
        public string _BoutonQuestionSuivanteTexte;
        public string BoutonQuestionSuivanteTexte
        {
            get { return _BoutonQuestionSuivanteTexte; }
            set { _BoutonQuestionSuivanteTexte = value; }
        }

        /// <summary>
        /// Titre du Bouton Question suivante
        /// </summary>
        public string _BoutonQuestionSuivanteAlt;
        public string BoutonQuestionSuivanteAlt
        {
            get { return _BoutonQuestionSuivanteAlt; }
            set { _BoutonQuestionSuivanteAlt = value; }
        }

        /// <summary>
        /// Nombre de Questions par Page
        /// </summary>
        public string _TaillePageQuestions = "100";
        public string TaillePageQuestions
        {
            get { return _TaillePageQuestions; }
            set { _TaillePageQuestions = value; }
        }

        #endregion

        public static MemberSettings Fill( DataRow r )
        {
            MemberSettings obj = new MemberSettings();

            obj.Approuve = ( bool )r[ "Approuve" ];
            obj.MenuExpert = ( bool )r[ "MenuExpert" ];
            obj.MenuColonne = ( bool )r[ "MenuColonne" ];
            obj.AfficherAide = ( bool )r[ "AfficherAide" ];
            obj.AfficherAideEnLigne = ( bool )r[ "AfficherAideEnLigne" ];
            obj.BarreNavigation = ( bool )r[ "BarreNavigation" ];
            obj.MenuToujoursVisible = ( bool )r[ "MenuToujoursVisible" ];
            obj.MenuToujoursVisiblePosition = r[ "MenuToujoursVisiblePosition" ].ToString();
            obj.PrevenirNouvelleReponse = ( bool )r[ "PrevenirNouvelleReponse" ];
            obj.BoutonQuestionSuivanteTexte = r[ "BoutonQuestionSuivanteTexte" ].ToString();
            obj.BoutonQuestionSuivanteAlt = r[ "BoutonQuestionSuivanteAlt" ].ToString();
            obj.TaillePageQuestions = r[ "TaillePageQuestions" ].ToString();
            
            return obj;
        }

        #region Update

        public int Update()
        {
            string membre = "";
            try
            {
                membre = SessionState.MemberInfo.NomUtilisateur;
            }
            catch
            {
                membre = "admin"; // dangereu, car tout membre non utilisateur devient admin
                                  // mais admin veut pouvoir mettre a jour son interface graphique
            }
            int status = XmlMemberSettingsProvider.Update( membre, this );
            return status;
        }

        public int Update( string userName, MemberSettings settings )
        {
            int status = XmlMemberSettingsProvider.Update( userName, settings );
            return status;
        }

        #endregion

        public static MemberSettings GetMemberSettings( string membre )
        {
            if ( membre == String.Empty )
            {
                return null;
            }

            MemberSettings memberSettings = new MemberSettings();
            memberSettings = XmlMemberSettingsProvider.GetMemberSettings( membre );

            return memberSettings;
        }
    }
}
