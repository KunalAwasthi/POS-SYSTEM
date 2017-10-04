Public Class Form5

    Public Sub Form5_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DataGridView1.DataSource = Form4.ds
    End Sub
End Class