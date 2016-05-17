//
// On ne peut pas mettre d'attibut static a un ViewState
// On utilise alors HttpContext.Current.Session pour les variables qui ont besoin d'etre static
// On affuble les varaibles d'un prefixe QuestionnaireStatAll_ pour ne pas les confondre avec les
// variables de SessionState
//
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
using System.Text;

public partial class Poll_QuestionnaireStatAll : PageBase
{
    // Synchroniser ListBoxQui et DropDownListQui
    // BUG10092009 ENORME si deux utilisateurs sont en meme temps sur ce formulaire !
    //private static PersonneCollection PersonnesDropDownListQui = new PersonneCollection();
    //private static int DropDownListQuiSelectedIndex = 0;

    //private static PersonneCollection Personnes = new PersonneCollection(); // Les personnes qui ont repondu au questionnaire
    //private static PollQuestionCollection Questions = new PollQuestionCollection(); // Cumuler les questions cliquees par l'utilisateur
    //private static PollAnswerCollection Reponses = new PollAnswerCollection(); // Cumuler les reponses cliquees par l'utilisateur
    //private static ArrayList PollAnswerID = new ArrayList(); // Cumuler les ID des reponses cliquees par l'utilisateur
    //private static PersonneCollection PersonnesOntReponduATout = new PersonneCollection();

    // Synchroniser ListBoxQui et DropDownListQui
    private static PersonneCollection PersonnesDropDownListQui
    {
        get
        {
            if ( HttpContext.Current.Session[ "QuestionnaireStatAll_PersonnesDropDownListQui" ] == null )
            {
                HttpContext.Current.Session[ "QuestionnaireStatAll_PersonnesDropDownListQui" ] = new PersonneCollection();
            }
            return ( PersonneCollection )HttpContext.Current.Session[ "QuestionnaireStatAll_PersonnesDropDownListQui" ];
        }
        set { HttpContext.Current.Session[ "QuestionnaireStatAll_PersonnesDropDownListQui" ] = value; }
    }

    private static int DropDownListQuiSelectedIndex
    {
        get
        {
            if ( HttpContext.Current.Session[ "QuestionnaireStatAll_DropDownListQuiSelectedIndex" ] == null )
            {
                HttpContext.Current.Session[ "QuestionnaireStatAll_DropDownListQuiSelectedIndex" ] = 0;
            }
            return ( int )HttpContext.Current.Session[ "QuestionnaireStatAll_DropDownListQuiSelectedIndex" ];
        }
        set { HttpContext.Current.Session[ "QuestionnaireStatAll_DropDownListQuiSelectedIndex" ] = value; }
    }

    // Les personnes qui ont repondu au questionnaire
    private static PersonneCollection Personnes
    {
        get
        {
            if ( HttpContext.Current.Session[ "QuestionnaireStatAll_Personnes" ] == null )
            {
                HttpContext.Current.Session[ "QuestionnaireStatAll_Personnes" ] = new PersonneCollection();
            }
            return ( PersonneCollection )HttpContext.Current.Session[ "QuestionnaireStatAll_Personnes" ];
        }
        set { HttpContext.Current.Session[ "QuestionnaireStatAll_Personnes" ] = value; }
    }

    // Initialiser le Tableau des votes des Personnes OPT17072010
    private void initTableauVotesPersonnes( int nbPersonnes, PersonneCollection personnes )
    {
        int indexPollVotes = 0;
        int nbVotes = 0;
        PollVoteCollection[] tableauPollVotes = new PollVoteCollection[ nbPersonnes ];
        foreach ( Personne p in personnes )
        {
            tableauPollVotes[ indexPollVotes ] = Votes.FindByPersonneGUID( p.PersonneGUID );
            nbVotes += tableauPollVotes[ indexPollVotes ].Count;
            indexPollVotes += 1;
        }
        TableauVotesPersonnes = tableauPollVotes;
        NombreVotesPersonnes = nbVotes;
    }

    // Tableau des votes des Personnes OPT17072010
    private static PollVoteCollection[] TableauVotesPersonnes
    {
        get
        {
            return ( PollVoteCollection[] )HttpContext.Current.Session[ "TableauVotesPersonnes" ];
        }
        set { HttpContext.Current.Session[ "TableauVotesPersonnes" ] = value; }
    }

    // Nombre de votes des Personnes OPT17072010
    private static int NombreVotesPersonnes
    {
        get
        {
            return ( int )HttpContext.Current.Session[ "NombreVotesPersonnes" ];
        }
        set { HttpContext.Current.Session[ "NombreVotesPersonnes" ] = value; }
    }

    // Cumuler les questions cliquees par l'utilisateur
    private static PollQuestionCollection Questions
    {
        get
        {
            if ( HttpContext.Current.Session[ "QuestionnaireStatAll_Questions" ] == null )
            {
                HttpContext.Current.Session[ "QuestionnaireStatAll_Questions" ] = new PollQuestionCollection();
            }
            return ( PollQuestionCollection )HttpContext.Current.Session[ "QuestionnaireStatAll_Questions" ];
        }
        set { HttpContext.Current.Session[ "QuestionnaireStatAll_Questions" ] = value; }
    }

