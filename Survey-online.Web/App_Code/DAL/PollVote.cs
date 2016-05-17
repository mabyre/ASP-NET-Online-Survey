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
/// Description résumée de PollVote
/// </summary>
namespace Sql.Web.Data
{
    [Serializable()]
    public class PollVote
    {
        public PollVote()
        {
        }

        #region Proprietees

        Guid _VoteId;
        public Guid VoteId
        {
            get { return _VoteId; }
            set { _VoteId = value; }
        }

        Guid _UserGUID;
        public Guid UserGUID
        {
            get { return _UserGUID; }
            set { _UserGUID = value; }
        }

        Guid _PollAnswerId;
        public Guid PollAnswerId
        {
            get { return _PollAnswerId; }
            set { _PollAnswerId = value; }
        }

        int _QuestionnaireID;
        public int QuestionnaireID
        {
            get { return _QuestionnaireID; }
            set { _QuestionnaireID = value; }
        }

        DateTime _CreationDate;
        public DateTime CreationDate
        {
            get { return _CreationDate; }
            set { _CreationDate = value; }
        }

        Guid _PollQuestionID;
        public Guid PollQuestionID
        {
            get { return _PollQuestionID; }
            set { _PollQuestionID = value; }
        }

        string _Vote;
        public string Vote
        {
            get { return _Vote; }
            set { _Vote = value; }
        }

        #endregion

        public static PollVote Fill( DataRow r )
        {
            PollVote o = new PollVote();

            o.VoteId = ( Guid )( r[ "VoteId" ] );
            o.UserGUID = ( Guid )( r[ "UserGUID" ] );
            o.PollAnswerId = ( Guid )( r[ "PollAnswerId" ] );
            o.CreationDate = DateTime.Parse( r[ "CreationDate" ].ToString() );
            o.PollQuestionID = ( Guid )( r[ "PollQuestionID" ] );
            o.QuestionnaireID = r[ "QuestionnaireID" ] == System.DBNull.Value ? 0 : ( int )r[ "QuestionnaireID" ]; // Garder la compatibilite avec les votes creer avant et donc QuestionnaireID == NULL
            o.Vote = r[ "Vote" ].ToString();

            return o;
        }

        public static PollVote FillFromXML( DataRow r )
        {
            PollVote o = new PollVote();

            o.VoteId = new Guid( r[ "VoteId" ].ToString() );
            o.UserGUID = new Guid( r[ "UserGUID" ].ToString() );
            o.PollAnswerId = new Guid( r[ "PollAnswerId" ].ToString() );
            o.CreationDate = DateTime.Parse( r[ "CreationDate" ].ToString() );
            o.PollQuestionID = new Guid( r[ "PollQuestionID" ].ToString() );

            try { o.QuestionnaireID = int.Parse( r[ "QuestionnaireID" ].ToString() ); }
            catch { o.QuestionnaireID = 0; }

            o.Vote = r[ "Vote" ].ToString();

            return o;
        }

        #region CreateUpdateDeleteMethodes

        public static int Create( PollVote o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PollVoteCreate", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            AddParameters( o, ref myCommand );

            // Parametre de sortie de la procedure le Guid du vote cree
            SqlParameter param = new SqlParameter( "@VoteId", SqlDbType.UniqueIdentifier );
            param.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            if ( status == 1 )
                o.VoteId = ( Guid )( param.Value );
            myConnection.Close();

            return status; // 1 : insert reussi
        }

        public static int Update( PollVote o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PollVoteUpdate", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            AddParameters( o, ref myCommand );

            myConnection.Open();
            int iStatusCode = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return iStatusCode; // 2 : l'objet existe, 1 : maj Ok, 0 : Sql Server Error
        }

        public static int Delete( Guid id )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "PollVoteDelete", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@VoteId", SqlDbType.UniqueIdentifier );
            param.Value = id;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return status;
        }

