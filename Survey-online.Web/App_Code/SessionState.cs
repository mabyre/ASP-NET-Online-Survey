using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.Collections.Generic;
using Sql.Web.Data;

/// <summary>
/// Conserver l'etat des CheckBox pendant toute la Session
/// </summary>
public class CheckBoxSessionState
{
    // Le constructeur est obligatoire ici sinon SessionState.CheckBox est vide
    public CheckBoxSessionState()
    {
    }

    new public bool this[ string key ]
    {
        get
        {
            if ( HttpContext.Current.Session[ "CheckBoxSessionState" ] == null )
            {
                Dictionary<string, bool> _cbss = new Dictionary<string, bool>();
                _cbss.Add( "CheckBoxInstruction", true );
                _cbss.Add( "CheckBoxMessage", true );
                _cbss.Add( "CheckBoxDate", false );
                _cbss.Add( "CheckBoxSociete", false );
                _cbss.Add( "CheckBoxAlignementQuestionReponse", false );
                _cbss.Add( "CheckBoxDescription", false );
                _cbss.Add( "CheckBoxAfficherReponseTextuelle", true );
                _cbss.Add( "CheckBoxAfficherDateVote", false );
                _cbss.Add( "CheckBoxAfficherMoyennePonderee", false );
                _cbss.Add( "CheckBoxModeTest", false );
                _cbss.Add( "CheckBoxChoixMultipleMinMax", false );
                HttpContext.Current.Session[ "CheckBoxSessionState" ] = _cbss;
            }
            return ( ( Dictionary<string, bool> )HttpContext.Current.Session[ "CheckBoxSessionState" ] )[ key ];
        }
        set 
        {
            ( ( Dictionary<string, bool> )HttpContext.Current.Session[ "CheckBoxSessionState" ] )[ key ] = value; 
        }
    }
}

public class BooleanSessionState
{
    // Le constructeur est obligatoire ici sinon SessionState.BooleanSate est vide
    public BooleanSessionState()
    {
    }

    new public bool this[ string key ]
    {
        get
        {
            if ( HttpContext.Current.Session[ "BooleanSessionState" ] == null )
            {
                Dictionary<string, bool> _bss = new Dictionary<string, bool>();
                _bss.Add( "ImageButtonExpandQuestion", false );
                _bss.Add( "ImageButtonExpandReponse", false );
                _bss.Add( "ImageButtonExpandReponseTextuelle", false );
                _bss.Add( "ImageButtonExpandNavigation", false );
                HttpContext.Current.Session[ "BooleanSessionState" ] = _bss;
            }
            return ( ( Dictionary<string, bool> )HttpContext.Current.Session[ "BooleanSessionState" ] )[ key ];
        }
        set
        {
            ( ( Dictionary<string, bool> )HttpContext.Current.Session[ "BooleanSessionState" ] )[ key ] = value;
        }
    }
}

/// <summary>
/// Gerer les variables de sessions d'une maniere structuree
/// </summary>
public class SessionState 
{
    public static CheckBoxSessionState CheckBox = new CheckBoxSessionState();
    public static BooleanSessionState BooleanSate = new BooleanSessionState();

    public static int ContactsParPage
    {
        get
        {
            if ( HttpContext.Current.Session[ "ContactsParPage" ] == null )
            {
                HttpContext.Current.Session[ "ContactsParPage" ] = int.Parse( Global.SettingsXml.ContactsParPageCourant );
            }
            return ( int )( HttpContext.Current.Session[ "ContactsParPage" ] );
        }

        set
        {
            HttpContext.Current.Session[ "ContactsParPage" ] = value;
        }
    }

    public static string ValidationMessage
    {
        get
        {
            if ( HttpContext.Current.Session[ "ValidationMessage" ] != null )
                return ( string )( HttpContext.Current.Session[ "ValidationMessage" ] );

            return null;
        }

        set
        {
            HttpContext.Current.Session[ "ValidationMessage" ] = value;
        }
    }

