//
// Attention :
// en mettant PollAnswerID (de type Guid.ToString()) dans textBoxDate.ID,
// ca plante les scripts java : "il manque une ;" il manque "ul" dans un coin ;)
// On utilise donc le rank de la reponse + indexID pour fabriquer l'ID
// mais si l'intervieweur mais deux reponses Date de meme rang dans la meme question
// on aura un plantage
// c'est peu probable
//
// C'est vraiment le genre de composant qui tombe en marche ! 
// Plutot qu'il ne fonctionne correctement.
// Chaque fois que je dois changer quelque choses ici c'est la galere.
// Les objets : RadioList, CheckBoxList, RadioButton, CheckBox, PopupTextBox et TextBoxDate
// ont tous des comportements trop differents et des bugs differents.
// sans oublier que PopupTextBox et TextBoxDate sont dans des UserControl.
// Il faut voir ce que l'on a du faire pour appliquer les style aux RadioList
// dans CheckBoxListStyle et RadioButtonListStyle !
// 
// Ca devient un truc de malade pour ne pas dupliquer de CreateChildControls()
// on fait aussi la creation de StyleWeb !
// 2879 lignes
//
// Mars 2010
// Et ça continue, je découvre le CellPading et le CellSpacing pour les tables
// et si on les utilise pas on arriva pas à faire ce que l'on veut
// ajout de ApplyStyleGraphique( string toolTip, ...
// 3155 lignes
//

#region Using
using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using UserControl.Web.Controls;
using Sql.Web.Data;
using StyleWebData;
using AjaxControlToolkit; 
using TraceReporter;
#endregion

public partial class Poll_Questionnaire : PageBase
{
    private string ImageButtonNePasAppliquerStyleImageUrl
    {
        get
        {
            if ( Session[ "ImageButtonNePasAppliquerStyleImageUrl" ] == null )
            {
                Session[ "ImageButtonNePasAppliquerStyleImageUrl" ] = "~/Images/Delete.gif";
            }
            return ( string )Session[ "ImageButtonNePasAppliquerStyleImageUrl" ];
        }
        set { Session[ "ImageButtonNePasAppliquerStyleImageUrl" ] = value; }
    }

    private string ImageButtonNePasAppliquerStyleToolTip
    {
        get
        {
            if ( Session[ "ImageButtonNePasAppliquerStyleToolTip" ] == null )
            {
                Session[ "ImageButtonNePasAppliquerStyleToolTip" ] = "Supprimer l'application des Styles";
            }
            return ( string )Session[ "ImageButtonNePasAppliquerStyleToolTip" ];
        }
        set { Session[ "ImageButtonNePasAppliquerStyleToolTip" ] = value; }
    }

    private bool CheckBoxModeDebugGraphiqueTableChecked
    {
        get 
        {
            if ( Session[ "CheckBoxModeDebugGraphiqueTableChecked" ] == null )
            {
                Session[ "CheckBoxModeDebugGraphiqueTableChecked" ] = false;
            }
            return ( bool )Session[ "CheckBoxModeDebugGraphiqueTableChecked" ]; 
        }
        set { Session[ "CheckBoxModeDebugGraphiqueTableChecked" ] = value; }
    }

    private bool CheckBoxModeDebugGraphiqueCelluleChecked
    {
        get 
        {
            if ( Session[ "CheckBoxModeDebugGraphiqueCelluleChecked" ] == null ) 
            {
                Session[ "CheckBoxModeDebugGraphiqueCelluleChecked" ] = false;
            }
            return ( bool )Session[ "CheckBoxModeDebugGraphiqueCelluleChecked" ]; 
        }
        set { Session[ "CheckBoxModeDebugGraphiqueCelluleChecked" ] = value; }
    }

    private bool CheckBoxModeDebugGraphiqueToolTipChecked
    {
        get 
        {
            if ( Session[ "CheckBoxModeDebugGraphiqueToolTipChecked" ] == null ) 
            {
                Session[ "CheckBoxModeDebugGraphiqueToolTipChecked" ] = false;
            }
            return ( bool )Session[ "CheckBoxModeDebugGraphiqueToolTipChecked" ]; 
        }
        set { Session[ "CheckBoxModeDebugGraphiqueToolTipChecked" ] = value; }
    }

    // Tableau est en erreur ou 
    // Question en erreur avec deux reponse Date ou PopupTextBox de meme Rang
    // on ne peut pas computer la question suivante
    private bool QuestionnaireErreur
    {
        get { return ( bool )ViewState[ "QuestionnaireErreur" ]; }
        set { ViewState[ "QuestionnaireErreur" ] = value; }
    }

    // Permet de resynchroniser l'interface graphique avec le Questionnaire en cours
    private string ResynchroControlPollQuestionId
    {
        get { return ( string )ViewState[ "ResynchroControlPollQuestionId" ]; }
        set { ViewState[ "ResynchroControlPollQuestionId" ] = value; }
    }

    private string PageQuestionsTitre
    {
        get
        {
            if ( HttpContext.Current.Session[ "PageQuestionsTitre" ] == null )
                return "Titre de la page";
            return ( string )( HttpContext.Current.Session[ "PageQuestionsTitre" ] );
        }
        set
        {
            HttpContext.Current.Session[ "PageQuestionsTitre" ] = value;
        }
    }

    // Trouver le Rank le plus eleve dans les Questions du Questionnaire
    private int DerniereQuestion
    {
        get
        {
            if ( HttpContext.Current.Session[ "DerniereQuestion" ] != null )
                return ( int )( HttpContext.Current.Session[ "DerniereQuestion" ] );

            if ( SessionState.Questions.Count <= 0 )
                return 0;

            int rank = SessionState.Questions.MaxRank();
            HttpContext.Current.Session[ "DerniereQuestion" ] = rank;
            return rank;
        }

        set
        {
            HttpContext.Current.Session[ "DerniereQuestion" ] = null;
        }
    }

    /*--------------------------------------------------------------------*\
    ** Fonction pour les deux mode RadioButtonList et CheckBoxList
    \*--------------------------------------------------------------------*/

    // L'utilisateur a t-il deja vote a cette question
    private bool HasUserAlreadyVotedInThePast()
    {
        foreach ( PollVote pv in SessionState.Votes )
        {
            if ( pv.PollQuestionID == SessionState.Question.PollQuestionId
                 && pv.UserGUID == SessionState.Personne.PersonneGUID )
            {
                return true;
            }
        }
        return false;
    }

    // L'utilisateur a t-il vote a la question courante ?
    private bool HasUserVotedToCurrentQuestion( int indexID, ref string erreurMessage )
    {
        string index = "";

        // Choix Multiple
        if ( SessionState.Question.ChoixMultiple )
        {
            index = "CheckBoxListStyleID" + indexID.ToString();
            CheckBoxListStyle cbl = new CheckBoxListStyle();
            cbl = ( CheckBoxListStyle )PanelQuestionnaire.FindControl( index );
            if ( cbl != null )
            {
                if ( cbl.SelectedValue != null && cbl.SelectedValue != string.Empty )
                {
                    if ( SessionState.Question.ChoixMultipleMin > 0 && SessionState.Question.ChoixMultipleMax > 0 )
                    {
                        // Verifier combien d'Item sont selectionnes
                        int selectedItems = 0;
                        foreach ( ListItem li in cbl.Items )
                        {
                            if ( li.Selected == true )
                            {
                                selectedItems +=1;
                            }
                        }
                        if ( selectedItems >= SessionState.Question.ChoixMultipleMin && selectedItems <= SessionState.Question.ChoixMultipleMax )
                        {
                            return true;
                        }
                        else
                        {
                            if ( SessionState.Question.ChoixMultipleMin == SessionState.Question.ChoixMultipleMax)
                            {
                                erreurMessage += "A la question : " + SessionState.Question.Question + " vous devez faire " + SessionState.Question.ChoixMultipleMin.ToString() + " choix.";
                            }
                            else
                            {
                                erreurMessage += "A la question : " + SessionState.Question.Question + " vous devez faire entre " + SessionState.Question.ChoixMultipleMin.ToString() + " et " + SessionState.Question.ChoixMultipleMax.ToString() + " choix.";
                            }
                            return false;
                        }
                    }
                    return true;
                }
            }
        }
        else // Choix simple
        {
            index = "RadioButtonListStyleID" + indexID.ToString();
            RadioButtonListStyle rbl = new RadioButtonListStyle();
            rbl = ( RadioButtonListStyle )PanelQuestionnaire.FindControl( index );
            if ( rbl != null ) 
            {
                if ( rbl.SelectedValue != null )
                    if ( rbl.SelectedValue != "" )
                        return true;
            }
        }

        int selectedItemsTableau = 0; // Pour verifier Choix Min et Choix Max dans les Tableaux
        foreach ( PollAnswer ans in SessionState.Reponses )
        {
            // Tableau
            // si SessionState.QuestionnaireEnModePage,
            // les Reponses sont representees par des CheckBox
            // sinon elles le sont par des CheckBoxListStyle 
            if ( SessionState.Question.Tableau != string.Empty && SessionState.QuestionnaireEnModePage )
            {
                if ( SessionState.Question.ChoixMultiple )
                {
                    CheckBox checkBox = ( CheckBox )PanelQuestionnaire.FindControl( ans.PollAnswerId.ToString() );
                    if ( checkBox.Checked )
                    {
                        selectedItemsTableau += 1;
                        //return true; On regarde plus loin dans "Verifier Choix Min et Choix Max dans les tableaux"
                    }
                }
                else
                {
                    RadioButton radioButton = ( RadioButton )PanelQuestionnaire.FindControl( ans.PollAnswerId.ToString() );
                    if ( radioButton.Checked )
                    {
                        return true;
                    }
                }
            }

            if ( TypeReponse.EstTextBox( ans.TypeReponse ) )
            {
                TextBox textbox = ( TextBox )PanelQuestionnaire.FindControl( ans.PollAnswerId.ToString() );

                // L'utilisateur a remplit quelque chose, c'est bon, c'est comme s'il avait vote
                if ( textbox.Text.Trim() != "" )
                {
                    if ( ans.TypeReponse == TypeReponse.Numerique )
                    {
                        try
                        {
                            int rep = int.Parse( textbox.Text.Trim() );
                        }
                        catch
                        {
                            erreurMessage += "La réponse : \"" + ans.Answer + "\" est numérique<br/>";
                            return false;
                        }
                    }

                    return true;
                }
            }

            if ( ans.TypeReponse == TypeReponse.Date )
            {
                TextBoxDate textBoxDate = ( TextBoxDate )PanelQuestionnaire.FindControl( "TextBoxDateID" + indexID.ToString() + ans.Rank.ToString() );
                TextBox textBox = ( TextBox )textBoxDate.FindControl( "TextBoxDateText" );
                
                // L'utilisateur a remplit quelque chose, c'est bon, c'est comme s'il avait vote
                if ( textBox.Text.Trim() != "" )
                {
                    return true;
                }
            }

            if ( ans.TypeReponse == TypeReponse.SemiOuverte )
            {
                PopupTextBox popupTextBox = ( PopupTextBox )PanelQuestionnaire.FindControl( "PopupTextBoxID" + indexID.ToString() + ans.Rank.ToString() );
                TextBox textBox = ( TextBox )popupTextBox.FindControl( "TextBoxText" );

                // L'utilisateur a remplit quelque chose, c'est bon, c'est comme s'il avait vote
                if ( textBox.Text.Trim() != "" )
                {
                    return true;
                }
            }
        }
        
        // Verifier Choix Min et Choix Max dans les tableaux
        if ( selectedItemsTableau > 0 && SessionState.Question.ChoixMultipleMin > 0 && SessionState.Question.ChoixMultipleMax > 0 )
        {
            if ( selectedItemsTableau >= SessionState.Question.ChoixMultipleMin && selectedItemsTableau <= SessionState.Question.ChoixMultipleMax )
            {
                return true;
            }
            else
            {
                if ( SessionState.Question.ChoixMultipleMin == SessionState.Question.ChoixMultipleMax )
                {
                    erreurMessage += "A la question : " + SessionState.Question.Question + " vous devez faire " + SessionState.Question.ChoixMultipleMin.ToString() + " choix.";
                }
                else
                {
                    erreurMessage += "A la question : " + SessionState.Question.Question + " vous devez faire entre " + SessionState.Question.ChoixMultipleMin.ToString() + " et " + SessionState.Question.ChoixMultipleMax.ToString() + " choix.";
                }
                return false;
            }
        }
        return false;
    }

    // L'utilisateur a t-il repondu a toutes les reponses textuelles obligatoires ?
    private bool HasUserVotedToObligatoireTextuellesReponses( int indexID, ref string erreurMessage )
    {
        bool result = true;

        foreach ( PollAnswer answer in SessionState.Reponses )
        {
            if ( answer.Obligatoire )
            {
                if ( answer.TypeReponse == TypeReponse.Numerique )
                {
                    TextBox textbox = ( TextBox )PanelQuestionnaire.FindControl( answer.PollAnswerId.ToString() );
                    try
                    {
                        int rep = int.Parse( textbox.Text.Trim() );
                        result = result && true;
                    }
                    catch
                    {
                        erreurMessage += "La réponse numérique : \"" + answer.Answer + "\" est obligatoire<br/>";
                        result = false;
                    }
                }

                if ( answer.TypeReponse == TypeReponse.Ouverte )
                {
                    TextBox textbox = ( TextBox )PanelQuestionnaire.FindControl( answer.PollAnswerId.ToString() );

                    if ( textbox.Text.Trim() == "" )
                    {
                        erreurMessage += "La réponse ouverte : \"" + answer.Answer + "\" est obligatoire<br/>";
                        result = false;
                    }
                    else
                    {
                        result = result && true;
                    }
                }

                if ( answer.TypeReponse == TypeReponse.SemiOuverte )
                {
                    // ce n'est pas logique une semi-ouverte obligatoire mais bon ... pour la coherence
                    PopupTextBox popupTextBox = ( PopupTextBox )PanelQuestionnaire.FindControl( "PopupTextBoxID" + indexID.ToString() + answer.Rank.ToString() );
                    TextBox textbox = ( TextBox )popupTextBox.FindControl( "TextBoxText" );

                    if ( textbox.Text.Trim() == "" )
                    {
                        erreurMessage += "La réponse semi-ouverte : \"" + answer.Answer + "\" est obligatoire<br/>";
                        result = false;
                    }
                    else
                    {
                        result = result && true;
                    }
                }

                if ( answer.TypeReponse == TypeReponse.Date )
                {
                    TextBoxDate textBoxDate = ( TextBoxDate )PanelQuestionnaire.FindControl( "TextBoxDateID" + indexID.ToString() + answer.Rank.ToString() );
                    TextBox textbox = ( TextBox )textBoxDate.FindControl( "TextBoxDateText" );

                    if ( textbox.Text.Trim() == "" )
                    {
                        erreurMessage += "La réponse date : \"" + answer.Answer + "\" est obligatoire<br/>";
                        result = false;
                    }
                    else
                    {
                        result = result && true;
                    }
                }
            }
        }

        return result;
    }

    // Pour les Reponses Textuelles
    private string GetUserAnswerForTextuelVote( ref bool voteInDataBase, Guid pollAnswerID )
    {
        string vote = "";
        voteInDataBase = false;

        // Recherche dans les votes deja effectue
        foreach ( PollVote pv in SessionState.Votes )
        {
            if ( pv.PollQuestionID == SessionState.Question.PollQuestionId
                 && pv.UserGUID == SessionState.Personne.PersonneGUID )
            {
                voteInDataBase = true;
                if ( pv.PollAnswerId == pollAnswerID )
                {
                    vote = pv.Vote;
                }
            }
        }

        // Recherche dans les votes en cours
        if ( vote == "" )
        {
            foreach ( PollVote pv in SessionState.VotesEnCours )
            {
                if ( pv.PollQuestionID == SessionState.Question.PollQuestionId
                     && pv.UserGUID == SessionState.Personne.PersonneGUID
                     && pv.PollAnswerId == pollAnswerID )
                {
                    vote = pv.Vote;
                }
            }
        }
        return vote;
    }

