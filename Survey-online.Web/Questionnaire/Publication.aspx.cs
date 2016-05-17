//
// Identique a /Poll/QuestionnaireStatAll.aspx.cs mais pour la publication des resultats
// des Objets sont supprimes
// On conservera le code au plus pres de /Poll/QuestionnaireStatAll.aspx.cs
// preferant mettre en commentaire que de supprimer ...
//
// 

#region Using

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

#endregion

public partial class Page_QuestionnairePublication: PageBase
{
    // Synchroniser ListBoxQui et DropDownListQui
    // BUG10092009
    private static PersonneCollection PersonnesDropDownListQui
    {
        get
        {
            if ( HttpContext.Current.Session[ "QuestionnairePublication_PersonnesDropDownListQui" ] == null )
            {
                HttpContext.Current.Session[ "QuestionnairePublication_PersonnesDropDownListQui" ] = new PersonneCollection();
            }
            return ( PersonneCollection )HttpContext.Current.Session[ "QuestionnairePublication_PersonnesDropDownListQui" ];
        }
        set { HttpContext.Current.Session[ "QuestionnairePublication_PersonnesDropDownListQui" ] = value; }
    }

    private static int DropDownListQuiSelectedIndex
    {
        get
        {
            if ( HttpContext.Current.Session[ "QuestionnairePublication_DropDownListQuiSelectedIndex" ] == null )
            {
                HttpContext.Current.Session[ "QuestionnairePublication_DropDownListQuiSelectedIndex" ] = 0;
            }
            return ( int )HttpContext.Current.Session[ "QuestionnairePublication_DropDownListQuiSelectedIndex" ];
        }
        set { HttpContext.Current.Session[ "QuestionnairePublication_DropDownListQuiSelectedIndex" ] = value; }
    }

    // Les personnes qui ont repondu au questionnaire
    //private static PersonneCollection Personnes = new PersonneCollection();
    private static PersonneCollection Personnes
    {
        get
        {
            if ( HttpContext.Current.Session[ "QuestionnairePublication_Personnes" ] == null )
            {
                HttpContext.Current.Session[ "QuestionnairePublication_Personnes" ] = new PersonneCollection();
            }
            return ( PersonneCollection )HttpContext.Current.Session[ "QuestionnairePublication_Personnes" ];
        }
        set { HttpContext.Current.Session[ "QuestionnairePublication_Personnes" ] = value; }
    }

    // Cumuler les questions cliquees par l'utilisateur
    //private static PollQuestionCollection Questions = new PollQuestionCollection();
    private static PollQuestionCollection Questions
    {
        get
        {
            if ( HttpContext.Current.Session[ "QuestionnairePublication_Questions" ] == null )
            {
                HttpContext.Current.Session[ "QuestionnairePublication_Questions" ] = new PollQuestionCollection();
            }
            return ( PollQuestionCollection )HttpContext.Current.Session[ "QuestionnairePublication_Questions" ];
        }
        set { HttpContext.Current.Session[ "QuestionnairePublication_Questions" ] = value; }
    }

    // Cumuler les reponses cliquees par l'utilisateur
    //private static PollAnswerCollection Reponses = new PollAnswerCollection();
    private static PollAnswerCollection Reponses
    {
        get
        {
            if ( HttpContext.Current.Session[ "QuestionnairePublication_Reponses" ] == null )
            {
                HttpContext.Current.Session[ "QuestionnairePublication_Reponses" ] = new PollAnswerCollection();
            }
            return ( PollAnswerCollection )HttpContext.Current.Session[ "QuestionnairePublication_Reponses" ];
        }
        set { HttpContext.Current.Session[ "QuestionnairePublication_Reponses" ] = value; }
    }

    // Cumuler les ID des reponses cliquees par l'utilisateur
    //private static ArrayList PollAnswerID = new ArrayList();
    private static ArrayList PollAnswerID
    {
        get
        {
            if ( HttpContext.Current.Session[ "QuestionnairePublication_PollAnswerID" ] == null )
            {
                HttpContext.Current.Session[ "QuestionnairePublication_PollAnswerID" ] = new ArrayList();
            }
            return ( ArrayList )HttpContext.Current.Session[ "QuestionnairePublication_PollAnswerID" ];
        }
        set { HttpContext.Current.Session[ "QuestionnairePublication_PollAnswerID" ] = value; }
    }

