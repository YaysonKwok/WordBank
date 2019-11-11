<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WordBank._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Word Bank</h1>
        <p class="lead">Word Bank is a free online website dedicated to improving your vocabulary!</p>
    </div>

    <div class="row">
        <div class="col-md-8">
            <h2>Want to test out your knowledge of English vocabulary?</h2>
            <p>
                Try out a quiz here!
            </p>
    Word:
    <asp:Label ID="Wordlbl" runat="server" Text="Label" Font-Bold="true"></asp:Label>
    <br />
    <br />
    <asp:Button ID="HintBtn" runat="server" OnClick="HintBtn_Click" Text="Hint" />
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
        </div>
        <div class="col-md-4">
            <h2>Interested but not sold?</h2>
            <p>
                How about a test run of the site?
            </p>
            <p>
                <asp:Button runat="server" OnClick="Guest_Click" Text="Learn More" CssClass="btn btn-default" />
            </p>
        </div>
    </div>

</asp:Content>
