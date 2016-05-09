<%@ Page
    Language="C#"
    MasterPageFile="~/PageTest/MasterPageTest.master"
    AutoEventWireup="true"
    CodeFile="PageTestAlwaysVisibleMenu.aspx.cs"
    Inherits="PageTest_AlwaysVisibleMenu"
    Title="AlwaysVisibleControl Sample"
    Theme="Sodevlog" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<ajaxToolkit:ToolkitScriptManager runat="server" EnablePartialRendering="true" ID="ScriptManager1" />
    <div class="demoarea">
        <div class="demoheading">AlwaysVisibleControl Demonstration</div>
        <asp:UpdatePanel runat="server" ID="UpdatePanel1" >
            <ContentTemplate>
                    <asp:Panel ID="PanelAlwaysVisible" runat="server" Width="250px" 
                        BackColor="White" 
                        ForeColor="DarkBlue"
                        BorderWidth="2" 
                        BorderStyle="solid" 
                        BorderColor="DarkBlue" 
                        style="z-index: 1;">
                            <asp:Button ID="ButtonAlwaysVisible" runat="server" 
                            OnClick="ButtonAlwaysVisible_Click" Text="Ok" />
                    </asp:Panel>
                    <ajaxToolkit:AlwaysVisibleControlExtender ID="avce" runat="server"
                        TargetControlID="PanelAlwaysVisible"
                        VerticalSide="Top"
                        VerticalOffset="10"
                        HorizontalSide="Right"
                        HorizontalOffset="10"
                        ScrollEffectDuration=".1" />
                </div>
                <p>
                    Choose a position for the clock from the list below. Scroll your browser window to see
                    the control maintain its always-visible position.  
                </p>
                <p>
                    Position: <asp:DropDownList ID="DropDownListPosition" runat="server" 
                        AutoPostBack="true" OnSelectedIndexChanged="DropDownListPosition_SelectedIndexChanged">
                        <asp:ListItem Text="Defaut" Selected="true"  Value="None" />
                        <asp:ListItem Text="Haut Gauche" Value="HG" />
                        <asp:ListItem Text="Haut Centre" Value="HC" />
                        <asp:ListItem Text="Haut Droite"  Value="HD" />
                        <asp:ListItem Text="Milieu Gauche" Value="MG" />
                        <asp:ListItem Text="Milieu Center" Value="MC" />
                        <asp:ListItem Text="Milieu Droite"  Value="MD" />
                        <asp:ListItem Text="Bas Gauche" Value="BG" />
                        <asp:ListItem Text="Bas Centre" Value="BC" />
                        <asp:ListItem Text="Bas Droite" Value="BD" />
                    </asp:DropDownList>
                </p>
            </ContentTemplate>
         </asp:UpdatePanel>
         
    </div>
    <div class="demobottom"></div>
    
    <asp:Panel ID="description_HeaderPanel" runat="server" style="cursor: pointer;">
        <div class="heading">
            <asp:ImageButton ID="Description_ToggleImage" runat="server" ImageUrl="~/images/collapse.jpg" AlternateText="collapse" />
            AlwaysVisibleControl Description
        </div>
    </asp:Panel>
    <asp:Panel id="description_ContentPanel" runat="server" style="overflow:hidden;">
        <p>    
            The AlwaysVisibleControl is a simple extender allowing you to pin controls to the page so that they
            appear to float over the background body content when it is scrolled or resized.  It targets any ASP.NET
            control and always keeps the position a specified distance from the desired horizontal and vertical sides.
        </p>
        <br />
        <p>
            To avoid having the control flash and move when the page loads, it is recommended that you absolutely
            position the control in the desired location in addition to attaching the extender.
        </p>
    </asp:Panel>

    <asp:Panel ID="Properties_HeaderPanel" runat="server" style="cursor: pointer;">
        <div class="heading">
            <asp:ImageButton ID="Properties_ToggleImage" runat="server" ImageUrl="~/images/expand.jpg" AlternateText="expand" />
            AlwaysVisibleControl Properties
        </div>
    </asp:Panel>
    <asp:Panel id="Properties_ContentPanel" runat="server" style="overflow:hidden;" Height="0px">
        The always visible extender has been initialized with these properties. The properties
        in <em>italics</em> are optional.<br /><br />
<pre>&lt;ajaxToolkit:AlwaysVisibleControlExtender ID="ace" runat="server"
    TargetControlID="timer"         
    <em>VerticalSide</em>="Top"
    <em>VerticalOffset</em>="10"
    <em>HorizontalSide</em>="Right"
    <em>HorizontalOffset</em>="10"
    <em>ScrollEffectDuration</em>=".1"/&gt;</pre>
        <ul>
            <li><strong>TargetControlID</strong> - ID of control for this extender to always make visible</li>
            <li><strong>HorizontalOffset</strong> - Distance to the HorizontalSide edge of the browser in pixels from the same side of the target control.  The default is 0 pixels.</li>
            <li><strong>HorizontalSide</strong> - Horizontal edge of the browser (either Left, Center, or Right) used to anchor the target control.  The default is Left.</li>
            <li><strong>VerticalOffset</strong> - Distance to the VerticalSide edge of the browser in pixels from the same side of the target control.  The default is 0 pixels.</li>
            <li><strong>VerticalSide</strong> - Vertical edge of the browser (either Top, Middle, or Bottom) used to anchor the target control.  The default is Top.</li>
            <li><strong>ScrollEffectDuration</strong> - Length in seconds of the scrolling effect to last when the target control is repositioned.  The default is .1 second.</li>
        </ul>
    </asp:Panel>

    <asp:Panel ID="ScrollPadding_HeaderPanel" runat="server" style="cursor: pointer;">
        <div class="heading">
            <asp:ImageButton ID="ScrollPadding_ToggleImage" runat="server" ImageUrl="~/images/expand.jpg" AlternateText="expand" /> Additional Text For Scrolling
        </div>
    </asp:Panel>

    <ajaxToolkit:CollapsiblePanelExtender ID="DescriptionCPE" runat="Server"
        TargetControlID="description_ContentPanel"
        ExpandControlID="description_HeaderPanel"
        CollapseControlID="description_HeaderPanel"
        Collapsed="False"
        ImageControlID="description_ToggleImage" />
    <ajaxToolkit:CollapsiblePanelExtender ID="PropertiesCPE" runat="Server"
        TargetControlID="Properties_ContentPanel"
        ExpandControlID="Properties_HeaderPanel"
        CollapseControlID="Properties_HeaderPanel"
        Collapsed="True" 
        ImageControlID="Properties_ToggleImage" />
        
        
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        Du texte en bas pour scroller
        <br />
        <br />
        <br />
        <br />
        
</asp:Content>