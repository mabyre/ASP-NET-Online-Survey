using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Web.Caching;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Security.Permissions;

public class Strings
{
    /// <summary>
    /// str == null || str == string.Empty
    /// </summary>
    static public bool IsNullOrEmpty( string str )
    {
        if ( str == null || str == string.Empty )
            return true;
        else
            return false;
    }

	static public string TexteToHTML( string str )
	{
		string r = str.Replace( "\r\n", "<br>" );
		r = r.Replace( "\n", "<br>" );
		r = r.Replace( "\r", "<br>" );
		return r;
	}

	static public string HTMLToTexte( string str )
	{
		string r = str.Replace( "<br>", "\r\n" );
		return r;
	}

    /// <summary>
    /// Sous Unix on ne trouve que "\r" comme retour charriot
    /// </summary>
    static public string UnixToDos( string str )
    {
        string r = str.Replace( "\r", "\r\n" );
        return r;
    }

    static public string ByteArrayToString( byte[] ba )
	{
        string s = "";
        for ( int i = 0 ; i < ba.Length ; i ++ )
            s += ba[ i ].ToString();

        return s;
	}
    
    // Sous entendu le tableau byte est un tableau de char
    static public string ByteArrayInString( byte[] ba )
	{
        string s = "";
        for ( int i = 0 ; i < ba.Length ; i ++ )
            s += (char)ba[ i ];

        return s;
	}

    static public string CharArrayToString( char[] ca )
	{
        string s = "";
        for ( int i = 0 ; i < ca.Length ; i ++ )
            s += ca[ i ].ToString();

        return s;
	}

    static public void CharArrayToByteArray( ref byte[] ba, char[] ca )
	{
        for ( int i = 0 ; i < ca.Length ; i ++ )
            ba[ i ] = (byte)ca[ i ];
	}

	// Rechercher "mot" dans la chaine "str"
	static public bool SearchWord( string str, string mot )
	{
		bool trouve = false;
	                
		for ( int i = 0; i < str.Length; i++ )
		{
			if ( String.Compare( mot, 0, str, i, mot.Length ) == 0 )
			{
				trouve = true;
			}
			if ( trouve == true ) break;
		}
		return trouve;
	}

    // Rechercher le "mot" dans la string "str" indiquer la position "pos" du mot
	static public bool SearchWord( string str, string mot, ref int pos )
	{
		bool trouve = false;

	    pos = 0;            
		for ( int i = 0; i < str.Length; i++ )
		{
			if ( String.Compare( mot, 0, str, i, mot.Length ) == 0 )
			{
				trouve = true;
			}
			if ( trouve == true ) 
            {
                pos = i;
                break;
            }
		}

		return trouve;
	}

    // Indiquer si le "car" est present dans la string "str" 
    static public bool SearchCar( char car , string str )
    {
        bool trouve = false;
        char[] strInChar = str.ToCharArray();

        for ( int i = 0; i < str.Length; i++ )
        {
            if ( car == strInChar[ i ] )
            {
                trouve = true;
                break;
            }
        }
        return trouve;
    }

    static public bool IsValideEmail( string email )
    {
        string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        Regex regularExpressions = new Regex( emailRegex );
        Match match = regularExpressions.Match( email );
        return ( match.Success );
    }

    //
    static public bool IsValideTelephone( string telephone )
    {
        //string emailRegex = @"^([\.|\-|\s]*[0-9]{2}){1,6}$";
        // ajout d'un + optionnel
        //string emailRegex = @"^(\+?[\.|\-|\s]*[0-9]{2}){1,6}$";

        // finalement je trouve ca sur le site http://www.dotnet-news.com/lien.aspx?ID=25412
        //string emailRegex = @"^([\(\+])?([0-9]{1,3}([\s])?)?([\+|\(|\-|\)|\s])?([0-9]{2,4})([\-|\)|\.|\s]([\s])?)?([0-9]{2,4})?([\.|\-|\s])?([0-9]{4,8})$";

        //string s1 = @"^([\(\+])?([0-9]{1,3}([\s])?)?([\+|\(|\-|\)|\s])"; // debut
        //string f1 = @"?([0-9]{2,4})([\-|\)|\.|\s]([\s])?)";
        //string f2 = @"?([0-9]{2,4})";
        //string f3 = @"?([\.|\-|\s])";
        //string f4 = @"?([0-9]{4,8})";
        //string fin = @"$"; // fin

        //string emailRegex = s1 + f1 + f2 + f3 + f4 + fin;

        // et finalement si on fesait simplement :
        string emailRegex = @"^[0-9\s\(\)\+\-\.]+$"; 

        Regex regularExpressions = new Regex( emailRegex );
        Match match = regularExpressions.Match( telephone );
        return ( match.Success );
    }

