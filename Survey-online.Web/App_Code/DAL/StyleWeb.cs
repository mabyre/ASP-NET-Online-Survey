
#region Using

using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel; // pour faire les tags et le TagCloud
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Xml;
using System.Data;
using System.ComponentModel;
using System.Net.Mail;
using System.Drawing;
using StyleWebData;
using System.Web.UI.WebControls;
using Sql.Web.Data;

#endregion

namespace Sql.Web.Data
{
    public class TypeStyleWeb
    {
        public const string Table = "Table";
        public const string Label = "Label";
        public const string TextBox = "TextBox";
        public const string RadioButtonList = "RadioButtonList";
        public const string CheckBoxList = "CheckBoxList";
    }

    [Serializable()]
    public class StyleWeb : IComparer<StyleWeb>
    {
        #region Constructor

        public StyleWeb()
        {
        }

        #endregion

        #region Properties

        private Guid _StyleWebGUID;
        public Guid StyleWebGUID
        {
            get { return _StyleWebGUID; }
            set { _StyleWebGUID = value; }
        }

        private string _NomStyleWeb;
        public string NomStyleWeb
        {
            get { return _NomStyleWeb; }
            set { _NomStyleWeb = value; }
        }

        private string _Type;
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        // Pour un objet TextBox par exemple, on ne sait retrouver le style de l'objet
        // identique a comme si on appliquait pas le style 
        // ( inset 2 pix, ne donne pas le rendu de l'objet TextBox si on ne lui applique
        // pas de style ) on rend donc le style Applicable == false pour ne pas appliquer
        // de style a l'objet !!!
        public bool _Applicable;
        public bool Applicable
        {
            get { return _Applicable; }
            set { _Applicable = value; }
        }

        public string _BackColor;
        public string BackColor
        {
            get { return _BackColor; }
            set { _BackColor = value; }
        }

        public string _BorderColor;
        public string BorderColor
        {
            get { return _BorderColor; }
            set { _BorderColor = value; }
        }

        public int _BorderStyle;
        public int BorderStyle
        {
            get { return _BorderStyle; }
            set { _BorderStyle = value; }
        }

        public int _BorderWidth;
        public int BorderWidth
        {
            get { return _BorderWidth; }
            set { _BorderWidth = value; }
        }

        public string _ForeColor;
        public string ForeColor
        {
            get { return _ForeColor; }
            set { _ForeColor = value; }
        }

        // Propriete specifique des objets Table
        public string _Padding;
        public string Padding
        {
            get { return _Padding; }
            set { _Padding = value; }
        }

        public string _Spacing;
        public string Spacing
        {
            get { return _Spacing; }
            set { _Spacing = value; }
        }

        // Taille
        public string _Width;
        public string Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        public string _Height;
        public string Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        // Font 
        public string _FontName;
        public string FontName
        {
            get { return _FontName; }
            set { _FontName = value; }
        }

        public int _FontSize;
        public int FontSize
        {
            get { return _FontSize; }
            set { _FontSize = value; }
        }

        public bool _Bold;
        public bool Bold
        {
            get { return _Bold; }
            set { _Bold = value; }
        }

        public bool _Underline;
        public bool Underline
        {
            get { return _Underline; }
            set { _Underline = value; }
        }

        public bool _Strikeout;
        public bool Strikeout
        {
            get { return _Strikeout; }
            set { _Strikeout = value; }
        }

        public bool _Italic;
        public bool Italic
        {
            get { return _Italic; }
            set { _Italic = value; }
        }

        public bool _Overline;
        public bool Overline
        {
            get { return _Overline; }
            set { _Overline = value; }
        }

        // Autre proprietes graphiques
        public string _TextAlign;
        public string TextAlign
        {
            get { return _TextAlign; }
            set { _TextAlign = value; }
        }

        #endregion


        #region IComparable

        public int Compare( StyleWeb x, StyleWeb y )
        {
            return x.NomStyleWeb.CompareTo( y.NomStyleWeb );
        }