    // BUG10092009 private static PersonneCollection PersonnesOntReponduATout = new PersonneCollection();
    private static PersonneCollection PersonnesOntReponduATout
    {
        get
        {
            if ( HttpContext.Current.Session[ "QuestionnairePublication_PersonnesOntReponduATout" ] == null )
            {
                HttpContext.Current.Session[ "QuestionnairePublication_PersonnesOntReponduATout" ] = new PersonneCollection();
            }
            return ( PersonneCollection )HttpContext.Current.Session[ "QuestionnairePublication_PersonnesOntReponduATout" ];
        }
        set { HttpContext.Current.Session[ "QuestionnairePublication_PersonnesOntReponduATout" ] = value; }
    }

    // Memoriser les tous les Votes du Questionnaire Courant - Optimisation
    private PollVoteCollection Votes
    {
        get
        {
            if ( ViewState[ "QuestionnairePublication_Votes" ] != null )
                return ( PollVoteCollection )( ViewState[ "QuestionnairePublication_Votes" ] );

            return null;
        }

        set
        {
            ViewState[ "QuestionnairePublication_Votes" ] = value;
        }
    }

    protected void Page_Load( object sender, EventArgs e )
    {
        if ( IsPostBack == false )
        {
            //// Choisir le premier Questionnaire a la place de l'utilisateur
            //if ( SessionState.Questionnaire == null && SessionState.Questionnaires.Count > 0 )
            //{
            //    SessionState.Questionnaire = SessionState.Questionnaires[ 0 ];
            //}

            //if ( Request.QueryString[ "QuestionnaireID" ] != null )
            //{
            //    int questionnaireID = int.Parse( Request.QueryString[ "QuestionnaireID" ] );
            //    SessionState.Questionnaire = SessionState.Questionnaires.FindByID( questionnaireID );
            //}

            if ( Request.QueryString[ "CodeAcces" ] != null )
            {
                int code = int.Parse( Request.QueryString[ "CodeAcces" ] );
                // Dans SessionState.Questionnaires on prend une precaution pour verifier le membre
                // si SessionState.MemberInfo == null alors Va te faire loguer et donc on utilise
                // pas ce code ici !
                // SessionState.Questionnaire = SessionState.Questionnaires.FindByCodeAcces( code );
                QuestionnaireCollection questionnaires = QuestionnaireCollection.GetAll();
                SessionState.Questionnaire = questionnaires.FindByCodeAcces( code );
            }

            if ( SessionState.Questionnaire == null || SessionState.Questionnaire.Publier == false )
            {
                PanelQuestionnairePasDePublication.Visible = true;
                PanelQuestionnairePublication.Visible = false;
                return;
            }

            // Pour un meilleur referencement
            LabelTitre.Text = "Statistiques " + SessionState.Questionnaire.Description;
            Page.Title = "Publication des Statistiques pour le questionnaire dont la description est " + SessionState.Questionnaire.Description;
            base.AddMetaContentType();
            base.AddMetaTag( "description", "Publication des statistiques pour le questionnaire" + SessionState.Questionnaire.Description );
            base.AddMetaTag( "keywords", "questionnaire, statistique, enquêtes, enquete, en ligne" );
            base.AddMetaTag( "revisit-after", "10 days" );
            base.AddMetaTag( "author", "Sodevlog" );
            base.AddMetaTag( "copyright", "Sodevlog" );

            // Charger les votes pour ce Questionnaire
            if ( SessionState.Questionnaire != null && Votes == null )
            {
                Votes = PollVoteCollection.GetPollVotesByQuestionnaireID( SessionState.Questionnaire.QuestionnaireID );
                Personnes = PersonneCollection.GetQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
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
                        PollQuestionCollection pollAnswerCollection = PollQuestionCollection.GetByQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
                        PollQuestion pollQuestion = pollAnswerCollection.FindByPollQuestionID( pollAnswer.PollQuestionId );

                        // Cumuler les Reponses cliquees par l'utilisateur et les Questions
                        Questions.Add( pollQuestion );
                        Reponses.Add( pollAnswer );

                        // BUB25092009 AfficherTout();
                    }
                }

                // BUG25092009 on peut afficher tout ici
                AfficherTout();

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
                    LabelListBoxQuiCount.Text = "Sous-population : " + ListBoxQui.Items.Count.ToString();
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
                QuestionnairePublication.SelectedQuestionnaireID = SessionState.Questionnaire.QuestionnaireID;

                LabelNombreContacts.Text = Personnes.Count.ToString();
                PersonnesDropDownListQui.Clear();
                // Trouver les votants
                if ( Votes.Count > 0 )
                {
                    foreach ( Personne p in Personnes )
                    {
                        //PollVoteCollection pvc = PollVoteCollection.GetPollVotes( SessionState.Questionnaire.QuestionnaireID, p.PersonneGUID ); 
                        PollVoteCollection pvc = Votes.FindByPersonneGUID( p.PersonneGUID );

                        // A t il vote pour ce questionnaire ?
                        if ( pvc.Count > 0 )
                        {
                            string personne = p.Nom + "/" + p.Prenom + "/" + p.EmailBureau;
                            DropDownListQui.Items.Add( personne );
                            PersonnesDropDownListQui.Add( p );
                        }
                    }
                }

                QuestionnairePublication.PersonneVotants = PersonnesDropDownListQui;
                // Afficher les votes du permier votant
                if ( PersonnesDropDownListQui.Count >= 1 )
                {
                    if ( DropDownListQuiSelectedIndex != 0 )
                    {
                        DropDownListQui.SelectedIndex = DropDownListQuiSelectedIndex;
                    }
                    Personne personne = PersonnesDropDownListQui[ DropDownListQui.SelectedIndex ];
                    QuestionnairePublication.SelectedQuestionnaireID = SessionState.Questionnaire.QuestionnaireID;
                    QuestionnairePublication.SelectedPersonneGUID = personne.PersonneGUID;
                }

                LabelVotes.Text = PersonnesDropDownListQui.Count.ToString();
            }

