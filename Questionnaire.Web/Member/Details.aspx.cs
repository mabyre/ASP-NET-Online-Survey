using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using Sql.Web.Data;

public partial class Member_Details : PageBase
{
    protected void Page_Load( object sender, System.EventArgs e )
    {
        if ( ! Page.User.Identity.IsAuthenticated )
        {
            Response.Write( "To use the member details functionality, you need to be authenticated. Please use the <a href='login.aspx'>Login</a> page to authenticate yourself." );
            Response.End();
        }

        if ( !IsPostBack )
        {
            MsgsPerPageTextBox.Text = "10"; //par defaut
            InitPageData();
        }
    }

    protected void btnCancel_Click( object sender, System.EventArgs e )
    {
        MembershipUser user = Membership.GetUser();
        MemberInfo membre = MemberInfo.GetMemberInfo( user.UserName, "hohoho" );

        Membership.DeleteUser( user.UserName, true );
        MemberInfo.Delete( membre.MembreGUID );

        FormsAuthentication.SignOut();
        Response.Redirect( "~/Default.aspx" );
    }

    protected void update_Click( object sender, System.EventArgs e )
    {
        MembershipUser user = Membership.GetUser();

        //if ( Email.Text != user.Email )
        //{
        //    user.Email = Email.Text;
        //    Membership.UpdateUser( user );
        //}

        //try
        //{
        //    MemberInfo member = new MemberInfo( MemberInfo.Columns.Memberid, user.ProviderUserKey );
        //    meme
        //    if ( meminf.IsLoaded == true )
        //    {
        //        Query qry = new Query( Tables.MemberInfo );
        //        qry.QueryType = QueryType.Update;
        //        qry.AddWhere( MemberInfo.Columns.Memberid, user.ProviderUserKey );
        //        qry.AddUpdateSetting( MemberInfo.Columns.Address, Addr.Text );
        //        qry.AddUpdateSetting( MemberInfo.Columns.Firstname, fname.Text );
        //        qry.AddUpdateSetting( MemberInfo.Columns.Lastname, lname.Text );
        //        qry.AddUpdateSetting( MemberInfo.Columns.Phone, Phone.Text );
        //        qry.AddUpdateSetting( MemberInfo.Columns.Newsletter, NewsletterCheck.Checked );
        //        qry.Execute();
        //        ContactStatus.Text = "Utilisateur mis à jour avec succès.";
        //        ContactStatus.ControlStyle.ForeColor = Color.Black;
        //    }
        //    else
        //    {
        //        MemberInfo.Insert
        //        ( 
        //            Addr.Text, 
        //            Phone.Text, 
        //            fname.Text, 
        //            lname.Text, 
        //            null, 
        //            (Guid)user.ProviderUserKey, 
        //            NewsletterCheck.Checked 
        //        );
        //        ContactStatus.Text = "Utilisateur mis à jour avec succès.";
        //        ContactStatus.ControlStyle.ForeColor = Color.Black;
        //    }
        //}
        //catch ( Exception ex )
        //{
        //    ContactStatus.Text = "Erreur de mise à jour de l'utilisateur : " + ex.Message;
        //    ContactStatus.ControlStyle.ForeColor = Color.Red;
        //}
    }

    void InitPageData()
    {
        //MembershipUser user = Membership.GetUser();
        //MemberInfo MemInfo = new MemberInfo( MemberInfo.Columns.Memberid, user.ProviderUserKey );
        //MemberProfile MemProf = new MemberProfile( MemberProfile.Columns.Memberid, user.ProviderUserKey );

        //if ( MemInfo.IsLoaded == true )
        //{
        //    UserName.Text = user.UserName;
        //    fname.Text = MemInfo.Firstname;
        //    lname.Text = MemInfo.Lastname;
        //    Email.Text = user.Email;
        //    Addr.Text = MemInfo.Address;
        //    Phone.Text = MemInfo.Phone;
        //    bool newsletter = ( bool )MemInfo.Newsletter;
        //    if ( newsletter == true )
        //    {
        //        NewsletterCheck.Checked = true;
        //    }
        //    else
        //    {
        //        NewsletterCheck.Checked = false;
        //    }
        //    if (  MemInfo.Avatar != null )
        //    {
        //        avatarimage.ImageUrl = "~\\UserControls\\AvatarImagefetch.ashx?Memberid=" + user.ProviderUserKey.ToString();
        //    }
        //}

        //if ( MemProf.IsLoaded == true )
        //{
        //    SignatureTextBox.Value = MemProf.Signature;
        //    BioTextBox.Value = MemProf.Bio;
        //    ShowEmailCheckBox.Checked = MemProf.Showemail;
        //    MsgsPerPageTextBox.Text = MemProf.Msgsperpage.ToString();
        //    SortDescendingCheckBox.Checked = MemProf.Sortdesc;
        //    ShowSignaturesCheckBox.Checked = MemProf.Showsignatures;
        //    ShowAvatarsCheckBox.Checked = MemProf.Showavatars;
        //    SendWatchEmailsCheckBox.Checked = MemProf.Sendwatchemails;
        //}
    }

