// 
// Langage de programmation du Questionnaire
// Un suite d'instruction est analysee si la première instruction donne une bonne reponse
// les suivantes ne sont pas annalysees 
// Formats des instructions exemple :
// next Qz la plus simple branchement inconditionnel vers Q
// si Rn alors Qp
// si !Rn alors Qp
// si Ra et Rb et ... Rz alors Qz
// Si Ra ou Rb ou ... Rz alors Qx
// et toutes les conbinaisons separees par une virgule
// seul equart par rapport au langage on peut mettre des espaces apres les virgules
//
// 13/11/2008
// Correction d'un bug dans Compute() bonneReponse = et; pour une suite d'instruction
// Si on avait trouve une bonne reponse dans une instruction precedente et qu'une instruction suivante
// etait du type case Type.SiEtAlors: elle remettait bonneReponse a faux
// Il faut continuer la validation de toutes les instructions mais conserver bonneReponse a vrai c'est 
// ce que fait : if ( bonneReponse == false )
// Si bonneReponse == false alors on prend en compte l'instruction de type Type.SiEtAlors
// Sinon on laisse bonneReponse a vrai 
//
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

/// <summary>
/// Traiter une Instruction au format "si Rn alors Qp"
/// </summary>
public class Instruction
{
    public static char Separateur = ','; // Separateur d'instruction
    private static string[] Ou = { " ou " };
    private static string[] Et = { " et " };
    private static ArrayList Reponses = new ArrayList();
    private static int Question = 0;

    public enum Type
    {
        Null,
        SiAlors,
        SiOuAlors,
        SiEtAlors,
        Next
    }

    // Interface avec les objets graphiques :
    // RadioList liste a choix unique : SiEtAlors est non valide
    // CheckBoxList liste a choix multiples
    public static bool Valide( string instruction, bool choixUnique )
    {
        string[] __instruction = instruction.Split( Separateur );

        bool valide = true;
        Type ti = Type.Null;

        foreach ( string _instruction in __instruction )
        {
            ti = Valide( _instruction );
            valide = valide & ( ti != Type.Null && ( ( ti == Type.SiEtAlors && choixUnique ) == false ) );
            if ( valide == false )
                break;
        }
        return valide;
    }

    /// <summary>
    /// On a le droit aux espace apres la virgule
    /// Deux usages d'abord on valide l'entree utilisateur et on se fou de Reponses et Question
    /// Puis dans Compute Valide remplie Reponses et Question et retourne le type d'instruction a computer
    /// </summary>
    /// <param name="instruction">instruction</param>
    /// <returns>Type.Null si l'instruction n'est pas valide</returns>
    public static Type Valide( string instruction )
    {
        string[] ___instruction = instruction.Split( Separateur );

        bool valide = true;
        Type ti = Type.Null;

        foreach ( string __instruction in ___instruction )
        {
            string _instruction = __instruction.Trim();

            if ( _instruction.Contains( "next " ) )
            {
                valide = valide & valideNext( _instruction, ref Question );
                ti = Type.Next;
            }
            else if ( _instruction.Contains( " ou " ) )
            {
                valide = valide & valideSiOuEtAlors( _instruction, Ou, ref Reponses, ref Question );
                ti = Type.SiOuAlors;
            }
            else if ( _instruction.Contains( " et " ) )
            {
                valide = valide & valideSiOuEtAlors( _instruction, Et, ref Reponses, ref Question );
                ti = Type.SiEtAlors;
            }
            else
            {
                valide = valide & valideSiAlors( _instruction, ref Reponses, ref Question );
                ti = Type.SiAlors;
            }
            if ( valide == false )
            {
                return Type.Null;
            }
        }

        return ti;
    }

    /// <summary>
    /// Format d'une instruction de branchement : next Qz
    /// </summary>
    private static bool valideNext( string instruction, ref int que  )
    {
        string[] inst = instruction.Split( ' ' );

        if ( inst.Length != 2 )
        {
            return false;
        }

        if ( inst[ 1 ].Substring( 0, 1 ) != "Q" )
        {
            return false;
        }

        try
        {
            int q = int.Parse( inst[ 1 ].Substring( 1 ) );
            que = q;
        }
        catch
        {
            return false;
        }

        return true;
    }


    /// <summary>
    /// Format d'une instruction conditionnnelle : si Rx alors Qy
    /// </summary>
    /// <param name="instruction">instruction</param>
    /// <returns>true si l'instruction est valide</returns>
    private static bool valideSiAlors( string instruction, ref ArrayList reponses, ref int question )
    {
        string[] inst = instruction.Split( ' ' );

        if ( inst.Length != 4 )
        {
            return false;
        }
        if ( inst[ 0 ] != "si" )
        {
            return false;
        }
        if ( inst[ 1 ].Substring( 0, 1 ) != "R" && inst[ 1 ].Substring( 0, 2 ) != "!R" )
        {
            return false;
        }
        if ( inst[ 1 ].Substring( 0, 1 ) == "R" )
        {
            try
            {
                int r = int.Parse( inst[ 1 ].Substring( 1 ) );
                reponses.Add( r );
            }
            catch
            {
                return false;
            }
        }
        else // !R
        {
            try
            {
                int r = int.Parse( inst[ 1 ].Substring( 2 ) );
                reponses.Add( - r );
            }
            catch
            {
                return false;
            }
        }

        if ( inst[ 2 ] != "alors" )
        {
            return false;
        }
        if ( inst[ 3 ].Substring( 0, 1 ) != "Q" )
        {
            return false;
        }
        try
        {
            int q = int.Parse( inst[ 3 ].Substring( 1 ) );
            question = q;
        }
        catch
        {
            return false;
        }
        return true;
    }

