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
/// Description résumée de PollReaction
/// </summary>
public class PollReaction
{
    public PollReaction()
    {
    }

	
    #region Proprietees

    Guid _ReactionId;
    public Guid ReactionId
    {
        get { return _ReactionId; }
        set { _ReactionId = value; }
    }

    Guid _UserId;
    public Guid UserId
    {
        get { return _UserId; }
        set { _UserId = value; }
    }

    DateTime _CreationDate;
    public DateTime CreationDate
    {
        get { return _CreationDate; }
        set { _CreationDate = value; }
    }

    string _Reaction;
    public string Reaction
    {
        get { return _Reaction; }
        set { _Reaction = value; }
    }

    Guid _PollId;
    public Guid PollId
    {
        get { return _PollId; }
        set { _PollId = value; }
    }
    #endregion

    public static PollReaction Fill( DataRow r )
    {
        PollReaction o = new PollReaction();

        o.ReactionId = (Guid)(r[ "ReactionId" ]);
        o.UserId = ( Guid )( r[ "UserId" ] );
        o.CreationDate = DateTime.Parse( r[ "CreationDate" ].ToString() );
        o.Reaction = r[ "Reaction" ].ToString();
        o.PollId = (Guid)( r[ "PollId" ] );

        return o;
    }

    #region CreateUpdateDeleteMethodes

    public static int Create( PollReaction o )
    {
        SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

        SqlCommand myCommand = new SqlCommand( "PollReactionCreate", myConnection );
        myCommand.CommandType = CommandType.StoredProcedure;

        AddParameters( o, ref myCommand );

        myConnection.Open();
        int iStatusCode = Convert.ToInt32( myCommand.ExecuteScalar() );
        //if ( iStatusCode == 1 )
        //    ID = Convert.ToInt32( param.Value );
        myConnection.Close();

        return iStatusCode;
    }

    public static int Update( PollReaction o )
    {
        SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

        SqlCommand myCommand = new SqlCommand( "PollReactionUpdate", myConnection );
        myCommand.CommandType = CommandType.StoredProcedure;

        AddParameters( o, ref myCommand );

        myConnection.Open();
        int iStatusCode = Convert.ToInt32( myCommand.ExecuteScalar() );
        myConnection.Close();

        return iStatusCode;
    }

    public static int Delete( Guid id )
    {
        SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

        SqlCommand myCommand = new SqlCommand( "PollReactionDelete", myConnection );
        myCommand.CommandType = CommandType.StoredProcedure;

        SqlParameter param = new SqlParameter( "@ReactionId", SqlDbType.UniqueIdentifier );
        param.Value = id;
        myCommand.Parameters.Add( param );

        myConnection.Open();
        int iStatusCode = Convert.ToInt32( myCommand.ExecuteScalar() );
        myConnection.Close();

        return iStatusCode;
    }

    private static void AddParameters( PollReaction o, ref SqlCommand cmd )
    {
        SqlParameter param = new SqlParameter();

        param = new SqlParameter( "@ReactionId", SqlDbType.UniqueIdentifier );
        param.Value = o.ReactionId;
        cmd.Parameters.Add( param );

        param = new SqlParameter( "@UserId", SqlDbType.UniqueIdentifier );
        param.Value = o.UserId;
        cmd.Parameters.Add( param );

        param = new SqlParameter( "@CreationDate", SqlDbType.DateTime );
        param.Value = o.CreationDate;
        cmd.Parameters.Add( param );

        param = new SqlParameter( "@Reaction", SqlDbType.NVarChar );
        param.Value = o.Reaction;
        cmd.Parameters.Add( param );

        param = new SqlParameter( "@PollId", SqlDbType.UniqueIdentifier );
        param.Value = o.PollId;
        cmd.Parameters.Add( param );
    }

    #endregion

    public static PollReactionCollection GetReactionsForPollQuestion( Guid pollQuestionId )
    {
        if ( pollQuestionId == Guid.Empty )
        {
            return null;
        }
        PollReactionCollection prc = new PollReactionCollection( true );
        PollReactionCollection newprc = new PollReactionCollection( false );
        foreach ( PollReaction pr in prc )
        {
            if ( pr.PollId == pollQuestionId )
            {
                newprc.Add( pr );
            }
        }
        return newprc;
    }


    public static int CountTotalReactions( Guid pollId )
    {
        PollReactionCollection p = new PollReactionCollection( true );
        return p.Count;
    }

    public class PollReactionCollection : List<PollReaction>
    {
        private List<PollReaction> _collection = null;

        public PollReactionCollection( bool init )
        {
            _collection = new List<PollReaction>();

            if ( init )
            {
                _collection = PollReactionDAL.GetQuestions();

                // this est une List<T> est donc en lecture seule
                foreach ( PollReaction p in _collection )
                    this.Add( p );
            }
        }
    }

    public class PollReactionDAL
    {
        public static List<PollReaction> GetQuestions()
        {
            DataSet ds = new DataSet();
            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetPollReactionAll"
            );

            List<PollReaction> oc = new List<PollReaction>();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                PollReaction o = PollReaction.Fill( r );
                oc.Add( o );
            }
            return oc;
        }
    }
}
