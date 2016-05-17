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
/// Description résumée de PollAnswer
/// </summary>
namespace Sql.Web.Data
{
    [Serializable()]
    public class PollAnswer
    {
        public PollAnswer()
        {
        }

        public PollAnswer( string answer )
        {
            this.Answer = answer;
        }

        #region Proprietees

        Guid _PollAnswerId;
        public Guid PollAnswerId
        {
            get { return _PollAnswerId; }
            set { _PollAnswerId = value; }
        }

        Guid _PollQuestionId;
        public Guid PollQuestionId
        {
            get { return _PollQuestionId; }
            set { _PollQuestionId = value; }
        }

        string _Answer;
        public string Answer
        {
            get { return _Answer; }
            set { _Answer = value; }
        }

        // Remplace Textuelle et Numerique cf. BusinessObjectQuestionnaire
        string _TypeReponse;
        public string TypeReponse
        {
            get { return _TypeReponse; }
            set { _TypeReponse = value; }
        }

        //bool _Textuelle;
        //public bool Textuelle
        //{
        //    get { return _Textuelle; }
        //    set { _Textuelle = value; }
        //}

        int _Width;
        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        int _Rows;
        public int Rows
        {
            get { return _Rows; }
            set { _Rows = value; }
        }

        bool _AlignLeft;
        public bool AlignLeft
        {
            get { return _AlignLeft; }
            set { _AlignLeft = value; }
        }

        bool _Horizontal;
        public bool Horizontal
        {
            get { return _Horizontal; }
            set { _Horizontal = value; }
        }

        // Ne concerne que les reponses Ouvertes
        bool _Obligatoire;
        public bool Obligatoire
        {
            get { return _Obligatoire; }
            set { _Obligatoire = value; }
        }

        //// Ne concerne que les reponses textuelles
        //bool _Numerique;
        //public bool Numerique
        //{
        //    get { return _Numerique; }
        //    set { _Numerique = value; }
        //}

        //DateTime _CreationDate;
        //public DateTime CreationDate
        //{
        //    get { return _CreationDate; }
        //    set { _CreationDate = value; }
        //}

        int _Rank;
        public int Rank
        {
            get { return _Rank; }
            set { _Rank = value; }
        }

        int _Score;
        public int Score
        {
            get { return _Score; }
            set { _Score = value; }
        }

        #endregion

        public static PollAnswer Fill( DataRow r )
        {
            PollAnswer o = new PollAnswer();

            o.PollAnswerId = ( Guid )( r[ "PollAnswerId" ] );
            o.PollQuestionId = ( Guid )( r[ "PollQuestionId" ] );
            o.Answer = r[ "Answer" ].ToString();
            o.TypeReponse = r[ "TypeReponse" ].ToString();
            //if ( r[ "Textuelle" ] != System.DBNull.Value ) o.Textuelle = ( bool )r[ "Textuelle" ];
            o.Width = r[ "Width" ] == System.DBNull.Value ? 0 : int.Parse( r[ "Width" ].ToString() );
            o.Rows = r[ "Rows" ] == System.DBNull.Value ? 0 : int.Parse( r[ "Rows" ].ToString() );
            if ( r[ "AlignLeft" ] != System.DBNull.Value ) o.AlignLeft = ( bool )r[ "AlignLeft" ];
            if ( r[ "Horizontal" ] != System.DBNull.Value ) o.Horizontal = ( bool )r[ "Horizontal" ];
            if ( r[ "Obligatoire" ] != System.DBNull.Value ) o.Obligatoire = ( bool )r[ "Obligatoire" ];
            //if ( r[ "Numerique" ] != System.DBNull.Value ) o.Numerique = ( bool )r[ "Numerique" ];
            //o.CreationDate = ( DateTime )( r[ "CreationDate" ] );
            o.Rank = int.Parse( r[ "Rank" ].ToString() );
            o.Score = r[ "Score" ] == System.DBNull.Value ? 0 : int.Parse( r[ "Score" ].ToString() );

            return o;
        }

