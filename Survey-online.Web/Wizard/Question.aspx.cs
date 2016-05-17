//
// A propos du CollapsiblePanelExtender c'est moche ca flicte
// une astuce consiste a mettre Height="0" dans le Panel mais bon ...
// c'est pas joli quand meme
//
// Seule, la permiere Reponse de la Collection, porte les attributs 
// AlignLeft et Horizontal
//
// Difficulte lorsque un TextBox est cablee avec un event TextChanged :
// si on change le texte et qu'on clique sur Ok les deux events pettent
// et on se retouve avec 2 elements dans la collection
// Solution : elle passe par un : .Click -= new EventHandler()
// dans le TextChanged pour annuler l'event click sur le bouton
//
// La gestion du Mode Mise a jour et du Mode Creation se fait par :
// QuestionReponse.Question.PollQuestionId != Guid.Empty => Mise a jour d'une Question existante
// sinon c'est une creation
//
// 09/11/08 Correction d'un gros bug qui creait un decallage on avait l'impression que la deuxieme
// question cree a la suite prenait la place de la première
// effectivement cela venait d'une confusion entre le mode creation et le mode mise a jour
// lorsqu'on click sur ButtonAjouterQuestion_Click il se passe un tas d'evenement en fonction
// de ce que l'utilisateur a modifier et des Update PollQuestion.Update() etait fait car 
// QuestionReponse.Question.PollQuestionId != Guid.Empty apres la creation de la premiere question
//
// 12/11/09
// Tenter une amelioration de ce formulaire c'est une catastrophe
// on ne fait plus d'Update a la vollee dans DroDown ca perturbe
// si l'utilisateur modifie il doit cliquer sur Modifier
// les deux trucs qui pilotent ce formulaire sont :
// DropDownListTypeQuestionReponse_SelectedIndexChanged()et
// choisirVoletReponseTextuelle()
// 
//
using System;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using Sql.Web.Data;
using UserControl.Web.Controls;
using StyleWebData;
using TraceReporter;

// Beyong Compare, il faut l'utiliser pour comparer avec Poll/Questionnaire.aspx.cs
// pour developper le meme rendu de la Question ...

public partial class Wizard_Question : System.Web.UI.Page
{
    // Finalement je sais pas faire autrement
    private static bool DEBUG_STYLEWEB0 = false; // tableau

    private static ArrayList ChoixReponsesPredefinies()
    {
        ArrayList al = new ArrayList();
        al.Add( "Choisir des réponses prédéfinies" );
        al.Add( "Oui" );
        al.Add( "Non" );
        al.Add( "Oui; Non" );
        al.Add( "Peut-être" );
        al.Add( "Je ne sais pas" );
        al.Add( "Ne sais pas" );
        al.Add( "1; 2; 3" );
        al.Add( "1; 2; 3; 4; 5" );
        al.Add( "1; 2; 3; 4; 5; 6; 7; 8; 9; 10" );
        al.Add( "D'accord; Pas d'accord" );
        al.Add( "Toujours; Parfois; Jamais" );
        al.Add( "Oui; Plutôt oui; Ne se prononce pas; Plutôt non; Non" );
        al.Add( "Absolument; Pas tout à fait; Pas toujours; Pas du tout" );
        al.Add( "Tout à fait; Plutôt oui; Plutôt non; Pas du tout" );
        al.Add( "Tout à fait d'accord; Plutôt d'accord" );
        al.Add( "Plutôt pas d'accord; Pas du tout d'accord" );
        return al;
    }

    [Serializable()]
    private class ClassQuestionReponse
    {
        public ClassQuestionReponse()
        {
        }

        private PollQuestion _Question = new PollQuestion();
        public PollQuestion Question
        {
            get { return _Question; }
            set { _Question = value; }
        }

        private PollAnswerCollection _ReponseCollection = new PollAnswerCollection();
        public PollAnswerCollection ReponseCollection
        {
            get { return _ReponseCollection; }
            set { _ReponseCollection = value; }
        }
    }

    // La QuestionReponse Courante que l'on peut mofidier graphiquement
    private ClassQuestionReponse QuestionReponse
    {
        get
        {
            if ( ViewState[ "_QuestionReponse" ] == null )
            {
                ViewState[ "_QuestionReponse" ] = new ClassQuestionReponse();
            }
            Type type = ViewState[ "_QuestionReponse" ].GetType();
            Object ocqr = new ClassQuestionReponse();
            ocqr = ViewState[ "_QuestionReponse" ];
            ClassQuestionReponse cqr = new ClassQuestionReponse();
            cqr = ( ClassQuestionReponse )ocqr;

            return ( ClassQuestionReponse )cqr;
        }
        set { ViewState[ "_QuestionReponse" ] = value; }
    }

    // Le Questionnaire
    //BUG10092009 
    //private static List<ClassQuestionReponse> QuestionnaireQuestionReponse = new List<ClassQuestionReponse>();
    private List<ClassQuestionReponse> QuestionnaireQuestionReponse
    {
        get
        {
            if ( ViewState[ "QuestionnaireQuestionReponse" ] == null )
            {
                ViewState[ "QuestionnaireQuestionReponse" ] = new List<ClassQuestionReponse>();
            }
            return ( List<ClassQuestionReponse> )ViewState[ "QuestionnaireQuestionReponse" ];
        }
        set { ViewState[ "QuestionnaireQuestionReponse" ] = value; }
    }

    // Se promener sur le Questionnaire
    private int IndexQuestionReponseCourante
    {
        get { return ( int )ViewState[ "IndexQuestionReponseCourante" ]; }
        set { ViewState[ "IndexQuestionReponseCourante" ] = value; }
    }

    // En mode demonstration on va preremplir la question et les reponses
    // pour donner un maximun d'infos a l'utilisateur
    private bool QuestionEnModeDemonstration
    {
        get 
        {
            if ( HttpContext.Current.Session[ "QuestionEnModeDemonstration" ] == null )
                HttpContext.Current.Session[ "QuestionEnModeDemonstration" ] = true;
            return ( bool )HttpContext.Current.Session[ "QuestionEnModeDemonstration" ]; 
        }
        set { HttpContext.Current.Session[ "QuestionEnModeDemonstration" ] = value; }
    }

    // Donner le Titre de la page en Cours
    private string PageTitre
    {
        get
        {
            if ( string.IsNullOrEmpty( QuestionReponse.Question.SautPage ) && ViewState[ "PageTitre" ] == null )
            {
                ViewState[ "PageTitre" ] = "Titre de la page";
            }
            else
            {
                if ( string.IsNullOrEmpty( QuestionReponse.Question.SautPage ) == false )
                {
                    ViewState[ "PageTitre" ] = QuestionReponse.Question.SautPage;
                }
            }
            return ( string )ViewState[ "PageTitre" ];
        }
    }

