Public Class AssetStatusBE

    Private _Asset_Status_Id As Integer
    Public Property Asset_Status_Id() As Integer
        Get
            Return _Asset_Status_Id
        End Get
        Set(ByVal value As Integer)
            _Asset_Status_Id = value
        End Set
    End Property


    Private _Asset_Status_Name As String
    Public Property Asset_Status_Name() As String
        Get
            Return _Asset_Status_Name
        End Get
        Set(ByVal value As String)
            _Asset_Status_Name = value
        End Set
    End Property


    Private _Asset_Status_Description As String
    Public Property Asset_Status_Description() As String
        Get
            Return _Asset_Status_Description
        End Get
        Set(ByVal value As String)
            _Asset_Status_Description = value
        End Set
    End Property

    Private _Sort_Order As Integer
    Public Property Sort_Order() As Integer
        Get
            Return _Sort_Order
        End Get
        Set(ByVal value As Integer)
            _Sort_Order = value
        End Set
    End Property

    Private _Active As Boolean
    Public Property Active() As Boolean
        Get
            Return _Active
        End Get
        Set(ByVal value As Boolean)
            _Active = value
        End Set
    End Property

End Class
