Imports System.Data.SQLite
Imports System.Text
Imports System.Net.Mail
Imports System.Text.RegularExpressions
Public Class Form2
    Shared Function is_Numeric(ByVal text As String)
        Dim count As Integer = 0
        Dim c As Char = ""
        Dim b As Integer = 0
        For b = 0 To text.Length - 1
            c = text.Chars(b)
            If (Char.IsNumber(c)) Then
                Return False
            End If
        Next
        Return True
    End Function
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim S As String
        Try
            If (TextBox1.Text IsNot Nothing And TextBox3.Text IsNot Nothing And TextBox4.Text IsNot Nothing And TextBox5.Text IsNot Nothing And MaskedTextBox3.Text IsNot Nothing And MaskedTextBox4.Text IsNot Nothing And MaskedTextBox6.Text IsNot Nothing And MaskedTextBox7.Text IsNot Nothing And emailaddresscheck(TextBox5.Text) = True) Then
                Dim Db As New SQLiteConnection()
                Db.ConnectionString = "Data Source=./test.db;Pooling=true;FailIfMissing=false"
                Db.Open()
                Dim address As String = TextBox1.Text & TextBox2.Text & "," & MaskedTextBox4.Text & "," & TextBox3.Text & "" & MaskedTextBox6.Text
                'this query is to add customers 
                S = String.Format("Insert Into N(tin,name,address,'e-mail',phone) Values ('{0}','{1}','{2}','{3}',{4});", MaskedTextBox3.Text, TextBox4.Text, address, TextBox5.Text, MaskedTextBox7.Text)
                Dim cmmd As New SQLiteCommand(S)
                cmmd.Connection = Db
                cmmd.ExecuteReader()
                S = ""
                MsgBox("Customer Added to the database.")

                Dim dataset2 As New DataSet
                S = "select name from N"
                Dim sqldr As New SQLiteDataAdapter(S, Db)
                sqldr.Fill(dataset2)
                Form1.ComboBox1.DataSource = dataset2.Tables(0)
                Form1.ComboBox1.DisplayMember = "name"
                Db.Close()
                Me.Close()
            Else
                MsgBox("Please Fill all the fields or you entered wrong inputs !!!!")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub MaskedTextBox8_MaskInputRejected(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox5.TextChanged
        Dim temp As String
        temp = TextBox5.Text
        If emailaddresscheck(temp) = False Then
            Label2.Text = "Please enter your email address correctly"
            TextBox5.BackColor = Color.MediumVioletRed
        Else
            TextBox5.BackColor = Color.Green
            Label2.Text = "Valid Mail Address"
        End If
    End Sub
    Private Function emailaddresscheck(ByVal emailaddress As String) As Boolean
        Dim pattern As String = "\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
        Dim emailAddressMatch As Match = Regex.Match(emailaddress, pattern)
        If emailAddressMatch.Success Then
            emailaddresscheck = True
        Else
            emailaddresscheck = False
        End If
    End Function
End Class
