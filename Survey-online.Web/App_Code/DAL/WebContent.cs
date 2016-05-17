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
/// Description résumée de WebContent
/// </summary>
namespace Sql.Web.Data
{
    public class WebContent
    {
        public WebContent()
        {
        }

        // string utilisees dans la BD
        public static string ToutLeMonde = "Tout le monde";
        public static string Admin = "admin";

        public static bool CanEdit()
        {
            if ( HttpContext.Current.User.IsInRole( "Administrateur" ) )
            {
                return true;
            }
            if ( SessionState.MemberInfo != null )
            {
                return true;
            }
            return false;
        }

        //public static string GetUtilisateur()
        //{
        //    if ( SessionState.Personne != null && SessionState.Personne.CodeAcces != 0 )
        //    {
        //        // Corriger un BUG : l'intervieweur authentifie ne voyait pas la bonne page
        //        if ( SessionState.Personne.Nom + SessionState.Personne.Prenom == "Monsieur l'Administrateur" )
        //        {
        //            return Admin;
        //        }

        //        if ( HttpContext.Current.Session[ "NomUtilisateur" ] == null )
        //        {
        //            // Trouver le NomUtilisateur du Membre auquel appartient cette Personne
        //            Questionnaire q = Questionnaire.GetQuestionnaire( SessionState.Personne.QuestionnaireID );
        //            MemberInfo membre = MemberInfo.Get( q.MembreGUID );
        //            HttpContext.Current.Session[ "NomUtilisateur" ] = membre.NomUtilisateur;
        //        }
        //        return HttpContext.Current.Session[ "NomUtilisateur" ].ToString();
        //    }
        //    else if ( SessionState.MemberInfo != null )
        //    {
        //        return SessionState.MemberInfo.NomUtilisateur;
        //    }
        //    else
        //    {
        //        // C'est l'admin
        //        return Admin;
        //    }
        //}

        public static string GetUtilisateur()
        {
            if ( SessionState.Personne != null && SessionState.Personne.CodeAcces != 0 )
            {
                if ( HttpContext.Current.Session[ "NomUtilisateur" ] == null )
                {
                    // Trouver le NomUtilisateur du Membre auquel appartient cette Personne
                    Questionnaire q = Questionnaire.GetQuestionnaire( SessionState.Personne.QuestionnaireID );
                    MemberInfo membre = MemberInfo.Get( q.MembreGUID );
                    HttpContext.Current.Session[ "NomUtilisateur" ] = membre.NomUtilisateur;
                }
                return HttpContext.Current.Session[ "NomUtilisateur" ].ToString();
            }
            else if ( SessionState.MemberInfo != null )
            {
                return SessionState.MemberInfo.NomUtilisateur;
            }
            return "";
        }

        public static string GetVisualiseur()
        {
            if ( SessionState.Personne != null && SessionState.Personne.CodeAcces != 0 )
            {
                return SessionState.Personne.CodeAcces.ToString();
            }
            else
            {
                // C'est l'admin ou un membre
                // Ils ne peuvent voir que les Pages "ToutLeMonde"
                return ToutLeMonde;
            }
        }

        #region Proprietees

        int _WebContentID = 0;
        public int WebContentID
        {
            get { return _WebContentID; }
            set { _WebContentID = value; }
        }

        string _Section;
        public string Section
        {
            get { return _Section; }
            set { _Section = value; }
        }

        string _SectionContent;
        public string SectionContent
        {
            get { return _SectionContent; }
            set { _SectionContent = value; }
        }

        string _Utilisateur;
        public string Utilisateur
        {
            get { return _Utilisateur; }
            set { _Utilisateur = value; }
        }

        string _Visualisateur;
        public string Visualisateur
        {
            get { return _Visualisateur; }
            set { _Visualisateur = value; }
        }

        DateTime _WebContentDate = DateTime.Now;
        public DateTime WebContentDate
        {
            get { return _WebContentDate; }
            set { _WebContentDate = value; }
        }

        #endregion

        public static WebContent Fill( DataRow r )
        {
            WebContent o = new WebContent();

            o.WebContentID = ( int )( r[ "WebContentID" ] );
            o.Section = ( string )( r[ "Section" ] );
            o.SectionContent = r[ "SectionContent" ].ToString();
            if ( r[ "Utilisateur" ] != null )
                o.Utilisateur = r[ "Utilisateur" ].ToString();
            if ( r[ "Visualisateur" ] != null )
                o.Visualisateur = r[ "Visualisateur" ].ToString();
            o.WebContentDate = DateTime.Parse( r[ "WebContentDate" ].ToString() );

            return o;
        }

        #region CreateUpdateDeleteMethodes

