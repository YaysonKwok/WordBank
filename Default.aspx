<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WordBank._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron container-fluid" style="padding: 0px 20px 0px 20px;">
        <h2 class="display-4">Word Bank</h2>
        <hr class="my-5">
        <p class="lead">WordBank is a free website that helps you retain your new vocabulary. It has the following features:</p>
        <p>
            1. Input new words, definitions, and usage sentences.
            <br />
            2. Review through multiple-choice quizzes given words.
            <br />
            3. Review through multiple-choice quizzes given definitions.
            <br />
            4. Review troubled words more frequently through a configurable computer algorithm.
            <br />
            5. Import new words, definitions, and usage sentences from a csv file.
            <br />
            6. Export your new vocabulary to a csv file.
        </p>
    </div>
    <p>
        <a class="btn btn-primary btn-lg" data-toggle="collapse" href="#collapse" role="button" aria-expanded="false" aria-controls="collapse">Want to test out your knowledge of English vocabulary? Click Here!</a>
    </p>
    <div class="row">
        <div class="collapse" id="collapse">
            <div class="card card-body">
                <div class="col-md-8">
                    Word:
                    <asp:Label ID="Wordlbl" runat="server" Text="Label" Font-Bold="true"></asp:Label>
                    <br />
                    <br />
                    <asp:RadioButtonList ID="AnswerList" runat="server" CssClass="form-check-input">
                    </asp:RadioButtonList>
                    <br />
                    <asp:Button ID="SubmitBtn" runat="server" Text="Submit Answer" OnClick="SubmitBtn_Click" CssClass="btn btn-primary" />
                    <br />
                    <br />
                    <asp:Label ID="Responselbl" runat="server"></asp:Label>
                </div>
            </div>
    </div>
    </div>
</asp:Content>