    // AME20100331 Cumuler les questions cliquees par l'utilisateur pour l'affichage des VotesEnBas
    private static PollQuestionCollection QuestionsVotesEnBas
    {
        get
        {
            if ( HttpContext.Current.Session[ "QuestionnaireStatAll_QuestionsVotesEnBas" ] == null )
            {
                HttpContext.Current.Session[ "QuestionnaireStatAll_QuestionsVotesEnBas" ] = new PollQuestionCollection();
            }
            return ( PollQuestionCollection )HttpContext.Current.Session[ "QuestionnaireStatAll_QuestionsVotesEnBas" ];
        }
        set { HttpContext.Current.Session[ "QuestionnaireStatAll_QuestionsVotesEnBas" ] = value; }
    }

    // Cumuler les reponses cliquees par l'utilisateur
    private static PollAnswerCollection Reponses
    {
        get
        {
            if ( HttpContext.Current.Session[ "QuestionnaireStatAll_Reponses" ] == null )
            {
                HttpContext.Current.Session[ "QuestionnaireStatAll_Reponses" ] = new PollAnswerCollection();
            }
            return ( PollAnswerCollection )HttpContext.Current.Session[ "QuestionnaireStatAll_Reponses" ];
        }
        set { HttpContext.Current.Session[ "QuestionnaireStatAll_Reponses" ] = value; }
    }

    // Cumuler les ID des reponses cliquees par l'utilisateur
    private static ArrayList PollAnswerID
    {
        get
        {
            if ( HttpContext.Current.Session[ "QuestionnaireStatAll_PollAnswerID" ] == null )
            {
                HttpContext.Current.Session[ "QuestionnaireStatAll_PollAnswerID" ] = new ArrayList();
            }
            return ( ArrayList )HttpContext.Current.Session[ "QuestionnaireStatAll_PollAnswerID" ];
        }
        set { HttpContext.Current.Session[ "QuestionnaireStatAll_PollAnswerID" ] = value; }
    }

    private static PersonneCollection PersonnesOntReponduATout
    {
        get
        {
            if ( HttpContext.Current.Session[ "QuestionnaireStatAll_PersonnesOntReponduATout" ] == null )
            {
                HttpContext.Current.Session[ "QuestionnaireStatAll_PersonnesOntReponduATout" ] = new PersonneCollection();
            }
            return ( PersonneCollection )HttpContext.Current.Session[ "QuestionnaireStatAll_PersonnesOntReponduATout" ];
        }
        set { HttpContext.Current.Session[ "QuestionnaireStatAll_PersonnesOntReponduATout" ] = value; }
    }

    // Initialiser le Tableau des votes des Personnes OPT17072010
    private void initTableauVotesPersonnesOntReponduATout( int nbPersonnes, PersonneCollection personnes )
    {
        int indexPollVotes = 0;
        int nbVotes = 0;
        PollVoteCollection[] tableauPollVotes = new PollVoteCollection[ nbPersonnes ];
        foreach ( Personne p in personnes )
        {
            tableauPollVotes[ indexPollVotes ] = Votes.FindByPersonneGUID( p.PersonneGUID );
            nbVotes += tableauPollVotes[ indexPollVotes ].Count;
            indexPollVotes += 1;
        }
        TableauVotesPersonnesOntReponduATout = tableauPollVotes;
        NombreVotesPersonnesOntReponduATout = nbVotes;
    }

    // Tableau des votes des Personnes OPT17072010
    private static PollVoteCollection[] TableauVotesPersonnesOntReponduATout
    {
        get
        {
            return ( PollVoteCollection[] )HttpContext.Current.Session[ "TableauVotesPersonnesOntReponduATout" ];
        }
        set { HttpContext.Current.Session[ "TableauVotesPersonnesOntReponduATout" ] = value; }
    }

    // Nombre de votes des Personnes OPT17072010
    private static int NombreVotesPersonnesOntReponduATout
    {
        get
        {
            return ( int )HttpContext.Current.Session[ "NombreVotesPersonnesOntReponduATout" ];
        }
        set { HttpContext.Current.Session[ "NombreVotesPersonnesOntReponduATout" ] = value; }
    }

    // Memoriser les tous les Votes du Questionnaire Courant - Optimisation
    private PollVoteCollection Votes
    {
        get
        {
            if ( ViewState[ "QuestionnaireStatAll_Votes" ] != null )
                return ( PollVoteCollection )( ViewState[ "QuestionnaireStatAll_Votes" ] );

            return null;
        }

        set
        {
            ViewState[ "QuestionnaireStatAll_Votes" ] = value;
        }
    }

    private bool AfficherTousLesVotes
    {
        get
        {
            if ( Session[ "AfficherTousLesVotes" ] != null )
                return ( bool )( Session[ "AfficherTousLesVotes" ] );

            return false;
        }

        set
        {
            Session[ "AfficherTousLesVotes" ] = value;
        }
    }


