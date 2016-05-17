//
// Difficulté : il faut se retaper toute les relations à la main car elles sont détruites pas la creation
// des objets
//
// Impossible de faire quoi que ce soit avec les objets graphiques du client le HttpContext.Current est perdu
// dès que l'on jump dans AnalyserFichier()
// un appel a Request.Reponse dans AnalyserFichier() cree une erreur grave au niveau de debuggeur et plante
// carrement Visual Studio les process DB20 Wed.Debugg sont à terminer ce qui au bout d'un temps
// plante egalement la machine ! La seule facon c'est donc de demander au client de cliquer pour
// savoir si le traitement est terminé.
//
// La procedure stockee GetMembreDatas et GetMembreQuestionnaireDatas doivent mettre les Objets dans
// l'ordre exacte de l'algorithme
//
// Il faut utiliser un lien href pour mettre target à _blank et télécharger le fichier dans une nouvelle page
// sinon la page expire l'utilisateur est perdu
//
// Soit le fichier a le nom du membre si l'export se fait sur tous les objets
// Soit le fichier a le nom du questionnaire s'il s'agit de l'export d'un questionnaire
// avec la difficulté que dans ce cas certains noms de questionnaires ne vont pas aller exemple des %_ ou %20
// dans le nom du questionnaire posent un probleme.
// Se resoud facilement : il suffit de modifier le nom du questionnaire NON finalement on a filtré
//
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.Linq;
using Sql.Data;
using Sql.Web.Data;
using System.IO;
using System.Threading;

public partial class MemberData_ImportExport : System.Web.UI.Page
{
    private const string repertoire = "~/MemberDataFiles/";
    private string MessageExport
    {
        get { return ( string )Session[ "MemberData_ImportExport_Message" ]; }
        set { Session[ "MemberData_ImportExport_Message" ] = value; }
    }

    private string MessageAnalyse
    {
        get { return ( string )Session[ "MemberData_ImportExport_MessageAnalyse" ]; }
        set { Session[ "MemberData_ImportExport_MessageAnalyse" ] = value; }
    }

    private MemberInfo MemberInfoData
    {
        get { return ( MemberInfo )ViewState[ "CurrentPageIndex" ]; }
        set { ViewState[ "CurrentPageIndex" ] = value; }
    }

    ManualResetEvent EventFinAnalyse = new ManualResetEvent( false );
    ManualResetEvent EventFinImport = new ManualResetEvent( false );
    ManualResetEvent EventFinExport = new ManualResetEvent( false );
    private bool FinTraitement
    {
        get { return ( bool )Session[ "MemberData_ImportExport_FinTraitement" ]; }
        set { Session[ "MemberData_ImportExport_FinTraitement" ] = value; }
    }

    // Passer le nom du fichier du bouton Exporter au bouton Telecharger
    private string NomFichier
    {
        get { return ( string )Session[ "MemberData_ImportExport_NomFichier" ]; }
        set { Session[ "MemberData_ImportExport_NomFichier" ] = value; }
    }

    protected void Page_Load( object sender, EventArgs e )
    {
        if ( IsPostBack == false )
        {
            DropDownListQuestionnaire.SelectedQuestionnaire = "-1";
            if ( User.IsInRole( "Administrateur" ) )
            {
                PanelAdministrateur.Visible = true;
            }
            else
            {
                DropDownListQuestionnaire.SelectedMembreGUID = SessionState.MemberInfo.MembreGUID;
                MemberInfoData = SessionState.MemberInfo;
            }
        }
    }

    protected void DropDownListMembre_SelectedIndexChanged( object sender, EventArgs e )
    {
        string user = DropDownListMembre.SelectedValue.ToString();
        if ( user != "-1" )
        {
            string[] userName = user.Split( '/' );
            MemberInfoData = MemberInfo.GetMemberInfo( userName[ 0 ], userName[ 1 ] );
            DropDownListQuestionnaire.SelectedMembreGUID = MemberInfoData.MembreGUID;

            LabelValidationMessageAdmin.Text = "";
            LabelValidationMessageAdmin.Visible = false;
        }
    }

    protected void ButtonAfficherResultatsExport_OnClick( object sender, EventArgs e )
    {
        if ( Session[ "MemberData_ImportExport_FinTraitement" ] == null )
        {
            MessageExport = "Traitement non commencé.<br/>";
            LabelMessageExport.Visible = true;
            LabelMessageExport.Text = MessageExport;
            return;
        }

        // On en peut pas utiliser EventFinAnalyse.WaitOne() ca ne marche pas
        // Est ce que cela vient du fait que ThreadPool.QueueUserWorkItem() et le 
        // EventFinAnalyse.WaitOne() sont dans la meme fonction ButtonAnalyserImportMemberData_OnClick() ?
        // et qu'ici on est dans un autre contexte ... va savoir
        //
        if ( FinTraitement )
        {
            //ButtonSupprimerMemberData.Visible = true; A cause de l'expiration et de l'Actualisation de la Page 
            // le bouton a tendance a disparaitre donc on le laisse visible

            MessageExport += "Traitement terminé.<br/>";
            string link = Utils.WebSiteUri + repertoire.Substring( 1 ) + NomFichier;
            string lien = string.Format( "<a href=\"{0}\" target=\"_blank\" >{1}</a>", link, NomFichier );
            MessageExport += "<br/>Pour télécharger le fichier, cliquez sur le lien : " + lien + "<br/>";

            PanelExportAttente.Visible = false;
            FinTraitement = false;
        }
        else
        {
            MessageExport += "Traitement non terminé.<br/>";
        }

        LabelMessageExport.Text = MessageExport;
        LabelMessageExport.Visible = true;
    }

