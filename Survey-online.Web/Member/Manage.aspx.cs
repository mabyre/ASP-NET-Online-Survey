//
// C'est la que Link est une grosse Merde
// Etant donne que l'on construit les donnees pas ComputeColumns on peut se demander a quoi sert
// sert la jointure faite en link ? et c'est une bonne question
// bon elle va servir a trier les donnees mais c'est tellement peut souple que l'on ne peut
// pas se passer de ComputeColumns ...
//
// <asp:BoundField> n'est pas non plus un control tres utilisable
// pour recuperer des donnes brutes d'une BD et c'est tout
//
// Le temps d'execution n'est pas tres satisfaisant et la modification des donnees des membres 
// entraine un boulot considerable.
//
// Attention LINK de Daube ne peut pas faire de tri sur une colonne acceptant des valeurs NULL
// il faut absoluement decocher la case dans la definition de la table 
// ca impose de creer des donnes anciennes
//

using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Sql.Data;
using Sql.Web.Data;
using ImportExportContact;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


public partial class Page_MemberManage : PageBase
{
    static int colonneCommandField = 0;
    static int colonneTelephone = 6;
    static int colonneAdresse = 7;
    static int colonneEmail = 12;
    static int colonneDateCreation = 13;
    static int colonneDateFinAbonnement = 14;
    static int colonneHyperLinkEmail = 15;
    static int colonneEditField = 16;

    private DataSet dataSetUser
    {
        get { return ( DataSet )ViewState[ "dataSetUser" ]; }
        set { ViewState[ "dataSetUser" ] = value; }
    }

    private DataSet dataSetMember
    {
        get { return ( DataSet )ViewState[ "dataSetMember" ]; }
        set { ViewState[ "dataSetMember" ] = value; }
    }

    // Pallier au fait que sur cette Gridview les deux proprietes ne marche pas !!
    private SortDirection sortDirection
    {
        get { return ( SortDirection )ViewState[ "sortDirection" ]; }
        set { ViewState[ "sortDirection" ] = value; }
    }

    private string sortExpression
    {
        get { return ( string )ViewState[ "sortExpression" ]; }
        set { ViewState[ "sortExpression" ] = value; }
    }

    protected override void OnPreInit( EventArgs e )
    {
        base.OnPreInit( e );

        if ( Request.QueryString[ "print" ] != null )
        {
            // MasterPageFile ne peut etre modifiee que dans OnPreInit()
            Page.MasterPageFile = "~/Print.Master";
        }
    }

    protected void Page_Load( object sender, System.EventArgs e )
    {
        if ( IsPostBack == false )
        {
            /* L'alphabet */
            DropDownListLettre.Items.Add( "---" );
            string _alpha = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] alpha = _alpha.Split( ',' );
            foreach ( string s in alpha )
            {
                DropDownListLettre.Items.Add( s );
            }

            if ( User.IsInRole( "Administrateur" ) )
            {
                /* Trouver les societes */
                DropDownListSociete.Items.Add( "Toutes les sociétés" );
                MemberInfoCollection membres = MemberInfoCollection.GetAll();
                foreach ( MemberInfo m in membres )
                {
                    ListItem item = new ListItem( m.Societe );
                    if ( !DropDownListSociete.Items.Contains( item ) )
                    {
                        DropDownListSociete.Items.Add( item );
                    }
                }
            }
            else
            {
                DropDownListSociete.Items.Add( SessionState.MemberInfo.Societe );
                DropDownListSociete.Enabled = false;
                RolloverLinkRegister.Visible = false;
                GridViewMembers.Columns[ colonneCommandField ].Visible = false;
                GridViewMembers.Columns[ colonneEditField ].Visible = false;
            }

            sortDirection = SortDirection.Descending;

            GridViewMembers.DataSource = BuildData();
            GridViewMembers.DataBind();
        }

        // Formulaire en mode print
        if ( Request.QueryString[ "print" ] != null )
        {
            GridViewMembers.Columns[ colonneCommandField ].Visible = false;
            GridViewMembers.Columns[ colonneEditField ].Visible = false;
            GridViewMembers.Columns[ colonneTelephone ].Visible = true;
            GridViewMembers.Columns[ colonneAdresse ].Visible = true;
            GridViewMembers.Columns[ colonneEmail ].Visible = true;
            GridViewMembers.Columns[ colonneHyperLinkEmail ].Visible = false;
            GridViewMembers.Columns[ colonneDateCreation ].Visible = true;
            GridViewMembers.Width = Unit.Percentage( 100 );
        }
        else
        {
            GridViewMembers.Columns[ colonneTelephone ].Visible = false;
            GridViewMembers.Columns[ colonneAdresse ].Visible = false;
            GridViewMembers.Columns[ colonneEmail ].Visible = false;

            // DEBUG GridViewMembers.Columns[ colonneDateCreation ].Visible = false;
        }

