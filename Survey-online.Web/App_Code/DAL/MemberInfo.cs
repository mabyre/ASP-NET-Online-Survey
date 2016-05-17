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
/// Description résumée de MemberInfo
/// </summary>
namespace Sql.Web.Data
{
    [Serializable()]
    public class MemberInfo
    {
        public MemberInfo()
        {
        }

        #region Proprietees

        // Liaison de donnees avec aspnet_Membership
        Guid _MembreGUID = new Guid();
        public Guid MembreGUID
        {
            get { return _MembreGUID; }
            set { _MembreGUID = value; }
        }

        int _MembreID = 0;
        public int MembreID
        {
            get { return _MembreID; }
            set { _MembreID = value; }
        }

        string _NomUtilisateur = "";
        public string NomUtilisateur
        {
            get { return _NomUtilisateur; }
            set { _NomUtilisateur = value; }
        }

        string _MotDePasse = "";
        public string MotDePasse
        {
            get { return _MotDePasse; }
            set { _MotDePasse = value; }
        }

        string _Nom = "";
        public string Nom
        {
            get { return _Nom; }
            set { _Nom = value; }
        }

        string _Prenom = "";
        public string Prenom
        {
            get { return _Prenom; }
            set { _Prenom = value; }
        }

        string _Adresse = "";
        public string Adresse
        {
            get { return _Adresse; }
            set { _Adresse = value; }
        }

        string _Telephone = "";
        public string Telephone
        {
            get { return _Telephone; }
            set { _Telephone = value; }
        }

        string _Societe = "";
        public string Societe
        {
            get { return _Societe; }
            set { _Societe = value; }
        }

        int _LimiteQuestionnaires = 0;
        public int LimiteQuestionnaires
        {
            get { return _LimiteQuestionnaires; }
            set { _LimiteQuestionnaires = value; }
        }

        int _LimiteQuestions = 0;
        public int LimiteQuestions
        {
            get { return _LimiteQuestions; }
            set { _LimiteQuestions = value; }
        }

        int _LimiteInterviewes = 0;
        public int LimiteInterviewes
        {
            get { return _LimiteInterviewes; }
            set { _LimiteInterviewes = value; }
        }

        int _LimiteReponses = 0;
        public int LimiteReponses
        {
            get { return _LimiteReponses; }
            set { _LimiteReponses = value; }
        }

        DateTime _DateFinAbonnement;
        public DateTime DateFinAbonnement
        {
            get { return _DateFinAbonnement; }
            set { _DateFinAbonnement = value; }
        }

        #endregion

        public static MemberInfo Fill( DataRow r )
        {
            MemberInfo o = new MemberInfo();

            o.MembreGUID = ( Guid )( r[ "MembreGUID" ] );
            o.MembreID = ( int )( r[ "MembreID" ] );
            o.NomUtilisateur = r[ "NomUtilisateur" ].ToString();
            o.MotDePasse = r[ "MotDePasse" ].ToString();
            o.Nom = r[ "Nom" ].ToString();
            o.Prenom = r[ "Prenom" ].ToString();
            o.Adresse = r[ "Adresse" ].ToString();
            o.Telephone = r[ "Telephone" ].ToString();
            o.Societe = r[ "Societe" ].ToString();
            o.LimiteQuestionnaires = r[ "LimiteQuestionnaires" ] == System.DBNull.Value ? 0 : ( int )r[ "LimiteQuestionnaires" ];  // Garder la compatibilite avec les anciens membres les limites peuvent etre NULL
            o.LimiteQuestions = r[ "LimiteQuestions" ] == System.DBNull.Value ? 0 : ( int )r[ "LimiteQuestions" ];
            o.LimiteInterviewes = r[ "LimiteInterviewes" ] == System.DBNull.Value ? 0 : ( int )r[ "LimiteInterviewes" ];
            o.LimiteReponses = r[ "LimiteReponses" ] == System.DBNull.Value ? 0 : ( int )r[ "LimiteReponses" ];
            o.DateFinAbonnement = r[ "DateFinAbonnement" ] == System.DBNull.Value ? DateTime.Now : DateTime.Parse( r[ "DateFinAbonnement" ].ToString() );

            return o;
        }

        #region CreateUpdateDeleteMethodes

        public static int Create( MemberInfo o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "MemberInfoCreate", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            AddParameters( o, ref myCommand );

            // Parametre de sortie de la procedure le Guid du vote cree
            SqlParameter param = new SqlParameter( "@MembreID", SqlDbType.Int );
            param.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            if ( status == 1 )
                o.MembreID = ( int )( param.Value );
            myConnection.Close();

            return status; // 1 : insert reussi 2 : c'est rate
        }

        public static int Update( MemberInfo o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "MemberInfoUpdate", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            AddParameters( o, ref myCommand );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return status; // 2 : l'objet existe, 1 : maj Ok, 0 : Sql Server Error
        }

