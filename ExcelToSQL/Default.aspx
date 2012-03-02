<%@ Page Title="Página principal" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ExcelToSQL._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:FileUpload ID="fupExcel" runat="server" />
    <asp:Button ID="ButtonImportar" runat="server" Text="Button" OnClick="ButtonImportar_Click" />
    <asp:Label ID="lblMessages" runat="server" Text="Label"></asp:Label>
    <asp:GridView ID="GridViewBebes" runat="server" 
        onrowcommand="GridViewBebes_RowCommand" >
        <Columns>
            <asp:TemplateField>            
                <ItemTemplate>
                <asp:LinkButton ID="LinkButtonDetalles" runat="server" CommandName="detalles" >Detalles</asp:LinkButton>
                <asp:LinkButton ID="LinkButtonDiagnostico" runat="server" CommandName="diagnostico" >Diagnostico</asp:LinkButton>
                </ItemTemplate>              
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </asp:Content>