    // Retourne les Guids des Reponses Votee dans le Questionnaire
    private ArrayList GetUserVotes( int indexID )
    {
        ArrayList al = new ArrayList();

        // Choix multiple
        if ( SessionState.Question.ChoixMultiple )
        {
            string index = "CheckBoxListStyleID" + indexID.ToString();
            CheckBoxListStyle cbl = ( CheckBoxListStyle )PanelQuestionnaire.FindControl( index );
            if ( cbl != null )
            {
                foreach ( ListItem li in cbl.Items )
                {
                    if ( li.Selected == true )
                        al.Add( li.Value );
                }
            }
        }
        else // Choix simple
        {
            string index = "RadioButtonListStyleID" + indexID.ToString();
            RadioButtonListStyle rbl = ( RadioButtonListStyle )PanelQuestionnaire.FindControl( index );
            if ( rbl != null )
            {
                foreach ( ListItem li in rbl.Items )
                {
                    if ( li.Selected == true )
                    {
                        al.Add( li.Value );
                        break;
                    }
                }
            }
        }

        // Reponses textuelles
        foreach ( PollAnswer ans in SessionState.Reponses )
        {
            // Tableau
            if ( SessionState.Question.Tableau != string.Empty && SessionState.QuestionnaireEnModePage )
            {
                if ( SessionState.Question.ChoixMultiple )
                {
                    CheckBox checkBox = ( CheckBox )PanelQuestionnaire.FindControl( ans.PollAnswerId.ToString() );
                    if ( checkBox.Checked )
                    {
                        al.Add( checkBox.ID );
                    }
                }
                else
                {
                    RadioButton radioButton = ( RadioButton )PanelQuestionnaire.FindControl( ans.PollAnswerId.ToString() );
                    if ( radioButton.Checked )
                    {
                        al.Add( radioButton.ID );
                        break;
                    }
                }
            }

            if ( TypeReponse.EstTextBox( ans.TypeReponse ) )
            {
                TextBox textbox = ( TextBox )PanelQuestionnaire.FindControl( ans.PollAnswerId.ToString() );

                // L'utilisateur a remplit quelque chose c'est comme s'il avait vote
                if ( textbox.Text.Trim() != "" )
                {
                    al.Add( textbox.ID ); // PollAnswerID est cache dans l'ID de la TextBox
                }
            }

            if ( ans.TypeReponse == TypeReponse.Date )
            {
                TextBoxDate textBoxDate = ( TextBoxDate )PanelQuestionnaire.FindControl( "TextBoxDateID" + indexID.ToString() + ans.Rank.ToString() );
                TextBox textBox = ( TextBox )textBoxDate.FindControl( "TextBoxDateText" );

                // L'utilisateur a remplit quelque chose c'est comme s'il avait vote
                if ( textBox.Text.Trim() != "" )
                {
                    al.Add( ans.PollAnswerId.ToString() ); 
                }
            }

            if ( ans.TypeReponse == TypeReponse.SemiOuverte )
            {
                PopupTextBox popupTextBox = ( PopupTextBox )PanelQuestionnaire.FindControl( "PopupTextBoxID" + indexID.ToString() + ans.Rank.ToString() );
                TextBox textBox = ( TextBox )popupTextBox.FindControl( "TextBoxText" );

                // L'utilisateur a remplit quelque chose, c'est bon, c'est comme s'il avait vote
                if ( textBox.Text.Trim() != "" )
                {
                    al.Add( ans.PollAnswerId.ToString() );
                }
           }
        }

        return al;
    }

    /*--------------------------------------------------------------------*\
    ** Fonction pour le mode RadioButtonList Choix simple
    \*--------------------------------------------------------------------*/

    // Retourne la Reponse deja donne par l'utilisateur
    private Guid GetUserAnswer( ref bool voteInDataBase )
    {
        Guid pollAnswerID = Guid.Empty;
        voteInDataBase = false;

        // Recherche dans les votes deja effectue
        foreach ( PollVote pv in SessionState.Votes )
        {
            if ( pv.PollQuestionID == SessionState.Question.PollQuestionId
                 && pv.UserGUID == SessionState.Personne.PersonneGUID )
            {
                pollAnswerID = pv.PollAnswerId;
                voteInDataBase = true;
            }
        }

        // Recherche dans les votes en cours
        if ( pollAnswerID == Guid.Empty )
        {
            foreach ( PollVote pv in SessionState.VotesEnCours )
            {
                if ( pv.PollQuestionID == SessionState.Question.PollQuestionId
                     && pv.UserGUID == SessionState.Personne.PersonneGUID )
                {
                    pollAnswerID = pv.PollAnswerId;
                }
            }
        }
        return pollAnswerID;
    }

    /*--------------------------------------------------------------------*\
    ** Fonction pour le mode CheckBoxListStyle Choix multiples
    \*--------------------------------------------------------------------*/

    // Retourne la Reponse deja donne par l'utilisateur
    private ArrayList GetUserAnswers( ref bool voteInDataBase )
    {
        ArrayList al = new ArrayList();
        Guid pollAnswerID = Guid.Empty;
        voteInDataBase = false;

        // Recherche dans les votes deja effectue
        foreach ( PollVote pv in SessionState.Votes )
        {
            if ( pv.PollQuestionID == SessionState.Question.PollQuestionId
                 && pv.UserGUID == SessionState.Personne.PersonneGUID )
            {
                al.Add( pv.PollAnswerId );
                voteInDataBase = true;
            }
        }

        // Recherche dans les votes en cours
        foreach ( PollVote pv in SessionState.VotesEnCours )
        {
            if ( pv.PollQuestionID == SessionState.Question.PollQuestionId
                 && pv.UserGUID == SessionState.Personne.PersonneGUID )
            {
                al.Add( pv.PollAnswerId );
            }
        }
        return al;
    }

    /*--------------------------------------------------------------------*/

    private void QuestionnaireEnModePage()
    {
        SessionState.QuestionnaireEnModePage = false;
        foreach ( PollQuestion pq in SessionState.Questions )
        {
            if ( string.IsNullOrEmpty( pq.SautPage ) == false )
            {
                SessionState.QuestionnaireEnModePage = true;
                break;
            }
        }
    }

    protected void Page_Load( object sender, EventArgs e )
    {
        Reporter.Trace( Report.TRACE0, "Page_Load : IsPostBack == {0}", IsPostBack.ToString() );
        Trace.Warn( "Page_Load()" );

        // AME12102010
        if ( IsPostBack == false )
        {
            if ( Page.Request[ "Test" ] != null )
            {
                if ( SessionState.Questionnaire != null )
                {
                    Response.Redirect( "~/Poll/Questionnaire.aspx?QuestionnaireID=" + SessionState.Questionnaire.QuestionnaireID.ToString(), true );
                }
            }
        }
 
        if ( IsPostBack == false )
        {
            QuestionnaireErreur = false;
            PanelAide.Visible = Page.User.Identity.IsAuthenticated;
            CheckBoxModeDebugGraphiqueTable.Checked = CheckBoxModeDebugGraphiqueTableChecked;
            CheckBoxModeDebugGraphiqueCellule.Checked = CheckBoxModeDebugGraphiqueCelluleChecked;
            CheckBoxModeDebugGraphiqueToolTip.Checked = CheckBoxModeDebugGraphiqueToolTipChecked;
            ImageButtonNePasAppliquerStyle.ImageUrl = ImageButtonNePasAppliquerStyleImageUrl;
            ImageButtonNePasAppliquerStyle.ToolTip = ImageButtonNePasAppliquerStyleToolTip;

            //
            // Questionnaire en mode creation des StyleWeb
            // On va chercher le Questionnaire : "StyleWeb"
            //
            int questionnaireID = 0; // distinguer le mode creation de style des autres modes
            if ( Page.Request[ "StyleWeb" ] != null )
            {
                if ( SessionState.Questionnaires == null || SessionState.Questionnaires.Count <= 0 )
                {
                    Tools.PageValidation( "Pas de questionnaire" );
                }

                bool trouve = false;
                foreach ( Questionnaire questionnaire in SessionState.Questionnaires )
                {
                    if ( questionnaire.Description == "StyleWeb" )
                    {
                        questionnaireID = questionnaire.QuestionnaireID;
                        trouve = true;
                        break;
                    }
                }
                if ( trouve == false )
                {
                    Tools.PageValidation( "Pas de questionnaire \"StyleWeb\"" );
                }

                // Chargement du Questionnaire StyleWeb
                SessionState.Questionnaire = Questionnaire.GetQuestionnaire( questionnaireID );

                PanelStyleWeb.Visible = true;
                CheckBoxModeTest.Visible = false;
                CheckBoxModeTest.Checked = false;
                ButtonSubmit.Visible = false;
            }
            else
            {
                TrStyleWebCreation.Visible = false;
                if ( SessionState.MemberInfo != null )
                {
                    PanelButtonRetourQuestionnaire.Visible = true;
                }

                // Pour un Membre authentifie, on peut afficher ou non les instructions
                // de programmation d'un Questionnaire 
                if ( HttpContext.Current.User.Identity.IsAuthenticated )
                {
                    CheckBoxModeTest.Checked = SessionState.CheckBox[ "CheckBoxModeTest" ];
                }

                CheckBoxModeDebugGraphiqueTable.Visible = false;
                CheckBoxModeDebugGraphiqueCellule.Visible = false;
                CheckBoxModeDebugGraphiqueToolTip.Visible = false;
                CheckBoxModeDebugGraphiqueTable.Checked = false;
                CheckBoxModeDebugGraphiqueCellule.Checked = false;
                CheckBoxModeDebugGraphiqueToolTip.Checked = false;
            }

            if ( Page.User.IsInRole( "Administrateur" ) || Page.User.IsInRole( "Client" ) )
            {
                if ( SessionState.Questionnaire == null )
                {
                    Tools.PageValidation( "Sélectionner un questionnaire" );
                }

                RolloverButtonListQuestions.Visible = true;
                PanelControlStyle.Visible = true;
                LabelQuestionnaire.Text = SessionState.Questionnaire.Description;

                // Pour gerer correctement le WebContent
                SessionState.Personne.CodeAcces = SessionState.Questionnaire.CodeAcces;
                SessionState.Personne.QuestionnaireID = SessionState.Questionnaire.QuestionnaireID;
                
                BindFichierStyle();
                
                // Le style du questionnaire a ete supprimer, le remettre a Défaut.xml
                string style = Tools.GetFileNameWithoutExtension( SessionState.Questionnaire.Style );
                if ( Tools.ListItemCollectionContainsText( DropDownListStyle.Items, new ListItem( style ) ) == false )
                {
                    SessionState.Questionnaire.Style = "Défaut.xml";
                    Questionnaire.Update( SessionState.Questionnaire );

                    // Mettre a jour le style du Questionnaire dans la collection
                    Questionnaire questionnaire = SessionState.Questionnaires.FindByID( SessionState.Questionnaire.QuestionnaireID );
                    questionnaire.Style = SessionState.Questionnaire.Style;
                }

                DropDownListStyle.SelectedValue = Tools.GetFileNameWithoutExtension( SessionState.Questionnaire.Style );
            }

            // Chargement automatique des Questions 
            // si elles sont nulles ou si elles ne correspondent pas au bon Questionnaire
            if ( SessionState.Questionnaire != null )
            {
                // On ne doit pas arriver la si la condition est remplie mais bon ...
                if ( SessionState.Questions != null && SessionState.Questions.Count <= 0 )
                {
                    Page.Response.Redirect( Tools.PageErreurPath + "Il n'y a pas de Question pour le Questionnaire : " + SessionState.Questionnaire.Description );
                }

                if ( SessionState.Questions == null || SessionState.Questionnaire.QuestionnaireID != SessionState.Questions[ 0 ].QuestionnaireID )
                {
                    // Charger le Questionnaire
                    SessionState.Questions = PollQuestionCollection.GetByQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
                    if ( SessionState.Questions.Count > 0 )
                    {
                        SessionState.Question = SessionState.Questions[ 0 ]; // permiere question
                        SessionState.Reponses = PollAnswerCollection.GetByPollQuestionID( SessionState.Question.PollQuestionId );
                        SessionState.CurrentQuestionIndex = 0;
                        SessionState.Votes = PollVoteCollection.GetPollVotes( SessionState.Questionnaire.QuestionnaireID, SessionState.Personne.PersonneGUID );

                        // Recalculer la DernierQuestion
                        DerniereQuestion = 0;
                    }

                    // Vides
                    SessionState.VotesEnCours = new PollVoteCollection();
                    SessionState.QuestionsEnCours = new PollQuestionCollection();
                }
            }

            // Chargement d'un Questionnaire depuis la permiere question
            if (    Page.Request[ "QuestionnaireID" ] != null 
                 || ( Page.Request[ "QuestionnaireID" ] == null && questionnaireID != 0 )
               )
            {
                // On n'est pas en mode creation de StyleWeb
                if ( questionnaireID == 0 )
                {
                    questionnaireID = int.Parse( Page.Request[ "QuestionnaireID" ].ToString() );
                }
                SessionState.Questionnaire = Questionnaire.GetQuestionnaire( questionnaireID );

                // Au dessus, le style vient d'etre recharge, 
                // restaurer le style choisi par l'utilisateur authentifie
                // a ne pas faire pour les interviewes
                if ( User.Identity.IsAuthenticated )
                {
                    SessionState.Questionnaire.Style = DropDownListStyle.SelectedValue + ".xml";
                }

                // Verifier qu'il y a un questionnaire
                if ( SessionState.Questionnaire == null )
                {
                    Page.Response.Redirect( Tools.PageErreurPath + "Il n'y a pas de Questionnaire." );
                }

                // Verifier que c'est la bonne Personne
                if ( HttpContext.Current.User.IsInRole( "Administrateur" ) == false )
                {
                    if ( SessionState.MemberInfo != null )
                    {
                        if ( SessionState.Questionnaire.MembreGUID != SessionState.MemberInfo.MembreGUID )
                        {
                            Page.Response.Redirect( Tools.PageErreurPath + "Il n'y a pas de Questionnaire." );
                        }
                    }
                    else // C'est une Personne
                    {
                        if ( SessionState.Personne == null )
                        {
                            Page.Response.Redirect( Tools.PageErreurPath + "Erreur : Vous n'êtes pas authentifié." );
                        }
                        if ( SessionState.Personne.QuestionnaireID != questionnaireID )
                        {
                            Page.Response.Redirect( Tools.PageErreurPath + "Il n'y a pas de Questionnaire." );
                        }
                    }
                }

                // Charger le Questionnaire
                SessionState.Questions = PollQuestionCollection.GetByQuestionnaire( questionnaireID );
                if ( SessionState.Questions.Count > 0 )
                {
                    SessionState.Question = SessionState.Questions[ 0 ]; // permiere question
                    SessionState.Reponses = PollAnswerCollection.GetByPollQuestionID( SessionState.Question.PollQuestionId );
                    SessionState.CurrentQuestionIndex = 0;
                    SessionState.Votes = PollVoteCollection.GetPollVotes( SessionState.Questionnaire.QuestionnaireID, SessionState.Personne.PersonneGUID );

                    // Recalculer la DernierQuestion
                    DerniereQuestion = 0;
                }

                // Vides
                SessionState.VotesEnCours = new PollVoteCollection();
                SessionState.QuestionsEnCours = new PollQuestionCollection();

                // Mode Page
                if ( SessionState.CurrentQuestionIndex != -1 ) // pas de question pour ce questionnaire
                {
                    SessionState.PageQuestionsIndex = SessionState.CurrentQuestionIndex;
                    SessionState.PageQuestions = new PollQuestionCollection();
                    ChargerPageQuestions();
                }

            }// Fin du if ( Page.Request[ "QuestionnaireID" ] != null )

            // Remettre le Questionnaire sur la Question choisie par l'utilisateur
            if ( Page.Request[ "PollQuestionId" ] != null )
            {
                // Tester le Questionnaire
                // Recharger les Questions si l'utilisateur en a ajoute une
                // Vider les Votes et les Questions en cours
                if ( Page.Request[ "t" ] != null )
                {
                    SessionState.Questions = PollQuestionCollection.GetByQuestionnaire( SessionState.Questionnaire.QuestionnaireID );

                    // Vides
                    SessionState.VotesEnCours = new PollVoteCollection();
                    SessionState.QuestionsEnCours = new PollQuestionCollection();

                    // Attention, il semble que les Votes disparaissent (deviennent null)
                    // pourtant on passe forcement par un code d'init des votes
                    // alors est-ce que ce n'est pas la session qui se termine ??
                    // si dessous le code pour parlier a ca 
                    // mais j'ai peur que cela ne cache un autre probleme
                    // en tout si on est ici c'est que l'on teste le Questionnaire depuis
                    // n'importe quelle question et donc les votes sont vides
                    // en tout cas ils ne doivent pas etre null !
                    if ( SessionState.Votes == null )
                    {
                        SessionState.Votes = new PollVoteCollection();
                    }
                }

                string questionId = Page.Request[ "PollQuestionId" ].ToString();

                // Trouver l'index de la Question cliquee
                int index = 0;
                foreach ( PollQuestion q in SessionState.Questions )
                {
                    if ( q.PollQuestionId.ToString() == questionId )
                        break;
                    index++;
                }
                SessionState.CurrentQuestionIndex = index;

                // Recharger la Question et ses Reponses
                SessionState.Question = SessionState.Questions[ SessionState.CurrentQuestionIndex ];
                SessionState.Reponses = PollAnswerCollection.GetByPollQuestionID( SessionState.Question.PollQuestionId );

                // Effacer les Questions en Cours au dela de la Question cliquee si il y en a
                if ( SessionState.QuestionsEnCours.Count > 0 )
                {
                    // Trouver l'index de la Question cliquee dans QuestionsEnCours
                    int indexQeC = 0;
                    foreach ( PollQuestion q in SessionState.QuestionsEnCours )
                    {
                        if ( q.PollQuestionId.ToString() == questionId )
                            break;
                        indexQeC++;
                    }
                    SessionState.QuestionsEnCours.RemoveRange( indexQeC + 1, SessionState.QuestionsEnCours.Count - ( indexQeC + 1 ) );

                    // On ne conserve pas les Votes de la derniere Question en Cours
                    PollQuestionCollection pqcConserve = SessionState.QuestionsEnCours;
                    pqcConserve.RemoveAt( SessionState.QuestionsEnCours.Count - 1 );

                    PollVoteCollection pvcConcerve = new PollVoteCollection();
                    foreach ( PollQuestion question in pqcConserve )
                    {
                        PollAnswerCollection reponses = PollAnswerCollection.GetByPollQuestionID( question.PollQuestionId );
                        foreach ( PollAnswer reponse in reponses )
                        {
                            PollVoteCollection pvc = SessionState.VotesEnCours.FindByAnswerID( reponse.PollAnswerId );
                            foreach ( PollVote v in pvc )
                            {
                                pvcConcerve.Add( v );
                            }
                        }
                    }

                    SessionState.VotesEnCours.Clear();
                    foreach ( PollVote v in pvcConcerve )
                    {
                        SessionState.VotesEnCours.Add( v );
                    }
                }

                // Mode Page
                SessionState.PageQuestionsIndex = SessionState.CurrentQuestionIndex;
                SessionState.PageQuestions = new PollQuestionCollection();
                ChargerPageQuestions();
            }

            if ( SessionState.Question == null )
            {
                Response.Redirect( Tools.PageErreurPath + "Il n'y a pas de Questions pour ce Questionnaire." );
            }
            
            ResynchroControlPollQuestionId = SessionState.Question.PollQuestionId.ToString();

            // Regarder si ce questionnaire est en mode page c.a.d.
            // si une des questions possede un saut de page
            QuestionnaireEnModePage();

            MemberInfo membre = MemberInfo.Get( SessionState.Questionnaire.MembreGUID );
            MemberSettings memberSettings = MemberSettings.GetMemberSettings( membre.NomUtilisateur );
            ButtonSubmit.Text = memberSettings.BoutonQuestionSuivanteTexte;
            ButtonSubmit.ToolTip = memberSettings.BoutonQuestionSuivanteAlt;

            // COR21092009
            HyperLinkVosReponses.NavigateUrl = "~/Poll/QuestionnaireEnCours.aspx?QuestionnaireID=" + SessionState.Question.QuestionnaireID;
            HyperLinkVosReponses.Text = "Vos&nbsp;réponses";
            HyperLinkVosReponses.ToolTip = SessionState.Questionnaire.MessageDeValidation( SessionState.Questionnaire.Valider, SessionState.Questionnaire.Fin );

            ProgressBarre.States = SessionState.Questions.Count;
        }

        if ( SessionState.QuestionnaireEnModePage )
            ButtonSubmit.Click += new EventHandler( SubmitButtonPageQuestions_Click );
        else
            ButtonSubmit.Click += new EventHandler( SubmitButton_Click );
    }


