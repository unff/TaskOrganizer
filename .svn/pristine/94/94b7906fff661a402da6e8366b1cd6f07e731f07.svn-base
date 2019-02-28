Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Xml

Public Class Task
    Implements INotifyPropertyChanged

    Private m_id As Integer
    Private m_desc As String
    Private m_type As String
    Private m_link As String
    Private m_ticket As String
    Private m_notes As String
    Private m_complete As Boolean

    Public Structure TaskType
        Public Key As String
        Public Value As ImageSource
    End Structure
    Public ReadOnly Property TypeMap As ImageSource
        Get
            Dim x As ImageSource = Nothing
            x = If(Application.Current.Resources.Item(m_type) Is Nothing, CType(Application.Current.Resources.Item("Other"), ImageSource), CType(Application.Current.Resources.Item(m_type), ImageSource))
            Return x
        End Get
    End Property
    Public Property id As Integer
        Get
            Return m_id
        End Get
        Set(value As Integer)
            m_id = value
            OnPropertyChanged(New PropertyChangedEventArgs("id"))
        End Set
    End Property

    Public Property desc As String
        Get
            Return m_desc
        End Get
        Set(value As String)
            m_desc = value
            OnPropertyChanged(New PropertyChangedEventArgs("desc"))
        End Set
    End Property

    Public Property type As String
        Get
            Return m_type
        End Get
        Set(value As String)
            m_type = value

            OnPropertyChanged(New PropertyChangedEventArgs("TypeMap"))
        End Set
    End Property

    Public Property link As String
        Get
            Return m_link
        End Get
        Set(value As String)
            m_link = value
            OnPropertyChanged(New PropertyChangedEventArgs("link"))
        End Set
    End Property
    Public Property ticket As String
        Get
            Return m_ticket
        End Get
        Set(value As String)
            m_ticket = value
            OnPropertyChanged(New PropertyChangedEventArgs("ticket"))
        End Set
    End Property
    Public Property notes As String
        Get
            Return m_notes
        End Get
        Set(value As String)
            m_notes = value
            OnPropertyChanged(New PropertyChangedEventArgs("notes"))
        End Set
    End Property
    Public Property complete As Boolean
        Get
            Return m_complete
        End Get
        Set(value As Boolean)
            m_complete = value
            OnPropertyChanged(New PropertyChangedEventArgs("complete"))
        End Set
    End Property
    Public Sub New()
        m_id = GetHighestId() + 1
        m_desc = String.Empty
        m_type = String.Empty
        m_link = String.Empty
        m_ticket = String.Empty
        m_notes = String.Empty
        m_complete = False
    End Sub
    Public Shared Function GetHighestId() As Integer
        Dim TaskXMLFile As XmlDocument = New XmlDocument()
        Dim Tasks As XmlNode
        Dim HighestId As Integer = 0
        Try
            TaskXMLFile.Load(My.Settings.XmlFileLocation)
            Tasks = TaskXMLFile.DocumentElement ' Loads the document element.  In this case, Tasks
            For Each Task As XmlNode In Tasks.SelectNodes("//Task")
                If Task.Attributes("id").Value > HighestId Then
                    HighestId = Task.Attributes("id").Value
                End If

            Next
            Return HighestId
        Catch ex As XmlException
            Return 0
        End Try
    End Function
    Public Sub MarkComplete(ByVal state As Boolean)
        m_complete = True
    End Sub

    Public Sub ToggleComplete()
        If m_complete Then
            m_complete = False
        Else
            m_complete = True
        End If
    End Sub

    Public Shared Sub UpdateTask(ByRef taskObject As Task)

        Dim TaskXMLFile As XmlDocument = New XmlDocument()
        Dim Tasks As XmlNode
        Try
            TaskXMLFile.Load(My.Settings.XmlFileLocation)
            Tasks = TaskXMLFile.DocumentElement ' Loads the document element.  In this case, Tasks
            Dim newItemFlag As Boolean = True
            For Each Task As XmlNode In Tasks.SelectNodes("//Task")
                If Task.Attributes("id").Value = taskObject.m_id Then
                    Dim type As New Task.TaskType
                    Task.Item("Desc").InnerText = taskObject.m_desc
                    Task.Item("Type").InnerText = taskObject.m_type
                    Task.Item("Ticket").InnerText = taskObject.m_ticket
                    Task.Item("Link").InnerText = taskObject.m_link
                    Task.Item("Notes").InnerText = taskObject.m_notes
                    Task.Item("Complete").InnerText = taskObject.m_complete.ToString.ToLower

                    TaskXMLFile.Save(My.Settings.XmlFileLocation)
                    newItemFlag = False ' not a new item, set flag
                End If

            Next
            If newItemFlag Then ' if it's new, add it
                AddNewTaskToXML(taskObject)
            End If
        Catch ex As XmlException
        End Try
    End Sub

    Public Shared Sub AddNewTaskToXML(ByRef taskObject As Task)
        Dim TaskXMLFile As XmlDocument = New XmlDocument
        Dim Root As XmlNode

        TaskXMLFile.Load(My.Settings.XmlFileLocation)
        Root = TaskXMLFile.DocumentElement

        Dim newTask As XmlElement = TaskXMLFile.CreateElement("Task")
        newTask.SetAttribute("id", taskObject.id.ToString)
        ' Set up the child nodes, append to newTask
        Dim newDesc As XmlElement = TaskXMLFile.CreateElement("Desc")
        newDesc.InnerText = taskObject.desc
        newTask.AppendChild(newDesc)

        Dim newType As XmlElement = TaskXMLFile.CreateElement("Type")
        newType.InnerText = taskObject.type
        newTask.AppendChild(newType)

        Dim newTicket As XmlElement = TaskXMLFile.CreateElement("Ticket")
        newTicket.InnerText = taskObject.ticket
        newTask.AppendChild(newTicket)

        Dim newLink As XmlElement = TaskXMLFile.CreateElement("Link")
        newLink.InnerText = taskObject.link
        newTask.AppendChild(newLink)

        Dim newNotes As XmlElement = TaskXMLFile.CreateElement("Notes")
        newNotes.InnerText = taskObject.notes
        newTask.AppendChild(newNotes)

        Dim newComp As XmlElement = TaskXMLFile.CreateElement("Complete")
        newComp.InnerText = taskObject.complete.ToString.ToLower
        newTask.AppendChild(newComp)

        'Append the XML object to Root
        Root.AppendChild(newTask)

        TaskXMLFile.Save(My.Settings.XmlFileLocation)
        MainWindow.AddTaskToUI(taskObject)
    End Sub


    Public Shared Sub RemoveTask(ByRef taskObject As Task)
        MessageBox.Show("Not yet implemented")
    End Sub
    Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub OnPropertyChanged(ByVal e As PropertyChangedEventArgs)
        RaiseEvent PropertyChanged(Me, e)
    End Sub

End Class