        static int CompareAuteur( StyleWeb x, StyleWeb y )
        {
            return x.NomStyleWeb.CompareTo( y.NomStyleWeb );
        }

        #endregion

        public static StyleWeb Fill( DataRow r )
        {
            StyleWeb o = new StyleWeb();

            o.StyleWebGUID = new Guid( r[ "StyleWebGUID" ].ToString() );
            o.NomStyleWeb = r[ "NomStyleWeb" ].ToString();
            o.Type = r[ "Type" ].ToString();
            o.Applicable = ( bool )r[ "Applicable" ];
            o.BackColor = r[ "BackColor" ].ToString();
            o.BorderColor = r[ "BorderColor" ].ToString();
            o.BorderStyle = int.Parse( r[ "BorderStyle" ].ToString() );
            o.BorderWidth = int.Parse( r[ "BorderWidth" ].ToString() );
            o.ForeColor = r[ "ForeColor" ].ToString();
            //o.Width = int.Parse( r[ "Width" ].ToString() );
            o.Padding = r[ "Padding" ].ToString();
            o.Spacing = r[ "Spacing" ].ToString();
            o.Width = r[ "Width" ].ToString();
            o.Height = r[ "Height" ].ToString();
            o.FontName = r[ "FontName" ].ToString();
            o.FontSize = int.Parse( r[ "FontSize" ].ToString() );
            o.Bold = ( bool )r[ "Bold" ];
            o.Underline = ( bool )r[ "Underline" ];
            o.Strikeout = ( bool )r[ "Strikeout" ];
            o.Italic = ( bool )r[ "Italic" ];
            o.Overline = ( bool )r[ "Overline" ];
            o.TextAlign = r[ "TextAlign" ].ToString();

            return o;
        }

        // Traduire un StyleWeb en Style
        public static Style StyleWebToStyle( StyleWeb sw )
        {
            Style style = new Style();

            if ( sw.BackColor != "none" )
                style.BackColor = ColorTranslator.FromHtml( sw.BackColor );

            if ( sw.BorderColor != "none" )
                style.BorderColor = ColorTranslator.FromHtml( sw.BorderColor );
            if ( sw.BorderStyle != 0 )
                style.BorderStyle = ( BorderStyle )sw.BorderStyle;
            if ( sw.BorderWidth != 0 )
                style.BorderWidth = sw.BorderWidth;

            if ( sw.ForeColor != "none" )
                style.ForeColor = ColorTranslator.FromHtml( sw.ForeColor );
            //if ( sw.Height != 0 )
                style.Height = new Unit( sw.Height );
            //if ( sw.Width != 0 )
                style.Width = new Unit( sw.Width );

            if ( sw.FontName != "none" )
                style.Font.Name = sw.FontName;
            if ( sw.FontSize != 0 )
                style.Font.Size = FontUnit.Point( sw.FontSize );

            style.Font.Bold = sw.Bold;
            style.Font.Underline = sw.Underline;
            style.Font.Strikeout = sw.Strikeout;
            style.Font.Italic = sw.Italic;
            style.Font.Overline = sw.Overline;

            return style;
        }

        // Traduire un Style en StyleWeb
        public static void StyleToStyleWeb( StyleWeb styleWeb, Style style )
        {
            if ( style.BackColor != Color.Empty )
                styleWeb.BackColor = Colors.ToHtml( style.BackColor );

            if ( style.BorderColor != Color.Empty )
                styleWeb.BorderColor = Colors.ToHtml( style.BorderColor );
            if ( style.BorderStyle != 0 )
                styleWeb.BorderStyle = ( int )style.BorderStyle;
            if ( style.BorderWidth != 0 )
                styleWeb.BorderWidth = (int)style.BorderWidth.Value;
            if ( style.ForeColor != Color.Empty )
                styleWeb.ForeColor = Colors.ToHtml( style.ForeColor );
            //if ( style.Height != 0 )
            styleWeb.Height = style.Height.ToString(); // style.Height.Value.ToString();
            //if ( style.Width != 0 )
            styleWeb.Width = style.Width.ToString(); //.Value.ToString();

            styleWeb.FontName = style.Font.Name;
            //if ( style.Font.Size != 0 )
                styleWeb.FontSize = (int)style.Font.Size.Unit.Value ;

            styleWeb.Bold = style.Font.Bold;
            styleWeb.Underline = style.Font.Underline;
            styleWeb.Strikeout = style.Font.Strikeout;
            styleWeb.Italic = style.Font.Italic;
            styleWeb.Overline = style.Font.Overline;
        }


