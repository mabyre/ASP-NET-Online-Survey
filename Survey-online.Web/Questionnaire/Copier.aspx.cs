//
// Copier un Questionnaire Existant pour un autre Utilisateur
// Copier les Questions d'un Questionnaire pour un Questionnaire deja Existant
// la distinction se fait par l'existance de QuestionnaireExistantID
//
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Sql.Web.Data;
using TraceReporter;

public partial class Questionnaire_Copier : PageBase
{
    // BUG10092009 remplace par l'utilisation du cache
    //private int QuestionnaireID
    //{
    //    get 
    //    { 
    //        if ( ViewState[ "QuestionnaireID" ] == null )
    //        {
    //            ViewState[ "QuestionnaireID" ] = 0;
    //        }
    //        return ( int )ViewState[ "QuestionnaireID" ]; 
    //    }
    //    set { ViewState[ "QuestionnaireID" ] = value; }
    //}

    //private int QuestionnaireExistantID
    //{
    //    get
    //    {
    //        if ( ViewState[ "QuestionnaireExistantID" ] == null )
    //        {
    //            ViewState[ "QuestionnaireExistantID" ] = 0;
    //        }
    //        return ( int )ViewState[ "QuestionnaireExistantID" ];
    //    }
    //    set { ViewState[ "QuestionnaireExistantID" ] = value; }
    //}

    //private Guid MembreGUID
    //{
    //    get
    //    {
    //        if ( ViewState[ "MembreGUID" ] == null )
    //        {
    //            ViewState[ "MembreGUID" ] = Guid.Empty;
    //        }
    //        return ( Guid )ViewState[ "MembreGUID" ];
    //    }
    //    set { ViewState[ "MembreGUID" ] = value; }
    //}

    //private int CodeAcces
    //{
    //    get
    //    {
    //        if ( ViewState[ "CodeAcces" ] == null )
    //        {
    //            ViewState[ "CodeAcces" ] = 0;
    //        }
    //        return ( int )ViewState[ "CodeAcces" ];
    //    }
    //    set { ViewState[ "CodeAcces" ] = value; }
    //}

    protected void Page_Load( object sender, System.EventArgs e )
    {
        if ( IsPostBack == false )
        {
            if ( Request.QueryString[ "MembreGUID" ] != null )
            {
                Cache[ "MembreGUID" ] = new Guid( Request.QueryString[ "MembreGUID" ].ToString() );
            }
            else
            {
                ValidationMessage.Text += "Erreur pas de Questionnaire à copier !<br/>";
                ValidationMessage.Visible = true;
            }

            Cache[ "CodeAcces" ] = 0;
            if ( Request.QueryString[ "CodeAcces" ] != null )
            {
                Cache[ "CodeAcces" ] = int.Parse( Request.QueryString[ "CodeAcces" ].ToString() );
            }

            Cache[ "QuestionnaireExistantID" ] = 0;
            if ( Request.QueryString[ "QuestionnaireExistantID" ] != null )
            {
                Cache[ "QuestionnaireExistantID" ] = int.Parse( Request.QueryString[ "QuestionnaireExistantID" ].ToString() );
            }

            if ( ( int )Cache[ "CodeAcces" ] == 0 && (( int )Cache[ "QuestionnaireExistantID" ]) == 0 )
            {
                ValidationMessage.Text += "Erreur pas de code d'accès !<br/>";
                ValidationMessage.Text += "Pas de Questionnaire à copier !<br/>";
                ValidationMessage.Visible = true;
            }

            if ( Request.QueryString[ "QuestionnaireID" ] != null )
            {
                Cache[ "QuestionnaireID " ] = int.Parse( Request.QueryString[ "QuestionnaireID" ] );
                Questionnaire questionnaire = Questionnaire.GetQuestionnaire( ( int )Cache[ "QuestionnaireID " ] );
                ValidationMessage.Text += "Copie du Questionnaire : " + questionnaire.Description + ":" + questionnaire.CodeAcces + "<br />";

                MemberInfo membre = MemberInfo.GetMemberInfo( ( Guid )Cache[ "MembreGUID" ] );
                if ( User.IsInRole( "Administrateur" ) )
                {
                    ValidationMessage.Text += "Pour le membre : " + membre.NomUtilisateur + "/" + membre.Nom + "/" + membre.Prenom + "<br />";
                }
                if ( ( int )Cache[ "CodeAcces" ] != 0 && ( int )Cache[ "CodeAcces" ] != questionnaire.CodeAcces && ( int )Cache[ "QuestionnaireExistantID" ] == 0 )
                {
                    ValidationMessage.Text += "Nouveau code d'accès : " + ( int )Cache[ "CodeAcces" ] + "<br />";
                }
                ValidationMessage.Text += "<br />";

                Cache["Questions"] = PollQuestionCollection.GetByQuestionnaire( ( int )Cache[ "QuestionnaireID " ] );
                foreach ( PollQuestion question in (PollQuestionCollection)Cache[ "Questions" ] )
                {
                    ValidationMessage.Text += "- Question : " + question.Question + "<br />";
                    PollAnswerCollection reponses = PollAnswerCollection.GetByPollQuestionID( question.PollQuestionId );
                    foreach ( PollAnswer reponse in reponses )
                    {
                        ValidationMessage.Text += "-- Réponse : " + reponse.Answer + "<br />";
                    }
                }

                if ( ( int )Cache[ "QuestionnaireExistantID" ] != 0 )
                {
                    questionnaire = Questionnaire.GetQuestionnaire( ( int )Cache[ "QuestionnaireExistantID" ] );
                    ValidationMessage.Text += "<br />";
                    ValidationMessage.Text += "Pour le Questionnaire : " + questionnaire.Description + ":" + questionnaire.CodeAcces + "<br />";
                }

                ValidationMessage.Visible = true;
            }
        }
    }

