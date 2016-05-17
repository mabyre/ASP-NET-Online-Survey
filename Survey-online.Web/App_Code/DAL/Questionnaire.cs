
// On va dire que comme convention les Get ... utilise de DAL pour chercher les donnees
// les FindBy ... cherche dans la collection elle meme sans utiliser le DAL

using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Sql.Data;
using TraceReporter;

/// <summary>
/// Description résumée de Questionnaire
/// </summary>
namespace Sql.Web.Data
{
    [Serializable()]
    public class Questionnaire : IComparer<Questionnaire>
    {

        public Questionnaire()
        {
        }

        #region Proprietees

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

        string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        // StyleWeb du Questionnaire au format XML dans le repertoire :
        // App_Data/StyleWeb/nom du membre/nom du style.xml
        string _Style;
        public string Style
        {
            get { return _Style; }
            set { _Style = value; }
        }

        DateTime _DateCreation;
        public DateTime DateCreation
        {
            get { return _DateCreation; }
            set { _DateCreation = value; }
        }

        bool _Valider;
        public bool Valider
        {
            get { return _Valider; }
            set { _Valider = value; }
        }

        bool _Fin;
        public bool Fin
        {
            get { return _Fin; }
            set { _Fin = value; }
        }

        // Delivrer le message en fonction de Valider et de Fin
        public string MessageDeValidation( bool valider, bool fin )
        {
            string message = "Consultez vos réponses";
            if ( valider == true && fin == false )
            {
                message = "Consultez, modifiez, validez vos réponses";
            }
            if ( valider == true && fin == true )
            {
                message = "Consultez, modifiez vos réponses, vous les validerez à la fin du questionnaire";
            }
            return message;
        }

        bool _Anonyme;
        public bool Anonyme
        {
            get { return _Anonyme; }
            set { _Anonyme = value; }
        }

        bool _Bloque;
        public bool Bloque
        {
            get { return _Bloque; }
            set { _Bloque = value; }
        }

        bool _Publier;
        public bool Publier
        {
            get { return _Publier; }
            set { _Publier = value; }
        }

        Guid _MembreGUID;
        public Guid MembreGUID
        {
            get { return _MembreGUID; }
            set { _MembreGUID = value; }
        }

        // Garantir l'anonymat des interviewes
        bool _Anonymat;
        public bool Anonymat
        {
            get { return _Anonymat; }
            set { _Anonymat = value; }
        }

        // Afficher ou non le compteur pour chaque question
        bool _Compteur;
        public bool Compteur
        {
            get { return _Compteur; }
            set { _Compteur = value; }
        }

        #endregion

        public static Questionnaire Fill( DataRow r )
        {
            Questionnaire o = new Questionnaire();

            o.QuestionnaireID = ( int )r[ "QuestionnaireID" ];
            o.CodeAcces = ( int )r[ "CodeAcces" ];
            o.Description = r[ "Description" ].ToString();
            o.Style = r[ "Style" ].ToString();
            if ( r[ "Valider" ] != System.DBNull.Value ) o.Valider = ( bool )r[ "Valider" ];
            if ( r[ "Fin" ] != System.DBNull.Value ) o.Fin = ( bool )r[ "Fin" ];
            if ( r[ "Anonyme" ] != System.DBNull.Value ) o.Anonyme = ( bool )r[ "Anonyme" ];
            if ( r[ "Anonymat" ] != System.DBNull.Value ) o.Anonymat = ( bool )r[ "Anonymat" ];
            if ( r[ "Bloque" ] != System.DBNull.Value ) o.Bloque = ( bool )r[ "Bloque" ];
            if ( r[ "Publier" ] != System.DBNull.Value ) o.Publier = ( bool )r[ "Publier" ];
            if ( r[ "Compteur" ] != System.DBNull.Value ) o.Compteur = ( bool )r[ "Compteur" ];
            o.DateCreation = DateTime.Parse( r[ "DateCreation" ].ToString() );
            o.MembreGUID = ( Guid )( r[ "MembreGUID" ] );

            return o;
        }

