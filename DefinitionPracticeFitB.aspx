<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DefinitionPracticeFitB.aspx.cs" Inherits="WordBank.DefinitionPracticeFitB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    Fill in the blank with the correct word that fits this definition.
    <br />
    <br />
    Definition:
    <asp:Label ID="Definitionlbl" runat="server" Text="Label" Font-Bold="true"></asp:Label>
    <br />
    <br />
    <asp:Button ID="SentenceHintBtn" runat="server" OnClick="SentenceHintBtn_Click" Text="Sentence Hint" CssClass="btn btn-primary btn-sm" />
    <br />
    <asp:Label ID="SentenceHintLbl" runat="server" Visible="False"></asp:Label>
    <br />
    <br />
    <asp:Button ID="LetterHintBtn" runat="server" OnClick="LetterHintBtn_Click" Text="Letter Hint" CssClass="btn btn-primary btn-sm" />
    <br />
    <asp:Label ID="LetterHintLbl" runat="server" Visible="False"></asp:Label>
    <br />
    <br />
    <asp:TextBox runat="server" ID="SubmittedAnswer" CssClass="form-control" />
    <br />
    <asp:Button ID="SubmitBtn" runat="server" Text="Submit Answer" OnClick="SubmitBtn_Click" CssClass="btn btn-primary" />
    <br />
    <br />
    <asp:Label ID="Responselbl" runat="server"></asp:Label>
</asp:Content>
