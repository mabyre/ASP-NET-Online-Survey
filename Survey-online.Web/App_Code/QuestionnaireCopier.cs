using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using Sql.Web.Data;

/// <summary>
/// 
/// J'ai eu besoin de cette Class pour ne pas dupliquer le code de copie d'un questionnaire
/// finalement on va avoir du mal a utiliser ce code dans Questionnaire/Copier.aspx
/// donc attention a synchroniser ces deux codes si la copie d'un questionnaire
/// devait etre modifiee ...
/// 
/// Utiliser BeyondCompare pour comparer les deux codes
/// 
/// Il y a un joyeux melange avec le questionnaire existant...
/// Fonctionnalité identique de copie de questionnaire dans
/// Quetionnaire/Copier.aspx
/// 
/// Ici c'est l'utilisateur que s'enregistre qui copie le questionnaire d'exemple pour son
/// son usage il n'est pas authentifié on ne peut pas utiliser 
/// 
/// !!!!!!!!!!!
/// ICI, NE PAS UTILISER : SessionSate 
/// !!!!!!!!!!!
/// </summary>
public class QuestionnaireCopier
{
    public QuestionnaireCopier()
    {
        //
        // TODO : ajoutez ici la logique du constructeur
        //
    }

    /// <summary>
    /// !!!!!!!!!!!!!
    /// Attention !!!
    /// !!!!!!!!!!!!!
    /// Je ne peux pas utiliser cette fonction pour copier un questionnaire pour un utilisateur non authentifie
    /// On ne peut pas utiliser SessionState pour un utilisateur non authentifie
    /// lorsque l'utilisateur s'enregsitre mais qu'il n'est pas encore totalement authentifié
    /// SessionSate crée des Erreurs d'un autre monde !!!
    /// Si je voulais réintrégrer ce code dans Questionnaire/Copier.aspx il faudrait prendre cette précaution
    /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    /// Ce jour là il y a 5 nouveaux utilisateurs qui ce sont enregistrés !
    /// </summary>
    /// <param name="QuestionnaireID"></param>
    /// <param name="MembreGUID"></param>
    /// <param name="QuestionnaireExistantID"></param>
    /// <returns></returns>
    public static string CopierQuestionnaire( int QuestionnaireID, Guid MembreGUID, int QuestionnaireExistantID )
    {
        string message = "";
        string msg = "";

        if ( QuestionnaireID == 0 )
        {
            message += "Choisir un questionnaire à copier.<br/>";
        }
        else
        {
            int status = 0;
            MemberInfo membre = MemberInfo.GetMemberInfo( MembreGUID );
            Questionnaire questionnaire = Questionnaire.GetQuestionnaire( QuestionnaireID );
            Questionnaire newquestionnaire = new Questionnaire();

            // Calculer un nouveau CodeAcces avant de pouvoir Copier le Questionnaire
            ArrayList codes = QuestionnaireDAL.GetCodeAccessAll();
            string codeAcces = Tools.CalculCodeAcces( membre.MembreID, codes ).ToString();

            // Creation d'un nouveau Questionnaire
            if ( QuestionnaireExistantID == 0 )
            {
                newquestionnaire.Description = questionnaire.Description;
                newquestionnaire.Style = questionnaire.Style;
                newquestionnaire.Valider = questionnaire.Valider;
                newquestionnaire.Fin = questionnaire.Fin;
                newquestionnaire.Anonyme = questionnaire.Anonyme;
                newquestionnaire.Compteur = questionnaire.Compteur;
                newquestionnaire.DateCreation = DateTime.Now;
                newquestionnaire.MembreGUID = MembreGUID;
                newquestionnaire.CodeAcces = int.Parse( codeAcces );

                message += "<br />Création du Questionnaire : " + newquestionnaire.Description + ":" + newquestionnaire.CodeAcces + "<br />";
                if ( HttpContext.Current.User.IsInRole( "Administrateur" ) )
                {
                    message += "Pour le membre : " + membre.NomUtilisateur + "/" + membre.Nom + "/" + membre.Prenom + "<br />";
                }

                //BUG00020100215
                //if ( SessionState.Limitations.LimiteQuestionnaires )
                //{
                //    Tools.PageValidation( "La limite du nombre de Questionnaires : " + SessionState.Limitations.NombreQuestionnaires + " est atteinte.<br/>Contactez l'administrateur." );
                //}

                status = Questionnaire.Create( newquestionnaire );
                if ( status == 1 )
                {
                    msg += "Copie du questionnaire d'exemple.<br/>";
                    message += "Questionnaire créé correctement.<br/>";
                    //BUG00020100215
                    //SessionState.Limitations.AjouterQuestionnaire();
                }
                else if ( status == 2 )
                {
                    msg += "Erreur à la copie du questionnaire exemple.<br/>";
                    message += "Le Questionnaire existe déjà.<br/>";
                }
                else
                {
                    msg += "Erreur à la copie du questionnaire exemple.<br/>";
                    message += "Erreur sur la création du Questionnaire<br/>";
                }
            }
            else // Questionnaire existant
            {
                newquestionnaire = Questionnaire.GetQuestionnaire( QuestionnaireExistantID );

                message += "<br />Copie du Questionnaire : " + newquestionnaire.Description + ":" + newquestionnaire.CodeAcces + "<br />";
                message += "Pour le membre : " + membre.NomUtilisateur + "/" + membre.Nom + "/" + membre.Prenom + "<br />";
            }

            int rank = 0;
            // Conserver le rang des Questions avec un biais MaxRank
            if ( QuestionnaireExistantID != 0 )
            {
                PollQuestionCollection questionsExistantes = PollQuestionCollection.GetByQuestionnaire( QuestionnaireExistantID );
                rank = questionsExistantes.MaxRank() + 1;
            }

            PollQuestionCollection questions = PollQuestionCollection.GetByQuestionnaire( QuestionnaireID );
            foreach ( PollQuestion question in questions )
            {
                message += "----Création de la Question : " + question.Question + "<br />";
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

                //BUG00020100215
                //// Tester les limitations avant d'ajouter la question
                //if ( SessionState.Limitations.LimiteQuestions )
                //{
                //    Tools.PageValidation( "La limite du nombre de Questions : " + SessionState.Limitations.NombreQuestions + " est atteinte.<br/>Contactez l'administrateur." );
                //}

                status = PollQuestion.Create( q );
                message += "    status : " + status + "<br/>";
                //BUG00020100215
                //SessionState.Limitations.AjouterQuestion();

                PollAnswerCollection reponses = PollAnswerCollection.GetByPollQuestionID( question.PollQuestionId );
                foreach ( PollAnswer reponse in reponses )
                {
                    message += "----Création de la Réponse : " + reponse.Answer + "<br />";
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
                    message += "    status : " + status.ToString() + "<br />";
                }

            } //foreach ( PollQuestion question in questions )

            // Ne laisser faire qu'une copie
            //ButtonCopier.Visible = false;

            //
            // Attention BUG l'acces a SessionState cree un BUG d'un autre monde
            //
            // peut pas evaluer l'exepression car trop en haut de la pile
            //SessionState.Questionnaire = newquestionnaire;

            // Creation d'un nouveau Questionnaire
            //if ( QuestionnaireExistantID == 0 )
            //{
            //    SessionState.Questionnaires.Add( newquestionnaire );
            //}
            
            QuestionnaireID = 0; // fermer le formulaire
            QuestionnaireExistantID = 0;

            // Attention c'est faux Questionnaire.Create() retourn 1 si OK
            if ( status != 0 )
            {
                message += "Erreur à la copie du Questionnaire status : " + status.ToString() + "<br />";
            }

            //ButtonAjouterQuestion.Visible = true;
        }

        //return message;
        return msg;
    }
}
