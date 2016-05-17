#region using

using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using XmlDataProvider;

#endregion

namespace SettingXmlProvider
{
    public class SettingXml
    {
        public static StringDictionary Dictionary = DataProviderXml.LoadXmlData( "settings.xml" );

        public SettingXml()
        {
            StringDictionary dic = DataProviderXml.LoadXmlData( "settings.xml" );
            tailleEditor = dic[ "tailleeditor" ];
            elargirEditor = dic[ "elargireditor" ];
            siteNom = dic[ "sitenom" ];
            siteSlogan = dic[ "siteslogan" ];
            siteAddress = dic[ "siteaddress" ];
            siteCopyright = dic[ "sitecopyright" ];
            adresseWebmaster = dic[ "adressewebmaster" ];
            sujetCourrielMaj = dic[ "sujetcourrielmaj" ];
            envoyerMiseAjour = dic[ "envoyermiseajour" ] == "vrai" ? true : false;
            labelBoutonQuestion = dic[ "labelboutonquestion" ];
            radioButtonListQuestionnaireRepeatColumn = dic[ "radiobuttonlistquestionnairerepeatcolumn" ];
            debloquerClient = dic[ "debloquerclient" ] == "vrai" ? true : false;
            membrePrevenir = dic[ "membrePrevenir" ] == "vrai" ? true : false;
            membreConnexionPrevenir = dic[ "membreConnexionPrevenir" ] == "vrai" ? true : false;
            membreApprouve = dic[ "membreApprouve" ] == "vrai" ? true : false;
            membreApprouveParEmail = dic[ "membreApprouveParEmail" ] == "vrai" ? true : false;
            logUser = dic[ "loguser" ] == "vrai" ? true : false;
            enregistrerContactAvecSociete = dic[ "enregistrercontactavecsociete" ] == "vrai" ? true : false;
            enregistrerContactAnonyme = dic[ "enregistrercontactanonyme" ] == "vrai" ? true : false;

            // Pour avoir les valeurs par defaut si les settings ne contiennent pas ces clefs
            virtualPath = dic[ "virtualpath" ] == null ? virtualPath : dic[ "virtualpath" ];
            reponseTextuelleLargeurMin = dic[ "reponsetextuellelargeurmin" ] == null ? reponseTextuelleLargeurMin : dic[ "reponsetextuellelargeurmin" ];
            reponseTextuelleLargeurMax = dic[ "reponsetextuellelargeurmax" ] == null ? reponseTextuelleLargeurMax : dic[ "reponsetextuellelargeurmax" ];
            reponseTextuelleLigneMax = dic[ "reponsetextuellelignemax" ] == null ? reponseTextuelleLigneMax : dic[ "reponsetextuellelignemax" ];
            codeAccesQuestionnaireExemple = dic[ "codeaccesquestionnaireexemple" ] == null ? codeAccesQuestionnaireExemple : dic[ "codeaccesquestionnaireexemple" ];

            contactsParPageMin = dic[ "contactsparpagemin" ] == null ? contactsParPageMin : dic[ "contactsparpagemin" ];
            contactsParPageMax = dic[ "contactsparpagemax" ] == null ? contactsParPageMax : dic[ "contactsparpagemax" ]; ;
            contactsParPageCourant = dic[ "contactsparpagecourant" ] == null ? contactsParPageCourant : dic[ "contactsparpagecourant" ]; ;

            // Limitations de l'utilisateur limite
            gratuitLimiteQuestionnaires = dic[ "gratuitlimitequestionnaires" ] == null ? gratuitLimiteQuestionnaires : dic[ "gratuitlimitequestionnaires" ];
            gratuitLimiteQuestions = dic[ "gratuitlimitequestions" ] == null ? gratuitLimiteQuestions : dic[ "gratuitlimitequestions" ];
            gratuitLimiteInterviewes = dic[ "gratuitlimiteinterviewes" ] == null ? gratuitLimiteInterviewes : dic[ "gratuitlimiteinterviewes" ];
            gratuitLimiteReponses = dic[ "gratuitlimitereponses" ] == null ? gratuitLimiteReponses : dic[ "gratuitlimitereponses" ];

            // Limitations de l'application
            abonneLimiteQuestionnaires = dic[ "abonnelimitequestionnaires" ] == null ? abonneLimiteQuestionnaires : dic[ "abonnelimitequestionnaires" ];
            abonneLimiteQuestions = dic[ "abonnelimitequestions" ] == null ? abonneLimiteQuestions : dic[ "abonnelimitequestions" ];
            abonneLimiteInterviewes = dic[ "abonnelimiteinterviewes" ] == null ? abonneLimiteInterviewes : dic[ "abonnelimiteinterviewes" ];
            abonneLimiteReponses = dic[ "abonnelimitereponses" ] == null ? abonneLimiteReponses : dic[ "abonnelimitereponses" ];

            // Limitation de la liste des interviewes importes
            limitationImportsInterviewes = dic[ "limitationimportsinterviewes" ] == null ? limitationImportsInterviewes : dic[ "limitationimportsinterviewes" ];
        }