    protected void CheckBoxModeDebugGraphiqueToolTip_CheckedChanged( object sender, EventArgs e )
    {
        CheckBoxModeDebugGraphiqueToolTipChecked = CheckBoxModeDebugGraphiqueToolTip.Checked;
        Response.Redirect( Request.RawUrl );
    }

    protected void CheckBoxModeDebugGraphiqueCellule_CheckedChanged( object sender, EventArgs e )
    {
        CheckBoxModeDebugGraphiqueCelluleChecked = CheckBoxModeDebugGraphiqueCellule.Checked;
        Response.Redirect( Request.RawUrl );
    }
        
    protected void CheckBoxModeDebugGraphiqueTable_CheckedChanged( object sender, EventArgs e )
    {
        CheckBoxModeDebugGraphiqueTableChecked = CheckBoxModeDebugGraphiqueTable.Checked;
        Response.Redirect( Request.RawUrl );
    }

    protected void CheckBoxModeTest_CheckedChanged( object sender, EventArgs e )
    {
        SessionState.CheckBox[ "CheckBoxModeTest" ] = CheckBoxModeTest.Checked;
        Response.Redirect( Request.RawUrl );
    }

    #region ControlStyleWeb

    private string GetNomUtilisateur()
    {
        string membre = "";
        if ( User.IsInRole( "Administrateur" ) )
        {
            membre = WebContent.Admin;
        }
        else
        {
            membre = SessionState.MemberInfo.NomUtilisateur;
        }
        return membre;
    }

    protected void BindFichierStyle()
    {
        ListItemCollection licMembre = new ListItemCollection();
        ListItemCollection licIntervieweur = new ListItemCollection();
        ListItemCollection licAll = new ListItemCollection();

        // Les Styles du membre
        string membre = GetNomUtilisateur();
        if ( membre != "Intervieweur" )
        {
            string repertoireStyleMembre = "~/App_Data/StyleWeb/" + membre;
            string dirName = Server.MapPath( repertoireStyleMembre );
            if ( Directory.Exists( dirName ) )
            {
                List<Fichier> fichiers = Tools.GetAllFichiers( dirName );
                foreach ( Fichier f in fichiers )
                {
                    string nomFichier = Tools.GetFileNameWithoutExtension( f.Nom );
                    licMembre.Add( new ListItem( nomFichier, f.Nom ) );
                }
            }
        }

        // Les Styles de l'intervieweur qui n'existent pas chez le membre
        string dirIntervieweur = Server.MapPath( "~/App_Data/StyleWeb/intervieweur" );
        if ( Directory.Exists( dirIntervieweur ) )
        {
            List<Fichier> fichiers = Tools.GetAllFichiers( dirIntervieweur );
            foreach ( Fichier f in fichiers )
            {
                string nomFichier = Tools.GetFileNameWithoutExtension( f.Nom );
                ListItem li = new ListItem( nomFichier, f.Nom );
                if ( Tools.ListItemCollectionContainsText( licMembre, li ) == false )
                {
                    licIntervieweur.Add( li );
                }
            }
        }

        foreach ( ListItem li in licIntervieweur )
        {
            licAll.Add( li );
        }
        foreach ( ListItem li in licMembre )
        {
            licAll.Add( li );
        }

        DropDownListStyle.DataSource = licAll;
        DropDownListStyle.DataBind();

        // Colorer les fichiers de l'Intervieweur
        for ( int i = 0;i < licIntervieweur.Count;i++ )
        {
            DropDownListStyle.Items[ i ].Attributes.Add( "style", "color:blue" );
        }
    }

    protected void DropDownListStyle_SelectedIndexChanged( object sender, EventArgs e )
    {
        SessionState.Questionnaire.Style = DropDownListStyle.SelectedValue + ".xml";
        Questionnaire.Update( SessionState.Questionnaire );

        // Mettre a jour le style du Questionnaire dans la collection
        Questionnaire questionnaire = SessionState.Questionnaires.FindByID( SessionState.Questionnaire.QuestionnaireID );
        questionnaire.Style = SessionState.Questionnaire.Style;

        string membre = GetNomUtilisateur();
        if ( SessionState.MemberInfo.NomUtilisateur != "Intervieweur" )
        {
            string _xmlFile = "StyleWeb/" + membre + "/" + SessionState.Questionnaire.Style;
            string fileName = HttpContext.Current.Request.MapPath( "~/App_Data/" + _xmlFile );
            if ( File.Exists( fileName ) == false )
            {
                // Copie du fichier de l'intervieweur pour un membre qui n'a pas ce fichier de styles
                string dir = HttpContext.Current.Request.MapPath( "~/App_Data/StyleWeb" );
                File.Copy( dir + "/intervieweur/" + SessionState.Questionnaire.Style, dir + "/" + membre + "/" + SessionState.Questionnaire.Style );
            }
        }

        Response.Redirect( Request.RawUrl );
    }

    // Creer un style a partir d'un style existant
    protected void ButtonStyleCreerStyle_Click( object sender, EventArgs e )
    {
        if ( TextBoxNomNouveauStyle.Text.Trim() != "" )
        {
            string membre = GetNomUtilisateur();
            string dirStyleWeb = "~/App_Data/StyleWeb/" + membre + "/";

            string fileNameToCopy = DropDownListStyle.SelectedValue + ".xml";
            string fileName = TextBoxNomNouveauStyle.Text.Trim() + ".xml";

            string dir = Server.MapPath( dirStyleWeb );
            try
            {
                File.Copy( dir + fileNameToCopy, dir + fileName );
            }
            catch
            {
                Response.Redirect( Tools.PageErreurPath + string.Format( "Erreur à la copie du fichier : {0}", fileName ) );
            }

            BindFichierStyle();

            // Appliquer le nouveau style
            DropDownListStyle.SelectedValue = TextBoxNomNouveauStyle.Text.Trim();
            SessionState.Questionnaire.Style = DropDownListStyle.SelectedValue + ".xml";
            Questionnaire.Update( SessionState.Questionnaire );

            // Mettre a jour le style du Questionnaire dans la collection
            Questionnaire questionnaire = SessionState.Questionnaires.FindByID( SessionState.Questionnaire.QuestionnaireID );
            questionnaire.Style = SessionState.Questionnaire.Style;
        }
    }

    private void toggleStyleApplicable( string styleNom, string type )
    {
        string membre = HttpContext.Current.User.Identity.Name;
        StyleWeb styleWebObjet = XmlStyleWebProvider.GetStyleWeb( membre, styleNom, type );
        styleWebObjet.Applicable = ImageButtonNePasAppliquerStyle.ImageUrl == "~/Images/Delete.gif" ? false : true;
        XmlStyleWebProvider.Update( membre, styleWebObjet );
    }
    protected void ButtonNePasAppliquerStyle_Click( object sender, EventArgs e )
    {
        // La Page
        toggleStyleApplicable( "CadreQuestionnaire", "Label" );
        toggleStyleApplicable( "PageQuestion", "Table" );
        toggleStyleApplicable( "TableTitrePage", "Table" );
        toggleStyleApplicable( "TitrePage", "Label" );

        // Les Questions
        toggleStyleApplicable( "Message", "Label" );
        toggleStyleApplicable( "CelluleQuestion", "Label");
        toggleStyleApplicable( "CadreQuestion", "Table");
        toggleStyleApplicable( "TableTitreQuestion", "Table" );
        toggleStyleApplicable( "Reponse", "RadioButtonList");
        toggleStyleApplicable( "Reponse", "CheckBoxList");
        toggleStyleApplicable( "TableReponseTextuelle", "Table");
        toggleStyleApplicable( "ReponseTextuelleLabel", "Label");
        toggleStyleApplicable( "ReponseTextuelleTextBox", "TextBox");
        toggleStyleApplicable( "TableCompteurQuestions", "Table");
        toggleStyleApplicable( "CompteurQuestions", "Label");
        toggleStyleApplicable( "Message", "Label");

        // Les Tableaux
        toggleStyleApplicable( "TableTitreTableau", "Table");
        toggleStyleApplicable( "TitreTableau", "Label");
        toggleStyleApplicable( "CadreTableau", "Table");
        toggleStyleApplicable( "CelluleQuestionTableau", "Label");
        toggleStyleApplicable( "QuestionTableau", "Label");
        toggleStyleApplicable( "CelluleReponseTableau", "Label");
        toggleStyleApplicable( "ReponseTableau", "Label" );

        ImageButtonNePasAppliquerStyleImageUrl = ImageButtonNePasAppliquerStyleImageUrl == "~/Images/Delete.gif" ? "~/Images/Select_bleu.gif" : "~/Images/Delete.gif";
        ImageButtonNePasAppliquerStyleToolTip = ImageButtonNePasAppliquerStyleToolTip == "Supprimer l'application des Styles" ? "Appliquer tous les styles" : "Supprimer l'application des Styles";
        Response.Redirect( Request.RawUrl );
    }

    protected void ButtonSupprimerStyle_Click( object sender, EventArgs e )
    {
        string membre = GetNomUtilisateur();
        string dirStyleWeb = "~/App_Data/StyleWeb/" + membre + "/";

        string fileName = DropDownListStyle.SelectedValue + ".xml";
        string dirName = Server.MapPath( dirStyleWeb );
        try
        {
            File.Delete( dirName + fileName );
        }
        catch
        {
            Response.Redirect( Tools.PageErreurPath + string.Format( "Erreur à la suppression du fichier : {0}", fileName ) );
        }

        BindFichierStyle();
        SessionState.Questionnaire.Style = DropDownListStyle.SelectedValue + ".xml";
        Questionnaire.Update( SessionState.Questionnaire );

        // Mettre a jour le style du Questionnaire dans la collection
        Questionnaire questionnaire = SessionState.Questionnaires.FindByID( SessionState.Questionnaire.QuestionnaireID );
        questionnaire.Style = SessionState.Questionnaire.Style;

        Response.Redirect( Request.RawUrl );
    }
    
    #endregion

