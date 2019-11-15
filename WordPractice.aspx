<%@ Page Title="Word Practice" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WordPractice.aspx.cs" Inherits="WordBank.WordPractice" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    Please select the correct definition:
    <br />
    
    <br />
    Word:
    <asp:Label ID="Wordlbl" runat="server" Text="Label" Font-Bold="true"></asp:Label>
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

    <asp:Button ID="SubmitBtn" runat="server" Text="Submit Answer" OnClick="SubmitBtn_Click"  CssClass="btn btn-primary btn-sm"/>


    <br />


    <br />


    <asp:Label ID="Responselbl" runat="server"></asp:Label>
</asp:Content>