    protected override void OnPreInit( EventArgs e )
    {
        base.OnPreInit( e );

        if ( Request.QueryString[ "print" ] != null || Request.QueryString[ "excel" ] != null )
        {
            // MasterPageFile ne peut etre modifiee que dans OnPreInit()
            Page.MasterPageFile = "~/Print.Master";

            if ( Request.QueryString[ "excel" ] != null )
            {
                // AME14112009
                // La propriete Theme ne peut etre modifiee que dans OnPreInit()
                // On cherche a annuler le theme sinon les feuilles de styles sont presentes
                // dans le document telecharge par le client et lorsqu'il tente d'ouvir
                // son document avec Excel ce con d'Excel ne les trouves pas evidemment car elles
                // sont sur le serveur !!
                Page.Theme = "";
            }
        }
    }

    // Formulaire en mode excel
    public bool FormulaireEnModeExcel
    {
        get
        {
            if ( Request.QueryString[ "excel" ] != null )
            {
                return true;
            }
            return false;
        }
    }

    private string FormatPersonne( Personne personne )
    {
        string format = personne.Nom;

        if ( format == string.Empty )
        {
            format += personne.Prenom;
        }
        else if (  personne.Prenom != string .Empty )
        {
            format += "/" + personne.Prenom;
        }

        if ( format == string.Empty )
        {
            format += personne.EmailBureau;
        }
        else if ( personne.EmailBureau != string.Empty )
        {
            format += "/" + personne.EmailBureau;
        }

        if ( format == string.Empty )
        {
            format += personne.TelephonePerso;
        }
        else if ( personne.TelephonePerso != string.Empty )
        {
            format += "/" + personne.TelephonePerso;
        }

        if ( format == string.Empty )
        {
            format += personne.Civilite;
        }
        else if ( personne.Civilite != string.Empty )
        {
            format += "/" + personne.Civilite;
        }

        return format;
    }