    protected override void CreateChildControls()
    {
        Trace.Warn("CreateChildControls()");

        // Si ResynchroControlPollQuestionId est != SessionState.Question.PollQuestionId
        // c'est que l'internaute a modifie l'interface graphique (cliquant sur previous ou next)
        // on se trouve en decallage / a l'interface graphique et c'est elle qui nous donne 
        // la question qu'est en train de visualiser l'internaute.
        bool resynchro = false;

        Reporter.Trace( " ResynchroControlPollQuestionId : {0}", ResynchroControlPollQuestionId );
        Reporter.Trace( " SessionState.Question.Question : {0}", SessionState.Question.Question );
        Reporter.Trace( " SessionState.Question.PollQuestionId : {0}", SessionState.Question.PollQuestionId.ToString() );

        if ( SessionState.Question.PollQuestionId.ToString() != ResynchroControlPollQuestionId )
        {
            Reporter.Trace( "resynchro" );
            resynchro = true;
            string questionId = ResynchroControlPollQuestionId;

            // Trouver l'index de la Question cliquee
            int index = 0;
            foreach ( PollQuestion q in SessionState.Questions )
            {
                if ( q.PollQuestionId.ToString() == questionId )
                    break;
                index++;
            }
            SessionState.CurrentQuestionIndex = index;

            // RechAjout bouton submitarger la Question et ses Reponses
            SessionState.Question = SessionState.Questions[ SessionState.CurrentQuestionIndex ];
            SessionState.Reponses = PollAnswerCollection.GetByPollQuestionID( SessionState.Question.PollQuestionId );

            // Effacer les Questions en Cours au dela de la Question cliquee
            if ( SessionState.QuestionsEnCours.Count > 0 )
            {
                // Trouver l'index de la Question cliquee dans QuestionsEnCours
                int indexQeC = 0;
                foreach ( PollQuestion q in SessionState.QuestionsEnCours )
                {
                    if ( q.PollQuestionId.ToString() == questionId )
                        break;
                    indexQeC++;
                }
                SessionState.QuestionsEnCours.RemoveRange( indexQeC + 1, SessionState.QuestionsEnCours.Count - ( indexQeC + 1 ) );

                // On ne conserve pas les Votes de la derniere Question en Cours
                PollQuestionCollection pqcConserve = SessionState.QuestionsEnCours;
                pqcConserve.RemoveAt( SessionState.QuestionsEnCours.Count - 1 );

                PollVoteCollection pvcConcerve = new PollVoteCollection();
                foreach ( PollQuestion question in pqcConserve )
                {
                    PollAnswerCollection reponses = PollAnswerCollection.GetByPollQuestionID( question.PollQuestionId );
                    foreach ( PollAnswer reponse in reponses )
                    {
                        PollVoteCollection pvc = SessionState.VotesEnCours.FindByAnswerID( reponse.PollAnswerId );
                        foreach ( PollVote v in pvc )
                        {
                            pvcConcerve.Add( v );
                        }
                    }
                }

                SessionState.VotesEnCours.Clear();
                foreach ( PollVote v in pvcConcerve )
                {
                    SessionState.VotesEnCours.Add( v );
                }
            }
        }

        if ( SessionState.QuestionnaireEnModePage )
        {
            if ( resynchro )
            {
                SessionState.PageQuestionsIndex = SessionState.CurrentQuestionIndex;
                SessionState.PageQuestions = new PollQuestionCollection();
                ChargerPageQuestions();
            }
        }

        if ( SessionState.QuestionnaireEnModePage )
        {
            CreatePageControls();
        }
        else
        {
            CreateQuestionControls();
        }

        base.CreateChildControls();
    }

    private void ChargerPageQuestions()
    {
        Reporter.Trace( "ChargerPageQuestions()" );

        SessionState.PageQuestions.Clear();

        bool sortir = false;
        bool titre = false;
        while ( sortir == false )
        {
            PollQuestion question = SessionState.Questions[ SessionState.PageQuestionsIndex ];

            // Attraper le titre de la premiere Question de la Page
            if ( titre == false )
            {
                SessionState.PageQuestions.Add( question );
                if ( string.IsNullOrEmpty( question.SautPage ) == false )
                {
                    PageQuestionsTitre = question.SautPage;
                }
                titre = true;
            }
            else
            {
                // Ajouter toutes les questions jusqu'au prochain saut de page
                if ( string.IsNullOrEmpty( question.SautPage ) )
                {
                    SessionState.PageQuestions.Add( question );
                }
                else
                {
                    sortir = true;
                    break;
                }
            }

            SessionState.PageQuestionsIndex += 1;
            sortir = SessionState.PageQuestionsIndex >= SessionState.Questions.Count;
        }
    }

    private HorizontalAlign AlignementEnum( string alignement )
    {
        HorizontalAlign ha = HorizontalAlign.NotSet;
        switch ( alignement )
        {
            case "Gauche":
                ha = HorizontalAlign.Left;
                break;
            case "Droite":
                ha = HorizontalAlign.Right;
                break;
            case "Centre":
                ha = HorizontalAlign.Center;
                break;
        }
        return ha;
    }

    private void AddNewRow( ref Table table, ref TableRow row, ref TableCell cell )
    {
        row.Cells.Add( cell );
        table.Rows.Add( row );
        cell = new TableCell();
        row = new TableRow();
    }

    private void AddNewRowQuestion( ref Table table, ref TableRow row, ref TableCell cell )
    {
        switch ( SessionState.Question.AlignementQuestion )
        {
            case "Gauche":
                cell.HorizontalAlign = HorizontalAlign.Left;
                break;
            case "Droite":
                cell.HorizontalAlign = HorizontalAlign.Right;
                break;
            case "Centre":
                cell.HorizontalAlign = HorizontalAlign.Center;
                break;
        }
        row.Controls.Add( cell );
        table.Controls.Add( row );
        cell = new TableCell();
        row = new TableRow();
    }

    private void ApplyStyleGraphique( string toolTip, string typeStyle, WebControl control )
    {
        // Attention toolTip = nomStyle
        StyleWeb.ApplyStyleWeb( toolTip, typeStyle, control );

        if ( CheckBoxModeDebugGraphiqueToolTip.Checked )
        {
            control.ToolTip = toolTip;
        }

        if ( CheckBoxModeDebugGraphiqueTable.Checked || CheckBoxModeDebugGraphiqueCellule.Checked )
        {
            Style oldStyle = control.ControlStyle;

            Style styleDebug = new Style();
            styleDebug.BorderStyle = BorderStyle.Solid;
            control.ToolTip = toolTip;

            // Seulement et seulement si BorderWidth est vide sinon tout en rouge 
            if ( oldStyle.BorderWidth.IsEmpty == true )
            {
                styleDebug.BorderWidth = new Unit( 1 );
            }

            // Pour les Tables
            if ( CheckBoxModeDebugGraphiqueTable.Checked )
            {
                if ( typeStyle == TypeStyleWeb.Table )
                {
                    styleDebug.BorderColor = Color.Red;
                    control.ApplyStyle( styleDebug );
                }
            }
            
            // Pour tous les autres Objets
            if ( CheckBoxModeDebugGraphiqueCellule.Checked )
            {
                if ( typeStyle != TypeStyleWeb.Table )
                {
                    styleDebug.BorderColor = Color.Green;
                    control.ApplyStyle( styleDebug );
                }
                if ( typeStyle == TypeStyleWeb.CheckBoxList )
                {
                    styleDebug.BorderColor = Color.Blue;
                    control.ApplyStyle( styleDebug );
                }
                if ( typeStyle == TypeStyleWeb.RadioButtonList )
                {
                    styleDebug.BorderColor = Color.Blue;
                    control.ApplyStyle( styleDebug );
                }
                if ( typeStyle == TypeStyleWeb.TextBox )
                {
                    styleDebug.BorderColor = Color.Orange;
                    control.ApplyStyle( styleDebug );
                }
            }
        }
    }

    private void CreatePageControls()
    {
        Reporter.Trace( Report.TRACE0, "CreatePageControls()" );

        // Garder la synchro
        PollQuestion questionSynchro = SessionState.Question;
        PollAnswerCollection reponsesSynchro = SessionState.Reponses;

        PanelQuestionnaire.Controls.Clear();
        ApplyStyleGraphique( "CadreQuestionnaire", TypeStyleWeb.Label, PanelQuestionnaire );

        Table tablePageQuestion = new Table();
        ApplyStyleGraphique( "PageQuestion", TypeStyleWeb.Table, tablePageQuestion );

        // Titre des pages
        Label titrePage = new Label();
        titrePage.Text = PageQuestionsTitre;
        ApplyStyleGraphique( "TitrePage", TypeStyleWeb.Label, titrePage );

        TableCell cellTable = new TableCell();
        TableRow rowTable = new TableRow();

        Table tableTitre = new Table();
        ApplyStyleGraphique( "TableTitrePage", TypeStyleWeb.Table, tableTitre );

        TableCell cellTitre = new TableCell();
        TableRow rowTitre = new TableRow();

        cellTitre.Controls.Add( titrePage );
        rowTitre.Controls.Add( cellTitre );
        tableTitre.Controls.Add( rowTitre );

        cellTable.Controls.Add( tableTitre );
        rowTable.Controls.Add( cellTable );
        if ( CheckBoxModeDebugGraphiqueToolTip.Checked )
        {
            cellTable.ToolTip = "Pas de style";
        }
        AddNewRow( ref tablePageQuestion, ref rowTable, ref cellTable );

        int indexID = 0;
        string erreurMessage = "";
        // BUG171020090001
        bool debutTableau = false;
        Reporter.Trace( Report.TRACE0, "CreatePageControls() SessionState.PageQuestions.Count : {0}", SessionState.PageQuestions.Count );
        while ( indexID < SessionState.PageQuestions.Count )
        {
            SessionState.Question = SessionState.PageQuestions[ indexID ];
            SessionState.Reponses = PollAnswerCollection.GetByPollQuestionID( SessionState.Question.PollQuestionId );

            // BUG171020090001
            // Verifier qu'il y a bien un debut de tableau
            if ( SessionState.Question.Tableau != string.Empty && SessionState.Question.Tableau != Tableau.Fin )
            {
                debutTableau = true;
            }
                
            // Presenter les questions sous forme de tableau
            // Debug false &&
            if ( SessionState.Question.Tableau != string.Empty /* BUG171020090001 && SessionState.Question.Tableau != Tableau.Fin */ )
            {
                bool sortir = false;
                bool erreur = false;
                int indexTableauID = 0;
                PollQuestionCollection questions = new PollQuestionCollection();

                // BUG171020090001 Test Molnlycke le tableau commence par une fin de Tableau
                if ( Page.Request[ "PollQuestionId" ] == null )
                {
                    if ( debutTableau == false && SessionState.Question.Tableau == Tableau.Fin )
                    {
                        sortir = true;
                        erreur = true;
                    }
                }

                while ( sortir == false )
                {
                    Reporter.Trace( Report.TRACE0, "  indexID : {0} indexTableauID {1}", indexID, indexTableauID );
                    if ( indexID + indexTableauID >= SessionState.PageQuestions.Count )
                    {
                        erreur = true;
                        break;
                    }

                    sortir = SessionState.PageQuestions[ indexID + indexTableauID ].Tableau == Tableau.Fin;

                    // Tagger Questions dans le tableau 
                    // pour reperer les objets RadioButton MutuallyExclusiveCheckBoxExtender
                    // attention de na pas tagger la fin de tableau : bug cherche pendant 3 heures
                    if ( SessionState.PageQuestions[ indexID + indexTableauID ].Tableau == string.Empty
                        && SessionState.PageQuestions[ indexID + indexTableauID ].Tableau != Tableau.Fin )
                    {
                        SessionState.PageQuestions[ indexID + indexTableauID ].Tableau = "tableau";
                    }

                    questions.Add( SessionState.PageQuestions[ indexID + indexTableauID ] );
                    indexTableauID += 1;
                }

                if ( erreur == false )
                {
                    Table tableTableau = new Table();
                    tableTableau.EnableViewState = false; // eviter un crash serveur
                    Table tableTitreTableau = new Table();
                    Table tablePager = new Table();

                    Reporter.Trace( Report.TRACE0, "CreateTableauControls( indexID : {0},", indexID.ToString() );
                    CreateTableauControls( indexID, ref tableTitreTableau, ref tableTableau, ref tablePager, questions );

                    cellTable.Controls.Add( tableTitreTableau );
                    rowTable.Controls.Add( cellTable );
                    AddNewRow( ref tablePageQuestion, ref rowTable, ref cellTable );

                    // Tableau et Pager dans la meme table 
                    // Pour que le Pager ait la meme taille que le Tableau
                    Table table = new Table();
                    TableCell cell = new TableCell();
                    TableRow row = new TableRow();

                    // Le message a l'utilisateur en haut du tableau
                    if ( questions[ 0 ].Message != "" && questions[ 0 ].MessageHaut == true )
                    {
                        // Mettre le message dans une table pour pouvoir appliquer un style Table
                        Label lblMessage = new Label();
                        lblMessage.Text = questions[ 0 ].Message;
                        Table _table = new Table();
                        TableCell _cell = new TableCell();
                        TableRow _row = new TableRow();
                        _cell.Controls.Add( lblMessage );
                        _row.Controls.Add( _cell );
                        _table.Controls.Add( _row );
                        ApplyStyleGraphique( "TableMessage", TypeStyleWeb.Table, _table );

                        cell.Controls.Add( _table );
                        AddNewRow( ref table, ref row, ref cell );
                    }

                    cell.Controls.Add( tableTableau );
                    ApplyStyleGraphique( "CadreQuestion", TypeStyleWeb.Table, table );

                    row.Controls.Add( cell );
                    AddNewRow( ref table, ref row, ref cell );

                    // Le message a l'utilisateur en bas du tableau
                    if ( questions[ questions.Count - 1 ].Message != "" && questions[ questions.Count - 1 ].MessageHaut == false )
                    {
                        // Mettre le message dans une table pour pouvoir appliquer un style Table
                        Label lblMessage = new Label();
                        lblMessage.Text = questions[ questions.Count - 1 ].Message;
                        Table _table = new Table();
                        TableCell _cell = new TableCell();
                        TableRow _row = new TableRow();
                        _cell.Controls.Add( lblMessage );
                        _row.Controls.Add( _cell );
                        _table.Controls.Add( _row );
                        ApplyStyleGraphique( "TableMessage", TypeStyleWeb.Table, _table );

                        cell.Controls.Add( _table );
                        AddNewRow( ref table, ref row, ref cell );
                    }

                    cell.Controls.Add( tablePager );
                    row.Controls.Add( cell );
                    AddNewRow( ref table, ref row, ref cell );

                    cellTable.Controls.Add( table );
                    rowTable.Controls.Add( cellTable );
                    ApplyStyleGraphique( "CelluleQuestion", TypeStyleWeb.Label, cellTable );
                    AddNewRow( ref tablePageQuestion, ref rowTable, ref cellTable );

                    indexID += indexTableauID;
                }
                else
                {
                    // BUG171020090001
                    if ( SessionState.Question.Tableau != Tableau.Fin )
                    {
                        erreurMessage = "Erreur : le tableau \"" + SessionState.Question.Tableau + "\" ne possède pas de fin tableau";
                    }
                    else
                    {
                        erreurMessage = "Erreur : le tableau ne possède pas de début de tableau";
                    }

                    QuestionnaireErreur = true;
                    // Sortir de la boucle while ( indexID < SessionState.PageQuestions.Count )
                    indexID = SessionState.PageQuestions.Count;
                }
            }
            else
            {
                Table tableQuestionReponse = new Table();
                tableQuestionReponse.EnableViewState = false; // eviter un crash serveur
                ApplyStyleGraphique( "CadreQuestion", TypeStyleWeb.Table, tableQuestionReponse );

                CreateControls( indexID, ref tableQuestionReponse );

                ApplyStyleGraphique( "CelluleQuestion", TypeStyleWeb.Label, cellTable );

                cellTable.Controls.Add( tableQuestionReponse );
                rowTable.Cells.Add( cellTable );
                AddNewRow( ref tablePageQuestion, ref rowTable, ref cellTable );

                indexID += 1;
            }
        }

        PanelQuestionnaire.Controls.Add( tablePageQuestion );

        if ( erreurMessage != "" )
        {
            Label labelMessage = new Label();
            labelMessage.Text = erreurMessage;
            labelMessage.CssClass = "LabelValidationMessageErrorStyle";

            Table table = new Table();
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            table.BorderWidth = new Unit( 0 );
            table.CellPadding = 10;
            table.CellSpacing = 1;
            table.Width = new Unit( "100%" );
            cell.Controls.Add( labelMessage );
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Controls.Add( labelMessage );
            row.Controls.Add( cell );
            table.Controls.Add( row );
            PanelQuestionnaire.Controls.Add( table );
        }

        // Remettre la synchro a jour 
        SessionState.Question = questionSynchro;
        SessionState.Reponses = reponsesSynchro;

        // Encore une grosse merde infame !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // on ne peut pas creer ce bouton dynamiquement comme lorsque
        // Questionnaire est un control dans App_Code/QuestionnaireControl.cs
        // Sinon il ne fonctionne pas ou du moins semble fonctionner une fois
        // sur deux !!!!!!!
        // Il est donc en dur dans l'interface graphique
        // J'ai mis une journee : le dimanche 28/09/2008 a corriger ca
        //
        // Ajouter le bouton submit en bas de la page
        //Reporter.Trace( "  Ajout bouton submit" );
        //RolloverButton rlb = new RolloverButton();
        //rlb.Text = "Suivante";
        //rlb.Click += new EventHandler( SubmitButtonPageQuestions_Click );
        //PanelQuestionnaire.Controls.Add( rlb );
    }

