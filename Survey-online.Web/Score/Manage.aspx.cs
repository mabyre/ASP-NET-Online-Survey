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

public partial class Score_Manage : PageBase
{
    private static int NombreMaxScores = 100;

    // Nombre de score pour ce questionnaire
    private int NombreScores
    {
        get { return ( int )ViewState[ "NombreScores" ]; }
        set { ViewState[ "NombreScores" ] = value; }
    }

    private ScoreCollection Scores
    {
        get { return ( ScoreCollection )ViewState[ "Scores" ]; }
        set { ViewState[ "Scores" ] = value; }
    }

    protected void Page_Load( object sender, EventArgs e )
    {
        if ( IsPostBack == false )
        {
            // Choisir le premier Questionnaire a la place de l'utilisateur
            if ( SessionState.Questionnaire == null && SessionState.Questionnaires.Count > 0 )
            {
                SessionState.Questionnaire = SessionState.Questionnaires[ 0 ];
            }

            LabelValidationMessage.Text = "";

            NombreScores = 1;
            TextBoxCombienDeScores.Text = "1";

            // BUG1107090002
            if ( SessionState.Questionnaire == null )
            {
                Tools.PageValidation( "Pas de questionnaire" );
            }

            Scores = ScoreCollection.GetScoreQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
        }

        CreateControlPanelScores();
        Page.Form.DefaultButton = ButtonCombienDeScoresOk.UniqueID; // Pour donner le focus
    }

    protected void TextBoxCombienDeScores_TextChanged( object sender, EventArgs e )
    {
        ButtonCombienDeScoresOk_Click( sender, e );
        ButtonCombienDeScoresOk.Click -= new EventHandler( ButtonCombienDeScoresOk_Click );
    }

    protected void ButtonCombienDeScoresOk_Click( object sender, EventArgs e )
    {
        try
        {
            int nbScores = int.Parse( TextBoxCombienDeScores.Text );
            if ( nbScores < 1 )
            {
                nbScores = 1;
            }
            if ( nbScores > NombreMaxScores )
            {
                nbScores = NombreMaxScores;
            }
            NombreScores = nbScores;
            TextBoxCombienDeScores.Text = NombreScores.ToString();
        }
        catch
        {
        }

        CreateControlPanelScores();
    }

    protected void ButtonAjouterScore_Click( object sender, EventArgs e )
    {
        LabelValidationMessage.Text = "";
        for ( int i = Scores.Count + 1;i <= Scores.Count + NombreScores;i++ )
        {
            string tableScoresID = "TableScore" + i.ToString();
            // BUG29112009
            //string userControlID = "Score" + i.ToString();
            string userControlID = "ScoreToAdd" + i.ToString();
            System.Web.UI.UserControl control = ( System.Web.UI.UserControl )PanelScores.FindControl( userControlID );
            Table tableScores = ( Table )control.FindControl( tableScoresID );

            // Petite precaution toujours aussi dur d'ecrire ce code
            if ( tableScores == null )
            {
                break;
            }

            TextBox textBoxScoreMin = ( TextBox )tableScores.FindControl( "TextBoxScoreMin" );
            int scoreMin = 0;
            if ( string.IsNullOrEmpty( textBoxScoreMin.Text.Trim() ) == false )
            {
                try
                {
                    scoreMin = int.Parse( textBoxScoreMin.Text.Trim() );
                }
                catch
                {
                    LabelValidationMessage.Visible = true;
                    LabelValidationMessage.Text += "Score mininum est un entier<br/>";
                    continue;
                }
            }

            TextBox textBoxScoreMax = ( TextBox )tableScores.FindControl( "TextBoxScoreMax" );
            int scoreMax = 0;
            if ( string.IsNullOrEmpty( textBoxScoreMax.Text.Trim() ) == false )
            {
                try
                {
                    scoreMax = int.Parse( textBoxScoreMax.Text.Trim() );
                }
                catch
                {
                    LabelValidationMessage.Visible = true;
                    LabelValidationMessage.Text += "Score maximum est un entier<br/>";
                    continue;
                }
            }

            if ( scoreMin > scoreMax )
            {
                LabelValidationMessage.Visible = true;
                LabelValidationMessage.Text += "Score minimum est supérieur au Score maximum<br/>";
                continue;
            }

            TextBox textBoxScoreTexte = ( TextBox )tableScores.FindControl( "TextBoxScoreTexte" );
            if ( string.IsNullOrEmpty( textBoxScoreTexte.Text.Trim() ) )
            {
                LabelValidationMessage.Visible = true;
                LabelValidationMessage.Text += "Donnez un texte de validation pour le Score<br/>";
                continue;
            }

            Score score = new Score();
            score.ScoreQuestionnaireID = SessionState.Questionnaire.QuestionnaireID;
            score.ScoreMin = scoreMin;
            score.ScoreMax = scoreMax;
            score.ScoreTexte = textBoxScoreTexte.Text.Trim();
            int status = Score.Create( score );
            if ( status != 1 )
            {
                LabelValidationMessage.Visible = true;
                LabelValidationMessage.Text += "Erreur à la création du Score .<br/>";
            }
            else
            {
                LabelValidationMessage.Visible = true;
                LabelValidationMessage.Text += "Score crée correctement : " + score.ScoreTexte + "<br/>";
            }
        } // fin du for ( int i = 1;i <= NombreScores;i++ )

        Scores = ScoreCollection.GetScoreQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
        CreateControlPanelScores();
    }

