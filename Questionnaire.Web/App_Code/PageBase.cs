/* 
** 
** Page de base dont toute les autres pages doivent deriver si elles veulent 
** utiliser les fonctionnalités :
** - Log de l'utilisateur
** 
*/

using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public class PageBase : System.Web.UI.Page
{
    protected void LogUser()
    {
        string dirName = Server.MapPath( "~/Logs/" );
        string fileName = dirName + Request.UserHostAddress.ToString() + ".log";

        StreamWriter streamWriter = null;
        FileStream fileStream = null;
        if ( Directory.Exists( dirName ) )
        {
            if ( !File.Exists( fileName ) )
            {
                fileStream = new FileStream( fileName, FileMode.Create, FileAccess.Write );
                streamWriter = new StreamWriter( fileStream );
            }
            else
            {
                fileStream = new FileStream( fileName, FileMode.Append, FileAccess.Write );
                streamWriter = new StreamWriter( fileStream );
            }
        }

        if ( streamWriter != null )
        {
            // On termine la session precedente si le fichier ne vient pas d'etre cree
            FileInfo fi = new FileInfo( fileName );
            if ( fi.Length != 0 )
            {
                streamWriter.WriteLine();
            }

            // Trouver d'ou vient l'utilisateur s'il vient d'ailleurs que du site actuel
            if ( Request.UrlReferrer != null )
            {
                Uri referrer = Request.UrlReferrer;
                if ( !referrer.Host.Equals( Utils.AbsoluteWebRoot.Host, StringComparison.OrdinalIgnoreCase ) && !ReferrerModule.IsSearchEngine( referrer.ToString() ) )
                {
                    string address = HttpUtility.HtmlEncode( referrer.ToString() );
                    streamWriter.Write( address );
                    streamWriter.WriteLine();
                }
            }

            // Tracer les utilisateurs non authentifies : les interviewes
            string personne = "";
            if ( SessionState.Personne != null && User.Identity.IsAuthenticated == false )
            {
                personne = SessionState.Personne.Prenom + " " + SessionState.Personne.Nom;
            }

            string log =
                DateTime.Now.ToString() +
                " " + Request.Browser.Platform.ToString() +
                " " + Request.UserHostName.ToString() +
                " " + User.Identity.Name +
                " " + User.Identity.IsAuthenticated.ToString() +
                " " + personne +
                " " + Request.RawUrl;

            streamWriter.Write( log );
            streamWriter.Close();
            fileStream.Close();
        }
    }

    //protected override void OnPreInit( EventArgs e )
    //{
    //    base.OnPreInit( e );
    //}

    protected override void OnLoad( EventArgs e )
    {
        if ( Page.IsPostBack == false )
        {
            if ( Global.SettingsXml.LogUser )
            {
                LogUser();
            }
        }
        base.OnLoad( e );
    }

    /// <summary>
    /// Adds the content-type meta tag to the header.
    /// </summary>
    protected virtual void AddMetaContentType()
    {
        HtmlMeta meta = new HtmlMeta();
        meta.HttpEquiv = "content-type";
        meta.Content = Response.ContentType + "; charset=" + Response.ContentEncoding.HeaderName;
        Page.Header.Controls.Add( meta );
    }

    /// <summary>
    /// Add a meta tag to the page's header.
    /// </summary>
    protected virtual void AddMetaTag( string name, string value )
    {
        if ( string.IsNullOrEmpty( name ) || string.IsNullOrEmpty( value ) )
            return;

        HtmlMeta meta = new HtmlMeta();
        meta.Name = name;
        meta.Content = value;
        Page.Header.Controls.Add( meta );
    }

}
