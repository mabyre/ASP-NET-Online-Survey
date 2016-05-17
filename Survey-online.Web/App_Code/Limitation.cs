using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Sql.Data;
using Sql.Web.Data;

/// <summary>
/// Calculer les limations de l'utilisateur
/// </summary>
public class Limitation
{
    /// <summary>
    /// Calcul des limitations pour un utilisateur authentifié
    /// pour un utilisateur non authentifie, on ne peut pas utiliser SessionState qui vaut n'importe quoi
    /// </summary>
    public Limitation()
    {
        // 
        // Calcul des Objets du membre
        //
        _Reponses = MemberInfo.GetMemberRepondantCount( SessionState.MemberInfo.MembreGUID );

        _Questionnaires = SessionState.Questionnaires.Count;
        foreach ( Questionnaire q in SessionState.Questionnaires )
        {
            // Interviewes
            PersonneCollection personnes = PersonneCollection.GetQuestionnaire( q.QuestionnaireID );
            _Interviewes += personnes.Count;

            PollQuestionCollection questionCollection = PollQuestionCollection.GetByQuestionnaire( q.QuestionnaireID );
            _Questions += questionCollection.Count;
        }

        // 
        // Les Limitations du membre
        //
        _NombreQuestionnaires = SessionState.MemberInfo.LimiteQuestionnaires;
        _NombreReponses = SessionState.MemberInfo.LimiteReponses;
        _NombreInterviewes = SessionState.MemberInfo.LimiteInterviewes;
        _NombreQuestions = SessionState.MemberInfo.LimiteQuestions;

        // Role du membre
        userIsAdministrateur = Roles.IsUserInRole( SessionState.MemberInfo.NomUtilisateur, "Administrateur" );
    }

    // Pour un utilisateur non authentifie 
    // ou pour l'administrateur qui cherche a connaitre les infos du membre
    public Limitation( Guid membreGUID )
    {
        _Reponses = MemberInfo.GetMemberRepondantCount( membreGUID );

        QuestionnaireCollection qc = QuestionnaireCollection.GetQuestionnaireMembre( membreGUID );
        _Questionnaires = qc.Count;
        foreach ( Questionnaire q in qc )
        {
            // Interviewes
            PersonneCollection personnes = PersonneCollection.GetQuestionnaire( q.QuestionnaireID );
            _Interviewes += personnes.Count;

            PollQuestionCollection repCollection = PollQuestionCollection.GetByQuestionnaire( q.QuestionnaireID );
            _Questions += repCollection.Count;
        }

        MemberInfo member = MemberInfo.GetMemberInfo( membreGUID );
        _NombreQuestionnaires = member.LimiteQuestionnaires;
        _NombreReponses = member.LimiteReponses;
        _NombreInterviewes = member.LimiteInterviewes;
        _NombreQuestions = member.LimiteQuestions;

        MembershipUser user = Membership.GetUser( membreGUID );
        userIsAdministrateur = Roles.IsUserInRole( user.UserName, "Administrateur" );
    }

    /* ----------
     * Proprietes
     * ----------
     */
    private bool userIsAdministrateur = true;

    //-----------------------------------------------------------------------------
    // Limitation en Questionnaires
    //-----------------------------------------------------------------------------

    // Nombre de Questionnaires de l'utilisateur
    private int _Questionnaires = 0;
    public int Questionnaires
    {
        get { return _Questionnaires; }
    }

    //  Limite en Questionnaires de l'utilisateur
    private int _NombreQuestionnaires = 0;
    public int NombreQuestionnaires
    {
        get
        {
            return _NombreQuestionnaires;
        }
    }

    // La limite en Questionnaires est-elle atteinte ?
    public bool LimiteQuestionnaires
    {
        get 
        {
            if ( userIsAdministrateur )
            {
                return false;
            }
            return _Questionnaires >= _NombreQuestionnaires;
        }
    }

    public void AjouterQuestionnaire()
    {
        _Questionnaires += 1;
    }

    public void SupprimerQuestionnaire()
    {
        _Questionnaires -= 1;
    }

    //------------------------------------------------------------------------
    // Limitation en Questions
    //------------------------------------------------------------------------
 
    // Nombre de Questions de l'utilisateur
    private int _Questions = 0;
    public int Questions
    {
        get { return _Questions; }
    }

    // Limite en Questions de l'utilisateur
    private int _NombreQuestions = 0;
    public int NombreQuestions
    {
        get
        {
            return _NombreQuestions;
        }
    }

    // La limite en Questions est-elle atteinte ?
    public bool LimiteQuestions
    {
        get
        {
            if ( userIsAdministrateur )
            {
                return false;
            }
            return _Questions >= _NombreQuestions;
        }
    }

    public void AjouterQuestion()
    {
        _Questions += 1;
    }

    public void SupprimerQuestion()
    {
        _Questions -= 1;
    }

    //--------------------------------------------------------------------------
    // Limitation en Interviewes
    //--------------------------------------------------------------------------

    /// Nombre d'Interviewes de l'utilisateur
    private int _Interviewes = 0;
    public int Interviewes
    {
        get { return _Interviewes; }
    }

    /// Limite en Interviewes
    private int _NombreInterviewes = 0;
    public int NombreInterviewes
    {
        get
        {
            return _NombreInterviewes;
        }
    }


    // La limite en Interviewes est-elle atteinte ?
    public bool LimitesInterviewes
    {
        get
        {
            if ( userIsAdministrateur )
            {
                return false;
            }
            return _Interviewes >= _NombreInterviewes;
        }
    }

    // La limite des interviewés va t-elle être atteinte avec nbInterviewes en plus ?
    public bool LimitesInterviewesAtteinte( int nbInterviewes )
    {
        if ( userIsAdministrateur )
        {
            return false;
        }
        return _Interviewes + nbInterviewes > _NombreInterviewes;
    }

    public void AjouterInterviewes( int nbInterviewes )
    {
        _Interviewes += nbInterviewes;
    }

    public void SupprimerInterviewes( int nbInterviewes )
    {
        _Interviewes -= nbInterviewes;
    }

    //------------------------------------------------------------------------
    // Limitation en Reponses
    //------------------------------------------------------------------------

    // Nombre de Reponses de l'utilisateur
    private int _Reponses = 0;
    public int Reponses
    {
        get { return _Reponses; }
    }

    /// Limite en Reponse
    private int _NombreReponses = 0;
    public int NombreReponses
    {
        get
        {
            return _NombreReponses;
        }
    }

    // La limite en Responses est-elle atteinte ?
    public bool LimitesReponses
    {
        get
        {
            if ( userIsAdministrateur )
            {
                return false;
            }
            return _Reponses >= _NombreReponses;
        }
    }

    public void AjouterReponse()
    {
        _Reponses += 1;
    }

    public void SupprimerReponse()
    {
        _Reponses -= 1;
    }

}