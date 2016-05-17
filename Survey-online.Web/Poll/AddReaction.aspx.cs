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
using System.Text.RegularExpressions;
using Sql.Web.Data;

partial class Poll_AddReaction : PageBase
{
    private Guid _pollId;
    public Guid PollId
    {
        get
        {
            if ( _pollId == Guid.Empty )
            {
                _pollId = ( Guid )ViewState[ "PollId" ];
            }
            return _pollId;
        }
        set { ViewState[ "PollId" ] = value; }
    }

    protected void Page_Load( object sender, System.EventArgs e )
    {
        try
        {
            PollId = new Guid( Request[ "PollId" ] );
        }
        catch
        {
            Response.Redirect( "~/Poll/List.aspx", true );
        }

        if ( PollId == Guid.Empty )
        {
            Response.Redirect( "~/Poll/List.aspx", true );
        }

        PollQuestion q = PollQuestion.GetQuestion( PollId );
        QuestionLabel.Text = q.Question;
    }

    protected void UpdateCancelButton_Command( object sender, System.Web.UI.WebControls.CommandEventArgs e )
    {
        if ( e.CommandName == "Cancel" )
        {
            Response.Redirect( e.CommandArgument.ToString(), true );
        }
    }

    protected void UpdateButton_Click( object sender, System.EventArgs e )
    {
        Guid uGuid = new Guid( Membership.GetUser().ProviderUserKey.ToString() );
        string reaction = HttpUtility.HtmlEncode( ReactionTextBox.Text.Trim() );
        
        ////hack to preserve at least some formatting in a post
        //reaction = Regex.Replace( reaction, "" + char( 13 ) + "" + Chr( 10 ) + "", "<br />", RegexOptions.IgnoreCase | RegexOptions.Multiline );
        //reaction = Regex.Replace( reaction, "&lt;br /&gt;", "<br />", RegexOptions.IgnoreCase | RegexOptions.Multiline );
        
        //suggest you get an html editor type input box instead
        //if you do, you'll need to sanitize the input here to check for malicious html and script junk
        PollReaction pr = new PollReaction();
        pr.Reaction = reaction;
        pr.PollId = PollId;
        pr.UserId = uGuid;
        PollReaction.Create( pr );
        Response.Redirect( string.Format( "~/Poll/View.aspx?pollId={0}", PollId.ToString() ), true );
    }
}

