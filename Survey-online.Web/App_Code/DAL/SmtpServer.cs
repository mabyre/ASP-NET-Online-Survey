
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
    /// L'objet SmtpServer
    /// </summary>
    public class SmtpServer : IComparer<SmtpServer>
    {
        public SmtpServer()
        {
        }

        #region Proprietees

        int _SmtpServerID;
        public int SmtpServerID
        {
            get { return _SmtpServerID; }
            set { _SmtpServerID = value; }
        }

        Guid _UserGUID;
        public Guid UserGUID
        {
            get { return _UserGUID; }
            set { _UserGUID = value; }
        }

        string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        string _UserPassWord;
        public string UserPassWord
        {
            get { return _UserPassWord; }
            set { _UserPassWord = value; }
        }

        string _Email;
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        string _EmailSubject;
        public string EmailSubject
        {
            get { return _EmailSubject; }
            set { _EmailSubject = value; }
        }

        string _EmailBody;
        public string EmailBody
        {
            get { return _EmailBody; }
            set { _EmailBody = value; }
        }

        int _ServerPort;
        public int ServerPort
        {
            get { return _ServerPort; }
            set { _ServerPort = value; }
        }

        string _ServerName;
        public string ServerName
        {
            get { return _ServerName; }
            set { _ServerName = value; }
        }

        bool _EnableSSL;
        public bool EnableSSL
        {
            get { return _EnableSSL; }
            set { _EnableSSL = value; }
        }

        #endregion

        public static SmtpServer Fill( DataRow r )
        {
            SmtpServer o = new SmtpServer();

            o.SmtpServerID = Convert.ToInt32( r[ "SmtpServerID" ].ToString() );
            o.UserGUID = ( Guid )r[ "UserGUID" ];
            o.UserName = r[ "UserName" ].ToString();
            o.UserPassWord = r[ "UserPassWord" ].ToString();
            o.Email = r[ "Email" ].ToString();
            o.EmailSubject = r[ "EmailSubject" ].ToString();
            o.EmailBody = r[ "EmailBody" ].ToString();
            o.ServerPort = int.Parse( r[ "ServerPort" ].ToString() );
            o.ServerName = r[ "ServerName" ].ToString();
            if ( r[ "EnableSSL" ] != System.DBNull.Value ) o.EnableSSL = ( bool )r[ "EnableSSL" ];

            return o;
        }

        public static SmtpServer FillFromXML( DataRow r )
        {
            SmtpServer o = new SmtpServer();

            o.SmtpServerID = Convert.ToInt32( r[ "SmtpServerID" ].ToString() );
            o.UserGUID = new Guid( r[ "UserGUID" ].ToString() );
            o.UserName = r[ "UserName" ].ToString();
            o.UserPassWord = r[ "UserPassWord" ].ToString();
            o.Email = r[ "Email" ].ToString();
            o.EmailSubject = r[ "EmailSubject" ].ToString();
            o.EmailBody = r[ "EmailBody" ].ToString();
            o.ServerPort = int.Parse( r[ "ServerPort" ].ToString() );
            o.ServerName = r[ "ServerName" ].ToString();

            try { o.EnableSSL = r[ "EnableSSL" ].ToString() == "true" ? true : false; }
            catch { o.EnableSSL = false; }

            return o;
        }

        #region Comparateurs

        /// <summary>
        /// Comparateur generic pour deriver de IComparer<T>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare( SmtpServer x, SmtpServer y )
        {
            return x.ServerName.CompareTo( y.ServerName );
        }

        #endregion

        #region GetMethodes

        public static SmtpServer Get( Guid userGUID )
        {
            DataSet ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                "GetSmtpServerUserGUID",
                userGUID
            );

            if ( ds.Tables[ 0 ].Rows.Count < 1 )
                return null;

            DataRow r = ds.Tables[ 0 ].Rows[ 0 ];
            SmtpServer o = Fill( r );
            return o;
        }

        #endregion

        #region CreateCreateUpdateDeleteMethodes

        public static int Create( SmtpServer o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "SmtpServerCreate", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            // Parametre de sortie de la procedure
            SqlParameter param = new SqlParameter( "@SmtpServerID", SqlDbType.Int, 4 );
            param.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add( param );

            AddParameters( o, ref myCommand );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            if ( status == 1 )
                o.SmtpServerID = Convert.ToInt32( param.Value );
            myConnection.Close();

            return status;
        }

        public static int Update( SmtpServer o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "SmtpServerUpdate", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@SmtpServerID", SqlDbType.Int, 4 );
            param.Value = o.SmtpServerID;
            myCommand.Parameters.Add( param );

            AddParameters( o, ref myCommand );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return status;
        }

        public static int Delete( int SmtpServerID )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "SmtpServerDelete", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@SmtpServerID", SqlDbType.Int, 4 );
            param.Value = SmtpServerID;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return status;
        }

        private static void AddParameters( SmtpServer o, ref SqlCommand cmd )
        {
            SqlParameter param = new SqlParameter();

            param = new SqlParameter( "@UserGUID", SqlDbType.UniqueIdentifier );
            param.Value = o.UserGUID;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@UserName", SqlDbType.NVarChar );
            param.Value = o.UserName;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@UserPassWord", SqlDbType.NVarChar );
            param.Value = o.UserPassWord;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Email", SqlDbType.NVarChar );
            param.Value = o.Email;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@EmailSubject", SqlDbType.NVarChar );
            param.Value = o.EmailSubject;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@EmailBody", SqlDbType.Text );
            param.Value = o.EmailBody;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@ServerPort", SqlDbType.Int );
            param.Value = o.ServerPort;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@ServerName", SqlDbType.NVarChar );
            param.Value = o.ServerName;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@EnableSSL", SqlDbType.Bit );
            if ( o.EnableSSL )
                param.Value = true;
            else
                param.Value = System.DBNull.Value;
            cmd.Parameters.Add( param );

        }

        #endregion
    }

    public class SmtpServerCollection : List<SmtpServer>
    {
        private List<SmtpServer> _collection = null;

        public SmtpServerCollection()
        {
            _collection = new List<SmtpServer>();
        }

        public static SmtpServerCollection GetAll()
        {
            return SmtpServerDAL.GetSmtpServers();
        }
    }

    public class SmtpServerDAL
    {
        public static SmtpServerCollection GetSmtpServers()
        {
            DataSet ds = new DataSet();
            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetSmtpServerAll"
            );

            SmtpServerCollection SmtpServers = new SmtpServerCollection();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                SmtpServer p = SmtpServer.Fill( r );
                SmtpServers.Add( p );
            }
            return SmtpServers;
        }
    }
}