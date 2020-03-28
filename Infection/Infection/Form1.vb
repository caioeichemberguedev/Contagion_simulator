Imports System.Drawing.Drawing2D

Public Class Form1


    Dim listadepessoas As ArrayList = New ArrayList
    Dim ronaldinho As New Random
    Const ronaldofenomeno As Integer = 10
    Dim Populacao As Integer = 150
    Dim valorsomatorioxpracada(Populacao - 1) As Integer
    Dim valorsomatorioypracada(Populacao - 1) As Integer
    Const velocidade As Integer = 1
    Dim pessoainfectada As Integer = -1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.CenterToScreen()
        Timer1.Interval = 20
        CRIA_PESSOAS(Populacao)

    End Sub

    Private Sub CRIA_PESSOAS(QtdePopulacao As Integer)
        Dim listaderetangulos As New List(Of Rectangle)
        For k = 0 To QtdePopulacao - 1
            Dim sorteiook As Boolean = False
            Do While sorteiook = False
                Dim valorx As Integer
                valorx = ronaldinho.Next(ronaldofenomeno / 2, PictureBox1.Width - (ronaldofenomeno / 2))
                Dim valory As Integer
                valory = ronaldinho.Next(ronaldofenomeno / 2, PictureBox1.Height - (ronaldofenomeno / 2))
                Dim rect As Rectangle
                rect = New Rectangle(valorx, valory, ronaldofenomeno, ronaldofenomeno)
                If listaderetangulos.Contains(rect) = False Then
                    sorteiook = True
                    listaderetangulos.Add(rect)
                    listadepessoas.Add(New ClassePessoas(rect, False, 4, True))
                    ComboBox1.Items.Add(k)
                    valorsomatorioxpracada(k) = velocidade
                    valorsomatorioypracada(k) = velocidade
                Else
                    sorteiook = False
                End If
            Loop
        Next

        ComboBox1.SelectedIndex = 0
        Me.Refresh()

    End Sub

    Sub desenharonaldosfenomenos(sender As Object, e As PaintEventArgs) Handles PictureBox1.Paint
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias

        If listadepessoas.Count > 0 Then
            For k = 0 To listadepessoas.Count - 1
                Dim rect As Rectangle = listadepessoas(k).GSlocal
                If listadepessoas(k).GSinfectado = True Then
                    e.Graphics.FillEllipse(Brushes.LimeGreen, rect)
                    e.Graphics.DrawEllipse(Pens.Red, rect)
                Else
                    e.Graphics.FillEllipse(Brushes.DeepSkyBlue, rect)
                End If
            Next
        End If

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        For k = 0 To Populacao - 1
            listadepessoas(k).GSinfectado = False
        Next
        If ComboBox1.SelectedIndex >= 0 Then
            listadepessoas(ComboBox1.SelectedIndex).GSinfectado = True
        End If
        Me.Refresh()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        For K = 0 To listadepessoas.Count - 1
            Dim novopontox As Integer
            novopontox = listadepessoas(K).GSlocal.X + valorsomatorioxpracada(K)
            Dim novopontoy As Integer
            novopontoy = listadepessoas(K).GSlocal.Y + valorsomatorioypracada(K)
            listadepessoas(K).GSlocal = New Rectangle(novopontox, novopontoy, ronaldofenomeno, ronaldofenomeno)
            If listadepessoas(K).GSinfectado = True And listadepessoas(K).GSvetor > 0 Then
                If VERIFICA_CONTAGIO(listadepessoas(K).GSlocal, K) = True And pessoainfectada >= 0 Then
                    listadepessoas(pessoainfectada).GSinfectado = True
                    listadepessoas(K).GSvetor -= 1
                End If
            End If

            If novopontox < ronaldofenomeno / 2 Then
                valorsomatorioxpracada(K) = -valorsomatorioxpracada(K)
            ElseIf novopontox > PictureBox1.Width - ronaldofenomeno Then
                valorsomatorioxpracada(K) = -valorsomatorioxpracada(K)
            End If

            If novopontoy < ronaldofenomeno / 2 Then
                valorsomatorioypracada(K) = -valorsomatorioypracada(K)
            ElseIf novopontoy > PictureBox1.Height - ronaldofenomeno Then
                valorsomatorioypracada(K) = -valorsomatorioypracada(K)
            End If

        Next
        Me.Refresh()
    End Sub

    Private Function VERIFICA_CONTAGIO(localDoInfectado As Rectangle, idpessoacomvirus As Integer) As Boolean
        For idpessoa = 0 To listadepessoas.Count - 1
            Dim localdapessoa As Rectangle = listadepessoas(idpessoa).GSlocal
            If localdapessoa.IntersectsWith(localDoInfectado) = True And idpessoa <> idpessoacomvirus Then
                pessoainfectada = idpessoa
                Return True
            End If
        Next
        pessoainfectada = -1
        Return False
    End Function

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Refresh()
    End Sub
End Class
