<%@ Page Title="Add Student" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddStudents.aspx.cs" Inherits="WebformDemo.AddStudents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .form-container {
            max-width: 700px;
            margin: 30px auto;
            padding: 25px;
            background-color: #f8f9fa;
            border-radius: 10px;
            box-shadow: 0 0 15px rgba(0,0,0,0.1);
            font-family: Arial, sans-serif;
        }
        .form-container h2 { margin-bottom: 25px; text-align: center; color: #333; }
        .form-group { margin-bottom: 15px; }
        .form-group label { font-weight: bold; }
        .form-group input, .form-group select { width: 100%; padding: 8px 10px; margin-top: 5px; border-radius: 5px; border: 1px solid #ccc; }
        .courses-section { margin-top: 20px; }
        .courses-section h4 { margin-bottom: 10px; color: #444; }
        .teacher-dropdown { margin-top: 5px; }
        .btn-submit { margin-top: 20px; width: 100%; }
        .text-success { color: green; font-weight: bold; margin-bottom: 15px; display: block; text-align: center; }
    </style>

    <div class="form-container">

        <!-- Success Message -->
        <asp:Label ID="lblSuccess" runat="server" CssClass="text-success" />

        <asp:FormView ID="fvStudent" runat="server" DefaultMode="Insert"
            ItemType="WebformDemo.Models.Student"
            InsertMethod="InsertStudent"
            OnItemInserted="fvStudent_ItemInserted">

            <InsertItemTemplate>

                <h2>Add New Student</h2>

                <div class="form-group">
                    <label for="TxtName">Name</label>
                    <asp:TextBox ID="TxtName" runat="server" CssClass="form-control" Text='<%# Bind("Name") %>' />
                    <asp:RequiredFieldValidator ID="rfvName" runat="server"
                        ControlToValidate="TxtName" ErrorMessage="Name is required" ForeColor="Red" Display="Dynamic" />
                </div>

                <div class="form-group">
                    <label for="TxtRollNo">Roll No</label>
                    <asp:TextBox ID="TxtRollNo" runat="server" CssClass="form-control" Text='<%# Bind("RollNo") %>' />
                    <asp:RequiredFieldValidator ID="rfvRollNo" runat="server"
                        ControlToValidate="TxtRollNo" ErrorMessage="Roll No is required" ForeColor="Red" Display="Dynamic" />
                </div>

                <div class="form-group">
                    <label for="TxtClass">Class</label>
                    <asp:TextBox ID="TxtClass" runat="server" CssClass="form-control" Text='<%# Bind("Class") %>' />
                    <asp:RequiredFieldValidator ID="rfvClass" runat="server"
                        ControlToValidate="TxtClass" ErrorMessage="Class is required" ForeColor="Red" Display="Dynamic" />
                </div>

                <div class="form-group">
                    <label for="TxtSection">Section</label>
                    <asp:TextBox ID="TxtSection" runat="server" CssClass="form-control" Text='<%# Bind("Section") %>' />
                    <asp:RequiredFieldValidator ID="rfvSection" runat="server"
                        ControlToValidate="TxtSection" ErrorMessage="Section is required" ForeColor="Red" Display="Dynamic" />
                </div>

                <div class="form-group">
                    <label for="TxtEmail">Email</label>
                    <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control" Text='<%# Bind("Email") %>' />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                        ControlToValidate="TxtEmail" ErrorMessage="Email is required" ForeColor="Red" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server"
                        ControlToValidate="TxtEmail"
                        ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"
                        ErrorMessage="Invalid Email"
                        ForeColor="Red" Display="Dynamic" />
                </div>

                <!-- Courses & Teachers -->
                <div class="courses-section">
                    <h4>Select Courses & Assign Teachers</h4>

                    <asp:CheckBoxList ID="chkCourses" runat="server"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="chkCourses_SelectedIndexChanged"
                        DataTextField="CourseName"
                        DataValueField="CourseID"
                        SelectMethod="GetCourses"
                        RepeatDirection="Vertical"
                        CssClass="form-check" />

                    <asp:Repeater ID="rptCourseTeachers" runat="server" OnItemDataBound="rptCourseTeachers_ItemDataBound">
                        <ItemTemplate>
                            <div class="form-group teacher-dropdown">
                                <asp:HiddenField ID="hfCourseID" runat="server" Value='<%# Eval("CourseID") %>' />
                                <label><%# Eval("CourseName") %> Teacher</label>
                                <asp:DropDownList ID="ddlTeachers" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="Add Student"
                    CssClass="btn btn-primary btn-submit" CausesValidation="true" />

            </InsertItemTemplate>

        </asp:FormView>

    </div>

</asp:Content>
