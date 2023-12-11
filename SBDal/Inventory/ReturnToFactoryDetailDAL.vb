﻿Imports SBDal
Imports SBModel
Imports System.Data.SqlClient

Public Class ReturnToFactoryDetailDAL
    Function Add(ByVal objModel As ReturnToFactoryDetailBE) As Boolean
        Dim Con As New SqlConnection(SQLHelper.CON_STR)
        If Con.State = ConnectionState.Closed Then Con.Open()
        Dim trans As SqlTransaction = Con.BeginTransaction()
        Try
            'Add(objModel, trans)
            trans.Commit()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function AddDetail(ByVal obj As ReturnToFactoryMasterBE, trans As SqlTransaction) As Boolean
        Try
            For Each objModel As ReturnToFactoryDetailBE In obj.Detail
                Dim strSQL As String = String.Empty
                If objModel.ID = 0 Then
                    strSQL = "insert into  ReturnToFactoryDetail (ReturnToFactoryId, LocationId, ArticleId, Unit, Rate, Sz1, Sz7, Qty, Comments) " _
                        & " Values (" & obj.ID & ", " & objModel.LocationId & ", " & objModel.ArticleId & ", N'" & objModel.Unit.Replace("'", "''") & "', " & objModel.Rate & ", " _
                        & " " & objModel.Sz1 & ", " & objModel.Sz7 & ", " & objModel.Qty & ", N'" & objModel.Comments.Replace("'", "''") & "') "
                    SQLHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL)
                Else
                    strSQL = "UPDATE  ReturnToFactoryDetail SET ReturnToFactoryId = " & obj.ID & ", LocationId= " & objModel.LocationId & ", " _
                        & " ArticleId = " & objModel.ArticleId & ", Unit = N'" & objModel.Unit.Replace("'", "''") & "', Rate= " & objModel.Rate & ", Sz1 = " & objModel.Sz1 & ", " _
                        & " Sz7 = " & objModel.Sz7 & ", Qty = " & objModel.Qty & ", Comments = '" & objModel.Comments.Replace("''", "'") & "' WHERE ID = " & objModel.ID & ""
                    SQLHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL)
                End If
            Next
            'objModel.ActivityLog.ActivityName = "Save"
            'objModel.ActivityLog.RecordType = "Configuration"
            'objModel.ActivityLog.RefNo = ""
            'UtilityDAL.BuildActivityLog(objModel.ActivityLog, trans)
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function Update(ByVal objModel As ReturnToFactoryDetailBE) As Boolean
        Dim Con As New SqlConnection(SQLHelper.CON_STR)
        If Con.State = ConnectionState.Closed Then Con.Open()
        Dim trans As SqlTransaction = Con.BeginTransaction()
        Try
            Update(objModel, trans)
            trans.Commit()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function Update(ByVal objModel As ReturnToFactoryDetailBE, trans As SqlTransaction) As Boolean
        Try
            Dim strSQL As String = String.Empty
            strSQL = "update ReturnToFactoryDetail set ReturnToFactoryId= N'" & objModel.ReturnToFactoryId & "', LocationId= N'" & objModel.LocationId & "', ArticleId= N'" & objModel.ArticleId & "', Unit= N'" & objModel.Unit.Replace("'", "''") & "', Rate= N'" & objModel.Rate & "', Sz1= N'" & objModel.Sz1 & "', Sz7= N'" & objModel.Sz7 & "', Qty= N'" & objModel.Qty & "', Comments= N'" & objModel.Comments.Replace("'", "''") & "' where ID=" & objModel.ID
            SQLHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL)
            objModel.ActivityLog.ActivityName = "Update"
            objModel.ActivityLog.RecordType = "Configuration"
            objModel.ActivityLog.RefNo = ""
            UtilityDAL.BuildActivityLog(objModel.ActivityLog, trans)
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function Delete(ByVal ID As Integer) As Boolean
        Dim Con As New SqlConnection(SQLHelper.CON_STR)
        If Con.State = ConnectionState.Closed Then Con.Open()
        Dim trans As SqlTransaction = Con.BeginTransaction()
        Try
            Delete(ID, trans)
            trans.Commit()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function Delete(ByVal ID As Integer, trans As SqlTransaction) As Boolean
        Dim strSQL As String = String.Empty
        Try
            strSQL = "Delete from ReturnToFactoryDetail WHERE ID= " & ID
            SQLHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL)
            'objModel.ActivityLog.ActivityName = "Delete"
            'objModel.ActivityLog.RecordType = "Configuration"
            'objModel.ActivityLog.RefNo = ""
            'UtilityDAL.BuildActivityLog(objModel.ActivityLog, trans)
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAll() As DataTable
        Dim strSQL As String = String.Empty
        Try
            strSQL = " Select ID, ReturnToFactoryId, LocationId, ArticleId, Unit, Rate, Sz1, Sz7, Qty, TotalQty, Comments from ReturnToFactoryDetail  "
            Dim dt As DataTable = UtilityDAL.GetDataTable(strSQL)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function DisplayDetail(ByVal ReturnToFactoryId As Integer) As DataTable
        Dim strSQL As String = String.Empty
        Try
            strSQL = " Select Detail.ID, Detail.ReturnToFactoryId, Detail.LocationId, Detail.ArticleId, Article.ArticleDescription AS Article, Detail.Unit, Detail.Rate, Detail.Sz1, Detail.Sz7, Detail.Qty, Detail.Comments from ReturnToFactoryDetail AS Detail INNER JOIN ArticleDefTable AS Article ON Detail.ArticleId = Article.ArticleId LEFT OUTER JOIN tblDefLocation AS Location ON Detail.LocationId = Location.location_id  WHERE ReturnToFactoryId= " & ReturnToFactoryId
            Dim dt As DataTable = UtilityDAL.GetDataTable(strSQL)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