    protected void Page_Load( object sender, EventArgs e )
    {
        Reporter.Trace( TraceReporter.Report.DEBUG1, string.Format( "Page_Load {0}", Page.IsPostBack.ToString() ) );

        if ( IsPostBack == false )
        {
            // Choisir le premier Questionnaire a la place de l'utilisateur
            if ( SessionState.Questionnaire == null && SessionState.Questionnaires.Count > 0 )
            {
                SessionState.Questionnaire = SessionState.Questionnaires[ 0 ];
            }
            // Affcher l'aide contextuelle
            CollapsiblePanelExtender1.Collapsed = !SessionState.MemberSettings.AfficherAide;
            CollapsiblePanelExtender2.Collapsed = !SessionState.MemberSettings.AfficherAide;
            CollapsiblePanelExtender3.Collapsed = !SessionState.MemberSettings.AfficherAide;
            CollapsiblePanelExtender4.Collapsed = !SessionState.MemberSettings.AfficherAide;
        }

        if ( IsPostBack == false )
        {
            LabelValidationMessage.Text = "";
            LabelReponseOuverte.Text = "Réponse ouverte :";

            DropDownListTypeQuestionReponse.DataSource = TypeQuestionReponse.List();
            DropDownListTypeQuestionReponse.DataBind();
            DropDownListTypeQuestionReponse.Items.Insert( 0, new ListItem( "Choisir un type de Question", "-1" ) );

            DropDownListChoixReponse.DataSource = ChoixReponsesPredefinies();
            DropDownListChoixReponse.DataBind();

            DropDownListAlignementQuestion.DataSource = PollQuestion.TypeAlignement();
            DropDownListAlignementQuestion.DataBind();

            DropDownListAlignementReponse.DataSource = PollQuestion.TypeAlignement();
            DropDownListAlignementReponse.DataBind();

            // Formulaire en mode :
            // Mise a jour d'une Question
            //
            if ( Request[ "PollQuestionId" ] != null )
            {
                QuestionEnModeDemonstration = false;
                TrBoutonEffacerReponses.Visible = true;
                ButtonModifier.Visible = true;

                Guid pollQuestionId = new Guid( Request[ "PollQuestionId" ] );
                RolloverSupprimer.Visible = true;

                LabelTitre.Text = "Modifier la Question";

                QuestionReponse.Question = PollQuestion.GetQuestion( pollQuestionId );
                if ( QuestionReponse.Question == null )
                {
                    Page.Response.Redirect( Tools.PageErreurPath + "Il n'y a pas de Question pour ce Questionnaire." );
                }

                TextBoxQuestion.Text = QuestionReponse.Question.Question;
                TextBoxRangQuestion.Text = QuestionReponse.Question.Rank.ToString();

                if ( QuestionReponse.Question.ChoixMultiple )
                {
                    TrOptionQuestion01.Visible = true;
                    TrOptionQuestion02.Visible = true;
                    TextBoxChoixMultipleMin.Text = QuestionReponse.Question.ChoixMultipleMin == 0 ? "" : QuestionReponse.Question.ChoixMultipleMin.ToString();
                    TextBoxChoixMultipleMax.Text = QuestionReponse.Question.ChoixMultipleMax == 0 ? "" : QuestionReponse.Question.ChoixMultipleMax.ToString();
                }

                // Reconstruire au passage la chaine de reponse de type choix
                PollAnswerCollection reponses = PollAnswerCollection.GetByPollQuestionID( QuestionReponse.Question.PollQuestionId );
                string reponsesChoix = "";
                foreach ( PollAnswer ans in reponses )
                {
                    QuestionReponse.ReponseCollection.Add( ans );
                    if ( ans.TypeReponse == TypeReponse.Choix )
                    {
                        reponsesChoix += ans.Answer + ";";
                    }
                }
                if ( reponsesChoix != "" )
                {
                    reponsesChoix = reponsesChoix.Substring( 0, reponsesChoix.Length - 1 ); // supprimer le dernier ';'
                }
                TextBoxReponse.Text = reponsesChoix;

                if ( QuestionReponse.Question.ChoixMultiple )
                {
                    DropDownListTypeQuestionReponse.SelectedValue = TypeQuestionReponse.ChoixMultiple;
                }
                else
                {
                    DropDownListTypeQuestionReponse.SelectedValue = TypeQuestionReponse.ChoixSimple;
                }
                if ( QuestionReponse.ReponseCollection.Count /*BRY0002010305 > si yen a une il faut faire le choix !*/>= 1 )
                {
                    if ( QuestionReponse.ReponseCollection[ 0 ].Horizontal )
                    {
                        DropDownListDirection.SelectedValue = "Horizontal";
                    }
                    CheckBoxTextAGauche.Checked = QuestionReponse.ReponseCollection[ 0 ].AlignLeft;

                    // AME12112009
                    choisirVoletReponseTextuelle();
                }
                DropDownListAlignementReponse.SelectedValue = QuestionReponse.Question.AlignementReponse;
                TextBoxMessageUtilisateur.Text = QuestionReponse.Question.Message;
                CheckBoxMessageEnHaut.Checked = QuestionReponse.Question.MessageHaut;
                CheckBoxQuestionObligatoire.Checked = QuestionReponse.Question.Obligatoire;
                CheckBoxQuestionFin.Checked = QuestionReponse.Question.Fin;

                // Charger le Questionnaire entier pour pouvoir acceder aux questions suivantes et precedentes
                PanelBoutonSuivantePrecedente.Visible = true;
                QuestionnaireQuestionReponse.Clear();
                PollQuestionCollection questions = PollQuestionCollection.GetByQuestionnaire( QuestionReponse.Question.QuestionnaireID );
                if ( questions.Count > 0 )
                {
                    foreach ( PollQuestion question in questions )
                    {
                        ClassQuestionReponse questionReponse = new ClassQuestionReponse();
                        questionReponse.Question = question;
                        PollAnswerCollection reponses2 = PollAnswerCollection.GetByPollQuestionID( question.PollQuestionId );
                        foreach ( PollAnswer ans in reponses2 )
                        {
                            questionReponse.ReponseCollection.Add( ans );
                        }
                        QuestionnaireQuestionReponse.Add( questionReponse );
                    }
                }
                // fin de Charge le Questionnaire entier

                // Trouver l'index de QuestionReponse (IndexOf de la Collection ne fonctionne pas !?)
                //int idx = QuestionnaireQuestionReponse.IndexOf( QuestionReponse );
                IndexQuestionReponseCourante = 0;
                foreach ( ClassQuestionReponse qr in QuestionnaireQuestionReponse )
                {
                    if ( qr.Question.PollQuestionId == QuestionReponse.Question.PollQuestionId )
                    {
                        break;
                    }
                    IndexQuestionReponseCourante += 1;
                }
            }
            // Formulaire en mode : 
            // Creation d'une Question
            //
            else 
            {
                ButtonAjouter.Visible = true;
                RolloverSupprimer.Visible = false;

                LabelTitre.Text = "Ajouter des Questions";

                // La Question
                TextBoxQuestion.Text = "Etes-vous parti en vacances cette année ?";
                QuestionReponse.Question.Question = TextBoxQuestion.Text.Trim();
                QuestionReponse.Question.AlignementQuestion = "Gauche";
                QuestionReponse.Question.AlignementReponse = "Gauche";

                DropDownListTypeQuestionReponse.SelectedValue = TypeQuestionReponse.ChoixSimple;
                
                PollQuestionCollection questions = PollQuestionCollection.GetByQuestionnaire( SessionState.Questionnaire.QuestionnaireID );
                QuestionReponse.Question.Rank = questions.MaxRank() + 1;
                TextBoxRangQuestion.Text = QuestionReponse.Question.Rank.ToString();

                // Les Reponses
                QuestionReponse.ReponseCollection.Clear();

                TrReponseTextuelle.Visible = false;
                TrBoutonEffacerReponses.Visible = false;
                TextBoxReponse.Text = ChoixReponsesPredefinies()[ 3 ].ToString(); // Oui, Non
                DropDownListChoixReponse.SelectedValue = ChoixReponsesPredefinies()[ 3 ].ToString();
                PollAnswer reponse = new PollAnswer( "Oui" );
                reponse.TypeReponse = TypeReponse.Choix;
                QuestionReponse.ReponseCollection.Add( reponse );
                reponse = new PollAnswer( "Non" );
                reponse.TypeReponse = TypeReponse.Choix;
                QuestionReponse.ReponseCollection.Add( reponse );
                QuestionReponse.Question.ChoixMultiple = false;

                if ( QuestionEnModeDemonstration == true )
                {
                    TextBoxMessageUtilisateur.Text = "Sachant que c'est d'une très grande importance pour notre enquête.";
                }
                QuestionReponse.Question.Message = TextBoxMessageUtilisateur.Text;
                QuestionReponse.Question.MessageHaut = CheckBoxMessageEnHaut.Checked;
            }

            CheckBoxModeDemonstration.Checked = QuestionEnModeDemonstration;

            //
            // Etat des Volets
            //

            // Question
            bool etatVoletQuestion = SessionState.BooleanSate[ "ImageButtonExpandQuestion" ];
            TrOptionQuestion1.Visible = etatVoletQuestion;
            TrOptionQuestion2.Visible = etatVoletQuestion;
            TrOptionQuestion3.Visible = etatVoletQuestion;
            if ( TrOptionQuestion1.Visible )
            {
                ImageButtonExpandQuestion.ImageUrl = "~/Images/collapse.jpg";
            }
            else
            {
                ImageButtonExpandQuestion.ImageUrl = "~/Images/expand.jpg";
            }

            // Reponse
            bool etatVoletReponse = SessionState.BooleanSate[ "ImageButtonExpandReponse" ];
            TrVoletReponse1.Visible = etatVoletReponse;
            TrVoletReponse2.Visible = etatVoletReponse;
            TrVoletReponse3.Visible = etatVoletReponse;
            if ( TrVoletReponse1.Visible )
            {
                ImageButtonExpandReponse.ImageUrl = "~/Images/collapse.jpg";
            }
            else
            {
                ImageButtonExpandReponse.ImageUrl = "~/Images/expand.jpg";
            }

            // Reponse textuelle
            bool etatVoletReponseTextuelle = SessionState.BooleanSate[ "ImageButtonExpandReponseTextuelle" ];
            TrVoletReponse4.Visible = etatVoletReponseTextuelle;
            TrVoletReponse5.Visible = etatVoletReponseTextuelle;
            TrVoletReponse6.Visible = etatVoletReponseTextuelle;
            if ( TrVoletReponse4.Visible )
            {
                ImageButtonExpandReponseTextuelle.ImageUrl = "~/Images/collapse.jpg";
            }
            else
            {
                ImageButtonExpandReponseTextuelle.ImageUrl = "~/Images/expand.jpg";
            }
        }

        BloquerQuestionnaire( SessionState.Questionnaire.Bloque );
        Page.Form.DefaultButton = ButtonQuestionOk.UniqueID; // Pour donner le focus
        // BRY00020100228 CreateQuestionControls();
    }

    // BRY00020100228
    protected override void CreateChildControls()
    {
        CreateQuestionControls();
        base.CreateChildControls();
    }