    private void CreateQuestionControls()
    {
        Table tablePageQuestion = new Table();
        ApplyStyleGraphique( "PageQuestion", TypeStyleWeb.Table, tablePageQuestion );

        Table tableQuestionReponse = new Table();
        tableQuestionReponse.EnableViewState = false; // eviter un crash serveur
        ApplyStyleGraphique( "CadreQuestion", TypeStyleWeb.Table, tableQuestionReponse );

        TableRow rowTable = new TableRow();
        TableCell cellTable = new TableCell();

        // Tout se passe ici :
        CreateControls( 0, ref tableQuestionReponse );

        ApplyStyleGraphique( "CelluleQuestion", TypeStyleWeb.Label, cellTable );

        cellTable.Controls.Add( tableQuestionReponse );
        rowTable.Cells.Add( cellTable );
        tablePageQuestion.Rows.Add( rowTable );

        PanelQuestionnaire.Controls.Add( tablePageQuestion );
    }

    // indexID pour incrementer les ID en cas de mode page
    private void CreateControls( int indexID, ref Table tableQuestion )
    {
        Reporter.Trace( "CreateControls( indexID : {0} )", indexID );

        // A cause de la construction merdique ID des TextBoxDate et des PopupTextBox
        // on est oblige de faire une petite verification : 
        // si deux TextBoxDate ou deux PopupTextBox on le meme rank => Erreur
        bool erreur = false;
        int rank = -100;
        string type = "";
        string question = "";
        foreach ( PollAnswer reponse1 in SessionState.Reponses )
        {
            rank = reponse1.Rank;
            if ( reponse1.TypeReponse == TypeReponse.Date || reponse1.TypeReponse == TypeReponse.SemiOuverte )
            {
                foreach ( PollAnswer reponse2 in SessionState.Reponses )
                {
                    if ( ( reponse2.TypeReponse == TypeReponse.Date
                              || reponse2.TypeReponse == TypeReponse.SemiOuverte )
                        && reponse2.Rank == rank
                        && reponse1.PollAnswerId != reponse2.PollAnswerId )
                    {
                        erreur = true;
                        type = reponse1.TypeReponse;
                        question = SessionState.Question.Question;
                        break;
                    }
                }
            }
            if ( erreur ) break;
        }

        if ( erreur == true )
        {
            Label labelMessage = new Label();
            labelMessage.Text = string.Format( "Erreur dans la Question : {0}<br/>deux réponses de type : {1} ont le même rang : <b>{2}</b>", question, type, rank );
            labelMessage.CssClass = "LabelValidationMessageErrorStyle";
            TableCell cell = new TableCell();
            cell.Controls.Add( labelMessage );
            TableRow row = new TableRow();
            row.Cells.Add( cell );
            AddNewRow( ref tableQuestion, ref row, ref cell );

            QuestionnaireErreur = true;
            return;
        }
        // fin des verifications on peut dessiner le control

        TableRow rowQuestionReponse = new TableRow();
        TableCell cellQuestionReponse = new TableCell();

        if ( SessionState.Questions.Count <= 0 )
        {
            Label noPollAvailableLabel = new Label();
            noPollAvailableLabel.Text = "Il n'y a pas de question pour ce questionnaire !";
            cellQuestionReponse.Controls.Add( new LiteralControl( "<i>" ) );
            cellQuestionReponse.Controls.Add( noPollAvailableLabel );
            cellQuestionReponse.Controls.Add( new LiteralControl( "</i>" ) );
            return; // TODO: might not be correct. Was : Exit Sub
        }

        // Le message a l'utilisateur
        if ( SessionState.Question.Message != "" && SessionState.Question.MessageHaut == true )
        {
            // Mettre le message dans une table pour pouvoir appliquer un style Table
            Label lblMessage = new Label();
            lblMessage.Text = SessionState.Question.Message;
            Table _table = new Table();
            TableCell _cell = new TableCell();
            TableRow _row = new TableRow();
            _cell.Controls.Add( lblMessage );
            _row.Controls.Add( _cell );
            _table.Controls.Add( _row );
            ApplyStyleGraphique( "TableMessage", TypeStyleWeb.Table, _table );

            cellQuestionReponse.Controls.Add( _table );
            AddNewRow( ref tableQuestion, ref rowQuestionReponse, ref cellQuestionReponse );
        }

        // Question
        // Mettre la Question dans une table pour pouvoir appliquer un style Table
        Label questionLabel = new Label();
        questionLabel.Text = SessionState.Question.Question;
        Table q_table = new Table();
        TableCell q_cell = new TableCell();
        TableRow q_row = new TableRow();
        q_cell.Controls.Add( questionLabel );
        q_row.Controls.Add( q_cell );
        q_table.Controls.Add( q_row );
        ApplyStyleGraphique( "TableTitreQuestion", TypeStyleWeb.Table, q_table );

        cellQuestionReponse.Controls.Add( q_table );
        AddNewRow( ref tableQuestion, ref rowQuestionReponse, ref cellQuestionReponse );

        // Reponses Choix
        ListItemCollection lic = new ListItemCollection();
        bool licEnable = true;

        if ( SessionState.Reponses.Count > 0 )
        {
            // Recuperer les Reponses de Type Choix
            PollAnswerCollection reponseChoix = new PollAnswerCollection();
            foreach ( PollAnswer reponse in SessionState.Reponses )
            {
                if ( reponse.TypeReponse == TypeReponse.Choix )
                {
                    reponseChoix.Add( reponse );
                }
            }

            foreach ( PollAnswer answer in reponseChoix )
            {
                string reponse = answer.Answer;
                if ( CheckBoxModeTest.Checked == true )
                {
                    reponse += " <font color=\"Green\">R" + answer.Rank.ToString() + "</font>";
                }

                ListItem itemReponse = new ListItem( reponse, answer.PollAnswerId.ToString() );

                if ( SessionState.Question.ChoixMultiple )
                {
                    bool voteInDataBase = false;
                    ArrayList pollAnswerId = GetUserAnswers( ref voteInDataBase );
                    // L'interviewe ne peut pas voter a nouveau
                    licEnable = ( voteInDataBase == false );
                    foreach ( Guid answerId in pollAnswerId )
                    {
                        if ( answerId == answer.PollAnswerId )
                            itemReponse.Selected = true;
                    }
                }
                else
                {
                    bool voteInDataBase = false;
                    Guid pollAnswerId = GetUserAnswer( ref voteInDataBase );
                    // L'interviewe ne peut pas voter a nouveau
                    licEnable = ( voteInDataBase == false );
                    if ( pollAnswerId != Guid.Empty )
                    {
                        if ( pollAnswerId == answer.PollAnswerId )
                            itemReponse.Selected = true;
                    }
                }

                lic.Add( itemReponse );
            }

            // Une table pour les Reponses afin de pouvoir aligner !
            Table tableReponse = new Table();
            TableRow rowReponse = new TableRow();
            TableCell cellReponse = new TableCell();

            bool directionHorizontal = SessionState.Reponses[ 0 ].Horizontal;
            bool alignLeft = SessionState.Reponses[ 0 ].AlignLeft;
            if ( SessionState.Question.ChoixMultiple )
            {
                CheckBoxListStyle cbl = new CheckBoxListStyle();
                cbl.ID = "CheckBoxListStyleID" + indexID.ToString();
                Reporter.Trace( Report.TRACE0, "CreateControls - CheckBoxListStyleID : {0}", cbl.ID );
                cbl.Enabled = licEnable;
                foreach ( ListItem li in lic )
                {
                    cbl.Items.Add( li );
                }
                if ( alignLeft )
                {
                    cbl.TextAlign = TextAlign.Left;
                }
                if ( directionHorizontal )
                {
                    cbl.RepeatDirection = System.Web.UI.WebControls.RepeatDirection.Horizontal;
                }

                ApplyStyleGraphique( "Reponse", TypeStyleWeb.CheckBoxList, cbl );
                cellReponse.Controls.Add( cbl );
            }
            else // facon choix unique
            {
                RadioButtonListStyle rbl = new RadioButtonListStyle();
                rbl.ID = "RadioButtonListStyleID" + indexID.ToString();
                Reporter.Trace( Report.TRACE0, "CreateControls - RadioButtonListStyleID : {0}", rbl.ID );
                rbl.Enabled = licEnable;
                foreach ( ListItem li in lic )
                {
                    rbl.Items.Add( li );
                }
                if ( alignLeft )
                {
                    rbl.TextAlign = TextAlign.Left;
                }
                if ( directionHorizontal )
                {
                    rbl.RepeatDirection = System.Web.UI.WebControls.RepeatDirection.Horizontal;
                }

                ApplyStyleGraphique( "Reponse", TypeStyleWeb.RadioButtonList, rbl );
                cellReponse.Controls.Add( rbl );
            }

            rowReponse.Cells.Add( cellReponse );
            rowReponse.HorizontalAlign = AlignementEnum( SessionState.Question.AlignementReponse );
            tableReponse.Rows.Add( rowReponse );
            cellQuestionReponse.Controls.Add( tableReponse );
            AddNewRow( ref tableQuestion, ref rowQuestionReponse, ref cellQuestionReponse );

            Table tableReponseTextuelle = new Table();
            ApplyStyleGraphique( "TableReponseTextuelle", TypeStyleWeb.Table, tableReponseTextuelle );
            foreach ( PollAnswer reponse in SessionState.Reponses )
            {
                if ( reponse.TypeReponse != TypeReponse.Choix )
                {
                    TableRow row = new TableRow();
                    TableCell cellLabel = new TableCell();
                    TableCell cellTextBox = new TableCell();

                    if ( CheckBoxModeDebugGraphiqueCellule.Checked )
                    {
                        cellLabel.BorderStyle = BorderStyle.Solid;
                        cellLabel.BorderWidth = 1;
                        cellLabel.BorderColor = Color.Green;
                        cellLabel.ToolTip = "CellLabel pas de style";
                    }

                    switch ( reponse.TypeReponse )
                    {
                        // Les deux s'affichent de la meme facon
                        case TypeReponse.Ouverte:
                        case TypeReponse.Numerique:

                            // Label
                            string rep1 = "";
                            if ( CheckBoxModeTest.Checked == true )
                            {
                                rep1 += " <font color=\"Green\">R" + reponse.Rank.ToString() + "</font>";
                            }
                            rep1 += " " + reponse.Answer;

                            Label rep = new Label();
                            rep.Text = rep1;
                            ApplyStyleGraphique( "ReponseTextuelleLabel", TypeStyleWeb.Label, rep );
                            cellLabel.VerticalAlign = VerticalAlign.Top;
                            cellLabel.HorizontalAlign = HorizontalAlign.Right;
                            cellLabel.Controls.Add( rep );
                            row.Cells.Add( cellLabel );

                            // TextBox
                            TextBox textbox = new TextBox();
                            ApplyStyleGraphique( "ReponseTextuelleTextBox", TypeStyleWeb.TextBox, textbox );
                            if ( reponse.Width != 0 )
                            {
                                textbox.Width = reponse.Width;
                            }
                            if ( reponse.Rows > 1 )
                            {
                                textbox.TextMode = TextBoxMode.MultiLine;
                                textbox.Rows = reponse.Rows;
                            }

                            bool voteInDataBase = false;
                            string vote = GetUserAnswerForTextuelVote( ref voteInDataBase, reponse.PollAnswerId );
                            // L'interviewe ne peut pas voter a nouveau
                            textbox.Enabled = ( voteInDataBase == false );
                            textbox.ID = reponse.PollAnswerId.ToString(); // astuce ? et oui ca marche on peut retrouver la valeur de la TextBox donnee par l'utilisateur
                            textbox.Text = vote;

                            cellTextBox.Controls.Add( textbox );
                            cellTextBox.HorizontalAlign = HorizontalAlign.Left;
                            row.Cells.Add( cellTextBox );
                            tableReponseTextuelle.Rows.Add( row );

                            break;

                        case TypeReponse.Date:

                            TextBoxDate textBoxDate = ( TextBoxDate )LoadControl( "~/UserControl/TextBoxDate.ascx" );

                            // Label
                            string rep2 = "";
                            if ( CheckBoxModeTest.Checked == true )
                            {
                                rep2 += " <font color=\"Green\">R" + reponse.Rank.ToString() + "</font>";
                            }
                            rep2 += " " + reponse.Answer;

                            Label label = new Label();
                            try
                            {
                                label = ( Label )textBoxDate.FindControl( "LabelTextBoxDate" );
                            }
                            catch ( Exception ex )
                            {
                                string message = ex.Message;
                            }
                            label.Text = rep2;
                            ApplyStyleGraphique( "ReponseTextuelleLabel", TypeStyleWeb.Label, label );
                            // Encore une merde :
                            // comme le label est ajoute dans une cellule pour aligner toutes
                            // les reponses textuelle ce putain de label 
                            // se retrouve avec son ID duplique dans la page !
                            // comme on s'en branle on y fou ce n'importe quoi du moment
                            // que c'est pas duplique !
                            label.ID = "labelID" + reponse.PollAnswerId.ToString();
                            cellLabel.VerticalAlign = VerticalAlign.Top;
                            cellLabel.HorizontalAlign = HorizontalAlign.Right;
                            cellLabel.Controls.Add( label );
                            row.Cells.Add( cellLabel );

                            // TextBox
                            TextBox textboxD = ( TextBox )textBoxDate.FindControl( "TextBoxDateText" );
                            ApplyStyleGraphique( "ReponseTextuelleTextBox", TypeStyleWeb.TextBox, textboxD );

                            bool voteInDataBaseD = false;
                            string voteD = GetUserAnswerForTextuelVote( ref voteInDataBaseD, reponse.PollAnswerId );
                            // L'interviewe ne peut pas voter a nouveau
                            // Ca ne fonctionne pas vraiment comme on veut ! 
                            // la TextBox est bien invalidee mais si l'utilisateur
                            // clique sur le calendrier la date change
                            // elle ne sera pas prise en compte de toutes les facons...
                            textBoxDate.Enabled = ( voteInDataBaseD == false );
                            textboxD.Enabled = ( voteInDataBaseD == false );
                            textboxD.Text = voteD;

                            // Grosse merde on ne peut pas faire :
                            // textBoxDate.ID = reponse.PollAnswerId.ToString();
                            // ca bugue il manque un ; !!!!!!!!!!!!!!!!!!!!!!!!!
                            // en utilisant le rang on prend le risque qu'un intervieweur
                            // mette deux reponses de meme rang mais bon on ne peut
                            // utiliser indexID car il conserne une reponse et il peut
                            // y avoir plusieures reponses date dans une question
                            textBoxDate.ID = "TextBoxDateID" + indexID.ToString() + reponse.Rank.ToString(); 

                            cellTextBox.Controls.Add( textBoxDate );
                            cellTextBox.HorizontalAlign = HorizontalAlign.Left;
                            row.Cells.Add( cellTextBox );
                            tableReponseTextuelle.Controls.Add( row );

                            break;

                        case TypeReponse.SemiOuverte:

                            PopupTextBox popupTextBox = ( PopupTextBox )LoadControl( "~/UserControl/PopupTextBox.ascx" );
                            
                            // Label
                            string rep3 = "";
                            if ( CheckBoxModeTest.Checked == true )
                            {
                                rep3 += " <font color=\"Green\">R" + reponse.Rank.ToString() + "</font>";
                            }
                            rep3 += " " + reponse.Answer;

                            Label labelSO = ( Label )popupTextBox.FindControl( "LabelChecbox" );
                            ApplyStyleGraphique( "ReponseTextuelleLabel", TypeStyleWeb.Label, labelSO );
                            labelSO.Text = rep3;
                            // grosse merde encore ...
                            labelSO.ID = "saloperiedemerde" + reponse.PollAnswerId.ToString();
                            popupTextBox.LabelCheckBox = rep3;
                            cellLabel.VerticalAlign = VerticalAlign.Top;
                            cellLabel.HorizontalAlign = HorizontalAlign.Right;
                            cellLabel.Controls.Add( labelSO );
                            row.Cells.Add( cellLabel );

                            // TextBox
                            TextBox textboxSO = ( TextBox )popupTextBox.FindControl( "TextBoxText" );
                            ApplyStyleGraphique( "ReponseTextuelleTextBox", TypeStyleWeb.TextBox, textboxSO );
                            if ( reponse.Width != 0 )
                            {
                                popupTextBox.TextBoxWidth = reponse.Width.ToString();
                            }
                            if ( reponse.Rows > 1 )
                            {
                                popupTextBox.TextBoxRows = reponse.Rows;
                            }

                            bool voteInDataBaseSO = false;
                            string voteSO = GetUserAnswerForTextuelVote( ref voteInDataBaseSO, reponse.PollAnswerId );
                            // L'interviewe ne peut pas voter a nouveau
                            textboxSO.Enabled = ( voteInDataBaseSO == false );
                            textboxSO.Text = voteSO;

                            // Grosse merde, on ne peut pas faire :
                            //popupTextBox.ID = reponse.PollAnswerId.ToString(); 
                            // idem les scripts en java sont perdus et plantent comme des merdes.
                            popupTextBox.ID = "PopupTextBoxID" + indexID.ToString() + reponse.Rank.ToString();

                            Reporter.Trace( "  PopupTextBoxID : {0}", popupTextBox.ID );

                            cellTextBox.Controls.Add( popupTextBox );
                            cellTextBox.HorizontalAlign = HorizontalAlign.Left;
                            row.Controls.Add( cellTextBox );
                            tableReponseTextuelle.Controls.Add( row );

                            break;
                    } // switch ( reponse.TypeReponse )

                    cellQuestionReponse.Controls.Add( tableReponseTextuelle );
                    AddNewRow( ref tableQuestion, ref rowQuestionReponse, ref cellQuestionReponse );

                } // fin du if ( reponse.TypeReponse != TypeReponse.Choix )
            } // fin du foreach ( PollAnswer reponse ...
        }

        // Message a l'utilisateur
        if ( SessionState.Question.Message != "" && SessionState.Question.MessageHaut == false )
        {
            // Mettre le message dans une table pour pouvoir appliquer un style Table
            Label lblMessage = new Label();
            lblMessage.Text = SessionState.Question.Message;
            Table _table = new Table();
            TableCell _cell = new TableCell();
            TableRow _row = new TableRow();
            _cell.Controls.Add( lblMessage );
            _row.Controls.Add( _cell );
            _table.Controls.Add( _row );
            ApplyStyleGraphique( "TableMessage", TypeStyleWeb.Table, _table );

            cellQuestionReponse.Controls.Add( _table );
            AddNewRow( ref tableQuestion, ref rowQuestionReponse, ref cellQuestionReponse );
        }

        Table tablePager = new Table();
        ApplyStyleGraphique( "TableCompteurQuestions", TypeStyleWeb.Table, tablePager );

        TableRow rowPager = new TableRow();
        TableCell cellPager = new TableCell();

        int currentIndex = SessionState.CurrentQuestionIndex + 1 + indexID; // mode page, indexID est l'index de la Question dans la Page
        
        // Pager
        // COR21092009
        // COR09112009
        Label pager = new Label();
        if ( SessionState.Questionnaire.Compteur )
        {
            ApplyStyleGraphique( "CompteurQuestions", TypeStyleWeb.Label, rowPager );
            pager.Text = "Question " + currentIndex + "/" + SessionState.Questions.Count;
        }
        
        // Barre de progression
        ProgressBarre.CurrentState = currentIndex;
        ProgressBarre.LabelProgressText = ProgressBarre.CurrentState.ToString() + "/" + ProgressBarre.States;
        if ( ProgressBarre.CurrentState == ProgressBarre.States )
        {
            ProgressBarre.LabelProgressText = "Dernière question";
        }

        // COR21092009
        // COR09112009
        if ( SessionState.Questionnaire.Compteur )
        {
            cellPager = new TableCell();
            cellPager.Controls.Add( pager );
            cellPager.HorizontalAlign = HorizontalAlign.Center;
            rowPager.Controls.Add( cellPager );
            tablePager.Controls.Add( rowPager );
        }

        // Encore une grosse merde infame !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // on ne peut pas creer ce bouton dynamiquement comme lorsque
        // Questionnaire est un control dans App_Code/QuestionnaireControl.cs
        // Sinon il ne fonctionne pas ou du moins semble fonctionner une fois
        // sur deux !!!!!!!
        //RolloverButton rolloverButtonSubmit = new RolloverButton();
        Label lblmessage = new Label();
        lblmessage.CssClass = "QuestionnaireLabelMessageUtilisateurStyle";

        // Le mode Trace du Questionnaire
        if ( CheckBoxModeTest.Checked )
        {
            Label lblMessageTest = new Label();
            lblMessageTest.Text += "<br/>rang : <b><font color=\"Red\">" + SessionState.Question.Rank + "</font></b>&nbsp;";
            lblMessageTest.Text += " idx : <b><font color=\"Blue\">" + SessionState.CurrentQuestionIndex + "</font></b>&nbsp;";
            if ( SessionState.Question.Instruction != "" )
                lblMessageTest.Text += " inst : <font color=\"Green\">" + SessionState.Question.Instruction + "</font>";
            if ( SessionState.Question.Obligatoire )
                lblMessageTest.Text += " <font color=\"Blue\">obligatoire</font>";
            if ( SessionState.Question.Fin )
                lblMessageTest.Text += " <font color=\"Red\">fin</font>";

            lblMessageTest.Text += "<br/><br/>";
            cellPager = new TableCell();
            cellPager.Controls.Add( lblMessageTest );
            cellPager.ColumnSpan = 2;
            cellPager.Width = new Unit( "100%" );
            cellPager.HorizontalAlign = HorizontalAlign.Center;
            rowPager = new TableRow();
            rowPager.Controls.Add( cellPager );
            tablePager.Controls.Add( rowPager );
        }

        if ( SessionState.Reponses.Count > 0 )
        {
            if ( HasUserAlreadyVotedInThePast() )
            {
                lblmessage.Text = "<br/>Vous avez déjà répondu à cette question.<br/><br/>";
            }
        }
        else // Il n'y a pas de réponse pour cette Question, juste le bouton Suivante
        {
            lblmessage.Text = "<br/>Pas de réponse à donner pour cette question.<br /><br/>";
        }

        if ( lblmessage.Text != "" )
        {
            cellPager = new TableCell();
            cellPager.Controls.Add( lblmessage );
            cellPager.ColumnSpan = 2;
            cellPager.Width = new Unit( "100%" );
            cellPager.HorizontalAlign = HorizontalAlign.Center;
            rowPager = new TableRow();
            rowPager.Controls.Add( cellPager );
            tablePager.Controls.Add( rowPager );
        }

        cellPager = new TableCell();

        // Sinon le bouton est ajoute dans CreatePageControls()
        // Faux on ne peut pas creer dynamiquement ce control
        // sinon il fonctionne un fois sur deux !!!!
        //if ( SessionState.QuestionnaireEnModePage == false )
        //{
        //    cellPager.Controls.Add( rolloverButtonSubmit );
        //}
        cellPager.ColumnSpan = 2;
        cellPager.Width = new Unit( "100%" );
        cellPager.HorizontalAlign = HorizontalAlign.Center;
        rowPager = new TableRow();
        rowPager.Controls.Add( cellPager );
        tablePager.Controls.Add( rowPager );

        cellQuestionReponse.Controls.Add( tablePager );
        AddNewRow( ref tableQuestion, ref rowQuestionReponse, ref cellQuestionReponse );
    }

