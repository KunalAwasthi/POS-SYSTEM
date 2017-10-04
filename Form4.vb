Imports System.Data.SQLite
Public Class Form4
    Dim Result As String
    Public ds As New DataTable()
    Private Sub Form4_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label1.Text = "Customer's Name"
        Label2.Text = "Customer's TIN NO"
        Button1.Text = "Serach"

    End Sub

    Public Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Dim Db As New SQLiteConnection("Data Source=./test.db;Pooling=true;FailIfMissing=false")
            Db.Open()
            ds.Clear()
            Dim S As String
            S = String.Format("select Tin,name,'E-Mail',Phone from N WHERE Name LIKE '{0}%' OR Tin LIKE '{1}';", TextBox1.Text, TextBox2.Text)
            Dim sqldr As New SQLiteDataAdapter(S, Db)
            sqldr.Fill(ds)
            If (ds IsNot Nothing) Then
                Form5.ShowDialog()
            End If
            Db.Close()
        Catch ex As Exception
            MsgBox("Something Went Wrong..!!" + ex.Message)
        End Try
    End Sub
End Class