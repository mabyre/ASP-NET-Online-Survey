using System.Web.UI.WebControls;
using System.Web.UI;
using System.Drawing;

namespace UserControl.Web.Controls
{
    public class SmallRolloverButton : Button
    {
        protected override void AddAttributesToRender( System.Web.UI.HtmlTextWriter writer )
        {
            base.AddAttributesToRender( writer );
            writer.AddAttribute( "onmouseover", "this.className='small_button';" );
            writer.AddAttribute( "onmouseout", "this.className='small_button';" );
            writer.AddAttribute( "class", "small_button" );
        }
    }

    public class RolloverButton : Button
    {
        protected override void AddAttributesToRender( System.Web.UI.HtmlTextWriter writer )
        {
            base.AddAttributesToRender( writer );
            writer.AddAttribute( "onmouseover", "this.className='button';" );
            writer.AddAttribute( "onmouseout", "this.className='button';" );
            writer.AddAttribute( "class", "button" );
        }
    }

    public class ButtonRepondre : Button
    {
        protected override void AddAttributesToRender( System.Web.UI.HtmlTextWriter writer )
        {
            base.AddAttributesToRender( writer );
            writer.AddAttribute( "onmouseover", "this.className='buttonRepondre';" );
            writer.AddAttribute( "onmouseout", "this.className='buttonRepondre';" );
            writer.AddAttribute( "class", "buttonRepondre" );
        }
    }

    public class SmallRolloverLink : Button
    {
        protected override void AddAttributesToRender( System.Web.UI.HtmlTextWriter writer )
        {
            base.AddAttributesToRender( writer );
            writer.AddAttribute( "onmouseover", "this.className='small_button'" );
            writer.AddAttribute( "onmouseout", "this.className='small_button'" );
            writer.AddAttribute( "class", "small_button" );

            string navurl = NavigateURL;
            if ( ( base.OnClientClick == "" & navurl != "" ) )
            {
                writer.AddAttribute( "onclick", "window.navigate('" + navurl + "');" );
            }
        }

        protected override void OnClick( System.EventArgs e )
        {
            base.OnClick( e );
            string navurl = NavigateURL;
            if ( navurl != "" )
            {
                Page.Response.Redirect( NavigateURL );
            }
        }

        public string NavigateURL
        {
            get
            {
                object u = ViewState[ "NavigateURL" ];
                if ( u == null )
                {
                    return "";
                }
                else
                {
                    return ( string )u;
                }
            }
            set { ViewState[ "NavigateURL" ] = value; }
        }
    }

    public class RolloverLink : Button
    {
        protected override void AddAttributesToRender( System.Web.UI.HtmlTextWriter writer )
        {
            base.AddAttributesToRender( writer );
            writer.AddAttribute( "onmouseover", "this.className='button'" );
            writer.AddAttribute( "onmouseout", "this.className='button'" );
            writer.AddAttribute( "class", "button" );

            string navurl = NavigateURL;
            if ( ( base.OnClientClick == "" & navurl != "" ) )
            {
                writer.AddAttribute( "onclick", "window.navigate('" + navurl + "');" );
            }
        }

        protected override void OnClick( System.EventArgs e )
        {
            base.OnClick( e );
            string navurl = NavigateURL;
            if ( navurl != "" )
            {
                Page.Response.Redirect( NavigateURL );
            }
        }

        public string NavigateURL
        {
            get
            {
                object u = ViewState[ "NavigateURL" ];
                if ( u == null )
                {
                    return "";
                }
                else
                {
                    return ( string )u;
                }
            }
            set { ViewState[ "NavigateURL" ] = value; }
        }
    }

}

