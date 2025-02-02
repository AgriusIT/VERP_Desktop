﻿Imports SBDal
Imports SBUtility
Public Class frmComplaintComparison
    Implements IGeneral
    Dim isFormLoaded As Boolean = False
    Dim _SearchDt As New DataTable
    Private Sub frmComplaintDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            FillCombos("party")
            FillCombos("product")
            FillCombos("complaintNo")
            Me.rdoComplaintAndRequest.Checked = True
            Me.rdoCode.Checked = True
            _SearchDt = CType(Me.lstItems.ListItem.DataSource, DataTable)
            _SearchDt.AcceptChanges()
            ApplySecurity(Utility.EnumDataMode.New)
            CtrlGrdBar1_Load(Nothing, Nothing)
            isFormLoaded = True
            Me.lstItems.DeSelect()
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Public Sub ApplyGridSettings(Optional Condition As String = "") Implements IGeneral.ApplyGridSettings
        If rdoComplaintAndRequest.Checked Then
            Me.GridResults.RootTable.Columns("ArticleId").Visible = False
            Me.GridResults.RootTable.Columns("ItemId").Visible = False
            Me.GridResults.RootTable.Columns("RequestPrice").TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            Me.GridResults.RootTable.Columns("RequestPrice").HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Far
            Me.GridResults.RootTable.Columns("RequestPrice").AggregateFunction = Janus.Windows.GridEX.AggregateFunction.Sum
            Me.GridResults.RootTable.Columns("RequestPrice").TotalFormatString = "N" & DecimalPointInValue
            Me.GridResults.RootTable.Columns("RequestPrice").TotalFormatString = "N" & TotalAmountRounding
            Me.GridResults.RootTable.Columns("RetrunPrice").TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            Me.GridResults.RootTable.Columns("RetrunPrice").HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Far
            Me.GridResults.RootTable.Columns("RetrunPrice").AggregateFunction = Janus.Windows.GridEX.AggregateFunction.Sum
            Me.GridResults.RootTable.Columns("RetrunPrice").TotalFormatString = "N" & DecimalPointInValue
            Me.GridResults.RootTable.Columns("RetrunPrice").TotalFormatString = "N" & TotalAmountRounding
            Me.GridResults.RootTable.Columns("TotalOutQty").TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            Me.GridResults.RootTable.Columns("TotalOutQty").HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Far
            Me.GridResults.RootTable.Columns("TotalOutQty").AggregateFunction = Janus.Windows.GridEX.AggregateFunction.Sum
            'Me.GridResults.RootTable.Columns("TotalOutQty").TotalFormatString = "N" & DecimalPointInValue
            'Me.GridResults.RootTable.Columns("TotalOutQty").TotalFormatString = "N" & TotalAmountRounding
            Me.GridResults.RootTable.Columns("TotalInQty").TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            Me.GridResults.RootTable.Columns("TotalInQty").HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Far
            Me.GridResults.RootTable.Columns("TotalInQty").AggregateFunction = Janus.Windows.GridEX.AggregateFunction.Sum
            'Me.GridResults.RootTable.Columns("TotalInQty").TotalFormatString = "N" & DecimalPointInValue
            'Me.GridResults.RootTable.Columns("TotalInQty").TotalFormatString = "N" & TotalAmountRounding
            Me.GridResults.RootTable.Columns("RequestPrice").FormatString = "N" & DecimalPointInValue
            Me.GridResults.RootTable.Columns("RequestPrice").FormatString = "N" & TotalAmountRounding
            Me.GridResults.RootTable.Columns("RetrunPrice").FormatString = "N" & DecimalPointInValue
            Me.GridResults.RootTable.Columns("RetrunPrice").FormatString = "N" & TotalAmountRounding
        Else
            Me.GridResults.RootTable.Columns("InQty").TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            Me.GridResults.RootTable.Columns("InQty").HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Far
            Me.GridResults.RootTable.Columns("InQty").AggregateFunction = Janus.Windows.GridEX.AggregateFunction.Sum
            Me.GridResults.RootTable.Columns("OutQty").TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            Me.GridResults.RootTable.Columns("OutQty").HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Far
            Me.GridResults.RootTable.Columns("OutQty").AggregateFunction = Janus.Windows.GridEX.AggregateFunction.Sum
            Me.GridResults.RootTable.Columns("BalanceQty").TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            Me.GridResults.RootTable.Columns("BalanceQty").HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Far
            Me.GridResults.RootTable.Columns("BalanceQty").AggregateFunction = Janus.Windows.GridEX.AggregateFunction.Sum
            'Me.GridResults.RootTable.Columns("Qty").TotalFormatString = "N" & DecimalPointInValue
            'Me.GridResults.RootTable.Columns("Qty").TotalFormatString = "N" & TotalAmountRounding
        End If
    End Sub

    Public Sub ApplySecurity(Mode As SBUtility.Utility.EnumDataMode, Optional Condition As String = "") Implements IGeneral.ApplySecurity
        Try
            '    If LoginGroup = "Administrator" Then
            '        Me.Visible = True
            '        Me.btnPrint.Enabled = True
            '        Me.btnShow.Enabled = True
            '        Me.CtrlGrdBar1.mGridPrint.Enabled = True
            '        Me.CtrlGrdBar1.mGridExport.Enabled = True
            '        Me.CtrlGrdBar1.mGridChooseFielder.Enabled = True
            '        'Exit Sub
            '    End If
            '    Dim strData As String = ""
            '    Dim dtData As New DataTable

            '    strData = "select * from tblformscontrols where formid =  (select formid from tblforms where formname = 'frmComplaintDetail')"
            '    dtData = GetDataTable(strData)
            '    dtData.AcceptChanges()
            '    For Each row As DataRow In dtData.Rows
            '        If row.Item("FormControlName") = "View" Then
            '            Me.Visible = True
            '        ElseIf row.Item("FormControlName") = "Show" Then
            '            Me.btnShow.Enabled = True
            '        ElseIf row.Item("FormControlName") = "Print" Then
            '            Me.btnPrint.Enabled = True
            '        ElseIf row.Item("FormControlName") = "Grid Print" Then
            '            Me.CtrlGrdBar1.mGridPrint.Enabled = True
            '        ElseIf row.Item("FormControlName") = "Grid Export" Then
            '            Me.CtrlGrdBar1.mGridExport.Enabled = True
            '        ElseIf row.Item("FormControlName") = "Field Chooser" Then
            '            Me.CtrlGrdBar1.mGridChooseFielder.Enabled = True
            '        End If
            '    Next


            If LoginGroup = "Administrator" Then
                Me.Visible = True
                Me.btnPrint.Enabled = True
                Me.btnShow.Enabled = True
                Me.CtrlGrdBar1.mGridPrint.Enabled = True
                Me.CtrlGrdBar1.mGridExport.Enabled = True
                Me.CtrlGrdBar1.mGridChooseFielder.Enabled = True
                Me.rdoToFactoryAndFromFactory.Enabled = True
                Me.rdoComplaintAndRequest.Enabled = True
                Exit Sub
            End If
            Me.Visible = False
            Me.btnPrint.Enabled = False
            Me.btnShow.Enabled = False
            Me.CtrlGrdBar1.mGridPrint.Enabled = False
            Me.CtrlGrdBar1.mGridExport.Enabled = False
            Me.CtrlGrdBar1.mGridChooseFielder.Enabled = False
            Me.rdoToFactoryAndFromFactory.Enabled = False
            Me.rdoComplaintAndRequest.Enabled = False
            For i As Integer = 0 To Rights.Count - 1
                If Rights.Item(i).FormControlName = "View" Then
                    Me.Visible = True
                ElseIf Rights.Item(i).FormControlName = "Show" Then
                    Me.btnShow.Enabled = True
                ElseIf Rights.Item(i).FormControlName = "Print" Then
                    Me.btnPrint.Enabled = True
                ElseIf Rights.Item(i).FormControlName = "Grid Print" Then
                    Me.CtrlGrdBar1.mGridPrint.Enabled = True
                ElseIf Rights.Item(i).FormControlName = "Grid Export" Then
                    Me.CtrlGrdBar1.mGridExport.Enabled = True
                ElseIf Rights.Item(i).FormControlName = "Field Chooser" Then
                    Me.CtrlGrdBar1.mGridChooseFielder.Enabled = True
                ElseIf Rights.Item(i).FormControlName = "Request/Return" Then
                    Me.rdoComplaintAndRequest.Enabled = True
                ElseIf Rights.Item(i).FormControlName = "Return To/From Factory" Then
                    Me.rdoToFactoryAndFromFactory.Enabled = True
                End If
            Next

        Catch ex As Exception
            msg_Error(ex.Message)
        End Try
    End Sub
    Public Sub ApplySecurityComplaintReturn()

    End Sub

    Public Function Delete(Optional Condition As String = "") As Boolean Implements IGeneral.Delete

    End Function

    Public Sub FillCombos(Optional Condition As String = "") Implements IGeneral.FillCombos
        Try
            If Condition = "party" Then
                FillUltraDropDown(cmbPartyName, "select coa_detail_id, detail_code, detail_title from vwCOADetail where account_type = 'Vendor' OR account_type = 'Customer' order by coa_detail_id ASC")
                Me.cmbPartyName.Rows(0).Activate()
                Me.cmbPartyName.DisplayLayout.Bands(0).Columns("coa_detail_id").Hidden = True
                UltraDropDownSearching(cmbPartyName, frmModProperty.blnListSeachStartWith, frmModProperty.blnListSeachContains)
            Else : Condition = "product"
                FillListBox(Me.lstItems.ListItem, "SELECT ArticleDefView.ArticleId ,ArticleDefView.ArticleCode+ ' ~ ' + ArticleDefView.ArticleDescription+ ' ~ ' +  ArticleDefView.ArticleColorName+ ' ~ ' + ArticleDefView.ArticleSizeName AS ArticleDescription FROM ArticleDefView")
                Me.lstItems.DeSelect()
                'ElseIf Condition = "complaintNo" Then
                '    FillUltraDropDown(cmbComplaintNo, "SELECT ComplaintReturnId, ComplaintReturnNo FROM ComplaintReturnMaster")
                '    Me.cmbComplaintNo.DisplayLayout.Bands(0).Columns("ComplaintId").Hidden = True
                '    UltraDropDownSearching(cmbComplaintNo, frmModProperty.blnListSeachStartWith, frmModProperty.blnListSeachContains)
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub FillModel(Optional Condition As String = "") Implements IGeneral.FillModel

    End Sub

    Public Sub GetAllRecords(Optional Condition As String = "") Implements IGeneral.GetAllRecords
        Try
            Dim strData As String = ""
            Dim dtData As New DataTable
            If rdoComplaintAndRequest.Checked = True Then
                strData = "EXEC dbo.SP_ComplaintComparision '" & Me.dtpFromDate.Value.ToString("yyyy-MM-dd 00:00:00") & "','" & Me.dtpToDate.Value.ToString("yyyy-MM-dd 00:00:00") & "', '" & Me.cmbPartyName.Value & "','" & Me.lstItems.SelectedIDs & "'"
            Else
                strData = "EXEC dbo.SP_ReturnToAndReturnFromFactoryComparision '" & Me.dtpFromDate.Value.ToString("yyyy-MM-dd 00:00:00") & "','" & Me.dtpToDate.Value.ToString("yyyy-MM-dd 00:00:00") & "', '" & Me.cmbPartyName.Value & "','" & Me.lstItems.SelectedIDs & "' "
            End If

            dtData = GetDataTable(strData)
            If rdoComplaintAndRequest.Checked = True Then
                'dtData.Columns("Status").Expression = "IIF(IsNull(TotalInQty,0)-IsNull(TotalOutQty,0) = 0,'Closed', 'Open')"




                'Else
                '    Dim dblQtyBalance As Decimal = 0D
                '    'Dim dblAmtBalance As Decimal = 0D
                '    For Each r As DataRow In dtData.Rows
                '        dblQtyBalance += r.Item("InQty") - r.Item("OutQty")
                '        'dblAmtBalance += r.Item("In_Amount") - r.Item("Out_Amount")
                '        r.BeginEdit()
                '        r("BalanceQty") = dblQtyBalance
                '        'r("Amount_Balance") = dblAmtBalance
                '        r.EndEdit()
                '        'dblQtyBalance += dblQtyBalance
                '        'dblAmtBalance += dblAmtBalance
                '    Next
            Else

             
            End If
            dtData.AcceptChanges()
            Me.GridResults.DataSource = dtData
            Me.GridResults.RetrieveStructure()
            If rdoComplaintAndRequest.Checked = False Then
                Dim group As New Janus.Windows.GridEX.GridEXGroup(Me.GridResults.RootTable.Columns("Product"))
                Me.GridResults.RootTable.Groups.Add(group)
                Me.GridResults.GroupTotals = Janus.Windows.GridEX.GroupTotals.Always
            End If
            Me.GridResults.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True
            Me.GridResults.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed
          
            ApplyGridSettings()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function IsValidate(Optional Mode As SBUtility.Utility.EnumDataMode = SBUtility.Utility.EnumDataMode.Disabled, Optional Condition As String = "") As Boolean Implements IGeneral.IsValidate

    End Function

    Public Sub ReSetControls(Optional Condition As String = "") Implements IGeneral.ReSetControls
        Try
            FillCombos("party")
            FillCombos("product")
            'FillCombos("complaintNo")
            CtrlGrdBar1_Load(Nothing, Nothing)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Public Function Save(Optional Condition As String = "") As Boolean Implements IGeneral.Save

    End Function

    Public Sub SetButtonImages() Implements IGeneral.SetButtonImages

    End Sub

    Public Sub SetConfigurationBaseSetting() Implements IGeneral.SetConfigurationBaseSetting

    End Sub

    Public Sub SetNavigationButtons(Mode As SBUtility.Utility.EnumDataMode, Optional Condition As String = "") Implements IGeneral.SetNavigationButtons

    End Sub

    Public Function Update1(Optional Condition As String = "") As Boolean Implements IGeneral.Update

    End Function

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        Try
            Application.DoEvents()
            Me.Cursor = Cursors.WaitCursor
            GetAllRecords()
            Me.UltraTabConrol1.Tabs(1).Selected = True
            'If rdoComplaintAndRequest.Checked = True Then
            '    Me.Label1.Text = "Complaint Request And Return Comparison Report"
            'Else
            '    Me.Label1.Text = "Return To And Return From Factory Comparison Report"
            'End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub CtrlGrdBar1_Load(sender As Object, e As EventArgs) Handles CtrlGrdBar1.Load
        Try
            If IO.File.Exists(Application.ExecutablePath & "\..\Layouts\" & Me.Name & "_" & Me.GridResults.Name) Then
                Dim fs As New IO.FileStream(Application.ExecutablePath & "\..\Layouts\" & Me.Name & "_" & Me.GridResults.Name, IO.FileMode.Open, IO.FileAccess.ReadWrite)
                Me.GridResults.LoadLayoutFile(fs)
                fs.Close()
                fs.Dispose()
            End If
            Me.CtrlGrdBar1.txtGridTitle.Text = CompanyTitle & Chr(10) & "Complaint Detail" & Chr(10) & "From Date : " & Me.dtpFromDate.Value.ToString("yyyy-MM-dd") & "To Date : " & Me.dtpToDate.Value.ToString("yyyy-MM-dd")
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Try
            GetCrystalReportRights()
            Application.DoEvents()
            Me.Cursor = Cursors.WaitCursor
            AddRptParam("@FromDate", Me.dtpFromDate.Value.ToString("yyyy-MM-dd 00:00:00"))
            AddRptParam("@ToDate", Me.dtpToDate.Value.ToString("yyyy-MM-dd 23:59:59"))
            AddRptParam("@Party", Me.cmbPartyName.Value)
            AddRptParam("@Product", Me.lstItems.SelectedIDs)
            If rdoToFactoryAndFromFactory.Checked = True Then
                ShowReport("rptReturnToAndReturnFromFactoryComparison")
            Else
                ShowReport("rptComplaintComparision")
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub txtSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyUp
        Try
            Dim dv As New DataView
            _SearchDt.TableName = "Default"
            _SearchDt.CaseSensitive = False
            dv.Table = _SearchDt
            dv.RowFilter = "ArticleDescription Like '%" & Me.txtSearch.Text & "%'"
            Me.lstItems.ListItem.DataSource = dv.ToTable
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub rdoComplaint_Click(sender As Object, e As EventArgs) Handles rdoComplaintAndRequest.CheckedChanged
        Me.Label1.Text = "Complaint Request And Return Comparison Report"
        ReSetControls()
    End Sub

    Private Sub rdoCompliantReturn_CheckedChanged(sender As Object, e As EventArgs) Handles rdoToFactoryAndFromFactory.CheckedChanged
        Try
            Me.Label1.Text = "Return To And Return From Factory Comparison Report"
            ReSetControls()
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub rdoCode_CheckedChanged(sender As Object, e As EventArgs) Handles rdoCode.CheckedChanged
        If isFormLoaded = False Then Exit Sub
        Me.cmbPartyName.DisplayMember = Me.cmbPartyName.Rows(0).Cells(1).Column.Key.ToString
        Me.cmbPartyName.Rows(0).Activate()
    End Sub

    Private Sub rdoName_CheckedChanged(sender As Object, e As EventArgs) Handles rdoName.CheckedChanged
        If isFormLoaded = False Then Exit Sub
        Me.cmbPartyName.DisplayMember = Me.cmbPartyName.Rows(0).Cells(2).Column.Key.ToString
        Me.cmbPartyName.Rows(0).Activate()
    End Sub


    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ReSetControls()
    End Sub
End Class