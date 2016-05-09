using System;
using System.Web;
//using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Schema;
using System.Data;
using System.IO;

/// <summary>
/// Provides util functions
/// </summary>
public class XmlUtil
{
    private static Object lockObj = new Object();

    /// <summary>
    /// Callback function invoked on xml validation errors
    /// </summary>
    private static void ValidationEventHandler( object sender, ValidationEventArgs e )
    {
        lock ( lockObj )
        {
            switch ( e.Severity )
            {
                case XmlSeverityType.Error:
                    break;

                case XmlSeverityType.Warning:
                    break;
            }
        }
    }

    /// <summary>
    /// Validates the xml file well formed and confirms to the schema 
    /// function is static as used by other classes as well
    /// </summary>
    private static void ValidateXml( string xmlFilePath, string schemaFilePath )
    {
        // create a schema set and copy it to settings.Schemas
        XmlSchemaSet schema = new XmlSchemaSet();
        schema.Add( null, schemaFilePath );

        XmlReaderSettings settings = new XmlReaderSettings();
        settings.ValidationType = ValidationType.Schema;
        settings.Schemas.Add( schema );

        // Parse the xml data file. 
        using ( XmlReader reader = XmlReader.Create( xmlFilePath, settings ) )
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load( reader );
            ValidationEventHandler eventHandler = new ValidationEventHandler( ValidationEventHandler );
            xmlDoc.Validate( eventHandler );
        }
    }

    /// <summary>
    /// Reads the 'xmlFilePath' and validates it against the scehma in 'schemaFilePath'. Returns dataset containing xml file data
    /// </summary>
    public static DataSet ReadAndValidateXml( string xmlFilePath, string schemaFilePath )
    {
        string _xmlFile = HttpContext.Current.Request.MapPath( "~/App_Data/" + xmlFilePath );
        string _xsdFile = HttpContext.Current.Request.MapPath( "~/App_Data/Schemas/" + schemaFilePath );

        DataSet dataSet = null;
        XmlUtil.ValidateXml( _xmlFile, _xsdFile );
        using ( FileStream fs_xml = new FileStream( _xmlFile, FileMode.Open, FileAccess.Read ) )
        {
            using ( FileStream fs_xsd = new FileStream( _xsdFile, FileMode.Open, FileAccess.Read ) )
            {
                dataSet = new DataSet();
                dataSet.ReadXmlSchema( fs_xsd );
                dataSet.ReadXml( fs_xml, XmlReadMode.IgnoreSchema );
            }
        }
        return dataSet;
    }

    public static void DataSetWriteXml( ref DataSet ds, string xmlFile )
    {
        ds.AcceptChanges();
        ds.WriteXml( HttpContext.Current.Request.MapPath( "~/App_Data/" + xmlFile ), XmlWriteMode.IgnoreSchema );
    }

}
