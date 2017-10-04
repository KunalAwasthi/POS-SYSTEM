Imports System.Data.SQLite
Public Class EditB
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If (TextBox1.Text.Length = 9) Then
            Try
                Dim ds As New DataSet
                Dim Db As New SQLiteConnection("Data Source=./test.db;Pooling=true;FailIfMissing=false")
                Db.Open()
                Dim S As String
                S = String.Format("select * from billnumber inner join invoice on Billnumber.bill = invoice.inv_no where inv_no = '{0}';", TextBox1.Text)
                Dim sqldr As New SQLiteDataAdapter(S, Db)
                sqldr.Fill(ds)
                DataGridView1.DataSource = ds.Tables(0)
                ComboBox1.Text = ds.Tables(0).Rows(0)(1)
            Catch ex As Exception
                MsgBox("error")
            Finally
                TextBox1.Text = ""
            End Try
        End If
    End Sub

    Private Sub EditB_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "open bill"
    End Sub
End Class