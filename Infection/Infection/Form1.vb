Imports System.Drawing.Drawing2D

Public Class Form1

    Dim N_NotInfected As Integer = 0
    Dim N_Infected As Integer = 0
    Dim N_Dead As Integer = 0
    Dim N_Recovered As Integer = 0

    Dim NewInfected As Integer = -1
    Const PeopleSpeed As Integer = 2
    Const PeopleSize As Integer = 8
    Const TimerInterval As Integer = 75
    Const AmountPeople As Integer = 80
    Private PeopleAge As New Random
    Private PeopleInitialLocation As New Random
    Private PeopleInitialDirection As New Random
    Private PopulationList As ArrayList = New ArrayList
    Private DirectionThatPeopleWillWalkInX(AmountPeople - 1) As Integer
    Private DirectionThatPeopleWillWalkInY(AmountPeople - 1) As Integer

    Dim InitialListPoint As New List(Of Point)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.CenterToScreen()
        Timer1.Interval = TimerInterval

        Dim NumberOfCollumns As Integer = PictureBox1.Width / PeopleSize
        Dim NumberOfLines As Integer = PictureBox1.Height / PeopleSize
        Dim NumberOfSpaces As Integer = NumberOfCollumns * NumberOfLines

        Dim CountCollumns As Integer = 0
        Dim CountLines As Integer = 0
        Dim Ponto As Point

        For k = 0 To NumberOfSpaces - 33 'Para não nascerem na ultima linha

            Ponto = New Point(PeopleSize * CountCollumns, PeopleSize * CountLines)
            InitialListPoint.Add(Ponto)

            If CountCollumns < NumberOfCollumns - 2 Then 'Para não nascerem na ultima coluna
                CountCollumns += 1
            Else
                CountCollumns = 0
                CountLines += 1
            End If

        Next
        Me.Refresh()
        SelectingPeople(AmountPeople)

    End Sub

    Private Sub UpdateInformation(v1 As Integer, v2 As Integer, v3 As Integer)
        N_Infected += v1
        N_Dead += v2
        N_Recovered += v3
        TextBox1.Text = N_Infected
        TextBox2.Text = N_Dead
        TextBox3.Text = N_Recovered
    End Sub

    Private Sub SelectingPeople(AmountPeople_ As Integer)

        Dim ListOfPointId As New List(Of Integer)

        PopulationList.Clear()

        For k = 0 To AmountPeople_ - 1
            Dim ValidatedRandom As Boolean = False
            Do While ValidatedRandom = False

                Dim GetLocation As Integer = PeopleInitialLocation.Next(0, InitialListPoint.Count)

                If ListOfPointId.Contains(GetLocation) = False Then
                    ListOfPointId.Add(GetLocation)

                    Dim idade As Integer = PeopleAge.Next(18, 70)
                    Dim PeopleSpace As Rectangle = New Rectangle(InitialListPoint(GetLocation).X, InitialListPoint(GetLocation).Y, PeopleSize, PeopleSize)
                    PopulationList.Add(New ClassePessoas(PeopleSpace, False, 4, True))

                    If PeopleInitialDirection.Next(0, 2) = 0 Then
                        DirectionThatPeopleWillWalkInX(k) = -PeopleSpeed
                    Else
                        DirectionThatPeopleWillWalkInX(k) = PeopleSpeed
                    End If

                    If PeopleInitialDirection.Next(0, 2) = 0 Then
                        DirectionThatPeopleWillWalkInY(k) = -PeopleSpeed
                    Else
                        DirectionThatPeopleWillWalkInY(k) = PeopleSpeed
                    End If

                    ValidatedRandom = True
                End If
            Loop
        Next

        PopulationList(10).GSinfectado = True
        UpdateInformation(1, 0, 0)
        Me.Refresh()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        For PeopleId = 0 To PopulationList.Count - 1

            If CheckContact(PeopleId, PopulationList(PeopleId).GSlocal, PopulationList(PeopleId).GSinfectado, PopulationList(PeopleId).GSvetor) = True Then

                PopulationList(NewInfected).GSinfectado = True
                PopulationList(PeopleId).GSvetor -= 1

            End If

            CheckIntersercLimitLocal(PeopleId)

        Next

        Me.Refresh()

    End Sub

    Private Sub CheckIntersercLimitLocal(Pid As Integer)
        Dim novopontox As Integer
        Dim novopontoy As Integer

        novopontox = PopulationList(Pid).GSlocal.X + DirectionThatPeopleWillWalkInX(Pid)
        novopontoy = PopulationList(Pid).GSlocal.Y + DirectionThatPeopleWillWalkInY(Pid)

        If novopontox <= 0 Or novopontox >= PictureBox1.Width - PeopleSize Then DirectionThatPeopleWillWalkInX(Pid) = -DirectionThatPeopleWillWalkInX(Pid)
        If novopontoy <= 0 Or novopontoy >= PictureBox1.Height - PeopleSize Then DirectionThatPeopleWillWalkInY(Pid) = -DirectionThatPeopleWillWalkInY(Pid)

        PopulationList(Pid).GSlocal = New Rectangle(novopontox, novopontoy, PeopleSize, PeopleSize)
    End Sub

    Private Function CheckContact(PId As Integer, PLocal As Rectangle, PInfected As Boolean, PVetor As Integer) As Boolean
        For idpessoa = 0 To PopulationList.Count - 1
            Dim localdapessoa As Rectangle = PopulationList(idpessoa).GSlocal

            If localdapessoa.IntersectsWith(PLocal) = True And idpessoa <> PId Then

                DirectionThatPeopleWillWalkInX(PId) = -DirectionThatPeopleWillWalkInX(PId)
                DirectionThatPeopleWillWalkInY(PId) = -DirectionThatPeopleWillWalkInY(PId)

                Dim novopontox As Integer
                Dim novopontoy As Integer
                novopontox = PopulationList(idpessoa).GSlocal.X + DirectionThatPeopleWillWalkInX(idpessoa)
                novopontoy = PopulationList(idpessoa).GSlocal.Y + DirectionThatPeopleWillWalkInY(idpessoa)
                DirectionThatPeopleWillWalkInX(idpessoa) = -DirectionThatPeopleWillWalkInX(idpessoa)
                DirectionThatPeopleWillWalkInY(idpessoa) = -DirectionThatPeopleWillWalkInY(idpessoa)
                PopulationList(idpessoa).GSlocal = New Rectangle(novopontox, novopontoy, PeopleSize, PeopleSize)

                If PopulationList(idpessoa).GSinfectado = False And PInfected = True And PVetor > 0 Then
                    NewInfected = idpessoa
                    UpdateInformation(1, 0, 0)
                    Return True
                End If
            End If

        Next

        NewInfected = -1
        Return False

    End Function

    Private Sub DrawingPeopleInTheirLocations(sender As Object, e As PaintEventArgs) Handles PictureBox1.Paint
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias

        If PopulationList.Count > 0 Then
            For k = 0 To PopulationList.Count - 1
                Dim rect As Rectangle = PopulationList(k).GSlocal
                If PopulationList(k).GSinfectado = True Then
                    e.Graphics.FillEllipse(Brushes.LimeGreen, rect)
                    e.Graphics.DrawEllipse(Pens.Red, rect)
                Else
                    'e.Graphics.DrawEllipse(Pens.White, rect)
                    e.Graphics.FillEllipse(Brushes.White, rect)
                End If
            Next
        End If

        'For k = 0 To InitialListPoint.Count - 1
        '    Dim rect As Rectangle = New Rectangle(InitialListPoint(k).X, InitialListPoint(k).Y, PeopleSize, PeopleSize)
        '    e.Graphics.FillEllipse(Brushes.Red, rect)
        'Next
    End Sub
End Class