    protected void ButtonExportMemberData_Click( object sender, EventArgs e )
    {
        EventFinExport.Reset();
        FinTraitement = false;
        MessageExport = "";
        LabelMessageExport.Text = "";
        //ButtonTelechargerMemberData.Visible = false;
        //ButtonSupprimerMemberData.Visible = false;

        if ( User.IsInRole( "Administrateur" ) && MemberInfoData == null )
        {
            LabelValidationMessageAdmin.Text = "Désolé Admin, sélectionnez un Membre pour exporter des données.";
            LabelValidationMessageAdmin.Visible = true;
            return;
        }

        if ( CheckBoxExportVotes.Checked && CheckBoxExportIntervieves.Checked == false )
        {
            CheckBoxExportIntervieves.Checked = true;
            LabelMessageExport.Text = "Vous ne pouvez pas exporter les votes sans exporter les interviewés.";
            LabelMessageExport.Visible = true;
            return;
        }

        int questionnaireID = 0;

        // Soit le fichier a le nom du membre pour un export total soit le nom du questionnaire
        string nomFichier = MemberInfoData.NomUtilisateur;
        // Un seul Questionnaire est selectionne
        if ( DropDownListQuestionnaire.SelectedIndex > 0 )
        {
            QuestionnaireCollection qc = QuestionnaireCollection.GetQuestionnaireMembre( MemberInfoData.MembreGUID );
            questionnaireID = qc[ DropDownListQuestionnaire.SelectedIndex - 1 ].QuestionnaireID;
            SessionState.Questionnaire = qc[ DropDownListQuestionnaire.SelectedIndex - 1 ];
            // Le nom du fichier est le nom du questionnnaire selectionne
            nomFichier = qc[ DropDownListQuestionnaire.SelectedIndex - 1 ].Description;
        }
        nomFichier = Strings.RemoveIllegalCharacters( nomFichier );

        string fileName = HttpContext.Current.Request.MapPath( repertoire );
        fileName += nomFichier;
        fileName += ".xml";

        // Donner l'info au Bouton ButtonAfficherResultatsExport
        NomFichier = nomFichier + ".xml";

        // -----------------------------
        // ThreadPool.QueueUserWorkItem
        // -----------------------------

        EventFinImport.Reset();
        FinTraitement = false;
        ValidationMessage.Text = "";
        MessageExport = "";

        PanelExportAttente.Visible = true;
        ThreadPool.QueueUserWorkItem( delegate { ExporterFichier( questionnaireID, fileName ); } );

        if ( EventFinExport.WaitOne( 10000, false ) )
        {
            PanelExportAttente.Visible = false;

            MessageExport += "Traitement terminé.<br/>";
            string link = Utils.WebSiteUri + repertoire.Substring( 1 ) + NomFichier;
            string lien = string.Format( "<a href=\"{0}\" target=\"_blank\" >{1}</a>", link, NomFichier );
            MessageExport += "<br/>Pour télécharger le fichier, cliquez sur le lien : " + lien + "<br/>";
        }
        else
        {
            MessageExport += "Traitement non terminé.<br/>";
        }

        LabelMessageExport.Visible = true;
        LabelMessageExport.Text += MessageExport;
        LabelMessageExport.CssClass = "LabelValidationMessageStyle";
    }

    private void ExporterFichier( int questionnaireID, string fileName )
    {
        DateTime dateDebutAnalyse = DateTime.Now;

        DataSet dataSetTotal = new DataSet();
        if ( questionnaireID == 0 )
        {
            dataSetTotal = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                "GetMembreDatas",
                MemberInfoData.MembreGUID,
                CheckBoxExportServeurSmtp.Checked,
                CheckBoxExportIntervieves.Checked,
                CheckBoxExportVotes.Checked
            );
        }
        else
        {
            dataSetTotal = SqlDataProvider.ExecuteDataset
            (
                Tools.DatabaseConnectionString,
                "GetMembreQuestionnaireDatas",
                MemberInfoData.MembreGUID,
                questionnaireID,
                CheckBoxExportServeurSmtp.Checked,
                CheckBoxExportIntervieves.Checked,
                CheckBoxExportVotes.Checked
            );
        }
        dataSetTotal.DataSetName = "MembreData";

        if ( File.Exists( fileName ) )
        {
            File.Delete( fileName );
        }

        // Encore une grosse merde : Impossible de nommer les lignes
        // On ne peut nommer que les tables donc on transforme les 
        // lignes en tables pour les nommer ...
        DataSet dataSetTotalTables = new DataSet( "MembreData" );

        int nbQuestionnaires = 0;
        int nbQuestions = 0;
        int nbInterviewes = 0;
        int nbReponses = 0;
        int nbVotes = 0;
        int nbScores = 0;

