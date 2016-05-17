using System;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Diagnostics;
using System.Web.Mail;
using System.Configuration;
using SettingXmlProvider;
using TraceReporter;

/// <summary>
/// Summary description for Global.
/// </summary>
public class Global : System.Web.HttpApplication
{
    public static SettingXml SettingsXml = new SettingXml();

    void Application_Start( object sender, EventArgs e )
    {
        Reporter.TraceInit( "Questionnaire" ); // pipi popo 
        Reporter.Trace( Report.TRACE0, "Application_Start()" );

        Application[ "ActiveUsers" ] = 0;
    }

    void Application_End( object sender, EventArgs e )
    {
    }

    void Session_Start( object sender, EventArgs e )
    {
        Reporter.Trace( "Session_Start()" );

        Application.Lock();
        Session[ "SettingsSet" ] = false;
        Application[ "ActiveUsers" ] = ( int )Application[ "ActiveUsers" ] + 1;
        Application.UnLock();
    }

    void Session_End( object sender, EventArgs e )
    {
        Reporter.Trace( "Session_End()" );

        Application.Lock();
        Application[ "ActiveUsers" ] = ( int )Application[ "ActiveUsers" ] - 1;
        Application.UnLock();
    }

    protected void Application_AuthenticateRequest( object sender, EventArgs e )
    {
    }

    void Application_Error( object sender, EventArgs e )
    {
    }
}