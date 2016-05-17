
// Impossible d'ecrire le Javascript de merde il plante 
// var product = lb.options(lb.selectedIndex).value 
// est NULL !!

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


public partial class ClientCallback : Page, ICallbackEventHandler
{
    protected ListDictionary catalog;

    protected void Page_Load(object sender, EventArgs e)
    {
        String cbReference = Page.ClientScript.GetCallbackEventReference
        ( 
            this,
            "arg", 
            "ReceiveServerData", 
            "context" 
        );

        String callbackScript;
        callbackScript = "function CallServer(arg, context)" + "{ " + cbReference + "} ;";
        Page.ClientScript.RegisterClientScriptBlock
        ( 
            this.GetType(),
            "CallServer", 
            callbackScript, 
            true 
        );

        catalog = new System.Collections.Specialized.ListDictionary();
        catalog.Add( "monitor", 12 );
        catalog.Add( "laptop", 10 );
        catalog.Add( "keyboard", 23 );
        catalog.Add( "mouse", 17 );

        //ListBox1.Items[].Value

        ListBox1.DataSource = catalog;
        ListBox1.DataTextField = "key";
        ListBox1.DataBind();
    }

    private string _Callback;

    public string GetCallbackResult()
    {
        return _Callback;
    }

    public void RaiseCallbackEvent( string eventArgument )
    {
        if ( catalog[ eventArgument ] == null )
        {
            _Callback = "-1";
        }
        else
        {
            _Callback = catalog[ eventArgument ].ToString();
        }
    }

}
