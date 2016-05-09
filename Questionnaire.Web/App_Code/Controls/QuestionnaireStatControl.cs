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
    public class QuestionnaireStatControl : WebControl, INamingContainer
    {
        public bool IsUserAllowedToVote
        {
            get
            {
                //if ( !Page.User.Identity.IsAuthenticated )
                //{
                //    return false;
                //}
                //if ( PollVote.CheckUserAlreadyVoted( Question.PollId, new Guid( Membership.GetUser().ProviderUserKey.ToString() ) ) )
                //{
                //    return false;
                //}
                return true;
            }
        }

        private bool HasUserAlreadyVoted()
        {
            PollVoteCollection pvc = PollVoteCollection.GetAll();
            foreach ( PollVote pv in pvc )
            {
                if (    pv.PollQuestionID == SessionState.Question.PollQuestionId
                     && pv.UserGUID == SessionState.Personne.PersonneGUID )
                {
                    return true;
                }
            }
            return false;
        }

        protected override void OnLoad( EventArgs e )
        {
            if ( !Page.IsPostBack )
            {
                if ( Page.Request[ "QuestionnaireID" ] != null )
                {
                    int questionnaireID = int.Parse( Page.Request[ "QuestionnaireID" ].ToString() );
                    SessionState.Questionnaire = Questionnaire.GetQuestionnaire( questionnaireID );
                    SessionState.Questions = PollQuestionCollection.GetByQuestionnaire( questionnaireID );
                    SessionState.Question = SessionState.Questions[ 0 ]; // permiere question
                    SessionState.Reponses = PollAnswerCollection.GetByPollQuestionID( SessionState.Question.PollQuestionId );
                    SessionState.CurrentQuestionIndex = 0;
                }
            }

            if ( !Page.IsCallback )
            {
                int tete = 123214;
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            CreateControls();
        }

        private void CreateControls()
        {
            if ( SessionState.Questions == null )
            {
                Label noPollAvailableLabel = new Label();
                noPollAvailableLabel.Text = "Il n'y a pas de question pour ce questionnaire !";
                this.Controls.Add( new LiteralControl( "<i>" ) );
                this.Controls.Add( noPollAvailableLabel );
                this.Controls.Add( new LiteralControl( "</i>" ) );
                return; // TODO: might not be correct. Was : Exit Sub
            }
            else
            {

            }

            //Hyperlink
            HyperLink hyp = new HyperLink();
            hyp.NavigateUrl = "~/Poll/List.aspx?QuestionnaireID=" + SessionState.Question.QuestionnaireID;
            hyp.Text = "Liste des Questions";

            this.Controls.Add( hyp );
            this.Controls.Add( new LiteralControl( "<br/>" ) );
            this.Controls.Add( new LiteralControl( "<br/>" ) );

            //Question
            Label questionLabel = new Label();
            questionLabel.Text = SessionState.Question.Question;
            questionLabel.Style.Add( HtmlTextWriterStyle.FontWeight, "bold" );
            this.Controls.Add( questionLabel );

            this.Controls.Add( new LiteralControl( "<br/>" ) );

            //Me.Controls.Add(New LiteralControl("<ul>"))
            this.Controls.Add( new LiteralControl( "<ul>" ) );
            
            //The Label answers
            foreach ( PollAnswer row in SessionState.Reponses )
            {
                decimal percentage = ComputePercentage( PollVoteCollection.NumberOfVotesByAnswer( row.PollAnswerId ) );

                this.Controls.Add( new LiteralControl( "<li>" ) );

                Label answerLabel = new Label();
                answerLabel.Text = string.Format( "{0}   ", row.Answer );
                this.Controls.Add( answerLabel );

                //this.Controls.Add( new LiteralControl( "<br/>" ) );

                System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                img.ImageUrl = "~/Images/pixel.png";
                img.Height = new Unit( 7 );
                img.Width = new Unit( percentage.ToString() );
                this.Controls.Add( img );

                Label percentageLabel = new Label();
                percentageLabel.Text = string.Format( " ({0}%)", percentage.ToString() );
                this.Controls.Add( percentageLabel );

                this.Controls.Add( new LiteralControl( "</li>" ) );

                this.Controls.Add( new LiteralControl( "</ul>" ) );

                //The summary
                HyperLink summaryLink = new HyperLink();
                string ttlvotes = PollVoteCollection.CountTotalVotes( SessionState.Question.PollQuestionId ).ToString();
                if ( ttlvotes == "1" )
                {
                    summaryLink.Text = string.Format( "{0} vote", ttlvotes );
                }
                else
                {
                    summaryLink.Text = string.Format( "{0} votes", ttlvotes );
                }

                summaryLink.NavigateUrl = "~/poll/View.aspx?PollId=" + SessionState.Question.PollQuestionId.ToString();
                this.Controls.Add( summaryLink );

                this.Controls.Add( new LiteralControl( "<br/>" ) );

                HyperLink totalReactionsLink = new HyperLink();
                string ttlrxns = PollReaction.CountTotalReactions( SessionState.Question.PollQuestionId ).ToString();
                if ( ttlrxns == "1" )
                {
                    totalReactionsLink.Text = string.Format( "{0} réaction", ttlrxns );
                }
                else
                {
                    totalReactionsLink.Text = string.Format( "{0} réactions", ttlrxns );
                }

                totalReactionsLink.NavigateUrl = "~/poll/View.aspx?PollId=" + SessionState.Question.PollQuestionId.ToString();
                this.Controls.Add( totalReactionsLink );
                this.Controls.Add( new LiteralControl( "<p>&nbsp</p>" ) );
            }

            // Question suivante
            this.Controls.Add( new LiteralControl( "<br/>" ) );
            RolloverButton rlb = new RolloverButton();
            rlb.Text = "Suivante";
            rlb.Click += new EventHandler( QuestionSuivante_Click );
            this.Controls.Add( rlb );
        }

        protected void SubmitButton_Click( object sender, EventArgs e )
        {
            if ( HasUserAlreadyVoted() )
            {
                this.Controls.Add( new LiteralControl( "<br/>Vous avez déjà répondu à cette question." ) );
            }
            else if ( !Strings.IsNullOrEmpty( ( ( ( RadioButtonList )this.Controls[ 5 ] ).SelectedValue ) ) )
            {
                Limitation limitation = new Limitation( SessionState.Questionnaire.MembreGUID );
                if ( limitation.LimitesReponses )
                {
                   Context.Response.Redirect( Tools.PageErreurPath + "Désolé mais le nombre de réponses pour ce questionnaire est atteinte.", true );
                }

                PollVote pv = new PollVote();
                pv.PollQuestionID = SessionState.Question.PollQuestionId;
                pv.PollAnswerId = new Guid( ( ( RadioButtonList )this.Controls[ 5 ] ).SelectedValue );
                pv.UserGUID = SessionState.Personne.PersonneGUID;
                pv.QuestionnaireID = SessionState.Question.QuestionnaireID;
                pv.CreationDate = DateTime.Now;
                PollVote.Create( pv );

                // Le questionnaire est termine
                if ( SessionState.CurrentQuestionIndex == SessionState.Questions.Count - 1 )
                {
                    SessionState.Questionnaire = null;
                    Page.Response.Redirect( "~/Poll/Termine.aspx", true );
                }

                // Passer a la question suivante
                SessionState.CurrentQuestionIndex += 1;
                SessionState.Question = SessionState.Questions[ SessionState.CurrentQuestionIndex ];
                SessionState.Reponses = PollAnswerCollection.GetByPollQuestionID( SessionState.Question.PollQuestionId );
                this.Controls.Clear();
                CreateControls();
            }
            else
            {
                this.Controls.Add( new LiteralControl( "<br/>Merci de répondre à la question." ) );
            }
        }

        protected void QuestionSuivante_Click( object sender, EventArgs e )
        {
            if ( SessionState.CurrentQuestionIndex == SessionState.Questions.Count - 1 )
            {
                SessionState.Questionnaire = null;
                Page.Response.Redirect( "~/Poll/Termine.aspx", true );
            }

            // Passer a la question suivante
            SessionState.CurrentQuestionIndex += 1;
            SessionState.Question = SessionState.Questions[ SessionState.CurrentQuestionIndex ];
            SessionState.Reponses = PollAnswerCollection.GetByPollQuestionID( SessionState.Question.PollQuestionId );
            this.Controls.Clear();
            CreateControls();
        }

        private decimal ComputePercentage( decimal numberOfVotes )
        {
            decimal percentage = 0;
            int totalVotes = PollVoteCollection.CountTotalVotes( SessionState.Question.PollQuestionId );
            if ( totalVotes > 0 )
            {
                percentage = System.Math.Round( ( numberOfVotes / totalVotes ) * 100, 0 );
            }
            return percentage;
        }
    }
}

