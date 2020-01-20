<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="WordBank.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>

    <div class="form-horizontal">
        <h4>Create a new account</h4>
        <hr />

        <p class="text-danger">
            <asp:Literal runat="server" ID="ErrorMessage" />
        </p>
        <p class="text-success">
            <asp:Literal runat="server" ID="SuccessMessage" />
        </p>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Username" CssClass="col-md-2 control-label">Username</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Username" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ID="reqName" ControlToValidate="Username" CssClass="text-danger" ErrorMessage="The username field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Password</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                    CssClass="text-danger" ErrorMessage="The password field is required." />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                    CssClass="text-danger"
                    ErrorMessage="Password must contain at least 8 characters, 1 digit, 1 special character"
                    ValidationExpression="^.*(?=.{8,})(?=.*[\d])(?=.*[\W]).*$"
                    ControlToValidate="Password" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="CreateUser_Click" Text="Register" CssClass="btn btn-default" />
            </div>
        </div>
    </div>
</asp:Content>