    protected void Page_Load( object sender, EventArgs e )
    {
        if ( IsPostBack == false )
        {
            // Formulaire en mode Impression
            // AME14112009
            if ( Request.QueryString[ "print" ] != null || Request.QueryString[ "excel" ] != null )
            {
                ImageButtonPrint.Visible = false;
                PanelControlSats.Visible = false;
                PanelAide.Visible = false;
            }
            if ( Request.QueryString[ "print" ] != null )
            {
                QuestionnaireControlStatAll.ModePrint = true;
                TrBoutonRetour.Visible = true;
            }
            if ( Request.QueryString[ "excel" ] != null )
            {
                PanelBoutonControl.Visible = false;

                // En fait cela revient a changer l'extension html en .xls 
                // c'est totalement bidon !
                Response.ContentType = "application/vnd.ms-excel"; // Set the content type to Excel
                Response.Charset = ""; // Remove the charset from the Content-Type header
                Page.EnableViewState = false; // Sinon Excel de merde ne sait pas lire le fichier genere !!!
                // Il faut absolument cette meta dans la MasterPage Print sinon cela ne marche pas pour 
                // Excel d'ailleur en positionnant Charset ici on ne retrouve rien dans la source !...?
                // <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

                QuestionnaireControlStatAll.ModeExcel = true;
            }

            // Choisir le premier Questionnaire a la place de l'utilisateur
            if ( SessionState.Questionnaire == null && SessionState.Questionnaires.Count > 0 )
            {
                SessionState.Questionnaire = SessionState.Questionnaires[ 0 ];
            }

            if ( Request.QueryString[ "QuestionnaireID" ] != null )
            {
                int questionnaireID = int.Parse( Request.QueryString[ "QuestionnaireID" ] );
                SessionState.Questionnaire = SessionState.Questionnaires.FindByID( questionnaireID );
            }

            // Charger les votes pour ce Questionnaire
            if ( SessionState.Questionnaire != null && Votes == null )
            {
                Votes = PollVoteCollection.GetPollVotesByQuestionnaireID( SessionState.Questionnaire.QuestionnaireID );
                Personnes = PersonneCollection.GetQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
                initTableauVotesPersonnes( Personnes.Count, Personnes );
                LabelNombreContacts.Text = Personnes.Count.ToString();
            }

            if ( Request.QueryString[ "PollAnswerID" ] != null )
            {
                PanelSousPopulation.Visible = true;
                Guid pollAnswerID = new Guid( Request.QueryString[ "PollAnswerID" ].ToString() );

                // BUG25092009 Il ne faut ajouter 2 fois la meme reponse sinon il y a cumul
                // il faut eviter le cumul de PollAnswerID de Questions et de Reponses
                if ( PollAnswerID.Contains( pollAnswerID ) == false )
                {
                    PollAnswerID.Add( pollAnswerID );

                    // Trouver les votants
                    if ( Votes.Count > 0 )
                    {
                        // Comme on a que pollAnswerID a notre disposition, pour trouver la question il faut 
                        // trouver la Reponses de pollAnswerID
                        // trouver la Question de la Reponse
                        PollAnswer pollAnswer = PollAnswerCollection.GetByPollAnswerID( pollAnswerID );
                        PollQuestionCollection pollQuestionCollection = PollQuestionCollection.GetByQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
                        PollQuestion pollQuestion = pollQuestionCollection.FindByPollQuestionID( pollAnswer.PollQuestionId );

                        // Cumuler les Reponses cliquees par l'utilisateur et les Questions
                        Questions.Add( pollQuestion );
                        Reponses.Add( pollAnswer );

                        // BUB25092009 AfficherOnReponduATout();
                    }
                }

                // BUG25092009 il faut afficher tout ici
                if ( Votes.Count > 0 )
                {
                    AfficherOnReponduATout();
                }

                if ( ListBoxQui.Items.Count <= 0 )
                {
                    ListBoxQui.Items.Add( "Pas de vote" );
                    ListBoxQui.Rows = 1;

                    // On efface tout, sinon l'utilisateur est perdu 
                    // et les Questions et les reponses continuent de s'accumuler
                    // et l'utilisateur ne voit rien
                    EffacerSousPopulation();
                }
                else
                {
                    ListBoxQui.Rows = ListBoxQui.Items.Count <= 10 ? ListBoxQui.Items.Count : 10;
                    LabelListBoxQuiCount.Text = ListBoxQui.Items.Count.ToString();
                    PanelReponses.Visible = true;
                }
            }
            else // du if ( Request.QueryString[ "PollAnswerID" ] != null )
            {
                ListBoxQui.Items.Add( "Cliquez sur une réponse" );
                ListBoxQui.Rows = 1;
                EffacerSousPopulation();
            }

            if ( SessionState.Questionnaire != null )
            {
                QuestionnaireControlStatAll.SelectedQuestionnaireID = SessionState.Questionnaire.QuestionnaireID;
                PersonnesDropDownListQui.Clear();

                // Trouver les votants
                if ( Votes.Count > 0 )
                {
                    int idx = 0;
                    foreach ( Personne p in Personnes )
                    {
                        //PollVoteCollection pvc = PollVoteCollection.GetPollVotes( SessionState.Questionnaire.QuestionnaireID, p.PersonneGUID ); 
                        //PollVoteCollection pvc = Votes.FindByPersonneGUID( p.PersonneGUID );
                        PollVoteCollection pvc = TableauVotesPersonnes[ idx ];

                        // A t il vote pour ce questionnaire ?
                        if ( pvc.Count > 0 )
                        {
                            // Cacher les infos sur les votants
                            if ( SessionState.Questionnaire.Anonymat )
                            {
                                string personne = "personne" + idx.ToString();
                                DropDownListQui.Items.Add( personne );

                                Personne anonymat = new Personne();
                                anonymat = p;
                                anonymat.Nom = personne;
                                anonymat.Prenom = "";
                                anonymat.EmailBureau = "";
                                anonymat.Civilite = "";
                                anonymat.TelephonePerso = "";

                                PersonnesDropDownListQui.Add( anonymat );

                                // BUG07062010 je vois pas pourquoi !!!
                                //ListBoxQui.Enabled = false;
                            }
                            else
                            {
                                string personne = FormatPersonne( p );
                                DropDownListQui.Items.Add( personne );

                                PersonnesDropDownListQui.Add( p );
                            }
                        }

                        idx += 1;
                    }
                }

                LabelVotes.Text = PersonnesDropDownListQui.Count.ToString();

                QuestionnaireControlStatAll.PersonneVotants = PersonnesDropDownListQui;
                // Afficher les votes du premier votant
                if ( PersonnesDropDownListQui.Count >= 1 )
                {
                    if ( DropDownListQuiSelectedIndex != 0 )
                    {
                        DropDownListQui.SelectedIndex = DropDownListQuiSelectedIndex;
                    }
                    Personne personne = PersonnesDropDownListQui[ DropDownListQui.SelectedIndex ];
                    QuestionnaireControlStatAll.SelectedQuestionnaireID = SessionState.Questionnaire.QuestionnaireID;
                    QuestionnaireControlStatAll.SelectedPersonneGUID = personne.PersonneGUID;

                    AfficherVotantEnModePrint();
                }
            }

            CheckBoxAfficherReponseTextuelle.Checked = SessionState.CheckBox[ "CheckBoxAfficherReponseTextuelle" ];
            CheckBoxAfficherDateVote.Checked = SessionState.CheckBox[ "CheckBoxAfficherDateVote" ];
            CheckBoxAfficherMoyennePonderee.Checked = SessionState.CheckBox[ "CheckBoxAfficherMoyennePonderee" ];
        }
    }

    // C'est pour le mode print reafficher dans le nouveau formulaire
    protected override void CreateChildControls()
    {
        if ( AfficherTousLesVotes )
        {
            AfficherToutLesVotesEnBas( Guid.Empty );
        }
        else if ( Request.QueryString[ "PollQuestionID" ] != null )
        {
            // Clique sur une Question
            Guid pollQuestionID = new Guid( Request.QueryString[ "PollQuestionID" ].ToString() );
            AfficherToutLesVotesEnBas( pollQuestionID );
        }
    }

    void EffacerSousPopulation()
    {
        Questions.Clear();
        Reponses.Clear();
        PollAnswerID.Clear();
        PersonnesOntReponduATout.Clear();
        QuestionnaireControlStatAll.PersonnesSousPopulation = new PersonneCollection();
    }

