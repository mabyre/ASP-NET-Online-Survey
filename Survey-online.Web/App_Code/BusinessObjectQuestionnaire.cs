// 
// A terme on devrait pouvoir retrouver ici les objets d'un Questionnaire de facon structure
// pour l'instant un questionnaire est definit par morceaux dans SessionState alors...
//
//
using System;
using System.Collections;
using System.Collections.Generic;

///<summary>
/// Business Object pour Questionnaire
///</summary>
///

 // En choisissant ce type,
 // on defini le type de question et le type de réponse
public class TypeQuestionReponse
{
    //public const string Ouverte = "Ouverte";
    //public const string FermeChoixSimple = "Fermée, choix simple";
    //public const string FermeChoixMulitple = "Fermée, choix multiple";
    //public const string SemiOuverteChoixSimple = "Semi-ouverte, choix simple";
    //public const string SemiOuverteChoixMultiple = "Semi-ouverte, choix multiple";
    //public const string Numerique = "Numérique";
    //public const string Date = "Date";

    public const string ChoixSimple = "Choix simple";
    public const string ChoixMultiple = "Choix multiple";
    public const string SemiOuverte = "Semi-ouverte";
    public const string Ouverte = "Ouverte";
    public const string Numerique = "Numérique";
    public const string Date = "Date";

    public static ArrayList List()
    {
        ArrayList al = new ArrayList();
        al.Add( ChoixSimple );
        al.Add( ChoixMultiple );
        al.Add( SemiOuverte );
        al.Add( Ouverte );
        al.Add( Numerique );
        al.Add( Date );
        return al;
    }

    // A partir de type QuestionReponse, trouver le type de Reponse
    public static string GetTypeReponse( string type )
    {
        string typeReponse = "";
        switch ( type )
        {
            case ChoixSimple: // Type de QuestionReponse
            case ChoixMultiple:
                typeReponse = TypeReponse.Choix;
                break;
            case Ouverte:
                typeReponse = TypeReponse.Ouverte;
                break;
            case SemiOuverte:
                typeReponse = TypeReponse.SemiOuverte;
                break;
            case Numerique:
                typeReponse = TypeReponse.Numerique;
                break;
            case Date:
                typeReponse = TypeReponse.Date;
                break;
        }
        return typeReponse;
    }

    // A partir de type QuestionReponse, trouver le type de Question
    public static bool GetTypeQuestion( string type )
    {
        bool choixMultiple = false;
        switch ( type )
        {
            case Ouverte:
            case ChoixSimple:
            case Numerique:
            case Date:
                choixMultiple = false; // choix simple
                break;
            case ChoixMultiple:
                choixMultiple = true; // choix multiple
                break;
        }
        return choixMultiple;
    }
}

public class TypeQuestion
{
    public const string ChoixSimple = "Choix simple";
    public const string ChoixMultiple = "Choix multiple";

    public static ArrayList List()
    {
        ArrayList al = new ArrayList();
        al.Add( ChoixSimple );
        al.Add( ChoixMultiple );
        return al;
    }
}

public class TypeReponse
{
    public const string Choix = "Choix";
    public const string Ouverte = "Ouverte";
    public const string SemiOuverte = "Semi-ouverte";
    public const string Numerique = "Numérique";
    public const string Date = "Date";

    public static ArrayList List()
    {
        ArrayList al = new ArrayList();
        al.Add( Choix );
        al.Add( Ouverte );
        al.Add( SemiOuverte );
        al.Add( Numerique );
        al.Add( Date );
        return al;
    }

    // Reponses reprentees par le meme objet TextBox
    public static bool EstTextuelle( string type )
    {
        return type == TypeReponse.Numerique 
            || type == TypeReponse.Ouverte
            || type == TypeReponse.Date
            || type == TypeReponse.SemiOuverte;
    }

    // Reponses reprentees par le meme objet TextBox
    public static bool EstTextBox( string type )
    {
        return type == TypeReponse.Numerique
            || type == TypeReponse.Ouverte;
    }
}

// Tag marquant la fin d'un tableau
public class Tableau
{
    public const string Fin = "fin tableau";
    public const string Classement = " %%CLASSEMENT%%";
}

namespace BusinessObject
{
    /// <summary>
    /// Tester le moyen de loguer l'interviewé, choisi par l'utilisateur
    /// </summary>
    public class LogonInterviewe
    {
        public LogonInterviewe( string corpsEmail )
        {
            if ( corpsEmail.Contains( "%%LIEN%%" ) == true )
            {
                _Automatique = true;
            }

            if ( _Automatique == false )
            {
                if ( corpsEmail.Contains( "%%ADRESSE_EMAIL%%" ) == true
                     && corpsEmail.Contains( "%%CODE_ACCES%%" ) == true
                     && corpsEmail.Contains( "%%LOG%%" ) == true )
                {
                    _AdresseEmailEtCode = true;
                }
            }

            if ( _Automatique == false && _AdresseEmailEtCode == false )
            {
                _Message += "Erreur : la section CorpsEmail ne contient pas de moyen d'authentifier l'interviewé pour le questionnaire.<br/>";
                _Message += "- Authentification automatique :<br/>";
                _Message += "  CorpsEmail doit contenir : %%LIEN%%<br/>";
                _Message += "- Authentification par adresse email et code d'accès :<br/>";
                _Message += "  CorpsEmail doit contenir : %%ADRESSE_EMAIL%%, %%CODE_ACCES%% et %%LOG%%<br/>";
                _Message += "<br/>Vérifiez le format de l'email envoyé aux interviewés.";
            }
        }

        private bool _Automatique = false;
        public bool Automatique
        {
            get { return _Automatique; }
        }

        private bool _AdresseEmailEtCode = false;
        public bool AdresseEmailEtCode
        {
            get { return _AdresseEmailEtCode; }
        }

        private string _Message = "";
        public string Message
        {
            get { return _Message; }
        }
    }
}


