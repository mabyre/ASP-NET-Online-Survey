//
// Les fonctions d'acces aux donnes 
// Create Update Delete ne sont pas utilisees... Et si Create pour copier un Questionnaire ! 
// Et si Delete aussi SqlDataSource qu'elle grosse daube
// puisqu'on definit directement les commandes SQL dans l'Interface Graphique !
//
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

/// <summary>
/// Description résumée de PollQuestion
/// </summary>
namespace Sql.Web.Data
{
    [Serializable()]
    public class PollQuestion : IComparer<PollQuestion>
    {
        public PollQuestion()
        {
        }

        public static ArrayList TypeAlignement()
        {
            ArrayList al = new ArrayList();
            al.Add( "Gauche" ); // 0
            al.Add( "Droite" ); // 1
            al.Add( "Centre" ); // 2
            return al;
        }

        #region Proprietees

        Guid _PollQuestionId;
        public Guid PollQuestionId
        {
            get { return _PollQuestionId; }
            set { _PollQuestionId = value; }
        }

        int _QuestionnaireID;
        public int QuestionnaireID
        {
            get { return _QuestionnaireID; }
            set { _QuestionnaireID = value; }
        }

        Guid _MembreGUID;
        public Guid MembreGUID
        {
            get { return _MembreGUID; }
            set { _MembreGUID = value; }
        }

        string _Question;
        public string Question
        {
            get { return _Question; }
            set { _Question = value; }
        }

        string _Societe;
        public string Societe
        {
            get { return _Societe; }
            set { _Societe = value; }
        }

        DateTime _CreationDate;
        public DateTime CreationDate
        {
            get { return _CreationDate; }
            set { _CreationDate = value; }
        }

        int _Rank;
        public int Rank
        {
            get { return _Rank; }
            set { _Rank = value; }
        }

        string _Instruction;
        public string Instruction
        {
            get { return _Instruction; }
            set { _Instruction = value; }
        }

        // Alignement de toute la Question Label + Reponse + Message - Gauche Droite Centre
        string _AlignementQuestion;
        public string AlignementQuestion
        {
            get { return _AlignementQuestion; }
            set { _AlignementQuestion = value; }
        }

        // Alignement de la RadioList ou de la CheckBoxList - Gauche Droite Centre
        string _AlignementReponse;
        public string AlignementReponse
        {
            get { return _AlignementReponse; }
            set { _AlignementReponse = value; }
        }

        string _Message;
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        // Permet de placer le Message a l'utilisateur en haut ou en bas
        bool _MessageHaut;
        public bool MessageHaut
        {
            get { return _MessageHaut; }
            set { _MessageHaut = value; }
        }

        string _SautPage;
        public string SautPage
        {
            get { return _SautPage; }
            set { _SautPage = value; }
        }

        string _Tableau;
        public string Tableau
        {
            get { return _Tableau; }
            set { _Tableau = value; }
        }

        bool _ChoixMultiple;
        public bool ChoixMultiple
        {
            get { return _ChoixMultiple; }
            set { _ChoixMultiple = value; }
        }

        bool _Obligatoire;
        public bool Obligatoire
        {
            get { return _Obligatoire; }
            set { _Obligatoire = value; }
        }

        bool _Fin;
        public bool Fin
        {
            get { return _Fin; }
            set { _Fin = value; }
        }

        int _ChoixMultipleMin;
        public int ChoixMultipleMin
        {
            get { return _ChoixMultipleMin; }
            set { _ChoixMultipleMin = value; }
        }

        int _ChoixMultipleMax;
        public int ChoixMultipleMax
        {
            get { return _ChoixMultipleMax; }
            set { _ChoixMultipleMax = value; }
        }

        #endregion

