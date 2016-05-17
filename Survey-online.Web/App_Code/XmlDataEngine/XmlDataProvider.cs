/*
** Utilisation :
** Le fichier XML se trouve dans le repertoire App_Data
** Le noeud principal du fichier xml doit etre nomme comme le fichier
**
** Remarque : 
** Gros soucis au moment d'utiliser la fonction SaveXmlData en effet l'objet StringDictionary
** modifie les Keys pour les mettre en non case sensitive et les reecrit en minuscule dans le
** fichier .xml. En plus je ne sais pas tres bien les trier et il modifie l'ordre
** des keys, quelle grosse merde ! Mais apres tout, qu'est ce que j'en ai a branler puisque
** le fichier settings.xml ne sera plus destine a etre modifie par un humain mais par l'interface
** d'administration de l'appli web et donc cela devient invisible pour l'utilisateur.
**
*/
#region Using

using System;
using System.Web;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.Specialized;

#endregion

namespace XmlDataProvider
{
    public class DataProviderXml
    {
        public static StringDictionary LoadXmlData( string xmlFileName )
        {
            string fileName = HttpContext.Current.Server.MapPath( "~/App_Data/" + xmlFileName );
            if ( !File.Exists( fileName ) )
            {
                string message = string.Format( "Fichier pour le XmlDataProvider non trouvé : {0}", fileName );
                throw new FileNotFoundException( message );
            }

            StringDictionary dic = new StringDictionary();
            XmlDocument doc = new XmlDocument();
            doc.Load( fileName );

            string nodeName = xmlFileName.Substring( 0, xmlFileName.LastIndexOf( '.' ) );
            foreach ( XmlNode settingsNode in doc.SelectSingleNode( nodeName ).ChildNodes )
            {
                string name = settingsNode.Name;
                string value = settingsNode.InnerText;

                dic.Add( name, value );
            }

            return dic;
        }

        public static void SaveXmlData( StringDictionary dico, string xmlFileName )
        {
            if ( dico == null )
                throw new ArgumentNullException( "dico" );

            // He oui le gros caca cette saloperie d'objet StringDictionary fout les keys
            // en small letters et possede une fonction de comparaison non case sensitive
            // on est de la baise ! grosCaca est tout le temps vrai !
            //bool grosCaca = false;
            //grosCaca = dico.ContainsKey( "SiteName" );
            //grosCaca = dico.ContainsKey( "sitename" );
            //grosCaca = dico.ContainsKey( "SITEname" );

            string filename = HttpContext.Current.Server.MapPath( "~/App_Data/" + xmlFileName );
            XmlWriterSettings writerSettings = new XmlWriterSettings(); ;
            writerSettings.Indent = true;

            using ( XmlWriter writer = XmlWriter.Create( filename, writerSettings ) )
            {
                string[] startElement = xmlFileName.Split( '.' );
                writer.WriteStartElement( startElement[ 0 ] );

                foreach ( string key in dico.Keys )
                {
                    writer.WriteElementString( key, dico[ key ] );
                }

                writer.WriteEndElement();
                writer.Close();
            }
        }
    }
}