    // meme chose que CreateControls mais pour un tableau
    // indexID est l'index de debut du tableau pour diffencier les RadioButtonList on prend un index
    // interne
    private void CreateTableauControls( int indexID, ref Table tableTitre, ref Table tableTableau, ref Table tablePager, PollQuestionCollection questions )
    {
        SessionState.Reponses = PollAnswerCollection.GetByPollQuestionID( questions[ 0 ].PollQuestionId );

        // Verification complete du tableau
        // on ne peut mettre que des reponses choix simple dans les tableaux de classement
        if ( questions[ 0 ].Tableau.Contains( Tableau.Classement ) )
        {
            foreach ( PollQuestion question in questions )
            {
                if ( question.ChoixMultiple )
                {
                    Label label = new Label();
                    label.Text = string.Format( "Erreur : La question \"{0}\" est de type choix multiple, il ne peut y avoir que des question de type choix simple dans les tableaux de classement", question.Question );
                    label.CssClass = "LabelValidationMessageErrorStyle";
                    TableRow row = new TableRow();
                    TableCell cell = new TableCell();
                    cell.Controls.Add( label );
                    cell.ColumnSpan = 10;
                    row.Cells.Add( cell );
                    tableTableau.Rows.Add( row );

                    QuestionnaireErreur = true;
                    break;
                }
            }
        }        
        // Verification complete du tableau
        // on ne peut pas mettre autre chose que des reponses choix dans les tableaux
        foreach ( PollQuestion question in questions )
        {
            PollAnswerCollection pollAnswers = PollAnswerCollection.GetByPollQuestionID( question.PollQuestionId );
            foreach ( PollAnswer reponse in pollAnswers )
            {
                if ( reponse.TypeReponse != TypeReponse.Choix )
                {
                    Label label = new Label();
                    label.Text = string.Format( "Erreur : La réponse \"{0}\" de la question \"{1}\" est de type \"{2}\" et n'est pas de type choix", reponse.Answer, question.Question, reponse.TypeReponse );
                    label.CssClass = "LabelValidationMessageErrorStyle";
                    TableRow row = new TableRow();
                    TableCell cell = new TableCell();
                    cell.Controls.Add( label );
                    cell.ColumnSpan = 10;
                    row.Cells.Add( cell );
                    tableTableau.Rows.Add( row );

                    SessionState.Reponses.Remove( reponse );
                    QuestionnaireErreur = true;
                }
            }
        }
        // Verification
        // dans les tableaux, seule la derniere question peut etre programmee
        // sinon c'est la panique
        // et il faut qu'elles soient de meme type "simple" ou "multiple"
        int index = 1;
        bool type = questions[ 0 ].ChoixMultiple;
        foreach ( PollQuestion question in questions )
        {
            if ( question.ChoixMultiple != type )
            {
                Label label = new Label();
                label.Text = string.Format( "Erreur : La question \"{0}\" n'est pas du type \"{1}\"", question.Question, type == true ? "Choix multiple" : "Choix simple" );
                label.CssClass = "LabelValidationMessageErrorStyle";
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.Controls.Add( label );
                cell.ColumnSpan = 10;
                row.Cells.Add( cell );
                tableTableau.Rows.Add( row );

                QuestionnaireErreur = true;
            }

            int count = questions.Count;
            if ( question.Instruction != "" )
            {
                if ( index != count )
                {
                    Label label = new Label();
                    label.Text = string.Format( "Erreur : La question \"{0}\" est programmée mais ce n'est pas la dernière question du tableau", question.Question );
                    label.CssClass = "LabelValidationMessageErrorStyle";
                    TableRow row = new TableRow();
                    TableCell cell = new TableCell();
                    cell.Controls.Add( label );
                    cell.ColumnSpan = 10;
                    row.Cells.Add( cell );
                    tableTableau.Rows.Add( row );

                    QuestionnaireErreur = true;
                }
                else
                {
                    // On positionne SessionState.Question sur la derniere question de la serie
                    // pour que le message : lblMessageTest affiche correctement l'instruction
                    // de la derniere question du tableau, c'est une astuce de merde ... c'est sur
                    SessionState.Question = question;
                }

            }
            index += 1;
        }

        // Table Titre du tableau
        ApplyStyleGraphique( "TableTitreTableau", TypeStyleWeb.Table, tableTitre );

        // Table Tableau Style
        ApplyStyleGraphique( "CadreTableau", TypeStyleWeb.Table, tableTableau );

        // Pager Style
        ApplyStyleGraphique( "TableCompteurQuestions", TypeStyleWeb.Table, tablePager );

        TableRow rowTableau = new TableRow();
        TableCell cellTableau = new TableCell();

        Reporter.Trace( "CreateTableauRow( questions : {0} )", questions.Count );

        bool tableauClassement = questions[ 0 ].Tableau.Contains( Tableau.Classement );
        string titreTableau = questions[ 0 ].Tableau; // BUG05062010_2
        if ( tableauClassement )
        {
            titreTableau = questions[ 0 ].Tableau.Substring( 0, questions[ 0 ].Tableau.Length - Tableau.Classement.Length );
        }

        // Titre du tableau
        TableRow rowTitre = new TableRow();
        TableCell cellTitre = new TableCell();

        Label titreLabel = new Label();
        titreLabel.Text = titreTableau;
        ApplyStyleGraphique( "TitreTableau", TypeStyleWeb.Label, titreLabel );

        cellTitre.Controls.Add( titreLabel );
        rowTitre.Cells.Add( cellTitre );
        tableTitre.Rows.Add( rowTitre );


        // On ne fait pas comme cela pour le tableau
        //ListItemCollection lic = new ListItemCollection();
        // car les RadioBox ou les CheckBox ne sont pas regroupees dans 
        // une seule liste !

        if ( SessionState.Reponses.Count > 0 )
        {
            // Premiere ligne du Tableau
            // la premiere cellule de la premiere ligne est vide
            cellTableau.Controls.Add( new LiteralControl( "&nbsp;" ) );
            ApplyStyleGraphique( "CelluleReponseTableau", TypeStyleWeb.Label, cellTableau );
            rowTableau.Cells.Add( cellTableau );

            foreach ( PollAnswer reponse in SessionState.Reponses )
            {
                cellTableau = new TableCell();
                cellTableau.HorizontalAlign = HorizontalAlign.Center;
                ApplyStyleGraphique( "CelluleReponseTableau", TypeStyleWeb.Label, cellTableau );

                Label labelReponse = new Label();
                labelReponse.Text = reponse.Answer;
                ApplyStyleGraphique( "ReponseTableau", TypeStyleWeb.Label, labelReponse );

                cellTableau.Controls.Add( labelReponse );
                rowTableau.Cells.Add( cellTableau );
            }
            tableTableau.Rows.Add( rowTableau );

            // Boucle sur les lignes
            int indexTableau = 0;
            int indexLigne = 0;
            int indexColonne = 0;
            foreach ( PollQuestion question in questions )
            {
                // Positionner la SessionState.Question sur la bonne question pour que les fonctions
                // GetUserAnswers et GetUserAnswer
                // fonctionnent 
                // ca commence a etre bien la merde ce composant ...
                SessionState.Question = question;

                // pour que les fonctions 
                rowTableau = new TableRow();
                cellTableau = new TableCell();
                // Debug
                //cellTableau.BorderStyle = BorderStyle.Solid;
                //cellTableau.BorderWidth = 1;

                Label labelQuestion = new Label();
                labelQuestion.Text = question.Question;
                ApplyStyleGraphique( "QuestionTableau", TypeStyleWeb.Label, labelQuestion );

                // Premiere colonne du tableau : la Question
                cellTableau.Controls.Add( labelQuestion );
                ApplyStyleGraphique( "CelluleQuestionTableau", TypeStyleWeb.Label, cellTableau );
                cellTableau.HorizontalAlign = HorizontalAlign.Right;
                rowTableau.Cells.Add( cellTableau );

                // Boucle sur les colonnes
                SessionState.Reponses = PollAnswerCollection.GetByPollQuestionID( question.PollQuestionId );
                foreach ( PollAnswer answer in SessionState.Reponses )
                {
                    cellTableau = new TableCell();
                    ApplyStyleGraphique( "CelluleQuestionTableau", TypeStyleWeb.Label, cellTableau );
                    cellTableau.HorizontalAlign = HorizontalAlign.Center;

                    string reponse = "";
                    if ( CheckBoxModeTest.Checked == true )
                    {
                        if ( question.Instruction != "" )
                        {
                            reponse += "<font color=\"Green\" style=\"font-size:smaller\">R" + answer.Rank.ToString() + "</font>";
                        }
                    }

                    //ListItem itemReponse = new ListItem( reponse, answer.PollAnswerId.ToString() );
                    if ( SessionState.Question.ChoixMultiple )
                    {
                        CheckBox cb = new CheckBox();
                        cb.ID = answer.PollAnswerId.ToString();
                        //"CheckBoxID" + ( indexID + indexTableau ).ToString();
                        // = answer.PollAnswerId.ToString(); on retente le coup ! HE NON
                        //C'est la grosse merde ca marche pas non plus a croire que cettte 
                        //putain d'astuce de merde ne marche qu'avec les TextBox c'est quand 
                        //la grosse merde

                        // L'utilisateur a deja vote ?
                        bool voteInDataBase = false;
                        ArrayList pollAnswerId = GetUserAnswers( ref voteInDataBase );
                        // L'interviewe ne peut pas voter a nouveau
                        foreach ( Guid answerId in pollAnswerId )
                        {
                            if ( answerId == answer.PollAnswerId )
                                cb.Checked = true;
                        }

                        cb.Enabled = ( voteInDataBase == false );
                        //StyleWeb.ApplyStyleWeb( "Reponse", TypeStyleWeb.CheckBoxList, cb );

                        cb.Text = reponse;

                        if ( SessionState.Reponses[ 0 ].AlignLeft )
                        {
                            cb.TextAlign = TextAlign.Left;
                        }

                        cellTableau.Controls.Add( cb );
                        rowTableau.Cells.Add( cellTableau );
                    }
                    else // Choix simple
                    {
                        RadioButton rb = new RadioButton();
                        rb.ID = answer.PollAnswerId.ToString();
                            //"RadioButtonID" + ( indexID + indexTableau ).ToString(); 
                            //// on retente le coup ?  HE NON 
                        //C'est la grosse merde ca marche pas non plus a croire que cettte 
                        //putain d'astuce de merde ne marche qu'avec les TextBox c'est quand 
                        //la grosse merde

                        // L'utilisateur a deja vote ?
                        bool voteInDataBase = false;
                        Guid pollAnswerId = GetUserAnswer( ref voteInDataBase );
                        // L'interviewe ne peut pas voter a nouveau
                        if ( pollAnswerId != Guid.Empty )
                        {
                            if ( pollAnswerId == answer.PollAnswerId )
                                rb.Checked = true;
                        }

                        rb.Enabled = ( voteInDataBase == false );
                        //StyleWeb.ApplyStyleWeb( "Reponse", TypeStyleWeb.RadioButtonList, rb );

                        MutuallyExclusiveCheckBoxExtender mece = new MutuallyExclusiveCheckBoxExtender();
                        mece.TargetControlID = rb.ID;
                        
                        // BUG12102009
                        //mece.Key = "RadioButtonExclusive" + indexLigne.ToString();
                        int indexMECBE = indexLigne + indexID; // ajout de indexID "index du tableau" pour differencier les MECBE
                        mece.Key = "RadioButtonExclusive" + indexMECBE.ToString();

                        MutuallyExclusiveCheckBoxExtender meceColonne = new MutuallyExclusiveCheckBoxExtender();
                        if ( tableauClassement )
                        {
                            meceColonne.TargetControlID = rb.ID;
                            int indexMECBEColonne = indexColonne + indexID; // ajout de indexID "index du tableau" pour differencier les MECBE dans differents tableaux
                            meceColonne.Key = "RadioButtonExclusiveColonne" + indexMECBEColonne.ToString();
                        }

                        Reporter.Trace( Report.TRACE0, "  MutuallyExclusiveCheckBoxExtender.ID {0}", mece.TargetControlID );
                        Reporter.Trace( Report.TRACE0, "  MutuallyExclusiveCheckBoxExtender.Key {0}", mece.Key );

                        rb.Text = reponse;

                        if ( SessionState.Reponses[ 0 ].AlignLeft )
                        {
                            rb.TextAlign = TextAlign.Left;
                        }

                        cellTableau.Controls.Add( rb );
                        cellTableau.Controls.Add( mece );
                        if ( tableauClassement )
                        {
                            cellTableau.Controls.Add( meceColonne );
                        }
                        rowTableau.Cells.Add( cellTableau );
                    }

                    indexTableau += 1; // on change de reponse
                    indexColonne += 1; // on change de colonne

                }// fin du  foreach ( PollAnswer answer in SessionState.Reponses )

                indexTableau += 1; // on change de question
                indexLigne += 1; // on change de ligne
                indexColonne = 0;

                tableTableau.Rows.Add( rowTableau );

                //rowReponse.HorizontalAlign = AlignementEnum( SessionState.Question.AlignementReponse );
                //tableReponse.Rows.Add( rowReponse );
                //cellReponse.Controls.Add( tableReponse );
                //AddNewRowQuestion( ref tableQuestion, ref rowQuestion, ref cellReponse );

            }// fin du foreach ( PollQuestion question in questions )

        }// fin du if ( SessionState.Reponses.Count > 0 )

        TableRow rowPager = new TableRow();
        TableCell cellPager = new TableCell();

        int currentIndex = SessionState.CurrentQuestionIndex + 1 + indexID; // mode page, indexID est l'index de la Question dans la Page

        // Pager
        // COR21092009
        // COR09112009
        Label pager = new Label();
        if ( SessionState.Questionnaire.Compteur )
        {
            ApplyStyleGraphique( "CompteurQuestions", TypeStyleWeb.Label, pager );
            pager.Text = "Questions " + currentIndex + " à " + ( currentIndex - 1 + questions.Count ).ToString() + "/" + SessionState.Questions.Count;
        }

        // Barre de progression
        ProgressBarre.CurrentState = currentIndex;
        ProgressBarre.LabelProgressText = ProgressBarre.CurrentState.ToString() + "/" + ProgressBarre.States;
        if ( ProgressBarre.CurrentState == ProgressBarre.States )
        {
            ProgressBarre.LabelProgressText = "Dernière question";
        }

        // COR21092009
        // COR09112009
        if ( SessionState.Questionnaire.Compteur )
        {
            cellPager = new TableCell();
            cellPager.Controls.Add( pager );
            cellPager.HorizontalAlign = HorizontalAlign.Center;
            rowPager.Controls.Add( cellPager );
            tablePager.Controls.Add( rowPager );
        }

        // Encore une grosse merde infame !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // on ne peut pas creer ce bouton dynamiquement comme lorsque
        // Questionnaire est un control dans App_Code/QuestionnaireControl.cs
        // Sinon il ne fonctionne pas ou du moins semble fonctionner une fois
        // sur deux !!!!!!!
        //RolloverButton rolloverButtonSubmit = new RolloverButton();
        Label lblmessage = new Label();
        lblmessage.CssClass = "QuestionnaireLabelMessageUtilisateurStyle";

        // Le mode Trace du Questionnaire
        if ( CheckBoxModeTest.Checked )
        {
            Label lblMessageTest = new Label();
            lblMessageTest.Text += "<br/>rang : <b><font color=\"Red\">" + SessionState.Question.Rank + "</font></b>&nbsp;";
            lblMessageTest.Text += " idx : <b><font color=\"Blue\">" + SessionState.CurrentQuestionIndex + "</font></b>&nbsp;";
            if ( SessionState.Question.Instruction != "" )
                lblMessageTest.Text += " inst : <font color=\"Green\">" + SessionState.Question.Instruction + "</font>";
            if ( SessionState.Question.Obligatoire )
                lblMessageTest.Text += " <font color=\"Blue\">obligatoire</font>";
            if ( SessionState.Question.Fin )
                lblMessageTest.Text += " <font color=\"Red\">fin</font>";

            lblMessageTest.Text += "<br/><br/>";
            cellPager = new TableCell();
            cellPager.Controls.Add( lblMessageTest );
            cellPager.ColumnSpan = 2;
            cellPager.HorizontalAlign = HorizontalAlign.Center;
            rowPager = new TableRow();
            rowPager.Controls.Add( cellPager );
            tablePager.Controls.Add( rowPager );
        }

        if ( SessionState.Reponses.Count > 0 )
        {
            if ( HasUserAlreadyVotedInThePast() )
            {
                lblmessage.Text = "<br/>Vous avez déjà répondu à cette question.<br/><br/>";
                //rolloverButtonSubmit.Text = "Suivante";
                //rolloverButtonSubmit.Click += new EventHandler( QuestionSuivante_Click );
                //ButtonSubmit.Click += new EventHandler( QuestionSuivante_Click );
            }
            else
            {
                //rolloverButtonSubmit.Text = Global.SettingsXml.LabelBoutonQuestion;
                //rolloverButtonSubmit.Click += new EventHandler( SubmitButton_Click );
                //ButtonSubmit.Click += new EventHandler( SubmitButton_Click );
            }
        }
        else // Il n'y a pas de réponse pour cette Question, juste le bouton Suivante
        {
            lblmessage.Text = "<br/>Pas de réponse à donner pour cette question.<br /><br/>";
            //rolloverButtonSubmit.Text = "Suivante";
            //rolloverButtonSubmit.Click += new EventHandler( QuestionSuivante_Click );
            //ButtonSubmit.Click += new EventHandler( QuestionSuivante_Click );
        }

        if ( lblmessage.Text != "" )
        {
            //PanelQuestionnaire.Controls.Add( lblmessage );
            cellPager = new TableCell();
            cellPager.Controls.Add( lblmessage );
            cellPager.ColumnSpan = 2;
            cellPager.HorizontalAlign = HorizontalAlign.Center;
            rowPager = new TableRow();
            rowPager.Controls.Add( cellPager );
            tablePager.Controls.Add( rowPager );
        }

        cellPager = new TableCell();

        // Sinon le bouton est ajoute dans CreatePageControls()
        // Faux on ne peut pas creer dynamiquement ce control
        // sinon il fonctionne un fois sur deux !!!!
        //if ( SessionState.QuestionnaireEnModePage == false )
        //{
        //    cellPager.Controls.Add( rolloverButtonSubmit );
        //}
        cellPager.ColumnSpan = 2;
        cellPager.HorizontalAlign = HorizontalAlign.Center;
        rowPager = new TableRow();
        rowPager.Controls.Add( cellPager );
        tablePager.Controls.Add( rowPager );

        //PanelQuestionnaire.Controls.Add( tablePager );

        //cellReponse.Controls.Add( tablePager );
        //AddNewRowQuestion( ref tableQuestion, ref rowQuestion, ref cellReponse );
    }