    private void BloquerQuestionnaire( bool bloque )
    {
        if ( bloque )
        {
            Tools.PageValidation( "Le questionnaire \"" + SessionState.Questionnaire.Description + "\" est clôturé." );
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

    private void AddNewRow( ref Table table, ref TableRow tableRow, ref TableCell tableCell )
    {
        switch ( QuestionReponse.Question.AlignementQuestion )
        {
            case "Gauche":
                tableCell.HorizontalAlign = HorizontalAlign.Left;
                break;
            case "Droite":
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                break;
            case "Centre":
                tableCell.HorizontalAlign = HorizontalAlign.Center;
                break;
        }
        tableRow.Controls.Add( tableCell );
        table.Controls.Add( tableRow );
        tableCell = new TableCell();
        tableRow = new TableRow();
    }

    private void CreateQuestionControls()
    {
        Reporter.Trace( TraceReporter.Report.DEBUG1, "CreateQuestionControls()" );

        PanelQuestion.Controls.Clear();
        StyleWeb.ApplyStyleWeb( "CadreQuestionnaire", TypeStyleWeb.Label, PanelQuestion );

        Table _tablePageQuestion = new Table();
        StyleWeb.ApplyStyleWeb( "PageQuestion", TypeStyleWeb.Table, _tablePageQuestion );

        TableCell _cellQuestion = new TableCell();
        TableRow _rowQuestion = new TableRow();

        Table tableQuestionReponse = new Table();
        tableQuestionReponse.EnableViewState = false; // eviter un crash serveur
        StyleWeb.ApplyStyleWeb( "CadreQuestion", TypeStyleWeb.Table, tableQuestionReponse );

        // Titre des pages
        Label titrePage = new Label();
        titrePage.Text = PageTitre;
        StyleWeb.ApplyStyleWeb( "TitrePage", TypeStyleWeb.Label, titrePage );

        TableRow rowQuestionReponse = new TableRow();
        TableCell cellQuestionReponse = new TableCell();

        Table tableTitre = new Table();
        StyleWeb.ApplyStyleWeb( "TableTitrePage", TypeStyleWeb.Table, tableTitre );

        TableCell cellTitre = new TableCell();
        TableRow rowTitre = new TableRow();

        cellTitre.Controls.Add( titrePage );
        rowTitre.Controls.Add( cellTitre );
        tableTitre.Controls.Add( rowTitre );

        cellQuestionReponse.Controls.Add( tableTitre );
        rowQuestionReponse.Controls.Add( cellQuestionReponse );
        AddNewRow( ref _tablePageQuestion, ref rowQuestionReponse, ref cellQuestionReponse );

        // Le message a l'utilisateur
        if ( QuestionReponse.Question.Message != "" && QuestionReponse.Question.MessageHaut == true )
        {
            // Mettre le message dans une table pour pouvoir appliquer un style Table
            Label lblMessage = new Label();
            lblMessage.Text = QuestionReponse.Question.Message;
            Table _table1 = new Table();
            TableCell _cell1 = new TableCell();
            TableRow _row1 = new TableRow();
            _cell1.Controls.Add( lblMessage );
            _row1.Controls.Add( _cell1 );
            _table1.Controls.Add( _row1 );
            StyleWeb.ApplyStyleWeb( "TableMessage", TypeStyleWeb.Table, _table1 );

            cellQuestionReponse.Controls.Add( _table1 );
            AddNewRow( ref tableQuestionReponse, ref rowQuestionReponse, ref cellQuestionReponse );
        }

        // Question
        // Mettre la Question dans une table pour pouvoir appliquer un style Table
        Label questionLabel = new Label();
        questionLabel.Text = QuestionReponse.Question.Question;
        Table _table2 = new Table();
        TableCell _cell2 = new TableCell();
        TableRow _row2 = new TableRow();
        _cell2.Controls.Add( questionLabel );
        _row2.Controls.Add( _cell2 );
        _table2.Controls.Add( _row2 );
        StyleWeb.ApplyStyleWeb( "TableTitreQuestion", TypeStyleWeb.Table, _table2 );

        cellQuestionReponse.Controls.Add( _table2 );
        AddNewRow( ref tableQuestionReponse, ref rowQuestionReponse, ref cellQuestionReponse );

        if ( QuestionReponse.ReponseCollection.Count > 0 )
        {
            // Recuperer les Reponses de Type Choix
            PollAnswerCollection reponseChoix = new PollAnswerCollection();
            foreach ( PollAnswer reponse in QuestionReponse.ReponseCollection )
            {
                if ( reponse.TypeReponse == TypeReponse.Choix )
                {
                    reponseChoix.Add( reponse );
                }
            }

            // Reponses Choix
            Table tableReponse = new Table();
            TableRow rowReponse = new TableRow();
            TableCell cellReponse = new TableCell();

            // Seule la premiere reponse possede l'attribut Horizontal ou Vertical
            bool directionHorizontal = QuestionReponse.ReponseCollection[ 0 ].Horizontal;
            bool alignLeft = QuestionReponse.ReponseCollection[ 0 ].AlignLeft;
            if ( QuestionReponse.Question.ChoixMultiple )
            {
                CheckBoxListStyle cbl = new CheckBoxListStyle();

                foreach ( PollAnswer pa in reponseChoix )
                {
                    cbl.Items.Add( pa.Answer );
                }
                if ( alignLeft )
                {
                    cbl.TextAlign = TextAlign.Left;
                }
                if ( directionHorizontal )
                {
                    cbl.RepeatDirection = RepeatDirection.Horizontal;
                }

                // On ne sait pas faire, c'est un attribut qui doit etre dans l'objet
                //if ( QuestionReponse.ReponseCollection.Count > int.Parse( Global.SettingsXml.RadioButtonListQuestionnaireRepeatColumn ) )
                //{
                //    cbl.RepeatColumns = QuestionReponse.ReponseCollection.Count / int.Parse( Global.SettingsXml.RadioButtonListQuestionnaireRepeatColumn );
                //}

                StyleWeb.ApplyStyleWeb( "Reponse", TypeStyleWeb.CheckBoxList, cbl );
                cellReponse.Controls.Add( cbl );
            }
            else
            {
                RadioButtonListStyle rbl = new RadioButtonListStyle();

                foreach ( PollAnswer pa in reponseChoix )
                {
                    rbl.Items.Add( pa.Answer );
                }
                if ( alignLeft )
                {
                    rbl.TextAlign = TextAlign.Left;
                }
                if ( directionHorizontal )
                {
                    rbl.RepeatDirection = RepeatDirection.Horizontal;
                }

                // On ne sait pas faire, c'est un attribut qui doit etre dans l'objet
                //if ( QuestionReponse.ReponseCollection.Count > int.Parse( Global.SettingsXml.RadioButtonListQuestionnaireRepeatColumn ) )
                //{
                //    rbl.RepeatColumns = QuestionReponse.ReponseCollection.Count / int.Parse( Global.SettingsXml.RadioButtonListQuestionnaireRepeatColumn );
                //}

                StyleWeb.ApplyStyleWeb( "Reponse", TypeStyleWeb.RadioButtonList, rbl );
                cellReponse.Controls.Add( rbl );
            }

            rowReponse.Controls.Add( cellReponse );
            rowReponse.HorizontalAlign = AlignementEnum( QuestionReponse.Question.AlignementReponse );
            tableReponse.Controls.Add( rowReponse );
            cellQuestionReponse.Controls.Add( tableReponse );
            AddNewRow( ref tableQuestionReponse, ref rowQuestionReponse, ref cellQuestionReponse );

            Table tableReponseTextuelle = new Table();
            StyleWeb.ApplyStyleWeb( "TableReponseTextuelle", TypeStyleWeb.Table, tableReponseTextuelle );
            foreach ( PollAnswer reponse in QuestionReponse.ReponseCollection )
            {
                if ( reponse.TypeReponse != TypeReponse.Choix )
                {
                    TableRow row = new TableRow();
                    TableCell cellLabel = new TableCell();
                    TableCell cellTextBox = new TableCell();

                    if ( DEBUG_STYLEWEB0 )
                    {
                        tableReponseTextuelle.BorderStyle = BorderStyle.Solid;
                        tableReponseTextuelle.BorderWidth = 1;
                        tableReponseTextuelle.BorderColor = Color.Red;

                        cellLabel.BorderStyle = BorderStyle.Solid;
                        cellLabel.BorderWidth = 1;
                        cellLabel.BorderColor = Color.Red;
                    }

                    switch ( reponse.TypeReponse )
                    {
                        // Les deux s'affichent de la meme facon
                        case TypeReponse.Ouverte:
                        case TypeReponse.Numerique:

                            // Label
                            Label rep = new Label();
                            rep.Text = reponse.Answer;
                            StyleWeb.ApplyStyleWeb( "ReponseTextuelleLabel", TypeStyleWeb.Label, rep );
                            cellLabel.VerticalAlign = VerticalAlign.Top;
                            cellLabel.HorizontalAlign = HorizontalAlign.Right;
                            cellLabel.Controls.Add( rep );
                            row.Cells.Add( cellLabel );

                            // TextBox
                            TextBox textbox = new TextBox();
                            StyleWeb.ApplyStyleWeb( "ReponseTextuelleTextBox", TypeStyleWeb.TextBox, textbox );
                            if ( reponse.Width != 0 )
                            {
                                textbox.Width = reponse.Width;
                            }
                            if ( reponse.Rows > 1 )
                            {
                                textbox.TextMode = TextBoxMode.MultiLine;
                                textbox.Rows = reponse.Rows;
                            }

                            cellTextBox.Controls.Add( textbox );
                            cellTextBox.HorizontalAlign = HorizontalAlign.Left;
                            row.Cells.Add( cellTextBox );
                            tableReponseTextuelle.Rows.Add( row );

                            break;

                        case TypeReponse.Date:

                            TextBoxDate textBoxDate = ( TextBoxDate )LoadControl( "~/UserControl/TextBoxDate.ascx" );
                            // Label
                            textBoxDate.Label = reponse.Answer;

                            Label label = ( Label )textBoxDate.FindControl( "LabelTextBoxDate" );
                            StyleWeb.ApplyStyleWeb( "ReponseTextuelleLabel", TypeStyleWeb.Label, label );
                            // Encore une merde :
                            // comme le label est ajoute dans une cellule pour aligner toutes
                            // les reponses textuelle ce putain de label 
                            // se retrouve avec son ID duplique dans la page !
                            // comme on s'en branle on y fou ce n'importe quoi du moment
                            // que c'est pas duplique !
                            label.ID = "labelDateID" + reponse.PollAnswerId.ToString();
                            if ( reponse.PollAnswerId == Guid.Empty )
                            {
                                // si la reponse date n'est pas encore cree le PollAnswerId est nul
                                label.ID = "labelDateID" + Guid.NewGuid().ToString();
                            }

                            cellLabel.VerticalAlign = VerticalAlign.Top;
                            cellLabel.HorizontalAlign = HorizontalAlign.Right;
                            cellLabel.Controls.Add( label );
                            row.Cells.Add( cellLabel );

                            // TextBox
                            TextBox textboxD = ( TextBox )textBoxDate.FindControl( "TextBoxDateText" );
                            StyleWeb.ApplyStyleWeb( "ReponseTextuelleTextBox", TypeStyleWeb.TextBox, textboxD );

                            cellTextBox.Controls.Add( textBoxDate );
                            cellTextBox.HorizontalAlign = HorizontalAlign.Left;
                            row.Cells.Add( cellTextBox );
                            tableReponseTextuelle.Controls.Add( row );

                            break;

                        case TypeReponse.SemiOuverte:

                            PopupTextBox popupTextBox = ( PopupTextBox )LoadControl( "~/UserControl/PopupTextBox.ascx" );

                            // Label
                            popupTextBox.LabelCheckBox = reponse.Answer;

                            Label labelSO = ( Label )popupTextBox.FindControl( "LabelChecbox" );
                            StyleWeb.ApplyStyleWeb( "ReponseTextuelleLabel", TypeStyleWeb.Label, labelSO );
                            // grosse merde encore ...
                            labelSO.ID = "labelSOID" + reponse.PollAnswerId.ToString();
                            if ( reponse.PollAnswerId == Guid.Empty )
                            {
                                // si la reponse SO n'est pas encore cree le PollAnswerId est nul
                                labelSO.ID = "labelSOID" + Guid.NewGuid().ToString();
                            }

                            cellLabel.VerticalAlign = VerticalAlign.Top;
                            cellLabel.HorizontalAlign = HorizontalAlign.Right;
                            cellLabel.Controls.Add( labelSO );
                            row.Cells.Add( cellLabel );

                            // TextBox
                            TextBox textboxSO = ( TextBox )popupTextBox.FindControl( "TextBoxText" );
                            StyleWeb.ApplyStyleWeb( "ReponseTextuelleTextBox", TypeStyleWeb.TextBox, textboxSO );
                            if ( reponse.Width != 0 )
                            {
                                popupTextBox.TextBoxWidth = reponse.Width.ToString();
                            }
                            if ( reponse.Rows > 1 )
                            {
                                popupTextBox.TextBoxRows = reponse.Rows;
                            }

                            cellTextBox.Controls.Add( popupTextBox );
                            cellTextBox.HorizontalAlign = HorizontalAlign.Left;
                            row.Controls.Add( cellTextBox );
                            tableReponseTextuelle.Controls.Add( row );

                            break;
                    } // switch ( reponse.TypeReponse )

                    cellQuestionReponse.Controls.Add( tableReponseTextuelle );
                    AddNewRow( ref tableQuestionReponse, ref rowQuestionReponse, ref cellQuestionReponse );

                } // fin du if ( reponse.TypeReponse != TypeReponse.Choix )
            } // fin du foreach ( PollAnswer reponse ...
        }

        // Message a l'utilisateur
        if ( QuestionReponse.Question.Message != string.Empty && QuestionReponse.Question.MessageHaut == false )
        {
            // Mettre le message dans une table pour pouvoir appliquer un style Table
            Label lblMessage = new Label();
            lblMessage.Text = QuestionReponse.Question.Message;
            Table _table3 = new Table();
            TableCell _cell3 = new TableCell();
            TableRow _row3 = new TableRow();
            _cell3.Controls.Add( lblMessage );
            _row3.Controls.Add( _cell3 );
            _table3.Controls.Add( _row3 );
            StyleWeb.ApplyStyleWeb( "TableMessage", TypeStyleWeb.Table, _table3 );

            cellQuestionReponse.Controls.Add( _table3 );
            AddNewRow( ref tableQuestionReponse, ref rowQuestionReponse, ref cellQuestionReponse );
        }

        _cellQuestion.Controls.Add( tableQuestionReponse );
        StyleWeb.ApplyStyleWeb( "CelluleQuestion", TypeStyleWeb.Label, _cellQuestion );
        _rowQuestion.Cells.Add( _cellQuestion );
        _tablePageQuestion.Rows.Add( _rowQuestion );

        PanelQuestion.Controls.Add( _tablePageQuestion );
    }

    #region Questionnaire

    protected void ButtonVoirQuestionaireOk_Click( object sender, EventArgs e )
    {
        Reporter.Trace( TraceReporter.Report.DEBUG1, "ButtonVoirQuestionaireOk_Click()" );

        if ( ButtonModifier.Visible == true )
        {
            Response.Redirect( "~/Poll/List.aspx" );
        }
        else // mode ajouter
        {
            Response.Redirect( "~/Poll/List.aspx#BasDePage" );
        }
    }

    protected void DropDownListQuestionnaire_SelectedIndexChanged( object sender, EventArgs e )
    {
        SessionState.Questionnaire = SessionState.Questionnaires[ DropDownListQuestionnaire.SelectedIndex ];
        CreateQuestionControls();
    }

    #endregion

    #region QuestionUpdate

    protected void TextBoxQuestion_TextChanged( object sender, EventArgs e )
    {
        Reporter.Trace( TraceReporter.Report.DEBUG1, "TextBoxQuestion_TextChanged()" );

        ButtonQuestionOk_Click( sender, e );
        ButtonQuestionOk.Click -= new EventHandler( ButtonQuestionOk_Click );
    }

    protected void ButtonQuestionOk_Click( object sender, EventArgs e )
    {
        Reporter.Trace( TraceReporter.Report.DEBUG1, "ButtonQuestionOk_Click()" );

        LabelValidationMessage.Text = "";

        if ( TextBoxQuestion.Text.Trim() != "" )
        {
            // L'utilisateur a rempli sa question, fini le mode Demo
            QuestionEnModeDemonstration = false;
            CheckBoxModeDemonstration.Checked = false;

            QuestionReponse.Question.Question = TextBoxQuestion.Text.Trim();
            int choixMin = 0;
            try
            {
                choixMin = int.Parse( TextBoxChoixMultipleMin.Text.Trim() );
            }
            catch
            { }
            int choixMax = 0;
            try
            {
                choixMax = int.Parse( TextBoxChoixMultipleMax.Text.Trim() );
            }
            catch
            { }
            if ( ( choixMin > choixMax ) || ( choixMin <= 0 ) || ( choixMax <= 0 ) )
            {
                choixMin = 0;
                choixMax = 0;
            }
            QuestionReponse.Question.ChoixMultipleMin = choixMin;
            QuestionReponse.Question.ChoixMultipleMax = choixMax;
            if ( QuestionReponse.Question.PollQuestionId != Guid.Empty )
            {
                Reporter.Trace( TraceReporter.Report.DEBUG1, string.Format( "PollQuestion.Update({0})", QuestionReponse.Question.Question ) );
                int status = PollQuestion.Update( QuestionReponse.Question );
                LabelValidationMessage.Visible = true;
                LabelValidationMessage.Text = "Question modifiée";
            }
        }

        CreateQuestionControls();
    }

    // AME12112009
    // Choisir l'ouverture ou la fermeture du volet reponse textuelle
    // Afficher la premiere reponse textuelle s'il y en a une
    // positionner la DropDownListTypeQuestionReponse
    private void choisirVoletReponseTextuelle()
    {
        TrReponseTextuelle.Visible = false;
        TrBoutonEffacerReponses.Visible = false;
        TextBoxReponseTextuelle.Text = "";

        // Afficher la premiere reponses qui ne soit pas Simple ou Multiple
        foreach ( PollAnswer reponse in QuestionReponse.ReponseCollection )
        {
            bool reponseTrouve = false;
            switch ( reponse.TypeReponse )
            {
                case TypeQuestionReponse.Ouverte:
                    TrReponseTextuelle.Visible = true;
                    TrBoutonEffacerReponses.Visible = true;
                    LabelReponseOuverte.Text = "Réponse ouverte : ";
                    ImageButtonReponseTextuelleEffacer.ToolTip = "Effacer les réponses ouvertes";
                    TextBoxReponseTextuelleLargeur.Text = reponse.Width.ToString();
                    TextBoxReponseTextuelleHauteur.Text = reponse.Rows.ToString();
                    reponseTrouve = true;
                    break;

                case TypeQuestionReponse.SemiOuverte:
                    TrReponseTextuelle.Visible = true;
                    TrBoutonEffacerReponses.Visible = true;
                    LabelReponseOuverte.Text = "Réponse semi-ouverte : ";
                    ImageButtonReponseTextuelleEffacer.ToolTip = "Effacer les réponses semi-ouvertes";
                    TextBoxReponseTextuelleLargeur.Text = reponse.Width.ToString();
                    TextBoxReponseTextuelleHauteur.Text = reponse.Rows.ToString();
                    reponseTrouve = true;
                    break;

                case TypeQuestionReponse.Numerique:
                    TrReponseTextuelle.Visible = true;
                    TrBoutonEffacerReponses.Visible = true;
                    LabelReponseOuverte.Text = "Réponse numérique : ";
                    ImageButtonReponseTextuelleEffacer.ToolTip = "Effacer les réponses numériques";
                    TextBoxReponseTextuelleLargeur.Text = reponse.Width.ToString();
                    TextBoxReponseTextuelleHauteur.Text = reponse.Rows.ToString();
                    reponseTrouve = true;
                    break;

                case TypeReponse.Date:
                    TrReponseTextuelle.Visible = true;
                    TrBoutonEffacerReponses.Visible = true;
                    LabelReponseOuverte.Text = "Réponse date : ";
                    ImageButtonReponseTextuelleEffacer.ToolTip = "Effacer les réponses dates";
                    TextBoxReponseTextuelleLargeur.Text = string.Empty;
                    TextBoxReponseTextuelleHauteur.Text = string.Empty;
                    reponseTrouve = true;
                    break;
            } // fin du switch

            if ( reponseTrouve )
            {
                TextBoxReponseTextuelle.Text = reponse.Answer;
                DropDownListTypeQuestionReponse.SelectedValue = reponse.TypeReponse;
                // BRY0002010305
                CheckBoxReponseTextuelleObligatoire.Checked = reponse.Obligatoire;
                break;
            }
            else
            {
                if ( QuestionReponse.Question.ChoixMultiple )
                {
                    DropDownListTypeQuestionReponse.SelectedValue = TypeQuestionReponse.ChoixMultiple;
                }
                else
                {
                    DropDownListTypeQuestionReponse.SelectedValue = TypeQuestionReponse.ChoixSimple;
                }
            }
        } // fin du foreach

        // AME24032010
        // Si il n'y a plus de reponses dans la question
        if ( QuestionReponse.ReponseCollection.Count == 0 )
        {
            // Remettre la dropdown a "Choisir un type de Question"
            DropDownListTypeQuestionReponse.SelectedValue = "-1";
        }
    }

    protected void DropDownListTypeQuestionReponse_SelectedIndexChanged( object sender, EventArgs e )
    {
        bool ancienChoixMultiple = QuestionReponse.Question.ChoixMultiple;
        TrOptionQuestion01.Visible = false;
        TrOptionQuestion02.Visible = false;

        switch ( DropDownListTypeQuestionReponse.SelectedValue )
        {
            case TypeQuestionReponse.ChoixSimple:
                QuestionReponse.Question.ChoixMultiple = false;
                if ( QuestionReponse.ReponseCollection.Count <= 0 )
                {
                    TrReponseTextuelle.Visible = false;
                    TrBoutonEffacerReponses.Visible = false;
                }
                ImageButtonReponseTextuelleEffacer.ToolTip = "Effacer les réponses choix";
                break;

            case TypeQuestionReponse.ChoixMultiple:
                QuestionReponse.Question.ChoixMultiple = true;
                if ( QuestionReponse.ReponseCollection.Count <= 0 )
                {
                    TrReponseTextuelle.Visible = false;
                    TrBoutonEffacerReponses.Visible = false;
                }
                ImageButtonReponseTextuelleEffacer.ToolTip = "Effacer les réponses choix";
                TrOptionQuestion01.Visible = true;
                TrOptionQuestion02.Visible = true;
                break;

            case TypeQuestionReponse.Ouverte:
                TrReponseTextuelle.Visible = true;
                TrBoutonEffacerReponses.Visible = true;
                LabelReponseOuverte.Text = "Réponse ouverte : ";
                ImageButtonReponseTextuelleEffacer.ToolTip = "Effacer les réponses ouvertes";
                break;

            case TypeQuestionReponse.SemiOuverte:
                TrReponseTextuelle.Visible = true;
                TrBoutonEffacerReponses.Visible = true;
                LabelReponseOuverte.Text = "Réponse semi-ouverte : ";
                ImageButtonReponseTextuelleEffacer.ToolTip = "Effacer les réponses semi-ouvertes";
                break;

            case TypeQuestionReponse.Numerique:
                TrReponseTextuelle.Visible = true;
                TrBoutonEffacerReponses.Visible = true;
                LabelReponseOuverte.Text = "Réponse numérique : ";
                ImageButtonReponseTextuelleEffacer.ToolTip = "Effacer les réponses numériques";
                break;

            case TypeReponse.Date:
                TrReponseTextuelle.Visible = true;
                TrBoutonEffacerReponses.Visible = true;
                LabelReponseOuverte.Text = "Réponse date : ";
                ImageButtonReponseTextuelleEffacer.ToolTip = "Effacer les réponses dates";
                break;

        } // fin du switch

        // AME12112009
        // Afficher la premiere reponses qui soit du type DropDownListTypeQuestionReponse
        // et differente des choix ...
        TextBoxReponseTextuelle.Text = "";
        // BRY0002010305
        CheckBoxReponseTextuelleObligatoire.Checked = false;
        foreach ( PollAnswer reponse in QuestionReponse.ReponseCollection )
        {
            if ( reponse.TypeReponse == DropDownListTypeQuestionReponse.SelectedValue )
            {
                TextBoxReponseTextuelle.Text = reponse.Answer;
                TextBoxReponseTextuelleLargeur.Text = reponse.Width.ToString();
                TextBoxReponseTextuelleHauteur.Text = reponse.Rows.ToString();
                // BRY0002010305
                CheckBoxReponseTextuelleObligatoire.Checked = reponse.Obligatoire;
                // Cas particulier de la reponse Date qui n'a pas de Width et de Rows
                if ( reponse.TypeReponse == TypeReponse.Date )
                {
                    TextBoxReponseTextuelleLargeur.Text = string.Empty;
                    TextBoxReponseTextuelleHauteur.Text = string.Empty;
                }
                break;
            }
        } // fin du foreach

        if ( QuestionEnModeDemonstration )
        {
            PollAnswer reponse = new PollAnswer();
            switch ( DropDownListTypeQuestionReponse.SelectedValue )
            {
                case TypeQuestionReponse.ChoixSimple:

                    // Question choix simple
                    TextBoxQuestion.Text = "Irez-vous en vacances cet été ?";
                    QuestionReponse.Question.Question = TextBoxQuestion.Text;
                    QuestionReponse.ReponseCollection.Clear();

                    reponse = new PollAnswer( "Oui" );
                    reponse.TypeReponse = TypeReponse.Choix;
                    QuestionReponse.ReponseCollection.Add( reponse );

                    reponse = new PollAnswer( "Non" );
                    reponse.TypeReponse = TypeReponse.Choix;
                    QuestionReponse.ReponseCollection.Add( reponse );

                    reponse = new PollAnswer( "Peut-être" );
                    reponse.TypeReponse = TypeReponse.Choix;
                    QuestionReponse.ReponseCollection.Add( reponse );

                    TextBoxMessageUtilisateur.Text = "Vous n'avez qu'un choix possible";
                    QuestionReponse.Question.Message = TextBoxMessageUtilisateur.Text;

                    TextBoxReponse.Text = "Oui; Non; Peut-être";

                    break;

                case TypeQuestionReponse.ChoixMultiple:

                    // Question choix multiple
                    TextBoxQuestion.Text = "Partez-vous en vacances ?";
                    QuestionReponse.Question.Question = TextBoxQuestion.Text;
                    QuestionReponse.ReponseCollection.Clear();

                    reponse = new PollAnswer( "Au printemps" );
                    reponse.TypeReponse = TypeReponse.Choix;
                    QuestionReponse.ReponseCollection.Add( reponse );

                    reponse = new PollAnswer( "En été" );
                    reponse.TypeReponse = TypeReponse.Choix;
                    QuestionReponse.ReponseCollection.Add( reponse );

                    reponse = new PollAnswer( "En automne" );
                    reponse.TypeReponse = TypeReponse.Choix;
                    QuestionReponse.ReponseCollection.Add( reponse );

                    reponse = new PollAnswer( "En hiver" );
                    reponse.TypeReponse = TypeReponse.Choix;
                    QuestionReponse.ReponseCollection.Add( reponse );

                    TextBoxMessageUtilisateur.Text = "Vous avez plusieurs choix possibles";
                    QuestionReponse.Question.Message = TextBoxMessageUtilisateur.Text;

                    TextBoxReponse.Text = "Au printemps; En été; En automne; En hiver";

                    break;

                case TypeQuestionReponse.SemiOuverte:

                    TextBoxReponseTextuelle.Text = "Autre préciser :";
                    reponse = new PollAnswer( TextBoxReponseTextuelle.Text );
                    reponse.TypeReponse = TypeReponse.SemiOuverte;
                    QuestionReponse.ReponseCollection.Add( reponse );

                    break;

                case TypeQuestionReponse.Ouverte:

                    TextBoxReponseTextuelle.Text = "Qu'avez vous fait pendant ces vacances :";
                    reponse = new PollAnswer( TextBoxReponseTextuelle.Text );
                    reponse.TypeReponse = TypeReponse.Ouverte;
                    reponse.Width = 250;
                    reponse.Rows = 4;
                    QuestionReponse.ReponseCollection.Add( reponse );

                    break;

                case TypeQuestionReponse.Numerique:

                    TextBoxReponseTextuelle.Text = "Votre âge ? : ";
                    reponse = new PollAnswer( TextBoxReponseTextuelle.Text );
                    reponse.TypeReponse = TypeReponse.Numerique;
                    QuestionReponse.ReponseCollection.Add( reponse );

                    break;

                case TypeQuestionReponse.Date:
                    TextBoxReponseTextuelle.Text = "Date de vos prochaines vacances ? : ";
                    reponse = new PollAnswer( TextBoxReponseTextuelle.Text );
                    reponse.TypeReponse = TypeReponse.Date;
                    QuestionReponse.ReponseCollection.Add( reponse );
                    break;

            } // fin du switch
        }

        //// AME12112009
        //// Mettre a jour le bool ChoixMultiple de la Question sur simple intervention 
        //// de l'utilisateur s'il retourne sur "Visualiser" : Poll\List.aspx
        //// c'est le seul endroit ou le faire
        //// historiquement le Question ne possede qu'un bool (choix simple/choix multiple) et pas de type
        if ( QuestionReponse.Question.PollQuestionId != Guid.Empty )
        {
            if ( DropDownListTypeQuestionReponse.SelectedValue == TypeQuestion.ChoixMultiple
                 && ancienChoixMultiple == false )
            {
                int status = PollQuestion.Update( QuestionReponse.Question );
                LabelValidationMessage.Visible = true;
                LabelValidationMessage.Text = "Type de la Question modifié en Choix multiple";
            }

            if ( DropDownListTypeQuestionReponse.SelectedValue == TypeQuestion.ChoixSimple
                 && ancienChoixMultiple == true )
            {
                int status = PollQuestion.Update( QuestionReponse.Question );
                LabelValidationMessage.Visible = true;
                LabelValidationMessage.Text = "Type de la Question modifié en Choix simple";
            }
        }

        CreateQuestionControls();
    }

    protected void DropDownListChoixReponse_SelectedIndexChanged( object sender, EventArgs e )
    {
        if ( DropDownListChoixReponse.SelectedIndex > 0 )
        {
            ArrayList choixReponse = ChoixReponsesPredefinies();
            string reponse = choixReponse[ DropDownListChoixReponse.SelectedIndex ].ToString();

            string[] reponses = reponse.Split( ';' );
            foreach ( string rep in reponses )
            {
                PollAnswer answer = new PollAnswer( rep.Trim() );
                answer.TypeReponse = TypeReponse.Choix;
                QuestionReponse.ReponseCollection.Add( answer );
            }

            if ( TextBoxReponse.Text.Trim() != string.Empty )
                TextBoxReponse.Text += "; ";
            TextBoxReponse.Text += reponse;

            //if ( ButtonModifier.Visible == true )
            //{
            //    LabelValidationMessage.Visible = true;
            //    LabelValidationMessage.Text = "Réponses modifiées, cliquez sur le bouton \"Modifier\" pour prendre en compte les modifications";
            //}
        }

        CreateQuestionControls();
    }

    protected void DropDownListAlignementQuestion_SelectedIndexChanged( object sender, EventArgs e )
    {
        switch ( DropDownListAlignementQuestion.SelectedIndex )
        {
            case 0:
                QuestionReponse.Question.AlignementQuestion = "Gauche";
                break;
            case 1:
                QuestionReponse.Question.AlignementQuestion = "Droite";
                break;
            case 2:
                QuestionReponse.Question.AlignementQuestion = "Centre";
                break;
        }

        if ( QuestionReponse.Question.PollQuestionId != Guid.Empty )
        {
            PollQuestion.UpdateAlignementQuestion( QuestionReponse.Question );
        }

        CreateQuestionControls();
    }

    protected void CheckBoxQuestionObligatoire_CheckedChanged( object sender, EventArgs e )
    {
        QuestionReponse.Question.Obligatoire = CheckBoxQuestionObligatoire.Checked;

        if ( QuestionReponse.Question.PollQuestionId != Guid.Empty )
        {
            int status = PollQuestion.Update( QuestionReponse.Question );
            LabelValidationMessage.Visible = true;
            LabelValidationMessage.Text = "Option Obligatoire de la Question modifiée";
        }

        CreateQuestionControls();
    }

    protected void CheckBoxQuestionTextuelleObligatoire_CheckedChanged( object sender, EventArgs e )
    {
        PollAnswerCollection reponses = PollAnswerCollection.GetByPollQuestionID( QuestionReponse.Question.PollQuestionId );
        foreach ( PollAnswer reponse in reponses )
        {
            // Trouver la reponse mais tanpis s'il y en a plusieurs et modifier le Obligatoire
            if ( TypeReponse.EstTextuelle( reponse.TypeReponse ) && TextBoxReponseTextuelle.Text == reponse.Answer )
            {
                reponse.Obligatoire = CheckBoxReponseTextuelleObligatoire.Checked;
                int status = PollAnswer.Update( reponse );
                LabelValidationMessage.Visible = true;
                LabelValidationMessage.Text = "Option Obligatoire de la Réponse : \"" + reponse.Answer + "\" modifiée<br/>";
            }
        }
        
        CreateQuestionControls();
    }

    protected void CheckBoxQuestionFin_CheckedChanged( object sender, EventArgs e )
    {
        QuestionReponse.Question.Fin = CheckBoxQuestionFin.Checked;

        if ( QuestionReponse.Question.PollQuestionId != Guid.Empty )
        {
            int status = PollQuestion.Update( QuestionReponse.Question );
            LabelValidationMessage.Visible = true;
            LabelValidationMessage.Text = "Option Fin de la Question modifiée";
        }

        CreateQuestionControls();
    }

    protected void CheckBoxModeDemonstration_CheckedChanged( object sender, EventArgs e )
    {
        QuestionEnModeDemonstration = CheckBoxModeDemonstration.Checked;
        CreateQuestionControls();
    }

    #endregion

    #region ReponseUpdate

    protected void DropDownListAlignementReponse_SelectedIndexChanged( object sender, EventArgs e )
    {
        switch ( DropDownListAlignementReponse.SelectedIndex )
        {
            case 0:
                QuestionReponse.Question.AlignementReponse = "Gauche";
                break;
            case 1:
                QuestionReponse.Question.AlignementReponse = "Droite";
                break;
            case 2:
                QuestionReponse.Question.AlignementReponse = "Centre";
                break;
        }

        if ( QuestionReponse.Question.PollQuestionId != Guid.Empty )
        {
            PollQuestion.UpdateAlignementReponse( QuestionReponse.Question );
        }

        CreateQuestionControls();
    }

    protected void TextBoxReponseTextuelle_TextChanged( object sender, EventArgs e )
    {
        ButtonReponseTextuelleOk_Click( sender, e );
        ButtonReponseTextuelleOk.Click -= new EventHandler( ButtonReponseTextuelleOk_Click );
    }
    
    protected void CheckBoxTextAGauche_CheckedChanged( object sender, EventArgs e )
    {
        if ( QuestionReponse.ReponseCollection.Count > 1 )
        {
            // C'est la premiere reponse de la collection qui porte l'attribut AlignLeft
            QuestionReponse.ReponseCollection[ 0 ].AlignLeft = CheckBoxTextAGauche.Checked;
            if ( QuestionReponse.Question.PollQuestionId != Guid.Empty )
            {
                PollAnswer.UpdateAlignLeft( QuestionReponse.ReponseCollection[ 0 ] );
                LabelValidationMessage.Visible = true;
                LabelValidationMessage.Text = "Option Texte à gauche de la Réponse modifiée";
            }

        }
        CreateQuestionControls();
    }

    protected void TextBoxReponse_TextChanged( object sender, EventArgs e )
    {
        Reporter.Trace( TraceReporter.Report.DEBUG1, "TextBoxReponse_TextChanged()" );
        
        ButtonReponseOk_Click( sender, e );
        ButtonReponseOk.Click -= new EventHandler( ButtonReponseOk_Click );
    }

    protected void TextBoxRangQuestion_TextChanged( object sender, EventArgs e )
    {
        try
        {
            int rang = int.Parse( TextBoxRangQuestion.Text );
            QuestionReponse.Question.Rank = rang;
        }
        catch
        {
            TextBoxRangQuestion.Text = QuestionReponse.Question.Rank.ToString();
        }
    }

    // Attention si l'utilisateur veut mettre un ';' dans sa reponse !!?
    protected void ButtonReponseOk_Click( object sender, EventArgs e )
    {
        Reporter.Trace( TraceReporter.Report.DEBUG1, "ButtonReponseOk_Click()" );

        string reponse = TextBoxReponse.Text.Trim();
        if ( reponse != "" )
        {
            effacerReponseDuType( "Choix simple" );
            effacerReponseDuType( "Choix multiple" );

            if ( reponse.Contains( ";" ) )
            {
                string[] reponses = reponse.Split( ';' );
                foreach ( string rep in reponses )
                {
                    PollAnswer answer = new PollAnswer( rep.Trim() );
                    answer.TypeReponse = TypeReponse.Choix;
                    QuestionReponse.ReponseCollection.Add( answer );
                }
            }
            else
            {
                PollAnswer answer = new PollAnswer( reponse );
                answer.TypeReponse = TypeReponse.Choix;
                QuestionReponse.ReponseCollection.Add( answer );
            }
        }

        //// Si ButtonModifier n'est pas visible, on est en mode ajout de questions 
        //// et pas modification de question existante
        //// ce message perturbait les utilisateurs
        //if ( ButtonModifier.Visible == true )
        //{
        //    LabelValidationMessage.Visible = true;
        //    LabelValidationMessage.Text = "Réponses modifiées, cliquez sur le bouton \"Modifier\" pour prendre en compte les modifications";
        //}

        CreateQuestionControls();
    }

    protected void ButtonReponseTextuelleOk_Click( object sender, EventArgs e )
    {
        Reporter.Trace( TraceReporter.Report.DEBUG1, "ButtonReponseTextuelleOk_Click()" );

        PollAnswer reponse = new PollAnswer();

        // Gros probleme pour les utilisateurs
        // s'il ne mettaient pas de libelle
        // il ne se passait rien
        // je prefere mettre a leur place un libelle " "
        if ( TextBoxReponseTextuelle.Text.Trim() == "" )
        {
            TextBoxReponseTextuelle.Text = " ";
        }
        //if ( TextBoxReponseTextuelle.Text.Trim() != "" )
        //{
            reponse.Answer = TextBoxReponseTextuelle.Text.Trim();
            reponse.TypeReponse = TypeReponse.Ouverte;
            try
            {
                int largeur = int.Parse( TextBoxReponseTextuelleLargeur.Text );
                reponse.Width = largeur;
                TextBoxReponseTextuelleLargeur.Text = "";
            }
            catch
            {
            }

            try
            {
                int hauteur = int.Parse( TextBoxReponseTextuelleHauteur.Text );
                reponse.Rows = hauteur;
                TextBoxReponseTextuelleHauteur.Text = "";
            }
            catch
            {
            }

            reponse.Obligatoire = CheckBoxReponseTextuelleObligatoire.Checked;
            reponse.TypeReponse = TypeQuestionReponse.GetTypeReponse( DropDownListTypeQuestionReponse.SelectedValue );
            QuestionReponse.ReponseCollection.Add( reponse );
        //}

        CreateQuestionControls();
    }

    protected void ButtonReponseEffacer_Click( object sender, EventArgs e )
    {
        QuestionReponse.ReponseCollection.Clear();
        CreateQuestionControls();
    }

    protected void ImageButtonReponseChoixEffacer_Click( object sender, ImageClickEventArgs e )
    {
        PollAnswerCollection reponses = new PollAnswerCollection();
        foreach ( PollAnswer reponse in QuestionReponse.ReponseCollection )
        {
            reponses.Add( reponse );
        }
        foreach ( PollAnswer reponse in reponses )
        {
            if ( reponse.TypeReponse == TypeReponse.Choix )
            {
                QuestionReponse.ReponseCollection.Remove( reponse );
            }
        }
        CreateQuestionControls();
    }

    // On ne peut pas faire directement le foreach sur QuestionReponse.ReponseCollection
    // et le remove directement sinon la collection d'iteration est modifee
    private void effacerReponseDuType( string typeReponse )
    {
        PollAnswerCollection reponses = new PollAnswerCollection();
        foreach ( PollAnswer reponse in QuestionReponse.ReponseCollection )
        {
            reponses.Add( reponse );
        }
        foreach ( PollAnswer reponse in reponses )
        {
            if ( reponse.TypeReponse == TypeQuestionReponse.GetTypeReponse( typeReponse ) )
            {
                QuestionReponse.ReponseCollection.Remove( reponse );
            }
        }
    }

    protected void ImageButtonReponseTextuelleEffacer_Click( object sender, ImageClickEventArgs e )
    {
        effacerReponseDuType( DropDownListTypeQuestionReponse.SelectedValue );

        // AME12112009
        choisirVoletReponseTextuelle();

        CreateQuestionControls();
    }

    protected void DropDownListDirection_SelectedIndexChanged( object sender, EventArgs e )
    {
        if ( QuestionReponse.ReponseCollection.Count > 1 )
        {
            // Seule la premiere reponse de la collection porte l'attribut Horizontal ou Vertical
            QuestionReponse.ReponseCollection[ 0 ].Horizontal = DropDownListDirection.SelectedValue == "Horizontal";
            if ( QuestionReponse.Question.PollQuestionId != Guid.Empty )
            {
                PollAnswer.UpdateVerticalHorizontal( QuestionReponse.ReponseCollection[ 0 ] );
                LabelValidationMessage.Visible = true;
                LabelValidationMessage.Text = "Option Direction de la Réponse modifiée";
            }
        }

        CreateQuestionControls();
    }

    #endregion

    #region MessageAuxInterviewes

    protected void ButtonMessageUtilisateurOk_Click( object sender, EventArgs e )
    {
        QuestionReponse.Question.Message = TextBoxMessageUtilisateur.Text;

        if ( QuestionReponse.Question.PollQuestionId != Guid.Empty )
        {
            int status = PollQuestion.Update( QuestionReponse.Question );
            LabelValidationMessage.Visible = true;
            LabelValidationMessage.Text = "Message à l'utilisateur modifié";
        }

        CreateQuestionControls();
    }

    protected void ImageButtonMessageUtilisateurEffacer_Click( object sender, ImageClickEventArgs e )
    {
        TextBoxMessageUtilisateur.Text = "";
        QuestionReponse.Question.Message = "";

        if ( QuestionReponse.Question.PollQuestionId != Guid.Empty )
        {
            int status = PollQuestion.Update( QuestionReponse.Question );
            LabelValidationMessage.Visible = true;
            LabelValidationMessage.Text = "Message à l'utilisateur effacé";
        }

        CreateQuestionControls();
    }

    protected void CheckBoxMessageEnHaut_CheckedChanged( object sender, EventArgs e )
    {
        QuestionReponse.Question.MessageHaut = CheckBoxMessageEnHaut.Checked;

        if ( QuestionReponse.Question.PollQuestionId != Guid.Empty )
        {
            int status = PollQuestion.Update( QuestionReponse.Question );
            LabelValidationMessage.Visible = true;
            LabelValidationMessage.Text = "Option En haut du message à l'utilisateur modifié";
        }

        CreateQuestionControls();
    }

    #endregion

    #region GestionDesVolets

    protected void ImageButtonExpandQuestion_Click( object sender, ImageClickEventArgs e )
    {
        TrOptionQuestion1.Visible = TrOptionQuestion1.Visible == false;
        TrOptionQuestion2.Visible = TrOptionQuestion2.Visible == false;
        TrOptionQuestion3.Visible = TrOptionQuestion3.Visible == false;
        if ( TrOptionQuestion1.Visible )
        {
            ImageButtonExpandQuestion.ImageUrl = "~/Images/collapse.jpg";
        }
        else
        {
            ImageButtonExpandQuestion.ImageUrl = "~/Images/expand.jpg";
        }
        SessionState.BooleanSate[ "ImageButtonExpandQuestion" ] = TrOptionQuestion1.Visible;
        CreateQuestionControls();
    }

    protected void ImageButtonExpandReponse_Click( object sender, ImageClickEventArgs e )
    {
        TrVoletReponse1.Visible = TrVoletReponse1.Visible == false;
        TrVoletReponse2.Visible = TrVoletReponse2.Visible == false;
        TrVoletReponse3.Visible = TrVoletReponse3.Visible == false;
        if ( TrVoletReponse1.Visible )
        {
            ImageButtonExpandReponse.ImageUrl = "~/Images/collapse.jpg";
        }
        else
        {
            ImageButtonExpandReponse.ImageUrl = "~/Images/expand.jpg";
        }
        SessionState.BooleanSate[ "ImageButtonExpandReponse" ] = TrVoletReponse1.Visible;
        CreateQuestionControls();
    }

    protected void ImageButtonExpandReponseTextuelle_Click( object sender, ImageClickEventArgs e )
    {
        TrVoletReponse4.Visible = TrVoletReponse4.Visible == false;
        TrVoletReponse5.Visible = TrVoletReponse5.Visible == false;
        TrVoletReponse6.Visible = TrVoletReponse6.Visible == false;
        if ( TrVoletReponse4.Visible )
        {
            ImageButtonExpandReponseTextuelle.ImageUrl = "~/Images/collapse.jpg";
        }
        else
        {
            ImageButtonExpandReponseTextuelle.ImageUrl = "~/Images/expand.jpg";
        }
        SessionState.BooleanSate[ "ImageButtonExpandReponseTextuelle" ] = TrVoletReponse4.Visible;
        CreateQuestionControls();
    }

    #endregion

    protected void ButtonSupprimerQuestion_Click( object sender, EventArgs e )
    {
        LabelValidationMessage.Text = "";
        LabelValidationMessage.Visible = true;

        if ( QuestionReponse.Question.PollQuestionId != null )
        {
            int status = PollQuestion.Delete( QuestionReponse.Question.PollQuestionId );
            if ( status == 0 )
            {
                LabelValidationMessage.Text = "Question supprimée";
                SessionState.Limitations.SupprimerQuestion();
            }
            else
            {
                LabelValidationMessage.Text = "Erreur à la suppression de la Question";
            }
        }

        RolloverSupprimer.Visible = false;
        ButtonAjouter.Visible = false;
        ButtonModifier.Visible = false;
        QuestionReponse = new ClassQuestionReponse();
    }

    protected void ButtonAjouterQuestion_Click( object sender, EventArgs e )
    {
        Reporter.Trace( TraceReporter.Report.DEBUG1, "ButtonAjouterQuestion_Click()" );

        LabelValidationMessage.Text = "";

        QuestionReponse.Question.CreationDate = DateTime.Now;
        if ( CheckBoxQuestionObligatoire.Checked )
            QuestionReponse.Question.Obligatoire = true;
        if ( CheckBoxQuestionFin.Checked )
            QuestionReponse.Question.Fin = true;

        PollQuestion question = new PollQuestion();
        question = QuestionReponse.Question;

        Reporter.Trace( TraceReporter.Report.DEBUG1, string.Format( "question ajouter : {0}", question.Question ) );
        
        // Completer la Question
        question.QuestionnaireID = SessionState.Questionnaire.QuestionnaireID;
        question.MembreGUID = SessionState.Questionnaire.MembreGUID;
        question.Societe = String.Empty;
        question.Instruction = String.Empty;

        Reporter.Trace( TraceReporter.Report.DEBUG1, string.Format( "question rang : {0}", question.Rank ) );

        // Tester les limitations avant d'ajouter la question
        if ( SessionState.Limitations.LimiteQuestions )
        {
            Tools.PageValidation( "La limite du nombre de Questions : " + SessionState.Limitations.NombreQuestions + " est atteinte.<br/>Contactez l'administrateur." );
        }

        int status = PollQuestion.Create( question );
        // QuestionReponse.Question.PollQuestionId  est modifie par l'instruction ci-dessus !
        // le formulaire sort du mode creation pour entrer en mode mise a jour et c'est un gros BUG
        // mais attention si on remet QuestionReponse.Question.PollQuestionId a Guid.Empty ici
        // alors question.PollQuestionId est aussi mit a Guid.Empty
        // incroyable pourtant on a fait PollQuestion question = new PollQuestion();
        // attention QuestionReponse dans Current.Session marche pareil !
        // 3 heures de recherche pour trouver ce BUG le 09/11/2008
        // C'est pas grave on va le remettre a Guid.Empty a la fin

        if ( status != 0 )
        {
            LabelValidationMessage.Visible = true;
            LabelValidationMessage.Text += "Erreur à la création de la Question<br/>";
        }
        else
        {
            SessionState.Limitations.AjouterQuestion();

            LabelValidationMessage.Visible = true;
            string typeQuestion = QuestionReponse.Question.ChoixMultiple == false ? "choix simple" : "choix multiple";
            LabelValidationMessage.Text += "Question " + typeQuestion + "de rang : " + question.Rank.ToString() + " crée correctement<br/>";
            int rank = 1;

            int statusCreation = 0;

            if ( QuestionReponse.ReponseCollection.Count > 0 )
            {
                foreach ( PollAnswer reponse in QuestionReponse.ReponseCollection )
                {
                    Reporter.Trace( TraceReporter.Report.DEBUG1, string.Format( "QuestionReponse : {0}", QuestionReponse.Question.Question ) );

                    PollAnswer a = new PollAnswer();

                    if( reponse.TypeReponse == TypeReponse.Choix )
                    {
                            bool alignleft = CheckBoxTextAGauche.Checked;
                            bool horizontal = DropDownListDirection.SelectedValue == "Horizontal";
                            // Ne taguer a left que la permiere reponse
                            if ( alignleft )
                            {
                                a.AlignLeft = true;
                                alignleft = false;
                            }
                            if ( horizontal )
                            {
                                a.Horizontal = true;
                                horizontal = false;
                            }
                    }

                    a = reponse;
                    a.PollQuestionId = question.PollQuestionId;
                    a.Rank = rank;

                    statusCreation = PollAnswer.Create( a );
                    if ( statusCreation != 0 )
                    {
                        statusCreation += 1;
                    }
                    rank = rank + 1;

                }// fin du foreach ( PollAnswer reponse ...

                if ( statusCreation == 0 )
                {
                    LabelValidationMessage.Text += "Réponses crées correctement<br />";
                }
                else
                {
                    LabelValidationMessage.Text += "Erreur à la création des réponses, status :" + statusCreation.ToString();
                    LabelValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                }

            }
        }

        // Incrementer le rang de la question
        QuestionReponse.Question.Rank += 1;
        TextBoxRangQuestion.Text = QuestionReponse.Question.Rank.ToString();

        // Laisser le formulaire en mode creation, non de dieu
        QuestionReponse.Question.PollQuestionId = Guid.Empty;
    }

    protected void ButtonModifierQuestion_Click( object sender, EventArgs e )
    {
        LabelValidationMessage.Text = "";
        int status = 0;

        // Suppression des Reponses existantes pour cette Question
        PollAnswerCollection reponses = PollAnswerCollection.GetByPollQuestionID( QuestionReponse.Question.PollQuestionId );
        foreach ( PollAnswer reponse in reponses )
        {
            // Suppression des Votes pour les Reponses
            PollVoteCollection votes = PollVoteCollection.GetVotes( reponse.PollAnswerId );
            int nbVotes = PollVoteCollection.NumberOfVotesByAnswer( reponse.PollAnswerId );
            foreach ( PollVote vote in votes )
            {
                status += PollVote.Delete( vote.VoteId );
            }
            if ( nbVotes > 0 )
            {
                LabelValidationMessage.Text += "Votes supprimés : " + nbVotes.ToString() + " status : " + status.ToString() + "<br/>";
            }

            LabelValidationMessage.Text += "Réponse : " + reponse.Answer;
            status = PollAnswer.Delete( reponse.PollAnswerId );
            if ( status == 0 )
            {
                LabelValidationMessage.Text += " supprimée<br />";
            }
            else
            {
                LabelValidationMessage.Text += " erreur<br />";
            }
        }

        if ( CheckBoxQuestionObligatoire.Checked )
            QuestionReponse.Question.Obligatoire = true;

        // Mise a jour de la Question
        status = PollQuestion.Update( QuestionReponse.Question );
        if ( status != 1 )
        {
            LabelValidationMessage.Visible = true;
            LabelValidationMessage.Text += "Erreur à la mise à jour de la Question<br/>";
        }
        else
        {
            LabelValidationMessage.Visible = true;
            string typeQuestion = QuestionReponse.Question.ChoixMultiple == false ? "choix simple" : "choix multiple";
            LabelValidationMessage.Text += "Question " + typeQuestion + " mis à jour correctement<br/>";

            if ( QuestionReponse.ReponseCollection.Count > 0 )
            {
                int rank = 1;
                int statusCreation = 0;
                bool alignleft = CheckBoxTextAGauche.Checked;
                bool horizontal = DropDownListDirection.SelectedValue == "Horizontal";

                foreach ( PollAnswer reponse in QuestionReponse.ReponseCollection )
                {
                    PollAnswer a = new PollAnswer();
                    switch ( reponse.TypeReponse )
                    {
                        case TypeReponse.Choix:

                            a = new PollAnswer();
                            a = reponse;
                            a.PollQuestionId = QuestionReponse.Question.PollQuestionId;
                            // Ne taguer a left que la permiere reponse
                            if ( alignleft )
                            {
                                a.AlignLeft = true;
                                alignleft = false;
                            }
                            if ( horizontal )
                            {
                                a.Horizontal = true;
                                horizontal = false;
                            }
                            a.Rank = rank;
                            rank = rank + 1;

                            statusCreation = PollAnswer.Create( a );
                            if ( statusCreation != 0 )
                            {
                                statusCreation += 1;
                            }

                            break;

                        case TypeReponse.Ouverte:
                        case TypeReponse.SemiOuverte:
                        case TypeReponse.Numerique:
                        case TypeReponse.Date:

                            a = new PollAnswer();
                            a = reponse;
                            a.PollQuestionId = QuestionReponse.Question.PollQuestionId;
                            a.Rank = rank;
                            rank = rank + 1;

                            statusCreation = PollAnswer.Create( a );
                            if ( statusCreation != 0 )
                            {
                                statusCreation += 1;
                            }

                            break;
                    }

                    if ( statusCreation == 0 )
                    {
                        LabelValidationMessage.Text += "Réponse type : " + reponse.TypeReponse + " : " + a.Answer + " crées correctement<br />";
                    }
                }

                if ( statusCreation != 0 )
                {
                    LabelValidationMessage.Text += "Erreur à la création des réponses, status :" + statusCreation.ToString();
                    LabelValidationMessage.CssClass = "LabelValidationMessageErrorStyle";
                }
            }
        }

        CreateQuestionControls();
    }

    protected void ButtonPrecedente_Click( object sender, EventArgs e )
    {
        IndexQuestionReponseCourante = ( IndexQuestionReponseCourante - 1 ) >= 0 ? ( IndexQuestionReponseCourante - 1 ) : QuestionnaireQuestionReponse.Count - 1;
        QuestionReponse = QuestionnaireQuestionReponse[ IndexQuestionReponseCourante ];
        Response.Redirect( "~/Wizard/Question.aspx?PollQuestionId=" + QuestionReponse.Question.PollQuestionId );
    }

    protected void ButtonSuivante_Click( object sender, EventArgs e )
    {
        IndexQuestionReponseCourante = ( IndexQuestionReponseCourante + 1 ) % QuestionnaireQuestionReponse.Count;
        QuestionReponse = QuestionnaireQuestionReponse[ IndexQuestionReponseCourante ];
        Response.Redirect( "~/Wizard/Question.aspx?PollQuestionId=" + QuestionReponse.Question.PollQuestionId );
    }
}
