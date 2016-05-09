using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;

public sealed class Utils
{
    public static Control FindControlRecursively( string controlID, ControlCollection controls )
    {
        if ( controlID == null || controls == null )
            return null;

        foreach ( Control c in controls )
        {
            if ( c.ID == controlID )
                return c;

            if ( c.HasControls() )
            {
                Control inner = FindControlRecursively( controlID, c.Controls );
                if ( inner != null )
                    return inner;
            }
        }
        return null;
    }

    /// <summary>
    /// L'URL du site web, ne me demandez pas comment ca marche !
    /// Et d'ailleurs cela ne marche pas si il y a une Query dans l'Url
    /// mais pour ne pas avoir a tout revalider je perfere ecrire WebSiteUriWhithOutQuery
    /// </summary>
    public static string WebSiteUri
    {
        get 
        {
            return HttpContext.Current.Request.Url.AbsoluteUri.Substring( 0, HttpContext.Current.Request.Url.AbsoluteUri.Length - HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Substring( 1 ).Length );
        }
    }

    /// <summary>
    /// Lorsqu'il y a une Query dans l'Url, WebSiteUri ne fonctionne pas
    /// </summary>
    public static string WebSiteUriWhithOutQuery
    {
        get
        {
            return HttpContext.Current.Request.Url.AbsoluteUri.Substring( 0, HttpContext.Current.Request.Url.AbsoluteUri.Length - HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Substring( 1 ).Length - HttpContext.Current.Request.Url.Query.Length );
        }
    }



    /// <summary>
    /// Gets the relative root of the website.
    /// </summary>
    /// <value>A string that ends with a '/'.</value>
    public static string RelativeWebRoot
    {
        get { return VirtualPathUtility.ToAbsolute( Global.SettingsXml.VirtualPath ); }
    }

    private static Uri _AbsoluteWebRoot;

    /// <summary>
    /// Gets the absolute root of the website.
    /// </summary>
    /// <value>A string that ends with a '/'.</value>
    public static Uri AbsoluteWebRoot
    {
        get
        {
            if ( _AbsoluteWebRoot == null )
            {
                HttpContext context = HttpContext.Current;
                if ( context == null )
                    throw new System.Net.WebException( "The current HttpContext is null" );

                _AbsoluteWebRoot = new Uri( context.Request.Url.Scheme + "://" + context.Request.Url.Authority + RelativeWebRoot );
            }
            return _AbsoluteWebRoot;
        }
    }

    public static void AlwaysVisibleControlExtenderPosition( ref AlwaysVisibleControlExtender avce )
    {
        avce.Enabled = true;
        switch ( SessionState.MemberSettings.MenuToujoursVisiblePosition[ 0 ] )
        {
            case 'H':
                avce.VerticalSide = VerticalSide.Top;
                break;
            case 'M':
                avce.VerticalSide = VerticalSide.Middle;
                break;
            case 'B':
                avce.VerticalSide = VerticalSide.Bottom;
                break;
            default:
                avce.Enabled = false;
                return;
        }

        switch ( SessionState.MemberSettings.MenuToujoursVisiblePosition[ 1 ] )
        {
            case 'G':
                avce.HorizontalSide = HorizontalSide.Left;
                break;
            case 'C':
                avce.HorizontalSide = HorizontalSide.Center;
                break;
            case 'D':
                avce.HorizontalSide = HorizontalSide.Right;
                break;
            default:
                avce.Enabled = false;
                return;
        }
    }
}
