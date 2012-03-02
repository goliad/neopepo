<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExportarExcel.aspx.cs" Inherits="ExcelToSQL.Formulario_web1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        Please choose the Excel version:<br />
        <asp:RadioButtonList ID="rblExtension" runat="server">
            <asp:ListItem Selected="True" Value="2003">Excel 97-2003</asp:ListItem>
            <asp:ListItem Value="2007">Excel 2007</asp:ListItem>
        </asp:RadioButtonList>
        <br />
        Generate the download link when finished:
        <asp:RadioButtonList 
            ID="rblDownload" runat="server">
            <asp:ListItem Selected="True" Value="Yes">Yes</asp:ListItem>
            <asp:ListItem Value="No">No</asp:ListItem>
        </asp:RadioButtonList>
        <br />
        <asp:Button ID="btnExport" runat="server" Text="Export" 
            onclick="btnExport_Click" />
        <br />
        <br />
        <asp:HyperLink ID="hlDownload" runat="server"></asp:HyperLink>        
    </div>
</asp:Content>
