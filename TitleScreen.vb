Public Class TitleScreen
    Private Sub TitleScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        My.Computer.Audio.Play("C:\Users\owens\Desktop\crap\Programming Project\Assets\BackgroundMusic.wav", AudioPlayMode.BackgroundLoop)

        ''''''''''''''https://stackoverflow.com/questions/29650788/how-to-play-2-different-sounds-at-the-same-timevisual-basic'''''''''''''''

    End Sub


    Private Sub btnPlay_Click(sender As Object, e As EventArgs) Handles btnPlay.Click
        Form1.Visible = True

        Me.Visible = False

        My.Computer.Audio.Stop()

    End Sub

    Private Sub btnMusic_Click(sender As Object, e As EventArgs) Handles btnMusicOff.Click
        My.Computer.Audio.Stop()
    End Sub

    Private Sub btnMusicOn_Click(sender As Object, e As EventArgs) Handles btnMusicOn.Click
        My.Computer.Audio.Play("C:\Users\owens\Desktop\crap\Programming Project\Assets\BackgroundMusic.wav", AudioPlayMode.BackgroundLoop)
    End Sub

    Private Sub btnMusic_Hover(sender As Object, e As EventArgs) Handles btnMusicOff.MouseHover
        MouseHoverEvent()
    End Sub

    Sub MouseHoverEvent()
        My.Computer.Audio.Play("C:\Users\owens\Desktop\crap\Programming Project\Assets\buttonHover.wav", AudioPlayMode.WaitToComplete)
    End Sub

    Sub MouseClickEvent()
        My.Computer.Audio.Play("C:\Users\owens\Desktop\crap\Programming Project\Assets\buttonClick.wav", AudioPlayMode.WaitToComplete)
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        If MsgBox("Are you sure you want to exit?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
            Application.Exit()
        End If
    End Sub
End Class