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

public partial class UserControl_WebContent : System.Web.UI.UserControl
{
    private string _Section = "";
    public string Section
    {
        get { return _Section; }
        set { _Section = value ; }
    }

    protected void Page_Load( object sender, System.EventArgs e )
    {
        if ( Section == null )
        {
            LabelContent.Text = "Il faut un nom de section pour le control WebContent.";
        }
        else
        {
            try
            {
                HiddenFieldSectionName.Value = _Section;
                string utilisateur = WebContent.GetUtilisateur();
                string visualiseur = WebContent.GetVisualiseur();
                WebContent web = WebContent.GetWebContent( _Section, utilisateur, visualiseur );

                if ( web != null && WebContent.CanEdit() )
                {
                    HyperlinkEdit.Visible = true;
                    string url = "~\\WebContent\\Edit.aspx?id=" + web.WebContentID.ToString(); //~\\WebContent\\Edit.aspx?sectionname=" + HiddenFieldSectionName.Value;
                    url += "&ReturnURL=";
                    url += System.Web.HttpUtility.UrlEncode( Request.RawUrl );
                    HyperlinkEdit.NavigateUrl = url;
                }

                // Il n'y a pas de WebContent pour cet Utilisateur et ce Visualiseur
                if ( web == null )
                {
                    // Y a t-il une page pour "Tout le monde" pour cet Utilisateur ?
                    web = WebContent.GetWebContent( _Section, utilisateur, WebContent.ToutLeMonde );
                }

                // Il n'y a pas de WebContent "Tout le monde" pour cet Utilisateur
                if ( web == null )
                {
                    // Y a t-il une page "Admin" pour tout le monde ?
                    web = WebContent.GetWebContent( _Section, WebContent.Admin, WebContent.ToutLeMonde );
                }

                // Section non créée, il faut la créer vide car sinon personne ne pourra le faire !
                if ( web == null )
                {
                    web = new WebContent();
                    web.Section = _Section;
                    web.SectionContent = "<p>" + _Section + "</p>";
                    web.Utilisateur = WebContent.Admin;
                    web.Visualisateur = WebContent.ToutLeMonde;
                    WebContent.Create( web );
                    LabelContent.Text = _Section;
                }
                else
                {
                    LabelContent.Text = web.SectionContent;
                }
            }
            catch ( Exception ex )
            {
                // Uri
                string uri = HttpUtility.UrlEncode( ex.Message );
                Response.Redirect( Tools.PageErreurPath + uri );
            }

        }
    }
}
