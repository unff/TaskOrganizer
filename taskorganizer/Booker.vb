﻿Imports System.Globalization
Imports System.IO
Imports System.Net

' DEFUCT CLASS AS WE NO LONGER USE BOOKER


Public Class Booker
    Private Function GetData() As String

        Dim address As String = "https://app.secure-booker.com/App/Calendar.ashx?data=24o8unL8T2%2FG2Bus1yWhDwqsjS0r60zlB2wwyS0s1Z%2FDQWRmVMz4ohSFj2M8viLfHw%3D%3D"
        Dim client As WebClient = New WebClient()
        Dim reader As StreamReader = New StreamReader(client.OpenRead(address))
        Return reader.ReadToEnd

    End Function

    Public Shared Sub CreateTasksFromBooker()
        Dim x As New Booker
        Dim bookerData As String = x.GetData()
        Dim currentLine As String = String.Empty
        Dim strReader As New StringReader(bookerData)
        Dim count = 0
        Try


            While True
                currentLine = strReader.ReadLine()
                If currentLine Is Nothing Then Exit While

                Dim lineParts As String() = currentLine.Split(New Char() {":"c}, 2)
                If lineParts(0).Trim.ToUpper = "BEGIN" And lineParts(1).Trim.ToUpper = "VEVENT" Then
                    ' Create a new task, read the next line and start assigning lines to the task.
                    Dim oldEvent As Boolean = False
                    Dim bookerTask As New Task
                    bookerTask.id = count - 1
                    count = count - 1
                    bookerTask.type = "Booker Appointment"
                    Dim noteAccumulator As String = String.Empty
                    While True
                        currentLine = strReader.ReadLine()
                        If currentLine.Trim.ToUpper = "END:VEVENT" Then Exit While
                        lineParts = currentLine.Split(New Char() {":"c}, 2)
                        noteAccumulator += vbCrLf + currentLine
                        Select Case lineParts(0)
                            Case "DTSTART"
                                Dim dtstart As Date = Date.ParseExact(lineParts(1).Trim, "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture).ToUniversalTime
                                If dtstart.ToLocalTime < Date.Now Then oldEvent = True
                                'Dim ststart As Date = Date.ParseExact(lineParts(1),))
                                bookerTask.ticket = dtstart.ToLocalTime.ToString("MM/dd HH:mm")
                            Case "X-ALT-DESC;FMTTYPE=text/html"
                                lineParts(1) = lineParts(1).Remove(0, 77) ' trim the HTML trash
                                bookerTask.desc = lineParts(1).Substring(0, lineParts(1).IndexOf("("c))
                            Case Else
                                Continue While
                        End Select

                    End While
                    bookerTask.notes = noteAccumulator
                    If Not oldEvent Then MainWindow.AddTaskToUI(bookerTask)

                End If


            End While

        Catch ex As Exception

        End Try

    End Sub

End Class
'Function get_data($url) {
'    $ch = curl_init();
'    $timeout = 5;
'    //curl_setopt($ch, CURLOPT_PROXY, '127.0.0.1:8888');
'    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, 0);
'    curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, 0);
'    curl_setopt($ch, CURLOPT_URL, $url);
'    curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
'    curl_setopt($ch, CURLOPT_CONNECTTIMEOUT, $timeout);
'    $data = curl_exec($ch);
'    curl_close($ch);
'    Return $data;
'  }
'  $rebuild = '';
'  $a = explode(" \ r \ n Then",get_data('https://app.secure-booker.com/App/Calendar.ashx?data=24o8unL8T2%2FG2Bus1yWhDwqsjS0r60zlB2wwyS0s1Z%2FDQWRmVMz4ohSFj2M8viLfHw%3D%3D'));

'  foreach($a as $value){
'      $c=substr($value,0,11);
'      If (substr($value, 0, 11) =='DESCRIPTION'){
'                continue;
'      }
'      If (substr($value, 0, 8) =='LOCATION'){
'                continue;
'      }
'      If (substr($value, 0, 7) =='SUMMARY'){
'                continue;
'      }
'      If (substr($value, 0, 11) =='X-ALT-DESC;'){
'          $skip=1;
'          $b=strip_tags($value,'<br>');
'          $d=explode("<BR>",substr($b,29));
'          foreach($d as $val){
'              $f=explode(":",$val,2);
'              $$f[0]=$f[1];
'          }
'              $rebuild.="LOCATION:".$Room."\r\n";
'              $rebuild.="DESCRIPTION:".$Customer."\r\n";
'              $g=substr($Customer,0,strpos($Customer,"(",0));
'              $rebuild.="SUMMARY:".$g."\r\n";
'              Continue Do;
'      }
'      If ($value=='END:VEVENT'){
'          $rebuild.="BEGIN:VALARM\r\nTRIGGER:-PT15M\r\nACTION:DISPLAY\r\nDESCRIPTION:Reminder\r\nEND:VALARM\r\n";
'          $rebuild.=$value."\r\n";
'          Continue Do;
'      }
'          $rebuild.=$value."\r\n";
'  }
'  echo $rebuild;

