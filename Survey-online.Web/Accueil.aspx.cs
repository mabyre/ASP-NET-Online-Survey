//
// Cette page etait bien pourrie, elle date du temps ou elle authentifiait tout le monde
// Maintenant elle ne sert plus qu'a l'accueil les interviewes et a presenter la page
// d'accueil configuree par l'intervieweur. 
//

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

public partial class Accueil : PageBase
{
    private bool anonymatViewState
    {
        get { return ( bool )ViewState[ "anonymatViewState" ]; }
        set { ViewState[ "anonymatViewState" ] = value; }
    }

    protected void Page_Load( object sender, EventArgs e )
    {
        if ( IsPostBack == false )
        {
            // Va te faire logguer
            if ( SessionState.Personne == null )
            {
                try
                {
                    Response.Redirect( "~/Member/Login.aspx" );
                }
                catch ( Exception ex )
                {
                    string msg = ex.Message;
                }
            }

            Questionnaire questionnaire = Questionnaire.GetQuestionnaire( SessionState.Personne.QuestionnaireID );
            if ( questionnaire.Bloque )
            {
                Tools.PageValidation( "Le questionnaire \"" + questionnaire.Description + "\" est clôturé." );
            }
            anonymatViewState = questionnaire.Anonymat;

            RolloverButtonRepondez.Visible = true;
        }
    }

    // AME25062010
    // S'occuper des meta mots du WebContent
    protected override void OnPreRenderComplete( EventArgs e )
    {
        Label labelContent = ( Label )WebContentPageAccueil.FindControl( "LabelContent" );

        if ( anonymatViewState )
        {
            labelContent.Text = labelContent.Text.Replace( "%%CIVILITE%%", string.Empty );
            labelContent.Text = labelContent.Text.Replace( "%%NOM%%", string.Empty );
            labelContent.Text = labelContent.Text.Replace( "%%PRENOM%%", string.Empty );
            labelContent.Text = labelContent.Text.Replace( "%%SOCIETE%%", string.Empty );
        }
        else
        {
            labelContent.Text = labelContent.Text.Replace( "%%CIVILITE%%", SessionState.Personne.Civilite );
            labelContent.Text = labelContent.Text.Replace( "%%NOM%%", SessionState.Personne.Nom );
            labelContent.Text = labelContent.Text.Replace( "%%PRENOM%%", SessionState.Personne.Prenom );
            labelContent.Text = labelContent.Text.Replace( "%%SOCIETE%%", SessionState.Personne.Societe );
        }

        base.OnPreRenderComplete( e );
    }

    protected void RolloverButtonRepondez_Click( object sender, System.EventArgs e )
    {
        // Charger les Questions du Questionnaire
        // Regarder si il y a au moins une Question pour ce Questionnaire        
        SessionState.Questions = PollQuestionCollection.GetByQuestionnaire( SessionState.Personne.QuestionnaireID );
        if ( SessionState.Questions.Count > 0 )
        {
            Response.Redirect( "~/Poll/Questionnaire.aspx?QuestionnaireID=" + SessionState.Personne.QuestionnaireID.ToString(), false );
        }
        else
        {
            Response.Redirect( Tools.PageErreurPath + "Désolé mais il n'y a pas de Questions pour ce Questionnaire.", false );
        }
    }
}
