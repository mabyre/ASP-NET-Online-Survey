//
// Ce n'est pas une cinecure d'ecrire des table en code !
//
// Code a peu pres identique a QuestionnaireStatAllControl.cs
// mais pour la publication du questionnaire
//
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using UserControl.Web.Controls;
using Sql.Web.Data;

namespace UserControl.Web.Controls
{
    public class QuestionnairePublication : WebControl, INamingContainer
    {
        private static bool _AfficherReponseTextuelle = true;
        public bool AfficherReponseTextuelle
        {
            get { return _AfficherReponseTextuelle; }
            set { _AfficherReponseTextuelle = value; }
        }

        private static bool _AfficherLaDateDesVotes = false;
        public bool AfficherLaDateDesVotes
        {
            get { return _AfficherLaDateDesVotes; }
            set { _AfficherLaDateDesVotes = value; }
        }

        private int _SelectedQuestionnaireID;
        public int SelectedQuestionnaireID
        {
            get { return _SelectedQuestionnaireID; }
            set { _SelectedQuestionnaireID = value; }
        }

        private Guid _SelectedPersonneGUID = Guid.Empty;
        public Guid SelectedPersonneGUID
        {
            get { return _SelectedPersonneGUID; }
            set { _SelectedPersonneGUID = value; }
        }

        // Personnes ayant vote a ce questionnaire pour trouver le nombre de Votants par Question
        private static PersonneCollection _PersonneVotants = new PersonneCollection();
        public PersonneCollection PersonneVotants
        {
            get { return _PersonneVotants; }
            set { _PersonneVotants = value; }
        }

        // N'afficher que les votes de cette sous population
        private static PersonneCollection _PersonnesSousPopulation = new PersonneCollection();
        public PersonneCollection PersonnesSousPopulation
        {
            get { return _PersonnesSousPopulation; }
            set { _PersonnesSousPopulation = value; }
        }

