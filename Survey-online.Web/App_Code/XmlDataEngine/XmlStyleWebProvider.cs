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
using TraceReporter;

/// <summary>
/// XML Data Layer for StyleWeb
/// Ca marche tout seul lors de la creation d'un utilisateur le fichier n'existe pas.
/// on fait une copie du fichier intervieweur.xml pour le nouvel utilisateur
/// Si un style n'existe pas et que l'utilisateur veut le modifier on cree le style.
/// 02/09/08 : Correction bug sur authentification des utilisateurs :
/// un utilisateur pour se connecter a l'application en mettant des espaces dans son nom
/// avant la correction, les espaces se retrouvaient dans le nom du fichier .xml
/// membre.Trim(), LowerCase UpperCase est regle ailleurs.
/// </summary>

namespace StyleWebData
{
    public class XmlStyleWebProvider
    {
        private static string _xsdFile = "StyleWeb.xsd";

        private static void Fill( ref DataRow row, StyleWeb sw )
        {
            row[ "NomStyleWeb" ] = sw.NomStyleWeb;
            row[ "Type" ] = sw.Type;
            row[ "Applicable" ] = sw.Applicable;
            row[ "BackColor" ] = sw.BackColor;
            row[ "Bold" ] = sw.Bold;
            row[ "BorderColor" ] = sw.BorderColor;
            row[ "BorderStyle" ] = sw.BorderStyle;
            row[ "BorderWidth" ] = sw.BorderWidth;
            row[ "FontName" ] = sw.FontName;
            row[ "FontSize" ] = sw.FontSize;
            row[ "ForeColor" ] = sw.ForeColor;
            row[ "Padding" ] = sw.Padding;
            row[ "Spacing" ] = sw.Spacing;
            row[ "Height" ] = sw.Height;
            row[ "Width" ] = sw.Width;
            row[ "Italic" ] = sw.Italic;
            row[ "Overline" ] = sw.Overline;
            row[ "Strikeout" ] = sw.Strikeout;
            row[ "Underline" ] = sw.Underline;
            row[ "TextAlign" ] = sw.TextAlign;
        }

        #region CreateUpdateDelete

        // On en peut pas faire de create si le fichier du membre n'existe pas !!!
        //
        public static int Create( string membre, StyleWeb o )
        {
            string _xmlFile = "StyleWeb/" + membre.Trim() + "/" + SessionState.Questionnaire.Style;

            DataSet dataSet = XmlUtil.ReadAndValidateXml( _xmlFile, _xsdFile );
            DataTable dataTable = dataSet.Tables[ 0 ];
            DataRow row = dataTable.NewRow();

            row[ "StyleWebGUID" ] = Guid.NewGuid();
            Fill( ref row, o );
            dataTable.Rows.Add( row );

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

        public static int Update( string membre, StyleWeb o )
        {
            string _xmlFile = "StyleWeb/" + membre.Trim() + "/" + SessionState.Questionnaire.Style;

            DataSet dataSet = XmlUtil.ReadAndValidateXml( _xmlFile, _xsdFile );
            DataTable dataTable = dataSet.Tables[ 0 ];
            foreach ( DataRow row in dataTable.Rows )
            {
                if ( o.StyleWebGUID == new Guid( row[ "StyleWebGUID" ].ToString() ) )
                {
                    DataRow r = row;
                    Fill( ref r, o );
                    break;
                }
            }

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

        public static int Delete( string membre, StyleWeb o )
        {
            string _xmlFile = "StyleWeb/" + membre.Trim() + "/" + SessionState.Questionnaire.Style;

            DataSet dataSet = XmlUtil.ReadAndValidateXml( _xmlFile, _xsdFile );
            DataTable dataTable = dataSet.Tables[ 0 ];
            foreach ( DataRow row in dataTable.Rows )
            {
                if ( o.StyleWebGUID == ( Guid )row[ "StyleWebGUID" ] )
                {
                    row.Delete();
                }
            }

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

        //public static StyleWebCollection GetAll( string membre )
        //{
        //    string _xmlFile = "StyleWeb/" + membre.Trim() + ".xml";
            
        //    StyleWebCollection list = new StyleWebCollection();
        //    DataSet dataSet = XmlUtil.ReadAndValidateXml( _xmlFile, _xsdFile );

        //    DataTable dataTable = dataSet.Tables[ 0 ];
        //    foreach ( DataRow r in dataTable.Rows )
        //    {
        //        StyleWeb current = StyleWeb.Fill( r );
        //        list.Add( current );
        //    }
        //    return list;
        //}

        /// <summary>
        /// Aller chercher le StyleWeb pour l'utilisateur
        /// </summary>
        /// <param name="membre"></param>
        /// <param name="nomStyleWeb">Question, Réponse, etc ...</param>
        /// <param name="typeStyleWeb">TextBox, Label, RadioButtonLis, etc..</param>
        public static StyleWeb GetStyleWeb( string membre, string nomStyleWeb, string typeStyleWeb )
        {
            Reporter.Trace( "GetStyleWeb()" );

            string _xmlFile = "StyleWeb/" + membre.Trim() + "/" + SessionState.Questionnaire.Style;
            Reporter.Trace( "  SessionState.Questionnaire.Style : {0}", SessionState.Questionnaire.Style );

            string fileName = HttpContext.Current.Request.MapPath( "~/App_Data/" + _xmlFile );
            if ( File.Exists( fileName ) == false )
            {
                string path =  HttpContext.Current.Request.MapPath( "~/App_Data/StyleWeb/" + membre.Trim() );
                if ( ! Directory.Exists( path ) )
                {
                    Directory.CreateDirectory( path );
                }
                // Copie du fichier de l'intervieweur pour un membre qui n'a pas ce fichier de styles
                string dir = HttpContext.Current.Request.MapPath( "~/App_Data/StyleWeb" );
                File.Copy( dir + "/intervieweur/" + SessionState.Questionnaire.Style, dir + "/" + membre.Trim() + "/" + SessionState.Questionnaire.Style );
            }

            DataSet dataSet = XmlUtil.ReadAndValidateXml( _xmlFile, _xsdFile );
            DataTable dataTable = dataSet.Tables[ 0 ];
            foreach ( DataRow r in dataTable.Rows )
            {
                if ( nomStyleWeb == ( string )r[ "NomStyleWeb" ] && typeStyleWeb == ( string )r[ "Type" ] ) // match found
                {
                    StyleWeb styleTrouve = StyleWeb.Fill( r );
                    return styleTrouve;
                }
            }

            // Le style n'existe pas on le cree
            StyleWeb style = new StyleWeb();
            style.NomStyleWeb = nomStyleWeb;
            style.Type = typeStyleWeb;
            style.Applicable = true;
            style.BackColor = "#FFFFFF";
            style.Bold = false;
            style.BorderColor = "#000000";
            style.BorderStyle = 1;
            style.BorderWidth = 1;
            style.FontName = "none";
            style.ForeColor = "#000000";
            style.Padding = "";
            style.Spacing = "";
            style.Width = "";
            style.Height = "";
            style.Italic = false;
            style.Overline = false;
            style.Strikeout = false;
            style.Underline = false;
            style.TextAlign = "left";

            Create( membre, style );

            return style;
        }
    }
}
