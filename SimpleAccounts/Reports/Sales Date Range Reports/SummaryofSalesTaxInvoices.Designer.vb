﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SummaryofSalesTaxInvoices
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
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Id")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ArticleDescription")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Size")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Color")
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SummaryofSalesTaxInvoices))
        Me.pnlCustomerType = New System.Windows.Forms.Panel()
        Me.lblCustomerType = New System.Windows.Forms.Label()
        Me.cmbCustomerType = New System.Windows.Forms.ComboBox()
        Me.pnlSalesType = New System.Windows.Forms.Panel()
        Me.rbtSalesTypeBoth = New System.Windows.Forms.RadioButton()
        Me.rbtCreditSales = New System.Windows.Forms.RadioButton()
        Me.rbtCashSales = New System.Windows.Forms.RadioButton()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbPeriod = New System.Windows.Forms.ComboBox()
        Me.pnlInvType = New System.Windows.Forms.Panel()
        Me.lblCompany = New System.Windows.Forms.Label()
        Me.cmbCompany = New System.Windows.Forms.ComboBox()
        Me.pnlCost = New System.Windows.Forms.Panel()
        Me.lblCostCenter = New System.Windows.Forms.Label()
        Me.cmbCostCenter = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.pnlVendorCustomer = New System.Windows.Forms.Panel()
        Me.cmbCustomer = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.lblCustomer = New System.Windows.Forms.Label()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.lblHeader = New System.Windows.Forms.Label()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.pnlPeriod = New System.Windows.Forms.Panel()
        Me.pnlCustomerType.SuspendLayout()
        Me.pnlSalesType.SuspendLayout()
        Me.pnlInvType.SuspendLayout()
        Me.pnlCost.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.pnlVendorCustomer.SuspendLayout()
        CType(Me.cmbCustomer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlHeader.SuspendLayout()
        Me.pnlPeriod.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlCustomerType
        '
        Me.pnlCustomerType.BackColor = System.Drawing.Color.Transparent
        Me.pnlCustomerType.Controls.Add(Me.lblCustomerType)
        Me.pnlCustomerType.Controls.Add(Me.cmbCustomerType)
        Me.pnlCustomerType.Location = New System.Drawing.Point(18, 74)
        Me.pnlCustomerType.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pnlCustomerType.Name = "pnlCustomerType"
        Me.pnlCustomerType.Size = New System.Drawing.Size(540, 60)
        Me.pnlCustomerType.TabIndex = 14
        '
        'lblCustomerType
        '
        Me.lblCustomerType.AutoSize = True
        Me.lblCustomerType.Location = New System.Drawing.Point(4, 20)
        Me.lblCustomerType.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCustomerType.Name = "lblCustomerType"
        Me.lblCustomerType.Size = New System.Drawing.Size(80, 20)
        Me.lblCustomerType.TabIndex = 0
        Me.lblCustomerType.Text = "Cust Type"
        '
        'cmbCustomerType
        '
        Me.cmbCustomerType.FormattingEnabled = True
        Me.cmbCustomerType.Location = New System.Drawing.Point(122, 14)
        Me.cmbCustomerType.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cmbCustomerType.Name = "cmbCustomerType"
        Me.cmbCustomerType.Size = New System.Drawing.Size(403, 28)
        Me.cmbCustomerType.TabIndex = 1
        '
        'pnlSalesType
        '
        Me.pnlSalesType.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(211, Byte), Integer))
        Me.pnlSalesType.Controls.Add(Me.rbtSalesTypeBoth)
        Me.pnlSalesType.Controls.Add(Me.rbtCreditSales)
        Me.pnlSalesType.Controls.Add(Me.rbtCashSales)
        Me.pnlSalesType.Location = New System.Drawing.Point(18, 466)
        Me.pnlSalesType.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pnlSalesType.Name = "pnlSalesType"
        Me.pnlSalesType.Size = New System.Drawing.Size(540, 60)
        Me.pnlSalesType.TabIndex = 26
        '
        'rbtSalesTypeBoth
        '
        Me.rbtSalesTypeBoth.AutoSize = True
        Me.rbtSalesTypeBoth.Checked = True
        Me.rbtSalesTypeBoth.Location = New System.Drawing.Point(309, 14)
        Me.rbtSalesTypeBoth.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.rbtSalesTypeBoth.Name = "rbtSalesTypeBoth"
        Me.rbtSalesTypeBoth.Size = New System.Drawing.Size(68, 24)
        Me.rbtSalesTypeBoth.TabIndex = 2
        Me.rbtSalesTypeBoth.TabStop = True
        Me.rbtSalesTypeBoth.Text = "Both"
        Me.rbtSalesTypeBoth.UseVisualStyleBackColor = True
        '
        'rbtCreditSales
        '
        Me.rbtCreditSales.AutoSize = True
        Me.rbtCreditSales.Location = New System.Drawing.Point(152, 14)
        Me.rbtCreditSales.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.rbtCreditSales.Name = "rbtCreditSales"
        Me.rbtCreditSales.Size = New System.Drawing.Size(124, 24)
        Me.rbtCreditSales.TabIndex = 1
        Me.rbtCreditSales.Text = "Credit Sales "
        Me.rbtCreditSales.UseVisualStyleBackColor = True
        '
        'rbtCashSales
        '
        Me.rbtCashSales.AutoSize = True
        Me.rbtCashSales.Location = New System.Drawing.Point(9, 14)
        Me.rbtCashSales.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.rbtCashSales.Name = "rbtCashSales"
        Me.rbtCashSales.Size = New System.Drawing.Size(115, 24)
        Me.rbtCashSales.TabIndex = 0
        Me.rbtCashSales.Text = "Cash Sales"
        Me.rbtCashSales.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 15)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(54, 20)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "Period"
        '
        'cmbPeriod
        '
        Me.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPeriod.FormattingEnabled = True
        Me.cmbPeriod.Items.AddRange(New Object() {"Today", "Yesterday", "Current Week", "Current Month", "Current Year"})
        Me.cmbPeriod.Location = New System.Drawing.Point(122, 11)
        Me.cmbPeriod.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cmbPeriod.Name = "cmbPeriod"
        Me.cmbPeriod.Size = New System.Drawing.Size(403, 28)
        Me.cmbPeriod.TabIndex = 19
        '
        'pnlInvType
        '
        Me.pnlInvType.BackColor = System.Drawing.Color.Transparent
        Me.pnlInvType.Controls.Add(Me.lblCompany)
        Me.pnlInvType.Controls.Add(Me.cmbCompany)
        Me.pnlInvType.Location = New System.Drawing.Point(18, 143)
        Me.pnlInvType.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pnlInvType.Name = "pnlInvType"
        Me.pnlInvType.Size = New System.Drawing.Size(540, 60)
        Me.pnlInvType.TabIndex = 16
        '
        'lblCompany
        '
        Me.lblCompany.AutoSize = True
        Me.lblCompany.Location = New System.Drawing.Point(4, 18)
        Me.lblCompany.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCompany.Name = "lblCompany"
        Me.lblCompany.Size = New System.Drawing.Size(68, 20)
        Me.lblCompany.TabIndex = 0
        Me.lblCompany.Text = "Inv Type"
        '
        'cmbCompany
        '
        Me.cmbCompany.FormattingEnabled = True
        Me.cmbCompany.Location = New System.Drawing.Point(122, 12)
        Me.cmbCompany.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cmbCompany.Name = "cmbCompany"
        Me.cmbCompany.Size = New System.Drawing.Size(403, 28)
        Me.cmbCompany.TabIndex = 1
        '
        'pnlCost
        '
        Me.pnlCost.BackColor = System.Drawing.Color.Transparent
        Me.pnlCost.Controls.Add(Me.lblCostCenter)
        Me.pnlCost.Controls.Add(Me.cmbCostCenter)
        Me.pnlCost.Location = New System.Drawing.Point(18, 212)
        Me.pnlCost.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pnlCost.Name = "pnlCost"
        Me.pnlCost.Size = New System.Drawing.Size(540, 60)
        Me.pnlCost.TabIndex = 17
        Me.pnlCost.Visible = False
        '
        'lblCostCenter
        '
        Me.lblCostCenter.AutoSize = True
        Me.lblCostCenter.Location = New System.Drawing.Point(4, 17)
        Me.lblCostCenter.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCostCenter.Name = "lblCostCenter"
        Me.lblCostCenter.Size = New System.Drawing.Size(94, 20)
        Me.lblCostCenter.TabIndex = 0
        Me.lblCostCenter.Text = "Cost Center"
        '
        'cmbCostCenter
        '
        Me.cmbCostCenter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCostCenter.FormattingEnabled = True
        Me.cmbCostCenter.Location = New System.Drawing.Point(122, 11)
        Me.cmbCostCenter.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cmbCostCenter.Name = "cmbCostCenter"
        Me.cmbCostCenter.Size = New System.Drawing.Size(403, 28)
        Me.cmbCostCenter.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(304, 62)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(27, 20)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "To"
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Location = New System.Drawing.Point(8, 62)
        Me.lblFrom.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(46, 20)
        Me.lblFrom.TabIndex = 20
        Me.lblFrom.Text = "From"
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.CustomFormat = "dd/MMM/yyyy"
        Me.DateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker2.Location = New System.Drawing.Point(351, 52)
        Me.DateTimePicker2.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(174, 26)
        Me.DateTimePicker2.TabIndex = 23
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.CustomFormat = "dd/MMM/yyyy"
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker1.Location = New System.Drawing.Point(122, 52)
        Me.DateTimePicker1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(172, 26)
        Me.DateTimePicker1.TabIndex = 21
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.Controls.Add(Me.btnPrint, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(314, 535)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(244, 45)
        Me.TableLayoutPanel1.TabIndex = 27
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnPrint.Location = New System.Drawing.Point(120, 5)
        Me.btnPrint.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(118, 35)
        Me.btnPrint.TabIndex = 1
        Me.btnPrint.Text = "Print"
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(4, 5)
        Me.OK_Button.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(106, 35)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "Show"
        '
        'pnlVendorCustomer
        '
        Me.pnlVendorCustomer.BackColor = System.Drawing.Color.Transparent
        Me.pnlVendorCustomer.Controls.Add(Me.cmbCustomer)
        Me.pnlVendorCustomer.Controls.Add(Me.lblCustomer)
        Me.pnlVendorCustomer.Location = New System.Drawing.Point(18, 282)
        Me.pnlVendorCustomer.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pnlVendorCustomer.Name = "pnlVendorCustomer"
        Me.pnlVendorCustomer.Size = New System.Drawing.Size(540, 60)
        Me.pnlVendorCustomer.TabIndex = 30
        '
        'cmbCustomer
        '
        Me.cmbCustomer.AlwaysInEditMode = True
        Me.cmbCustomer.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        Me.cmbCustomer.CheckedListSettings.CheckStateMember = ""
        Appearance1.BackColor = System.Drawing.Color.White
        Appearance1.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(61, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(255, Byte), Integer))
        Appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal
        Me.cmbCustomer.DisplayLayout.Appearance = Appearance1
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4})
        Me.cmbCustomer.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.cmbCustomer.DisplayLayout.InterBandSpacing = 10
        Me.cmbCustomer.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.cmbCustomer.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.cmbCustomer.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Appearance19.BackColor = System.Drawing.Color.Transparent
        Me.cmbCustomer.DisplayLayout.Override.CardAreaAppearance = Appearance19
        Me.cmbCustomer.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.cmbCustomer.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand
        Appearance20.BackColor = System.Drawing.Color.FromArgb(CType(CType(61, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(255, Byte), Integer))
        Appearance20.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(208, Byte), Integer))
        Appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance20.ForeColor = System.Drawing.Color.White
        Appearance20.TextHAlignAsString = "Left"
        Appearance20.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent
        Me.cmbCustomer.DisplayLayout.Override.HeaderAppearance = Appearance20
        Me.cmbCustomer.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Appearance21.BorderColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.cmbCustomer.DisplayLayout.Override.RowAppearance = Appearance21
        Appearance22.BackColor = System.Drawing.Color.FromArgb(CType(CType(61, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(255, Byte), Integer))
        Appearance22.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(208, Byte), Integer))
        Appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Me.cmbCustomer.DisplayLayout.Override.RowSelectorAppearance = Appearance22
        Me.cmbCustomer.DisplayLayout.Override.RowSelectorWidth = 12
        Me.cmbCustomer.DisplayLayout.Override.RowSpacingBefore = 2
        Appearance23.BackColor = System.Drawing.Color.FromArgb(CType(CType(129, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(226, Byte), Integer))
        Appearance23.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(221, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(254, Byte), Integer))
        Appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance23.ForeColor = System.Drawing.Color.Black
        Me.cmbCustomer.DisplayLayout.Override.SelectedRowAppearance = Appearance23
        Me.cmbCustomer.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.cmbCustomer.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.cmbCustomer.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Me.cmbCustomer.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.cmbCustomer.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid
        Me.cmbCustomer.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.cmbCustomer.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.cmbCustomer.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl
        Me.cmbCustomer.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.cmbCustomer.LimitToList = True
        Me.cmbCustomer.Location = New System.Drawing.Point(122, 12)
        Me.cmbCustomer.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cmbCustomer.MaxDropDownItems = 20
        Me.cmbCustomer.Name = "cmbCustomer"
        Me.cmbCustomer.Size = New System.Drawing.Size(405, 29)
        Me.cmbCustomer.TabIndex = 2
        '
        'lblCustomer
        '
        Me.lblCustomer.AutoSize = True
        Me.lblCustomer.Location = New System.Drawing.Point(4, 22)
        Me.lblCustomer.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCustomer.Name = "lblCustomer"
        Me.lblCustomer.Size = New System.Drawing.Size(78, 20)
        Me.lblCustomer.TabIndex = 0
        Me.lblCustomer.Text = "Customer"
        '
        'pnlHeader
        '
        Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(196, Byte), Integer), CType(CType(212, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.pnlHeader.Controls.Add(Me.lblHeader)
        Me.pnlHeader.Controls.Add(Me.Button5)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(576, 65)
        Me.pnlHeader.TabIndex = 112
        '
        'lblHeader
        '
        Me.lblHeader.AutoSize = True
        Me.lblHeader.Font = New System.Drawing.Font("Times New Roman", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeader.ForeColor = System.Drawing.Color.Black
        Me.lblHeader.Location = New System.Drawing.Point(4, 14)
        Me.lblHeader.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(438, 36)
        Me.lblHeader.TabIndex = 1
        Me.lblHeader.Text = "Summary of Sales Tax Invoices"
        '
        'Button5
        '
        Me.Button5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button5.BackgroundImage = CType(resources.GetObject("Button5.BackgroundImage"), System.Drawing.Image)
        Me.Button5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Button5.FlatAppearance.BorderSize = 0
        Me.Button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button5.Location = New System.Drawing.Point(381, 14)
        Me.Button5.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(38, 35)
        Me.Button5.TabIndex = 0
        Me.Button5.UseVisualStyleBackColor = True
        Me.Button5.Visible = False
        '
        'pnlPeriod
        '
        Me.pnlPeriod.BackColor = System.Drawing.Color.Transparent
        Me.pnlPeriod.Controls.Add(Me.cmbPeriod)
        Me.pnlPeriod.Controls.Add(Me.Label3)
        Me.pnlPeriod.Controls.Add(Me.lblFrom)
        Me.pnlPeriod.Controls.Add(Me.DateTimePicker1)
        Me.pnlPeriod.Controls.Add(Me.Label2)
        Me.pnlPeriod.Controls.Add(Me.DateTimePicker2)
        Me.pnlPeriod.Location = New System.Drawing.Point(18, 351)
        Me.pnlPeriod.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pnlPeriod.Name = "pnlPeriod"
        Me.pnlPeriod.Size = New System.Drawing.Size(540, 106)
        Me.pnlPeriod.TabIndex = 31
        '
        'SummaryofSalesTaxInvoices
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(211, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(576, 602)
        Me.Controls.Add(Me.pnlHeader)
        Me.Controls.Add(Me.pnlVendorCustomer)
        Me.Controls.Add(Me.pnlSalesType)
        Me.Controls.Add(Me.pnlInvType)
        Me.Controls.Add(Me.pnlCost)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.pnlCustomerType)
        Me.Controls.Add(Me.pnlPeriod)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "SummaryofSalesTaxInvoices"
        Me.Text = "Summary of Sales Tax Invoices"
        Me.pnlCustomerType.ResumeLayout(False)
        Me.pnlCustomerType.PerformLayout()
        Me.pnlSalesType.ResumeLayout(False)
        Me.pnlSalesType.PerformLayout()
        Me.pnlInvType.ResumeLayout(False)
        Me.pnlInvType.PerformLayout()
        Me.pnlCost.ResumeLayout(False)
        Me.pnlCost.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.pnlVendorCustomer.ResumeLayout(False)
        Me.pnlVendorCustomer.PerformLayout()
        CType(Me.cmbCustomer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.pnlPeriod.ResumeLayout(False)
        Me.pnlPeriod.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlCustomerType As System.Windows.Forms.Panel
    Friend WithEvents lblCustomerType As System.Windows.Forms.Label
    Friend WithEvents cmbCustomerType As System.Windows.Forms.ComboBox
    Friend WithEvents pnlSalesType As System.Windows.Forms.Panel
    Friend WithEvents rbtSalesTypeBoth As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCreditSales As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCashSales As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbPeriod As System.Windows.Forms.ComboBox
    Friend WithEvents pnlInvType As System.Windows.Forms.Panel
    Friend WithEvents lblCompany As System.Windows.Forms.Label
    Friend WithEvents cmbCompany As System.Windows.Forms.ComboBox
    Friend WithEvents pnlCost As System.Windows.Forms.Panel
    Friend WithEvents lblCostCenter As System.Windows.Forms.Label
    Friend WithEvents cmbCostCenter As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblFrom As System.Windows.Forms.Label
    Friend WithEvents DateTimePicker2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents pnlVendorCustomer As System.Windows.Forms.Panel
    Friend WithEvents cmbCustomer As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents lblCustomer As System.Windows.Forms.Label
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents lblHeader As System.Windows.Forms.Label
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents pnlPeriod As System.Windows.Forms.Panel
End Class