        public static int Create( WebContent o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "WebContentCreate", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            AddParameters( o, ref myCommand );

            // Parametre de sortie de la procedure l'ID de l'objet cree
            SqlParameter param = new SqlParameter( "@WebContentID", SqlDbType.Int );
            param.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            if ( status == 1 )
                o.WebContentID = Convert.ToInt32( param.Value );
            myConnection.Close();

            return status;
        }

        public static int Update( WebContent o )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "WebContentUpdate", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            AddParameters( o, ref myCommand );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return status;
        }

        public static int Delete( int id )
        {
            SqlConnection myConnection = new SqlConnection( Tools.DatabaseConnectionString );

            SqlCommand myCommand = new SqlCommand( "WebContentDelete", myConnection );
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter( "@WebContentID", SqlDbType.Int );
            param.Value = id;
            myCommand.Parameters.Add( param );

            myConnection.Open();
            int status = Convert.ToInt32( myCommand.ExecuteScalar() );
            myConnection.Close();

            return status;
        }

        private static void AddParameters( WebContent o, ref SqlCommand cmd )
        {
            SqlParameter param = new SqlParameter();

            param = new SqlParameter( "@Section", SqlDbType.NVarChar );
            param.Value = o.Section;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@SectionContent", SqlDbType.Text );
            param.Value = o.SectionContent;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Utilisateur", SqlDbType.NVarChar, 50 );
            param.Value = o.Utilisateur;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@Visualisateur", SqlDbType.NVarChar, 50 );
            param.Value = o.Visualisateur;
            cmd.Parameters.Add( param );

            param = new SqlParameter( "@WebContentDate", SqlDbType.DateTime );
            param.Value = o.WebContentDate;
            cmd.Parameters.Add( param );
        }

        #endregion

        public static WebContent GetWebContent( int webContentID )
        {
            if ( webContentID == 0 )
            {
                return null;
            }

            WebContentCollection collection = WebContentCollection.GetAll();
            foreach ( WebContent o in collection )
            {
                if ( o.WebContentID == webContentID )
                {
                    return o;
                }
            }

            return null;
        }

        public static WebContent GetWebContent( string section, string utilisateur, string visualiseur )
        {
            if ( section == String.Empty )
            {
                return null;
            }

            WebContentCollection collection = WebContentCollection.GetAll();
            foreach ( WebContent o in collection )
            {
                if (   o.Section == section
                    && o.Utilisateur == utilisateur
                    && o.Visualisateur == visualiseur )
                {
                    return o;
                }
            }

            return null;
        }
    }

    public class WebContentCollection : List<WebContent>
    {
        private List<WebContent> _collection = null;

        public WebContentCollection()
        {
            _collection = new List<WebContent>();
        }

        public static WebContentCollection GetAll()
        {
            return WebContentDAL.GetAll();
        }

        // Toute les sections d'un utilisateur sans distinction de visualiseur
        public static WebContentCollection GetAllByUser( string utilisateur )
        {
            WebContentCollection collection = WebContentDAL.GetByUser( utilisateur );
            return collection;
        }

        // Trouver tous les WebContent d'un utilisateur pour un visualiseur ( pour la suppression )
        public static WebContentCollection GetWebContents( string utilisateur, string visualiseur )
        {
            WebContentCollection newwcc = new WebContentCollection();
            WebContentCollection collection = WebContentDAL.GetByUser( utilisateur );
            foreach ( WebContent o in collection )
            {
                if ( o.Visualisateur == visualiseur )
                {
                    newwcc.Add( o );
                }
            }
            return newwcc;
        }

        // Trouver tous les WebContent d'un utilisateur pour une section 
        // b ne sert à rien
        public static WebContentCollection GetWebContents( string utilisateur, string section, bool b )
        {
            WebContentCollection newwcc = new WebContentCollection();
            WebContentCollection collection = WebContentDAL.GetByUser( utilisateur );
            foreach ( WebContent o in collection )
            {
                if ( o.Section == section )
                {
                    newwcc.Add( o );
                }
            }
            return newwcc;
        }
    }

    public class WebContentDAL
    {
        public static WebContentCollection GetAll()
        {
            DataSet ds = new DataSet();
            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetWebContentAll"
            );

            WebContentCollection oc = new WebContentCollection();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                WebContent o = WebContent.Fill( r );
                oc.Add( o );
            }
            return oc;
        }

        public static WebContentCollection GetByUser( string utilisateur )
        {
            SqlParameter[] parameters = new SqlParameter[ 1 ];
            parameters[ 0 ] = new SqlParameter( "@Utilisateur", SqlDbType.NVarChar );
            parameters[ 0 ].Value = utilisateur;

            DataSet ds = new DataSet();
            ds = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                CommandType.StoredProcedure,
                "GetWebContentByUser",
                parameters
            );

            WebContentCollection oc = new WebContentCollection();
            foreach ( DataRow r in ds.Tables[ 0 ].Rows )
            {
                WebContent o = WebContent.Fill( r );
                oc.Add( o );
            }
            return oc;
        }

    }
}