    private void CreateControlPanelScores()
    {
        PanelScores.Controls.Clear();

        if ( Scores.Count > 0 )
        {
            foreach ( Score score in Scores )
            {
                System.Web.UI.UserControl control = ( System.Web.UI.UserControl )Page.LoadControl( "~/UserControl/ScoreControl.ascx" );
                control.ID = "Score" + score.ScoreID.ToString();

                HiddenField hiddenField = ( HiddenField )control.FindControl( "HiddenFieldScoreID" );
                hiddenField.Value = score.ScoreID.ToString();

                Table table = ( Table )control.FindControl( "TableScore" );
                table.ID  = "TableScore" + score.ScoreID.ToString();
                table.CssClass = "TableScoreStyle";

                TextBox textBoxScoreMin = ( TextBox )table.FindControl( "TextBoxScoreMin" );
                textBoxScoreMin.Text = score.ScoreMin.ToString();

                TextBox textBoxScoreMax = ( TextBox )table.FindControl( "TextBoxScoreMax" );
                textBoxScoreMax.Text = score.ScoreMax.ToString();

                TextBox textBoxScoreTexte = ( TextBox )table.FindControl( "TextBoxScoreTexte" );
                textBoxScoreTexte.Text = score.ScoreTexte;

                ImageButton imageButtonSupprimer = ( ImageButton )control.FindControl( "ImageButtonSupprimerScore" );
                imageButtonSupprimer.Click += new ImageClickEventHandler( ImageButtonScoreSupprimer_Click );

                ImageButton imageButtonSauver = ( ImageButton )control.FindControl( "ImageButtonSauverScore" );
                imageButtonSauver.Click += new ImageClickEventHandler( ImageButtonScoreSauver_Click );

                PanelScores.Controls.Add( control );
                PanelScores.Controls.Add( new LiteralControl( "<br/>" ) );
            }
        }

        // Scores a ajouter
        for ( int i = Scores.Count + 1;i <= Scores.Count + NombreScores;i++ )
        {
            System.Web.UI.UserControl control = ( System.Web.UI.UserControl )Page.LoadControl( "~/UserControl/ScoreControl.ascx" );
            // BUG29112009
            //control.ID = "Score" + i.ToString();
            control.ID = "ScoreToAdd" + i.ToString();
            Table table = ( Table )control.FindControl( "TableScore" );
            table.ID += i.ToString();

            ImageButton imageButtonSupprimer = ( ImageButton )control.FindControl( "ImageButtonSupprimerScore" );
            imageButtonSupprimer.Visible = false;

            ImageButton imageButtonSauver = ( ImageButton )control.FindControl( "ImageButtonSauverScore" );
            imageButtonSauver.Visible = false;

            PanelScores.Controls.Add( control );
            PanelScores.Controls.Add( new LiteralControl( "<br/>" ) );
        }
    }

    protected void ImageButtonScoreSauver_Click( object sender, EventArgs e )
    {
        ImageButton imageButton = ( ImageButton )sender;
        HiddenField hiddenField = ( HiddenField )imageButton.Parent.FindControl( "HiddenFieldScoreID" );
        Score score = new Score();
        score.ScoreID = int.Parse( hiddenField.Value );
        Score scoreOldValues = Scores.FindByScoreID( score.ScoreID );
        score = scoreOldValues;

        TextBox textBoxScoreMin = ( TextBox )imageButton.Parent.FindControl( "TextBoxScoreMin" );
        try
        {
            score.ScoreMin = int.Parse( textBoxScoreMin.Text );
        }
        catch
        {
            score.ScoreMin = scoreOldValues.ScoreMin;
        }

        TextBox textBoxScoreMax = ( TextBox )imageButton.Parent.FindControl( "TextBoxScoreMax" );
        try
        {
            score.ScoreMax = int.Parse( textBoxScoreMax.Text );
        }
        catch
        {
            score.ScoreMax = scoreOldValues.ScoreMax;
        }

        TextBox textBoxScoreTexte = ( TextBox )imageButton.Parent.FindControl( "TextBoxScoreTexte" );
        if ( string.IsNullOrEmpty( textBoxScoreTexte.Text.Trim() ) == false )
        {
            score.ScoreTexte = textBoxScoreTexte.Text;
        }
        else
        {
            score.ScoreTexte = scoreOldValues.ScoreTexte;
        }

        Score.Update( score );

        Scores = ScoreCollection.GetScoreQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
        CreateControlPanelScores();
    }

    protected void ImageButtonScoreSupprimer_Click( object sender, EventArgs e )
    {
        ImageButton imageButton = ( ImageButton )sender;
        HiddenField hiddenField = ( HiddenField )imageButton.Parent.FindControl( "HiddenFieldScoreID" );
        
        Score.Delete( int.Parse( hiddenField.Value ) );

        Scores = ScoreCollection.GetScoreQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
        CreateControlPanelScores();
    }

    #region Questionnaire

    protected void DropDownListQuestionnaire_SelectedIndexChanged( object sender, EventArgs e )
    {
        SessionState.Questionnaire = SessionState.Questionnaires[ DropDownListQuestionnaire.SelectedIndex ];
        Scores = ScoreCollection.GetScoreQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
        CreateControlPanelScores();
    }

    #endregion
}
