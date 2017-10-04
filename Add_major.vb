Imports System.Data.SQLite
Imports System.Data
Public Class Add_major
    Dim ds As New DataSet()
    'ADD MAJOR 
    Public Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try
            If (TextBox2.Text <> "") Then
                Dim Db As New SQLiteConnection()
                Dim S As String
                ds.Clear()
                Db.ConnectionString = "Data Source=./test.db;Pooling=true;FailIfMissing=false"
                Db.Open()
                'this query is to add customers 
                S = String.Format("INSERT INTO Majors (Major) VALUES ('{0}');", TextBox2.Text)
                Dim cmmd As New SQLiteCommand(S, Db)
                cmmd.ExecuteReader()
                S = ""
                Db.Close()
            End If
        Catch ex As Exception
            MsgBox("We Encountered a Error While Connceting to System Files Please Contact Technical support...")
        End Try
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Edit_major.ShowDialog()
    End Sub

    Private Sub Add_major_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim Db As New SQLiteConnection()
            Dim S As String
            ds.Clear()
            Db.ConnectionString = "Data Source=./test.db;Pooling=true;FailIfMissing=false"
            Db.Open()
            'this query is to add customers 
            S = String.Format("SELECT * FROM Majors")
            Dim Adp As New SQLiteDataAdapter(S, Db)
            Adp.Fill(ds)
            S = ""
            If (ds IsNot Nothing) Then
                ComboBox1.DataSource = ds.Tables(0)
                ComboBox1.DisplayMember = "Major"
            End If
        Catch ex As Exception
            MsgBox("We Encountered a Error While Connceting to System Files Please Contact Technical support...")
        End Try
    End Sub
End Class