﻿' R- Instat
' Copyright (C) 2015-2017
'
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License 
' along with this program.  If not, see <http://www.gnu.org/licenses/>.

Imports System.IO
Public Class ucrFilePath
    'event raised when the file path contents has changed
    Public Event FilePathChanged()

    'gets and sets the text propert of the browse button.
    Public Property FilePathBrowseText() As String
        Get
            Return btnBrowse.Text
        End Get
        Set(ByVal value As String)
            btnBrowse.Text = value
        End Set
    End Property

    'gets and sets the label of the input textbox
    Public Property FilePathLabel() As String
        Get
            Return lblName.Text
        End Get
        Set(ByVal value As String)
            lblName.Text = value
        End Set
    End Property

    'used to set the title of the SaveFileDialog prompt
    Public Property FilePathDialogTitle As String = "Save As"

    'used to set the allowed file filters or file extensions
    Public Property FilePathDialogFilter As String = "All Files *.*"

    'used to set the suggestive name if no file name was set before
    Public Property DefaultFileSuggestionName As String = ""

    'gets or sets the input file path
    Public Property FilePath() As String
        Get
            Return GetPathControl().GetText()
        End Get
        Set(ByVal value As String)
            'first replace backward slashed with forward slashes cause of R path formats
            GetPathControl().SetName(value.Replace("\", "/"))
            RaiseEvent FilePathChanged()
        End Set
    End Property

    ''' <summary>
    ''' gets the control that contains the input file path and name
    ''' </summary>
    ''' <returns>ucrInputTextBox</returns>    
    Public Function GetPathControl() As ucrInputTextBox
        Return ucrInputFilePath
    End Function

    ''' <summary>
    ''' Sets the R parameter to the path control
    ''' </summary>
    ''' <param name="rParamter"></param>
    Public Sub SetPathControlParameter(rParamter As RParameter)
        GetPathControl().SetParameter(rParamter)
    End Sub

    ''' <summary>
    ''' sets the R code to the path control
    ''' </summary>
    ''' <param name="clsNewCodeStructure"></param>
    ''' <param name="bReset"></param>
    Public Sub SetPathControlRcode(clsNewCodeStructure As RCodeStructure, Optional bReset As Boolean = False)
        GetPathControl().SetRCode(clsNewCodeStructure, bReset)
    End Sub

    ''' <summary>
    ''' resets the path control and and the file path
    ''' </summary>
    Public Sub ResetPathControl()
        GetPathControl().Reset()
        Clear()
    End Sub

    Public Function IsEmpty() As Boolean
        Return String.IsNullOrEmpty(FilePath)
    End Function

    Public Sub Clear()
        FilePath = ""
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Using dlgSave As New SaveFileDialog
            dlgSave.Title = FilePathDialogTitle
            dlgSave.Filter = FilePathDialogFilter
            If IsEmpty() Then
                dlgSave.FileName = DefaultFileSuggestionName
                dlgSave.InitialDirectory = frmMain.clsInstatOptions.strWorkingDirectory
            Else
                'use the file path in windows backslash convention format
                Dim strFilePathInWindowsFormat As String = FilePath.Replace("/", "\")
                dlgSave.FileName = Path.GetFileName(strFilePathInWindowsFormat)
                dlgSave.InitialDirectory = Path.GetDirectoryName(strFilePathInWindowsFormat)
            End If
            If DialogResult.OK = dlgSave.ShowDialog() Then
                FilePath = dlgSave.FileName
            End If
        End Using
    End Sub

    Private Sub ucrInputFilePath_Click(sender As Object, e As EventArgs) Handles ucrInputFilePath.Click
        btnBrowse.PerformClick()
    End Sub

End Class