        public static int Delete( Guid id )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "MemberInfoDelete", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@MembreGUID", SqlDbType.UniqueIdentifier );
            param.Value = id;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return status;
        }

        private static void AddParameters( MemberInfo o, ref SqlCommand cmd )
        {
            SqlParameter param = new SqlParameter();

            param = new SqlParameter( "@MembreGUID", SqlDbType.UniqueIdentifier );
            param.Value = o.MembreGUID;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@NomUtilisateur", SqlDbType.NVarChar, 50 );
            param.Value = o.NomUtilisateur;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@MotDePasse", SqlDbType.NVarChar, 50 );
            param.Value = o.MotDePasse;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Nom", SqlDbType.NVarChar, 50 );
            param.Value = o.Nom;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Prenom", SqlDbType.NVarChar, 50 );
            param.Value = o.Prenom;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Adresse", SqlDbType.NVarChar, 500 );
            param.Value = o.Adresse;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Telephone", SqlDbType.NVarChar, 50 );
            param.Value = o.Telephone;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Societe", SqlDbType.NVarChar, 80 );
            param.Value = o.Societe;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@LimiteQuestionnaires", SqlDbType.Int );
            param.Value = o.LimiteQuestionnaires;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@LimiteQuestions", SqlDbType.Int );
            param.Value = o.LimiteQuestions;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@LimiteInterviewes", SqlDbType.Int );
            param.Value = o.LimiteInterviewes;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@LimiteReponses", SqlDbType.Int );
            param.Value = o.LimiteReponses;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@DateFinAbonnement", SqlDbType.DateTime );
            param.Value = o.DateFinAbonnement;
            cmd.Parameters.Add( param );
        }

        #endregion

        #region GetMethodes

        public static MemberInfo Get( Guid guid )
        {
            MemberInfoCollection oc = MemberInfoCollection.GetAll();
            foreach ( MemberInfo o in oc )
            {
                if ( o.MembreGUID == guid )
                {
                    return o;
                }
            }
            return null;
        }

        public static MemberInfo GetMemberInfo( string nom, string prenom )
        {
            MemberInfoCollection oc = MemberInfoCollection.GetAll();
            foreach ( MemberInfo o in oc )
            {
                if ( o.Nom == nom && o.Prenom == prenom )
                {
                    return o;
                }
            }
            return null;
        }

        // MembershipUser.UserName
        public static MemberInfo GetMemberInfo( string nomUtilisateur )
        {
            MemberInfoCollection oc = MemberInfoCollection.GetAll();
            foreach ( MemberInfo o in oc )
            {
                if ( o.NomUtilisateur == nomUtilisateur )
                {
                    return o;
                }
            }
            return null;
        }

        //public static MemberInfo GetMemberInfo( Guid membreGUID )
        //{
        //    MemberInfoCollection oc = MemberInfoCollection.GetAll();
        //    foreach ( MemberInfo o in oc )
        //    {
        //        if ( o.MembreGUID == membreGUID )
        //        {
        //            return o;
        //        }
        //    }
        //    return null;
        //}

        // OPT20072010
        public static MemberInfo GetMemberInfo( Guid membreGUID )
        {
            DataSet ds = new DataSet();
            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                "GetMemberInfo",
                membreGUID
            );

            if ( ds.Tables[ 0 ].Rows.Count < 1 )
                return null;

            MemberInfo o = new MemberInfo();
            o = MemberInfo.Fill( ds.Tables[ 0 ].Rows[ 0 ] );

            return o;
        }

        #endregion

        #region SpecifiqueMethodes

        // Optimisation obtenir le nombre de repondants pour un Membre
        // retourne -1 si il y a eu une erreur
        public static int GetMemberRepondantCount( Guid membreGUID )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "GetMemberRepondantsCount", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@MembreGUID", SqlDbType.UniqueIdentifier );
            param.Value = membreGUID;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return status;
        }

        #endregion
    }

    public class MemberInfoCollection : List<MemberInfo>
    {
        private List<MemberInfo> _collection = null;

        public MemberInfoCollection()
        {
            _collection = new List<MemberInfo>();
        }

        public static MemberInfoCollection GetAll()
        {
            return MemberInfoDAL.GetMembres();
        }
    }

    /// <summary>
    /// SELECT * FROM MemberInfo ORDER BY Nom ASC
    /// </summary>
    public class MemberInfoDAL
    {
        public static MemberInfoCollection GetMembres()
        {
            DataSet ds = new DataSet();
            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetMemberInfoAll"
            );

            MemberInfoCollection oc = new MemberInfoCollection();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                MemberInfo o = MemberInfo.Fill( r );
                oc.Add( o );
            }
            return oc;
        }
    }
}
