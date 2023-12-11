Public Class Unit
    Private _ID As Integer
    Private _Name As String
    Private _ActivityLog As ActivityLog
    Private _Comments As String
    Private _Active As Boolean
    Private _SortOrder As Integer

    Public Property Comments() As String
        Get
            Return Me._Comments
        End Get
        Set(ByVal value As String)
            Me._Comments = value
        End Set
    End Property

    Public Property ActivityLog() As ActivityLog
        Get
            Return Me._ActivityLog
        End Get
        Set(ByVal value As ActivityLog)
            Me._ActivityLog = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return Me._Name
        End Get
        Set(ByVal value As String)
            Me._Name = value
        End Set
    End Property
    Public Property ID() As Integer
        Get
            Return Me._ID
        End Get
        Set(ByVal value As Integer)
            Me._ID = value
        End Set
    End Property
    Public Property Active() As Boolean
        Get
            Return Me._Active
        End Get
        Set(ByVal value As Boolean)
            Me._Active = value
        End Set
    End Property
    Public Property SortOrder() As Integer
        Get
            Return Me._SortOrder
        End Get
        Set(ByVal value As Integer)
            Me._SortOrder = value
        End Set
    End Property
End Class