    private void AfficherVotantEnModePrint()
    {
        if ( Request.QueryString[ "print" ] != null || Request.QueryString[ "excel" ] != null )
        {
            PanelAfficherVotantEnModePrint.Visible = true;
            PanelAfficherVotantEnModePrint.Controls.Clear();
            Table table = new Table();
            table.CssClass = "TableSousPopulationStyle";
            if ( DropDownListQui.SelectedIndex != -1 )
            {
                string msg = "A voté : " + DropDownListQui.SelectedItem.Value;
                Tables.CreateRowForLabel( ref table, msg, "TdSousPopulationStyle", "LabelSousPopulationStyle" );
            }
            PanelAfficherVotantEnModePrint.Controls.Add( table );
        }
    }

    void AfficherEnHautReponsesDeSousPopulation()
    {
        // Afficher toute les questions et toutes les reponses en haut du formulaire
        for ( int i = 0;i < Questions.Count;i++ )
        {
            Label labelQ = new Label();
            labelQ.CssClass = "LabelQuestionStyle";
            labelQ.Text = Questions[ i ].Rank.ToString() + " - " + Questions[ i ].Question;
            PanelReponses.Controls.Add( labelQ );

            Label labelBR = new Label();
            labelBR.Text = "<br/>";
            PanelReponses.Controls.Add( labelBR );

            Label labelR = new Label();
            labelR.CssClass = "HyperLinkQuestionEnCoursStyle";
            labelR.Text = Reponses[ i ].Rank.ToString() + " - " + Reponses[ i ].Answer;
            PanelReponses.Controls.Add( labelR );

            Label labelBR1 = new Label();
            labelBR1.Text = "<br/>";
            PanelReponses.Controls.Add( labelBR1 );
        }
    }