        public SettingXml Reload()
        {
            SettingXml sxml = new SettingXml();
            return sxml;
        }

        public void Save( SettingXml sxml )
        {
            StringDictionary dic = DataProviderXml.LoadXmlData( "settings.xml" );

            dic[ "virtualpath" ] = virtualPath;
            dic[ "sitenom" ] = siteNom;
            dic[ "siteslogan" ] = siteSlogan;
            dic[ "siteaddress" ] = siteAddress;
            dic[ "sitecopyright" ] = siteCopyright;
            dic[ "adressewebmaster" ] = adresseWebmaster;
            dic[ "sujetcourrielmaj" ] = sujetCourrielMaj;
            dic[ "envoyermiseajour" ] = envoyerMiseAjour == true ? "vrai" : "faux";
            dic[ "labelboutonquestion" ] = labelBoutonQuestion;
            dic[ "radiobuttonlistquestionnairerepeatcolumn" ] = radioButtonListQuestionnaireRepeatColumn;
            dic[ "debloquerclient" ] = debloquerClient == true ? "vrai" : "faux";
            dic[ "membrePrevenir" ] = membrePrevenir == true ? "vrai" : "faux";
            dic[ "membreConnexionPrevenir" ] = membreConnexionPrevenir == true ? "vrai" : "faux";
            dic[ "membreApprouve" ] = membreApprouve == true ? "vrai" : "faux";
            dic[ "membreApprouveParEmail" ] = membreApprouveParEmail == true ? "vrai" : "faux";
            dic[ "codeaccesquestionnaireexemple" ] = codeAccesQuestionnaireExemple;
            dic[ "loguser" ] = logUser == true ? "vrai" : "faux";
            dic[ "enregistrercontactavecsociete" ] = enregistrerContactAvecSociete == true ? "vrai" : "faux";
            dic[ "enregistrercontactanonyme" ] = enregistrerContactAnonyme == true ? "vrai" : "faux";
            dic[ "reponsetextuellelargeurmin" ] = reponseTextuelleLargeurMin;
            dic[ "reponsetextuellelargeurmax" ] = reponseTextuelleLargeurMax;
            dic[ "reponsetextuellelignemax" ] = reponseTextuelleLigneMax;
            dic[ "contactsparpagemin" ] = contactsParPageMin;
            dic[ "contactsparpagemax" ] = contactsParPageMax;
            dic[ "contactsparpagecourant" ] = contactsParPageCourant;

            // Limitations de l'utilisateur decouverte
            dic[ "gratuitlimitequestionnaires" ] = gratuitLimiteQuestionnaires;
            dic[ "gratuitlimitequestions" ] = gratuitLimiteQuestions;
            dic[ "gratuitlimiteinterviewes" ] = gratuitLimiteInterviewes;
            dic[ "gratuitlimitereponses" ] = gratuitLimiteReponses;

            // Limitations de l'utilisateur abonne
            dic[ "abonnelimitequestionnaires" ] = abonneLimiteQuestionnaires;
            dic[ "abonnelimitequestions" ] = abonneLimiteQuestions;
            dic[ "abonnelimiteinterviewes" ] = abonneLimiteInterviewes;
            dic[ "abonnelimitereponses" ] = abonneLimiteReponses;

            // Limitation de la liste des import d'interviewes
            dic[ "limitationimportsinterviewes" ] = limitationImportsInterviewes;

            XmlDataProvider.DataProviderXml.SaveXmlData( dic, "settings.xml" );
        }

