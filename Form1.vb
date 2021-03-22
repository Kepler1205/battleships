Public Class Form1

    Dim isMousePressed As Boolean = False

    'Main board loc: 15, 530
    'Radar board loc: 15, 12

    'Ship Declaration

    Dim carrier, battleship, cruiser, submarine, destroyer As New Ship 'Players Ships

    Dim carrier2, battleship2, cruiser2, submarine2, destroyer2 As New Ship 'Opponents Ships

    Dim isStartPressed As Boolean = False
    Dim isAttacking As Boolean = False
    Shared random As New Random

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'btnDarkTheme.PerformClick()

        btnAttack.Visible = False

        'Ship Creation

        carrier.isVertical = True
        carrier.length = 5
        carrier.origPos.X = 534
        carrier.origPos.Y = 779
        carrier.picture = shipL5

        battleship.isVertical = True
        battleship.length = 4
        battleship.origPos.X = 602
        battleship.origPos.Y = 829
        battleship.picture = shipL4

        cruiser.isVertical = True
        cruiser.length = 3
        cruiser.origPos.X = 671
        cruiser.origPos.Y = 879
        cruiser.picture = shipL3

        'put in the rest of ships:

    End Sub

    'Vars For Ship Mover

    Dim startx As Integer
    Dim starty As Integer
    Dim endy As Integer
    Dim endx As Integer
    Dim finalx As Integer
    Dim finaly As Integer
    Dim mdown As Boolean
    Dim valx As Boolean
    Dim valy As Boolean

    Dim startPosX As Integer
    Dim startPosY As Integer

    '''''''''''''''''' Ship Events '''''''''''''''''' 


    'Carrier Events

    Private Sub ship_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles shipL5.MouseMove
        sender = ShipMove(carrier, sender)
    End Sub
    Private Sub picbox_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles shipL5.MouseDown
        mousePress(carrier)
    End Sub
    Private Sub picbox_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles shipL5.MouseUp
        carrier.Location = shipL5.Location
        shipL5.Location = SnapToGrid(carrier)
        carrier.Location = shipL5.Location
    End Sub
    Private Sub picbox_Rotate(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles shipL5.KeyDown
        If e.KeyCode = Keys.R And Control.MouseButtons = MouseButtons.Left Then
            RotateShip(carrier)
        End If
    End Sub

    'Battleship Events

    Private Sub ship_MouseMove2(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles shipL4.MouseMove
        sender = ShipMove(battleship, sender)
    End Sub
    Private Sub picbox_MouseDown2(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles shipL4.MouseDown
        mousePress(battleship)
    End Sub
    Private Sub picbox_MouseUp2(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles shipL4.MouseUp
        battleship.Location = shipL4.Location
        shipL4.Location = SnapToGrid(battleship)
        battleship.Location = shipL4.Location
    End Sub
    Private Sub picbox_Rotate2(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles shipL4.KeyDown
        If e.KeyCode = Keys.R And Control.MouseButtons = MouseButtons.Left Then
            RotateShip(battleship)
        End If
    End Sub


    'Ship Setup Functions '''''''''''''eventually put all of this into ship class?

    Sub mousePress(ship As Ship)
        startx = MousePosition.X
        starty = MousePosition.Y
        mdown = True
        valx = False
        valy = False

        ship.picture.Select()
    End Sub

    Function ShipMove(ship As Ship, sender As Object)

        startPosX = ship.Location.X
        startPosY = ship.Location.Y

        'Check if mouse=down

        If mdown = True Then
            endx = (MousePosition.X - Me.Left)
            endy = (MousePosition.Y - Me.Top)

            If valy = False Then
                starty = endy - sender.top
                valy = True
            End If
            If valx = False Then
                startx = endx - sender.left
                valx = True
            End If
            sender.left = endx - startx
            sender.top = endy - starty

            Return sender

        Else
            Return False

        End If

    End Function

    Function SnapToGrid(ship As Ship)

        mdown = False '< Needed
        valx = False
        valy = False

        If (ship.Location.X > MainBoard.Location.X And ship.Location.X < (MainBoard.Location.X + MainBoard.Size.Width - 25)) And (ship.Location.Y > MainBoard.Location.Y And ship.Location.Y < (MainBoard.Location.Y + MainBoard.Size.Height - 25) And isShipOutOfBounds(ship) = False) Then

            ship.Location = ship.PixelToGridLoc(MainBoard.Location)
            ship.Location = ship.GridLocToPixel(MainBoard.Location)

            Return ship.Location

        Else

            If ship.isVertical = False Then
                Dim x, y As Integer

                Dim newSize As Size

                x = ship.picture.Size.Width
                y = ship.picture.Size.Height

                newSize.Height = x  'x & y are flipped
                newSize.Width = y

                ship.picture.Size = newSize
                ship.picture.Image.RotateFlip(RotateFlipType.Rotate270FlipNone)
                ship.picture.Update()

                'ship.RotToggle = False
                ship.isVertical = True

            End If

            Return ship.origPos

        End If

    End Function

    Sub RotateShip(ship As Ship)

        Dim x, y As Integer
        Dim newSize As Size

        If ship.isVertical Then 'ship.RotToggle = False Then

            x = ship.picture.Size.Width
            y = ship.picture.Size.Height

            newSize.Height = x  'x & y are flipped
            newSize.Width = y

            ship.picture.Size = newSize
            ship.picture.Image.RotateFlip(RotateFlipType.Rotate90FlipNone)
            ship.picture.Update()

            'ship.RotToggle = True
            ship.isVertical = False
        Else

            x = ship.picture.Size.Width
            y = ship.picture.Size.Height

            newSize.Height = x  'x & y are flipped
            newSize.Width = y

            ship.picture.Size = newSize
            ship.picture.Image.RotateFlip(RotateFlipType.Rotate270FlipNone)
            ship.picture.Update()

            'ship.RotToggle = False
            ship.isVertical = True
        End If
    End Sub

    Function isShipOutOfBounds(ship As Ship) As Boolean

        If ship.isVertical Then
            If ship.Location.Y + ship.length * 50 > MainBoard.Size.Height + MainBoard.Location.Y Then
                Return True
            Else
                Return False
            End If
        Else
            If ship.Location.X + ship.length * 50 > MainBoard.Size.Width + MainBoard.Location.X Then
                Return True
            Else
                Return False
            End If
        End If
    End Function


    'Ship Playing Functions

    'WIP
    Dim pointArr(8) As Point 'Length is the sum of all ship lengths
    Dim alreadyStored As Integer = 0
    Dim tempShip As Ship
    Function isShipOverlapping(ship As Ship) As Boolean

        If ship.length = 5 Then
            pointArr = ship.allPos
            tempShip = carrier2
            debug2.Text = "is carrier"
            Return False
        End If

        ' For i As Integer = 0 To (carrier2.allPos.Length + battleship2.allPos.Length) - 1
        '     For j As Integer = 0 To 4
        '
        '     Next
        ' Next

        Dim rec As Rectangle
        rec.X = ship.Location.X
        rec.Y = ship.Location.Y
        rec.Width = ship.picture.Width
        rec.Height = ship.picture.Height

        Dim rec2 As Rectangle
        rec2.X = tempShip.Location.X
        rec2.Y = tempShip.Location.Y
        rec2.Width = tempShip.picture.Width
        rec2.Height = tempShip.picture.Height

        If rec.IntersectsWith(rec2) Then
            debug1.Text = "collided "
        End If

        Return rec.IntersectsWith(rec2)


        'ReDim Preserve pointArr(8)
        '
        'carrier2.GetAllLocations()
        'battleship2.GetAllLocations()
        '
        'Array.Copy(ship.allPos, pointArr, ship.allPos.Length)
        '
        'Dim j As Integer
        'j = 0
        'For i As Integer = 0 To pointArr.Length - 1
        '    If pointArr(i).X = 0 & pointArr(i).Y = 0 Then
        '        pointArr(i) = ship.allPos(j)
        '        j += 1
        '    End If
        'Next
        '
        '
        ''pointArr = pointArr.Concat(ship.allPos)
        '
        'Dim Str As String = ""
        'For i As Integer = 0 To pointArr.Length - 1
        '    Str += "(" & pointArr(i).X & ", " & pointArr(i).Y & ") "
        'Next
        'MsgBox(Str)

        'For i As Integer = 0 To pointArr.Length - 1
        '    If pointArr(i) = pointArr(i + 1) Then
        '        Return True
        '    End If
        'Next

        'For i As Integer = 0 To ship.allPos.Length - 1
        '    For j As Integer = 0 To pointArr.Length - 1
        '        If ship.allPos(i) = pointArr(j) Then
        '            debug1.Text = "Overlapped"
        '            Return True
        '        End If
        '    Next
        'Next

        ' For i As Integer = 0 To ship.length - 1
        '     pointArr(i + alreadyStored) = ship.allPos(i)
        ' Next
        '
        ' alreadyStored += ship.length

        Return False
    End Function

    Sub SetOpponentShip(ship As Ship)
        Do
            If random.Next(0, 2) Then

                RotateShip(ship)

            End If

            Dim sizex, sizey As Integer

            If ship.isVertical Then
                sizex = 0
                sizey = ship.length
            Else
                sizex = ship.length
                sizey = 0
            End If

            ship.Location.X = (random.Next(0, 10 - sizex) * 50) + RadarBoard.Location.X
            ship.Location.Y = (random.Next(0, 10 - sizey) * 50) + RadarBoard.Location.Y

        Loop While isShipOverlapping(ship)

        ship.picture.Location = ship.Location

    End Sub

    Function isShipOutOfRadar(ship As Ship) As Boolean

        If ship.isVertical Then
            If ship.Location.Y + (ship.length * 50) > (RadarBoard.Size.Height + RadarBoard.Location.Y) Then
                Return True
            Else
                Return False
            End If
        Else
            If ship.Location.X + (ship.length * 50) > (RadarBoard.Size.Width + RadarBoard.Location.X) Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    'Buttons

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        btnAttack.Visible = True
        debug1.Text = ""
        debug2.Text = ""
        'DEBUG'

        'pointArr.Initialize()
        alreadyStored = 0

        '''''''


        'If isStartPressed = False Then

        carrier2.length = 5
        carrier2.origPos.X = 602
        carrier2.origPos.Y = 163
        carrier2.picture = ship2L5
        'carrier2.picture.Visible = False
        SetOpponentShip(carrier2)

        battleship2.length = 4
        battleship2.origPos.X = 534
        battleship2.origPos.Y = 112
        battleship2.picture = ship2L4
        'battleship2.picture.Visible = False
        SetOpponentShip(battleship2)


        'End If

        isStartPressed = True
        'btnStart.Visible = False
    End Sub

    Private Sub btnAttack_Click(sender As Object, e As EventArgs) Handles btnAttack.Click
        isAttacking = True
        btnAttack.BackColor = Color.Red

        Dim int1(2), int2(2) As Integer

        int1(0) = 1
        int1(1) = 2
        int1(2) = 3

        int2(0) = 4
        int2(1) = 5
        int2(2) = 6


        ' debug1.Text =

        'set mouse icon to red crosshair when hovering over board?

    End Sub

    Private Sub AttackPos(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles RadarBoard.MouseClick
        If isAttacking And (e.Location.X > RadarBoard.Location.X And e.Location.X < (RadarBoard.Location.X + RadarBoard.Size.Width - 25)) And (e.Location.Y > RadarBoard.Location.Y And e.Location.Y < (RadarBoard.Location.Y + RadarBoard.Size.Height - 25)) Then

            Dim attackPoint As New PictureBox
            Dim NewPos As Point
            Dim NewSize As Size

            NewSize.Height = 50
            NewSize.Width = 50

            attackPoint.Size = NewSize

            NewPos.X = (e.Location.X - RadarBoard.Location.X) / 50
            NewPos.X *= 50
            NewPos.X += RadarBoard.Location.X

            NewPos.Y = (e.Location.Y - RadarBoard.Location.Y) / 50
            NewPos.Y *= 50
            NewPos.Y += RadarBoard.Location.Y

            attackPoint.Image = Image.FromFile("C:\Users\owens\Desktop\crap\Programming Project\Assets\red.bmp")

            attackPoint.Location = NewPos

            'MsgBox(attackPoint.Location.X & " " & attackPoint.Location.Y)
            'MsgBox(e.Location.X & " " & e.Location.Y)

            attackPoint.Show()
            attackPoint.BringToFront()
            btnAttack.BackColor = Color.White
        End If
        isAttacking = False
    End Sub

    Private Sub btnNewGame_Click(sender As Object, e As EventArgs) Handles btnNewGame.Click 'BROKEN DOESNT RESET SHIP ROT VALS

        '   'Save Score WIP
        '
        '   ''''''''''''''
        isStartPressed = False

        Controls.Clear()
        InitializeComponent()
        Form1_Load(e, e)
    End Sub


    'Settings

    Private Sub btnMusic_Click(sender As Object, e As EventArgs) Handles btnMusic.Click
        My.Computer.Audio.Play("C:\Users\owens\Desktop\crap\Programming Project\Assets\BackgroundMusic.wav", AudioPlayMode.BackgroundLoop)
    End Sub

    Private Sub btnMusicOff_Click(sender As Object, e As EventArgs) Handles btnMusicOff.Click
        My.Computer.Audio.Stop()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click

        TitleScreen.Visible = True

        Me.Visible = False

        My.Computer.Audio.Stop()

    End Sub

    Private Sub BtnDarkTheme_Click(sender As Object, e As EventArgs) Handles btnDarkTheme.Click
        BackColor = Color.Black
        ForeColor = Color.Gray
    End Sub

    Private Sub BtnLightTheme_Click(sender As Object, e As EventArgs) Handles btnLightTheme.Click
        BackColor = Color.White
        ForeColor = Color.Black
    End Sub

End Class

Public Class Ship

    Public length As Integer
    Public Location As Point
    Public origPos As Point
    Public allPos(length) As Point
    Public isVertical As Boolean = True
    Public picture As PictureBox

    Sub New()
        allPos.Initialize()
        GetAllLocations()
        isVertical = True
    End Sub

    Sub GetAllLocations()
        ReDim allPos(length)

        If isVertical Then
            For i As Integer = 0 To length
                allPos(i).X = Location.X
                allPos(i).Y = (i * 50) + Location.Y
            Next
        Else
            For j As Integer = 0 To length
                allPos(j).X = (j * 50) + Location.X
                allPos(j).Y = Location.Y
            Next
        End If
    End Sub

    Function PixelToGridLoc(boardLocation As Point) As Point
        Dim result As Point

        result.X = (Location.X - boardLocation.X) / 50
        result.Y = (Location.Y - boardLocation.Y) / 50

        Return result
    End Function

    Function GridLocToPixel(boardLocation As Point) As Point
        Dim result As Point

        result.X = Location.X * 50
        result.Y = Location.Y * 50
        result += boardLocation

        Return result
    End Function

End Class