        // Remplir l'objet Questionnaire depuis le fichier d'export XML
        public static Questionnaire FillFromXML( DataRow r )
        {
            Questionnaire o = new Questionnaire();

            o.QuestionnaireID = int.Parse( r[ "QuestionnaireID" ].ToString() );
            o.CodeAcces = int.Parse( r[ "CodeAcces" ].ToString() );
            o.Description = r[ "Description" ].ToString();
            o.Style = r[ "Style" ].ToString();
            try { o.Valider = r[ "Valider" ].ToString() == "true" ? true : false; }
            catch { o.Valider = false; }
            try { o.Fin = r[ "Fin" ].ToString() == "true" ? true : false; }
            catch { o.Fin = false; }
            try { o.Anonyme = r[ "Anonyme" ].ToString() == "true" ? true : false; }
            catch { o.Anonyme = false; }
            try { o.Anonymat = r[ "Anonymat" ].ToString() == "true" ? true : false; }
            catch { o.Anonymat = false; }
            try { o.Bloque = r[ "Bloque" ].ToString() == "true" ? true : false; }
            catch { o.Bloque = false; }
            try { o.Publier = r[ "Publier" ].ToString() == "true" ? true : false; }
            catch { o.Publier = false; }
            try { o.Compteur = r[ "Compteur" ].ToString() == "true" ? true : false; }
            catch { o.Compteur = false; }
            o.DateCreation = DateTime.Parse( r[ "DateCreation" ].ToString() );
            //o.MembreGUID = ( Guid )( r[ "MembreGUID" ] ); Remplis pas l'appelant lors d'un Import

            return o;
        }

        #region Comparateurs

        /// <summary>
        /// Comparateur generic pour deriver de IComparer<T>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare( Questionnaire x, Questionnaire y )
        {
            return x.DateCreation.CompareTo( y.DateCreation );
        }

        #endregion

        #region CreateUpdateDeleteMethodes

        public static int Create( Questionnaire o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "QuestionnaireCreate", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            // Parametre de sortie de la procedure
            SqlParameter param = new SqlParameter( "@QuestionnaireID", SqlDbType.Int );
            param.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add( param );

            AddParameters( o, ref myCommand );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            if ( status == 1 )
                o.QuestionnaireID = Convert.ToInt32( param.Value );
            myConnection.Close();

            return status;
        }

        public static int Update( Questionnaire o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "QuestionnaireUpdate", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@QuestionnaireID", SqlDbType.Int, 4 );
            param.Value = o.QuestionnaireID;
            myCommand.Parameters.Add( param );

            AddParameters( o, ref myCommand );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return status;
        }

        public static int Delete( int id )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "QuestionnaireDelete", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@QuestionnaireID", SqlDbType.Int );
            param.Value = id;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return status;
        }

        private static void AddParameters( Questionnaire o, ref SqlCommand cmd )
        {
            SqlParameter param = new SqlParameter();

            param = new SqlParameter( "@MembreGUID", SqlDbType.UniqueIdentifier );
            param.Value = o.MembreGUID;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@CodeAcces", SqlDbType.Int );
            param.Value = o.CodeAcces;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Description", SqlDbType.NVarChar, 100 );
            param.Value = o.Description;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Style", SqlDbType.NVarChar, 50 );
            param.Value = o.Style;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Valider", SqlDbType.Bit );
            if ( o.Valider )
                param.Value = true;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Fin", SqlDbType.Bit );
            if ( o.Fin )
                param.Value = true;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Anonyme", SqlDbType.Bit );
            if ( o.Anonyme )
                param.Value = true;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Anonymat", SqlDbType.Bit );
            if ( o.Anonymat )
                param.Value = true;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Bloque", SqlDbType.Bit );
            if ( o.Bloque )
                param.Value = true;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Publier", SqlDbType.Bit );
            if ( o.Publier )
                param.Value = true;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Compteur", SqlDbType.Bit );
            if ( o.Compteur )
                param.Value = true;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@DateCreation", SqlDbType.DateTime );
            param.Value = o.DateCreation;
            cmd.Parameters.Add( param );
        }

