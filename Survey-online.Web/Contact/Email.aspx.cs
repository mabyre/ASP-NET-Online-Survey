//
// Dans un Button si un message de Validation doit etre envoye vers le client il faut
// imperativement faire un Reponse.Redirect()
// Houlalala difficile de faire une image et alors ya tous ces evenements a gerer
// et puis le DataBind() a disparu finalement !!?
//
using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using Sql.Web.Data;
using ImportExportContact;

public partial class Contact_Email : PageBase
{
    static int columnEnvoyer = 0;
    static int columnNumero = 3;
    static int columnEmailEnvois = 4;
    static int columnVotes = 5;
    static int columnCreationDate = 13;
    static int columnEmail = 14;
    static int columnEdit = 15;

    //BUG10092009
    private int CurrentPageIndex
    {
        get 
        { 
            if ( ViewState[ "CurrentPageIndex" ] == null )
            {
                ViewState[ "CurrentPageIndex" ] = 0;
            }
            return ( int )ViewState[ "CurrentPageIndex" ]; 
        }
        set { ViewState[ "CurrentPageIndex" ] = value; }
    }

    private static PollVoteCollection Votes
    {
        get
        {
            // Differencier de SessionState.Votes
            if ( HttpContext.Current.Session[ "_Votes" ] != null )
                return ( PollVoteCollection )( HttpContext.Current.Session[ "_Votes" ] );

            return null;
        }

        set
        {
            HttpContext.Current.Session[ "_Votes" ] = value;
        }
    }

    private bool EmailAssynchrone
    {
        get
        {
            if ( Session[ "EmailAssynchrone" ] == null )
            {
                Session[ "EmailAssynchrone" ] = false;
            }
            return ( bool )Session[ "EmailAssynchrone" ];
        }
        set
        {
            Session[ "EmailAssynchrone" ] = value;
        }
    }

    protected override void OnPreInit( EventArgs e )
    {
        base.OnPreInit( e );

        // Formulaire en mode Impression
        // AME15112009
        if ( Request.QueryString[ "print" ] != null || Request.QueryString[ "excel" ] != null )
        {
            // MasterPageFile ne peut etre modifiee que dans OnPreInit()
            Page.MasterPageFile = "~/Print.Master";

            if ( Request.QueryString[ "excel" ] != null )
            {
                // AME15112009
                // La propriete Theme ne peut etre modifiee que dans OnPreInit()
                // On cherche a annuler le theme sinon les feuilles de styles sont presentes
                // dans le document telecharge par le client et lorsqu'il tente d'ouvir
                // son document avec Excel ce con d'Excel ne les trouves pas evidemment car elles
                // sont sur le serveur !!
                Page.Theme = "";
            }
        }
    }

    protected void Page_Load( object sender, System.EventArgs e )
    {
        Trace.Warn( "Page_Load:Begin" );

        if ( Request.QueryString[ "tel" ] != null )
        {
            LabelTitre.Text = "Réponses des Interviewés";
            GridViewContacts.Columns[ columnEnvoyer ].Visible = false;
            GridViewContacts.Columns[ columnEmailEnvois ].Visible = false;
            GridViewContacts.Columns[ columnEmail ].Visible = false;
            ButtonEnvoyer.Visible = false; // On n'evoit pas d'email
        }

        // Choisir le premier Questionnaire a la place de l'utilisateur
        if ( IsPostBack == false )
        {
            if ( SessionState.Questionnaire == null && SessionState.Questionnaires.Count > 0 )
            {
                SessionState.Questionnaire = SessionState.Questionnaires[ 0 ];
            }
        }

        if ( IsPostBack == false )
        {
            if ( User.IsInRole( "Administrateur" ) )
            {
                ButtonEnvoyer.Visible = false; // L'Admin n'envoye pas d'email
            }

            if ( SessionState.Questionnaire != null )
            {
                Votes = PollVoteCollection.GetPollVotesByQuestionnaireID( SessionState.Questionnaire.QuestionnaireID );
                CalculerVotants();
            }

            /* L'alphabet */
            DropDownListLettre.Items.Add( "---" );
            string _alpha = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] alpha = _alpha.Split( ',' );
            foreach ( string s in alpha )
            {
                DropDownListLettre.Items.Add( s );
            }

            CheckBoxDateVotes.Checked = SessionState.CheckBox[ "CheckBoxDate" ];
            GridViewContacts.Columns[ columnCreationDate ].Visible = CheckBoxDateVotes.Checked;

            GridViewContacts.PageSize = SessionState.ContactsParPage;
            GridViewContacts.PageIndex = CurrentPageIndex;
            TextBoxContactsParPage.Text = SessionState.ContactsParPage.ToString();

            CheckBoxEmailAssynchrone.Checked = EmailAssynchrone;

            SqlDataSourcePersonne.SelectCommand = SelectCommand();
        }

