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
using Sql.Web.Data;


public partial class PageSortGridViewSample : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( !IsPostBack )
        {
            // En accrochant ici au lieu de SqlDataSource on genere un evenement :
            // GridView 'CustomersGridView' a déclenché un événement Sorting qui n'était pas géré. 
            //CustomersGridView.DataSource = Personne.GetPersonneAllInDataSet();

            // Et non ca ne pmarche pas non plus !!!!!!!!
            //CustomersGridView.DataSource = Personne.CreateDataSource();
            //CustomersGridView.DataBind();
        }
    }

    protected void SortButton_Click( Object sender, EventArgs e )
    {

        String expression = "";
        SortDirection direction;

        // Create the sort expression from the values selected 
        // by the user from the DropDownList controls. Multiple
        // columns can be sorted by creating a sort expression
        // that contains a comma-separated list of field names.
        expression = SortList1.SelectedValue; // +"," + SortList2.SelectedValue;

        //  Determine the sort direction. The sort direction
        // applies only to the second column sorted.
        switch ( DirectionList.SelectedValue )
        {
            case "Ascending":
                direction = SortDirection.Ascending;
                break;
            case "Descending":
                direction = SortDirection.Descending;
                break;
            default:
                direction = SortDirection.Ascending;
                break;
        }

        // Use the Sort method to programmatically sort the GridView
        // control using the sort expression and direction.
        CustomersGridView.Sort( expression, direction );

    }

    //// NE MARCHE PAS
    //protected void CustomersGridView_Sorting( object sender, GridViewSortEventArgs e )
    //{

    //}
}
