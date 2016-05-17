
// Comme on a montre que pour qu'une GridView se comporte correctement vis a vis
// du Sort elle doit etre liee a un asp:sqldatasource sinon ca ne marche pas !!!!!!!
// La seule chose qu'on peut faire c'est modifier la SqlCommand

using System;
using System.Web;

using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using Sql.Web.Data;

public partial class PageSortGridViewSample3 : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( !IsPostBack )
        {
            DropDownListChoix.SelectedValue = "Nom Prénom";
            CustomersSource.SelectCommand = ChoisirSqlCommand( DropDownListChoix.SelectedValue );
        }

        CustomersGridView.DataBind();
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
        CustomersSource.SelectCommand = ChoisirSqlCommand( DropDownListChoix.SelectedValue );

        // Display the sort expression and sort direction.
        SortInformationLabel.Text = "Sorting by " +
          CustomersGridView.SortExpression.ToString() +
          " in " + CustomersGridView.SortDirection.ToString() +
          " order.";
    }

    string ChoisirSqlCommand( string choix )
    {
        string SqlCommand = "Select [PersonneNom], [PersonnePrenom], [PersonneEmailBureau] From [Personne]";
        switch ( choix )
        {
            case "Nom":
                SqlCommand = "Select [PersonneNom], [PersonneEmailBureau] From [Personne]";
                break;
            case "Nom Prénom":
                SqlCommand = "Select [PersonneNom], [PersonnePrenom], [PersonneEmailBureau] From [Personne]";
                break;
            default:
                SqlCommand = "Select [PersonneNom], [PersonnePrenom], [PersonneEmailBureau] From [Personne]";
                break;
        }

        return SqlCommand;
    }

    protected void DropDownListChoix_SelectedIndexChanged( object sender, EventArgs e )
    {
        CustomersSource.SelectCommand = ChoisirSqlCommand( DropDownListChoix.SelectedValue );
    }
}
