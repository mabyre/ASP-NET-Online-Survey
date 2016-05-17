//
// Dans un fichier CSV les colonnes sont presentes ou non
// on va faire l'hypothese qu'en plus elles peuvent etre dans un ordre different 
//
// Bon c'est vrai, je rale beaucoup pendant le developpement de ce composant, alors que j'ai commence 
// a developper cette fonctionnalite sans connaitre les specs de l'export depuis outlook au format 
// "fichier separe par des virgules (windows)"
// Je n'ai eu aucun probleme avec le carnet d'adresses specs simples sans fioritures : colonne;
// outlook est une grosse merde d'application stupide avec des specs idiotes du genre "Nom, Prenom" 
// qui repette 2 fois l'info Prénom c'est debile ! Et pourtant absolument necessaire si veut que
// l'import soit un tant soit peu automatique et sinon on a faire a la fonction de mappage !
// Avec des specs du genre colonne non presente representee par "", ou ,, Qui a code cet algo ?
// Avec des /r/n dans les valeurs de colonnes
// Avec la colonne "Titre" qui doit etre presente et vide pour l'import soit automatique
// Et je ne parlerais pas de la fonction debile de mappage d'outlook qui est impossible a maniper
// outlook une vraie daube d'appli microsoft de merde incompatible avec le carnet d'adresses 
// de microsoft pourtant
// Avec des \r\n dans une chaine valeur d'un champ l'algo se complique 
// terriblement tanpis on gruge outlook de merde en declarant que la ligne est terminee 
// par "Non spécifié" ce qui est le cas lorsque l'on a pas d'"Utilisateur 4" quelle merde !
// Bon c'est un vrai scandale cette merde d'oulook rien ne se ressemble, il suffit de mapper 
// autrement et ya la moitie des champs qui petent alle on y arrive quand meme mais c'est pas 
// stable.
//
// Creation de 2 objets un pour le Carnet d'adresse (Accessoire de Windows)
// l'autre pour outlook
// 
// Restrictions importation Carnet d'adresse :
// La "fonction" d'un contact n'est pas importée, il faudrait la créer dans la base 
// ce qui implique que si l'utilisateur à fait n'importe quoi dans son carnet on se retrouverait avec
// une base curieuse pleine de fonction, on laisse donc l'utilisateur resynchroniser à la main !
// On ne recupere qe l'adresse du domicile sachant que dans l'application un contact est lie a un site
// et que la encore l'utilisateur devra faire une synchro manuelle
//
// Exportation Carnet Outlook :
// Il faut creer artificiellement une colonne "Titre" vide pour que cette conne d'outlook prenne en compte
// la deuxieme colonne "Titre" comme etant la fonction du contact, simple non ?
//
// Utilisation de la colonne Titre qui est en fait Civilite
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
using System.IO;
using System.Text;
using System.Collections;
using Sql.Data;
using Sql.Web.Data;

namespace ImportExportContact
{
    class Colonne
    {
        public Colonne( int indice, string nom, bool existe )
        {
            _indice = indice;
            _nom = nom;
            _existe = false;
        }

        int _indice = 0;
        public int Indice
        {
            get { return _indice; }
            set { _indice = value; }
        }

        string _nom = "";
        public string Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }

