Imports System.Data.SQLite ' Mainline man
Imports System.Drawing.Printing 'for printing
Imports System.Text 'didn't remember 
Imports GenCode128 'this import will be used to generate the barcode on the printed bill......
Public Class Form1
    Dim sum As Double = 0 'global variable for the end total of the bill
    Dim ds As New DataSet 'not specific work but used over the subroutines 
    Dim temo As Double
    Dim billnocounter As Integer 'counter that will generate the bill number and gets update when the 
    'print operation has been completed....
    '
    '
    'Form_Load Events and Logs...
    '
    '
    '
    '
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '
        'add system date to form
        '
        '
        Label4.Text = System.DateTime.Now
        '
        '
        '
        'datagrid Setting
        DataGridView1.ForeColor = Color.White
        '
        '
        '
        Label2.Text = ""
        TextBox1.ReadOnly = True
        '
        '
        '
        'Generate a new Bill Number
        Dim flag As Integer = 0
        While (flag = 0)
            Dim st As String = billNO_GEN().ToString
            If (check_availablity(st) = True) Then
                flag = 1
                Label2.Text = st
            End If
        End While
        '
        '
        'It will gets all Customer Name in Combobox.
        '
        '
        Try
            Dim dataset2 As New DataSet
            Dim Db As New SQLiteConnection("Data Source=./test.db;Pooling=true;FailIfMissing=false")
            Db.Open()
            Dim S As String
            S = "select name from N"
            Dim sqldr As New SQLiteDataAdapter(S, Db)
            sqldr.Fill(dataset2)
            ComboBox1.DataSource = dataset2.Tables(0)
            ComboBox1.DisplayMember = "name"
        Catch ex As Exception
            MsgBox("ERROR" + ex.Message)
        End Try
        '
        '
        '
        DataGridView1.ForeColor = Color.Black
        '
        '
        '
        'this query will put the status data in combobox
        '
        '
        Try
            Dim dataset3 As New DataSet
            Dim Db As New SQLiteConnection("Data Source=./test.db;Pooling=true;FailIfMissing=false")
            Db.Open()
            Dim S As String
            S = "select STATUS from STATUS"
            Dim sqldr As New SQLiteDataAdapter(S, Db)
            sqldr.Fill(dataset3)
            ComboBox2.DataSource = dataset3.Tables(0)
            ComboBox2.DisplayMember = "STATUS"
        Catch ex As Exception
            MsgBox("ERROR" + ex.Message)
        End Try

    End Sub
    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        If (TextBox3.Text.Length = 3) Then
            Try
                Dim Db As New SQLiteConnection("Data Source=./test.db;Pooling=true;FailIfMissing=false")
                Db.Open()
                Dim S As String
                S = String.Format("select * from Products WHERE PID='{0}' &&  STOCK > 0;", TextBox3.Text)
                Dim sqldr As New SQLiteDataAdapter(S, Db)
                sqldr.Fill(ds)
                DataGridView1.DataSource = ds.Tables(0)
                sum = 0
                For index As Integer = 0 To DataGridView1.RowCount - 1
                    sum += Convert.ToInt32(DataGridView1.Rows(index).Cells("PRICE").Value)
                Next
                Db.Close()
                TextBox1.Text = sum.ToString
                TextBox3.Focus()
                TextBox2.Text = gen_change(sum).ToString
            Catch ex As Exception
            Finally
                TextBox3.Text = ""
            End Try

        End If
    End Sub

    Private Sub NumericUpDown2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown2.ValueChanged
        If Convert.ToInt32(NumericUpDown2.Text) <> 0 Then
            Dim discount As Integer = Convert.ToDouble(NumericUpDown2.Text)
            Dim total As Integer = Convert.ToDouble(sum)
            total = total * discount / 100
            TextBox1.Text = Convert.ToDouble(sum - total)
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        PrintPreviewDialog1.ShowDialog()
        If (DataGridView1.Rows.Count > 0) Then
            Dim printing As DialogResult = PrintDialog1.ShowDialog
            If (printing = Windows.Forms.DialogResult.OK) Then
                Try
                    Dim Db As New SQLiteConnection("Data Source=./test.db;Pooling=true;FailIfMissing=false")
                    Db.Open()
                    Dim d1 As String = ""
                    Dim d2 As Integer = 0
                    Dim d3 As String = ""
                    Dim d4 As Integer = 0
                    Dim d5 As String = ""
                    Dim d6 As String = ""
                    Dim S1 As String = ""
                    Dim d8 As String = System.DateTime.Now.Date
                    Dim d7 As String = ComboBox1.Text
                    For index As Integer = 0 To DataGridView1.RowCount - 1
                        For index2 As Integer = 0 To 6
                            If (index2 = 0) Then d2 = Convert.ToInt32(DataGridView1.Rows(index).Cells(index2).Value)
                            If (index2 = 1) Then d3 = Convert.ToString(DataGridView1.Rows(index).Cells(index2).Value)
                            If (index2 = 2) Then d4 = Convert.ToInt32(DataGridView1.Rows(index).Cells(index2).Value)
                            If (index2 = 3) Then d5 = Convert.ToString(DataGridView1.Rows(index).Cells(index2).Value)
                            If (index2 = 4) Then d6 = Convert.ToString(DataGridView1.Rows(index).Cells(index2).Value)
                        Next index2
                        d1 = Label2.Text 'this is the billnumber
                        S1 = String.Format("insert into invoice (inv_no,PID,PN,PRICE,MAJOR,PDESC,customer,Date) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", d1, d2, d3, d4, d5, d6, d7, d8)
                        Dim sql As New SQLiteCommand(S1, Db)
                        sql.ExecuteScalar()
                    Next index
                    Db.Close()
                Catch ex As Exception
                    MsgBox(" After printing there was an Error :" & ex.Message & " \n Please Contact Developers soon ")
                End Try
                '
                '<------this will increment the counter by 1 ------>
                '<------------------------------------------------->
                Try
                    Dim Db1 As New SQLiteConnection("Data Source=./test.db;Pooling=true;FailIfMissing=false")
                    Db1.Open()
                    Dim S As String = ""
                    For counter As Integer = 0 To 1
                        If (counter = 0) Then S = String.Format("delete from counter WHERE counter='{0}';", billnocounter)
                        If (counter = 1) Then
                            billnocounter = billnocounter + 1
                            S = String.Format("insert into counter (counter) values ('{0}');", billnocounter)
                        End If
                        Dim sql As New SQLiteCommand(S, Db1)
                        sql.ExecuteScalar()
                    Next

                    S = String.Format("insert into BILLNUMBER (BILL,Status,customer) values ('{0}','{1}','{2}');", Label2.Text, ComboBox2.Text, ComboBox1.Text)
                    Dim cmd As New SQLiteCommand(S, Db1)
                    cmd.ExecuteScalar()
                    Controls.Clear()
                    ' Invenotry_Change(ds)
                    ds.Clear()
                    Me.InitializeComponent()
                    Label4.Text = System.DateTime.Now
                    Dim flag As Integer = 0
                    Dim fl As Integer = 0
                    While (fl = 0)
                        Dim st As String = billNO_GEN()
                        If (check_availablity(st) = True) Then
                            fl = 1
                            Label2.Text = st
                        End If
                    End While
                    Dim da As New DataSet
                    S = "select STATUS from STATUS"
                    Dim sqldr As New SQLiteDataAdapter(S, Db1)
                    sqldr.Fill(da)
                    ComboBox2.DataSource = da.Tables(0)
                    ComboBox2.DisplayMember = "STATUS"
                Catch ex As Exception
                    MsgBox("ERROR     " + ex.Message)
                End Try
            End If
        End If
    End Sub
    Private Sub DataGridView1_rowDelete(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridView1.UserDeletedRow
        Try
            If (DataGridView1.Rows.Count <= 1) Then 'if no rows found
                TextBox1.Text = 0
            Else 'obvi case:2 
                sum = 0
                For index As Integer = 0 To DataGridView1.RowCount - 1
                    sum += Convert.ToDouble(DataGridView1.Rows(index).Cells("PRICE").Value)
                Next
                TextBox1.Text = sum.ToString
            End If
        Catch ex As Exception

        End Try
    End Sub
    '
    '
    '
    '
    '
    'this function will generate the total CHANGE to BE Given 
    Public Function gen_change(ByVal sum As Double)
        Dim curen() As Integer = {1, 2, 5, 10, 20, 50, 100, 500, 2000}
        Dim change As Double = 0.0
        For counter As Integer = 0 To curen.Length - 1
            If (sum <= curen(counter) And sum < 2000) Then
                change = curen(counter)
                Exit For
            End If
        Next counter
        Return change - sum & " ON " & change
    End Function
    '
    '
    '
    'Function ends...
    '
    '
    '
    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim rc_ As Integer = 180
        Dim cc_ As Integer = 10
        For index As Integer = 0 To DataGridView1.ColumnCount - 4
            cc_ += 80
        Next
        cc_ = 10
        For index As Integer = 0 To DataGridView1.RowCount - 2
            For j As Integer = 0 To DataGridView1.ColumnCount - 4
                cc_ = 80 + cc_
            Next
            rc_ += 35
            cc_ = 10
        Next
        PrintDocument1.DefaultPageSettings.PaperSize = New PaperSize("PAge", 250, rc_ + 190)
        Dim banner As String = "Yash Mobile Shop"
        Dim rc As Integer = 180
        Dim cc As Integer = 10
        e.Graphics.DrawString(banner, New Font("Arial", 13, FontStyle.Regular And FontStyle.Italic), Brushes.Black, 40, 10)
        e.Graphics.DrawString("--------------------------", New Font("Arial", 10, FontStyle.Regular), Brushes.Black, 40, 30)
        e.Graphics.DrawString("Bill no : " + Label2.Text & " [ " & ComboBox2.Text & " ]", New Font("Arial", 8, FontStyle.Italic), Brushes.Black, 10, 80)
        e.Graphics.DrawString("Customer : " & ComboBox1.Text, New Font("Arial", 8, FontStyle.Italic), Brushes.Black, 10, 100)
        e.Graphics.DrawString("Date & Time : " + Convert.ToDateTime(System.DateTime.Now), New Font("Arial", 8, FontStyle.Italic And FontStyle.Bold), Brushes.Black, 10, 120)
        e.Graphics.DrawString("--------------------------------------------------------", New Font("Arial", 10, FontStyle.Regular), Brushes.Black, 0, 150)
        For index As Integer = 0 To DataGridView1.ColumnCount - 4
            e.Graphics.DrawString(DataGridView1.Columns(index).Name.ToString, New Font("Arial", 8, FontStyle.Regular And FontStyle.Italic), Brushes.Black, cc, 170)
            cc += 80
        Next
        cc = 10
        For index As Integer = 0 To DataGridView1.RowCount - 2
            For j As Integer = 0 To DataGridView1.ColumnCount - 4
                e.Graphics.DrawString(Trim(DataGridView1.Rows(index).Cells(j).Value), New Font("Arial", 8, FontStyle.Regular And FontStyle.Italic), Brushes.Black, cc, rc)
                cc = 80 + cc
            Next
            rc += 35
            cc = 10
        Next
        Dim inage As Image = Code128Rendering.MakeBarcodeImage(Label2.Text, Convert.ToInt32(1), True)
        e.Graphics.DrawString("--------------------------------------------------------", New Font("Arial", 10, FontStyle.Regular), Brushes.Black, 0, rc + 20)
        e.Graphics.DrawString("Grand Total", New Font("Arial", 10, FontStyle.Regular And FontStyle.Bold), Brushes.Black, 10, rc + 40)
        e.Graphics.DrawString(TextBox1.Text & " @ Discount " & NumericUpDown2.Value & "%", New Font("Arial", 8, FontStyle.Regular And FontStyle.Bold), Brushes.Black, 130, rc + 40)
        e.Graphics.DrawString("---------------------------------------------------------", New Font("Arial", 10, FontStyle.Regular), Brushes.Black, 0, rc + 80)
        e.Graphics.DrawImage(inage, 40, rc + 160)
        PrintDocument1.DefaultPageSettings.PaperSize = New PaperSize("PAge", 250, rc + 190)
    End Sub
    Function billNO_GEN()
        Dim final As String
        Try
            Dim Db As New SQLiteConnection("Data Source=./test.db;Pooling=true;FailIfMissing=false")
            Db.Open()
            Dim S As String
            S = "select * from counter"
            Dim sql As New SQLiteCommand(S, Db)
            billnocounter = Convert.ToInt32(sql.ExecuteScalar)
            Db.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Dim sb As New StringBuilder
        sb.Append("17-")
        sb.Append(System.DateTime.Now.Month & "-")
        If billnocounter < 10 Then
            sb.Append("000" & billnocounter)
        ElseIf billnocounter > 9 And billnocounter < 99 Then
            sb.Append("00" & billnocounter)
        ElseIf billnocounter >= 100 And billnocounter < 1000 Then
            sb.Append("0" & billnocounter)
        ElseIf billnocounter >= 1000 Then
            sb.Append(billnocounter)
        End If
        final = sb.ToString
        Return final
    End Function
    '
    '
    '
    'checks weather a bill number is used or not... 
    Public Function check_availablity(ByVal st As String)
        Dim dataset1 As New DataTable
        Dim Db As New SQLiteConnection("Data Source=./test.db;Pooling=true;FailIfMissing=false")
        Db.Open()
        Dim S As String
        S = String.Format("select bill from BILLNUMBER WHERE BILL='{0}';", st)
        Dim sqldr As New SQLiteDataAdapter(S, Db)
        sqldr.Fill(dataset1)
        If dataset1.Rows.Count > 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub DataGridView1_RowAdded(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridView1.RowsAdded
        Label10.Text = DataGridView1.Rows.Count - 1
    End Sub
    'Customer_SearchCustomer_Press Event...
    Private Sub SelectCustomersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectCustomersToolStripMenuItem.Click
        Form4.ShowDialog()
    End Sub
    'Customer_AddCustomer_Press Event..
    Private Sub AddCustomersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddCustomersToolStripMenuItem.Click
        Form2.ShowDialog()
    End Sub
    'Menu_Files_Exit_Press Event...
    Private Sub ExitAltF4ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitAltF4ToolStripMenuItem.Click
        Me.Close()
    End Sub

    'Menu_Quit_Button_Press Event...
    Private Sub QuitAltF4ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles QuitAltF4ToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub AddProductsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddProductsToolStripMenuItem.Click
        Add_P.ShowDialog()
    End Sub

    Private Sub AddMajormentToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddMajormentToolStripMenuItem.Click
        Add_major.ShowDialog()
    End Sub
    Private Sub NewBillToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewBillToolStripMenuItem.Click
        Controls.Clear()
        Me.InitializeComponent()
        Label4.Text = System.DateTime.Now
        ds.Clear()
        Dim f As Integer = 0
        While (f = 0)
            Dim st As String = billNO_GEN().ToString
            If (check_availablity(st) = True) Then
                f = 1
                Label2.Text = st
            End If
        End While
        Try
            Dim dataset3 As New DataSet
            Dim Db As New SQLiteConnection("Data Source=./test.db;Pooling=true;FailIfMissing=false")
            Db.Open()
            Dim S As String
            S = "select STATUS from STATUS"
            Dim sqldr As New SQLiteDataAdapter(S, Db)
            sqldr.Fill(dataset3)
            ComboBox2.DataSource = dataset3.Tables(0)
            ComboBox2.DisplayMember = "STATUS"
            Dim dataset2 As New DataSet
            S = "select name from N"
            sqldr = New SQLiteDataAdapter(S, Db)
            sqldr.Fill(dataset2)
            ComboBox1.DataSource = dataset2.Tables(0)
            ComboBox1.DisplayMember = "name"
            Db.Close()

        Catch ex As Exception
            MsgBox("something went Wrong" + ex.Message)
        End Try
    End Sub

    Private Sub OpenBillToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenBillToolStripMenuItem.Click
        OpenB.ShowDialog() 'open a bill...
    End Sub

    Private Sub SalesStatsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesStatsToolStripMenuItem.Click
        EditB.Name = "Open & Edit Bill"
        EditB.ShowDialog() 'open and edit bills...
    End Sub



    'this will show the dialog of sales information form.....

    Private Sub SalesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesToolStripMenuItem.Click
        sales.ShowDialog()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim Db As New SQLiteConnection("Data Source=./test.db;Pooling=true;FailIfMissing=false")
        Db.Open()
        Dim S As String
        S = "select * from N where name='" & ComboBox1.SelectedText.ToString & "'"
        Dim Sqlcmd As SQLiteCommand = New SQLiteCommand(S, Db)
        Dim Sqladp As New SQLiteDataAdapter(S, Db)
        Dim dataset As New DataSet
        Sqladp.Fill(dataset)
        Dim dt As New DataTable("Customer")
        'Label21.Text = 
    End Sub
End Class