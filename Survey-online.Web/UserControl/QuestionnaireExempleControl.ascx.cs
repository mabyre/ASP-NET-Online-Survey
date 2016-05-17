using System;
using System.Collections;
using System.Configuration;
using System.Data;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using Sql.Web.Data;

public partial class UserControl_QuestionnaireExempleControl : System.Web.UI.UserControl
{
    // AME20100330
    protected override void OnPreRender( EventArgs e )
    {
        Trace.Warn( "UserControl_QuestionnaireExempleControl:OnPreRender" );
        base.OnPreRender( e );

        if ( Page.IsPostBack == false )
        {
            // Ajouter les questionnaires du membre dans la liste en bleu
            // Pour qu'il puisse s'en servir comme exemples
            QuestionnaireCollection qc = QuestionnaireCollection.GetQuestionnaireMembre( SessionState.MemberInfo.MembreGUID );
            foreach ( Questionnaire q in qc )
            {
                ListItem li = new ListItem( q.Description + ":" + q.CodeAcces );
                li.Attributes.Add( "style", "color:blue" );
                DropDownListQuestionnaire.Items.Add( li );
            }
        }
    }

    protected void Page_Load( object sender, System.EventArgs e )
    {
        if ( Page.IsPostBack == false )
        {
            PanelQuestionnaireExempleControl.Visible = false;
            MemberInfo member = MemberInfo.GetMemberInfo( "Intervieweur" );
            if ( member != null )
            {
                DropDownListQuestionnaire.SelectedMembreGUID = member.MembreGUID;
                DropDownListQuestionnaire.DefaultText = "Choisir un Questionnaire existant";
                DropDownListQuestionnaire.SelectedQuestionnaire = "-1";
            }
            else
            {
                DropDownListQuestionnaire.DefaultText = "Pas d'Intervieweur";
                DropDownListQuestionnaire.SelectedMembreGUID = Guid.Empty;
            }

            if ( SessionState.Questionnaire != null )
            {
                LabelValider.Visible = SessionState.Questionnaire.Valider;
                LabelFin.Visible = SessionState.Questionnaire.Fin;
                LabelBloque.Visible = SessionState.Questionnaire.Bloque;
                BuildDataList();
            }
        }
    }

    private void BuildDataList()
    {
        PollQuestionCollection pollQuestions = PollQuestionCollection.GetByQuestionnaire( SessionState.Questionnaire.QuestionnaireID );

        DataListQuestion.DataSource = pollQuestions;
        DataListQuestion.DataBind();

        // Trouver les reponses
        foreach ( DataListItem dli in DataListQuestion.Items )
        {
            DataList dl = new DataList();
            dl = ( DataList )dli.FindControl( "DataListReponse" );

            HiddenField hf = new HiddenField();
            hf = ( HiddenField )dli.FindControl( "PollQuestionId" );
            Guid pollQuestionId = new Guid( hf.Value );
            PollAnswerCollection answers = PollAnswerCollection.GetByPollQuestionID( pollQuestionId );

            dl.DataSource = answers;
            dl.DataBind();
        }
    }

    protected void DropDownListQuestionnaire_SelectedIndexChanged( object sender, EventArgs e )
    {
        if ( DropDownListQuestionnaire.SelectedQuestionnaire != "-1" )
        {
            int codeAcces = int.Parse( DropDownListQuestionnaire.SelectedCodeAcces );
            SessionState.Questionnaire = Questionnaire.GetByCodeAcces( codeAcces );
            PanelQuestionnaireExempleControl.Visible = true;
            BuildDataList();
        }
        else
        {
            PanelQuestionnaireExempleControl.Visible = false;
        }
    }

    protected void RolloverButtonTestez_Click( object sender, EventArgs e )
    {
        if ( SessionState.Questionnaire != null )
        {
            Response.Redirect( "~/Poll/Questionnaire.aspx?QuestionnaireID=" + SessionState.Questionnaire.QuestionnaireID.ToString(), true );
        }
    }

    protected void ButtonCopier_Click( object sender, EventArgs e )
    {
        if ( SessionState.MemberInfo != null )
        {
            if ( DropDownListQuestionnaire.SelectedQuestionnaire != "-1" )
            {
                // Calculer un nouveau CodeAcces avant de pouvoir Copier le Questionnaire
                ArrayList codes = QuestionnaireDAL.GetCodeAccessAll();
                string codeAcces = Tools.CalculCodeAcces( SessionState.MemberInfo.MembreID, codes ).ToString();

                Response.Redirect( "~/Questionnaire/Copier.aspx?QuestionnaireID=" + SessionState.Questionnaire.QuestionnaireID.ToString() + "&MembreGUID=" + SessionState.MemberInfo.MembreGUID.ToString() + "&CodeAcces=" + codeAcces );
            }
        }
    }

    protected void ButtonAjouter_Click( object sender, EventArgs e )
    {
        if ( SessionState.MemberInfo != null )
        {
            if ( DropDownListQuestionnaire.SelectedQuestionnaire != "-1" )
            {
                if ( PanelQuestionnaireExistant.Visible == false )
                {
                    PanelQuestionnaireExistant.Visible = true;
                    ButtonCopier.Visible = false;
                }
                else // l'utilisateur peut copier des questions dans un Questionnaire existant
                {
                    if ( DropDownListQuestionnaireExistant.SelectedQuestionnaire != "-1" )
                    {
                        Response.Redirect( "~/Questionnaire/Copier.aspx?QuestionnaireID=" + SessionState.Questionnaire.QuestionnaireID.ToString() + "&MembreGUID=" + SessionState.MemberInfo.MembreGUID.ToString() + "&QuestionnaireExistantID=" + DropDownListQuestionnaireExistant.QuestionnaireID.ToString() );
                    }
                }
            }
        }
    }
}
