''''''''''''''''''''''''''''''''''''''''''''''
'###########################################''
'#Kush Patel.                              #''
'#If help is needed...                     #''
'#Email: kushpatel35@hotmail.com           #''
'#.NET Framework 2.0+ is a requirement!    #''
'###########################################''  
''''''''''''''''''''''''''''''''''''''''''''''
Public Class Form1

    Dim key As String
    Dim nameAValue As String

    'Combobox switch constants
    'These are used for all the preset clubs, sports etc. Can freely add and remove things in their rightful categories. 
    Dim ATHLETIC() As String = {"Baseball", "Cross Country", "Curling", "Golf", "Gymnastics", "Hockey", "Jr. Badminton", _
    "Jr. Basketball", "Jr. Football", "Jr. Rugby", "Jr. Tennis", "Jr. Volleyball", "Lacrosse", "Mid. Basketball", _
    "Mid. Volleyball", "Soccer", "Softball", "Sr. Badminton", "Sr. Basketball", "Sr. Football", "Sr. Rugby", "Sr. Tennis", _
    "Sr. Volleyball", "Tennis", "Track and Field", "Wrestling"}
    Dim LEADERSHIP() As String = {"Club Executive", "DARE", "Peer Mediators", "PJ-CSI", "Representive on a school committe", "SIGMA", "Student Coach", _
    "Student Council Minister", "Student Council Prime Minister", "Team Captain", "Teen Esteem", "Yearbook Editor"}
    Dim SOCIAL() As String = {"Art Club", "Badminton Club", "Chess Club", "Choir", "Drama Club - Major Production", _
    "Drama Club – Minor Production", "Envirothon", "History Club", "Impact", "Jr. Band", "Key Club", "Literary Magazine", _
    "Ministry of Arts", "Ministry of Environment", "Ministry of Fun", "Ministry of Graduate Affairs", "Ministry of Sports and Recreation", _
    "Ministry of Technical Affairs", "Morning Announcements", "Ongwehonwe (Native Club)", "OSAID", "Rainbow Coalition", _
    "SITE", "Skills Canada Participant", "Spirit Squad", "Sr. Band", "Students’ Council Minister", "White Pine Book Club", _
    "Yearbook"}
    Dim ACADEMIC() As String = {"90% Average", "80% Average", "70% Average", "Chemistry Contest", "French Contest", _
    "Geography Contest", "Math Contest"}

    'This event triggers one time when form loads. I do not think this will ever require changing. 
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Fill the Student Table!
        Me.StudentsTableAdapter.Fill(Me.PjcvsStudentPointsDataSet1.Students)
        'This allows the data to be altered.
        'If this line is removed the whole program will stop working in a sense that changes will not save!
        Me.PjcvsStudentPointsDataSet1.AcceptChanges()
        'Set the date for adding points
        txtYear.Text = Date.Today.Year
    End Sub

    'This is used for cell edit changes in the table to the left.
    'Stuff like when you add a new row, cell paints, this event will fire, this event will update the data
    Private Sub DataGridView1_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles DataGridView1.CellPainting
        Me.StudentsTableAdapter.Update(Me.PjcvsStudentPointsDataSet1.Students)
    End Sub

    'Row Remove Listener for the table on the left.
    Private Sub DataGridView1_RowsRemoved(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles DataGridView1.RowsRemoved
        'Update the data when rows are removed to remove rows from data. 
        Me.StudentsTableAdapter.Update(Me.PjcvsStudentPointsDataSet1.Students)
    End Sub

    'This is essentially where it gets interesting.
    'This is where when a row is selected, it pulls information related to that row then
    'displays it in the table to the right of the screen then also displays selected name
    'and the total amount of points above the table on the left.
    Private Sub DataGridView1_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.RowEnter
        'The blank row with no name and all that still counts as a row.
        'Therefore this line of code will exit out of this method if that row is the selected row for obvious reasons. 
        If e.RowIndex > DataGridView1.Rows.Count - 2 Then Exit Sub
        'Set the current selected row's student name. 
        key = Me.DataGridView1.Rows(e.RowIndex).Cells(0).Value.ToString()
        'Populate the table on the right by the selected student name. Then update the gridview so it's viewable.
        Me.MainTableAdapter1.FillByName(Me.PjcvsStudentPointsDataSet1.Main, key)
        Me.DataGridView2.Update()
        'This simply counts all the points that student has by accessing the values set in the table to the right.
        'Again this is another one of those things I doubt will ever have to be changed.
        Dim tPoints As Integer
        For i As Integer = 0 To DataGridView2.Rows.Count - 2
            tPoints += DataGridView2.Rows(i).Cells(3).Value
        Next
        'This will display all the data above the table on the left
        'Recall that key = the Student Name that is selected
        lblSName.Text = "Selected Name: " + key
        lblTPoints.Text = "Total Points: " + tPoints.ToString
        'This goes in the little text field that is above the table on the right
        txtName.Text = key
    End Sub

    'Same concept as DataGridView1! Look at that!
    Private Sub DataGridView2_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles DataGridView2.CellPainting
        Me.MainTableAdapter1.Update(Me.PjcvsStudentPointsDataSet1.Main)
    End Sub

    'Same concept as DataGridView1! However after this one updates after a row is removed, it has to recount the point total then update that.
    Private Sub DataGridView2_RowsRemoved(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles DataGridView2.RowsRemoved
        Me.MainTableAdapter1.Update(Me.PjcvsStudentPointsDataSet1.Main)
        Me.DataGridView2.Update()
        'Recount points
        Dim tPoints As Integer
        For i As Integer = 0 To DataGridView2.Rows.Count - 2
            tPoints += DataGridView2.Rows(i).Cells(3).Value
        Next
        If tPoints <> 0 Then lblTPoints.Text = "Total Points: " + tPoints.ToString
    End Sub

    'This event fires everytime the value for the activity combobox changes.
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        'Basically, this will recognize 4 builtin categories. Athletic, Leadership, Social, Academic. Then it'll set the list of stuff in the 
        'other combobox that is what you can get points for like what sports if it's Athletic etc to the builtin lists that are at the very 
        'top of this code.
        Dim selected As String = ComboBox1.SelectedItem.ToString
        If selected = "Athletic" Then
            ComboBox2.Items.Clear()
            ComboBox2.Items.AddRange(ATHLETIC)
        ElseIf selected = "Leadership" Then
            ComboBox2.Items.Clear()
            ComboBox2.Items.AddRange(LEADERSHIP)
        ElseIf selected = "Social" Then
            ComboBox2.Items.Clear()
            ComboBox2.Items.AddRange(SOCIAL)
        ElseIf selected = "Academic" Then
            ComboBox2.Items.Clear()
            ComboBox2.Items.AddRange(ACADEMIC)
        End If
    End Sub

    'This event fires when you manually typed in text into the category combobox.
    'This will simply clear the activity combobox when you do. 
    Private Sub ComboBox1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox1.TextChanged
        ComboBox2.Items.Clear()
    End Sub

    'Instant Search Box Code - Gets text entered, uses name contains query. Do NOT remove the "%". They are important to it's function.
    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        Dim tValue As String = TextBox2.Text.Trim
        Me.StudentsTableAdapter.FillByNameContains(Me.PjcvsStudentPointsDataSet1.Students, "%" + tValue + "%")
    End Sub

    'The add points button click event listener, this will fire everytime that button is clicked on.
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Declare the variables
        Dim name, activity, category, points, year As String
        'Intialize variables using variables from the text fields and comboboxes above the right hand table
        name = txtName.Text
        activity = ComboBox2.Text
        category = ComboBox1.Text
        points = txtPAdd.Text
        year = txtYear.Text
        'If all intialized variables have a value then move on to inserting.
        If name <> Nothing And activity <> Nothing And category <> Nothing And points <> Nothing And year <> Nothing Then
            'Insert a new record for student points into the data then refresh the right hand table and update the displayed data
            Me.MainTableAdapter1.InsertQuery(name, activity, category, points, year)
            Me.MainTableAdapter1.FillByName(Me.PjcvsStudentPointsDataSet1.Main, key)
            Me.DataGridView2.Update()
            'Since a new record has been inserted, recount the totaly points the currently selected student has then display new Total
            Dim tPoints As Integer
            For i As Integer = 0 To DataGridView2.Rows.Count - 2
                tPoints += DataGridView2.Rows(i).Cells(3).Value
            Next
            lblTPoints.Text = "Total Points: " + tPoints.ToString
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox1.Text.Equals("Academic") Then
            txtPAdd.Text = 4 - ComboBox2.SelectedIndex
        End If
    End Sub

    Private Sub DataGridView1_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles DataGridView1.UserDeletingRow
        Dim confirm As MsgBoxResult = MsgBox("***Are you sure you want to delete all records of " + e.Row.Cells(0).Value.ToString + "***", MsgBoxStyle.YesNo)
        If confirm = MsgBoxResult.No Then
            e.Cancel = True
        End If
    End Sub

    'Generate Junior Letter Winner List - This is the bar on top 
    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        Dim msgBoxR As MsgBoxResult = MsgBox("!IMPORTANT! Make sure the instant search field is blank and the full student list is showing in the left table before proceeding. Would you like to continue?", MsgBoxStyle.YesNo, "ALERT!")
        If msgBoxR = MsgBoxResult.No Then Exit Sub
        'Count amount of rows in students data
        Dim rowCount As Integer = Me.PjcvsStudentPointsDataSet1.Students.Rows.Count
        'Declare variable where names will be stored
        Dim winnerList As String = "Junior Letter Winners"
        'Loop through all the rows!
        For i As Integer = 0 To rowCount - 1
            'If the current student doesn't have a junior letter date entered then continue
            If Me.PjcvsStudentPointsDataSet1.Students.Rows(i).Item(1).ToString = Nothing Then
                'Get student name
                Dim sn As String = Me.PjcvsStudentPointsDataSet1.Students.Rows(i).Item(0).ToString
                'Get all data related to the student
                Dim dt As DataTable = Me.MainTableAdapter1.GetDataByName(sn)
                'Count all points the student has 
                Dim pt As Integer = 0
                For Each r As DataRow In dt.Rows
                    pt += r.Item(4)
                Next
                'If the amount of points is more than or equal to 20 then add to winner list
                If pt >= 20 Then
                    winnerList += vbCrLf + sn
                End If
            End If
        Next
        'Set the clipboard text to the list
        Clipboard.SetText(winnerList)
        'Let the user know it's done searching for junior letter winners
        MsgBox("Junior Letter List has been copied to clipboard, just paste it into your favourite word processor now!")
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        Dim msgBoxR As MsgBoxResult = MsgBox("!IMPORTANT! Make sure the instant search field is blank and the full student list is showing in the left table before proceeding. Would you like to continue?", MsgBoxStyle.YesNo, "ALERT!")
        If msgBoxR = MsgBoxResult.No Then Exit Sub
        Dim rowCount As Integer = Me.PjcvsStudentPointsDataSet1.Students.Rows.Count
        Dim winnerList As String = "Senior Letter Winners"
        For i As Integer = 0 To rowCount - 1
            If Me.PjcvsStudentPointsDataSet1.Students.Rows(i).Item(2).ToString = Nothing Then
                Dim sn As String = Me.PjcvsStudentPointsDataSet1.Students.Rows(i).Item(0).ToString
                Dim dt As DataTable = Me.MainTableAdapter1.GetDataByName(sn)
                Dim pt As Integer = 0
                For Each r As DataRow In dt.Rows
                    pt += r.Item(4)
                Next
                If pt >= 40 Then
                    winnerList += vbCrLf + sn
                End If
            End If
        Next
        Clipboard.SetText(winnerList)
        MsgBox("Senior Letter List has been copied to clipboard, just paste it into your favourite word processor now!")
    End Sub

    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
        Dim msgBoxR As MsgBoxResult = MsgBox("!IMPORTANT! Make sure the instant search field is blank and the full student list is showing in the left table before proceeding. Would you like to continue?", MsgBoxStyle.YesNo, "ALERT!")
        If msgBoxR = MsgBoxResult.No Then Exit Sub
        Dim rowCount As Integer = Me.PjcvsStudentPointsDataSet1.Students.Rows.Count
        Dim winnerList As String = "Students' Council Major Award List"
        For i As Integer = 0 To rowCount - 1
            If Me.PjcvsStudentPointsDataSet1.Students.Rows(i).Item(3).ToString = Nothing Then
                Dim sn As String = Me.PjcvsStudentPointsDataSet1.Students.Rows(i).Item(0).ToString
                Dim dt As DataTable = Me.MainTableAdapter1.GetDataByName(sn)
                Dim pt As Integer = 0
                For Each r As DataRow In dt.Rows
                    pt += r.Item(4)
                Next
                If pt >= 50 Then
                    Dim curCat As String = ""
                    Dim a As Integer = 0
                    Dim b As Integer = 0
                    Dim c As Integer = 0
                    Dim d As Integer = 0
                    For Each r As DataRow In dt.Rows
                        curCat = r.Item(3).ToString.Trim
                        If curCat.Equals(ComboBox1.Items(0)) Then
                            a += r.Item(4)
                        ElseIf curCat.Equals(ComboBox1.Items(1)) Then
                            b += r.Item(4)
                        ElseIf curCat.Equals(ComboBox1.Items(2)) Then
                            c += r.Item(4)
                        ElseIf curCat.Equals(ComboBox1.Items(3)) Then
                            d += r.Item(4)
                        End If
                    Next
                    Dim count As Integer = 0
                    If a >= 10 Then count += 1
                    If b >= 10 Then count += 1
                    If c >= 10 Then count += 1
                    If d >= 10 Then count += 1
                    If count >= 3 Then
                        winnerList += vbCrLf + sn
                    End If
                End If
            End If
        Next
        Clipboard.SetText(winnerList)
        MsgBox("Students' Council Major Award List has been copied to clipboard, just paste it into your favourite word processor now!")
    End Sub

    Private Sub DataGridView1_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit
        If e.ColumnIndex = 0 And nameAValue <> Nothing Then
            Dim nameBValue = Me.DataGridView1.Rows(e.RowIndex).Cells(0).Value.ToString
            If Not nameBValue.Equals(nameAValue) Then
                Me.MainTableAdapter1.UpdateStudentNameByStudentName(nameBValue, nameAValue)
            End If
        End If
    End Sub

    Private Sub DataGridView1_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles DataGridView1.CellBeginEdit
        If e.ColumnIndex = 0 Then
            nameAValue = Me.DataGridView1.Rows(e.RowIndex).Cells(0).Value.ToString
        End If
    End Sub

    Private Sub Delete(ByVal year As String, ByVal smart As Boolean)
        MsgBox("Please wait until you get the message that says deletion is completed. Do not close the application while deleting is going on!")
        Dim db As DataTable = Me.StudentsTableAdapter.GetDataByEntryYear(year)
        Dim dCount As Integer = 0
        For Each r As DataRow In db.Rows
            Dim eYear As Integer = r.Item(4)
            Dim sName As String = r.Item(0).ToString
            Dim db2 As DataTable = Me.MainTableAdapter1.GetDataByName(sName)
            If db2.Rows.Count = 0 Or Not smart Then
                Me.StudentsTableAdapter.DeleteByName(sName)
                dCount += 1
            End If
        Next
        MsgBox("Successfully deleted " + dCount.ToString + " student entries from the database!")
        Me.StudentsTableAdapter.Fill(Me.PjcvsStudentPointsDataSet1.Students)
    End Sub

    Private Sub SmartDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SmartDelete.Click
        Dim year As String = txtYear1.Text
        If year <> Nothing Then
            Dim response As MsgBoxResult = MsgBox("Continuing this action will delete all records of students that have an entry year less" + _
            " than or equal to " + year + ", that have no point records. Would you like to continue?", MsgBoxStyle.YesNo)
            If response = MsgBoxResult.Yes Then
                Delete(year, True)
            End If
        Else
            MsgBox("You must enter year!")
        End If
    End Sub

    Private Sub CompleteDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompleteDelete.Click
        Dim year As String = txtYear1.Text
        If year <> Nothing Then
            Dim response As MsgBoxResult = MsgBox("Continuing this action will delete ALL records of students that have an entry year less" + _
            " than or equal to " + year + " even if they have points. Would you like to continue?", MsgBoxStyle.YesNo)
            If response = MsgBoxResult.Yes Then
                Delete(year, False)
            End If
        Else
            MsgBox("You must enter year!")
        End If
    End Sub

    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click
        Dim msgBoxR As MsgBoxResult = MsgBox("!IMPORTANT! Make sure the instant search field is blank and the full student list is showing in the left table before proceeding. Would you like to continue?", MsgBoxStyle.YesNo, "ALERT!")
        If msgBoxR = MsgBoxResult.No Then Exit Sub
        Dim rowCount As Integer = Me.PjcvsStudentPointsDataSet1.Students.Rows.Count
        Dim winnerList As String = "Potential Students' Council Major Award List"
        For i As Integer = 0 To rowCount - 1
            If Me.PjcvsStudentPointsDataSet1.Students.Rows(i).Item(3).ToString = Nothing Then
                Dim sn As String = Me.PjcvsStudentPointsDataSet1.Students.Rows(i).Item(0).ToString
                Dim dt As DataTable = Me.MainTableAdapter1.GetDataByName(sn)
                Dim pt As Integer = 0
                For Each r As DataRow In dt.Rows
                    pt += r.Item(4)
                Next
                If pt >= 46 Then
                    Dim curCat As String = ""
                    Dim a As Integer = 0
                    Dim b As Integer = 0
                    Dim c As Integer = 0
                    Dim d As Integer = 0
                    For Each r As DataRow In dt.Rows
                        curCat = r.Item(3).ToString.Trim
                        If curCat.Equals(ComboBox1.Items(0)) Then
                            a += r.Item(4)
                        ElseIf curCat.Equals(ComboBox1.Items(1)) Then
                            b += r.Item(4)
                        ElseIf curCat.Equals(ComboBox1.Items(2)) Then
                            c += r.Item(4)
                        ElseIf curCat.Equals(ComboBox1.Items(3)) Then
                            d += r.Item(4)
                        End If
                    Next
                    Dim count As Integer = 0
                    If a >= 10 Then count += 1
                    If b >= 10 Then count += 1
                    If c >= 10 Then count += 1
                    If d >= 10 Then count += 1
                    If (count = 2 And d >= 6 And d <= 9) Or (pt >= 46 And pt <= 49 And count >= 3) Then
                        winnerList += vbCrLf + sn
                    End If
                End If
            End If
        Next
        Clipboard.SetText(winnerList)
        MsgBox("Potential Students' Council Major Award List has been copied to clipboard, just paste it into your favourite word processor now!")
    End Sub

    Private Sub ToolStripButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton5.Click
        Dim f As New OpenFileDialog
        f.Multiselect = False
        f.Filter = "CSV SpreadSheet (*.csv)|*.csv|All Files (*.*)|*.*"
        f.ShowDialog()
        If f.FileName <> Nothing Then
            Dim stream As System.IO.Stream = f.OpenFile
            Dim streamReader As System.IO.StreamReader = New System.IO.StreamReader(f.OpenFile())
            Dim s As String = InputBox("Please enter the entry year of the students being imported. After you press okay please patiently wait several minutes for another popup to come up that will say the import is complete. The program may be unresponsive while this process is going on.", "Entry Year", "")
            If s <> "" And s <> Nothing Then
                Dim i As Integer = Integer.Parse(s)
                Dim count As Integer
                If i <> 0 And i <> -1 Then
                    Dim nextName As String
                    While Not streamReader.EndOfStream
                        nextName = streamReader.ReadLine()
                        If nextName <> Nothing Then
                            nextName = nextName.Replace("""", "").Trim()
                            If Me.StudentsTableAdapter.GetDataByName(nextName).Rows.Count = 0 Then
                                Me.StudentsTableAdapter.InsertQuery(nextName, "", "", "", i)
                                count += 1
                            End If
                        End If
                    End While
                    If count = 0 Then MsgBox("Import complete! No new students were imported!") Else  : MsgBox("Import complete! Successfully imported " & count & " new students!")
                    Me.StudentsTableAdapter.Fill(Me.PjcvsStudentPointsDataSet1.Students)
                End If
            End If
            stream.Flush()
            stream.Close()
            stream.Dispose()
            streamReader.Close()
            streamReader.Dispose()
        Else
            MsgBox("No file was selected")
        End If
    End Sub

    Private Sub DataGridView2_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles DataGridView2.UserDeletingRow
        Dim confirm As MsgBoxResult = MsgBox("Are you sure you want to delete selected row of points?",MsgBoxStyle.YesNo)
        If confirm = MsgBoxResult.No Then
            e.Cancel = True
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        For i As Integer = 0 To Me.PjcvsStudentPointsDataSet1.Students.Count
            MsgBox(Me.PjcvsStudentPointsDataSet1.Students.Item(i).Student_Name)
        Next
    End Sub
End Class