        public static PollQuestion Fill( DataRow r )
        {
            PollQuestion o = new PollQuestion();

            o.PollQuestionId = ( Guid )( r[ "PollQuestionId" ] );
            o.QuestionnaireID = ( int )( r[ "QuestionnaireID" ] );
            o.MembreGUID = ( Guid )( r[ "MembreGUID" ] );
            o.Question = r[ "Question" ].ToString();
            o.Societe = r[ "Societe" ].ToString();
            o.Instruction = r[ "Instruction" ].ToString();
            o.AlignementQuestion = r[ "AlignementQuestion" ].ToString();
            o.AlignementReponse = r[ "AlignementReponse" ].ToString();
            o.Message = r[ "Message" ].ToString();
            o.SautPage = r[ "SautPage" ].ToString();
            o.Tableau = r[ "Tableau" ].ToString();
            if ( r[ "MessageHaut" ] != System.DBNull.Value ) o.MessageHaut = ( bool )r[ "MessageHaut" ];
            if ( r[ "ChoixMultiple" ] != System.DBNull.Value ) o.ChoixMultiple = ( bool )r[ "ChoixMultiple" ];
            if ( r[ "QuestionObligatoire" ] != System.DBNull.Value ) o.Obligatoire = ( bool )r[ "QuestionObligatoire" ];
            if ( r[ "QuestionFin" ] != System.DBNull.Value ) o.Fin = ( bool )r[ "QuestionFin" ];
            o.Rank = ( int )( r[ "Rank" ] );
            o.CreationDate = DateTime.Parse( r[ "CreationDate" ].ToString() );
            o.ChoixMultipleMin = r[ "ChoixMultipleMin" ] == System.DBNull.Value ? 0 : (int)r[ "ChoixMultipleMin" ];
            o.ChoixMultipleMax = r[ "ChoixMultipleMax" ] == System.DBNull.Value ? 0 : (int)r[ "ChoixMultipleMax" ];

            return o;
        }

        public static PollQuestion FillFromXML( DataRow r )
        {
            PollQuestion o = new PollQuestion();

            o.PollQuestionId = new Guid( r[ "PollQuestionId" ].ToString() ); ;
            o.QuestionnaireID = int.Parse( r[ "QuestionnaireID" ].ToString() );
            o.MembreGUID = new Guid( r[ "MembreGUID" ].ToString() );
            o.Question = r[ "Question" ].ToString();
            o.Societe = r[ "Societe" ].ToString();
            try { o.Instruction = r[ "Instruction" ].ToString(); }
            catch { o.Instruction = string.Empty; }
            try { o.AlignementQuestion = r[ "AlignementQuestion" ].ToString(); }
            catch { o.AlignementQuestion = string.Empty; }
            try { o.AlignementReponse = r[ "AlignementReponse" ].ToString(); }
            catch { o.AlignementReponse = string.Empty; }
            try { o.Message = r[ "Message" ].ToString(); }
            catch { o.Message = string.Empty; }
            try { o.SautPage = r[ "SautPage" ].ToString(); }
            catch { o.SautPage = string.Empty; }
            try { o.Tableau = r[ "Tableau" ].ToString(); }
            catch { o.Tableau = string.Empty; }
            try { o.MessageHaut = r[ "MessageHaut" ].ToString() == "true" ? true : false; }
            catch { o.MessageHaut = false; }
            try { o.ChoixMultiple = r[ "ChoixMultiple" ].ToString() == "true" ? true : false; }
            catch { o.ChoixMultiple = false; }
            try { o.Obligatoire = r[ "QuestionObligatoire" ].ToString() == "true" ? true : false; }
            catch { o.Obligatoire = false; }
            try { o.Fin = r[ "QuestionFin" ].ToString() == "true" ? true : false; }
            catch { o.Fin = false; }
            o.Rank = int.Parse( r[ "Rank" ].ToString() );
            o.CreationDate = DateTime.Parse( r[ "CreationDate" ].ToString() );
            try { o.ChoixMultipleMin = int.Parse( r[ "ChoixMultipleMin" ].ToString()); }
            catch { o.ChoixMultipleMin = 0; }
            try { o.ChoixMultipleMax = int.Parse( r[ "ChoixMultipleMax" ].ToString()); }
            catch { o.ChoixMultipleMax = 0; }

            return o;
        }

        #region Comparateurs

        /// <summary>
        /// Comparateur generic pour deriver de IComparer<T>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare( PollQuestion x, PollQuestion y )
        {
            return x.CreationDate.CompareTo( y.CreationDate );
        }

        #endregion

        #region CreateUpdateDeleteMethodes

        public static int Create( PollQuestion o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PollQuestionCreate", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@QuestionnaireID", SqlDbType.Int, 4 );
            param.Value = o.QuestionnaireID;
            myCommand.Parameters.Add( param );

            param = new SqlParameter( "@MembreGUID", SqlDbType.UniqueIdentifier );
            param.Value = o.MembreGUID;
            myCommand.Parameters.Add( param );

            AddParameters( o, ref myCommand );

            // Parametre de sortie de la procedure
            param = new SqlParameter( "@PollQuestionId", SqlDbType.UniqueIdentifier );
            param.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            if ( status == 0 ) // Ok
                o.PollQuestionId = new Guid( param.Value.ToString() );
            myConnection.Close();

            return status;
        }

        /// <summary>
        /// status == 2 => la Question n'existe pas
        /// status == 1 => la Question est mise a jour
        /// </summary>
        public static int Update( PollQuestion o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PollQuestionUpdate", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@PollQuestionId", SqlDbType.UniqueIdentifier );
            param.Value = o.PollQuestionId;
            myCommand.Parameters.Add( param );

