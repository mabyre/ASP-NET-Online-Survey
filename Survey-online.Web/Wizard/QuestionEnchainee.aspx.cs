//
//

#region Using
using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Sql.Web.Data;
using UserControl.Web.Controls;
using StyleWebData;
#endregion

public partial class Wizard_QuestionEnchainee : System.Web.UI.Page
{
    private static int nombreMaxQuestions = 100;

    // Nombre de questions a enchainer
    private int NombreQuestionEnchainee
    {
        get { return ( int )ViewState[ "NombreQuestionEnchainee" ]; }
        set { ViewState[ "NombreQuestionEnchainee" ] = value; }
    }

    protected void Page_Load( object sender, EventArgs e )
    {
        if ( Page.IsPostBack == false )
        {
            // Choisir le premier Questionnaire a la place de l'utilisateur
            if ( SessionState.Questionnaire == null && SessionState.Questionnaires.Count > 0 )
            {
                SessionState.Questionnaire = SessionState.Questionnaires[ 0 ];
            }

            LabelValidationMessage.Text = "";

            NombreQuestionEnchainee = 3;
            TextBoxCombienDeQuestions.Text = "3";
        }

        BloquerQuestionnaire( SessionState.Questionnaire.Bloque );
        Page.Form.DefaultButton = ButtonCombienDeQuestionsOk.UniqueID; // Pour donner le focus
        CreateControlPanelQuestions();
    }

    private void BloquerQuestionnaire( bool bloque )
    {
        if ( bloque )
        {
            Tools.PageValidation( "Le questionnaire \"" + SessionState.Questionnaire.Description + "\" est clôturé." );
        }
    }

    protected void TextBoxCombienDeQuestions_TextChanged( object sender, EventArgs e )
    {
        ButtonCombienDeQuestionsOk_Click( sender, e );
        ButtonCombienDeQuestionsOk.Click -= new EventHandler( ButtonCombienDeQuestionsOk_Click );
    }

    protected void ButtonCombienDeQuestionsOk_Click( object sender, EventArgs e )
    {
        try
        {
            int nbQuestions = int.Parse( TextBoxCombienDeQuestions.Text );
            if ( nbQuestions < 1 )
            {
                nbQuestions = 1;
            }
            if ( nbQuestions > nombreMaxQuestions )
            {
                nbQuestions = nombreMaxQuestions;
            }
            NombreQuestionEnchainee = nbQuestions;
        }
        catch
        {
        }

        CreateControlPanelQuestions();
    }