    // format : si Ra ou Rb ou ... Rn alors Qz
    // ou si Ra et Rb et ... Rn alors Qz
    private static bool valideSiOuEtAlors( string instruction, string[] ouEt, ref ArrayList reponses, ref int question )
    {
        string[] inst = instruction.Split( ouEt, System.StringSplitOptions.None );

        // entete
        string[] si = inst[ 0 ].Split( ' ' );
        if ( si.Length != 2 )
        {
            return false;
        }
        if ( si[ 0 ] != "si" )
        {
            return false;
        }

        if ( si[ 1 ].Substring( 0, 1 ) != "R" && si[ 1 ].Substring( 0, 2 ) != "!R" )
        {
            return false;
        }
        if ( si[ 1 ].Substring( 0, 1 ) == "R" )
        {
            try
            {
                int r = int.Parse( si[ 1 ].Substring( 1 ) );
                reponses.Add( r );
            }
            catch
            {
                return false;
            }
        }
        else // !R
        {
            try
            {
                int r = int.Parse( si[ 1 ].Substring( 2 ) );
                reponses.Add( -r );
            }
            catch
            {
                return false;
            }
        }

        // corps
        for ( int i = 1;i <= inst.Length - 2;i++ )
        {
            if ( inst[ i ].Substring( 0, 1 ) != "R" && inst[ i ].Substring( 0, 2 ) != "!R" )
            {
                return false;
            }
            if ( inst[ i ].Substring( 0, 1 ) == "R" )
            {
                try
                {
                    int r = int.Parse( inst[ 1 ].Substring( 1 ) );
                    reponses.Add( r );
                }
                catch
                {
                    return false;
                }
            }
            else // !R
            {
                try
                {
                    int r = int.Parse( inst[ 1 ].Substring( 2 ) );
                    reponses.Add( -r );
                }
                catch
                {
                    return false;
                }
            }

        }

        // fin
        string[] alors = { " alors " };
        string[] fin = inst[ inst.Length - 1 ].Split( alors, System.StringSplitOptions.None );
        if ( fin[ 0 ].Substring( 0, 1 ) != "R" && fin[ 0 ].Substring( 0, 2 ) != "!R" )
        {
            return false;
        }

        if ( fin[ 0 ].Substring( 0, 1 ) == "R" )
        {
            try
            {
                int r = int.Parse( fin[ 0 ].Substring( 1 ) );
                reponses.Add( r );
            }
            catch
            {
                return false;
            }
        }
        else // !R
        {
            try
            {
                int r = int.Parse( fin[ 0 ].Substring( 2 ) );
                reponses.Add( -r );
            }
            catch
            {
                return false;
            }
        }

        if ( fin[ 1 ].Substring( 0, 1 ) != "Q" )
        {
            return false;
        }
        try
        {
            int q = int.Parse( fin[ 1 ].Substring( 1 ) );
            question = q;
        }
        catch
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Trouver la prochaine Question de Saut dans ref int question
    /// retourne true si l'utilisateur a fait le bon choix
    /// </summary>
    public static bool Compute( string instructions, ArrayList reponses, ref int question, ref ArrayList reponsesTrouvees, ref ArrayList questionsTrouvees )
    {
        string[] __instructions = instructions.Split( Separateur );
        bool bonneReponse = false;
        question = -1;
        foreach ( string _instruction in __instructions )
        {
            Reponses.Clear();

            Type ti = Valide( _instruction ); // Remplie au passage Reponse et Question

            // Donner les Questions trouvees dans les instructions pour verification
            questionsTrouvees.Add( Question );

            // Donner les Reponses trouvees dans l'instruction pour verification
            foreach ( int r in Reponses )
            {
                if ( r < 0 )
                    reponsesTrouvees.Add( -r ); 
                else
                    reponsesTrouvees.Add( r );
            }      
      
            switch ( ti )
            {
                case Type.Next:
                    bonneReponse = true;
                    break;

                case Type.SiAlors:
                case Type.SiOuAlors:
                        foreach ( int r in Reponses )
                        {
                            if ( reponses.Contains( r ) == true )
                            {
                                bonneReponse = true;
                                break;
                            }
                        }
                    break;

                case Type.SiEtAlors:
                    bool et = true;
                    foreach ( int r in Reponses )
                    {
                        if ( reponses.Contains( r ) == false )
                        {
                            et = false;
                        }
                    }
                    if ( bonneReponse == false )
                    {
                        // pas encore de bonne reponse, c'est peut etre celle la
                        // si bonneReponse == true c'est qu'une instruction precedente etait bonne
                        bonneReponse = et;
                    }
                    break;
            }

            if ( bonneReponse && question == -1 )
            {
                question = Question;
                // on ne break pas pour continuer la verification des instructions
                // mais on sauvegarde la premiere bonne Question
            }
        }

        return bonneReponse;
    }
}
