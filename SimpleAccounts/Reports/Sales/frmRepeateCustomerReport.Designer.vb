﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRepeateCustomerReport
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim GridEX1_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRepeateCustomerReport))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnRefresh = New System.Windows.Forms.ToolStripButton()
        Me.btnReport = New System.Windows.Forms.ToolStripButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.lblToDate = New System.Windows.Forms.Label()
        Me.lblFromDate = New System.Windows.Forms.Label()
        Me.dtpInquiryToDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpInquiryFromDate = New System.Windows.Forms.DateTimePicker()
        Me.lblLiftWiseBusiness = New System.Windows.Forms.Label()
        Me.GridEX1 = New Janus.Windows.GridEX.GridEX()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ToolStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.GridEX1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnRefresh, Me.btnReport})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(591, 25)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnRefresh
        '
        Me.btnRefresh.Image = Global.SimpleAccounts.My.Resources.Resources.Refresh
        Me.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(66, 22)
        Me.btnRefresh.Text = "&Refresh"
        '
        'btnReport
        '
        Me.btnReport.Image = Global.SimpleAccounts.My.Resources.Resources._1495105162_Document
        Me.btnReport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnReport.Name = "btnReport"
        Me.btnReport.Size = New System.Drawing.Size(52, 22)
        Me.btnReport.Text = "&Print"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(90, Byte), Integer), CType(CType(211, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.TextBox1)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.lblToDate)
        Me.Panel1.Controls.Add(Me.lblFromDate)
        Me.Panel1.Controls.Add(Me.dtpInquiryToDate)
        Me.Panel1.Controls.Add(Me.dtpInquiryFromDate)
        Me.Panel1.Controls.Add(Me.lblLiftWiseBusiness)
        Me.Panel1.Location = New System.Drawing.Point(0, 28)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(585, 68)
        Me.Panel1.TabIndex = 2
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(485, 30)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(38, 23)
        Me.btnPrint.TabIndex = 21
        Me.btnPrint.Text = "Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'lblToDate
        '
        Me.lblToDate.AutoSize = True
        Me.lblToDate.ForeColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.lblToDate.Location = New System.Drawing.Point(208, 34)
        Me.lblToDate.Name = "lblToDate"
        Me.lblToDate.Size = New System.Drawing.Size(46, 13)
        Me.lblToDate.TabIndex = 20
        Me.lblToDate.Text = "To Date"
        '
        'lblFromDate
        '
        Me.lblFromDate.AutoSize = True
        Me.lblFromDate.ForeColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.lblFromDate.Location = New System.Drawing.Point(9, 34)
        Me.lblFromDate.Name = "lblFromDate"
        Me.lblFromDate.Size = New System.Drawing.Size(56, 13)
        Me.lblFromDate.TabIndex = 19
        Me.lblFromDate.Text = "From Date"
        '
        'dtpInquiryToDate
        '
        Me.dtpInquiryToDate.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.dtpInquiryToDate.CustomFormat = "dd/MMM/yyyy"
        Me.dtpInquiryToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpInquiryToDate.Location = New System.Drawing.Point(260, 32)
        Me.dtpInquiryToDate.Name = "dtpInquiryToDate"
        Me.dtpInquiryToDate.ShowCheckBox = True
        Me.dtpInquiryToDate.Size = New System.Drawing.Size(131, 20)
        Me.dtpInquiryToDate.TabIndex = 15
        '
        'dtpInquiryFromDate
        '
        Me.dtpInquiryFromDate.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.dtpInquiryFromDate.CustomFormat = "dd/MMM/yyyy"
        Me.dtpInquiryFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpInquiryFromDate.Location = New System.Drawing.Point(71, 32)
        Me.dtpInquiryFromDate.Name = "dtpInquiryFromDate"
        Me.dtpInquiryFromDate.ShowCheckBox = True
        Me.dtpInquiryFromDate.Size = New System.Drawing.Size(131, 20)
        Me.dtpInquiryFromDate.TabIndex = 15
        '
        'lblLiftWiseBusiness
        '
        Me.lblLiftWiseBusiness.AutoSize = True
        Me.lblLiftWiseBusiness.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLiftWiseBusiness.ForeColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.lblLiftWiseBusiness.Location = New System.Drawing.Point(8, 6)
        Me.lblLiftWiseBusiness.Name = "lblLiftWiseBusiness"
        Me.lblLiftWiseBusiness.Size = New System.Drawing.Size(230, 20)
        Me.lblLiftWiseBusiness.TabIndex = 16
        Me.lblLiftWiseBusiness.Text = "Repeated Customer Report"
        '
        'GridEX1
        '
        Me.GridEX1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        GridEX1_DesignTimeLayout.LayoutString = resources.GetString("GridEX1_DesignTimeLayout.LayoutString")
        Me.GridEX1.DesignTimeLayout = GridEX1_DesignTimeLayout
        Me.GridEX1.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown
        Me.GridEX1.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges
        Me.GridEX1.GroupByBoxVisible = False
        Me.GridEX1.Location = New System.Drawing.Point(0, 102)
        Me.GridEX1.Name = "GridEX1"
        Me.GridEX1.NewRowEnterKeyBehavior = Janus.Windows.GridEX.NewRowEnterKeyBehavior.AddRowAndMoveToFirstCellInNewRow
        Me.GridEX1.Size = New System.Drawing.Size(587, 313)
        Me.GridEX1.TabIndex = 4
        Me.GridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(429, 32)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(38, 20)
        Me.TextBox1.TabIndex = 22
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(397, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(26, 13)
        Me.Label1.TabIndex = 23
        Me.Label1.Text = "Visit"
        '
        'frmRepeatedCustomerReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(591, 418)
        Me.Controls.Add(Me.GridEX1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "frmRepeateCustomerReport"
        Me.Text = "Lift Wise Percentage Report"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.GridEX1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnRefresh As System.Windows.Forms.ToolStripButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents dtpInquiryToDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpInquiryFromDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblLiftWiseBusiness As System.Windows.Forms.Label
    Friend WithEvents btnReport As System.Windows.Forms.ToolStripButton
    Friend WithEvents lblToDate As System.Windows.Forms.Label
    Friend WithEvents lblFromDate As System.Windows.Forms.Label
    Friend WithEvents GridEX1 As Janus.Windows.GridEX.GridEX
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
End Class