    void AfficherOnReponduATout()
    {
        if ( Page.IsPostBack == false )
        {
            ListBoxQui.Items.Clear();
            PersonnesOntReponduATout.Clear();
        }

        PanelReponses.Controls.Clear();
        PanelReponsesEnBas.Controls.Clear();

        AfficherVotantEnModePrint();

        AfficherEnHautReponsesDeSousPopulation();

        if ( PollAnswerID.Count > 0 )
        {
            // Afficher les questions et les reponses en bas du formulaire
            // Une seule reponse cliquee par l'utilisateur
            if ( PollAnswerID.Count == 1 )
            {
                // Afficher la seule Reponse et la seule Question
                Label labelQ = new Label();
                labelQ.CssClass = "LabelQuestionStyle";
                labelQ.Text = Questions[ 0 ].Rank.ToString() + " - " + Questions[ 0 ].Question;
                PanelReponsesEnBas.Controls.Add( labelQ );

                Label labelBR = new Label();
                labelBR.Text = "<br/>";
                PanelReponsesEnBas.Controls.Add( labelBR );

                Label labelR = new Label();
                labelR.CssClass = "HyperLinkQuestionEnCoursStyle";
                labelR.Text = Reponses[ 0 ].Rank.ToString() + " - " + Reponses[ 0 ].Answer;
                PanelReponsesEnBas.Controls.Add( labelR );

                Label labelBR1 = new Label();
                labelBR1.Text = "<br/>";
                PanelReponsesEnBas.Controls.Add( labelBR1 );

                Guid pollAnswerID = new Guid( PollAnswerID[ 0 ].ToString() );
                Table table = new Table();
                int idx = 0;
                foreach ( Personne p in Personnes )
                {
                    //PollVoteCollection pvc = Votes.FindByPersonneGUID( p.PersonneGUID );
                    PollVoteCollection pvc = TableauVotesPersonnes[ idx ].FindByAnswerID( pollAnswerID );
                    foreach ( PollVote pv in pvc )
                    {
                        string personne = FormatPersonne( p );
                        if ( Page.IsPostBack == false )
                        {
                            if ( SessionState.Questionnaire.Anonymat )
                            {
                                personne = "personne" + idx.ToString();

                                Personne anonymat = new Personne();
                                anonymat = p;
                                anonymat.Nom = personne;
                                anonymat.Prenom = "";
                                anonymat.EmailBureau = "";

                                ListBoxQui.Items.Add( personne );
                                PersonnesOntReponduATout.Add( anonymat );
                            }
                            else
                            {
                                ListBoxQui.Items.Add( personne );
                                PersonnesOntReponduATout.Add( p );
                            }
                        }

                        // Table de Reponses des Interviewes
                        TableCell cellP = new TableCell();
                        TableRow rowP = new TableRow();

                        if ( SessionState.CheckBox[ "CheckBoxAfficherDateVote" ] )
                        {
                            TableCell cellD = new TableCell();
                            cellD.Text = pv.CreationDate.ToString();
                            rowP.Cells.Add( cellD );
                        }

                        cellP.Text = Strings.TexteToHTML( personne );
                        cellP.CssClass = "TableReponsePersonneStyle";
                        rowP.Cells.Add( cellP );
                        table.Rows.Add( rowP );

                        // Pour les reponses textuelles
                        if ( pv.Vote != "" )
                        {
                            TableCell cellV = new TableCell();
                            TableRow rowV = new TableRow();
                            cellV.Text = Strings.TexteToHTML( pv.Vote );
                            //cell.CssClass = "TableReponsePersonneStyle";
                            rowV.Cells.Add( cellV );
                            table.Rows.Add( rowV );
                        }
                    }

                    idx += 1;
                }
                PanelReponsesEnBas.Controls.Add( table );
            }
            else // Reponse multiples cliquee par l'utilisateur
            {
                // Remplir d'abord ListBoxQui trouver au passage les personnes qui ont
                // repondu a tout
                int idx = 0;
                foreach ( Personne p in Personnes )
                {
                    //PollVoteCollection pvc = Votes.FindByPersonneGUID( p.PersonneGUID );
                    PollVoteCollection pvc = TableauVotesPersonnes[ idx ];
                    bool aReponduATout = true;
                    foreach ( Guid guid in PollAnswerID )
                    {
                        bool aRepondu = false;
                        foreach ( PollVote pv in pvc )
                        {
                            if ( guid == pv.PollAnswerId )
                            {
                                aRepondu = true;
                                break;
                            }
                        }
                        aReponduATout = aReponduATout & aRepondu;
                        if ( aRepondu == false )
                        {
                            break;
                        }
                    }

                    if ( aReponduATout )
                    {
                        string personne = FormatPersonne( p );
                        if ( Page.IsPostBack == false )
                        {
                            if ( SessionState.Questionnaire.Anonymat )
                            {
                                personne = "personne" + idx.ToString();

                                Personne anonymat = new Personne();
                                anonymat = p;
                                anonymat.Nom = personne;
                                anonymat.Prenom = "";
                                anonymat.EmailBureau = "";
                                anonymat.Civilite = "";
                                anonymat.TelephonePerso = "";

                                ListBoxQui.Items.Add( personne );
                                PersonnesOntReponduATout.Add( anonymat );
                            }
                            else
                            {
                                ListBoxQui.Items.Add( personne );
                                PersonnesOntReponduATout.Add( p );
                            }
                        }
                    }

                    idx += 1;
                }

                int i = 0;
                foreach ( Guid pollAnswerGUID in PollAnswerID )
                {
                    // Afficher la Reponse et le Question
                    Label labelQ = new Label();
                    labelQ.CssClass = "LabelQuestionStyle";
                    labelQ.Text = Questions[ i ].Rank.ToString() + " - " + Questions[ i ].Question;
                    PanelReponsesEnBas.Controls.Add( labelQ );

                    Label labelBR = new Label();
                    labelBR.Text = "<br/>";
                    PanelReponsesEnBas.Controls.Add( labelBR );

                    Label labelR = new Label();
                    labelR.CssClass = "HyperLinkQuestionEnCoursStyle";
                    labelR.Text = Reponses[ i ].Rank.ToString() + " - " + Reponses[ i ].Answer;
                    PanelReponsesEnBas.Controls.Add( labelR );

                    Label labelBR1 = new Label();
                    labelBR1.Text = "<br/>";
                    PanelReponsesEnBas.Controls.Add( labelBR1 );

                    i = i + 1;

                    Table table = new Table();
                    idx = 0;
                    foreach ( Personne p in PersonnesOntReponduATout )
                    {
                        //PollVoteCollection pvc = Votes.FindByPersonneGUID( p.PersonneGUID );
                        PollVoteCollection pvc = TableauVotesPersonnesOntReponduATout[ idx ].FindByAnswerID( pollAnswerGUID );
                        string personne = FormatPersonne( p );

                        foreach ( PollVote pv in pvc )
                        {
                            // Table de Reponses des Interviewes
                            TableCell cellP = new TableCell();
                            TableRow rowP = new TableRow();

                            if ( SessionState.CheckBox[ "CheckBoxAfficherDateVote" ] )
                            {
                                TableCell cellD = new TableCell();
                                cellD.Text = pv.CreationDate.ToString();
                                rowP.Cells.Add( cellD );
                            }

                            cellP.Text = Strings.TexteToHTML( personne );
                            cellP.CssClass = "TableReponsePersonneStyle";
                            rowP.Cells.Add( cellP );
                            table.Rows.Add( rowP );
                            // Pour les reponses textuelles
                            if ( pv.Vote != "" )
                            {
                                TableCell cellV = new TableCell();
                                TableRow rowV = new TableRow();
                                cellV.Text = Strings.TexteToHTML( pv.Vote );
                                //cell.CssClass = "TableReponsePersonneStyle";
                                rowV.Cells.Add( cellV );
                                table.Rows.Add( rowV );
                            }
                        }
                        idx += 1;
                    }

                    PanelReponsesEnBas.Controls.Add( table );
                }
            }

            Label labelCount = new Label();
            labelCount.Text = "Nombre de réponses : " + ListBoxQui.Items.Count.ToString();
            PanelReponsesEnBas.Controls.Add( labelCount );

            initTableauVotesPersonnesOntReponduATout( PersonnesOntReponduATout.Count, PersonnesOntReponduATout );
        }
    }

