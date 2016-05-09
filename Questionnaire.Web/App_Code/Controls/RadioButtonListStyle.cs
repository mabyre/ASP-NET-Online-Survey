//
// Palier au fait que cette saloperie d'objet RadioButtonList 
// n'applique pas le render au label de l'input
// 
//
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace UserControl.Web.Controls
{
    public class RadioButtonListStyle : RadioButtonList
    {
        // On recupere le style de this pour l'appliquer au Text
        protected override void RenderItem
        (
            ListItemType itemType,
            int repeatIndex,
            RepeatInfo repeatInfo,
            HtmlTextWriter writer
        )
        {
            RadioButton radioButton = new RadioButton();
            radioButton.Page = this.Page;
            radioButton.GroupName = this.UniqueID;
            radioButton.ID = this.ClientID + "_" + repeatIndex.ToString();

            string text = string.Format
            ( 
                "<font color=\"{0}\" style=\"font-family:{1};font-size:{2};font-weight:{3}\">{4}</font>",
                Colors.ToHtml( this.ForeColor ),
                this.Font.Name,
                this.Font.Size,
                this.Font.Bold == true ? "bold" : "normal",
                this.Items[ repeatIndex ].Text
            );
            radioButton.Text = text;

            radioButton.Attributes[ "value" ] = this.Items[ repeatIndex ].Value;
            radioButton.Checked = this.Items[ repeatIndex ].Selected;
            radioButton.TextAlign = this.TextAlign;
            radioButton.AutoPostBack = this.AutoPostBack;
            radioButton.TabIndex = this.TabIndex;
            radioButton.Enabled = this.Enabled;

            // pour modifier le input
            //writer.AddAttribute( "name", this.UniqueID + "$" + repeatIndex.ToString() );
            //writer.AddAttribute( "align", "right" );

            radioButton.RenderControl( writer );
        }
    }
}