        private static void AddParameters( PollVote o, ref SqlCommand cmd )
        {
            SqlParameter param = new SqlParameter();

            param = new SqlParameter( "@UserGUID", SqlDbType.UniqueIdentifier );
            param.Value = o.UserGUID;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PollAnswerId", SqlDbType.UniqueIdentifier );
            param.Value = o.PollAnswerId;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@CreationDate", SqlDbType.DateTime );
            param.Value = o.CreationDate;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@PollQuestionId", SqlDbType.UniqueIdentifier );
            param.Value = o.PollQuestionID;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@QuestionnaireID", SqlDbType.Int );
            param.Value = o.QuestionnaireID;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Vote", SqlDbType.Text );
            param.Value = o.Vote == null ? System.DBNull.Value.ToString() : o.Vote;
            cmd.Parameters.Add( param );
        }

        #endregion

    }

    [Serializable()]
    public class PollVoteCollection : List<PollVote>
    {
        private List<PollVote> _collection = null;

        public PollVoteCollection()
        {
            _collection = new List<PollVote>();
        }

        public static PollVoteCollection GetAll()
        {
            return PollVoteDAL.GetAll();
        }

        // Trouver les Votes pour une Reponse donnee
        public static PollVoteCollection GetVotes( Guid answerID )
        {
            //PollVoteCollection pvc = PollVoteCollection.GetAll();
            //PollVoteCollection newpvc = new PollVoteCollection();
            //foreach ( PollVote pv in pvc )
            //{
            //    if ( pv.PollAnswerId == answerID )
            //    {
            //        newpvc.Add( pv );
            //    }
            //}

            // OPT00020100219
            PollVoteCollection newpvc = PollVoteDAL.GetPollVoteByPollAnswerID( answerID );
            return newpvc;
        }

        // Trouver les Votes pour une Reponse donnee dans la Collection
        public PollVoteCollection FindByAnswerID( Guid answerID )
        {
            PollVoteCollection newpvc = new PollVoteCollection();
            foreach ( PollVote pv in this )
            {
                if ( pv.PollAnswerId == answerID )
                {
                    newpvc.Add( pv );
                }
            }
            return newpvc;
        }

        // Dire si la personne a deja vote
        public bool ADejaVote( Guid personneGUID )
        {
            foreach ( PollVote pv in this )
            {
                if ( pv.UserGUID == personneGUID )
                {
                    return true;
                }
            }
            return false;
        }

        // Dire si la personne a deja vote et a quelle heure
        public bool ADejaVote( Guid personneGUID, ref DateTime dateVote )
        {
            foreach ( PollVote pv in this )
            {
                if ( pv.UserGUID == personneGUID )
                {
                    dateVote = pv.CreationDate;
                    return true;
                }
            }
            return false;
        }

        // Trouver les Votes faits par une Personne pour un Questionnaire donne
        public static PollVoteCollection GetPollVotes( int questionnaireID, Guid personneGUID )
        {
            return PollVoteDAL.GetPollVotes( questionnaireID, personneGUID );
        }

        // Lorsque l'on boucle sur toutes les Personne d'un Questionnaire > 500
        // l'utilisation de GetPollVotes lance un procedure stockee 500 fois
        // ce n'est pas optimum on va preferer lancer une fois GetPollVotesByQuestionnaireID
        // et utiliser cette fonction
        public PollVoteCollection FindByPersonneGUID( Guid userGUID )
        {
            PollVoteCollection newpvc = new PollVoteCollection();
            foreach ( PollVote pv in this )
            {
                if ( pv.UserGUID == userGUID )
                {
                    newpvc.Add( pv );
                }
            }
            return newpvc;
        }

        // Trouver tous les Votes faits pour un Questionnaire
        public static PollVoteCollection GetPollVotesByQuestionnaireID( int questionnaireID )
        {
            return PollVoteDAL.GetPollVotesByQuestionnaireID( questionnaireID );
        }

        // Pareil 70 ms environ
        public static int CountTotalVotes( Guid pollQuestionId )
        {
            int count = 0;
            PollVoteCollection pvc = PollVoteCollection.GetAll();
            foreach ( PollVote pv in pvc )
            {
                if ( pv.PollQuestionID == pollQuestionId )
                {
                    count += 1;
                }
            }
            return count;
        }

        // Pallier 70 ms
        public int FindCountTotalVotes( Guid pollQuestionId )
        {
            int count = 0;
            foreach ( PollVote pv in this )
            {
                if ( pv.PollQuestionID == pollQuestionId )
                {
                    count += 1;
                }
            }
            return count;
        }

        // Pallier 70 ms
        public bool FindIfPersonneHasVoted( Guid pollQuestionId, Guid personneGuid )
        {
            foreach ( PollVote pv in this )
            {
                if ( pv.PollQuestionID == pollQuestionId && pv.UserGUID == personneGuid )
                {
                    return true;
                }
            }
            return false;
        }

        // Pour pallier au fait que NumberOfVotesByAnswer prenne 100 ms
        public int FindNumberOfVotesByAnswer( Guid answerID )
        {
            int count = 0;
            foreach ( PollVote pv in this )
            {
                if ( pv.PollAnswerId == answerID )
                {
                    count += 1;
                }
            }
            return count;
        }

        // OPT00020100219
        // Voila une procedure qui prend 100 ms au bas mot lorsque l'on a 1300 votes
        public static int NumberOfVotesByAnswer( Guid answerID )
        {
            // OPT00020100219int count = 0;
            //PollVoteCollection pvc = PollVoteCollection.GetAll();
            //foreach ( PollVote pv in pvc )
            //{
            //    if ( pv.PollAnswerId == answerID )
            //    {
            //        count += 1;
            //    }
            //}

            PollVoteCollection pvc = PollVoteDAL.GetPollVoteByPollAnswerID( answerID );
            return pvc.Count;
        }
    }

    public class PollVoteDAL
    {
        public static PollVoteCollection GetAll()
        {
            DataSet ds = new DataSet();
            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetPollVoteAll"
            );

            PollVoteCollection oc = new PollVoteCollection();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                PollVote o = PollVote.Fill( r );
                oc.Add( o );
            }
            return oc;
        }

        public static PollVoteCollection GetPollVotes( int questionnaireID, Guid personneGUID )
        {
            SqlParameter[] parameters = new SqlParameter[ 2 ];
            parameters[ 0 ] = new SqlParameter( "@QuestionnaireID", SqlDbType.Int );
            parameters[ 0 ].Value = questionnaireID;
            parameters[ 1 ] = new SqlParameter( "@UserGUID", SqlDbType.UniqueIdentifier );
            parameters[ 1 ].Value = personneGUID;

            DataSet ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetPollVotes",
                parameters
            );

            PollVoteCollection oc = new PollVoteCollection();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                PollVote o = PollVote.Fill( r );
                oc.Add( o );
            }
            return oc;
        }

        public static PollVoteCollection GetPollVotesByQuestionnaireID( int questionnaireID )
        {
            SqlParameter[] parameters = new SqlParameter[ 1 ];
            parameters[ 0 ] = new SqlParameter( "@QuestionnaireID", SqlDbType.Int );
            parameters[ 0 ].Value = questionnaireID;

            DataSet ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetPollVotesByQuestionnaireID",
                parameters
            );

            PollVoteCollection oc = new PollVoteCollection();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                PollVote o = PollVote.Fill( r );
                oc.Add( o );
            }
            return oc;
        }

        // OPT00020100219
        public static PollVoteCollection GetPollVoteByPollAnswerID( Guid pollAnswerID )
        {
            DataSet ds = new DataSet();
            SqlParameter param = new SqlParameter();
            param = new SqlParameter( "@PollAnswerID", SqlDbType.UniqueIdentifier );
            param.Value = pollAnswerID;

            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetPollVoteByPollAnswerID",
                param
            );

            PollVoteCollection oc = new PollVoteCollection();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                PollVote o = PollVote.Fill( r );
                oc.Add( o );
            }
            return oc;
        }
    }
}