    void AfficherToutLesVotesEnBas( Guid pollQuestionID )
    {
        PanelReponsesEnBas.Controls.Clear();

        bool afficherDateVote = SessionState.CheckBox[ "CheckBoxAfficherDateVote" ];
        PollQuestionCollection pollQuestionCollection = PollQuestionCollection.GetByQuestionnaire( SessionState.Questionnaire.QuestionnaireID );

        // On cumul les Questions cliquees par l'utilisateur
        if ( pollQuestionID != Guid.Empty )
        {
            // La Collection se reduit a une seule Question
            PollQuestion pollQuestion = pollQuestionCollection.FindByPollQuestionID( pollQuestionID );

            // Cumuler les questions cliquees par l'utilisateur 
            // Sauf si elle est deja dans QuestionsVotesEnBas
            //   cela permet d'eviter de cumuler encore quand on passe aux formulaires print ou excel
            bool trouve = false;
            foreach ( PollQuestion q in QuestionsVotesEnBas )
            {
                if ( q.PollQuestionId == pollQuestionID )
                {
                    trouve = true;
                    break;
                }
            }
            if ( trouve == false )
            {
                QuestionsVotesEnBas.Add( pollQuestion );
            }
        }
        else
        {
            // On prend toutes les questions du questionnaire
            QuestionsVotesEnBas = pollQuestionCollection;
        }

        // Si une sous-population est a l'etude on affiche que cette sous-population AME13072010
        PersonneCollection personnesAffichees = new PersonneCollection();
        PollVoteCollection[] tableauPollVotespersonnesAffichees;
        if ( PersonnesOntReponduATout.Count > 0 )
        {
            personnesAffichees = PersonnesOntReponduATout;
            // Il faut reafficher PanelReponse sinon il disparait
            // Attention ici on est appellé par ListBoxQui_SelectedIndexChange donc on doit faire un Clear
            PanelReponses.Controls.Clear();
            AfficherEnHautReponsesDeSousPopulation();
            tableauPollVotespersonnesAffichees = TableauVotesPersonnesOntReponduATout;
        }
        else
        {
            personnesAffichees = Personnes;
            tableauPollVotespersonnesAffichees = TableauVotesPersonnes;
        }

        foreach ( PollQuestion question in QuestionsVotesEnBas )
        {
            PollAnswerCollection reponses = PollAnswerCollection.GetByPollQuestionID( question.PollQuestionId );

            Label labelQ = new Label();
            labelQ.CssClass = "LabelQuestionStyle";
            labelQ.Text = question.Rank.ToString() + " - " + question.Question;
            
            Table tableQ = new Table();
            TableCell cellQ = new TableCell();
            TableRow rowQ = new TableRow();

            cellQ.Controls.Add( labelQ );
            rowQ.Controls.Add( cellQ );
            tableQ.Controls.Add( rowQ );
            PanelReponsesEnBas.Controls.Add( tableQ );

            foreach ( PollAnswer reponse in reponses )
            {
                Table tableR = new Table();
                TableCell cellR = new TableCell();
                TableRow rowR = new TableRow();

                Label labelR = new Label();
                labelR.CssClass = "HyperLinkQuestionEnCoursStyle";
                if ( FormulaireEnModeExcel )
                {
                    labelR.Text = "r : "; // ajouter un petit " r : " pour que ce con d'excel ne prenne pas ca pour une date
                }
                labelR.Text += reponse.Rank.ToString() + " - " + reponse.Answer;

                cellR.Controls.Add( labelR );
                rowR.Controls.Add( cellR );
                tableR.Controls.Add( rowR );
                PanelReponsesEnBas.Controls.Add( tableR );

                Table tableP = new Table();

                int indexPollVotes = 0;
                foreach ( Personne p in personnesAffichees )
                {
                    PollVoteCollection pvc = tableauPollVotespersonnesAffichees[ indexPollVotes ].FindByAnswerID( reponse.PollAnswerId );
                    string personne = FormatPersonne( p );

                    foreach ( PollVote pv in pvc )
                    {
                        // Table de Reponses des Interviewes
                        TableCell cellP = new TableCell();
                        TableRow rowP = new TableRow();

                        if ( afficherDateVote )
                        {
                            TableCell cellD = new TableCell();
                            cellD.Text = pv.CreationDate.ToString();
                            rowP.Cells.Add( cellD );
                        }

                        cellP.Text = personne; // OPT17072010 Strings.TexteToHTML( personne );
                        cellP.CssClass = "TableReponsePersonneStyle";
                        rowP.Cells.Add( cellP );
                        tableP.Rows.Add( rowP );
                        // Pour les reponses textuelles
                        if ( pv.Vote != "" )
                        {
                            TableCell cellV = new TableCell();
                            cellV.Text = Strings.TexteToHTML( pv.Vote );
                            rowP.Cells.Add( cellV );
                            tableP.Rows.Add( rowP );
                        }
                    }
                    if ( pvc.Count > 0 /* votant */ )
                    {
                        PanelReponsesEnBas.Controls.Add( tableP );
                    }
                    indexPollVotes += 1;
                }
            }
        }
    }

    protected void DropDownListQuestionnaires_SelectedIndexChanged( object sender, EventArgs e )
    {
        SessionState.Questionnaire = SessionState.Questionnaires[ DropDownListQuestionnaires.SelectedIndex ];
        Votes = PollVoteCollection.GetPollVotesByQuestionnaireID( SessionState.Questionnaire.QuestionnaireID );
        Personnes = PersonneCollection.GetQuestionnaire( SessionState.Questionnaire.QuestionnaireID );

        EffacerSousPopulation();
        PanelSousPopulation.Visible = false;
        AfficherTousLesVotes = false;

        Response.Redirect( "~/Poll/QuestionnaireStatAll.aspx" );
    }

