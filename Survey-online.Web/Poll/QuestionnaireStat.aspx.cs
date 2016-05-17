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

public partial class Poll_QuestionnaireStat : PageBase
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( !IsPostBack )
        {
            if ( User.Identity.IsAuthenticated )
            {
                PollQuestionCollection pollQuestions = new PollQuestionCollection();
                if ( User.IsInRole( "Administrateur" ) )
                {
                    pollQuestions = PollQuestionCollection.GetAll();
                    DropDownListQuestionnaires.Items.Add( "Tous les questionnaires" );
                    foreach ( Questionnaire questionnaire in SessionState.Questionnaires )
                    {
                        DropDownListQuestionnaires.Items.Add( questionnaire.Description );
                    }
                }

                if ( SessionState.Questionnaire != null )
                    DropDownListQuestionnaires.SelectedValue = SessionState.Questionnaire.Description;
            }
        }
    }

    protected void DropDownListQuestionnaires_SelectedIndexChanged( object sender, EventArgs e )
    {
        if ( DropDownListQuestionnaires.SelectedValue != "Tous les questionnaires" )
        {
            Questionnaire questionnaire = SessionState.Questionnaires[ DropDownListQuestionnaires.SelectedIndex - 1 ];
            Response.Redirect( "~/Poll/QuestionnaireStat.aspx?QuestionnaireID=" + questionnaire.QuestionnaireID.ToString() );
        }
    }

    protected void ButtonStatistiques_Click( object sender, EventArgs e )
    {
        if ( DropDownListQuestionnaires.SelectedValue != "Tous les questionnaires" )
        {
            Questionnaire questionnaire = SessionState.Questionnaires[ DropDownListQuestionnaires.SelectedIndex - 1 ];
            Response.Redirect( "~/Poll/QuestionnaireStat.aspx?QuestionnaireID=" + questionnaire.QuestionnaireID.ToString() );
        }
    }
}
