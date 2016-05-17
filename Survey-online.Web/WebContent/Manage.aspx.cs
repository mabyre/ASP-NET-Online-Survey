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
using System.Data.SqlClient;
using Sql.Web.Data;
using UserControl.Web.Controls;

partial class Page_WebContentManage : PageBase
{
    static int colonneUtilisateur = 1;
    static int colonneDescription = 3;
    static int colonneDate = 4;

    public string ColorSection( string section )
    {
        if ( SectionExclues.AdminSections().Contains( section ) )
        {
            return "PageSectionExclueStyle";
        }
        return "PageSectionNormalStyle";
    }


    protected void Page_Load( object sender, EventArgs e )
    {
        if ( !IsPostBack )
        {
            if ( User.IsInRole( "Administrateur" ) )
            {
                GridView1.Columns[ colonneUtilisateur ].Visible = true;
            }
            else
            {
                if ( SessionState.MemberInfo != null )
                {
                    SqlDataSourceMembre.SelectParameters[ "Utilisateur" ].DefaultValue = SessionState.MemberInfo.NomUtilisateur;
                    GridView1.DataSourceID = "SqlDataSourceMembre";
                }
            }

            CheckBoxDescription.Checked = SessionState.CheckBox[ "CheckBoxDescription" ];
            GridView1.Columns[ colonneDescription ].Visible = CheckBoxDescription.Checked;
            CheckBoxDate.Checked = SessionState.CheckBox[ "CheckBoxDate" ];
            GridView1.Columns[ colonneDate ].Visible = CheckBoxDate.Checked;
        }
    }

    protected void CheckBoxDescription_CheckedChanged( object sender, EventArgs e )
    {
        SessionState.CheckBox[ "CheckBoxDescription" ] = CheckBoxDescription.Checked;
        GridView1.Columns[ colonneDescription ].Visible = CheckBoxDescription.Checked; 
    }

    protected void CheckBoxDate_CheckedChanged( object sender, EventArgs e )
    {
        SessionState.CheckBox[ "CheckBoxDate" ] = CheckBoxDate.Checked;
        GridView1.Columns[ colonneDate ].Visible = CheckBoxDate.Checked;
    }

    protected void GridView1_Sorting( object sender, GridViewSortEventArgs e )
    {
    }

    protected void GridView1_Sorted( object sender, EventArgs e )
    {
    }

    protected void GridView1_RowUpdating( object sender, GridViewUpdateEventArgs e )
    {
    }
}