    // Remplacer les caracteres par le code Ascii
	// pour trouver un code copier le caractere et passer en mode hexa
	// ya moyen de faire autrement mais faut trouver un truc
	// dans le bordel de l'aide en ligne sur la string ...
	static public string FiltrerASCII( string stringEntree )
	{
		string[,] change = new string[,] 
		{ 
			{"\r\n","%0D%0A"},
			{"ç","%E7"},
			{"\"","%22"},
			{"@","%40"},
			{"à","%E0"}, {"â","%E2"}, {"ä","%E4"}, 
			{"é","%E9"}, {"è","%E8"}, {"ê","%EA"}, {"ê","%EA"}, {"ë","%EB"}, 
			{"ï","%EF"}, {"î","%EE"},
			{"ô","%F4"}, {"ö","%F6"}, 
			{"ù","%F9"}, {"û","%F9"}, {"ü","%FB"}                        
		};

		if ( stringEntree == null )
			return "";
            
		string filtre = stringEntree;
		for ( int i = 0; i < change.Length/2; i++ )
		{
			filtre = filtre.Replace( change[ i, 0 ], change[ i, 1 ] );
		}

		return filtre;
	}

    // Supprimer ce qui n'est pas le nom du fichier dans le path
	static public string GetFileName( string filePath )
	{
        string[] name = filePath.Split( '\\' );
        return name[ name.Length - 1 ];
	}

    // Retirer d'un text les caracteres qui ne conviennent pas dans une Url
    // Ya des fois en informatique ce qu'il faut pas ecrire de truc CON
    public static string RemoveIllegalCharacters( string text )
    {
        if ( string.IsNullOrEmpty( text ) )
            return text;

        text = text.Replace( ":", string.Empty );
        text = text.Replace( "/", string.Empty );
        text = text.Replace( "?", string.Empty );
        text = text.Replace( "#", string.Empty );
        text = text.Replace( "[", string.Empty );
        text = text.Replace( "]", string.Empty );
        text = text.Replace( "@", string.Empty );
        text = text.Replace( "*", string.Empty );
        text = text.Replace( ".", string.Empty );
        text = text.Replace( ",", string.Empty );
        text = text.Replace( "\"", string.Empty );
        text = text.Replace( "&", string.Empty );
        text = text.Replace( "'", string.Empty );
        text = text.Replace( "%", string.Empty );
        text = text.Replace( ">", string.Empty );
        text = text.Replace( "<", string.Empty );
        text = text.Replace( "|", string.Empty );
        text = text.Replace( "À", "A" );
		text = text.Replace( "Á", "A" );
		text = text.Replace( "Â", "A" );
		text = text.Replace( "Ã", "A" );
		text = text.Replace( "Ä", "A" );
		text = text.Replace( "Å", "A" );
		text = text.Replace( "Æ", "A" );
		text = text.Replace( "Ç", "C" );
		text = text.Replace( "È", "E" );
		text = text.Replace( "É", "E" );
		text = text.Replace( "Ê", "E" );
		text = text.Replace( "Ë", "E" );
		text = text.Replace( "Ì", "I" );
		text = text.Replace( "Í", "I" );
		text = text.Replace( "Î", "I" );
		text = text.Replace( "Ï", "I" );
		text = text.Replace( "Ò", "O" );
		text = text.Replace( "Ó", "O" );
		text = text.Replace( "Ô", "O" );
		text = text.Replace( "Õ", "O" );
		text = text.Replace( "Ö", "O" );
		text = text.Replace( "Ù", "U" );
		text = text.Replace( "Ú", "U" );
		text = text.Replace( "Û", "U" );
		text = text.Replace( "Ü", "U" );
		text = text.Replace( "Ý", "Y" );
		text = text.Replace( "à", "a" );
		text = text.Replace( "á", "a" );
		text = text.Replace( "â", "a" );
		text = text.Replace( "ã", "a" );
		text = text.Replace( "ä", "a" );
		text = text.Replace( "å", "a" );
		text = text.Replace( "æ", "a" );
		text = text.Replace( "ç", "c" );
		text = text.Replace( "è", "e" );
		text = text.Replace( "é", "e" );
		text = text.Replace( "ê", "e" );
		text = text.Replace( "ë", "e" );
		text = text.Replace( "ì", "i" );
		text = text.Replace( "í", "i" );
		text = text.Replace( "î", "i" );
		text = text.Replace( "ï", "i" );
		text = text.Replace( "ñ", "n" );
		text = text.Replace( "ò", "o" );
		text = text.Replace( "ó", "o" );
		text = text.Replace( "ô", "o" );
		text = text.Replace( "õ", "o" );
		text = text.Replace( "ö", "o" );
		text = text.Replace( "ù", "u" );
		text = text.Replace( "ú", "u" );
		text = text.Replace( "û", "u" );
		text = text.Replace( "ü", "u" );
		text = text.Replace( "ý", "y" );
		text = text.Replace( "ÿ", "y" );
		text = text.Replace( "œ", "o" );
		text = text.Replace( "Š", "S" );
		text = text.Replace( "š", "s" );
        text = text.Replace( "Ÿ", "Y" );
        text = text.Replace( " ", "-" );
        text = text.Replace( "--", "-" );
        //text = RemoveExtraHyphen( text );

        //return HttpUtility.UrlEncode( text ).Replace( "%", string.Empty );
        return text;
    }
}