            CheckBoxAfficherReponseTextuelle.Checked = SessionState.CheckBox[ "CheckBoxAfficherReponseTextuelle" ];
            CheckBoxAfficherDateVote.Checked = SessionState.CheckBox[ "CheckBoxAfficherDateVote" ];
        }

        // Effacer les objets que l'on publie pas
        //DropDownListQuestionnaires.Visible = false;
        ListBoxQui.Visible = false;
        //DropDownListQui.Visible = false;
        CheckBoxAfficherReponseTextuelle.Checked = false;
        QuestionnairePublication.AfficherReponseTextuelle = false;
        QuestionnairePublication.AfficherLaDateDesVotes = false;
    }

    void EffacerSousPopulation()
    {
        Questions.Clear();
        Reponses.Clear();
        PollAnswerID.Clear();
        QuestionnairePublication.PersonnesSousPopulation = new PersonneCollection();
    }

    void AfficherTout()
    {
        if ( Page.IsPostBack == false )
        {
            ListBoxQui.Items.Clear();
            PersonnesOntReponduATout.Clear();
        }

        PanelReponses.Controls.Clear();
        PanelReponsesEnBas.Controls.Clear();

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
                foreach ( Personne p in Personnes )
                {
                    PollVoteCollection pvc = Votes.FindByPersonneGUID( p.PersonneGUID );
                    foreach ( PollVote pv in pvc )
                    {
                        if ( pollAnswerID == pv.PollAnswerId )
                        {
                            string personne = p.Nom + "/" + p.Prenom + "/" + p.EmailBureau;
                            if ( Page.IsPostBack == false )
                            {
                                ListBoxQui.Items.Add( personne );
                                PersonnesOntReponduATout.Add( p );
                            }

                            // Table de Reponses des Interviewes
                            //TableCell cellP = new TableCell();
                            //TableRow rowP = new TableRow();
                            //cellP.Text = Strings.TexteToHTML( personne );
                            //cellP.CssClass = "TableReponsePersonneStyle";
                            //rowP.Cells.Add( cellP );
                            //table.Rows.Add( rowP );

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
                    }
                }
                PanelReponsesEnBas.Controls.Add( table );
            }
            else // Reponse multiples cliquee par l'utilisateur
            {
                // Remplir d'abord ListBoxQui trouver au passage les personnes qui ont
                // repondu a tout
                foreach ( Personne p in Personnes )
                {
                    PollVoteCollection pvc = Votes.FindByPersonneGUID( p.PersonneGUID );
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
                        string personne = p.Nom + "/" + p.Prenom + "/" + p.EmailBureau;
                        if ( Page.IsPostBack == false )
                        {
                            ListBoxQui.Items.Add( personne );
                            PersonnesOntReponduATout.Add( p );
                        }
                    }
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
                    foreach ( Personne p in PersonnesOntReponduATout )
                    {
                        PollVoteCollection pvc = Votes.FindByPersonneGUID( p.PersonneGUID );
                        string personne = p.Nom + "/" + p.Prenom + "/" + p.EmailBureau;

                        foreach ( PollVote pv in pvc )
                        {
                            if ( pv.PollAnswerId == pollAnswerGUID )
                            {
                                // Table de Reponses des Interviewes
                                //TableCell cellP = new TableCell();
                                //TableRow rowP = new TableRow();
                                //cellP.Text = Strings.TexteToHTML( personne );
                                //cellP.CssClass = "TableReponsePersonneStyle";
                                //rowP.Cells.Add( cellP );
                                //table.Rows.Add( rowP );
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
                        }
                    }

                    PanelReponsesEnBas.Controls.Add( table );
                }
            }

            Label labelCount = new Label();
            labelCount.Text = "Nombre de réponses : " + ListBoxQui.Items.Count.ToString();
            PanelReponsesEnBas.Controls.Add( labelCount );
        }
    }

    protected void DropDownListQui_SelectedIndexChanged( object sender, EventArgs e )
    {
        if ( PersonnesDropDownListQui.Count > 0 )
        {
            Personne personne = PersonnesDropDownListQui[ DropDownListQui.SelectedIndex ];
            QuestionnairePublication.SelectedQuestionnaireID = SessionState.Questionnaire.QuestionnaireID;
            QuestionnairePublication.SelectedPersonneGUID = personne.PersonneGUID;
            DropDownListQuiSelectedIndex = DropDownListQui.SelectedIndex;

            AfficherTout();
        }
    }

    protected void CheckBoxAfficherReponseTextuelle_CheckedChanged( object sender, EventArgs e )
    {
        //SessionState.CheckBox[ "CheckBoxAfficherReponseTextuelle" ] = CheckBoxAfficherReponseTextuelle.Checked;
        //QuestionnairePublication.AfficherReponseTextuelle = CheckBoxAfficherReponseTextuelle.Checked;
        //Response.Redirect( "~/Questionnaire/Publication.aspx" );
    }

    protected void CheckBoxAfficherDateVote_CheckedChanged( object sender, EventArgs e )
    {
        //SessionState.CheckBox[ "CheckBoxAfficherDateVote" ] = CheckBoxAfficherDateVote.Checked;
        //QuestionnairePublication.AfficherLaDateDesVotes = CheckBoxAfficherDateVote.Checked;
        //Response.Redirect( "~/Poll/QuestionnaireStatAll.aspx" );
    }
    

    protected void ListBoxQui_SelectedIndexChanged( object sender, EventArgs e )
    {
        //if ( PersonnesDropDownListQui.Count > 0 )
        //{
        //    ListItem pers = ListBoxQui.SelectedItem;
        //    if ( DropDownListQui.Items.FindByValue( pers.Value ) != null )
        //    {
        //        int idx = DropDownListQui.Items.IndexOf( pers );
        //        DropDownListQuiSelectedIndex = idx;
        //        DropDownListQui.SelectedIndex = idx;
        //        Personne personne = PersonnesDropDownListQui[ idx ];
        //        QuestionnairePublication.SelectedQuestionnaireID = SessionState.Questionnaire.QuestionnaireID;
        //        QuestionnairePublication.SelectedPersonneGUID = personne.PersonneGUID;
        //    }

        //    AfficherTout();
        //}
    }

    protected void ButtonEffacerQuiARepondu_Click( object sender, EventArgs e )
    {
        EffacerSousPopulation();
        PanelSousPopulation.Visible = false;
        Response.Redirect( "~/Questionnaire/Publication.aspx" );
    }

    protected void ButtonAfficherSousPopulation_Click( object sender, EventArgs e )
    {
        QuestionnairePublication.PersonnesSousPopulation = PersonnesOntReponduATout;
        
        // Afficher les votes du permier votant
        if ( PersonnesOntReponduATout.Count > 0 )
        {
            QuestionnairePublication.SelectedPersonneGUID = PersonnesOntReponduATout[ 0 ].PersonneGUID;
            ListBoxQui.SelectedIndex = 0;
        }

        AfficherTout();
    }
}
