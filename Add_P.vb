Imports System.Data.SQLite
Public Class Add_P
    Private Sub Add_P_Load_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim Db As New SQLiteConnection()
            Dim S As String
            Dim ds As New DataSet
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
            Db.Close()
        Catch ex As Exception
            MsgBox("We Encountered a Error While Connceting to System Files", " Please Contact Technical support...@8005836230")
        End Try
    End Sub
    '
    '
    'this sub will refresh the combobox1 if user clicks PictureBox1
    '
    '
    '
    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        Try
            Dim Db As New SQLiteConnection()
            Dim S As String
            Dim ds As New DataSet
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
            Db.Close()
        Catch ex As Exception
            MsgBox("We Encountered a Error While Connceting to System Files Please Contact Technical support...")
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim S As String = ""
        Try
            If (TextBox1.Text IsNot "" And TextBox2.Text IsNot "" And TextBox3.Text IsNot "" And TextBox4.Text IsNot "") Then
                Dim Db As New SQLiteConnection()
                Db.ConnectionString = "Data Source=./test.db;Pooling=true;FailIfMissing=false"
                Db.Open()
                'this query is to add customers 
                Try
                    Dim tax As Double = 0.0
                    If (TextBox4.Text <> "") Then
                        tax = Convert.ToDouble(TextBox3.Text) * Convert.ToDouble(TextBox4.Text) / 100
                        tax += Convert.ToDouble(TextBox3.Text)
                    End If
                    S = String.Format("insert into Products (PID,PN,PRICE,Measurement,PDESC,TAX,STOCK) Values ('{0}','{1}','{2}','{3}','{4}',{5},{6});", TextBox1.Text, TextBox2.Text, tax, ComboBox1.Text, RichTextBox1.Text, TextBox4.Text, TextBox5.Text)
                    Dim cmmd As New SQLiteCommand(S, Db)
                    cmmd.ExecuteNonQuery()
                Catch exx As Exception
                    MsgBox(exx.Message)
                End Try
                Db.Close()
            Else
                MsgBox("Please Fill all the fields or you entered wrong Details.")
            End If

        Catch ex As Exception
            MsgBox(ex.Message + "please Fill all the fields Correctly")
        End Try
    End Sub
End Class