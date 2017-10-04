Imports System.Data.SQLite
Public Class Edit_major

    Private Sub Edit_major_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label2.Text = Add_major.ComboBox1.Text
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Try
            Dim Db As New SQLiteConnection()
            Dim S As String
            Dim ds As New DataSet
            Db.ConnectionString = "Data Source=./test.db;Pooling=true;FailIfMissing=false"
            Db.Open()
            'this query is to add customers 
            S = String.Format("Update Majors set Major='{0}' where Major='{1}'", TextBox1.Text, Label2.Text)
            Dim cmmd As New SQLiteCommand(S)
            cmmd.Connection = Db
            cmmd.ExecuteReader()
            Db.Close()
            Me.Close()
        Catch ex As Exception
            MsgBox("We Encountered a Error While Connceting to System Files Please Contact Technical support...")
        End Try
    End Sub
End Class