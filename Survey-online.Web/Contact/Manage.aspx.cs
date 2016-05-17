//
// Ne jamais oublier que l'evenement DropDownListQuestionnaire_SelectedIndexChanged a lieu
// apres le Page_Load( IsPostBack == true ) ainsi pour determiner SelectCommand on ne peut
// pas se servir des variables positionnees sur le DropDownListQuestionnaire_SelectedIndexChanged
// qui ont un coup de retard.
//
// Encore des heures passees pour valider ce formulaire et utiliser correctement l'objet pourri
// SqlDataSourcePersonne :
// Il faut que SelectCommand="" soit remplie dans la page .aspx au depart
// en suite ne pas faire le :
// SqlDataSourcePersonne.SelectCommand = SelectCommand();
// GridViewContacts.DataBind();
// dans le Page_Load()
// que dans les evenements lies comme DropDownListQuestionnaire_SelectedIndexChanged
// Si c'est dans le Page_Load() :
//      SqlDataSourcePersonne.SelectCommand = SelectCommand();
//      GridViewContacts.DataBind();
// On a une erreur de merde dans l'edition d'une ligne apres Sauvegarde
// Si SelectCommand n'est pas remplie on a la meme erreur de merde qui dit qu'il faut mettre
// EnableEventValidation="false" dans la page grosse daube ca marchera plus jamais
// En suite j'ai cru que le <asp:CommandField CausesValidation="false" 
// avait resolut mon probleme grosse daube
// Et pour finir le clou j'avais ecri SET [PersonneNom] = [@PersonneNom] qui me renvoyait un :
// @PersonneNom nom de Colonne inconnue !!
//
// D'une manière générale les objets APS.NET de Microsoft lorsqu'il ne sont pas exactement utiliser
// a la maniere de Microsoft on un comportement heratique et genere des erreurs pourries !
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
using Sql.Web.Data;
using ImportExportContact;

public partial class Contact_Manage : PageBase
{
    // BUG10092009
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

    private bool ValiderContactsImportes
    {
        get
        {
            if ( ViewState[ "ValiderContactsImportes" ] == null )
            {
                ViewState[ "ValiderContactsImportes" ] = false;
            }
            return ( bool )ViewState[ "ValiderContactsImportes" ];
        }
        set { ViewState[ "ValiderContactsImportes" ] = value; }
    }

    protected void Page_Load( object sender, System.EventArgs e )
    {
        Trace.Warn( "Page_Load:Begin" );

        ValidationMessage.Text = "";
        LabelValidationMassageGridView.Text = "";

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
            CheckBoxListChoixParsing.Items.Add( "CSV" );
            CheckBoxListChoixParsing.Items.Add( "Outlook" );
        }
        
