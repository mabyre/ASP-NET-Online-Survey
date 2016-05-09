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
using UserControl.Web.Controls;
using AjaxControlToolkit; // creer dynamiquement ?!

public partial class PageTest_PageTestCheckBox : System.Web.UI.Page
{
    private RadioButtonExtender RadioButtonExtender1 = new RadioButtonExtender();
    private RadioButtonExtender RadioButtonExtender2 = new RadioButtonExtender();

    protected void Page_Load(object sender, EventArgs e)
    {
        if ( IsPostBack == false )
        {
            RadioButtonList1.Items.Add( "Choix 1" );
            RadioButtonList1.Items.Add( "Choix 2" );
            RadioButtonList1.Items.Add( "Choix 3" );
            RadioButtonList1.Items.Add( "Choix 4" );
            RadioButtonList1.Items.Add( "Choix 5" );
            RadioButtonList1.Items.Add( "Choix 6" );
        }

        RadioButtonExtender1.Add( RadioButton1 );
        RadioButtonExtender1.Add( RadioButton2 );
        RadioButtonExtender1.Add( RadioButton3 );
    }

    protected override void CreateChildControls()
    {
        CreateChildControlsPanelRadioButtonList();

        CreateChildControlsPanelRadioButtonExclusiveCheckBoxExtender();
    }

    private void CreateChildControlsPanelRadioButtonList()
    {
        Table tableRadioButtonList = new Table();
        tableRadioButtonList.CellPadding = 15;
        tableRadioButtonList.CellSpacing = 15;
        TableCell cell = new TableCell();
        TableRow row = new TableRow();

        RadioButton rb1 = new RadioButton();
        rb1.AutoPostBack = true;
        rb1.Text = "Radio 1";
        rb1.CheckedChanged += new EventHandler( RadioButton_CheckedChanged );
        cell.Controls.Add( rb1 );
        row.Cells.Add( cell );
        cell = new TableCell();

        RadioButton rb2 = new RadioButton();
        rb2.Text = "Radio 2";
        rb2.AutoPostBack = true;
        rb2.CheckedChanged += new EventHandler( RadioButton_CheckedChanged );
        cell.Controls.Add( rb2 );
        row.Cells.Add( cell );
        cell = new TableCell();

        RadioButton rb3 = new RadioButton();
        rb3.AutoPostBack = true;
        rb3.Text = "Radio 3";
        rb3.CheckedChanged += new EventHandler( RadioButton_CheckedChanged );
        cell.Controls.Add( rb3 );
        row.Cells.Add( cell );
        cell = new TableCell();

        RadioButtonExtender2.Add( rb1 );
        RadioButtonExtender2.Add( rb2 );
        RadioButtonExtender2.Add( rb3 );

        tableRadioButtonList.Rows.Add( row );
        PanelRadioButtonList.Controls.Add( tableRadioButtonList );
    }

    protected void RadioButtonList1_SelectedIndexChanged( object sender, EventArgs e )
    {

    }

    protected void RadioButton_CheckedChanged( object sender, EventArgs e )
    {
        RadioButton rb = (RadioButton)sender;
        PanelRadioButtonList.Controls.Add( new LiteralControl( rb.Text ) );
    }

    private void CreateChildControlsPanelRadioButtonExclusiveCheckBoxExtender()
    {
        Table tableRadioButtonList = new Table();
        tableRadioButtonList.CellPadding = 15;
        tableRadioButtonList.CellSpacing = 15;
        TableCell cell = new TableCell();
        TableRow row = new TableRow();

        RadioButton rb1 = new RadioButton();
        //rb1.AutoPostBack = true;
        rb1.Text = "Radio 1";
        rb1.ID = "RadioButtonMEE1";

        MutuallyExclusiveCheckBoxExtender mece1 = new MutuallyExclusiveCheckBoxExtender();
        mece1.TargetControlID = "RadioButtonMEE1";
        mece1.Key = "RadioButtonExclusive";

        cell.Controls.Add( rb1 );
        cell.Controls.Add( mece1 );
        row.Cells.Add( cell );
        cell = new TableCell();

        RadioButton rb2 = new RadioButton();
        rb2.Text = "Radio 2";
        //rb2.AutoPostBack = true;
        rb2.ID = "RadioButtonMEE2";

        MutuallyExclusiveCheckBoxExtender mece2 = new MutuallyExclusiveCheckBoxExtender();
        mece2.TargetControlID = "RadioButtonMEE2";
        mece2.Key = "RadioButtonExclusive";

        cell.Controls.Add( rb2 );
        cell.Controls.Add( mece2 );
        row.Cells.Add( cell );
        cell = new TableCell();

        RadioButton rb3 = new RadioButton();
        //rb3.AutoPostBack = true;
        rb3.Text = "Radio 3";
        rb3.ID = "RadioButtonMEE3";

        MutuallyExclusiveCheckBoxExtender mece3 = new MutuallyExclusiveCheckBoxExtender();
        mece3.TargetControlID = "RadioButtonMEE3";
        mece3.Key = "RadioButtonExclusive";

        cell.Controls.Add( rb3 );
        cell.Controls.Add( mece3 );
        row.Cells.Add( cell );
        cell = new TableCell();

        //RadioButtonExtender2.Add( rb1 );
        //RadioButtonExtender2.Add( rb2 );
        //RadioButtonExtender2.Add( rb3 );

        tableRadioButtonList.Rows.Add( row );
        PanelRadioButtonExclusiveCheckBoxExtender.Controls.Add( tableRadioButtonList );
    }

    protected void ButtonOk_Click( object sender, EventArgs e )
    {
        RadioButton rb = ( RadioButton )PanelRadioButtonExclusiveCheckBoxExtender.FindControl( "RadioButtonMEE1" );
        if ( rb.Checked )
        {
            PanelRadioButtonExclusiveCheckBoxExtender.Controls.Add( new LiteralControl( rb.Text ) );
        }
        rb = ( RadioButton )PanelRadioButtonExclusiveCheckBoxExtender.FindControl( "RadioButtonMEE2" );
        if ( rb.Checked )
        {
            PanelRadioButtonExclusiveCheckBoxExtender.Controls.Add( new LiteralControl( rb.Text ) );
        }
        rb = ( RadioButton )PanelRadioButtonExclusiveCheckBoxExtender.FindControl( "RadioButtonMEE3" );
        if ( rb.Checked )
        {
            PanelRadioButtonExclusiveCheckBoxExtender.Controls.Add( new LiteralControl( rb.Text ) );
        }
    }
}
