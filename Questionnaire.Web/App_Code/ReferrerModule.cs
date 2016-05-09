#region Using

using System;
using System.Xml;
using System.IO;
using System.Web;
using System.Collections;
using System.Globalization;
//using BlogEngine.Core;
using System.Net;
//using System.Threading;

#endregion


public class ReferrerModule
{
    /// <summary>
    /// Handles the BeginRequest event of the context control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    //private void context_BeginRequest( object sender, EventArgs e )
    //{
    //    HttpContext context = ( ( HttpApplication )sender ).Context;
    //    if ( !context.Request.PhysicalPath.ToLowerInvariant().Contains( ".aspx" ) )
    //        return;

    //    if ( context.Request.UrlReferrer != null )
    //    {
    //        Uri referrer = context.Request.UrlReferrer;
    //        if ( !referrer.Host.Equals( Utils.AbsoluteWebRoot.Host, StringComparison.OrdinalIgnoreCase ) && !IsSearchEngine( referrer.ToString() ) )
    //        {
    //            //ThreadStart threadStart = delegate { BeginRegisterClick( new DictionaryEntry( referrer.ToString(), context.Request.Url ) ); };
    //            //Thread thread = new Thread( threadStart );
    //            //thread.IsBackground = true;
    //            //thread.Start();
    //        }
    //    }
    //}

    //#region Private fields

    ///// <summary>
    ///// Used to thread safe the file operations
    ///// </summary>
    //private static object _SyncRoot = new object();

    ///// <summary>
    ///// The relative path of the XML file.
    ///// </summary>
    //private static string _Folder = HttpContext.Current.Server.MapPath( "~/app_data/log/" );

    //#endregion

    public static bool IsSearchEngine( string referrer )
    {
        string lower = referrer.ToLowerInvariant();
        if ( lower.Contains( "yahoo" ) && lower.Contains( "p=" ) )
            return true;

        return lower.Contains( "?q=" ) || lower.Contains( "&q=" );
    }

    /// <summary>
    /// Determines whether the specified referrer is spam.
    /// </summary>
    /// <param name="referrer">The referrer.</param>
    /// <param name="url">The URL.</param>
    /// <returns>
    /// 	<c>true</c> if the specified referrer is spam; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsSpam( string referrer, Uri url )
    {
        try
        {
            using ( WebClient client = new WebClient() )
            {
                string html = client.DownloadString( referrer ).ToUpperInvariant();
                string subdomain = GetSubDomain( url );
                string host = url.Host.ToUpperInvariant();

                if ( subdomain != null )
                    host = host.Replace( subdomain.ToUpperInvariant() + ".", string.Empty );

                return !html.Contains( host );
            }
        }
        catch
        {
            return true;
        }
    }

    /// <summary>
    /// Retrieves the subdomain from the specified URL.
    /// </summary>
    /// <param name="url">The URL from which to retrieve the subdomain.</param>
    /// <returns>The subdomain if it exist, otherwise null.</returns>
    private static string GetSubDomain( Uri url )
    {
        if ( url.HostNameType == UriHostNameType.Dns )
        {
            string host = url.Host;
            if ( host.Split( '.' ).Length > 2 )
            {
                int lastIndex = host.LastIndexOf( "." );
                int index = host.LastIndexOf( ".", lastIndex - 1 );
                return host.Substring( 0, index );
            }
        }

        return null;
    }

    //private static void BeginRegisterClick( object stateInfo )
    //{
    //    DictionaryEntry entry = ( DictionaryEntry )stateInfo;
    //    string referrer = ( string )entry.Key;
    //    Uri url = ( Uri )entry.Value;
    //    bool isSpam = IsSpam( referrer, url );

    //    RegisterClick( referrer, isSpam );
    //    stateInfo = null;
    //    OnReferrerRegistered( referrer );
    //}

    //private static void RegisterClick( string url, bool isSpam )
    //{
    //    string fileName = _Folder + DateTime.Now.Date.ToString( "dddd", CultureInfo.InvariantCulture ) + ".xml";

    //    lock ( _SyncRoot )
    //    {
    //        XmlDocument doc = CreateDocument( fileName );

    //        string address = HttpUtility.HtmlEncode( url );
    //        XmlNode node = doc.SelectSingleNode( "urls/url[@address='" + address + "']" );
    //        if ( node == null )
    //        {
    //            AddNewUrl( doc, address, isSpam );
    //        }
    //        else
    //        {
    //            int count = int.Parse( node.InnerText, CultureInfo.InvariantCulture );
    //            node.InnerText = ( count + 1 ).ToString( CultureInfo.InvariantCulture );
    //        }

    //        doc.Save( fileName );
    //    }
    //}

    /// <summary>
    /// Adds a new Url to the XmlDocument.
    /// </summary>
    //private static void AddNewUrl( XmlDocument doc, string address, bool isSpam )
    //{
    //    XmlNode newNode = doc.CreateElement( "url" );

    //    XmlAttribute attrAddress = doc.CreateAttribute( "address" );
    //    attrAddress.Value = address;
    //    newNode.Attributes.Append( attrAddress );

    //    XmlAttribute attrSpam = doc.CreateAttribute( "isSpam" );
    //    attrSpam.Value = isSpam.ToString();
    //    newNode.Attributes.Append( attrSpam );

    //    newNode.InnerText = "1";
    //    doc.ChildNodes[ 1 ].AppendChild( newNode );
    //}

    //private static DateTime _Date = DateTime.Now;

    /// <summary>
    /// Creates the XML file for first time use.
    /// </summary>
    //private static XmlDocument CreateDocument( string fileName )
    //{
    //    XmlDocument doc = new XmlDocument();

    //    if ( !Directory.Exists( _Folder ) )
    //        Directory.CreateDirectory( _Folder );

    //    if ( DateTime.Now.Day != _Date.Day || !File.Exists( fileName ) )
    //    {
    //        using ( XmlWriter writer = XmlWriter.Create( fileName ) )
    //        {
    //            writer.WriteStartDocument( true );
    //            writer.WriteStartElement( "urls" );
    //            writer.WriteEndElement();
    //        }
    //    }

    //    _Date = DateTime.Now;
    //    doc.Load( fileName );
    //    return doc;
    //}
}
