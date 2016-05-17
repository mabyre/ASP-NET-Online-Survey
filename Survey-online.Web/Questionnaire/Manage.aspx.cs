//
// Mais qui est le gros CONNARD qui a decide que l'attribut ReadOnly s'ecrivait InsertVisible 
// dans le code ca marche pas encore un Gros bug de MERDE
//

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Sql.Web.Data;

partial class Questionnaire_Manage : PageBase
{
    static int colonneCommandes = 0;
    static int colonneNom = 3;
    static int colonnePrenom = 4;
    static int colonneSociete = 5;
    static int colonneAnonymat = 11;
    static int colonneBloque = 12;
    static int colonneDateCreation = 13;
    static int colonneEditer = 17;

    protected void Page_Load( object sender, System.EventArgs e )
    {
        if ( User.IsInRole( "Administrateur" ) == false )
        {
            GridView1.Columns[ colonneNom ].Visible = false;
            GridView1.Columns[ colonnePrenom ].Visible = false;
            GridView1.Columns[ colonneSociete ].Visible = false;
        }

        if ( User.IsInRole( "Administrateur" ) == false && User.IsInRole( "Client" ) == false )
        {
            GridView1.Columns[ colonneCommandes ].Visible = false; // Command Edit Delete
            GridView1.Columns[ colonneEditer ].Visible = false; // Editer le Questionnaire
            RolloverLinkEdit.Visible = false;
        }

        if ( User.IsInRole( "Administrateur" ) )
        {
            GridView1.Columns[ colonneBloque ].Visible = true;
            ( ( BoundField )GridView1.Columns[ colonneAnonymat ] ).ReadOnly = false;
        }

        if ( IsPostBack == false )
        {
            CheckBoxDate.Checked = SessionState.CheckBox[ "CheckBoxDate" ];
            GridView1.Columns[ colonneDateCreation ].Visible = CheckBoxDate.Checked;
        }

        SqlDataSourceQuestionnaire.SelectCommand = SelectCommand();
    }

    private string SelectCommand()
    {
        string sql = "SELECT QuestionnaireID, Description, Style, DateCreation, CodeAcces, Valider, Fin, Anonyme, Anonymat, Publier, Bloque, Questionnaire.MembreGUID, Nom, Prenom, Societe FROM Questionnaire JOIN MemberInfo ON MemberInfo.MembreGUID = Questionnaire.MembreGUID";

        if ( User.IsInRole( "Administrateur" ) == false )
        {
            sql += " WHERE Questionnaire.MembreGUID = '" + SessionState.MemberInfo.MembreGUID + "'";
        }

        return sql;
    }

    protected void GridView1_RowUpdating( object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e )
    {
        if ( e.NewValues[ "Description" ] == null )
        {
            e.NewValues[ "Description" ] = "Description ?";
        }

        if ( CheckBoxDate.Checked == false )
        {
            e.NewValues[ "DateCreation" ] = SessionState.Questionnaire.DateCreation;
        }
        else
        {
            if ( e.NewValues[ "CreationDate" ] != null )
            {
                try
                {
                    DateTime dt = DateTime.Parse( e.NewValues[ "CreationDate" ].ToString() );
                }
                catch
                {
                    e.NewValues[ "CreationDate" ] = e.OldValues[ "CreationDate" ];
                }
            }
        }

        if ( e.NewValues[ "Valider" ] != null )
        {
            e.NewValues[ "Valider" ] = Tools.StringToBoolean( e.NewValues[ "Valider" ].ToString() );
            if ( e.NewValues[ "Valider" ] != null )
                e.NewValues[ "Valider" ] = Tools.StringBooleanFalseToNull( e.NewValues[ "Valider" ].ToString() );
        }

        if ( e.NewValues[ "Fin" ] != null )
        {
            e.NewValues[ "Fin" ] = Tools.StringToBoolean( e.NewValues[ "Fin" ].ToString() );
            if ( e.NewValues[ "Fin" ] != null )
                e.NewValues[ "Fin" ] = Tools.StringBooleanFalseToNull( e.NewValues[ "Fin" ].ToString() );
        }

        if ( e.NewValues[ "Anonyme" ] != null )
        {
            e.NewValues[ "Anonyme" ] = Tools.StringToBoolean( e.NewValues[ "Anonyme" ].ToString() );
            if ( e.NewValues[ "Anonyme" ] != null )
                e.NewValues[ "Anonyme" ] = Tools.StringBooleanFalseToNull( e.NewValues[ "Anonyme" ].ToString() );
        }

        if ( e.NewValues[ "Anonymat" ] != null )
        {
            e.NewValues[ "Anonymat" ] = Tools.StringToBoolean( e.NewValues[ "Anonymat" ].ToString() );
            if ( e.NewValues[ "Anonymat" ] != null )
                e.NewValues[ "Anonymat" ] = Tools.StringBooleanFalseToNull( e.NewValues[ "Anonymat" ].ToString() );
        }
        else
        {
            e.NewValues[ "Anonymat" ] = e.OldValues[ "Anonymat" ];
        }

        if ( e.NewValues[ "Publier" ] != null )
        {
            e.NewValues[ "Publier" ] = Tools.StringToBoolean( e.NewValues[ "Publier" ].ToString() );
            if ( e.NewValues[ "Publier" ] != null )
                e.NewValues[ "Publier" ] = Tools.StringBooleanFalseToNull( e.NewValues[ "Publier" ].ToString() );
        }

        if ( User.IsInRole( "Administrateur" ) )
        {
            if ( e.NewValues[ "Bloque" ] != null )
            {
                e.NewValues[ "Bloque" ] = Tools.StringToBoolean( e.NewValues[ "Bloque" ].ToString() );
                if ( e.NewValues[ "Bloque" ] != null )
                    e.NewValues[ "Bloque" ] = Tools.StringBooleanFalseToNull( e.NewValues[ "Bloque" ].ToString() );
            }
        }
        else
        {
            e.NewValues[ "Bloque" ] = SessionState.Questionnaire.Bloque;
        }
    }

