using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class PageTest_PageProgressBarre : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        Trace.Warn( "Page_Load" );
        ProgressBarre.LabelMessagesText = "Cliquez sur ok pour progresser";

        if ( IsPostBack == false )
        {
            ProgressBarre.CurrentState = 0;
            ProgressBarre.LabelProgressText = ProgressBarre.CurrentState.ToString() + "/" + ProgressBarre.States;
        }
    }

    protected void ButtonProgress_Click( object sender, EventArgs e )
    {
        Trace.Warn( "ButtonProgress_Click" );

        ProgressBarre.CurrentState += 1;
        ProgressBarre.LabelProgressText = ProgressBarre.CurrentState.ToString() + "/" + ProgressBarre.States;

        if ( ProgressBarre.CurrentState == ProgressBarre.States )
        {
            ProgressBarre.LabelProgressText = "Terminé";
        }
    }
}