    // Faire avancer le "pointeur" SessionState.CurrentQuestionIndex sur la bonne Question
    // en fonction de la reponse de l'interviewe
    // erreur :
    // 0 : pas d'erreur
    // 1 : erreur sur le Rang de la Reponse
    // 2 : erreur sur le Rang de la Question
    // 3 : erreur sur la fin du Questionnaire
    // rang : le rang de l'erreur
    // indexID index de la Question dans la page, pour pouvoir recuperer les votes
    private bool ComputeQuestionSuivante( int indexID, ref int erreur, ref int erreurRang )
    {
        erreur = 0;
        int oldCurrentQuestionIndex = SessionState.CurrentQuestionIndex;

        // Conditions de sortie du Questionnaire 
        if ( SessionState.Question.Rank == DerniereQuestion || SessionState.Question.Fin )
        {
            if ( SessionState.Questionnaire.Valider )
            {
                Page.Response.Redirect( "~/Poll/QuestionnaireEnCours.aspx?fin=1", true );
            }

            Page.Response.Redirect( "~/Poll/Termine.aspx", true );
        }

        // --------------------------------------
        // Si la Question possede une instruction 
        // --------------------------------------
        if ( SessionState.Question.Instruction != "" )
        {
            int question = 0;
            bool bonneReponse = false;
            ArrayList reponses = new ArrayList();

            ArrayList reponseGUID = GetUserVotes( indexID ); 
            foreach ( PollAnswer ans in SessionState.Reponses )
            {
                if ( reponseGUID.Contains( ans.PollAnswerId.ToString() ) )
                {
                    reponses.Add( ans.Rank );
                }
                else
                {
                    reponses.Add( -ans.Rank );
                }
            }

            ArrayList reponsesTrouvees = new ArrayList();
            ArrayList questionsTrouvees = new ArrayList();
            bonneReponse = Instruction.Compute( SessionState.Question.Instruction, reponses, ref question, ref reponsesTrouvees, ref questionsTrouvees );

            // Verifier les Reponses trouvees dans l'instruction
            foreach ( int r in reponsesTrouvees )
            {
                bool trouveR = false;
                foreach ( PollAnswer ans in SessionState.Reponses )
                {
                    if ( ans.Rank == r )
                    {
                        trouveR = true;
                        break;
                    }
                }
                if ( trouveR == false )
                {
                    erreurRang = r;
                    erreur = 1;
                    return false;
                }
            }

            // Verifier les Questions trouvees dans l'instruction
            foreach ( int q in questionsTrouvees )
            {
                bool trouveQ = false;
                foreach ( PollQuestion rep in SessionState.Questions )
                {
                    if ( rep.Rank == q )
                    {
                        trouveQ = true;
                        break;
                    }
                }
                if ( trouveQ == false )
                {
                    erreurRang = q;
                    erreur = 2;
                    return false;
                }
            }

            // Trouver l'index de la Question de rang question
            int index = 0;
            foreach ( PollQuestion q in SessionState.Questions )
            {
                if ( q.Rank == question )
                {
                    break;
                }
                index++;
            }

            if ( bonneReponse && erreur == 0 )
            {
                SessionState.CurrentQuestionIndex = index;
            }
            else if ( erreur == 0 ) // Avancer tout simplement sur la prochaine Question
            {
                SessionState.CurrentQuestionIndex += 1;
            }
        }
        else // Avancer tout simplement sur la prochaine Question
        {
            SessionState.CurrentQuestionIndex += 1;
        }

        if ( SessionState.CurrentQuestionIndex >= SessionState.Questions.Count )
        {
            SessionState.CurrentQuestionIndex = oldCurrentQuestionIndex;
            erreur = 3;
            return false;
        }

        return true;
    }

