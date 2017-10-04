Imports Microsoft.VisualBasic.PowerPacks.LineShape
Public Class sales

    Private Sub sales_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DateTimePicker2.MaxDate = System.DateTime.Now.Date
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Dim ds As New DataSet
            Dim Db As New SQLiteConnection("Data Source=./test.db;Pooling=true;FailIfMissing=false")
            Db.Open()
            Dim S As String
            S = "Select sum(PRICE) from invoice where date=4/6/17"
            Dim Cmd As New SQLiteCommand(S, Db)
            Dim sum As Integer = Cmd.ExecuteScalar
            Chart1.Series("Sales").Points.AddXY("April", sum)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class