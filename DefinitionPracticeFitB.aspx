<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DefinitionPracticeFitB.aspx.cs" Inherits="WordBank.DefinitionPracticeFitB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="SubmitBtn">
        <br />
        Enter the correct word/phrase that fits this definition.
        &nbsp;<asp:Label ID="ResortLbl" runat="server" Text="Label" ForeColor="#999999"></asp:Label>
        <br />
        <br />
        Definition:
        <asp:Label ID="Definitionlbl" runat="server" Text="Label" Font-Bold="true"></asp:Label>
        <br /> <br />
        <asp:Button ID="SentenceHintBtn" runat="server" OnClick="SentenceHintBtn_Click" Text="Sentence Hint" CssClass="btn btn-primary btn-sm" Width="100px" />
        &nbsp;
        <asp:Label ID="SentenceHintLbl" runat="server" Visible="False"></asp:Label>
        <br /> <br />
        <asp:Button ID="LetterHintBtn" runat="server" OnClick="LetterHintBtn_Click" Text="Letter Hint" CssClass="btn btn-primary btn-sm" Width="100px" />
        &nbsp;
        <asp:Label ID="LetterHintLbl" runat="server" Visible="False"></asp:Label>
        <br /> <br />
        <asp:Label ID="Label1" runat="server">Answer: </asp:Label>
        <asp:TextBox runat="server" ID="SubmittedAnswer" CssClass="form-control" />
        <br />
        <asp:Button ID="SubmitBtn" runat="server" Text="Submit Answer" OnClick="SubmitBtn_Click" CssClass="btn btn-primary" />
        &nbsp; &nbsp;
        <asp:Label ID="LabelNumTotal" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Responselbl" runat="server"></asp:Label>
    </asp:Panel>
</asp:Content>