        if ( IsPostBack == false )
        {
            /* Importer des contacts */
            if ( Page.Request[ "cmd" ] == "parse" )
            {
                string erreurMessage = "";
                int countPersonne = -1;
                int countEmailNonValide = 0;
                if ( ValiderContactsImportes == true )
                {
                    erreurMessage += "Vous devez d'abord valider les Contacts importés.";
                }
                else
                {
                    if ( SessionState.Questionnaire == null )
                    {
                        erreurMessage += "Choisir un Questionnaire.";
                    }
                    else
                    {
                        string filename = Page.Request[ "file" ].ToString();
                        bool parseCSV = CheckBoxListChoixParsing.TrueSelectedValue == "CSV" ? true : false;
                        PersonneCollection personnes = new PersonneCollection();
                        if ( parseCSV == true )
                        {
                            erreurMessage = CarnetAdresse.ImportFile( filename, ref personnes );
                        }
                        else
                        {
                            erreurMessage = CarnetOutlook.ImportFile( filename, ref personnes );
                        }

                        // Traiter l'erreur d'importation
                        if ( erreurMessage == "" )
                        {
                            countPersonne = 0; // L'import s'est bien passe
                        }
                        else
                        {
                            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                        }

                        // BRY00020100209
                        if ( SessionState.Limitations.LimitesInterviewesAtteinte( personnes.Count ) )
                        {
                            Tools.PageValidation( "La limite du nombre d'Interviewés : " + SessionState.Limitations.NombreInterviewes + " est atteinte.<br/>Contactez l'administrateur." );
                        }

                        foreach ( Personne p in personnes )
                        {
                            // Corriger les petites merdouilles d'Outlook (pour outlook c'est adr1 adr2 adr3 ...
                            // et oui la rubrique "Classer sous" peut contenir une ',' qui met a mal l'algo !
                            if ( ( p.EmailBureau == "" || !p.EmailBureau.Contains("@") ) && p.EmailPerso != "" )
                            {
                                p.EmailBureau = p.EmailPerso;
                            }
                            p.EmailBureau = p.EmailBureau.Trim();
                            p.TelephonePerso = p.TelephonePerso.Trim();
                            p.TelephoneBureau = p.TelephoneBureau.Trim();
                            if ( p.TelephonePerso == string.Empty && p.TelephoneBureau != string.Empty )
                            {
                                p.TelephonePerso = p.TelephoneBureau;
                            }
                            p.CodeAcces = SessionState.Questionnaire.CodeAcces;
                            p.QuestionnaireID = SessionState.Questionnaire.QuestionnaireID;
                            p.PersonneGUID = Guid.NewGuid();

                            int status = Personne.Create( p, false, ref erreurMessage );
                            if ( status == 1 )
                            {
                                countPersonne += 1;
                            }
                            else
                            {
                                countEmailNonValide += 1;
                            }
                        }

                        // A la fin de la sequence d'importation l'url contient les chaines cmd et file
                        // je ne trouve que ce moyen pour obliger l'url a se cleaner sinon 
                        // on recommence le parsing indefiniment
                        ValiderContactsImportes = true;
                    }
                }
                PanelValiderContactImport.Visible = ValiderContactsImportes;

                // Interpreter le message d'erreur, 
                // s'il n'est pas vide il contient les contacts que l'on n'a pas pu importer
                if ( erreurMessage != "" )
                {
                    ValidationMessage.Visible = true;
                    ValidationMessage.Text = erreurMessage;
                }
                else
                {
                    ValidationMessage.Visible = true;
                }

                if ( countPersonne != -1 )
                {
                    ValidationMessage.Text += "<br/>Contacts importés avec succès : " + countPersonne.ToString();
                    // Si, malgres tout on a reussi a importer des contacts on l'ecrit en bleu
                    ValidationMessage.CssClass = "LabelValidationMessageStyle";
                    SessionState.Limitations.AjouterInterviewes( countPersonne );
                }

                if ( countEmailNonValide > 0 )
                {
                    ValidationMessage.Text += "<br/>Contacts non valides : " + countEmailNonValide.ToString();
                }

            }/* Importer des contacts */

            if ( SessionState.Questionnaire == null )
            {
                LabelCodeAccess.Text = "Invalide";
                LabelQuestionnaire.Text = "";
                // Pas de Questionnaire, pas de bouton nouveau contact
                RolloverLinkEdit.Visible = false;
            }
            else
            {
                LabelCodeAccess.Text = SessionState.Questionnaire.CodeAcces.ToString();
                LabelQuestionnaire.Text = SessionState.Questionnaire.Description;
                BloquerQuestionnaire( SessionState.Questionnaire.Bloque );
            }

            /* L'alphabet */
            DropDownListLettre.Items.Add( "---" );
            string _alpha = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] alpha = _alpha.Split( ',' );
            foreach ( string s in alpha )
            {
                DropDownListLettre.Items.Add( s );
            }

            GridViewContacts.PageSize = SessionState.ContactsParPage;
            GridViewContacts.PageIndex = CurrentPageIndex;
            TextBoxContactsParPage.Text = SessionState.ContactsParPage.ToString();
            SqlDataSourcePersonne.SelectCommand = SelectCommand();
            GridViewContacts.DataBind();

