
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Web;   
using System.Web.Security;
using System.ComponentModel;
using Sql.Data;

namespace Sql.Web.Data
{
    /// <summary>
    /// L'objet Personne
    /// </summary>
    [Serializable()]
    public class Personne : IComparer<Personne>
    {
        public Personne()
        {
        }

        /// <summary>
        /// Un Contact est Valide si sont email ou son telephone est valide
        /// </summary>
        /// <param name="personne">personne à tester</param>
        /// <returns></returns>
        public static bool ContactValide( Personne personne )
        {
            bool retour = false;
            retour = personne.EmailBureau != string.Empty && Strings.IsValideEmail( personne.EmailBureau );
            retour = retour || ( personne.TelephonePerso != string.Empty && Strings.IsValideTelephone( personne.TelephonePerso ) );
            return retour;
        }

        private static string messageErrorContactInvalide( Personne personne )
        {
            string message = "Contact avec";
            if ( personne.EmailBureau != string.Empty )
            {
                message += " l'email : " + personne.EmailBureau;
            }
            if ( personne.TelephonePerso != string.Empty )
            {
                // personne.EmailBureau est vide
                if ( message == "Contact avec" )
                {
                    message += " le téléphone : " + personne.TelephonePerso;
                }
                else
                {
                    message += " ou le téléphone : " + personne.TelephonePerso;
                }
            }
            if ( personne.EmailBureau == string.Empty && personne.TelephonePerso == string.Empty )
            {
                message += " email vide et téléphone vide";
            }
            message += " n'est pas un contact valide.<br>";
            return message;
        }

        private static string messageErrorContactExisteDeja( Personne personne )
        {
            string message = "Contact avec ";
            if ( personne.EmailBureau != string.Empty )
            {
                message += " l'email : " + personne.EmailBureau;
            }
            if ( personne.TelephonePerso != string.Empty )
            {
                if ( message == "Contact avec " )
                {
                    message += " le téléphone : " + personne.TelephonePerso;
                }
                else
                {
                    message += " ou le téléphone : " + personne.TelephonePerso;
                }
            }
            message += " et le code d'accès : " + personne.CodeAcces + " existe déjà dans la base.<br>";
            return message;
        }

        #region Proprietees

        int _id;
        public int ID_Personne
        {
            get { return _id; }
            set { _id = value; }
        }

        Guid _PersonneGUID;
        public Guid PersonneGUID
        {
            get { return _PersonneGUID; }
            set { _PersonneGUID = value; }
        }

        int _QuestionnaireID;
        public int QuestionnaireID
        {
            get { return _QuestionnaireID; }
            set { _QuestionnaireID = value; }
        }

        int _CodeAcces;
        public int CodeAcces
        {
            get { return _CodeAcces; }
            set { _CodeAcces = value; }
        }

        int _EmailEnvois;
        /// <summary>
        /// Compter le nombre d'emails envoyes a la personne
        /// </summary>
        public int EmailEnvois
        {
            get { return _EmailEnvois; }
            set { _EmailEnvois = value; }
        }

        string _civilite = "";
        public string Civilite
        {
            get { return _civilite; }
            set { _civilite = value; }
        }

        string _nom = "";
        public string Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }
        string _prenom = "";
        public string Prenom
        {
            get { return _prenom; }
            set { _prenom = value; }
        }

        string _emailPerso = "";
        public string EmailPerso
        {
            get { return _emailPerso; }
            set { _emailPerso = value; }
        }

        string _emailBureau = "";
        public string EmailBureau
        {
            get { return _emailBureau; }
            set { _emailBureau = value; }
        }

        string _telephonePerso = "";
        public string TelephonePerso
        {
            get { return _telephonePerso; }
            set { _telephonePerso = value; }
        }

        string _telephoneBureau = "";
        public string TelephoneBureau
        {
            get { return _telephoneBureau; }
            set { _telephoneBureau = value; }
        }

