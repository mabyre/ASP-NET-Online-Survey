// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.
using System;
using System.Collections;
using System.Configuration;
using System.Data;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

public partial class PopupControl_PopupControl : System.Web.UI.Page
{
    /// <summary>
    /// Handler for the "add reminder" button
    /// </summary>
    /// <param name="sender">source</param>
    /// <param name="e">arguments</param>
    protected void ReminderButton_Click( object sender, EventArgs e )
    {
        string text;
        try
        {
            text = string.Format( "A reminder would have been created for {0} with the message \"{1}\"",
                DateTime.Parse( DateTextBox.Text ).ToLongDateString(), MessageTextBox.Text );
        }
        catch ( FormatException ex )
        {
            text = string.Format( "[Unable to parse \"{0}\": {1}]", DateTextBox.Text, ex.Message );
        }
        Label1.Text = HttpUtility.HtmlEncode( text );
    }

    /// <summary>
    /// Handler for calendar changes
    /// </summary>
    /// <param name="sender">source</param>
    /// <param name="e">arguments</param>
    protected void Calendar1_SelectionChanged( object sender, EventArgs e )
    {
        // Popup result is the selected date
        PopupControlExtender1.Commit( Calendar1.SelectedDate.ToShortDateString() );
    }

    /// <summary>
    /// Handler for radio button changes
    /// </summary>
    /// <param name="sender">source</param>
    /// <param name="e">arguments</param>
    protected void RadioButtonList1_SelectedIndexChanged( object sender, EventArgs e )
    {
        if ( !string.IsNullOrEmpty( RadioButtonList1.SelectedValue ) )
        {
            // Popup result is the selected task
            PopupControlExtender2.Commit( RadioButtonList1.SelectedValue );
        }
        else
        {
            // Cancel the popup
            PopupControlExtender2.Cancel();
        }
        // Reset the selected item
        RadioButtonList1.ClearSelection();
    }
}