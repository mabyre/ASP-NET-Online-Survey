// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.


using System;
using AjaxControlToolkit;

public partial class PageTest_AlwaysVisibleMenu : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        // Don't initially hook up the extender
        if ( IsPostBack == false )
            avce.Enabled = false;

    }

    /// <summary>
    /// Update properties of the extender
    /// </summary>
    protected void DropDownListPosition_SelectedIndexChanged( object sender, EventArgs e )
    {
        avce.Enabled = true;
        switch ( DropDownListPosition.SelectedValue[ 0 ] )
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

        switch ( DropDownListPosition.SelectedValue[ 1 ] )
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

    protected void ButtonAlwaysVisible_Click( object sender, EventArgs e )
    {
        int i = 1234;
    }
}