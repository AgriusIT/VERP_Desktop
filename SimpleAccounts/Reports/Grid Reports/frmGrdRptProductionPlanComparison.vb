﻿Public Class frmGrdRptProductionPlanComparison

    Private Sub UltraTabPageControl1_Paint(sender As Object, e As PaintEventArgs) Handles UltraTabPageControl1.Paint
















































































































































































































































    End Sub

    Private Sub grdReport_FormattingRow(sender As Object, e As Janus.Windows.GridEX.RowLoadEventArgs) Handles grdReport.FormattingRow

    End Sub

    Private Sub CtrlGrdBar1_Load(sender As Object, e As EventArgs) Handles CtrlGrdBar1.Load
        Try
            If IO.File.Exists(str_ApplicationStartUpPath & "\Layouts\" & Me.Name & "_" & Me.grdReport.Name) Then
                Dim fs As New System.IO.FileStream(str_ApplicationStartUpPath & "\Layouts\" & Me.Name & "_" & Me.grdReport.Name, IO.FileMode.Open, IO.FileAccess.ReadWrite, IO.FileShare.ReadWrite)
                Me.grdReport.LoadLayoutFile(fs)
                fs.Close()
                fs.Dispose()
            End If
            Me.CtrlGrdBar1.txtGridTitle.Text = CompanyTitle & vbCrLf & "Production Plan Comparison "
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
End Class