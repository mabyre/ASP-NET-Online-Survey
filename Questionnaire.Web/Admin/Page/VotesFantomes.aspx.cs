//
// Correction du probleme des votes fantomes
// apres analyse je suis incapable de reproduire ce probleme en test
// s'agit-il d'un probleme d'accès concurrentiel ? uniquement sur la plateforme
// 
// 

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using Sql.Data;
using Sql.Web.Data;

public partial class Admin_Page_VotesFantomes : System.Web.UI.Page
{
    static int columnVoteId = 0;
    static int columnUserGUID = 1;
    static int columnCreationDate = 3;
    static int PollQuestionID = 4;

    protected void Page_Load( object sender, EventArgs e )
    {
        if ( IsPostBack == false )
        {
            string queryString = "SELECT * FROM PollVotes WHERE (PollVotes.UserGUID NOT IN (SELECT PersonneGUID FROM Personne)) ORDER BY CreationDate;";

            using ( SqlConnection connection = new SqlConnection( Tools.DatabaseConnectionString ) )
            {
                SqlCommand command = new SqlCommand( queryString, connection );
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                int count = 0;
                while ( reader.Read() )
                {
                    Label1.Text += String.Format( "{0}", reader[ columnVoteId ] ) + "<br/>";
                    Label2.Text += String.Format( "{0}", reader[ columnUserGUID ] ) + "<br/>";
                    Label3.Text += String.Format( "{0}", reader[ columnCreationDate ] ) + "<br/>";
                    Label4.Text += String.Format( "{0}", reader[ PollQuestionID ] ) + "<br/>";
                    count += 1;
                }

                LabelVotes.Text = "Votes fantômes : " + count.ToString();
                reader.Close();
            }
        }
    }

    protected void ButtonSupprimer_Click( object sender, EventArgs e )
    {
        // Rien pour l'instant on observe
        // Rafraichir le formulaire
        Response.Redirect( Request.RawUrl );
    }


    protected void ButtonGetQuestion_Click( object sender, EventArgs e )
    {
        int columnQuestion = 3;
        int columnQuestionnaireID = 2;

        try
        {
            Guid pollQuestionID = new Guid( TextBoxPollQuestionID.Text );
            string queryString = "SELECT * FROM PollQuestions WHERE PollQuestionID = '" + pollQuestionID + "'";
            using ( SqlConnection connection = new SqlConnection( Tools.DatabaseConnectionString ) )
            {
                SqlCommand command = new SqlCommand( queryString, connection );
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while ( reader.Read() )
                {
                    // Trouver le Questionnaire de la Question
                    int questionnaireID = int.Parse( reader[ columnQuestionnaireID ].ToString() );
                    string queryStringQuestionnaire = "SELECT * FROM Questionnaire WHERE QuestionnaireID = '" + questionnaireID + "'";
                    SqlConnection connection2 = new SqlConnection( Tools.DatabaseConnectionString );
                    connection2.Open();
                    SqlCommand command2 = new SqlCommand( queryStringQuestionnaire, connection2 );
                    SqlDataReader reader2 = command2.ExecuteReader();
                    while ( reader2.Read() )
                    {
                        // Trouver le Membre reponsable de ce vote fantome
                        Guid membreGUID = new Guid ( reader2[ 5 ].ToString() );
                        string queryStringMembre = "SELECT * FROM MemberInfo WHERE MembreGUID = '" + membreGUID.ToString() + "'";
                        SqlConnection connection3 = new SqlConnection( Tools.DatabaseConnectionString );
                        connection3.Open();
                        SqlCommand command3 = new SqlCommand( queryStringMembre, connection3 );
                        SqlDataReader reader3 = command3.ExecuteReader();
                        while ( reader3.Read() )
                        {
                            LabelQuestion.Text += String.Format( "{0}/{1}/{2}", reader3[ 2 ], reader3[ 4 ], reader3[ 5 ] ) + " | ";
                        }
                        reader3.Close();
                        // Fin de Trouver le Membre

                        LabelQuestion.Text += String.Format( "{0}", reader2[ 1 ] ) + " | ";
                    }
                    reader2.Close();
                    // Fin de Trouver le Questionnaire

                    LabelQuestion.Text += String.Format( "{0}", reader[ columnQuestion ] ) + "<br/>";
                }
                reader.Close();
            }
        }
        catch ( Exception ex )
        {
            LabelQuestion.Text = ex.Message;
        }
    }

    protected void ButtonSupprimerVote_Click( object sender, EventArgs e )
    {
        int status = 0;

        try
        {
            Guid voteID = new Guid( TextBoxVoteID.Text );
            status += PollVote.Delete( voteID );
            LabelVote.Text += "Vote supprimé : " + voteID.ToString() + " satus : " + status.ToString() + "<br/>";
        }
        catch ( Exception ex )
        {
            LabelVote.Text += ex.Message;
        }
    }

    protected void ButtonSupprimerVotesUserGUID_Click( object sender, EventArgs e )
    {
        try
        {
            Guid userGUID = new Guid( TextBoxUserGUID.Text );
            string queryString = "DELETE FROM PollVotes WHERE UserGUID = '" + userGUID + "'";
            using ( SqlConnection connection = new SqlConnection( Tools.DatabaseConnectionString ) )
            {
                SqlCommand command = new SqlCommand( queryString, connection );
                connection.Open();
                int count = command.ExecuteNonQuery();
                LabelVotesUserGUID.Text += String.Format( "Votes supprimés : {0}", count.ToString() ) + "<br/>";
            }
        }
        catch ( Exception ex )
        {
            LabelVotesUserGUID.Text = ex.Message;
        }
    }

}
