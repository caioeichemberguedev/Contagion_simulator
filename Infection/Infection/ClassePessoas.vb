Public Class ClassePessoas

    Dim local As Rectangle
    Dim infectado As Boolean
    Dim vetor As Integer
    Dim circulando As Boolean

    Public Sub New(local As Rectangle, infectado As Boolean, vetor As Integer, circulando As Boolean)

        Me.local = local
        Me.infectado = infectado
        Me.circulando = circulando
        Me.vetor = vetor

    End Sub

    Public Property GSlocal() As Rectangle
        Get
            Return local
        End Get
        Set(local_ As Rectangle)
            local = local_
        End Set
    End Property

    Public Property GSinfectado() As Boolean
        Get
            Return infectado
        End Get
        Set(infectado_ As Boolean)
            infectado = infectado_
        End Set
    End Property

    Public Property GSvetor() As Integer
        Get
            Return vetor
        End Get
        Set(vetor_ As Integer)
            vetor = vetor_
        End Set
    End Property

    Public Property GScirculando() As Boolean
        Get
            Return circulando
        End Get
        Set(circulando_ As Boolean)
            circulando = circulando_
        End Set
    End Property
End Class