    protected void CheckBoxDate_CheckedChanged( object sender, EventArgs e )
    {
        SessionState.CheckBox[ "CheckBoxDate" ] = CheckBoxDate.Checked;
        GridView1.Columns[ colonneDateCreation ].Visible = CheckBoxDate.Checked; // DateCreation
    }

    protected void GridView1_RowCommand( object sender, GridViewCommandEventArgs e )
    {
        if ( e.CommandName == "Edit" )
        {
            // Trouver le Questionnaire selectionne par l'utilisateur
            int index = Convert.ToInt32( e.CommandArgument );
            GridView gv = ( GridView )e.CommandSource;
            string q = gv.DataKeys[ index ].Value.ToString();
            int questionnaireID = int.Parse( q );
            SessionState.Questionnaire = Questionnaire.GetQuestionnaire( questionnaireID );
        }

        if ( e.CommandName == "Select" )
        {
            // Trouver le Questionnaire selectionne par l'utilisateur
            int index = Convert.ToInt32( e.CommandArgument );
            GridView gv = ( GridView )e.CommandSource;
            string q = gv.DataKeys[ index ].Value.ToString();
            int questionnaireID = int.Parse( q );
            SessionState.Questionnaire = Questionnaire.GetQuestionnaire( questionnaireID );
            // Pour gerer correctement le WebContent
            SessionState.Personne.CodeAcces = SessionState.Questionnaire.CodeAcces;
            SessionState.Personne.QuestionnaireID = SessionState.Questionnaire.QuestionnaireID;

            SqlDataSourceQuestionnaireInfos.SelectCommand = string.Format( "SELECT Description, CodeAcces, DateCreation, QuestionnaireID FROM Questionnaire WHERE QuestionnaireID = '{0}' ORDER BY DateCreation DESC", questionnaireID );
            DataListQuestionnaireInfos.DataBind();
        }

        if ( e.CommandName == "Update" )
        {
        }
    }

    protected void GridView1_RowUpdated( object sender, GridViewUpdatedEventArgs e )
    {
        // Forcer les Questionnaires a se recharger depuis la Base de Donnees
        SessionState.Questionnaires = null;
        // Recharger le Questionnaire edite depuis la BD recuperer les modifs
        SessionState.Questionnaire = Questionnaire.GetQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
    }

    // Formater et Calculer les elements de la DataList
    protected void DataListQuestionnaireInfos_ItemDataBound( object sender, DataListItemEventArgs e )
    {
        if ( e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem )
        {
            Label labelDateCreation = ( Label )e.Item.FindControl( "LabelDateCreation" );
            labelDateCreation.Text = labelDateCreation.Text.Substring( 0, 10 );

            HiddenField hiddenFieldQuestionnaireID = ( HiddenField )e.Item.FindControl( "HiddenFieldQuestionnaireID" );
            int questionnaireID = int.Parse( hiddenFieldQuestionnaireID.Value );

            //
            // Calcul du nombre d'interviewes pour ce Questionnaire
            //
            PersonneCollection personnes = PersonneCollection.GetQuestionnaire( questionnaireID );
            Label labelNombreContacts = ( Label )e.Item.FindControl( "labelNombreContacts" );
            labelNombreContacts.Text = personnes.Count.ToString();
            if ( personnes.Count == 0 )
            {
                labelNombreContacts.ForeColor = System.Drawing.Color.Black;
            }

            //
            // Calculer le nombre de Votants
            //
            int votants = 0;
            PollVoteCollection pollVotes = PollVoteCollection.GetPollVotesByQuestionnaireID( questionnaireID );
            foreach ( Personne p in personnes )
            {
                if ( pollVotes.ADejaVote( p.PersonneGUID ) )
                {
                    votants += 1;
                }
            }

            // Nombre de Votants pour ce Questionnaire
            Label labelVotes = ( Label )e.Item.FindControl( "LabelVotes" );
            labelVotes.Text = votants.ToString();
            if ( votants == 0 )
            {
                labelVotes.ForeColor = System.Drawing.Color.Black;
            }

            //
            // Calculer le nombre de Questions
            //
            Questionnaire quest = SessionState.Questionnaires.FindByID( questionnaireID );
            PollQuestionCollection pollAnswerCollection = PollQuestionCollection.GetByQuestionnaire( quest.QuestionnaireID );

            // Nombre de Votants pour ce Questionnaire
            Label labelQuestions = ( Label )e.Item.FindControl( "LabelQuestions" );
            labelQuestions.Text = pollAnswerCollection.Count.ToString();
            if ( pollAnswerCollection.Count == 0 )
            {
                labelQuestions.ForeColor = System.Drawing.Color.Black;
            }
        }
    }
}

