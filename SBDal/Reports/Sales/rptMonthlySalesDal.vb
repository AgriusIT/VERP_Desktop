Imports SBUtility
Imports SBModel
Imports System.Data.SqlClient
Public Class rptMonthlySalesDal

    Public Function GetDataTable(ByVal ToDate As String) As DataTable
        Dim objDA As SqlClient.SqlDataAdapter
        Try

            Dim StrSQL As New System.Text.StringBuilder
            Dim cn As New SqlClient.SqlConnection(SQLHelper.CON_STR)
            Dim cm As New SqlClient.SqlCommand
            cm.CommandType = CommandType.StoredProcedure

            cm.CommandText = "SP_Rpt_DailySales"

            cm.Connection = cn
            cm.CommandTimeout = 500
            cm.Parameters.Add("@Date", SqlDbType.DateTime)

            cm.Parameters.Item("@Date").Value = ToDate

            objDA = New SqlClient.SqlDataAdapter(cm)
            Dim MyCollectionList As New DataTable("GetRecords")

            objDA.Fill(MyCollectionList)

            Return MyCollectionList
        Catch ex As SqlException
            Throw ex
        Catch ex As Exception
            Throw ex
        Finally
            objDA = Nothing
        End Try

    End Function


End Class
