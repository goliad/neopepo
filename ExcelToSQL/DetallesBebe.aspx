<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetallesBebe.aspx.cs" Inherits="ExcelToSQL.Formulario_web11" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <table >
            <tr>
                <td></td>
                <td><asp:TextBox ID="TextBoxPeso" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td></td>
                <td><asp:TextBox ID="TextBoxfechaNac" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
        <asp:CheckBox ID="CheckBox1" runat="server" />
        <asp:CheckBox ID="CheckBox2" runat="server" />
        <asp:GridView ID="GridViewEstablecimientos" runat="server">
        </asp:GridView>
</asp:Content>
