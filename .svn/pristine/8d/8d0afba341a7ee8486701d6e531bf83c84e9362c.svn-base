Imports System.Text
Imports System.Xml
Imports System.Xml.Schema
Public Class XmlHandlers

    Public Shared Function ValidateXMLFile(ByVal TaskFile As String) As Boolean
        Try
            Dim xmlFile As New XmlDocument()
            Dim errorBuilder As New XmlValidationErrorBuilder()
            xmlFile.Load(TaskFile)
            xmlFile.Schemas.Add(Nothing, "./TaskFile.xsd")
            xmlFile.Validate(New ValidationEventHandler(AddressOf errorBuilder.ValidationEventHandler))
            Dim errorsText As String = errorBuilder.GetErrors()
            If errorsText Is Nothing Then
                Return True
            Else
                MessageBox.Show(errorBuilder.GetErrors)
                Return False
            End If
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function CreateNewTaskXMLFile() As Boolean
        If My.Computer.FileSystem.FileExists(My.Settings.XmlFileLocation) Then
            Try
                'MessageBox.Show("File " & My.Settings.XmlFileLocation & " already exists.")
                Return False
            Catch ex As Exception
                MessageBox.Show("Problem opening a messagebox.  This message attempt is thereby futile.")
                Return False
            End Try

        Else
            Try
                Dim TaskFile As New XmlDocument

                Dim taskElement As XmlElement = TaskFile.CreateElement("Tasks")
                TaskFile.AppendChild(taskElement)

                Dim subElement As XmlElement = TaskFile.CreateElement("Task")
                subElement.SetAttribute("id", "1")
                For Each taskChild As String In New String() {"Desc", "Type", "Ticket", "Link", "Notes"}
                    Dim childElement As XmlElement = TaskFile.CreateElement(taskChild)
                    If taskChild = "Desc" Then childElement.InnerText = "Sample Task.  Click to edit"
                    subElement.AppendChild(childElement)
                Next
                Dim completeElement As XmlElement = TaskFile.CreateElement("Complete")
                completeElement.InnerText = "false"
                subElement.AppendChild(completeElement)
                taskElement.AppendChild(subElement)
                TaskFile.Save(My.Settings.XmlFileLocation)
                Return True
            Catch ex As Exception
                MessageBox.Show("Error creating a new XMl Task List")
                Return False
            End Try
        End If
    End Function

End Class

Public Class XmlValidationErrorBuilder
    Private _errors As New List(Of ValidationEventArgs)()

    Public Sub ValidationEventHandler(ByVal sender As Object, ByVal args As ValidationEventArgs)
        If args.Severity = XmlSeverityType.Error Then
            _errors.Add(args)
        End If
    End Sub

    Public Function GetErrors() As String
        If _errors.Count <> 0 Then
            Dim builder As New StringBuilder()
            builder.Append("The following ")
            builder.Append(_errors.Count.ToString())
            builder.AppendLine(" error(s) were found while validating the XML document against the XSD:")
            For Each i As ValidationEventArgs In _errors
                builder.Append("* ")
                builder.AppendLine(i.Message)
            Next
            Return builder.ToString()
        Else
            Return Nothing
        End If
    End Function
End Class
