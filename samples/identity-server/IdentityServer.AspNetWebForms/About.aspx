<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="IdentityServer.AspNetWebForms.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Your application description page.</h3>
    <p>Use this area to provide additional information.</p>
    <h2>Secure Page, logged in as <%= User.Identity.Name %></h2>
    <ul>
    <% foreach (var claim in ((System.Security.Claims.ClaimsPrincipal)User).Claims){ %>
        <li><%: claim.Type + ", " + claim.Value %></li>
    <%} %>
    </ul>
</asp:Content>