        FormulaireEnModeImpression();

        MessageValider();
        Trace.Warn( "Page_Load:End" );
    }

    private void FormulaireEnModeImpression()
    {
        if ( Request.QueryString[ "print" ] != null || Request.QueryString[ "excel" ] != null )
        {
            GridViewContacts.Columns[ columnEnvoyer ].Visible = false;
            GridViewContacts.Columns[ columnNumero ].Visible = false;
            GridViewContacts.Columns[ columnEmailEnvois ].Visible = false;
            //GridViewContacts.Columns[ columnVotes ].Visible = CheckBoxVotes.Checked;
            GridViewContacts.Columns[ columnEmail ].Visible = false;
            GridViewContacts.Columns[ columnEdit ].Visible = false;

            PanelNonImprime.Visible = false;
            PanelNonImprime2.Visible = false;
            TrBouton.Visible = false;
            TableBoutonPageHaut.Visible = false;
        }
        if ( Request.QueryString[ "excel" ] != null )
        {
            Response.ContentType = "application/vnd.ms-excel"; // Set the content type to Excel
            Response.Charset = ""; // Remove the charset from the Content-Type header
            Page.EnableViewState = false; // Sinon Excel de merde ne sait pas lire le fichier genere !!!
            UpdateProgress2.Visible = false; // incroyable mais ce con d'excel voit l'UpdatePanel
            UpdateProgress1.Visible = false;
        }
    }

    void MessageValider()
    {
        if ( SessionState.ValidationMessage != null )
        {
            ValidationMessage.Text = SessionState.ValidationMessage;
            ValidationMessage.Visible = true;
            SessionState.ValidationMessage = null;
        }
        else
        {
            ValidationMessage.Visible = false;
        }
    }

    string SelectCommand()
    {
        if ( SessionState.Questionnaires.Count <= 0 )
        {
            return "";
        }

        string sqlCommand = "SELECT [ID_Personne], PersonneGUID, PersonneEmailEnvois, [PersonneCivilite], [PersonneNom], [PersonnePrenom], [PersonneEmailBureau], [PersonneTelephonePerso], [PersonneSociete], [PersonneCode], PersonneGUID FROM [Personne]";
        sqlCommand += " WHERE QuestionnaireID = '" + DropDownListQuestionnaire.QuestionnaireID + "'";
        if ( DropDownListLettre.SelectedValue != "---" )
        {
            sqlCommand += " AND UPPER(PersonneNom) LIKE '" + DropDownListLettre.SelectedValue + "%'";
        }
        return sqlCommand;
    }

    void CalculerVotants()
    {
        PersonneCollection pc = PersonneCollection.GetQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
        LabelNombreContacts.Text = pc.Count.ToString();
        int nbVotant = 0;
        foreach ( Personne p in pc )
        {
            //PollVoteCollection pvc = PollVoteCollection.GetPollVotes( SessionState.Questionnaire.QuestionnaireID, p.PersonneGUID );
            PollVoteCollection pvc = Votes.FindByPersonneGUID( p.PersonneGUID );

            // A t il vote pour ce questionnaire ?
            if ( pvc.Count > 0 )
            {
                nbVotant = nbVotant + 1;
            }
        }
        LabelVotes.Text = nbVotant.ToString();
    }

    protected void ImageButtonPrint_Click( object sender, ImageClickEventArgs e )
    {
        if ( Request.RawUrl.Contains( "?" ) )
        {
            Response.Redirect( Request.RawUrl + "&print=1" );
        }
        else
        {
            Response.Redirect( Request.RawUrl + "?print=1" );
        }
    }

    protected void ImageButtonExcel_Click( object sender, ImageClickEventArgs e )
    {
        if ( Request.RawUrl.Contains( "?" ) )
        {
            Response.Redirect( Request.RawUrl + "&excel=1" );
        }
        else
        {
            Response.Redirect( Request.RawUrl + "?excel=1" );
        }
    }


    protected void DropDownListQuestionnaire_SelectedIndexChanged( object sender, EventArgs e )
    {
        SessionState.Questionnaire = Questionnaire.GetQuestionnaire( DropDownListQuestionnaire.QuestionnaireID );
        Votes = PollVoteCollection.GetPollVotesByQuestionnaireID( SessionState.Questionnaire.QuestionnaireID );
        CalculerVotants();
        BloquerQuestionnaire( SessionState.Questionnaire.Bloque );
        SqlDataSourcePersonne.SelectCommand = SelectCommand();
    }

    protected void DropDownListLettre_SelectedIndexChanged( object sender, EventArgs e )
    {
        SqlDataSourcePersonne.SelectCommand = SelectCommand();
    }

    protected void CheckBoxTousSelectionner_CheckedChanged( object sender, EventArgs e )
    {
        int indexRow = 0;
        if ( CheckBoxTousSelectionner.Checked )
        {
            foreach ( GridViewRow r in GridViewContacts.Rows )
            {
                CheckBox cb = ( CheckBox )GridViewContacts.Rows[ indexRow++ ].FindControl( "CheckBoxEnvoyerEmail" );
                cb.Checked = true;
            }
        }
        else
        {
            foreach ( GridViewRow r in GridViewContacts.Rows )
            {
                CheckBox cb = ( CheckBox )GridViewContacts.Rows[ indexRow++ ].FindControl( "CheckBoxEnvoyerEmail" );
                cb.Checked = false;
            }
        }
    }

    protected void CheckBoxNonVotantsSelectionner_CheckedChanged( object sender, EventArgs e )
    {
        int indexRow = 0;
        foreach ( GridViewRow r in GridViewContacts.Rows )
        {
            CheckBox cb = ( CheckBox )GridViewContacts.Rows[ indexRow ].FindControl( "CheckBoxEnvoyerEmail" );
            Image vote = ( Image )GridViewContacts.Rows[ indexRow ].FindControl( "ImageVote" );
            if ( vote.ImageUrl.Contains( "oui" ) )
            {
                cb.Checked = false;
            }
            else
            {
                cb.Checked = true;
            }
            indexRow++;
        }
        CheckBoxVotantsSelectionner.Checked = false;
    }

    protected void CheckBoxVotantsSelectionner_CheckedChanged( object sender, EventArgs e )
    {
        int indexRow = 0;
        foreach ( GridViewRow r in GridViewContacts.Rows )
        {
            CheckBox cb = ( CheckBox )GridViewContacts.Rows[ indexRow ].FindControl( "CheckBoxEnvoyerEmail" );
            Image vote = ( Image )GridViewContacts.Rows[ indexRow ].FindControl( "ImageVote" );
            if ( vote.ImageUrl.Contains( "oui" ) )
            {
                cb.Checked = true;
            }
            else
            {
                cb.Checked = false;
            }
            indexRow++;
        }
        CheckBoxNonVotantsSelectionner.Checked = false;
    }

    protected void CheckBoxInverser_CheckedChanged( object sender, EventArgs e )
    {
        int indexRow = 0;
        foreach ( GridViewRow r in GridViewContacts.Rows )
        {
            CheckBox cb = ( CheckBox )GridViewContacts.Rows[ indexRow++ ].FindControl( "CheckBoxEnvoyerEmail" );
            cb.Checked = !cb.Checked;// == true ? cb.Checked = false : cb.Checked = true;
        }
    }

    private void BloquerQuestionnaire( bool bloque )
    {
        if ( bloque )
        {
            Tools.PageValidation( "Le questionnaire \"" + SessionState.Questionnaire.Description + "\" est clôturé." );
        }
    }

    protected void ButtonEnvoyer_Click( object sender, System.EventArgs e )
    {
        if ( User.IsInRole( "Administrateur" ) )
        {
            SessionState.ValidationMessage = "L'admin ne peut pas envoyer d'email.";
            Response.Redirect( Request.RawUrl );
        }

        BloquerQuestionnaire( SessionState.Questionnaire.Bloque );

        int indexRow = 0;
        foreach ( GridViewRow r in GridViewContacts.Rows )
        {
            CheckBox cb = ( CheckBox )GridViewContacts.Rows[ indexRow ].FindControl( "CheckBoxEnvoyerEmail" );
            if ( cb.Checked )
            {
                Label civilite = ( Label )GridViewContacts.Rows[ indexRow ].FindControl( "LabelCivilite" );
                Label nom = ( Label )GridViewContacts.Rows[ indexRow ].FindControl( "LabelNom" );
                Label prenom = ( Label )GridViewContacts.Rows[ indexRow ].FindControl( "LabelPrenom" );
                Label email = ( Label )GridViewContacts.Rows[ indexRow ].FindControl( "LabelEmail" );
                Label societe = ( Label )GridViewContacts.Rows[ indexRow ].FindControl( "LabelSociete" );
                Label code = ( Label )GridViewContacts.Rows[ indexRow ].FindControl( "LabelCode" );
                Label personneGUID = ( Label )GridViewContacts.Rows[ indexRow ].FindControl( "LabelPersonneGUID" );

                // Si email est vide c'est un interviewe par telephone
                if ( email.Text.Trim() != string.Empty )
                {
                    MailAddress adresse = new MailAddress( email.Text, prenom.Text + " " + nom.Text );
                    if ( CheckBoxEmailAssynchrone.Checked )
                    {
                        //EmailAssynchrone = true;
                        //SessionState.ValidationMessage += Courriel.EnvoyerEmailQuestionnaireAssynchrone( adresse, civilite.Text, nom.Text, prenom.Text, code.Text ) + "<br />";
                        //Courriel.EnvoyerEmailQuestionnaireAssynchrone( adresse, civilite.Text, nom.Text, prenom.Text, code.Text, SessionState.Questionnaire.Description, personneGUID );
                        //SessionState.ValidationMessage += "message envoyé";
                    }
                    else
                    {
                        EmailAssynchrone = false;
                        SessionState.ValidationMessage += Courriel.EnvoyerEmailQuestionnaire( adresse, civilite.Text, nom.Text, prenom.Text, societe.Text, code.Text, SessionState.Questionnaire.Description, personneGUID.Text ) + "<br />";
                    }

                    if ( SessionState.ValidationMessage.Contains( "message envoyé" ) )
                    {
                        // Ajouter 1 aux envois d'emails
                        Guid _personneGUID = new Guid( personneGUID.Text );
                        Personne.UpdateEmailEnvois( _personneGUID );

                        // Mettre a jour l'interface graphique pour ne pas avoir a recharger la page entiere
                        Label emailEnvois = ( Label )GridViewContacts.Rows[ indexRow ].FindControl( "LabelEmailEnvois" );
                        emailEnvois.CssClass = "LabelListRedStyle";
                        int cptEnvois = int.Parse( emailEnvois.Text );
                        cptEnvois += 1;
                        emailEnvois.Text = cptEnvois.ToString();
                        emailEnvois.Visible = true;
                        cb.Checked = false;
                    }
                }
                else
                {
                    cb.Checked = false;
                }
            }
            indexRow += 1;
        }

        MessageValider();
    }

    // Il faut laisser cet evenement sinon on a une erreur evenement non gere !
    protected void GridViewContacts_OnSorting( object sender, GridViewSortEventArgs e )
    {
    }

    protected void GridViewContacts_PageIndexChanged( object sender, System.EventArgs e )
    {
        CheckBoxTousSelectionner.Checked = false;

        CurrentPageIndex = GridViewContacts.PageIndex;
        SqlDataSourcePersonne.SelectCommand = SelectCommand();
    }

    protected void GridViewContacts_OnDataBound( object sender, System.EventArgs e )
    {
    }

    protected void GridViewContacts_OnSorted( object sender, EventArgs e )
    {
        GridViewContacts.PageIndex = CurrentPageIndex;
        SqlDataSourcePersonne.SelectCommand = SelectCommand();
    }

    protected void GridViewContacts_RowCreated( object sender, System.Web.UI.WebControls.GridViewRowEventArgs e )
    {
        if ( e.Row.RowType == DataControlRowType.Header )
        {
            AddGlyph( GridViewContacts, e.Row );
        }
    }


    protected void AddGlyph( GridView grid, GridViewRow item )
    {
        Image glyph = new Image();
        glyph.EnableTheming = false;
        glyph.ImageAlign = ImageAlign.Bottom;
        glyph.ImageUrl = string.Format( "~/App_Themes/" + Page.Theme.ToString() + "/images/move{0}.gif", ( string )( grid.SortDirection == SortDirection.Ascending ? "down" : "up" ) );

        int i;
        string colExpr;
        for ( i = 0;i <= grid.Columns.Count - 1;i++ )
        {
            colExpr = grid.Columns[ i ].SortExpression;
            if ( colExpr != "" && colExpr == grid.SortExpression )
            {
                item.Cells[ i ].Controls.Add( glyph );
            }
        }
    }

    protected void ComputeVoteColumn()
    {
        if ( SessionState.Questionnaire != null )
        {
            if ( GridViewContacts.Rows.Count > 0 )
            {
                int indexRow = GridViewContacts.Rows.Count - 1;
                Image vote = ( Image )GridViewContacts.Rows[ indexRow ].FindControl( "ImageVote" );
                vote.EnableViewState = true;
                Label personneID = ( Label )GridViewContacts.Rows[ indexRow ].FindControl( "LabelPersonneGUID" );
                Guid personneGUID = new Guid( personneID.Text );

                Label labelVote = new Label();
                if ( Request.QueryString[ "print" ] != null || Request.QueryString[ "excel" ] != null )
                {
                    vote.Visible = false;
                    labelVote = ( Label )GridViewContacts.Rows[ indexRow ].FindControl( "LabelVote" );
                }

                // Optimiser 
                if ( CheckBoxDateVotes.Checked )
                {
                    DateTime dateVote = new DateTime();
                    bool aVote = Votes.ADejaVote( personneGUID, ref dateVote );
                    vote.ImageUrl = string.Format( "~/App_Themes/" + Page.Theme.ToString() + "/Images/vote_{0}.gif", aVote == true ? "oui" : "non" );

                    // Date du vote
                    if ( aVote )
                    {
                        Label date = ( Label )GridViewContacts.Rows[ indexRow ].FindControl( "LabelDateVotes" );
                        date.Text = dateVote.ToShortDateString();
                        date.ToolTip = dateVote.ToShortTimeString();
                        labelVote.Visible = true;
                    }
                }
                else
                {
                    bool aVote = Votes.ADejaVote( personneGUID );
                    vote.ImageUrl = string.Format( "~/App_Themes/" + Page.Theme.ToString() + "/Images/vote_{0}.gif", aVote == true ? "oui" : "non" );

                    if ( aVote )
                    {
                        labelVote.Visible = true;
                    }
                }

                // Colonne numero
                Label numero = ( Label )GridViewContacts.Rows[ indexRow ].FindControl( "LabelNumero" );
                int num = GridViewContacts.Rows.Count + ( GridViewContacts.PageIndex * GridViewContacts.PageSize );
                numero.Text = num.ToString();
            }
        }
    }

    protected void TextBoxContactsParPage_TextChanged( object sender, EventArgs e )
    {
        // BUG10092009 corrige dans .aspx CausesValidation="false"
        int contactsParPage = int.Parse( Global.SettingsXml.ContactsParPageCourant );
        int max = int.Parse( Global.SettingsXml.ContactsParPageMax );
        int min = int.Parse( Global.SettingsXml.ContactsParPageMin );
        try
        {
            contactsParPage = int.Parse( TextBoxContactsParPage.Text );
            if ( contactsParPage >= min && contactsParPage <= max )
            {
                SessionState.ContactsParPage = contactsParPage;
            }
            else if ( contactsParPage < min )
            {
                SessionState.ContactsParPage = min;
            }
            else if ( contactsParPage > max )
            {
                SessionState.ContactsParPage = max;
            }
            GridViewContacts.PageSize = SessionState.ContactsParPage;
            SqlDataSourcePersonne.SelectCommand = SelectCommand();
        }
        catch
        {
            GridViewContacts.PageSize = SessionState.ContactsParPage;
        }
        CheckBoxTousSelectionner.Checked = false;
        TextBoxContactsParPage.Text = SessionState.ContactsParPage.ToString();
    }

    protected void ButtonSupprimer_Click( object sender, EventArgs e )
    {
        int indexRow = 0;
        int nbContacts = 0;
        foreach ( GridViewRow r in GridViewContacts.Rows )
        {
            CheckBox cb = ( CheckBox )GridViewContacts.Rows[ indexRow ].FindControl( "CheckBoxEnvoyerEmail" );
            if ( cb.Checked )
            {
                Label personneID = ( Label )GridViewContacts.Rows[ indexRow ].FindControl( "LabelPersonneID" );
                int pID = int.Parse( personneID.Text );
                Personne personne = Personne.Get( pID );
                PollVoteCollection pvc = PollVoteCollection.GetPollVotes( SessionState.Questionnaire.QuestionnaireID, personne.PersonneGUID );
                int status = 0;
                foreach ( PollVote pv in pvc )
                {
                    status += PollVote.Delete( pv.VoteId );
                }
                string str = " votes supprimés : " + pvc.Count;
                status += Personne.Delete( pID );
                string statusString = status.ToString() == "0" ? " status : Ok" : " status  : Ko";
                SessionState.ValidationMessage += personne.Civilite + "/" + personne.Nom + "/" + personne.Prenom + "/" + personne.EmailBureau + str + statusString + "<br />";
                nbContacts += 1;
            }
            indexRow += 1;
        }

        SessionState.ValidationMessage += nbContacts.ToString() + " contacts supprimés.";
        SessionState.Limitations.SupprimerInterviewes( nbContacts );
        Response.Redirect( Request.RawUrl );
    }

    protected void GridViewContacts_RowUpdated( object sender, GridViewUpdatedEventArgs e )
    {
    }
    protected void GridViewContacts_RowUpdating( object sender, GridViewUpdateEventArgs e )
    {
        ComputeVoteColumn(); 
    }
    protected void GridViewContacts_RowDataBound( object sender, GridViewRowEventArgs e )
    {
        ComputeVoteColumn();
    }
    protected void GridViewContacts_Load( object sender, EventArgs e )
    {
        ComputeVoteColumn();
    }

    protected void CheckBoxDateVotes_CheckedChanged( object sender, EventArgs e )
    {
        SessionState.CheckBox[ "CheckBoxDate" ] = CheckBoxDateVotes.Checked;
        GridViewContacts.Columns[ columnCreationDate ].Visible = CheckBoxDateVotes.Checked;
        CheckBoxTousSelectionner.Checked = false;

        // BUG100920090002
        // si on execute pas le code ci-dessous on se retrouve avec tous les contacts de tous le monde
        // dans la grille ... ?!! 
        SqlDataSourcePersonne.SelectCommand = SelectCommand();
        GridViewContacts.DataBind();
    }
}