        if ( Request.QueryString[ "QuestionnaireID" ] != null )
        {
            int questionnaireID = int.Parse( Request.QueryString[ "QuestionnaireID" ] );
            SessionState.Questionnaire = SessionState.Questionnaires.FindByID( questionnaireID );
            Response.Redirect( "~/Contact/Email.aspx" );
        }
    }

    public DataTable BuildData()
    {
        ///
        /// Acces aux donnes
        /// 
        // On peut recuperer les MemberUser mais on ne sait pas quoi en faire
        MembershipUserCollection muc = Membership.GetAllUsers();

        // Preparation de l'appel a la procedure stockee aspnet_Membership_GetAllUsers
        SqlParameter[] parameters = new SqlParameter[ 3 ];
        parameters[ 0 ] = new SqlParameter( "@ApplicationName", SqlDbType.VarChar );
        parameters[ 0 ].Value = "/"; // Application.ToString();
        parameters[ 1 ] = new SqlParameter( "@PageIndex", SqlDbType.Int );
        parameters[ 1 ].Value = 0;
        parameters[ 2 ] = new SqlParameter( "@PageSize", SqlDbType.Int );
        parameters[ 2 ].Value = 5000;

        // Infos du User
        dataSetUser = SqlDataProvider.ExecuteDataset
        (
            System.Configuration.ConfigurationManager.ConnectionStrings[ "LocalSqlServer" ].ConnectionString,
            CommandType.StoredProcedure,
            "aspnet_Membership_GetAllUsers",
            parameters
        );

        // Infos du membre
        dataSetMember = SqlDataProvider.ExecuteDataset
        (
            Tools.DatabaseConnectionString,
            CommandType.StoredProcedure,
            "GetMemberInfoAll"
        );

        ///
        /// La Jointure 
        /// 
        IEnumerable<DataRow> memberQuery =
            from member in dataSetMember.Tables[ 0 ].AsEnumerable()
            select member;

        IEnumerable<DataRow> userQuery =
            from user in dataSetUser.Tables[ 0 ].AsEnumerable()
            select user;

        var userMemberQuery =
            from m in memberQuery.AsEnumerable()
            join u in userQuery.AsEnumerable()
            on m.Field<Guid>( "MembreGUID" ) equals u.Field<Guid>( "UserId" )
            orderby u.Field<DateTime>( "CreateDate" ) ascending
            select new
            {
                MembreGuid = m[ "MembreGUID" ],
                NomUtilisateur = m[ "NomUtilisateur" ],
                MotDePasse = m[ "MotDePasse" ],
                Nom = m[ "Nom" ],
                Prenom = m[ "Prenom" ],
                Societe = m[ "Societe" ],
                Telephone = m[ "Telephone" ],
                Adresse = m[ "Adresse" ],
                DateFinAbonnement = m[ "DateFinAbonnement" ],
                IsApproved = u[ "IsApproved" ],
                IsLockedOut = u[ "IsLockedOut" ],
                LastLoginDate = u[ "LastLoginDate" ],
                CreateDate = u[ "CreateDate" ],
                UserId = u[ "UserId" ]
            };

        //
        // Construction de la table de resultats
        //
        DataTable userMember = new DataTable();
        ConstructColumns( ref userMember );

        foreach ( var row in userMemberQuery )
        {
            DataRow r = userMember.NewRow();

            r[ "MembreGUID" ] = row.MembreGuid;
            r[ "NomUtilisateur" ] = row.NomUtilisateur;
            r[ "MotDePasse" ] = row.MotDePasse;
            r[ "Nom" ] = row.Nom;
            r[ "Prenom" ] = row.Prenom;
            r[ "Societe" ] = row.Societe;
            r[ "Telephone" ] = row.Telephone;
            r[ "Adresse" ] = row.Adresse;
            r[ "IsApproved" ] = row.IsApproved;
            r[ "IsLockedOut" ] = row.IsLockedOut;
            r[ "LastLoginDate" ] = row.LastLoginDate;
            r[ "CreateDate" ] = row.CreateDate;
            r[ "DateFinAbonnement" ] = row.DateFinAbonnement;
            r[ "UserId" ] = row.UserId;

            userMember.Rows.Add( r );
        }

        return userMember;
    }

    public DataTable GetSortedData( string sortExpression, SortDirection sortDirection )
    {
        IEnumerable<DataRow> memberQuery =
            from member in dataSetMember.Tables[ 0 ].AsEnumerable()
            select member;

        IEnumerable<DataRow> userQuery =
            from user in dataSetUser.Tables[ 0 ].AsEnumerable()
            select user;

        var query =
            from m in memberQuery.AsEnumerable()
            join u in userQuery.AsEnumerable()
            on m.Field<Guid>( "MembreGUID" ) equals u.Field<Guid>( "UserId" )
            select new
            {
                MembreGuid = m[ "MembreGUID" ],
                NomUtilisateur = m[ "NomUtilisateur" ],
                MotDePasse = m[ "MotDePasse" ],
                Nom = m[ "Nom" ],
                Prenom = m[ "Prenom" ],
                Societe = m[ "Societe" ],
                Telephone = m[ "Telephone" ],
                Adresse = m[ "Adresse" ],
                DateFinAbonnement = m[ "DateFinAbonnement" ],
                IsApproved = u[ "IsApproved" ],
                IsLockedOut = u[ "IsLockedOut" ],
                LastLoginDate = u[ "LastLoginDate" ],
                CreateDate = u[ "CreateDate" ],
                UserId = u[ "UserId" ]
            };

        DataTable userMember = new DataTable();
        if ( sortDirection == SortDirection.Ascending )
        {
            var memberUserQuery =
                from mu in query
                orderby GetPropertyValue( mu, sortExpression ) ascending
                select new
                {
                    MembreGuid = mu.MembreGuid,
                    NomUtilisateur = mu.NomUtilisateur,
                    MotDePasse = mu.MotDePasse,
                    Nom = mu.Nom,
                    Prenom = mu.Prenom,
                    Societe = mu.Societe,
                    Telephone = mu.Telephone,
                    Adresse = mu.Adresse,
                    IsApproved = mu.IsApproved,
                    IsLockedOut = mu.IsLockedOut,
                    LastLoginDate = mu.LastLoginDate,
                    CreateDate = mu.CreateDate,
                    DateFinAbonnement = mu.DateFinAbonnement,
                    UserId = mu.UserId
                };

            //
            // Construction de la table de resultats
            //
            ConstructColumns( ref userMember );
            foreach ( var row in memberUserQuery )
            {
                DataRow r = userMember.NewRow();

                r[ "MembreGUID" ] = row.MembreGuid;
                r[ "NomUtilisateur" ] = row.NomUtilisateur;
                r[ "MotDePasse" ] = row.MotDePasse;
                r[ "Nom" ] = row.Nom;
                r[ "Prenom" ] = row.Prenom;
                r[ "Societe" ] = row.Societe;
                r[ "Telephone" ] = row.Telephone;
                r[ "Adresse" ] = row.Adresse;
                r[ "IsApproved" ] = row.IsApproved;
                r[ "IsLockedOut" ] = row.IsLockedOut;
                r[ "LastLoginDate" ] = row.LastLoginDate;
                r[ "CreateDate" ] = row.CreateDate;
                r[ "DateFinAbonnement" ] = row.DateFinAbonnement;
                r[ "UserId" ] = row.UserId;

                userMember.Rows.Add( r );
            }
        }
        else // Descending
        {
            var memberUserQuery =
                from mu in query.AsEnumerable()
                orderby GetPropertyValue( mu, sortExpression ) descending
                select new
                {
                    MembreGuid = mu.MembreGuid,
                    NomUtilisateur = mu.NomUtilisateur,
                    MotDePasse = mu.MotDePasse,
                    Nom = mu.Nom,
                    Prenom = mu.Prenom,
                    Societe = mu.Societe,
                    Telephone = mu.Telephone,
                    Adresse = mu.Adresse,
                    IsApproved = mu.IsApproved,
                    IsLockedOut = mu.IsLockedOut,
                    LastLoginDate = mu.LastLoginDate,
                    CreateDate = mu.CreateDate,
                    DateFinAbonnement = mu.DateFinAbonnement,
                    UserId = mu.UserId
                };

            //
            // Construction de la table de resultats
            //
            ConstructColumns( ref userMember );
            foreach ( var row in memberUserQuery )
            {
                DataRow r = userMember.NewRow();

                r[ "MembreGUID" ] = row.MembreGuid;
                r[ "NomUtilisateur" ] = row.NomUtilisateur;
                r[ "MotDePasse" ] = row.MotDePasse;
                r[ "Nom" ] = row.Nom;
                r[ "Prenom" ] = row.Prenom;
                r[ "Societe" ] = row.Societe;
                r[ "Telephone" ] = row.Telephone;
                r[ "Adresse" ] = row.Adresse;
                r[ "IsApproved" ] = row.IsApproved;
                r[ "IsLockedOut" ] = row.IsLockedOut;
                r[ "LastLoginDate" ] = row.LastLoginDate;
                r[ "CreateDate" ] = row.CreateDate;
                r[ "DateFinAbonnement" ] = row.DateFinAbonnement;
                r[ "UserId" ] = row.UserId;

                userMember.Rows.Add( r );
            }
        }

        return userMember;
    }

    private static object GetPropertyValue( object obj, string property )
    {
        System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty( property );
        return propertyInfo.GetValue( obj, null );
    }

    private void ConstructColumns( ref DataTable table )
    {
        DataColumn[] colums = new DataColumn[ dataSetUser.Tables[ 0 ].Columns.Count + dataSetMember.Tables[ 0 ].Columns.Count ];
        dataSetUser.Tables[ 0 ].Columns.CopyTo( colums, 0 );
        dataSetMember.Tables[ 0 ].Columns.CopyTo( colums, dataSetUser.Tables[ 0 ].Columns.Count );
        foreach ( DataColumn dc in colums )
        {
            DataColumn dc1 = new DataColumn();
            dc1.ColumnName = dc.ColumnName;
            table.Columns.Add( dc1 );
        }
    }

    string SqlCommand( string societe, string lettre )
    {
        string sqlCommand = "SELECT * From [MemberInfo]";
        if ( societe != "Toutes les sociétés" )
        {
            sqlCommand += " WHERE Societe = '" + societe + "'";
            if ( lettre != "---" )
            {
                sqlCommand += " AND UPPER(Nom) LIKE '" + lettre + "%'";
            }
        }
        else
        {
            if ( lettre != "---" )
            {
                sqlCommand += " WHERE UPPER(Nom) LIKE '" + lettre + "%'";
            }
        }
        return sqlCommand;
    }

    protected void ImageButtonPrint_Click( object sender, ImageClickEventArgs e )
    {
        Response.Redirect( "~/Member/Manage.aspx?print=1" );
    }

    protected void GridViewMembers_OnLoad( object sender, EventArgs e )
    {
        Trace.Warn( "GridViewMembers_OnLoad" );
        ComputeColumns();
    }

    protected void ComputeColumns()
    {
        int indexRow = 0;
        DataKeyArray dka = GridViewMembers.DataKeys;
        foreach ( GridViewRow r in GridViewMembers.Rows )
        {
            Label isApproved = ( Label )GridViewMembers.Rows[ indexRow ].FindControl( "LabelIsApproved" );
            Label isLocked = ( Label )GridViewMembers.Rows[ indexRow ].FindControl( "LabelLocked" );
            Label isOnline = ( Label )GridViewMembers.Rows[ indexRow ].FindControl( "LabelOnline" );
            Label lastLoginDate = ( Label )GridViewMembers.Rows[ indexRow ].FindControl( "LabelLastLoginDate" );
            Label email = ( Label )GridViewMembers.Rows[ indexRow ].FindControl( "LabelEmail" );
            HyperLink hyperlinkEmail = ( HyperLink )GridViewMembers.Rows[ indexRow ].FindControl( "HyperLinkEmail" );
            Label dateCreation = ( Label )GridViewMembers.Rows[ indexRow ].FindControl( "LabelDateCreation" );
            Label dateFinAbonnement = ( Label )GridViewMembers.Rows[ indexRow ].FindControl( "LabelDateFinAbonnement" );

            Guid memberGuid = new Guid( dka[ indexRow ].Value.ToString() );
            MembershipUser user = Membership.GetUser( memberGuid );
            if ( user.IsApproved )
            {
                isApproved.Text = "Vrai";
                isApproved.CssClass = "LabelBlueStyle";
            }
            else
            {
                isApproved.Text = "Faux";
                isApproved.CssClass = "LabelRedStyle";
            }

            if ( user.IsLockedOut )
            {
                isLocked.Text = "Vrai";
                isLocked.CssClass = "LabelRedStyle";
            }
            else
            {
                isLocked.Text = "Faux";
                isLocked.CssClass = "LabelBlueStyle";
            }

            if ( user.IsOnline )
            {
                isOnline.Text = "Vrai";
                isOnline.CssClass = "LabelGreenStyle";
            }
            else
            {
                isOnline.Text = "Faux";
                isOnline.CssClass = "LabelBlueStyle";
            }

            lastLoginDate.Text = user.LastLoginDate.ToShortDateString();
            lastLoginDate.ToolTip = user.LastLoginDate.ToShortTimeString();
            email.Text = user.Email;
            hyperlinkEmail.NavigateUrl = "mailto:" + user.Email;
            hyperlinkEmail.ToolTip = user.Email;
            dateCreation.Text = user.CreationDate.ToShortDateString();

            if ( Roles.IsUserInRole( user.UserName, "Découverte" ) )
            {
                dateCreation.ToolTip = "Découverte";
            }
            else
            {
                dateCreation.ToolTip = "Client";
                dateCreation.CssClass = "BoundFieldDateGreenStyle";
            }

            MemberInfo member = MemberInfo.Get( memberGuid );
            dateFinAbonnement.Text = member.DateFinAbonnement.ToShortDateString();
            DateTime now = DateTime.Now;
            if ( now < member.DateFinAbonnement )
            {
                if ( Roles.IsUserInRole( user.UserName, "Découverte" ) == false )
                {
                    dateFinAbonnement.CssClass = "BoundFieldDateGreenStyle";
                }
            }
            else
            {
                dateFinAbonnement.CssClass = "BoundFieldDateRedStyle";
                // Dernier jour d'Abonnement
                if ( now < member.DateFinAbonnement.AddDays( 1 ) )
                {
                    dateFinAbonnement.CssClass = "BoundFieldDateOrangeStyle";
                }
            }
            TimeSpan ts = member.DateFinAbonnement - DateTime.Now;
            dateFinAbonnement.ToolTip = ts.Days.ToString();

            indexRow += 1;
        }
    }

    protected void DropDownListSociete_SelectedIndexChanged( object sender, EventArgs e )
    {
    }

    protected void DropDownListLettre_SelectedIndexChanged( object sender, EventArgs e )
    {
    }

    protected void GridViewMembers_RowCreated( object sender, GridViewRowEventArgs e )
    {
        if ( e.Row.RowType == DataControlRowType.Header )
        {
            AddGlyph( GridViewMembers, e.Row );
        }
    }

    protected void GridViewMembers_RowUpdated( object sender, GridViewUpdatedEventArgs e )
    {
    }

    protected void GridViewMembers_RowUpdating( object sender, GridViewUpdateEventArgs e )
    {
    }

    protected void GridViewMembers_RowDataBound( object sender, GridViewRowEventArgs e )
    {
        ComputeColumns();
    }

    protected void AddGlyph( GridView grid, GridViewRow item )
    {
        Image glyph = new Image();
        glyph.EnableTheming = false;
        glyph.ImageAlign = ImageAlign.Bottom;
        glyph.ImageUrl = string.Format( "~/App_Themes/" + Page.Theme.ToString() + "/images/move{0}.gif", ( string )( /*grid.SortDirection*/ sortDirection == SortDirection.Ascending ? "down" : "up" ) );

        int i;
        string colExpr;
        for ( i = 0;i <= grid.Columns.Count - 1;i++ )
        {
            colExpr = grid.Columns[ i ].SortExpression;
            if ( colExpr != "" && colExpr == /*grid.SortExpression*/ sortExpression )
            {
                item.Cells[ i ].Controls.Add( glyph );
            }
        }
    }

    protected void GridViewMembers_PageIndexChanged( object sender, System.EventArgs e )
    {
    }

    protected void GridViewMembers_OnDataBound( object sender, System.EventArgs e )
    {
    }

    protected void GridViewMembers_OnSorted( object sender, EventArgs e )
    {
        GridViewMembers.DataBind();
    }

    protected void GridViewMembers_OnSorting( object sender, GridViewSortEventArgs e )
    {
        sortExpression = e.SortExpression;
        GridViewMembers.DataSource = GetSortedData( e.SortExpression, /*e.SortDirection*/ sortDirection );
        sortDirection = sortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
    }

    // Se declenche quand on clique sur les boutons edit/delete/update/cancel
    protected void GridViewMembers_RowCommand( object sender, GridViewCommandEventArgs e )
    {
        if ( e.CommandName == "Select" )
        {
            int index = Convert.ToInt32( e.CommandArgument );
            GridView gv = ( GridView )e.CommandSource;
            string MembreGUID = gv.DataKeys[ index ].Value.ToString();
            SqlDataSourceMembreQuestionnaire.SelectCommand = string.Format( "SELECT Description, CodeAcces, QuestionnaireID, DateCreation FROM Questionnaire WHERE MembreGUID = '{0}' ORDER BY DateCreation DESC", MembreGUID );
            DataListMembreQuestionnaire.DataBind();
        }
    }

    // Formater et Calculer les elements de la DataList
    protected void DataListMembreQuestionnaire_ItemDataBound( object sender, DataListItemEventArgs e )
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

