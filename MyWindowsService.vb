Imports System.Timers
Imports System.Runtime.InteropServices

Public Class MyWindowsService

    Private eventId As Integer = 1

    Public Enum ServiceState
        SERVICE_STOPPED = 1
        SERVICE_START_PENDING = 2
        SERVICE_STOP_PENDING = 3
        SERVICE_RUNNING = 4
        SERVICE_CONTINUE_PENDING = 5
        SERVICE_PAUSE_PENDING = 6
        SERVICE_PAUSED = 7
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Public Structure ServiceStatus
        Public dwServiceType As Long
        Public dwCurrentState As ServiceState
        Public dwControlsAccepted As Long
        Public dwWin32ExitCode As Long
        Public dwServiceSpecificExitCode As Long
        Public dwCheckPoint As Long
        Public dwWaitHint As Long
    End Structure

    ' To access the OnStart in Visual Basic, select OnStart from the
    ' method name drop-down list. 
    Protected Overrides Sub OnStart(ByVal args() As String)
        EventLog1.WriteEntry("In OnStart")

        ' Update the service state to Start Pending.
        Dim serviceStatus As ServiceStatus = New ServiceStatus()
        serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING
        serviceStatus.dwWaitHint = 100000
        SetServiceStatus(Me.ServiceHandle, serviceStatus)

        ' Set up a timer that triggers every minute.
        Dim timer As Timer = New Timer()
        timer.Interval = 60000 ' 60 seconds
        AddHandler timer.Elapsed, AddressOf Me.OnTimer
        timer.Start()

        ' Update the service state to Running.
        serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING
        SetServiceStatus(Me.ServiceHandle, serviceStatus)
    End Sub

    Private Sub OnTimer(sender As Object, e As Timers.ElapsedEventArgs)
        ' TODO: Insert monitoring activities here.
        EventLog1.WriteEntry("Monitoring the System v0.4", EventLogEntryType.Information, eventId)
        eventId = eventId + 1
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        EventLog1.WriteEntry("In OnStop.")

        ' Update the service state to Stop Pending.
        Dim serviceStatus As ServiceStatus = New ServiceStatus()
        serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING
        serviceStatus.dwWaitHint = 100000
        SetServiceStatus(Me.ServiceHandle, serviceStatus)

        ' Update the service state to Stopped.
        serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED
        SetServiceStatus(Me.ServiceHandle, serviceStatus)
    End Sub

    Protected Overrides Sub OnContinue()
        EventLog1.WriteEntry("In OnContinue.")
    End Sub

    ' To access the constructor in Visual Basic, select New from the
    ' method name drop-down list. 
    Public Sub New()
        MyBase.New()
        InitializeComponent()
        Me.EventLog1 = New System.Diagnostics.EventLog
        If Not System.Diagnostics.EventLog.SourceExists("MySource") Then
            System.Diagnostics.EventLog.CreateEventSource("MySource",
            "MyNewLog")
        End If
        EventLog1.Source = "MySource"
        EventLog1.Log = "MyNewLog"
    End Sub

    Declare Auto Function SetServiceStatus Lib "advapi32.dll" (ByVal handle As IntPtr, ByRef serviceStatus As ServiceStatus) As Boolean

End Class