    protected void DropDownListQui_SelectedIndexChanged( object sender, EventArgs e )
    {
        if ( PersonnesDropDownListQui.Count > 0 )
        {
            Personne personne = PersonnesDropDownListQui[ DropDownListQui.SelectedIndex ];
            QuestionnaireControlStatAll.SelectedQuestionnaireID = SessionState.Questionnaire.QuestionnaireID;
            QuestionnaireControlStatAll.SelectedPersonneGUID = personne.PersonneGUID;
            DropDownListQuiSelectedIndex = DropDownListQui.SelectedIndex;

            AfficherOnReponduATout();
        }
    }

    protected void CheckBoxAfficherReponseTextuelle_CheckedChanged( object sender, EventArgs e )
    {
        SessionState.CheckBox[ "CheckBoxAfficherReponseTextuelle" ] = CheckBoxAfficherReponseTextuelle.Checked;
        QuestionnaireControlStatAll.AfficherReponseTextuelle = CheckBoxAfficherReponseTextuelle.Checked;
        Response.Redirect( "~/Poll/QuestionnaireStatAll.aspx" );
    }

    protected void CheckBoxAfficherDateVote_CheckedChanged( object sender, EventArgs e )
    {
        SessionState.CheckBox[ "CheckBoxAfficherDateVote" ] = CheckBoxAfficherDateVote.Checked;
        QuestionnaireControlStatAll.AfficherLaDateDesVotes = CheckBoxAfficherDateVote.Checked;
        Response.Redirect( "~/Poll/QuestionnaireStatAll.aspx" );
    }

    protected void CheckBoxAfficherMoyennePonderee_CheckedChanged( object sender, EventArgs e )
    {
        SessionState.CheckBox[ "CheckBoxAfficherMoyennePonderee" ] = CheckBoxAfficherMoyennePonderee.Checked;
        QuestionnaireControlStatAll.AfficherLaMoyennePonderee = CheckBoxAfficherMoyennePonderee.Checked;
        Response.Redirect( "~/Poll/QuestionnaireStatAll.aspx" );
    }

    protected void ListBoxQui_SelectedIndexChanged( object sender, EventArgs e )
    {
        if ( PersonnesDropDownListQui.Count > 0 )
        {
            ListItem pers = ListBoxQui.SelectedItem;
            if ( DropDownListQui.Items.FindByValue( pers.Value ) != null )
            {
                int idx = DropDownListQui.Items.IndexOf( pers );
                DropDownListQuiSelectedIndex = idx;
                DropDownListQui.SelectedIndex = idx;
                Personne personne = PersonnesDropDownListQui[ idx ];
                QuestionnaireControlStatAll.SelectedQuestionnaireID = SessionState.Questionnaire.QuestionnaireID;
                QuestionnaireControlStatAll.SelectedPersonneGUID = personne.PersonneGUID;
            }

            AfficherOnReponduATout();
        }
    }

    protected void ButtonEffacerQuiARepondu_Click( object sender, EventArgs e )
    {
        EffacerSousPopulation();
        PanelSousPopulation.Visible = false;
        AfficherTousLesVotes = false;
        Response.Redirect( "~/Poll/QuestionnaireStatAll.aspx" );
    }

    protected void ButtonAfficherSousPopulation_Click( object sender, EventArgs e )
    {
        QuestionnaireControlStatAll.PersonnesSousPopulation = PersonnesOntReponduATout;
        
        // Afficher les votes du permier votant
        if ( PersonnesOntReponduATout.Count > 0 )
        {
            QuestionnaireControlStatAll.SelectedPersonneGUID = PersonnesOntReponduATout[ 0 ].PersonneGUID;
            ListBoxQui.SelectedIndex = 0;
        }

        AfficherOnReponduATout();
    }

    protected void ButtonAfficherTousLesVotes_Click( object sender, EventArgs e )
    {
        QuestionsVotesEnBas.Clear();
        PanelReponsesEnBas.Controls.Clear();
        AfficherTousLesVotes = true;
        AfficherToutLesVotesEnBas( Guid.Empty );
    }

    protected void ButtonEffacerTousLesVotes_Click( object sender, EventArgs e )
    {
        QuestionsVotesEnBas.Clear();
        PanelReponsesEnBas.Controls.Clear();
        AfficherTousLesVotes = false;
        Response.Redirect( "~/Poll/QuestionnaireStatAll.aspx" );
    }

    protected void ImageButtonPrint_Click( object sender, ImageClickEventArgs e )
    {
        // Pour afficher les reponses des interviewes en bas du formulaire en mode print
        // il faut conserver le PolQuestionID dans l'url
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
        // Pour afficher les reponses des interviewes en bas du formulaire en mode print
        // il faut conserver le PolQuestionID dans l'url
        if ( Request.RawUrl.Contains( "?" ) )
        {
            Response.Redirect( Request.RawUrl + "&excel=1" );
        }
        else
        {
            Response.Redirect( Request.RawUrl + "?excel=1" );
        }
    }
}