        public static PollAnswer FillFromXML( DataRow r )
        {
            PollAnswer o = new PollAnswer();

            o.PollAnswerId = new Guid( r[ "PollAnswerId" ].ToString() );
            o.PollQuestionId = new Guid( r[ "PollQuestionId" ].ToString() );
            o.Answer = r[ "Answer" ].ToString();
            o.TypeReponse = r[ "TypeReponse" ].ToString();
            try { o.Width = int.Parse( r[ "Width" ].ToString() ); }
            catch { /*o.Width = string.Empty;*/ }
            try { o.Rows = int.Parse( r[ "Rows" ].ToString() ); }
            catch { /*o.Rows = string.Empty;*/ }

            try { o.AlignLeft = r[ "AlignLeft" ].ToString() == "true" ? true : false; }
            catch { o.AlignLeft = false; }

            try { o.Horizontal = r[ "Horizontal" ].ToString() == "true" ? true : false; }
            catch { o.Horizontal = false; }

            try { o.Obligatoire = r[ "Obligatoire" ].ToString() == "true" ? true : false; }
            catch { o.Obligatoire = false; }

            o.Rank = int.Parse( r[ "Rank" ].ToString() );

            try { o.Score = int.Parse( r[ "Score" ].ToString() ); }
            catch { o.Score = 0; }

            return o;
        }
        #region CreateUpdateDeleteMethodes

        public static int Create( PollAnswer o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PollAnswerCreate", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            // Parametre de sortie de la procedure
            SqlParameter param = new SqlParameter( "@PollAnswerId", SqlDbType.UniqueIdentifier );
            param.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add( param );

            AddParameters( o, ref myCommand );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            if ( status == 0 )
                // Ya probablement un gros ici
                //o.PollQuestionId = new Guid( param.Value.ToString() );
                o.PollAnswerId = new Guid( param.Value.ToString() );
            myConnection.Close();

            return status;
        }

        public static int Update( PollAnswer o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PollAnswerUpdate", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@PollAnswerId", SqlDbType.UniqueIdentifier );
            param.Value = o.PollAnswerId;
            myCommand.Parameters.Add( param );

            AddParameters( o, ref myCommand );

            myConnection.Open();
            int iStatusCode = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return iStatusCode;
        }

        public static int UpdateAlignLeft( PollAnswer o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PollAnswerUpdateAlignLeft", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@PollAnswerId", SqlDbType.UniqueIdentifier );
            param.Value = o.PollAnswerId;
            myCommand.Parameters.Add( param );

