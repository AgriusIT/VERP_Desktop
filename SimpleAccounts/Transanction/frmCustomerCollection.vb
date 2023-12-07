''16-Dec-2013 R933   Imran Ali           Slow working save/update in transaction forms
''TASK TFS2018 Muhammad Ameen done on 02-01-2018. Posted and Checked by user should be displayed in case exists while on edit mode.
''TASK TFS2701 Ayesha Rehman  on 09-03-2018. Receipt Approval hierarchy
''TFS2988 Ayesha Rehman : 09-04-2018 : If document is approved from one stage then it should not change able for previous stage
''TFS2989 Ayesha Rehman : 10-04-2018 : If document is rejected from one stage then it should open for previous stage for approval
''TFS4689 Ayesha Rehman : 03-10-2018 : Show only relevant Accounts on Transactional screens while User wise COA Configuration.
Imports System.Data
Imports System.Data.OleDb
Imports SBDal
Imports SBModel
Imports SBUtility.Utility
Imports SBUtility
Imports Janus.Windows.GridEX
Imports Neodynamic.SDK.Barcode
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Shared.ExportOptions
Imports CrystalDecisions.Windows.Forms
Imports SBDal.PrintLogDAL
Imports System.Data.SqlClient
Imports Microsoft.VisualStudio.Shell.RegistrationAttribute
Imports Microsoft.Office.Interop
Imports System.Text.RegularExpressions
Imports System.Text