            SessionState.Personnes = null; // BUG080909 forcer le recalcul de SessionState.Personnes.Count
        }
        // BUG1107090001
        LabelNombreContacts.Text = SessionState.Personnes == null ? "0" : SessionState.Personnes.Count.ToString();

        Trace.Warn( "Page_Load:End" );
    }

    string SelectCommand()
    {
        if ( SessionState.Questionnaires.Count <= 0 )
        {
            return "";
        }

        string sqlCommand = "SELECT [ID_Personne], [PersonneCivilite], [PersonneNom], [PersonnePrenom], [PersonneEmailBureau], [PersonneTelephonePerso], [PersonneSociete], [PersonneCode] FROM [Personne]";
        sqlCommand += " WHERE QuestionnaireID = '" + DropDownListQuestionnaire.QuestionnaireID + "'";
        if ( DropDownListLettre.SelectedValue != "---" )
        {
            sqlCommand += " AND UPPER(PersonneNom) LIKE '" + DropDownListLettre.SelectedValue + "%'";
        }
        return sqlCommand;
    }

    private void BloquerQuestionnaire( bool bloque )
    {
        if ( bloque )
        {
            Tools.PageValidation( "Le questionnaire \"" + SessionState.Questionnaire.Description + "\" est clôturé." );
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
        TextBoxContactsParPage.Text = SessionState.ContactsParPage.ToString();
    }

    protected void DropDownListQuestionnaire_SelectedIndexChanged( object sender, EventArgs e )
    {
        int questionnaireID = DropDownListQuestionnaire.QuestionnaireID;
        SessionState.Questionnaire = Questionnaire.GetQuestionnaire( questionnaireID );
        LabelCodeAccess.Text = SessionState.Questionnaire.CodeAcces.ToString();
        LabelQuestionnaire.Text = SessionState.Questionnaire.Description;

        BloquerQuestionnaire( SessionState.Questionnaire.Bloque );

        SqlDataSourcePersonne.SelectCommand = SelectCommand();
        GridViewContacts.DataBind();
        SessionState.Personnes = null;
        LabelNombreContacts.Text = SessionState.Personnes.Count.ToString();
    }

    protected void ButtonValider_Click( object sender, EventArgs e )
    {
        ValiderContactsImportes = false;
        Response.Redirect( "~/Contact/Manage.aspx", true );
    }

    protected void DropDownListLettre_SelectedIndexChanged( object sender, EventArgs e )
    {
        SqlDataSourcePersonne.SelectCommand = SelectCommand();
        GridViewContacts.DataBind();
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

    protected void GridViewContacts_PageIndexChanged( object sender, System.EventArgs e )
    {
        CurrentPageIndex = GridViewContacts.PageIndex;

        SqlDataSourcePersonne.SelectCommand = SelectCommand();
        GridViewContacts.DataBind();
    }

    protected void GridViewContacts_OnDataBound( object sender, System.EventArgs e )
    {
    }

    protected void GridViewContacts_OnSorted( object sender, EventArgs e )
    {
        GridViewContacts.PageIndex = CurrentPageIndex;
    }

    // Il faut laisser cet evenement sinon on a une erreur evenement non gere !
    protected void GridViewContacts_OnSorting( object sender, GridViewSortEventArgs e )
    {
    }

    protected void GridViewContacts_RowUpdating( object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e )
    {
        if ( e.NewValues[ "PersonneCivilite" ] == null )
        {
            e.NewValues[ "PersonneCivilite" ] = "";
        }

        if ( e.NewValues[ "PersonneNom" ] == null )
        {
            e.NewValues[ "PersonneNom" ] = e.OldValues[ "PersonneNom" ];
        }

        if ( e.NewValues[ "PersonnePrenom" ] == null )
        {
            e.NewValues[ "PersonnePrenom" ] = e.OldValues[ "PersonnePrenom" ];
        }

        if ( e.NewValues[ "PersonneSociete" ] == null )
        {
            e.NewValues[ "PersonneSociete" ] = string.Empty;
        }

        SqlDataSourcePersonne.SelectCommand = SelectCommand();
        GridViewContacts.DataBind();
    }

    protected void GridViewContacts_RowCommand( object sender, GridViewCommandEventArgs e )
    {
        if ( e.CommandName == "Delete" )
        {
            LabelValidationMassageGridView.CssClass = "LabelValidationMessageStyle";
            LabelValidationMassageGridView.Visible = true;

            // Supprime les votes associes au contact mais pas le contact lui meme qui est supprime
            // par le SqlDataSource
            int index = Convert.ToInt32( e.CommandArgument );
            GridView gv = ( GridView )e.CommandSource;
            string personneID = gv.DataKeys[ index ].Value.ToString();
            int pID = int.Parse( personneID );
            Personne personne = Personne.Get( pID );
            PollVoteCollection pvc = PollVoteCollection.GetPollVotes( SessionState.Questionnaire.QuestionnaireID, personne.PersonneGUID );
            int status = 0;
            foreach ( PollVote pv in pvc )
            {
                status += PollVote.Delete( pv.VoteId );
            }
            string str = " votes supprimés : " + pvc.Count;
            string statusString = status.ToString() == "0" ? " status : Ok" : " status  : Ko";
            LabelValidationMassageGridView.Text += "Contact supprimé : " + personne.Civilite + "/" + personne.Nom + "/" + personne.Prenom + "/" + personne.EmailBureau + str + statusString + "<br />";
            SessionState.Limitations.SupprimerInterviewes( 1 );
        }

        SqlDataSourcePersonne.SelectCommand = SelectCommand();
        GridViewContacts.DataBind();

        // BUG080909 recalculer LabelNombreContacts apres le SqlDataSourcePersonne.SelectCommand
        // sinon le resultat est faux
        // NON ca ne marche pas meme GridViewContacts.Rows n'est pas a jour ici 
        // et on ne va pas refaire un PostBack pour mettre a jour l'interface !!!
        //SessionState.Personnes = null;
        //LabelNombreContacts.Text = SessionState.Personnes.Count.ToString();
    }

    // Format de la Liste des Contacts :
    // NomComplet (adress@email.com); ...
    private bool ContactValideFaconOutlook( string contact )
    {
        if (    contact.Contains( "(" ) == true
             && contact.Contains( ")" ) == true
             && contact.Contains( "@" ) == true
             && contact.Contains( "." ) == true
           )
        {
            return true;
        }
        return false;
    }

    // adress@email.com\r\n
    private bool ContactValideFaconListe( string contact )
    {
        if ( contact.Contains( "@" ) == true
             && contact.Contains( "." ) == true
           )
        {
            return true;
        }
        return false;
    }

    private bool ValiderSeparateur( string contact )
    {
        if (    ( contact.Contains( ";" ) == true && contact.Contains( "\r\n" ) == true )
             || ( contact.Contains( ";" ) == true && contact.Contains( "\r" ) == true )
           )
        {
            return false;
        }
        return true;
    }

    protected void ButtonAjouterContacts_Click( object sender, EventArgs e )
    {
        ValidationMessage.Text = "";
        ValidationMessage.CssClass = "LabelValidationMessageStyle";
        ValidationMessage.Visible = true;

        if ( TextBoxContactAjouter.Text.Trim() != "" )
        {
            int countPersonne = 0;
            int countEmailNonValide = 0;

            if ( ValiderSeparateur( TextBoxContactAjouter.Text ) == false )
            {
                ValidationMessage.Text = "Erreur de séparateur : votre liste de contacts contient à la fois le séparateur \";\" et des retours chariots.";
                ValidationMessage.Text += "<br/>Vous devez nettoyer votre liste.";
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                return;
            }
            
            // Tester le format
            if ( ContactValideFaconOutlook( TextBoxContactAjouter.Text ) )
            {
                string[] separateur = { ";" };
                string[] contacts = TextBoxContactAjouter.Text.Split( separateur, StringSplitOptions.RemoveEmptyEntries );
                
                // Ici, on peut encore se faire avoir si la liste contient les deux formats
                string[] separateur2 = { "\r\n" };
                string[] contacts2 = TextBoxContactAjouter.Text.Split( separateur2, StringSplitOptions.RemoveEmptyEntries );
                if ( contacts2.Length > contacts.Length )
                {
                    ValidationMessage.Text = "Erreur de format : vous utilisez le format \"nom complet (adresse@email.com)\" avec le séprateur retour chariot";
                    ValidationMessage.Text += "<br/>Vous devez nettoyer votre liste.";
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                    return;
                }

                // Encore une verification, trop d'entree ici fait ramer le serveur d'une façon peu recommandable
                if ( contacts.Length > int.Parse( Global.SettingsXml.LimitationImportsInterviewes ) )
                {
                    ValidationMessage.Text = "Erreur : Taille maximum de la liste d'imports atteinte : " + Global.SettingsXml.LimitationImportsInterviewes;
                    ValidationMessage.Text += "<br/>Vous devez réduire votre liste.";
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                    return;
                }

                //BRY00020100209
                if ( SessionState.Limitations.LimitesInterviewesAtteinte( contacts.Length ) )
                {
                    Tools.PageValidation( "La limite du nombre d'Interviewés : " + SessionState.Limitations.NombreInterviewes + " est atteinte.<br/>Contactez l'administrateur." );
                }

                // Trim()
                int i = 0;
                foreach ( string s in contacts )
                {
                    contacts[ i ] = s.Trim();
                    i += 1;
                }

                foreach ( string contact in contacts )
                {
                    if ( ContactValideFaconOutlook( contact ) )
                    {
                        string[] nomComplet = contact.Split( '(' );

                        int idx1 = contact.IndexOf( '(' ) + 1;
                        int idx2 = contact.IndexOf( ')' );
                        string email = contact.Substring( idx1, idx2 - idx1 );

                        if ( Strings.IsValideEmail( email.Trim() ) )
                        {
                            Personne personne = new Personne();
                            personne.Nom = nomComplet[ 0 ].Trim();
                            personne.EmailBureau = email.Trim();

                            personne.CodeAcces = SessionState.Questionnaire.CodeAcces;
                            personne.QuestionnaireID = SessionState.Questionnaire.QuestionnaireID;
                            personne.PersonneGUID = Guid.NewGuid();

                            string message = string.Empty;
                            int status = Personne.Create( personne, false, ref message );
                            if ( status == 1 )
                            {
                                countPersonne += 1;
                            }
                            else
                            {
                                ValidationMessage.Text += message;
                                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                            }
                        }
                        else
                        {
                            ValidationMessage.Text += email.Trim() + " n'est pas un email valide<br/>";
                            countEmailNonValide += 1;
                        }
                    }
                    else
                    {
                        ValidationMessage.Text += contact + " n'est pas un format valide<br/>";
                    }
                }
            }
            else if ( ContactValideFaconListe( TextBoxContactAjouter.Text ) )
            {
                string contactAjouter = TextBoxContactAjouter.Text;
                string separateurDOS = "\r\n";
                string separateurUnix = "\r";
                if ( contactAjouter.Contains( separateurDOS ) == false )
                {
                    if ( contactAjouter.Contains( separateurUnix ) == true )
                    {
                        ValidationMessage.Text += "Contacts au format Unix à importer<br/>";
                        contactAjouter = Strings.UnixToDos( contactAjouter );
                    }
                }

                string[] separateur = { separateurDOS };
                string[] contacts = TextBoxContactAjouter.Text.Split( separateur, System.StringSplitOptions.RemoveEmptyEntries );

                // Encore une verification, trop d'entree ici fait ramer le serveur d'une façon peu recommandable
                if ( contacts.Length > int.Parse( Global.SettingsXml.LimitationImportsInterviewes ) )
                {
                    ValidationMessage.Text = "Erreur : Taille maximum de la liste d'imports atteinte : " + Global.SettingsXml.LimitationImportsInterviewes;
                    ValidationMessage.Text += "<br/>Vous devez réduire votre liste.";
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                    return;
                }

                //BRY00020100209
                if ( SessionState.Limitations.LimitesInterviewesAtteinte( contacts.Length ) )
                {
                    Tools.PageValidation( "La limite du nombre d'Interviewés : " + SessionState.Limitations.NombreInterviewes + " est atteinte.<br/>Contactez l'administrateur." );
                }

                // Trim()
                int i = 0;
                foreach ( string s in contacts )
                {
                    contacts[ i ] = s.Trim();
                    i += 1;
                }

                foreach ( string contact in contacts )
                {
                    if ( Strings.IsValideEmail( contact ) )
                    {
                        Personne personne = new Personne();
                        personne.Nom = "";
                        personne.EmailBureau = contact;

                        personne.CodeAcces = SessionState.Questionnaire.CodeAcces;
                        personne.QuestionnaireID = SessionState.Questionnaire.QuestionnaireID;
                        personne.PersonneGUID = Guid.NewGuid();

                        string message = string.Empty;
                        int status = Personne.Create( personne, false, ref message );
                        if ( status == 1 )
                        {
                            countPersonne += 1;
                        }
                        else
                        {
                            ValidationMessage.Text += message;
                            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                        }
                    }
                    else
                    {
                        ValidationMessage.Text += contact + " n'est pas un email valide<br/>";
                        countEmailNonValide += 1;
                    }
                }
            }
            else if ( TextBoxContactAjouter.Text.Length > 9 && TextBoxContactAjouter.Text.Substring( 0, 9 ) == "Téléphone" )
            {
                string contactAjouter = TextBoxContactAjouter.Text;
                string separateurDOS = "\r\n";
                string separateurUnix = "\r";
                if ( contactAjouter.Contains( separateurDOS ) == false )
                {
                    if ( contactAjouter.Contains( separateurUnix ) == true )
                    {
                        ValidationMessage.Text += "Contacts au format Unix à importer<br/>";
                        contactAjouter = Strings.UnixToDos( contactAjouter );
                    }
                }

                // Retirer la chaine "Téléphone"
                contactAjouter = contactAjouter.Substring( 9 + separateurDOS.Length );

                string[] separateur = { separateurDOS };
                string[] contacts = contactAjouter.Split( separateur, System.StringSplitOptions.RemoveEmptyEntries );

                // Encore une verification, trop d'entree ici fait ramer le serveur d'une façon peu recommandable
                if ( contacts.Length > int.Parse( Global.SettingsXml.LimitationImportsInterviewes ) )
                {
                    ValidationMessage.Text = "Erreur : Taille maximum de la liste d'imports atteinte : " + Global.SettingsXml.LimitationImportsInterviewes;
                    ValidationMessage.Text += "<br/>Vous devez réduire votre liste.";
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                    return;
                }

                //BRY00020100209
                if ( SessionState.Limitations.LimitesInterviewesAtteinte( contacts.Length ) )
                {
                    Tools.PageValidation( "La limite du nombre d'Interviewés : " + SessionState.Limitations.NombreInterviewes + " est atteinte.<br/>Contactez l'administrateur." );
                }

                // Trim()
                int i = 0;
                foreach ( string s in contacts )
                {
                    contacts[ i ] = s.Trim();
                    i += 1;
                }

                foreach ( string contact in contacts )
                {
                    if ( Strings.IsValideTelephone( contact ) )
                    {
                        Personne personne = new Personne();
                        personne.Nom = "";
                        personne.TelephonePerso = contact;

                        personne.CodeAcces = SessionState.Questionnaire.CodeAcces;
                        personne.QuestionnaireID = SessionState.Questionnaire.QuestionnaireID;
                        personne.PersonneGUID = Guid.NewGuid();

                        string message = string.Empty;
                        int status = Personne.Create( personne, false, ref message );
                        if ( status == 1 )
                        {
                            countPersonne += 1;
                        }
                        else
                        {
                            ValidationMessage.Text += message;
                            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                        }
                    }
                    else
                    {
                        ValidationMessage.Text += contact + " n'est pas un numéro de téléphone valide<br/>";
                        countEmailNonValide += 1;
                    }
                }
            }
            else if ( TextBoxContactAjouter.Text.Length > 14 && TextBoxContactAjouter.Text.Substring( 0, 14 ) == "Nom[Téléphone]" )
            {
                string contactAjouter = TextBoxContactAjouter.Text;
                string separateurDOS = "\r\n";
                string separateurUnix = "\r";
                if ( contactAjouter.Contains( separateurDOS ) == false )
                {
                    if ( contactAjouter.Contains( separateurUnix ) == true )
                    {
                        ValidationMessage.Text += "Contacts au format Unix à importer<br/>";
                        contactAjouter = Strings.UnixToDos( contactAjouter );
                    }
                }

                // Retirer la chaine "Téléphone"
                contactAjouter = contactAjouter.Substring( 14 + separateurDOS.Length );

                string[] separateur = { separateurDOS };
                string[] contacts = contactAjouter.Split( separateur, StringSplitOptions.RemoveEmptyEntries );

                // Encore une verification, trop d'entree ici fait ramer le serveur d'une façon peu recommandable
                if ( contacts.Length > int.Parse( Global.SettingsXml.LimitationImportsInterviewes ) )
                {
                    ValidationMessage.Text = "Erreur : Taille maximum de la liste d'imports atteinte : " + Global.SettingsXml.LimitationImportsInterviewes;
                    ValidationMessage.Text += "<br/>Vous devez réduire votre liste.";
                    ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                    return;
                }

                //BRY00020100209
                if ( SessionState.Limitations.LimitesInterviewesAtteinte( contacts.Length ) )
                {
                    Tools.PageValidation( "La limite du nombre d'Interviewés : " + SessionState.Limitations.NombreInterviewes + " est atteinte.<br/>Contactez l'administrateur." );
                }

                // Trim()
                int i = 0;
                foreach ( string s in contacts )
                {
                    contacts[ i ] = s.Trim();
                    i += 1;
                }

                foreach ( string contact in contacts )
                {
                    if ( contact.Contains( "[" ) == true && contact.Contains( "]" ) == true )
                    {
                        string[] nomComplet = contact.Split( '[' );

                        int idx1 = contact.IndexOf( '[' ) + 1;
                        int idx2 = contact.IndexOf( ']' );
                        string telephone = contact.Substring( idx1, idx2 - idx1 );

                        if ( Strings.IsValideTelephone( telephone.Trim() ) )
                        {
                            Personne personne = new Personne();
                            personne.Nom = nomComplet[ 0 ].Trim();
                            personne.TelephonePerso = telephone.Trim();

                            personne.CodeAcces = SessionState.Questionnaire.CodeAcces;
                            personne.QuestionnaireID = SessionState.Questionnaire.QuestionnaireID;
                            personne.PersonneGUID = Guid.NewGuid();

                            string message = string.Empty;
                            int status = Personne.Create( personne, false, ref message );
                            if ( status == 1 )
                            {
                                countPersonne += 1;
                            }
                            else
                            {
                                ValidationMessage.Text += message;
                                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                            }
                        }
                        else
                        {
                            ValidationMessage.Text += telephone.Trim() + " n'est pas un numéro de téléphone valide<br/>";
                            countEmailNonValide += 1;
                        }
                    }
                    else
                    {
                        ValidationMessage.Text += contact + " n'est pas un format valide<br/>";
                    }
                }
            }
            else
            {
                ValidationMessage.Text = "Ce n'est pas un format de contact valide";
                ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
            }

            ValidationMessage.Text += "<br/>Contacts importés avec succès : " + countPersonne.ToString();
            SessionState.Limitations.AjouterInterviewes( countPersonne );

            if ( countEmailNonValide > 0 )
            {
                ValidationMessage.Text += "<br/>Contacts non valides : " + countEmailNonValide.ToString();
            }
        }
        else
        {
            ValidationMessage.Text = "La liste des contacts est vide";
            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
        }

        SessionState.Personnes = null; // BUG080909 forcer le recalcul de SessionState.Personnes.Count
        LabelNombreContacts.Text = SessionState.Personnes.Count.ToString();

        SqlDataSourcePersonne.SelectCommand = SelectCommand();
        GridViewContacts.DataBind();
    }

    protected void ImageButtonSupprimerListeContacts_Click( object sender, ImageClickEventArgs e )
    {
        ValidationMessage.Text = "";
        ValidationMessage.CssClass = "LabelValidationMessageStyle";
        ValidationMessage.Visible = true;

        int countPersonneSupprime = 0;
        int countContactNonValide = 0;
        int status = 0;

        // Tester le format
        if ( ContactValideFaconListe( TextBoxContactAjouter.Text ) )
        {
            string contactSupprimer = TextBoxContactAjouter.Text;
            string separateurDOS = "\r\n";
            string separateurUnix = "\r";
            if ( contactSupprimer.Contains( separateurDOS ) == false )
            {
                if ( contactSupprimer.Contains( separateurUnix ) == true )
                {
                    ValidationMessage.Text += "Contacts au format Unix à supprimer";
                    contactSupprimer = Strings.UnixToDos( contactSupprimer );
                }
            }

            string[] separateur = { separateurDOS };
            string[] contacts = TextBoxContactAjouter.Text.Split( separateur, System.StringSplitOptions.RemoveEmptyEntries );

            // Trim()
            int i = 0;
            foreach ( string s in contacts )
            {
                contacts[ i ] = s.Trim();
                i += 1;
            }

            PersonneCollection personneCollection = PersonneCollection.GetQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
            PollVoteCollection pollVotes = PollVoteCollection.GetPollVotesByQuestionnaireID( SessionState.Questionnaire.QuestionnaireID );
            foreach ( string contact in contacts )
            {
                if ( Strings.IsValideEmail( contact ) || Strings.IsValideTelephone( contact ) )
                {
                    Personne personneTrouve = null;
                    foreach ( Personne p in personneCollection )
                    {
                        if ( p.EmailBureau == contact || p.TelephonePerso == contact )
                        {
                            personneTrouve = p;
                            break;
                        }
                    }

                    if ( personneTrouve != null )
                    {
                        PollVoteCollection pvc = pollVotes.FindByPersonneGUID( personneTrouve.PersonneGUID );

                        // A t il deja vote pour ce questionnaire ?
                        if ( pvc.Count > 0 )
                        {
                            ValidationMessage.Text += personneTrouve.EmailBureau;
                            if ( ValidationMessage.Text != string.Empty )
                            {
                                ValidationMessage.Text += "/" + personneTrouve.TelephonePerso;
                            }
                            else
                            {
                                ValidationMessage.Text += personneTrouve.TelephonePerso;
                            }
                            ValidationMessage.Text += " a déjà voté<br />";
                        }
                        else
                        {
                            status += Personne.Delete( personneTrouve.ID_Personne );
                            if ( status != 0 )
                            {
                                ValidationMessage.Text += "Erreur à la suppression du contact : " + personneTrouve.EmailBureau + "/" + personneTrouve.TelephonePerso + " status : " + status.ToString() + "<br />";
                            }
                            countPersonneSupprime += 1;
                        }
                    }
                }
                else
                {
                    ValidationMessage.Text += contact + " n'est pas un contact valide<br/>";
                    countContactNonValide += 1;
                }

            }// fin du foreach ( string contact in contacts )

            ValidationMessage.Text += "<br/>Contacts supprimés avec succès : " + countPersonneSupprime.ToString() + "<br/>";
            SessionState.Limitations.SupprimerInterviewes( countPersonneSupprime );

            SessionState.Personnes = null; // BUG080909 forcer le recalcul de SessionState.Personnes.Count
            LabelNombreContacts.Text = SessionState.Personnes.Count.ToString();

            if ( countContactNonValide > 0 )
            {
                ValidationMessage.Text += "<br/>Contacts non valides : " + countContactNonValide.ToString();
            }

            SqlDataSourcePersonne.SelectCommand = SelectCommand();
            GridViewContacts.DataBind();

        }// fin du if ( ContactValideFaconListe( TextBoxContactAjouter.Text ) )
        else
        {
            ValidationMessage.Text = "Ce n'est pas un format de contact valide";
            ValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
        }
    }
}