            param = new SqlParameter( "@AlignLeft", SqlDbType.Bit );
            if ( o.AlignLeft )
                param.Value = true;
            else
                param.Value = System.DBNull.Value;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int iStatusCode = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return iStatusCode;
        }

        public static int UpdateVerticalHorizontal( PollAnswer o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PollAnswerUpdateVerticalHorizontal", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@PollAnswerId", SqlDbType.UniqueIdentifier );
            param.Value = o.PollAnswerId;
            myCommand.Parameters.Add( param );

            param = new SqlParameter( "@Horizontal", SqlDbType.Bit );
            if ( o.Horizontal )
                param.Value = true;
            else
                param.Value = System.DBNull.Value;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int iStatusCode = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return iStatusCode;
        }

        public static int Delete( Guid id )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PollAnswerDelete", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@PollAnswerId", SqlDbType.UniqueIdentifier );
            param.Value = id;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return status;
        }

        private static void AddParameters( PollAnswer o, ref SqlCommand cmd )
        {
            SqlParameter param = new SqlParameter();

            param = new SqlParameter( "@PollQuestionId", SqlDbType.UniqueIdentifier );
            param.Value = o.PollQuestionId;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Answer", SqlDbType.NVarChar, 256 );
            param.Value = o.Answer;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@TypeReponse", SqlDbType.NVarChar, 50 );
            param.Value = o.TypeReponse;
            cmd.Parameters.Add( param );

            //param = new SqlParameter( "@Textuelle", SqlDbType.Bit );
            //if ( o.Textuelle )
            //    param.Value = true;
            //else
            //    param.Value = System.DBNull.Value;
            //cmd.Parameters.Add( param );

            param = new SqlParameter( "@Width", SqlDbType.Int, 4 );
            if ( o.Width != 0 )
                param.Value = o.Width;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Rows", SqlDbType.Int, 4 );
            if ( o.Rows != 0 )
                param.Value = o.Rows;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@AlignLeft", SqlDbType.Bit );
            if ( o.AlignLeft )
                param.Value = true;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Horizontal", SqlDbType.Bit );
            if ( o.Horizontal )
                param.Value = true;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Obligatoire", SqlDbType.Bit );
            if ( o.Obligatoire )
                param.Value = true;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

            //param = new SqlParameter( "@Numerique", SqlDbType.Bit );
            //if ( o.Numerique )
            //    param.Value = true;
            //else
            //    param.Value = System.DBNull.Value;
            //cmd.Parameters.Add( param );

            param = new SqlParameter( "@Rank", SqlDbType.Int, 4 );
            param.Value = o.Rank;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Score", SqlDbType.Int );
            if ( o.Score != 0 )
                param.Value = o.Score;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );
        }

        #endregion
    }

    [Serializable()]
    public class PollAnswerCollection : List<PollAnswer>
    {
        private List<PollAnswer> _collection = null;

        public PollAnswerCollection()
        {
            _collection = new List<PollAnswer>();
        }

        public static PollAnswerCollection GetAll()
        {
            return PollAnswerDAL.GetAll();
        }

        public static PollAnswerCollection GetByPollQuestionID( Guid pollQuestionId )
        {
            if ( pollQuestionId == Guid.Empty )
            {
                return null;
            }
            //PollAnswerCollection pac = PollAnswerCollection.GetAll();
            //PollAnswerCollection newpac = new PollAnswerCollection();
            //foreach ( PollAnswer pa in pac )
            //{
            //    if ( pa.PollQuestionId == pollQuestionId )
            //    {
            //        newpac.Add( pa );
            //    }
            //}

            // OPT00020100219
            PollAnswerCollection newpac = PollAnswerDAL.GetByPollQuestionID( pollQuestionId );

            return newpac;
        }

        // Optimisation
        public PollAnswerCollection FindByPollQuestionID( Guid pollQuestionId )
        {
            if ( pollQuestionId == Guid.Empty )
            {
                return null;
            }
            PollAnswerCollection newpac = new PollAnswerCollection();
            foreach ( PollAnswer pa in this )
            {
                if ( pa.PollQuestionId == pollQuestionId )
                {
                    newpac.Add( pa );
                }
            }

            return newpac;
        }

        public static PollAnswer GetByPollAnswerID( Guid pollAnswerID )
        {
            if ( pollAnswerID == Guid.Empty )
            {
                return null;
            }
            PollAnswerCollection pac = PollAnswerCollection.GetAll();
            foreach ( PollAnswer pa in pac )
            {
                if ( pa.PollAnswerId == pollAnswerID )
                {
                    return pa;
                }
            }
            return null;
        }

        public PollAnswer FindByPollAnswerID( Guid pollAnswerID )
        {
            if ( pollAnswerID == Guid.Empty )
            {
                return null;
            }
            foreach ( PollAnswer pa in this )
            {
                if ( pa.PollAnswerId == pollAnswerID )
                {
                    return pa;
                }
            }
            return null;
        }

        // Trouver la Reponse de Rang max
        public int MaxRank()
        {
            int rank = 0;
            foreach ( PollAnswer r in this )
            {
                if ( r.Rank > rank )
                    rank = r.Rank;
            }
            return rank;
        }
    }

    public class PollAnswerDAL
    {
        /// <summary>
        /// SELECT * FROM PollAnswers ORDER BY Rank ASC
        /// </summary>
        public static PollAnswerCollection GetAll()
        {
            DataSet ds = new DataSet();
            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetPollAnswerAll"
            );

            PollAnswerCollection oc = new PollAnswerCollection();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                PollAnswer o = PollAnswer.Fill( r );
                oc.Add( o );
            }
            return oc;
        }

        // OPT00020100219
        // Obtenir toutes les Reponse d'une Question
        public static PollAnswerCollection GetByPollQuestionID( Guid questionID )
        {
            DataSet ds = new DataSet();
            SqlParameter param = new SqlParameter();

            param = new SqlParameter( "@QuestionID", SqlDbType.UniqueIdentifier );
            param.Value = questionID;

            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetPollAnswerByPollQuestionID",
                param
            );

            PollAnswerCollection oc = new PollAnswerCollection();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                PollAnswer o = PollAnswer.Fill( r );
                oc.Add( o );
            }
            return oc;
        }
    }
}
