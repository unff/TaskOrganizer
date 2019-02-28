Imports System.ComponentModel
Public Class TaskControl
    Inherits UserControl

    Private TaskHolder As Task
    Public id As Integer
    Private color As ImageSource
    Public availableTasks As List(Of String) = ValidTasks()

    Public Sub New(ByRef taskObject As Task)

        InitializeComponent()
        TaskHolder = taskObject
        id = TaskHolder.id
        Me.DataContext = TaskHolder
        'bkgImage.ImageSource = taskObject.type
        'Me.color = CType(Resources.Item(taskObject.type), ImageSource)

    End Sub
    Private Sub clicked(sender As Object, e As RoutedEventArgs) Handles TaskControl.MouseDoubleClick
        Dim x As New TaskDetails(Me.TaskHolder)
        x.Show()
    End Sub

    Public Function ValidTasks() As List(Of String)
        'key = Key, value = BitMapFrame
        For Each x As DictionaryEntry In Resources
            availableTasks.Add(x.Key)
        Next
        Return availableTasks
    End Function

    Public Function getValueforKey(key As String) As String
        Return Nothing
    End Function

    Private Sub controlMenu_close_Click(sender As Object, e As RoutedEventArgs)
        Task.RemoveTask(TaskHolder)
    End Sub

    Private Function UrlIsLive(ByVal url As String) As Boolean
        Dim is_valid As Boolean = False
        If url.ToLower().StartsWith("www.") Then url =
            "http://" & url

        Dim web_response As Net.HttpWebResponse = Nothing
        Try
            Dim web_request As Net.HttpWebRequest =
                Net.HttpWebRequest.Create(url)
            web_response =
                DirectCast(web_request.GetResponse(),
                Net.HttpWebResponse)
            Return True
        Catch ex As Exception
            Return False
        Finally
            If Not (web_response Is Nothing) Then _
                web_response.Close()
        End Try
    End Function

    Private Function UrlIsValid(ByVal url As String) As Boolean
        Dim result As Uri = Nothing
        If Uri.TryCreate(url, UriKind.RelativeOrAbsolute, result) Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub lblLink_MouseUp(sender As Object, e As MouseButtonEventArgs)
        If UrlIsValid(TaskHolder.link) Then
            Try
                Mouse.OverrideCursor = Cursors.AppStarting
                Process.Start(TaskHolder.link)
            Catch ex As Exception

            Finally
                Mouse.OverrideCursor = Nothing
            End Try
        End If
    End Sub

    Private Sub controlMenu_delete_Click(sender As Object, e As RoutedEventArgs)
        MainWindow.RemoveTaskFromUI(TaskHolder)
    End Sub
End Class