        private string contactsParPageMin = "5";
        public string ContactsParPageMin
        {
            get { return contactsParPageMin; }
            set { contactsParPageMin = value; }
        }

        private string contactsParPageMax = "100";
        public string ContactsParPageMax
        {
            get { return contactsParPageMax; }
            set { contactsParPageMax = value; }
        }

        private string contactsParPageCourant = "50";
        public string ContactsParPageCourant
        {
            get { return contactsParPageCourant; }
            set { contactsParPageCourant = value; }
        }

        private string reponseTextuelleLargeurMin = "10";
        public string ReponseTextuelleLargeurMin
        {
            get { return reponseTextuelleLargeurMin; }
            set { reponseTextuelleLargeurMin = value; }
        }

        private string reponseTextuelleLargeurMax = "980";
        public string ReponseTextuelleLargeurMax
        {
            get { return reponseTextuelleLargeurMax; }
            set { reponseTextuelleLargeurMax = value; }
        }

        private string reponseTextuelleLigneMax = "500";
        public string ReponseTextuelleLigneMax
        {
            get { return reponseTextuelleLigneMax; }
            set { reponseTextuelleLigneMax = value; }
        }

        private string virtualPath = "~/";
        public string VirtualPath
        {
            get { return virtualPath; }
            set { virtualPath = value; }
        }

        private bool enregistrerContactAnonyme = false;
        public bool EnregistrerContactAnonyme
        {
            get { return enregistrerContactAnonyme; }
            set { enregistrerContactAnonyme = value; }
        }

        private bool enregistrerContactAvecSociete = true;
        public bool EnregistrerContactAvecSociete
        {
            get { return enregistrerContactAvecSociete; }
            set { enregistrerContactAvecSociete = value; }
        }

        private bool logUser = false;
        public bool LogUser
        {
            get { return logUser; }
            set { logUser = value; }
        }

        private bool debloquerClient = false;
        public bool DebloquerClient
        {
            get { return debloquerClient; }
            set { debloquerClient = value; }
        }

        //**************************
        // Nouveau Membre enregsitre
        //**************************
        private bool membrePrevenir = false;
        public bool MembrePrevenir
        {
            get { return membrePrevenir; }
            set { membrePrevenir = value; }
        }

        //
        // Connexion d'un Membre
        //
        private bool membreConnexionPrevenir = false;
        public bool MembreConnexionPrevenir
        {
            get { return membreConnexionPrevenir; }
            set { membreConnexionPrevenir = value; }
        }

        //
        // Le Membre enregistre est approuve
        //
        private bool membreApprouve = false;
        public bool MembreApprouve
        {
            get { return membreApprouve; }
            set { membreApprouve = value; }
        }

        //
        // Le Membre doit s'approuver par Email
        //
        private bool membreApprouveParEmail = false;
        public bool MembreApprouveParEmail
        {
            get { return membreApprouveParEmail; }
            set { membreApprouveParEmail = value; }
        }

        // code d'accès du questionnaire d'exemple a copier pour
        // un nouveau membre enregistre
        private string codeAccesQuestionnaireExemple = "0000";
        public string CodeAccesQuestionnaireExemple 
        {
            get { return codeAccesQuestionnaireExemple; }
            set { codeAccesQuestionnaireExemple = value; }
        }

        //**********************************
        private string tailleEditor = "200";
        public string TailleEditor
        {
            get { return tailleEditor; }
        }