    // Retourne vrai si on peut ComputeQuestionSuivante
    // typeMessage : 0 un message utilisateur
    //               1 un message d'erreur
    protected bool ValiderReponse( int indexID, ref string message, ref int typeMessage )
    {
        typeMessage = 0;
        string erreurMessage = "";

        // Ajout de la Question si elle n'a pas ete deja ajoutee, 
        // si on ne vient pas de QuestionnaireEnCours.aspx sinon elle a deja ete ajoutee
        if ( SessionState.QuestionsEnCours.Contains( SessionState.Question ) == false )
        {
            SessionState.QuestionsEnCours.Add( SessionState.Question );
        }

        if ( HasUserAlreadyVotedInThePast() == false )
        {
            if ( HasUserVotedToCurrentQuestion( indexID, ref erreurMessage ) )
            {
                // Question Obligatoire ou pas l'interviewe a vote

                // L'interviewe a t-il repondu aux Reponses textuelles Obligatoires
                if ( HasUserVotedToObligatoireTextuellesReponses( indexID, ref erreurMessage ) )
                {
                    // Creation des Votes de l'utilisateur
                    ArrayList alv = GetUserVotes( indexID );
                    foreach ( string guid in alv )
                    {
                        PollVote pv = new PollVote();
                        pv.PollQuestionID = SessionState.Question.PollQuestionId;
                        pv.PollAnswerId = new Guid( guid );
                        pv.UserGUID = SessionState.Personne.PersonneGUID;
                        pv.QuestionnaireID = SessionState.Question.QuestionnaireID;
                        pv.CreationDate = DateTime.Now;

                        PollAnswer answer = SessionState.Reponses.FindByPollAnswerID( pv.PollAnswerId );
                        if ( TypeReponse.EstTextBox( answer.TypeReponse ) )
                        {
                            // astuce auparavant on a pu mettre answer.PollAnswerId.ToString() dans l'ID de la texbox
                            TextBox textBox = ( TextBox )PanelQuestionnaire.FindControl( answer.PollAnswerId.ToString() );
                            pv.Vote = textBox.Text;
                        }
                        if ( answer.TypeReponse == TypeReponse.Date )
                        {
                            TextBoxDate textBoxDate = ( TextBoxDate )PanelQuestionnaire.FindControl( "TextBoxDateID" + indexID.ToString() + answer.Rank.ToString() );
                            TextBox textBox = ( TextBox )textBoxDate.FindControl( "TextBoxDateText" );
                            pv.Vote = textBox.Text;
                        }
                        if ( answer.TypeReponse == TypeReponse.SemiOuverte )
                        {
                            PopupTextBox popupTextBox = ( PopupTextBox )PanelQuestionnaire.FindControl( "PopupTextBoxID" + indexID.ToString() + answer.Rank.ToString() );
                            TextBox textBox = ( TextBox )popupTextBox.FindControl( "TextBoxText" );
                            pv.Vote = textBox.Text;
                        }

                        // Creer le vote "A la vollee"
                        if ( SessionState.Questionnaire.Valider == false )
                        {
                            if ( HttpContext.Current.User.Identity.IsAuthenticated == false )
                            {
                                Limitation limitation = new Limitation( SessionState.Questionnaire.MembreGUID );
                                if ( limitation.LimitesReponses )
                                {
                                    Response.Redirect( Tools.PageErreurPath + "Désolé mais le nombre de réponses pour ce questionnaire est atteinte.", true );
                                }

                                int status = PollVote.Create( pv );
                                if ( status != 1 )
                                {
                                    typeMessage = 1;
                                    message += "<br/>Erreur à la création du Vote.<br/><br/>";
                                }
                            }
                            SessionState.Votes.Add( pv );
                        }
                        else
                        {
                            SessionState.VotesEnCours.Add( pv );
                        }
                    }

                    // L'utisateur a bien vote
                    // on peut computer la Question suivante
                    return true;
                }
                else
                {
                    message += "<br/>" + erreurMessage + "<br/>";
                }
            }
            else if ( SessionState.Question.Obligatoire )
            {
                if ( SessionState.QuestionnaireEnModePage )
                {
                    message += "<br/>Merci de répondre à la Question : " + SessionState.Question.Question + " elle est obligatoire.<br/>";
                }
                else
                {
                    message += "<br/>Merci de répondre à la Question elle est obligatoire.<br/>";
                }
                if ( erreurMessage != "" )
                {
                    message += "<br/>" + erreurMessage + "<br/>";
                }
            }
            else if ( HasUserVotedToObligatoireTextuellesReponses( indexID, ref erreurMessage ) == false )
            {
                if ( erreurMessage != "" )
                {
                    message += "<br/>" + erreurMessage + "<br/>";
                }
            }
            else
            {
                // L'interviewe n'etait pas oblige de voter mais il a commis une erreur
                // il a entre n'importe quoi dans une reponse numerique
                if ( erreurMessage != "" )
                {
                    message += "<br/>" + erreurMessage + "<br/>";
                }
                else
                {
                    // L'interviewe n'a pas vote mais la question n'etait pas Obligatoire, 
                    // on peut computer la Question suivante
                    return true;
                }
            }
        }
        else
        {
            // L'interviewe a deja vote dans le passe, 
            // on computer la Question suivante
            //message += "<br/>Vous avez déjà répondu à cette question.<br/><br/>"; deja fait ailleurs !?
            return true;
        }

        return false;
    }

    // Une seule question par page
    protected void SubmitButton_Click( object sender, EventArgs e )
    {
        if ( QuestionnaireErreur )
        {
            return;
        }

        string message = "";
        int typeMessage = 0;

        int erreur = 0;
        int erreurRang = 0;

        if ( ValiderReponse( 0, ref message, ref typeMessage ) )
        {
            if ( ComputeQuestionSuivante( 0, ref erreur, ref erreurRang ) )
            {
                // Charger la prochaine Question et ses Reponses
                SessionState.Question = SessionState.Questions[ SessionState.CurrentQuestionIndex ];
                SessionState.Reponses = PollAnswerCollection.GetByPollQuestionID( SessionState.Question.PollQuestionId );
                ResynchroControlPollQuestionId = SessionState.Question.PollQuestionId.ToString();

                // Dessiner la nouvelle Question
                PanelQuestionnaire.Controls.Clear(); 
                CreateQuestionControls();
            }
        }

        // Erreur de programmation
        if ( erreur != 0 )
        {
            typeMessage = 1;
            switch ( erreur )
            {
                case 1:
                    message += "<br/>Erreur de programmation : " + SessionState.Question.Instruction + " dans la Réponse R" + erreurRang.ToString();
                    break;
                case 2:
                    message += "<br/>Erreur de programmation : " + SessionState.Question.Instruction + " dans la Question Q" + erreurRang.ToString();
                    break;
                case 3:
                    message += "<br/>Erreur de programmation : " + SessionState.Question.Instruction + " sur la fin du Questionnaire";
                    break;
            }
        }

        // Message a l'utilisateur
        if ( message != "" )
        {
            Label lblmessage = new Label();
            lblmessage.Text = message;
            if ( typeMessage == 0 )
                lblmessage.CssClass = "QuestionnaireLabelMessageUtilisateurStyle";
            else
                lblmessage.CssClass = "LabelValidationMessageErrorStyle";

            Table table = new Table();
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            table.BorderWidth = new Unit( 0 );
            table.CellPadding = 12;
            table.CellSpacing = 1;
            table.Width = new Unit( "100%" );
            cell.Controls.Add( lblmessage );
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Controls.Add( lblmessage );
            row.Controls.Add( cell );
            table.Controls.Add( row );
            PanelQuestionnaire.Controls.Add( table );
        }
    }

    protected void SubmitButtonPageQuestions_Click( object sender, EventArgs e )
    {
        Reporter.Trace( "SubmitButtonPageQuestions_Click()" );
        Trace.Warn( "SubmitButtonPageQuestions_Click()" );

        if ( QuestionnaireErreur )
        {
            return;
        }

        string message = "";
        int typeMessage = 0;

        bool questionValide = true;

        int indexID = 0; // index de la Question dans la Page graphique
        foreach ( PollQuestion question in SessionState.PageQuestions )
        {
            SessionState.Question = question;
            SessionState.Reponses = PollAnswerCollection.GetByPollQuestionID( question.PollQuestionId );
            if ( SessionState.Reponses.Count > 0 )
            {
                if ( ValiderReponse( indexID, ref message, ref typeMessage ) == false )
                {
                    questionValide = false;
                    break;
                }
            }
            indexID += 1;
        }

        int erreur = 0;
        int erreurRang = 0;

        if ( questionValide )
        {
            Reporter.Trace( " questionValide" );
            Reporter.Trace( " SessionState.PageQuestionsIndex : {0}", SessionState.PageQuestionsIndex );

            // Resynchroniser CurrentQuestionIndex sur la derniere question de la page en cours
            // avant ComputeQuestionSuivante 
            SessionState.CurrentQuestionIndex = SessionState.PageQuestionsIndex - 1;

            // On ne compute que la derniere Question de la Page
            // ainsi l'intervieweur peut brancher sur la dernière question de la Page
            // ou mettre la question de Branchement seule sur une Page
            if ( ComputeQuestionSuivante( indexID - 1, ref erreur, ref erreurRang ) )
            {
                Reporter.Trace( " ComputeQuestionSuivante : SessionState.CurrentQuestionIndex {0}", SessionState.CurrentQuestionIndex );

                // Sauvegarder le resultat CurrentQuestionIndex de ComputeQuestionSuivante()
                SessionState.PageQuestionsIndex = SessionState.CurrentQuestionIndex;

                ChargerPageQuestions();

                // Resynchroniser avec l'interface graphique
                SessionState.Question = SessionState.Questions[ SessionState.CurrentQuestionIndex ];
                SessionState.Reponses = PollAnswerCollection.GetByPollQuestionID( SessionState.Question.PollQuestionId );
                ResynchroControlPollQuestionId = SessionState.Question.PollQuestionId.ToString();

                Reporter.Trace( "  SessionState.CurrentQuestionIndex : {0}", SessionState.CurrentQuestionIndex );
                Reporter.Trace( "  ResynchroControlPollQuestionId : {0}", ResynchroControlPollQuestionId );
                Reporter.Trace( "  SessionState.Question.Question : {0}", SessionState.Question.Question );
                Reporter.Trace( "  SessionState.Question.PollQuestionId : {0}", SessionState.Question.PollQuestionId );

                // Dessiner la nouvelle Page
                Reporter.Trace( "SubmitButtonPageQuestions_Click:PanelQuestionnaire.Controls.Clear()" ); 
                PanelQuestionnaire.Controls.Clear(); 
                CreatePageControls();
            }

            // Erreur de programmation
            if ( erreur != 0 )
            {
                typeMessage = 1;
                switch ( erreur )
                {
                    case 1:
                        message += "<br/>Erreur de programmation : " + SessionState.Question.Instruction + " dans la Réponse R" + erreurRang.ToString();
                        break;
                    case 2:
                        message += "<br/>Erreur de programmation : " + SessionState.Question.Instruction + " dans la Question Q" + erreurRang.ToString();
                        break;
                    case 3:
                        message += "<br/>Erreur de programmation : " + SessionState.Question.Instruction + " sur la fin du Questionnaire";
                        break;
                }
            }
        }

        // Message a l'utilisateur
        if ( message != "" )
        {
            Label lblmessage = new Label();
            lblmessage.Text = message;
            if ( typeMessage == 0 )
                lblmessage.CssClass = "QuestionnaireLabelMessageUtilisateurStyle";
            else
                lblmessage.CssClass = "LabelValidationMessageErrorStyle";

            Table table = new Table();
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            table.BorderWidth = new Unit( 0 );
            table.CellPadding = 12;
            table.CellSpacing = 1;
            table.Width = new Unit( "100%" );
            cell.Controls.Add( lblmessage );
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Controls.Add( lblmessage );
            row.Controls.Add( cell );
            table.Controls.Add( row );
            PanelQuestionnaire.Controls.Add( table );
        }
    }

    protected void QuestionSuivante_Click( object sender, EventArgs e )
    {
        if ( SessionState.QuestionsEnCours.Contains( SessionState.Question ) == false )
        {
            SessionState.QuestionsEnCours.Add( SessionState.Question );
        }

        int erreur = 0;
        int erreurRang = 0;
        if ( ComputeQuestionSuivante( 0, ref erreur, ref erreurRang ) )
        {
            // Charger la prochaine Question et ses Reponses
            SessionState.Question = SessionState.Questions[ SessionState.CurrentQuestionIndex ];
            SessionState.Reponses = PollAnswerCollection.GetByPollQuestionID( SessionState.Question.PollQuestionId );
            ResynchroControlPollQuestionId = SessionState.Question.PollQuestionId.ToString();

            // Dessiner la nouvelle Question
            PanelQuestionnaire.Controls.Clear();
            CreateQuestionControls();
        }
        else
        {
            Label lblmessage = new Label();
            lblmessage.CssClass = "LabelValidationMessageErrorStyle";
            switch ( erreur )
            {
                case 1:
                    lblmessage.Text = "<br/>Erreur de programmation : " + SessionState.Question.Instruction + " dans la Réponse R" + erreurRang.ToString();
                    break;
                case 2:
                    lblmessage.Text = "<br/>Erreur de programmation : " + SessionState.Question.Instruction + " dans la Question Q" + erreurRang.ToString();
                    break;
                case 3:
                    lblmessage.Text = "<br/>Erreur de programmation : " + SessionState.Question.Instruction + " sur la fin du Questionnaire";
                    break;
            }
            PanelQuestionnaire.Controls.Add( lblmessage );
        }
    }

    protected void ButtonReset_Click( object sender, EventArgs e )
    {
        if ( SessionState.Questionnaire != null )
        {
            Response.Redirect( "~/Poll/Questionnaire.aspx?QuestionnaireID=" + SessionState.Questionnaire.QuestionnaireID.ToString(), true );
        }
    }

    #region EditionStyleWeb

    protected void ImageButtonEditStyleWebPageQuestion_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Page de Question";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=PageQuestion&Type=Table&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebTableTitre_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Table Titre Page";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=TableTitrePage&Type=Table&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebTableTitreQuestionQuestion_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Table Titre de la Question";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=TableTitreQuestion&Type=Table&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebReponseChoixSimple_Click( object sender, ImageClickEventArgs e )
    {
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=Reponse&Type=RadioButtonList&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebReponseChoixMutiple_Click( object sender, ImageClickEventArgs e )
    {
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=Reponse&Type=CheckBoxList&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebTableReponseTextuelle_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Table des Réponses Textuelles";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=TableReponseTextuelle&Type=Table&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebReponseTextuelleLabel_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Label des Réponses Textuelles";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=ReponseTextuelleLabel&Type=Label&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebReponseTextuelleTextBox_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "TextBox style des Réponses Textuelles";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=ReponseTextuelleTextBox&Type=TextBox&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebCelluleQuestion_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Style de la Cellule autour de la Question";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=CelluleQuestion&Type=Label&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebCadreQuestion_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Style du Cadre de la Question";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=CadreQuestion&Type=Table&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebTableCompteurQuestions_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Table du Compteur de questions";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=TableCompteurQuestions&Type=Table&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebCompteurQuestions_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Label du Compteur de questions";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=CompteurQuestions&Type=Label&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebTableMessage_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Table du Message destiné à l'utilisateur";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=TableMessage&Type=Table&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebTitrePageQuestionnaire_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Titre des pages du Questionnaire";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=TitrePage&Type=Label&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebPanelPageQuestionnaire_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Cadre du Questionnaire";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=CadreQuestionnaire&Type=Label&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebTableTitreTableau_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Table du Titre du Tableaux";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=TableTitreTableau&Type=Table&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebTitreTableau_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Titres des Tableaux";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=TitreTableau&Type=Label&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebCadreTableau_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Cadre des Tableaux";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=CadreTableau&Type=Table&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebQuestionTableau_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Questions dans les Tableaux";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=QuestionTableau&Type=Label&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebReponseTableau_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Réponses dans les Tableaux";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=ReponseTableau&Type=Label&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebCelluleReponseTableau_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Cellules de Réponses dans les Tableaux";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=CelluleReponseTableau&Type=Label&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    protected void ImageButtonEditStyleWebCelluleQuestionTableau_Click( object sender, ImageClickEventArgs e )
    {
        HttpContext.Current.Session[ "StyleWeb_Edit_Texte" ] = "Cellules de Question dans les Tableaux";
        Response.Redirect( "~/StyleWeb/Edit.aspx?Style=CelluleQuestionTableau&Type=Label&ReturnUrl=" + Request.RawUrl.ToString() );//~/Wizard/Question.aspx" );
    }

    #endregion
}
