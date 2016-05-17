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

public partial class PageTest1 : System.Web.UI.Page
{
    static int CheckBoxListSelectedIndex = 0;

    protected void Page_Load( object sender, EventArgs e )
    {
        Trace.Warn( string.Format( "Page_Load : {0}", ( IsPostBack == true ? "True" : "False" )) );
        LabelText.Text = "Page_Load : " + ( IsPostBack == true ? "True" : "False" ) + "<br/>";
        int idx = 0;
        foreach ( ListItem li in CheckBoxList1.Items )
        {
            string str = CheckBoxList1.Items[ idx ].Selected == true ? "Vrai" : "Faux";
            LabelText.Text += CheckBoxList1.Items[ idx ].Value + " " + str + "<br/>";
            idx++;
        }

        Trace.Warn( string.Format( "CheckBoxListSelectedIndex : {0}", CheckBoxListSelectedIndex.ToString() ) );
        Trace.Warn( string.Format( "CheckBoxList1.SelectedIndex : {0}", CheckBoxList1.SelectedIndex.ToString() ) );


        if ( !IsPostBack )
        {
            CheckBoxListExclusive1.Items.Add( "Choix ex 1" );
            CheckBoxListExclusive1.Items.Add( "Choix ex 2" );
            CheckBoxListExclusive1.Items.Add( "Choix ex 3" );
        }

        LabelTextExclusif.Text = "CheckBoxListExclusive1 Value : " + CheckBoxListExclusive1.Items[ CheckBoxListExclusive1.TrueSelectedIndex ].Value + "<br />";
        LabelTextExclusif.Text += "          TrueSelectedIndex : " + CheckBoxListExclusive1.TrueSelectedIndex.ToString() + "<br />";
    }

    protected override void OnSaveStateComplete( EventArgs e )
    {
        base.OnSaveStateComplete( e );

        Trace.Warn( string.Format( "CheckBoxListSelectedIndex : {0}", CheckBoxListSelectedIndex.ToString() ) );
        Trace.Warn( string.Format( "CheckBoxList1.SelectedIndex : {0}", CheckBoxList1.SelectedIndex.ToString() ) );
    }

    // Ca marche pas on a un coup de retard parcequ'il faut construire l'objet avant 
    // d'appeler la class de base
    protected void CheckBoxList1_SelectedIndexChanged( object sender, EventArgs e )
    {
        // CheckBoxList exclusive avec choix 0 par defaut
        if ( CheckBoxList1.SelectedIndex == -1 )
        {
            CheckBoxList1.Items[ 0 ].Selected = true;
            CheckBoxListSelectedIndex = 0;
            Trace.Warn( string.Format( "CheckBoxListSelectedIndex : {0}", CheckBoxListSelectedIndex ) );
        }
        else
        {
            for ( int i = 0;i < CheckBoxList1.Items.Count;i++ )
            {
                ListItem li = CheckBoxList1.Items[ i ];
                if ( li.Selected && ( li.Value != CheckBoxList1.Items[ CheckBoxListSelectedIndex ].Value ) )
                {
                    CheckBoxList1.Items[ CheckBoxListSelectedIndex ].Selected = false;
                    CheckBoxListSelectedIndex = i;
                    Trace.Warn( string.Format( "CheckBoxListSelectedIndex : {0}", CheckBoxListSelectedIndex )  );
                }
            }
        }
    }

    protected void CheckBoxListExclusive1_SelectedIndexChanged( object sender, EventArgs e )
    {
    }

    protected void ButtonChoixCheckBoxList1_Click( object sender, EventArgs e )
    {
        LabelText.Text = "Choix : " + CheckBoxList1.SelectedItem.Value;
    }

    protected void ButtonCheckBoxListExclusive1_Click( object sender, EventArgs e )
    {
        if ( CheckBoxListExclusive1.SelectedItem != null )
            LabelTextExclusif.Text = "Choix exclusif : " + CheckBoxListExclusive1.SelectedItem.Value;
        else
            LabelTextExclusif.Text = "null";
    }

    protected void ButtonPostBack_Click( object sender, EventArgs e )
    {
        Server.Transfer( Request.RawUrl );
    }

}
