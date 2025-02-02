Imports System.Data.OleDb
Public Class rptCategoryWiseSaleReport
    Dim CurrentId As Integer

    Private Sub rptInventoryForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

    End Sub


    Private Sub DefMainAcc_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.BindGrid()
        'Me.FillAllCombos()
        'Me.RefreshForm()    'FillComboBox()
        Me.cmbType.SelectedIndex = 0
        'Me.dtpFrom.Value = Me.dtpFrom.Value.AddMonths(-1)
        Me.cmbPeriod.Text = "Current Month"
        Me.BindGrid()
        Me.GetSecurityRights()

    End Sub

    Sub BindGrid()

        If Not Me.IsValidate Then Exit Sub

        Dim strSql As String
        Dim strCol As String = String.Empty
        If Me.cmbType.SelectedIndex = 0 Then
            strCol = "SUM(dbo.SalesDetailTable.Qty)"
        Else
            strCol = "SUM(dbo.SalesDetailTable.Qty * dbo.SalesDetailTable.Price)"
        End If

        strSql = "SELECT      convert(datetime , left(convert(varchar , dbo.SalesMasterTable.SalesDate),11), 102) as SalesDate , isnull(" & strCol & " , 0) AS Col, dbo.ArticleGroupDefTable.ArticleGroupName  " _
                & " FROM         dbo.SalesMasterTable INNER JOIN" _
                & "                   dbo.SalesDetailTable ON dbo.SalesMasterTable.SalesId = dbo.SalesDetailTable.SalesId INNER JOIN " _
                & "               dbo.ArticleDefTable ON dbo.SalesDetailTable.ArticleDefId = dbo.ArticleDefTable.ArticleId INNER JOIN " _
                & "           dbo.ArticleGroupDefTable ON dbo.ArticleDefTable.ArticleGroupId = dbo.ArticleGroupDefTable.ArticleGroupId " _
                & " WHERE  (Convert(varchar,dbo.SalesMasterTable.SalesDate,102) BETWEEN CONVERT(DATETIME, '" & Me.dtpFrom.Value.ToString("yyyy-M-d 00:00:00") & "', 102) AND CONVERT(DATETIME, '" & Me.dtpTo.Value.ToString("yyyy-M-d 23:59:59") & "', 102)) AND ArticleDefTable.Active=1 " _
                & " GROUP BY  convert(datetime , left(convert(varchar , dbo.SalesMasterTable.SalesDate),11), 102), dbo.ArticleGroupDefTable.ArticleGroupName " _
                & " order by convert(datetime , left(convert(varchar , dbo.SalesMasterTable.SalesDate),11), 102)  "

        Dim dt As DataTable = GetDataTable(strSql)
        If Not dt Is Nothing Then
            If Not dt.Rows.Count = 0 Then
                Dim objDtSales As New DataTable
                Dim dc As DataColumn
                dc = New DataColumn("Date")
                dc.Caption = "Date"
                dc.DataType = GetType(DateTime)
                objDtSales.Columns.Add(dc)

                For Each r As DataRow In dt.Rows
                    If Not objDtSales.Columns.Contains(r.Item("ArticleGroupName")) Then
                        objDtSales.Columns.Add(New DataColumn(r.Item("ArticleGroupName"), GetType(System.Double)))
                    End If
                Next

                dc = New DataColumn("Total Sale", GetType(System.Double))
                objDtSales.Columns.Add(dc)

                Dim dtp As Date = Date.MinValue
                Dim dr As DataRow
                For Each r As DataRow In dt.Rows
                    If r.Item("SalesDate") = dtp Then
                        dr = objDtSales.NewRow
                        dr.Item(0) = r.Item("SalesDate").ToString
                        dr.Item(r.Item("ArticleGroupName")) = r.Item("Col")
                        dtp = r.Item("SalesDate")
                        If Not IsDBNull(dr.Item("Total Sale")) Then
                            dr.Item("Total Sale") = dr.Item("Total Sale") + r.Item("Col")
                        Else
                            dr.Item("Total Sale") = r.Item("Col")
                        End If
                    Else
                        dr = objDtSales.NewRow
                        dr.Item(0) = r.Item("SalesDate")
                        dr.Item(r.Item("ArticleGroupName")) = r.Item("Col")
                        dr.Item("Total Sale") = r.Item("Col")
                        objDtSales.Rows.Add(dr)
                        dtp = r.Item("SalesDate")
                    End If
                Next
                Me.grdStock.DataSource = objDtSales

                For i As Integer = 1 To Me.grdStock.DisplayLayout.Bands(0).Columns.Count - 1
                    Me.grdStock.DisplayLayout.Bands(0).Columns(i).Format = "n"
                Next

            Else
                Me.grdStock.DataSource = dt
            End If
        Else
            Me.grdStock.DataSource = dt
        End If
    End Sub


    Private Sub SaveToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripButton.Click
        'If Not Me.uicmbCategory.ActiveRow.Cells(0).Value > 0 Then
        '    msg_Error("Please select category")
        '    Me.uicmbCategory.Focus()
        '    Exit Sub
        'End If

        'If Not Me.uicmbType.ActiveRow.Cells(0).Value > 0 Then
        '    msg_Error("Please select type")
        '    Me.uicmbType.Focus()
        '    Exit Sub
        'End If

        'If Me.uitxtItemCode.Text = "" Then
        '    msg_Error("Please enter valid Item Code")
        '    Me.uitxtItemCode.Focus()
        '    Exit Sub

        'End If

        'If Me.uitxtItemName.Text = "" Then
        '    msg_Error("Please enter valid Item Name")
        '    Me.uitxtItemName.Focus()
        '    Exit Sub
        'End If

        'If Not Me.uicmbColor.ActiveRow.Cells(0).Value > 0 Then
        '    msg_Error("Please select color")
        '    Me.uicmbColor.Focus()
        '    Exit Sub
        'End If

        'If Not Me.uicmbSize.ActiveRow.Cells(0).Value > 0 Then
        '    msg_Error("Please select size")
        '    Me.uicmbSize.Focus()
        '    Exit Sub
        'End If


        ''If Not Me.uitxtPrice.Text > 0 Then
        ''    msg_Error("Please enter valid purchase price")
        ''    Me.uitxtPrice.Focus()
        ''    Exit Sub
        ''End If

        ''If Not Me.uitxtSalePrice.Text > 0 Then
        ''    msg_Error("Please enter valid sale price")
        ''    Me.uitxtSalePrice.Focus()
        ''    Exit Sub
        ''End If

        'If Not Me.uitxtPackQty.Text > 0 Then
        '    msg_Error("Please enter minimum pack quantity of 1")
        '    Me.uitxtPackQty.Focus()
        '    Exit Sub
        'End If

        'If Not msg_Confirm(str_ConfirmSave) = True Then Exit Sub

        'Dim cm As New OleDbCommand

        'If Con.State = ConnectionState.Closed Then Con.Open()
        'cm.Connection = Con
        'Try
        '    If Me.SaveToolStripButton.Text = "&Save" Or Me.SaveToolStripButton.Text = "&Save" Then
        '        cm.CommandText = "insert into ArticleDefTable(ArticleCode,ArticleDescription,ArticleGroupId,SizeRangeId,ArticleColorId,ArticleTypeId,PurchasePrice,SalePrice,PackQty,StockLevel,Active,SortOrder,IsDate) values('" & Me.uitxtItemCode.Text & "','" & Me.uitxtItemName.Text & "'," & Me.uicmbCategory.ActiveRow.Cells(0).Value & "," & Me.uicmbSize.ActiveRow.Cells(0).Value & "," & Me.uicmbColor.ActiveRow.Cells(0).Value & "," & Me.uicmbType.ActiveRow.Cells(0).Value & "," & Me.uitxtPrice.Text & "," & Me.uitxtSalePrice.Text & "," & Me.uitxtPackQty.Text & "," & Me.uitxtStockLevel.Text & "," & IIf(Me.uichkActive.Checked = True, 1, 0) & "," & Me.uitxtSortOrder.Text & "," & Date.Today & ")"
        '    Else
        '        cm.CommandText = "update ArticleDefTable set ArticleCode='" & Me.uitxtItemCode.Text & "',ArticleDescription='" & Me.uitxtItemName.Text & "', ArticleGroupId='" & Me.uicmbCategory.ActiveRow.Cells(0).Value & "',SizeRangeId=" & Me.uicmbSize.ActiveRow.Cells(0).Value & ",articleColorId=" & Me.uicmbColor.ActiveRow.Cells(0).Value & ",ArticleTypeId=" & Me.uicmbType.ActiveRow.Cells(0).Value & ",PurchasePrice=" & Me.uitxtPrice.Text & ",SalePrice=" & Me.uitxtSalePrice.Text & ",PackQty=" & Me.uitxtPackQty.Text & ",StockLevel=" & Me.uitxtStockLevel.Text & ",Active=" & IIf(Me.uichkActive.Checked = True, 1, 0) & ",SortOrder=" & Me.uitxtSortOrder.Text & " where ArticleId=" & Me.CurrentId
        '    End If
        '    cm.ExecuteNonQuery()
        '    msg_Information(str_informSave)
        '    Me.CurrentId = 0
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'Finally
        '    Con.Close()
        'End Try
        'Me.RefreshForm()
    End Sub






    Private Sub RefreshDataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshDataToolStripMenuItem.Click
        Me.BindGrid()
        GetSecurityRights()
    End Sub

    Private Sub ExportToExcelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportToExcelToolStripMenuItem.Click
        Me.SaveFileDialog1.Filter = "Excel Files|.xls"
        'Me.SaveFileDialog1.DefaultExt = ".xls"
        Me.SaveFileDialog1.FileName = "Category Wise Sales Report"
        Me.SaveFileDialog1.InitialDirectory = "C:\"
        If Me.SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If Me.SaveFileDialog1.FileName = "" Then Exit Sub

        Me.UltraGridExcelExporter1.Export(Me.grdStock, Me.SaveFileDialog1.FileName) '& ".xls") '"c:\temp.xls")
        System.Diagnostics.Process.Start(Me.SaveFileDialog1.FileName) '& ".xls")
    End Sub

    Private Sub PrintItemLedgerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim FromDate As Date = CDate("01-jan-2008")
        Dim ToDate As Date = Date.Now
        Dim opening As Integer = GetStockOpeningBalance(0, FromDate.Year & "-" & FromDate.Month & "-" & FromDate.Day & " 00:00:00")
        If Not Me.grdStock.Rows.Count > 0 Then msg_Error(str_ErrorNoRecordFound) : Exit Sub
        'Dim fromDate As String = Me.DateTimePicker1.Value.Year & "." & Me.DateTimePicker1.Value.Month & "." & Me.DateTimePicker1.Value.Day
        'Dim ToDate As String = Me.DateTimePicker2.Value.Year & "." & Me.DateTimePicker2.Value.Month & "." & Me.DateTimePicker2.Value.Day
        ' ShowReport("Ledger", "{SP_Rpt_Ledger.coa_detail_id}=" & Me.grdStock.ActiveRow.Cells(0).Value, fromDate, ToDate)
        rptDateRange.ReportName = rptDateRange.ReportList.ItemLedger
        rptDateRange.AccountId = Me.grdStock.ActiveRow.Cells(0).Value
        rptDateRange.ShowDialog()

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.BindGrid()
        Me.GetSecurityRights()
    End Sub


    Private Sub GetSecurityRights()
        Try
            Dim dt As DataTable = GetFormRights(EnumForms.rptCategoryWiseSaleReport)
            If Not dt Is Nothing Then
                If Not dt.Rows.Count = 0 Then
                    If Me.SaveToolStripButton.Text = "Save" Or Me.SaveToolStripButton.Text = "&Save" Then
                        Me.SaveToolStripButton.Enabled = dt.Rows(0).Item("Save_Rights").ToString()
                    Else
                        Me.SaveToolStripButton.Enabled = dt.Rows(0).Item("Update_Rights").ToString
                    End If
                    Me.DeleteToolStripButton.Enabled = dt.Rows(0).Item("Delete_Rights").ToString
                    Me.PrintToolStripButton.Enabled = dt.Rows(0).Item("Print_Rights").ToString
                    Me.ContextMenuStrip1.Items("ExportToExcelToolStripMenuItem").Enabled = dt.Rows(0).Item("Export_Rights").ToString
                    Me.Visible = dt.Rows(0).Item("View_Rights").ToString
                End If
            End If
        Catch ex As Exception
            msg_Error(ex.Message)
        End Try
    End Sub

    Private Function IsValidate() As Boolean
        Try
            If Me.dtpFrom.Value > Me.dtpTo.Value Then
                msg_Error("From Date can't be greate than To Date")
                Me.dtpFrom.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Sub PrintToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripButton.Click
        Me.grdStock.PrintPreview()
    End Sub
    Private Sub cmbPeriod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPeriod.SelectedIndexChanged
        If Me.cmbPeriod.Text = "Today" Then
            Me.dtpFrom.Value = Date.Today
            Me.dtpTo.Value = Date.Today
        ElseIf Me.cmbPeriod.Text = "Yesterday" Then
            Me.dtpFrom.Value = Date.Today.AddDays(-1)
            Me.dtpTo.Value = Date.Today.AddDays(-1)
        ElseIf Me.cmbPeriod.Text = "Current Week" Then
            Me.dtpFrom.Value = Date.Today.AddDays(-(Date.Now.DayOfWeek))
            Me.dtpTo.Value = Date.Today
        ElseIf Me.cmbPeriod.Text = "Current Month" Then
            Me.dtpFrom.Value = New Date(Date.Now.Year, Date.Now.Month, 1)
            Me.dtpTo.Value = Date.Today
        ElseIf Me.cmbPeriod.Text = "Current Year" Then
            Me.dtpFrom.Value = New Date(Date.Now.Year, 1, 1)
            Me.dtpTo.Value = Date.Today
        End If
    End Sub
End Class