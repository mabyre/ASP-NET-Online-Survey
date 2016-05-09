using System;
using System.Data;
using System.Configuration;
using System.Collections;


// Cet exemple montre qu'a l'evidence pour qu'une GridView se comporte correctement vis a vis
// du Sort elle doit etre liee a un asp:sqldatasource sinon ca ne marche pas !!!!!!!

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Sql.Web.Data;

public partial class PageSortGridViewSample2 : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( !IsPostBack )
        {
            // datasourceid="CustomersSource" 
            //CustomersGridView.DataSource = Personne.CreateDataSource();
            //CustomersGridView.DataBind();
        }
    }

    protected void CustomersGridView_Sorting( Object sender, GridViewSortEventArgs e )
    {
        // Cancel the sorting operation if the user attempts
        // to sort by address.
        if ( e.SortExpression == "Address" )
        {
            e.Cancel = true;
            Message.Text = "You cannot sort by address.";
            SortInformationLabel.Text = "";
        }
        else
        {
            Message.Text = "";
        }
    }

    protected void CustomersGridView_Sorted( Object sender, EventArgs e )
    {
        CustomersGridView.DataBind();

        // Display the sort expression and sort direction.
        SortInformationLabel.Text = "Sorting by " +
          CustomersGridView.SortExpression.ToString() +
          " in " + CustomersGridView.SortDirection.ToString() +
          " order.";
    }
}