            AddParameters( o, ref myCommand );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return status;
        }

        public static int UpdateRank( PollQuestion o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PollQuestionUpdateRank", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter param = new SqlParameter();

            param = new SqlParameter( "@Rank", SqlDbType.Int, 4 );
            param.Value = o.Rank;
            myCommand.Parameters.Add( param );

            param = new SqlParameter( "@PollQuestionId", SqlDbType.UniqueIdentifier );
            param.Value = o.PollQuestionId;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int iStatusCode = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return iStatusCode;
        }

        public static int UpdateAlignementQuestion( PollQuestion o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PollQuestionUpdateAlignementQuestion", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter param = new SqlParameter();

            param = new SqlParameter( "@AlignementQuestion", SqlDbType.NVarChar );
            param.Value = o.AlignementQuestion;
            myCommand.Parameters.Add( param );

            param = new SqlParameter( "@PollQuestionId", SqlDbType.UniqueIdentifier );
            param.Value = o.PollQuestionId;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int iStatusCode = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return iStatusCode;
        }

        public static int UpdateAlignementReponse( PollQuestion o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PollQuestionUpdateAlignementReponse", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter param = new SqlParameter();

            param = new SqlParameter( "@AlignementReponse", SqlDbType.NVarChar );
            param.Value = o.AlignementReponse;
            myCommand.Parameters.Add( param );

            param = new SqlParameter( "@PollQuestionId", SqlDbType.UniqueIdentifier );
            param.Value = o.PollQuestionId;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int iStatusCode = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return iStatusCode;
        }

        public static int UpdateSautPage( PollQuestion o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PollQuestionUpdateSautPage", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter param = new SqlParameter();

            param = new SqlParameter( "@SautPage", SqlDbType.NVarChar );
            if ( string.IsNullOrEmpty( o.SautPage ) )
                param.Value = System.DBNull.Value;
            else
                param.Value = o.SautPage;
            myCommand.Parameters.Add( param );

            param = new SqlParameter( "@PollQuestionId", SqlDbType.UniqueIdentifier );
            param.Value = o.PollQuestionId;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int iStatusCode = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return iStatusCode;
        }

        public static int UpdateTableau( PollQuestion o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PollQuestionUpdateTableau", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter param = new SqlParameter();

            param = new SqlParameter( "@Tableau", SqlDbType.NVarChar );
            if ( string.IsNullOrEmpty( o.Tableau ) )
                param.Value = System.DBNull.Value;
            else
                param.Value = o.Tableau;
            myCommand.Parameters.Add( param );

            param = new SqlParameter( "@PollQuestionId", SqlDbType.UniqueIdentifier );
            param.Value = o.PollQuestionId;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int iStatusCode = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return iStatusCode;
        }

        /// <summary>
        /// satus == 0 => Delete == Ok
        /// </summary>
        public static int Delete( Guid id )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PollQuestionDelete", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@PollQuestionId", SqlDbType.UniqueIdentifier );
            param.Value = id;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return status;
        }

        private static void AddParameters( PollQuestion o, ref SqlCommand cmd )
        {
            SqlParameter param = new SqlParameter();

            param = new SqlParameter( "@Question", SqlDbType.NVarChar, 256 );
            param.Value = o.Question;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Societe", SqlDbType.NVarChar, 50 );
            param.Value = o.Societe;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Rank", SqlDbType.Int );
            param.Value = o.Rank;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@CreationDate", SqlDbType.DateTime );
            param.Value = o.CreationDate;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@QuestionFin", SqlDbType.Bit );
            if ( o.Fin )
                param.Value = true;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@QuestionObligatoire", SqlDbType.Bit );
            if ( o.Obligatoire )
                param.Value = true;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Instruction", SqlDbType.NVarChar, 256 );
            param.Value = o.Instruction;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Message", SqlDbType.NVarChar );
            param.Value = o.Message;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@MessageHaut", SqlDbType.Bit );
            if ( o.MessageHaut )
                param.Value = true;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@SautPage", SqlDbType.NVarChar );
            if ( string.IsNullOrEmpty( o.SautPage ) == false )
                param.Value = o.SautPage;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Tableau", SqlDbType.NVarChar );
            if ( string.IsNullOrEmpty( o.Tableau ) == false )
                param.Value = o.Tableau;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@AlignementQuestion", SqlDbType.NVarChar, 50 );
            if ( o.AlignementQuestion == null )
            {
                param.Value = PollQuestion.TypeAlignement()[ 0 ];
            }
            else
            {
                param.Value = o.AlignementQuestion;
            }
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@AlignementReponse", SqlDbType.NVarChar, 50 );
            if ( o.AlignementReponse == null )
            {
                param.Value = PollQuestion.TypeAlignement()[ 0 ];
            }
            else
            {
                param.Value = o.AlignementReponse;
            }
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@ChoixMultiple", SqlDbType.Bit );
            if ( o.ChoixMultiple )
                param.Value = true;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@ChoixMultipleMin", SqlDbType.Int );
            if ( o.ChoixMultipleMin == 0 )
                param.Value = System.DBNull.Value;
            else
                param.Value = o.ChoixMultipleMin;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@ChoixMultipleMax", SqlDbType.Int );
            if ( o.ChoixMultipleMax == 0 )
                param.Value = System.DBNull.Value;
            else
                param.Value = o.ChoixMultipleMax;
            cmd.Parameters.Add( param );
        }

        #endregion

        #region GetMethodes

        public static PollQuestion GetQuestion( Guid PollQuestionId )
        {
            if ( PollQuestionId == Guid.Empty )
            {
                return null;
            }

            PollQuestionCollection pqc = PollQuestionCollection.GetAll();
            foreach ( PollQuestion pc in pqc )
            {
                if ( pc.PollQuestionId == PollQuestionId )
                {
                    return pc;
                }
            }
            return null;
        }

        public static PollQuestion GetLatestQuestion()
        {
            PollQuestion pq = new PollQuestion();
            PollQuestionCollection pqc = PollQuestionCollection.GetAll();
            pqc.Sort( pq.Compare );

            return pqc[ 0 ];
        }

        #endregion
    }

    [Serializable()]
    public class PollQuestionCollection : List<PollQuestion>
    {
        private List<PollQuestion> _collection = null;

        public PollQuestionCollection()
        {
            _collection = new List<PollQuestion>();
        }

        public static PollQuestionCollection GetAll()
        {
            return PollQuestionDAL.GetAll();
        }

        public PollQuestion FindByPollQuestionID( Guid pollQuestionID )
        {
            foreach ( PollQuestion o in this )
            {
                if ( o.PollQuestionId == pollQuestionID )
                {
                    return o;
                }
            }

            return null;
        }

        // Trouver la Question de Rang max
        public int MaxRank()
        {
            int rank = 0;
            foreach ( PollQuestion q in this )
            {
                if ( q.Rank > rank )
                    rank = q.Rank;
            }
            return rank;
        }


        /// <summary>
        /// Trouver les Questions d'un Questionnaire
        /// </summary>
        /// <param name="questionnaireID">questionnaireID</param>
        /// <returns></returns>
        public static PollQuestionCollection GetByQuestionnaire( int questionnaireID )
        {
            // OPT00020100219
            //PollQuestionCollection newpqc = new PollQuestionCollection();
            //PollQuestionCollection pqc = PollQuestionCollection.GetAll();
            //foreach ( PollQuestion pc in pqc )
            //{
            //    if ( pc.QuestionnaireID == questionnaireID )
            //    {
            //        newpqc.Add( pc );
            //    }
            //}

            PollQuestionCollection newpqc = PollQuestionDAL.GetByQuestionnaireID( questionnaireID );
            return newpqc;
        }

        public static PollQuestionCollection GetBySociete( string societe )
        {
            PollQuestionCollection newpqc = new PollQuestionCollection();
            PollQuestionCollection pqc = PollQuestionCollection.GetAll();
            foreach ( PollQuestion pc in pqc )
            {
                if ( pc.Societe == societe )
                {
                    newpqc.Add( pc );
                }
            }
            return newpqc;
        }
    }

    public class PollQuestionDAL
    {
        public static PollQuestionCollection GetAll()
        {
            DataSet ds = new DataSet();
            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetPollQuestionAll"
            );

            PollQuestionCollection oc = new PollQuestionCollection();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                PollQuestion o = PollQuestion.Fill( r );
                oc.Add( o );
            }
            return oc;
        }

        // OPT00020100219
        // Obtenir toutes les Questions d'un Questionnaire
        public static PollQuestionCollection GetByQuestionnaireID( int questionnaireID )
        {
            DataSet ds = new DataSet();
            SqlParameter param = new SqlParameter();
            param = new SqlParameter( "@QuestionnaireID", SqlDbType.Int, 4 );
            param.Value = questionnaireID;

            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetPollQuestionByQuestionnaireID",
                param
            );

            PollQuestionCollection oc = new PollQuestionCollection();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                PollQuestion o = PollQuestion.Fill( r );
                oc.Add( o );
            }
            return oc;
        }

    }
}