        // Nommer les tables pour les distinguer lors de l'Import
        int idx = 1;
        foreach ( DataTable dt in dataSetTotal.Tables )
        {
            if ( dt.Columns[ 0 ].ColumnName == "MembreID" )
            {
                DataTable dt1 = dt.Copy();
                dt1.TableName = "MembreInfo";
                dataSetTotalTables.Tables.Add( dt1 );
            }
            if ( dt.Columns[ 0 ].ColumnName == "QuestionnaireID" )
            {
                foreach ( DataRow dr in dt.Rows )
                {
                    DataTable dt1 = dt.Clone();
                    dt1.TableName = "Questionnaire" + idx.ToString();
                    dt1.Rows.Add( dr.ItemArray );
                    dataSetTotalTables.Tables.Add( dt1 );
                    nbQuestionnaires += 1;
                    idx += 1;
                }
            }
            if ( dt.Columns[ 0 ].ColumnName == "PollQuestionID" )
            {
                foreach ( DataRow dr in dt.Rows )
                {
                    DataTable dt1 = dt.Clone();
                    dt1.TableName = "QQuestion" + idx.ToString(); // Q pour distinguer Question de Questionnaire qui commence paraillent
                    dt1.Rows.Add( dr.ItemArray );
                    dataSetTotalTables.Tables.Add( dt1 );
                    nbQuestions += 1;
                    idx += 1;
                }
            }
            if ( dt.Columns[ 0 ].ColumnName == "PollAnswerId" )
            {
                foreach ( DataRow dr in dt.Rows )
                {
                    DataTable dt1 = dt.Clone();
                    dt1.TableName = "Reponse" + idx.ToString();
                    dt1.Rows.Add( dr.ItemArray );
                    dataSetTotalTables.Tables.Add( dt1 );
                    nbReponses += 1;
                    idx += 1;
                }
            }
            if ( dt.Columns[ 0 ].ColumnName == "ID_Personne" )
            {
                foreach ( DataRow dr in dt.Rows )
                {
                    DataTable dt1 = dt.Clone();
                    dt1.TableName = "Personne" + idx.ToString();
                    dt1.Rows.Add( dr.ItemArray );

                    // compatibilite ascendante des anonymes ont ete enregsitres avec guid a la place de EmailPerso
                    // ajouter @com.com pour en faire une adresse email
                    try
                    {
                        Guid guid = new Guid( dt1.Rows[ 0 ][ "PersonneEmailBureau" ].ToString() );
                        dt1.Rows[ 0 ][ "PersonneEmailBureau" ] = dt1.Rows[ 0 ][ "PersonneEmailBureau" ] + "@a.fr";
                    }
                    catch
                    {
                    }
                    // fin compatibilite ascendante

                    dataSetTotalTables.Tables.Add( dt1 );
                    nbInterviewes += 1;
                    idx += 1;
                }
            }
            if ( dt.Columns[ 0 ].ColumnName == "VoteId" )
            {
                foreach ( DataRow dr in dt.Rows )
                {
                    DataTable dt1 = dt.Clone();
                    dt1.TableName = "Vote" + idx.ToString();
                    dt1.Rows.Add( dr.ItemArray );
                    dataSetTotalTables.Tables.Add( dt1 );
                    nbVotes += 1;
                    idx += 1;
                }
            }
            if ( dt.Columns[ 0 ].ColumnName == "ScoreID" )
            {
                foreach ( DataRow dr in dt.Rows )
                {
                    DataTable dt1 = dt.Clone();
                    dt1.TableName = "Score" + idx.ToString();
                    dt1.Rows.Add( dr.ItemArray );
                    dataSetTotalTables.Tables.Add( dt1 );
                    nbScores += 1;
                    idx += 1;
                }
            }
            if ( dt.Columns[ 0 ].ColumnName == "SmtpServerID" )
            {
                DataTable dt1 = dt.Copy();
                dt1.TableName = "SmtpServeur";
                dataSetTotalTables.Tables.Add( dt1 );
            }
            idx += 1;
        }

        // Sauvegarde des donnees Membre du le fichier .xml
        using ( XmlWriter xmlw = XmlWriter.Create( fileName ) )
        {
            dataSetTotalTables.AcceptChanges();
            dataSetTotalTables.WriteXml( xmlw );
            xmlw.Close();
        }

        MessageExport += "Données membre :<br/>";
        MessageExport += "Questionnaires : " + nbQuestionnaires.ToString() + "<br/>";
        MessageExport += "     Questions : " + nbQuestions.ToString() + "<br/>";
        MessageExport += "      Réponses : " + nbReponses.ToString() + "<br/>";
        MessageExport += "   Interviewes : " + nbInterviewes.ToString() + "<br/>";
        MessageExport += "         Votes : " + nbVotes.ToString() + "<br/>";
        if ( nbScores > 0 )
        {
            MessageExport += "         Scores : " + nbScores.ToString() + "<br/>";
        }
        MessageExport += "<br/>";

        DateTime dateFinAnalyse = DateTime.Now;
        TimeSpan tempsTraitement = dateFinAnalyse - dateDebutAnalyse;
        FileInfo fi = new FileInfo( fileName );
        MessageExport += "- Taille du fichier : " + Tools.FileSizeFormat( fi.Length, "N" ) + "<br/>";
        MessageExport += "- Temps d'export : " + Tools.FormatTimeSpan( tempsTraitement) + "<br/>";
        MessageExport += "<br/>";

