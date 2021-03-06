﻿
Imports System.IO
Imports System.Xml
Imports System.Environment
Imports System.Reflection
Imports System.Windows.Data
Class MainWindow
    Public Shared FullTaskList As New Dictionary(Of Integer, Task)
    Public Sub New()

        InitializeComponent()
        'Handle first run.  Settings are in the TaskOrganizer project properties under Settings.
        If My.Settings.FirstRun = True Then
            Dim datafolder As String = GetFolderPath(SpecialFolder.LocalApplicationData)
            Dim assem As AssemblyName = Assembly.GetExecutingAssembly().GetName ' ensures that the localAppData folder is the same name as the project.
            Dim x As String = datafolder + "\" + assem.Name + "\TaskList.xml"
            My.Settings.XmlFileLocation = x
            My.Settings.FirstRun = False
            My.Settings.Save() ' write changes to project settings (registry entries for this project)
            'If Not XmlHandlers.CreateNewTaskXMLFile() Then XmlHandlers.ValidateXMLFile(x)
        End If

        ' Check to see if the XML Task List is there or not
        If Not My.Computer.FileSystem.FileExists(My.Settings.XmlFileLocation) Then
            XmlHandlers.CreateNewTaskXMLFile()
        End If

        'Check to see if the XMl file is valid
        If Not XmlHandlers.ValidateXMLFile(My.Settings.XmlFileLocation) Then
            If MsgBox("Invalid XMl file: " & My.Settings.XmlFileLocation & vbCrLf & vbCrLf & "Create New Tasks File?", MsgBoxStyle.YesNo Or MsgBoxStyle.Critical Or MsgBoxStyle.DefaultButton1, "Bad XML Tasks File") = MsgBoxResult.Yes Then
                My.Computer.FileSystem.DeleteFile(My.Settings.XmlFileLocation)
                XmlHandlers.CreateNewTaskXMLFile()
            Else
                Close()
            End If
        End If
        LoadTasks()
        PopulateUI()
        'Booker.CreateTasksFromBooker() ' no no booker!
    End Sub


    Private Sub LoadTasks()
        Dim TaskList As New XmlDocument
        TaskList.Load(My.Settings.XmlFileLocation)
        Dim Tasks As XmlNode = TaskList.DocumentElement
        For Each Task As XmlNode In Tasks.SelectNodes("//Task")
            Dim id As Integer = Nothing
            id = CInt(Task.Attributes("id").Value)
            Dim ThisTask As New Task
            ThisTask.id = id.ToString
            ThisTask.desc = Task.SelectSingleNode("Desc").InnerText
            ThisTask.link = Task.SelectSingleNode("Link").InnerText
            ThisTask.type = Task.SelectSingleNode("Type").InnerText
            ThisTask.ticket = Task.SelectSingleNode("Ticket").InnerText
            ThisTask.notes = Task.SelectSingleNode("Notes").InnerText
            ThisTask.complete = CBool(Task.SelectSingleNode("Complete").InnerText)

            FullTaskList.Add(id, ThisTask)

        Next

    End Sub

    Private Sub PopulateUI()
        For Each TaskEntry As KeyValuePair(Of Integer, Task) In FullTaskList
            Dim id As Integer = TaskEntry.Key
            Dim Task As Task = TaskEntry.Value
            If Not Task.complete Then
                Dim NewTaskUIItem As TaskControl = New TaskControl(Task)
                taskControls.Children.Add(NewTaskUIItem)

            End If
        Next
    End Sub

    Public Shared Sub AddTaskToUI(ByRef taskObject As Task)
        FullTaskList.Add(taskObject.id, taskObject)
        Dim NewTaskUIItem As TaskControl = New TaskControl(taskObject)
        Dim parent As MainWindow = Application.Current.Windows(0)
        parent.taskControls.Children.Add(NewTaskUIItem)
    End Sub
    Public Shared Sub RemoveTaskFromUI(ByRef taskObject As Task)
        FullTaskList.Remove(taskObject.id)
        Dim parent As MainWindow = Application.Current.Windows(0)
        For Each taskcontrol As TaskControl In parent.taskControls.Children
            If taskcontrol.id = taskObject.id Then
                parent.taskControls.Children.Remove(taskcontrol)
                Exit For
            End If
        Next
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As RoutedEventArgs) Handles btnAdd.Click
        Dim t As New TaskDetails()
        t.Show()

    End Sub

    Private Sub titleBar_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        If e.ClickCount = 2 Then
            WindowState = WindowState.Minimized
        Else
            Me.DragMove()
        End If
    End Sub

    Private Sub menuItem_close_Click(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

    Private Sub Window_Closed(sender As Object, e As EventArgs)
        My.Settings.Save()

    End Sub

    Private Sub menuItem_reload_Click(sender As Object, e As RoutedEventArgs)

    End Sub
End Class
