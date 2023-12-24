Imports System.Text.RegularExpressions
Imports System.IO.Compression
Imports System.IO
Imports System.Text

Public Class Form1

    Public filepath = ""
    Public folderpath = ""
    Public processed_folderpath = ""
    Public zipfound As Boolean = False
    Public validextensions As String() = {".zip", ".tar", ".gz", ".bz2", ".gzip", ".tgz"}
    Public folderOrFile As String = ""
    'Open File
    Private Sub OpenToolStripButton_ItemClicked(sender As Object, e As EventArgs) Handles OpenToolStripButton.Click

        'MessageBox.Show("Open Clicked Event")
        folderOrFile = "file"
        If OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            filepath = OpenFileDialog1.FileName

            'MsgBox(filepath, vbOKOnly)
            'MessageBox.Show(filepath, "hello")
            RichTextBox1.Text = System.IO.File.ReadAllText(filepath)

            'ExtractZip(filepath)
        End If
    End Sub

    ' Extract All inner archives

    Public Sub ExtractZip(ByVal zipPath)
        Dim zipfileinfo = New FileInfo(zipPath)
        Dim zipdir = zipfileinfo.DirectoryName
        Dim zipFolder = Path.GetFileNameWithoutExtension(zipPath)
        Dim extract_path = Path.Join(zipdir, zipFolder)
        System.IO.Directory.CreateDirectory(extract_path)
        'Dim regexSearch = New String(Path.GetInvalidFileNameChars()) + New String(Path.GetInvalidPathChars())
        Dim invalidfilechars2() As Char = {"<", ">", "@", "!"}
        Dim invalidchars = New String(invalidfilechars2)
        Dim invalidfilechars = New String(Path.GetInvalidFileNameChars()) + invalidchars
        Dim invalidpathchars = New String(Path.GetInvalidPathChars()) + invalidchars
        Dim fileregx = New Regex(String.Format("[{0}]", Regex.Escape(invalidfilechars)))
        Dim pathregx = New Regex(String.Format("[{0}]", Regex.Escape(invalidpathchars)))

        Using archive As ZipArchive = ZipFile.OpenRead(zipPath)
            For Each entry As ZipArchiveEntry In archive.Entries

                Dim filename = entry.Name

                ' Gets the full path to ensure that relative segments are removed.
                Dim sanitized_entry = fileregx.Replace(filename, "_")

                Dim destinationPath = ""
                ' file is directly underneath the zip, else if entry is inside another subfolder
                If String.Equals(entry.Name, entry.FullName) Then
                    destinationPath = Path.GetFullPath(Path.Combine(extract_path, sanitized_entry))
                Else
                    Dim subdirs As String = entry.FullName.Substring(0, entry.FullName.Length - filename.Length - 1)
                    'replacing invalid characters with underscore _
                    Dim sanitized_path = pathregx.Replace(subdirs, "_")
                    destinationPath = Path.GetFullPath(Path.Combine(extract_path, sanitized_path, sanitized_entry))
                End If

                Dim dest_path_info = New FileInfo(destinationPath).DirectoryName

                If (Not System.IO.Directory.Exists(dest_path_info)) Then
                    System.IO.Directory.CreateDirectory(dest_path_info)
                End If
                If destinationPath.StartsWith(extract_path, StringComparison.Ordinal) Then
                    entry.ExtractToFile(destinationPath, True)
                End If

            Next
        End Using

    End Sub

    Public Function Extract_7zip() As Integer
        ' if archive found call again
        Return 1
    End Function

    Sub GetAllFiles(strPath As String)
        Dim objRoot As New DirectoryInfo(strPath)
        Dim objSubDir As DirectoryInfo
        Dim objFile As FileInfo

        If objRoot.Exists Then
            'if you don't want this to be recursive, remove this for loop
            For Each objSubDir In objRoot.GetDirectories
                GetAllFiles(objSubDir.FullName)
            Next

            'keep this for loop
            For Each objFile In objRoot.GetFiles


                If validextensions.Contains(objFile.Extension) Then

                    Dim name = objFile.Name.Substring(0, objFile.Name.Length - objFile.Extension.Length - 1)
                    Dim dirname = objFile.Directory.FullName
                    Dim dirpath = Path.Combine(dirname, name)
                    RichTextBox2.AppendText(vbTab & "extracting... " & objFile.FullName & vbCrLf)
                    Dim p = New ProcessStartInfo()
                    p.FileName = "7z_cli.exe"
                    p.Arguments = "x -r " & objFile.FullName & " -o" & dirpath & " -aoa"
                    p.WindowStyle = ProcessWindowStyle.Hidden
                    p.CreateNoWindow = True
                    Dim x = Process.Start(p)
                    x.WaitForExit()
                    GetAllFiles(dirpath)  'calling to check if the extracted archive contains sub archives
                Else
                    RichTextBox1.AppendText(objFile.FullName & vbCrLf)
                End If
            Next
        End If

    End Sub

    'Open Folder
    Private Sub OpenToolStripButton1_ItemClicked(sender As Object, e As EventArgs) Handles OpenToolStripButton1.Click

        'MessageBox.Show("Open Clicked Event")

        If FolderBrowserDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            folderpath = FolderBrowserDialog1.SelectedPath
            folderOrFile = "folder"
            RichTextBox1.ReadOnly = True
            GetAllFiles(folderpath)
            MessageBox.Show("Click Run to process Extracted Files", "Extract Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'MsgBox(filepath, vbOKOnly)
            'MessageBox.Show(folderpath, "hello")

            'RichTextBox1.Text = System.IO.File.ReadAllText(filepath)
            Return
            Dim directoryinfo As New DirectoryInfo(folderpath)
            Dim filesinfo As FileInfo() = directoryinfo.GetFiles()
            Dim listdirs As New System.Text.StringBuilder()
            Dim listfiles As New System.Text.StringBuilder()
            For Each folder In My.Computer.FileSystem.GetDirectories(folderpath)
                listdirs.Append(folder & vbCrLf)
            Next
            RichTextBox1.Text = listdirs.ToString()
            RichTextBox1.SelectAll()
            RichTextBox1.SelectionColor = Color.FromArgb(229, 192, 123)
            'RichTextBox1.SelectionFont = New Font(RichTextBox1.Font, FontStyle.Bold)
            For Each file In filesinfo
                listfiles.Append(file.FullName & vbCrLf)
            Next
            Dim selectionstart = RichTextBox1.Text.Length
            RichTextBox1.AppendText(listfiles.ToString())

            'RichTextBox1.SelectionStart = selectionstart
            'RichTextBox1.SelectionLength = RichTextBox1.Text.Length - selectionstart
            'RichTextBox1.SelectionColor = Color.Navy




        End If
    End Sub


    Private Sub RichTextBox1_VScroll(sender As Object, e As EventArgs) Handles RichTextBox1.VScroll
        Dim nPos = GetScrollPos(RichTextBox1.Handle, ScrollBarType.SbVert)
        nPos <<= 16
        Dim wParam = ScrollBarCommands.SB_THUMBPOSITION Or nPos
        PostMessageA(RichTextBox2.Handle, Message.WM_VSCROLL, New IntPtr(wParam), New IntPtr(0))
    End Sub

    Private Sub RichTextBox1_HScroll(sender As Object, e As EventArgs) Handles RichTextBox1.HScroll
        Dim nPos = GetScrollPos(RichTextBox1.Handle, ScrollBarType.SbHorz)
        nPos <<= 16
        Dim wParam = ScrollBarCommands.SB_THUMBPOSITION Or nPos
        PostMessageA(RichTextBox2.Handle, Message.WM_HSCROLL, New IntPtr(wParam), New IntPtr(0))
    End Sub


    'Date Regex Click
    Private Sub DataGridView1_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged
        If DataGridView1.Rows.Count > 1 Then

            If DataGridView1.Columns(e.ColumnIndex).Index = 0 Then
                If DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = "Date Regex" Then

                    MessageBox.Show("example([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])) to match yyyy-mm-dd ", "Enter Regex Expression", MessageBoxButtons.OK)
                    DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex + 1).Value = "([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))"
                    'Dim regex = New Regex("([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))")
                    'Dim match As Match = regex.Match("ijuiuiu 2021-12-20 kljkj   jkjkjj")
                    'If match.Success Then
                    'MsgBox("ok")
                    'End If
                End If
            End If
        End If
    End Sub

    ' Committing dirty state. user directly clicking save button for example without clicking any adjacent cell in datagridview
    ' https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.datagridview.currentcelldirtystatechanged?redirectedfrom=MSDN&view=net-5.0
    Sub dataGridView1_CurrentCellDirtyStateChanged(
    ByVal sender As Object, ByVal e As EventArgs) Handles DataGridView1.CurrentCellDirtyStateChanged


        If DataGridView1.IsCurrentCellDirty Then
            DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    'Select All
    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        Dim caller = CType(CType(ToolStripMenuItem1.Owner, ContextMenuStrip).SourceControl, RichTextBox)
        If caller IsNot Nothing Then
            caller.SelectAll()
        Else
            If RichTextBox1.Focused Then
                RichTextBox1.SelectAll()

            ElseIf RichTextBox2.Focused Then
                RichTextBox2.SelectAll()
            End If
        End If

    End Sub

    'Cut
    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        Dim caller = CType(CType(ToolStripMenuItem1.Owner, ContextMenuStrip).SourceControl, RichTextBox)
        If caller IsNot Nothing Then
            caller.Cut()
        Else
            If RichTextBox1.Focused Then
                RichTextBox1.Cut()

            ElseIf RichTextBox2.Focused Then
                RichTextBox2.Cut()
            End If
        End If


    End Sub

    'Copy

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem3.Click
        Dim caller = CType(CType(ToolStripMenuItem1.Owner, ContextMenuStrip).SourceControl, RichTextBox)
        If caller IsNot Nothing Then
            caller.Copy()
        Else
            If RichTextBox1.Focused Then
                RichTextBox1.Copy()

            ElseIf RichTextBox2.Focused Then
                RichTextBox2.Copy()
            End If
        End If


    End Sub

    'Paste
    Private Sub ToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem4.Click
        Dim caller = CType(CType(ToolStripMenuItem1.Owner, ContextMenuStrip).SourceControl, RichTextBox)
        If caller IsNot Nothing Then
            caller.Paste()
        Else
            If RichTextBox1.Focused Then
                RichTextBox1.Paste()

            ElseIf RichTextBox2.Focused Then
                RichTextBox2.Paste()
            End If
        End If


    End Sub

    'Save Settings
    Private Sub SaveToolStripButton1_click(sender As Object, e As EventArgs) Handles SaveToolStripButton1.Click
        TabControl1.SelectTab(1)
        Dim result = MessageBox.Show("Save Settings", "Confirmation", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            If DataGridView1.Rows.Count = 1 Then
                My.Settings.SearchKeyType.Clear()
                My.Settings.SearchKey.Clear()
                My.Settings.NewValue.Clear()
                My.Settings.runnable = False
                My.Settings.Save()
                Return
            End If
            Dim count = DataGridView1.Rows.Count
            Dim searchkeytype As New System.Collections.Specialized.StringCollection
            Dim searchkey As New System.Collections.Specialized.StringCollection
            Dim newvalue As New System.Collections.Specialized.StringCollection
            For i = 0 To count - 1
                If Not IsDBNull(DataGridView1.Rows(i).Cells(0).Value) Then
                    If Not IsNothing(DataGridView1.Rows(i).Cells(0).Value) Then
                        searchkeytype.Add(DataGridView1.Rows(i).Cells(0).Value)
                        If Not IsDBNull(DataGridView1.Rows(i).Cells(1).Value) And Not IsNothing(DataGridView1.Rows(i).Cells(1).Value) Then
                            searchkey.Add(DataGridView1.Rows(i).Cells(1).Value)
                            If IsDBNull(DataGridView1.Rows(i).Cells(2).Value) Then
                                newvalue.Add(String.Empty)
                            Else
                                newvalue.Add(DataGridView1.Rows(i).Cells(2).Value)
                            End If
                        Else
                            MessageBox.Show("Search Key cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If


                    End If
                End If

            Next
            My.Settings.SearchKeyType = searchkeytype
            My.Settings.SearchKey = searchkey
            My.Settings.NewValue = newvalue
            My.Settings.runnable = True
            My.Settings.Save()

        End If


    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.SearchKeyType IsNot Nothing Then
            If My.Settings.SearchKeyType.Count > 0 Then
                LoadRules()

            End If

        End If

    End Sub

    Private Sub LoadRules()


        If My.Settings.SearchKeyType IsNot Nothing Then
            Dim dt = New DataTable()
            dt.Columns.Add("SearchKeyType", GetType(String))
            dt.Columns.Add("SearchKey", GetType(String))
            dt.Columns.Add("NewValue", GetType(String))

            Dim num_rows = My.Settings.SearchKeyType.Count
            'initialize an object array of size 12
            Dim row(2) As Object
            For i = 0 To num_rows - 1
                'row = New Object() {My.Settings.SearchKeyType.Item(i), My.Settings.SearchKey.Item(i), My.Settings.NewValue.Item(i)}
                row(0) = My.Settings.SearchKeyType.Item(i)
                row(1) = My.Settings.SearchKey.Item(i)
                row(2) = My.Settings.NewValue.Item(i)
                dt.Rows.Add(row)

            Next
            DataGridView1.DataSource = dt


        End If

    End Sub

    Private Sub SaveToolStripButton_click(sender As Object, e As EventArgs) Handles SaveToolStripButton.Click
        If filepath <> "" Then
            SaveFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(filepath)

            SaveFileDialog1.FileName = System.IO.Path.GetFileName(filepath)
        End If

        SaveFileDialog1.Filter = "(*.csv)|*.csv|(*.txt)|*.txt|All Files (*.*)|*.*"
        If SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            My.Computer.FileSystem.WriteAllText(SaveFileDialog1.FileName, RichTextBox2.Text, True)
        End If

    End Sub

    'Assuming all files are text and not binary
    'TODO: not binary check
    Private Sub Run_click(sender As Object, e As EventArgs) Handles Run.Click
        If My.Settings.runnable Then
            If IsDBNull(RichTextBox1.Text) Or IsNothing(RichTextBox1.Text) Or Trim(RichTextBox1.Text) = "" Then
                MessageBox.Show("No Data to Process. Load File/Files or Paste data in Left TextBox", "Nothing to Process", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return
            End If
        Else
            MessageBox.Show("Define some rules inorder to process", "No Rules Present", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If
        Dim text_lhs = RichTextBox1.Text
        If folderOrFile = "file" Then

            Dim text_rhs = ""
            Dim temp_text = text_lhs
            For i = 0 To My.Settings.SearchKeyType.Count - 1
                Dim searchkeytype = My.Settings.SearchKeyType.Item(i)

                'Dim regex = New Regex("([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))")
                Dim regex_value = My.Settings.SearchKey.Item(i)
                'Dim text = "ijuiuiu 2021-12-20 kljkj   jkjkjj"
                'Dim replaced_text = Regex.Replace(text, "([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))", Date.Now.ToShortDateString)
                'If Match.Success Then
                '    MsgBox(replaced_text)
                'End If
                'Dim regex = New Regex(regex_value)
                text_rhs = Regex.Replace(temp_text, regex_value, My.Settings.NewValue.Item(i))
                temp_text = text_rhs
                'MsgBox(text_rhs, vbOK)
            Next
            RichTextBox2.Text = text_rhs
        End If


        If folderOrFile = "folder" Then
            Dim files = RichTextBox1.Lines
            ' last line is vbcrlf
            processed_folderpath = folderpath & "_processed_" & DateTime.Now.ToString("yyyyMMdd_HHmmss")
            Dim f_processed_folderpath = Regex.Replace(processed_folderpath, "\\", "/")
            Dim regx = New Regex(Regex.Escape(folderpath))
            For f = 0 To files.Count - 2
                Dim id_count = New StringBuilder
                'Debug.WriteLine(files.ElementAt(f))
                ' Do not escape the text input
                ' using replace on escaped search and replacement results in newfilepath having both \\ and \
                'e.g. C:\\Users\\DK186039\\Downloads\\cde-job-run-110_processed_20210429_191023\driver_event.log
                'so convert \ to  / then do the replace, then convert back to original \
                Dim f_folderpath = Regex.Replace(folderpath, "\\", "/")
                Dim f_file = Regex.Replace(files.ElementAt(f), "\\", "/")
                Dim newfilepath = Regex.Replace(f_file, f_folderpath, f_processed_folderpath)
                newfilepath = Regex.Replace(newfilepath, "/", "\\")
                newfilepath = Regex.Unescape(newfilepath)
                Dim filetext = File.ReadAllText(files.ElementAt(f))
                Dim text_rhs = ""
                Dim temp_text = filetext
                For i = 0 To My.Settings.SearchKeyType.Count - 1
                    Dim searchkeytype = My.Settings.SearchKeyType.Item(i)
                    Dim regex_value = My.Settings.SearchKey.Item(i)
                    Dim matches As MatchCollection = Regex.Matches(temp_text, regex_value)
                    id_count.Append(",id=" & i & ",count=" & matches.Count)
                    text_rhs = Regex.Replace(temp_text, regex_value, My.Settings.NewValue.Item(i))
                    temp_text = text_rhs
                    'MsgBox(text_rhs, vbOK)
                Next
                Dim fileinfo = New FileInfo(newfilepath)
                fileinfo.Directory.Create()
                File.WriteAllText(newfilepath, text_rhs)
                RichTextBox2.AppendText("processed..." & newfilepath & id_count.ToString & vbCrLf)

            Next
        End If
        RichTextBox1.ReadOnly = False
        MessageBox.Show("Processing Complete", "Simple Obfuscator", MessageBoxButtons.OK, MessageBoxIcon.Information)


    End Sub


End Class
