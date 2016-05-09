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

public partial class PageTest_PageValiderEmailRegEx : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        LabelValidation.Text = "Entrer un email";
        LabelValidation.CssClass = "LabelValidationMessageStyle";
    }

    protected void ButtonOk_Click( object sender, EventArgs e )
    {
        LabelValidation.Text = "";
        LabelValidation.CssClass = "LabelValidationMessageStyle";
        LabelValidation.Visible = true;

        int emailNonValide = 0;
        int emailValide = 0;

        string contactSupprimer = TextBoxContactAjouter.Text;
        string separateurDOS = "\r\n";
        string separateurUnix = "\r";
        if ( contactSupprimer.Contains( separateurDOS ) == false )
        {
            if ( contactSupprimer.Contains( separateurUnix ) == true )
            {
                LabelValidation.Text += "Contacts au format Unix à supprimer";
                contactSupprimer = Strings.UnixToDos( contactSupprimer );
            }
        }

        string[] separateur = { separateurDOS };
        string[] contacts = TextBoxContactAjouter.Text.Split( separateur, System.StringSplitOptions.RemoveEmptyEntries );

        foreach ( string contact in contacts )
        {
            string contactTrim = contact.Trim();

            if ( Strings.IsValideEmail( contactTrim ) )
            {
                //LabelValidation.Text += contactTrim + " est un email valide<br/>";
                emailValide += 1;
            }
            else
            {
                emailNonValide += 1;
                LabelValidation.Text += contactTrim + " n'est pas un email valide<br/>";
            }

        }// fin du foreach ( string contact in contacts )

        LabelValidation.Text += "Email non valides : " + emailNonValide.ToString() + "<br/>";
        LabelValidation.Text += "Email valides : " + emailValide.ToString() + "<br/>";
    }
}
