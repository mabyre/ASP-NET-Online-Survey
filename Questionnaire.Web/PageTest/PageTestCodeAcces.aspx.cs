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

public partial class PageTestCodeAcces : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    // nb representera le nombre de questionnaires par Membre
    private int CalculCodeAcces( int seed, int nb )
    {
      Random rand = new Random( seed );

      int next = rand.Next( 999, 9999 );
      next += nb;

      return next;
    }

    protected void ButtonCalculer_Click( object sender, EventArgs e )
    {
        int seed = int.Parse( TextBoxSeed.Text );
        int nb = int.Parse( TextBoxNb.Text );

        LabelResultat.Text = CalculCodeAcces( seed, nb ).ToString();
    }
}