        private string elargirEditor = "80";
        public string ElargirEditor
        {
            get { return elargirEditor; }
        }

        private string siteNom = "Nom du Site";
        public string SiteNom
        {
            get { return siteNom; }
            set { siteNom = value; }
        }

        private string siteSlogan = "Slogan du Site";
        public string SiteSlogan
        {
            get { return siteSlogan; }
            set { siteSlogan = value; }
        }

        private string siteAddress = "Adresse du Site";
        public string SiteAddress
        {
            get { return siteAddress; }
            set { siteAddress = value; }
        }

        private string siteCopyright = "Copyright du Site";
        public string SiteCopyright
        {
            get { return siteCopyright; }
            set { siteCopyright = value; }
        }

        private string adresseWebmaster = "webmaster@monsite.com";
        public string AdresseWebmaster
        {
            get { return adresseWebmaster; }
            set { adresseWebmaster = value; }
        }

        private string sujetCourrielMaj = "Mise à jour";
        public string SujetCourrielMaj
        {
            get { return sujetCourrielMaj; }
            set { sujetCourrielMaj = value; }
        }

        private string labelBoutonQuestion = "Répondre";
        public string LabelBoutonQuestion
        {
            get { return labelBoutonQuestion; }
            set { labelBoutonQuestion = value; }
        }

        private bool envoyerMiseAjour = true;
        public bool EnvoyerMiseAjour
        {
            get { return envoyerMiseAjour; }
            set { envoyerMiseAjour = value; }
        }

        private string radioButtonListQuestionnaireRepeatColumn = "5";
        public string RadioButtonListQuestionnaireRepeatColumn
        {
            get { return radioButtonListQuestionnaireRepeatColumn; }
            set { radioButtonListQuestionnaireRepeatColumn = value; }
        }
        
        //
        // Limitations de l'utilisateur decouverte
        // Utilisees au moment de creer un nouvel utilisateur
        //
        private string gratuitLimiteQuestionnaires = "2";
        public string GratuitLimiteQuestionnaires
        {
            get { return gratuitLimiteQuestionnaires; }
            set { gratuitLimiteQuestionnaires = value; }
        }
        private string gratuitLimiteQuestions = "5";
        public string GratuitLimiteQuestions
        {
            get { return gratuitLimiteQuestions; }
            set { gratuitLimiteQuestions = value; }
        }
        private string gratuitLimiteInterviewes = "10";
        public string GratuitLimiteInterviewes
        {
            get { return gratuitLimiteInterviewes; }
            set { gratuitLimiteInterviewes = value; }
        }
        private string gratuitLimiteReponses = "5";
        public string GratuitLimiteReponses
        {
            get { return gratuitLimiteReponses; }
            set { gratuitLimiteReponses = value; }
        }

        //
        // Limitations l'utilisateur standard abonne
        // Utilisees au moment ou l'utilisateur a paye son abonnement
        //
        private string abonneLimiteQuestionnaires = "3";
        public string AbonneLimiteQuestionnaires
        {
            get { return abonneLimiteQuestionnaires; }
            set { abonneLimiteQuestionnaires = value; }
        }
        private string abonneLimiteQuestions = "100";
        public string AbonneLimiteQuestions
        {
            get { return abonneLimiteQuestions; }
            set { abonneLimiteQuestions = value; }
        }
        private string abonneLimiteInterviewes = "1000";
        public string AbonneLimiteInterviewes
        {
            get { return abonneLimiteInterviewes; }
            set { abonneLimiteInterviewes = value; }
        }
        // N'est pas utilisée
        private string abonneLimiteReponses = "500";
        public string AbonneLimiteReponses
        {
            get { return abonneLimiteReponses; }
            set { abonneLimiteReponses = value; }
        }

        //
        // Limitation de la liste des imports
        //
        private string limitationImportsInterviewes = "500";
        public string LimitationImportsInterviewes
        {
            get { return limitationImportsInterviewes; }
            set { limitationImportsInterviewes = value; }
        }

    }
}