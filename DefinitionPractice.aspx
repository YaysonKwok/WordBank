<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DefinitionPractice.aspx.cs" Inherits="WordBank.DefinitionPractice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <br />
    Please select the correct word given this definition:
    <br />
    Definition:
    <asp:Label ID="Definitionlbl" runat="server" Text="Label"></asp:Label>
    <br />
    <asp:RadioButtonList ID="AnswerList" runat="server" CssClass="form-check-input">
    </asp:RadioButtonList>

    <asp:Button ID="SubmitBtn" runat="server" Text="Submit Answer" OnClick="SubmitBtn_Click" CssClass="btn btn-primary btn-lg btn-block"/>


    <br />


    <asp:Label ID="Responselbl" runat="server"></asp:Label>
</asp:Content>