        #endregion

        #region GetMethodes

        /// <summary>
        /// Trouver le Questionnaire par son ID
        /// </summary>
        /// <param name="questionnaireID">L'ID du Questionnaire</param>
        /// <returns></returns>
        public static Questionnaire GetQuestionnaire( int questionnaireID )
        {
            QuestionnaireCollection qc = QuestionnaireDAL.GetQuestionnaireID( questionnaireID );
            if ( qc.Count > 0 )
                return ( QuestionnaireDAL.GetQuestionnaireID( questionnaireID ) )[ 0 ];
            return null;
        }

        public static Questionnaire GetByCodeAcces( int code )
        {
            QuestionnaireCollection qc = QuestionnaireDAL.GetQuestionnaireAll();
            foreach ( Questionnaire q in qc )
            {
                if ( q.CodeAcces == code )
                {
                    return q;
                }
            }
            return null;
        }
        #endregion
    }

    [Serializable()]
    public class QuestionnaireCollection : List<Questionnaire>
    {
        private static List<Questionnaire> _collection = null;

        public QuestionnaireCollection()
        {
            _collection = new List<Questionnaire>();
        }

        public static QuestionnaireCollection GetAll()
        {
            return QuestionnaireDAL.GetQuestionnaireAll();
        }

        public static QuestionnaireCollection GetQuestionnaireMembre( Guid membreGUID )
        {
            return QuestionnaireDAL.GetQuestionnaireMembre( membreGUID );
        }

        public Questionnaire FindByCodeAcces( int codeAcces )
        {
            foreach ( Questionnaire o in this )
            {
                if ( o.CodeAcces == codeAcces )
                {
                    return o;
                }
            }

            return null;
        }

        public Questionnaire FindByID( int questionnaireID )
        {
            foreach ( Questionnaire o in this )
            {
                if ( o.QuestionnaireID == questionnaireID )
                {
                    return o;
                }
            }

            return null;
        }
    }

    public class QuestionnaireDAL
    {
        public static QuestionnaireCollection GetQuestionnaireAll()
        {
            DataSet ds = new DataSet();
            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetQuestionnaireAll"
            );

            QuestionnaireCollection oc = new QuestionnaireCollection();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                Questionnaire o = Questionnaire.Fill( r );
                oc.Add( o );
            }
            return oc;
        }

        public static QuestionnaireCollection GetQuestionnaireID( int questionnaireID )
        {
            DataSet ds = new DataSet();
            SqlParameter param = new SqlParameter();

            param = new SqlParameter( "@QuestionnaireID", SqlDbType.Int, 4 );
            param.Value = questionnaireID;

            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetQuestionnaireID",
                param
            );

            QuestionnaireCollection oc = new QuestionnaireCollection();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                Questionnaire o = Questionnaire.Fill( r );
                oc.Add( o );
            }
            return oc;
        }

        // Obtenir tous les Questionnaires d'un membre
        public static QuestionnaireCollection GetQuestionnaireMembre( Guid membreID )
        {
            DataSet ds = new DataSet();
            SqlParameter param = new SqlParameter();

            param = new SqlParameter( "@MembreGUID", SqlDbType.UniqueIdentifier );
            param.Value = membreID;

            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetQuestionnaireMembre",
                param
            );

            QuestionnaireCollection oc = new QuestionnaireCollection();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                Questionnaire o = Questionnaire.Fill( r );
                oc.Add( o );
            }
            return oc;
        }

        public static ArrayList GetCodeAccessAll()
        {
            DataSet ds = new DataSet();
            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetQuestionnaireAll"
            );

            ArrayList al = new ArrayList();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                int code = ( int )r[ "CodeAcces" ];
                al.Add( code );
            }
            return al;
        }
    }
}
