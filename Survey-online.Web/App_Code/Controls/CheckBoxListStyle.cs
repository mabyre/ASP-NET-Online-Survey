//
// Palier au fait que cette saloperie d'objet CheckBoxList 
// n'applique pas le render au label de l'input
// 
// Ce code est tres, tres dangereux !!!
// Les deux objets RadioButtonList et CheckBoxList semblent se ressembler
// mais leur comportement est bien different
// J'ai ete oblige d'ajouter un writer.AddAttribute( "value" pour utiliser 
// l'attribut value, qu'est ce que cela nous reserve t-il encore !
//
// 
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace UserControl.Web.Controls
{
    public class CheckBoxListStyle : CheckBoxList
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
            CheckBox checkBox = new CheckBox();
            checkBox.Page = this.Page;
            // Avec le 
            // radioButton.GroupName = this.UniqueID; mais checkBox ne possede pas cette propriete
            // c'est le debut de la galere 3 heures de recherche de bug pour finalement ecrire
            // writer.AddAttribute l'objet a 2 attributs name mais ca marche !
            // 
            checkBox.ID = this.ClientID + "_" + repeatIndex.ToString();

            string text = string.Format
            ( 
                "<font color=\"{0}\" style=\"font-family:{1};font-size:{2};font-weight:{3}\">{4}</font>",
                Colors.ToHtml( this.ForeColor ),
                this.Font.Name,
                this.Font.Size,
                this.Font.Bold == true ? "bold" : "normal",
                this.Items[ repeatIndex ].Text
            );
            checkBox.Text = text;

            checkBox.Attributes[ "value" ] = this.Items[ repeatIndex ].Value;
            checkBox.Checked = this.Items[ repeatIndex ].Selected;
            checkBox.TextAlign = this.TextAlign;
            checkBox.AutoPostBack = this.AutoPostBack;
            checkBox.TabIndex = this.TabIndex;
            checkBox.Enabled = this.Enabled;

            // modifie le input
            writer.AddAttribute( "name", this.UniqueID + "$" + repeatIndex.ToString() );
            writer.AddAttribute( "value", this.Items[ repeatIndex ].Value );

            checkBox.RenderControl( writer );
        }
    }
}