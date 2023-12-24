Imports System.Runtime.InteropServices
Module Module1




    Public Enum ScrollBarType As UInteger
        SbHorz = 0
        SbVert = 1
        SbCtl = 2
        SbBoth = 3
    End Enum


    Public Enum Message As UInteger
        WM_HSCROLL = &H114
        WM_VSCROLL = &H115

    End Enum


    Public Enum ScrollBarCommands As UInteger
        SB_THUMBPOSITION = 4
    End Enum

    Public Declare Function GetScrollPos Lib "user32.dll" (
        ByVal hWnd As IntPtr,
        ByVal nBar As Integer) As Integer

    Public Declare Function SetScrollPos Lib "user32.dll" (
        ByVal hWnd As IntPtr,
        ByVal nBar As Integer,
        ByVal nPos As Integer,
        ByVal bRedraw As Boolean) As Integer

    Public Declare Function PostMessageA Lib "user32.dll" (
        ByVal hwnd As IntPtr,
        ByVal wMsg As Integer,
        ByVal wParam As Integer,
        ByVal lParam As Integer) As Boolean

End Module