        bool _existe = false;
        public bool Existe
        {
            get { return _existe; }
            set { _existe = value; }
        }
    }

    class ColonneCollection : IEnumerable
    {
        ArrayList _colonnes = new ArrayList();

        public ColonneCollection() { }

        public ColonneCollection( string[] headerColumns )
        {
            for ( int i = 0;i < headerColumns.Length;i++ )
            {
                Colonne c1 = new Colonne( i, headerColumns[ i ], false );
                _colonnes.Add( c1 );
            }
        }

        public IEnumerator GetEnumerator()
        {
            return _colonnes.GetEnumerator();
        }

        public int Count
        {
            get { return _colonnes.Count; }
        }

        public void Add( Colonne c )
        {
            _colonnes.Add( c );
        }

        public void RemoveAt( int i )
        {
            _colonnes.RemoveAt( i );
        }

        public Colonne this[ int index ]
        {
            get { return ( Colonne )( _colonnes[ index ] ); }
        }

        public bool Contains( Colonne colonne )
        {
            foreach ( Colonne c in this )
            {
                if ( c.Nom == colonne.Nom && c.Indice == colonne.Indice )
                    return true;
            }
            return false;
        }
    }

    public class Utility
    {
        // Transforme la table en string
        public static string TableToString( DataTable dataTable )
        {
            StringBuilder sb = new StringBuilder();

            if ( dataTable.Columns.Count != 0 )
            {
                foreach ( DataColumn column in dataTable.Columns )
                {
                    sb.Append( column.ColumnName );
                }
                sb.AppendLine();

                foreach ( DataRow row in dataTable.Rows )
                {
                    foreach ( DataColumn column in dataTable.Columns )
                    {
                        sb.Append( row[ column ].ToString() );
                    }
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// Le carnet d'adresse c'est l'accessoire de Windows
    /// </summary>
    public class CarnetAdresse
    {
        // Une seul adresse de messagerie, celle par defaut dans le carnet d'adresses
        // c'est la definition de la fiche contact en accord avec le carnet d'adresses
        // on peut a ajouter des champs, il faut simplement verifier les indexes correspondants
        public static string[] HeaderColumns =
        {
            "Civilité",                 // 0
            "Nom",                      // 1
            "Prénom",                   // 2
            "Adresse de messagerie",    // 3
            "Rue (domicile)",           // 4
            "Ville (domicile)",         // 5
            "Code postal (domicile)",   // 6
            "Téléphone personnel",      // 7
            "Téléphone professionnel",  // 8
            "Téléphone mobile",         // 9
            "Télécopie professionnelle",// 10
            "Société",                  // 11
            "Fonction",                 // 12 
            "Page Web (bureau)",        // 13
            "Remarques"                 // 14
        };

        /// <summary>
        /// Creer la table "traduire" la table au format CSV
        /// </summary>
        // On remarque que toutes les infos ne sont extraites du carnet d'adresse
        public static DataTable CreateTable()
        {
            ArrayList entetes = new ArrayList();
            ColonneCollection colonnes = new ColonneCollection( HeaderColumns );
            for ( int i = 0;i < colonnes.Count;i++ )
            {
                // Pas de ";" pour la derniere colonne
                if ( i < colonnes.Count - 1 )
                {
                    entetes.Add( colonnes[ i ].Nom + ";" );
                }
                else
                {
                    entetes.Add( colonnes[ i ].Nom );
                }
            }

            DataTable dt = new DataTable();
            foreach ( string entete in entetes )
            {
                DataColumn colonne = new DataColumn( entete, System.Type.GetType( "System.String" ) );
                dt.Columns.Add( colonne );
            }

            PersonneCollection personnes = PersonneCollection.GetAll();
            foreach ( Personne p in personnes )
            {
                DataRow dr;
                dr = dt.NewRow();

                dr[ entetes[ 0 ].ToString() ] = p.Civilite + ";";
                dr[ entetes[ 1 ].ToString() ] = p.Nom + ";";
                dr[ entetes[ 2 ].ToString() ] = p.Prenom + ";";
                dr[ entetes[ 3 ].ToString() ] = p.EmailBureau + ";";
                dr[ entetes[ 4 ].ToString() ] = p.Adresse + ";";
                dr[ entetes[ 5 ].ToString() ] = p.Ville + ";";
                dr[ entetes[ 6 ].ToString() ] = p.CodePostal + ";";
                dr[ entetes[ 7 ].ToString() ] = p.TelephonePerso + ";";
                dr[ entetes[ 8 ].ToString() ] = p.TelephoneBureau + ";";
                dr[ entetes[ 9 ].ToString() ] = p.TelephoneMobile + ";";
                dr[ entetes[ 10 ].ToString() ] = p.Fax + ";";
                dr[ entetes[ 11 ].ToString() ] = p.Societe + ";";
                dr[ entetes[ 12 ].ToString() ] = p.Fonction + ";";
                dr[ entetes[ 13 ].ToString() ] = p.LienHTML + ";";
                dr[ entetes[ 14 ].ToString() ] = p.Memo; //+ ";";

                dt.Rows.Add( dr );
            }

            return dt;
        }

        /// <summary>
        /// Lecture du fichier au format CVS creation d'une personne et creation dans la database
        /// </summary>
        /// <param name="fileName">le fichier</param>
        /// <param name="createInDataBase">créer la personne dans la database</param>
        /// <param name="personnes">sinon c'est la collection de personnes</param>
        /// <returns></returns>
        public static string ImportFile( string fileName )
        {
            PersonneCollection personnes = null;
            string status = ImportFile( fileName, ref personnes );
            return status;
        }

        /// <summary>
        /// Lecture du fichier au format CVS creation d'une personne
        /// </summary>
        /// <param name="fileName">le fichier</param>
        /// <param name="personnes">collection de personnes si null on cree dans la database</param>
        /// <returns></returns>
        public static string ImportFile( string fileName, ref PersonneCollection personnes )
        {
            string erreurMessage = "";
            FileStream fs = new FileStream( fileName, FileMode.Open, FileAccess.Read );
            StreamReader sr = new StreamReader( fs, Encoding.UTF7 );
            string s = sr.ReadToEnd();
            fs.Close();
            sr.Close();

            string separateurDOS = "\r\n";
            string separateurUnix = "\r";
            if ( s.Contains( separateurDOS ) == false )
            {
                if ( s.Contains( separateurUnix ) == false )
                {
                    erreurMessage = "Ce fichier n'est ni au format Unix ni au format DOS.<br>";
                    return erreurMessage;
                }
                erreurMessage += "Conversion du fichier Unix vers DOS.<br>";
                s = Strings.UnixToDos( s );
            }

            string[] separateur = { "\r\n" };
            string[] lignes = s.Split( separateur, System.StringSplitOptions.None );
            string entete = lignes[ 0 ];
            string[] entetes = entete.Split( ';' );

            if ( entetes.Length <= 2 )
            {
                erreurMessage = "Ce fichier n'est pas au format du carnet d'adresses.<br>";
                return erreurMessage;
            }

            // La deuxieme colonne Nom pose un probleme c'est le nom complet 
            // mais on n'en veut pas d'ou ce caviardage par une colonne qui n'existe pas
            // sauf si le fichier vient d'excel dans ce cas on ne ve pas faire chier l'utilisateur !
            //if ( entetes.Length > 3 ) entetes[ 3 ] = "aaaaaaaaa"; RAS LE BOL JE SAIS PLUS

            ColonneCollection cc = new ColonneCollection( HeaderColumns );
            ColonneCollection colonnesExistantes = new ColonneCollection();

            // Enregistrer les colonnes qui existent
            for ( int i = 0;i < entetes.Length;i++ )
            {
                foreach ( Colonne c in cc )
                {
                    if ( c.Nom.CompareTo( entetes[ i ] ) == 0 )
                    {
                        // La colonne existe dans le fichier
                        c.Existe = true;
                        c.Indice = i;
                        colonnesExistantes.Add( c );
                    }
                }
            }

            if ( colonnesExistantes.Count <= 0 )
            {
                erreurMessage = "Format de fichier non valide.<br>";
                return erreurMessage;
            }

            //BRY00020100209
            if ( SessionState.Limitations.LimitesInterviewesAtteinte( lignes.Length ) )
            {
                Tools.PageValidation( "La limite du nombre d'Interviewés : " + SessionState.Limitations.NombreInterviewes + " est atteinte.<br/>Contactez l'administrateur." );
            }

            // La derniere ligne peut etre vide
            for ( int i = 1;i < lignes.Length && lignes[ i ] != "";i++ )
            {
                // Incroyable on voit Excel de daube qui met des ';' a lin de la ligne
                // mais il s'arrete au bout de 15 lignes et il en met plus !!!
                string[] essai = lignes[ i ].Split( ';' );
                if ( essai.Length < colonnesExistantes.Count )
                {
                    // Il manque une colonne ! 
                    // Donc on ajoute une colonne vide !
                    lignes[ i ] = lignes[ i ] + ";";
                }

                string[] valeurs = lignes[ i ].Split( ';' );

                Personne p = new Personne();

                foreach ( Colonne c in colonnesExistantes )
                {
                    if ( c.Existe )
                    {
                        if ( c.Nom == HeaderColumns[ 0 ] )
                            p.Civilite = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 1 ] )
                            p.Nom = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 2 ] )
                            p.Prenom = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 3 ] )
                            p.EmailBureau = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 4 ] )
                            p.Adresse = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 5 ] )
                            p.Ville = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 6 ] )
                            p.CodePostal = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 7 ] )
                            p.TelephonePerso = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 8 ] )
                            p.TelephoneBureau = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 9 ] )
                            p.TelephoneMobile = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 10 ] )
                            p.Fax = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 11 ] )
                            p.Societe = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 12 ] )
                            p.Fonction = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 13 ] )
                            p.LienHTML = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 14  ] )
                            p.Memo = valeurs[ c.Indice ];
                    }
                }

                if ( personnes == null )
                {
                     int ret = Personne.Create( p, true, ref erreurMessage );
                }
                else
                {
                    personnes.Add( p );
                }
            }

            return erreurMessage;
        }
    }

    public class CarnetOutlook
    {
        public static string[] HeaderColumns =
        {
            "Civlité",                  // 0
            "Nom",                      // 1
            "Prénom",                   // 2
            "Société",                  // 3
            "Titre",                    // 4 
            "Adresse de messagerie",    // 5
            "Adresse de messagerie 2",  // 6
            "Rue (domicile)",           // 7
            "Ville (domicile)",         // 8
            "Code postal (domicile)",   // 9
            "Téléphone (domicile)",     // 10
            "Téléphone (bureau)",       // 11
            "Tél. mobile",              // 12
            "Télécopie (bureau)",       // 13
            "Page Web (domicile)",      // 14
            "Notes"                     // 15
        };

        // Creer la table "traduire" la table au format Outlook
        public static DataTable CreateTable()
        {
            ArrayList entetes = new ArrayList();
            
            // Creer artificiellement une colonne "Titre" vide ! 
            // on lui baise la gueule a outlook de merde avec son format pourri
            // on remarquera l'espace dans le titre de la colonne sinon erreur :
            // elle existe deja dans le DataTable et c'est la cata ca voudrait
            // qu'on ne peut pas utiliser un DataTable mais ca marche alors
            //entetes.Add( "\"Titre \"," );

            ColonneCollection colonnes = new ColonneCollection( HeaderColumns );
            for ( int i = 0;i < colonnes.Count;i++ )
            {
                // Pas de "," pour la derniere colonne
                if ( i < colonnes.Count - 1 )
                {
                    entetes.Add( "\"" + colonnes[ i ].Nom + "\"," );
                }
                else
                {
                    entetes.Add( "\"" + colonnes[ i ].Nom + "\"" );
                }
            }

            DataTable dt = new DataTable();
            foreach ( string entete in entetes )
            {
                DataColumn colonne = new DataColumn( entete, System.Type.GetType( "System.String" ) );
                dt.Columns.Add( colonne );
            }

            PersonneCollection personnes = PersonneCollection.GetAll();
            foreach ( Personne p in personnes )
            {
                DataRow dr;
                dr = dt.NewRow();
                
                dr[ entetes[ 0 ].ToString() ] = "\"" + p.Civilite + "\",";
                dr[ entetes[ 1 ].ToString() ] = "\"" + p.Nom + ", " + p.Prenom + "\",";
                dr[ entetes[ 2 ].ToString() ] = "\"" + p.Prenom + "\",";
                dr[ entetes[ 3 ].ToString() ] = "\"" + p.Societe + "\",";
                dr[ entetes[ 4 ].ToString() ] = "\"" + p.Fonction + "\",";
                dr[ entetes[ 5 ].ToString() ] = "\"" + p.EmailPerso + "\",";
                dr[ entetes[ 6 ].ToString() ] = "\"" + p.EmailBureau + "\",";
                dr[ entetes[ 7 ].ToString() ] = "\"" + p.Adresse + "\",";
                dr[ entetes[ 8 ].ToString() ] = "\"" + p.Ville + "\",";
                dr[ entetes[ 9 ].ToString() ] = "\"" + p.CodePostal + "\",";
                dr[ entetes[ 10 ].ToString() ] = "\"" + p.TelephonePerso + "\",";
                dr[ entetes[ 11 ].ToString() ] = "\"" + p.TelephoneBureau + "\",";
                dr[ entetes[ 12 ].ToString() ] = "\"" + p.TelephoneMobile + "\",";
                dr[ entetes[ 13 ].ToString() ] = "\"" + p.Fax + "\",";
                dr[ entetes[ 14 ].ToString() ] = "\"" + p.LienHTML + "\",";
                dr[ entetes[ 15 ].ToString() ] = "\"" + p.Memo + "\""; // pas de virgule pour la derniere valeur

                dt.Rows.Add( dr );
            }

            return dt;
        }

        // apres avoir splite en valeur on se retrouve avec des " au debut et des " a la fin!
        protected static string SupprimeCaractereDeMerde( string str )
        {
            string s = str;
            if ( str != "" )
                s = s.Substring( 1, str.Length - 2 );
            return s;
        }

        /// <summary>
        /// Lecture du fichier au format CVS creation d'une personne et creation dans la database
        /// </summary>
        /// <param name="fileName">le fichier</param>
        /// <param name="createInDataBase"> créer la personne dans la database</param>
        /// <param name="personnes">sinon c'est la collection de personnes</param>
        /// <returns></returns>
        public static string ImportFile( string fileName )
        {
            PersonneCollection personnes = null;
            string status = ImportFile( fileName, ref personnes );
            return status;
        }

        // Transforme la table en string
        public static string ImportFile( string fileName, ref PersonneCollection personnes )
        {
            string erreurMessage = "";
            FileStream fs = new FileStream( fileName, FileMode.Open, FileAccess.Read );
            StreamReader sr = new StreamReader( fs, Encoding.UTF7 );
            string s = sr.ReadToEnd();
            fs.Close();
            sr.Close();

            string[] separateurLigne = { "\"Non spécifié\"\r\n", "\"Utilisateur 4\"\r\n" }; // Une hyptohese risquee
            string[] lignes = s.Split( separateurLigne, StringSplitOptions.RemoveEmptyEntries );
            if ( lignes.Length <= 1 )
            {
                erreurMessage = "Ce fichier n'est pas au format du carnet outlook.<br>";
                return erreurMessage;
            }

            string entete = lignes[ 0 ];
            char[] separteurValeur = { ',', '\"' };
            string[] entetes = entete.Split( separteurValeur, StringSplitOptions.RemoveEmptyEntries );
            if ( entetes.Length <= 1 )
            {
                erreurMessage = "Ce fichier n'est pas au format du carnet outlook.<br>";
                return erreurMessage;
            }

            // La deuxieme colonne Nom pose un probleme c'est le nom complet 
            // mais on en veut pas d'ou ce caviardage par une colonne qui n'existe pas
            // entetes[ 3 ] = "aaaaaaaaa"; Non faut pas ya pas de non complet dans outlook de merde et si !! putain de bordel quelle merde enfin microsoft est donne son plein potentiel !!
            // outlook de merde on trouve 2 colonnes Titre une pour civlite ou autre pour ...
            // donc ici on ruse a mort

            ColonneCollection cc = new ColonneCollection( HeaderColumns );
            ColonneCollection colonnesExistantes = new ColonneCollection();

            if ( entetes[ 0 ] == "Titre" )
            {
                cc[ 0 ].Existe = true;
                cc[ 0 ].Indice = 0;
                colonnesExistantes.Add( cc[ 0 ] );
                cc.RemoveAt( 0 );
            }

            foreach ( Colonne c in cc )
            {
                for ( int i = 1;i < entetes.Length;i++ )
                {
                    if ( c.Nom.CompareTo( entetes[ i ] ) == 0 )
                    {
                        // La colonne existe dans le fichier
                        c.Existe = true;
                        c.Indice = i;
                        colonnesExistantes.Add( c );
                    }
                }
            }
            
            if ( colonnesExistantes.Count <= 0 )
            {
                erreurMessage = "Format de fichier non valide.<br>";
                return erreurMessage;
            }

            //BRY00020100209
            if ( SessionState.Limitations.LimitesInterviewesAtteinte( lignes.Length ) )
            {
                Tools.PageValidation( "La limite du nombre d'Interviewés : " + SessionState.Limitations.NombreInterviewes + " est atteinte.<br/>Contactez l'administrateur." );
            }

            // Le split marche dans l'entete car on RemoveEmptyEntries
            // ici on ne peut pas le faire pour garder le bon indice de la place de la valeur
            // et donc on recommence ...
            string[] separateurVal = { "\",\"" };
            for ( int i = 1;i < lignes.Length;i++ )
            {
                string[] valeurs = lignes[ i ].Split( ',' );
                for ( int v = 0;v < valeurs.Length;v++ )
                    valeurs[ v ] = SupprimeCaractereDeMerde( valeurs[ v ] );

                Personne p = new Personne();
                foreach ( Colonne c in colonnesExistantes )
                {
                    if ( c.Existe )
                    {
                        if ( c.Nom == HeaderColumns[ 0 ] )
                            p.Civilite = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 1 ] )
                            p.Nom = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 2 ] )
                            p.Prenom = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 3 ] )
                            p.Societe = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 4 ] )
                            p.Fonction = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 5 ] )
                            p.EmailPerso = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 6 ] )
                            p.EmailBureau = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 7 ] )
                            p.Adresse = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 8 ] )
                            p.Ville = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 9 ] )
                            p.CodePostal = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 10 ] )
                            p.TelephonePerso = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 11 ] )
                            p.TelephoneBureau = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 12 ] )
                            p.TelephoneMobile = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 13 ] )
                            p.Fax = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 14 ] )
                            p.LienHTML = valeurs[ c.Indice ];
                        if ( c.Nom == HeaderColumns[ 15 ] )
                            p.Memo = valeurs[ c.Indice ];
                    }
                }

                if ( personnes == null )
                {
                    int ret = Personne.Create( p, true, ref erreurMessage );
                }
                else
                {
                    personnes.Add( p );
                }
            }

            return erreurMessage;
        }
    }
}