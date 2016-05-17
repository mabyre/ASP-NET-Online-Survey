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

public partial class UserControl_QuestionControl : System.Web.UI.UserControl
{
    protected void Page_Load( object sender, EventArgs e )
    {
        // Il faut le faire tout le temps
        DropDownListTypeQuestionReponse.DataSource = TypeQuestionReponse.List();
        DropDownListTypeQuestionReponse.DataBind();
        DropDownListTypeQuestionReponse.Items.Insert( 0, new ListItem( "Choisir un type de Question", "-1" ) );
        DropDownListTypeQuestionReponse.SelectedValue = TypeQuestionReponse.ChoixSimple;
    }
}