Public Class frmCustomerCollection
    ' Change on 23-11-2013  For Multiple Print Vouchers
    Implements IGeneral
    Dim lngSelectedVoucherId As Long
    Dim blnEditMode As Boolean = False
    Dim blnFirstTimeInvoked As Boolean = False
    Dim Mode As String = "Normal"
    Dim Email As Email
    Dim IsLoadedForm As Boolean = False
    Dim SourceFile As String = String.Empty
    Dim FileName As String = String.Empty
    Dim crpt As New ReportDocument
    Dim GetVoucherId As Integer = 0
    Dim setVoucherNo As String = String.Empty
    Dim setVoucherType As String = String.Empty
    Dim setEditMode As Boolean = False
    Dim Total_Amount As Double = 0D
    Dim Total_Previouse_Amount As Double = 0D
    Dim PrintLog As SBModel.PrintLogBE
    Dim DiscountAccountId As Integer = 0I
    Dim ExchangeGainLossAccountId As Integer = 0I
    Dim flgCompanyRights As Boolean = False
    Dim SelectedMode As Boolean = False
    Dim TaxPayableAccountId As Integer = 0I
    Dim EnabledBrandedSMS As Boolean = False 'Task:2577 Added Flag For Branded SMS
    'Marked Against Task#2015060001 Ali Ansari
    'Dim arrfile As String
    'Marked Against Task#2015060001 Ali Ansari
    'Altered Against Task#2015060001 Ali Ansari
    ' Convert string into List of string for making proper count
    Dim arrFile As List(Of String)
    'Altered Against Task#2015060001 Ali Ansari
    Dim BaseCurrencyId As Integer
    Dim BaseCurrencyName As String = String.Empty
    Dim Msgfrm As New frmMessages
    Dim IsGetAllAllowed As Boolean = False
    Dim IsAdminGroup As Boolean = False
    Dim flgMemoRemarks As Boolean = False
    '#Task1608 added by zainab
    'Dim TaxPer As Integer
    Dim GrossAmount As Integer
    Dim Key As String = String.Empty
    Dim r As Janus.Windows.GridEX.GridEXRow
    Dim AccountId As Integer
    ''TFS2701 : Ayesha Rehman : This Variable is Added to check ApprovalProcessId ,if it is mapped against the document
    Dim ApprovalProcessId As Integer = 0
    ''TFS2701 : Ayesha Rehman :End

    ''TASK TFS3111
    Dim RestrictEntryInParentDetailAC As Boolean = False
    'END TASK TFS3111

    Dim flgSalesInvoice As Boolean
    Public Shared InvoiceId As Integer
    Dim Row_Index As Integer = 0

    Dim EmailTemplate As String = String.Empty
    Dim UsersEmail As List(Of String)
    Dim EmailBody As String = String.Empty
    Dim VoucherNo As String
    Dim VoucherId As Integer
    Dim AllFields As List(Of String)
    Dim AfterFieldsElement As String = String.Empty
    Dim dtEmail As DataTable
    Dim EmailDAL As New EmailTemplateDAL
    Dim html As StringBuilder
    Dim VendorEmails As String = String.Empty
    Public InvoiceCurrencyRate As Double
    Public InvoiceTotalAmount As Double
    Dim ObjMod As InvoicesBasedReceiptMaster
    Public InvoicesBasedReceiptDAL As InvoicesBasedReceiptDAL = New InvoicesBasedReceiptDAL()

    Enum grdEnm
        Voucher_Id
        coa_detail_id
        detail_title
        detail_code
        ''Currency related fields TASK-407
        CurrencyId
        CurrencyAmount
        CurrencyRate
        BaseCurrencyId
        BaseCurrencyRate
        ''End
        Amount
        CurrencyDiscount
        Discount
        IncomeAccountId
        Tax
        TaxCurrencyAmount
        TaxAmount
        '#Task1608 added by zainab
        AccountId
        SalesTaxPer
        SalesTaxAmount
        Sales_Tax
        WHTaxPer
        WHTaxAmount
        WH_Tax
        NetAmount
        Net_Amount
        Reference
        Cheque_No
        Cheque_Date
        BankDescription
        Phone 'Task:2577 Added Index
        Type
        CostCenterId
        Employee
        InvoiceId
        '#Task1608 added by zainab
        'SalesTaxPer
        'SalesTaxAmount
        'NetAmount
    End Enum



    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="Condition"></param>
    ''' <remarks></remarks>
    Sub FillPaymentMethod(Optional ByVal Condition As String = "")
        Try
            blnFirstTimeInvoked = True
            Dim dt As New DataTable
            Dim dr As DataRow
            Dim dr1 As DataRow
            Dim dr2 As DataRow
            dt.Columns.Add("Id")
            dt.Columns.Add("Name")
            dr = dt.NewRow
            dr1 = dt.NewRow
            dr2 = dt.NewRow

            dr(0) = Convert.ToInt32(5)
            dr(1) = "Bank"
            dt.Rows.InsertAt(dr, 0)

            dr1(0) = Convert.ToInt32(3)
            dr1(1) = "Cash"
            dt.Rows.InsertAt(dr1, 0)


            Me.cmbVoucherType.DisplayMember = dt.Columns(1).ColumnName.ToString() 'objDataSet.Tables(0).Columns(1).ColumnName
            Me.cmbVoucherType.ValueMember = dt.Columns(0).ColumnName.ToString() 'objDataSet.Tables(0).Columns(0).ColumnName)
            Me.cmbVoucherType.DataSource = dt



        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="strVoucherType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetVoucherTypeId(ByVal strVoucherType As String) As Long
        Try
            Dim lngVoucherTypeId As Long
            Dim strQuery As String
            strQuery = "SELECT Voucher_Type_ID, Voucher_Type FROM tblDefVoucherType WHERE voucher_type = N'" & strVoucherType & "'"
            Dim objCommand As New OleDbCommand(strQuery, Con)
            lngVoucherTypeId = objCommand.ExecuteScalar

            Return lngVoucherTypeId
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="strCondition"></param>
    ''' <remarks></remarks>
    Sub PopulateGrid(Optional ByVal strCondition As String = "")
        Try
            Dim ClosingDate As DateTime = Convert.ToDateTime(getConfigValueByType("EndOfDate").ToString)
            Dim PreviouseRecordShow As Boolean = Convert.ToBoolean(getConfigValueByType("PreviouseRecordShow").ToString)
            Dim UserName As String = LoginUserName
            Dim strSql As String = String.Empty
            If Mode = "Normal" Then
                strSql = "SELECT  DISTINCT " & IIf(strCondition.ToString = "All", "", "Top 50") & "   tblvoucher.voucher_id, voucher_no, voucher_date, CASE WHEN (voucher_Type_ID = 5) THEN 'Bank' ELSE 'Cash' END AS [Payment Method], tblVoucher.Cheque_No,tblVoucher.Remarks, isnull(ReceiptAmount.Amount,0) as Amount, Post, Case When Post=1 then 'Posted' else 'UnPosted' end as Status,IsNull(tblVoucher.Checked,0) as Checked,Isnull(tblVoucher.Employee_Id,0) as Employee_Id, CASE WHEN ISNULL(PrintLog.cont,0)=0 THEN 'Print Pending' ELSE 'Printed' end as [Print Status], IsNull([No Of Attachment],0) as  [No Of Attachment],tblVoucher.USERNAME  AS 'User Name',CompanyDefTable.CompanyName, Posted_UserName AS PostedUser, CheckedByUser AS CheckedUser " _
                                      & " FROM tblVoucher INNER JOIN tblVoucherDetail ON tblVoucher.Voucher_Id = tblVoucherDetail.Voucher_Id LEFT OUTER JOIN vwCOADetail ON vwCOADetail.coa_detail_id = tblVoucherDetail.coa_detail_id Left Outer join  CompanyDefTable on tblVoucher.location_id=CompanyDefTable.CompanyId    LEFT OUTER JOIN(Select Count(Id) as Cont, DocumentNo From tblPrint_Log Group By DocumentNo) PrintLog On PrintLog.DocumentNo = voucher_no INNER JOIN(Select voucher_id, SUM(ISNULL(credit_amount,0)) as Amount From tblVoucherDetail Group By Voucher_Id) ReceiptAmount On ReceiptAmount.Voucher_Id = tblVoucher.Voucher_Id LEFT OUTER JOIN (Select Count(*) as [No Of Attachment],DocId From DocumentAttachment WHERE (source = N'" & Me.Name & "') Group By DocId, Source) Doc_Att on Doc_Att.DocId = tblVoucher.Voucher_Id " _
                                      & " WHERE source in ('" & Me.Name & "', N'frmOldCustomerCollection')  " & IIf(PreviouseRecordShow = True, "", " AND (Convert(varchar, voucher_date,102) > Convert(datetime, N'" & ClosingDate & "', 102))") & ""
                'End Task#04082015

                'End Task:2854
                'Altered Against Task#201507010 Ali Ansari to add user name field in Grid of all transactions forms
                If flgCompanyRights = True Then
                    strSql += " And tblVoucher.Location_Id=" & MyCompanyId & ""
                End If
            End If
            'If strCondition <> "" Then
            '    ' strSql &= " AND account_type = N'" & strCondition & "'"
            'End If

            If Me.dtpFrom.Checked = True Then
                strSql += " AND tblVoucher.Voucher_Date >= Convert(Datetime, N'" & Me.dtpFrom.Value.ToString("yyyy-M-d 00:00:00") & "', 102) "
            End If
            If Me.dtpTo.Checked = True Then
                strSql += " AND tblVoucher.Voucher_Date <= Convert(Datetime, N'" & Me.dtpTo.Value.ToString("yyyy-M-d 23:59:59") & "', 102) "
            End If
            If Me.cmbSearchVoucherType.SelectedIndex > 0 Then
                strSql += " AND tblVoucher.Voucher_Type_Id=" & Me.cmbSearchVoucherType.SelectedValue
            End If
            If Me.txtSearchVoucherNo.Text <> String.Empty Then
                strSql += " AND tblvoucher.Voucher_No LIKE '%" & Me.txtSearchVoucherNo.Text & "%'"
            End If
            If Me.txtSearchChequeNo.Text <> String.Empty Then
                strSql += " AND tblVoucherDetail.Cheque_No LIKE '%" & Me.txtSearchChequeNo.Text & "%'"
            End If
            If Me.dtpSearchChequeDate.Checked = True Then
                strSql += " AND (Convert(Varchar, tblVoucherDetail.Cheque_Date,102) = Convert(Datetime, N'" & Me.dtpSearchChequeDate.Value.ToString("yyyy-M-d 00:00:00") & "',102))"
            End If
            If Me.txtFromAmount.Text <> String.Empty Then
                If Val(Me.txtFromAmount.Text) > 0 Then
                    strSql += " AND tblVoucherDetail.credit_amount >=" & Val(Me.txtFromAmount.Text) & ""
                End If
            End If
            If Me.txtToAmount.Text <> String.Empty Then
                If Val(Me.txtFromAmount.Text) > 0 Then
                    strSql += " AND tblVoucherDetail.credit_amount <=" & Val(Me.txtToAmount.Text) & ""
                End If
            End If
            If cmbSearchAccount.SelectedRow IsNot Nothing Then
                If Me.cmbSearchAccount.SelectedRow.Cells(0).Value <> 0 Then
                    strSql += " AND tblVoucherDetail.coa_detail_id = " & Me.cmbSearchAccount.Value
                End If
            End If
            If Me.txtSearchComments.Text <> String.Empty Then
                strSql += " AND tblVoucherDetail.Comments LIKE '%" & Me.txtSearchComments.Text & "%'"
            End If
            If UserName.Length > 0 AndAlso IsGetAllAllowed = False AndAlso IsAdminGroup = False Then
                strSql += " And tblVoucher.UserName LIKE '%" & UserName & "%'"
            End If
            strSql = strSql + " order by tblvoucher.voucher_id desc"
            FillGridEx(grdVouchers, strSql, True)

            Me.grdVouchers.RootTable.Columns("No Of Attachment").ColumnType = Janus.Windows.GridEX.ColumnType.Link

            ' Change on 23-11-2013  For Multiple Print Vouchers
            Me.grdVouchers.RootTable.Columns.Add("Column1")
            Me.grdVouchers.RootTable.Columns("Column1").ActAsSelector = True
            Me.grdVouchers.RootTable.Columns("Column1").UseHeaderSelector = True

            '------------------------------'
            Me.grdVouchers.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True
            Me.grdVouchers.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed
            grdVouchers.RootTable.Columns(0).Visible = False  'Voucher ID
            grdVouchers.RootTable.Columns("Post").Visible = False 'Voucher Post 
            grdVouchers.RootTable.Columns("Employee_Id").Visible = False
            grdVouchers.RootTable.Columns("PostedUser").Visible = False
            grdVouchers.RootTable.Columns("CheckedUser").Visible = False
            Me.grdVouchers.RootTable.Columns("voucher_no").Caption = "Vouchr No"
            Me.grdVouchers.RootTable.Columns("voucher_date").Caption = "Date"
            Me.grdVouchers.RootTable.Columns("Amount").FormatString = "N"
            Me.grdVouchers.RootTable.Columns("Amount").TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            Me.grdVouchers.RootTable.Columns("Amount").HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Far
            Me.grdVouchers.RootTable.Columns("Amount").AggregateFunction = Janus.Windows.GridEX.AggregateFunction.Sum
            Me.grdVouchers.RootTable.Columns("Voucher_Date").FormatString = str_DisplayDateFormat
            Me.grdVouchers.AutoSizeColumns()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Sub ClearFields(Optional ByVal condition As String = "")
        Try

            GetSecurityRights()
            If condition.Length > 0 Then
                pnlBank.Enabled = True
                Me.txtAmount.Text = ""
                Me.cmbCostCenter.Enabled = True
                Me.cmbSaleman.SelectedIndex = 0
                Me.txtDiscount.Text = String.Empty
                Me.txtTax.Text = String.Empty
                Me.txtTaxAmount.Text = String.Empty
                Me.txtReference.Text = String.Empty
                Me.txtGrossAmount.Text = "0"
                Me.txtAmount.Text = "0"
                Me.txtTax.Text = "0"
                Me.txtTaxAmount.Text = "0"
                Me.txtSalesTax.Text = "0"
                Me.txtWithHolding.Text = "0"
                Me.txtNetAmount.Text = "0"
                Me.txtSalesTaxAmount.Text = "0"
                Me.txtWithHolding.Text = "0"
                Me.txtWithHoldingTaxAmount.Text = "0"
                Me.cmbAccounts.Rows(0).Activate()
                Me.cmbAccounts.Focus()

                '   Me.grd.RootTable.Columns("CurrencyAmount").Caption = "" & Me.cmbCurrency.Text & " Amount"
                FillCombo("CostCenter")
                FillCombo("IncomeTaxAccount")
                FillCombo("SalesTaxAccount")
                'Ayesha Rehman : TFS2701 : Enable Approval History button only in Eidt Mode
                If blnEditMode = True Then
                    Me.btnApprovalHistory.Visible = True
                    Me.btnApprovalHistory.Enabled = True
                Else
                    Me.btnApprovalHistory.Visible = False
                End If
                'Ayesha Rehman : TFS2701 : End
                ''Ayesha Rehman :TFS2701 :Making Approval Button Enable in Edit Mode
                If Not getConfigValueByType("ReceiptApproval") = "Error" Then
                    ApprovalProcessId = getConfigValueByType("ReceiptApproval")
                End If
                If ApprovalProcessId = 0 Then
                    Me.chkPost.Visible = True
                    Me.chkPost.Enabled = True
                    Me.chkChecked.Visible = True
                    Me.chkChecked.Enabled = True
                    Me.lblPostedBy.Visible = True
                    Me.lblCheckedBy.Visible = True
                Else
                    Me.chkPost.Visible = True
                    Me.chkPost.Enabled = False
                    Me.chkPost.Checked = False
                    Me.chkChecked.Visible = True
                    Me.chkChecked.Enabled = False
                    Me.chkChecked.Checked = False
                    Me.lblPostedBy.Visible = False
                    Me.lblCheckedBy.Visible = False
                End If
                ''Ayesha Rehman :TFS2701 :End

            End If
            'cmbCashAccount.SelectedIndex = 0 'Paid To
            If SelectedMode = True Then
                Me.cmbSaleman.SelectedIndex = Me.cmbSaleman.SelectedIndex
            Else
                Me.cmbSaleman.SelectedIndex = 0
            End If
            If condition.Length = 0 Then
                Me.dtVoucherDate.Focus()
                Me.dtVoucherDate.Enabled = True
                Me.BtnSave.Text = "&Save"
                blnEditMode = False
                lngSelectedVoucherId = 0
                dtVoucherDate.Value = Date.Now 'Voucher Date
                Me.cmbVoucherType.SelectedIndex = 0 'Payment 
                Me.cmbVoucherType.Enabled = True
                cmbVoucherType_SelectedIndexChanged(Me, New EventArgs)
                Me.txtChequeNo.Text = String.Empty
                'Me.grd.Rows.Clear()
                Me.txtChequeNo.Enabled = True
                Me.dtChequeDate.Enabled = True
                Me.cmbBank.Text = String.Empty
                Me.chkAll.Checked = False
                If Not Me.cmbBank.SelectedIndex = -1 Then Me.cmbBank.Text = String.Empty
                'Me.chkAll.Checked = False
                DisplayDetail(-1)
                FillComboByEdit()
                Me.dtChequeDate.Value = Date.Now
                Me.txtDiscount.Text = "0"
                Me.cmbCashAccount.Enabled = True
                txtVoucherNo.Text = GetVoucherNo()
                Me.txtDiscount.Text = "0"
                Me.txtMemo.Text = String.Empty
                'AddHandler grd.Click, AddressOf grd_Click
                FillCombo("SearchVoucherType")
                Me.UltraTabControl1.SelectedTab = Me.UltraTabControl1.Tabs(0).TabPage.Tab
                Me.lblPrintStatus.Text = String.Empty
                'Me.cmbSearchAccount.Rows(0).Activate()
                Me.dtpFrom.Value = Date.Now.AddMonths(-1)
                Me.dtpTo.Value = Date.Now
                Me.dtpSearchChequeDate.Value = Date.Now
                Me.dtpFrom.Checked = False
                Me.dtpTo.Checked = False
                Me.dtpSearchChequeDate.Checked = False
                Me.cmbSearchVoucherType.SelectedIndex = 0
                Me.txtSearchVoucherNo.Text = String.Empty
                Me.txtSearchChequeNo.Text = String.Empty
                Me.txtFromAmount.Text = String.Empty
                Me.txtToAmount.Text = String.Empty
                Me.txtGrossAmount.Text = "0"
                Me.txtAmount.Text = "0"
                Me.txtTax.Text = "0"
                Me.txtTaxAmount.Text = "0"
                Me.txtSalesTax.Text = "0"
                Me.txtWithHolding.Text = "0"
                Me.txtNetAmount.Text = "0"
                Me.txtSalesTaxAmount.Text = "0"
                Me.txtWithHolding.Text = "0"
                Me.txtWithHoldingTaxAmount.Text = "0"
                FillCombo("CostCenter")
                FillCombo("IncomeTaxAccount")
                FillCombo("SalesTaxAccount")
                Me.txtSearchComments.Text = String.Empty
                Me.SplitContainer1.Panel1Collapsed = True
                PopulateGrid()
                'Hide Buttons Edit,Delete and Print on Load Form
                Me.BtnEdit.Visible = False
                Me.BtnDelete.Visible = False
                Me.BtnPrint.Visible = False
                ''''''''''''''''''''''''''''''''''''''''''''''''''

                arrFile = New List(Of String)
                'Altered Against Task#2015060001 Ali Ansari
                Me.btnAttachment.Text = "Attachment"
                Me.chkEnableDepositAc.Visible = False
                Me.chkEnableDepositAc.Checked = False
                btnUpdateTimes.Text = "No of update times"
                Me.btnUpdateTimes.Visible = False
                FillCombo("Currency")
                ''TASK TFS1474
                Me.cmbCurrency.SelectedValue = BaseCurrencyId
                Me.cmbCurrency.Enabled = True

                'Me.txtCurrentRate.Enabled = False
                ''END TASK TFS1474
                'Ayesha Rehman : TFS2701 : Enable Approval History button only in Eidt Mode
                If blnEditMode = True Then
                    Me.btnApprovalHistory.Visible = True
                    Me.btnApprovalHistory.Enabled = True
                Else
                    Me.btnApprovalHistory.Visible = False
                End If
                'Ayesha Rehman : TFS2701 : End
                ''Ayesha Rehman :TFS2701 :Making Approval Button Enable in Edit Mode
                If Not getConfigValueByType("ReceiptApproval") = "Error" Then
                    ApprovalProcessId = getConfigValueByType("ReceiptApproval")
                End If
                If ApprovalProcessId = 0 Then
                    Me.chkPost.Visible = True
                    Me.chkPost.Enabled = True
                    Me.chkChecked.Visible = True
                    Me.chkChecked.Enabled = True
                    Me.lblPostedBy.Visible = True
                    Me.lblCheckedBy.Visible = True
                Else
                    Me.chkPost.Visible = True
                    Me.chkPost.Enabled = False
                    Me.chkPost.Checked = False
                    Me.chkChecked.Visible = True
                    Me.chkChecked.Enabled = False
                    Me.chkChecked.Checked = False
                    Me.lblPostedBy.Visible = False
                    Me.lblCheckedBy.Visible = False
                End If
                ''Ayesha Rehman :TFS2701 :End
                flgSalesInvoice = False
                lblInvoice.Visible = False
                cmbInvoice.Visible = False
            End If
            ''TASK TFS2018
            Me.btnReplace.Visible = False
            Me.btnAdd.Visible = True
            Me.btnCancel.Visible = False
            Me.chkChecked.Text = "Checked"
            Me.chkPost.Text = "Posted"
            Me.lblPostedBy.Text = ""
            Me.lblCheckedBy.Text = ""

            ''END TASK TFS2018

            'TASK TFS3111
            If Not getConfigValueByType("RestrictEntryInParentDetailAC").ToString = "Error" Then
                RestrictEntryInParentDetailAC = CBool(getConfigValueByType("RestrictEntryInParentDetailAC"))
            End If
            ''END TASK TFS3111
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetVoucherNo() As String 
        'rafay
        Dim date1 As DateTime = "2021-6-30 23:59:59"
        Dim date2 As DateTime = "2021-12-31 23:59:59"
        'rafay end
        Dim docNo As String = String.Empty
        Dim VType As String = String.Empty
        Try
            If Me.cmbVoucherType.SelectedIndex > 0 Then
                'Task#03082015 Concatenate iif condition with prefix
                VType = "BRV" & IIf(getConfigValueByType("CompanyWisePrefix").ToString = "True", Me.cmbCompany.SelectedValue, String.Empty)
            Else
                VType = "CRV" & IIf(getConfigValueByType("CompanyWisePrefix").ToString = "True", Me.cmbCompany.SelectedValue, String.Empty)
                'End Task#03082015 
            End If
            If getConfigValueByType("VoucherNo").ToString = "Yearly" Then
                'Rafay
                If CompanyPrefix = "V-ERP (UAE)" Then
                    'companyinitials = "UE"
                    'Agar tou date 1 jan say less ho tou wo puranay walay serial no show karay warna naye serial no ka format use karain
                    If dtVoucherDate.Value.ToString("yyyy-M-d 00:00:00") <= date2 Then
                        Return GetSerialNo(VType + "-" + Microsoft.VisualBasic.Right(Me.dtVoucherDate.Value.Year, 2) + "-", "tblVoucher", "voucher_no")
                    Else
                        Return GetSerialNo(VType + "-" + Microsoft.VisualBasic.Right(Me.dtVoucherDate.Value.Year, 2) + "-", "tblVoucher", "voucher_no")
                        '   Return GetSerialNo(VType + "-" & companyinitials & "-" + Microsoft.VisualBasic.Right(Me.dtVoucherDate.Value.Year, 2) + "-", "tblVoucher", "voucher_no")
                    End If
                Else
                    ''companyinitials = "PK"
                    'Agar tou date 30 june say less ho tou wo puranay walay serial no show karay warna naye serial no ka format use karain
                    If dtVoucherDate.Value.ToString("yyyy-M-d 00:00:00") <= date1 Then
                        'Return GetNextDocNo(VType & "-" & Format(Me.dtVoucherDate.Value, "yy") & Me.dtVoucherDate.Value.Month.ToString("00"), 4, "tblVoucher", "voucher_no")
                        Return GetNextDocNo(VType & "-" & Format(Me.dtVoucherDate.Value, "yy"), 4, "tblVoucher", "voucher_no")
                    Else
                        Return GetNextDocNo(VType & "-" & companyinitials & "-" & Format(Me.dtVoucherDate.Value, "yy"), 4, "tblVoucher", "voucher_no")
                    End If

                End If
                '' Return GetSerialNo(VType + "-" + Microsoft.VisualBasic.Right(Me.dtVoucherDate.Value.Year, 2) + "-", "tblVoucher", "voucher_no")
                'Rafay
            Else
                Dim strSQL As String = "Select * from ConfigValuesTable Where Config_type='VoucherNo'"
                Dim dr As DataRow = SBDal.UtilityDAL.ReturnDataRow(strSQL)
                If Not dr Is Nothing Then
                    If dr("config_Value") = "Monthly" Then
                        'Rafay
                        'Return GetNextDocNo(VType & "-" & Format(Me.dtVoucherDate.Value, "yy") & Me.dtVoucherDate.Value.Month.ToString("00"), 4, "tblVoucher", "voucher_no")
                        If CompanyPrefix = "V-ERP (UAE)" Then
                            ' companyinitials = "UE"
                            'Return GetSerialNo(VType + "-" + Microsoft.VisualBasic.Right(Me.dtVoucherDate.Value.Year, 2) + "-", "tblVoucher", "voucher_no")
                            'Agar tou date 1 jan say less ho tou wo puranay walay serial no show karay warna naye serial no ka format use karain
                            If dtVoucherDate.Value.ToString("yyyy-M-d 00:00:00") <= date2 Then
                                Return GetSerialNo(VType + "-" + Microsoft.VisualBasic.Right(Me.dtVoucherDate.Value.Year, 2) + "-", "tblVoucher", "voucher_no")
                            Else
                                Return GetSerialNo(VType + "-" + Microsoft.VisualBasic.Right(Me.dtVoucherDate.Value.Year, 2) + "-", "tblVoucher", "voucher_no")
                                'Return GetSerialNo(VType + "-" & companyinitials & "-" + Microsoft.VisualBasic.Right(Me.dtVoucherDate.Value.Year, 2) + "-", "tblVoucher", "voucher_no")
                            End If
                        Else
                            ''companyinitials = "PK"
                            'Agar tou date 30 june say less ho tou wo puranay walay serial no show karay warna naye serial no ka format use karain
                            If dtVoucherDate.Value.ToString("yyyy-M-d 00:00:00") <= date1 Then
                                'Return GetNextDocNo(VType & "-" & Format(Me.dtVoucherDate.Value, "yy") & Me.dtVoucherDate.Value.Month.ToString("00"), 4, "tblVoucher", "voucher_no")
                                Return GetNextDocNo(VType & "-" & Format(Me.dtVoucherDate.Value, "yy"), 4, "tblVoucher", "voucher_no")
                            Else
                                Return GetNextDocNo(VType & "-" & companyinitials & "-" & Format(Me.dtVoucherDate.Value, "yy"), 4, "tblVoucher", "voucher_no")
                            End If

                        End If
                        'rafay
                    Else
                        docNo = GetNextDocNo(VType, 6, "tblVoucher", "voucher_no")
                        Return docNo
                    End If
                Else
                    docNo = GetNextDocNo(VType, 6, "tblVoucher", "voucher_no")
                    Return docNo
                End If
                Return ""
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveRecord()
        'Validation on Configuration Request No 826
        'by Imran Ali 25-9-2013
        '
        Try
            Me.lblProgress.Text = "Processing Please Wait ..."
            Me.lblProgress.Visible = True
            Application.DoEvents()
            Dim objCommand As New OleDbCommand
            Dim objTrans As OleDbTransaction
            Dim lngVoucherMasterId As Long

            If ApprovalProcessId = 0 Then
                If Me.chkPost.Visible = False Then
                    Me.chkPost.Checked = False
                End If
                ''Start TFS2701
                If Me.chkChecked.Visible = False Then
                    Me.chkChecked.Checked = False
                End If
                ''End 
            Else
                'If Me.chkPost.Visible = False Then
                '    Me.chkPost.Checked = False
                'End If
                ' ''Start TFS2701
                'If Me.chkChecked.Visible = False Then
                '    Me.chkChecked.Checked = False
                'End If
                ' ''End 
            End If

            Dim blnCashOptionDetail As Boolean = False
            If Not getConfigValueByType("CashAccountOptionForDetail").ToString = "Error" Then
                blnCashOptionDetail = getConfigValueByType("CashAccountOptionForDetail")
            End If

            If Not getConfigValueByType("SalesDiscountAccount").ToString = "Error" Then
                DiscountAccountId = Convert.ToInt32(getConfigValueByType("SalesDiscountAccount").ToString)
            End If
            If Not getConfigValueByType("taxpayableACid").ToString = "Error" Then
                TaxPayableAccountId = Val(getConfigValueByType("taxpayableACid").ToString)
            End If
            If Not getConfigValueByType("ExchangeGainLossAccount").ToString = "Error" Then
                ExchangeGainLossAccountId = Convert.ToInt32(getConfigValueByType("ExchangeGainLossAccount").ToString)
            End If

            ''TASK : TFS1265
            If Not getConfigValueByType("MemoRemarks").ToString = "Error" Then
                flgMemoRemarks = getConfigValueByType("MemoRemarks")
            End If
            ''End TASK: TFS1265

            If Val(Me.txtDiscount.Text) <> 0 Then 'Discount Grater than zeror
                If DiscountAccountId <= 0 Then 'Diccount Account Id if Less Than Or Equal is Zero
                    ShowErrorMessage("Disccount account is not map")
                    Me.txtDiscount.Focus()
                    Exit Function
                End If
            End If

            If Val(Me.txtTax.Text) <> 0 Then 'Tax Grater than zeror
                If TaxPayableAccountId <= 0 Then 'Tax Account Id if Less Than Or Equal is Zero
                    ShowErrorMessage("Tax account is not map")
                    Me.txtTax.Focus()
                    Exit Function
                End If
            End If
            'If Me.cmbCashAccount.SelectedIndex < 0 Then 'Tax Grater than zeror
            '    'Tax Account Id if Less Than Or Equal is Zero
            '    ShowErrorMessage("Please select a account")
            '    Me.cmbCashAccount.Focus()
            '    Exit Sub
            'End If
            Dim dtchk As New DataTable

            dtchk = GetDataTable("Select Count(*) from ReceivedChequeAdjustmentTable WHERE cheque_voucher_id=" & lngSelectedVoucherId & "")
            dtchk.AcceptChanges()
            If dtchk.HasErrors = False Then
                If Val(dtchk.Rows(0).Item(0).ToString) > 0 Then
                    ShowErrorMessage(str_ErrorDependentUpdateRecordFound)
                    Exit Function
                End If
            End If


            Dim enableChequeBook As Boolean = getConfigValueByType("EnableAutoChequeBook")

            Dim strChequeNo As String = String.Empty
            Dim Cheque_Date As DateTime = Nothing
            Dim strBankDescription As String = String.Empty
            If Not Me.blnEditMode Then
                ''If Me.cmbVoucherType.SelectedIndex > 0 Then
                ''    Me.txtVoucherNo.Text = GetNextDocNo("BRV", 6, "tblVoucher", "voucher_no")
                ''Else
                ''    Me.txtVoucherNo.Text = GetNextDocNo("CRV", 6, "tblVoucher", "voucher_no")
                '' End If

                Me.txtVoucherNo.Text = GetVoucherNo()
                setVoucherNo = Me.txtVoucherNo.Text
                setEditMode = False
            End If
            If Con.State = ConnectionState.Closed Or Con.State = ConnectionState.Broken Then Con.Open()
            objTrans = Con.BeginTransaction
            objCommand.Connection = Con
            objCommand.Transaction = objTrans
            Try
                Dim coaID As Integer = 0
                If Me.cmbVoucherType.Text = "Bank" Then

                    For Each r As Janus.Windows.GridEX.GridEXRow In Me.grd.GetRows
                        coaID = r.Cells("coa_detail_id").Value
                    Next
                End If
                For Each r As Janus.Windows.GridEX.GridEXRow In Me.grd.GetRows
                    If flgSalesInvoice = True Then
                        Dim str As String
                        str = "select voucher_id from tblvoucher where voucher_no = (SELECT SalesNo from salesmastertable where salesid = " & Val(r.Cells("InvoiceId").Value.ToString()) & ")"
                        Dim dt As DataTable = GetDataTable(str)
                        Dim sid As Integer
                        If dt.Rows.Count > 0 Then
                            sid = dt.Rows(0).Item("voucher_id")
                        End If
                    End If
                Next
                If Not Me.blnEditMode Then

                    objCommand.CommandText = String.Empty
                    objCommand.CommandText = "INSERT INTO tblVoucher(location_id, finiancial_year_id, voucher_type_id, voucher_no, voucher_date, " _
                                            & " post,source, coa_detail_id, UserName,Posted_UserName, Employee_Id,Checked, CheckedByUser,Remarks, PostedByDate, CheckedByDate)" _
                                            & " VALUES(" & IIf(flgCompanyRights = True, "" & MyCompanyId & "", Me.cmbCompany.SelectedValue.ToString) & ", 1, " & cmbVoucherType.SelectedValue & ", N'" & txtVoucherNo.Text & "', N'" & dtVoucherDate.Value.ToString("yyyy-M-d h:mm:ss tt") & "', " _
                                            & " " & IIf(Me.chkPost.Checked = True, 1, 0) & ", N'" & Me.Name & "', " & coaID & ", N'" & LoginUserName & "', " & IIf(Me.chkPost.Checked = True, "N'" & LoginUserName & "'", "NULL") & ", " & Me.cmbSaleman.SelectedValue & "," & IIf(Me.chkChecked.Checked = True, 1, 0) & ", " & IIf(Me.chkChecked.Checked = True, "N'" & LoginUserName.Replace("'", "''") & "'", "NULL") & ",N'" & Me.txtMemo.Text.Replace("'", "''") & "', " & IIf(Me.chkChecked.Checked = True, "N'" & Now.ToString("yyyy-M-d h:mm:ss tt") & "'", "NULL") & ", " & IIf(Me.chkPost.Checked = True, "N'" & Now.ToString("yyyy-M-d h:mm:ss tt") & "'", "NULL") & " " _
                                            & ") SELECT @@IDENTITY"
                    lngVoucherMasterId = objCommand.ExecuteScalar
                    If arrFile.Count > 0 Then
                        SaveDocument(lngVoucherMasterId, Me.Name, objTrans)
                    End If
                    'Altered Against Task#2015060001 Ali Ansari
                Else
                    setEditMode = True
                    setVoucherNo = Me.txtVoucherNo.Text
                    objCommand.CommandText = String.Empty

                    If getConfigValueByType("EnabledDuplicateVoucherLog").ToString.ToUpper = "TRUE" Then
                        Call CreateDuplicationVoucher(lngSelectedVoucherId, "Update", objTrans) 'TASKM2710151 Call Create Duplicate Voucher Function
                    End If

                    objCommand.CommandText = "update tblVoucher set location_id = " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", Me.cmbCompany.SelectedValue.ToString) & " , finiancial_year_id = 1 , voucher_type_id = " & cmbVoucherType.SelectedValue & "" _
                                         & " , voucher_no =N'" & txtVoucherNo.Text & "' , voucher_date = N'" & dtVoucherDate.Value.ToString("yyyy-M-d h:mm:ss tt") & "', " _
                                        & " post =" & IIf(Me.chkPost.Checked = True, 1, 0) & " ,source = N'" & Me.Name & "', coa_detail_id= " & coaID & "  " & IIf(Me.chkPost.Checked = False, " , UserName=N'" & LoginUserName & "'", "") & ", " _
                                        & " Posted_UserName=" & IIf(Me.chkPost.Checked = True, "N'" & LoginUserName & "'", "NULL") & ", Employee_Id=" & Me.cmbSaleman.SelectedValue & ", Checked=" & IIf(Me.chkChecked.Checked = True, 1, 0) & ", CheckedByUser=" & IIf(Me.chkChecked.Checked = True, "N'" & LoginUserName.Replace("'", "''") & "'", "NULL") & ",Remarks=N'" & Me.txtMemo.Text.Replace("'", "''") & "',  PostedByDate = " & IIf(Me.chkChecked.Checked = True, "N'" & Now.ToString("yyyy-M-d h:mm:ss tt") & "'", "NULL") & ", CheckedByDate =  " & IIf(Me.chkPost.Checked = True, "N'" & Now.ToString("yyyy-M-d h:mm:ss tt") & "'", "NULL") & "  where voucher_id = " & lngSelectedVoucherId & " "
                    'End Task:M101
                    objCommand.ExecuteNonQuery()
                    lngVoucherMasterId = lngSelectedVoucherId

                    If arrFile.Count > 0 Then
                        SaveDocument(Val(lngVoucherMasterId), Me.Name, objTrans)
                    End If

                    If enableChequeBook = True Then
                        objCommand.Transaction = objTrans
                        objCommand.CommandText = String.Empty
                        objCommand.CommandText = "Update ChequeDetailTable Set VoucherDetailId=0,Cheque_Issued=0  From  ChequeDetailTable, tblVoucherDetail WHERE tblVoucherDetail.Voucher_Detail_Id = ChequeDetailTable.VoucherDetailId AND tblVoucherDetail.Voucher_Id=" & lngVoucherMasterId & ""
                        'End Task:2383
                        objCommand.ExecuteNonQuery()

                    End If

                    'Delete From Detail Voucher
                    objCommand.Transaction = objTrans
                    objCommand.CommandText = String.Empty
                    objCommand.CommandText = "DELETE FROM tblVoucherDetail WHERE voucher_id = " & lngSelectedVoucherId
                    objCommand.ExecuteNonQuery()

                End If



                Dim strMultiChequeDetail As String = String.Empty 'Task:2443  e.g Multi Cheque Detail Value Store 



                For Each r As Janus.Windows.GridEX.GridEXRow In Me.grd.GetRows

                    '***********************
                    'Inserting Credit Amount
                    '***********************
                    strChequeNo = r.Cells("Cheque_No").Value.ToString

                    Cheque_Date = IIf(r.Cells("Cheque_No").Value.ToString = "", Nothing, IIf(r.Cells("Cheque_Date").Value Is DBNull.Value, Now, r.Cells("Cheque_Date").Value))
                    If BtnSave.Text <> "&Save" Then
                        If strChequeNo.Length = 0 AndAlso Cheque_Date <> Date.MinValue Then
                            Throw New Exception("Please Enter Cheque No.")
                        End If
                    End If


                    If r.Cells("Type").Value.ToString.Trim.ToUpper = "LC" Then
                        If Val(r.Cells("CostCenterId").Value.ToString) = 0 Then
                            Throw New Exception("Please select cost center.")
                        End If
                    End If

                    If Val(r.Cells(grdEnm.Tax).Value.ToString) > 0 AndAlso Val(r.Cells(grdEnm.IncomeAccountId).Value.ToString) = Val(0) Then
                        If Val(r.Cells(grdEnm.IncomeAccountId).Value.ToString) = Val(0) Then
                            GetVoucherId = 0
                            Throw New Exception("Please select Income Tax Account.")
                        End If
                    End If

                    If Val(r.Cells(grdEnm.SalesTaxPer).Value.ToString) > 0 AndAlso Val(r.Cells(grdEnm.AccountId).Value.ToString) = Val(0) Then
                        If Val(r.Cells(grdEnm.AccountId).Value.ToString) = Val(0) Then
                            GetVoucherId = 0
                            Throw New Exception("Please select Sales Tax Account.")
                        End If
                    End If

                    strBankDescription = r.Cells("BankDescription").Value.ToString
                    objCommand = New OleDbCommand
                    objCommand.Connection = Con
                    objCommand.Transaction = objTrans
                    Dim objId As Object
                    If flgSalesInvoice = True Then
                        If InvoiceCurrencyRate <> Val(r.Cells("CurrencyRate").Value.ToString) Then
                            If InvoiceTotalAmount - Val(r.Cells("Amount").Value) > 0 Then
                                objCommand.CommandText = String.Empty
                                objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, ChequeDescription,Adjustment, CostCenterId, EmpID, InvoiceId, Cheque_No, Cheque_Date, BankDescription, Tax_Percent,Tax_Amount,SalesTaxPer,SalesTaxAmount,WHTaxPer,WHTaxAmount,NetAmount,contra_coa_detail_id, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_debit_amount, Currency_credit_amount, Currency_Symbol,AccountId,IncomeAccountId) " _
                                                      & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & r.Cells("coa_detail_id").Value & ", " & 0 & ",  " & InvoiceTotalAmount & ", N'" & r.Cells("Reference").Value.ToString.Trim.Replace("'", "''") & "', N'" & GetComments(r).Replace("Party Name.", "").Replace("" & r.Cells("detail_title").Value.ToString & "", "").Replace("'", "''") & "', " & Val(r.Cells("CurrencyDiscount").Value) & ", " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & Val(r.Cells("Employee").Value.ToString()) & "," & Val(r.Cells("InvoiceId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & ", " & Val(r.Cells("Tax").Value.ToString) & "," & Val(r.Cells("Tax_Amount").Value.ToString) & "," & Val(r.Cells("SalesTaxPer").Value.ToString) & "," & Val(r.Cells("SalesTaxAmount").Value.ToString) & "," & Val(r.Cells("WHTaxPer").Value.ToString) & "," & Val(r.Cells("WHTaxAmount").Value.ToString) & "," & Val(r.Cells("NetAmount").Value.ToString) & "," _
                                                      & Me.cmbCashAccount.SelectedValue & ", " & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString) & ", 0, " & InvoiceTotalAmount / InvoiceCurrencyRate & ", '" & Me.cmbCurrency.Text & "', " & Val(r.Cells("AccountId").Value) & ", " & Val(r.Cells("IncomeAccountId").Value) & ") Select @@Identity"
                                objCommand.Transaction = objTrans
                                objId = objCommand.ExecuteScalar()

                                objCommand.CommandText = String.Empty
                                objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, ChequeDescription,Adjustment, CostCenterId, EmpID, InvoiceId, Cheque_No, Cheque_Date, BankDescription, Tax_Percent,Tax_Amount,SalesTaxPer,SalesTaxAmount,WHTaxPer,WHTaxAmount,NetAmount,contra_coa_detail_id, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_debit_amount, Currency_credit_amount, Currency_Symbol,AccountId,IncomeAccountId) " _
                                                      & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & ExchangeGainLossAccountId & ", " & InvoiceTotalAmount - Val(r.Cells("Amount").Value.ToString) & ",  " & 0 & ", N'" & r.Cells("Reference").Value.ToString.Trim.Replace("'", "''") & "', N'" & GetComments(r).Replace("Party Name.", "").Replace("" & r.Cells("detail_title").Value.ToString & "", "").Replace("'", "''") & "', " & Val(r.Cells("CurrencyDiscount").Value) & ", " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & Val(r.Cells("Employee").Value.ToString()) & "," & Val(r.Cells("InvoiceId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & ", " & Val(r.Cells("Tax").Value.ToString) & "," & Val(r.Cells("Tax_Amount").Value.ToString) & "," & Val(r.Cells("SalesTaxPer").Value.ToString) & "," & Val(r.Cells("SalesTaxAmount").Value.ToString) & "," & Val(r.Cells("WHTaxPer").Value.ToString) & "," & Val(r.Cells("WHTaxAmount").Value.ToString) & "," & Val(r.Cells("NetAmount").Value.ToString) & "," _
                                                      & Me.cmbCashAccount.SelectedValue & ", " & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString) & ", " & 0 & ", " & 0 & ", '" & Me.cmbCurrency.Text & "', " & Val(r.Cells("AccountId").Value) & ", " & Val(r.Cells("IncomeAccountId").Value) & ") Select @@Identity"
                                objCommand.Transaction = objTrans
                                objId = objCommand.ExecuteScalar()

                            ElseIf InvoiceTotalAmount - Val(r.Cells("Amount").Value) < 0 Then
                                objCommand.CommandText = String.Empty
                                objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, ChequeDescription,Adjustment, CostCenterId, EmpID, InvoiceId, Cheque_No, Cheque_Date, BankDescription, Tax_Percent,Tax_Amount,SalesTaxPer,SalesTaxAmount,WHTaxPer,WHTaxAmount,NetAmount,contra_coa_detail_id, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_debit_amount, Currency_credit_amount, Currency_Symbol,AccountId,IncomeAccountId) " _
                                                      & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & r.Cells("coa_detail_id").Value & ", " & 0 & ",  " & InvoiceTotalAmount & ", N'" & r.Cells("Reference").Value.ToString.Trim.Replace("'", "''") & "', N'" & GetComments(r).Replace("Party Name.", "").Replace("" & r.Cells("detail_title").Value.ToString & "", "").Replace("'", "''") & "', " & Val(r.Cells("CurrencyDiscount").Value) & ", " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & Val(r.Cells("Employee").Value.ToString()) & "," & Val(r.Cells("InvoiceId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & ", " & Val(r.Cells("Tax").Value.ToString) & "," & Val(r.Cells("Tax_Amount").Value.ToString) & "," & Val(r.Cells("SalesTaxPer").Value.ToString) & "," & Val(r.Cells("SalesTaxAmount").Value.ToString) & "," & Val(r.Cells("WHTaxPer").Value.ToString) & "," & Val(r.Cells("WHTaxAmount").Value.ToString) & "," & Val(r.Cells("NetAmount").Value.ToString) & "," _
                                                      & Me.cmbCashAccount.SelectedValue & ", " & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString) & ", 0, " & InvoiceTotalAmount / InvoiceCurrencyRate & ", '" & Me.cmbCurrency.Text & "', " & Val(r.Cells("AccountId").Value) & ", " & Val(r.Cells("IncomeAccountId").Value) & ") Select @@Identity"
                                objCommand.Transaction = objTrans
                                objId = objCommand.ExecuteScalar()

                                objCommand.CommandText = String.Empty
                                objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, ChequeDescription,Adjustment, CostCenterId, EmpID, InvoiceId, Cheque_No, Cheque_Date, BankDescription, Tax_Percent,Tax_Amount,SalesTaxPer,SalesTaxAmount,WHTaxPer,WHTaxAmount,NetAmount,contra_coa_detail_id, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_debit_amount, Currency_credit_amount, Currency_Symbol,AccountId,IncomeAccountId) " _
                                                      & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & ExchangeGainLossAccountId & ", " & 0 & ",  " & Val(r.Cells("Amount").Value.ToString) - InvoiceTotalAmount & ", N'" & r.Cells("Reference").Value.ToString.Trim.Replace("'", "''") & "', N'" & GetComments(r).Replace("Party Name.", "").Replace("" & r.Cells("detail_title").Value.ToString & "", "").Replace("'", "''") & "', " & Val(r.Cells("CurrencyDiscount").Value) & ", " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & Val(r.Cells("Employee").Value.ToString()) & "," & Val(r.Cells("InvoiceId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & ", " & Val(r.Cells("Tax").Value.ToString) & "," & Val(r.Cells("Tax_Amount").Value.ToString) & "," & Val(r.Cells("SalesTaxPer").Value.ToString) & "," & Val(r.Cells("SalesTaxAmount").Value.ToString) & "," & Val(r.Cells("WHTaxPer").Value.ToString) & "," & Val(r.Cells("WHTaxAmount").Value.ToString) & "," & Val(r.Cells("NetAmount").Value.ToString) & "," _
                                                      & Me.cmbCashAccount.SelectedValue & ", " & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString) & ", " & 0 & ", " & 0 & ", '" & Me.cmbCurrency.Text & "', " & Val(r.Cells("AccountId").Value) & ", " & Val(r.Cells("IncomeAccountId").Value) & ") Select @@Identity"
                                objCommand.Transaction = objTrans
                                objId = objCommand.ExecuteScalar()
                            End If
                        Else
                            objCommand.CommandText = String.Empty
                            objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, ChequeDescription,Adjustment, CostCenterId, EmpID, InvoiceId, Cheque_No, Cheque_Date, BankDescription, Tax_Percent,Tax_Amount,SalesTaxPer,SalesTaxAmount,WHTaxPer,WHTaxAmount,NetAmount,contra_coa_detail_id, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_debit_amount, Currency_credit_amount, Currency_Symbol,AccountId,IncomeAccountId) " _
                                                  & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & r.Cells("coa_detail_id").Value & ", " & 0 & ",  " & Val(r.Cells("Amount").Value) & ", N'" & r.Cells("Reference").Value.ToString.Trim.Replace("'", "''") & "', N'" & GetComments(r).Replace("Party Name.", "").Replace("" & r.Cells("detail_title").Value.ToString & "", "").Replace("'", "''") & "', " & Val(r.Cells("CurrencyDiscount").Value) & ", " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & Val(r.Cells("Employee").Value.ToString()) & "," & Val(r.Cells("InvoiceId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & ", " & Val(r.Cells("Tax").Value.ToString) & "," & Val(r.Cells("Tax_Amount").Value.ToString) & "," & Val(r.Cells("SalesTaxPer").Value.ToString) & "," & Val(r.Cells("SalesTaxAmount").Value.ToString) & "," & Val(r.Cells("WHTaxPer").Value.ToString) & "," & Val(r.Cells("WHTaxAmount").Value.ToString) & "," & Val(r.Cells("NetAmount").Value.ToString) & "," _
                                                  & Me.cmbCashAccount.SelectedValue & ", " & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString) & ", 0, " & Val(r.Cells("CurrencyAmount").Value) & ", '" & Me.cmbCurrency.Text & "', " & Val(r.Cells("AccountId").Value) & ", " & Val(r.Cells("IncomeAccountId").Value) & ") Select @@Identity"
                            objCommand.Transaction = objTrans
                            objId = objCommand.ExecuteScalar()

                            'If Val(r.Cells("VoucherDetailId").Value.ToString) > 0 Then
                            '    objCommand.CommandText = ""
                            '    objCommand.CommandText = "Update InvoiceAdjustmentTable Set VoucherDetailId=" & objId & " WHERE VoucherDetailId=" & Val(r.Cells("VoucherDetailId").Value.ToString) & ""
                            '    objCommand.ExecuteNonQuery()
                            'End If
                            'If blnCashOptionDetail = True Then
                            '    objCommand.CommandText = String.Empty
                            '    objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, ChequeDescription,Adjustment, CostCenterId, EmpID, InvoiceId, Cheque_No, Cheque_Date, BankDescription, Tax_Percent,Tax_Amount,SalesTaxPer,SalesTaxAmount,WHTaxPer,WHTaxAmount,NetAmount,contra_coa_detail_id, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_debit_amount, Currency_credit_amount, Currency_Symbol,AccountId,IncomeAccountId) " _
                            '                      & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & Me.cmbCashAccount.SelectedValue & ", " & Val(r.Cells("Net_Amount").Value.ToString) * Val(r.Cells("CurrencyRate").Value.ToString) & ",  " & 0 & ", N'" & IIf(flgMemoRemarks = False, r.Cells("Reference").Value.ToString.Trim.Replace("'", "''"), Me.txtMemo.Text.Replace("'", "''")) & " Party Name. " & r.Cells("detail_title").Value.ToString & "', N'" & GetComments(r).Replace("Party Name.", "").Replace("" & r.Cells("detail_title").Value.ToString & "", "").Replace("'", "''") & "', " & Val(r.Cells("Discount").Value) & ", " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & Val(r.Cells("Employee").Value.ToString()) & "," & Val(r.Cells("InvoiceId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & ", " & Val(r.Cells("Tax").Value.ToString) & "," & Val(r.Cells("Tax_Amount").Value.ToString) & "," & Val(r.Cells("SalesTaxPer").Value.ToString) & "," & Val(r.Cells("SalesTaxAmount").Value.ToString) & "," & Val(r.Cells("WHTaxPer").Value.ToString) & "," & Val(r.Cells("WHTaxAmount").Value.ToString) & "," & Val(r.Cells("NetAmount").Value.ToString) & "," _
                            '                      & Val(r.Cells("coa_detail_id").Value.ToString) & ", " & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString) & ", " & Val(r.Cells("Net_Amount").Value.ToString) & ",0 , '" & Me.cmbCurrency.Text & "', " & Val(r.Cells("AccountId").Value) & ", " & Val(r.Cells("IncomeAccountId").Value) & ") Select @@Identity"
                            '    objCommand.Transaction = objTrans
                            '    objId = objCommand.ExecuteScalar()
                            'End If
                        End If
                        'objCommand.CommandText = String.Empty
                        'objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, ChequeDescription,Adjustment, CostCenterId, EmpID, InvoiceId, Cheque_No, Cheque_Date, BankDescription, Tax_Percent,Tax_Amount,SalesTaxPer,SalesTaxAmount,WHTaxPer,WHTaxAmount,NetAmount,contra_coa_detail_id, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_debit_amount, Currency_credit_amount, Currency_Symbol,AccountId,IncomeAccountId) " _
                        '                      & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & r.Cells("coa_detail_id").Value & ", " & 0 & ",  " & Val(r.Cells("Amount").Value.ToString) & ", N'" & r.Cells("Reference").Value.ToString.Trim.Replace("'", "''") & "', N'" & GetComments(r).Replace("Party Name.", "").Replace("" & r.Cells("detail_title").Value.ToString & "", "").Replace("'", "''") & "', " & Val(r.Cells("CurrencyDiscount").Value) & ", " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & Val(r.Cells("Employee").Value.ToString()) & "," & Val(r.Cells("InvoiceId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & ", " & Val(r.Cells("Tax").Value.ToString) & "," & Val(r.Cells("Tax_Amount").Value.ToString) & "," & Val(r.Cells("SalesTaxPer").Value.ToString) & "," & Val(r.Cells("SalesTaxAmount").Value.ToString) & "," & Val(r.Cells("WHTaxPer").Value.ToString) & "," & Val(r.Cells("WHTaxAmount").Value.ToString) & "," & Val(r.Cells("NetAmount").Value.ToString) & "," _
                        '                      & Me.cmbCashAccount.SelectedValue & ", " & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString) & ", 0, " & Val(r.Cells("CurrencyAmount").Value) & ", '" & Me.cmbCurrency.Text & "', " & Val(r.Cells("AccountId").Value) & ", " & Val(r.Cells("IncomeAccountId").Value) & ") Select @@Identity"
                        'objCommand.Transaction = objTrans
                        'objId = objCommand.ExecuteScalar()

                        If Val(r.Cells("VoucherDetailId").Value.ToString) > 0 Then
                            objCommand.CommandText = ""
                            objCommand.CommandText = "Update InvoiceAdjustmentTable Set VoucherDetailId=" & objId & " WHERE VoucherDetailId=" & Val(r.Cells("VoucherDetailId").Value.ToString) & ""
                            objCommand.ExecuteNonQuery()
                        End If

                        If blnCashOptionDetail = True Then
                            objCommand.CommandText = String.Empty
                            objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, ChequeDescription,Adjustment, CostCenterId, EmpID, InvoiceId, Cheque_No, Cheque_Date, BankDescription, Tax_Percent,Tax_Amount,SalesTaxPer,SalesTaxAmount,WHTaxPer,WHTaxAmount,NetAmount,contra_coa_detail_id, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_debit_amount, Currency_credit_amount, Currency_Symbol,AccountId,IncomeAccountId) " _
                                              & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & Me.cmbCashAccount.SelectedValue & ", " & Val(r.Cells("Net_Amount").Value.ToString) * Val(r.Cells("CurrencyRate").Value.ToString) & ",  " & 0 & ", N'" & IIf(flgMemoRemarks = False, r.Cells("Reference").Value.ToString.Trim.Replace("'", "''"), Me.txtMemo.Text.Replace("'", "''")) & " Party Name. " & r.Cells("detail_title").Value.ToString & "', N'" & GetComments(r).Replace("Party Name.", "").Replace("" & r.Cells("detail_title").Value.ToString & "", "").Replace("'", "''") & "', " & Val(r.Cells("Discount").Value) & ", " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & Val(r.Cells("Employee").Value.ToString()) & "," & Val(r.Cells("InvoiceId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & ", " & Val(r.Cells("Tax").Value.ToString) & "," & Val(r.Cells("Tax_Amount").Value.ToString) & "," & Val(r.Cells("SalesTaxPer").Value.ToString) & "," & Val(r.Cells("SalesTaxAmount").Value.ToString) & "," & Val(r.Cells("WHTaxPer").Value.ToString) & "," & Val(r.Cells("WHTaxAmount").Value.ToString) & "," & Val(r.Cells("NetAmount").Value.ToString) & "," _
                                              & Val(r.Cells("coa_detail_id").Value.ToString) & ", " & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString) & ", " & Val(r.Cells("Net_Amount").Value.ToString) & ",0 , '" & Me.cmbCurrency.Text & "', " & Val(r.Cells("AccountId").Value) & ", " & Val(r.Cells("IncomeAccountId").Value) & ") Select @@Identity"
                            objCommand.Transaction = objTrans
                            objId = objCommand.ExecuteScalar()
                        End If
                        'objCommand.CommandText = String.Empty
                        'objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, ChequeDescription,Adjustment, CostCenterId, EmpID, InvoiceId, Cheque_No, Cheque_Date, BankDescription, Tax_Percent,Tax_Amount,SalesTaxPer,SalesTaxAmount,WHTaxPer,WHTaxAmount,NetAmount,contra_coa_detail_id, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_debit_amount, Currency_credit_amount, Currency_Symbol,AccountId,IncomeAccountId) " _
                        '                      & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & r.Cells("coa_detail_id").Value & ", " & 0 & ",  " & InvoiceTotalAmount & ", N'" & r.Cells("Reference").Value.ToString.Trim.Replace("'", "''") & "', N'" & GetComments(r).Replace("Party Name.", "").Replace("" & r.Cells("detail_title").Value.ToString & "", "").Replace("'", "''") & "', " & Val(r.Cells("CurrencyDiscount").Value) & ", " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & Val(r.Cells("Employee").Value.ToString()) & "," & Val(r.Cells("InvoiceId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & ", " & Val(r.Cells("Tax").Value.ToString) & "," & Val(r.Cells("Tax_Amount").Value.ToString) & "," & Val(r.Cells("SalesTaxPer").Value.ToString) & "," & Val(r.Cells("SalesTaxAmount").Value.ToString) & "," & Val(r.Cells("WHTaxPer").Value.ToString) & "," & Val(r.Cells("WHTaxAmount").Value.ToString) & "," & Val(r.Cells("NetAmount").Value.ToString) & "," _
                        '                      & Me.cmbCashAccount.SelectedValue & ", " & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString) & ", 0, " & InvoiceTotalAmount / InvoiceCurrencyRate & ", '" & Me.cmbCurrency.Text & "', " & Val(r.Cells("AccountId").Value) & ", " & Val(r.Cells("IncomeAccountId").Value) & ") Select @@Identity"
                        'objCommand.Transaction = objTrans
                        'objId = objCommand.ExecuteScalar()
                        ObjMod = New InvoicesBasedReceiptMaster 'With New Variable Invoice Based Receipt Master Class 
                        With ObjMod
                            ''.ReceiptID = MasterID
                            .ReceiptNo = txtVoucherNo.Text 'Me.GetVoucherNo
                            .ReceiptDate = Me.dtVoucherDate.Value.ToString("yyyy-M-d h:mm:ss tt")
                            .Reference = r.Cells("Reference").Value.ToString.Trim.Replace("'", "''")
                            .PaymentAccountId = Me.cmbCashAccount.SelectedValue
                            '.PaymentMethod = Me.cmbVoucherType.SelectedValue
                            'Task#4082015 Fill model for CompanyName added  by Ahmad Sharif
                            .CompanyName = IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1")
                            'End Task#04082015
                            'Task No 2537 Append One cODE Line For Assigning The Checked Value Of Check Box To Prperty
                            .Post = Me.chkPost.Checked
                            .CustomerCode = r.Cells("coa_detail_id").Value
                            .ChequeNo = IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'")
                            '.ChequeDate = IIf(Me.DtpChequeDate.Visible = True, "'" & Me.DtpChequeDate.Value & "'", Nothing) 'before task:2375
                            .ChequeDate = CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") 'Task:2375 Change DataType
                            .UserName = LoginUserName
                            .RVNo = txtVoucherNo.Text 'Me.txtVoucherNo.Text
                            .ProjectId = Val(r.Cells("CostCenterId").Value.ToString())
                            .UserID = LoginUserId
                            .InvoiceBasedReceiptDetail = New List(Of InvoicesBasedReceiptDetail)
                            .VoucherMaster = New VouchersMaster
                            Dim InvoiceDetail As InvoicesBasedReceiptDetail
                            InvoiceDetail = New InvoicesBasedReceiptDetail 'With New Variable Invoice Based Receipt Detail Class
                            With InvoiceDetail
                                .InvoiceId = Val(r.Cells("InvoiceId").Value.ToString())
                                '.InvoiceNo = Dr.Item(enmGrdReceiptDetail.SalesNo).ToString
                                '.InvoiceDate = Dr.Item(enmGrdReceiptDetail.SalesDate).ToString
                                .Remarks = r.Cells("Reference").Value.ToString.Trim.Replace("'", "''")
                                '.Gst_Percentage = Val(Dr.Item(enmGrdReceiptDetail.Gst_Percentage).ToString) 'Tax Receiveable Here... 
                                '.InvoiceAmount = Val(Dr.Item(enmGrdReceiptDetail.SalesAmount).ToString) 'Invoice Amount Here... 
                                .ReceiptAmount = Val(r.Cells("Amount").Value.ToString) '' + ((Val(r.Cells("Tax_Currency_Amount").Value.ToString) * Val(r.Cells("CurrencyRate").Value.ToString)) + (Val(r.Cells("WH_Tax").Value.ToString) * Val(r.Cells("CurrencyRate").Value.ToString))) 'Receitp Amount From Customer Account Or Other Receiveables
                                '+.InvoiceBalance = Dr.Item(enmGrdReceiptDetail.BalanceAmount).ToString ' Due Balance of Customer Account 
                                ''NetReceipt = NetReceipt + Dr.Item(enmGrdReceiptDetail.ReceiptAmount).ToString 'Net Receipt Amount From Customer Account 
                                ' Task 3198 Add CostCenter in Receipt Detail
                                .CostCenter = Val(r.Cells("CostCenterId").Value.ToString())
                                'Task:2370 Set Value in SalesTaxAmount, OtherTaxAmount, OtherTaxAccountId
                                '.SalesTaxAmount = Val(Dr.Item(enmGrdReceiptDetail.SalesTaxAmount).ToString)
                                '.OtherTaxAmount = Val(Dr.Item(enmGrdReceiptDetail.OtherTaxAmount).ToString)
                                '.OtherTaxAccountId = Val(Dr.Item(enmGrdReceiptDetail.OtherTaxAccountId).ToString)
                                'End Task:2370
                                ''.Description = Dr.Item(enmGrdReceiptDetail.Description).ToString 'Task:2470 Set Description Value
                            End With
                            .InvoiceBasedReceiptDetail.Add(InvoiceDetail)
                        End With
                        If BtnSave.Text = "&Save" Then
                            InvoicesBasedReceiptDAL.Add(ObjMod)
                        ElseIf BtnSave.Text = "&Update" Then
                            InvoicesBasedReceiptDAL.Update(ObjMod)
                        End If
                    Else
                        objCommand.CommandText = String.Empty
                        objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, ChequeDescription,Adjustment, CostCenterId, EmpID, InvoiceId, Cheque_No, Cheque_Date, BankDescription, Tax_Percent,Tax_Amount,SalesTaxPer,SalesTaxAmount,WHTaxPer,WHTaxAmount,NetAmount,contra_coa_detail_id, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_debit_amount, Currency_credit_amount, Currency_Symbol,AccountId,IncomeAccountId) " _
                                              & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & r.Cells("coa_detail_id").Value & ", " & 0 & ",  " & Val(r.Cells("Amount").Value.ToString) & ", N'" & r.Cells("Reference").Value.ToString.Trim.Replace("'", "''") & "', N'" & GetComments(r).Replace("Party Name.", "").Replace("" & r.Cells("detail_title").Value.ToString & "", "").Replace("'", "''") & "', " & Val(r.Cells("CurrencyDiscount").Value) & ", " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & Val(r.Cells("Employee").Value.ToString()) & "," & Val(r.Cells("InvoiceId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & ", " & Val(r.Cells("Tax").Value.ToString) & "," & Val(r.Cells("Tax_Amount").Value.ToString) & "," & Val(r.Cells("SalesTaxPer").Value.ToString) & "," & Val(r.Cells("SalesTaxAmount").Value.ToString) & "," & Val(r.Cells("WHTaxPer").Value.ToString) & "," & Val(r.Cells("WHTaxAmount").Value.ToString) & "," & Val(r.Cells("NetAmount").Value.ToString) & "," _
                                              & Me.cmbCashAccount.SelectedValue & ", " & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString) & ", 0, " & Val(r.Cells("CurrencyAmount").Value) & ", '" & Me.cmbCurrency.Text & "', " & Val(r.Cells("AccountId").Value) & ", " & Val(r.Cells("IncomeAccountId").Value) & ") Select @@Identity"
                        objCommand.Transaction = objTrans
                        objId = objCommand.ExecuteScalar()

                        If Val(r.Cells("VoucherDetailId").Value.ToString) > 0 Then
                            objCommand.CommandText = ""
                            objCommand.CommandText = "Update InvoiceAdjustmentTable Set VoucherDetailId=" & objId & " WHERE VoucherDetailId=" & Val(r.Cells("VoucherDetailId").Value.ToString) & ""
                            objCommand.ExecuteNonQuery()
                        End If
                        '-Me.cmbCashAccount.SelectedValue
                        If blnCashOptionDetail = True Then
                            'objCommand.CommandText = String.Empty
                            'objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, credit_amount,debit_amount,  comments, ChequeDescription,Adjustment, CostCenterId, EmpID, InvoiceId, Cheque_No, Cheque_Date, BankDescription, Tax_Percent,Tax_Amount,contra_coa_detail_id, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_Debit_Amount, Currency_Credit_Amount, Currency_Symbol) " _
                            '                   & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & Me.cmbCashAccount.SelectedValue & ", " & 0 & ",  " & Val(r.Cells("Net_Amount").Value.ToString) * Val(r.Cells("CurrencyRate").Value.ToString) & ", N'" & IIf(flgMemoRemarks = False, r.Cells("Reference").Value.ToString.Trim.Replace("'", "''"), Me.txtMemo.Text.Replace("'", "''")) & " Party Name. " & r.Cells("detail_title").Value.ToString & "', N'" & GetComments(r).Replace("Party Name.", "").Replace("" & r.Cells("detail_title").Value.ToString & "", "").Replace("'", "''") & "', " & Val(r.Cells("Discount").Value) & ", " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & Val(r.Cells("Employee").Value.ToString()) & ", " & Val(r.Cells("InvoiceId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & ", " & Val(r.Cells("Tax").Value.ToString) & "," & Val(r.Cells("Tax_Amount").Value.ToString) & "," & Val(r.Cells("coa_detail_id").Value.ToString) & ", " _
                            '                   & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString()) & ",  " & Val(r.Cells("Net_Amount").Value.ToString) & ", 0, '" & Me.cmbCurrency.Text & "')Select @@Identity"
                            'objCommand.Transaction = objTrans
                            'objCommand.ExecuteNonQuery()
                            objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, ChequeDescription,Adjustment, CostCenterId, EmpID, InvoiceId, Cheque_No, Cheque_Date, BankDescription, Tax_Percent,Tax_Amount,SalesTaxPer,SalesTaxAmount,WHTaxPer,WHTaxAmount,NetAmount,contra_coa_detail_id, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_debit_amount, Currency_credit_amount, Currency_Symbol,AccountId,IncomeAccountId) " _
                                              & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & Me.cmbCashAccount.SelectedValue & ", " & Val(r.Cells("Net_Amount").Value.ToString) * Val(r.Cells("CurrencyRate").Value.ToString) & ",  " & 0 & ", N'" & IIf(flgMemoRemarks = False, r.Cells("Reference").Value.ToString.Trim.Replace("'", "''"), Me.txtMemo.Text.Replace("'", "''")) & " Party Name. " & r.Cells("detail_title").Value.ToString & "', N'" & GetComments(r).Replace("Party Name.", "").Replace("" & r.Cells("detail_title").Value.ToString & "", "").Replace("'", "''") & "', " & Val(r.Cells("Discount").Value) & ", " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & Val(r.Cells("Employee").Value.ToString()) & "," & Val(r.Cells("InvoiceId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & ", " & Val(r.Cells("Tax").Value.ToString) & "," & Val(r.Cells("Tax_Amount").Value.ToString) & "," & Val(r.Cells("SalesTaxPer").Value.ToString) & "," & Val(r.Cells("SalesTaxAmount").Value.ToString) & "," & Val(r.Cells("WHTaxPer").Value.ToString) & "," & Val(r.Cells("WHTaxAmount").Value.ToString) & "," & Val(r.Cells("NetAmount").Value.ToString) & "," _
                                              & Val(r.Cells("coa_detail_id").Value.ToString) & ", " & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString) & ", " & Val(r.Cells("Net_Amount").Value.ToString) & ",0 , '" & Me.cmbCurrency.Text & "', " & Val(r.Cells("AccountId").Value) & ", " & Val(r.Cells("IncomeAccountId").Value) & ") Select @@Identity"
                            objCommand.Transaction = objTrans
                            objId = objCommand.ExecuteScalar()
                        End If
                    End If





                    If Val(r.Cells("VoucherDetailId").Value.ToString) > 0 Then
                        objCommand.CommandText = ""
                        objCommand.CommandText = "Update InvoiceAdjustmentTable Set VoucherDetailId=" & objId & " WHERE VoucherDetailId=" & Val(r.Cells("VoucherDetailId").Value.ToString) & ""
                        objCommand.ExecuteNonQuery()
                    End If


                    '-Me.cmbCashAccount.SelectedValue
                    'If blnCashOptionDetail = True Then
                    '    objCommand.CommandText = String.Empty
                    '    objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, credit_amount,debit_amount,  comments, ChequeDescription,Adjustment, CostCenterId, EmpID, InvoiceId, Cheque_No, Cheque_Date, BankDescription, Tax_Percent,Tax_Amount,contra_coa_detail_id, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_Debit_Amount, Currency_Credit_Amount, Currency_Symbol) " _
                    '                       & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & Me.cmbCashAccount.SelectedValue & ", " & 0 & ",  " & Val(r.Cells("NetAmount").Value.ToString) & ", N'" & IIf(flgMemoRemarks = False, r.Cells("Reference").Value.ToString.Trim.Replace("'", "''"), Me.txtMemo.Text.Replace("'", "''")) & " Party Name. " & r.Cells("detail_title").Value.ToString & "', N'" & GetComments(r).Replace("Party Name.", "").Replace("" & r.Cells("detail_title").Value.ToString & "", "").Replace("'", "''") & "', " & Val(r.Cells("Discount").Value) & ", " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & Val(r.Cells("Employee").Value.ToString()) & ", " & Val(r.Cells("InvoiceId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & ", " & Val(r.Cells("Tax").Value.ToString) & "," & Val(r.Cells("Tax_Amount").Value.ToString) & "," & Val(r.Cells("coa_detail_id").Value.ToString) & ", " _
                    '                       & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString()) & ",  " & Val(r.Cells("Net_Amount").Value.ToString) & ", 0, '" & Me.cmbCurrency.Text & "')Select @@Identity"
                    '    objCommand.Transaction = objTrans
                    '    objCommand.ExecuteNonQuery()
                    'End If

                    If enableChequeBook = True Then
                        objCommand.Transaction = objTrans
                        objCommand.CommandText = String.Empty
                        objCommand.CommandText = "UPDATE ChequeDetailTable SET VoucherDetailId=" & objId & ", Cheque_Issued=1 From ChequeDetailTable, ChequeMasterTable WHERE ChequeDetailTable.ChequeSerialId=ChequeMasterTable.ChequeSerialId AND ChequeNo=N'" & strChequeNo & "' AND BankAcId=" & Me.cmbCashAccount.SelectedValue & ""
                        objCommand.ExecuteNonQuery()
                    End If

                    If Val(r.Cells("Discount").Value) <> 0 Then
                        'Discount Voucher ...................
                        objCommand.Transaction = objTrans
                        objCommand.CommandText = String.Empty
                        objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, CostCenterId, EmpID, InvoiceId , Cheque_No,Cheque_Date,BankDescription, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_Debit_Amount, Currency_Credit_Amount, Currency_Symbol ) " _
                                               & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & Val(DiscountAccountId) & ", " & Val(r.Cells("Discount").Value) & ", " & 0 & ", N'" & r.Cells("Reference").Value.ToString.Trim.Replace("'", "''") & "', " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & Val(r.Cells("Employee").Value.ToString()) & "," & Val(r.Cells("InvoiceId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString = "", "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & " , " _
                                               & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString()) & ", " & Val(r.Cells("CurrencyDiscount").Value) & ",  0 , '" & Me.cmbCurrency.Text & "')"
                        objCommand.ExecuteNonQuery()
                    End If
                    'added by zainab Task1608
                    'in this query whtax is saved as debit
                    If cmbSalesTax.SelectedValue IsNot Nothing Then
                        objCommand.Transaction = objTrans
                        objCommand.CommandText = ""
                        objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments,CostCenterId, EmpID, InvoiceId, Cheque_No,Cheque_Date,BankDescription, Tax_Percent,Tax_Amount,SalesTaxPer,SalesTaxAmount,WHTaxPer,WHTaxAmount, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_Debit_Amount, Currency_Credit_Amount, Currency_Symbol, AccountId) " _
                                                   & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & Val(r.Cells("AccountId").Value) & ", " & Val(r.Cells("WH_Tax").Value.ToString) * Val(r.Cells("CurrencyRate").Value.ToString) & ", " & 0 & ", N'" & r.Cells("Reference").Value.ToString.Trim.Replace("'", "''") & "', " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & Val(r.Cells("Employee").Value.ToString()) & "," & Val(r.Cells("InvoiceId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString = "", "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & ", " & Val(r.Cells("Tax").Value.ToString) & "," & Val(r.Cells("Tax_Amount").Value.ToString) & "," & Val(r.Cells("SalesTaxPer").Value.ToString) & "," & Val(r.Cells("SalesTaxAmount").Value.ToString) & "," & Val(r.Cells("WHTaxPer").Value.ToString) & ", " & Val(r.Cells("WHTaxAmount").Value.ToString) & ", " _
                                                   & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString()) & ", " & Val(r.Cells("WH_Tax").Value.ToString) & ", 0, '" & Me.cmbCurrency.Text & "', " & Val(r.Cells("AccountId").Value) & ")"
                        objCommand.ExecuteNonQuery()

                    End If

                    If cmbIncomeTaxAccount.SelectedValue IsNot Nothing Then
                        objCommand.Transaction = objTrans
                        objCommand.CommandText = String.Empty
                        'objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, CostCenterId, EmpID, InvoiceId, Cheque_No,Cheque_Date,BankDescription,Tax_Percent,Tax_Amount, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_Debit_Amount, Currency_Credit_Amount, Currency_Symbol,IncomeAccountId) " _
                        '                       & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & Val(r.Cells("IncomeAccountId").Value) & ", " & Val(r.Cells("Tax_Currency_Amount").Value.ToString) * Val(r.Cells("CurrencyRate").Value.ToString) & ", " & 0 & ", N'" & r.Cells("Reference").Value.ToString.Trim.Replace("'", "''") & "', " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & Val(r.Cells("Employee").Value.ToString()) & "," & Val(r.Cells("InvoiceId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString = "", "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & "," & Val(r.Cells("Tax").Value.ToString) & ", " & Val(r.Cells("Tax_Amount").Value.ToString) & ", " _
                        '                       & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString()) & ", " & Val(r.Cells("Tax_Currency_Amount").Value.ToString) & ", 0, '" & Me.cmbCurrency.Text & "'," & Val(r.Cells("IncomeAccountId").Value) & ")"
                        objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments,CostCenterId, EmpID, InvoiceId, Cheque_No,Cheque_Date,BankDescription, Tax_Percent,Tax_Amount,SalesTaxPer,SalesTaxAmount,WHTaxPer,WHTaxAmount, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_Debit_Amount, Currency_Credit_Amount, Currency_Symbol, AccountId) " _
                                                   & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & Val(r.Cells("IncomeAccountId").Value) & ", " & Val(r.Cells("Tax_Currency_Amount").Value.ToString) * Val(r.Cells("CurrencyRate").Value.ToString) & ", " & 0 & ", N'" & r.Cells("Reference").Value.ToString.Trim.Replace("'", "''") & "', " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & Val(r.Cells("Employee").Value.ToString()) & "," & Val(r.Cells("InvoiceId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString = "", "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & ", " & Val(r.Cells("Tax").Value.ToString) & "," & Val(r.Cells("Tax_Amount").Value.ToString) & "," & Val(r.Cells("SalesTaxPer").Value.ToString) & "," & Val(r.Cells("SalesTaxAmount").Value.ToString) & "," & Val(r.Cells("WHTaxPer").Value.ToString) & ", " & Val(r.Cells("WHTaxAmount").Value.ToString) & ", " _
                                                   & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString()) & ", " & Val(r.Cells("Tax_Currency_Amount").Value.ToString) & ", 0, '" & Me.cmbCurrency.Text & "', " & Val(r.Cells("IncomeAccountId").Value) & ")"
                        objCommand.ExecuteNonQuery()

                    End If

                    If flgSalesInvoice = True Then
                        Dim adapter As New OleDbDataAdapter()
                        'Dim str As String
                        objCommand.CommandText = "select voucher_id from tblvoucher where voucher_no = (SELECT SalesNo from salesmastertable where salesid = " & Val(r.Cells("InvoiceId").Value.ToString()) & ")"
                        adapter.SelectCommand = objCommand
                        Dim dt As New DataTable
                        adapter.Fill(dt)
                        dt.AcceptChanges()
                        Dim sid As Integer
                        If dt.Rows.Count > 0 Then
                            sid = dt.Rows(0).Item("voucher_id")
                        End If
                        objCommand.Transaction = objTrans
                        objCommand.CommandText = String.Empty
                        objCommand.CommandText = "Update tblVoucherDetail set InvoiceId = " & Val(r.Cells("InvoiceId").Value.ToString()) & " WHERE voucher_id = " & sid & ""
                        objCommand.ExecuteNonQuery()
                    End If
                    ' Next


                    'If intSalesAccountID > 0 Then

                    '    If cmbSalesTax.SelectedValue IsNot Nothing Then
                    '        objCommand.Transaction = objTrans
                    '        objCommand.CommandText = String.Empty
                    '        objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, CostCenterId, Cheque_No,Cheque_Date,BankDescription,SalesTaxPer,SalesTaxAmount, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_Debit_Amount, Currency_Credit_Amount, Currency_Symbol ) " _
                    '                               & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & Val(TaxPayableAccountId) & ", " & Val(r.Cells("SalesTaxAmount").Value) & ", " & 0 & ", N'" & r.Cells("Reference").Value.ToString.Trim.Replace("'", "''") & "', " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString = "", "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & "," & Val(r.Cells("SalesTaxPer").Value.ToString) & ", " & Val(r.Cells("SalesTaxAmount").Value.ToString) & ", " _
                    '                               & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString()) & ", " & Val(r.Cells("Tax_Currency_Amount").Value) & ", 0, '" & Me.cmbCurrency.Text & "')"
                    '        objCommand.ExecuteNonQuery()
                    '    End If
                    '    objCommand.CommandText = ""
                    '    objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, CostCenterId, Cheque_No,Cheque_Date,BankDescription,SalesTaxPer,SalesTaxAmount, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_Debit_Amount, Currency_Credit_Amount, Currency_Symbol ) " _
                    '                               & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & Val(TaxPayableAccountId) & ", " & Val(r.Cells("SalesTaxAmount").Value) & ", " & 0 & ", N'" & r.Cells("Reference").Value.ToString.Trim.Replace("'", "''") & "', " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString = "", "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & "," & Val(r.Cells("SalesTaxPer").Value.ToString) & ", " & Val(r.Cells("SalesTaxAmount").Value.ToString) & ", " _
                    '                               & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString()) & ", " & Val(r.Cells("Tax_Currency_Amount").Value) & ", 0, '" & Me.cmbCurrency.Text & "')"
                    '    objCommand.ExecuteNonQuery()

                    'End If

                    'added by zainab #task1608

                    'If cmbSalesTax.SelectedValue IsNot Nothing Then
                    '    'Discount Voucher ...................
                    '    objCommand.Transaction = objTrans
                    '    objCommand.CommandText = String.Empty
                    '    objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, CostCenterId, Cheque_No,Cheque_Date,BankDescription,SalesTaxPer,SalesTaxAmount, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_Debit_Amount, Currency_Credit_Amount, Currency_Symbol ) " _
                    '                           & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & Val(TaxPayableAccountId) & ", " & Val(r.Cells("SalesTaxAmount").Value) & ", " & 0 & ", N'" & r.Cells("Reference").Value.ToString.Trim.Replace("'", "''") & "', " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString = "", "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & "," & Val(r.Cells("SalesTaxPer").Value.ToString) & ", " & Val(r.Cells("SalesTaxAmount").Value.ToString) & ", " _
                    '                           & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString()) & ", " & Val(r.Cells("Tax_Currency_Amount").Value) & ", 0, '" & Me.cmbCurrency.Text & "')"
                    '    objCommand.ExecuteNonQuery()
                    'End If
                    'If Val(r.Cells("WHTaxAmount").Value) <> 0 Then
                    '    'Discount Voucher ...................
                    '    objCommand.Transaction = objTrans
                    '    objCommand.CommandText = String.Empty
                    '    objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, CostCenterId, Cheque_No,Cheque_Date,BankDescription,WHTaxPer,WHTaxAmount, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_Debit_Amount, Currency_Credit_Amount, Currency_Symbol ) " _
                    '                           & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & Val(TaxPayableAccountId) & "," & Val(r.Cells("WHTaxAmount").Value) & "," & 0 & ", N'" & r.Cells("Reference").Value.ToString.Trim.Replace("'", "''") & "', " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString = "", "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & "," & Val(r.Cells("WHTaxPer").Value.ToString) & ", " & Val(r.Cells("WHTaxAmount").Value.ToString) & ", " _
                    '                           & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString()) & ", " & Val(r.Cells("Tax_Currency_Amount").Value) & ", 0, '" & Me.cmbCurrency.Text & "')"
                    '    objCommand.ExecuteNonQuery()
                    'End If
                    'If Val(r.Cells("NetAmount").Value) <> 0 Then
                    '    'Discount Voucher ...................
                    '    objCommand.Transaction = objTrans
                    '    objCommand.CommandText = String.Empty
                    '    objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, CostCenterId, Cheque_No,Cheque_Date,BankDescription,NetAmount, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_Debit_Amount, Currency_Credit_Amount, Currency_Symbol ) " _
                    '                           & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & Val(TaxPayableAccountId) & ", " & Val(r.Cells("NetAmount").Value) & ", " & 0 & ", N'" & r.Cells("Reference").Value.ToString.Trim.Replace("'", "''") & "', " & Val(r.Cells("CostCenterId").Value.ToString()) & ", " & IIf(r.Cells("Cheque_No").Value.ToString = "", "NULL", "N'" & r.Cells("Cheque_No").Value.ToString.Replace("'", "''") & "'") & ", " & IIf(r.Cells("Cheque_No").Value.ToString.Length = 0, "NULL", "N'" & CDate(IIf(IsDBNull(r.Cells("Cheque_Date").Value), Now, r.Cells("Cheque_Date").Value)).ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(r.Cells("BankDescription").Value.ToString = "", "NULL", "N'" & r.Cells("BankDescription").Value.ToString.Replace("'", "''") & "'") & "," & Val(r.Cells("NetAmount").Value.ToString) & ", " _
                    '                           & Val(r.Cells("CurrencyId").Value.ToString) & ", " & Val(r.Cells("CurrencyAmount").Value.ToString) & ", " & Val(r.Cells("CurrencyRate").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyId").Value.ToString) & ", " & Val(r.Cells("BaseCurrencyRate").Value.ToString()) & ", " & Val(r.Cells("Tax_Currency_Amount").Value) & ", 0, '" & Me.cmbCurrency.Text & "')"
                    '    objCommand.ExecuteNonQuery()
                    'End If



                    'Task:2443  e.g Multi Cheque Detail Value Store 
                    If r.Cells("Cheque_No").Value.ToString.Length > 0 Then
                        If strMultiChequeDetail.Length > 0 Then
                            strMultiChequeDetail += "|" & r.Cells("Cheque_No").Value.ToString & ":" & IIf(r.Cells("Cheque_Date").Value.ToString <> "", CDate(r.Cells("Cheque_Date").Value.ToString).ToString("d/MMM/yy"), "")
                        Else
                            strMultiChequeDetail = r.Cells("Cheque_No").Value.ToString & ":" & IIf(r.Cells("Cheque_Date").Value.ToString <> "", CDate(r.Cells("Cheque_Date").Value.ToString).ToString("d/MMM/yy"), "")
                        End If
                    End If

                Next


                If strMultiChequeDetail.Length > 0 AndAlso strMultiChequeDetail.Length < 8000 Then
                    objCommand.CommandText = ""
                    objCommand.Transaction = objTrans
                    objCommand.CommandText = "Update tblVoucher SET Cheque_No=N'" & strMultiChequeDetail.Replace("'", "''") & "' WHERE Voucher_Id=" & lngVoucherMasterId
                    objCommand.ExecuteNonQuery()
                End If
                'End Task:2443

                '***********************
                'Inserting Debit Amount
                '***********************
                'Task:2728 Comment Code
                'Chaning Against Request No 801
                'objCommand = New OleDbCommand
                'objCommand.Connection = Con
                If blnCashOptionDetail = False Then
                    objCommand.CommandText = String.Empty
                    objCommand.CommandText = "INSERT INTO tblVoucherDetail(voucher_id, location_id, coa_detail_id, debit_amount, credit_amount, comments, CostCenterId, EmpID, InvoiceId, Cheque_No, Cheque_Date, BankDescription, CurrencyId, CurrencyAmount, CurrencyRate, BaseCurrencyId, BaseCurrencyRate, Currency_Debit_Amount, Currency_Credit_Amount, Currency_Symbol) " _
                                           & " VALUES(" & lngVoucherMasterId & ", " & IIf(flgCompanyRights = True, "" & MyCompanyId & "", "1") & ", " & Me.cmbCashAccount.SelectedValue & ", " & Val(Me.grd.GetTotal(Me.grd.RootTable.Columns("NetAmount"), Janus.Windows.GridEX.AggregateFunction.Sum)) & ", 0, N'" & Me.txtMemo.Text.Replace("'", "''") & "', " & Val(Me.cmbCostCenter.SelectedValue) & ",  " & Val(Me.cmbSaleman.SelectedValue) & "," & IIf(flgSalesInvoice = True, InvoiceId, 0) & ", " & IIf(strChequeNo.Length > 0, "N'" & strChequeNo.Replace("'", "''") & "'", "NULL") & ", " & IIf(strChequeNo.Length = 0, "NULL", "N'" & Cheque_Date.ToString("yyyy-M-d h:mm:ss tt") & "'") & ", " & IIf(strChequeNo.Length > 0, "N'" & strBankDescription.Replace("'", "''") & "'", "NULL") & ", " _
                                           & Val(Me.cmbCurrency.SelectedValue) & ", " & Val(Me.grd.GetTotal(grd.RootTable.Columns("CurrencyAmount"), Janus.Windows.GridEX.AggregateFunction.Sum)) & ", " & Me.txtCurrencyRate.Text & ", " & Me.BaseCurrencyId & ", 1 ,  " & Val(Me.grd.GetTotal(Me.grd.RootTable.Columns("Net_Amount"), Janus.Windows.GridEX.AggregateFunction.Sum)) & ", 0, '" & Me.cmbCurrency.Text & "')"

                    objCommand.Transaction = objTrans
                    objCommand.ExecuteNonQuery()
                    'End Task:2728
                End If

                'If chkPost.Checked = False Then

                objCommand.CommandText = " if exists (select COUNT(*) from VoucherApprovalGroupSetting) " _
                                            & " if not exists (select * from VoucherApprovedLog where voucher_id=" & lngVoucherMasterId & ") " _
                                            & " insert into VoucherApprovedLog (Voucher_Id , UserGroupId ,VALstatus,UserId ,UserName,ModificationDate ) values (" & lngVoucherMasterId & ",1,'Pending', " & LoginUserId & ",'" & LoginUserName & "',GETDATE())"
                objCommand.ExecuteNonQuery()

                'End If

                objTrans.Commit()
                'Altered against task#20150514 Send SMS after confirmation of message
                SendSMS()
                'Altered against task#20150514 Send SMS after confirmation of message
                strChequeNo = String.Empty
                strBankDescription = String.Empty
                Cheque_Date = Nothing
                'Marked Against Task#2015060023 Ali Ansari to save proper activity log
                'SaveActivityLog("Accounts", Me.Text, EnumActions.Save, LoginUserId, EnumRecordType.AccountTransaction, txtVoucherNo.Text.Trim, True)
                'Marked Against Task#2015060023 Ali Ansari to save proper activity log
                'Altered Against Task#2015060023 Ali Ansari to save proper activity log
                If BtnSave.Text = "&Save" Then
                    SaveActivityLog("Accounts", Me.Text, EnumActions.Save, LoginUserId, EnumRecordType.AccountTransaction, txtVoucherNo.Text.Trim, True)
                    ''Start TFS2701
                    ''insert Approval Log
                    SaveApprovalLog(EnumReferenceType.Receipt, lngVoucherMasterId, Me.txtVoucherNo.Text.Trim, Me.dtVoucherDate.Value.Date, "Receipt," & cmbCompany.Text & "", Me.Name, cmbVoucherType.SelectedValue)
                    ''End TFS2701
                Else
                    SaveActivityLog("Accounts", Me.Text, EnumActions.Update, LoginUserId, EnumRecordType.AccountTransaction, txtVoucherNo.Text.Trim, True)
                    ''Start TFS2989
                    If ValidateApprovalProcessMapped(Me.txtVoucherNo.Text.Trim, Me.Name) Then
                        If ValidateApprovalProcessIsInProgressAgain(Me.txtVoucherNo.Text.Trim, Me.Name) = False Then
                            SaveApprovalLog(EnumReferenceType.Receipt, lngVoucherMasterId, Me.txtVoucherNo.Text.Trim, Me.dtVoucherDate.Value.Date, "Receipt," & cmbCompany.Text & "", Me.Name, cmbVoucherType.SelectedValue)
                        End If
                    End If
                    ''End TFS2989
                End If
                'Altered Against Task#2015060023 Ali Ansari to save proper activity log
                'Me.ClearFields()
                'Me.grd.Rows.Clear()
                GetVoucherId = lngVoucherMasterId
            Catch ex As Exception
                objTrans.Rollback()
                Throw ex
            End Try
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        Finally
            Me.lblProgress.Visible = False
        End Try
    End Function

    Public Function DeleteRecord(ByVal strDeleteWhat As String, Optional ByVal LogActivity As Boolean = True)
        Try

            Me.lblProgress.Text = "Processing Please Wait ..."
            Me.lblProgress.Visible = True
            Application.DoEvents()

            If grdVouchers.CurrentRow Is Nothing Then
                ShowErrorMessage("Please select a row to delete")
            Else
                Dim enableChequeBook As Boolean = getConfigValueByType("EnableAutoChequeBook")
                'lngSelectedVoucherId = grdVouchers.CurrentRow.Cells(0).Value
                Dim objCommand As New OleDbCommand
                Dim objTrans As OleDbTransaction
                If Con.State = ConnectionState.Closed Or Con.State = ConnectionState.Broken Then Con.Open()
                objTrans = Con.BeginTransaction
                Try

                    objCommand.Connection = Con
                    objCommand.Transaction = objTrans
                    objCommand.CommandType = CommandType.Text
                    objCommand.CommandTimeout = 120

                    lngSelectedVoucherId = Val(Me.grdVouchers.GetRow.Cells("Voucher_ID").Value.ToString)

                    Dim adapter As New OleDbDataAdapter()
                    'Dim str As String
                    objCommand.CommandText = "select invoiceid from tblVoucherDetail where voucher_id = " & lngSelectedVoucherId & " Group by invoiceid"
                    adapter.SelectCommand = objCommand
                    Dim dt As New DataTable
                    adapter.Fill(dt)
                    dt.AcceptChanges()
                    Dim adapter1 As New OleDbDataAdapter()
                    'Dim str As String
                    objCommand.CommandText = "select voucher_id from tblVoucherDetail where invoiceid = " & dt.Rows(0).Item("invoiceid") & " group by voucher_id"
                    adapter1.SelectCommand = objCommand
                    Dim dt1 As New DataTable
                    adapter1.Fill(dt1)
                    dt1.AcceptChanges()
                    Dim sid As Integer
                    For Each row As DataRow In dt1.Rows
                        sid = row.Item("voucher_id")
                        objCommand.Transaction = objTrans
                        objCommand.CommandText = String.Empty
                        objCommand.CommandText = "Update tblVoucherDetail set InvoiceId = " & 0 & " WHERE voucher_id = " & sid & ""
                        objCommand.ExecuteNonQuery()
                    Next
                    If strDeleteWhat = "Detail Only" Then
                        'objCommand.Transaction = objTrans
                        'objCommand.CommandText = String.Empty
                        'objCommand.CommandText = "DELETE FROM tblVoucherDetail WHERE voucher_id = " & lngSelectedVoucherId
                        'objCommand.ExecuteNonQuery()
                    ElseIf strDeleteWhat = "Both" Then
                        If getConfigValueByType("EnabledDuplicateVoucherLog").ToString.ToUpper = "TRUE" Then
                            Call CreateDuplicationVoucher(lngSelectedVoucherId, "Delete", objTrans) 'TASKM2710151
                        End If
                        '' Revised Update Cheque Serial No
                        If enableChequeBook = True Then
                            objCommand.CommandText = String.Empty
                            'Before against Task:2383
                            'objCommand.CommandText = "Update ChequeDetailTable Set VoucherDetailId=0, Cheque_Issued=0 From  ChequeDetailTable, tblVoucherDetail WHERE tblVoucherDetail.Voucher_Detail_Id = ChequeDetailTable.VoucherDetailId WHERE tblVoucherDetail.Voucher_Id=" & lngSelectedVoucherId & ""
                            'Task:2383 Change Filter
                            objCommand.CommandText = "Update ChequeDetailTable Set VoucherDetailId=0, Cheque_Issued=0 From  ChequeDetailTable, tblVoucherDetail WHERE tblVoucherDetail.Voucher_Detail_Id = ChequeDetailTable.VoucherDetailId And tblVoucherDetail.Voucher_Id=" & lngSelectedVoucherId & ""
                            'End Task:2383
                            objCommand.ExecuteNonQuery()
                        End If

                        'lngSelectedVoucherId = grdVouchers.GetRow.Cells(0).Value
                        'objCommand.Transaction = objTrans
                        objCommand.CommandText = String.Empty
                        objCommand.CommandText = "DELETE FROM tblVoucherDetail WHERE voucher_id = " & lngSelectedVoucherId
                        objCommand.ExecuteNonQuery()

                        'objCommand = New OleDbCommand
                        'objCommand.Connection = Con
                        'objCommand.Transaction = objTrans
                        objCommand.CommandText = String.Empty
                        objCommand.CommandText = "DELETE FROM tblVoucher WHERE voucher_id = " & lngSelectedVoucherId
                        objCommand.ExecuteNonQuery()

                    End If
                    ObjMod = New InvoicesBasedReceiptMaster
                    ObjMod.ReceiptNo = grdVouchers.GetRow.Cells(1).Value
                    ObjMod.ReceiptNo = grdVouchers.GetRow.Cells(1).Value
                    ObjMod.VoucherMaster = New VouchersMaster
                    ObjMod.RVNo = grdVouchers.GetRow.Cells(1).Value
                    ''14-Jul-2014 TASK:2736 Imran Ali Delete Voucher Problem On Invoice Base Receipt
                    ObjMod.VoucherMaster.VoucherCode = ObjMod.RVNo
                    ObjMod.VoucherMaster.VoucherNo = ObjMod.RVNo
                    ObjMod.VoucherMaster.VNo = ObjMod.RVNo
                    'End Task:2736
                    ObjMod.VoucherMaster.ActivityLog = New ActivityLog
                    ObjMod.VoucherMaster.ActivityLog.ApplicationName = "Accounts"
                    ObjMod.VoucherMaster.ActivityLog.FormCaption = Me.Text
                    ObjMod.VoucherMaster.ActivityLog.UserID = LoginUserId
                    ObjMod.VoucherMaster.ActivityLog.LogDateTime = Date.Now
                    ObjMod.ActivityLog = New ActivityLog
                    ObjMod.ActivityLog.ApplicationName = "Accounts"
                    ObjMod.ActivityLog.FormCaption = Me.Text
                    ObjMod.ActivityLog.UserID = LoginUserId
                    ObjMod.ActivityLog.LogDateTime = Date.Now
                    'End Task:2381
                    If New InvoicesBasedReceiptDAL().Delete(ObjMod) Then Return True
                    objTrans.Commit()
                    'If LogActivity = True Then SaveActivityLog("Accounts", Me.Text, EnumActions.Delete, LoginUserId, EnumRecordType.AccountTransaction, Me.grdVouchers.CurrentRow.Cells(1).Value.ToString, True)
                    If strDeleteWhat = "Both" Then SaveActivityLog("Accounts", Me.Text, EnumActions.Delete, LoginUserId, EnumRecordType.AccountTransaction, Me.grdVouchers.CurrentRow.Cells(1).Value.ToString, True)
                Catch ex As Exception
                    objTrans.Rollback()
                End Try

            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        Finally
            Me.lblProgress.Visible = False
        End Try

    End Function
    Sub EditRecord()
        Try

            If Me.grdVouchers.Row < 0 Then Exit Sub
            Me.cmbAccounts.Rows(0).Activate()
            Me.cmbVoucherType.Enabled = False
            Me.cmbCashAccount.Enabled = False
            Me.chkEnableDepositAc.Visible = True
            Me.chkEnableDepositAc.Checked = False
            lngSelectedVoucherId = Me.grdVouchers.GetRow.Cells(0).Value
            cmbVoucherType.Text = grdVouchers.GetRow.Cells("Payment Method").Value    'Payment Method
            txtVoucherNo.Text = grdVouchers.GetRow.Cells(1).Value 'VoucherNo
            dtVoucherDate.Value = grdVouchers.GetRow.Cells(2).Value 'VoucherDate
            If IsDBNull(grdVouchers.CurrentRow.Cells("Employee_Id")) Then
                Me.cmbSaleman.SelectedIndex = 0
            Else
                cmbSaleman.SelectedValue = grdVouchers.CurrentRow.Cells("Employee_Id").Value
            End If

            Me.btnAttachment.Text = "Attachment (" & grdVouchers.CurrentRow.Cells("No Of Attachment").Value & ")"

            MainEditRecord()
            Me.DisplayDetail(grdVouchers.CurrentRow.Cells(0).Value)
            Total_Previouse_Amount = Me.grd.GetTotal(Me.grd.RootTable.Columns("Amount"), Janus.Windows.GridEX.AggregateFunction.Sum)
            If cmbVoucherType.Text.Equals("Bank") Then


                Me.pnlBank.Visible = True
                Me.pnlBank.Enabled = True

                Me.pnlDetail.Size = New Size(903, 449)
                Me.grd.RootTable.Columns("Cheque_No").Visible = True
                Me.grd.RootTable.Columns("Cheque_Date").Visible = True
                Me.grd.RootTable.Columns("BankDescription").Visible = True
                'Me.cmbCashAccount.Enabled = True ''17-Mar-2014 TASK:M27 Editable Bank Account In Payment Voucher
                'Me.cmbVoucherType.Enabled = False

            ElseIf cmbVoucherType.Text.Equals("Cash") Then


                Me.pnlBank.Visible = False
                Me.pnlBank.Enabled = False
                Me.pnlDetail.Size = New Size(903, 393)
                Me.grd.RootTable.Columns("Cheque_No").Visible = False
                Me.grd.RootTable.Columns("Cheque_Date").Visible = False
                Me.grd.RootTable.Columns("BankDescription").Visible = False
                ' Me.cmbCashAccount.Enabled = True
                'Me.cmbVoucherType.Enabled = True
            End If


            ' Me.chkAll.Checked = True
            Me.chkChecked.Checked = grdVouchers.CurrentRow.Cells("Checked").Value
            ''TASK TFS2018
            If Me.chkChecked.Checked = True Then
                Me.chkChecked.Text = "Checked By"
                Me.lblCheckedBy.Text = Me.grdVouchers.CurrentRow.Cells("CheckedUser").Value.ToString
            Else
                Me.chkChecked.Text = "Checked"
                Me.lblCheckedBy.Text = ""
            End If
            ''END TASK TFS2018
            'Me.cmbCostCenter.Enabled = False
            Me.BtnSave.Text = "&Update"
            blnEditMode = True
            GetSecurityRights()
            'Me.GetTotal()
            'Me.BtnSave.Text = "&Update"
            ''Ayesha Rehman :TFS2701 :Making Approval Button Enable in Edit Mode
            ApprovalProcessId = getConfigValueByType("ReceiptApproval")
            If ApprovalProcessId = 0 Then
                Me.btnApprovalHistory.Visible = False
                Me.btnApprovalHistory.Enabled = False
            Else
                Me.btnApprovalHistory.Visible = True
                Me.btnApprovalHistory.Enabled = True
                Me.lblPostedBy.Visible = False
                Me.chkPost.Visible = True
                Me.lblCheckedBy.Visible = False
                Me.chkChecked.Visible = True
            End If
            ''Ayesha Rehman :TFS2701 :End
            Me.chkPost.Checked = grdVouchers.CurrentRow.Cells("Post").Value
            ''TASK TFS2018
            If Me.chkPost.Checked = True Then
                Me.chkPost.Text = "Posted By"
                Me.lblPostedBy.Text = Me.grdVouchers.CurrentRow.Cells("PostedUser").Value.ToString
            Else
                Me.chkPost.Text = "Posted"
                Me.lblPostedBy.Text = ""
            End If
            ''END TASK TFS2018
            Me.UltraTabControl1.SelectedTab = Me.UltraTabPageControl1.Tab
            Me.lblPrintStatus.Text = "Print Status: " & Me.grdVouchers.GetRow.Cells("Print Status").Text.ToString

            If flgDateLock = True Then
                If Convert.ToDateTime(CDate(MyDateLock.ToString("yyyy-M-d 00:00:00"))) >= Convert.ToDateTime(CDate(Me.dtVoucherDate.Value.ToString("yyyy-M-d 00:00:00"))) Then
                    'ShowErrorMessage("Previous date work not allowed") : Exit Sub
                    Me.dtVoucherDate.Enabled = False
                Else
                    Me.dtVoucherDate.Enabled = True
                End If
            Else
                Me.dtVoucherDate.Enabled = True
            End If
            'Altered Against Task# 2015060001 Ali Ansari
            'Get no of attached files
            Dim intCountAttachedFiles As Integer = 0I
            If Me.BtnSave.Text <> "&Save" Then
                If Me.grdVouchers.RowCount > 0 Then
                    intCountAttachedFiles = Val(grdVouchers.CurrentRow.Cells("No Of Attachment").Value)
                    Me.btnAttachment.Text = "Attachment (" & intCountAttachedFiles & ")"
                End If
            End If

            'Task#04082015 Edit Company Name altered by Ahmad Sharif
            Me.cmbCompany.Text = Me.grdVouchers.CurrentRow.Cells("CompanyName").Value.ToString
            'End Task#04082015

            'Altered Against Task# 2015060001 Ali Ansari
            'AddHandler cmbVoucherType.SelectedIndexChanged, AddressOf cmbVoucherType_SelectedIndexChanged


            If getConfigValueByType("EnabledDuplicateVoucherLog").ToString.ToUpper = "TRUE" Then
                Dim intCountVouchers As Integer = 0
                Dim dtCountVouches As New DataTable
                dtCountVouches = GetDuplicateVouchers(Convert.ToInt32(lngSelectedVoucherId))
                dtCountVouches.AcceptChanges()
                If dtCountVouches.Rows.Count > 0 Then
                    intCountVouchers = dtCountVouches.Rows.Count
                    Me.btnUpdateTimes.Visible = True
                    btnUpdateTimes.Text = "No of update times (" & intCountVouchers & ")"
                    Call CreateContextMenu(Val(Me.grdVouchers.GetRow.Cells("Voucher_Id").Value.ToString), btnUpdateTimes)
                Else
                    Me.btnUpdateTimes.Visible = False
                End If
            End If
            Me.btnCancel.Visible = False
            Me.btnReplace.Visible = False
            Me.btnAdd.Visible = True
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ValidateInput() As Boolean

        Try

            If cmbAccounts.ActiveRow.Cells(0).Value <= 0 Then
                ShowErrorMessage("Please select customer account")
                cmbAccounts.Focus()
                Return False
            End If

            If txtAmount.Text = "" Then
                ShowErrorMessage("Please enter amount")
                txtAmount.Focus()
                Return False
            End If
            If cmbIncomeTaxAccount.SelectedIndex <= 0 AndAlso Val(txtTax.Text) > 0 Then
                ShowErrorMessage("Please select Income account")
                cmbCashAccount.Focus()
                Return False
            End If

            If cmbSalesTax.SelectedIndex <= 0 AndAlso Val(txtSalesTax.Text) > 0 Then
                ShowErrorMessage("Please select Sales account")
                cmbCashAccount.Focus()
                Return False
            End If


            If cmbCashAccount.SelectedIndex <= 0 Then
                ShowErrorMessage("Please select deposit account")
                cmbCashAccount.Focus()
                Return False
            End If
            'TASK-407
            If Val(txtCurrencyRate.Text) = 0 Then
                ShowErrorMessage("Currency rate value more than 0 is required")
                Me.txtCurrencyRate.Focus()
                Return False
            End If
            'END TASK-407

            'If cmbVoucherType.SelectedValue = "5" Then
            '    If txtChequeNo.Text = "" Then
            '        ShowErrorMessage("Please enter cheque No.")
            '        txtChequeNo.Focus()
            '        Return False
            '    End If
            'End If

            'If Me.cmbVoucherType.SelectedValue = "5" Then
            '    If Not Me.BackendValidation() Then
            '        MsgBox("Cheque No Already Exist", MsgBoxStyle.Critical)
            '        Me.txtChequeNo.Focus()
            '        Return False
            '    End If
            'End If
            '' Validate Cheque No
            'TASK:M13            Imran Ali        Auto Cheque Book Validation Problem
            'If getConfigValueByType("EnableAutoChequeBook").ToString = "True" Then
            '    If Me.cmbVoucherType.SelectedIndex > 0 Then
            '        If IsValidateChequeSerialNo(Me.cmbCashAccount.SelectedValue, Me.txtChequeNo.Text.Trim) = False Then
            '            Me.txtChequeNo.Focus()
            '            Return False
            '        End If
            '        If Me.grd.RowCount > 0 Then
            '            Dim dt As DataTable = CType(Me.grd.DataSource, DataTable)
            '            Dim dr() As DataRow
            '            dr = dt.Select("Cheque_No=N'" & Me.txtChequeNo.Text.Trim & "'")
            '            If dr IsNot Nothing Then
            '                If dr.Length > 0 Then
            '                    msg_Error("Cheque No: [" & Me.txtChequeNo.Text.Trim & "] is already added")
            '                    Return False
            '                End If
            '            End If
            '        End If
            '    End If
            'End If


            If Me.cmbAccounts.ActiveRow.Cells("Type").Value.ToString.Trim.ToUpper = "LC" Then
                If Me.cmbCostCenter.SelectedIndex <= 0 Then
                    ShowErrorMessage("Please select cost center")
                    Me.cmbCostCenter.Focus()
                    Return False
                End If
            End If
            ''TASK TFS3111
            'If RestrictEntryInParentDetailAC = True Then
            '    If IsParentAccount(Me.cmbAccounts.Value) Then
            '        ShowErrorMessage("Account enrty is restricted. It is a parent account.")
            '        cmbAccounts.Focus()
            '        Return False
            '    End If
            'End If
            ''END TASK TFS3111
            'Change by murtaza default currency rate(10/26/2022) 
            If cmbCurrency.SelectedValue <> BaseCurrencyId AndAlso Val(txtCurrencyRate.Text) = 1 Then
                msg_Error(cmbCurrency.Text + "Currency Rate cannot be 1")
                txtCurrencyRate.Focus() : ValidateInput = False : Exit Function
            End If
            'Change by murtaza default currency rate(10/26/2022)

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmCustomerCollection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Try
            'R-974 Ehtisham ul Haq user friendly system modification on 29-12-13 
            If e.KeyCode = Keys.F4 Then
                If Me.BtnSave.Enabled = True Then
                    SaveToolStripButton_Click(Nothing, Nothing)
                End If
                Exit Sub
            End If
            If e.KeyCode = Keys.Escape Then
                '''''''Task No 2619 Mughees Escape Code Updation 
                If Me.grd.RowCount > 0 Then
                    '    If Not msg_Confirm(str_ConfirmGridClear) = True Then Exit Sub
                    NewToolStripButton_Click(Nothing, Nothing)
                    Exit Sub
                End If
                'End Task 2619

            End If
            If e.KeyCode = Keys.P AndAlso e.Control = True Then
                BtnPrint_ButtonClick(Nothing, Nothing)
                Exit Sub
            End If
            If e.KeyCode = Keys.Insert Then
                btnAdd_Click(Nothing, Nothing)
            End If
            If e.KeyCode = Keys.F5 Then
                BtnRefresh_Click(Nothing, Nothing)
            End If
            If e.KeyCode = Keys.U AndAlso e.Alt Then
                If Me.BtnSave.Text = "&Update" Then
                    If Me.BtnSave.Enabled = False Then
                        RemoveHandler Me.BtnSave.Click, AddressOf Me.SaveToolStripButton_Click
                    End If
                End If
            End If
            If e.KeyCode = Keys.D AndAlso e.Alt Then
                If Me.BtnSave.Text = "&Delete" Then
                    If Me.BtnDelete.Enabled = False Then
                        RemoveHandler Me.BtnDelete.Click, AddressOf Me.DeleteToolStripButton_Click
                    End If
                End If
            End If
            '--------------------------------------------
            'short key for attachment f6 added by zainab
            If e.KeyCode = Keys.F6 Then
                If BtnSave.Enabled = True Then
                    btnAttachment_ButtonClick(btnAttachment, Nothing)
                End If
            End If
            'Old Reciept
            If e.KeyCode = Keys.O AndAlso e.Alt Then
                btnBackToOldReciept_Click(Nothing, Nothing)
            End If


            'Edit
            If e.KeyCode = Keys.E AndAlso e.Alt Then
                If Me.BtnSave.Enabled = True Then
                    OpenToolStripButton_Click(Nothing, Nothing)
                End If
            End If

            ''Delete Alt+T
            'If e.KeyCode = Keys.T AndAlso e.Alt Then
            '    If Me.btnSearchDelete.Enabled = True Then
            '        btnSearchDelete_Click(Nothing, Nothing)
            '    End If
            'End If

            'Reminder ctrl+R
            If e.KeyCode = Keys.R AndAlso e.Control Then
                btnReminder_Click(Nothing, Nothing)
            End If
            'Print Update Voucher ctrl+V+P
            'If e.KeyCode = Keys.V AndAlso e.Control AndAlso Keys.P Then
            '    If btnPrintUpdatedVoucher.Enabled = True Then
            '        btnPrintUpdatedVoucher_Click(Nothing, Nothing)
            '    End If
            'End If
            'Load All ctrl+L
            If e.KeyCode = Keys.L AndAlso e.Control Then
                btnHistoryLoadAll_Click(Nothing, Nothing)
            End If
            'Save Attachment F6+ctrl
            If e.KeyCode = Keys.F6 AndAlso e.Control Then
                If BtnSave.Enabled = True And btnAttachment.Enabled Then
                    Btn_SaveAttachment_Click(Nothing, Nothing)
                End If
            End If
            'SMS template F8
            If e.KeyCode = Keys.F8 Then
                btnSMSTemplate_Click(Nothing, Nothing)
            End If

            If e.KeyCode = Keys.F3 Then
                btnSalesInvoice_Click(Nothing, Nothing)
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    'R-974 Ehtisham ul Haq user friendly system modification on 29-12-13 
    Private Sub frmCustomerCollection_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try
            Me.lblProgress.Text = "Loading Please Wait ..."
            Me.lblProgress.BackColor = Color.LightYellow
            Me.lblProgress.Visible = True
            Me.Cursor = Cursors.WaitCursor
            Application.DoEvents()

            Neodynamic.SDK.Barcode.BarcodeProfessional.LicenseKey = "KD83KNYYMA6XA6E7HNEL7QEUSGXGLKPQVCH7YVMEPXD7BG5FUCPA"
            Neodynamic.SDK.Barcode.BarcodeProfessional.LicenseOwner = "LumenSoft Technologies-Standard Edition-OEM Developer License"

            If Not getConfigValueByType("CompanyRights").ToString = "Error" Then
                flgCompanyRights = getConfigValueByType("CompanyRights")
            End If
            'TASK : TFS1265
            'If Not getConfigValueByType("MemoRemarks").ToString = "Error" Then
            '    flgMemoRemarks = getConfigValueByType("MemoRemarks")
            'End If
            'End TASK : TFS1265
            'TASK:2577 Get Branded SMS Configuration
            If Not getConfigValueByType("EnabledBrandedSMS").ToString = "Error" Then
                EnabledBrandedSMS = getConfigValueByType("EnabledBrandedSMS")
            End If
            'End Task:2577
            'TASK TFS3111
            If Not getConfigValueByType("RestrictEntryInParentDetailAC").ToString = "Error" Then
                RestrictEntryInParentDetailAC = CBool(getConfigValueByType("RestrictEntryInParentDetailAC"))
            End If
            ''END TASK TFS3111
            Me.FillPaymentMethod()
            FillCombo("SM")
            FillCombo("Customer")
            FillCombo("Bank")
            'FillCombo("CostCenter")
            FillCombo("Company")
            'PopulateGrid()

            BaseCurrencyId = Val(getConfigValueByType("Currency").ToString)
            BaseCurrencyName = GetBasicCurrencyName(BaseCurrencyId)

            FillCombo("Currency") 'TASK-407
            Me.GetSecurityRights()
            ClearFields()

            IsLoadedForm = True
            'Me.PopulateGrid() R933 Commented History Data
            Get_All(frmModProperty.Tags)

            'TFS3360
            UltraDropDownSearching(cmbAccounts, frmModProperty.blnListSeachStartWith, frmModProperty.blnListSeachContains)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        Finally
            Me.lblProgress.Visible = False
            Me.Cursor = Cursors.Default
            If frmModProperty.Tags.Length > 0 Then frmModProperty.Tags = String.Empty ''18-Feb-2014 Task:2429 Imran Ali 1-error in payable/receivable tracing
        End Try
    End Sub

    Private Sub FillCombo(Optional ByVal Condition As String = "")
        Try
            Dim str As String = String.Empty
            If Condition = "Customer" Then

                str = "SELECT dbo.vwCOADetail.coa_detail_id AS Id, dbo.vwCOADetail.detail_title as Name,dbo.vwCOAdetail.detail_code as Code, dbo.vwCOADetail.sub_sub_title AS [Sub Sub A/c],Account_Type as Type, dbo.tblListState.StateName as State, dbo.tblListCity.CityName as City,  " & _
                           " dbo.tblListTerritory.TerritoryName as Territory, vwCOADetail.Contact_Phone as Phone, vwCOADetail.Contact_Mobile as Mobile, Isnull(vwCOADetail.AccessLevel,'Everyone') as AccessLevel  " & _
                           " FROM         dbo.tblCustomer INNER JOIN " & _
                           " dbo.tblListTerritory ON dbo.tblCustomer.Territory = dbo.tblListTerritory.TerritoryId INNER JOIN " & _
                           " dbo.tblListCity ON dbo.tblListTerritory.CityId = dbo.tblListCity.CityId INNER JOIN " & _
                           " dbo.tblListState ON dbo.tblListCity.StateId = dbo.tblListState.StateId RIGHT OUTER JOIN " & _
                           " dbo.vwCOADetail ON dbo.tblCustomer.AccountId = dbo.vwCOADetail.coa_detail_id " & _
                           " WHERE  detail_title is not null " & IIf(RestrictEntryInParentDetailAC = True, " AND dbo.vwCOADetail.coa_detail_id NOT IN (SELECT DISTINCT Parent_Id From dbo.vwCOADetail WHERE ISNULL(Parent_Id, 0) > 0) ", "") & " "
                If flgCompanyRights = True Then
                    str += " AND vwCOADetail.CompanyId=" & MyCompanyId
                End If

                ''Start TFS3322 : Ayesha Rehman : 15-05-2018
                'If LoginGroup = "Administrator" Then
                If GetMappedUserId() > 0 And getGroupAccountsConfigforAccounts(Me.Name) And LoginGroup <> "Administrator" Then
                    str = "SELECT dbo.vwCOADetail.coa_detail_id AS Id, dbo.vwCOADetail.detail_title as Name,dbo.vwCOAdetail.detail_code as Code, dbo.vwCOADetail.sub_sub_title AS [Sub Sub A/c],Account_Type as Type, dbo.tblListState.StateName as State, dbo.tblListCity.CityName as City,  " & _
                          " dbo.tblListTerritory.TerritoryName as Territory, vwCOADetail.Contact_Phone as Phone, vwCOADetail.Contact_Mobile as Mobile, Isnull(vwCOADetail.AccessLevel,'Everyone') as AccessLevel  " & _
                          " FROM         dbo.tblCustomer INNER JOIN " & _
                          " dbo.tblListTerritory ON dbo.tblCustomer.Territory = dbo.tblListTerritory.TerritoryId INNER JOIN " & _
                          " dbo.tblListCity ON dbo.tblListTerritory.CityId = dbo.tblListCity.CityId INNER JOIN " & _
                          " dbo.tblListState ON dbo.tblListCity.StateId = dbo.tblListState.StateId RIGHT OUTER JOIN " & _
                          " dbo.vwCOADetail ON dbo.tblCustomer.AccountId = dbo.vwCOADetail.coa_detail_id " & _
                          " WHERE  detail_title is not null "
                    str += " And (coa_detail_id in (Select COAAccountMapping.AccountId FROM COAAccountMapping INNER JOIN COAGroups ON COAAccountMapping.COAGroupId = COAGroups.COAGroupId INNER JOIN COAUserMapping ON COAGroups.COAGroupId = COAUserMapping.COAGroupId WHERE (COAAccountMapping.AccountLevel = 3) and COAUserMapping.[User_Id]= " & LoginGroupId & " ) " _
                           & " or main_sub_sub_id in (SELECT COAAccountMapping.AccountId FROM COAAccountMapping INNER JOIN COAGroups ON COAAccountMapping.COAGroupId = COAGroups.COAGroupId INNER JOIN COAUserMapping ON COAGroups.COAGroupId = COAUserMapping.COAGroupId WHERE (COAAccountMapping.AccountLevel = 2) and COAUserMapping.[User_Id]= " & LoginGroupId & " ) " _
                           & " or main_sub_id in (SELECT COAAccountMapping.AccountId FROM COAAccountMapping INNER JOIN COAGroups ON COAAccountMapping.COAGroupId = COAGroups.COAGroupId INNER JOIN COAUserMapping ON COAGroups.COAGroupId = COAUserMapping.COAGroupId WHERE (COAAccountMapping.AccountLevel = 1) and COAUserMapping.[User_Id]= " & LoginGroupId & " ) " _
                           & " or coa_main_id in (SELECT   COAAccountMapping.AccountId FROM COAAccountMapping INNER JOIN COAGroups ON COAAccountMapping.COAGroupId = COAGroups.COAGroupId INNER JOIN COAUserMapping ON COAGroups.COAGroupId = COAUserMapping.COAGroupId WHERE (COAAccountMapping.AccountLevel = 0) and COAUserMapping.[User_Id]= " & LoginGroupId & "))"
                End If
                ''End TFS3322
                ''Start TFS4741
                If Me.chkAll.Checked = False Then
                    str += " And (dbo.vwCOADetail.account_type = 'Customer') "
                End If
                ''End TFS4741
                If blnEditMode = False Then
                    str += " And vwCOADetail.Active=1"
                Else
                    str += " And vwCOADetail.Active in (1,0,NULL)"
                End If
                str += " order by tblCustomer.Sortorder, vwCOADetail.detail_title "
                FillUltraDropDown(Me.cmbAccounts, str) '"select coa_detail_id,detail_title from vwCoaDetail where account_type='Customer'")
                Me.cmbAccounts.Rows(0).Activate()
                If Me.cmbAccounts.DisplayLayout.Bands.Count > 0 Then
                    Me.cmbAccounts.DisplayLayout.Bands(0).Columns("AccessLevel").Hidden = True
                    Me.cmbAccounts.DisplayLayout.Bands(0).Columns(0).Hidden = True
                    Me.cmbAccounts.DisplayLayout.Bands(0).Columns("Phone").Hidden = True 'Task:2577 Set Phone Field Hidden 
                End If
            ElseIf Condition = "SearchCustomer" Then
                str = "SELECT dbo.vwCOADetail.coa_detail_id AS Id, dbo.vwCOADetail.detail_title as Name,dbo.vwCOADetail.sub_sub_title AS [Sub Sub A/c], dbo.tblListState.StateName as State, dbo.tblListCity.CityName as City,  " & _
                                                   " dbo.tblListTerritory.TerritoryName as Territory , Isnull(vwCOADetail.AccessLevel,'Everyone') as AccessLevel " & _
                                                   " FROM         dbo.tblCustomer INNER JOIN " & _
                                                   " dbo.tblListTerritory ON dbo.tblCustomer.Territory = dbo.tblListTerritory.TerritoryId INNER JOIN " & _
                                                   " dbo.tblListCity ON dbo.tblListTerritory.CityId = dbo.tblListCity.CityId INNER JOIN " & _
                                                   " dbo.tblListState ON dbo.tblListCity.StateId = dbo.tblListState.StateId RIGHT OUTER JOIN " & _
                                                   " dbo.vwCOADetail ON dbo.tblCustomer.AccountId = dbo.vwCOADetail.coa_detail_id " & _
                                                   " WHERE (dbo.vwCOADetail.account_type = 'Customer') and detail_title is not null"
                If flgCompanyRights = True Then
                    str += " AND vwCOADetail.CompanyId=" & MyCompanyId
                End If
                ''Start TFS3322 : Ayesha Rehman : 15-05-2018
                'If LoginGroup = "Administrator" Then
                If GetMappedUserId() > 0 And getGroupAccountsConfigforAccounts(Me.Name) And LoginGroup <> "Administrator" Then
                    str = "SELECT dbo.vwCOADetail.coa_detail_id AS Id, dbo.vwCOADetail.detail_title as Name,dbo.vwCOADetail.sub_sub_title AS [Sub Sub A/c], dbo.tblListState.StateName as State, dbo.tblListCity.CityName as City,  " & _
                                                 " dbo.tblListTerritory.TerritoryName as Territory , Isnull(vwCOADetail.AccessLevel,'Everyone') as AccessLevel " & _
                                                 " FROM         dbo.tblCustomer INNER JOIN " & _
                                                 " dbo.tblListTerritory ON dbo.tblCustomer.Territory = dbo.tblListTerritory.TerritoryId INNER JOIN " & _
                                                 " dbo.tblListCity ON dbo.tblListTerritory.CityId = dbo.tblListCity.CityId INNER JOIN " & _
                                                 " dbo.tblListState ON dbo.tblListCity.StateId = dbo.tblListState.StateId RIGHT OUTER JOIN " & _
                                                 " dbo.vwCOADetail ON dbo.tblCustomer.AccountId = dbo.vwCOADetail.coa_detail_id " & _
                                                 " WHERE  detail_title is not null"
                    str += " And (coa_detail_id in (Select COAAccountMapping.AccountId FROM COAAccountMapping INNER JOIN COAGroups ON COAAccountMapping.COAGroupId = COAGroups.COAGroupId INNER JOIN COAUserMapping ON COAGroups.COAGroupId = COAUserMapping.COAGroupId WHERE (COAAccountMapping.AccountLevel = 3) and COAUserMapping.[User_Id]= " & LoginGroupId & " ) " _
                           & " or main_sub_sub_id in (SELECT COAAccountMapping.AccountId FROM COAAccountMapping INNER JOIN COAGroups ON COAAccountMapping.COAGroupId = COAGroups.COAGroupId INNER JOIN COAUserMapping ON COAGroups.COAGroupId = COAUserMapping.COAGroupId WHERE (COAAccountMapping.AccountLevel = 2) and COAUserMapping.[User_Id]= " & LoginGroupId & " ) " _
                           & " or main_sub_id in (SELECT COAAccountMapping.AccountId FROM COAAccountMapping INNER JOIN COAGroups ON COAAccountMapping.COAGroupId = COAGroups.COAGroupId INNER JOIN COAUserMapping ON COAGroups.COAGroupId = COAUserMapping.COAGroupId WHERE (COAAccountMapping.AccountLevel = 1) and COAUserMapping.[User_Id]= " & LoginGroupId & " ) " _
                           & " or coa_main_id in (SELECT   COAAccountMapping.AccountId FROM COAAccountMapping INNER JOIN COAGroups ON COAAccountMapping.COAGroupId = COAGroups.COAGroupId INNER JOIN COAUserMapping ON COAGroups.COAGroupId = COAUserMapping.COAGroupId WHERE (COAAccountMapping.AccountLevel = 0) and COAUserMapping.[User_Id]= " & LoginGroupId & ")) "
                    str += " And (dbo.vwCOADetail.account_type = 'Customer') "
                End If
                ''End TFS3322
                str += " order by tblCustomer.Sortorder, vwCOADetail.detail_title "
                FillUltraDropDown(Me.cmbSearchAccount, str)
                Me.cmbSearchAccount.Rows(0).Activate()
                If Me.cmbSearchAccount.DisplayLayout.Bands.Count > 0 Then
                    Me.cmbAccounts.DisplayLayout.Bands(0).Columns("AccessLevel").Hidden = True
                    Me.cmbSearchAccount.DisplayLayout.Bands(0).Columns(0).Hidden = True
                End If
            ElseIf Condition = "SearchVoucherType" Then
                Dim dt1 As New DataTable
                dt1.Columns.Add("Id", GetType(Integer))
                dt1.Columns.Add("Name", GetType(String))

                Dim dr As DataRow
                dr = dt1.NewRow
                dr(0) = Convert.ToInt32(5)
                dr(1) = "Bank"
                dt1.Rows.InsertAt(dr, 0)

                Dim dr1 As DataRow
                dr1 = dt1.NewRow
                dr1(0) = Convert.ToInt32(3)
                dr1(1) = "Cash"
                dt1.Rows.InsertAt(dr1, 0)

                Dim dr2 As DataRow
                dr2 = dt1.NewRow
                dr2(0) = Convert.ToInt32(0)
                dr2(1) = ".... Select any value ...."
                dt1.Rows.InsertAt(dr2, 0)

                Me.cmbSearchVoucherType.DisplayMember = "Name"
                Me.cmbSearchVoucherType.ValueMember = "Id"
                Me.cmbSearchVoucherType.DataSource = dt1
                'added by zainab #task1608
            ElseIf Condition = "IncomeTaxAccount" Then
                str = "select * from tblDefIncomeTaxAccount where active=1 and FormName<>'Payment' order by sortorder,2"
                FillDropDown(Me.cmbIncomeTaxAccount, str)
                'Me.txtTax.Text = "select TaxPer from tblDefIncomeTaxAccount where IncomeTaxAccId='" & cmbIncomeTaxAccount.Text & "'"

            ElseIf Condition = "SalesTaxAccount" Then
                str = "select * from  tblDefSalesTaxAccount where active=1 and FormName<>'Payment' order by sortorder,2"
                FillDropDown(Me.cmbSalesTax, str)
                'end  #task1608
            ElseIf Condition = "Bank" Then
                str = "Select DISTINCT BankDesc, BankDesc From tblVoucher WHERE BankDesc IS NOT NULL AND BankDesc <> ''"
                FillDropDown(Me.cmbBank, str, False)
            ElseIf Condition = "CostCenter" Then
                str = "If  exists(select CostCentre_Id FROM tblUserCostCentreRights where UserID = " & LoginUserId & " and ISNULL(CostCentre_Id, 0) > 0) " _
                    & "Select CostCenterID, Name from tblDefCostCenter where CostCenterID in (select CostCentre_Id FROM tblUserCostCentreRights where UserID = " & LoginUserId & ") order by SortOrder " _
                    & "Else " _
                    & "Select CostCenterID, Name from tblDefCostCenter where Active = 1 order by SortOrder"
                FillDropDown(Me.cmbCostCenter, str, True)
            ElseIf Condition = "SM" Then
                str = "select Employee_Id, Employee_Name +'-'+ Employee_Code as Employee_Name From tblDefEmployee Where Active = 1 " ''TASKTFS75 added and set active =1
                FillDropDown(Me.cmbSaleman, str)

            ElseIf Condition = "GrdCostCenter" Then
                str = "Select CostCenterId, Name From tblDefCostCenter Union Select 0, '" & strZeroIndexItem & "' Order by 2 ASC"
                Dim dt As New DataTable
                dt = GetDataTable(str)
                dt.AcceptChanges()
                Me.grd.RootTable.Columns("CostCenterId").ValueList.PopulateValueList(dt.DefaultView, "CostCenterId", "Name")

            ElseIf Condition = "GrdEmployee" Then
                str = "select Employee_Id, Employee_Name +'-'+ Employee_Code as Employee_Name From tblDefEmployee Where Active = 1 Union Select 0, '" & strZeroIndexItem & "' Order by 2 ASC"
                Dim dt As New DataTable
                dt = GetDataTable(str)
                dt.AcceptChanges()
                Me.grd.RootTable.Columns("Employee").ValueList.PopulateValueList(dt.DefaultView, "Employee_Id", "Employee_Name")
                'Task#03082015 Fill combo box with companies name
            ElseIf Condition = "Company" Then
                str = String.Empty
                str = "Select CompanyId, CompanyName, Isnull(CostCenterId,0) as CostCenterId, IsNull(CommercialInvoice,0) as CommercialInvoice from CompanyDefTable " & IIf(flgCompanyRights = True, " WHERE CompanyId=" & MyCompanyId, "") & ""
                FillDropDown(Me.cmbCompany, str, False)
                'End Task#03082015
            ElseIf Condition = "Currency" Then
                str = String.Empty
                str = "Select tblCurrency.currency_id, tblCurrency.currency_code, IsNull(tblCurrencyRate.CurrencyRate, 0) As CurrencyRate From tblCurrency Left Outer Join(Select * FROM tblCurrencyRate Where CurrencyRateId in (Select Max(CurrencyRateId) From tblCurrencyRate group by CurrencyId)) tblCurrencyRate On tblCurrency.currency_id = tblCurrencyRate.CurrencyId "
                FillDropDown(Me.cmbCurrency, str, False)
                Me.cmbCurrency.SelectedValue = BaseCurrencyId
            ElseIf Condition = "GrdIncomeTaxAccount" Then
                str = "SELECT IsNull(AccountId,0) AccountId, IncomeAccountName FROM tblDefIncomeTaxAccount WHERE Active = 1 AND FormName<>'Payment' UNION SELECT 0, '" & strZeroIndexItem & "' ORDER BY 1"
                Dim dt As DataTable
                dt = GetDataTable(str)
                dt.AcceptChanges()
                Me.grd.RootTable.Columns("IncomeAccountId").ValueList.PopulateValueList(dt.DefaultView, "AccountId", "IncomeAccountName")
            ElseIf Condition = "GrdSalesTaxAccount" Then
                str = "SELECT IsNull(AccountId,0) AccountId, SalesAccountName FROM tblDefSalesTaxAccount WHERE Active = 1 AND FormName<>'Payment' UNION SELECT 0, '" & strZeroIndexItem & "' ORDER BY 1"
                Dim dt As DataTable
                dt = GetDataTable(str)
                dt.AcceptChanges()
                Me.grd.RootTable.Columns("AccountId").ValueList.PopulateValueList(dt.DefaultView, "AccountId", "SalesAccountName")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Private Sub cmbVoucherType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbVoucherType.SelectedIndexChanged
        Try
            'If Not blnEditMode = True Then
            '    Me.txtVoucherNo.Text = GetVoucherNo()
            'End If
            'If IsLoadedForm = True Then Me.cmbAccounts.Focus()
            Dim Str As String = "If  exists(select Account_Id FROM tblUserAccountRights where UserID = " & LoginUserId & " And Rights = 1 And Account_Id Is Not Null) " _
                      & " select coa_detail_id, detail_title from vwCoaDetail where account_type=N'" & Me.cmbVoucherType.Text & "' And coa_detail_id in (select Account_Id FROM tblUserAccountRights where UserID = " & LoginUserId & " And Rights = 1) " & IIf(flgCompanyRights = True, " AND vwCoaDetail.CompanyId=" & MyCompanyId & "", "") & " " _
                      & " Else " _
                      & " select coa_detail_id, detail_title from vwCoaDetail where account_type=N'" & Me.cmbVoucherType.Text & "' " & IIf(flgCompanyRights = True, " AND vwCoaDetail.CompanyId=" & MyCompanyId & "", "") & " "

            'FillDropDown(Me.cmbCashAccount, "select coa_detail_id,detail_title from vwCoaDetail where account_type=N'" & Me.cmbVoucherType.Text & "' " & IIf(flgCompanyRights = True, " AND vwCoaDetail.CompanyId=" & MyCompanyId & "", "") & "")
            FillDropDown(Me.cmbCashAccount, Str)

            'Start Task 3913 Saad Afzaal Adjust combo box drop down list width to longest string width 

            SetComboBoxMaxWidth(cmbCashAccount)

            'End Task

            If blnEditMode = True Then Exit Sub

            If cmbVoucherType.Text.Equals("Bank") Then
                Me.txtVoucherNo.Text = GetVoucherNo()
                'Me.Panel2.Size = New Size(718, 317)
                Me.pnlBank.Visible = True
                Me.pnlBank.Enabled = True
                'Me.pnlBank.Location = New Point(6, 191)
                'Me.pnlCost.Location = New Point(3, 52)
                'Me.pnlDetail.Location = New Point(20, 54)
                'Me.pnlGrd.Location = New Point(20, 507)
                ' Me.Panel2.Size = New Size(676, 316)
                'Me.grd.Location = New Point(0, 498)
                'Me.pnlDetail.Size = New Size(903, 449)
                'Me.pnlGrd.Size = New Size(1031, 145)
                Me.grd.RootTable.Columns("Cheque_No").Visible = True
                Me.grd.RootTable.Columns("Cheque_Date").Visible = True
                Me.grd.RootTable.Columns("BankDescription").Visible = True

            ElseIf cmbVoucherType.Text.Equals("Cash") Then
                Me.txtVoucherNo.Text = GetVoucherNo()
                'Me.Panel2.Size = New Size(676, 231)
                Me.pnlBank.Visible = False
                Me.pnlBank.Enabled = False
                ' Me.pnlDetail.Location = New Point(20, 54)
                'Me.pnlGrd.Location = New Point(20, 444)
                ' Me.pnlCost.Location = New Point(6, 191)
                'Me.Panel2.Size = New Size(676, 265)
                Me.pnlDetail.Size = New Size(903, 393)
                ' Me.pnlGrd.Location = New Point(1, 431)
                ' Me.pnlGrd.Dock = DockStyle.None
                'Me.pnlGrd.Width = 241
                Me.grd.RootTable.Columns("Cheque_No").Visible = False
                Me.grd.RootTable.Columns("Cheque_Date").Visible = False
                Me.grd.RootTable.Columns("BankDescription").Visible = False
                'Me.cmbCashAccount.Enabled = True
                'Me.cmbVoucherType.Enabled = True
            End If

            If Not blnFirstTimeInvoked Then
                PopulateGrid(IIf(cmbVoucherType.SelectedValue = 3, "Cash", "Bank"))
            End If
            setVoucherType = Me.cmbVoucherType.Text
            blnFirstTimeInvoked = False
            'changing:

        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub SaveToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Try

            'ValidateDateLock()
            'If flgDateLock = True Then ShowErrorMessage("Previous date work not allowed") : Exit Sub
            'If flgDateLock = True Then
            '    If Convert.ToDateTime(CDate(MyDateLock).ToString("yyyy-M-d 00:00:00")) >= Convert.ToDateTime(CDate(Me.dtVoucherDate.Value.ToString("yyyy-M-d 00:00:00"))) Then
            '        
            '    End If
            'End If
            If Me.BtnSave.Enabled = False Then Exit Sub
            If IsDateLock(Me.dtVoucherDate.Value) = True Then
                ShowErrorMessage(str_ErrorPreviouseDateRecordUpdateAllow) : Exit Sub
            End If
            'Change by murtaza default currency rate(10/26/2022) 
            If cmbCurrency.SelectedValue <> BaseCurrencyId AndAlso Val(txtCurrencyRate.Text) = 1 Then
                msg_Error(cmbCurrency.Text + "Currency Rate cannot be 1")
                txtCurrencyRate.Focus()
                Exit Sub
            End If
            'Change by murtaza default currency rate(10/26/2022)
            If Me.dtVoucherDate.Value <= Convert.ToDateTime((getConfigValueByType("EndOfDate").ToString)) Then
                ShowErrorMessage("You can not change this becuase financial year is closed")
                Me.dtVoucherDate.Focus()
                Exit Sub
            End If
            If cmbCashAccount.SelectedIndex <= 0 Then
                ShowErrorMessage("Please select deposit account")
                cmbCashAccount.Focus()
                Exit Sub
            End If

            For Each r As Janus.Windows.GridEX.GridEXRow In Me.grd.GetRows
                If r.Cells("Cheque_No").Value.ToString.Length > 0 Then
                    'If ValidateduplicateChequeInGrid(r.Cells("Cheque_No").Value.ToString) = False Then
                    '    Exit Sub
                    'End If
                    Dim rowStyle As Janus.Windows.GridEX.GridEXFormatStyle = New Janus.Windows.GridEX.GridEXFormatStyle()
                    Dim strCheque As String = r.Cells("Cheque_No").Value.ToString
                    Dim vendorID As Integer = r.Cells("coa_detail_id").Value
                    If strCheque.Length > 0 Then
                        If IsValidateChequeReceipt(strCheque, vendorID, lngSelectedVoucherId) = False Then
                            'Throw New Exception("This cheque is already issued, Please issue another cheque.")
                            rowStyle.BackColor = Color.Ivory
                            r.RowStyle = rowStyle
                            Exit Sub
                        Else
                            rowStyle.BackColor = Color.White
                            r.RowStyle = rowStyle
                        End If
                    Else
                        rowStyle.BackColor = Color.White
                    End If
                Else
                    Exit For
                End If
            Next
            If Not Me.grd.RowCount = 0 Then
                Me.grd.UpdateData()
                If blnEditMode = True Then

                    'R-974 Ehtisham ul Haq user friendly system modification on 29-12-13 
                    'If Not BtnSave.Text = "Save" And Not BtnSave.Text = "&Save" Then
                    'Comment against TASK:2398           Imran Ali        Update, Delete Problem in Cash Management
                    'If Not msg_Confirm(str_ConfirmUpdate) = True Then

                    'End If
                    'Exit Sub
                    'Else

                    ''Start TFS2988
                    If ValidateApprovalProcessMapped(Me.txtVoucherNo.Text.Trim, Me.Name) Then
                        If ValidateApprovalProcessInProgress(Me.txtVoucherNo.Text.Trim, Me.Name) Then
                            msg_Error("Document is in Approval Process ") : Exit Sub
                        End If
                    End If
                    ''End TFS2988

                    If Not msg_Confirm(str_ConfirmUpdate) = True Then
                        Exit Sub
                    End If

                    setEditMode = True
                    Total_Amount = Me.grd.GetTotal(Me.grd.RootTable.Columns("Amount"), Janus.Windows.GridEX.AggregateFunction.Sum)
                    'DeleteRecord("Detail Only", False)
                    SaveRecord()

                    If GetVoucherId > 0 Then
                        DualPrinting()
                        If BackgroundWorker2.IsBusy Then Exit Sub
                        BackgroundWorker2.RunWorkerAsync()
                        'Do While BackgroundWorker2.IsBusy
                        '    Application.DoEvents()
                        'Loop
                        'ExportFile(GetVoucherId)
                        'EmailSave()

                        If BackgroundWorker1.IsBusy Then Exit Sub
                        BackgroundWorker1.RunWorkerAsync()
                        'Do While BackgroundWorker1.IsBusy
                        '    Application.DoEvents()
                        'Loop
                        ClearFields()
                        flgSalesInvoice = False
                        lblInvoice.Visible = False
                        cmbInvoice.Visible = False
                        'End If
                    End If
                    GetSecurityRights()
                Else
                    setEditMode = False
                    Total_Amount = Me.grd.GetTotal(Me.grd.RootTable.Columns("Amount"), Janus.Windows.GridEX.AggregateFunction.Sum)
                    SaveRecord()
                    SendAutoEmail()
                    If GetVoucherId > 0 Then
                        ClearFields()
                        flgSalesInvoice = False
                        lblInvoice.Visible = False
                        cmbInvoice.Visible = False
                        DualPrinting()
                        If BackgroundWorker2.IsBusy Then Exit Sub
                        BackgroundWorker2.RunWorkerAsync()
                        If BackgroundWorker1.IsBusy Then Exit Sub
                        BackgroundWorker1.RunWorkerAsync()
                    End If
                    GetSecurityRights()
                End If

                blnEditMode = False
                Total_Amount = 0D
            Else
                msg_Error("There is no record in the grid")
            End If
            Msgfrm.chkEnableVoucherPrints.Visible = False
            Msgfrm.chkEnableSlipPrints.Visible = False
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub SendAutoEmail(Optional ByVal Activity As String = "")
        Try
            GetTemplate("Receipt")
            If EmailTemplate.Length > 0 Then
                GetAutoEmailData()
                UsersEmail = New List(Of String)
                'UsersEmail.Add("adil@agriusit.com")
                If Con.Database.Contains("Remms") Then
                    UsersEmail.Add("r.ejaz@remmsit.com")
                Else
                    UsersEmail.Add("r.ejaz@agriusit.com")
                End If
                FormatStringBuilder(dtEmail)
                For Each _email As String In UsersEmail
                    CreateOutLookMail(_email)
                    SaveEmailLog(VoucherNo, _email, "frmCustomerCollection", Activity)
                Next
            Else
                ShowErrorMessage("No email template is found for Receipt.")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub FormatStringBuilder(ByVal dt As DataTable)
        Try
            html = New StringBuilder
            html.Append(EmailTemplate)
            html.Append("<table border = '1'>")
            html.Append("<tr bgcolor='#58ACFA'>")
            For Each column As DataColumn In dt.Columns
                Dim ColumnName As String = ""
                Dim Pattern = "([a-z?])[_ ]?([A-Z])"
                If column.ColumnName = "SerialNo" Then
                    ColumnName = "Sr#"
                Else
                    ColumnName = Regex.Replace(column.ColumnName, Pattern, "$1 $2")
                End If
                html.Append("<th>")
                html.Append(ColumnName)
                html.Append("</th>")
            Next
            html.Append("</tr>")
            For Each row As DataRow In dt.Rows
                If row.Table.Columns.Contains("Alternate") Then
                    If row.Item("Alternate") = "Yes" Then
                        html.Append("<tr bgcolor='#A9F5BC'>")
                        For Each column As DataColumn In dt.Columns
                            html.Append("<td>")
                            html.Append(row(column.ColumnName))
                            html.Append("</td>")
                        Next
                        html.Append("</tr>")
                    Else
                        html.Append("<tr>")
                        For Each column As DataColumn In dt.Columns
                            html.Append("<td>")
                            html.Append(row(column.ColumnName))
                            html.Append("</td>")
                        Next
                        html.Append("</tr>")
                    End If
                Else
                    html.Append("<tr>")
                    For Each column As DataColumn In dt.Columns
                        html.Append("<td>")
                        html.Append(row(column.ColumnName))
                        html.Append("</td>")
                    Next
                    html.Append("</tr>")
                End If
            Next
            html.Append("</table>")
            html.Append(AfterFieldsElement)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub GetAutoEmailData()
        Dim Dr As DataRow
        Try
            Dim str As String
            str = "select TOP 1 voucher_id, voucher_no, voucher_date from tblvoucher where source in ('frmCustomerCollection', 'frmOldCustomerCollection') order by 1 desc"
            Dim dt1 As DataTable = GetDataTable(str)
            If dt1.Rows.Count > 0 Then
                VoucherId = dt1.Rows(0).Item("voucher_id")
                VoucherNo = dt1.Rows(0).Item("voucher_no")
            End If
            Dim str1 As String
            str1 = " SELECT vwCOADetail.detail_title, vwCOADetail.detail_code, ISNULL(tblVoucherDetail.CurrencyAmount, tblVoucherDetail.credit_amount) AS CurrencyAmount, ISNULL(tblVoucherDetail.CurrencyRate, 0) AS CurrencyRate, ISNULL(tblVoucherDetail.BaseCurrencyRate, 0) AS BaseCurrencyRate, tblVoucherDetail.credit_amount AS Amount, tblVoucherDetail.Adjustment AS CurrencyDiscount, ISNULL(tblVoucherDetail.Tax_Percent, 0) AS Tax, ISNULL(tblVoucherDetail.Tax_Amount, 0) AS Tax_Currency_Amount,  ISNULL(tblVoucherDetail.Tax_Amount, 0) AS Tax_Amount, ISNULL(tblVoucherDetail.SalesTaxPer, 0) AS SalesTaxPer,  ISNULL(tblVoucherDetail.SalesTaxAmount, 0) AS SalesTaxAmount, ISNULL(tblVoucherDetail.SalesTaxAmount, 0) AS Sales_Tax, ISNULL(tblVoucherDetail.WHTaxPer, 0) AS WHTaxPer,  ISNULL(tblVoucherDetail.WHTaxAmount, 0) AS WHTaxAmount, ISNULL(tblVoucherDetail.WHTaxAmount, 0) AS WH_Tax, ISNULL(tblVoucherDetail.NetAmount, 0) AS NetAmount, ISNULL(tblVoucherDetail.NetAmount, 0) AS Net_Amount, tblVoucherDetail.comments AS Reference, tblVoucherDetail.Cheque_No, ISNULL(tblVoucherDetail.Cheque_Date, GETDATE()) AS Cheque_Date, tblVoucherDetail.BankDescription, Cust.Mobile,  vwCOADetail.account_type AS Type, SalesMasterTable.SalesNo as InvoiceNo FROM tblVoucherDetail INNER JOIN vwCOADetail ON tblVoucherDetail.coa_detail_id = vwCOADetail.coa_detail_id LEFT OUTER JOIN SalesMasterTable ON tblVoucherDetail.InvoiceId = SalesMasterTable.SalesId LEFT OUTER JOIN tblCustomer AS Cust ON Cust.AccountId = vwCOADetail.coa_detail_id LEFT OUTER JOIN (SELECT DISTINCT VoucherDetailId FROM InvoiceAdjustmentTable) AS InvAdj ON InvAdj.VoucherDetailId = tblVoucherDetail.voucher_detail_id  " _
              & " Where voucher_id =" & VoucherId & " AND (tblVoucherDetail.credit_amount > 0) Order By tblVoucherDetail.Voucher_Detail_Id ASC "
            'rafay :Add this filter to show value after decimal 2 in outlook mail
            Me.grd.RootTable.Columns("NetAmount").FormatString = "N2"
            Me.grd.RootTable.Columns("Net_Amount").FormatString = "N2"
            Me.grd.RootTable.Columns("Tax_Currency_Amount").FormatString = "N2"
            Me.grd.RootTable.Columns("Tax_Amount").FormatString = "N2"
            Me.grd.RootTable.Columns("Tax_Currency_Amount").FormatString = "N2"
            Me.grd.RootTable.Columns("WHTaxAmount").FormatString = "N2"
            Me.grd.RootTable.Columns("WH_Tax").FormatString = "N2"
            Me.grd.RootTable.Columns("SalesTaxAmount").FormatString = "N2"
            'rafay

            Dim dt As DataTable = GetDataTable(str1)
            For Each Row1 As DataRow In dt.Rows
                Dr = dtEmail.NewRow
                For Each col As String In AllFields
                    If Row1.Table.Columns.Contains(col) Then
                        Dr.Item(col) = Row1.Item(col).ToString
                    End If
                Next
                dtEmail.Rows.Add(Dr)
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub



    Private Sub GetTemplate(ByVal Title As String)
        Dim Fields As String = String.Empty

        Try
            dtEmail = New DataTable
            EmailTemplate = EmailDAL.GetTemplate(Title)
            If EmailTemplate.Length > 0 Then
                Dim i, j As Integer
                i = EmailTemplate.IndexOf("<Fields>") + "<Fields>".Length
                j = EmailTemplate.IndexOf("</Fields>") - i

                Dim Searched As String = "</Fields>"
                AfterFieldsElement = EmailTemplate.Substring(EmailTemplate.IndexOf(Searched) + Searched.Length)
                Fields = EmailTemplate.Substring(i, j)
                Dim WOAtTheRate As String = Fields.Replace("@", "")
                Dim WOSpace As String = WOAtTheRate.Replace(" ", "")
                Dim IndexOfFieldElement As Integer = EmailTemplate.IndexOf("<Fields>")
                If IndexOfFieldElement > 0 Then
                    EmailTemplate = EmailTemplate.Remove(IndexOfFieldElement)
                End If
                'EmailTemplate = EmailTemplate.Remove(i, j)
                AllFields = New List(Of String)

                dtEmail.Columns.Clear()
                For Each word As String In WOSpace.Split(",")
                    Dim TrimSpace As String = word.Trim()
                    If Me.grd.RootTable.Columns.Contains(TrimSpace) Then
                        dtEmail.Columns.Add(TrimSpace)
                        AllFields.Add(TrimSpace)
                    End If
                Next
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub CreateOutLookMail(ByVal Email As String)
        Try
            Dim senderemail As String
            Dim oApp As Outlook.Application = New Outlook.Application
            Dim mailItem As Outlook.MailItem = oApp.CreateItem(Outlook.OlItemType.olMailItem)
            Dim OutAccount As Outlook.Account
            Dim str As String
            str = "select Email from tblUser where User_ID = " & LoginUserId
            Dim dt As DataTable = GetDataTable(str)
            If dt.Rows.Count > 0 Then
                senderemail = dt.Rows(0).Item("Email").ToString
                OutAccount = oApp.Session.Accounts(senderemail)
                mailItem.SendUsingAccount = OutAccount
            End If
            'OutAccount = oApp.Session.Accounts(senderemail)
            'mailItem.SendUsingAccount = OutAccount
            mailItem.Subject = "Creating New Receipt: " + VoucherNo
            mailItem.To = Email
            Email = String.Empty
            mailItem.Importance = Outlook.OlImportance.olImportanceNormal
            mailItem.Display(mailItem)
            mailItem.HTMLBody = html.ToString + mailItem.HTMLBody
            EmailBody = html.ToString
            mailItem.Send()
            mailItem = Nothing
            oApp = Nothing
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Private Sub OpenToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEdit.Click
        Try
            If grdVouchers.CurrentRow Is Nothing Then
                ShowErrorMessage("Please select a row to delete")
            Else
                blnEditMode = True
                lngSelectedVoucherId = CLng(grdVouchers.CurrentRow.Cells(0).Value)
                EditRecord()
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DeleteToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDelete.Click

        Try
            If Not Me.grdVouchers.RowCount > 0 Then
                msg_Error(str_ErrorNoRecordFound)
                Exit Sub
            End If
            If IsDateLock(Me.dtVoucherDate.Value) = True Then
                ShowErrorMessage(str_ErrorPreviouseDateRecordDeleteAllow) : Exit Sub
            End If
            ' If IsValidToDelete("MasterTable", "Id", Me.grdVouchers.CurrentRow.Cells("").Value.ToString) = True Then

            If Not msg_Confirm(str_ConfirmDelete) = True Then
                Exit Sub
            Else
                If Me.grdVouchers.RowCount = 0 Then Exit Sub
                ''Start TFS2988
                If ValidateApprovalProcessMapped(Me.txtVoucherNo.Text.Trim, Me.Name) Then
                    If ValidateApprovalProcessInProgress(Me.txtVoucherNo.Text.Trim, Me.Name) Then
                        msg_Error("Document is in Approval Process") : Exit Sub
                    End If
                End If
                ''End TFS2988
                If CheckInvAdjustmentDependedVoucher(Val(Me.grdVouchers.GetRow.Cells("Voucher_Id").Value.ToString)) = True Then
                    ShowErrorMessage("Record can't be deleted, voucher adjusted against invoice.")
                    Exit Sub
                End If


                DeleteRecord("Both")
                Me.ClearFields()
                'PopulateGrid()'R933 Commented History Data
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try

    End Sub
    Private Sub grdVouchers_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Me.grdVouchers.Row < 0 Then
                Exit Sub
            Else
                EditRecord()
                Me.UltraTabControl1.SelectedTab = Me.UltraTabControl1.Tabs(0).TabPage.Tab
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub NewToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnNew.Click

        Try
            If Me.grd.RowCount > 0 Then
                If Not msg_Confirm(str_ConfirmGridClear) = True Then Exit Sub
            End If
            SelectedMode = False
            ClearFields()
            cmbVoucherType.Focus()
        Catch ex As Exception
            msg_Error(ex.Message)
        End Try


    End Sub


    'Private Sub PrintToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    ShowReport("rptVoucher", "{VwGlVoucherSingle.VoucherId}=" & Me.grdVouchers.CurrentRow.Cells(0).Value & "")
    'End Sub
    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            If Me.ValidateInput Then
                'Me.grd.Rows.Add(Me.txtVoucherNo.Text, Me.dtVoucherDate.Value, Me.cmbAccounts.ActiveRow.Cells(1).Value, Me.cmbAccounts.ActiveRow.Cells(0).Value, Me.txtAmount.Text.Trim, Me.cmbVoucherType.Text.ToString, Me.cmbVoucherType.SelectedValue, Me.cmbCashAccount.Text.ToString, Me.cmbCashAccount.SelectedValue, Me.txtChequeNo.Text.Trim, Me.dtChequeDate.Value, Me.txtReference.Text.Trim, Me.txtMemo.Text.Trim)
                ''14-Mar-2014 TASK:2491  Imran Ali  Numeric Validation 
                If CheckNumericValue(Me.txtAmount.Text, Me.txtAmount) = False Then
                    Throw New Exception("Amount is not valid.")
                End If
                If CheckNumericValue(Me.txtDiscount.Text, Me.txtDiscount) = False Then
                    Throw New Exception("Amount is not valid.")
                End If
                If CheckNumericValue(Me.txtTax.Text, Me.txtTax) = False Then
                    Throw New Exception("Amount is not valid.")
                End If
                If txtAmount.Text <= 0 Then
                    ShowErrorMessage("Enter Amount")
                    cmbCashAccount.Focus()
                    Exit Sub
                End If
                'End Task:2491
                Dim dtGrd As DataTable = CType(Me.grd.DataSource, DataTable)
                dtGrd.AcceptChanges()
                Dim drGrd As DataRow
                drGrd = dtGrd.NewRow
                drGrd.Item(0) = 0

                drGrd.Item(grdEnm.coa_detail_id) = Me.cmbAccounts.ActiveRow.Cells(0).Value
                drGrd.Item(grdEnm.detail_title) = Me.cmbAccounts.ActiveRow.Cells(1).Text
                drGrd.Item(grdEnm.detail_code) = Me.cmbAccounts.ActiveRow.Cells(2).Text
                ''TASK-407
                drGrd.Item(grdEnm.CurrencyId) = Me.cmbCurrency.SelectedValue
                drGrd.Item(grdEnm.CurrencyAmount) = Val(Me.txtAmount.Text)
                'drGrd.Item(grdEnm.CurrencyAmount) = 
                drGrd.Item(grdEnm.CurrencyRate) = Val(txtCurrencyRate.Text)
                Dim ConfigCurrencyVal As String = getConfigValueByType("Currency").ToString
                If ConfigCurrencyVal.Length > 0 AndAlso Not ConfigCurrencyVal.ToString.ToUpper = "ERROR" Then
                    drGrd.Item(grdEnm.BaseCurrencyId) = Val(ConfigCurrencyVal)
                    drGrd.Item(grdEnm.BaseCurrencyRate) = Val(GetCurrencyRate(Val(ConfigCurrencyVal)))
                End If
                ''End TASK-407
                'drGrd.Item(grdEnm.Amount) = Val(Me.txtAmount.Text) * Val(txtCurrencyRate.Text)
                'drGrd.Item(grdEnm.CurrencyDiscount) = Val(Me.txtDiscount.Text)
                ''rafay tax % column
                'drGrd.Item(grdEnm.Tax) = Val(Me.txtTax.Text)
                ''rafay tax Amount field
                'drGrd.Item(grdEnm.TaxAmount) = Val(Me.txtTaxAmount.Text)
                ''added by zainab task1608
                ''rafay s.tax% field
                'drGrd.Item(grdEnm.SalesTaxPer) = Val(Me.txtSalesTax.Text)
                ''rafay s.taxamount field
                'drGrd.Item(grdEnm.Sales_Tax) = Val(Me.txtSalesTaxAmount.Text)
                ''rafay WH%
                'drGrd.Item(grdEnm.WHTaxPer) = Val(Me.txtWithHolding.Text)
                ''rafay WH Tax Amount
                'drGrd.Item(grdEnm.WHTaxAmount) = Val(Me.txtWithHoldingTaxAmount.Text)
                ''rafay Net Amount
                'drGrd.Item(grdEnm.NetAmount) = Val(Me.txtNetAmount.Text)
                ''added by zainab
                drGrd.Item(grdEnm.Amount) = Val(Me.txtAmount.Text) * Val(txtCurrencyRate.Text)
                drGrd.Item(grdEnm.CurrencyDiscount) = Val(Me.txtDiscount.Text)
                drGrd.Item(grdEnm.Tax) = Val(Me.txtTax.Text)
                drGrd.Item(grdEnm.TaxAmount) = Format(Math.Round(Val(Me.txtTaxAmount.Text), TotalAmountRounding), "0.00")
                drGrd.Item(grdEnm.SalesTaxPer) = Val(Me.txtSalesTax.Text)
                drGrd.Item(grdEnm.Sales_Tax) = Format(Math.Round(Val(Me.txtSalesTaxAmount.Text), TotalAmountRounding), "0.00")
                drGrd.Item(grdEnm.WHTaxPer) = Val(Me.txtWithHolding.Text)
                drGrd.Item(grdEnm.WHTaxAmount) = Format((Me.txtWithHoldingTaxAmount.Text), "0.00")
                drGrd.Item(grdEnm.NetAmount) = Format(Val(Me.txtNetAmount.Text), "0.00")
                drGrd.Item(grdEnm.AccountId) = Val(CType(Me.cmbSalesTax.SelectedItem, DataRowView).Row.Item("AccountId").ToString)
                drGrd.Item(grdEnm.IncomeAccountId) = Val(CType(Me.cmbIncomeTaxAccount.SelectedItem, DataRowView).Row.Item("AccountId").ToString)

                'If flgMemoRemarks = False Then
                drGrd.Item(grdEnm.Reference) = Me.txtReference.Text.Trim.ToString.Replace("'", "''")
                'Else
                '   drGrd.Item(grdEnm.Reference) = Me.txtMemo.Text.Trim.ToString.Replace("'", "''")
                'End If
                ''END TFS1265
                drGrd.Item(grdEnm.Cheque_No) = IIf(Me.pnlBank.Visible = True, Me.txtChequeNo.Text, DBNull.Value)
                drGrd.Item(grdEnm.Cheque_Date) = IIf(Me.pnlBank.Visible = True, Me.dtChequeDate.Value, DBNull.Value)
                drGrd.Item(grdEnm.BankDescription) = IIf(Me.pnlBank.Visible = True, Me.cmbBank.Text, DBNull.Value)
                drGrd.Item(grdEnm.Type) = Me.cmbAccounts.ActiveRow.Cells("Type").Value.ToString
                drGrd.Item(grdEnm.CostCenterId) = Me.cmbCostCenter.SelectedValue
                drGrd.Item(grdEnm.Employee) = Me.cmbSaleman.SelectedValue
                ''TASK : TFS1265
                If flgSalesInvoice = True Then
                    drGrd.Item(grdEnm.InvoiceId) = cmbInvoice.Value
                    drGrd.Item("InvoiceNo") = cmbInvoice.Text
                Else
                    drGrd.Item(grdEnm.InvoiceId) = 0
                    drGrd.Item("InvoiceNo") = ""
                End If
                'Task:2577 Add Phone Value In Phone Field
                'Before against task:2631
                'drGrd.Item(11) = Me.cmbAccounts.ActiveRow.Cells("Phone").Value.ToString
                'End Task:2577
                'Task:2631 Changed Column Index
                drGrd.Item(grdEnm.Phone) = Me.cmbAccounts.ActiveRow.Cells("Mobile").Value.ToString
                'End Task:2631
                dtGrd.Rows.InsertAt(drGrd, 0)
                SelectedMode = True
                Me.ClearFields("Detail")
                If Me.cmbCashAccount.SelectedIndex > 0 Then
                    Me.cmbVoucherType.Enabled = False
                    Me.cmbCashAccount.Enabled = False
                End If
                Me.cmbCurrency.Enabled = False

                'Me.GroupBox1.Enabled = True
                'Me.GetTotal()
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="VoucherID"></param>
    ''' <remarks></remarks>
    Private Sub DisplayDetail(ByVal VoucherID As Integer)
        Try
            Dim str As String

            str = " SELECT tblVoucherDetail.voucher_id, tblVoucherDetail.coa_detail_id, vwCOADetail.detail_title, vwCOADetail.detail_code, ISNULL(tblVoucherDetail.CurrencyId, 0) AS CurrencyId, ISNULL(tblVoucherDetail.CurrencyAmount, tblVoucherDetail.credit_amount) AS CurrencyAmount, ISNULL(tblVoucherDetail.CurrencyRate, 0) AS CurrencyRate, ISNULL(tblVoucherDetail.BaseCurrencyId, 0) AS BaseCurrencyId,  ISNULL(tblVoucherDetail.BaseCurrencyRate, 0) AS BaseCurrencyRate, tblVoucherDetail.credit_amount AS Amount, tblVoucherDetail.Adjustment AS CurrencyDiscount, 0 AS Discount,  ISNULL(tblVoucherDetail.IncomeAccountId, 0) AS IncomeAccountId, ISNULL(tblVoucherDetail.Tax_Percent, 0) AS Tax, ISNULL(tblVoucherDetail.Tax_Amount, 0) AS Tax_Currency_Amount,  ISNULL(tblVoucherDetail.Tax_Amount, 0) AS Tax_Amount, ISNULL(tblVoucherDetail.AccountId, 0) AS AccountId, ISNULL(tblVoucherDetail.SalesTaxPer, 0) AS SalesTaxPer,  ISNULL(tblVoucherDetail.SalesTaxAmount, 0) AS SalesTaxAmount, ISNULL(tblVoucherDetail.SalesTaxAmount, 0) AS Sales_Tax, ISNULL(tblVoucherDetail.WHTaxPer, 0) AS WHTaxPer,  ISNULL(tblVoucherDetail.WHTaxAmount, 0) AS WHTaxAmount, ISNULL(tblVoucherDetail.WHTaxAmount, 0) AS WH_Tax, ISNULL(tblVoucherDetail.NetAmount, 0) AS NetAmount, ISNULL(tblVoucherDetail.NetAmount, 0) AS Net_Amount, tblVoucherDetail.comments AS Reference, tblVoucherDetail.Cheque_No, ISNULL(tblVoucherDetail.Cheque_Date, GETDATE()) AS Cheque_Date, tblVoucherDetail.BankDescription, Cust.Mobile,  vwCOADetail.account_type AS Type, ISNULL(tblVoucherDetail.CostCenterID, 0) AS CostCenterId, ISNULL(tblVoucherDetail.EmpID, 0) AS Employee, ISNULL(tblVoucherDetail.InvoiceId, 0) AS InvoiceId,  ISNULL(InvAdj.VoucherDetailId, 0) AS VoucherDetailId, SalesMasterTable.SalesNo as InvoiceNo FROM tblVoucherDetail INNER JOIN vwCOADetail ON tblVoucherDetail.coa_detail_id = vwCOADetail.coa_detail_id LEFT OUTER JOIN SalesMasterTable ON tblVoucherDetail.InvoiceId = SalesMasterTable.SalesId LEFT OUTER JOIN tblCustomer AS Cust ON Cust.AccountId = vwCOADetail.coa_detail_id LEFT OUTER JOIN (SELECT DISTINCT VoucherDetailId FROM InvoiceAdjustmentTable) AS InvAdj ON InvAdj.VoucherDetailId = tblVoucherDetail.voucher_detail_id  " _
              & " Where voucher_id =" & VoucherID & " AND (tblVoucherDetail.credit_amount > 0) Order By tblVoucherDetail.Voucher_Detail_Id ASC "
            Dim dtDisplayDetail As DataTable = GetDataTable(str)
            dtDisplayDetail.Columns("Amount").Expression = "(CurrencyAmount*CurrencyRate)"

            ''dtDisplayDetail.Columns("Tax_Amount").Expression = "(((Amount-Discount)*Tax)/100)"
            dtDisplayDetail.Columns("Tax").Expression = "((Tax_Amount/(CurrencyAmount-Discount))*100)"
            dtDisplayDetail.Columns("Tax_Currency_Amount").Expression = "(((CurrencyAmount-CurrencyDiscount)*Tax)/100)"
            dtDisplayDetail.Columns("Discount").Expression = "(CurrencyDiscount) * (CurrencyRate)"
            ''Below line is commented on 13-12-2017 AGAINST TASK TFS1926
            'dtDisplayDetail.Columns("SalesTaxAmount").Expression = "((Amount-Discount)* (SalesTaxPer/100))"

            ''TASK TFS1926. Below lines are modified as per Ahmad's advice that gross amount should be divided by sales tax % + 100 then be divided by sales tax %. done by Ameen on 13-12-2017
            ''dtDisplayDetail.Columns("SalesTaxAmount").Expression = "((Amount-Discount)/(SalesTaxPer+100)*(SalesTaxPer))"
            ''END TASK TFS1926
            ''waqar modified and commented above line for Muhammad Ali WHT points calculation Issue
            'dtDisplayDetail.Columns("SalesTaxAmount").Expression = "((Amount-Discount)/(SalesTaxPer))"
            'dtDisplayDetail.Columns("Sales_Tax").Expression = "(((CurrencyAmount-CurrencyDiscount)*SalesTaxPer)/100)"
            'dtDisplayDetail.Columns("Sales_Tax").Expression = "((Amount-Discount)/(SalesTaxAmount))"
            dtDisplayDetail.Columns("SalesTaxPer").Expression = "((Sales_Tax/(CurrencyAmount-Discount))*100)"
            ''dtDisplayDetail.Columns("Sales_Tax").Expression = "((CurrencyAmount-CurrencyDiscount)*(SalesTaxPer)/100)"
            dtDisplayDetail.Columns("SalesTaxAmount").Expression = "((Amount-Discount)*(SalesTaxPer)/100)"
            dtDisplayDetail.Columns("WHTaxAmount").Expression = "(SalesTaxAmount* (WHTaxPer/100))"
            dtDisplayDetail.Columns("WH_Tax").Expression = "(Sales_Tax*WHTaxPer/100)"
            dtDisplayDetail.Columns("NetAmount").Expression = "((Amount-Discount)-Tax_Amount-WHTaxAmount)"
            dtDisplayDetail.Columns("Net_Amount").Expression = "((CurrencyAmount-CurrencyDiscount)-Tax_Currency_Amount-WH_Tax)"

            dtDisplayDetail.AcceptChanges()
            Me.grd.DataSource = Nothing
            Me.grd.DataSource = dtDisplayDetail
            If dtDisplayDetail.Rows.Count > 0 Then
                If IsDBNull(dtDisplayDetail.Rows.Item(0).Item("CurrencyId")) Or Val(dtDisplayDetail.Rows.Item(0).Item("CurrencyId").ToString) = 0 Then
                    'Me.cmbCurrency.SelectedValue = Nothing
                    'Me.cmbCurrency.Enabled = False
                Else
                    Me.cmbCurrency.SelectedValue = Val(dtDisplayDetail.Rows.Item(0).Item("CurrencyId").ToString)
                    Me.txtCurrencyRate.Text = Val(dtDisplayDetail.Rows.Item(0).Item("CurrencyRate").ToString)
                    Me.cmbCurrency.Enabled = False
                End If
            End If
            FillCombo("GrdCostCenter")
            FillCombo("GrdEmployee")
            FillCombo("GrdIncomeTaxAccount")
            FillCombo("GrdSalesTaxAccount")
            ApplyGridSettings()
            ''TASK-407

            Me.grd.RootTable.Columns("CurrencyAmount").Caption = "" & Me.cmbCurrency.Text & " Amount"
            Me.grd.RootTable.Columns("Amount").Caption = "" & GetBasicCurrencyName(Val(getConfigValueByType("Currency").ToString)) & " Amount"
            If Me.cmbCurrency.Text.ToUpper.ToString = GetBasicCurrencyName(Val(getConfigValueByType("Currency").ToString)).ToUpper.ToString Then
                Me.grd.RootTable.Columns("Amount").Visible = False
            Else
                Me.grd.RootTable.Columns("Amount").Visible = True
            End If

            '' ''END TASK-407
            'Me.cmbCashAccount.Enabled = False
            'Me.cmbVoucherType.Enabled = False
            'Me.grd_Click(grd, Nothing)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub GetSecurityRights()
        Try
            If LoginGroup = "Administrator" Then
                Me.BtnSave.Enabled = True
                Me.BtnDelete.Enabled = True
                Me.BtnPrint.Enabled = True
                Me.btnSearchDelete.Enabled = True ''R934  Added Delete Button
                Me.btnHistoryPrint.Enabled = True ''R934 Added Print Button
                If Me.BtnSave.Text = "&Save" Then Me.chkPost.Checked = True
                Me.chkPost.Visible = True
                If Me.BtnSave.Text = "&Save" Then Me.chkChecked.Checked = True 'Task:2826 
                Me.chkChecked.Visible = True
                Me.chkAll.Visible = True  ''TFS4741
                Me.btnGetAllRecord.Enabled = True
                IsGetAllAllowed = True
                IsAdminGroup = True
                Exit Sub
            End If
            If getConfigValueByType("NewSecurityRights").ToString = "False" Or getConfigValueByType("NewSecurityRights").ToString = "Error" Then
                If RegisterStatus = EnumRegisterStatus.Expired Then
                    Me.BtnSave.Enabled = False
                    Me.BtnDelete.Enabled = False
                    Me.BtnPrint.Enabled = False
                    Me.btnSearchDelete.Enabled = False ''R934  Added Delete Button
                    Me.btnHistoryPrint.Enabled = False ''R934 Added Print Button
                    Me.chkAll.Visible = False  ''TFS4741
                    'Me.btnHistoryLoadAll.Enabled = False
                    'Me.btnGetAllRecord.Enabled = False
                    IsGetAllAllowed = False
                    IsAdminGroup = False
                    'Me.PrintListToolStripMenuItem.Enabled = False
                    'PrintToolStripMenuItem.Enabled = False
                    Exit Sub
                End If
                Dim dt As DataTable = GetFormRights(EnumForms.frmCustomerCollection)
                If Not dt Is Nothing Then
                    If Not dt.Rows.Count = 0 Then
                        If Me.BtnSave.Text = "Save" Or Me.BtnSave.Text = "&Save" Then
                            Me.BtnSave.Enabled = dt.Rows(0).Item("Save_Rights").ToString()
                        Else
                            Me.BtnSave.Enabled = dt.Rows(0).Item("Update_Rights").ToString
                        End If
                        Me.BtnDelete.Enabled = dt.Rows(0).Item("Delete_Rights").ToString
                        Me.BtnPrint.Enabled = dt.Rows(0).Item("Print_Rights").ToString
                        Me.btnSearchDelete.Enabled = dt.Rows(0).Item("Delete_Rights").ToString ''R934  Added Delete Button
                        Me.btnHistoryPrint.Enabled = dt.Rows(0).Item("Print_Rights").ToString ''R934 Added Print Button
                        Me.Visible = dt.Rows(0).Item("View_Rights").ToString
                    End If
                End If
                UserPostingRights = GetUserPostingRights(LoginUserId)
                If UserPostingRights = True Then
                    Me.chkPost.Visible = True
                Else
                    Me.chkPost.Visible = False
                    Me.chkPost.Checked = False
                End If
                GetSecurityByPostingUser(UserPostingRights, BtnSave, BtnDelete)
            Else
                'Me.Visible = False
                Me.BtnSave.Enabled = False
                Me.BtnDelete.Enabled = False
                Me.BtnPrint.Enabled = False
                Me.chkPost.Visible = False
                If Me.BtnSave.Text = "&Save" Then Me.chkPost.Checked = False
                Me.btnSearchDelete.Enabled = False ''R934  Added Delete Button
                Me.btnHistoryPrint.Enabled = False ''R934 Added Print Button
                'Me.btnHistoryLoadAll.Enabled = False
                CtrlGrdBar10.mGridPrint.Enabled = False
                CtrlGrdBar10.mGridExport.Enabled = False
                CtrlGrdBar4.mGridPrint.Enabled = False
                CtrlGrdBar4.mGridExport.Enabled = False
                CtrlGrdBar10.mGridChooseFielder.Enabled = False
                CtrlGrdBar4.mGridChooseFielder.Enabled = False
                'Me.btnGetAllRecord.Enabled = False
                IsGetAllAllowed = False
                IsAdminGroup = False
                Me.chkAll.Visible = False ''TFS4741
                If Me.BtnSave.Text = "&Save" Then Me.chkChecked.Checked = False 'Task:2826 Default Apply Security Checked
                Me.chkChecked.Visible = False
                'For i As Integer = 0 To Rights.Count - 1
                For Each RightsDt As GroupRights In Rights
                    If RightsDt.FormControlName = "View" Then
                        'Me.Visible = True
                    ElseIf RightsDt.FormControlName = "Save" Then
                        If Me.BtnSave.Text = "&Save" Then BtnSave.Enabled = True
                    ElseIf RightsDt.FormControlName = "Update" Then
                        If Me.BtnSave.Text = "&Update" Then BtnSave.Enabled = True
                    ElseIf RightsDt.FormControlName = "Delete" Then
                        Me.BtnDelete.Enabled = True
                        Me.btnSearchDelete.Enabled = True ''R934  Added Delete Button
                    ElseIf RightsDt.FormControlName = "Print" Then
                        Me.BtnPrint.Enabled = True
                        Me.btnHistoryPrint.Enabled = True ''R934 Added Print Button
                        CtrlGrdBar4.mGridPrint.Enabled = True
                    ElseIf RightsDt.FormControlName = "Export" Then
                        CtrlGrdBar4.mGridExport.Enabled = True
                    ElseIf RightsDt.FormControlName = "Post" Then
                        Me.chkPost.Visible = True
                        If Me.BtnSave.Text = "&Save" Then Me.chkPost.Checked = True
                        'Task:2826 Apply Security Checked Voucher ...
                        CtrlGrdBar10.mGridPrint.Enabled = True
                    ElseIf RightsDt.FormControlName = "Export" Then
                        CtrlGrdBar10.mGridExport.Enabled = True
                    ElseIf RightsDt.FormControlName = "Field Chooser" Then
                        CtrlGrdBar10.mGridChooseFielder.Enabled = True
                        CtrlGrdBar4.mGridChooseFielder.Enabled = True
                    ElseIf RightsDt.FormControlName = "Post" Then
                        Me.chkPost.Visible = True
                        If Me.BtnSave.Text = "&Save" Then Me.chkPost.Checked = True
                    ElseIf RightsDt.FormControlName = "Checked" Then
                        Me.chkChecked.Visible = True
                        If Me.BtnSave.Text = "&Save" Then Me.chkChecked.Checked = True
                        'End Task:2826
                    ElseIf RightsDt.FormControlName = "GetAll" Then
                        'Me.btnGetAllRecord.Enabled = True
                        IsGetAllAllowed = True
                        ''Start TFS4741
                    ElseIf RightsDt.FormControlName = "Show All Accounts" Then
                        Me.chkAll.Visible = True
                        ''End TFS4741
                    End If
                Next

                If getConfigValueByType("UpdatePostedVoucher").ToString = "False" Then
                    If Me.BtnSave.Text <> "&Save" Then
                        If grdVouchers.RowCount > 0 Then
                            If Me.grdVouchers.GetRow.Cells("Post").Value = True Then
                                Me.BtnSave.Enabled = False
                                'ShowErrorMessage("Your not update this record.")
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub cmbAccounts_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAccounts.Enter, cmbInvoice.Enter
        Try
            cmbAccounts.PerformAction(Infragistics.Win.UltraWinGrid.UltraComboAction.Dropdown)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Function BackendValidation() As Boolean
        Try

            Dim str As String = "Select voucher_id from tblvoucher where voucher_id <> " & Val(Me.grdVouchers.CurrentRow.Cells(0).Value.ToString) & "  and cheque_no = N'" & Me.txtChequeNo.Text & "' and voucher_type_id = 5 "

            Dim cmd As New OleDbCommand
            cmd.Connection = Con
            cmd.CommandText = str

            Dim da As New OleDbDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                Return False
            Else
                Return True
            End If

        Catch ex As OleDbException
            Throw ex
        Catch ex As Exception
            Throw ex
        Finally

        End Try
    End Function
    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        Try
            Dim CustId As Integer = 0
            CustId = Me.cmbAccounts.Value

            ''Task 3461: Aashir: Edited Becuase Shortcut link account add option was not working 
            'FrmAddCustomers.FormType = "Customer"
            'FrmAddCustomers.ShowDialog()
            frmMain.LoadControl("AddCustomer")

            Dim str As String = String.Empty
            str = "select coa_detail_id,detail_title, sub_sub_title AS [Sub Sub A/c] from vwCoaDetail where account_type = 'Customer' And Active=1 and detail_title is not null "
            ''Start TFS3322 : Ayesha Rehman : 15-05-2018
            'If LoginGroup = "Administrator" Then
            If GetMappedUserId() > 0 And getGroupAccountsConfigforAccounts(Me.Name) And LoginGroup <> "Administrator" Then
                str = "select coa_detail_id,detail_title, sub_sub_title AS [Sub Sub A/c] from vwCoaDetail where  Active=1 and detail_title is not null "
                str += " And (coa_detail_id in (Select COAAccountMapping.AccountId FROM COAAccountMapping INNER JOIN COAGroups ON COAAccountMapping.COAGroupId = COAGroups.COAGroupId INNER JOIN COAUserMapping ON COAGroups.COAGroupId = COAUserMapping.COAGroupId WHERE (COAAccountMapping.AccountLevel = 3) and COAUserMapping.[User_Id]= " & LoginGroupId & " ) " _
                       & " or main_sub_sub_id in (SELECT COAAccountMapping.AccountId FROM COAAccountMapping INNER JOIN COAGroups ON COAAccountMapping.COAGroupId = COAGroups.COAGroupId INNER JOIN COAUserMapping ON COAGroups.COAGroupId = COAUserMapping.COAGroupId WHERE (COAAccountMapping.AccountLevel = 2) and COAUserMapping.[User_Id]= " & LoginGroupId & " ) " _
                       & " or main_sub_id in (SELECT COAAccountMapping.AccountId FROM COAAccountMapping INNER JOIN COAGroups ON COAAccountMapping.COAGroupId = COAGroups.COAGroupId INNER JOIN COAUserMapping ON COAGroups.COAGroupId = COAUserMapping.COAGroupId WHERE (COAAccountMapping.AccountLevel = 1) and COAUserMapping.[User_Id]= " & LoginGroupId & " ) " _
                       & " or coa_main_id in (SELECT   COAAccountMapping.AccountId FROM COAAccountMapping INNER JOIN COAGroups ON COAAccountMapping.COAGroupId = COAGroups.COAGroupId INNER JOIN COAUserMapping ON COAGroups.COAGroupId = COAUserMapping.COAGroupId WHERE (COAAccountMapping.AccountLevel = 0) and COAUserMapping.[User_Id]= " & LoginGroupId & ")) "
                str += " And vwCoaDetail.account_type = 'Customer' "
            End If
            str = " order by 2 "
            ''End TFS3322
            FillUltraDropDown(Me.cmbAccounts, str)
            'FillUltraDropDown(Me.cmbAccounts, "select coa_detail_id,detail_title, sub_sub_title AS [Sub Sub A/c] from vwCoaDetail where account_type = 'Customer' And Active=1 and detail_title is not null order by 2 ")
            Me.cmbAccounts.Value = CustId
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    'Private Sub BtnLoadAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Me.PopulateGrid("All")
    '    DisplayDetail(-1)
    'End Sub
    'Private Sub UltraTabControl1_SelectedTabChanged(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs) Handles UltraTabControl1.SelectedTabChanged
    '    If Me.UltraTabControl1.SelectedTab.Index = 0 Then
    '        Me.BtnLoadAll.Visible = False
    '        Me.btnSearch.Visible = False
    '        ToolStripSeparator2.Visible = False
    '    Else
    '        Me.BtnLoadAll.Visible = True
    '        Me.btnSearch.Visible = True
    '        ToolStripSeparator2.Visible = True
    '    End If
    'End Sub
    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRefresh.Click
        Try
            Dim id As Integer = 0
            id = Me.cmbAccounts.SelectedRow.Cells(0).Value
            FillCombo("Customer")
            FillCombo("SearchCustomer")
            FillCombo("Company")
            FillCombo("IncomeTaxAccount")
            FillCombo("SalesTaxAccount")

            Me.cmbAccounts.Value = id
            If Not getConfigValueByType("SalesDiscountAccount").ToString = "Errro" Then
                DiscountAccountId = Convert.ToInt32(getConfigValueByType("SalesDiscountAccount").ToString)
            End If

            id = Me.cmbCashAccount.SelectedValue
            FillCombo("Bank")
            Me.cmbCashAccount.SelectedValue = id

            id = Me.cmbCostCenter.SelectedValue
            FillCombo("CostCenter")
            Me.cmbCostCenter.SelectedValue = id

            FillCombo("GrdCostCenter")
            FillCombo("GrdEmployee")
            'Altered Against Task#2015060001 Ali Ansri
            'Clear Attached file records
            'arrFile = New List(Of String)
            'Me.btnAttachment.Text = "Attachment (" & arrFile.Count & ")"
            'Altered Against Task#2015060001 Ali Ansri

        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub FillComboByEdit()
        Try
            FillCombo("Customer")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub grd_ColumnButtonClick(ByVal sender As System.Object, ByVal e As Janus.Windows.GridEX.ColumnActionEventArgs) Handles grd.ColumnButtonClick

        Try
            If Not msg_Confirm(str_ConfirmDelete) = True Then Exit Sub
            If e.Column.Key = "Delete" Then
                If Not Val(Me.grd.GetRow.Cells("VoucherDetailId").Value.ToString) > 0 Then
                    Me.grd.GetRow.Delete()
                    grd.UpdateData()
                    If Me.grd.RowCount = 0 Then
                        Me.cmbCurrency.Enabled = True
                    End If
                Else
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try

    End Sub

    Public Sub ApplyGridSettings(Optional ByVal Condition As String = "") Implements IGeneral.ApplyGridSettings
        Try
            For Each col As Janus.Windows.GridEX.GridEXColumn In Me.grd.RootTable.Columns
                If col.Index <> grdEnm.Reference AndAlso col.Index <> grdEnm.CurrencyDiscount AndAlso col.Index <> grdEnm.Cheque_No AndAlso col.Index <> grdEnm.Cheque_Date AndAlso col.Index <> grdEnm.BankDescription AndAlso col.Index <> grdEnm.Tax AndAlso col.Index <> grdEnm.TaxAmount AndAlso col.Index <> grdEnm.SalesTaxPer AndAlso col.Index <> grdEnm.WHTaxPer AndAlso col.Index <> grdEnm.WHTaxAmount AndAlso col.Index <> grdEnm.NetAmount AndAlso col.Index <> grdEnm.CostCenterId AndAlso col.Index <> grdEnm.Employee AndAlso col.Index <> grdEnm.InvoiceId AndAlso col.Index <> grdEnm.CurrencyAmount AndAlso col.Index <> grdEnm.IncomeAccountId AndAlso col.Index <> grdEnm.AccountId Then
                    col.EditType = Janus.Windows.GridEX.EditType.NoEdit
                End If
            Next



            Me.grd.RootTable.Columns(grdEnm.Amount).FormatString = "N"
            Me.grd.RootTable.Columns(grdEnm.Amount).TotalFormatString = "N"
            Me.grd.RootTable.Columns(grdEnm.Amount).FormatString = "N" & DecimalPointInValue
            Me.grd.RootTable.Columns(grdEnm.Amount).FormatString = "N" & TotalAmountRounding
            Me.grd.RootTable.Columns(grdEnm.Amount).TotalFormatString = "N" & TotalAmountRounding ''27-Jul-2014 Task:2762 Imran Ali Total Amount Rounding configuration


            'Task:2762 Set Rounding Format
            'Me.grd.RootTable.Columns("Tax_Amount").FormatString = "N" & 2
            'Me.grd.RootTable.Columns("Tax_Amount").TotalFormatString = "N" & 2
            'Me.grd.RootTable.Columns("Tax_Amount").FormatString = "N" & DecimalPointInValue
            'Me.grd.RootTable.Columns("Tax_Amount").FormatString = "N" & TotalAmountRounding
            'Me.grd.RootTable.Columns("Tax_Amount").TotalFormatString = "N" & TotalAmountRounding ''27-Jul-2014 Task:2762 Imran Ali Total Amount Rounding configuration
            ''rafay
            Me.grd.RootTable.Columns(grdEnm.WHTaxAmount).FormatString = "N" & 2
            Me.grd.RootTable.Columns(grdEnm.WHTaxAmount).TotalFormatString = "N" & 2
            Me.grd.RootTable.Columns(grdEnm.WH_Tax).FormatString = "N" & 2
            Me.grd.RootTable.Columns(grdEnm.WH_Tax).TotalFormatString = "N" & 2

            'rafay
            Me.grd.RootTable.Columns(grdEnm.TaxCurrencyAmount).FormatString = "N"
            Me.grd.RootTable.Columns(grdEnm.TaxCurrencyAmount).TotalFormatString = "N"
            Me.grd.RootTable.Columns(grdEnm.TaxCurrencyAmount).FormatString = "N" & DecimalPointInValue  'Task:2647 Set Rounding Format
            Me.grd.RootTable.Columns(grdEnm.TaxCurrencyAmount).FormatString = "N" & TotalAmountRounding
            Me.grd.RootTable.Columns(grdEnm.TaxCurrencyAmount).TotalFormatString = "N" & TotalAmountRounding



            Me.grd.RootTable.Columns(grdEnm.Net_Amount).FormatString = "N"
            Me.grd.RootTable.Columns(grdEnm.Net_Amount).TotalFormatString = "N"
            Me.grd.RootTable.Columns(grdEnm.NetAmount).FormatString = "N"
            Me.grd.RootTable.Columns(grdEnm.NetAmount).TotalFormatString = "N"



            Me.grd.RootTable.Columns("CurrencyDiscount").FormatString = "N"
            Me.grd.RootTable.Columns("CurrencyDiscount").TotalFormatString = "N"
            Me.grd.RootTable.Columns("CurrencyDiscount").FormatString = "N" & DecimalPointInValue
            Me.grd.RootTable.Columns("CurrencyDiscount").FormatString = "N" & TotalAmountRounding
            Me.grd.RootTable.Columns("CurrencyDiscount").TotalFormatString = "N" & TotalAmountRounding ''27-Jul-2014 Task:2762 Imran Ali Total Amount Rounding configuration

            Me.grd.RootTable.Columns("Discount").FormatString = "N" & DecimalPointInValue
            Me.grd.RootTable.Columns("Discount").FormatString = "N" & TotalAmountRounding
            Me.grd.RootTable.Columns("Discount").TotalFormatString = "N" & TotalAmountRounding ''27-Jul-2014 Task:2762 Imran Ali Total Amount Rounding configuration

            Me.grd.RootTable.Columns("CurrencyRate").FormatString = "N" & 4
            Me.grd.RootTable.Columns("CurrencyRate").FormatString = "N" & 4
            Me.grd.RootTable.Columns("CurrencyRate").TotalFormatString = "N" & 4

            Me.grd.RootTable.Columns("CurrencyAmount").FormatString = "N" & DecimalPointInValue
            Me.grd.RootTable.Columns("CurrencyAmount").FormatString = "N" & TotalAmountRounding
            Me.grd.RootTable.Columns("CurrencyAmount").TotalFormatString = "N" & TotalAmountRounding

            'Me.grd.RootTable.Columns("CurrencyTotalAmount").FormatString = "N" & DecimalPointInValue
            'Me.grd.RootTable.Columns("CurrencyTotalAmount").FormatString = "N" & TotalAmountRounding
            'Me.grd.RootTable.Columns("CurrencyTotalAmount").TotalFormatString = "N" & TotalAmountRounding

            Me.grd.RootTable.Columns("SalesTaxAmount").FormatString = "N" & 2
            Me.grd.RootTable.Columns("SalesTaxAmount").TotalFormatString = "N" & 2
            Me.grd.RootTable.Columns("SalesTaxAmount").FormatString = "N" & DecimalPointInValue
            Me.grd.RootTable.Columns("SalesTaxAmount").FormatString = "N" & TotalAmountRounding
            Me.grd.RootTable.Columns("Sales_Tax").FormatString = "N" & DecimalPointInValue
            Me.grd.RootTable.Columns("Sales_Tax").FormatString = "N" & TotalAmountRounding
            'End Task:2762
            'Me.grd.RootTable.Columns("SalesTaxPer").FormatStyle.ForeColor = Color.Blue
            Me.grd.RootTable.Columns(grdEnm.WH_Tax).Caption = "WHS Tax"
            Me.grd.RootTable.Columns(grdEnm.WHTaxAmount).Caption = "WHS Tax Amount"
            Me.grd.RootTable.Columns(grdEnm.WHTaxPer).Caption = "WHS Tax"
        Catch ex As Exception
            Throw ex
        End Try
    End Sub



    Public Sub ApplySecurity(ByVal Mode As SBUtility.Utility.EnumDataMode, Optional ByVal Condition As String = "") Implements IGeneral.ApplySecurity

    End Sub

    Public Function Delete(Optional ByVal Condition As String = "") As Boolean Implements IGeneral.Delete

    End Function

    Public Sub FillCombos(Optional ByVal Condition As String = "") Implements IGeneral.FillCombos

    End Sub

    Public Sub FillModel(Optional ByVal Condition As String = "") Implements IGeneral.FillModel

    End Sub

    Public Sub GetAllRecords(Optional ByVal Condition As String = "") Implements IGeneral.GetAllRecords

    End Sub

    Public Function IsValidate(Optional ByVal Mode As SBUtility.Utility.EnumDataMode = SBUtility.Utility.EnumDataMode.Disabled, Optional ByVal Condition As String = "") As Boolean Implements IGeneral.IsValidate

    End Function

    Public Sub ReSetControls(Optional ByVal Condition As String = "") Implements IGeneral.ReSetControls

    End Sub

    Public Function Save(Optional ByVal Condition As String = "") As Boolean Implements IGeneral.Save

    End Function

    Public Sub SetButtonImages() Implements IGeneral.SetButtonImages

    End Sub

    Public Sub SetConfigurationBaseSetting() Implements IGeneral.SetConfigurationBaseSetting

    End Sub

    Public Sub SetNavigationButtons(ByVal Mode As SBUtility.Utility.EnumDataMode, Optional ByVal Condition As String = "") Implements IGeneral.SetNavigationButtons

    End Sub

    Public Function Update1(Optional ByVal Condition As String = "") As Boolean Implements IGeneral.Update

    End Function
    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function EmailSave()
        Try
            EmailSave = Nothing
            Dim toEmail As String = String.Empty
            Dim flg As Boolean = False
            If IsEmailAlert = True Then
                Dim dtForm As DataTable = GetDataTable("Select ISNULL(EmailAlert,0) as EmailAlert  From tblForm WHERE Form_Name=N'" & Me.Name & "' AND EmailAlert=1")
                If dtForm.Rows.Count > 0 Then
                    flg = True
                Else
                    flg = False
                End If
                If flg = True Then
                    If AdminEmail <> "" Then
                        Email = New SBModel.Email
                        Email.ToEmail = AdminEmail
                        Email.CCEmail = String.Empty
                        Email.BccEmail = String.Empty
                        Email.Attachment = SourceFile
                        Email.Subject = "" & IIf(setVoucherType = "Cash", "Cash Receipt", "Bank Receipt") & " " & setVoucherNo & " "
                        Email.Body = "" & IIf(setVoucherType = "Cash", "Cash Receipt", "Bank Receipt") & " " _
                        & " " & IIf(setEditMode = False, "of amount " & Total_Amount & " is made", "of amount " & Total_Previouse_Amount & " is updated to " & Total_Amount & "") & " by user " & LoginUserName & " " & vbCrLf & " " & vbCrLf & " " & vbCrLf & " " & vbCrLf & " " & vbCrLf & " " & vbCrLf & " " & vbCrLf & "Auto Generated By SIRIUS ERP System"
                        Email.Status = "Pending"
                        Call New MailSentDAL().Add(Email)
                    End If
                End If
            End If
            SourceFile = String.Empty
            setVoucherNo = String.Empty
            Return EmailSave
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Try
    '        If Me.SplitContainer1.Panel1Collapsed = True Then
    '            Me.SplitContainer1.Panel1Collapsed = False
    '        Else
    '            Me.SplitContainer1.Panel1Collapsed = True
    '        End If
    '    Catch ex As Exception
    '        ShowErrorMessage(ex.Message)
    '    End Try
    'End Sub
    Private Sub Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Search.Click
        Try
            Me.PopulateGrid("All")
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    Private Sub chkAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            FillCombo("Customer")
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    Private Sub btnHistorySearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try


            If Not Me.cmbSearchAccount.IsItemInList Then
                FillCombo("SearchCustomer")
                Me.cmbSearchAccount.Rows(0).Activate()
            Else
                Me.cmbSearchAccount.Rows(0).Activate()
            End If
            If Me.SplitContainer1.Panel1Collapsed = True Then
                Me.SplitContainer1.Panel1Collapsed = False
            Else
                Me.SplitContainer1.Panel1Collapsed = True
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    Private Sub btnHistoryLoadAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Me.PopulateGrid("All")
            DisplayDetail(-1)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    Private Sub BtnPrint_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPrint.ButtonClick
        Try
            If Me.grdVouchers.RowCount = 0 Then Exit Sub
            'PrintLog = New SBModel.PrintLogBE
            'PrintLog.DocumentNo = grdVouchers.GetRow.Cells("voucher_no").Value.ToString
            'PrintLog.UserName = LoginUserName
            'PrintLog.PrintDateTime = Date.Now
            'Call SBDal.PrintLogDAL.PrintLog(PrintLog)
            ''ShowReport("rptVoucher", "{VwGlVoucherSingle.VoucherId}=" & Me.grdVouchers.CurrentRow.Cells(0).Value & "")
            ''Changing Against Request No 798
            'AddRptParam("@VoucherId", Me.grdVouchers.CurrentRow.Cells(0).Value)
            'ShowReport("rptVoucher")
            PrintVoucherBC(Me.grdVouchers.GetRow.Cells("voucher_id").Value, Me.grdVouchers.GetRow.Cells("Voucher_No").Value.ToString())

        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub PrintInvoiceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            AddRptParam("@vid", Me.grd.CurrentRow.Cells(0).Value)
            ShowReport("rptVoucherPrint")
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub btnHistoryPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Cursor = Cursors.WaitCursor
        Try


            If Me.grdVouchers.RowCount = 0 Then Exit Sub
            PrintLog = New SBModel.PrintLogBE
            PrintLog.DocumentNo = grdVouchers.GetRow.Cells("Voucher_No").Value.ToString
            PrintLog.UserName = LoginUserName
            PrintLog.PrintDateTime = Date.Now
            Call SBDal.PrintLogDAL.PrintLog(PrintLog)
            'ShowReport("rptVoucher", "{VwGlVoucherSingle.VoucherId}=" & Me.grdVouchers.CurrentRow.Cells(0).Value & "")
            'Chaning Against Request No 798
            AddRptParam("@vid", Me.grdVouchers.CurrentRow.Cells(0).Value)
            ShowReport("rptCashReceipt")



        Catch ex As Exception
            Throw ex
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub btnHisotryEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            If grdVouchers.CurrentRow Is Nothing Then
                ShowErrorMessage("Please select a row to delete")
            Else
                blnEditMode = True
                lngSelectedVoucherId = CLng(grdVouchers.CurrentRow.Cells(0).Value)
                EditRecord()
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    Public Function Get_All(ByVal Voucher_No As String)
        Try

            Get_All = Nothing
            If Not Voucher_No.Length > 0 Then Exit Try
            If IsLoadedForm = True Then

                '' Task# H08062015  Ahmad Sharif:
                IsDrillDown = True
                If Me.grdVouchers.RowCount <= 50 Then
                    Me.btnHistoryLoadAll_Click(Nothing, Nothing)
                End If

                Dim flag As Boolean = False
                flag = Me.grdVouchers.Find(Me.grdVouchers.RootTable.Columns("Voucher_No"), Janus.Windows.GridEX.ConditionOperator.Equal, Voucher_No, 0, 1)

                blnEditMode = True
                ' If flag = True Then
                Me.grdVouchers_DoubleClick(Nothing, Nothing)
                frmModProperty.Tags = ""
                'Else
                '    Exit Function
                'End If
                '' End Task# H08062015 
            End If


            Return Get_All
        Catch ex As Exception
            Throw ex
        Finally
            frmModProperty.Tag = String.Empty
        End Try
    End Function
    Private Sub UltraTabControl1_SelectedTabChanging(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventArgs) Handles UltraTabControl1.SelectedTabChanging
        Try
            If e.Tab.Index = 1 Then
                'PopulateGrid()
                ''  Hide Buttons Edit,Delete and Print on Load Form
                Me.BtnDelete.Visible = True
                Me.BtnPrint.Visible = True
            Else
                Me.BtnEdit.Visible = False
                If blnEditMode = False Then Me.BtnDelete.Visible = False
                If blnEditMode = False Then Me.BtnPrint.Visible = False
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="VoucherId"></param>
    ''' <remarks></remarks>
    Public Sub ExportFile(ByVal VoucherId As Integer)
        Try
            If IsEmailAlert = True Then
                If IsAttachmentFile = True Then
                    crpt = New ReportDocument
                    If IO.File.Exists(str_ApplicationStartUpPath & "\Reports\rptVoucher.rpt") = False Then Exit Sub
                    crpt.Load(str_ApplicationStartUpPath & "\Reports\rptVoucher.rpt", DBServerName)
                    If DBUserName <> "" Then
                        crpt.DataSourceConnections.Item(0).SetConnection(DBServerName, DBName, DBUserName, DBPassword)
                        crpt.DataSourceConnections.Item(0).SetLogon(DBName, DBPassword)
                    Else
                        crpt.DataSourceConnections.Item(0).SetConnection(DBServerName, DBName, True)
                    End If

                    Dim ConnectionInfo As New ConnectionInfo
                    With ConnectionInfo
                        .ServerName = DBServerName
                        .DatabaseName = DBName
                        If DBUserName <> "" Then
                            .UserID = DBUserName
                            .Password = DBPassword
                            .IntegratedSecurity = False
                        Else
                            .IntegratedSecurity = True
                        End If
                    End With
                    Dim tbLogOnInfo As New TableLogOnInfo
                    For Each dt As Table In crpt.Database.Tables
                        tbLogOnInfo = dt.LogOnInfo
                        tbLogOnInfo.ConnectionInfo = ConnectionInfo
                        dt.ApplyLogOnInfo(tbLogOnInfo)
                    Next

                    crpt.SetParameterValue("@VoucherId", Me.grdVouchers.CurrentRow.Cells(0).Value) '"{VwGlVoucherSingle.VoucherId}=" & VoucherId & ""
                    Dim crExportOps As New ExportOptions
                    Dim crDiskOps As New DiskFileDestinationOptions
                    Dim crExportType As New PdfRtfWordFormatOptions


                    If Not IO.Directory.Exists(str_ApplicationStartUpPath & "\EmailAttachments\") Then
                        IO.Directory.CreateDirectory(str_ApplicationStartUpPath & "\EmailAttachments\")
                    Else
                    End If
                    FileName = String.Empty
                    FileName = "Voucher" & "-" & setVoucherNo & ""
                    SourceFile = String.Empty
                    SourceFile = _FileExportPath & "\" & FileName & ".pdf"
                    crDiskOps.DiskFileName = SourceFile
                    crExportOps = crpt.ExportOptions
                    With crExportOps
                        .ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile
                        .ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
                        .ExportDestinationOptions = crDiskOps
                        .ExportFormatOptions = crExportType
                    End With
                    'crpt.Refresh()
                    crpt.Export()
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BackgroundWorker2_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker2.DoWork
        Try
            ExportFile(GetVoucherId)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            EmailSave()
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub


    Private Sub btnReminder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReminder.Click
        Try
            Dim frm As New frmreminder
            If Me.grdVouchers.RowCount = 0 Then Exit Sub
            frm.txtSubject.Text = "" & IIf(Me.cmbVoucherType.Text = "Cash", "Cash Receipt", "Bank Receipt") & " " & Me.grdVouchers.GetRow.Cells("Voucher_No").Text & " "
            frm.txtmessage.Text = "" & IIf(setVoucherType = "Cash", "Cash Receipt", "Bank Receipt") & " " _
                    & " " & IIf(setEditMode = False, "of amount " & Me.grdVouchers.GetRow.Cells("Amount").Value & " is made", "of amount " & Me.grdVouchers.GetRow.Cells("Amount").Value & "") & " by user " & LoginUserName & " " & vbCrLf & " " & vbCrLf & " " & vbCrLf & " " & vbCrLf & " " & vbCrLf & " " & vbCrLf & " " & vbCrLf & "Auto Generated By SIRIUS ERP System"
            frm.ShowDialForm = True
            frm.ComboBox1.Text = "Only My"
            frm.ShowDialog()
            frm.Close()
            Exit Sub
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub btnReminder1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            btnReminder_Click(Nothing, Nothing)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="DateLock"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function chkDateLock(ByVal DateLock As SBModel.DateLockBE) As Boolean
        Try
            If DateLock.DateLock.ToString("yyyy-M-d 00:00:00") = Me.dtVoucherDate.Value.ToString("yyyy-M-d 00:00:00") Then
                If DateLock.Lock = True Then
                    Return True
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ValidateDateLock()
        Try
            Dim dateLock As New SBModel.DateLockBE
            dateLock = DateLockList.Find(AddressOf chkDateLock)
            If dateLock IsNot Nothing Then
                If dateLock.DateLock.ToString.Length > 0 Then
                    flgDateLock = True
                Else
                    flgDateLock = False
                End If
            Else
                flgDateLock = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    '#task1608 added by zainab

    Private Sub txtTax_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Try
        '    ''14-Mar-2014 TASK:2491  Imran Ali  Numeric Validation 
        '    If CheckNumericValue(Me.txtTax.Text, Me.txtTax) = False Then
        '        Throw New Exception("Amount is not valid.")
        '    End If
        '    'End Task:2491
        '    Me.txtTaxAmount.Text = Math.Round((((Val(Me.txtAmount.Text) - Val(Me.txtDiscount.Text)) * Val(Me.txtTax.Text)) / 100), TotalAmountRounding)
        'Catch ex As Exception
        '    ShowErrorMessage(ex.Message)
        'End Try
    End Sub
    ''14-Mar-2014 TASK:2491  Imran Ali  Numeric Validation 
    Private Sub txtDiscount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDiscount.KeyPress
        'Try
        '    'If (Char.IsDigit(e.KeyChar) Or Keys.Back = AscW(e.KeyChar) Or e.KeyChar.Equals("."c)) Then
        '    '    e.Handled = False
        '    'Else
        '    '    e.Handled = True
        '    'End If
        '    NumValidation(sender, e)
        'Catch ex As Exception
        '    ShowErrorMessage(ex.Message)
        'End Try
    End Sub
    Private Sub txtTax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Try
        '    'If (Char.IsDigit(e.KeyChar) Or Keys.Back = AscW(e.KeyChar) Or e.KeyChar.Equals("."c)) Then
        '    '    e.Handled = False
        '    'Else
        '    '    e.Handled = True
        '    'End If
        '    NumValidation(sender, e)
        'Catch ex As Exception
        '    ShowErrorMessage(ex.Message)
        'End Try
    End Sub
    Private Sub txtAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAmount.KeyPress
        'Try
        '    'If (Char.IsDigit(e.KeyChar) Or Keys.Back = AscW(e.KeyChar) Or e.KeyChar.Equals("."c)) Then
        '    '    e.Handled = False
        '    'Else
        '    '    e.Handled = True
        '    'End If
        '    NumValidation(sender, e)


        'Catch ex As Exception
        '    ShowErrorMessage(ex.Message)
        'End Try
    End Sub
    'End Task:2491
    Private Sub txtDiscount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiscount.TextChanged
        'Try
        '    'addd comma sperators on all numeric text
        '    'txtDiscount.Text = CDbl(txtDiscount.Text).ToString("N")
        '    'txtDiscount.SelectionStart = txtDiscount.Text.IndexOf(".")
        '    ''14-Mar-2014 TASK:2491  Imran Ali  Numeric Validation 
        '    If CheckNumericValue(Me.txtDiscount.Text, Me.txtDiscount) = False Then
        '        Throw New Exception("Amount is not valid.")
        '    End If
        '    'End Task:2491

        '    Me.txtGrossAmount.Text = Math.Round((((Val(Me.txtAmount.Text) - Val(Me.txtDiscount.Text)))), TotalAmountRounding)
        '    ' txtGrossAmount.Text = Format(Val(txtGrossAmount.Text), "0")
        'Catch ex As Exception
        '    ShowErrorMessage(ex.Message)
        'End Try

    End Sub
    Private Sub txtAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAmount.TextChanged
        Try
            If CheckNumericValue(Me.txtAmount.Text.Trim, CType(sender, TextBox)) = False Then
                Throw New Exception("Amount is not valid.")
            End If
            'End Task:2491

            'start task 3217: Abubakar Siddiq
            If Me.txtAmount.Text = "" Then
                lblAmountNumberConvertor.Text = ""
            Else
                lblAmountNumberConvertor.Text = ModGlobel.NumberToWords(Me.txtAmount.Text)
            End If

            'end task 3217: Abubakar Siddiq

            Me.txtGrossAmount.Text = Math.Round((((Val(Me.txtAmount.Text) - Val(Me.txtDiscount.Text)))), TotalAmountRounding)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PrintReceiptToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintReceiptToolStripMenuItem.Click
        Me.Cursor = Cursors.WaitCursor
        Try
            If Me.grdVouchers.RowCount = 0 Then Exit Sub
            PrintLog = New SBModel.PrintLogBE
            PrintLog.DocumentNo = grdVouchers.GetRow.Cells("Voucher_No").Value.ToString
            PrintLog.UserName = LoginUserName
            PrintLog.PrintDateTime = Date.Now
            Call SBDal.PrintLogDAL.PrintLog(PrintLog)
            'ShowReport("rptVoucher", "{VwGlVoucherSingle.VoucherId}=" & Me.grdVouchers.CurrentRow.Cells(0).Value & "")
            'Chaning Against Request No 798
            AddRptParam("@vid", Me.grdVouchers.CurrentRow.Cells(0).Value)
            ShowReport("rptCashReceipt")
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PrintReceiptToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Cursor = Cursors.WaitCursor
        Try
            If Me.grdVouchers.RowCount = 0 Then Exit Sub

            PrintLog = New SBModel.PrintLogBE
            PrintLog.DocumentNo = grdVouchers.GetRow.Cells("Voucher_No").Value.ToString
            PrintLog.UserName = LoginUserName
            PrintLog.PrintDateTime = Date.Now
            Call SBDal.PrintLogDAL.PrintLog(PrintLog)
            'ShowReport("rptVoucher", "{VwGlVoucherSingle.VoucherId}=" & Me.grdVouchers.CurrentRow.Cells(0).Value & "")
            'Chaning Against Request No 798
            AddRptParam("@vid", Me.grdVouchers.CurrentRow.Cells(0).Value)
            ShowReport("rptCashReceipt")
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="voucherID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DTFromGrid(ByVal voucherID As Int32) As DataTable 'TASK42
        Me.Cursor = Cursors.WaitCursor
        Try
            If Me.grdVouchers.RowCount = 0 Then
                Exit Function
            End If

            'For Each r As Janus.Windows.GridEX.GridEXRow In Me.grdSaved.GetCheckedRows
            Dim DT As New DataTable
            DT = GetDataTable("SP_RptVoucher " & voucherID & "") ' r.Cells(EnumGridMaster.Voucher_Id).Value
            DT.AcceptChanges()
            'DT.Columns.Add("Convert(image, null) as BarCode")
            'Next
            For Each DR As DataRow In DT.Rows
                DR.BeginEdit()
                Dim bcp As New BarcodeProfessional()
                'bcp.Symbology = Symbology.Code39
                bcp.Symbology = Symbology.Code128
                'bcp.Symbology = Symbology.Code93



                bcp.Extended = True
                bcp.DisplayCode = False

                ' bcp.Text=Symbology.
                'bcp.Text = String.Empty
                'bcp.Symbology = Symbology.Code128

                bcp.AddChecksum = False

                'bcp.BarWidth = 3
                'bcp.BarHeight = 0.04F
                'DR.Item("Convert(image, null) as BarCode")
                bcp.Code = "?" & DR.Item("voucher_no").ToString
                'bcp.Code = DR.Item("Employee_Code").ToString
                DR.Item("BarCode") = bcp.GetBarcodeImage(System.Drawing.Imaging.ImageFormat.Png)
                'LoadPicture(DR, "Picture", DR.Item("EmpPicture"))


                DR.EndEdit()


            Next
            Return DT
        Catch ex As Exception
            Throw ex
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Function
    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="voucherID"></param>
    ''' <param name="voucherNo"></param>
    ''' <param name="Print"></param>
    ''' <remarks></remarks>
    Private Sub PrintVoucherBC(ByVal voucherID As Integer, Optional ByVal voucherNo As String = Nothing, Optional Print As Boolean = False) 'TASK42
        Try
            Dim DT As New DataTable
            DT = DTFromGrid(voucherID) 'GetDataTable("SP_RptVoucher " & r.Cells(EnumGridMaster.Voucher_Id).Value & "")
            DT.AcceptChanges()


            '   AddRptParam("@VoucherId", r.Cells("voucher_id").Value)
            ShowReport("rptVoucher", , , , Print, , , DT)
            PrintLog = New SBModel.PrintLogBE
            PrintLog.DocumentNo = voucherNo 'r.Cells("Voucher_No").Value.ToString
            PrintLog.UserName = LoginUserName
            PrintLog.PrintDateTime = Date.Now
            Call SBDal.PrintLogDAL.PrintLog(PrintLog)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PrintSelectedVouchersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Change on 23-11-2013  For Multiple Print Vouchers
        Me.Cursor = Cursors.WaitCursor
        Try
            If Me.grdVouchers.RowCount = 0 Then Exit Sub
            'If Me.grdVouchers.GetCheckedRows = False Then Exit Sub
            Dim cont As Integer = Me.grdVouchers.GetCheckedRows.Length
            If cont > 0 Then
                For Each r As Janus.Windows.GridEX.GridEXRow In Me.grdVouchers.GetCheckedRows
                    'AddRptParam("@vid", r.Cells(0).Value)
                    'ShowReport("rptCashReceipt", , , , True)
                    'PrintLog = New SBModel.PrintLogBE
                    'PrintLog.DocumentNo = r.Cells("Voucher_No").Value.ToString
                    'PrintLog.UserName = LoginUserName
                    'PrintLog.PrintDateTime = Date.Now
                    'Call SBDal.PrintLogDAL.PrintLog(PrintLog)
                    PrintVoucherBC(r.Cells("voucher_id").Value, r.Cells("Voucher_No").Value.ToString())
                Next
            Else
                PrintVoucherBC(Me.grdVouchers.GetRow.Cells("voucher_id").Value, Me.grdVouchers.GetRow.Cells("Voucher_No").Value.ToString())
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    ''R934 Added Delete Event
    Private Sub btnSearchDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchDelete.Click
        Try
            DeleteToolStripButton_Click(Nothing, Nothing)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub


    'R-974 Ehtisham ul Haq user friendly system modification on 29-12-13 
    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grd_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            If e.KeyCode = Keys.F2 Then
                OpenToolStripButton_Click(Me.BtnEdit, Nothing)
                Exit Sub
            End If

            'If e.KeyCode = Keys.Delete Then
            '    DeleteToolStripButton_Click(BtnDelete, Nothing)
            '    Exit Sub
            If e.KeyCode = Keys.PageDown Then
                grdVouchers_DoubleClick(Nothing, Nothing)
            End If
            'End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub grdVouchers_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)

    End Sub
    'Task:2728 Added Function Comments
    Public Function GetComments(ByVal Row As Janus.Windows.GridEX.GridEXRow) As String
        Try
            Dim str As String = String.Empty
            Dim blnCommentsChequeNo As Boolean = Boolean.Parse(getConfigValueByType("CommentsChequeNo").ToString)
            Dim blnCommentsChequeDate As Boolean = Boolean.Parse(getConfigValueByType("CommentsChequeDate").ToString)
            Dim blnCommentsPartyName As Boolean = Boolean.Parse(getConfigValueByType("CommentsPartyName").ToString)
            If Me.cmbVoucherType.Text = "Bank" Then
                If Row IsNot Nothing Then
                    If blnCommentsChequeNo = True Then
                        str += " Chq No. " & Row.Cells("Cheque_No").Value.ToString & ""
                    End If
                    If blnCommentsChequeDate = True Then
                        str += " Chq Date. " & Row.Cells("Cheque_Date").Value.ToString & ""
                    End If
                    If blnCommentsPartyName = True Then
                        str += " Party Name. " & Row.Cells("detail_title").Value.ToString & ""
                    End If
                End If
            ElseIf Me.cmbVoucherType.Text = "Cash" Then
                If Row IsNot Nothing Then
                    If blnCommentsPartyName = True Then
                        str += " Party Name. " & Row.Cells("detail_title").Value.ToString & ""
                    End If
                End If
            End If
            Return str
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'End Task:2728
    Private Sub btnAttachment_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttachment.ButtonClick
        Try
            OpenFileDialog1.FileName = String.Empty
            OpenFileDialog1.Filter = "Word Documents|*.doc|Excel Worksheets|*.xls|Portable Document Format|*.pdf|Corel Draw Files|*.cdr|All Images|*.BMP;*.DIB;*.RLE;*.JPG;*.JPEG;*.JPE;*.JFIF;*.GIF;*.TIF;*.TIFF;*.PNG|" + _
            "All files (*.*)|*.*"

            If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                'Marked Against Task#2015060001 Ali Ansari
                'arrFile = OpenFileDialog1.FileNames
                'Marked Against Task#2015060001 Ali Ansari


                'Altered Against Task#2015060001 Ali Ansari
                Dim a As Integer = 0I
                For a = 0 To OpenFileDialog1.FileNames.Length - 1
                    arrFile.Add(OpenFileDialog1.FileNames(a))
                Next a
                'Altered Against Task#2015060001 Ali Ansari

                Dim intCountAttachedFiles As Integer = 0I
                If Me.BtnSave.Text <> "&Save" Then
                    If Me.grdVouchers.RowCount > 0 Then
                        intCountAttachedFiles = Val(grdVouchers.CurrentRow.Cells("No Of Attachment").Value)
                    End If
                End If
                'Me.btnAttachment.Text = "Attachment (" & arrFile.Length + intCountAttachedFiles & ")"
                'Marked Against Task#2015060001 Ali Ansari
                'Altered Against Task#2015060001 Ali Ansari
                Me.btnAttachment.Text = "Attachment (" & arrFile.Count + intCountAttachedFiles & ")"
                'Altered Against Task#2015060001 Ali Ansari
                'Me.btnAttachment.Text = "Attachment (" & arrFile.Length + intCountAttachedFiles & ")"
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Public Function SaveDocument(ByVal DocId As Integer, ByVal Source As String, ByVal objTrans As OleDb.OleDbTransaction) As Boolean
        Dim cmd As New OleDbCommand
        cmd.Connection = objTrans.Connection
        cmd.Transaction = objTrans
        Try

            Dim dt As New DataTable
            dt = GetDataTable("Select DocId, Source, Path + '\' + FileName  as [FileNames]  From DocumentAttachment WHERE DocId=" & DocId & " AND Source=N'" & Source & "'", objTrans)
            dt.AcceptChanges()


            Dim objdt As New DataTable
            objdt = GetDataTable("Select IsNull(Count(*),0)+1 as Cont From DocumentAttachment WHERE DocId=" & DocId & " AND Source=N'" & Source & "'", objTrans)
            Dim intId As Integer = objdt.Rows(0)(0)

            Dim strSQL As String = String.Empty
            cmd.CommandText = String.Empty
            strSQL = "Delete From DocumentAttachment WHERE DocId=" & DocId & " AND Source=N'" & Source & "'"
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            Dim objPath As String = getConfigValueByType("FileAttachmentPath").ToString

            'Altered Against Task#2015060001 Ali Ansari
            If arrFile.Count > 0 Then
                'Altered Against Task#2015060001 Ali Ansari
                'Marked Against Task#2015060001 Ali Ansari
                '            If arrFile.Length > 0 Then
                'Marked Against Task#2015060001 Ali Ansari
                For Each objFile As String In arrFile
                    If IO.File.Exists(objFile) Then
                        If IO.Directory.Exists(objPath) = False Then
                            IO.Directory.CreateDirectory(objPath)
                        End If
                        Dim New_Files As String = intId & "_" & DocId & "_" & Me.dtVoucherDate.Value.ToString("yyyyMMdd") & "." & objFile.Substring(objFile.LastIndexOf(".") + 1)
                        Dim dr As DataRow
                        dr = dt.NewRow
                        dr(0) = DocId
                        dr(1) = Source
                        dr(2) = objPath & "\" & New_Files
                        dt.Rows.Add(dr)
                        dt.AcceptChanges()
                        If IO.File.Exists(objPath & "\" & New_Files) Then
                            IO.File.Delete(objPath & "\" & New_Files)
                        End If
                        IO.File.Copy(objFile, objPath & "\" & New_Files)
                        intId += 1
                    End If
                Next
            End If


            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    For Each r As DataRow In dt.Rows
                        Dim strPath As String = objPath
                        Dim strFileName As String = r.Item("FileNames").ToString.Substring(r.Item("FileNames").ToString.LastIndexOf("\") + 1)
                        cmd.CommandText = String.Empty
                        strSQL = "INSERT INTO DocumentAttachment(DocId, Source, FileName, Path) Values(" & Val(r("DocId").ToString) & ",N'" & r.Item("Source").ToString.Replace("'", "''") & "', N'" & strFileName.Replace("'", "''") & "', N'" & strPath.Replace("'", "''") & "')"
                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()
                    Next
                End If
            End If


        Catch ex As Exception
            objTrans.Rollback()
            Throw ex
        End Try
    End Function

    Private Sub PrintAttachmentVoucherToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintAttachmentVoucherToolStripMenuItem.Click
        Try
            If Me.grdVouchers.RowCount = 0 Then Exit Sub
            'AddRptParam("Pm-dtVoucher.Voucher_Id", Me.grdVouchers.GetRow.Cells(0).Value)
            DataSetShowReport("RptVoucherDocument", GetVoucherRecord())
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Public Function GetVoucherRecord() As DataSet
        Try

            Dim strSQL As String = String.Empty
            Dim ds As New dsVoucherDocumentAttachment
            ds.Tables.Clear()
            strSQL = "SELECT  TOP 100 PERCENT V.voucher_id, V.voucher_no, V.voucher_date, V.voucher_code, VTP.voucher_type, V.Reference, V.post, V.BankDesc, V.UserName, " _
                    & " V.Posted_UserName, V.CheckedByUser, V.Checked, VD.voucher_detail_id, VD.coa_detail_id, COA.detail_code, COA.detail_title, VD.comments, VD.debit_amount,  " _
                    & " VD.credit_amount, VD.sp_refrence, VD.direction, VD.CostCenterID, VD.EmpID, VD.InvoiceId, VD.Adjustment, VD.Cheque_No, VD.Cheque_Date, VD.BankDescription, VD.Tax_Percent,  " _
                    & " VD.Tax_Amount, VD.Cheque_Clearance_Date, VD.PayeeTitle, VD.Cheque_Status, VD.ChequeDescription, COA.sub_sub_code, COA.sub_sub_title " _
                    & " FROM dbo.tblVoucher AS V INNER JOIN " _
                    & " dbo.tblVoucherDetail AS VD ON V.voucher_id = VD.voucher_id INNER JOIN " _
                    & " dbo.vwCOADetail AS COA ON VD.coa_detail_id = COA.coa_detail_id LEFT OUTER JOIN  " _
                    & " dbo.tblDefVoucherType AS VTP ON V.voucher_type_id = VTP.voucher_type_id WHERE (V.voucher_id=" & Me.grdVouchers.GetRow.Cells(0).Value & ") " _
                    & " ORDER BY VD.voucher_detail_id "
            Dim dt As New DataTable
            dt = GetDataTable(strSQL)
            dt.AcceptChanges()
            ds.Tables.Add(dt)
            ds.Tables(0).TableName = "dtVoucher"


            strSQL = String.Empty
            'Marked Against Task#2015060008 To Discard Non Images
            'strSQL = "Select DocId,FileName,Path,Convert(Image,'') as Attachment_Image From DocumentAttachment WHERE (DocId=" & Me.grdVouchers.GetRow.Cells(0).Value & ") AND Source=N'" & Me.Name & "'"
            'Marked Against Task#2015060008 To Discard Non Images
            'Altered Against Task#2015060008 To Discard Non Images
            strSQL = "Select DocId,FileName,Path,Convert(Image,'') as Attachment_Image,1 as pic From DocumentAttachment  where right(filename,3) in ('BMP','DIB','RLE','JPG','JPEG','JPE','JFIF','GIF','TIF','TIFF','PNG')  and  DocId=" & Val(Me.grdVouchers.GetRow.Cells(0).Value.ToString) & " AND Source=N'" & Me.Name & "'"
            'Altered Against Task#2015060008 To Discard Non Images
            Dim dtAttach As New DataTable
            dtAttach.TableName = "dtAttachment"
            dtAttach = GetDataTable(strSQL)

            If dtAttach IsNot Nothing Then
                If dtAttach.Rows.Count > 0 Then
                    For Each r As DataRow In dtAttach.Rows
                        If r.Item("PIC").ToString = "1" Then
                            r.BeginEdit()
                            If IO.File.Exists(CStr(r("Path").ToString & "\" & r("FileName").ToString)) Then
                                LoadPicture(r, "Attachment_Image", CStr(r("Path").ToString & "\" & r("FileName").ToString))
                            End If
                            r.EndEdit()
                        End If
                    Next

                End If
            End If

            ds.Tables.Add(dtAttach)
            ds.Tables(1).TableName = "dtAttachment"
            ds.AcceptChanges()

            Return ds
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ''' <summary>
    ''' TASK TFS4915 done on 15-11-2018
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grdVouchers_LinkClicked(ByVal sender As Object, ByVal e As Janus.Windows.GridEX.ColumnActionEventArgs) Handles grdVouchers.LinkClicked
        Try
            If grdVouchers.RowCount = 0 Then Exit Sub
            If e.Column.Key = "No Of Attachment" Then
                Dim frm As New frmAttachmentView
                frm._Source = Me.Name
                frm._VoucherId = Me.grdVouchers.GetRow.Cells(0).Value
                frm.ShowDialog()
                Exit Sub
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub



#Region "SMS Template Setting"
    Public Function GetSMSParamters() As List(Of String)
        Try
            Dim str As New List(Of String)
            str.Add("@AccountCode")
            str.Add("@AccountTitle")
            str.Add("@DocumentNo")
            str.Add("@DocumentDate")
            str.Add("@OtherDocNo")
            str.Add("@Remarks")
            str.Add("@Amount")
            str.Add("@ChequeNo")
            str.Add("@ChequeDate")
            str.Add("@CompanyName")
            str.Add("@CellNo")
            str.Add("@SIRIUS")
            Return str
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetSMSKey() As List(Of String)
        Try
            Dim str As New List(Of String)
            str.Add("Cash Receipt")
            str.Add("Bank Receipt")
            Return str
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Sub btnSMSTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSMSTemplate.Click
        Try
            Dim frmSMS As New frmSMSTemplate
            ApplyStyleSheet(frmSMS)
            frmSMS.cmbKey.DataSource = GetSMSKey()
            frmSMS.lstParameters.DataSource = GetSMSParamters()
            frmSMS.Show()
            frmSMS.BringToFront()
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
#End Region

    Private Sub chkEnableDepositAc_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkEnableDepositAc.CheckedChanged
        Try
            If Me.BtnSave.Text <> "&Save" Then
                If Me.chkEnableDepositAc.Checked = True Then
                    Me.cmbCashAccount.Enabled = True
                Else
                    Me.cmbCashAccount.Enabled = False
                End If
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    'Altered against Task#20150514 to send SMS on Message Ali Ansari
    Public Sub SendSMS()
        Try

            If Me.chkPost.Checked = True Then
                If EnabledBrandedSMS = True Then
                    If GetSMSConfig("Receipt").Enable = True Or GetSMSConfig("Receipt").EnabledAdmin = True Then
                        If msg_Confirm(str_ConfirmSendSMSMessage) = True Then
                            For Each r As Janus.Windows.GridEX.GridEXRow In Me.grd.GetRows
                                'If (r.Cells("Mobile").Value.ToString <> "" Or r.Cells("Mobile").Value.ToString.Length >= 10) Then
                                Try
                                    Dim strMSGBody As String = String.Empty ' Task:2631 Added object
                                    Dim objSMSTemp As New SMSTemplateParameter
                                    If Me.cmbVoucherType.Text = "Bank" Then
                                        objSMSTemp = GetSMSTemplate("Bank Receipt")
                                    Else
                                        objSMSTemp = GetSMSTemplate("Cash Receipt")
                                    End If
                                    If objSMSTemp IsNot Nothing Then
                                        Dim objSMSParam As New SMSParameters
                                        objSMSParam.AccountCode = r.Cells("detail_code").Value.ToString
                                        objSMSParam.AccountTitle = r.Cells("detail_title").Value.ToString
                                        objSMSParam.DocumentNo = Me.txtVoucherNo.Text
                                        objSMSParam.DocumentDate = Me.dtVoucherDate.Value
                                        objSMSParam.Remarks = Me.txtMemo.Text
                                        objSMSParam.CellNo = r.Cells("Mobile").Value.ToString
                                        objSMSParam.Amount = Math.Round((Val(r.Cells("Amount").Value.ToString) - (Val(r.Cells("Discount").Value.ToString) + Val(r.Cells("Tax_Amount").Value.ToString))), 0)
                                        If Me.cmbVoucherType.Text = "Bank" Then
                                            objSMSParam.ChequeNo = r.Cells("Cheque_No").Value.ToString
                                            If IsDBNull(r.Cells("Cheque_Date").Value) Then
                                                objSMSParam.ChequeDate = Nothing
                                            Else
                                                objSMSParam.ChequeDate = r.Cells("Cheque_Date").Value
                                            End If
                                        End If
                                        objSMSParam.CompanyName = CompanyTitle
                                        Dim objSMSLog As SMSLogBE
                                        If GetSMSConfig("Receipt").EnabledAdmin = True Then
                                            For Each strMob As String In strAdminMobileNo.Replace(",", ";").Replace("|", ";").Replace("^", ";").Split(";")
                                                If strMob.Length > 10 Then
                                                    objSMSLog = New SMSLogBE
                                                    objSMSLog.SMSBody = objSMSTemp.SMSTemplate
                                                    objSMSLog.PhoneNo = strMob 'r.Cells("Mobile").Value.ToString
                                                    objSMSLog.CreatedByUserID = LoginUserId
                                                    Call SMSTemplateDAL.AddSMSLog(objSMSLog, objSMSParam)
                                                End If
                                            Next
                                        End If
                                        If GetSMSConfig("Receipt").Enable = True AndAlso (r.Cells("Mobile").Value.ToString.Trim.Length >= 10) Then
                                            objSMSLog = New SMSLogBE
                                            objSMSLog.SMSBody = objSMSTemp.SMSTemplate
                                            objSMSLog.PhoneNo = r.Cells("Mobile").Value.ToString
                                            objSMSLog.CreatedByUserID = LoginUserId
                                            Call SMSTemplateDAL.AddSMSLog(objSMSLog, objSMSParam)
                                        End If
                                    End If
                                Catch ex As Exception

                                End Try
                            Next
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        Finally
            Me.lblProgress.Visible = False
        End Try
    End Sub
    'Altered against Task#20150514 to send SMS on Message Ali Ansari

    Private Sub Btn_SaveAttachment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If Con.State = ConnectionState.Closed Then Con.Open()
        Dim trans As OleDbTransaction = Con.BeginTransaction

        Try
            Dim intCountAttachedFiles As Integer = 0I
            OpenFileDialog1.FileName = String.Empty

            OpenFileDialog1.Filter = "Word Documents|*.doc|Excel Worksheets|*.xls|Portable Document Format|*.pdf|Corel Draw Files|*.cdr|All Images|*.BMP;*.DIB;*.RLE;*.JPG;*.JPEG;*.JPE;*.JFIF;*.GIF;*.TIF;*.TIFF;*.PNG|" + _
                        "All files (*.*)|*.*"

            If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then

                Dim a As Integer = 0I
                For a = 0 To OpenFileDialog1.FileNames.Length - 1
                    arrFile.Add(OpenFileDialog1.FileNames(a))
                Next a

                If arrFile.Count > 0 Then
                    SaveDocument(Me.grdVouchers.CurrentRow.Cells(0).Value, Me.Name, trans)
                    trans.Commit()
                    PopulateGrid()

                End If
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    'Task#04082015 company changed from combo box
    Private Sub cmbCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCompany.SelectedIndexChanged
        Try

            If Not blnEditMode = True Then
                Me.txtVoucherNo.Text = GetVoucherNo()
            End If
            'Ali Faisal : TFS1296 : Select the Cost Center for seleted company if that cost center is associated with that company : 21-Aug-2017
            If Not Me.cmbCompany.SelectedIndex = -1 Then
                If Not Me.cmbCostCenter.SelectedIndex = -1 Then Me.cmbCostCenter.SelectedValue = Val(CType(Me.cmbCompany.SelectedItem, DataRowView).Row.Item("CostCenterId").ToString)
            End If
            'Ali Faisal : TFS1296 : End
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    'End Task#04082015
    Public Sub MainEditRecord()
        Try
            Dim str As String = " SELECT DISTINCT vwCOADetail.detail_title, tblVoucherDetail.coa_detail_id, tblVoucherDetail.Comments, isnull(tblVoucherDetail.CostCenterId,0) as CostCenterId , isnull(tblVoucherDetail.EmpID,0) as Employee, isnull(tblVoucherDetail.InvoiceId,0) as InvoiceId " _
                  & " FROM         tblVoucherDetail INNER JOIN" _
                  & " vwCOADetail ON tblVoucherDetail.coa_detail_id = vwCOADetail.coa_detail_id" _
                  & " Where voucher_id =" & Me.grdVouchers.GetRow.Cells("Voucher_Id").Value & " AND (tblVoucherDetail.debit_amount > 0) AND Account_Type IN('Cash','Bank')"
            Dim objCommand As New OleDbCommand
            Dim objDataAdapter As New OleDbDataAdapter
            Dim dt As New DataTable
            Dim objCon As New OleDbConnection(Con.ConnectionString)
            If objCon.State = ConnectionState.Open Then objCon.Close()

            objCon.Open()
            objCommand.Connection = objCon
            objCommand.CommandType = CommandType.Text
            objCommand.CommandText = str

            objDataAdapter.SelectCommand = objCommand
            objDataAdapter.Fill(dt)
            dt.AcceptChanges()
            objCon.Close()
            If dt.Rows.Count = 0 Then Exit Sub
            If cmbCashAccount IsNot Nothing Then
                Me.cmbCashAccount.SelectedValue = dt.Rows(0)(1).ToString
                'Me.cmbCashAccount.SelectedValue = dt.Rows(1)(1).ToString

                Me.cmbCostCenter.SelectedValue = dt.Rows(0)(3).ToString
                Me.txtMemo.Text = dt.Rows(0)(2).ToString
            Else
                Me.cmbCashAccount.SelectedValue = 0
            End If


        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub btnPrintUpdatedVoucher_Click(sender As Object, e As EventArgs)
        Try

            If grdVouchers.RowCount = 0 Then Exit Sub
            AddRptParam("@VoucherId", Val(Me.grdVouchers.GetRow.Cells("Voucher_Id").Value.ToString))
            ShowReport("rptVoucherUpdated")

        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub PrintUpdatedVoucherToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrintUpdatedVoucherToolStripMenuItem.Click
        Try
            Me.btnPrintUpdatedVoucher_Click(Nothing, Nothing)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub cmbCurrency_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCurrency.SelectedIndexChanged
        Try
            'If Not Me.cmbCurrency.SelectedItem Is Nothing Then
            '    Dim dr As DataRowView = CType(cmbCurrency.SelectedItem, DataRowView)
            '    Me.txtCurrentRate.Text = dr.Row.Item("CurrencyRate").ToString
            '    Me.grd.RootTable.Columns("CurrencyAmount").Caption = "" & Me.cmbCurrency.Text & " Amount"
            '    Me.grd.RootTable.Columns("Amount").Caption = "" & GetBasicCurrencyName(Val(getConfigValueByType("Currency").ToString)) & " Amount"
            '    If Me.cmbCurrency.Text.ToUpper.ToString = GetBasicCurrencyName(Val(getConfigValueByType("Currency").ToString)).ToUpper.ToString Then
            '        Me.grd.RootTable.Columns("Amount").Visible = False
            '    Else
            '        Me.grd.RootTable.Columns("Amount").Visible = True
            '    End If
            'End If

            If Not Me.cmbCurrency.SelectedItem Is Nothing Then
                Dim dr As DataRowView = CType(cmbCurrency.SelectedItem, DataRowView)
                If blnEditMode = True Then

                Else
                    Me.txtCurrencyRate.Text = Math.Round(Convert.ToDouble(dr.Row.Item("CurrencyRate").ToString), 4)
                End If
                ''TASK TFS1474
                If Me.cmbCurrency.SelectedValue = BaseCurrencyId Then
                    Me.txtCurrencyRate.Enabled = False
                Else
                    Me.txtCurrencyRate.Enabled = True
                End If
                ''END TASK TFS1474
                Me.grd.RootTable.Columns("CurrencyAmount").Caption = "Amount (" & Me.cmbCurrency.Text & ")"

                Me.grd.RootTable.Columns("CurrencyDiscount").Caption = "Discount (" & Me.cmbCurrency.Text & ")"
                Me.grd.RootTable.Columns(grdEnm.TaxCurrencyAmount).Caption = "Tax (" & Me.cmbCurrency.Text & ")"

                Me.grd.RootTable.Columns(grdEnm.Sales_Tax).Caption = "SalesTax (" & Me.cmbCurrency.Text & ")"

                Me.grd.RootTable.Columns(grdEnm.WH_Tax).Caption = "WHSTax (" & Me.cmbCurrency.Text & ")"
                Me.grd.RootTable.Columns(grdEnm.Net_Amount).Caption = "NetAmount (" & Me.cmbCurrency.Text & ")"
                Me.grd.RootTable.Columns("Amount").Caption = "Amount (" & Me.BaseCurrencyName & ")"
                Me.grd.RootTable.Columns("Discount").Caption = "Discount (" & Me.BaseCurrencyName & ")"
                Me.grd.RootTable.Columns(grdEnm.TaxAmount).Caption = "Tax (" & Me.BaseCurrencyName & ")"
                Me.grd.RootTable.Columns(grdEnm.SalesTaxPer).Caption = "SalesTax% "
                Me.grd.RootTable.Columns(grdEnm.SalesTaxAmount).Caption = "SalesTax (" & Me.BaseCurrencyName & ")"
                Me.grd.RootTable.Columns(grdEnm.WHTaxPer).Caption = "WHSTax% "
                Me.grd.RootTable.Columns(grdEnm.WHTaxAmount).Caption = "WHSTax (" & Me.BaseCurrencyName & ")"
                Me.grd.RootTable.Columns(grdEnm.NetAmount).Caption = "NetAmount (" & Me.BaseCurrencyName & ")"




                If Me.cmbCurrency.SelectedValue = Me.BaseCurrencyId Then
                    Me.grd.RootTable.Columns("Amount").Visible = False
                    Me.grd.RootTable.Columns("Discount").Visible = False
                    Me.grd.RootTable.Columns(grdEnm.TaxAmount).Visible = False
                    'added by zainab #task1608
                    Me.grd.RootTable.Columns(grdEnm.SalesTaxAmount).Visible = False
                    Me.grd.RootTable.Columns(grdEnm.WHTaxAmount).Visible = False
                    Me.grd.RootTable.Columns(grdEnm.NetAmount).Visible = False

                Else
                    Me.grd.RootTable.Columns("Amount").Visible = True
                    Me.grd.RootTable.Columns("Discount").Visible = True
                    Me.grd.RootTable.Columns(grdEnm.TaxAmount).Visible = True
                    'added by zainab #task1608
                    Me.grd.RootTable.Columns(grdEnm.SalesTaxAmount).Visible = True
                    Me.grd.RootTable.Columns(grdEnm.WHTaxAmount).Visible = True
                    Me.grd.RootTable.Columns(grdEnm.NetAmount).Visible = True

                End If

                If Val(Me.txtCurrencyRate.Text) = 0 Then
                    Me.txtCurrencyRate.Text = 1
                End If
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    Private Function GetCurrencyRate(ByVal currencyId As Integer) As Double
        Dim str As String = String.Empty
        Dim dt As New DataTable
        Dim currencyRate As Double = 0
        Try
            str = " Select CurrencyRate, CurrencyId From tblCurrencyRate Where CurrencyRateId in ( Select Max(CurrencyRateId) From tblCurrencyRate group by CurrencyId) And CurrencyId = " & currencyId & ""
            dt = GetDataTable(str)
            dt.AcceptChanges()
            If dt.Rows.Count > 0 Then
                currencyRate = Val(dt.Rows.Item(0).Item(0).ToString)
            End If

            Return currencyRate

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub grd_CellEdited(sender As Object, e As Janus.Windows.GridEX.ColumnActionEventArgs)
        Try
            Me.grd.GetRow.Cells(grdEnm.Amount).Value = Me.grd.GetRow.Cells(grdEnm.CurrencyAmount).Value * Me.grd.GetRow.Cells(grdEnm.CurrencyRate).Value

        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DualPrinting()

        Try
            If msg_Confirm_DualPrint(str_ConfirmPrintVoucher, True, True, Msgfrm) = True Then
                If Me.grdVouchers.RowCount = 0 Then Exit Sub

                If Msgfrm.chkEnableVoucherPrints.Checked = True AndAlso Msgfrm.chkEnableSlipPrints.Checked = True Then
                    PrintVoucherBC(Me.grdVouchers.GetRow.Cells("voucher_id").Value, Me.grdVouchers.GetRow.Cells("Voucher_No").Value.ToString(), True)

                    PrintLog = New SBModel.PrintLogBE
                    PrintLog.DocumentNo = grdVouchers.GetRow.Cells("Voucher_No").Value.ToString
                    PrintLog.UserName = LoginUserName
                    PrintLog.PrintDateTime = Date.Now
                    Call SBDal.PrintLogDAL.PrintLog(PrintLog)
                    AddRptParam("@vid", Me.grdVouchers.CurrentRow.Cells(0).Value)
                    ShowReport("rptCashReceipt", , , , True)

                ElseIf Msgfrm.chkEnableVoucherPrints.Checked = True Then
                    PrintVoucherBC(Me.grdVouchers.GetRow.Cells("voucher_id").Value, Me.grdVouchers.GetRow.Cells("Voucher_No").Value.ToString(), True)
                ElseIf Msgfrm.chkEnableSlipPrints.Checked = True Then
                    PrintLog = New SBModel.PrintLogBE
                    PrintLog.DocumentNo = grdVouchers.GetRow.Cells("Voucher_No").Value.ToString
                    PrintLog.UserName = LoginUserName
                    PrintLog.PrintDateTime = Date.Now
                    Call SBDal.PrintLogDAL.PrintLog(PrintLog)
                    AddRptParam("@vid", Me.grdVouchers.CurrentRow.Cells(0).Value)
                    ShowReport("rptCashReceipt", , , , True)
                Else
                    msg_Error("Please Select any one Print option")
                    DualPrinting()

                End If

            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub GetAllUsersRecord(Optional ByVal strCondition As String = "")
        Try
            Dim ClosingDate As DateTime = Convert.ToDateTime(getConfigValueByType("EndOfDate").ToString)
            Dim PreviouseRecordShow As Boolean = Convert.ToBoolean(getConfigValueByType("PreviouseRecordShow").ToString)
            Dim strSql As String = String.Empty
            If Mode = "Normal" Then
                strSql = "SELECT  DISTINCT tblvoucher.voucher_id, voucher_no, voucher_date, CASE WHEN (voucher_Type_ID = 5) THEN 'Bank' ELSE 'Cash' END AS [Payment Method], tblVoucher.Cheque_No,tblVoucher.Remarks, isnull(ReceiptAmount.Amount,0) as Amount, Post, Case When Post=1 then 'Posted' else 'UnPosted' end as Status,IsNull(tblVoucher.Checked,0) as Checked,Isnull(tblVoucher.Employee_Id,0) as Employee_Id, CASE WHEN ISNULL(PrintLog.cont,0)=0 THEN 'Print Pending' ELSE 'Printed' end as [Print Status], IsNull([No Of Attachment],0) as  [No Of Attachment],tblVoucher.USERNAME  AS 'User Name',CompanyDefTable.CompanyName " _
                     & " FROM tblVoucher INNER JOIN tblVoucherDetail ON tblVoucher.Voucher_Id = tblVoucherDetail.Voucher_Id LEFT OUTER JOIN vwCOADetail ON vwCOADetail.coa_detail_id = tblVoucherDetail.coa_detail_id Left Outer join  CompanyDefTable on tblVoucher.location_id=CompanyDefTable.CompanyId    LEFT OUTER JOIN(Select Count(Id) as Cont, DocumentNo From tblPrint_Log Group By DocumentNo) PrintLog On PrintLog.DocumentNo = voucher_no INNER JOIN(Select voucher_id, SUM(ISNULL(credit_amount,0)) as Amount From tblVoucherDetail Group By Voucher_Id) ReceiptAmount On ReceiptAmount.Voucher_Id = tblVoucher.Voucher_Id LEFT OUTER JOIN (Select Count(*) as [No Of Attachment],DocId From DocumentAttachment WHERE (source = N'" & Me.Name & "') Group By DocId, Source) Doc_Att on Doc_Att.DocId = tblVoucher.Voucher_Id " _
                     & " WHERE     (source = N'" & Me.Name & "')  " & IIf(PreviouseRecordShow = True, "", " AND (Convert(varchar, voucher_date,102) > Convert(datetime, N'" & ClosingDate & "', 102))") & ""
                If flgCompanyRights = True Then
                    strSql += " And tblVoucher.Location_Id=" & MyCompanyId & ""
                End If
            End If
            If Me.dtpFrom.Checked = True Then
                strSql += " AND tblVoucher.Voucher_Date >= Convert(Datetime, N'" & Me.dtpFrom.Value.ToString("yyyy-M-d 00:00:00") & "', 102) "
            End If
            If Me.dtpTo.Checked = True Then
                strSql += " AND tblVoucher.Voucher_Date <= Convert(Datetime, N'" & Me.dtpTo.Value.ToString("yyyy-M-d 23:59:59") & "', 102) "
            End If
            If Me.cmbSearchVoucherType.SelectedIndex > 0 Then
                strSql += " AND tblVoucher.Voucher_Type_Id=" & Me.cmbSearchVoucherType.SelectedValue
            End If
            If Me.txtSearchVoucherNo.Text <> String.Empty Then
                strSql += " AND tblvoucher.Voucher_No LIKE '%" & Me.txtSearchVoucherNo.Text & "%'"
            End If
            If Me.txtSearchChequeNo.Text <> String.Empty Then
                strSql += " AND tblVoucherDetail.Cheque_No LIKE '%" & Me.txtSearchChequeNo.Text & "%'"
            End If
            If Me.dtpSearchChequeDate.Checked = True Then
                strSql += " AND (Convert(Varchar, tblVoucherDetail.Cheque_Date,102) = Convert(Datetime, N'" & Me.dtpSearchChequeDate.Value.ToString("yyyy-M-d 00:00:00") & "',102))"
            End If
            If Me.txtFromAmount.Text <> String.Empty Then
                If Val(Me.txtFromAmount.Text) > 0 Then
                    strSql += " AND tblVoucherDetail.credit_amount >=" & Val(Me.txtFromAmount.Text) & ""
                End If
            End If
            If Me.txtToAmount.Text <> String.Empty Then
                If Val(Me.txtFromAmount.Text) > 0 Then
                    strSql += " AND tblVoucherDetail.credit_amount <=" & Val(Me.txtToAmount.Text) & ""
                End If
            End If
            If cmbSearchAccount.SelectedRow IsNot Nothing Then
                If Me.cmbSearchAccount.SelectedRow.Cells(0).Value <> 0 Then
                    strSql += " AND tblVoucherDetail.coa_detail_id = " & Me.cmbSearchAccount.Value
                End If
            End If
            If Me.txtSearchComments.Text <> String.Empty Then
                strSql += " AND tblVoucherDetail.Comments LIKE '%" & Me.txtSearchComments.Text & "%'"
            End If
            strSql = strSql + " order by tblvoucher.voucher_id desc"
            FillGridEx(grdVouchers, strSql, True)
            Me.grdVouchers.RootTable.Columns("No Of Attachment").ColumnType = Janus.Windows.GridEX.ColumnType.Link
            Me.grdVouchers.RootTable.Columns.Add("Column1")
            Me.grdVouchers.RootTable.Columns("Column1").ActAsSelector = True
            Me.grdVouchers.RootTable.Columns("Column1").UseHeaderSelector = True
            Me.grdVouchers.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True
            Me.grdVouchers.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed
            grdVouchers.RootTable.Columns(0).Visible = False  'Voucher ID
            grdVouchers.RootTable.Columns("Post").Visible = False 'Voucher Post 
            grdVouchers.RootTable.Columns("Employee_Id").Visible = False
            Me.grdVouchers.RootTable.Columns("voucher_no").Caption = "Vouchr No"
            Me.grdVouchers.RootTable.Columns("voucher_date").Caption = "Date"
            Me.grdVouchers.RootTable.Columns("Amount").FormatString = "N"
            Me.grdVouchers.RootTable.Columns("Amount").TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            Me.grdVouchers.RootTable.Columns("Amount").HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Far
            Me.grdVouchers.RootTable.Columns("Amount").AggregateFunction = Janus.Windows.GridEX.AggregateFunction.Sum
            Me.grdVouchers.RootTable.Columns("Voucher_Date").FormatString = str_DisplayDateFormat
            Me.grdVouchers.RootTable.Columns.Add("Selector", Janus.Windows.GridEX.ColumnType.CheckBox)
            Me.grdVouchers.RootTable.Columns("Selector").ActAsSelector = True
            Me.grdVouchers.RootTable.Columns("Selector").UseHeaderSelector = True
            Me.grdVouchers.AutoSizeColumns()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub btnGetAllRecord_Click(sender As Object, e As EventArgs)
        Try
            GetAllUsersRecord()
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub txtChequeNo_TextChanged(sender As Object, e As EventArgs)

    End Sub



    '#task1608 added by zainab
    Private Sub txtTax_TextChanged_1(sender As Object, e As EventArgs) Handles txtTax.TextChanged
        Try
            'addd comma sperators on all numeric text
            'txtTax.Text = CDbl(txtTax.Text).ToString("N")
            'txtTax.SelectionStart = txtTax.Text.IndexOf(".")
            If CheckNumericValue(Me.txtTax.Text, Me.txtTax) = False Then
                Throw New Exception("Amount is not valid.")
            End If
            'End Task:2491
            'GrossAmount = Math.Round(((Val(Me.txtAmount.Text) - Val(Me.txtDiscount.Text) / 100)), DecimalPointInValue)
            ''Me.txtTaxAmount.Text = Math.Round((Val(Me.txtGrossAmount.Text) * (Val(Me.txtTax.Text) / 100)), TotalAmountRounding)
            'rafay txtTaxAmount.Text = Format(Val(txtTaxAmount.Text), "0")
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub cmbCashAccount_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbCashAccount.KeyDown
        ''TFS1781 : Ayesha Rehman :Added for Selection of Customer or Vendor
        Try
            If e.KeyCode = Keys.F1 Then
                frmAccountSearch.AccountType = "'" & Me.cmbVoucherType.Text & "'"
                frmAccountSearch.BringToFront()
                frmAccountSearch.ShowDialog()
                If frmAccountSearch.DialogResult = Windows.Forms.DialogResult.OK Then
                    cmbCashAccount.SelectedValue = frmAccountSearch.SelectedAccountId
                End If
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try


    End Sub


    Private Sub cmbCashAccount_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCashAccount.SelectedValueChanged
        Try

            If Me.cmbCashAccount.SelectedIndex > 0 Then
                Try
                    Dim objCommand As New OleDbCommand("SELECT SUM(tblVoucherDetail.debit_amount) - SUM(tblVoucherDetail.credit_amount) FROM tblVoucherDetail  INNER JOIN tblVoucher On tblVoucherDetail.Voucher_Id = tblVoucher.Voucher_Id" _
                                                           & " WHERE tblVoucherDetail.coa_detail_id = " & Me.cmbCashAccount.SelectedValue _
                                                         & " AND tblVoucher.Post=1", Con)
                    If Con.State = ConnectionState.Closed Or Con.State = ConnectionState.Broken Then Con.Open()
                    objCommand.Connection = Con

                    txtDepositBeforeBalance.Text = CInt(Val(objCommand.ExecuteScalar))
                    If Val(Me.txtDepositBeforeBalance.Text) < 0 Then
                        Me.txtDepositBeforeBalance.Text = "(" & Replace(Me.txtDepositBeforeBalance.Text, "-", "") & ")"
                    End If
                Catch ex As Exception
                    Me.txtDepositBeforeBalance.Text = 0
                End Try
            Else
                Me.txtDepositBeforeBalance.Text = ""
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try

    End Sub

    Private Sub cmbAccounts_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbAccounts.KeyDown, cmbInvoice.KeyDown
        Try
            ''TFS1781 : Ayesha Rehman :Added for Selection of Customer or Vendor
            If e.KeyCode = Keys.F1 Then

                frmAccountSearch.AccountType = String.Empty

                frmAccountSearch.BringToFront()
                frmAccountSearch.ShowDialog()
                If frmAccountSearch.DialogResult = Windows.Forms.DialogResult.OK Then
                    cmbAccounts.Value = frmAccountSearch.SelectedAccountId
                End If
            End If
        Catch ex As Exception
            msg_Error(ex.Message)
        End Try
    End Sub

    Private Sub cmbAccounts_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAccounts.Leave, cmbInvoice.Leave
        Try
            If Me.cmbAccounts.IsItemInList = False Then Exit Sub
            If cmbAccounts.ActiveRow.Cells(0).Value > 0 AndAlso (LoginGroup = "Administrator" Or (Me.cmbAccounts.ActiveRow.Cells("AccessLevel").Value.ToString = "Everyone")) Then
                Try
                    Dim objCommand As New OleDbCommand("SELECT SUM(tblVoucherDetail.debit_amount) - SUM(tblVoucherDetail.credit_amount) FROM tblVoucherDetail INNER JOIN tblVoucher ON tblVoucherDetail.Voucher_Id = tblVoucher.Voucher_Id " _
                                                                           & " WHERE tblVoucherDetail.coa_detail_id = " & cmbAccounts.ActiveRow.Cells(0).Value _
                                                                        & " AND tblVoucher.Post=1", Con)
                    If Con.State = ConnectionState.Closed Or Con.State = ConnectionState.Broken Then Con.Open()
                    objCommand.Connection = Con

                    txtCustomerBalance.Text = Math.Round(Val(objCommand.ExecuteScalar), 0)
                    If Val(txtCustomerBalance.Text) < 0 Then
                        Me.txtCustomerBalance.Text = "(" & Replace(Me.txtCustomerBalance.Text, "-", "") & ")"
                    End If

                Catch ex As Exception
                    txtCustomerBalance.Text = 0
                End Try
            Else
                txtCustomerBalance.Text = " "
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub


    Private Sub txtGrossAmount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtGrossAmount.KeyPress
        Try
            NumValidation(sender, e)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub txtTax_KeyPress_1(sender As Object, e As KeyPressEventArgs) Handles txtTax.KeyPress
        Try
            NumValidation(sender, e)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub txtTaxAmount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTaxAmount.KeyPress
        Try
            NumValidation(sender, e)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub txtSalesTax_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSalesTax.KeyPress
        Try
            ' rafay  NumValidation(sender, e)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub txtSalesTaxAmount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSalesTaxAmount.KeyPress
        Try
            'rafay NumValidation(sender, e)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub txtWithHolding_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtWithHolding.KeyPress
        Try
            NumValidation(sender, e)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub txtWithHoldingTaxAmount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtWithHoldingTaxAmount.KeyPress
        Try
            NumValidation(sender, e)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub txtNetAmount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNetAmount.KeyPress
        Try
            NumValidation(sender, e)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub





    Private Sub btnPrintUpdatedVoucher_Click1(sender As Object, e As EventArgs) Handles btnPrintUpdatedVoucher.Click
        Try
            If grdVouchers.RowCount = 0 Then Exit Sub
            AddRptParam("@VoucherId", Val(Me.grdVouchers.GetRow.Cells("Voucher_Id").Value.ToString))
            ShowReport("rptVoucherUpdated")
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub btnHistoryLoadAll_Click1(sender As Object, e As EventArgs) Handles btnHistoryLoadAll.Click
        Me.Cursor = Cursors.WaitCursor
        Try
            Me.PopulateGrid("All")
            DisplayDetail(-1)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub btnBackToOldReciept_Click(sender As Object, e As EventArgs) Handles btnBackToOldReciept.Click



        'Me.Hide()
        'frmOldCustomerCollection.Show()
        Try
            SaveConfiguration("NewReceiptForm", "False")
            frmMain.LoadControl("frmOldCustomerCollection")
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub btnHistoryPrint_ButtonClick(sender As Object, e As EventArgs) Handles btnHistoryPrint.ButtonClick
        Try
            If Me.grdVouchers.RowCount = 0 Then Exit Sub
            'PrintLog = New SBModel.PrintLogBE
            'PrintLog.DocumentNo = grdVouchers.GetRow.Cells("voucher_no").Value.ToString
            'PrintLog.UserName = LoginUserName
            'PrintLog.PrintDateTime = Date.Now
            'Call SBDal.PrintLogDAL.PrintLog(PrintLog)
            ''ShowReport("rptVoucher", "{VwGlVoucherSingle.VoucherId}=" & Me.grdVouchers.CurrentRow.Cells(0).Value & "")
            ''Changing Against Request No 798
            'AddRptParam("@VoucherId", Me.grdVouchers.CurrentRow.Cells(0).Value)
            'ShowReport("rptVoucher")
            PrintVoucherBC(Me.grdVouchers.GetRow.Cells("voucher_id").Value, Me.grdVouchers.GetRow.Cells("Voucher_No").Value.ToString())

        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub grdVouchers_FormattingRow(sender As Object, e As Janus.Windows.GridEX.RowLoadEventArgs)

    End Sub

    Private Sub btnSearchDelete_Click1(sender As Object, e As EventArgs)
        Try
            DeleteToolStripButton_Click(Nothing, Nothing)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PrintSelectedVouchersToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles PrintSelectedVouchersToolStripMenuItem.Click
        ' Change on 23-11-2013  For Multiple Print Vouchers
        Me.Cursor = Cursors.WaitCursor
        Try
            If Me.grdVouchers.RowCount = 0 Then Exit Sub
            'If Me.grdVouchers.GetCheckedRows = False Then Exit Sub
            Dim cont As Integer = Me.grdVouchers.GetCheckedRows.Length
            If cont > 0 Then
                For Each r As Janus.Windows.GridEX.GridEXRow In Me.grdVouchers.GetCheckedRows
                    'AddRptParam("@vid", r.Cells(0).Value)
                    'ShowReport("rptCashReceipt", , , , True)
                    'PrintLog = New SBModel.PrintLogBE
                    'PrintLog.DocumentNo = r.Cells("Voucher_No").Value.ToString
                    'PrintLog.UserName = LoginUserName
                    'PrintLog.PrintDateTime = Date.Now
                    'Call SBDal.PrintLogDAL.PrintLog(PrintLog)
                    PrintVoucherBC(r.Cells("voucher_id").Value, r.Cells("Voucher_No").Value.ToString())
                Next
            Else
                PrintVoucherBC(Me.grdVouchers.GetRow.Cells("voucher_id").Value, Me.grdVouchers.GetRow.Cells("Voucher_No").Value.ToString())
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnHisotryEdit_Click_1(sender As Object, e As EventArgs) Handles btnHisotryEdit.Click
        Try
            If grdVouchers.CurrentRow Is Nothing Then
                ShowErrorMessage("Please select a row to delete")
            Else
                blnEditMode = True
                lngSelectedVoucherId = CLng(grdVouchers.CurrentRow.Cells(0).Value)
                EditRecord()
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Ali Faisal : TFS3677 : Exception handling on Voucher entry, Payment and Receipt screen.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PrintReceiptToolStripMenuItem1_Click_1(sender As Object, e As EventArgs) Handles PrintReceiptToolStripMenuItem1.Click
        Me.Cursor = Cursors.WaitCursor
        Try
            If Me.grdVouchers.RowCount = 0 Then Exit Sub

            PrintLog = New SBModel.PrintLogBE
            PrintLog.DocumentNo = grdVouchers.GetRow.Cells("Voucher_No").Value.ToString
            PrintLog.UserName = LoginUserName
            PrintLog.PrintDateTime = Date.Now
            Call SBDal.PrintLogDAL.PrintLog(PrintLog)
            'ShowReport("rptVoucher", "{VwGlVoucherSingle.VoucherId}=" & Me.grdVouchers.CurrentRow.Cells(0).Value & "")
            'Chaning Against Request No 798
            AddRptParam("@vid", Me.grdVouchers.CurrentRow.Cells(0).Value)
            ShowReport("rptCashReceipt")
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub PrintAttachmentVoucherToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles PrintAttachmentVoucherToolStripMenuItem1.Click
        Try
            If Me.grdVouchers.RowCount = 0 Then Exit Sub
            'AddRptParam("Pm-dtVoucher.Voucher_Id", Me.grdVouchers.GetRow.Cells(0).Value)
            DataSetShowReport("RptVoucherDocument", GetVoucherRecord())
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub btnHistorySearch_Click_1(sender As Object, e As EventArgs) Handles btnHistorySearch.Click
        Try


            If Not Me.cmbSearchAccount.IsItemInList Then
                FillCombo("SearchCustomer")
                Me.cmbSearchAccount.Rows(0).Activate()
            Else
                Me.cmbSearchAccount.Rows(0).Activate()
            End If
            If Me.SplitContainer1.Panel1Collapsed = True Then
                Me.SplitContainer1.Panel1Collapsed = False
            Else
                Me.SplitContainer1.Panel1Collapsed = True
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub btnReminder1_Click_1(sender As Object, e As EventArgs) Handles btnReminder1.Click
        Try
            btnReminder_Click(Nothing, Nothing)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub Btn_SaveAttachment_Click_1(sender As Object, e As EventArgs) Handles Btn_SaveAttachment.Click

        If Con.State = ConnectionState.Closed Then Con.Open()
        Dim trans As OleDbTransaction = Con.BeginTransaction

        Try
            Dim intCountAttachedFiles As Integer = 0I
            OpenFileDialog1.FileName = String.Empty

            OpenFileDialog1.Filter = "Word Documents|*.doc|Excel Worksheets|*.xls|Portable Document Format|*.pdf|Corel Draw Files|*.cdr|All Images|*.BMP;*.DIB;*.RLE;*.JPG;*.JPEG;*.JPE;*.JFIF;*.GIF;*.TIF;*.TIFF;*.PNG|" + _
                        "All files (*.*)|*.*"

            If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then

                Dim a As Integer = 0I
                For a = 0 To OpenFileDialog1.FileNames.Length - 1
                    arrFile.Add(OpenFileDialog1.FileNames(a))
                Next a

                If arrFile.Count > 0 Then
                    SaveDocument(Me.grdVouchers.CurrentRow.Cells(0).Value, Me.Name, trans)
                    trans.Commit()
                    PopulateGrid()

                End If
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub btnGetAllRecord_Click_1(sender As Object, e As EventArgs) Handles btnGetAllRecord.Click
        Try
            GetAllUsersRecord()
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub grdVouchers_DoubleClick_1(sender As Object, e As EventArgs) Handles grdVouchers.DoubleClick
        Try
            If Me.grdVouchers.Row < 0 Then
                Exit Sub
            Else
                blnEditMode = True
                lngSelectedVoucherId = CLng(grdVouchers.CurrentRow.Cells(0).Value)
                EditRecord()
                'Hide Buttons Edit,Delete and Print on Load Form
                Me.BtnDelete.Visible = True
                Me.btnCancel.Visible = True

                Me.UltraTabControl1.SelectedTab = Me.UltraTabControl1.Tabs(0).TabPage.Tab
                'EditRecord()
                'Me.UltraTabControl1.SelectedTab = Me.UltraTabControl1.Tabs(0).TabPage.Tab
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
        ''End Task#A1 12-06-2015
    End Sub

    Private Sub grd_CellEdited_1(sender As Object, e As Janus.Windows.GridEX.ColumnActionEventArgs)
        Try
            Me.grd.GetRow.Cells(grdEnm.Amount).Value = Me.grd.GetRow.Cells(grdEnm.CurrencyAmount).Value * Me.grd.GetRow.Cells(grdEnm.CurrencyRate).Value

        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub grd_ColumnButtonClick_1(sender As Object, e As Janus.Windows.GridEX.ColumnActionEventArgs)
        Try
            If Not msg_Confirm(str_ConfirmDelete) = True Then Exit Sub
            If e.Column.Key = "Delete" Then
                If Not Val(Me.grd.GetRow.Cells("VoucherDetailId").Value.ToString) > 0 Then
                    Me.grd.GetRow.Delete()
                    grd.UpdateData()
                    If Me.grd.RowCount = 0 Then
                        Me.cmbCurrency.Enabled = True
                    End If
                Else
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub grd_KeyDown_1(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.F2 Then
            OpenToolStripButton_Click(Me.BtnEdit, Nothing)
            Exit Sub
        End If

        'If e.KeyCode = Keys.Delete Then
        '    DeleteToolStripButton_Click(BtnDelete, Nothing)
        '    Exit Sub
        If e.KeyCode = Keys.PageDown Then
            grdVouchers_DoubleClick(Nothing, Nothing)
        End If
        'End If
    End Sub

    Private Sub CtrlGrdBar4_Load(sender As Object, e As EventArgs) Handles CtrlGrdBar4.Load
        Try
            If IO.File.Exists(Application.ExecutablePath & "\..\Layouts\" & Me.Name & "_" & Me.grd.Name) Then
                Dim fs As New IO.FileStream(Application.ExecutablePath & "\..\Layouts\" & Me.Name & "_" & Me.grd.Name, IO.FileMode.Open, IO.FileAccess.ReadWrite)
                Me.grd.LoadLayoutFile(fs)
                fs.Close()
                fs.Dispose()
            End If
            Me.CtrlGrdBar4.txtGridTitle.Text = CompanyTitle & Chr(10) & " Receipts "

            'CtrlGrdBar.strForm_Name = CtrlGrdBar.EnmAccountType.Vendors
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub CtrlGrdBar10_Load(sender As Object, e As EventArgs) Handles CtrlGrdBar10.Load
        Try
            If IO.File.Exists(Application.ExecutablePath & "\..\Layouts\" & Me.Name & "_" & Me.grdVouchers.Name) Then
                Dim fs As New IO.FileStream(Application.ExecutablePath & "\..\Layouts\" & Me.Name & "_" & Me.grdVouchers.Name, IO.FileMode.Open, IO.FileAccess.ReadWrite)
                Me.grdVouchers.LoadLayoutFile(fs)
                fs.Close()
                fs.Dispose()
            End If
            Me.CtrlGrdBar10.txtGridTitle.Text = CompanyTitle & Chr(10) & " Reciept Detail "

            'CtrlGrdBar.strForm_Name = CtrlGrdBar.EnmAccountType.Vendors
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub



    Private Sub cmbIncomeTaxAccount_SelectedValueChanged(sender As Object, e As EventArgs) Handles cmbIncomeTaxAccount.SelectedValueChanged
        Try
            If Me.cmbIncomeTaxAccount.SelectedIndex > 0 Then
                Try
                    Dim dt As DataTable = GetDataTable("Select IncomeTaxPer From tblDefIncomeTaxAccount Where IncomeTaxAccId = " & Me.cmbIncomeTaxAccount.SelectedValue & "")
                    If dt.Rows.Count > 0 Then
                        Me.txtTax.Text = dt.Rows(0).Item(0).ToString()
                    End If
                    If Val(Me.txtTax.Text) < 0 Then
                        Me.txtTax.Text = Me.txtTax.Text
                    End If
                Catch ex As Exception
                    Me.txtTax.Text = 0
                End Try
            Else
                Me.txtTax.Text = ""
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)

        End Try
    End Sub

    Private Sub frmCustomerCollection_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            FillCombo("IncomeTaxAccount")
            FillCombo("SalesTaxAccount")
            'FillCombo("CostCenter")
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    '#task1608 added by zainab
    Private Sub txtSalesTax_TextChanged(sender As Object, e As EventArgs) Handles txtSalesTax.TextChanged
        Try

            'If CheckNumericValue(Me.txtSalesTax.Text, Me.txtSalesTax) = False Then
            '    Throw New Exception("Amount is not valid.")
            'End If
            ' ''Below lines are commented on 13-12-2017 against TASK TFS1926
            ''Me.txtSalesTaxAmount.Text = Math.Round((Val(Me.txtGrossAmount.Text) * (Val(Me.txtSalesTax.Text) / 100)), DecimalPointInValue)
            ' '' TASK TFS1926. Below lines are modified as per Ahmad's advice that gross amount should be divided by sales tax % + 100 then be divided by sales tax %. done by Ameen on 13-12-2017
            'Me.txtSalesTaxAmount.Text = Math.Round((Val(Me.txtGrossAmount.Text) / (Val(Me.txtSalesTax.Text) + 100) * Val(Me.txtSalesTax.Text)), TotalAmountRounding)
            ''rafay txtSalesTaxAmount.Text = Format(Val(txtSalesTaxAmount.Text), "0")
            'Me.txtWithHoldingTaxAmount.Text = Math.Round(((Val(Me.txtSalesTaxAmount.Text) * Val(Me.txtWithHolding.Text)) / 100), TotalAmountRounding)
            ''rafay txtWithHoldingTaxAmount.Text = Format(Val(txtWithHoldingTaxAmount.Text), "0")
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    '#task1608 added by zainab
    Private Sub txtWithHolding_TextChanged(sender As Object, e As EventArgs) Handles txtWithHolding.TextChanged
        Try
            If CheckNumericValue(Me.txtWithHolding.Text, Me.txtWithHolding) = False Then
                Throw New Exception("Amount is not valid.")
            End If
            'Waqar Raza commented this line because it change the value of sales tax when we enter WHT at TUV premisses
            'Me.txtSalesTaxAmount.Text = Math.Round((Val(Me.txtGrossAmount.Text) * (Val(Me.txtSalesTax.Text) / 100)), TotalAmountRounding)
            Me.txtWithHoldingTaxAmount.Text = Math.Round(((Val(Me.txtSalesTaxAmount.Text) * Val(Me.txtWithHolding.Text)) / 100), TotalAmountRounding)
            'rafay txtWithHoldingTaxAmount.Text = Format(Val(txtWithHoldingTaxAmount.Text), "0")
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    Private Sub txtGrossAmount_TextChanged(sender As Object, e As EventArgs) Handles txtGrossAmount.TextChanged
        Try
            If CheckNumericValue(Me.txtGrossAmount.Text, Me.txtGrossAmount) = False Then
                Throw New Exception("Amount is not valid.")
            End If
            Me.txtNetAmount.Text = Math.Round(Val(Me.txtGrossAmount.Text) - Val(Me.txtTaxAmount.Text) - Val(Me.txtWithHoldingTaxAmount.Text), TotalAmountRounding)
            Me.txtTaxAmount.Text = Math.Round((Val(Me.txtGrossAmount.Text) * (Val(Me.txtTax.Text) / 100)), TotalAmountRounding)

            ''Below lines are commented on 13-12-2017 AGAINST TASK TFS1926
            'Me.txtSalesTaxAmount.Text = Math.Round((Val(Me.txtGrossAmount.Text) * (Val(Me.txtSalesTax.Text) / 100)), DecimalPointInValue)
            ''TASK TFS1926. Below lines are modified as per Ahmad's advice that gross amount should be divided by sales tax % + 100 then be divided by sales tax %. done by Ameen on 13-12-2017
            Me.txtSalesTaxAmount.Text = Math.Round((Val(Me.txtGrossAmount.Text) / (Val(Me.txtSalesTax.Text) + 100) * Val(Me.txtSalesTax.Text)), TotalAmountRounding)
            Me.txtWithHoldingTaxAmount.Text = Math.Round(((Val(Me.txtSalesTaxAmount.Text) * Val(Me.txtWithHolding.Text)) / 100), TotalAmountRounding)
            'rafay txtNetAmount.Text = Format(Val(txtNetAmount.Text), "0")
            'rafay txtTaxAmount.Text = Format(Val(txtTaxAmount.Text), "0")
            'rafay txtSalesTaxAmount.Text = Format(Val(txtSalesTaxAmount.Text), "0")
            'rafay txtWithHoldingTaxAmount.Text = Format(Val(txtWithHoldingTaxAmount.Text), "0")

        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    '#task1608 added by zainab
    Private Sub txtTaxAmount_TextChanged(sender As Object, e As EventArgs) Handles txtTaxAmount.TextChanged
        Try
            'addd comma sperators on all numeric text
            'txtTaxAmount.Text = CDbl(txtTaxAmount.Text).ToString("N")
            'txtTaxAmount.SelectionStart = txtTaxAmount.Text.IndexOf(".")

            If CheckNumericValue(Me.txtTaxAmount.Text, Me.txtTaxAmount) = False Then
                Throw New Exception("Amount is not valid.")
            End If
            Me.txtNetAmount.Text = Math.Round(Val(Me.txtGrossAmount.Text) - Val(Me.txtTaxAmount.Text) - Val(Me.txtWithHoldingTaxAmount.Text), TotalAmountRounding)
            Me.txtTax.Text = Math.Round(Val(Me.txtTaxAmount.Text) / Val(Me.txtGrossAmount.Text) * 100, TotalAmountRounding)
            'rafay txtNetAmount.Text = Format(Val(txtNetAmount.Text), "0")
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    '#task1608 added by zainab

    Private Sub txtWithHoldingTaxAmount_TextChanged(sender As Object, e As EventArgs) Handles txtWithHoldingTaxAmount.TextChanged
        Try
            'addd comma sperators on all numeric text
            'txtWithHoldingTaxAmount.Text = CDbl(txtWithHoldingTaxAmount.Text).ToString("N")
            'txtWithHoldingTaxAmount.SelectionStart = txtWithHoldingTaxAmount.Text.IndexOf(".")

            If CheckNumericValue(Me.txtWithHoldingTaxAmount.Text, Me.txtWithHoldingTaxAmount) = False Then
                Throw New Exception("Amount is not valid.")
            End If
            Me.txtNetAmount.Text = Math.Round(Val(Me.txtGrossAmount.Text) - Val(Me.txtTaxAmount.Text) - Val(Me.txtWithHoldingTaxAmount.Text), TotalAmountRounding)
            'rafay txtNetAmount.Text = Format(Val(txtNetAmount.Text), "0")
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    '#task1608 added by zainab
    Private Sub cmbSalesTax_SelectedValueChanged(sender As Object, e As EventArgs) Handles cmbSalesTax.SelectedValueChanged
        Try
            If Me.cmbSalesTax.SelectedIndex > 0 Then
                Try
                    Dim dt As DataTable = GetDataTable("Select SalesTaxPer,WHTaxPer From tblDefSalesTaxAccount Where SalesTaxAccId = " & Me.cmbSalesTax.SelectedValue & "")
                    If dt.Rows.Count > 0 Then
                        Me.txtSalesTax.Text = dt.Rows(0).Item(0).ToString()
                        Me.txtWithHolding.Text = dt.Rows(0).Item(1).ToString()

                    End If
                    If Val(Me.txtSalesTax.Text) < 0 Then
                        Me.txtSalesTax.Text = Me.txtSalesTax.Text
                        If Val(Me.txtWithHolding.Text) < 0 Then
                            Me.txtWithHolding.Text = Me.txtWithHolding.Text
                        Else
                            Me.txtWithHolding.Text = ""
                        End If
                    End If
                Catch ex As Exception
                    Me.txtSalesTax.Text = 0
                    Me.txtWithHolding.Text = 0
                End Try
            Else
                Me.txtSalesTax.Text = ""

            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)

        End Try

    End Sub
    '#task1608 to save the used screen configuration
    Public Function SaveConfiguration(ByVal KeyType As String, ByVal KeyValue As String, Optional ByVal CreateNewKey As Boolean = False) As Boolean
        Dim Con As New SqlClient.SqlConnection(SQLHelper.CON_STR)
        If Con.State = ConnectionState.Closed Then Con.Open()
        Dim trans As SqlTransaction = Con.BeginTransaction
        Dim Cmd As New SqlCommand
        Cmd.Connection = Con
        Cmd.Transaction = trans
        Try
            Dim strSQL As String = String.Empty

            If CreateNewKey = True Then

                strSQL = "IF NOT EXISTS(SELECT * From ConfigValuesTable  WHERE Config_Type='" & KeyType & "' ) " _
                        & " Insert into ConfigValuesTable(config_id, config_Type, config_Value, Comments, IsActive) Select Max(config_id) + 1, '" & KeyType & "', '" & KeyValue & "', '', 1 from ConfigValuesTable " _
                        & "Else " _
                        & " UPDATE ConfigValuesTable SET Config_Value=N'" & KeyValue & "' WHERE Config_Type=N'" & KeyType & "'"

            Else

                strSQL = "UPDATE ConfigValuesTable SET Config_Value=N'" & KeyValue & "' WHERE Config_Type=N'" & KeyType & "'"

            End If

            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = strSQL
            Cmd.ExecuteNonQuery()
            trans.Commit()

            SaveActivityLog("Configuration", Me.Text.ToString, EnumActions.Update, LoginUserId, EnumRecordType.Configuration, KeyType, , , , , Me.Name.ToString)
            Key = KeyType
            Dim config As ConfigSystem = objConfigValueList.Find(AddressOf GetObj)

            objConfigValueList.Remove(config)
            Dim AddConfig As New ConfigSystem
            AddConfig.Config_Type = KeyType.ToString
            AddConfig.Config_Value = KeyValue.ToString
            If config IsNot Nothing Then
                If config.Comments IsNot Nothing Then
                    AddConfig.Comments = config.Comments
                Else
                    AddConfig.Comments = Nothing
                End If
                AddConfig.IsActive = config.IsActive
            End If

            objConfigValueList.Add(AddConfig)
            Key = String.Empty
            Return True
        Catch ex As Exception
            trans.Rollback()
            Throw ex
        Finally
            Con.Close()
        End Try
    End Function
    Public Function GetObj(ByVal Config As ConfigSystem) As Boolean
        Try
            If Config.Config_Type = Key Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Private Sub txtSalesTaxAmount_TextChanged(sender As Object, e As EventArgs) Handles txtSalesTaxAmount.TextChanged
        Try
            If CheckNumericValue(Me.txtSalesTaxAmount.Text, Me.txtSalesTaxAmount) = False Then
                Throw New Exception("Amount is not valid.")
            End If
            txtSalesTax.Text = (Val(txtSalesTaxAmount.Text) / (Val(txtGrossAmount.Text))) * 100
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub txtNetAmount_TextChanged(sender As Object, e As EventArgs) Handles txtNetAmount.TextChanged
        Try
            ''addd comma sperators on all numeric text
            'txtNetAmount.Text = CDbl(txtNetAmount.Text).ToString("N")
            'txtNetAmount.SelectionStart = txtNetAmount.Text.IndexOf(".")

            'start task 3217: Abubakar Siddiq
            If Me.txtNetAmount.Text = "" Then
                lblNetAmountNumberConvertor.Text = ""
            Else
                lblNetAmountNumberConvertor.Text = ModGlobel.NumberToWords(Me.txtNetAmount.Text)
            End If



            If CheckNumericValue(Me.txtNetAmount.Text, Me.txtNetAmount) = False Then
                Throw New Exception("Amount is not valid.")
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub


    Private Sub TaxConfiguration_Click(sender As Object, e As EventArgs) Handles TaxConfiguration.Click
        Try
            'frmMain.LoadControl("frmIncomeTaxOrSalesTaxAccount")
            frmIncomeTaxOrSalesTaxAccount.formType = "Reciept"
            frmIncomeTaxOrSalesTaxAccount.ShowDialog()
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub


    Private Sub txtReference_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtReference.Validating
        Try
            SpellChecker(Me.txtReference)
            If Me.txtMemo.Text.Trim.ToString.Length = 0 Then
                Me.txtMemo.Text = txtReference.Text
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub cmbBank_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cmbBank.Validating

    End Sub

    Private Sub txtMemo_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtMemo.Validating
        Try
            SpellChecker(Me.txtMemo)
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Ayesha Rehman : TFS2701 : Show Approval History of the current Document
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnApprovalHistory_Click(sender As Object, e As EventArgs) Handles btnApprovalHistory.Click
        Try
            frmApprovalHistory.DocumentNo = Me.txtVoucherNo.Text
            frmApprovalHistory.Source = Me.Name
            frmApprovalHistory.ShowDialog()
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub


    Private Sub btnSalesInvoice_Click(sender As Object, e As EventArgs) Handles btnSalesInvoice.Click
        Try
            flgSalesInvoice = True
            frmInvoiceSearch.ShowDialog()
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    'Private Sub cmbInvoice_ValueChanged(sender As Object, e As EventArgs) Handles cmbInvoice.ValueChanged
    '    Try
    '        If flgSalesInvoice = True Then
    '            Dim str As String
    '            Dim dt As DataTable
    '            str = "SELECT ArticleDefView.ArticleDescription, SalesDetailTable.Qty, SalesDetailTable.Price, SalesDetailTable.DiscountValue, SalesDetailTable.TaxPercent, SalesDetailTable.SEDPercent FROM SalesDetailTable INNER JOIN ArticleDefView ON SalesDetailTable.ArticleDefId = ArticleDefView.ArticleId Where SalesId = " & cmbInvoice.Value & ""
    '            dt = GetDataTable(str)
    '            Me.grdItems.DataSource = dt
    '            grdItems.RetrieveStructure()
    '            txtAmount.Text = Val(Me.grdItems.GetTotal(Me.grdItems.RootTable.Columns("Price"), Janus.Windows.GridEX.AggregateFunction.Sum)) * Val(Me.grdItems.GetTotal(Me.grdItems.RootTable.Columns("Qty"), Janus.Windows.GridEX.AggregateFunction.Sum))
    '            txtDiscount.Text = Val(Me.grdItems.GetTotal(Me.grdItems.RootTable.Columns("DiscountValue"), Janus.Windows.GridEX.AggregateFunction.Sum))
    '            txtSalesTax.Text = Val(Me.grdItems.GetTotal(Me.grdItems.RootTable.Columns("TaxPercent"), Janus.Windows.GridEX.AggregateFunction.Sum)) + Val(Me.grdItems.GetTotal(Me.grdItems.RootTable.Columns("SEDPercent"), Janus.Windows.GridEX.AggregateFunction.Sum)) / grdItems.RowCount
    '        End If
    '    Catch ex As Exception
    '        ShowErrorMessage(ex.Message)
    '    End Try
    'End Sub

    Private Sub cmbAccounts_ValueChanged(sender As Object, e As EventArgs) Handles cmbAccounts.ValueChanged
        Try
            If flgSalesInvoice = True Then
                Dim str As String
                str = "select SalesId, SalesNo from SalesMasterTable where CustomerCode = " & cmbAccounts.Value & "AND SalesmasterTable.SalesId not in (select isnull(InvoiceId,0) from tblVoucherDetail)"
                FillUltraDropDown(cmbInvoice, str)

            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Public Sub fillInvoiceGrid(ByVal dt As DataTable, ByVal TicketId As Integer, ByVal PlanId As Integer)
        Dim _dt As DataTable
        Try
            'cmbPlan.Value = PlanId
            'cmbTicket.Value = TicketId
            _dt = CType(Me.grd.DataSource, DataTable).Clone()
            _dt.Merge(dt)
            Me.grd.DataSource = _dt
            Me.grd.UpdateData()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub grd_ColumnHeaderClick(sender As Object, e As Janus.Windows.GridEX.ColumnActionEventArgs) Handles grd.ColumnHeaderClick
        Try
            If Me.grd.RowCount = 0 Then Exit Sub
            Dim i As Integer = 0
            Dim myMarks As Double = 0D
            If e.Column.Key = "SalesTaxPer" Then
                For Each row As DataRow In CType(Me.grd.DataSource, DataTable).Rows
                    row.BeginEdit()
                    If i = 0 Then
                        myMarks = row("SalesTaxPer")
                    Else
                        row("SalesTaxPer") = myMarks
                    End If
                    row.EndEdit()
                    i += 1
                Next
                i = 0
            ElseIf e.Column.Key = "WHTaxPer" Then
                For Each row As DataRow In CType(Me.grd.DataSource, DataTable).Rows
                    row.BeginEdit()
                    If i = 0 Then
                        myMarks = row("WHTaxPer")
                    Else
                        row("WHTaxPer") = myMarks
                    End If
                    row.EndEdit()
                    i += 1
                Next
                i = 0
            ElseIf e.Column.Key = "Tax" Then
                For Each row As DataRow In CType(Me.grd.DataSource, DataTable).Rows
                    row.BeginEdit()
                    If i = 0 Then
                        myMarks = row("Tax")
                    Else
                        row("Tax") = myMarks
                    End If
                    row.EndEdit()
                    i += 1
                Next
                i = 0
            ElseIf e.Column.Key = "AccountId" Then
                For Each row As DataRow In CType(Me.grd.DataSource, DataTable).Rows
                    row.BeginEdit()
                    If i = 0 Then
                        myMarks = row("AccountId")
                    Else
                        row("AccountId") = myMarks
                    End If
                    row.EndEdit()
                    i += 1
                Next
            ElseIf e.Column.Key = "IncomeAccountId" Then
                For Each row As DataRow In CType(Me.grd.DataSource, DataTable).Rows
                    row.BeginEdit()
                    If i = 0 Then
                        myMarks = row("IncomeAccountId")
                    Else
                        row("IncomeAccountId") = myMarks
                    End If
                    row.EndEdit()
                    i += 1
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub grd_DoubleClick(sender As Object, e As EventArgs) Handles grd.DoubleClick
        Try
            If Me.grd.RowCount = 0 Then Exit Sub
            Row_Index = Me.grd.CurrentRow.RowIndex
            Me.cmbAccounts.Value = Me.grd.GetRow.Cells(grdEnm.coa_detail_id).Value
            Me.txtAmount.Text = Me.grd.GetRow.Cells(grdEnm.CurrencyAmount).Value
            Me.txtDiscount.Text = Me.grd.GetRow.Cells(grdEnm.CurrencyDiscount).Value
            Me.cmbIncomeTaxAccount.Text = Me.grd.GetRow.Cells(grdEnm.IncomeAccountId).Text.ToString
            Me.txtTax.Text = Me.grd.GetRow.Cells(grdEnm.Tax).Value
            Me.txtTaxAmount.Text = Format((Me.grd.GetRow.Cells(grdEnm.TaxAmount).Value), "0.00")
            Me.cmbSalesTax.Text = Me.grd.GetRow.Cells(grdEnm.AccountId).Text.ToString
            'Me.txtDebit.Text = Val(Me.grd.GetRow.Cells("Debit").Value)
            'Me.txtCredit.Text = Val(Me.grd.GetRow.Cells("Credit").Value)
            Me.txtSalesTax.Text = Val(Me.grd.GetRow.Cells(grdEnm.SalesTaxPer).Value)
            Me.txtSalesTaxAmount.Text = Format(Val(Me.grd.GetRow.Cells(grdEnm.Sales_Tax).Value), "0.00")
            Me.txtWithHolding.Text = Val(Me.grd.GetRow.Cells(grdEnm.WHTaxPer).Value)
            Me.txtWithHoldingTaxAmount.Text = Format(Val(Me.grd.GetRow.Cells(grdEnm.WHTaxAmount).Value), "0.00")
            Me.txtNetAmount.Text = Format(Val(Me.grd.GetRow.Cells(grdEnm.NetAmount).Value), "0.00")
            If (Me.grd.GetRow.Cells(grdEnm.Cheque_No).Text.ToString()) = "" Then
                pnlBank.Visible = False
            Else
                pnlBank.Visible = True
                txtChequeNo.Text = Me.grd.GetRow.Cells(grdEnm.Cheque_No).Text.ToString()
                dtChequeDate.Value = Me.grd.GetRow.Cells(grdEnm.Cheque_Date).Value
                cmbBank.SelectedText = Me.grd.GetRow.Cells(grdEnm.BankDescription).Text.ToString()
            End If
            Me.txtWithHolding.Text = Val(Me.grd.GetRow.Cells(grdEnm.WHTaxPer).Value)
            Me.txtWithHoldingTaxAmount.Text = Format(Val(Me.grd.GetRow.Cells(grdEnm.WHTaxAmount).Value), "0.00")
            Me.cmbCostCenter.Text = Me.grd.GetRow.Cells(grdEnm.CostCenterId).Text.ToString()
            Me.txtReference.Text = Me.grd.GetRow.Cells(grdEnm.Reference).Text.ToString()
            Me.cmbSaleman.SelectedValue = Me.grd.GetRow.Cells(grdEnm.Employee).Value
            Dim rowCol As New Janus.Windows.GridEX.GridEXFormatStyle
            rowCol.BackColor = Color.AntiqueWhite
            Me.grd.CurrentRow.RowStyle = rowCol

            Me.grd.Enabled = False
            Me.btnAdd.Visible = False
            Me.btnReplace.Visible = True
            Me.btnCancel.Visible = True
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub btnReplace_Click(sender As Object, e As EventArgs) Handles btnReplace.Click
        Try
            Dim dtGrd As DataTable = CType(Me.grd.DataSource, DataTable)
            dtGrd.Rows.RemoveAt(Row_Index)
            dtGrd.AcceptChanges()
            Dim dr As DataRow
            dr = dtGrd.NewRow

            dr.Item(grdEnm.coa_detail_id) = Me.cmbAccounts.ActiveRow.Cells(0).Text
            dr.Item(grdEnm.detail_title) = Me.cmbAccounts.ActiveRow.Cells(1).Text
            dr.Item(grdEnm.detail_code) = Me.cmbAccounts.ActiveRow.Cells(2).Text
            dr.Item(grdEnm.CurrencyId) = Me.cmbCurrency.SelectedValue
            dr.Item(grdEnm.CurrencyAmount) = Val(Me.txtAmount.Text)
            dr.Item(grdEnm.CurrencyRate) = Val(txtCurrencyRate.Text)
            Dim ConfigCurrencyVal As String = getConfigValueByType("Currency").ToString
            If ConfigCurrencyVal.Length > 0 AndAlso Not ConfigCurrencyVal.ToString.ToUpper = "ERROR" Then
                dr.Item(grdEnm.BaseCurrencyId) = Val(ConfigCurrencyVal)
                dr.Item(grdEnm.BaseCurrencyRate) = Val(GetCurrencyRate(Val(ConfigCurrencyVal)))
            End If
            dr.Item(grdEnm.Amount) = Val(Me.txtAmount.Text) * Val(txtCurrencyRate.Text)
            dr.Item(grdEnm.CurrencyDiscount) = Val(Me.txtDiscount.Text)
            dr.Item(grdEnm.Tax) = Val(Me.txtTax.Text)
            dr.Item(grdEnm.TaxAmount) = Format(Math.Round(Val(Me.txtTaxAmount.Text), TotalAmountRounding), "0.00")
            dr.Item(grdEnm.SalesTaxPer) = Val(Me.txtSalesTax.Text)
            dr.Item(grdEnm.Sales_Tax) = Format(Math.Round(Val(Me.txtSalesTaxAmount.Text), TotalAmountRounding), "0.00")
            dr.Item(grdEnm.WHTaxPer) = Val(Me.txtWithHolding.Text)
            dr.Item(grdEnm.WHTaxAmount) = Format((Me.txtWithHoldingTaxAmount.Text), "0.00")
            dr.Item(grdEnm.NetAmount) = Format(Val(Me.txtNetAmount.Text), "0.00")
            dr.Item(grdEnm.AccountId) = Val(CType(Me.cmbSalesTax.SelectedItem, DataRowView).Row.Item("AccountId").ToString)
            dr.Item(grdEnm.IncomeAccountId) = Val(CType(Me.cmbIncomeTaxAccount.SelectedItem, DataRowView).Row.Item("AccountId").ToString)
            dr.Item(grdEnm.Reference) = Me.txtReference.Text.Trim.ToString.Replace("'", "''")
            dr.Item(grdEnm.Cheque_No) = IIf(Me.pnlBank.Visible = True, Me.txtChequeNo.Text, DBNull.Value)
            dr.Item(grdEnm.Cheque_Date) = IIf(Me.pnlBank.Visible = True, Me.dtChequeDate.Value, DBNull.Value)
            dr.Item(grdEnm.BankDescription) = IIf(Me.pnlBank.Visible = True, Me.cmbBank.Text, DBNull.Value)
            dr.Item(grdEnm.Type) = Me.cmbAccounts.ActiveRow.Cells("Type").Value.ToString
            dr.Item(grdEnm.CostCenterId) = Me.cmbCostCenter.SelectedValue
            dr.Item(grdEnm.Employee) = Me.cmbSaleman.SelectedValue
            dr.Item(grdEnm.Phone) = Me.cmbAccounts.ActiveRow.Cells("Mobile").Value.ToString
            dtGrd.Rows.InsertAt(dr, Row_Index)
            SelectedMode = True
            Me.ClearFields("Detail")
            If Me.cmbCashAccount.SelectedIndex > 0 Then
                Me.cmbVoucherType.Enabled = False
                Me.cmbCashAccount.Enabled = False
            End If
            Me.cmbCurrency.Enabled = False
            Me.grd.Enabled = True
            Me.btnAdd.Visible = True
            Me.btnCancel.Visible = False
            Me.btnReplace.Visible = False
            cmbAccounts.Focus()
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.ClearFields("Detail")
        If Me.cmbCashAccount.SelectedIndex > 0 Then
            Me.cmbVoucherType.Enabled = False
            Me.cmbCashAccount.Enabled = False
        End If
        Me.cmbCurrency.Enabled = False
        Me.grd.Enabled = True
        Me.btnAdd.Visible = True
        Me.btnCancel.Visible = False
        Me.btnReplace.Visible = False
        cmbAccounts.Focus()
    End Sub

    Private Sub chkAll_CheckedChanged1(sender As Object, e As EventArgs) Handles chkAll.CheckedChanged
        Try
            FillCombo("Customer")
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' TASK TFS4915 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPreviewAttachment_Click(sender As Object, e As EventArgs) Handles btnPreviewAttachment.Click
        Try
            If grdVouchers.RowCount = 0 Then Exit Sub
            Dim frm As New frmAttachmentView
            frm._Source = Me.Name
            frm._VoucherId = Me.grdVouchers.GetRow.Cells(0).Value
            frm.ShowDialog()
            Exit Sub
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    'Change by Murtaza for txtcurrencyrate text change (11/09/2022) 
    Private Sub txtCurrencyRate_TextChanged(sender As Object, e As EventArgs) Handles txtCurrencyRate.TextChanged
        Try
            If Not Me.cmbCurrency.SelectedItem Is Nothing Then
                Dim dr As DataRowView = CType(cmbCurrency.SelectedItem, DataRowView)
                If Me.cmbCurrency.SelectedValue = BaseCurrencyId Then
                    Me.txtCurrencyRate.Enabled = False
                Else
                    Me.txtCurrencyRate.Enabled = True

                End If
                If Val(Me.txtCurrencyRate.Text) = 0 Then
                    Me.txtCurrencyRate.Text = 1
                End If

                'Me.grd.RootTable.Columns("CurrencyAmount").Caption = "Amount (" & Me.cmbCurrency.Text & ")"
                Me.grd.RootTable.Columns(grdEnm.CurrencyRate).Caption = "Currency Rate (" & Me.cmbCurrency.Text & ")"
                'Me.grd.RootTable.Columns(EnumGridDetail.CurrencyTaxAmount).Caption = "Tax Amount (" & Me.cmbCurrency.Text & ")"

                'grd.AutoSizeColumns()
                If cmbCurrency.SelectedValue = Me.BaseCurrencyId Then
                    'Me.grd.RootTable.Columns(EnumGrid.CurrencyAmount).Visible = False
                    Me.grd.RootTable.Columns(grdEnm.BaseCurrencyRate).Visible = False
                    Me.grd.RootTable.Columns(grdEnm.CurrencyRate).Visible = False
                    'Me.grd.RootTable.Columns(EnumGridDetail.CurrencyTaxAmount).Visible = False
                    If Me.grd.RowCount > 0 Then
                        For Each GriDEX As Janus.Windows.GridEX.GridEXRow In grd.GetRows
                            GriDEX.BeginEdit()
                            GriDEX.Cells("CurrencyRate").Value = Val(Me.txtCurrencyRate.Text)
                            GriDEX.Cells("CurrencyId").Value = Me.cmbCurrency.SelectedValue
                            GriDEX.Cells("BaseCurrencyId").Value = BaseCurrencyId
                            GriDEX.Cells("BaseCurrencyRate").Value = 1
                            GriDEX.EndEdit()
                        Next
                    End If
                Else
                    'Me.grd.RootTable.Columns(EnumGrid.CurrencyAmount).Visible = True
                    'Me.grd.RootTable.Columns(EnumGrid.CurrencyRate).Visible = True
                    'Me.grd.RootTable.Columns(EnumGridDetail.CurrencyTaxAmount).Visible = False
                    If Me.grd.RowCount > 0 Then
                        For Each GriDEX As Janus.Windows.GridEX.GridEXRow In grd.GetRows
                            GriDEX.BeginEdit()
                            GriDEX.Cells("CurrencyRate").Value = Val(Me.txtCurrencyRate.Text)
                            GriDEX.Cells("CurrencyId").Value = Me.cmbCurrency.SelectedValue
                            GriDEX.Cells("BaseCurrencyId").Value = BaseCurrencyId
                            GriDEX.Cells("BaseCurrencyRate").Value = 1
                            GriDEX.EndEdit()
                        Next
                    End If
                End If
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    'Change by Murtaza for txtcurrencyrate text change (11/09/2022)
End Class