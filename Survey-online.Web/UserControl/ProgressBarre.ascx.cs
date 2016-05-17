
/**
 * Barre de progression :
 * States : nombre d'etats de la barre de progression
 * CurrentState : etat en cours
**/

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
using System.Xml.Linq;
using System.Text;
using System.Drawing;

public partial class ProgessBarre : System.Web.UI.UserControl
{
    // Proprietes du control
    public int States
    {
        get
        {
            if ( Session[ "_ProgressBarreStates" ] == null )
            {
                Session[ "_ProgressBarreStates" ] = 10;
            }
            return ( int )Session[ "_ProgressBarreStates" ];
        }
        set { Session[ "_ProgressBarreStates" ] = value; }
    }

    public int CurrentState
    {
        get 
        {
            if ( Session[ "_ProgressBarreCurrentState" ] == null )
            {
                Session[ "_ProgressBarreCurrentState" ] = 0;
            }
            return (int)Session[ "_ProgressBarreCurrentState" ];
        }
        set { Session[ "_ProgressBarreCurrentState" ] = value; }
    }

    // LabelMessages
    private string _LabelMessagesText = "";
    public string LabelMessagesText
    {
        get { return _LabelMessagesText; }
        set { _LabelMessagesText = value; }
    }

    private string _LabelMessagesCssClass = "";
    public string LabelMessagesCssClass
    {
        get { return _LabelMessagesCssClass; }
        set { _LabelMessagesCssClass = value; }
    }

    // PanelBarSide
    private string _PanelBarSideWidth = "300px";
    public string PanelBarSideWidth
    {
        get { return _PanelBarSideWidth; }
        set { _PanelBarSideWidth = value; }
    }

    private string _PanelBarSideForeColor = "Silver";
    public string PanelBarSideForeColor
    {
        get { return _PanelBarSideForeColor; }
        set { _PanelBarSideForeColor = value; }
    }

    private string _PanelBarSideHeight = "10px";
    public string PanelBarSideHeight 
    {
        get { return _PanelBarSideHeight; }
        set { _PanelBarSideHeight = value; }
    }

    // PanelProgress
    private string _PanelProgressBackColor = "Green";
    public string PanelProgressBackColor
    {
        get { return _PanelProgressBackColor; }
        set { _PanelProgressBackColor = value; }
    }

    // LabelProgress
    private string _LabelProgressText = "";
    public string LabelProgressText
    {
        get { return _LabelProgressText; }
        set { _LabelProgressText = value; }
    }

    private string _LabelProgressCssClass = "";
    public string LabelProgressCssClass
    {
        get { return _LabelProgressCssClass; }
        set { _LabelProgressCssClass = value; }
    }

    // Rappelles toi cet evt a lieu apres le click() de la page parent
    // c'est pour creer des controls qui reagissent a des evt parent
    protected override void OnPreRender( EventArgs e )
    {
        base.OnPreRender( e );

        if ( LabelMessagesText != "" )
        {
            LabelMessages.Text = LabelMessagesText;
            LabelMessages.CssClass = LabelMessagesCssClass;
            LabelMessages.Visible = true;
        }
        else
        {
            LabelMessages.Visible = false;
        }

        PanelBarSide.Width = new Unit( PanelBarSideWidth );
        PanelBarSide.Height = new Unit( PanelBarSideHeight );
        PanelBarSide.ForeColor = ColorTranslator.FromHtml( PanelBarSideForeColor );

        PanelProgress.BackColor = ColorTranslator.FromHtml( PanelProgressBackColor );
        PanelProgress.Height = PanelBarSide.Height;

        if ( LabelProgressText != "" )
        {
            LabelProgress.Text = LabelProgressText;
            LabelProgress.CssClass = LabelProgressCssClass;
            LabelProgress.Visible = true;
        }
        else
        {
            LabelProgress.Visible = false;
        }

        if ( CurrentState > 0 && CurrentState <= States )
        {
            PanelProgress.Width = new Unit( CurrentState * ( int )PanelBarSide.Width.Value / States );
        }
        else
        {
            CurrentState = 0;
            PanelProgress.Width = new Unit( 0 );
        }
    }
}