    protected void ButtonCancel_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Questionnaire/Manage.aspx" );
    }

    protected void ButtonAjouterQuestion_Click( object sender, EventArgs e )
    {
        Response.Redirect( "~/Wizard/Question.aspx", true );
    }

    protected void ButtonCopier_Click( object sender, EventArgs e )
    {
        Reporter.Trace( "ButtonCopier_Click" );

        if ( ( int )Cache[ "QuestionnaireID " ] == 0 )
        {
            ValidationMessage.Text += "Choisir un questionnaire à copier.<br/>";
            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            ValidationMessage.Visible = true;
        }
        else
        {
            int status = 0;
            MemberInfo membre = MemberInfo.GetMemberInfo( ( Guid )Cache[ "MembreGUID" ] );
            Questionnaire questionnaire = Questionnaire.GetQuestionnaire( ( int )Cache[ "QuestionnaireID " ] );
            Questionnaire newquestionnaire = new Questionnaire();

            Reporter.Trace( "GetMemberInfo() GetQuestionnaire()" );

            // Creation d'un nouveau Questionnaire
            if ( ( int )Cache[ "QuestionnaireExistantID" ] == 0 )
            {
                newquestionnaire.Description = questionnaire.Description;
                newquestionnaire.Style = questionnaire.Style;
                newquestionnaire.Valider = questionnaire.Valider;
                newquestionnaire.Fin = questionnaire.Fin;
                newquestionnaire.Anonyme = questionnaire.Anonyme;
                newquestionnaire.Compteur = questionnaire.Compteur;
                newquestionnaire.DateCreation = DateTime.Now;
                newquestionnaire.MembreGUID = ( Guid )Cache[ "MembreGUID" ];
                newquestionnaire.CodeAcces = ( int )Cache[ "CodeAcces" ];

                ValidationMessage.Text += "<br />Création du Questionnaire : " + newquestionnaire.Description + ":" + newquestionnaire.CodeAcces + "<br />";
                if ( User.IsInRole( "Administrateur" ) )
                {
                    ValidationMessage.Text += "Pour le membre : " + membre.NomUtilisateur + "/" + membre.Nom + "/" + membre.Prenom + "<br />";
                }

                // Tester les limitations avant d'ajouter le questionnaire
                if ( SessionState.Limitations.LimiteQuestionnaires )
                {
                    // BUG05062010 12072010
                    SessionState.Questionnaire = null;

                    Tools.PageValidation( "La limite du nombre de Questionnaires : " + SessionState.Limitations.NombreQuestionnaires + " est atteinte.<br/>Contactez l'administrateur." );
                }

                status = Questionnaire.Create( newquestionnaire );
                if ( status == 1 )
                {
                    ValidationMessage.Text += "Questionnaire créé correctement.<br/>";

                    // BUG05062010
                    // Prendre en compte le nouveau Questionnaire
                    //SessionState.Questionnaires.Add( questionnaire );
                    //SessionState.Questionnaire = questionnaire;
                    //SessionState.Limitations.AjouterQuestionnaire();
                    SessionState.Questionnaires.Add( newquestionnaire );
                    SessionState.Questionnaire = newquestionnaire;
                    SessionState.Limitations.AjouterQuestionnaire();
                }
                else if ( status == 2 )
                {
                    ValidationMessage.Text += "Le Questionnaire existe déjà.<br>";
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                }
                else
                {
                    ValidationMessage.Text += "Erreur sur la création du Questionnaire<br/>";
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                }
            }
            else // Questionnaire existant
            {
                newquestionnaire = Questionnaire.GetQuestionnaire( ( int )Cache[ "QuestionnaireExistantID" ] );

                // BUG05062010
                // Arrive ici la DropDownListQuestionnaire a modifie 
                // SessionState.Questionnaire avec le questionnaire de l'Intervieweur
                // il faut donc remmettre SessionState.Questionnaire a la valeur de l'utilisateur
                SessionState.Questionnaire = newquestionnaire;

                ValidationMessage.Text += "<br />Copie du Questionnaire : " + newquestionnaire.Description + ":" + newquestionnaire.CodeAcces + "<br />";
                ValidationMessage.Text += "Pour le membre : " + membre.NomUtilisateur + "/" + membre.Nom + "/" + membre.Prenom + "<br />";
            }

            int rank = 0;
            // Conserver le rang des Questions avec un biais MaxRank
            if ( ( int )Cache[ "QuestionnaireExistantID" ] != 0 )
            {
                PollQuestionCollection questionsExistantes = PollQuestionCollection.GetByQuestionnaire( ( int )Cache[ "QuestionnaireExistantID" ] );
                rank = questionsExistantes.MaxRank() + 1;
            }

            Reporter.Trace( "Questionnaire copier début" );

            foreach ( PollQuestion question in ( PollQuestionCollection )Cache[ "Questions" ] )
            {
                ValidationMessage.Text += "----Création de la Question : " + question.Question + "<br />";
                PollQuestion q = new PollQuestion();
                q.Question = question.Question;
                q.Rank = question.Rank + rank; // Ajouter le Biais
                q.Societe = question.Societe;
                q.Obligatoire = question.Obligatoire;
                q.Fin = question.Fin;
                q.ChoixMultiple = question.ChoixMultiple;
                q.ChoixMultipleMin = question.ChoixMultipleMin; //BUG20100330
                q.ChoixMultipleMax = question.ChoixMultipleMax;
                q.CreationDate = DateTime.Now;
                q.Instruction = question.Instruction;
                q.Message = question.Message;
                q.MessageHaut = question.MessageHaut;
                q.SautPage = question.SautPage;
                q.Tableau = question.Tableau;
                q.AlignementQuestion = question.AlignementQuestion;
                q.AlignementReponse = question.AlignementReponse;
                q.QuestionnaireID = newquestionnaire.QuestionnaireID;
                q.MembreGUID = membre.MembreGUID;

                // Tester les limitations avant d'ajouter la question
                if ( SessionState.Limitations.LimiteQuestions )
                {
                    Tools.PageValidation( "La limite du nombre de Questions : " + SessionState.Limitations.NombreQuestions + " est atteinte.<br/>Contactez l'administrateur." );
                }

                status = PollQuestion.Create( q );
                ValidationMessage.Text += "    status : " + status + "<br/>";
                SessionState.Limitations.AjouterQuestion();

                PollAnswerCollection reponses = PollAnswerCollection.GetByPollQuestionID( question.PollQuestionId );
                foreach ( PollAnswer reponse in reponses )
                {
                    Reporter.Trace( "PollAnswer.Create()" );

                    ValidationMessage.Text += "----Création de la Réponse : " + reponse.Answer + "<br />";
                    PollAnswer a = new PollAnswer();
                    a.PollQuestionId = q.PollQuestionId;
                    a.Answer = reponse.Answer;
                    a.TypeReponse = reponse.TypeReponse;
                    a.Width = reponse.Width;
                    a.Rows = reponse.Rows;
                    a.AlignLeft = reponse.AlignLeft;
                    a.Horizontal = reponse.Horizontal;
                    a.Obligatoire = reponse.Obligatoire;
                    a.Rank = reponse.Rank;
                    a.Score = reponse.Score;

                    status = PollAnswer.Create( a );
                    ValidationMessage.Text += "    status : " + status.ToString() + "<br />";
                }

            } //foreach ( PollQuestion question in questions )

            // Ne laisser faire qu'une copie
            ButtonCopier.Visible = false;

            // BUG05062010
            // NE PAS FAIRE ICI
            //// Creation d'un nouveau Questionnaire
            //// Prendre en compte le nouveau Questionnaire
            //if ( ( int )Cache[ "QuestionnaireExistantID" ] == 0 )
            //{
            //    SessionState.Questionnaires.Add( newquestionnaire );
            //    SessionState.Questionnaire = newquestionnaire;
            //}

            Cache[ "QuestionnaireID " ] = 0; // fermer le formulaire
            Cache[ "QuestionnaireExistantID" ] = 0;

            // Attention c'est faux Questionnaire.Create() retourn 1 si OK
            if ( status != 0 )
            {
                ValidationMessage.Text += "Erreur à la copie du Questionnaire status : " + status.ToString() + "<br />";
            }

            Reporter.Trace( "Questionnaire copier fin" );
            
            ButtonAjouterQuestion.Visible = true;
        }
    }
}