        string _mobile = "";
        public string TelephoneMobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }

        string _fax = "";
        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        string _adresse = "";
        public string Adresse
        {
            get { return _adresse; }
            set { _adresse = value; }
        }

        string _codePostal = "";
        public string CodePostal
        {
            get { return _codePostal; }
            set { _codePostal = value; }
        }

        string _ville = "";
        public string Ville
        {
            get { return _ville; }
            set { _ville = value; }
        }

        string _lienHtml = "";
        public string LienHTML
        {
            get { return _lienHtml; }
            set { _lienHtml = value; }
        }

        string _fonction = "";
        public string Fonction
        {
            get { return _fonction; }
            set { _fonction = value; }
        }

        string _societe = "";
        public string Societe
        {
            get { return _societe; }
            set { _societe = value; }
        }

        string _memo = "";
        public string Memo
        {
            get { return _memo; }
            set { _memo = value; }
        }

        #endregion

        public static Personne Fill( DataRow r )
        {
            Personne p = new Personne();

            p.ID_Personne = Convert.ToInt32( r[ "ID_Personne" ].ToString() );
            p.PersonneGUID = ( Guid )r[ "PersonneGUID" ];
            p.QuestionnaireID = int.Parse( r[ "QuestionnaireID" ].ToString() );
            p.CodeAcces = int.Parse( r[ "PersonneCode" ].ToString() );
            p.EmailEnvois = int.Parse( r[ "PersonneEmailEnvois" ].ToString() );
            p.Civilite = r[ "PersonneCivilite" ].ToString();
            p.Nom = r[ "PersonneNom" ].ToString();
            p.Prenom = r[ "PersonnePrenom" ].ToString();
            p.EmailPerso = r[ "PersonneEmailPerso" ].ToString();
            p.EmailBureau = r[ "PersonneEmailBureau" ].ToString();
            p.TelephonePerso = r[ "PersonneTelephonePerso" ].ToString();
            p.TelephoneBureau = r[ "PersonneTelephoneBureau" ].ToString();
            p.TelephoneMobile = r[ "PersonneTelephoneMobile" ].ToString();
            p.Fax = r[ "PersonneFax" ].ToString();
            p.Adresse = r[ "PersonneAdresse" ].ToString();
            p.CodePostal = r[ "PersonneCodePostal" ].ToString();
            p.Ville = r[ "PersonneVille" ].ToString();
            p.Societe = r[ "PersonneSociete" ].ToString();
            p.LienHTML = r[ "PersonneHtml" ].ToString();
            p.Memo = r[ "PersonneMemo" ].ToString();

            return p;
        }

        public static Personne FillFromXML( DataRow r )
        {
            Personne p = new Personne();

            p.ID_Personne = Convert.ToInt32( r[ "ID_Personne" ].ToString() );

            p.PersonneGUID = new Guid( r[ "PersonneGUID" ].ToString() );

            p.QuestionnaireID = int.Parse( r[ "QuestionnaireID" ].ToString() );
            p.CodeAcces = int.Parse( r[ "PersonneCode" ].ToString() );
            p.EmailEnvois = int.Parse( r[ "PersonneEmailEnvois" ].ToString() );
            try { p.Civilite = r[ "PersonneCivilite" ].ToString(); }
            catch { p.Civilite = string.Empty; }
            try { p.Nom = r[ "PersonneNom" ].ToString(); }
            catch { p.Nom = string.Empty; }
            try { p.Prenom = r[ "PersonnePrenom" ].ToString(); }
            catch { p.Prenom = string.Empty; }
            try { p.EmailPerso = r[ "PersonneEmailPerso" ].ToString(); }
            catch { p.EmailPerso = string.Empty; }
            try { p.EmailBureau = r[ "PersonneEmailBureau" ].ToString(); }
            catch { p.EmailBureau = string.Empty; }
            try { p.TelephonePerso = r[ "PersonneTelephonePerso" ].ToString(); }
            catch { p.TelephonePerso = string.Empty; }
            try { p.TelephoneBureau = r[ "PersonneTelephoneBureau" ].ToString(); }
            catch { p.TelephoneBureau = string.Empty; }
            try { p.TelephoneMobile = r[ "PersonneTelephoneMobile" ].ToString(); }
            catch { p.TelephoneMobile = string.Empty; }
            try { p.Fax = r[ "PersonneFax" ].ToString(); }
            catch { p.Fax = string.Empty; }
            try { p.Adresse = r[ "PersonneAdresse" ].ToString(); }
            catch { p.Adresse = string.Empty; }
            try { p.CodePostal = r[ "PersonneCodePostal" ].ToString(); }
            catch { p.CodePostal = string.Empty; }
            try { p.Ville = r[ "PersonneVille" ].ToString(); }
            catch { p.Ville = string.Empty; }
            try { p.Societe = r[ "PersonneSociete" ].ToString(); }
            catch { p.Societe = string.Empty; }
            try { p.LienHTML = r[ "PersonneHtml" ].ToString(); }
            catch { p.LienHTML = string.Empty; }
            try { p.Memo = r[ "PersonneMemo" ].ToString(); }
            catch { p.Memo = string.Empty; }

            return p;
        }

        #region Comparateurs

        /// <summary>
        /// Comparateur generic pour deriver de IComparer<T>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare( Personne x, Personne y )
        {
            return x.Nom.CompareTo( y.Nom );
        }

        public int CompareNom( Personne x, Personne y )
        {
            return x.Nom.CompareTo( y.Nom );
        }

        #endregion

        #region GetMethodes

        public static Personne Get( int id )
        {
            DataSet ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                "GetPersonne",
                id
            );

            if ( ds.Tables[ 0 ].Rows.Count < 1 )
                return null;

            DataRow r = ds.Tables[ 0 ].Rows[ 0 ];
            Personne p = Fill( r );
            return p;
        }

        #endregion

        #region CreateCreateUpdateDeleteMethodes

        // AME17062010
        // -1 Contact non valide
        // 0 Erreur a la creation
        // 1 Creation effectuee
        // 2 Existe deja (( PersonneEmailBureau != '' AND PersonneEmailBureau = @PersonneEmailBureau ) OR ( PersonneTelephonePerso != '' AND PersonneTelephonePerso = @PersonneTelephonePerso )) AND PersonneCode = @PersonneCode )
        public static int Create( Personne p, bool msgCreate, ref string message )
        {
            if ( ContactValide( p ) == false )
            {
                message += messageErrorContactInvalide( p );
                return -1;
            }

            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PersonneCreate", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            // Parametre de sortie de la procedure
            SqlParameter param = new SqlParameter( "@PersonneID", SqlDbType.Int, 4 );
            param.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add( param );

            AddParameters( p, ref myCommand );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            if ( status == 1 )
                p.ID_Personne = Convert.ToInt32( param.Value );
            myConnection.Close();

            if ( status == 0 )
            {
                message += "Erreur à la creation du contact.<br/>";
            }
            else if ( status == 1 && msgCreate )
            {
                message += "Contact créé correctement.<br/>";
            }
            else if ( status == 2 )
            {
                message += messageErrorContactExisteDeja( p );
            }
            return status;
        }

        // AME17062010
        // -1 contact invalide
        // 0 OK maj effectuee
        // 1 si une personne avec cette adresse email ou ce telephone existe deja ans la base
        // 2 n'existe pas
        public static int Update( Personne p, ref string message )
        {
            if ( ContactValide( p ) == false )
            {
                message += messageErrorContactInvalide( p );
                return -1;
            }

            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PersonneUpdate", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@PersonneID", SqlDbType.Int, 4 );
            param.Value = p.ID_Personne;
            myCommand.Parameters.Add( param );

            AddParameters( p, ref myCommand );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            if ( status == 0 )
            {
                message += "Contact mis à jour correctement.<br/>";
            }
            else if ( status == 1 )
            {
                message += messageErrorContactExisteDeja( p );
            }
            else if ( status == 2 )
            {
                message += "Erreur à la mise à jour du contact.<br/>";
            }
            return status;
        }

        public static int UpdateEmailEnvois( Guid PersonneGUID )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PersonneUpdateEmailEnvois", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@PersonneGUID", SqlDbType.UniqueIdentifier );
            param.Value = PersonneGUID;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return status;
        }

        public static int Delete( int personneID )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PersonneDelete", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@PersonneID", SqlDbType.Int, 4 );
            param.Value = personneID;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return status;
        }

        private static void AddParameters( Personne o, ref SqlCommand cmd )
        {
            SqlParameter param = new SqlParameter( "@PersonneCivilite", SqlDbType.NVarChar, 50 );
            param.Value = o.Civilite;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PersonneGUID", SqlDbType.UniqueIdentifier );
            param.Value = o.PersonneGUID;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@QuestionnaireID", SqlDbType.Int, 4 );
            param.Value = o.QuestionnaireID;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PersonneCode", SqlDbType.Int );
            param.Value = o.CodeAcces;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PersonneEmailEnvois", SqlDbType.Int );
            param.Value = o.EmailEnvois;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PersonneNom", SqlDbType.NVarChar, 50 );
            param.Value = o.Nom;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PersonnePrenom", SqlDbType.NVarChar, 50 );
            param.Value = o.Prenom;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PersonneEmailPerso", SqlDbType.NVarChar, 50 );
            param.Value = o.EmailPerso;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PersonneEmailBureau", SqlDbType.NVarChar, 50 );
            param.Value = o.EmailBureau;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PersonneTelephonePerso", SqlDbType.NVarChar, 50 );
            param.Value = o.TelephonePerso;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PersonneTelephoneBureau", SqlDbType.NVarChar, 50 );
            param.Value = o.TelephoneBureau;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PersonneTelephoneMobile", SqlDbType.NVarChar, 50 );
            param.Value = o.TelephoneMobile;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PersonneAdresse", SqlDbType.NVarChar, 50 );
            param.Value = o.Adresse;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PersonneFax", SqlDbType.NVarChar, 50 );
            param.Value = o.Fax;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PersonneCodePostal", SqlDbType.NVarChar, 50 );
            param.Value = o.CodePostal;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PersonneVille", SqlDbType.NVarChar, 50 );
            param.Value = o.Ville;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PersonneHtml", SqlDbType.NVarChar, 50 );
            param.Value = o.LienHTML;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PersonneSociete", SqlDbType.NVarChar, 50 );
            param.Value = o.Societe;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PersonneMemo", SqlDbType.NText );
            param.Value = o.Memo;
            cmd.Parameters.Add( param );
        }

        #endregion
    }

    [Serializable()]
    public class PersonneCollection : List<Personne>
    {
        private List<Personne> _collection = null;

        public PersonneCollection()
        {
            _collection = new List<Personne>();
        }

        public static PersonneCollection GetAll()
        {
            return PersonneDAL.GetPersonnes();
        }

        /// <summary>
        /// Trouver les Personnes qui sont liees au Questionnaire
        /// </summary>
        /// <param name="questionnaireID"></param>
        /// <returns></returns>
        public static PersonneCollection GetQuestionnaire( int questionnaireID )
        {
            //PersonneCollection pc = PersonneCollection.GetAll();
            //PersonneCollection newpc = new PersonneCollection();
            //foreach ( Personne p in pc )
            //{
            //    if ( p.QuestionnaireID == questionnaireID )
            //    {
            //        newpc.Add( p );
            //    }
            //}

            // OPT00020100219
            PersonneCollection newpc = PersonneDAL.GetPersonneByQuestionnaireID( questionnaireID );
            return newpc;
        }
    }

    public class PersonneDAL
    {
        public static PersonneCollection GetPersonnes()
        {
            DataSet ds = new DataSet();
            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetPersonneAll"
            );

            PersonneCollection personnes = new PersonneCollection();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                Personne p = Personne.Fill( r );
                personnes.Add( p );
            }
            return personnes;
        }

        // OPT00020100219
        // Obtenir tous les Personnes (contacts) d'un Questionnaire
        public static PersonneCollection GetPersonneByQuestionnaireID( int questionnaireID )
        {
            DataSet ds = new DataSet();
            SqlParameter param = new SqlParameter();
            param = new SqlParameter( "@QuestionnaireID", SqlDbType.Int, 4 );
            param.Value = questionnaireID;

            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetPersonneByQuestionnaireID",
                param
            );

            PersonneCollection personnes = new PersonneCollection();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                Personne p = Personne.Fill( r );
                personnes.Add( p );
            }
            return personnes;
        }
    }
}