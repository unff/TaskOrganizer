Public Class TaskDetails
    Public TaskHolder As Task
    Public Sub New()
        InitializeComponent()
        Dim newTask As New Task
        newTask.id = Task.GetHighestId() + 1
        PopulateDetails(newTask)
        TaskHolder = newTask

    End Sub
    Public Sub New(ByRef taskObject As Task)

        InitializeComponent()
        PopulateDetails(taskObject)
        TaskHolder = taskObject

    End Sub
    Private Sub PopulateDetails(ByRef taskObject As Task)
        Title = Title + ": " + taskObject.id.ToString
        If taskObject.complete Then
            Title = Title + " - COMPLETED"
        End If
        If taskObject.type = "Booker Appointment" Then
            btnSave.IsEnabled = False
        End If
        tbxDesc.Text = taskObject.desc
        tbxLink.Text = taskObject.link
        ' populate the combobox with all values
        Dim validType As Boolean = False
        For Each x As DictionaryEntry In Application.Current.Resources
            cbxType.Items.Add(x.Key)
            validType = If(x.Key = taskObject.type, True, validType)
        Next
        cbxType.SelectedItem = If(validType, taskObject.type, "Other")
        tbxTicket.Text = taskObject.ticket
        chkComplete.IsChecked = taskObject.complete
        tbxNotes.Text = taskObject.notes

    End Sub

    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        TaskHolder.desc = tbxDesc.Text
        TaskHolder.link = tbxLink.Text
        TaskHolder.type = cbxType.SelectedValue
        TaskHolder.notes = tbxNotes.Text
        TaskHolder.ticket = tbxTicket.Text
        TaskHolder.complete = chkComplete.IsChecked
        Task.UpdateTask(TaskHolder)
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As RoutedEventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub cbxType_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cbxType.SelectionChanged

    End Sub

    Private Sub Window_Closed(sender As Object, e As EventArgs)
        My.Settings.Save()
    End Sub
End Class
