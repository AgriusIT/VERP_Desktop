<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAttendanceEmployees
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAttendanceEmployees))
        Dim grd_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.BtnNew = New System.Windows.Forms.ToolStripButton()
        Me.BtnEdit = New System.Windows.Forms.ToolStripButton()
        Me.BtnSave = New System.Windows.Forms.ToolStripButton()
        Me.BtnSaveAll = New System.Windows.Forms.ToolStripButton()
        Me.BtnPrint = New System.Windows.Forms.ToolStripButton()
        Me.toolStripSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.BtnDelete = New System.Windows.Forms.ToolStripButton()
        Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnLoadAll = New System.Windows.Forms.ToolStripButton()
        Me.btnFullScreen = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnRefresh = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnOutTimeAttendance = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnSearchHistory = New System.Windows.Forms.ToolStripButton()
        Me.btnSMSTemplate = New System.Windows.Forms.ToolStripButton()
        Me.btnImportAttendance = New System.Windows.Forms.ToolStripButton()
        Me.btnBarcodeScan = New System.Windows.Forms.ToolStripButton()
        Me.HelpToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.grd = New Janus.Windows.GridEX.GridEX()
        Me.RbtOut = New System.Windows.Forms.RadioButton()
        Me.RbtIn = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblAttendanceTime = New System.Windows.Forms.Label()
        Me.lblDepartment = New System.Windows.Forms.Label()
        Me.lblDesignation = New System.Windows.Forms.Label()
        Me.lblEmployeeName = New System.Windows.Forms.Label()
        Me.lblAttendanceDate = New System.Windows.Forms.Label()
        Me.dtpAttendanceTime = New System.Windows.Forms.DateTimePicker()
        Me.txtEmpDesignation = New System.Windows.Forms.TextBox()
        Me.txtEmpDepartment = New System.Windows.Forms.TextBox()
        Me.dtpAttendanceDate = New System.Windows.Forms.DateTimePicker()
        Me.cmbShift = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbAttendanceStatus = New System.Windows.Forms.ComboBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmbEmployees = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.cmbSearchByEmployee = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.dtpTo = New System.Windows.Forms.DateTimePicker()
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtBarCodeEmpId = New System.Windows.Forms.TextBox()
        Me.grbBarcodeScan = New System.Windows.Forms.GroupBox()
        Me.lblError = New System.Windows.Forms.Label()
        Me.lblDept = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblAttendanceType = New System.Windows.Forms.Label()
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.gbGeneralControls = New System.Windows.Forms.GroupBox()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.lblHeader = New System.Windows.Forms.Label()
        Me.lblSeeU = New System.Windows.Forms.Label()
        Me.lblWelcome = New System.Windows.Forms.Label()
        Me.lblDesig = New System.Windows.Forms.Label()
        Me.lblEmpCode = New System.Windows.Forms.Label()
        Me.lblEmpName = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.gbSearchBy = New System.Windows.Forms.GroupBox()
        Me.cmbSearchByDepartment = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.lblSearchByDepartment = New System.Windows.Forms.Label()
        Me.btnHideSearchBy = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.lblSearchByEmployee = New System.Windows.Forms.Label()
        Me.cmbSearchByDesignation = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.lblSearchByDesignation = New System.Windows.Forms.Label()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.CtrlGrdBar1 = New SimpleAccounts.CtrlGrdBar()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.grd, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbEmployees, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbSearchByEmployee, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grbBarcodeScan.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.gbGeneralControls.SuspendLayout()
        Me.pnlHeader.SuspendLayout()
        Me.gbSearchBy.SuspendLayout()
        CType(Me.cmbSearchByDepartment, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbSearchByDesignation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ToolStrip1.AutoSize = False
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BtnNew, Me.BtnEdit, Me.BtnSave, Me.BtnSaveAll, Me.BtnPrint, Me.toolStripSeparator, Me.BtnDelete, Me.toolStripSeparator1, Me.ToolStripSeparator2, Me.btnLoadAll, Me.btnFullScreen, Me.ToolStripSeparator4, Me.btnRefresh, Me.ToolStripSeparator5, Me.btnOutTimeAttendance, Me.ToolStripSeparator3, Me.btnSearchHistory, Me.btnSMSTemplate, Me.btnImportAttendance, Me.btnBarcodeScan, Me.HelpToolStripButton})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1315, 31)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'BtnNew
        '
        Me.BtnNew.Image = CType(resources.GetObject("BtnNew.Image"), System.Drawing.Image)
        Me.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BtnNew.Name = "BtnNew"
        Me.BtnNew.Size = New System.Drawing.Size(63, 28)
        Me.BtnNew.Text = "&New"
        '
        'BtnEdit
        '
        Me.BtnEdit.Image = CType(resources.GetObject("BtnEdit.Image"), System.Drawing.Image)
        Me.BtnEdit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BtnEdit.Name = "BtnEdit"
        Me.BtnEdit.Size = New System.Drawing.Size(59, 28)
        Me.BtnEdit.Text = "&Edit"
        '
        'BtnSave
        '
        Me.BtnSave.Image = CType(resources.GetObject("BtnSave.Image"), System.Drawing.Image)
        Me.BtnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(64, 28)
        Me.BtnSave.Text = "&Save"
        '
        'BtnSaveAll
        '
        Me.BtnSaveAll.Image = Global.SimpleAccounts.My.Resources.Resources.sendcontactdetails
        Me.BtnSaveAll.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BtnSaveAll.Name = "BtnSaveAll"
        Me.BtnSaveAll.Size = New System.Drawing.Size(86, 28)
        Me.BtnSaveAll.Text = "Save All"
        '
        'BtnPrint
        '
        Me.BtnPrint.Image = CType(resources.GetObject("BtnPrint.Image"), System.Drawing.Image)
        Me.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BtnPrint.Name = "BtnPrint"
        Me.BtnPrint.Size = New System.Drawing.Size(63, 28)
        Me.BtnPrint.Text = "&Print"
        '
        'toolStripSeparator
        '
        Me.toolStripSeparator.Name = "toolStripSeparator"
        Me.toolStripSeparator.Size = New System.Drawing.Size(6, 31)
        '
        'BtnDelete
        '
        Me.BtnDelete.Image = CType(resources.GetObject("BtnDelete.Image"), System.Drawing.Image)
        Me.BtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(77, 28)
        Me.BtnDelete.Text = "D&elete"
        '
        'toolStripSeparator1
        '
        Me.toolStripSeparator1.Name = "toolStripSeparator1"
        Me.toolStripSeparator1.Size = New System.Drawing.Size(6, 31)
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 31)
        '
        'btnLoadAll
        '
        Me.btnLoadAll.Image = Global.SimpleAccounts.My.Resources.Resources.sendcontactdetails
        Me.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnLoadAll.Name = "btnLoadAll"
        Me.btnLoadAll.Size = New System.Drawing.Size(88, 28)
        Me.btnLoadAll.Text = "Load All"
        '
        'btnFullScreen
        '
        Me.btnFullScreen.Image = Global.SimpleAccounts.My.Resources.Resources.copy_doc
        Me.btnFullScreen.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnFullScreen.Name = "btnFullScreen"
        Me.btnFullScreen.Size = New System.Drawing.Size(104, 28)
        Me.btnFullScreen.Text = "Full Screen"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 31)
        '
        'btnRefresh
        '
        Me.btnRefresh.Image = Global.SimpleAccounts.My.Resources.Resources.Refresh
        Me.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(82, 28)
        Me.btnRefresh.Text = "Refresh"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 31)
        '
        'btnOutTimeAttendance
        '
        Me.btnOutTimeAttendance.Image = Global.SimpleAccounts.My.Resources.Resources.sendcontactdetails
        Me.btnOutTimeAttendance.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnOutTimeAttendance.Name = "btnOutTimeAttendance"
        Me.btnOutTimeAttendance.Size = New System.Drawing.Size(174, 28)
        Me.btnOutTimeAttendance.Text = "Out Time Attendance"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 31)
        '
        'btnSearchHistory
        '
        Me.btnSearchHistory.Image = Global.SimpleAccounts.My.Resources.Resources.search
        Me.btnSearchHistory.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSearchHistory.Name = "btnSearchHistory"
        Me.btnSearchHistory.Size = New System.Drawing.Size(77, 28)
        Me.btnSearchHistory.Text = "Search"
        '
        'btnSMSTemplate
        '
        Me.btnSMSTemplate.Image = Global.SimpleAccounts.My.Resources.Resources.Attach
        Me.btnSMSTemplate.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSMSTemplate.Name = "btnSMSTemplate"
        Me.btnSMSTemplate.Size = New System.Drawing.Size(179, 28)
        Me.btnSMSTemplate.Text = "Setting SMS Template"
        '
        'btnImportAttendance
        '
        Me.btnImportAttendance.Image = Global.SimpleAccounts.My.Resources.Resources.import
        Me.btnImportAttendance.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnImportAttendance.Name = "btnImportAttendance"
        Me.btnImportAttendance.Size = New System.Drawing.Size(158, 24)
        Me.btnImportAttendance.Text = "Import Attendance"
        '
        'btnBarcodeScan
        '
        Me.btnBarcodeScan.Image = Global.SimpleAccounts.My.Resources.Resources.barcode4
        Me.btnBarcodeScan.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnBarcodeScan.Name = "btnBarcodeScan"
        Me.btnBarcodeScan.Size = New System.Drawing.Size(228, 24)
        Me.btnBarcodeScan.Text = "Employee Card Barcode Scan"
        '
        'HelpToolStripButton
        '
        Me.HelpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.HelpToolStripButton.Image = CType(resources.GetObject("HelpToolStripButton.Image"), System.Drawing.Image)
        Me.HelpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.HelpToolStripButton.Name = "HelpToolStripButton"
        Me.HelpToolStripButton.Size = New System.Drawing.Size(24, 24)
        Me.HelpToolStripButton.Text = "He&lp"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 300
        '
        'grd
        '
        Me.grd.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.[False]
        Me.grd.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        grd_DesignTimeLayout.LayoutString = resources.GetString("grd_DesignTimeLayout.LayoutString")
        Me.grd.DesignTimeLayout = grd_DesignTimeLayout
        Me.grd.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic
        Me.grd.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown
        Me.grd.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges
        Me.grd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.grd.Location = New System.Drawing.Point(1, 359)
        Me.grd.Margin = New System.Windows.Forms.Padding(4)
        Me.grd.Name = "grd"
        Me.grd.RecordNavigator = True
        Me.grd.Size = New System.Drawing.Size(1357, 473)
        Me.grd.TabIndex = 21
        Me.ToolTip1.SetToolTip(Me.grd, "Attendance Detail")
        Me.grd.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007
        '
        'RbtOut
        '
        Me.RbtOut.AutoSize = True
        Me.RbtOut.Location = New System.Drawing.Point(231, 242)
        Me.RbtOut.Margin = New System.Windows.Forms.Padding(4)
        Me.RbtOut.Name = "RbtOut"
        Me.RbtOut.Size = New System.Drawing.Size(52, 21)
        Me.RbtOut.TabIndex = 19
        Me.RbtOut.TabStop = True
        Me.RbtOut.Text = "Out"
        Me.ToolTip1.SetToolTip(Me.RbtOut, "Out Time")
        Me.RbtOut.UseVisualStyleBackColor = True
        '
        'RbtIn
        '
        Me.RbtIn.AutoSize = True
        Me.RbtIn.Checked = True
        Me.RbtIn.Location = New System.Drawing.Point(177, 242)
        Me.RbtIn.Margin = New System.Windows.Forms.Padding(4)
        Me.RbtIn.Name = "RbtIn"
        Me.RbtIn.Size = New System.Drawing.Size(40, 21)
        Me.RbtIn.TabIndex = 18
        Me.RbtIn.TabStop = True
        Me.RbtIn.Text = "In"
        Me.ToolTip1.SetToolTip(Me.RbtIn, "In Time")
        Me.RbtIn.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(5, 245)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(126, 17)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Attendance Type"
        '
        'lblAttendanceTime
        '
        Me.lblAttendanceTime.AutoSize = True
        Me.lblAttendanceTime.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAttendanceTime.Location = New System.Drawing.Point(5, 215)
        Me.lblAttendanceTime.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAttendanceTime.Name = "lblAttendanceTime"
        Me.lblAttendanceTime.Size = New System.Drawing.Size(41, 17)
        Me.lblAttendanceTime.TabIndex = 15
        Me.lblAttendanceTime.Text = "Time"
        '
        'lblDepartment
        '
        Me.lblDepartment.AutoSize = True
        Me.lblDepartment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDepartment.Location = New System.Drawing.Point(5, 116)
        Me.lblDepartment.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDepartment.Name = "lblDepartment"
        Me.lblDepartment.Size = New System.Drawing.Size(92, 17)
        Me.lblDepartment.TabIndex = 9
        Me.lblDepartment.Text = "Department"
        '
        'lblDesignation
        '
        Me.lblDesignation.AutoSize = True
        Me.lblDesignation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesignation.Location = New System.Drawing.Point(5, 84)
        Me.lblDesignation.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDesignation.Name = "lblDesignation"
        Me.lblDesignation.Size = New System.Drawing.Size(91, 17)
        Me.lblDesignation.TabIndex = 7
        Me.lblDesignation.Text = "Designation"
        '
        'lblEmployeeName
        '
        Me.lblEmployeeName.AutoSize = True
        Me.lblEmployeeName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmployeeName.Location = New System.Drawing.Point(5, 50)
        Me.lblEmployeeName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblEmployeeName.Name = "lblEmployeeName"
        Me.lblEmployeeName.Size = New System.Drawing.Size(119, 17)
        Me.lblEmployeeName.TabIndex = 5
        Me.lblEmployeeName.Text = "Employee Name"
        '
        'lblAttendanceDate
        '
        Me.lblAttendanceDate.AutoSize = True
        Me.lblAttendanceDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAttendanceDate.Location = New System.Drawing.Point(5, 20)
        Me.lblAttendanceDate.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAttendanceDate.Name = "lblAttendanceDate"
        Me.lblAttendanceDate.Size = New System.Drawing.Size(126, 17)
        Me.lblAttendanceDate.TabIndex = 3
        Me.lblAttendanceDate.Text = "Attendance Date"
        '
        'dtpAttendanceTime
        '
        Me.dtpAttendanceTime.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dtpAttendanceTime.Location = New System.Drawing.Point(177, 210)
        Me.dtpAttendanceTime.Margin = New System.Windows.Forms.Padding(4)
        Me.dtpAttendanceTime.Name = "dtpAttendanceTime"
        Me.dtpAttendanceTime.Size = New System.Drawing.Size(268, 22)
        Me.dtpAttendanceTime.TabIndex = 16
        Me.ToolTip1.SetToolTip(Me.dtpAttendanceTime, "Attendance Time")
        '
        'txtEmpDesignation
        '
        Me.txtEmpDesignation.Enabled = False
        Me.txtEmpDesignation.Location = New System.Drawing.Point(177, 80)
        Me.txtEmpDesignation.Margin = New System.Windows.Forms.Padding(4)
        Me.txtEmpDesignation.Name = "txtEmpDesignation"
        Me.txtEmpDesignation.Size = New System.Drawing.Size(268, 22)
        Me.txtEmpDesignation.TabIndex = 8
        Me.txtEmpDesignation.TabStop = False
        Me.ToolTip1.SetToolTip(Me.txtEmpDesignation, "Designation")
        '
        'txtEmpDepartment
        '
        Me.txtEmpDepartment.Enabled = False
        Me.txtEmpDepartment.Location = New System.Drawing.Point(177, 112)
        Me.txtEmpDepartment.Margin = New System.Windows.Forms.Padding(4)
        Me.txtEmpDepartment.Name = "txtEmpDepartment"
        Me.txtEmpDepartment.Size = New System.Drawing.Size(268, 22)
        Me.txtEmpDepartment.TabIndex = 10
        Me.txtEmpDepartment.TabStop = False
        Me.ToolTip1.SetToolTip(Me.txtEmpDepartment, "Department")
        '
        'dtpAttendanceDate
        '
        Me.dtpAttendanceDate.CustomFormat = "dd/MMM/yyyy"
        Me.dtpAttendanceDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpAttendanceDate.Location = New System.Drawing.Point(177, 15)
        Me.dtpAttendanceDate.Margin = New System.Windows.Forms.Padding(4)
        Me.dtpAttendanceDate.Name = "dtpAttendanceDate"
        Me.dtpAttendanceDate.Size = New System.Drawing.Size(268, 22)
        Me.dtpAttendanceDate.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.dtpAttendanceDate, "Attendance Date")
        '
        'cmbShift
        '
        Me.cmbShift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbShift.FormattingEnabled = True
        Me.cmbShift.Location = New System.Drawing.Point(177, 144)
        Me.cmbShift.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbShift.Name = "cmbShift"
        Me.cmbShift.Size = New System.Drawing.Size(268, 24)
        Me.cmbShift.TabIndex = 12
        Me.ToolTip1.SetToolTip(Me.cmbShift, "Select Shift")
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(5, 148)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(41, 17)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Shift"
        '
        'cmbAttendanceStatus
        '
        Me.cmbAttendanceStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAttendanceStatus.FormattingEnabled = True
        Me.cmbAttendanceStatus.Items.AddRange(New Object() {"Present", "Absent", "Leave", "Half Leave", "Short Leave", "Casual Leave", "Anual Leave", "Sick Leave", "OD", "Break"})
        Me.cmbAttendanceStatus.Location = New System.Drawing.Point(177, 177)
        Me.cmbAttendanceStatus.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbAttendanceStatus.Name = "cmbAttendanceStatus"
        Me.cmbAttendanceStatus.Size = New System.Drawing.Size(268, 24)
        Me.cmbAttendanceStatus.TabIndex = 14
        Me.ToolTip1.SetToolTip(Me.cmbAttendanceStatus, "Status")
        '
        'cmbEmployees
        '
        Me.cmbEmployees.AlwaysInEditMode = True
        Me.cmbEmployees.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Suggest
        Me.cmbEmployees.AutoSuggestFilterMode = Infragistics.Win.AutoSuggestFilterMode.Contains
        Me.cmbEmployees.CheckedListSettings.CheckStateMember = ""
        Me.cmbEmployees.DisplayLayout.InterBandSpacing = 10
        Me.cmbEmployees.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.cmbEmployees.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.cmbEmployees.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.cmbEmployees.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.cmbEmployees.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand
        Me.cmbEmployees.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.cmbEmployees.DisplayLayout.Override.RowSelectorWidth = 12
        Me.cmbEmployees.DisplayLayout.Override.RowSpacingBefore = 2
        Appearance1.BackColor = System.Drawing.Color.FromArgb(CType(CType(129, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(226, Byte), Integer))
        Appearance1.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(221, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(254, Byte), Integer))
        Appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance1.ForeColor = System.Drawing.Color.Black
        Me.cmbEmployees.DisplayLayout.Override.SelectedRowAppearance = Appearance1
        Me.cmbEmployees.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.cmbEmployees.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.cmbEmployees.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Me.cmbEmployees.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.cmbEmployees.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid
        Me.cmbEmployees.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.cmbEmployees.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.cmbEmployees.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl
        Me.cmbEmployees.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.cmbEmployees.LimitToList = True
        Me.cmbEmployees.Location = New System.Drawing.Point(177, 47)
        Me.cmbEmployees.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbEmployees.MaxDropDownItems = 20
        Me.cmbEmployees.Name = "cmbEmployees"
        Me.cmbEmployees.Size = New System.Drawing.Size(269, 25)
        Me.cmbEmployees.TabIndex = 6
        Me.ToolTip1.SetToolTip(Me.cmbEmployees, "Select any Item")
        '
        'cmbSearchByEmployee
        '
        Me.cmbSearchByEmployee.AlwaysInEditMode = True
        Me.cmbSearchByEmployee.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Suggest
        Me.cmbSearchByEmployee.AutoSuggestFilterMode = Infragistics.Win.AutoSuggestFilterMode.Contains
        Me.cmbSearchByEmployee.CheckedListSettings.CheckStateMember = ""
        Me.cmbSearchByEmployee.DisplayLayout.InterBandSpacing = 10
        Me.cmbSearchByEmployee.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.cmbSearchByEmployee.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.cmbSearchByEmployee.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.cmbSearchByEmployee.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.cmbSearchByEmployee.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand
        Me.cmbSearchByEmployee.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.cmbSearchByEmployee.DisplayLayout.Override.RowSelectorWidth = 12
        Me.cmbSearchByEmployee.DisplayLayout.Override.RowSpacingBefore = 2
        Appearance3.BackColor = System.Drawing.Color.FromArgb(CType(CType(129, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(226, Byte), Integer))
        Appearance3.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(221, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(254, Byte), Integer))
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.ForeColor = System.Drawing.Color.Black
        Me.cmbSearchByEmployee.DisplayLayout.Override.SelectedRowAppearance = Appearance3
        Me.cmbSearchByEmployee.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.cmbSearchByEmployee.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.cmbSearchByEmployee.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Me.cmbSearchByEmployee.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.cmbSearchByEmployee.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid
        Me.cmbSearchByEmployee.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.cmbSearchByEmployee.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.cmbSearchByEmployee.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl
        Me.cmbSearchByEmployee.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.cmbSearchByEmployee.LimitToList = True
        Me.cmbSearchByEmployee.Location = New System.Drawing.Point(119, 119)
        Me.cmbSearchByEmployee.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbSearchByEmployee.MaxDropDownItems = 20
        Me.cmbSearchByEmployee.Name = "cmbSearchByEmployee"
        Me.cmbSearchByEmployee.Size = New System.Drawing.Size(588, 25)
        Me.cmbSearchByEmployee.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.cmbSearchByEmployee, "Select any Item")
        '
        'dtpTo
        '
        Me.dtpTo.CustomFormat = "dd/MMM/yyyy"
        Me.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpTo.Location = New System.Drawing.Point(437, 20)
        Me.dtpTo.Margin = New System.Windows.Forms.Padding(4)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Size = New System.Drawing.Size(268, 22)
        Me.dtpTo.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.dtpTo, "Attendance Date")
        '
        'dtpFrom
        '
        Me.dtpFrom.CustomFormat = "dd/MMM/yyyy"
        Me.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFrom.Location = New System.Drawing.Point(119, 20)
        Me.dtpFrom.Margin = New System.Windows.Forms.Padding(4)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Size = New System.Drawing.Size(268, 22)
        Me.dtpFrom.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.dtpFrom, "Attendance Date")
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(5, 181)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(140, 17)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Attendance Status"
        '
        'txtBarCodeEmpId
        '
        Me.txtBarCodeEmpId.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBarCodeEmpId.BackColor = System.Drawing.SystemColors.Info
        Me.txtBarCodeEmpId.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBarCodeEmpId.Location = New System.Drawing.Point(8, 23)
        Me.txtBarCodeEmpId.Margin = New System.Windows.Forms.Padding(4)
        Me.txtBarCodeEmpId.Name = "txtBarCodeEmpId"
        Me.txtBarCodeEmpId.Size = New System.Drawing.Size(980, 36)
        Me.txtBarCodeEmpId.TabIndex = 0
        Me.txtBarCodeEmpId.TabStop = False
        '
        'grbBarcodeScan
        '
        Me.grbBarcodeScan.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grbBarcodeScan.Controls.Add(Me.lblError)
        Me.grbBarcodeScan.Controls.Add(Me.txtBarCodeEmpId)
        Me.grbBarcodeScan.Controls.Add(Me.lblDept)
        Me.grbBarcodeScan.Controls.Add(Me.Label9)
        Me.grbBarcodeScan.Location = New System.Drawing.Point(497, 219)
        Me.grbBarcodeScan.Margin = New System.Windows.Forms.Padding(4)
        Me.grbBarcodeScan.Name = "grbBarcodeScan"
        Me.grbBarcodeScan.Padding = New System.Windows.Forms.Padding(4)
        Me.grbBarcodeScan.Size = New System.Drawing.Size(997, 98)
        Me.grbBarcodeScan.TabIndex = 20
        Me.grbBarcodeScan.TabStop = False
        Me.grbBarcodeScan.Text = "Employee Card Barcode Scan"
        '
        'lblError
        '
        Me.lblError.AutoSize = True
        Me.lblError.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblError.ForeColor = System.Drawing.Color.Red
        Me.lblError.Location = New System.Drawing.Point(11, 73)
        Me.lblError.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblError.Name = "lblError"
        Me.lblError.Size = New System.Drawing.Size(0, 17)
        Me.lblError.TabIndex = 1
        '
        'lblDept
        '
        Me.lblDept.AutoSize = True
        Me.lblDept.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDept.Location = New System.Drawing.Point(65, 54)
        Me.lblDept.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDept.Name = "lblDept"
        Me.lblDept.Size = New System.Drawing.Size(155, 29)
        Me.lblDept.TabIndex = 32
        Me.lblDept.Text = "Department"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(296, 58)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(155, 29)
        Me.Label9.TabIndex = 28
        Me.Label9.Text = "Department"
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Location = New System.Drawing.Point(495, 118)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(812, 441)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 22
        Me.PictureBox1.TabStop = False
        '
        'lblAttendanceType
        '
        Me.lblAttendanceType.BackColor = System.Drawing.Color.Transparent
        Me.lblAttendanceType.Font = New System.Drawing.Font("Verdana", 36.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAttendanceType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAttendanceType.Location = New System.Drawing.Point(16, 322)
        Me.lblAttendanceType.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAttendanceType.Name = "lblAttendanceType"
        Me.lblAttendanceType.Size = New System.Drawing.Size(483, 130)
        Me.lblAttendanceType.TabIndex = 23
        Me.lblAttendanceType.Text = "Out"
        Me.lblAttendanceType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SerialPort1
        '
        Me.SerialPort1.BaudRate = 2400
        Me.SerialPort1.ReadTimeout = 100
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.gbGeneralControls)
        Me.Panel1.Controls.Add(Me.pnlHeader)
        Me.Panel1.Controls.Add(Me.lblSeeU)
        Me.Panel1.Controls.Add(Me.lblWelcome)
        Me.Panel1.Controls.Add(Me.lblDesig)
        Me.Panel1.Controls.Add(Me.lblEmpCode)
        Me.Panel1.Controls.Add(Me.lblEmpName)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.lblAttendanceType)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.grbBarcodeScan)
        Me.Panel1.Location = New System.Drawing.Point(0, 34)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1360, 798)
        Me.Panel1.TabIndex = 24
        '
        'gbGeneralControls
        '
        Me.gbGeneralControls.Controls.Add(Me.lblAttendanceDate)
        Me.gbGeneralControls.Controls.Add(Me.dtpAttendanceDate)
        Me.gbGeneralControls.Controls.Add(Me.txtEmpDepartment)
        Me.gbGeneralControls.Controls.Add(Me.txtEmpDesignation)
        Me.gbGeneralControls.Controls.Add(Me.dtpAttendanceTime)
        Me.gbGeneralControls.Controls.Add(Me.Label3)
        Me.gbGeneralControls.Controls.Add(Me.Label4)
        Me.gbGeneralControls.Controls.Add(Me.cmbEmployees)
        Me.gbGeneralControls.Controls.Add(Me.lblAttendanceTime)
        Me.gbGeneralControls.Controls.Add(Me.cmbAttendanceStatus)
        Me.gbGeneralControls.Controls.Add(Me.lblEmployeeName)
        Me.gbGeneralControls.Controls.Add(Me.lblDesignation)
        Me.gbGeneralControls.Controls.Add(Me.cmbShift)
        Me.gbGeneralControls.Controls.Add(Me.lblDepartment)
        Me.gbGeneralControls.Controls.Add(Me.Label1)
        Me.gbGeneralControls.Controls.Add(Me.RbtOut)
        Me.gbGeneralControls.Controls.Add(Me.RbtIn)
        Me.gbGeneralControls.Location = New System.Drawing.Point(4, 50)
        Me.gbGeneralControls.Margin = New System.Windows.Forms.Padding(4)
        Me.gbGeneralControls.Name = "gbGeneralControls"
        Me.gbGeneralControls.Padding = New System.Windows.Forms.Padding(4)
        Me.gbGeneralControls.Size = New System.Drawing.Size(488, 265)
        Me.gbGeneralControls.TabIndex = 38
        Me.gbGeneralControls.TabStop = False
        '
        'pnlHeader
        '
        Me.pnlHeader.Controls.Add(Me.lblHeader)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(1360, 43)
        Me.pnlHeader.TabIndex = 37
        '
        'lblHeader
        '
        Me.lblHeader.AutoSize = True
        Me.lblHeader.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeader.ForeColor = System.Drawing.Color.Navy
        Me.lblHeader.Location = New System.Drawing.Point(16, 5)
        Me.lblHeader.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(305, 29)
        Me.lblHeader.TabIndex = 24
        Me.lblHeader.Text = "Employee Attendance"
        '
        'lblSeeU
        '
        Me.lblSeeU.BackColor = System.Drawing.Color.Transparent
        Me.lblSeeU.Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSeeU.ForeColor = System.Drawing.Color.Navy
        Me.lblSeeU.Location = New System.Drawing.Point(16, 453)
        Me.lblSeeU.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSeeU.Name = "lblSeeU"
        Me.lblSeeU.Size = New System.Drawing.Size(483, 130)
        Me.lblSeeU.TabIndex = 34
        Me.lblSeeU.Text = "اللہ حافظ"
        Me.lblSeeU.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWelcome
        '
        Me.lblWelcome.BackColor = System.Drawing.Color.Transparent
        Me.lblWelcome.Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWelcome.ForeColor = System.Drawing.Color.Navy
        Me.lblWelcome.Location = New System.Drawing.Point(16, 453)
        Me.lblWelcome.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblWelcome.Name = "lblWelcome"
        Me.lblWelcome.Size = New System.Drawing.Size(483, 130)
        Me.lblWelcome.TabIndex = 33
        Me.lblWelcome.Text = "خوش آمدید"
        Me.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDesig
        '
        Me.lblDesig.AutoSize = True
        Me.lblDesig.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesig.Location = New System.Drawing.Point(268, 239)
        Me.lblDesig.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDesig.Name = "lblDesig"
        Me.lblDesig.Size = New System.Drawing.Size(154, 29)
        Me.lblDesig.TabIndex = 31
        Me.lblDesig.Text = "Designation"
        '
        'lblEmpCode
        '
        Me.lblEmpCode.AutoSize = True
        Me.lblEmpCode.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmpCode.Location = New System.Drawing.Point(268, 197)
        Me.lblEmpCode.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblEmpCode.Name = "lblEmpCode"
        Me.lblEmpCode.Size = New System.Drawing.Size(198, 29)
        Me.lblEmpCode.TabIndex = 30
        Me.lblEmpCode.Text = "Employee Code"
        '
        'lblEmpName
        '
        Me.lblEmpName.AutoSize = True
        Me.lblEmpName.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmpName.Location = New System.Drawing.Point(268, 155)
        Me.lblEmpName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblEmpName.Name = "lblEmpName"
        Me.lblEmpName.Size = New System.Drawing.Size(206, 29)
        Me.lblEmpName.TabIndex = 29
        Me.lblEmpName.Text = "Employee Name"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(16, 239)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(154, 29)
        Me.Label8.TabIndex = 27
        Me.Label8.Text = "Designation"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(16, 197)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(198, 29)
        Me.Label7.TabIndex = 26
        Me.Label7.Text = "Employee Code"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(16, 155)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(206, 29)
        Me.Label6.TabIndex = 25
        Me.Label6.Text = "Employee Name"
        '
        'Timer2
        '
        Me.Timer2.Enabled = True
        Me.Timer2.Interval = 10000
        '
        'gbSearchBy
        '
        Me.gbSearchBy.Controls.Add(Me.cmbSearchByDepartment)
        Me.gbSearchBy.Controls.Add(Me.lblSearchByDepartment)
        Me.gbSearchBy.Controls.Add(Me.btnHideSearchBy)
        Me.gbSearchBy.Controls.Add(Me.btnSearch)
        Me.gbSearchBy.Controls.Add(Me.lblSearchByEmployee)
        Me.gbSearchBy.Controls.Add(Me.cmbSearchByDesignation)
        Me.gbSearchBy.Controls.Add(Me.cmbSearchByEmployee)
        Me.gbSearchBy.Controls.Add(Me.lblSearchByDesignation)
        Me.gbSearchBy.Controls.Add(Me.lblTo)
        Me.gbSearchBy.Controls.Add(Me.dtpTo)
        Me.gbSearchBy.Controls.Add(Me.lblFrom)
        Me.gbSearchBy.Controls.Add(Me.dtpFrom)
        Me.gbSearchBy.Location = New System.Drawing.Point(497, 65)
        Me.gbSearchBy.Margin = New System.Windows.Forms.Padding(4)
        Me.gbSearchBy.Name = "gbSearchBy"
        Me.gbSearchBy.Padding = New System.Windows.Forms.Padding(4)
        Me.gbSearchBy.Size = New System.Drawing.Size(724, 181)
        Me.gbSearchBy.TabIndex = 0
        Me.gbSearchBy.TabStop = False
        Me.gbSearchBy.Text = "Search By"
        '
        'cmbSearchByDepartment
        '
        Me.cmbSearchByDepartment.AlwaysInEditMode = True
        Me.cmbSearchByDepartment.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Suggest
        Me.cmbSearchByDepartment.AutoSuggestFilterMode = Infragistics.Win.AutoSuggestFilterMode.Contains
        Me.cmbSearchByDepartment.CheckedListSettings.CheckStateMember = ""
        Me.cmbSearchByDepartment.DisplayLayout.InterBandSpacing = 10
        Me.cmbSearchByDepartment.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.cmbSearchByDepartment.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.cmbSearchByDepartment.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.cmbSearchByDepartment.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.cmbSearchByDepartment.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand
        Me.cmbSearchByDepartment.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.cmbSearchByDepartment.DisplayLayout.Override.RowSelectorWidth = 12
        Me.cmbSearchByDepartment.DisplayLayout.Override.RowSpacingBefore = 2
        Appearance2.BackColor = System.Drawing.Color.FromArgb(CType(CType(129, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(226, Byte), Integer))
        Appearance2.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(221, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(254, Byte), Integer))
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.ForeColor = System.Drawing.Color.Black
        Me.cmbSearchByDepartment.DisplayLayout.Override.SelectedRowAppearance = Appearance2
        Me.cmbSearchByDepartment.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.cmbSearchByDepartment.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.cmbSearchByDepartment.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Me.cmbSearchByDepartment.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.cmbSearchByDepartment.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid
        Me.cmbSearchByDepartment.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.cmbSearchByDepartment.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.cmbSearchByDepartment.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl
        Me.cmbSearchByDepartment.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.cmbSearchByDepartment.LimitToList = True
        Me.cmbSearchByDepartment.Location = New System.Drawing.Point(119, 50)
        Me.cmbSearchByDepartment.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbSearchByDepartment.MaxDropDownItems = 20
        Me.cmbSearchByDepartment.Name = "cmbSearchByDepartment"
        Me.cmbSearchByDepartment.Size = New System.Drawing.Size(588, 25)
        Me.cmbSearchByDepartment.TabIndex = 5
        '
        'lblSearchByDepartment
        '
        Me.lblSearchByDepartment.AutoSize = True
        Me.lblSearchByDepartment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearchByDepartment.Location = New System.Drawing.Point(8, 55)
        Me.lblSearchByDepartment.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSearchByDepartment.Name = "lblSearchByDepartment"
        Me.lblSearchByDepartment.Size = New System.Drawing.Size(92, 17)
        Me.lblSearchByDepartment.TabIndex = 4
        Me.lblSearchByDepartment.Text = "Department"
        '
        'btnHideSearchBy
        '
        Me.btnHideSearchBy.Location = New System.Drawing.Point(501, 149)
        Me.btnHideSearchBy.Margin = New System.Windows.Forms.Padding(4)
        Me.btnHideSearchBy.Name = "btnHideSearchBy"
        Me.btnHideSearchBy.Size = New System.Drawing.Size(100, 28)
        Me.btnHideSearchBy.TabIndex = 11
        Me.btnHideSearchBy.Text = "Hide"
        Me.btnHideSearchBy.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(607, 149)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 28)
        Me.btnSearch.TabIndex = 10
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'lblSearchByEmployee
        '
        Me.lblSearchByEmployee.AutoSize = True
        Me.lblSearchByEmployee.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearchByEmployee.Location = New System.Drawing.Point(8, 124)
        Me.lblSearchByEmployee.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSearchByEmployee.Name = "lblSearchByEmployee"
        Me.lblSearchByEmployee.Size = New System.Drawing.Size(80, 17)
        Me.lblSearchByEmployee.TabIndex = 8
        Me.lblSearchByEmployee.Text = "Employee "
        '
        'cmbSearchByDesignation
        '
        Me.cmbSearchByDesignation.AlwaysInEditMode = True
        Me.cmbSearchByDesignation.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Suggest
        Me.cmbSearchByDesignation.AutoSuggestFilterMode = Infragistics.Win.AutoSuggestFilterMode.Contains
        Me.cmbSearchByDesignation.CheckedListSettings.CheckStateMember = ""
        Me.cmbSearchByDesignation.DisplayLayout.InterBandSpacing = 10
        Me.cmbSearchByDesignation.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.cmbSearchByDesignation.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.cmbSearchByDesignation.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.cmbSearchByDesignation.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.cmbSearchByDesignation.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand
        Me.cmbSearchByDesignation.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.cmbSearchByDesignation.DisplayLayout.Override.RowSelectorWidth = 12
        Me.cmbSearchByDesignation.DisplayLayout.Override.RowSpacingBefore = 2
        Appearance4.BackColor = System.Drawing.Color.FromArgb(CType(CType(129, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(226, Byte), Integer))
        Appearance4.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(221, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(254, Byte), Integer))
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance4.ForeColor = System.Drawing.Color.Black
        Me.cmbSearchByDesignation.DisplayLayout.Override.SelectedRowAppearance = Appearance4
        Me.cmbSearchByDesignation.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.cmbSearchByDesignation.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.cmbSearchByDesignation.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Me.cmbSearchByDesignation.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.cmbSearchByDesignation.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid
        Me.cmbSearchByDesignation.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.cmbSearchByDesignation.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.cmbSearchByDesignation.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl
        Me.cmbSearchByDesignation.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.cmbSearchByDesignation.LimitToList = True
        Me.cmbSearchByDesignation.Location = New System.Drawing.Point(119, 85)
        Me.cmbSearchByDesignation.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbSearchByDesignation.MaxDropDownItems = 20
        Me.cmbSearchByDesignation.Name = "cmbSearchByDesignation"
        Me.cmbSearchByDesignation.Size = New System.Drawing.Size(588, 25)
        Me.cmbSearchByDesignation.TabIndex = 7
        '
        'lblSearchByDesignation
        '
        Me.lblSearchByDesignation.AutoSize = True
        Me.lblSearchByDesignation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearchByDesignation.Location = New System.Drawing.Point(8, 91)
        Me.lblSearchByDesignation.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSearchByDesignation.Name = "lblSearchByDesignation"
        Me.lblSearchByDesignation.Size = New System.Drawing.Size(91, 17)
        Me.lblSearchByDesignation.TabIndex = 6
        Me.lblSearchByDesignation.Text = "Designation"
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTo.Location = New System.Drawing.Point(403, 25)
        Me.lblTo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(24, 17)
        Me.lblTo.TabIndex = 2
        Me.lblTo.Text = "To"
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFrom.Location = New System.Drawing.Point(8, 26)
        Me.lblFrom.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(44, 17)
        Me.lblFrom.TabIndex = 0
        Me.lblFrom.Text = "From"
        '
        'CtrlGrdBar1
        '
        Me.CtrlGrdBar1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CtrlGrdBar1.BackColor = System.Drawing.Color.Transparent
        Me.CtrlGrdBar1.Email = Nothing
        Me.CtrlGrdBar1.FormName = Me
        Me.CtrlGrdBar1.Location = New System.Drawing.Point(1315, 0)
        Me.CtrlGrdBar1.Margin = New System.Windows.Forms.Padding(4)
        Me.CtrlGrdBar1.MyGrid = Me.grd
        Me.CtrlGrdBar1.Name = "CtrlGrdBar1"
        Me.CtrlGrdBar1.Size = New System.Drawing.Size(45, 33)
        Me.CtrlGrdBar1.TabIndex = 1
        '
        'frmAttendanceEmployees
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(1360, 833)
        Me.Controls.Add(Me.gbSearchBy)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.grd)
        Me.Controls.Add(Me.CtrlGrdBar1)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmAttendanceEmployees"
        Me.Text = "Employee Attendance"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.grd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbEmployees, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbSearchByEmployee, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grbBarcodeScan.ResumeLayout(False)
        Me.grbBarcodeScan.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.gbGeneralControls.ResumeLayout(False)
        Me.gbGeneralControls.PerformLayout()
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.gbSearchBy.ResumeLayout(False)
        Me.gbSearchBy.PerformLayout()
        CType(Me.cmbSearchByDepartment, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbSearchByDesignation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents BtnNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents BtnEdit As System.Windows.Forms.ToolStripButton
    Friend WithEvents BtnSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents BtnPrint As System.Windows.Forms.ToolStripButton
    Friend WithEvents toolStripSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BtnDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents HelpToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents grd As Janus.Windows.GridEX.GridEX
    Friend WithEvents RbtOut As System.Windows.Forms.RadioButton
    Friend WithEvents RbtIn As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblAttendanceTime As System.Windows.Forms.Label
    Friend WithEvents lblDepartment As System.Windows.Forms.Label
    Friend WithEvents lblDesignation As System.Windows.Forms.Label
    Friend WithEvents lblEmployeeName As System.Windows.Forms.Label
    Friend WithEvents lblAttendanceDate As System.Windows.Forms.Label
    Friend WithEvents dtpAttendanceTime As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtEmpDesignation As System.Windows.Forms.TextBox
    Friend WithEvents txtEmpDepartment As System.Windows.Forms.TextBox
    Friend WithEvents dtpAttendanceDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents btnRefresh As System.Windows.Forms.ToolStripButton
    Friend WithEvents CtrlGrdBar1 As SimpleAccounts.CtrlGrdBar
    Friend WithEvents cmbShift As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbAttendanceStatus As System.Windows.Forms.ComboBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnLoadAll As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnImportAttendance As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents txtBarCodeEmpId As System.Windows.Forms.TextBox
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnBarcodeScan As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents grbBarcodeScan As System.Windows.Forms.GroupBox
    Friend WithEvents lblError As System.Windows.Forms.Label
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblAttendanceType As System.Windows.Forms.Label
    Friend WithEvents SerialPort1 As System.IO.Ports.SerialPort
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblDept As System.Windows.Forms.Label
    Friend WithEvents lblDesig As System.Windows.Forms.Label
    Friend WithEvents lblEmpCode As System.Windows.Forms.Label
    Friend WithEvents lblEmpName As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    Friend WithEvents lblWelcome As System.Windows.Forms.Label
    Friend WithEvents lblSeeU As System.Windows.Forms.Label
    Friend WithEvents cmbEmployees As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents btnSMSTemplate As System.Windows.Forms.ToolStripButton
    Friend WithEvents BtnSaveAll As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnFullScreen As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnOutTimeAttendance As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSearchHistory As System.Windows.Forms.ToolStripButton
    Friend WithEvents gbSearchBy As System.Windows.Forms.GroupBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents lblSearchByEmployee As System.Windows.Forms.Label
    Friend WithEvents cmbSearchByDesignation As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents cmbSearchByEmployee As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents lblSearchByDesignation As System.Windows.Forms.Label
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents dtpTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblFrom As System.Windows.Forms.Label
    Friend WithEvents dtpFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents gbGeneralControls As System.Windows.Forms.GroupBox
    Friend WithEvents btnHideSearchBy As System.Windows.Forms.Button
    Friend WithEvents cmbSearchByDepartment As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents lblSearchByDepartment As System.Windows.Forms.Label
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents lblHeader As System.Windows.Forms.Label
End Class