    protected void uploadimage_Click( object sender, System.EventArgs e )
    {
        //if ( string.IsNullOrEmpty( newavatar.FileName ) || newavatar.PostedFile == null ) return;
        //byte[] thumbimage = ImageUtils.MakeThumb( newavatar.FileBytes, 69, 69 );
        //UpdateAvatar( thumbimage );
        //avatarimage.ImageUrl = "~\\UserControls\\AvatarImagefetch.ashx?Memberid=" + Membership.GetUser().ProviderUserKey.ToString();
    }


    public void UpdateAvatar( byte[] Avatar )
    {
        //Guid memberid = ( Guid )Membership.GetUser().ProviderUserKey;
        //Query qry = new Query( Tables.MemberInfo );
        //qry.QueryType = QueryType.Update;
        //qry.AddWhere( MemberInfo.Columns.Memberid, memberid );
        //if ( Avatar == null )
        //{

        //}
        //else
        //{
        //    qry.AddUpdateSetting( MemberInfo.Columns.Avatar, Avatar );
        //}
        //qry.Execute();
    }

    protected void UpdateProfileButton_Click( object sender, System.EventArgs e )
    {
        MembershipUser user = Membership.GetUser();
        //MemberProfile MemProf = new MemberProfile( MemberProfile.Columns.Memberid, user.ProviderUserKey );
        //try
        //{
        //    if ( MemProf.IsLoaded == true )
        //    {
        //        ClubStarterKit.Data.Member Mem = new ClubStarterKit.Data.Member();
        //        Mem.Signature = SignatureTextBox.Value;
        //        Mem.Bio = BioTextBox.Value;
        //        Mem.ShowEmail = ShowEmailCheckBox.Checked;
        //        Mem.MsgsPerPage = int.Parse( MsgsPerPageTextBox.Text );
        //        Mem.SortDescending = SortDescendingCheckBox.Checked;
        //        Mem.ShowSignatures = ShowSignaturesCheckBox.Checked;
        //        Mem.ShowAvatars = ShowAvatarsCheckBox.Checked;
        //        Mem.SendWatchEmails = SendWatchEmailsCheckBox.Checked;
        //        Mem.MemberId = user.ProviderUserKey;
        //        ClubStarterKit.Data.Member.Update( Mem );
        //        ProfStatus.Text = "Profile mis à jour avec succès.";
        //    }
        //    else
        //    {
        //        MemberProfile.Insert
        //        ( 
        //            (Guid)(user.ProviderUserKey), 
        //            DateTime.Now, 
        //            SignatureTextBox.Value, 
        //            BioTextBox.Value, 
        //            SortDescendingCheckBox.Checked,
        //            int.Parse( MsgsPerPageTextBox.Text ), 
        //            ShowAvatarsCheckBox.Checked, 
        //            ShowSignaturesCheckBox.Checked, 
        //            SendWatchEmailsCheckBox.Checked, 
        //            ShowEmailCheckBox.Checked
        //        );
        //        ProfStatus.Text = "Profile mis à jour avec succès.";
        //    }
        //}
        //catch ( Exception ex )
        //{
        //    ProfStatus.Text = "Erreur de  mis à jour du profile : " + ex.Message;
        //    ProfStatus.ControlStyle.ForeColor = Color.Red;
        //}
    }
}