        /* TextAlign sera dans chaque reponse donc pas dans le XML mais dans la BD
         * sinon cela n'a pas de sens pour l'utilisateur
         * TextAlign n'existe pas dans le Style
         */
        public static void ApplyStyleWeb( string nomStyle, string typeStyle, WebControl webControl )
        {
            // C'est un peu loufoque mais comme le boulot de lier un nom de membre a un
            // questionnaire etait deja fait il est repris ici
            // une reflexion plus global devrait etre mene pour par exemple
            // configurer une nouvelle variable SessionState.Membre dans login.cs
            // par exemple
            string membre = WebContent.GetUtilisateur();

            StyleWeb sw = new StyleWeb();
            Style st = new Style();
            try
            {
                sw = XmlStyleWebProvider.GetStyleWeb( membre, nomStyle, typeStyle );
                st = StyleWeb.StyleWebToStyle( sw );
                if ( sw.Applicable )
                {
                    webControl.ApplyStyle( st );

                    if ( typeStyle == TypeStyleWeb.Table )
                    {
                        // On aurait bien aimé utiliser Unit() ici mais CellPadding et CellSpacing sont des int !
                        // Tandis que Height et Width sont des Unit()
                        if ( sw.Padding != "" ) ( ( Table )webControl ).CellPadding = int.Parse( sw.Padding );
                        if ( sw.Spacing != "" ) ( ( Table )webControl ).CellSpacing = int.Parse( sw.Spacing );
                    }
                }
            }
            catch
            {
                // Mettre un break point pour detecter les erreurs Xml
                int i = 3;
            }
        }

        #region CreateUpdateDeleteMethodes

        public int Create()
        {
            string membre = SessionState.MemberInfo.NomUtilisateur;
            int status = XmlStyleWebProvider.Create( membre, this );
            return status;
        }

        public int Update()
        {
            string membre = SessionState.MemberInfo.NomUtilisateur;
            int status = XmlStyleWebProvider.Update( membre, this );
            return status;
        }

        public int Delete()
        {
            string membre = SessionState.MemberInfo.NomUtilisateur;
            int status = XmlStyleWebProvider.Delete( membre, this );
            return status;
        }

        #endregion

        //public static StyleWeb GetStyleWeb( Guid StyleWebGUID )
        //{
        //    if ( StyleWebGUID == Guid.Empty )
        //    {
        //        return null;
        //    }

        //    StyleWebCollection collection = StyleWebCollection.GetAll();
        //    foreach ( StyleWeb o in collection )
        //    {
        //        if ( o.StyleWebGUID == StyleWebGUID )
        //        {
        //            return o;
        //        }
        //    }

        //    return null;
        //}

        //public static StyleWeb GetStyleWeb( string nomStyleWeb )
        //{
        //    if ( nomStyleWeb == String.Empty )
        //    {
        //        return null;
        //    }

        //    StyleWebCollection collection = StyleWebCollection.GetAll();
        //    foreach ( StyleWeb o in collection )
        //    {
        //        if ( o.NomStyleWeb == nomStyleWeb )
        //        {
        //            return o;
        //        }
        //    }

        //    return null;
        //}
    }

    public class StyleWebCollection : List<StyleWeb>
    {
        private List<StyleWeb> _collection = null;

        public StyleWebCollection()
        {
            _collection = new List<StyleWeb>();
        }

        //public static StyleWebCollection GetAll()
        //{
        //    string membre = HttpContext.Current.User.Identity.Name;
        //    return XmlStyleWebProvider.GetAll( membre );
        //}
    }
}