    protected void ButtonAjouterQuestion_Click( object sender, EventArgs e )
    {
        LabelValidationMessage.Text = "";
        for ( int i = 1;i <= NombreQuestionEnchainee;i++ )
        {
            string tableQuestionEnchaineeID = "TableQuestionEnchainee" + i.ToString();
            string userControlID = "Question" + i.ToString();
            System.Web.UI.UserControl control = ( System.Web.UI.UserControl )PanelQuestions.FindControl( userControlID );
            Table tableQuestionEnchainee = ( Table )control.FindControl( tableQuestionEnchaineeID );

            // Petite precaution toujours aussi dur d'ecrire ce code
            if ( tableQuestionEnchainee == null )
            {
                break;
            }

            TextBox textBox = ( TextBox )tableQuestionEnchainee.FindControl( "TextBoxQuestionEnchainee" );
            if ( string.IsNullOrEmpty( textBox.Text.Trim() ) )
            {
                LabelValidationMessage.Visible = true;
                LabelValidationMessage.Text += "Donner un libellé pour la Question : " + i.ToString() + "<br/>";
                continue;
            }

            DropDownList dropDownList = ( DropDownList )tableQuestionEnchainee.FindControl( "DropDownListTypeQuestionReponse" );
            if ( dropDownList.SelectedValue == "-1" )
            {
                LabelValidationMessage.Visible = true;
                LabelValidationMessage.Text += "Choisir un type de Question pour la Question : " + textBox.Text + "<br/>";
                continue;
            }

            CheckBox checkBox = ( CheckBox )tableQuestionEnchainee.FindControl( "CheckBoxQuestionObligatoire" );

            PollQuestion question = new PollQuestion();

            // Premiere Question, est-ce un tableau ?
            if ( i == 1 )
            {
                if ( TextBoxTitreTableau.Text.Trim() != "" )
                {
                    question.Tableau = TextBoxTitreTableau.Text.Trim();
                }
                if ( TextBoxPageTableau.Text.Trim() != "" )
                {
                    question.SautPage = TextBoxPageTableau.Text.Trim();
                }
            }

            // Derniere Question, terminer le tableau
            if ( i == NombreQuestionEnchainee )
            {
                if ( TextBoxTitreTableau.Text.Trim() != "" )
                {
                    question.Tableau = Tableau.Fin;
                }
            }

            question.CreationDate = DateTime.Now;
            question.Question = textBox.Text.Trim();
            question.Obligatoire = checkBox.Checked;
            question.ChoixMultiple = TypeQuestionReponse.GetTypeQuestion( dropDownList.SelectedValue );
            question.QuestionnaireID = SessionState.Questionnaire.QuestionnaireID;
            question.MembreGUID = SessionState.Questionnaire.MembreGUID;
            question.Societe = String.Empty;
            question.Instruction = String.Empty;
            question.Message = String.Empty;
            PollQuestionCollection pollQuestions = PollQuestionCollection.GetByQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
            question.Rank = pollQuestions.MaxRank() + 1;

            // Tester les limitations avant d'ajouter la question
            if ( SessionState.Limitations.LimiteQuestions )
            {
                Tools.PageValidation( "La limite du nombre de Questions : " + SessionState.Limitations.NombreQuestions + " est atteinte.<br/>Contactez l'administrateur." );
            }

            int status = PollQuestion.Create( question );
            if ( status != 0 )
            {
                LabelValidationMessage.Visible = true;
                LabelValidationMessage.Text += "Erreur à la création de la Question.<br/>";
            }
            else
            {
                SessionState.Limitations.AjouterQuestion();

                LabelValidationMessage.Visible = true;
                LabelValidationMessage.Text += "Question crée correctement : " + question.Question + "<br/>";
            }

            // Creer les Reponses a la Question
            TextBox textBoxReponses = ( TextBox )tableQuestionEnchainee.FindControl( "TextBoxReponses" );
            if ( string.IsNullOrEmpty( textBoxReponses.Text.Trim() ) == false )
            {
                int rank = 1;
                string[] reponsesSplit = textBoxReponses.Text.Trim().Split( ';' );
                foreach ( string rep in reponsesSplit )
                {
                    PollAnswer reponse = new PollAnswer( rep.Trim() );
                    reponse.PollQuestionId = question.PollQuestionId;
                    reponse.TypeReponse = TypeQuestionReponse.GetTypeReponse( dropDownList.SelectedValue );
                    //reponse.Obligatoire = cbxObligatoire.Checked; on ne sait pas faire
                    reponse.Rank = rank;

                    int status2 = PollAnswer.Create( reponse );
                    rank += 1;
                    if ( status2 != 0 )
                    {
                        LabelValidationMessage.Visible = true;
                        LabelValidationMessage.Text += "Erreur à la création de la Réponse : " + rep + "<br/>";
                    }
                    else
                    {
                        LabelValidationMessage.Visible = true;
                        LabelValidationMessage.Text += "  Réponse crée correctement : " + rep + "<br/>";
                    }
                }
            }
        }
    }

    private void CreateControlPanelQuestions()
    {
        PanelQuestions.Controls.Clear();
        for ( int i = 1;i <= NombreQuestionEnchainee;i++ )
        {
            System.Web.UI.UserControl control = ( System.Web.UI.UserControl )Page.LoadControl( "~/UserControl/QuestionControl.ascx" );
            control.ID = "Question" + i.ToString();
            Table table = ( Table )control.FindControl( "TableQuestionEnchainee" );
            table.ID += i.ToString();
            Label label = ( Label )control.FindControl( "LabelQuestionEnchainee" );
            label.Text += " <font color=\"#5282D4\">" + i.ToString() + "</font> : ";
            PanelQuestions.Controls.Add( control );
            PanelQuestions.Controls.Add( new LiteralControl( "<br/>" ) );
        }
    }

    #region Questionnaire

    protected void ButtonVoirQuestionaireOk_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Poll/List.aspx" );
    }

    protected void DropDownListQuestionnaire_SelectedIndexChanged( object sender, EventArgs e )
    {
        SessionState.Questionnaire = SessionState.Questionnaires[ DropDownListQuestionnaire.SelectedIndex ];
        BloquerQuestionnaire( SessionState.Questionnaire.Bloque );
        CreateControlPanelQuestions();
    }

    #endregion
}