    /// <summary>
    ///  Contact authentifie par la page d'authentification
    /// </summary>
    public static Personne Personne
    {
        get
        {
            if ( HttpContext.Current.Session[ "Personne" ] != null )
                return ( Personne )( HttpContext.Current.Session[ "Personne" ] );

            // Pour plus tard, on particularise un testeur pour savoir qui fait quoi
            // pour l'instant on ne s'en sert pas
            // Creation d'un testeur
            if ( HttpContext.Current.User.IsInRole( "Administrateur" ) )
            {
                Personne admin = new Personne();
                admin.PersonneGUID = Guid.NewGuid();
                admin.Nom = "Monsieur l'";
                admin.Prenom = "Administrateur";
                admin.CodeAcces = 0; // pour distinguer des autres "Personne" simple contact
                HttpContext.Current.Session[ "Personne" ] = admin;
            }
            else if ( HttpContext.Current.User.Identity.IsAuthenticated )
            {
                Personne p = new Personne();
                p.PersonneGUID = Guid.NewGuid();
                
                if ( SessionState.MemberInfo != null )
                {
                    p.Nom = SessionState.MemberInfo.Nom;
                    p.Prenom = SessionState.MemberInfo.Prenom;
                }
                else // l'user est authentifie dans une autre appli
                {
                    p.Nom =  HttpContext.Current.User.Identity.Name;
                    p.Prenom = "";
                }
                p.CodeAcces = 0; // pour distinguer des autres "Personne" simple contact
                HttpContext.Current.Session[ "Personne" ] = p;
            }

            return ( Personne )HttpContext.Current.Session[ "Personne" ];
        }

        set
        {
            // Le seul endroit ou l'on peut setter cette valeur c'est dans Login.aspx.cs
            HttpContext.Current.Session[ "Personne" ] = value;
        }
    }

    /// <summary>
    /// Contenu Web d'une Section
    /// </summary>
    public static WebContent WebContent
    {
        get
        {
            if ( HttpContext.Current.Session[ "WebContent" ] != null )
                return ( WebContent )( HttpContext.Current.Session[ "WebContent" ] );

            return null;
        }

        set
        {
            HttpContext.Current.Session[ "WebContent" ] = value;
        }
    }

    /// <summary>
    /// Informations sur un Membre authentifie
    /// </summary>
    public static MemberInfo MemberInfo
    {
        get
        {
            if ( HttpContext.Current.Session[ "MemberInfo" ] == null )
            {
                if ( HttpContext.Current.User != null )
                {
                    if ( HttpContext.Current.User.Identity.IsAuthenticated )
                    {
                        MembershipUser user = Membership.GetUser();
                        MemberInfo member = MemberInfo.Get( ( Guid )user.ProviderUserKey );
                        HttpContext.Current.Session[ "MemberInfo" ] = member;
                    }
                }
                else // La session de l'utilisateur a expiree
                {
                    FormsAuthentication.SignOut();
                    HttpContext.Current.Response.Redirect( "~/Default.aspx" );
                }
            }

            return ( MemberInfo )HttpContext.Current.Session[ "MemberInfo" ];
        }
    }

    /// <summary>
    /// Les Settings d'un Membre
    /// </summary>
    public static MemberSettings MemberSettings
    {
        get
        {
            if ( HttpContext.Current.Session[ "MemberSettings" ] == null )
            {
                string membre = HttpContext.Current.User.Identity.Name;
                MemberSettings memberSettings = MemberSettings.GetMemberSettings( membre );
                HttpContext.Current.Session[ "MemberSettings" ] = memberSettings;
            }

            return ( MemberSettings )HttpContext.Current.Session[ "MemberSettings" ];
        }

        set { HttpContext.Current.Session[ "MemberSettings" ] = value; }
    }
    
    public static QuestionnaireCollection Questionnaires
    {
        get
        {
            if ( HttpContext.Current.Session[ "Questionnaires" ] != null )
                return ( QuestionnaireCollection )( HttpContext.Current.Session[ "Questionnaires" ] );

            QuestionnaireCollection Questionnaires = new QuestionnaireCollection();
            if ( HttpContext.Current.User.IsInRole( "Administrateur" ) )
            {
                Questionnaires = QuestionnaireCollection.GetAll();
            }
            else
            {
                // Va te faire loguer ...
                if ( SessionState.MemberInfo == null ) 
                    HttpContext.Current.Response.Redirect( "~/Default.aspx" );

                Questionnaires = QuestionnaireCollection.GetQuestionnaireMembre( SessionState.MemberInfo.MembreGUID );
            }
            HttpContext.Current.Session[ "Questionnaires" ] = Questionnaires;
            return ( QuestionnaireCollection )HttpContext.Current.Session[ "Questionnaires" ];
        }

        set { HttpContext.Current.Session[ "Questionnaires" ] = null; }
    }

    public static Questionnaire Questionnaire
    {
        get
        {
            if ( HttpContext.Current.Session[ "Questionnaire" ] != null )
                return ( Questionnaire )( HttpContext.Current.Session[ "Questionnaire" ] );

            return null;
        }

        set
        {
            HttpContext.Current.Session[ "Questionnaire" ] = value;
        }
    }

    // Une page de Questions
    public static PollQuestionCollection PageQuestions
    {
        get
        {
            if ( HttpContext.Current.Session[ "PageQuestions" ] != null )
            {
                return ( PollQuestionCollection )( HttpContext.Current.Session[ "PageQuestions" ] );
            }
            return null;
        }

        set
        {
            HttpContext.Current.Session[ "PageQuestions" ] = value;
        }
    }