        EventFinExport.Set();
        FinTraitement = true;
    }

    /// <summary>
    /// Lors de la creation des Objets, ils se trouvent avec de nouveau ID.
    /// Ici on remet dans le dataSet les nouveaux ID pour que les relations de données soient conservées.
    /// </summary>
    /// <param name="dataSet"></param>
    /// <param name="nomObjet"></param>
    /// <param name="nomColonne"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private void TaggerObjet( ref DataSet dataSet, string nomObjet, string nomColonne, string oldValue, string newValue )
    {
        foreach ( DataTable dt in dataSet.Tables )
        {
            if ( dt.TableName.Contains( nomObjet ) )
            {
                if ( dt.Rows[ 0 ][ nomColonne ].ToString() == oldValue )
                {
                    dt.Rows[ 0 ][ nomColonne ] = newValue;
                }
            }
        }
    }

    protected void ButtonAfficherResultats_OnClick( object sender, EventArgs e )
    {
        if ( Session[ "MemberData_ImportExport_FinTraitement" ] == null )
        {
            MessageAnalyse = "Traitement non commencé.<br/>";
            ValidationMessage.Visible = true;
            ValidationMessage.Text = MessageAnalyse;
            ValidationMessage.CssClass = "LabelValidationMessageStyle";
            return;
        }

        // On en peut pas utiliser EventFinAnalyse.WaitOne() ca ne marche pas
        // Est ce que cela vient du fait que ThreadPool.QueueUserWorkItem() et le 
        // EventFinAnalyse.WaitOne() sont dans la meme fonction ButtonAnalyserImportMemberData_OnClick()
        // et qu'ici on est dans un autre contexte ... va savoir
        //
        if ( FinTraitement )
        {
            MessageAnalyse += "Traitement terminé.<br/>";
            PanelImportAttente.Visible = false;
            FinTraitement = false;
        }
        else
        {
            MessageAnalyse += "Traitement non terminé.<br/>";
        }

        ValidationMessage.Visible = true;
        ValidationMessage.Text = MessageAnalyse;
        ValidationMessage.CssClass = "LabelValidationMessageStyle";
    }

    protected void ButtonAnalyserImportMemberData_OnClick( object sender, EventArgs e )
    {
        EventFinAnalyse.Reset();
        FinTraitement = false;
        ValidationMessage.Text = "";
        MessageAnalyse = "";

        PanelImportAttente.Visible = true;
        ThreadPool.QueueUserWorkItem( delegate { AnalyserFichier(); } );

        if ( EventFinAnalyse.WaitOne( 10000, false ) )
        {
            PanelImportAttente.Visible = false;
            MessageAnalyse += "Traitement terminé.<br/>";
        }
        else
        {
            MessageAnalyse += "Traitement non terminé.<br/>";
        }

        ValidationMessage.Visible = true;
        ValidationMessage.Text += MessageAnalyse;
        ValidationMessage.CssClass = "LabelValidationMessageStyle";
    }

    protected void AnalyserFichier()
    {
        DateTime dateDebutAnalyse = DateTime.Now;

        if ( DocumentNom.PostedFile.FileName == "" )
        {
            MessageAnalyse = "Choisissez un fichier à analyser. ";
            EventFinAnalyse.Set();
            FinTraitement = true;
            return;
        }

        if ( CheckBoxImportVotes.Checked && CheckBoxImportInterviewes.Checked == false )
        {
            CheckBoxExportIntervieves.Checked = true;
            MessageAnalyse = "Vous ne pouvez pas importer les votes sans exporter les interviewés.";
            EventFinAnalyse.Set();
            FinTraitement = true;
            return;
        }

        HttpPostedFile file = DocumentNom.PostedFile;
        int size = file.ContentLength;
        if ( size <= 0 )
        {
            MessageAnalyse = "Un problème est survenu avec le téléchargement du fichier sur le serveur, le fichier n'est pas disponible.";
            EventFinAnalyse.Set();
            FinTraitement = true;
            return;
        }

        String virtualPath = "~/MemberDataFiles/";
        string filePath = virtualPath;
        virtualPath += Path.GetFileName( DocumentNom.PostedFile.FileName );
        String physicalDir = Server.MapPath( filePath );
        String physicalPath = Server.MapPath( virtualPath );

        // Sauver le fichier
        try
        {
            int tailleMax = 10 * 1024 * 1024; // 10 Mo
            if ( DocumentNom.PostedFile.ContentLength >= tailleMax )
            {
                MessageAnalyse += "Erreur taille du fichier : " + Tools.FileSizeFormat( DocumentNom.PostedFile.ContentLength, "N" );
                MessageAnalyse += " supérieure à la taille maximum : " + Tools.FileSizeFormat( tailleMax, "N" );
                EventFinAnalyse.Set();
                FinTraitement = true;
                return;
            }
            DocumentNom.PostedFile.SaveAs( physicalPath );
        }
        catch ( Exception ex )
        {
            MessageAnalyse += ex.Message;
            EventFinAnalyse.Set();
            FinTraitement = true;
            return;
        }

        DataSet dataSet = new DataSet();
        try
        {
            XmlReader xmlr = XmlReader.Create( physicalPath );
            dataSet.ReadXml( xmlr );
            xmlr.Close();
        }
        catch
        {
            MessageAnalyse += "Fichier non valide.";
            EventFinAnalyse.Set();
            FinTraitement = true;
            return;
        }

        // Infos du Membre pour l'administrateur
        if ( User.IsInRole( "Administrateur" ) )
        {
            DataTable dt = dataSet.Tables[ 0 ];
            MessageAnalyse +="Infos du membre :<br/>";
            MessageAnalyse +="Nom d'utilisateur : " + dt.Rows[ 0 ][ "NomUtilisateur" ].ToString() + "<br/>";
            MessageAnalyse +="Mot de passe : " + dt.Rows[ 0 ][ "MotDePasse" ].ToString() + "<br/>";
            MessageAnalyse +="Nom : " + dt.Rows[ 0 ][ "Nom" ].ToString() + "<br/>";
            MessageAnalyse +="Prénom : " + dt.Rows[ 0 ][ "Prenom" ].ToString() + "<br/>";
            MessageAnalyse +="Adresse : " + dt.Rows[ 0 ][ "Adresse" ].ToString() + "<br/>";
            MessageAnalyse +="Téléphone : " + dt.Rows[ 0 ][ "Telephone" ].ToString() + "<br/>";
            MessageAnalyse +="Société : " + dt.Rows[ 0 ][ "Societe" ].ToString() + "<br/>";
            MessageAnalyse +="LimiteQuestionnaires : " + dt.Rows[ 0 ][ "LimiteQuestionnaires" ].ToString() + "<br/>";
            MessageAnalyse +="LimiteQuestions : " + dt.Rows[ 0 ][ "LimiteQuestions" ].ToString() + "<br/>";
            MessageAnalyse +="LimiteInterviewes : " + dt.Rows[ 0 ][ "LimiteInterviewes" ].ToString() + "<br/>";
            MessageAnalyse +="LimiteReponses : " + dt.Rows[ 0 ][ "LimiteReponses" ].ToString() + "<br/>";
            MessageAnalyse +="Date fin d'Abonnement : " + dt.Rows[ 0 ][ "DateFinAbonnement" ].ToString() + "<br/>";
            MessageAnalyse +="<br/>";
        }

        //
        // Questionnaires
        //
        int nbQuestionnaires = 0;
        bool entete = true;
        foreach ( DataTable dt in dataSet.Tables )
        {
            if ( dt.TableName.Contains( "Questionnaire" ) )
            {
                if ( entete )
                {
                    MessageAnalyse +="Questionnaire(s) :<br/>";
                    entete = false;
                }
                Questionnaire questionnaire = Questionnaire.FillFromXML( dt.Rows[ 0 ] );
                MessageAnalyse +=questionnaire.Description + " date de création : " + questionnaire.DateCreation.ToShortDateString() + "<br/>";
                nbQuestionnaires += 1;
            }
            if ( dt.TableName.Contains( "QQuestion" ) )
            {
                // On passe aux Questions
                break;
            }
        }

        //
        // Questions
        //
        int nbQuestions = 0;
        foreach ( DataTable dt in dataSet.Tables )
        {
            if ( dt.TableName.Contains( "QQuestion" ) )
            {
                nbQuestions += 1;
            }
            if ( dt.TableName.Contains( "Reponse" ) )
            {
                break;
            }
        }

        int nbReponses = 0;
        foreach ( DataTable dt in dataSet.Tables )
        {
            if ( dt.TableName.Contains( "Reponse" ) )
            {
                nbReponses += 1;
            }
            if ( dt.TableName.Contains( "Personne" ) )
            {
                break;
            }
        }

        int nbInterviewes = 0;
        foreach ( DataTable dt in dataSet.Tables )
        {
            if ( dt.TableName.Contains( "Personne" ) )
            {
                nbInterviewes += 1;
            }
            if ( dt.TableName.Contains( "Vote" ) )
            {
                break;
            }
        }

        int nbVotes = 0;
        foreach ( DataTable dt in dataSet.Tables )
        {
            if ( dt.TableName.Contains( "Vote" ) )
            {
                nbVotes += 1;
            }
            if ( dt.TableName.Contains( "Score" ) )
            {
                break;
            }
        }

        int nbScores = 0;
        foreach ( DataTable dt in dataSet.Tables )
        {
            if ( dt.TableName.Contains( "Score" ) )
            {
                nbScores += 1;
            }
        }

        MessageAnalyse +="<br/>";
        MessageAnalyse +="Données membre :<br/>";
        MessageAnalyse +="Questionnaires : " + nbQuestionnaires.ToString() + "<br/>";
        MessageAnalyse +="     Questions : " + nbQuestions.ToString() + "<br/>";
        MessageAnalyse +="      Réponses : " + nbReponses.ToString() + "<br/>";
        MessageAnalyse +="   Interviewes : " + nbInterviewes.ToString() + "<br/>";
        MessageAnalyse +="         Votes : " + nbVotes.ToString() + "<br/>";
        if ( nbScores > 0 )
        {
            MessageAnalyse +="         Scores : " + nbScores.ToString() + "<br/>";
        }
        MessageAnalyse +="<br/>";

        bool smtpPresent = false;
        foreach ( DataTable dt in dataSet.Tables )
        {
            if ( dt.TableName.Contains( "SmtpServeur" ) )
            {
                SmtpServer smtp = SmtpServer.FillFromXML( dt.Rows[ 0 ] );
                MessageAnalyse +="Stmp serveur :<br/>";
                MessageAnalyse +="  nom d'utilisateur : " + smtp.UserName + "<br/>";
                MessageAnalyse +="       mot de passe : " + smtp.UserPassWord + "<br/>";
                MessageAnalyse +="      adresse email : " + smtp.Email + "<br/>";
                MessageAnalyse +="     nom du serveur : " + smtp.ServerName + "<br/>";
                smtpPresent = true;
            }
        }
        if ( smtpPresent ) MessageAnalyse +="<br/>";
        
        ValidationMessage.Visible = true;

        File.Delete( physicalPath );

        DateTime dateFinAnalyse = DateTime.Now;
        TimeSpan tempsTraitement = dateFinAnalyse - dateDebutAnalyse;
        MessageAnalyse += "- Taille du fichier : " + Tools.FileSizeFormat( DocumentNom.PostedFile.ContentLength, "N" ) + "<br/>";
        MessageAnalyse += "- Temps d'analyse : " + Tools.FormatTimeSpan( tempsTraitement ) + "<br/>";

        EventFinAnalyse.Set();
        FinTraitement = true;
    }

    protected void ButtonImportMemberData_OnClick( object sender, EventArgs e )
    {
        EventFinImport.Reset();
        FinTraitement = false;
        ValidationMessage.Text = "";
        MessageAnalyse = "";

        PanelImportAttente.Visible = true;
        ThreadPool.QueueUserWorkItem( delegate { ImporterFichier(); } );

        if ( EventFinImport.WaitOne( 10000, false ) )
        {
            PanelImportAttente.Visible = false;
            MessageAnalyse += "Traitement terminé.<br/>";
        }
        else
        {
            MessageAnalyse += "Traitement non terminé.<br/>";
        }

        // La liste des Questionnaire va certainement changer
        // Prevenir les autres composants
        SessionState.Questionnaires = null;

        ValidationMessage.Visible = true;
        ValidationMessage.Text += MessageAnalyse;
        ValidationMessage.CssClass = "LabelValidationMessageStyle";
    }

    private void ImporterFichier()
    {
        DateTime dateDebutAnalyse = DateTime.Now;

        if ( User.IsInRole( "Administrateur" ) && MemberInfoData == null )
        {
            MessageAnalyse = "Désolé Admin, sélectionnez un Membre pour importer des données.";
            EventFinImport.Set();
            FinTraitement = true;
            return;
        }

        if ( DocumentNom.PostedFile.FileName == "" )
        {
            MessageAnalyse = "Choisissez un fichier à importer. ";
            EventFinImport.Set();
            FinTraitement = true;
            return;
        }

        if ( CheckBoxImportVotes.Checked && CheckBoxImportInterviewes.Checked == false )
        {
            CheckBoxExportIntervieves.Checked = true;
            MessageAnalyse = "Vous ne pouvez pas importer les votes sans exporter les interviewés. ";
            EventFinImport.Set();
            FinTraitement = true;
            return;
        }

        HttpPostedFile file = DocumentNom.PostedFile;
        int size = file.ContentLength;
        if ( size <= 0 )
        {
            MessageAnalyse = "Un problème est survenu avec le téléchargement du fichier sur le serveur, le fichier n'est pas disponible. ";
            EventFinImport.Set();
            FinTraitement = true;
            return;
        }

        String virtualPath = "~/MemberDataFiles/";
        string filePath = virtualPath;
        virtualPath += Path.GetFileName( DocumentNom.PostedFile.FileName );
        String physicalDir = Server.MapPath( filePath );
        String physicalPath = Server.MapPath( virtualPath );

        // Sauver le fichier
        try
        {
            int tailleMax = 10 * 1024 * 1024; // 10 Mo
            if ( DocumentNom.PostedFile.ContentLength >= tailleMax )
            {
                MessageAnalyse += "Error taille du fichier : " + Tools.FileSizeFormat( DocumentNom.PostedFile.ContentLength, "N" );
                MessageAnalyse += " supérieure à la taille maximum : " + Tools.FileSizeFormat( tailleMax, "N" );
                EventFinImport.Set();
                FinTraitement = true;
                return;
            }
            DocumentNom.PostedFile.SaveAs( physicalPath );
        }
        catch ( Exception ex )
        {
            MessageAnalyse += ex.Message;
            EventFinImport.Set();
            FinTraitement = true;
            return;
        }

        DataSet dataSet = new DataSet();
        try
        {
            XmlReader xmlr = XmlReader.Create( physicalPath );
            dataSet.ReadXml( xmlr );
            xmlr.Close();
        }
        catch
        {
            MessageAnalyse += "Fichier non valide. ";
            File.Delete( physicalPath );
            EventFinImport.Set();
            FinTraitement = true;
            return;
        }

        // Dans le cadre d'un Tread on ne peut pas utiliser SessionState
        Limitation limitations = new Limitation( MemberInfoData.MembreGUID );

        //
        // Creer les Questionnaires
        //
        foreach ( DataTable dt in dataSet.Tables )
        {
            if ( dt.TableName.Contains( "Questionnaire" ) )
            {
                // Tester les limitations avant d'ajouter le questionnaire
                if ( limitations.LimiteQuestionnaires )
                {
                    MessageAnalyse += "La limite du nombre de Questionnaires : " + limitations.NombreQuestionnaires + " est atteinte.";
                    File.Delete( physicalPath );
                    EventFinImport.Set();
                    FinTraitement = true;
                    return;
                }

                Questionnaire questionnaire = Questionnaire.FillFromXML( dt.Rows[ 0 ] );
                int oldQuestionnaireID = questionnaire.QuestionnaireID;
                string oldCodeAcces = questionnaire.CodeAcces.ToString();

                questionnaire.MembreGUID = MemberInfoData.MembreGUID;
                questionnaire.DateCreation = DateTime.Now; // Modifier la date de creation a maintenant

                ArrayList codes = QuestionnaireDAL.GetCodeAccessAll();
                string codeAcces = Tools.CalculCodeAcces( MemberInfoData.MembreID, codes ).ToString();
                questionnaire.CodeAcces = int.Parse( codeAcces );

                int status = Questionnaire.Create( questionnaire );
                if ( status == 1 )
                {
                    MessageAnalyse += "Questionnaire : " + questionnaire.Description + " créé correctement.<br/>";
                    limitations.AjouterQuestionnaire();
                }
                else if ( status == 2 )
                {
                    MessageAnalyse += "Le Questionnaire : " + questionnaire.Description + " existe déjà.<br>";
                    File.Delete( physicalPath );
                    EventFinImport.Set();
                    FinTraitement = true;
                    return;
                }
                else
                {
                    MessageAnalyse += "Erreur sur la création du Questionnaire : " + questionnaire.Description + " <br/>";
                    File.Delete( physicalPath );
                    EventFinImport.Set();
                    FinTraitement = true;
                    return;
                }

                //
                // Tagger les Objets du Questionnaire avec le nouvel ID
                //
                int newQuestionnaireID = questionnaire.QuestionnaireID;
                string newCodeAcces = codeAcces;

                // Questions
                TaggerObjet( ref dataSet, "QQuestion", "QuestionnaireID", oldQuestionnaireID.ToString(), newQuestionnaireID.ToString() );

                // Interviewe
                TaggerObjet( ref dataSet, "Personne", "QuestionnaireID", oldQuestionnaireID.ToString(), newQuestionnaireID.ToString() );
                TaggerObjet( ref dataSet, "Personne", "PersonneCode", oldCodeAcces, newCodeAcces );

                // Votes
                TaggerObjet( ref dataSet, "Vote", "QuestionnaireID", oldQuestionnaireID.ToString(), newQuestionnaireID.ToString() );

                // Score
                TaggerObjet( ref dataSet, "Score", "ScoreQuestionnaireID", oldQuestionnaireID.ToString(), newQuestionnaireID.ToString() );
            }
            if ( dt.TableName.Contains( "QQuestion" ) )
            {
                // On passe aux Questions
                break;
            }
        }

        //
        // Creer les Questions
        //
        int nbQuestions = 0;
        foreach ( DataTable dt in dataSet.Tables )
        {
            if ( dt.TableName.Contains( "QQuestion" ) )
            {
                // Tester les limitations avant d'ajouter la question
                if ( limitations.LimiteQuestions )
                {
                    MessageAnalyse += "La limite du nombre de Questions : " + limitations.NombreQuestions + " est atteinte.";
                    File.Delete( physicalPath );
                    EventFinImport.Set();
                    FinTraitement = true;
                    return;
                }

                PollQuestion question = PollQuestion.FillFromXML( dt.Rows[ 0 ] );
                string oldQuestionID = question.PollQuestionId.ToString();
                question.MembreGUID = MemberInfoData.MembreGUID;
                int status = PollQuestion.Create( question );
                if ( status != 0 )
                {
                    MessageAnalyse += "Erreur à la création de la Question : " + question.Question + "<br/>";
                    File.Delete( physicalPath );
                    EventFinImport.Set();
                    FinTraitement = true;
                    return;
                }
                else
                {
                    nbQuestions += 1;
                    limitations.AjouterQuestion();
                }

                //
                // Tagger les Objets de la Question avec le nouvel ID
                //
                string newQuestionID = question.PollQuestionId.ToString();

                // Reponse
                TaggerObjet( ref dataSet, "Reponse", "PollQuestionId", oldQuestionID, newQuestionID );

                // Votes
                TaggerObjet( ref dataSet, "Vote", "PollQuestionId", oldQuestionID, newQuestionID );
            }
            if ( dt.TableName.Contains( "Reponse" ) )
            {
                // On passe aux Reponses
                break;
            }
        }
        MessageAnalyse += "Question créées : " + nbQuestions.ToString() + "<br/>";

        int nbReponses = 0;
        foreach ( DataTable dt in dataSet.Tables )
        {
            if ( dt.TableName.Contains( "Reponse" ) )
            {
                PollAnswer reponse = PollAnswer.FillFromXML( dt.Rows[ 0 ] );
                string oldPollAnswerID = reponse.PollAnswerId.ToString();
                int status = PollAnswer.Create( reponse );
                if ( status == 0 )
                {
                    nbReponses += 1;
                }
                else
                {
                    MessageAnalyse += "Erreur à la création de la réponse : " + reponse.Answer;
                    File.Delete( physicalPath );
                    EventFinImport.Set();
                    FinTraitement = true;
                    return;
                }

                //
                // Tagger les Objets de la Reponse avec le nouvel ID
                //
                string newPollAnswerID = reponse.PollAnswerId.ToString();

                // Votes
                TaggerObjet( ref dataSet, "Vote", "PollAnswerId", oldPollAnswerID, newPollAnswerID );
            }
            if ( dt.TableName.Contains( "Personne" ) )
            {
                break;
            }
        }
        MessageAnalyse += "Réponses créés : " + nbReponses.ToString() + "<br/>";

        int nbInterviewes = 0;
        if ( CheckBoxImportInterviewes.Checked )
        {
            foreach ( DataTable dt in dataSet.Tables )
            {
                if ( dt.TableName.Contains( "Personne" ) )
                {
                    string message = "";
                    Personne personne = Personne.FillFromXML( dt.Rows[ 0 ] );
                    string oldPersonneGUID = personne.PersonneGUID.ToString();

                    // Trouver si l'utilisateur de ce questionnaire est limite
                    if ( limitations.LimitesInterviewes )
                    {
                        MessageAnalyse += "La limite du nombre d'Interviewés : " + limitations.NombreInterviewes + " est atteinte.";
                        File.Delete( physicalPath );
                        EventFinImport.Set();
                        FinTraitement = true;
                        return;
                    }

                    int retCode = Personne.Create( personne, true, ref message );
                    if ( retCode == 1 )
                    {
                        nbInterviewes += 1;
                        limitations.AjouterInterviewes( 1 );
                    }
                    else
                    {
                        MessageAnalyse += message;
                        File.Delete( physicalPath );
                        EventFinImport.Set();
                        FinTraitement = true;
                        return;
                    }

                    //
                    // Tagger les Objets de la Reponse avec le nouvel ID
                    //
                    string newPersonneGUID = personne.PersonneGUID.ToString();

                    // Votes
                    TaggerObjet( ref dataSet, "Vote", "UserGUID", oldPersonneGUID, newPersonneGUID );
                }
                if ( dt.TableName.Contains( "Vote" ) )
                {
                    break;
                }
            }
        }
        MessageAnalyse += "Interviewés créés : " + nbInterviewes.ToString() + "<br/>";

        int nbVotes = 0;
        if ( CheckBoxImportVotes.Checked )
        {
            foreach ( DataTable dt in dataSet.Tables )
            {
                if ( dt.TableName.Contains( "Vote" ) )
                {
                    PollVote vote = PollVote.FillFromXML( dt.Rows[ 0 ] );

                    int status = PollVote.Create( vote );
                    if ( status == 2 )
                    {
                        MessageAnalyse += "Vote existe déjà";
                        File.Delete( physicalPath );
                        EventFinImport.Set();
                        FinTraitement = true;
                        return;
                    }
                    else
                    {
                        nbVotes += 1;
                    }
                }
                if ( dt.TableName.Contains( "Score" ) )
                {
                    break;
                }
            }

            if ( limitations.LimitesReponses )
            {
                MessageAnalyse += "La limite du nombre de Réponses : " + limitations.NombreReponses + " est atteinte.";
                File.Delete( physicalPath );
                EventFinImport.Set();
                FinTraitement = true;
                return;
            }
        }
        MessageAnalyse += "Votes créés : " + nbVotes.ToString() + "<br/>";

        int nbScore = 0;
        foreach ( DataTable dt in dataSet.Tables )
        {
            if ( dt.TableName.Contains( "Score" ) )
            {
                Score score = Score.FillFromXML( dt.Rows[ 0 ] );
                int status = Score.Create( score );
                if ( status != 1 )
                {
                    MessageAnalyse += "Erreur à la création du Score .<br/>";
                }
                else
                {
                    nbScore += 1;
                }
            }
        }
        MessageAnalyse += "Score créés : " + nbScore.ToString() + "<br/>";

        if ( CheckBoxImportServeurSmtp.Checked )
        {
            foreach ( DataTable dt in dataSet.Tables )
            {
                if ( dt.TableName.Contains( "SmtpServeur" ) )
                {
                    SmtpServer smtp = SmtpServer.FillFromXML( dt.Rows[ 0 ] );
                    smtp.UserGUID = MemberInfoData.MembreGUID;
                    int status = SmtpServer.Create( smtp );
                    if ( status == 2 )
                    {
                        MessageAnalyse += "Smtp : " + smtp.Email + " existe déjà.<br/>";
                        File.Delete( physicalPath );
                        EventFinImport.Set();
                        FinTraitement = true;
                        return;
                    }
                    else
                    {
                        MessageAnalyse += "Smtp crée correctement : " + smtp.Email + "<br/>";
                    }
                }
            }
        }

        DateTime dateFinAnalyse = DateTime.Now;
        TimeSpan tempsTraitement = dateFinAnalyse - dateDebutAnalyse;
        MessageAnalyse += "- Taille du fichier : " + Tools.FileSizeFormat( DocumentNom.PostedFile.ContentLength, "N" ) + "<br/>";
        MessageAnalyse += "- Temps d'import : " + Tools.FormatTimeSpan( tempsTraitement ) + "<br/>";

        File.Delete( physicalPath );
        EventFinImport.Set();
        FinTraitement = true;
    }

    protected void ButtonSupprimerMemberData_Click( object sender, EventArgs e )
    {
        string fileName = HttpContext.Current.Request.MapPath( repertoire + NomFichier );

        if ( File.Exists( fileName ) )
        {
            File.Delete( fileName );
            MessageExport += "Fichier supprimé : " + NomFichier + "<br/>";
        }
        else
        {
            MessageExport += "Fichier : " + NomFichier + " n'est plus sur le serveur<br/>";
        }

        LabelMessageExport.Text = MessageExport;
        LabelMessageExport.Visible = true;
    }
}

