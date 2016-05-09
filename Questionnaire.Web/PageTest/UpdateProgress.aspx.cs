using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Threading;

public partial class PageTest_UpdateProgress : System.Web.UI.Page
{
    AutoResetEvent autoResetEvent = new AutoResetEvent( false );

    int invokeCount, maxCount;

    public void StatusChecker( int count )
    {
        invokeCount = 0;
        maxCount = count;
    }

    // This method is called by the timer delegate.
    public void CheckStatus( object stateInfo )
    {
        AutoResetEvent autoEvent = ( AutoResetEvent )stateInfo;
        string str = string.Format( "{0} Checking status {1,2}.", DateTime.Now.ToString( "h:mm:ss.fff" ),
            ( ++invokeCount ).ToString() );

        //UpdateProgress1.
        //LabelStatusChecker.Text += ".";
        //Label lab = ( Label )UpdateProgress1.FindControl( "LabelProgress" );
        //lab.Text += ".";

        if ( invokeCount == maxCount )
        {
            // Reset the counter and signal Main.
            invokeCount = 0;
            autoEvent.Set();
        }
    }    

    protected void Page_Load(object sender, EventArgs e)
    {
        if ( !IsPostBack )
        {
            LabelStatusChecker.Text = "...";

            // Impossible UpdateProgress1 possede 0 controls !!
            //Label lab = ( Label )UpdateProgress1.FindControl( "ProgressTemplate" );
        }
    }

    protected void ButtonWait_Click( object sender, EventArgs e )
    {
        StatusChecker( 10 );
        TimerCallback timerDelegate = new TimerCallback( CheckStatus );

        System.Threading.Timer stateTimer = new System.Threading.Timer( timerDelegate, autoResetEvent, 1000, 250 );

        // 1/2 second.
        autoResetEvent.WaitOne( 5000, false );
        stateTimer.Change( 0, 500 );
        Console.WriteLine( "\nChanging period.\n" );

        // When autoEvent signals the second time, dispose of 
        // the timer.
        autoResetEvent.WaitOne( 5000, false );
        stateTimer.Dispose();
        Console.WriteLine( "\nDestroying timer." );
    }
}