    public static PollQuestionCollection Questions
    {
        get
        {
            if ( HttpContext.Current.Session[ "Questions" ] != null )
            {
                return ( PollQuestionCollection )( HttpContext.Current.Session[ "Questions" ] );
            }
            return null;
        }

        set
        {
            HttpContext.Current.Session[ "Questions" ] = value;
        }
    }

    // Pour le Mode Validation a la fin du Questionnaire
    public static PollQuestionCollection QuestionsEnCours
    {
        get
        {
            if ( HttpContext.Current.Session[ "QuestionsEnCours" ] != null )
                return ( PollQuestionCollection )( HttpContext.Current.Session[ "QuestionsEnCours" ] );

            return null;
        }

        set
        {
            HttpContext.Current.Session[ "QuestionsEnCours" ] = value;
        }
    }

    public static PollQuestion Question
    {
        get
        {
            if ( HttpContext.Current.Session[ "Question" ] != null )
                return ( PollQuestion )( HttpContext.Current.Session[ "Question" ] );

            return null;
        }

        set
        {
            HttpContext.Current.Session[ "Question" ] = value;
        }
    }

    public static PollAnswerCollection Reponses
    {
        get
        {
            if ( HttpContext.Current.Session[ "Reponses" ] != null )
                return ( PollAnswerCollection )( HttpContext.Current.Session[ "Reponses" ] );

            return null;
        }

        set
        {
            HttpContext.Current.Session[ "Reponses" ] = value;
        }
    }

    public static PollVoteCollection Votes
    {
        get
        {
            if ( HttpContext.Current.Session[ "Votes" ] != null )
                return ( PollVoteCollection )( HttpContext.Current.Session[ "Votes" ] );

            return null;
        }

        set
        {
            HttpContext.Current.Session[ "Votes" ] = value;
        }
    }

    // Pour le Mode Validation a la fin du Questionnaire
    // la premiere fois retourne une collection vide plutot que null
    public static PollVoteCollection VotesEnCours
    {
        get
        {
            if ( HttpContext.Current.Session[ "VotesEnCours" ] != null )
                return ( PollVoteCollection )( HttpContext.Current.Session[ "VotesEnCours" ] );

            return null;
        }

        set
        {
            HttpContext.Current.Session[ "VotesEnCours" ] = value;
        }
    }

    public static int CurrentQuestionIndex
    {
        get
        {
            if ( HttpContext.Current.Session[ "CurrentQuestionIndex" ] != null )
                return ( int )( HttpContext.Current.Session[ "CurrentQuestionIndex" ] );

            return -1; // null on ne peut pas !
        }

        set
        {
            HttpContext.Current.Session[ "CurrentQuestionIndex" ] = value;
        }
    }

    // Pour le mode page
    public static bool QuestionnaireEnModePage
    {
        get
        {
            if ( HttpContext.Current.Session[ "QuestionnaireEnModePage" ] != null )
                return ( bool )( HttpContext.Current.Session[ "QuestionnaireEnModePage" ] );

            return false;
        }

        set
        {
            HttpContext.Current.Session[ "QuestionnaireEnModePage" ] = value;
        }
    }

    public static int PageQuestionsIndex
    {
        get
        {
            if ( HttpContext.Current.Session[ "PageQuestionsIndex" ] == null )
                return 0; // premiere Question du Questionnaire
            return ( int )( HttpContext.Current.Session[ "PageQuestionsIndex" ] );
        }
        set
        {
            HttpContext.Current.Session[ "PageQuestionsIndex" ] = value;
        }
    }

    /// <summary>
    /// Les contacts lies au questionnaire en cours
    /// </summary>
    public static PersonneCollection Personnes
    {
        get
        {
            if ( HttpContext.Current.Session[ "Personnes" ] != null )
                return ( PersonneCollection )( HttpContext.Current.Session[ "Personnes" ] );

            if ( SessionState.Questionnaire != null )
            {
                PersonneCollection personnes = PersonneCollection.GetQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
                HttpContext.Current.Session[ "Personnes" ] = personnes;
            }

            return ( PersonneCollection )HttpContext.Current.Session[ "Personnes" ];
        }

        set { HttpContext.Current.Session[ "Personnes" ] = null; }
    }

    /// <summary>
    /// Les Limitations
    /// </summary>
    public static Limitation Limitations
    {
        get
        {
            if ( HttpContext.Current.Session[ "Limitations" ] == null )
            {
                Limitation limites = new Limitation();
                HttpContext.Current.Session[ "Limitations" ] = limites;
            }
            return ( Limitation )HttpContext.Current.Session[ "Limitations" ];
        }

        set
        {
            HttpContext.Current.Session[ "Limitations" ] = value;
        }
    }
}
