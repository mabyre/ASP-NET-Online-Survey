//
// Convention les Get ... utilise de DAL pour chercher les donnees
// les FindBy ... recherche dans la collection elle meme sans utiliser le DAL
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
/// Score associes aux Score
/// </summary>
namespace Sql.Web.Data
{
    [Serializable()]
    public class Score : IComparer<Score>
    {
        public Score()
        {
        }

        #region Proprietees

        int _ScoreID;
        public int ScoreID
        {
            get { return _ScoreID; }
            set { _ScoreID = value; }
        }

        int _ScoreQuestionnaireID;
        public int ScoreQuestionnaireID
        {
            get { return _ScoreQuestionnaireID; }
            set { _ScoreQuestionnaireID = value; }
        }

        int _ScoreMin;
        public int ScoreMin
        {
            get { return _ScoreMin; }
            set { _ScoreMin = value; }
        }

        int _ScoreMax;
        public int ScoreMax
        {
            get { return _ScoreMax; }
            set { _ScoreMax = value; }
        }

        string _ScoreTexte;
        public string ScoreTexte
        {
            get { return _ScoreTexte; }
            set { _ScoreTexte = value; }
        }

        #endregion

        public static Score Fill( DataRow r )
        {
            Score o = new Score();

            o.ScoreID = ( int )r[ "ScoreID" ];
            o.ScoreQuestionnaireID = ( int )r[ "ScoreQuestionnaireID" ];
            o.ScoreMin = ( int )r[ "ScoreMin" ];
            o.ScoreMax = ( int )r[ "ScoreMax" ];
            o.ScoreTexte = r[ "ScoreTexte" ].ToString();

            return o;
        }

        public static Score FillFromXML( DataRow r )
        {
            Score o = new Score();

            o.ScoreID = int.Parse( r[ "ScoreID" ].ToString() );
            o.ScoreQuestionnaireID = int.Parse( r[ "ScoreQuestionnaireID" ].ToString() );
            o.ScoreMin = int.Parse( r[ "ScoreMin" ].ToString() );
            o.ScoreMax = int.Parse( r[ "ScoreMax" ].ToString() );
            o.ScoreTexte = r[ "ScoreTexte" ].ToString();

            return o;
        }

        #region Comparateurs

        /// <summary>
        /// Comparateur generic pour deriver de IComparer<T>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare( Score x, Score y )
        {
            return x.ScoreTexte.CompareTo( y.ScoreTexte ); // jamais utilise
        }

        #endregion

        #region CreateUpdateDeleteMethodes

        public static int Create( Score o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "ScoreCreate", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            // Parametre de sortie de la procedure
            SqlParameter param = new SqlParameter( "@ScoreID", SqlDbType.Int );
            param.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add( param );

            AddParameters( o, ref myCommand );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            if ( status == 1 )
                o.ScoreID = Convert.ToInt32( param.Value );
            myConnection.Close();

            return status;
        }

        public static int Update( Score o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "ScoreUpdate", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@ScoreID", SqlDbType.Int, 4 );
            param.Value = o.ScoreID;
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

            SqlCommand myCommand = new SqlCommand( "ScoreDelete", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@ScoreID", SqlDbType.Int );
            param.Value = id;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return status;
        }

        private static void AddParameters( Score o, ref SqlCommand cmd )
        {
            SqlParameter param = new SqlParameter();

            param = new SqlParameter( "@ScoreQuestionnaireID", SqlDbType.Int );
            param.Value = o.ScoreQuestionnaireID;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@ScoreMin", SqlDbType.Int );
            param.Value = o.ScoreMin;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@ScoreMax", SqlDbType.Int );
            param.Value = o.ScoreMax;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@ScoreTexte", SqlDbType.Text );
            param.Value = o.ScoreTexte;
            cmd.Parameters.Add( param );
        }

        #endregion
    }
    
    [Serializable()]
    public class ScoreCollection : List<Score>
    {
        private static List<Score> _collection = null;

        public ScoreCollection()
        {
            _collection = new List<Score>();
        }

        public static ScoreCollection GetScoreQuestionnaire( int questionnaireID )
        {
            return ScoreDAL.GetScoreQuestionnaire( questionnaireID );
        }

        public Score FindByScoreID( int scoreID )
        {
            foreach ( Score o in this )
            {
                if ( o.ScoreID == scoreID )
                {
                    return o;
                }
            }
            return null;
        }
    }

    public class ScoreDAL
    {
        // Obtenir tous les Scores d'un Questionnaire
        public static ScoreCollection GetScoreQuestionnaire( int questionnaireID )
        {
            DataSet ds = new DataSet();
            SqlParameter param = new SqlParameter();

            param = new SqlParameter( "@ScoreQuestionnaireID", SqlDbType.Int );
            param.Value = questionnaireID;

            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetScoreQuestionnaire",
                param
            );

            ScoreCollection oc = new ScoreCollection();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                Score o = Score.Fill( r );
                oc.Add( o );
            }
            return oc;
        }
    }
}
