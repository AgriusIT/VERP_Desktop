﻿'25-July-2017 TFS# 1045 : Waqar Raza : Add new form to save update and delete records through this form.
Imports SBModel
Imports SBDal

Public Class frmUserWiseCustomer
    Implements IGeneral
    Dim objModel As UserWiseCustomerBE
    Dim obDdal As UserWiseCustomerDAL
    Dim list As List(Of UserWiseCustomerBE)
    Dim objDal As UserWiseCustomerDAL
    ''' <summary>
    ''' Waqar Raza: Apply grid seeting to hide columns containing Id's and also to add a new column of Action to remove the selected row
    ''' </summary>
    ''' <param name="Condition"></param>
    ''' <remarks>25-July-2017 TFS# 1045 : Waqar Raza</remarks>
    Public Sub ApplyGridSettings(Optional Condition As String = "") Implements IGeneral.ApplyGridSettings
        Try

            Me.grd.RootTable.Columns("UWCID").Visible = False
            Me.grd.RootTable.Columns("UserID").Visible = False
            Me.grd.RootTable.Columns("CustomerID").Visible = False
            If Me.grd.RootTable.Columns.Contains("Delete") = False Then
                Me.grd.RootTable.Columns.Add("Delete")
                Me.grd.RootTable.Columns("Delete").ButtonDisplayMode = Janus.Windows.GridEX.CellButtonDisplayMode.Always
                Me.grd.RootTable.Columns("Delete").ButtonStyle = Janus.Windows.GridEX.ButtonStyle.ButtonCell
                Me.grd.RootTable.Columns("Delete").ButtonText = "Delete"
                Me.grd.RootTable.Columns("Delete").Key = "Delete"
                Me.grd.RootTable.Columns("Delete").Width = 75
                Me.grd.RootTable.Columns("Delete").Caption = "Action"
                Me.grd.RootTable.Columns("UserName").Caption = "User Name"
                Me.grd.RootTable.Columns("UserName").Width = 200
                Me.grd.RootTable.Columns("CustomerName").Caption = "Customer Name"
                Me.grd.RootTable.Columns("CustomerName").Width = 200
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    '25-July-2017 TFS# 1045 : Waqar Raza : End Apply grid settings
    ''' <summary>
    ''' Waqar Raza : Apply Security for Standard User to show some specific buttons that he/she have rights to use at this form.
    ''' </summary>
    ''' <param name="Mode"></param>
    ''' <param name="Condition"></param>
    ''' <remarks>25-July-2017 TFS# 1045 : Waqar Raza</remarks>
    Public Sub ApplySecurity(Mode As SBUtility.Utility.EnumDataMode, Optional Condition As String = "") Implements IGeneral.ApplySecurity
        Try
            If LoginGroup = "Administrator" Then
                Me.Visible = True
                Me.btnSave.Enabled = True
                Me.btnNew.Enabled = True
                CtrlGrdBar1.mGridPrint.Enabled = True
                CtrlGrdBar1.mGridExport.Enabled = True
                CtrlGrdBar1.mGridChooseFielder.Enabled = True
                Exit Sub
            End If
            Me.Visible = False
            Me.btnSave.Enabled = False
            CtrlGrdBar1.mGridPrint.Enabled = False
            CtrlGrdBar1.mGridExport.Enabled = False
            CtrlGrdBar1.mGridChooseFielder.Enabled = False
            
            For i As Integer = 0 To Rights.Count - 1
                If Rights.Item(i).FormControlName = "View" Then
                    Me.Visible = True
                ElseIf Rights.Item(i).FormControlName = "Save" Then
                    If Me.btnSave.Text = "&Save" Then btnSave.Enabled = True
                ElseIf Rights.Item(i).FormControlName = "Update" Then
                    If Me.btnSave.Text = "&Update" Then btnSave.Enabled = True
                ElseIf Rights.Item(i).FormControlName = "Grid Print" Then
                    CtrlGrdBar1.mGridPrint.Enabled = True
                ElseIf Rights.Item(i).FormControlName = "Export" Then
                    CtrlGrdBar1.mGridExport.Enabled = True
                ElseIf Rights.Item(i).FormControlName = "Field Chooser" Then
                    CtrlGrdBar1.mGridChooseFielder.Enabled = True
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    '25-July-2017 TFS# 1045 : Waqar Raza: End Apply Security
    Public Function Delete(Optional Condition As String = "") As Boolean Implements IGeneral.Delete

    End Function
    ''' <summary>
    ''' Waqar Raza : FillCombos of items here.
    ''' </summary>
    ''' <param name="Condition"></param>
    ''' <remarks>25-July-2017 TFS# 1045 : Waqar Raza:</remarks>
    Public Sub FillCombos(Optional Condition As String = "") Implements IGeneral.FillCombos
        Dim str As String = ""
        If Condition = "User" Then
            str = "Select User_ID, FullName from tblUser where block = 0"
            FillUltraDropDown(Me.cmbUser, str, True)
            Me.cmbUser.DisplayLayout.Bands(0).Columns("User_ID").Hidden = True
            Me.cmbUser.DisplayLayout.Bands(0).Columns("FullName").Width = 200
        ElseIf Condition = "Customer" Then
            str = "SELECT   coa_detail_id as id, detail_title AS Name, detail_code AS Code, Contact_Mobile AS Mobile FROM vwCOADetail WHERE (account_type = 'Customer') AND (coa_detail_id IS NOT NULL) AND (Active = 1) ORDER BY Name  "
            FillUltraDropDown(Me.cmbCustomer, str, True)
            Me.cmbCustomer.DisplayLayout.Bands(0).Columns("id").Hidden = True
            Me.cmbCustomer.DisplayLayout.Bands(0).Columns("Name").Width = 200
        End If
    End Sub
    '25-July-2017 TFS# 1045 : Waqar Raza: End Fill Combos
    ''' <summary>
    ''' Waqar Raza: FillModel to fill the values of Master and Detail records for further use
    ''' </summary>
    ''' <param name="Condition"></param>
    ''' <remarks>25-July-2017 TFS# 1045 : Waqar Raza</remarks>
    Public Sub FillModel(Optional Condition As String = "") Implements IGeneral.FillModel
        Try
            list = New List(Of UserWiseCustomerBE)
            For Each Row As Janus.Windows.GridEX.GridEXRow In Me.grd.GetDataRows
                Dim Detail As New UserWiseCustomerBE
                Detail.UWCID = Val(Row.Cells("UWCId").Value.ToString)
                Detail.UserId = Val(Row.Cells("UserId").Value.ToString)
                Detail.UserName = Row.Cells("UserName").Value.ToString
                Detail.CustomerId = Val(Row.Cells("CustomerId").Value.ToString)
                Detail.CustomerName = Row.Cells("CustomerName").Value.ToString
                list.Add(Detail)
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    '25-July-2017 TFS# 1045 : Waqar Raza: End Fill Model

    Public Sub GetAllRecords(Optional Condition As String = "") Implements IGeneral.GetAllRecords

    End Sub
    ''' <summary>
    ''' Waqar Raza: Validate that Item is selected in the drop down
    ''' </summary>
    ''' <param name="Mode"></param>
    ''' <param name="Condition"></param>
    ''' <returns></returns>
    ''' <remarks>25-July-2017 TFS# 1045 : Waqar Raza</remarks>
    Public Function IsValidate(Optional Mode As SBUtility.Utility.EnumDataMode = SBUtility.Utility.EnumDataMode.Disabled, Optional Condition As String = "") As Boolean Implements IGeneral.IsValidate
        Try
            If Not Me.cmbUser.Value > 0 Then
                msg_Error("Please select any User")
                Me.cmbUser.Focus()
                Return False
            ElseIf Not Me.cmbCustomer.Value > 0 Then
                msg_Error("Please select any Customer")
                Me.cmbCustomer.Focus()
                Return False
            End If
            FillModel()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    '25-July-2017 TFS# 1045 : Waqar Raza: End Is Validate
    ''' <summary>
    ''' Waqar Raza : ResetControls to initial stage from where start
    ''' </summary>
    ''' <param name="Condition"></param>
    ''' <remarks>25-July-2017 TFS# 1045 : Waqar Raza</remarks>
    Public Sub ReSetControls(Optional Condition As String = "") Implements IGeneral.ReSetControls
        Try
            FillCombos("User")
            Me.cmbUser.Value = 0
            FillCombos("Customer")
            Me.cmbCustomer.Value = 0
            DisplayDetail(-1)
            Me.cmbUser.Focus()
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    '25-July-2017 TFS# 1045 : Waqar Raza : End Reset Controls
    ''' <summary>
    ''' Waqar Raza: Calls the save from DAL and also checked the validity before save.
    ''' </summary>
    ''' <param name="Condition"></param>
    ''' <returns></returns>
    ''' <remarks>25-July-2017 TFS# 1045 : Waqar Raza</remarks>
    Public Function Save(Optional Condition As String = "") As Boolean Implements IGeneral.Save
        Try
            If IsValidate() = True Then
                objDal = New UserWiseCustomerDAL
                FillModel()
                objDal.Save(list)
                SaveActivityLog("Config", Me.Text, EnumActions.Save, LoginUserId, EnumRecordType.Configuration, Me.cmbUser.Value, True)
                Return True
            End If
            Return False
            Catch ex As Exception
            Throw ex
        End Try
    End Function
    '25-July-2017 TFS# 1045 : Waqar Raza : End Save
    Public Sub SetButtonImages() Implements IGeneral.SetButtonImages

    End Sub

    Public Sub SetConfigurationBaseSetting() Implements IGeneral.SetConfigurationBaseSetting

    End Sub

    Public Sub SetNavigationButtons(Mode As SBUtility.Utility.EnumDataMode, Optional Condition As String = "") Implements IGeneral.SetNavigationButtons

    End Sub
    ''' <summary>
    ''' Waqar Raza : Calls the Update from DAL and also checked the validity before update.
    ''' </summary>
    ''' <param name="Condition"></param>
    ''' <returns></returns>
    ''' <remarks>25-July-2017 TFS# 1045 : Waqar Raza</remarks>
    Public Function Update1(Optional Condition As String = "") As Boolean Implements IGeneral.Update
        Try
            Try
                If IsValidate() = True Then
                    objDal = New UserWiseCustomerDAL
                    FillModel()
                    objDal.Update(list)
                    SaveActivityLog("Config", Me.Text, EnumActions.Update, LoginUserId, EnumRecordType.Configuration, Me.cmbUser.Value, True)
                    Return True
                End If
                Return False
            Catch ex As Exception
                Throw ex
            End Try
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Function
    '25-July-2017 TFS# 1045 : Waqar Raza : End Update1

    '25-July-2017 TFS# 1045 : Waqar Raza : Add Form Key Down to add short keys for Save, Refresh and New
    Private Sub frmUserWiseCustomer_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Try
            If e.KeyCode = Keys.F4 Then
                If Me.btnSave.Enabled = True Then
                    Me.btnSave_Click(Nothing, Nothing)
                End If
            End If
            If e.KeyCode = Keys.Escape Then
                Me.btnNew_Click(Nothing, Nothing)
            End If
            If e.KeyCode = Keys.F5 Then
                Me.btnRefresh_Click(Nothing, Nothing)
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    '25-July-2017 TFS# 1045 : Waqar Raza : End KeyDown For Short Keys

    ''' <summary>
    ''' Waqar Raza: DisplayRecords to get the data from tblUserWiseCustomerList for selected Item in drop down
    ''' </summary>
    ''' <param name="Id"></param>
    ''' <remarks>25-July-2017 TFS# 1045 : Waqar Raza</remarks>
    Public Sub DisplayDetail(ByVal Id As Integer)
        Try
            Dim str As String = ""
            Dim dt As DataTable
            str = "select * from tblUserWiseCustomerList where UserId = " & Id & ""
            dt = GetDataTable(str)
            Me.grd.DataSource = dt
            Me.grd.RetrieveStructure()
            ApplyGridSettings()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    '25-July-2017 TFS# 1045 : Waqar Raza : End Display Detail

    ''25-July-2017 TFS# 1045 : Waqar Raza : To put value of selected combo box in grid 
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            If IsValidate() = True Then
                Dim dt As DataTable
                dt = CType(Me.grd.DataSource, DataTable)
                Dim dr As DataRow
                '25-July-2017 TFS# 1045 : Waqar Raza :Applying For loop to retrict adding same record in grid. 
                For Each row As Janus.Windows.GridEX.GridEXRow In Me.grd.GetDataRows
                    If Val(row.Cells("CustomerId").Value) = Me.cmbCustomer.Value Then
                        msg_Error("Selected Customer Exists Already")
                        Exit Sub
                    End If
                Next
                ''25-July-2017 TFS# 1045 : Waqar Raza :End Loop
                dr = dt.NewRow
                dr("UWCID") = 0
                dr("UserId") = Me.cmbUser.Value
                dr("UserName") = Me.cmbUser.Text
                dr("CustomerId") = Me.cmbCustomer.Value
                dr("CustomerName") = Me.cmbCustomer.Text
                dt.Rows.Add(dr)
             End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    '25-July-2017 TFS# 1045 : Waqar Raza :End Adding Record in Grid

    Private Sub frmUserWiseCustomer_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            ApplySecurity(SBUtility.Utility.EnumDataMode.New)
            ReSetControls()
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Try
            ReSetControls()
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    '25-July-2017 TFS# 1045 : Waqar Raza : Add to refresh the selected value of combobox.
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Try
            Dim id As Integer
            id = Me.cmbUser.Value
            FillCombos("User")
            Me.cmbUser.Value = id

            id = Me.cmbCustomer.Value
            FillCombos("Customer")
            Me.cmbCustomer.Value = id
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    '25-July-2017 TFS# 1045 : Waqar Raza : End Refresh Click

    '25-July-2017 TFS# 1045 : Waqar Raza :To Save the grid record against selected combo box
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            Me.lblProgress.Text = "Processing Please Wait ..."
            Me.lblProgress.Visible = True
            Application.DoEvents()
            Me.Cursor = Cursors.WaitCursor
            FillModel()
            If Me.btnSave.Text = "&Save" Then
                '25-July-2017 TFS# 1045 : Waqar Raza :Calling save function to save grid data 
                If Save() = True Then
                    ReSetControls()
                    '25-July-2017 TFS# 1045 : Waqar Raza : To Display nothing in Grid
                    DisplayDetail(-1)
                    msg_Information(str_informSave)
                End If
            Else
                If Not msg_Confirm(str_ConfirmUpdate) = True Then Exit Sub
                '25-July-2017 TFS# 1045 : Waqar Raza :Calling Update1 function to save grid data 
                If Update1() = True Then
                    ReSetControls()
                    '25-July-2017 TFS# 1045 : Waqar Raza : To Display nothing in Grid
                    DisplayDetail(-1)
                    msg_Information(str_informUpdate)
                End If
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
            Me.lblProgress.Visible = False
        End Try
    End Sub
    '25-July-2017 TFS# 1045 : Waqar Raza : End Save 

    ''25-July-2017 TFS# 1045 : Waqar Raza : To change the detail record on seelction of User ComboBox
    Private Sub cmbUser_ValueChanged(sender As Object, e As EventArgs) Handles cmbUser.ValueChanged
        Try
            '25-July-2017 TFS# 1045 : Waqar Raza : It will show Display Detail using seleted combo box id
            DisplayDetail(cmbUser.Value)
            If Me.cmbUser.Value = 0 Or Me.grd.RowCount = 0 Then
                Me.btnSave.Text = "&Save"
            Else
                Me.btnSave.Text = "&Update"
            End If
            ApplySecurity(SBUtility.Utility.EnumDataMode.New)
            FillModel()
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    '25-July-2017 TFS# 1045 : Waqar Raza : End User ComboBox Event

    '25-July-2017 TFS# 1045 : Waqar Raza : To delete Each Row of Grid using its delete button in Grid
    Private Sub grd_ColumnButtonClick(sender As Object, e As Janus.Windows.GridEX.ColumnActionEventArgs) Handles grd.ColumnButtonClick
        Try
            objDal = New UserWiseCustomerDAL
            If e.Column.Key = "Delete" Then
                If Not msg_Confirm(str_ConfirmDelete) = True Then Exit Sub
                '25-July-2017 TFS# 1045 : Waqar Raza : Calling the Function from DAL to Delte detail Record against ID
                objDal.DeleteDetail(Val(Me.grd.GetRow.Cells("UWCID").Value.ToString))
                Me.grd.GetRow.Delete()
                SaveActivityLog("Config", Me.Text, EnumActions.Delete, LoginUserId, EnumRecordType.Configuration, Me.cmbUser.Value, True)
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    '25-July-2017 TFS# 1045 : Waqar Raza : End Grid Delete Button Click Event
End Class
