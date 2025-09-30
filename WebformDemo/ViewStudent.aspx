<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewStudent.aspx.cs" Inherits="WebformDemo.ViewStudent" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Students</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:GridView ID="GridView1" runat="server"
            AutoGenerateColumns="false"
            AllowPaging="true" PageSize="5"
            OnRowEditing="GridView1_RowEditing"
            OnRowCancelingEdit="GridView1_RowCancelingEdit"
            OnRowUpdating="GridView1_RowUpdating"
            OnRowDeleting="GridView1_RowDeleting"
            OnPageIndexChanging="GridView1_PageIndexChanging"
            DataKeyNames="StudentID"
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged">

            <Columns>
               
                <asp:BoundField DataField="StudentID" HeaderText="ID" ReadOnly="true" />

                
                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate><%# Eval("Name") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                
                <asp:TemplateField HeaderText="Roll No">
                    <ItemTemplate><%# Eval("RollNo") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRollNo" runat="server" Text='<%# Bind("RollNo") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Class">
                    <ItemTemplate><%# Eval("Class") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtClass" runat="server" Text='<%# Bind("Class") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                
                <asp:TemplateField HeaderText="Section">
                    <ItemTemplate><%# Eval("Section") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSection" runat="server" Text='<%# Bind("Section") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

               
                <asp:TemplateField HeaderText="Email">
                    <ItemTemplate><%# Eval("Email") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                
                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ShowSelectButton="true" />
            </Columns>
        </asp:GridView>
        <p>
            <asp:Button ID="AddStudent" runat="server" Text="AddStudent" OnClick="Add_Click"  />
        </p>
    </form>
</body>
</html>
