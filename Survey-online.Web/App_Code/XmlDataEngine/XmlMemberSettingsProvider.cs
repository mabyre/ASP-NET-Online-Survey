using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Sql.Web.Data;

/// <summary>
/// XML Data Layer for MemberSettings
/// Ca marche tout seul, lors de la creation d'un utilisateur le fichier n'existe pas,
/// on fait une copie du fichier intervieweur.xml pour le nouvel utilisateur.
/// </summary>

namespace MemberSettingsData
{
    public class XmlMemberSettingsProvider
    {
        private static string _xsdFile = "MemberSettings.xsd";

        private static void Fill( ref DataRow row, MemberSettings obj )
        {
            row[ "Approuve" ] = obj.Approuve;
            row[ "MenuExpert" ] = obj.MenuExpert;
            row[ "MenuColonne" ] = obj.MenuColonne;
            row[ "AfficherAide" ] = obj.AfficherAide;
            row[ "AfficherAideEnLigne" ] = obj.AfficherAideEnLigne;
            row[ "BarreNavigation" ] = obj.BarreNavigation;
            row[ "MenuToujoursVisible" ] = obj.MenuToujoursVisible;
            row[ "MenuToujoursVisiblePosition" ] = obj.MenuToujoursVisiblePosition;
            row[ "PrevenirNouvelleReponse" ] = obj.PrevenirNouvelleReponse;
            row[ "BoutonQuestionSuivanteTexte" ] = obj.BoutonQuestionSuivanteTexte;
            row[ "BoutonQuestionSuivanteAlt" ] = obj.BoutonQuestionSuivanteAlt;
            row[ "TaillePageQuestions" ] = obj.TaillePageQuestions;
        }

        #region Update

        public static int Update( string membre, MemberSettings obj )
        {
            string _xmlFile = "MemberSettings/" + membre.Trim() + ".xml";

            DataSet dataSet = XmlUtil.ReadAndValidateXml( _xmlFile, _xsdFile );
            DataRow dataRow  = dataSet.Tables[ 0 ].Rows[ 0 ];
            Fill( ref dataRow, obj );

            try
            {
                XmlUtil.DataSetWriteXml( ref dataSet, _xmlFile );
            }
            catch
            {
                return 0;
            }

            return 1;
        }

        #endregion

        public static MemberSettings GetMemberSettings( string membre )
        {
            string _xmlFile = "MemberSettings/" + membre.Trim() + ".xml";

            string fileName = HttpContext.Current.Request.MapPath( "~/App_Data/" + _xmlFile );
            if ( File.Exists( fileName ) == false )
            {
                // Copie du fichier de l'intervieweur pour un membre qui n'a pas de fichier de settings
                string dir = HttpContext.Current.Request.MapPath( "~/App_Data/MemberSettings/" );
                File.Copy( dir + "intervieweur.xml", dir + membre.Trim() + ".xml" );
            }

            DataSet dataSet = new DataSet();
            dataSet = XmlUtil.ReadAndValidateXml( _xmlFile, _xsdFile );
            DataRow dataRow = dataSet.Tables[ 0 ].Rows[ 0 ];
            MemberSettings memberSettings = MemberSettings.Fill( dataRow );

            return memberSettings;
        }
    }
}
