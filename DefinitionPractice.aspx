<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DefinitionPractice.aspx.cs" Inherits="WordBank.DefinitionPractice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <br />
    Please select the correct word given this definition:
    <br />
    <br />
    Definition:
    <asp:Label ID="Definitionlbl" runat="server" Text="Label" Font-Bold="true"></asp:Label>
    <br />
    <br />
    <asp:Button ID="HintBtn" runat="server" OnClick="HintBtn_Click" Text="Hint" CssClass="btn btn-primary btn-sm"/>
    <br />
    <asp:Label ID="HintLbl" runat="server" Visible="False"></asp:Label>
    <br />
    <br />
    <asp:RadioButtonList ID="AnswerList" runat="server" CssClass="form-check-input">
    </asp:RadioButtonList>
    <br />
    <asp:Button ID="SubmitBtn" runat="server" Text="Submit Answer" OnClick="SubmitBtn_Click" CssClass="btn btn-primary"/>
    <br />
    <br />
    <asp:Label ID="Responselbl" runat="server"></asp:Label>
</asp:Content>