        protected override void OnLoad( EventArgs e )
        {
            if ( Page.IsPostBack == false )
            {
                if ( SelectedQuestionnaireID != 0 )
                {
                    SessionState.Questionnaire = Questionnaire.GetQuestionnaire( SelectedQuestionnaireID );
                    SessionState.Questions = PollQuestionCollection.GetByQuestionnaire( SelectedQuestionnaireID );
                    SessionState.Votes = PollVoteCollection.GetPollVotesByQuestionnaireID( SelectedQuestionnaireID );
                    SessionState.Reponses = PollAnswerCollection.GetAll();
                }
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            if ( PersonnesSousPopulation.Count > 0 )
            {
                PollVoteCollection newPollVotes = new PollVoteCollection();
                PollVoteCollection pollVotes = PollVoteCollection.GetPollVotesByQuestionnaireID( SessionState.Questionnaire.QuestionnaireID );
                foreach ( Personne p in PersonnesSousPopulation )
                {
                    PollVoteCollection pvc = pollVotes.FindByPersonneGUID( p.PersonneGUID );
                    newPollVotes.AddRange( pvc );
                }
                SessionState.Votes = newPollVotes;
            }

            CreateControls();
        }

        private string CreateHtmlLabel( string label, string labelCssClass )
        {
            return string.Format( "<label class=\"{0}\">{1}</label>", labelCssClass, label );
        }

        private Table CreateTableForQuestion( PollQuestion question )
        {
            Table table = new Table();
            TableCell cell = new TableCell();
            TableRow row = new TableRow();

            string q = CreateHtmlLabel( question.Rank.ToString() + " - " + question.Question, "LabelQuestionStyle" );
            if ( question.Obligatoire )
            {
                q += "&nbsp;";
                string obligatoire = CreateHtmlLabel( "Obligatoire", "LabelListUserQuestionObligatoireStyle" );
                q += obligatoire;
            }
            if ( question.ChoixMultiple )
            {
                 q += "&nbsp;";
                 string multiple = CreateHtmlLabel( "Multiple", "LabelListUserQuestionChoixMultipleStyle" );
                 q += multiple;
            }
            if ( question.Fin )
            {
                q += "&nbsp;";
                string fin = CreateHtmlLabel( "Fin", "LabelListUserQuestionFinStyle" );
                q += fin;
            }
            if ( question.Instruction != "" )
            {
                q += "<br />";
                string inst = CreateHtmlLabel( question.Instruction, "LabelListUserQuestionInstructionStyle" );
                q += inst;
            }
            if ( question.Message != "" )
            {
                q += "<br />";
                string inst = CreateHtmlLabel( question.Message, "LabelListUserQuestionMessageStyle" );
                q += inst;
            }

            cell.Text = q;
            //cell.CssClass = cellCssClass;

            row.Cells.Add( cell );
            table.Rows.Add( row );
            return table;
        }

        private Table CreateTableForLabel( string label, string cellCssClass, string labelCssClass )
        {
            Table table = new Table();
            TableCell cell = new TableCell();
            TableRow row = new TableRow();

            cell.Text = string.Format( "<label class=\"{0}\">{1}</label>", labelCssClass, label );
            cell.CssClass = cellCssClass;
       
            row.Cells.Add( cell );
            table.Rows.Add( row );
            return table;
        }

        private Table CreateReponseTable( Guid pollQuestionID )
        {
            PollAnswerCollection answers = SessionState.Reponses.FindByPollQuestionID( pollQuestionID );

            Table table = new Table();
            TableRow row = new TableRow();
            TableCell cell = new TableCell();

            TableCell cellPix = new TableCell();
            cellPix.CssClass = "TableReponseCellPix";
            row.Cells.Add( cellPix );
            
            cell.Text = "Réponse";
            cell.CssClass = "TdHeaderReponseTableStyle";
            row.Cells.Add( cell );

            cell = new TableCell();
            cell.Text = "Vote";
            cell.CssClass = "TdHeaderReponseTableStyle";
            row.Cells.Add( cell );

            cell = new TableCell();
            cell.Text = "%";
            cell.CssClass = "TdHeaderReponseTableStyle";
            row.Cells.Add( cell );

            PollVoteCollection pvc = new PollVoteCollection();
            TableCell cellDateVote = new TableCell();
            if ( SelectedPersonneGUID != Guid.Empty )
            {
                pvc = SessionState.Votes.FindByPersonneGUID( SelectedPersonneGUID );

                //cell = new TableCell();
                //cell.Text = "A voté";
                //cell.CssClass = "TdHeaderReponseTableStyle";
                //row.Cells.Add( cell );

                //if ( AfficherLaDateDesVotes )
                //{
                //    cellDateVote.Text = "Date";
                //    cellDateVote.CssClass = "TdHeaderReponseTableStyle";
                //    row.Cells.Add( cellDateVote );
                //}
            }


            table.Rows.Add( row );

            int totalVotant = 0;
            foreach ( Personne p in PersonneVotants )
            {
                if ( SessionState.Votes.FindIfPersonneHasVoted( pollQuestionID, p.PersonneGUID ) )
                {
                    totalVotant += 1;
                }
            }
            int totalVotes = 0;
            foreach ( PollAnswer answer in answers )
            {
                int votes = SessionState.Votes.FindNumberOfVotesByAnswer( answer.PollAnswerId );
                totalVotes = SessionState.Votes.FindCountTotalVotes( answer.PollQuestionId );
                decimal percentage = ComputePercentage( votes, totalVotes );

                row = new TableRow();
                cell = new TableCell();

                cellPix = new TableCell();
                cellPix.CssClass = "TableReponseCellPix";
                row.Cells.Add( cellPix );

                HyperLink hyp = new HyperLink();
                hyp.NavigateUrl = "~/Questionnaire/Publication.aspx?PollAnswerID=" + answer.PollAnswerId; 
                hyp.Text = answer.Rank + " - " + answer.Answer;
                hyp.ToolTip = "Qui a répondu";
                hyp.CssClass = "HyperLinkReponseStatAllStyle";
                if ( TypeReponse.EstTextuelle( answer.TypeReponse ) )
                {
                    hyp.CssClass = "HyperLinkReponseTextuelleStatAllStyle";
                }
                cell.Controls.Add( hyp );

                if ( TypeReponse.EstTextuelle( answer.TypeReponse )
                    && SelectedPersonneGUID != Guid.Empty 
                    && AfficherReponseTextuelle )
                {
                    Label lbl = new Label();
                    foreach ( PollVote pv in pvc )
                    {
                        if ( pv.UserGUID == SelectedPersonneGUID && answer.PollAnswerId == pv.PollAnswerId )
                        {
                            lbl.Text = "&nbsp;" + pv.Vote;
                            break;
                        }
                    }
                    cell.Controls.Add( lbl );
                }

                cell.CssClass = "CellLinkReponseStatAllStyle";
                row.Cells.Add( cell );

                cell = new TableCell();
                cell.Text = votes.ToString();
                cell.CssClass = "TdStatTableStyle";
                row.Cells.Add( cell );

                cell = new TableCell();
                cell.Text = percentage.ToString();
                cell.CssClass = "TdStatTableStyle";
                row.Cells.Add( cell );

                //if ( SelectedPersonneGUID != Guid.Empty )
                //{
                //    cell = new TableCell();

                //    if ( AfficherLaDateDesVotes )
                //    {
                //        cellDateVote = new TableCell();
                //    }
                    
                //    // A t-il vote pour cette reponse ?
                //    cell.Text = " ";
                //    cellDateVote.Text = " ";
                //    foreach ( PollVote pv in pvc )
                //    {
                //        if ( pv.UserGUID == SelectedPersonneGUID && answer.PollAnswerId == pv.PollAnswerId )
                //        {
                //            cell.Text = "X";
                //            if ( AfficherLaDateDesVotes )
                //            {
                //                cellDateVote.Text = pv.CreationDate.ToString();
                //            }
                //            break;
                //        }
                //    }
                //    cell.CssClass = "TdVoteTableStyle";
                //    row.Cells.Add( cell );

                //    if ( AfficherLaDateDesVotes )
                //    {
                //        cellDateVote.CssClass = "TdStatTableDateStyle";
                //        row.Cells.Add( cellDateVote );
                //    }
                //}

                cell = new TableCell();
                System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                img.ImageUrl = "~/Images/pixel.png";
                img.Height = new Unit( 7 );
                img.Width = new Unit( percentage.ToString() );
                cell.Controls.Add( img );
                row.Cells.Add( cell );

                table.Rows.Add( row );
            }

            row = new TableRow();

            cell = new TableCell();
            cell.Text = "Votants : " + totalVotant.ToString();
            cell.CssClass = "TdTotalVoteTableStyle";
            cell.ColumnSpan = 2;
            row.Cells.Add( cell );

            cell = new TableCell();
            cell.Text = "Votes : " + totalVotes.ToString();
            cell.CssClass = "TdTotalVoteTableStyle";
            cell.ColumnSpan = 3;
            row.Cells.Add( cell );

            table.Rows.Add( row );

            return table;
        }

        private void CreateControls()
        {
            // Le Questionnaire
            Table tableLabel1 = CreateTableForLabel( SessionState.Questionnaire.Description, "TdStatQuestionnaireLabelStyle", "LabelQuestionnaireStyle" );
            this.Controls.Add( tableLabel1 );

            foreach( PollQuestion question in SessionState.Questions )
            {
                // Page
                if ( question.SautPage != "" )
                {
                    Table tablePage = new Table();
                    TableCell cellPage = new TableCell();
                    TableRow rowPage = new TableRow();

                    Label labelTitrePage = new Label();
                    StyleWeb.ApplyStyleWeb( "TitrePage", TypeStyleWeb.Label, labelTitrePage );
                    labelTitrePage.Text = question.SautPage;

                    cellPage.Controls.Add( labelTitrePage );
                    rowPage.Cells.Add( cellPage );
                    tablePage.Rows.Add( rowPage );

                    this.Controls.Add( tablePage );
                }

                // Tableau
                if ( question.Tableau != "" )
                {
                    Table tableTableau = new Table();
                    TableCell cellTableau = new TableCell();
                    TableRow rowTableau = new TableRow();

                    Label labelTableau = new Label();
                    StyleWeb.ApplyStyleWeb( "TitreTableau", TypeStyleWeb.Label, labelTableau );
                    labelTableau.Text = question.Tableau;

                    cellTableau.Controls.Add( labelTableau );
                    rowTableau.Cells.Add( cellTableau );
                    tableTableau.Rows.Add( rowTableau );

                    this.Controls.Add( tableTableau );
                }

                // Question
                Table tableLabel2 = CreateTableForQuestion( question );
                this.Controls.Add( tableLabel2 );

                Table table = CreateReponseTable( question.PollQuestionId );
                table.CssClass = "TableStatAllStyle";
                this.Controls.Add( table );
            }
        }

        private decimal ComputePercentage( decimal numberOfVotes, int totalVotes )
        {
            decimal percentage = 0;
            if ( totalVotes > 0 )
            {
                percentage = System.Math.Round( ( numberOfVotes / totalVotes ) * 100, 0 );
            }
            return percentage;
        }
    }
}

