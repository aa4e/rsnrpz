Imports System
Imports System.Runtime.InteropServices

Namespace Scpi

    ''' <summary>
    ''' Class for Rohde&amp;Schwartz NRP tool control.
    ''' </summary>
    Friend Class RsNrpz
        Implements System.IDisposable

#Region "CTOR"

        Private _Handle As System.Runtime.InteropServices.HandleRef

        ''' <summary>
        ''' Creates an IVI instrument driver session, typically using the C session instrument handle.
        ''' </summary>
        ''' <param name="instrumentHandle">The instrument handle that is used to create an IVI instrument driver session.</param>
        Public Sub New(ByVal instrumentHandle As System.IntPtr)
            MyBase.New()
            Me._Handle = New System.Runtime.InteropServices.HandleRef(Me, instrumentHandle)
            Me._Disposed = False
        End Sub

        ''' <summary>
        ''' Performs the following initialization actions and assigned specified sesnor to channel 1:
        ''' - Opens a session to the specified device using the interface and address specified in the Resource Name control.
        ''' - Performs an identification query on the sensor.
        ''' - Resets the instrument to a known state.
        ''' - Sends initialization commands to the sensor
        ''' - Returns an Instrument Handle which is used to differentiate between different sessions of this instrument driver.
        ''' </summary>
        ''' <param name="resourceName">
        ''' Specifies the interface and address of the device that is to be initialized (Instrument Descriptor). 
        ''' The exact grammar to be used in this control is shown in the note below. 
        ''' Default Value:  "USB::0x0aad::0x000b::100000"
        ''' 
        ''' Note:
        ''' Based on the Instrument Descriptor, this operation establishes a communication session with a device.
        ''' The grammar for the Instrument Descriptor is shown below. Optional parameters are shown in square brackets ([]).
        ''' 
        ''' Interface   Grammar
        ''' ------------------------------------------------------
        ''' USB         USB::vendor Id::product Id::serial number
        ''' 
        ''' where:
        '''   vendor Id is 0aad for all Rohde&amp;Schwarz instruments.
        '''   product Id depends on your sensor model:
        '''                        NRP-Z21  : 0x0003
        '''                        NRP-FU   : 0x0004
        '''                        FSH-Z1   : 0x000b
        '''                        NRP-Z11  : 0x000c
        '''                        NRP-Z22  : 0x0013
        '''                        NRP-Z23  : 0x0014
        '''                        NRP-Z24  : 0x0015
        '''                        NRP-Z51  : 0x0016
        '''                        NRP-Z52  : 0x0017
        '''                        NRP-Z55  : 0x0018
        '''                        NRP-Z56  : 0x0019
        '''                        FSH-Z18  : 0x001a
        '''                        NRP-Z91  : 0x0021
        '''                        NRP-Z81  : 0x0023
        '''                        NRP-Z31  : 0x002c
        '''                        NRP-Z37  : 0x002d
        '''                        NRP-Z96  : 0x002e
        '''                        NRP-Z27  : 0x002f
        '''                        NRP-Z28  : 0x0051
        '''                        NRP-Z98  : 0x0052
        '''                        NRP-Z92  : 0x0062
        '''                        NRP-Z57  : 0x0070
        '''                        NRP-Z85  : 0x0083
        '''                        NRPC40   : 0x008f
        '''                        NRPC50   : 0x0090
        '''                        NRP-Z86  : 0x0095
        '''                        NRP-Z211 : 0x00a6
        '''                        NRP-Z221 : 0x00a7
        '''                        NRP-Z58  : 0x00a8
        '''                        NRPC33   : 0x00b6
        '''                        NRPC18   : 0x00bf
        ''' 
        '''   serial number you can find on your sensor. Serial number is number with 6 digits. For example 9000003
        '''  you can use star '*' for product id or serial number in resource descriptor. Use star only for one connected sensor, because driver opens only first sensor on the bus.
        ''' The USB keyword is used for USB interface.
        ''' Examples:
        ''' USB   - "USB::0x0aad::0x000b::100000"
        ''' USB   - "USB::0x0aad::0x000b::*" - Opens first FSH-Z1 sensor
        ''' USB   - "USB::0x0aad::*"         - Opens first R&amp;S sensor
        ''' USB   - "*"                      - Opens first R&amp;S sensor
        ''' </param>
        ''' <param name="queryId">
        ''' Specifies if an ID Query is sent to the instrument during the initialization procedure.
        ''' Default Value - true (Do Query).
        ''' Note:
        ''' Under normal circumstances the ID Query ensures that the instrument initialized is the type supported by this driver. 
        ''' However circumstances may arise where it is undesirable to send an ID Query to the instrument. 
        ''' In those cases; set this control to "Skip Query" and this function will initialize the selected interface, without doing an ID Query.
        ''' </param>
        ''' <param name="resetDevice">
        ''' Specifies if the instrument is to be reset to its power-on settings during the initialization procedure.
        ''' Default Value - True (Reset Device).
        ''' Note: If you do not want the instrument reset. Set this control to "Don't Reset" while initializing the instrument.
        ''' </param>
        Public Sub New(ByVal resourceName As String, ByVal queryId As Boolean, ByVal resetDevice As Boolean)
            MyBase.New()
            Dim instrumentHandle As System.IntPtr
            Dim pInvokeResult As Integer = Native.init(resourceName, System.Convert.ToUInt16(queryId), System.Convert.ToUInt16(resetDevice), instrumentHandle)
            Me._Handle = New System.Runtime.InteropServices.HandleRef(Me, instrumentHandle)
            Native.TestForError(Me._Handle, pInvokeResult)
            Me._Disposed = False
        End Sub

        ''' <summary>
        ''' Performs the following initialization actions and assigned specified sesnor to channel 1:
        ''' - Opens a session to the specified device using the interface and address specified in the Resource Name control.
        ''' - Performs an identification query on the sensor.
        ''' - Resets the instrument to a known state.
        ''' - Sends initialization commands to the sensor
        ''' - Returns an Instrument Handle which is used to differentiate between different sessions of this instrument driver.
        ''' </summary>
        ''' <param name="queryId">
        ''' Specifies if an ID Query is sent to the instrument during the initialization procedure. 
        ''' Default Value - true (Do Query).
        ''' Note:
        ''' Under normal circumstances the ID Query ensures that the instrument initialized is the type supported by this driver. 
        ''' However circumstances may arise where it is undesirable to send an ID Query to the instrument.  
        ''' In those cases; set this control to "Skip Query" and this function will initialize the selected interface, without doing an ID Query.
        ''' </param>
        ''' <param name="port">Selects the port.</param>
        ''' <param name="resetDevice">
        ''' Specifies if the instrument is to be reset to its power-on settings during the initialization procedure. 
        ''' Default Value - True (Reset Device).
        ''' </param>
        Public Sub New(ByVal queryId As Boolean, ByVal port As Z5Port, ByVal resetDevice As Boolean)
            MyBase.New()
            Dim instrumentHandle As System.IntPtr
            Dim pInvokeResult As Integer = Native.initZ5(System.Convert.ToUInt16(queryId), port, System.Convert.ToUInt16(resetDevice), instrumentHandle)
            Me._Handle = New System.Runtime.InteropServices.HandleRef(Me, instrumentHandle)
            Native.TestForError(Me._Handle, pInvokeResult)
            Me._Disposed = False
        End Sub

#End Region '/CTOR

#Region "TIMING"

        ''' <summary>
        ''' Configures times that is to be excluded at the beginning and at the end of the integration.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="excludeStart">
        ''' Sets a time (in seconds) that is to be excluded at the beginning of the integration.
        ''' Valid Range: 
        ''' NRP-Z21: 0.0-0.1 s.
        ''' FSH-Z1:  0.0-0.1 s.
        ''' Default Value: 0 s.
        ''' </param>
        ''' <param name="excludeStop">
        ''' Sets a time (in seconds) that is to be excluded at the end of the integration.
        ''' Valid Range:
        ''' NRP-Z21: 0.0-0.003 s.
        ''' FSH-Z1:  0.0-0.003 s.
        ''' Default Value: 0 s.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TIM:EXCL:STAR
        ''' SENS:TIM:EXCL:STOP
        ''' This function is NOT available for NRP-Z51.
        ''' </remarks>
        Public Sub Timing_ConfigureExclude(ByVal channel As Integer, ByVal excludeStart As Double, ByVal excludeStop As Double)
            Dim pInvokeResult As Integer = Native.timing_configureExclude(Me._Handle, channel, excludeStart, excludeStop)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Sets a time that is to be excluded at the beginning of the integration.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="excludeStart">
        ''' Sets a time (in seconds) that is to be excluded at the beginning of the integration
        ''' Valid Range:
        ''' NRP-Z21: 0.0-0.1 s.
        ''' FSH-Z1:  0.0-0.1 s.
        ''' Default Value: 0 s.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TIM:EXCL:STAR
        ''' This function is NOT available for NRP-Z51.
        ''' </remarks>
        Public Sub Timing_SetTimingExcludeStart(ByVal channel As Integer, ByVal excludeStart As Double)
            Dim pInvokeResult As Integer = Native.timing_setTimingExcludeStart(Me._Handle, channel, excludeStart)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads a time (in seconds) that is to be excluded at the beginning of the integration.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command:
        ''' SENS:TIM:EXCL:STAR?
        ''' </remarks>
        Public Function Timing_GetTimingExcludeStart(ByVal channel As Integer) As Double
            Dim excludeStart As Double
            Dim pInvokeResult As Integer = Native.timing_getTimingExcludeStart(Me._Handle, channel, excludeStart)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return excludeStart
        End Function

        ''' <summary>
        ''' Sets a time that is to be excluded at the end of the integration.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="excludeStop">
        ''' Sets a time (in seconds) that is to be excluded at the end of the integration.
        ''' Valid Range:
        ''' NRP-Z21: 0.0-0.003 s.
        ''' FSH-Z1:  0.0-0.003 s.
        ''' Default Value: 0 s.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TIM:EXCL:STOP
        ''' </remarks>
        Public Sub Timing_SetTimingExcludeStop(ByVal channel As Integer, ByVal excludeStop As Double)
            Dim pInvokeResult As Integer = Native.timing_setTimingExcludeStop(Me._Handle, channel, excludeStop)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads a time (in seconds) that is to be excluded at the end of the integration.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TIM:EXCL:STOP?
        ''' </remarks>
        Public Function Timing_GetTimingExcludeStop(ByVal channel As Integer) As Double
            Dim excludeStop As Double
            Dim pInvokeResult As Integer = Native.timing_getTimingExcludeStop(Me._Handle, channel, excludeStop)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return excludeStop
        End Function

#End Region '/TIMING

#Region "BW"

        ''' <summary>
        ''' This function can be used to reduce the video bandwidth for the Trace and Statistics modes. 
        ''' As a result, trigger sensitivity is increased and the display noise reduced. 
        ''' To prevent signals from being corrupted, the selected video bandwidth should not be smaller than the RF bandwidth of the measurement signal. 
        ''' The "FULL" setting corresponds to a video bandwidth of at least 30 MHz if there is an associated frequency setting (SENSe:FREQuency command) greater than or equal to 500 MHz. 
        ''' If a frequency below 500 MHz is set and the video bandwidth is set to "FULL", the video bandwidth is automatically reduced to approx. 7.5 MHz.
        ''' If the video bandwidth is limited with the SENSe:BWIDth:VIDEo command, the sampling rate is also automatically reduced, 
        ''' i.e. the effective time resolution in the Trace mode is reduced accordingly. 
        ''' In the Statistics modes, the measurement time must be increased appropriately if the required sample size is to be maintained:
        ''' Video bandwidth Sampling rate   Sampling interval
        ''' "Full"            8e7 1/s       12.5 ns
        ''' "5 MHz"           4e7 1/s         25 ns
        ''' "1.5 MHz"         1e7 1/s        100 ns
        ''' "300 kHz"       2.5e6 1/s        400 ns
        ''' sets the video bandwidth according to a list of available bandwidths. 
        ''' The list can be obtained using function rsnrpz_bandwidth_getBwList.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="bandwidth">
        ''' Sets the video bandwidth according to a list of available bandwidths. The list can be obtained using function rsnrpz_bandwidth_getBwList.
        ''' Valid Range: Index of bandwidth in the list. Default Value: 0.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:BWIDth:VIDeo
        ''' </remarks>
        Public Sub Bandwidth_SetBw(ByVal channel As Integer, ByVal bandwidth As Integer)
            Dim pInvokeResult As Integer = Native.bandwidth_setBw(Me._Handle, channel, bandwidth)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries selected video bandwidth as index in bandwidth list.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:BWIDth:VIDeo?
        ''' </remarks>
        Public Function Bandwidth_GetBw(ByVal channel As Integer) As Integer
            Dim bandwidth As Integer
            Dim pInvokeResult As Integer = Native.bandwidth_getBw(Me._Handle, channel, bandwidth)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return bandwidth
        End Function

        ''' <summary>
        ''' Queries the comma separated list of possible video bandwidths.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="bufferSize">
        ''' Defines the size of buffer passed to Bandwidth List argument.
        ''' Valid Range: &gt; 0. Default Value: 200.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:BWIDth:VIDeo:LIST?
        ''' </remarks>
        Public Function Bbandwidth_GetBwList(ByVal channel As Integer, ByVal bufferSize As Integer) As Text.StringBuilder
            Dim bandwidthList As New System.Text.StringBuilder()
            Dim pInvokeResult As Integer = Native.bandwidth_getBwList(Me._Handle, channel, bufferSize, bandwidthList)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return bandwidthList
        End Function

#End Region '/BW

#Region "AVG"

        ''' <summary>
        ''' Configures all parameters necessary for automatic detection of filter bandwidth.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="resolution">
        ''' Defines the number of significant places for linear units and the number of decimal places for logarithmic units which should be free of noise in the measurement result.
        ''' Default Value <see cref="Resolution.Res3"/>.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN:AUTO ON
        ''' SENS:AVER:COUN:AUTO:TYPE RES
        ''' SENS:AVER:COUN:AUTO:RES &lt;value&gt;
        ''' SENS:AVER:TCON REP
        ''' </remarks>
        Public Sub Avg_ConfigureAvgAuto(ByVal channel As Integer, ByVal resolution As Resolution)
            Dim pInvokeResult As Integer = Native.avg_configureAvgAuto(Me._Handle, channel, resolution)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Configures all parameters necessary for setting the noise ratio in the measurement result and automatic detection of filter bandwidth.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="maximumNoiseRatioDb">
        ''' Sets the maximum noise ratio in the measurement result, dB.
        ''' Valid Range:
        ''' NRP-Z21: 0.0-1.0.
        ''' FSH-Z1:  0.0-1.0.
        ''' Default Value: 0.1.
        ''' Notes: This value is not range checked; the actual range depends on sensor used.
        ''' </param>
        ''' <param name="upperTimeLimit">
        ''' Sets the upper time limit (maximum time to fill the filter).
        ''' Valid Range:
        ''' NRP-21: 0.01-999.99.
        ''' FSH-Z1: 0.01-999.99.
        ''' Default Value: 4.0.
        ''' Notes: This value is not range checked, the actual range depends on sensor used.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN:AUTO ON
        ''' SENS:AVER:COUN:AUTO:TYPE NSR
        ''' SENS:AVER:COUN:AUTO:NSR &lt;value&gt;
        ''' SENS:AVER:COUN:AUTO:MTIM &lt;value&gt;
        ''' SENS:AVER:TCON REP
        ''' </remarks>
        Public Sub Avg_ConfigureAvgNSRatio(ByVal channel As Integer, ByVal maximumNoiseRatioDb As Double, ByVal upperTimeLimit As Double)
            Dim pInvokeResult As Integer = Native.avg_configureAvgNSRatio(Me._Handle, channel, maximumNoiseRatioDb, upperTimeLimit)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Configures all parameters necessary for manual setting of filter bandwidth.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="count">Sets the filter bandwidth. Valid Range: 1-0x10000. Default Value: 4.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN
        ''' SENS:AVER:COUN:AUTO OFF
        ''' SENS:AVER:TCON REP
        ''' </remarks>
        Public Sub Avg_ConfigureAvgManual(ByVal channel As Integer, ByVal count As Integer)
            Dim pInvokeResult As Integer = Native.avg_configureAvgManual(Me._Handle, channel, count)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' This function can be used to automatically determine a value for filter bandwidth.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="autoEnabled">
        ''' Activates or deactivates automatic determination of a value for filter bandwidth.
        ''' If the automatic switchover is activated with the ON parameter, the sensor always defines a suitable filter length.
        ''' Default Value: On.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN:AUTO ON|OFF
        ''' </remarks>
        Public Sub Avg_SetAutoEnabled(ByVal channel As Integer, ByVal autoEnabled As Boolean)
            Dim pInvokeResult As Integer = Native.avg_setAutoEnabled(Me._Handle, channel, System.Convert.ToUInt16(autoEnabled))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the setting of automatic switchover of filter bandwidth.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN:AUTO?
        ''' </remarks>
        Public Function Avg_GetAutoEnabled(ByVal channel As Integer) As Boolean
            Dim autoEnabledAsUShort As UShort
            Dim pInvokeResult As Integer = Native.avg_getAutoEnabled(Me._Handle, channel, autoEnabledAsUShort)
            Dim autoEnabled As Boolean = System.Convert.ToBoolean(autoEnabledAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return autoEnabled
        End Function

        ''' <summary>
        ''' Sets an upper time limit can be set via (maximum time). It should never be exceeded. 
        ''' Undesired long measurement times can thus be prevented if the automatic filter length switchover is on.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="upperTimeLimit">
        ''' Sets the upper time limit (maximum time to fill the filter).
        ''' Valid Range:
        ''' NRP-21: 0.01-999.99.
        ''' FSH-Z1: 0.01-999.99.
        ''' Default Value: 4.0.
        ''' Notes: This value is not range checked, the actual range depends on sensor used.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN:AUTO:MTIM
        ''' </remarks>
        Public Sub Avg_SetAutoMaxMeasuringTime(ByVal channel As Integer, ByVal upperTimeLimit As Double)
            Dim pInvokeResult As Integer = Native.avg_setAutoMaxMeasuringTime(Me._Handle, channel, upperTimeLimit)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries an upper time limit (maximum time to fill the filter).
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' </remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN:AUTO:MTIM?
        Public Function Avg_GetAutoMaxMeasuringTime(ByVal channel As Integer) As Double
            Dim upperTimeLimit As Double
            Dim pInvokeResult As Integer = Native.avg_getAutoMaxMeasuringTime(Me._Handle, channel, upperTimeLimit)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return upperTimeLimit
        End Function

        ''' <summary>
        ''' Sets the maximum noise ratio in the measurement result.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="maximumNoiseRatio">
        ''' Sets the maximum noise ratio in the measurement result. The value is set in dB.
        ''' Valid Range:
        ''' NRP-Z21: 0.0-1.0.
        ''' FSH-Z1:  0.0-1.0.
        ''' Default Value: 0.1.
        ''' Notes: This value is not range checked; the actual range depends on sensor used.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN:AUTO:NSR
        ''' </remarks>
        Public Sub Avg_SetAutoNoiseSignalRatio(ByVal channel As Integer, ByVal maximumNoiseRatio As Double)
            Dim pInvokeResult As Integer = Native.avg_setAutoNoiseSignalRatio(Me._Handle, channel, maximumNoiseRatio)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the maximum noise signal ratio value in dB.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN:AUTO:NSR?
        ''' </remarks>
        Public Function Avg_GetAutoNoiseSignalRatio(ByVal channel As Integer) As Double
            Dim maximumNoiseRatio As Double
            Dim pInvokeResult As Integer = Native.avg_getAutoNoiseSignalRatio(Me._Handle, channel, maximumNoiseRatio)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return maximumNoiseRatio
        End Function

        ''' <summary>
        ''' Defines the number of significant places for linear units and the number of decimal places for logarithmic units which should be free of noise in the measurement result. 
        ''' This setting does not affect the setting of display.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="resolution">
        ''' Defines the number of significant places for linear units and the number of decimal places for logarithmic units which should be free of noise in the measurement result.
        ''' Default Value: <see cref="Resolution.Res3"/>.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN:AUTO:RES 1 ... 4
        ''' </remarks>
        Public Sub Avg_SetAutoResolution(ByVal channel As Integer, ByVal resolution As Resolution)
            Dim pInvokeResult As Integer = Native.avg_setAutoResolution(Me._Handle, channel, resolution)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the number of significant places for linear units and the number of decimal places for logarithmic units which should be free of noise in the measurement result. 
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN:AUTO:RES?
        ''' </remarks>
        Public Function Avg_GetAutoResolution(ByVal channel As Integer) As Resolution
            Dim resolution As Integer
            Dim pInvokeResult As Integer = Native.avg_getAutoResolution(Me._Handle, channel, resolution)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return CType(resolution, Resolution)
        End Function

        ''' <summary>
        ''' Selects a method by which the automatic filter length switchover can operate.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="method">Selects a method by which the automatic filter length switchover can operate.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN:AUTO:TYPE RES | NSR
        ''' </remarks>
        Public Sub Avg_SetAutoType(ByVal channel As Integer, ByVal method As AutoCountType)
            Dim pInvokeResult As Integer = Native.avg_setAutoType(Me._Handle, channel, method)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns a method by which the automatic filter length switchover can operate.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN:AUTO:TYPE?
        ''' </remarks>
        Public Function Avg_GetAutoType(ByVal channel As Integer) As AutoCountType
            Dim method As AutoCountType
            Dim pInvokeResult As Integer = Native.avg_getAutoType(Me._Handle, channel, method)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return CType(method, AutoCountType)
        End Function

        ''' <summary>
        ''' Sets the filter bandwidth. The wider the filter the lower the noise and the longer it takes to obtain a measured value.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="count">Sets the filter bandwidth. Valid Range: 1-0x10000. Default Value: 4.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN
        ''' </remarks>
        Public Sub Avg_SetCount(ByVal channel As Integer, ByVal count As Integer)
            Dim pInvokeResult As Integer = Native.avg_setCount(Me._Handle, channel, count)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the filter bandwidth.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN?
        ''' </remarks>
        Public Function Avg_GetCount(ByVal channel As Integer) As Integer
            Dim count As Integer
            Dim pInvokeResult As Integer = Native.avg_getCount(Me._Handle, channel, count)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return count
        End Function

        ''' <summary>
        ''' Switches the filter function of a sensor on or off. 
        ''' When the filter is switched on, the number of measured values set with rsnrpz_avg_setCount() is averaged. 
        ''' This reduces the effect of noise so that more reliable results are obtained.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="averaging">Switches the filter function of a sensor on or off. Default Value: On.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:STAT ON | OFF
        ''' </remarks>
        Public Sub Avg_SetEnabled(ByVal channel As Integer, ByVal averaging As Boolean)
            Dim pInvokeResult As Integer = Native.avg_setEnabled(Me._Handle, channel, System.Convert.ToUInt16(averaging))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the state of the filter function of a sensor.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:STAT?
        ''' </remarks>
        Public Function Avg_GetEnabled(ByVal channel As Integer) As Boolean
            Dim averagingAsUShort As UShort
            Dim pInvokeResult As Integer = Native.avg_getEnabled(Me._Handle, channel, averagingAsUShort)
            Dim averaging As Boolean = System.Convert.ToBoolean(averagingAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return averaging
        End Function

        ''' <summary>
        ''' Sets a timeslot whose measured value is used to automatically determine the filter length.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="timeslot">
        ''' Sets a timeslot whose measured value is used to automatically determine the filter length.
        ''' Valid Range:
        ''' NRP-Z21: 1-8.
        ''' FSH-Z1:  1-8.
        ''' Default Value: 1.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN:AUTO:SLOT
        ''' </remarks>
        Public Sub Avg_SetSlot(ByVal channel As Integer, ByVal timeslot As Integer)
            Dim pInvokeResult As Integer = Native.avg_setSlot(Me._Handle, channel, timeslot)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns a timeslot whose measured value is used to automatically determine the filter length.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN:AUTO:SLOT?
        ''' </remarks>
        Public Function Avg_GetSlot(ByVal channel As Integer) As Integer
            Dim timeslot As Integer
            Dim pInvokeResult As Integer = Native.avg_getSlot(Me._Handle, channel, timeslot)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return timeslot
        End Function

        ''' <summary>
        ''' Determines whether a new result is calculated immediately after a new measured value is available 
        ''' or only after an entire range of new values is available for the filter.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="terminalControl">Determines the type of terminal control. Default value - <see cref="TerminalControl.TerminalControlRepeat"/>.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:TCON MOV | REP
        ''' </remarks>
        Public Sub Avg_SetTerminalControl(ByVal channel As Integer, ByVal terminalControl As TerminalControl)
            Dim pInvokeResult As Integer = Native.avg_setTerminalControl(Me._Handle, channel, terminalControl)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the type of terminal control.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe[1..4]:AVERage:TCONtrol?
        ''' </remarks>
        Public Function Avg_GetTerminalControl(ByVal channel As Integer) As TerminalControl
            Dim terminalControl As Integer
            Dim pInvokeResult As Integer = Native.avg_getTerminalControl(Me._Handle, channel, terminalControl)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return CType(terminalControl, TerminalControl)
        End Function

        ''' <summary>
        ''' This function initializes the digital filter by deleting the stored measured values.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:RES
        ''' </remarks>
        Public Sub Avg_Reset(ByVal channel As Integer)
            Dim pInvokeResult As Integer = Native.avg_reset(Me._Handle, channel)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

#End Region '/AVG

#Region "RANGE"

        ''' <summary>
        ''' Sets the automatic selection of a measurement range to ON or OFF.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="autoRange">Sets the automatic selection of a measurement range to ON or OFF. Default Value: On.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:RANG:AUTO ON | OFF
        ''' </remarks>
        Public Sub Range_SetAutoEnabled(ByVal channel As Integer, ByVal autoRange As Boolean)
            Dim pInvokeResult As Integer = Native.range_setAutoEnabled(Me._Handle, channel, System.Convert.ToUInt16(autoRange))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the state of automatic selection of a measurement range.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:RANG:AUTO?
        ''' </remarks>
        Public Function Range_GetAutoEnabled(ByVal channel As Integer) As Boolean
            Dim autoRangeAsUShort As UShort
            Dim pInvokeResult As Integer = Native.range_getAutoEnabled(Me._Handle, channel, autoRangeAsUShort)
            Dim autoRange As Boolean = System.Convert.ToBoolean(autoRangeAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return autoRange
        End Function

        ''' <summary>
        ''' Sets the cross-over level. Shifts the transition ranges between the measurement ranges. 
        ''' This may improve the measurement accuracy for special signals, i.e. signals with a high crest factor.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="crossoverLevel">Sets the cross-over level in dB.
        ''' Valid Range:
        ''' NRP-Z21: -20.0-0.0 dB.
        ''' FSH-Z1:  -20.0-0.0 dB.
        ''' Default Value: 0 dB.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:RANG:AUTO:CLEV
        ''' </remarks>
        Public Sub Range_SetCrossoverLevel(ByVal channel As Integer, ByVal crossoverLevel As Double)
            Dim pInvokeResult As Integer = Native.range_setCrossoverLevel(Me._Handle, channel, crossoverLevel)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads the cross-over level in dB.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:RANG:AUTO:CLEV?
        ''' </remarks>
        Public Function Range_GetCrossoverLevel(ByVal channel As Integer) As Double
            Dim crossoverLevel As Double
            Dim pInvokeResult As Integer = Native.range_getCrossoverLevel(Me._Handle, channel, crossoverLevel)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return crossoverLevel
        End Function

        ''' <summary>
        ''' Selects a measurement range in which the corresponding sensor is to perform a measurement.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="range">Selects a measurement range in which the corresponding sensor is to perform a measurement.
        ''' Valid Range:
        ''' NRP-Z21:  0 to 2.
        ''' FSH-1:    0 to 2.
        ''' Default Value: 2.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:RANG 0 .. 2
        ''' </remarks>
        Public Sub Range_SetRange(ByVal channel As Integer, ByVal range As Integer)
            Dim pInvokeResult As Integer = Native.range_setRange(Me._Handle, channel, range)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns a measurement range in which the corresponding sensor is to perform a measurement.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:RANG?
        ''' </remarks>
        Public Function Range_GetRange(ByVal channel As Integer) As Integer
            Dim range As Integer
            Dim pInvokeResult As Integer = Native.range_getRange(Me._Handle, channel, range)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return range
        End Function

#End Region '/RANGE

#Region "CORRECTION"

        ''' <summary>
        ''' Configures all correction parameters.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="offsetState">Switches the offset correction on or off. Default Value: Off.</param>
        ''' <param name="offset">Sets a fixed offset value can be defined for multiplying (logarithmically adding) the measured value of a sensor. Valid Range: -200-200 dB. Default Value: 0 dB.</param>
        ''' <param name="reserved1">Reserved. Value is ignored.</param>
        ''' <param name="reserved2">Reserved. Value is ignored. Default Value: "".</param>
        ''' <param name="sParameterEnable">Enables or disables measured-value correction by means of the stored s-parameter device. Default Value: Off.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:CORR:OFFS
        ''' SENS:CORR:OFFS:STAT ON | OFF
        ''' SENS:CORR:SPD:STAT ON | OFF
        ''' </remarks>
        Public Sub Corr_ConfigureCorrections(channel As Integer, offsetState As Boolean, offset As Double, reserved1 As Boolean, reserved2 As String, sParameterEnable As Boolean)
            Dim pInvokeResult As Integer = Native.corr_configureCorrections(Me._Handle, channel, System.Convert.ToUInt16(offsetState), offset, System.Convert.ToUInt16(reserved1), reserved2, System.Convert.ToUInt16(sParameterEnable))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Informs the R&amp;S NRP about the frequency of the power to be measured since this frequency is not automatically determined.
        ''' The frequency is used to determine a frequency-dependent correction factor for the measurement results.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="frequency">Sets the frequency in Hz of the power to be measured since this frequency is not automatically determined.
        ''' Valid Range: NRP-Z21: 10e6-18e9; FSH-Z1: 10e6-8e9; NRP-Z51: 0-18e9 (depends on the calibration data). Default Value: 50e6 Hz.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:FREQ
        ''' </remarks>
        Public Sub Chan_SetCorrectionFrequency(ByVal channel As Integer, ByVal frequency As Double)
            Dim pInvokeResult As Integer = Native.Chan_SetCorrectionFrequency(Me._Handle, channel, frequency)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the instrument for the frequency in Hz of the power to be measured since this frequency is not automatically determined.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe[1..4]:FREQuency?
        ''' </remarks>
        Public Function Chan_GetCorrectionFrequency(ByVal channel As Integer) As Double
            Dim frequency As Double
            Dim pInvokeResult As Integer = Native.Chan_GetCorrectionFrequency(Me._Handle, channel, frequency)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return frequency
        End Function

        ''' <summary>
        ''' If the frequency step parameter is set to a value greater than 0 the sensor does a internal frequency increment if buffered mode is enabled
        ''' Depending on the parameter "frequency spacing" the sensor adds this value to the current frequency or it multiplies this value with the current frequency.
        ''' This function is used to do a simple scalar network nalysis. 
        ''' To enable this automativally frequency stepping you have to configure CONTAV sensor mode, enable buffered measurements and set frequency stepping to a value greater than 0.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="frequencyStep">Sets the frequency step value. Valid Range: 0-0.5 * MaxFreq. Default Value: 0.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:FREQ:STEP
        ''' </remarks>
        Public Sub Chan_SetCorrectionFrequencyStep(ByVal channel As Integer, ByVal frequencyStep As Double)
            Dim pInvokeResult As Integer = Native.Chan_SetCorrectionFrequencyStep(Me._Handle, channel, frequencyStep)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the frequency step value.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:FREQ:STEP?
        ''' </remarks>
        Public Function Chan_GetCorrectionFrequencyStep(ByVal channel As Integer) As Double
            Dim frequencyStep As Double
            Dim pInvokeResult As Integer = Native.Chan_GetCorrectionFrequencyStep(Me._Handle, channel, frequencyStep)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return frequencyStep
        End Function

        ''' <summary>
        ''' If scalar network analysis is enabled Defines how the frequency is incremented.
        ''' Linear spacing means that the frequency step value is added to the current frequency after each buffered measurement.
        ''' Logarithmic spacing means that the frequency step value is multiplied with the current frequency after each buffered measurement.
        ''' This command is used to do a simple scalar network nalysis. 
        ''' To enable this automativally frequency stepping you have to configure CONTAV sensor mode, enable buffered measurements and set frequency stepping to a value greater than 0.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="frequencySpacing">Selects the frequency spacing value. Default Value: <see cref="Spacing.Linear"/>.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:FREQ:SPAC
        ''' </remarks>
        Public Sub Chan_SetCorrectionFrequencySpacing(ByVal channel As Integer, ByVal frequencySpacing As Spacing)
            Dim pInvokeResult As Integer = Native.Chan_SetCorrectionFrequencySpacing(Me._Handle, channel, frequencySpacing)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the frequency spacing value.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:FREQ:SPAC?
        ''' </remarks>
        Public Function Chan_GetCorrectionFrequencySpacing(ByVal channel As Integer) As Spacing
            Dim frequencySpacing As Integer
            Dim pInvokeResult As Integer = Native.Chan_GetCorrectionFrequencySpacing(Me._Handle, channel, frequencySpacing)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return CType(frequencySpacing, Spacing)
        End Function

        ''' <summary>
        ''' With this function a fixed offset value can be defined for multiplying (logarithmically adding) the measured value of a sensor.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="offset">Sets a fixed offset value can be defined for multiplying (logarithmically adding) the measured value of a sensor. Valid Range: -200-200 dB. Default Value: 0 dB.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:CORR:OFFS
        ''' </remarks>
        Public Sub Corr_SetOffset(ByVal channel As Integer, ByVal offset As Double)
            Dim pInvokeResult As Integer = Native.corr_setOffset(Me._Handle, channel, offset)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Gets a fixed offset value defined for multiplying (logarithmically adding) the measured value of a sensor.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:CORR:OFFS?
        ''' </remarks>
        Public Function Corr_GetOffset(ByVal channel As Integer) As Double
            Dim offset As Double
            Dim pInvokeResult As Integer = Native.corr_getOffset(Me._Handle, channel, offset)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return offset
        End Function

        ''' <summary>
        ''' Switches the offset correction on or off.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="offsetState">Switches the offset correction on or off. Default Value: Off.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:CORR:OFFS:STAT ON | OFF
        ''' </remarks>
        Public Sub Corr_SetOffsetEnabled(ByVal channel As Integer, ByVal offsetState As Boolean)
            Dim pInvokeResult As Integer = Native.corr_setOffsetEnabled(Me._Handle, channel, System.Convert.ToUInt16(offsetState))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the offset correction on or off.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:CORR:OFFS:STAT?
        ''' </remarks>
        Public Function Corr_GetOffsetEnabled(ByVal channel As Integer) As Boolean
            Dim offsetStateAsUShort As UShort
            Dim pInvokeResult As Integer = Native.corr_getOffsetEnabled(Me._Handle, channel, offsetStateAsUShort)
            Dim offsetState As Boolean = System.Convert.ToBoolean(offsetStateAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return offsetState
        End Function

        ''' <summary>
        ''' Instructs the sensor to perform a measured-value correction by means of the stored s-parameter device.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="sParameterEnable">Enables or disables measured-value correction by means of the stored s-parameter device. Default Value: Off.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:CORR:SPD:STAT ON | OFF
        ''' </remarks>
        Public Sub Corr_SetSParamDeviceEnabled(ByVal channel As Integer, ByVal sParameterEnable As Boolean)
            Dim pInvokeResult As Integer = Native.corr_setSParamDeviceEnabled(Me._Handle, channel, System.Convert.ToUInt16(sParameterEnable))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the state of a measured-value correction by means of the stored s-parameter device.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe[1..4]:CORRection:SPDevice:STATe?
        ''' </remarks>
        Public Function Corr_GetSParamDeviceEnabled(ByVal channel As Integer) As Boolean
            Dim sParameterCorrectionAsUShort As UShort
            Dim pInvokeResult As Integer = Native.corr_getSParamDeviceEnabled(Me._Handle, channel, sParameterCorrectionAsUShort)
            Dim sParameterCorrection As Boolean = System.Convert.ToBoolean(sParameterCorrectionAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return sParameterCorrection
        End Function

        ''' <summary>
        ''' This function can be used to select a loaded data set for S-parameter correction. 
        ''' This data set is accessed by means of a consecutive number, starting with 1 for the first data set. 
        ''' If an invalid data set consecutive number is entered, an error message is output.
        ''' This function is available only on NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="sParameter">This control can be used to select a loaded data set for S-parameter correction.
        ''' This data set is accessed by means of a consecutive number, starting with 1 for the first data set.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:CORR:SPD:SEL
        ''' </remarks>
        Public Sub Corr_SetSParamDevice(ByVal channel As Integer, ByVal sParameter As Integer)
            Dim pInvokeResult As Integer = Native.corr_setSParamDevice(Me._Handle, channel, sParameter)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Gets selected data set for S-parameter correction. This function is available only on NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:CORR:SPD:SEL?
        ''' </remarks>
        Public Function Corr_GetSParamDevice(ByVal channel As Integer) As Integer
            Dim sParameter As Integer
            Dim pInvokeResult As Integer = Native.corr_getSParamDevice(Me._Handle, channel, sParameter)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return sParameter
        End Function

        ''' <summary>
        ''' Gets list of S-Parameter devices. This function is available only on NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="bufferSize">Defines the size of buffer. Valid Range: not checked. Default Value: 1000.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:CORR:SPD:LIST?
        ''' </remarks>
        Public Function Corr_GetSParamDevList(ByVal channel As Integer, ByVal bufferSize As Integer) As Text.StringBuilder
            Dim sParameterDeviceList As New System.Text.StringBuilder()
            Dim pInvokeResult As Integer = Native.corr_getSParamDevList(Me._Handle, channel, bufferSize, sParameterDeviceList)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return sParameterDeviceList
        End Function

        ''' <summary>
        ''' Configures all duty cycle parameters.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="dutyCycleState">Switches measured-value correction for a specific duty cycle on or off. Default Value: Off.</param>
        ''' <param name="dutyCycle">Sets the duty cycle of power to be measured. Valid Range: 0.001-99.999%. Default Value: 1.0%.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:CORR:DCYC
        ''' SENS:CORR:DCYC:STAT ON | OFF
        ''' Notes: Actual valid range depends on sensor used.
        ''' </remarks>
        Public Sub Corr_ConfigureDutyCycle(ByVal channel As Integer, ByVal dutyCycleState As Boolean, ByVal dutyCycle As Double)
            Dim pInvokeResult As Integer = Native.corr_configureDutyCycle(Me._Handle, channel, System.Convert.ToUInt16(dutyCycleState), dutyCycle)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Informs the R&amp;S NRP about the duty cycle of the power to be measured. 
        ''' Specifying a duty cycle only makes sense in the ContAv mode where measurements are performed continuously without taking the timing of the signal into account. 
        ''' For this reason, this setting can only be chosen in the local mode when the sensor performs measurements in the ContAv mode.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="dutyCycle">Sets the duty cycle of power to be measured. Valid Range: 0.001-99.999%. Default Value: 1.0%.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:CORR:DCYC
        ''' Notes: Actual valid range depends on sensor used.
        ''' </remarks>
        Public Sub Corr_SetDutyCycle(ByVal channel As Integer, ByVal dutyCycle As Double)
            Dim pInvokeResult As Integer = Native.corr_setDutyCycle(Me._Handle, channel, dutyCycle)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Gets the size of duty cycle of the power to be measured. Units are %.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:CORR:DCYC?
        ''' </remarks>
        Public Function Corr_GetDutyCycle(ByVal channel As Integer) As Double
            Dim dutyCycle As Double
            Dim pInvokeResult As Integer = Native.corr_getDutyCycle(Me._Handle, channel, dutyCycle)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return dutyCycle
        End Function

        ''' <summary>
        ''' Switches measured-value correction for a specific duty cycle on or off.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="dutyCycleState">Switches measured-value correction for a specific duty cycle on or off. Default Value: Off.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:CORR:DCYC:STAT ON | OFF
        ''' </remarks>
        Public Sub Corr_SetDutyCycleEnabled(ByVal channel As Integer, ByVal dutyCycleState As Boolean)
            Dim pInvokeResult As Integer = Native.corr_setDutyCycleEnabled(Me._Handle, channel, System.Convert.ToUInt16(dutyCycleState))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Gets the setting of duty cycle.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:CORR:DCYC:STAT?
        ''' </remarks>
        Public Function Corr_GetDutyCycleEnabled(ByVal channel As Integer) As Boolean
            Dim dutyCycleStateAsUShort As UShort
            Dim pInvokeResult As Integer = Native.corr_getDutyCycleEnabled(Me._Handle, channel, dutyCycleStateAsUShort)
            Dim dutyCycleState As Boolean = System.Convert.ToBoolean(dutyCycleStateAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return dutyCycleState
        End Function

#End Region '/CORRECTION

#Region "CHAN"

        ''' <summary>
        ''' Sets the sensor to one of the measurement modes.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="measurementMode">Specifies the measurement mode to which sensor should be switched.</param>
        ''' <remarks>
        ''' Remote-control commands: 
        ''' SENSe[1..4]:FUNCtion 
        ''' SENSe[1..4]:BUFFer:STATe ON | OFF 
        ''' </remarks>
        Public Sub Chan_Mode(ByVal channel As Integer, ByVal measurementMode As SensorMode)
            Dim pInvokeResult As Integer = Native.chan_mode(Me._Handle, channel, measurementMode)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Sets the parameters of the reflection coefficient for measured-value correction.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="sourceGammaCorrection">Enables or disables source gamma correction of the measured value. Default Value: Off.</param>
        ''' <param name="magnitude">Sets the magnitude of the reflection coefficient. Valid Range: NRP-Z21 0-1; FSH-Z1: 0-1. Default Value: 1.</param>
        ''' <param name="phase">Defines the phase angle of the reflection coefficient. Units are degrees. Valid Range: -360-360 deg. Default Value: 0 deg.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:SGAM
        ''' SENS:SGAM:PHAS
        ''' SENS:SGAM:CORR:STAT ON | OFF
        ''' </remarks>
        Public Sub Chan_ConfigureSourceGammaCorr(ByVal channel As Integer, ByVal sourceGammaCorrection As Boolean, ByVal magnitude As Double, ByVal phase As Double)
            Dim pInvokeResult As Integer = Native.chan_configureSourceGammaCorr(Me._Handle, channel, System.Convert.ToUInt16(sourceGammaCorrection), magnitude, phase)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Sets the magnitude of the reflection coefficient for measured-value correction.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="magnitude">Sets the magnitude of the reflection coefficient. Valid Range: NRP-Z21 0-1; FSH-Z1: 0-1. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:SGAM:MAGN
        ''' </remarks>
        Public Sub Chan_SetSourceGammaMagnitude(ByVal channel As Integer, ByVal magnitude As Double)
            Dim pInvokeResult As Integer = Native.Chan_SetSourceGammaMagnitude(Me._Handle, channel, magnitude)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads the magnitude of the reflection coefficient for measured-value correction.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:SGAM:MAGN?
        ''' </remarks>
        Public Function Chan_GetSourceGammaMagnitude(ByVal channel As Integer) As Double
            Dim magnitude As Double
            Dim pInvokeResult As Integer = Native.Chan_GetSourceGammaMagnitude(Me._Handle, channel, magnitude)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return magnitude
        End Function

        ''' <summary>
        ''' Sets the phase angle of the reflection coefficient for measured-value correction.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="phase">Defines the phase angle of the reflection coefficient, degrees. Valid Range: -360-360 deg. Default Value: 0 deg.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:SGAM:PHAS
        ''' </remarks>
        Public Sub Chan_SetSourceGammaPhase(ByVal channel As Integer, ByVal phase As Double)
            Dim pInvokeResult As Integer = Native.Chan_SetSourceGammaPhase(Me._Handle, channel, phase)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads the phase angle of the reflection coefficient for measured-value correction. Units are degrees.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:SGAM:PHAS?
        ''' </remarks>
        Public Function Chan_GetSourceGammaPhase(ByVal channel As Integer) As Double
            Dim phase As Double
            Dim pInvokeResult As Integer = Native.Chan_GetSourceGammaPhase(Me._Handle, channel, phase)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return phase
        End Function

        ''' <summary>
        ''' Switches the measured-value correction of the reflection coefficient effect of the source gamma ON or OFF.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="sourceGammaCorrection">Enables or disables source gamma correction of the measured value. Default Value: Off.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:SGAM:CORR:STAT ON | OFF
        ''' </remarks>
        Public Sub Chan_SetSourceGammaCorrEnabled(ByVal channel As Integer, ByVal sourceGammaCorrection As Boolean)
            Dim pInvokeResult As Integer = Native.Chan_SetSourceGammaCorrEnabled(Me._Handle, channel, System.Convert.ToUInt16(sourceGammaCorrection))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads the state of source gamma correction.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:RGAM:CORR:STAT?
        ''' </remarks>
        Public Function Chan_GetSourceGammaCorrEnabled(ByVal channel As Integer) As Boolean
            Dim sourceGammaCorrectionAsUShort As UShort
            Dim pInvokeResult As Integer = Native.Chan_GetSourceGammaCorrEnabled(Me._Handle, channel, sourceGammaCorrectionAsUShort)
            Dim sourceGammaCorrection As Boolean = System.Convert.ToBoolean(sourceGammaCorrectionAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return sourceGammaCorrection
        End Function

        ''' <summary>
        ''' Sets the parameters of the compensation of the load distortion at the signal output. This function is available only for sensors NRP-Z27 and NRP-Z37.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="magnitude">Sets the magnitude of the reflection coefficient of the load for distortion compensation. Valid Range: 0-1. Default Value: 0.</param>
        ''' <param name="phase">Defines the phase angle (in degrees) of the complex reflection factor of the load at the signal output. Valid Range: -360-360 deg. Default Value: 0 deg.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:RGAM
        ''' SENS:RGAM:PHAS
        ''' </remarks>
        Public Sub Chan_ConfigureReflectGammaCorr(ByVal channel As Integer, ByVal magnitude As Double, ByVal phase As Double)
            Dim pInvokeResult As Integer = Native.chan_configureReflectGammaCorr(Me._Handle, channel, magnitude, phase)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Sets the magnitude of the reflection coefficient of the load for distortion compensation.
        ''' To deactivate distortion compensation, set <paramref name="magnitude"/> to 0. 
        ''' Distortion compensation should remain deactivated in the case of questionable measurement values for the reflection coefficient of the load.
        ''' This function is available only for sensors NRP-Z27 and NRP-Z37.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="magnitude">Sets the magnitude of the reflection coefficient of the load for distortion compensation. Valid Range: 0-1. Default Value: 0.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:RGAM:MAGN
        ''' </remarks>
        Public Sub Chan_SetReflectionGammaMagn(ByVal channel As Integer, ByVal magnitude As Double)
            Dim pInvokeResult As Integer = Native.Chan_SetReflectionGammaMagn(Me._Handle, channel, magnitude)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads the magnitude of the reflection coefficient of the load for distortion compensation. This function is available only for sensors NRP-Z27 and NRP-Z37.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:RGAM:MAGN?
        ''' </remarks>
        Public Function Chan_GetReflectionGammaMagn(ByVal channel As Integer) As Double
            Dim magnitude As Double
            Dim pInvokeResult As Integer = Native.Chan_GetReflectionGammaMagn(Me._Handle, channel, magnitude)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return magnitude
        End Function

        ''' <summary>
        ''' Defines the phase angle of the complex reflection factor of the load at the signal output.
        ''' This function is available only for sensors NRP-Z27 and NRP-Z37.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="phase">Defines the phase angle (in degrees) of the complex reflection factor of the load at the signal output. Valid Range:-360-360 deg. Default Value: 0 deg.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:RGAM:PHAS
        ''' </remarks>
        Public Sub Chan_SetReflectionGammaPhase(ByVal channel As Integer, ByVal phase As Double)
            Dim pInvokeResult As Integer = Native.Chan_SetReflectionGammaPhase(Me._Handle, channel, phase)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads the phase angle (in degrees) of the complex reflection factor of the load at the signal output. This function is available only for sensors NRP-Z27 and NRP-Z37.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:RGAM:PHAS?
        ''' </remarks>
        Public Function Chan_GetReflectionGammaPhase(ByVal channel As Integer) As Double
            Dim phase As Double
            Dim pInvokeResult As Integer = Native.Chan_GetReflectionGammaPhase(Me._Handle, channel, phase)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return phase
        End Function

        ''' <summary>
        ''' Defines reflection gamma uncertainty. This function is available only for sensors NRP-Z27 and NRP-Z37.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="uncertainty">Defines the uncertainty. Valid Range: 0-1. Default Value: 0.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:RGAM:EUNC
        ''' </remarks>
        Public Sub Chan_SetReflectionGammaUncertainty(ByVal channel As Integer, ByVal uncertainty As Double)
            Dim pInvokeResult As Integer = Native.Chan_SetReflectionGammaUncertainty(Me._Handle, channel, uncertainty)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries reflection gamma uncertainty.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:RGAM:EUNC?
        ''' Note: This function is available only for sensors NRP-Z27 and NRP-Z37.
        ''' </remarks>
        Public Function Chan_GetReflectionGammaUncertainty(ByVal channel As Integer) As Double
            Dim uncertainty As Double
            Dim pInvokeResult As Integer = Native.Chan_GetReflectionGammaUncertainty(Me._Handle, channel, uncertainty)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return uncertainty
        End Function

        ''' <summary>
        ''' Determines the integration time for a single measurement in the ContAv mode. 
        ''' To increase the measurement accuracy, this integration is followed by a second averaging procedure in a window with a selectable number of values.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="contAvAperture">Defines the ContAv Aperture in seconds.
        ''' Valid Range: NRP-Z21: 0.1e-6-0.3 s; NRP-Z51: 0.1e-3-0.3 s; FSH-Z1: 0.1e-6-0.3 s. Default Value: 0.02 s.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:POW:AVG:APER
        ''' </remarks>
        Public Sub Chan_SetContAvAperture(ByVal channel As Integer, ByVal contAvAperture As Double)
            Dim pInvokeResult As Integer = Native.Chan_SetContAvAperture(Me._Handle, channel, contAvAperture)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the value of ContAv mode aperture size in seconds.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:POW:AVG:APER?
        ''' </remarks>
        Public Function Chan_GetContAvAperture(ByVal channel As Integer) As Double
            Dim contAvAperture As Double
            Dim pInvokeResult As Integer = Native.Chan_GetContAvAperture(Me._Handle, channel, contAvAperture)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return contAvAperture
        End Function

        ''' <summary>
        ''' Activates digital lowpass filtering of the sampled video signal.
        ''' The problem of instable display values due to a modulation of a test signal can also be eliminated by lowpass filtering of the video signal. 
        ''' The lowpass filter eliminates the variations of the display even in case of unperiodic modulation and does not require any other setting.
        ''' If the modulation is periodic, the setting of the sampling window is the better method since it allows for shorter measurement times.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="contAvSmoothing">Sets the state of digital lowpass filtering of the sampled video signal.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:POW:AVG:SMO:STAT ON | OFF
        ''' </remarks>
        Public Sub Chan_SetContAvSmoothingEnabled(ByVal channel As Integer, ByVal contAvSmoothing As Boolean)
            Dim pInvokeResult As Integer = Native.Chan_SetContAvSmoothingEnabled(Me._Handle, channel, System.Convert.ToUInt16(contAvSmoothing))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Gets the state of digital lowpass filtering of the sampled video signal.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:POW:AVG:SMO:STAT?
        ''' </remarks>
        Public Function Chan_GetContAvSmoothingEnabled(ByVal channel As Integer) As Boolean
            Dim contAv_SmoothingAsUShort As UShort
            Dim pInvokeResult As Integer = Native.Chan_GetContAvSmoothingEnabled(Me._Handle, channel, contAv_SmoothingAsUShort)
            Dim contAvSmoothing As Boolean = System.Convert.ToBoolean(contAv_SmoothingAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return contAvSmoothing
        End Function

        ''' <summary>
        ''' Switches on the buffered ContAv mode, after which data blocks rather than single measured values are then returned. 
        ''' In this mode a higher data rate is achieved than in the non-buffered ContAv mode.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="contAvBufferedMode">Turns on or off ContAv buffered measurement mode. Default Value: Off.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:POW:AVG:BUFF:STAT ON | OFF
        ''' </remarks>
        Public Sub Chan_SetContAvBufferedEnabled(ByVal channel As Integer, ByVal contAvBufferedMode As Boolean)
            Dim pInvokeResult As Integer = Native.Chan_SetContAvBufferedEnabled(Me._Handle, channel, System.Convert.ToUInt16(contAvBufferedMode))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the state of ContAv Buffered Measurement Mode.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command:
        ''' SENS:POW:AVG:BUFF:STAT?
        ''' </remarks>
        Public Function Chan_GetContAvBufferedEnabled(ByVal channel As Integer) As Boolean
            Dim contAvBufferedModeAsUShort As UShort
            Dim pInvokeResult As Integer = Native.Chan_GetContAvBufferedEnabled(Me._Handle, channel, contAvBufferedModeAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim contAvBufferedMode As Boolean = System.Convert.ToBoolean(contAvBufferedModeAsUShort)
            Return contAvBufferedMode
        End Function

        ''' <summary>
        ''' Sets the number of desired values for the buffered ContAv mode.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="bufferSize">Sets the number of desired values for buffered ContAv mode. Valid Range: 1..1024. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:POW:AVG:BUFF:SIZE
        ''' </remarks>
        Public Sub Chan_SetContAvBufferSize(ByVal channel As Integer, ByVal bufferSize As Integer)
            Dim pInvokeResult As Integer = Native.Chan_SetContAvBufferSize(Me._Handle, channel, bufferSize)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the number of desired values for the buffered ContAv mode.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:POW:AVG:BUFF:SIZE?
        ''' </remarks>
        Public Function Chan_GetContAvBufferSize(ByVal channel As Integer) As Integer
            Dim bufferSize As Integer
            Dim pInvokeResult As Integer = Native.Chan_GetContAvBufferSize(Me._Handle, channel, bufferSize)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return bufferSize
        End Function

        ''' <summary>
        ''' Returns the number of measurement values currently stored in the sensor buffer while the buffered measurement is running.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:POW:AVG:BUFF:COUN?
        ''' </remarks>
        Public Function Chan_GetContAvBufferCount(ByVal channel As Integer) As Integer
            Dim bufferCount As Integer
            Dim pInvokeResult As Integer = Native.Chan_GetContAvBufferCount(Me._Handle, channel, bufferCount)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return bufferCount
        End Function

        ''' <summary>
        ''' Returns some important settings for the scalar network analysis. 
        ''' The information for the types "FAST", "NORMAL" and "HIGHPRECISION" is a comma separated string including the following fields:
        ''' 1) infotype,
        ''' 2) aperture time,
        ''' 3) average count,
        ''' 4) min. time between two trigger events,
        ''' 5) trigger delay,
        ''' 6) flag if this mode is available (0 if not).
        ''' The type "TRACEMODE" returns a "1" if tracemode is supported by the sensor.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="infoType">
        ''' Specifies which info should be retrieved from the sensor. If no infoType is given the sensor returns the complete information string.
        ''' Valid Values: "FAST", "NORMAL", "HIGHPRECISION", "TRACEMODE". Default Value: "".
        ''' </param>
        ''' <param name="arraySize">Defines the size of array 'Info'. Default Value: 100.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:POW:AVG:BUFF:INFO? &lt;Info Type&gt;
        ''' </remarks>
        Public Function Chan_GetContAvBufferInfo(ByVal channel As Integer, ByVal infoType As String, ByVal arraySize As Integer) As Text.StringBuilder
            Dim info As New Text.StringBuilder()
            Dim pInvokeResult As Integer = Native.Chan_GetContAvBufferInfo(Me._Handle, channel, infoType, arraySize, info)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return info
        End Function

        ''' <summary>
        ''' The end of a burst (power pulse) is recognized when the signal level drops below the trigger level. 
        ''' Especially with modulated signals, this may also happen for a short time within a burst. 
        ''' To prevent the supposed end of the burst is from being recognized too early or incorrectly at these positions, 
        ''' a time interval can be defined via using this function (drop-out tolerance parameter) in which the pulse end 
        ''' is only recognized if the signal level no longer exceeds the trigger level.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="dropOutTolerance">Defines the Drop-Out Tolerance time interval in seconds. Valid Range: 0-3e-3 s. Default Value: 100e-6 s.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:POW:BURS:DTOL
        ''' </remarks>
        Public Sub Chan_SetBurstDropoutTolerance(ByVal channel As Integer, ByVal dropOutTolerance As Double)
            Dim pInvokeResult As Integer = Native.Chan_SetBurstDropoutTolerance(Me._Handle, channel, dropOutTolerance)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the drop-out tolerance parameter. This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:POW:BURS:DTOL?
        ''' </remarks>
        Public Function Chan_GetBurstDropoutTolerance(ByVal channel As Integer) As Double
            Dim dropOutTolerance As Double
            Dim pInvokeResult As Integer = Native.Chan_GetBurstDropoutTolerance(Me._Handle, channel, dropOutTolerance)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return dropOutTolerance
        End Function

        ''' <summary>
        ''' This function enables or disables the chopper in BurstAv mode. 
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="burstAvChopper">Enables or disables the chopper for BurstAv mode. Default Value: On.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:POWer:BURSt:CHOPper:STATe
        ''' Disabling the chopper is only good for fast but unexact and noisy measurements. 
        ''' If the chopper is disabled, averaging is ignored internally also disabled.
        ''' </remarks>
        Public Sub Chan_SetBurstChopperEnabled(ByVal channel As Integer, ByVal burstAvChopper As Boolean)
            Dim pInvokeResult As Integer = Native.Chan_SetBurstChopperEnabled(Me._Handle, channel, System.Convert.ToUInt16(burstAvChopper))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the state of the chopper in BurstAv mode.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:POWer:BURSt:CHOPper:STATe?
        ''' </remarks>
        Public Function Chan_GetBurstChopperEnabled(ByVal channel As Integer) As Boolean
            Dim burstAvChopperAsUShort As UShort
            Dim pInvokeResult As Integer = Native.Chan_GetBurstChopperEnabled(Me._Handle, channel, burstAvChopperAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim burstAvChopper As Boolean = System.Convert.ToBoolean(burstAvChopperAsUShort)
            Return burstAvChopper
        End Function

#End Region '/CHAN

#Region "STAT"

        ''' <summary>
        ''' Configures the timegate (depends on trigger event) in which the sensor is doing statistic measurements.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="offset">Sets the start after the trigger of the timegate in which the sensor is doing statistic measurements. Default Value: 0 s.</param>
        ''' <param name="time">Sets the length of the timegate in which the sensor is doing statistic measurements. Valid Range: 1E-6-0.3 s. Default Value: 0.01 s.</param>
        ''' <param name="midambleOffset">Sets the midamble offset after timeslot start in seconds in the timegate in which the sensor is doing statistic measurements. Valid Range: 0..10 s. Default Value: 0 s.</param>
        ''' <param name="midambleLength">Sets the midamble length in seconds. Valid Range: 0..10 s. Default Value: 0 s.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:STATistics:OFFSet:TIME
        ''' SENSe:STATistics:TIME
        ''' SENSe:STATistics:[EXCLude]:MID:OFFSet[:TIME]
        ''' SENSe:STATistics:[EXCLude]:MID:TIME
        ''' </remarks>
        Public Sub Stat_ConfTimegate(channel As Integer, offset As Double, time As Double, midambleOffset As Double, midambleLength As Double)
            Dim pInvokeResult As Integer = Native.stat_confTimegate(Me._Handle, channel, offset, time, midambleOffset, midambleLength)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Sets the X-Axis of statistical measurement.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="referenceLevel">Sets the lower limit of the level range for the analysis result in both Statistics modes. 
        ''' This level can be assigned to the first pixel. 
        ''' The level assigned to the last pixel is equal to the level of the first pixel plus the level range.
        ''' Valid Range: -80..20 dB. Default Value: -30 dB.
        ''' </param>
        ''' <param name="range">Sets the width of the level range for the analysis result in both Statistics modes. Valid Range: 0.01..100. Default Value: 50.</param>
        ''' <param name="points">Sets the measurement-result resolution in both Statistics modes. 
        ''' This function specifies the number of pixels that are to be assigned to the logarithmic level range (rsnrpz_stat_setScaleRange function) for measured value output. 
        ''' The width of the level range divided by N-1, where N is the number of pixels, must not be less than the value which can be read out with rsnrpz_stat_getScaleWidth.
        ''' Valid Range: 3..8192. Default Value: 200.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:STATistics:SCALE:X:RLEVel
        ''' SENSe:STATistics:SCALE:X:RANGe
        ''' SENSe:STATistics:SCALE:X:POINts
        ''' </remarks>
        Public Sub Stat_ConfScale(ByVal channel As Integer, ByVal referenceLevel As Double, ByVal range As Double, ByVal points As Integer)
            Dim pInvokeResult As Integer = Native.stat_confScale(Me._Handle, channel, referenceLevel, range, points)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Sets the start after the trigger of the timegate in which the sensor is doing statistic measurements.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="offset">Sets the start after the trigger of the timegate in which the sensor is doing statistic measurements. Default Value: 0 s.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:STATistics:OFFSet:TIME
        ''' </remarks>
        Public Sub Stat_SetOffsetTime(ByVal channel As Integer, ByVal offset As Double)
            Dim pInvokeResult As Integer = Native.stat_setOffsetTime(Me._Handle, channel, offset)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the start after the trigger of the timegate in which the sensor is doing statistic measurements.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:STATistics:OFFSet:TIME?
        ''' </remarks>
        Public Function Stat_GetOffsetTime(ByVal channel As Integer) As Double
            Dim offset As Double
            Dim pInvokeResult As Integer = Native.stat_getOffsetTime(Me._Handle, channel, offset)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return offset
        End Function

        ''' <summary>
        ''' Sets the length of the timegate in which the sensor is doing statistic measurements.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="time">Sets the length of the timegate in which the sensor is doing statistic measurements. Valid Range: 1E-6..0.3 s. Default Value: 0.01 s.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:STATistics:TIME
        ''' </remarks>
        Public Sub Stat_SetTime(ByVal channel As Integer, ByVal time As Double)
            Dim pInvokeResult As Integer = Native.stat_setTime(Me._Handle, channel, time)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the length of the timegate in which the sensor is doing statistic measurements.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:STATistics:TIME?
        ''' </remarks>
        Public Function Stat_GetTime(ByVal channel As Integer) As Double
            Dim time As Double
            Dim pInvokeResult As Integer = Native.stat_getTime(Me._Handle, channel, time)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return time
        End Function

        ''' <summary>
        ''' Sets the midamble offset after timeslot start in seconds in the timegate in which the sensor is doing statistic measurements.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="offset">Sets the midamble offset after timeslot start in seconds in the timegate in which the sensor is doing statistic measurements. Valid Range: 0..10 s. Default Value: 0 s.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:STATistics:[EXCLude]:MID:OFFSet[:TIME]
        ''' </remarks>
        Public Sub Stat_SetMidOffset(ByVal channel As Integer, ByVal offset As Double)
            Dim pInvokeResult As Integer = Native.stat_setMidOffset(Me._Handle, channel, offset)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the midamble offset after timeslot start in seconds in the timegate in which the sensor is doing statistic measurements.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:STATistics:[EXCLude]:MID:OFFSet[:TIME]?
        ''' </remarks>
        Public Function Stat_GetMidOffset(ByVal channel As Integer) As Double
            Dim offset As Double
            Dim pInvokeResult As Integer = Native.stat_getMidOffset(Me._Handle, channel, offset)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return offset
        End Function

        ''' <summary>
        ''' Sets the midamble length in seconds.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="length">Sets the midamble length in seconds. Valid Range: 0..10 s. Default Value: 0 s.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:STATistics:[EXCLude]:MID:TIME
        ''' </remarks>
        Public Sub Stat_SetMidLength(ByVal channel As Integer, ByVal length As Double)
            Dim pInvokeResult As Integer = Native.stat_setMidLength(Me._Handle, channel, length)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the midamble length in seconds.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:STATistics:[EXCLude]:MID:TIME?
        ''' </remarks>
        Public Function Stat_GetMidLength(ByVal channel As Integer) As Double
            Dim length As Double
            Dim pInvokeResult As Integer = Native.stat_getMidLength(Me._Handle, channel, length)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return length
        End Function

        ''' <summary>
        ''' Sets the lower limit of the level range for the analysis result in both Statistics modes. 
        ''' This level can be assigned to the first pixel. The level assigned to the last pixel is equal to the level of the first pixel plus the level range.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="referenceLevel">Sets the lower limit of the level range for the analysis result in both Statistics modes. 
        ''' This level can be assigned to the first pixel. 
        ''' The level assigned to the last pixel is equal to the level of the first pixel plus the level range.
        ''' Valid Range: -80..20 dB. Default Value: -30 dB.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:STATistics:SCALE:X:RLEVel
        ''' </remarks>
        Public Sub Stat_SetScaleRefLevel(ByVal channel As Integer, ByVal referenceLevel As Double)
            Dim pInvokeResult As Integer = Native.stat_setScaleRefLevel(Me._Handle, channel, referenceLevel)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the lower limit of the level range for the analysis result in both Statistics modes. 
        ''' This level can be assigned to the first pixel. The level assigned to the last pixel is equal to the level of the first pixel plus the level range.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:STATistics:SCALE:X:RLEVel?
        ''' </remarks>
        Public Function Stat_GetScaleRefLevel(ByVal channel As Integer) As Double
            Dim referenceLevel As Double
            Dim pInvokeResult As Integer = Native.stat_getScaleRefLevel(Me._Handle, channel, referenceLevel)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return referenceLevel
        End Function

        ''' <summary>
        ''' Sets the width of the level range for the analysis result in both Statistics modes.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="range">Sets the width of the level range for the analysis result in both Statistics modes. Valid Range: 0.01..100. Default Value: 50.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:STATistics:SCALE:X:RANGe
        ''' </remarks>
        Public Sub Stat_setScaleRange(ByVal channel As Integer, ByVal range As Double)
            Dim pInvokeResult As Integer = Native.stat_setScaleRange(Me._Handle, channel, range)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the width of the level range for the analysis result in both Statistics modes.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:STATistics:SCALE:X:RANGe?
        ''' </remarks>
        Public Function Stat_GetScaleRange(ByVal channel As Integer) As Double
            Dim range As Double
            Dim pInvokeResult As Integer = Native.stat_getScaleRange(Me._Handle, channel, range)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return range
        End Function

        ''' <summary>
        ''' Sets the measurement-result resolution in both Statistics modes.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="points">This control the measurement-result resolution in both Statistics modes. Valid Range: 3..8192. Default Value: 200.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:STATistics:SCALE:X:POINts
        ''' This function specifies the number of pixels that are to be assigned to the logarithmic level range (rsnrpz_stat_setScaleRange function) for measured value output. 
        ''' The width of the level range divided by N-1, where N is the number of pixels, must not be less than the value which can be read out with rsnrpz_stat_getScaleWidth.
        ''' </remarks>
        Public Sub Stat_SetScalePoints(ByVal channel As Integer, ByVal points As Integer)
            Dim pInvokeResult As Integer = Native.stat_setScalePoints(Me._Handle, channel, points)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the measurement-result resolution in both Statistics modes.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:STATistics:SCALE:X:POINts?
        ''' </remarks>
        Public Function Stat_GetScalePoints(ByVal channel As Integer) As Integer
            Dim points As Integer
            Dim pInvokeResult As Integer = Native.stat_getScalePoints(Me._Handle, channel, points)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return points
        End Function

        ''' <summary>
        ''' Queries the greatest attainable level resolution. For the R&amp;S NRP-Z81 power sensor, this value is fixed at 0.006 dB per pixel. 
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:STATistics:SCALE:X:MPWidth?
        ''' If this value is exceeded, a "Settings conflict" message is output. 
        ''' The reason for the conflict may be that the number of pixels that has been selected is too great 
        ''' or that the width chosen for the level range is too small (rsnrpz_stat_setScalePoints and rsnrpz_stat_setScaleRange functions).
        ''' </remarks>
        Public Function Stat_GetScaleWidth(ByVal channel As Integer) As Double
            Dim width As Double
            Dim pInvokeResult As Integer = Native.stat_getScaleWidth(Me._Handle, channel, width)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return width
        End Function

#End Region '/STAT

#Region "TSLOT"

        ''' <summary>
        ''' Configures the parameters of Timeslot measurement mode. Both exclude start and stop are set to 10% of timeslot width each.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="timeSlotCount">Sets the number of simultaneously measured timeslots in the Timeslot mode. Valid Range: 1..128. Default Value: 8.</param>
        ''' <param name="width">Sets the length in seconds of the timeslot in the Timeslot mode. Valid Range: 10e-6-0.1. Default Value: 1e-3 s.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:POW:TSL:AVG:COUN
        ''' SENS:POW:TSL:AVG:WIDT
        ''' SENS:TIM:EXCL:STAR
        ''' SENS:TIM:EXCL:STOP
        ''' </remarks>
        Public Sub Tslot_ConfigureTimeSlot(ByVal channel As Integer, ByVal timeSlotCount As Integer, ByVal width As Double)
            Dim pInvokeResult As Integer = Native.tslot_configureTimeSlot(Me._Handle, channel, timeSlotCount, width)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Sets the number of simultaneously measured timeslots in the Timeslot mode. This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="timeSlotCount">Sets the number of simultaneously measured timeslots in the Timeslot mode. Valid Range: 1..128. Default Value: 8.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:POW:TSL:AVG:COUN
        ''' </remarks>
        Public Sub Tslot_SetTimeSlotCount(ByVal channel As Integer, ByVal timeSlotCount As Integer)
            Dim pInvokeResult As Integer = Native.tslot_setTimeSlotCount(Me._Handle, channel, timeSlotCount)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads the number of simultaneously measured timeslots in the Timeslot mode.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:POW:TSL:AVG:COUN?
        ''' </remarks>
        Public Function Tslot_GetTimeSlotCount(ByVal channel As Integer) As Integer
            Dim timeSlotCount As Integer
            Dim pInvokeResult As Integer = Native.tslot_getTimeSlotCount(Me._Handle, channel, timeSlotCount)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return timeSlotCount
        End Function

        ''' <summary>
        ''' Sets the length of the timeslot in the Timeslot mode. This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="width">Sets the length in seconds of the timeslot in the Timeslot mode. Valid Range: 10e-6..0.1. Default Value: 1.0e-3 s.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:POW:TSL:AVG:WIDT
        ''' </remarks>
        Public Sub Tslot_SetTimeSlotWidth(ByVal channel As Integer, ByVal width As Double)
            Dim pInvokeResult As Integer = Native.tslot_setTimeSlotWidth(Me._Handle, channel, width)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads the length of the timeslot in the Timeslot mode.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:POW:TSL:AVG:WIDT?
        ''' </remarks>
        Public Function Tslot_GetTimeSlotWidth(ByVal channel As Integer) As Double
            Dim width As Double
            Dim pInvokeResult As Integer = Native.tslot_getTimeSlotWidth(Me._Handle, channel, width)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return width
        End Function

        ''' <summary>
        ''' Sets the start of an exclusion interval in a timeslot. 
        ''' In conjunction with the function rsnrpz_tslot_setTimeSlotMidLength, it is possible to exclude, for example, a midamble from the measurement. 
        ''' The start of the timeslot is used as the reference point for defining the start of the exclusion interval and this applies to each of the timeslots.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="offset">Sets sets the start of an exclusion interval in a timeslot. Valid Range: 0..0.1 s. Default Value: 0 s.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:POWer:TSLot[:AVG]:MID:OFFSet
        ''' </remarks>
        Public Sub Tslot_SetTimeSlotMidOffset(ByVal channel As Integer, ByVal offset As Double)
            Dim pInvokeResult As Integer = Native.tslot_setTimeSlotMidOffset(Me._Handle, channel, offset)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the start of an exclusion interval in a timeslot.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:POWer:TSLot[:AVG]:MID:OFFSet?
        ''' </remarks>
        Public Function Tslot_GetTimeSlotMidOffset(ByVal channel As Integer) As Double
            Dim offset As Double
            Dim pInvokeResult As Integer = Native.tslot_getTimeSlotMidOffset(Me._Handle, channel, offset)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return offset
        End Function

        ''' <summary>
        ''' Sets the length of an exclusion interval in a timeslot. 
        ''' In conjunction with the function rsnrpz_tslot_setTimeSlotMidOffset, it can be used, for example, to exclude a midamble from the measurement.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="length">Sets the length of an exclusion interval in a timeslot. Valid Range: 0..0.1. Default Value: 0 s.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:POWer:TSLot[:AVG]:MID:TIME
        ''' </remarks>
        Public Sub Tslot_SetTimeSlotMidLength(ByVal channel As Integer, ByVal length As Double)
            Dim pInvokeResult As Integer = Native.tslot_setTimeSlotMidLength(Me._Handle, channel, length)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the length of an exclusion interval in a timeslot. Valid Range: 0..0.1 s.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:POWer:TSLot[:AVG]:MID:TIME?
        ''' </remarks>
        Public Function Tslot_GetTimeSlotMidLength(ByVal channel As Integer) As Double
            Dim length As Double
            Dim pInvokeResult As Integer = Native.tslot_getTimeSlotMidLength(Me._Handle, channel, length)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return length
        End Function

        ''' <summary>
        ''' This function enables or disables the chopper in Time Slot mode. Disabling the chopper is only good for fast but unexact and noisy measurements. 
        ''' If the chopper is disabled, averaging is ignored internally also disabled.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="timeSlotChopper">Enables or disables the chopper for Time Slot mode. Default Value: On.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:POWer:TSLot[:AVG]:CHOPper:STATe
        ''' </remarks>
        Public Sub Tslot_SetTimeSlotChopperEnabled(ByVal channel As Integer, ByVal timeSlotChopper As Boolean)
            Dim pInvokeResult As Integer = Native.tslot_setTimeSlotChopperEnabled(Me._Handle, channel, System.Convert.ToUInt16(timeSlotChopper))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the state of the chopper in Time Slot mode.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:POWer:TSLot[:AVG]:CHOPper:STATe?
        ''' </remarks>
        Public Function Tslot_GetTimeSlotChopperEnabled(ByVal channel As Integer) As Boolean
            Dim timeSlotChopperAsUShort As UShort
            Dim pInvokeResult As Integer = Native.tslot_getTimeSlotChopperEnabled(Me._Handle, channel, timeSlotChopperAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim timeSlotChopper As Boolean = System.Convert.ToBoolean(timeSlotChopperAsUShort)
            Return timeSlotChopper
        End Function

#End Region '/TSLOT

#Region "SCOPE"

        ''' <summary>
        ''' Sets parameters of the Scope mode. This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="scopePoints">Sets the number of desired values per Scope sequence. Valid Range: 1..1024. Default Value: 312.</param>
        ''' <param name="scopeTime">Sets the value of scope time. Valid Range: 0.1e-3..0.3 s. Default Value: 0.01 s.</param>
        ''' <param name="offsetTime">Sets the value of offset time. Valid Range: -5e-3..100 s. Default Value: 0 s.</param>
        ''' <param name="realtime">Sets the state of real-time measurement. Default Value: On.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:POIN
        ''' SENS:TRAC:TIME
        ''' SENS:TRAC:OFFS:TIME
        ''' SENS:TRAC:REAL ON | OFF
        ''' </remarks>
        Public Sub Scope_ConfigureScope(ByVal channel As Integer, ByVal scopePoints As Integer, ByVal scopeTime As Double, ByVal offsetTime As Double, ByVal realtime As Boolean)
            Dim pInvokeResult As Integer = Native.scope_configureScope(Me._Handle, channel, scopePoints, scopeTime, offsetTime, System.Convert.ToUInt16(realtime))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Performs fast zeroing, but can be called only in the sensor's Trace mode and Statistics modes. 
        ''' In any other measurement mode, the error message NRPERROR_CALZERO is output. 
        ''' Even though the execution time is shorter than that for standard zeroing by a factor of 100 or more, measurement accuracy is not affected. 
        ''' Fast zeroing is available for the entire frequency range.
        ''' </summary>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' CAL:ZERO:FAST:AUTO
        ''' </remarks>
        Public Sub Scope_FastZero()
            Dim pInvokeResult As Integer = Native.scope_fastZero(Me._Handle)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' For the Scope mode, Switches the filter function of a sensor on or off. 
        ''' When the filter is switched on, the number of measured values set with SENS:TRAC:AVER:COUN (function rsnrpz_scope_setAverageCount) is averaged. 
        ''' This reduces the effect of noise so that more reliable results are obtained.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="scopeAveraging">Switches the filter function of a sensor on or off. Default Value: On.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:AVER:STAT ON | OFF
        ''' </remarks>
        Public Sub Scope_SetAverageEnabled(ByVal channel As Integer, ByVal scopeAveraging As Boolean)
            Dim pInvokeResult As Integer = Native.scope_setAverageEnabled(Me._Handle, channel, System.Convert.ToUInt16(scopeAveraging))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads the state of filter function of a sensor.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:AVER:STAT?
        ''' </remarks>
        Public Function Scope_GetAverageEnabled(ByVal channel As Integer) As Boolean
            Dim scopeAveragingAsUShort As UShort
            Dim pInvokeResult As Integer = Native.scope_getAverageEnabled(Me._Handle, channel, scopeAveragingAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim scopeAveraging As Boolean = System.Convert.ToBoolean(scopeAveragingAsUShort)
            Return scopeAveraging
        End Function

        ''' <summary>
        ''' Sets the length of the filter for the Scope mode. The wider the filter the lower the noise and the longer it takes to obtain a measured value.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="count">Sets the length of the filter for the Scope mode. Valid Range: 1-65536. Default Value: 4.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:AVER:COUN
        ''' </remarks>
        Public Sub Scope_SetAverageCount(ByVal channel As Integer, ByVal count As Integer)
            Dim pInvokeResult As Integer = Native.scope_setAverageCount(Me._Handle, channel, count)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the length of the filter for the Scope mode.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:AVER:COUN?
        ''' </remarks>
        Public Function Scope_GetAverageCount(ByVal channel As Integer) As Integer
            Dim count As Integer
            Dim pInvokeResult As Integer = Native.scope_getAverageCount(Me._Handle, channel, count)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return count
        End Function

        ''' <summary>
        ''' As soon as a new single value is determined, the filter window is advanced by one value so that the new value 
        ''' is taken into account by the filter and the oldest value is forgotten.
        ''' Terminal control then determines in the Scope mode whether a new result will be calculated immediately after 
        ''' a new measured value is available or only after an entire range of new values is available for the filter.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="terminalControl">Determines the type of terminal control. Default value <see cref="TerminalControl.TerminalControlRepeat"/>.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:AVER:TCON MOV | REP
        ''' </remarks>
        Public Sub Scope_SetAverageTerminalControl(ByVal channel As Integer, ByVal terminalControl As TerminalControl)
            Dim pInvokeResult As Integer = Native.scope_setAverageTerminalControl(Me._Handle, channel, terminalControl)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns selected terminal control type in the Scope mode. This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:AVER:TCON?
        ''' </remarks>
        Public Function Scope_GetAverageTerminalControl(ByVal channel As Integer) As TerminalControl
            Dim terminalControl As Integer
            Dim pInvokeResult As Integer = Native.scope_getAverageTerminalControl(Me._Handle, channel, terminalControl)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return CType(terminalControl, TerminalControl)
        End Function

        ''' <summary>
        ''' Determines the relative position of the trigger event in relation to the beginning of the Scope measurement sequence. 
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="offsetTime">Sets the value of offset time. Valid Range: -5e-3-100 s. Default Value: 0 s.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:OFFS:TIME
        ''' </remarks>
        Public Sub Scope_SetOffsetTime(ByVal channel As Integer, ByVal offsetTime As Double)
            Dim pInvokeResult As Integer = Native.scope_setOffsetTime(Me._Handle, channel, offsetTime)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads the relative position of the trigger event in relation to the beginning of the Scope measurement sequence.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:OFFS:TIME?
        ''' </remarks>
        Public Function Scope_GetOffsetTime(ByVal channel As Integer) As Double
            Dim offsetTime As Double
            Dim pInvokeResult As Integer = Native.scope_getOffsetTime(Me._Handle, channel, offsetTime)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return offsetTime
        End Function

        ''' <summary>
        ''' Sets the number of desired values per Scope sequence.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="scopePoints">Sets the number of desired values per Scope sequence. Valid Range: 1..1024. Default Value: 312.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:POIN
        ''' </remarks>
        Public Sub Scope_SetPoints(ByVal channel As Integer, ByVal scopePoints As Integer)
            Dim pInvokeResult As Integer = Native.scope_setPoints(Me._Handle, channel, scopePoints)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads the number of desired values per Scope sequence.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:POIN?
        ''' </remarks>
        Public Function Scope_GetPoints(ByVal channel As Integer) As Integer
            Dim scopePoints As Integer
            Dim pInvokeResult As Integer = Native.scope_getPoints(Me._Handle, channel, scopePoints)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return scopePoints
        End Function

        ''' <summary>
        ''' In the default state (OFF), each measurement sequence from the sensor is averaged over several sequences. 
        ''' Since the measured values of a sequence may be closer to each other in time than the measurements, several measurement sequences with a slight time offset 
        ''' are also superimposed on the desired sequence. With the state turned ON - this effect can be switched off, which may increase the measurement speed. 
        ''' This ensures that the measured values of an individual sequence are immediately delivered.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="realtime">Sets the state of real-time measurement. Default value - false.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:REAL ON | OFF
        ''' </remarks>
        Public Sub Scope_SetRealtimeEnabled(ByVal channel As Integer, ByVal realtime As Boolean)
            Dim pInvokeResult As Integer = Native.scope_setRealtimeEnabled(Me._Handle, channel, System.Convert.ToUInt16(realtime))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads the state of real-time measurement setting.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:REAL?
        ''' </remarks>
        Public Function Scope_GetRealtimeEnabled(ByVal channel As Integer) As Boolean
            Dim realtimeAsUShort As UShort
            Dim pInvokeResult As Integer = Native.scope_getRealtimeEnabled(Me._Handle, channel, realtimeAsUShort)
            Dim realtime As Boolean = System.Convert.ToBoolean(realtimeAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return realtime
        End Function

        ''' <summary>
        ''' Sets the time to be covered by the Scope sequence.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="scopeTime">Sets the value of scope time. Valid Range: 0.1e-3 to 0.3 s. Default Value: 0.01 s.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:TIME
        ''' </remarks>
        Public Sub Scope_SetTime(ByVal channel As Integer, ByVal scopeTime As Double)
            Dim pInvokeResult As Integer = Native.scope_setTime(Me._Handle, channel, scopeTime)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads the value of the time to be covered by the Scope sequence.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:TIME?
        ''' </remarks>
        Public Function Scope_GetTime(ByVal channel As Integer) As Double
            Dim scopeTime As Double
            Dim pInvokeResult As Integer = Native.scope_getTime(Me._Handle, channel, scopeTime)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return scopeTime
        End Function

        ''' <summary>
        ''' This function can be used to automatically determine a value for filter bandwidth.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="autoEnabled">Activates or deactivates automatic determination of a value for filter bandwidth.
        ''' If the automatic switchover is activated with the ON parameter, the sensor always defines a suitable filter length. Default Value: On.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:AVER:COUN:AUTO ON|OFF
        ''' </remarks>
        Public Sub Scope_SetAutoEnabled(ByVal channel As Integer, ByVal autoEnabled As Boolean)
            Dim pInvokeResult As Integer = Native.scope_setAutoEnabled(Me._Handle, channel, System.Convert.ToUInt16(autoEnabled))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the setting of automatic switchover of filter bandwidth.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:AVER:COUN:AUTO?
        ''' </remarks>
        Public Function Scope_GetAutoEnabled(ByVal channel As Integer) As Boolean
            Dim autoEnabledAsUShort As UShort
            Dim pInvokeResult As Integer = Native.scope_getAutoEnabled(Me._Handle, channel, autoEnabledAsUShort)
            Dim autoEnabled As Boolean = System.Convert.ToBoolean(autoEnabledAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return autoEnabled
        End Function

        ''' <summary>
        ''' Sets an upper time limit can be set via (maximum time). It should never be exceeded. Undesired long measurement times can thus be prevented if the automatic filter length switchover is on.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="upperTimeLimit">Sets the upper time limit (maximum time to fill the filter). Valid Range: NRP-21: 0.01..999.99; FSH-Z1: 0.01..999.99. Default Value: 4.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:AVER:COUN:AUTO:MTIM
        ''' </remarks>
        Public Sub Scope_SetAutoMaxMeasuringTime(ByVal channel As Integer, ByVal upperTimeLimit As Double)
            Dim pInvokeResult As Integer = Native.scope_setAutoMaxMeasuringTime(Me._Handle, channel, upperTimeLimit)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries an upper time limit (maximum time to fill the filter).
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:AVER:COUN:AUTO:MTIM
        ''' </remarks>
        Public Function Scope_GetAutoMaxMeasuringTime(ByVal channel As Integer) As Double
            Dim upperTimeLimit As Double
            Dim pInvokeResult As Integer = Native.scope_getAutoMaxMeasuringTime(Me._Handle, channel, upperTimeLimit)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return upperTimeLimit
        End Function

        ''' <summary>
        ''' Sets the maximum noise ratio in the measurement result.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="maximumNoiseRatio">Sets the maximum noise ratio in the measurement result, dB. Valid Range: NRP-Z21: 0..1; FSH-Z1: 0..1. Default Value: 0.1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:AVER:COUN:AUTO:NSR
        ''' </remarks>
        Public Sub Scope_SetAutoNoiseSignalRatio(ByVal channel As Integer, ByVal maximumNoiseRatio As Double)
            Dim pInvokeResult As Integer = Native.scope_setAutoNoiseSignalRatio(Me._Handle, channel, maximumNoiseRatio)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the maximum noise signal ratio value.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:AVER:COUN:AUTO:NSR?
        ''' </remarks>
        Public Function Scope_GetAutoNoiseSignalRatio(ByVal channel As Integer) As Double
            Dim maximumNoiseRatio As Double
            Dim pInvokeResult As Integer = Native.scope_getAutoNoiseSignalRatio(Me._Handle, channel, maximumNoiseRatio)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return maximumNoiseRatio
        End Function

        ''' <summary>
        ''' Defines the number of significant places for linear units and the number of decimal places for logarithmic units which should be free of noise in the measurement result. 
        ''' This setting does not affect the setting of display.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="resolution">Defines the number of significant places for linear units and the number of decimal places for logarithmic units which should be free of noise in the measurement result.
        ''' Default value <see cref="Resolution.Res3"/>.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN:AUTO:RES 1 ... 4
        ''' </remarks>
        Public Sub Scope_SetAutoResolution(ByVal channel As Integer, ByVal resolution As Resolution)
            Dim pInvokeResult As Integer = Native.scope_setAutoResolution(Me._Handle, channel, resolution)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the number of significant places for linear units and the number of decimal places for logarithmic units which should be free of noise in the measurement result. 
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:AVER:COUN:AUTO:RES?
        ''' </remarks>
        Public Function Scope_GetAutoResolution(ByVal channel As Integer) As Resolution
            Dim resolution As Integer
            Dim pInvokeResult As Integer = Native.scope_getAutoResolution(Me._Handle, channel, resolution)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return CType(resolution, Resolution)
        End Function

        ''' <summary>
        ''' Selects a method by which the automatic filter length switchover can operate.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="method">Selects a method by which the automatic filter length switchover can operate. Default Value <see cref="AutoCountType.AutoCountTypeResolution"/>.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:AVER:COUN:AUTO:TYPE RES | NSR
        ''' </remarks>
        Public Sub Scope_SetAutoType(ByVal channel As Integer, ByVal method As AutoCountType)
            Dim pInvokeResult As Integer = Native.scope_setAutoType(Me._Handle, channel, method)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns a method by which the automatic filter length switchover can operate.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:AVER:COUN:AUTO:TYPE?
        ''' </remarks>
        Public Function Scope_GetAutoType(ByVal channel As Integer) As AutoCountType
            Dim method As Integer
            Dim pInvokeResult As Integer = Native.scope_getAutoType(Me._Handle, channel, method)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return CType(method, AutoCountType)
        End Function

        ''' <summary>
        ''' Activates or deactivates the automatic equivalent sampling if the resolution of the trace measurement is configured lower than a sample period.
        ''' This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="scopeEquivalentSampling">Activates or deactivates the automatic equivalent sampling if the resolution of the trace measurement is configured lower than a sample period. Default Value: On.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:ESAM:AUTO
        ''' </remarks>
        Public Sub Scope_SetEquivalentSampling(ByVal channel As Integer, ByVal scopeEquivalentSampling As Boolean)
            Dim pInvokeResult As Integer = Native.scope_setEquivalentSampling(Me._Handle, channel, System.Convert.ToUInt16(scopeEquivalentSampling))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the state of the automatic equivalent sampling if the resolution of the trace measurement is configured lower than a sample period.
        ''' This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:ESAM:AUTO?
        ''' </remarks>
        Public Function Scope_GetEquivalentSampling(ByVal channel As Integer) As Boolean
            Dim scopeEquivalentSamplingAsUShort As UShort
            Dim pInvokeResult As Integer = Native.scope_getEquivalentSampling(Me._Handle, channel, scopeEquivalentSamplingAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim scopeEquivalentSampling As Boolean = System.Convert.ToBoolean(scopeEquivalentSamplingAsUShort)
            Return scopeEquivalentSampling
        End Function

        ''' <summary>
        ''' Turns on or off the automatic pulse measurement feature. 
        ''' When traceMeasurements is set to on, the sensor tries to compute the pulse parameters for the current measured trace.
        ''' This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="traceMeasurements">Switches the automatic pulse measurement feature. Default Value: On.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:MEAS:STAT ON | OFF
        ''' SENS:TRAC:MEAS:AUTO ON | OFF
        ''' </remarks>
        Public Sub Scope_Meas_SetMeasEnabled(ByVal channel As Integer, ByVal traceMeasurements As Boolean)
            Dim pInvokeResult As Integer = Native.Scope_Meas_SetMeasEnabled(Me._Handle, channel, System.Convert.ToUInt16(traceMeasurements))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the state of the automatic pulse measurement feature.
        ''' This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:MEAS:STAT?
        ''' SENS:TRAC:MEAS:TRAN:AUTO?
        ''' </remarks>
        Public Function Scope_Meas_GetMeasEnabled(ByVal channel As Integer) As Boolean
            Dim traceMeasurementsAsUShort As UShort
            Dim pInvokeResult As Integer = Native.Scope_Meas_GetMeasEnabled(Me._Handle, channel, traceMeasurementsAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim traceMeasurements As Boolean = System.Convert.ToBoolean(traceMeasurementsAsUShort)
            Return traceMeasurements
        End Function

        ''' <summary>
        ''' Selects the algorithm for detecting the top and the base level of the pulsed signal.
        ''' This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="algorithm">Selects the algorithm for detecting the top and the base level of the pulsed signal. Default Value <see cref="ScopeMeasAlg.Histogram"/>.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:MEAS:ALG HIST | INT
        ''' </remarks>
        Public Sub Scope_Meas_SetMeasAlgorithm(ByVal channel As Integer, ByVal algorithm As ScopeMeasAlg)
            Dim pInvokeResult As Integer = Native.Scope_Meas_SetMeasAlgorithm(Me._Handle, channel, algorithm)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries selected algorithm for detecting the top and the base level of the pulsed signal.
        ''' This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:MEAS:ALG?
        ''' </remarks>
        Public Function Scope_Meas_GetMeasAlgorithm(ByVal channel As Integer) As ScopeMeasAlg
            Dim algorithm As Integer
            Dim pInvokeResult As Integer = Native.Scope_Meas_GetMeasAlgorithm(Me._Handle, channel, algorithm)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return CType(algorithm, ScopeMeasAlg)
        End Function

        ''' <summary>
        ''' Defines the thresholds of the reference and transition levels that are used for the calculation  of the pulse's time parameter.
        ''' The duration reference level is used to calculate pulse duration and pulse period, the transition low and high levels are used to calculate 
        ''' the pulse transition?s rise/fall time and their occurrences.
        ''' This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:MEAS:DEF:DUR:REF 
        ''' SENS:TRAC:MEAS:DEF:TRAN:LREF
        ''' SENS:TRAC:MEAS:DEF:TRAN:HREF
        ''' </remarks>
        Public Sub Scope_Meas_SetLevelThresholds(ByVal channel As Integer, ByVal thresholds As LevelThresholds)
            Dim pInvokeResult As Integer = Native.Scope_Meas_SetLevelThresholds(Me._Handle, channel, thresholds.DurationRef, thresholds.TransitionLowRef, thresholds.TransitionHighRef)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the thresholds of the reference and transition levels that are used for the calculation of the pulse's time parameter.
        ''' This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:MEAS:DEF:DUR:REF? 
        ''' SENS:TRAC:MEAS:DEF:TRAN:LREF?
        ''' SENS:TRAC:MEAS:DEF:TRAN:HREF?
        ''' </remarks>
        Public Function Scope_Meas_GetLevelThresholds(ByVal channel As Integer) As LevelThresholds
            Dim thresholds As New LevelThresholds()
            Dim pInvokeResult As Integer = Native.Scope_Meas_GetLevelThresholds(Me._Handle, channel, thresholds.DurationRef, thresholds.TransitionLowRef, thresholds.TransitionHighRef)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return thresholds
        End Function

        ''' <summary>
        ''' Defines measurement time which sets the duration of analysing the current trace for the pulse parameters. 
        ''' The measurement time could be used together with the measurement offset time to select the second (or any other) pulse in the trace and not the whole trace.
        ''' This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="measTime">
        ''' The measurement time is used to set the duration of analysing the current trace for the pulse parameters. 
        ''' The measurement time could be used together with the measurement offset time to select the second (or any other) pulse in the trace and not the whole trace.
        ''' To disable this "time gate" set the measurement time to 0. Default Value: 0 s.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:TRACe:MEAS:TIME
        ''' </remarks>
        Public Sub Scope_Meas_SetTime(ByVal channel As Integer, ByVal measTime As Double)
            Dim pInvokeResult As Integer = Native.Scope_Meas_SetTime(Me._Handle, channel, measTime)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the measurement time. This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:TRACe:MEAS:TIME?
        ''' </remarks>
        Public Function Scope_Meas_GetTime(ByVal channel As Integer) As Double
            Dim measTime As Double
            Dim pInvokeResult As Integer = Native.Scope_Meas_GetTime(Me._Handle, channel, measTime)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return measTime
        End Function

        ''' <summary>
        ''' Defines offset time used to set the start point of analysing the current trace for the pulse parameters. 
        ''' The offset time could be used to start analysis from the second (or any other) pulse occurrence in the trace and not from the beginning of the trace.
        ''' This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="offsetTime">Defines offset time used to set the start point of analysing the current trace for the pulse parameters. 
        ''' The offset time could be used to start analysis from the second (or any other) pulse occurrence in the trace and not from the beginning of the trace.
        ''' Valid Range: 0..0.99 s. Default Value: 0 s.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:TRACe:MEAS:OFFS:TIME
        ''' </remarks>
        Public Sub Scope_Meas_SetOffsetTime(ByVal channel As Integer, ByVal offsetTime As Double)
            Dim pInvokeResult As Integer = Native.Scope_Meas_SetOffsetTime(Me._Handle, channel, offsetTime)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries offset time used to set the start point of analysing the current trace for the pulse parameters.
        ''' This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:TRACe:MEAS:OFFS:TIME?
        ''' </remarks>
        Public Function Scope_Meas_GetOffsetTime(ByVal channel As Integer) As Double
            Dim offsetTime As Double
            Dim pInvokeResult As Integer = Native.Scope_Meas_GetOffsetTime(Me._Handle, channel, offsetTime)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return offsetTime
        End Function

        ''' <summary>
        ''' Returns the calculated pulse time parameters of the last recorded trace. 
        ''' If a parameter could not be calculated the returned value is NAN. 
        ''' The Sensor takes the time values when the trace crosses the reference level points for duration and period calculation.
        ''' This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:MEAS:PULS:DCYC?
        ''' SENS:TRAC:MEAS:PULS:DUR?
        ''' SENS:TRAC:MEAS:PULS:PER?
        ''' </remarks>
        Public Function Scope_Meas_GetPulseTimes(ByVal channel As Integer) As PulseTimes
            Dim pulseTimes As New PulseTimes()
            Dim pInvokeResult As Integer = Native.Scope_Meas_GetPulseTimes(Me._Handle, channel, pulseTimes.DutyCycle, pulseTimes.PulseDuration, pulseTimes.PulsePeriod)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return pulseTimes
        End Function

        ''' <summary>
        ''' Returns the transition parameters of the last examined trace data.
        ''' The NRP Sensor always searches for the first rising edge and the first falling edge in the trace. 
        ''' If offset time is set greater than zero the algorithm searches the edges from this time in the trace.
        ''' This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="slope">Selects measured transition. Default value <see cref="Slope.SlopePositive"/>.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:MEAS:TRAN:POS:DUR?
        ''' SENS:TRAC:MEAS:TRAN:POS:OCC?
        ''' SENS:TRAC:MEAS:TRAN:POS:OVER?
        ''' SENS:TRAC:MEAS:TRAN:NEG:DUR?
        ''' SENS:TRAC:MEAS:TRAN:NEG:OCC?
        ''' SENS:TRAC:MEAS:TRAN:NEG:OVER?
        ''' </remarks>
        Public Function Scope_Meas_GetPulseTransition(ByVal channel As Integer, ByVal slope As Slope) As PulseTransition
            Dim pulseTransition As New PulseTransition()
            Dim pInvokeResult As Integer = Native.Scope_Meas_GetPulseTransition(Me._Handle, channel, slope, pulseTransition.Duration, pulseTransition.Occurence, pulseTransition.Overshoot)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return pulseTransition
        End Function

        ''' <summary>
        ''' Returns the average power, the minimum level and the maximum level of the analysed trace in Watt. This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:MEAS:POW:AVG?
        ''' SENS:TRAC:MEAS:POW:MIN?
        ''' SENS:TRAC:MEAS:POW:MAX?
        ''' </remarks>
        Public Function Scope_Meas_GetPulsePower(ByVal channel As Integer) As PulsePower
            Dim pp As New PulsePower()
            Dim pInvokeResult As Integer = Native.Scope_Meas_GetPulsePower(Me._Handle, channel, pp.Average, pp.MinPeak, pp.MaxPeak)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return pp
        End Function

        ''' <summary>
        ''' Returns the pulse top level and the pulse base level. 
        ''' Both levels are calculated with the algorithm that was set with the rsnrpz_scope_meas_setMeasAlgorithm(...) function.
        ''' This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:MEAS:POW:PULS:TOP?
        ''' SENS:TRAC:MEAS:POW:PULS:BASE?
        ''' </remarks>
        Public Function Scope_Meas_GetPulseLevels(ByVal channel As Integer) As PulseLevels
            Dim pl As New PulseLevels()
            Dim pInvokeResult As Integer = Native.Scope_Meas_GetPulseLevels(Me._Handle, channel, pl.TopLevel, pl.BaseLevel)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return pl
        End Function

        ''' <summary>
        ''' Returns the calculated reference level in Watt at the defined thresholds of the last recorded trace. 
        ''' The thresholds in percent are defined with the function  rsnrpz_scope_meas_setLevelThresholds(?) of the pulse amplitude.
        ''' This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:MEAS:POW:LREF?
        ''' SENS:TRAC:MEAS:POW:HREF?
        ''' SENS:TRAC:MEAS:POW:REF?
        ''' </remarks>
        Public Function Scope_Meas_GetPulseReferenceLevels(ByVal channel As Integer) As PulseReferenceLevels
            Dim prl As New PulseReferenceLevels()
            Dim pInvokeResult As Integer = Native.Scope_Meas_GetPulseReferenceLevels(Me._Handle, channel, prl.LowRefLevel, prl.HighRefLevel, prl.DurationRefLevel)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return prl
        End Function

        ''' <summary>
        ''' Activates or deactivates the automatic equivalent sampling mode during automatic pulse analysis.
        ''' If equivalent sampling is enabled, the sensor tries to measure the time parameters (mainly rise/fall times) of the pulse with a high resolution by doing equivalent sampling.
        ''' To do the equivalent sampling a periodic signal is mandatory. The sensor decides automatically if equivalent sampling is possible. 
        ''' To get the resolution of the measured time parameter the function "rsnrpz_scope_meas_getSamplePeriod" could be used.
        ''' This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="scopeMeasEquivSampling">Activates or deactivates the automatic equivalent sampling mode during automatic pulse analysis. Default Value: On.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:MEAS:TRANS:ESAM:AUTO
        ''' </remarks>
        Public Sub Scope_Meas_SetEquivalentSampling(ByVal channel As Integer, ByVal scopeMeasEquivSampling As Boolean)
            Dim pInvokeResult As Integer = Native.Scope_Meas_SetEquivalentSampling(Me._Handle, channel, System.Convert.ToUInt16(scopeMeasEquivSampling))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the state of the automatic equivalent sampling mode during automatic pulse analysis. This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:MEAS:TRANS:ESAM:AUTO?
        ''' </remarks>
        Public Function Scope_Meas_GetEquivalentSampling(ByVal channel As Integer) As Boolean
            Dim scopeMeasEquivSamplingAsUShort As UShort
            Dim pInvokeResult As Integer = Native.Scope_Meas_GetEquivalentSampling(Me._Handle, channel, scopeMeasEquivSamplingAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim scopeMeasEquivSampling As Boolean = System.Convert.ToBoolean(scopeMeasEquivSamplingAsUShort)
            Return scopeMeasEquivSampling
        End Function

        ''' <summary>
        ''' Returns the sample period (in seconds) of the last pulse analysis. 
        ''' This parameter is a good indicator if the equivalent sampling mode of measuring the rise and fall times was successful or not.
        ''' This function is only available for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:TRAC:MEAS:TRANS:SPER?
        ''' </remarks>
        Public Function Scope_Meas_GetSamplePeriod(ByVal channel As Integer) As Double
            Dim samplePeriod As Double
            Dim pInvokeResult As Integer = Native.Scope_Meas_GetSamplePeriod(Me._Handle, channel, samplePeriod)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return samplePeriod
        End Function

#End Region '/SCOPE

#Region "TRIGGER"

        ''' <summary>
        ''' Configures the parameters of internal trigger system.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="triggerLevel">Determines the power (in W) a trigger signal must exceed before a trigger event is detected. Valid Range: 0.1e-6..0.2 W. Default Value: 1e-6 W.</param>
        ''' <param name="triggerSlope">Determines whether the rising (POSitive) or the falling (NEGative) edge of the signal is used for triggering. Default Value <see cref="Slope.SlopePositive"/>.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:DEL:AUTO ON
        ''' TRIG:ATR OFF
        ''' TRIG:COUN 1
        ''' TRIG:DEL 0.0
        ''' TRIG:HOLD 0.0
        ''' TRIG:HYST 3DB
        ''' TRIG:LEV &lt;value&gt;
        ''' TRIG:SLOP POS | NEG
        ''' TRIG:SOUR INT
        ''' </remarks>
        Public Sub Trigger_ConfigureInternal(ByVal channel As Integer, ByVal triggerLevel As Double, ByVal triggerSlope As Slope)
            Dim pInvokeResult As Integer = Native.Trigger_ConfigureInternal(Me._Handle, channel, triggerLevel, triggerSlope)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Configures the parameters of external trigger system.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="triggerDelay">Sets a the delay (in seconds) between the trigger event and the beginning of the actual measurement (integration). Valid Range: -5e-3..100 s. Default Value: 0 s.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:DEL &lt;value&gt;
        ''' TRIG:SOUR EXT
        ''' TRIG:COUN 1
        ''' TRIG:ATR OFF
        ''' TRIG:HOLD 0.0
        ''' TRIG:DEL:AUTO ON
        ''' </remarks>
        Public Sub Trigger_ConfigureExternal(ByVal channel As Integer, ByVal triggerDelay As Double)
            Dim pInvokeResult As Integer = Native.Trigger_ConfigureExternal(Me._Handle, channel, triggerDelay)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Performs triggering and ensures that the sensor directly changes from the WAIT_FOR_TRG state to the MEASURING state irrespective of the selected trigger source. 
        ''' A trigger delay set with TRIG:DEL is ignored but not the automatic delay determined when TRIG:DEL:AUTO:ON is set.
        ''' When the trigger source is HOLD, a measurement can only be started with TRIG.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:IMM
        ''' </remarks>
        Public Sub Trigger_Immediate(ByVal channel As Integer)
            Dim pInvokeResult As Integer = Native.Trigger_Immediate(Me._Handle, channel)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Ensures (if parameter is set to On) by means of an automatically determined delay that a measurement is started only after the sensor has settled. 
        ''' This is important when thermal sensors are used.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="autoDelay">Enables or disables automatic determination of delay. Default Value: Off.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:DEL:AUTO ON | OFF
        ''' </remarks>
        Public Sub Trigger_SetAutoDelayEnabled(ByVal channel As Integer, ByVal autoDelay As Boolean)
            Dim pInvokeResult As Integer = Native.Trigger_SetAutoDelayEnabled(Me._Handle, channel, System.Convert.ToUInt16(autoDelay))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads the setting of auto delay feature.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:DEL:AUTO?
        ''' </remarks>
        Public Function Trigger_GetAutoDelayEnabled(ByVal channel As Integer) As Boolean
            Dim autoDelayAsUShort As UShort
            Dim pInvokeResult As Integer = Native.Trigger_GetAutoDelayEnabled(Me._Handle, channel, autoDelayAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim autoDelay As Boolean = System.Convert.ToBoolean(autoDelayAsUShort)
            Return autoDelay
        End Function

        ''' <summary>
        ''' Turns on or off the auto trigger feature. When auto trigger is set to On, the WAIT_FOR_TRG state is automatically exited 
        ''' when no trigger event occurs within a period that corresponds to the reciprocal of the display update rate.
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="autoTrigger">Enables or disables the Auto Trigger. Default Value: Off.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:ATR:STAT ON | OFF
        ''' </remarks>
        Public Sub Trigger_SetAutoTriggerEnabled(ByVal channel As Integer, ByVal autoTrigger As Boolean)
            Dim pInvokeResult As Integer = Native.Trigger_SetAutoTriggerEnabled(Me._Handle, channel, System.Convert.ToUInt16(autoTrigger))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads the state of Auto Trigger. This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:ATR:STAT?
        ''' </remarks>
        Public Function Trigger_GetAutoTriggerEnabled(ByVal channel As Integer) As Boolean
            Dim autoTriggerAsUShort As UShort
            Dim pInvokeResult As Integer = Native.Trigger_GetAutoTriggerEnabled(Me._Handle, channel, autoTriggerAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim autoTrigger = System.Convert.ToBoolean(autoTriggerAsUShort)
            Return autoTrigger
        End Function

        ''' <summary>
        ''' Sets the number of measurement cycles to be performed when the measurement is started with INIT.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="triggerCount">Sets the number of measurement cycles to be  performed when the measurement is started with INIT. Valid Range: 1..0x7FFF_FFFE. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:COUN
        ''' </remarks>
        Public Sub Trigger_SetCount(ByVal channel As Integer, ByVal triggerCount As Integer)
            Dim pInvokeResult As Integer = Native.Trigger_SetCount(Me._Handle, channel, triggerCount)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads the number of measurement cycles to be performed when the measurement is started with INIT.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:COUN?
        ''' </remarks>
        Public Function Trigger_GetCount(ByVal channel As Integer) As Integer
            Dim triggerCount As Integer
            Dim pInvokeResult As Integer = Native.Trigger_GetCount(Me._Handle, channel, triggerCount)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return triggerCount
        End Function

        ''' <summary>
        ''' Defines the delay between the trigger event and the beginning of the actual measurement (integration).
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="triggerDelay">Sets a the delay (in seconds) between the trigger event and the beginning of the actual measurement (integration).
        ''' Valid Range: NRP-Z21: -5.0e-3..1000 s; NRP-Z51: 0..100 s; FSH-Z1: -5e-3..100 s. Default Value: 0 s.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:DEL
        ''' </remarks>
        Public Sub Trigger_SetDelay(ByVal channel As Integer, ByVal triggerDelay As Double)
            Dim pInvokeResult As Integer = Native.Trigger_SetDelay(Me._Handle, channel, triggerDelay)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads value of the delay (in seconds) between the trigger event and the beginning of the actual measurement (integration).
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:DEL?
        ''' </remarks>
        Public Function Trigger_GetDelay(ByVal channel As Integer) As Double
            Dim triggerDelay As Double
            Dim pInvokeResult As Integer = Native.Trigger_GetDelay(Me._Handle, channel, triggerDelay)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return triggerDelay
        End Function

        ''' <summary>
        ''' Defines a period after a trigger event within which all further trigger events are ignored.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="triggerHoldoff">Defines a period (in seconds) after a trigger event within which all further trigger events are ignored. Valid Range: 0..-10 s. Default Value: 0 s.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:HOLD
        ''' </remarks>
        Public Sub Trigger_SetHoldoff(ByVal channel As Integer, ByVal triggerHoldoff As Double)
            Dim pInvokeResult As Integer = Native.Trigger_SetHoldoff(Me._Handle, channel, triggerHoldoff)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the value of a period after a trigger event within which all further trigger events are ignored.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIGger[1..4]:HOLDoff?
        ''' </remarks>
        Public Function Trigger_GetHoldoff(ByVal channel As Integer) As Double
            Dim triggerHoldoff As Double
            Dim pInvokeResult As Integer = Native.Trigger_GetHoldoff(Me._Handle, channel, triggerHoldoff)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return triggerHoldoff
        End Function

        ''' <summary>
        ''' This function is used to specify how far the signal level has to drop below the trigger level before a new signal edge can be detected as a trigger event. 
        ''' Thus, this command can be used to eliminate the effects of noise in the signal on the transition filters of the trigger system.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="triggerHysteresis">Defines the trigger hysteresis in dB. Valid Range: 0..10 dB. Default Value: 0 dB.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:HYST
        ''' </remarks>
        Public Sub Trigger_SetHysteresis(ByVal channel As Integer, ByVal triggerHysteresis As Double)
            Dim pInvokeResult As Integer = Native.Trigger_SetHysteresis(Me._Handle, channel, triggerHysteresis)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads the value of trigger hysteresis.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:HYST?
        ''' </remarks>
        Public Function Trigger_GetHysteresis(ByVal channel As Integer) As Double
            Dim triggerHysteresis As Double
            Dim pInvokeResult As Integer = Native.Trigger_GetHysteresis(Me._Handle, channel, triggerHysteresis)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return triggerHysteresis
        End Function

        ''' <summary>
        ''' Determines the power a trigger signal must exceed before a trigger event is detected. This setting is only used for internal trigger signal source.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="triggerLevel">Determines the power (in W) a trigger signal must exceed before a trigger event is detected.
        ''' Valid Range: NRP-Z21: 0.1e-6..0.2 W; NRP-Z51: 0.25e-6..0.1 W; FSH-Z1: 0.1e-6..0.2 W. Default Value: 1.0e-6 W.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:LEV
        ''' </remarks>
        Public Sub Trigger_SetLevel(ByVal channel As Integer, ByVal triggerLevel As Double)
            Dim pInvokeResult As Integer = Native.Trigger_SetLevel(Me._Handle, channel, triggerLevel)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads the power a trigger signal must exceed before a trigger event is detected.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:LEV?
        ''' </remarks>
        Public Function Trigger_GetLevel(ByVal channel As Integer) As Double
            Dim triggerLevel As Double
            Dim pInvokeResult As Integer = Native.Trigger_GetLevel(Me._Handle, channel, triggerLevel)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return triggerLevel
        End Function

        ''' <summary>
        ''' Determines whether the rising (POSitive) or the falling (NEGative) edge of the signal is used for triggering.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="triggerSlope">Determines whether the rising (POSitive) or the falling (NEGative) edge of the signal is used for triggering. Default Value: <see cref="Slope.SlopePositive"/>.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:SLOP POSitive | NEGative
        ''' </remarks>
        Public Sub Trigger_SetSlope(ByVal channel As Integer, ByVal triggerSlope As Slope)
            Dim pInvokeResult As Integer = Native.Trigger_SetSlope(Me._Handle, channel, triggerSlope)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads whether the rising (POSitive) or the falling (NEGative) edge of the signal is used for triggering.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:SLOP?
        ''' </remarks>
        Public Function Trigger_GetSlope(ByVal channel As Integer) As Slope
            Dim triggerSlope As Integer
            Dim pInvokeResult As Integer = Native.Trigger_GetSlope(Me._Handle, channel, triggerSlope)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return CType(triggerSlope, Slope)
        End Function

        ''' <summary>
        ''' Sets the trigger signal source for the WAIT_FOR_TRG state.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="triggerSource">Selects the trigger signal source for the WAIT_FOR_TRG state. Default Value: <see cref="TriggerSource.Immediate"/>.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:SOUR BUS | EXT | HOLD | IMM | INT
        ''' </remarks>
        Public Sub Trigger_SetSource(ByVal channel As Integer, ByVal triggerSource As TriggerSource)
            Dim pInvokeResult As Integer = Native.Trigger_SetSource(Me._Handle, channel, triggerSource)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Gets the trigger signal source for the WAIT_FOR_TRG state.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:SOUR?
        ''' </remarks>
        Public Function Trigger_GetSource(ByVal channel As Integer) As TriggerSource
            Dim triggerSource As Integer
            Dim pInvokeResult As Integer = Native.Trigger_GetSource(Me._Handle, channel, triggerSource)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return CType(triggerSource, TriggerSource)
        End Function

        ''' <summary>
        ''' Defines the dropout time value. With a positive (negative) trigger slope, the dropout time is the minimum time 
        ''' for which the signal must be below (above) the power level defined by rsnrpz_trigger_setLevel and rsnrpz_trigger_setHysteresis before triggering can occur again. 
        ''' As with the Holdoff parameter, unwanted trigger events can be excluded. The set dropout time only affects the internal trigger source.
        ''' The dropout time parameter is useful when dealing with, for example, GSM signals with several active slots.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="dropoutTime">Defines the dropout time value. Valid Range: 0..10 s. Default Value: 0 s.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIGger:DTIMe
        ''' </remarks>
        Public Sub Trigger_SetDropoutTime(ByVal channel As Integer, ByVal dropoutTime As Double)
            Dim pInvokeResult As Integer = Native.Trigger_SetDropoutTime(Me._Handle, channel, dropoutTime)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries the dropout time value.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIGger:DTIMe?
        ''' </remarks>
        Public Function Trigger_GetDropoutTime(ByVal channel As Integer) As Double
            Dim dropoutTime As Double
            Dim pInvokeResult As Integer = Native.Trigger_GetDropoutTime(Me._Handle, channel, dropoutTime)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return dropoutTime
        End Function

        ''' <summary>
        ''' This function can be used to configure an R&amp;S NRP-Z81 power sensor as the trigger master, enabling it to output a digital trigger signal in sync with its own trigger event. 
        ''' This makes it possible to synchronize several sensors (see rsnrpz_trigger_setSyncState) and to perform measurements in sync with a signal at very low power, 
        ''' which normally would not allow signal triggering. The trigger signal which is output has a length of 1Rs and the positive slope coincides with the physical trigger point. 
        ''' At present, it can be distributed to other R&amp;S NRP-Zxx sensors only via the R&amp;S NRP base unit and not via the R&amp;S NRP-Z3/-Z4 interface adapter.
        ''' Generally, the trigger master is set to internal triggering (signal triggering) (the BUS and IMMEDIATE settings can also be used); the sensors acting as trigger slaves
        ''' must be set to external triggering and positive trigger slope.
        ''' With the R&amp;S NRP-Z81 power sensor, digital trigger signals are sent and received via a single differential line pair, the trigger bus. 
        ''' Only one instrument on the trigger bus can act as the trigger master. If the application is time-critical, the trigger-signal delay from the master to a slave 
        ''' must be taken into account.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="state">Enables or disables trigger master. Default Value: Off.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:MAST:STAT ON | OFF
        ''' </remarks>
        Public Sub Trigger_SetMasterState(ByVal channel As Integer, ByVal state As Boolean)
            Dim pInvokeResult As Integer = Native.Trigger_SetMasterState(Me._Handle, channel, System.Convert.ToUInt16(state))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries state of trigger master.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:MAST:STAT?
        ''' </remarks>
        Public Function Trigger_GetMasterState(ByVal channel As Integer) As Boolean
            Dim StateAsUShort As UShort
            Dim pInvokeResult As Integer = Native.Trigger_GetMasterState(Me._Handle, channel, StateAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim state As Boolean = System.Convert.ToBoolean(StateAsUShort)
            Return state
        End Function

        ''' <summary>
        ''' This function can be used to synchronize the sensors connected to the trigger bus. Synchronization is achieved by enabling the
        ''' trigger signal only when all the sensors are in the WAIT_FOR_TRIGGER state (wired-OR). 
        ''' This ensures that the measurements are started simultaneously and also that repetitions due to averaging start at the same time. 
        ''' It must be ensured that the number of repetitions is the same for all the sensors involved in the measurement. 
        ''' Otherwise, the trigger bus will be blocked by any sensor that has completed its measurements before the others and has returned to the IDLE state.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="state">Enables or disables sensor synchronization. Default Value: Off.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:SYNC:STAT ON | OFF
        ''' </remarks>
        Public Sub Trigger_SetSyncState(ByVal channel As Integer, ByVal state As Boolean)
            Dim pInvokeResult As Integer = Native.Trigger_SetSyncState(Me._Handle, channel, System.Convert.ToUInt16(state))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries state of sensor synchronization.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:SYNC:STAT?
        ''' </remarks>
        Public Function Trigger_GetSyncState(ByVal channel As Integer) As Boolean
            Dim stateAsUShort As UShort
            Dim pInvokeResult As Integer = Native.Trigger_GetSyncState(Me._Handle, channel, stateAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim state As Boolean = System.Convert.ToBoolean(stateAsUShort)
            Return state
        End Function

        ''' <summary>
        ''' Sets the master port.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="port">Sets the master port. Default Value:<see cref="PortExt.PortExt1"/>.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:MAST:PORT
        ''' </remarks>
        Public Sub Trigger_SetMasterPort(ByVal channel As Integer, ByVal port As PortExt)
            Dim pInvokeResult As Integer = Native.Trigger_SetMasterPort(Me._Handle, channel, port)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the master port.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:MAST:PORT
        ''' </remarks>
        Public Function Trigger_GetMasterPort(ByVal channel As Integer) As PortExt
            Dim port As UInteger
            Dim pInvokeResult As Integer = Native.Trigger_GetMasterPort(Me._Handle, channel, port)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return CType(port, PortExt)
        End Function

        ''' <summary>
        ''' Sets the sync port.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="port">Sets the sync port. Default Value: <see cref="PortExt.PortExt1"/>.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:SYNC:PORT
        ''' </remarks>
        Public Sub Trigger_SetSyncPort(ByVal channel As Integer, ByVal port As PortExt)
            Dim pInvokeResult As Integer = Native.Trigger_SetSyncPort(Me._Handle, channel, port)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the sync port.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TRIG:SYNC:PORT
        ''' </remarks>
        Public Function Trigger_GetSyncPort(ByVal channel As Integer) As PortExt
            Dim port As UInteger
            Dim pInvokeResult As Integer = Native.Trigger_GetSyncPort(Me._Handle, channel, port)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return CType(port, PortExt)
        End Function

#End Region '/TRIGGER

#Region "CHAN"

        ''' <summary>
        ''' Returns selected information on a sensor.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="infoType">
        ''' Specifies which info should be retrieved from the sensor.
        ''' Valid Values from <see cref="InfoTypes"/>: "Manufacturer", "Type", "Stock Number", "Serial", "HWVersion", "HWVariant", "SW Build", "Technology", "Function", "MinPower", "MaxPower", 
        ''' "MinFreq", "MaxFreq", "Resolution", "Impedance", "Coupling", "Cal. Abs.", "Cal. Refl.", "Cal. S-Para.", "Cal. Misc.", "Cal. Temp.", "Cal. Lin.", "SPD Mnemonic".
        ''' Default Value: "".
        ''' </param>
        ''' <param name="arraySize">Defines the size of array 'Info'. Default Value: 100.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SYST:INFO? &lt;Info Type&gt;
        ''' </remarks>
        Public Function Chan_Info(ByVal channel As Integer, ByVal infoType As String, ByVal arraySize As Integer) As Text.StringBuilder
            Dim info As New System.Text.StringBuilder
            Dim pInvokeResult As Integer = Native.Chan_Info(Me._Handle, channel, infoType, arraySize, info)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return info
        End Function

        ''' <summary>
        ''' Returns specified parameter header which can be retrieved from selected sensor.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="parameterNumber">Defines the position of parameter header to be retrieved. Valid Range: 0 to (count of headers - 1). Default Value: 0.
        ''' Notes: Only Minimum value of the parameter is checked. Maximum value depends on sensor used and can be retrieved by function rsnrpz_chan_infosCount().
        ''' </param>
        ''' <param name="arraySize">Defines the size of array 'Header'. Default Value: 100.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SYST:INFO?
        ''' </remarks>
        Public Function Chan_InfoHeader(ByVal channel As Integer, ByVal parameterNumber As Integer, ByVal arraySize As Integer) As Text.StringBuilder
            Dim header As New System.Text.StringBuilder()
            Dim pInvokeResult As Integer = Native.Chan_InfoHeader(Me._Handle, channel, parameterNumber, arraySize, header)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return header
        End Function

        ''' <summary>
        ''' Returns the number of info headers for selected channel.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SYST:INFO?
        ''' </remarks>
        Public Function Chan_InfosCount(ByVal channel As Integer) As Integer
            Dim count As Integer
            Dim pInvokeResult As Integer = Native.Chan_InfosCount(Me._Handle, channel, count)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return count
        End Function

        ''' <summary>
        ''' This function immediately sets selected sensor to the IDLE state. Measurements in progress are interrupted. 
        ''' If INIT:CONT ON is set, a new measurement is immediately started since the trigger system is not influenced.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' ABOR
        ''' </remarks>
        Public Sub Chan_Abort(ByVal channel As Integer)
            Dim pInvokeResult As Integer = Native.Chan_Abort(Me._Handle, channel)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' This function starts a single-shot measurement on selected channel. The respective sensor goes to the INITIATED state. 
        ''' The command is completely executed when the sensor returns to the IDLE state. 
        ''' The command is ignored when the sensor is not in the IDLE state or when continuous measurements are selected (INIT:CONT ON). 
        ''' The command is only fully executed when the measurement is completed and the trigger system has again reached the IDLE state. 
        ''' INIT is the only remote control command that permits overlapping execution. Other commands can be received and processed while the command is being executed.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' STAT:OPER:MEAS?
        ''' INITiate[1..4]
        ''' </remarks>
        Public Sub Chan_Initiate(ByVal channel As Integer)
            Dim pInvokeResult As Integer = Native.Chan_Initiate(Me._Handle, channel)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Selects either single-shot or continuous (free-running) measurement cycles.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="continuousInitiate">Enables or disables the continuous measurement mode. Default Value: Off.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' INIT:CONT ON | OFF
        ''' </remarks>
        Public Sub Chan_SetInitContinuousEnabled(ByVal channel As Integer, ByVal continuousInitiate As Boolean)
            Dim pInvokeResult As Integer = Native.Chan_SetInitContinuousEnabled(Me._Handle, channel, System.Convert.ToUInt16(continuousInitiate))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns whether single-shot or continuous (free-running) measurement is selected.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' INIT:CONT?
        ''' </remarks>
        Public Function Chan_GetInitContinuousEnabled(ByVal channel As Integer) As Boolean
            Dim continuousInitiateAsUShort As UShort
            Dim pInvokeResult As Integer = Native.Chan_GetInitContinuousEnabled(Me._Handle, channel, continuousInitiateAsUShort)
            Dim continuousInitiate As Boolean = System.Convert.ToBoolean(continuousInitiateAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return continuousInitiate
        End Function

        ''' <summary>
        ''' From the point of view of the R&amp;S NRP basic unit, the sensors are stand-alone measuring devices. 
        ''' They communicate with the R&amp;S NRP via a command set complying with SCPI.
        ''' This function prompts the basic unit to send an *RST to the respective sensor. Measurements in progress are interrupted.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SYSTem:SENSor[1..4]:RESet
        ''' </remarks>
        Public Sub Chan_Reset(ByVal channel As Integer)
            Dim pInvokeResult As Integer = Native.Chan_Reset(Me._Handle, channel)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' If the signal to be measured has modulation sections just above the video bandwidth of the sensor used, measurement errors might be caused due to aliasing effects. 
        ''' In this case, the sampling rate of the sensor can be set to a safe lower value (Sampling Frequency 2). 
        ''' However, the measurement time required to obtain noise-free results is extended compared to the normal sampling rate (Sampling Frequency 1).
        ''' This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="samplingFrequency">Selects the sampling frequency. Default value <see cref="SamplingFrequency.SamplingFrequency1"/>.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:SAMP FREQ1 | FREQ2
        ''' </remarks>
        Public Sub Chan_SetSamplingFrequency(ByVal channel As Integer, ByVal samplingFrequency As SamplingFrequency)
            Dim pInvokeResult As Integer = Native.Chan_SetSamplingFrequency(Me._Handle, channel, samplingFrequency)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the selected sampling frequency. This function is NOT available for NRP-Z51.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:SAMP?
        ''' </remarks>
        Public Function Chan_GetSamplingFrequency(ByVal channel As Integer) As SamplingFrequency
            Dim samplingFrequency As Integer
            Dim pInvokeResult As Integer = Native.Chan_GetSamplingFrequency(Me._Handle, channel, samplingFrequency)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return CType(samplingFrequency, SamplingFrequency)
        End Function

        ''' <summary>
        ''' This function starts zeroing of the selected sensor using the signal at the sensor input. Zeroing is an asynchronous operation which will require a couple of seconds. 
        ''' Therefore, after starting the function, the user should poll the current execution status by continuously calling rsnrpz_chan_isZeroComplete(). 
        ''' As soon as the zeroing has finished, the result of the operation can be queried by a call to rsnrpz_error_query(). See the example code below.
        ''' Note: The sensor must be disconnected from all power sources. If the signal at the input considerably deviates from 0 W, the sensor aborts the zero calibration and raises an error condition. 
        ''' The rsnrpz driver queues the error for later retrieval by the rsnrpz_error_query() function.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <example>
        ''' bool Zero( ViSession lSesID )
        ''' {
        '''   const int CH1 = 1;
        '''   ViStatus lStat = VI_SUCCESS;
        '''   ViBoolean bZeroComplete = VI_FALSE;
        '''   ViInt32 iErrorCode = VI_SUCCESS;
        '''   ViChar szErrorMsg[256];
        '''   /* Start zeroing the sensor */
        '''   lStat = rsnrpz_chan_zero( lSesID, CH1 );
        '''   if ( lStat != VI_SUCCESS )
        '''   {
        '''     fprintf( stderr, "Error 0x%08x in rsnrpz_chan_zero()", lStat );
        '''     return false;
        '''   }
        '''   while ( bZeroComplete == VI_FALSE )
        '''   {
        '''     lStat = rsnrpz_chan_isZeroComplete( lSesID, CH1, &amp;bZeroComplete );
        '''     if ( bZeroComplete )
        '''     {
        '''       rsnrpz_error_query( lSesID, &amp;iErrorCode, szErrorMsg );
        '''       fprintf( stderr, "Zero-Cal.: error %d, %s\n\n", iErrorCode, szErrorMsg );
        '''       break;
        '''     }
        '''     else 
        '''       SLEEP( 200 );
        '''   }
        '''   return iErrorCode == 0;
        ''' }
        ''' </example>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' STAT:OPER:MEAS?
        ''' CAL:ZERO:AUTO ONCE
        ''' </remarks>
        Public Sub Chan_Zero(ByVal channel As Integer)
            Dim pInvokeResult As Integer = Native.Chan_Zero(Me._Handle, channel)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Performs zeroing using the signal at the sensor input. 
        ''' The sensor must be disconnected from all power sources. 
        ''' If the signal at the input considerably deviates from 0 W, an error message is issued and the function is aborted.
        ''' This function is valid only for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="zeroing">Selects type of advanced zeroing. Default Value: <see cref="Zeroing.ZeroLfr"/>.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' CAL:ZERO:AUTO LFR | UFR
        ''' </remarks>
        Public Sub Chan_ZeroAdvanced(ByVal channel As Integer, ByVal zeroing As Zeroing)
            Dim pInvokeResult As Integer = Native.Chan_ZeroAdvanced(Me._Handle, channel, zeroing)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' This function should be used for polling whether a previously started zero calibration has already finished. 
        ''' Zero calibration is an asynchronous operation and may take some seconds until it completes. 
        ''' See the example code under rsnrpz_chan_zero() on how to conduct a sensor zeroing calibration.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        Public Function Chan_IsZeroComplete(ByVal channel As Integer) As Boolean
            Dim zeroingCompleteAsUShort As UShort
            Dim pInvokeResult As Integer = Native.Chan_IsZeroComplete(Me._Handle, channel, zeroingCompleteAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim zeroingComplete As Boolean = System.Convert.ToBoolean(zeroingCompleteAsUShort)
            Return zeroingComplete
        End Function

        ''' <summary>
        ''' Returns the state of the measurement.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        Public Function Chan_IsMeasurementComplete(ByVal channel As Integer) As Boolean
            Dim measurementCompleteAsUShort As UShort
            Dim pInvokeResult As Integer = Native.Chan_IsMeasurementComplete(Me._Handle, channel, measurementCompleteAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim measurementComplmte As Boolean = System.Convert.ToBoolean(measurementCompleteAsUShort)
            Return measurementComplmte
        End Function

        ''' <summary>
        ''' Performs a sensor test and returns a list of strings separated by commas. 
        ''' The contents of this test protocol is sensor-specific. For its meaning, please refer to the sensor documentation.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' TEST:SENS?
        ''' </remarks>
        Public Function Chan_SelfTest(ByVal channel As Integer) As Text.StringBuilder
            Dim result As New System.Text.StringBuilder()
            Dim pInvokeResult As Integer = Native.Chan_SelfTest(Me._Handle, channel, result)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return result
        End Function

        ''' <summary>
        ''' Selects which measurement results are to be made available in the Trace mode. This functions is available only for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="auxiliaryValue">Selects which measurement results are to be made available in the Trace mode.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:AUXiliary NONE | MINMAX | RNDMAX
        ''' </remarks>
        Public Sub Chan_SetAuxiliary(ByVal channel As Integer, ByVal auxiliaryValue As Aux)
            Dim pInvokeResult As Integer = Native.Chan_SetAuxiliary(Me._Handle, channel, auxiliaryValue)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Queries which measurement results are available in the Trace mode. This functions is available only for NRP-Z81
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENSe:AUXiliary?
        ''' </remarks>
        Public Function Chan_GetAuxiliary(ByVal channel As Integer) As Aux
            Dim auxiliaryValue As Integer
            Dim pInvokeResult As Integer = Native.Chan_GetAuxiliary(Me._Handle, channel, auxiliaryValue)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return CType(auxiliaryValue, Aux)
        End Function

#End Region '/CHAN

#Region "SYST"

        ''' <summary>
        ''' Checking whether the firmware-version of a sensor is reasonably actual.
        ''' </summary>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SYST:INFO? "TYPE"
        ''' SYST:INFO? "SW BUILD"
        ''' </remarks>
        Public Function Fw_Version_Check() As FwStructure
            Dim fv As New FwStructure()
            Dim firmwareOkayAsUShort As UShort
            Dim pInvokeResult As Integer = Native.fw_version_check(Me._Handle, fv.BufferSize, fv.FirmwareCurrent, fv.FirmwareRequiredMinimum, firmwareOkayAsUShort)
            fv.FirmwareOkay = System.Convert.ToBoolean(firmwareOkayAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return fv
        End Function

        ''' <summary>
        ''' Sets status update time, which influences USB traffic during sensor's waiting for trigger. This function is available only for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="statusUpdateTime">Sets status update time, which influences USB traffic during sensor's waiting for trigger. Valid Range: 0..10 s. Default Value: 100e-4 s.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SYST:SUT
        ''' </remarks>
        Public Sub System_SetStatusUpdateTime(ByVal channel As Integer, ByVal statusUpdateTime As Double)
            Dim pInvokeResult As Integer = Native.System_SetStatusUpdateTime(Me._Handle, channel, statusUpdateTime)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Gets status update time. This function is available only for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SYST:SUT
        ''' </remarks>
        Public Function System_GetStatusUpdateTime(ByVal channel As Integer) As Double
            Dim statusUpdateTime As Double
            Dim pInvokeResult As Integer = Native.System_GetStatusUpdateTime(Me._Handle, channel, statusUpdateTime)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return statusUpdateTime
        End Function

        ''' <summary>
        ''' Sets result update time, which influences USB traffic if sensor is in continuous sweep mode. This function is available only for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="resultUpdateTime">Sets result update time, which influences USB traffic if sensor is in continuous sweep mode. Valid Range: 0..10 s. Default Value: 0.1 s.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SYST:RUT
        ''' </remarks>
        Public Sub System_SetResultUpdateTime(ByVal channel As Integer, ByVal resultUpdateTime As Double)
            Dim pInvokeResult As Integer = Native.System_SetResultUpdateTime(Me._Handle, channel, resultUpdateTime)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Gets result update time. This function is available only for NRP-Z81.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SYST:RUT
        ''' </remarks>
        Public Function System_GetResultUpdateTime(ByVal channel As Integer) As Double
            Dim resultUpdateTime As Double
            Dim pInvokeResult As Integer = Native.System_GetResultUpdateTime(Me._Handle, channel, resultUpdateTime)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return resultUpdateTime
        End Function

#End Region '/SYST

#Region "CALIB"

        ''' <summary>
        ''' This function does internal test measurements with enabled and disabled heater and returns the power difference between both measurements.
        ''' The result of this test is used to determine the long time drift of the power sensor.
        ''' This function is available only for NRP-Z56 and NRP-Z57.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' CAL:TEST?
        ''' </remarks>
        Public Function Calib_Test(ByVal channel As Integer) As Double
            Dim calibTest2 As Double
            Dim pInvokeResult As Integer = Native.Calib_Test(Me._Handle, channel, calibTest2)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return calibTest2
        End Function

        ''' <summary>
        ''' This function first does an internal heater test with CAL:TEST and returns the relative deviation between the test result 
        ''' and the result that was measured in the calibration lab during sensor calibration.
        ''' This function is available only for NRP-Z56 and NRP-Z57.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' CAL:TEST:DEV?
        ''' </remarks>
        Public Function Calib_GetTestDeviation(ByVal channel As Integer) As Double
            Dim testDeviation As Double
            Dim pInvokeResult As Integer = Native.Calib_GetTestDeviation(Me._Handle, channel, testDeviation)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return testDeviation
        End Function

        ''' <summary>
        ''' Returns the heater test result that was measured in the calibration lab during sensor calibration.
        ''' This function is available only for NRP-Z56 and NRP-Z57.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' CAL:TEST:REF?
        ''' </remarks>
        Public Function Calib_GetTestReference(ByVal channel As Integer) As Double
            Dim testReference As Double
            Dim pInvokeResult As Integer = Native.Calib_GetTestReference(Me._Handle, channel, testReference)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return testReference
        End Function

#End Region '/CALIB

#Region "MEAS"

        ''' <summary>
        ''' Returns the summary status of measurements on all channels. Returns TRUE if all channels have measurement ready.
        ''' </summary>
        Public Function Chans_IsMeasurementComplete() As Boolean
            Dim mc As UShort
            Dim pInvokeResult As Integer = Native.chans_isMeasurementComplete(Me._Handle, mc)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim measurementCompleted As Boolean = System.Convert.ToBoolean(mc)
            Return measurementCompleted
        End Function

        ''' <summary>
        ''' This function initiates an acquisition on the channels that you specifies in channel parameter. 
        ''' It then waits for the acquisition to complete, and returns the measurement for the channel you specify.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="timeoutMs">Pass the maximum length of time in which to allow the read measurement operation to complete.    
        ''' If the operation does not complete within this time interval, the function returns the RSNRPZ_ERROR_MAX_TIME_EXCEEDED error code.
        ''' When this occurs, you can call rsnrpz_chan_abort to cancel the read measurement operation and return the sensor to the Idle state.
        ''' Defined Values: <see cref="MeasMaxTime"/>. Default Value: 5000 (ms).
        ''' Notes: The Maximum Time parameter applies only to this function. It has no effect on other timeout parameters.
        ''' </param>
        Public Function Meass_ReadMeasurement(ByVal channel As Integer, ByVal timeoutMs As Integer) As Double
            Dim measurement As Double
            Dim pInvokeResult As Integer = Native.Meass_ReadMeasurement(Me._Handle, channel, timeoutMs, measurement)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return measurement
        End Function

        ''' <summary>
        ''' Returns the measurement the sensor acquires for the channel you specify. 
        ''' The measurement is from an acquisition that you previously initiated.  
        ''' You use the rsnrpz_chan_initiate function to start an acquisition on the channels that you specify. 
        ''' You use the rsnrpz_chan_isMeasurementComplete function to determine when the acquisition is complete.
        ''' You can call the rsnrpz_meass_readMeasurement function instead of the rsnrpz_chan_initiate function. 
        ''' The rsnrpz_meass_readMeasurement function starts an acquisition, waits for the acquisition to complete, and returns the measurement for the channel you specify.
        ''' Note: If the acquisition has not be initialized or measurement is still in progress and value is not available, function returns an error( RSNRPZ_ERROR_MEAS_NOT_AVAILABLE ).
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        Public Function Meass_FetchMeasurement(ByVal channel As Integer) As Double
            Dim measurement As Double
            Dim pInvokeResult As Integer = Native.Meass_FetchMeasurement(Me._Handle, channel, measurement)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return measurement
        End Function

        ''' <summary>
        ''' This function initiates an acquisition on the channels that you specifies in channel parameter.
        ''' It then waits for the acquisition to complete, and returns the measurement for the channel you specify.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="maximumTimeMs">Pass the maximum length of time in which to allow the read measurement operation to complete.    
        ''' If the operation does not complete within this time interval, the function returns the RSNRPZ_ERROR_MAX_TIME_EXCEEDED error code. 
        ''' When this occurs, you can call rsnrpz_chan_abort to cancel the read measurement operation and return the sensor to the Idle state.
        ''' Defined Values: <see cref="MeasMaxTime"/>. Default Value: 5000 (ms)
        ''' Notes: The Maximum Time parameter applies only to this function. It has no effect on other timeout parameters.
        ''' </param>
        ''' <param name="bufferSize">Pass the number of elements in the Measurement Array parameter. Default Value: None.</param>
        Public Function Meass_ReadBufferMeasurement(ByVal channel As Integer, ByVal maximumTimeMs As Integer, ByVal bufferSize As Integer) As Double()
            Dim measurementArray(bufferSize - 1) As Double 'Returns the measurement buffer that the sensor acquires.
            Dim readCount As Integer 'Indicates the number of points the function places in the Measurement Array.
            Dim pInvokeResult As Integer = Native.Meass_ReadBufferMeasurement(Me._Handle, channel, maximumTimeMs, bufferSize, measurementArray, readCount)
            Native.TestForError(Me._Handle, pInvokeResult)
            ReDim Preserve measurementArray(readCount - 1)
            Return measurementArray
        End Function

        ''' <summary>
        ''' Returns the buffer measurement the sensor acquires for the channel you specify. 
        ''' The measurement is from an acquisition that you previously initiated.  
        ''' You use the rsnrpz_chan_initiate function to start an acquisition on the channels that you specify. 
        ''' You use the rsnrpz_chan_isMeasurementComplete function to determine when the acquisition is complete.
        ''' You can call the rsnrpz_meas_readBufferMeasurement function instead of the rsnrpz_chan_initiate function. 
        ''' The rsnrpz_meass_readBufferMeasurement function starts an acquisition, waits for the acquisition to complete, and returns the measurement for the channel you specify.
        ''' Note: If the acquisition has not be initialized or measurement is still in progress and value is not available, function returns an error( RSNRPZ_ERROR_MEAS_NOT_AVAILABLE ).
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="arraySize"> Pass the number of elements in the Measurement Array parameter. Default Value: None.</param>
        Public Function Meass_FetchBufferMeasurement(ByVal channel As Integer, ByVal arraySize As Integer) As Double()
            Dim measurementArray(arraySize - 1) As Double 'Returns the measurement buffer that the sensor acquires.
            Dim readCount As Integer 'Indicates the number of points the function places in the Measurement Array.
            Dim pInvokeResult As Integer = Native.Meass_FetchBufferMeasurement(Me._Handle, channel, arraySize, measurementArray, readCount)
            Native.TestForError(Me._Handle, pInvokeResult)
            ReDim Preserve measurementArray(readCount - 1)
            Return measurementArray
        End Function

        ''' <summary>
        ''' Triggers a BUS event. If the sensor is in the WAIT_FOR_TRG state and the source for the trigger source is set to BUS, the sensor enters the MEASURING state.
        ''' This function invalidates all current measuring results. 
        ''' A query of measurement data following this function will thus always return the measured value determined in response to this function.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' *TRG
        ''' </remarks>
        Public Sub Meass_SendSoftwareTrigger(ByVal channel As Integer)
            Dim pInvokeResult As Integer = Native.Meass_SendSoftwareTrigger(Me._Handle, channel)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' This function initiates an acquisition on the channels that you specifies in channel parameter. 
        ''' It then waits for the acquisition to complete, and returns the auxiliary measurement for the channel you specify.
        ''' Note(s): If SENSE:AUX is set to None, Returns error.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="timeoutMs">Pass the maximum length of time in which to allow the read measurement operation to complete.    
        ''' If the operation does not complete within this time interval, the function returns the RSNRPZ_ERROR_MAX_TIME_EXCEEDED error code.  
        ''' When this occurs, you can call rsnrpz_chan_abort to cancel the read measurement operation and return the sensor to the Idle state.
        ''' Defined Values: <see cref="MeasMaxTime"/>. Default Value: 5000 (ms).
        ''' Notes: The Maximum Time parameter applies only to this function. It has no effect on other timeout parameters.
        ''' </param>
        Public Function Meass_ReadMeasurementAux(ByVal channel As Integer, ByVal timeoutMs As Integer) As AuxMeasurement
            Dim meas As New AuxMeasurement()
            Dim pInvokeResult As Integer = Native.Meass_ReadMeasurementAux(Me._Handle, channel, timeoutMs, meas.Measurement, meas.Aux1, meas.Aux2)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return meas
        End Function

        ''' <summary>
        ''' Returns the measurement the sensor acquires for the channel you specify.  The measurement is from an acquisition that you previously initiated.  
        ''' You use the rsnrpz_chan_initiate function to start an acquisition on the channels that you specify. 
        ''' You use the rsnrpz_chan_isMeasurementComplete function to determine when the acquisition is complete.
        ''' You can call the rsnrpz_meass_readMeasurement function instead of the rsnrpz_chan_initiate function.  
        ''' The rsnrpz_meass_readMeasurement function starts an acquisition, waits for the acquisition to complete, and returns the measurement for the channel you specify.
        ''' Note(s):
        ''' 1) If the acquisition has not be initialized or measurement is still in progress and value is not available, function returns an error( RSNRPZ_ERROR_MEAS_NOT_AVAILABLE ).
        ''' 2) If SENSE:AUX is set to None, Returns error.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="timeoutMs">Pass the maximum length of time in which to allow the read measurement operation to complete.    
        ''' If the operation does not complete within this time interval, the function returns the RSNRPZ_ERROR_MAX_TIME_EXCEEDED error code.  
        ''' When this occurs, you can call rsnrpz_chan_abort to cancel the read measurement operation and return the sensor to the Idle state.
        ''' Defined Values: <see cref="MeasMaxTime"/>. Default Value: 5000 (ms).
        ''' Notes: The Maximum Time parameter applies only to this function. It has no effect on other timeout parameters.
        ''' </param>
        Public Function Meass_FetchMeasurementAux(ByVal channel As Integer, ByVal timeoutMs As Integer) As AuxMeasurement
            Dim meas As New AuxMeasurement()
            Dim pInvokeResult As Integer = Native.Meass_FetchMeasurementAux(Me._Handle, channel, timeoutMs, meas.Measurement, meas.Aux1, meas.Aux2)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return meas
        End Function

        ''' <summary>
        ''' This function initiates an acquisition on the channels that you specifies in channel parameter. 
        ''' It then waits for the acquisition to complete, and returns the measurement for the channel you specify.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="maximumTimeMs">Pass the maximum length of time in which to allow the read measurement operation to complete.    
        ''' If the operation does not complete within this time interval, the function returns the RSNRPZ_ERROR_MAX_TIME_EXCEEDED error code. 
        ''' When this occurs, you can call rsnrpz_chan_abort to cancel the read measurement operation and return the sensor to the Idle state.
        ''' Defined Values: <see cref="MeasMaxTime"/>. Default Value: 5000 (ms).
        ''' Notes: The Maximum Time parameter applies only to this function.  It has no effect on other timeout parameters.
        ''' </param>
        ''' <param name="bufferSize"> Pass the number of elements in the Measurement Array parameter. Default Value: None.</param>
        Public Function Meass_ReadBufferMeasurementAux(ByVal channel As Integer, ByVal maximumTimeMs As Integer, ByVal bufferSize As Integer) As AuxMeasurement()
            Dim measurementArray(bufferSize - 1) As Double
            Dim aux1(bufferSize - 1) As Double
            Dim aux2(bufferSize - 1) As Double
            Dim readCount As Integer
            Dim pInvokeResult As Integer = Native.Meass_ReadBufferMeasurementAux(Me._Handle, channel, maximumTimeMs, bufferSize, measurementArray, aux1, aux2, readCount)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim auxMeas(readCount - 1) As AuxMeasurement
            For i As Integer = 0 To readCount - 1
                auxMeas(i) = New AuxMeasurement()
                With auxMeas(i)
                    .Measurement = measurementArray(i)
                    .Aux1 = aux1(i)
                    .Aux2 = aux2(i)
                End With
            Next
            Return auxMeas
        End Function

        ''' <summary>
        ''' Returns the buffer measurement the sensor acquires for the channel you specify.
        ''' The measurement is from an acquisition that you previously initiated.
        ''' You use the rsnrpz_chan_initiate function to start an acquisition on the channels that you specify.
        ''' You use the rsnrpz_chan_isMeasurementComplete function to determine when the acquisition is complete.
        ''' You can call the rsnrpz_meas_readBufferMeasurement function instead of the rsnrpz_chan_initiate function.
        ''' The rsnrpz_meass_readBufferMeasurement function starts an acquisition, waits for the acquisition to complete, and returns the measurement for the channel you specify.
        ''' Note:
        ''' 1) If the acquisition has not be initialized or measurement is still in progress and value is not available, function returns an error( RSNRPZ_ERROR_MEAS_NOT_AVAILABLE ).
        ''' 2) If SENSE:AUX is set to None, Returns error.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="maximumTimeMs">Pass the maximum length of time in which to allow the read measurement operation to complete.
        ''' If the operation does not complete within this time interval, the function returns the RSNRPZ_ERROR_MAX_TIME_EXCEEDED error code.
        ''' When this occurs, you can call rsnrpz_chan_abort to cancel the read measurement operation and return the sensor to the Idle state.
        ''' Defined Values: <see cref="MeasMaxTime"/>. Default Value: 5000 (ms).
        ''' Note: The Maximum Time parameter applies only to this function. It has no effect on other timeout parameters.
        ''' </param>
        ''' <param name="bufferSize">
        ''' Pass the number of elements in the Measurement Array parameter. Default Value: None.
        ''' </param>
        Public Function Meass_FetchBufferMeasurementAux(ByVal channel As Integer, ByVal maximumTimeMs As Integer, ByVal bufferSize As Integer) As AuxMeasurement()
            Dim measurementArray(bufferSize - 1) As Double
            Dim aux1(bufferSize - 1) As Double
            Dim aux2(bufferSize - 1) As Double
            Dim readCount As Integer
            Dim pInvokeResult As Integer = Native.Meass_FetchBufferMeasurementAux(Me._Handle, channel, maximumTimeMs, bufferSize, measurementArray, aux1, aux2, readCount)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim auxMeas(readCount - 1) As AuxMeasurement
            For i As Integer = 0 To readCount - 1
                auxMeas(i) = New AuxMeasurement()
                With auxMeas(i)
                    .Measurement = measurementArray(i)
                    .Aux1 = aux1(i)
                    .Aux2 = aux2(i)
                End With
            Next
            Return auxMeas
        End Function

#End Region '/MEAS

#Region "STATUS"

        ''' <summary>
        ''' This function resets the R&amp;S NRPZ registry to default values.
        ''' </summary>
        Public Sub Status_Preset()
            Dim pInvokeResult As Integer = Native.status_preset(Me._Handle)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' This function checks selected status register for bits defined in Bitmask and returns a logical OR of all defined bits.
        ''' </summary>
        ''' <param name="statusClass">Selects the status register.</param>
        ''' <param name="mask">Defines the bit which should be checked in the specified Status Register.</param>
        Public Function Status_CheckCondition(ByVal statusClass As StatClass, Optional ByVal mask As Sensors = Sensors.RSNRPZ_ALL_SENSORS) As Boolean
            Dim stateAsUShort As UShort
            Dim pInvokeResult As Integer = Native.Status_CheckCondition(Me._Handle, statusClass, mask, stateAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim state As Boolean = System.Convert.ToBoolean(stateAsUShort)
            Return state
        End Function

        ''' <summary>
        ''' Sets the PTransition and NTransition register of selected status register according to bitmask.
        ''' </summary>
        ''' <param name="statusClass">Selects the status register. Notes: For meaning of each status register consult Operation Manual.</param>
        ''' <param name="direction">Defines the direction of transition of the event.</param>
        ''' <param name="mask">Defines the bit which should be checked in the specified Status Register.</param>
        Public Sub Status_CatchEvent(ByVal statusClass As StatClass, ByVal direction As Direction, Optional ByVal mask As Sensors = Sensors.RSNRPZ_ALL_SENSORS)
            Dim pInvokeResult As Integer = Native.Status_CatchEvent(Me._Handle, statusClass, mask, direction)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' This function checks the selected status register for events specified by bitmask and sets returns their states. 
        ''' Finally all bits of shadow status register specified by Reset Action will be set to zero.
        ''' </summary>
        ''' <param name="statusClass">Selects the status register. Notes: For meaning of each status register consult Operation Manual.</param>
        ''' <param name="mask">Defines the bit which should be checked in the specified Status Register.</param>
        ''' <param name="resetMask">Defines which bits of the shadow status register will reset to zero when finishing the function.</param>
        Public Function Status_CheckEvent(ByVal statusClass As StatClass, ByVal mask As Sensors, Optional ByVal resetMask As Sensors = Sensors.RSNRPZ_ALL_SENSORS) As Boolean
            Dim eventsAsUShort As UShort
            Dim pInvokeResult As Integer = Native.Status_CheckEvent(Me._Handle, statusClass, mask, resetMask, eventsAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim events As Boolean = System.Convert.ToBoolean(eventsAsUShort)
            Return events
        End Function

        ''' <summary>
        ''' This function enables events defined by Bitmask in enable register respective to the selected status register.
        ''' </summary>
        ''' <param name="statusClass">Selects the status register. Notes: For meaning of each status register consult Operation Manual.</param>
        ''' <param name="mask">Defines the bits (channels) which should be set to one and will generate SRQ.
        ''' You can use following constant for enabling SRQ for specified channels. To disable multiple channels, bitwise OR them together. 
        ''' </param>
        Public Sub Status_EnableEventNotification(ByVal statusClass As StatClass, Optional ByVal mask As Sensors = Sensors.RSNRPZ_ALL_SENSORS)
            Dim pInvokeResult As Integer = Native.Status_EnableEventNotification(Me._Handle, statusClass, mask)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' This function disables events defined by Bitmask in enable register respective to the selected status register.
        ''' </summary>
        ''' <param name="statusClass">Selects the status register. Notes: For meaning of each status register consult Operation Manual.</param>
        ''' <param name="mask">Defines the bit which should be set to zero in the specified Enable Register.
        ''' You can use following constant for disabling SRQ for specified channels. To disable multiple channels, bitwise OR them together. 
        ''' </param>
        Public Sub Status_DisableEventNotification(ByVal statusClass As StatClass, Optional ByVal mask As Sensors = Sensors.RSNRPZ_ALL_SENSORS)
            Dim pInvokeResult As Integer = Native.Status_DisableEventNotification(Me._Handle, statusClass, mask)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the Nrp low level driver state
        ''' </summary>
        Public Function Status_DriverOpenState() As Boolean
            Dim driverStateAsUShort As UShort
            Dim pInvokeResult As Integer = Native.Status_DriverOpenState(Me._Handle, driverStateAsUShort)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim driverState As Boolean = System.Convert.ToBoolean(driverStateAsUShort)
            Return driverState
        End Function

        ''' <summary>
        ''' This function registers message, which will be send to specified window, when SRQ is occured.
        ''' </summary>
        ''' <param name="windowHandle">Handle to the window whose window procedure is to receive the message. If the parameter is set to 0 (NULL), the message is disabled.</param>
        ''' <param name="messageID">Specifies the message to be posted. If the message ID is set to 0, message will be not posted.</param>
        Public Sub Status_RegisterWindowMessage(ByVal windowHandle As UInteger, ByVal messageID As UInteger)
            Dim pInvokeResult As Integer = Native.Status_RegisterWindowMessage(Me._Handle, windowHandle, messageID)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

#End Region '/STATUS

#Region "SERVICE"

        ''' <summary>
        ''' This function initiates a temperature measurement of the sensor and reads the temperature value from the instrument.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        Public Function Service_GetDetectorTemperature(ByVal channel As Integer) As Double
            Dim temperature As Double
            Dim pInvokeResult As Integer = Native.Service_GetDetectorTemperature(Me._Handle, channel, temperature)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return temperature
        End Function

        ''' <summary>
        ''' Sets the number of simulation pairs count-value.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="blockCount">Sets the number of simulation pairs count-value. Valid Values: not checked. Default Value: 100.</param>
        Public Sub Service_StartSimulation(ByVal channel As Integer, ByVal blockCount As Integer)
            Dim pInvokeResult As Integer = Native.Service_StartSimulation(Me._Handle, channel, blockCount)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Sets the values which will be simulated. Right before calling this function must be called function rsnrpz_service_startSimulation!
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="valueCount">Sets the value count.  The amount of values is equal to Block Count set with function rsnrpz_service_startSimulation.</param>
        ''' <param name="values">Sets the values which will be simulated.  The amount of values is equal to Block Count set with function rsnrpz_service_startSimulation.</param>
        Public Sub Service_SetSimulationValues(ByVal channel As Integer, ByVal valueCount() As Integer, ByVal values() As Double)
            Dim pInvokeResult As Integer = Native.Service_SetSimulationValues(Me._Handle, channel, valueCount, values)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' This function stops the simulation by setting the count-value pairs to zero.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        Public Sub Service_StopSimulation(ByVal channel As Integer)
            Dim pInvokeResult As Integer = Native.Service_StopSimulation(Me._Handle, channel)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

#End Region '/SERVICE

#Region "INFO"

        ''' <summary>
        ''' This function should be used for polling whether a previously started zero calibration on a group of sensor has already finished. 
        ''' Zero calibration is an asynchronous operation and may take some seconds until completion. 
        ''' See the example code under rsnrpz_chans_zero() on how to conduct a zeroing calibration on a group of sensors.
        ''' Returns TRUE if all channels have calibration ready.
        ''' </summary>
        Public Function Chans_IsZeroingComplete() As Boolean
            Dim zc As UShort
            Dim pInvokeResult As Integer = Native.chans_isZeroingComplete(Me._Handle, zc)
            Native.TestForError(Me._Handle, pInvokeResult)
            Dim zeroingCompleted As Boolean = System.Convert.ToBoolean(zc)
            Return zeroingCompleted
        End Function

        ''' <summary>
        ''' Returns number of available channels (1, 2 or 4 depending on installed options NRP-B2 - Two channel interface and NRP-B5 - Four channel interface).
        ''' </summary>
        Public Function Chans_GetCount() As Integer
            Dim count As Integer
            Dim pInvokeResult As Integer = Native.chans_getCount(Me._Handle, count)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return count
        End Function

        ''' <summary>
        ''' Returns the revision numbers of the instrument driver and instrument firmware, and tells the user with which instrument firmware this revision of the driver is compatible. 
        ''' </summary>
        Public Function Revision_Query() As RevisionInfo
            Dim ri As New RevisionInfo()
            Dim pInvokeResult As Integer = Native.revision_query(Me._Handle, ri.InstrumentDriverRevision, ri.FirmwareRevision)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return ri
        End Function

        ''' <summary>
        ''' Returns the number of currently connected sensors.
        ''' </summary>
        Public Function GetSensorCount() As Integer
            Dim sensor_Count As Integer
            Dim pInvokeResult As Integer = Native.GetSensorCount(Me._Handle, sensor_Count)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return sensor_Count
        End Function

        ''' <summary>
        ''' Returns the name/descriptor of a connected sensor.
        ''' </summary>
        Public Function GetSensorInfo() As SensorInfo
            Dim si As New SensorInfo()
            Dim pInvokeResult As Integer = Native.GetSensorInfo(Me._Handle, si.Channel, si.Sensor_Name, si.Sensor_Type, si.Sensor_Serial)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return si
        End Function

        ''' <summary>
        ''' Sets the sensor name.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="name">Sets the sensor name. Valid Range: any string. Default Value: "".</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:POW:TSL:AVG:COUN
        ''' </remarks>
        Public Sub SetSensorName(ByVal channel As Integer, ByVal name As String)
            Dim pInvokeResult As Integer = Native.setSensorName(Me._Handle, channel, name)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the sensor name.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="maxLength">Sets the allocated size of Name buffer. Valid Values: &gt;0. Default Value: 10.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SENS:POW:TSL:AVG:COUN
        ''' </remarks>
        Public Function GetSensorName(ByVal channel As Integer, ByVal maxLength As UInteger) As Text.StringBuilder
            Dim name As New System.Text.StringBuilder()
            Dim pInvokeResult As Integer = Native.getSensorName(Me._Handle, channel, name, maxLength)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return name
        End Function

        ''' <summary>
        ''' If the above status function (= NrpGetDeviceStatusZ5() ) indicates that there is an NRP-Z5, this function supplies information 
        ''' about the connected devices at its ports A...D (using 'iPortIdx' = 0...3).
        ''' </summary>
        ''' <param name="port">Selects the port. Default Value <see cref="Z5Port.A"/>.</param>
        Public Function GetDeviceInfoZ5(ByVal port As Z5Port) As SensorInfo
            Dim si As New SensorInfo()
            Dim connectedAsUShort As UShort
            Dim pInvokeResult As Integer = Native.GetDeviceInfoZ5(Me._Handle, port, si.Sensor_Name, si.Sensor_Type, si.Sensor_Serial, connectedAsUShort)
            Dim connected As Boolean = System.Convert.ToBoolean(connectedAsUShort) 'TODO Возвращать или нет?
            Native.TestForError(Me._Handle, pInvokeResult)
            Return si
        End Function

        ''' <summary>
        ''' Returns a list showing the USB resources which are in use by what application.
        ''' </summary>
        ''' <param name="maxLen">Defines size of the array pointed to by 'cp Map'. Valid Values: no range checking. Default Value: 1024.</param>
        Public Function GetUsageMap(ByVal maxLen As UInteger) As UsageMap
            Dim um As New UsageMap()
            Dim pInvokeResult As Integer = Native.getUsageMap(Me._Handle, um.Map, maxLen, um.RetLen)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return um
        End Function

#End Region '/INFO

#Region "LED"

        ''' <summary>
        ''' Sets the led mode.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="mode">Sets the led mode. Default Value: <see cref="Ledmode.LedmodeUser"/>.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SYST:LED:MODE
        ''' </remarks>
        Public Sub SetLedMode(ByVal channel As Integer, ByVal mode As Ledmode)
            Dim pInvokeResult As Integer = Native.setLedMode(Me._Handle, channel, mode)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the led mode.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SYST:LED:MODE
        ''' </remarks>
        Public Function GetLedMode(ByVal channel As Integer) As Ledmode
            Dim mode As UInteger
            Dim pInvokeResult As Integer = Native.getLedMode(Me._Handle, channel, mode)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return CType(mode, Ledmode)
        End Function

        ''' <summary>
        ''' Sets the led color.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <param name="color">Sets the led color. Valid Range: 0 to 0xFF_FF_FF. Default Value: 0.
        ''' Color is 24-bit value which represents RGB values in the form of 0x00rrggbb, where rr, gg and bb are 8 bit values for red, green and blue respectively.
        ''' </param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SYST:LED:COL
        ''' </remarks>
        Public Sub SetLedColor(ByVal channel As Integer, ByVal color As UInteger)
            Dim pInvokeResult As Integer = Native.setLedColor(Me._Handle, channel, color)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Returns the led color.
        ''' </summary>
        ''' <param name="channel">Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.</param>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' SYST:LED:COL
        ''' </remarks>
        Public Function GetLedColor(ByVal channel As Integer) As UInteger
            Dim color As UInteger
            Dim pInvokeResult As Integer = Native.getLedColor(Me._Handle, channel, color)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return color
        End Function

#End Region '/LED

#Region "ERROR"

        ''' <summary>
        ''' Switches state checking of the instrument (reading of the Standard Event Register and checking it for error) status subsystem. 
        ''' Driver functions are using state checking which is by default enabled.
        ''' Notes:
        ''' (1) In debug mode enable state checking.
        ''' (2) For better bus throughput and instruments performance disable state checking.
        ''' (3) When state checking is disabled driver does not check if correct instrument model or option is used with each of the functions. This might cause unexpected behaviour of the instrument.
        ''' </summary>
        ''' <param name="stateChecking">Switches instrument state checking On or Off. Default Value: On.</param>
        Public Sub ErrorCheckState(ByVal stateChecking As Boolean)
            Dim pInvokeResult As Integer = Native.ErrorCheckState(Me._Handle, System.Convert.ToUInt16(stateChecking))
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Reads an error code from the instrument's error queue.
        ''' </summary>
        Public Function Error_Query() As ErrorInfo
            Dim ei As New ErrorInfo()
            Dim pInvokeResult As Integer = Native.Error_query(Me._Handle, ei.ErrorCode, ei.ErrorMessage)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return ei
        End Function

#End Region '/ERROR

#Region "CONTROL"

        ''' <summary>
        ''' Starts a single-shot measurement on all channels. The respective sensor goes to the INITIATED state. 
        ''' The command is completely executed when the sensor returns to the IDLE state. 
        ''' The command is ignored when the sensor is not in the IDLE state or when continuous measurements are selected (INIT:CONT ON). 
        ''' The command is only fully executed when the measurement is completed and the trigger system has again reached the IDLE state. 
        ''' INIT is the only remote control command that permits overlapping execution. 
        ''' Other commands can be received and processed while the command is being executed.
        ''' </summary>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' STAT:OPER:MEAS?
        ''' INIT:IMM
        ''' </remarks>
        Public Sub Chans_Initiate()
            Dim pInvokeResult As Integer = Native.chans_initiate(Me._Handle)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Starts zeroing of all sensors using the signal at each sensor input. 
        ''' Zeroing is an asynchronous operation which will require a couple of seconds. 
        ''' Therefore, after starting the function, the user should poll the current execution status by continuously calling rsnrpz_chans_isZeroComplete(). 
        ''' As soon as the zeroing has finished, the result of the operation can be queried by a call to rsnrpz_error_query(). 
        ''' </summary>
        ''' <remarks>
        ''' Remote-control command(s):
        ''' CAL:ZERO:AUTO
        ''' If the signal at the input considerably deviates from 0 W, the sensor aborts the zero calibration and raises an error condition. 
        ''' The rsnrpz driver queues the error for later retrieval by the rsnrpz_error_query() function.
        ''' Note: All sensors must be disconnected from all power sources. 
        ''' </remarks>
        ''' <example>
        ''' bool Zero( ViSession lSesID )
        ''' {
        '''   const int CH1 = 1;
        '''   ViStatus lStat = VI_SUCCESS;
        '''   ViBoolean bZeroComplete = VI_FALSE;
        '''   ViInt32 iErrorCode = VI_SUCCESS;
        '''   ViChar szErrorMsg[256];
        '''   /* Start zeroing the sensor  */
        '''   lStat = rsnrpz_chans_zero( lSesID );
        '''   if ( lStat != VI_SUCCESS )
        '''   {
        '''     fprintf( stderr, "Error 0x%08x in rsnrpz_chan_zero()", lStat );
        '''     return false;
        '''   }
        '''   while ( bZeroComplete == VI_FALSE )
        '''   {
        '''     lStat = rsnrpz_chans_isZeroComplete( lSesID, &amp;bZeroComplete );
        '''     if ( bZeroComplete )
        '''     {
        '''       rsnrpz_error_query( lSesID, &amp;iErrorCode, szErrorMsg );
        '''       fprintf( stderr, "Zero-Cal.: error %d, %s\n\n", iErrorCode, szErrorMsg );
        '''       break;
        '''     }
        '''     else 
        '''       SLEEP( 200 );
        '''   }
        '''   return iErrorCode == 0;
        ''' }
        ''' </example>
        Public Sub Chans_Zero()
            Dim pInvokeResult As Integer = Native.chans_zero(Me._Handle)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' Performs the same initialization as the rsnrpz_init() function (see there), but should be used when the sensor(s) are connected via an extension unit like, for example, AnywhereUSB.
        ''' 
        ''' Notes:
        ''' 1) Never use both the rsnrpz_init() and rsnrpz_init_long_distance() funtions concurrently
        '''    Locally connected sensors can also be used with a session ID returned by the 'long distance' version of init.
        ''' 
        ''' 2) Do not initilize every sensor with a rsnrpz_init... function. 
        '''    If you want comunicate with more than one sensor use rsnrpz_AddSensor() for adding a new channel.
        '''    The reason is that the rsnrpz_close() function destroys all sensor sessions assigned to a process.
        ''' </summary>
        ''' <param name="queryId">Specifies if an ID Query is sent to the instrument during the initialization procedure. Default Value: true (Do Query).
        ''' Notes: Under normal circumstances the ID Query ensures that the instrument initialized is the type supported by this driver. 
        ''' However circumstances may arise where it is undesirable to send an ID Query to the instrument.
        ''' In those cases; set this control to "Skip Query" and this function will initialize the selected interface, without doing an ID Query.
        ''' </param>
        ''' <param name="resetDevice">Specifies if the instrument is to be reset to its power-on settings during the initialization procedure. Default Value: True.</param>
        ''' <param name="port">Selects the port.</param>
        Public Shared Function InitZ5(ByVal queryId As Boolean, ByVal port As Z5Port, ByVal resetDevice As Boolean) As RsNrpz
            Dim handle As IntPtr
            Dim pInvokeResult As Integer = Native.initZ5(System.Convert.ToUInt16(queryId), port, System.Convert.ToUInt16(resetDevice), handle)
            Native.TestForError(New System.Runtime.InteropServices.HandleRef(Nothing, System.IntPtr.Zero), pInvokeResult)
            Return New RsNrpz(handle)
        End Function

        ''' <summary>
        ''' This function immediately sets all the sensors to the IDLE state. Measurements in progress are interrupted. 
        ''' If INIT:CONT ON is set, a new measurement is immediately started since the trigger system is not influenced.
        ''' </summary>
        ''' <remarks>
        ''' Remote-control command(s): ABOR
        ''' </remarks>
        Public Sub Chans_Abort()
            Dim pInvokeResult As Integer = Native.chans_abort(Me._Handle)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' This function resets the instrument to a known state and sends initialization commands to the instrument that set any necessary programmatic variables 
        ''' such as Headers Off, Short Command form, and Data Transfer Binary to the state necessary for the operation of the instrument driver.
        ''' </summary>
        Public Sub Reset()
            Dim pInvokeResult As Integer = Native.reset(Me._Handle)
            Native.TestForError(Me._Handle, pInvokeResult)
        End Sub

        ''' <summary>
        ''' This function runs the instrument's self test routine and returns the test result(s).
        ''' </summary>
        Public Function Self_Test() As SelfTestResult
            Dim res As New SelfTestResult()
            Dim pInvokeResult As Integer = Native.self_test(Me._Handle, res.Result, res.Message)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return res
        End Function

        ''' <summary>
        ''' This functions provides information about whether an NRP-Z5 is connected to the PC.
        ''' </summary>
        Public Function GetZ5IsAvailable() As Boolean
            Dim availability As Integer
            Dim pInvokeResult As Integer = Native.GetDeviceStatusZ5(Me._Handle, availability)
            Native.TestForError(Me._Handle, pInvokeResult)
            Return (availability = 1) '0 - Not Available; 1 - Available
        End Function

#End Region '/CONTROL

#Region "IDISPOSABLE"

        Private _Disposed As Boolean = True

        Public Overloads Sub Dispose() Implements System.IDisposable.Dispose
            Me.Dispose(True)
            System.GC.SuppressFinalize(Me)
        End Sub

        Private Overloads Sub Dispose(ByVal disposing As Boolean)
            If (Me._Disposed = False) Then
                Native.Close(Me._Handle)
                Me._Handle = New System.Runtime.InteropServices.HandleRef(Nothing, System.IntPtr.Zero)
            End If
            Me._Disposed = True
        End Sub

        Protected Overrides Sub Finalize()
            Me.Dispose(False)
        End Sub

#End Region '/IDISPOSABLE

#Region "NATIVE"

        ''' <summary>
        ''' Нативные функции.
        ''' </summary>
        Private Class Native

            Private Const LibPath As String = "c:/temp/rsnrpz_32.dll"

            <DllImport(LibPath, EntryPoint:="rsnrpz_init", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function init(ByVal Resource_Name As String, ByVal ID_Query As UShort, ByVal Reset_Device As UShort, ByRef Instrument_Handle As System.IntPtr) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_init_long_distance", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function long_distance_setup(ByVal Resource_Name As String, ByVal ID_Query As UShort, ByVal Reset_Device As UShort, ByRef Instrument_Handle As System.Runtime.InteropServices.HandleRef) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_initZ5", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function initZ5(ByVal ID_Query As UShort, ByVal Port As Integer, ByVal Reset_Device As UShort, ByRef Instrument_Handle As System.IntPtr) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chans_abort", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function chans_abort(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chans_getCount", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function chans_getCount(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByRef Count As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chans_initiate", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function chans_initiate(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chans_zero", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function chans_zero(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chans_isZeroingComplete", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function chans_isZeroingComplete(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByRef Zeroing_Completed As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chans_isMeasurementComplete", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function chans_isMeasurementComplete(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByRef Measurement_Completed As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_mode", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function chan_mode(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Measurement_Mode As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_timing_configureExclude", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function timing_configureExclude(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Exclude_Start As Double, ByVal Exclude_Stop As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_timing_setTimingExcludeStart", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function timing_setTimingExcludeStart(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Exclude_Start As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_timing_getTimingExcludeStart", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function timing_getTimingExcludeStart(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Exclude_Start As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_timing_setTimingExcludeStop", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function timing_setTimingExcludeStop(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Exclude_Stop As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_timing_getTimingExcludeStop", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function timing_getTimingExcludeStop(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Exclude_Stop As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_bandwidth_setBw", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function bandwidth_setBw(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Bandwidth As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_bandwidth_getBw", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function bandwidth_getBw(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Bandwidth As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_bandwidth_getBwList", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function bandwidth_getBwList(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Buffer_Size As Integer, ByVal Bandwidth_List As System.Text.StringBuilder) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_configureAvgAuto", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_configureAvgAuto(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Resolution As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_configureAvgNSRatio", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_configureAvgNSRatio(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal maximumNoiseRatio As Double, ByVal upperTimeLimit As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_configureAvgManual", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_configureAvgManual(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal count As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_setAutoEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_setAutoEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Auto_Enabled As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_getAutoEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_getAutoEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Auto_Enabled As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_setAutoMaxMeasuringTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_setAutoMaxMeasuringTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal upperTimeLimit As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_getAutoMaxMeasuringTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_getAutoMaxMeasuringTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Upper_Time_Limit As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_setAutoNoiseSignalRatio", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_setAutoNoiseSignalRatio(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal maximumNoiseRatio As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_getAutoNoiseSignalRatio", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_getAutoNoiseSignalRatio(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef maximumNoiseRatio As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_setAutoResolution", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_setAutoResolution(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Resolution As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_getAutoResolution", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_getAutoResolution(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Resolution As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_setAutoType", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_setAutoType(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal method As AutoCountType) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_getAutoType", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_getAutoType(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef method As AutoCountType) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_setCount", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_setCount(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal count As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_getCount", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_getCount(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Count As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_setEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_setEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Averaging As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_getEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_getEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Averaging As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_setSlot", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_setSlot(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Timeslot As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_getSlot", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_getSlot(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Timeslot As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_setTerminalControl", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_setTerminalControl(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal terminalControl As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_getTerminalControl", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_getTerminalControl(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Terminal_Control As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_avg_reset", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function avg_reset(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_range_setAutoEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function range_setAutoEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Auto_Range As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_range_getAutoEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function range_getAutoEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Auto_Range As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_range_setCrossoverLevel", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function range_setCrossoverLevel(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Crossover_Level As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_range_getCrossoverLevel", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function range_getCrossoverLevel(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Crossover_Level As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_range_setRange", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function range_setRange(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Range As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_range_getRange", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function range_getRange(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Range As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_corr_configureCorrections", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function corr_configureCorrections(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Offset_State As UShort, ByVal offset As Double, ByVal Reserved_1 As UShort, ByVal Reserved_2 As String, ByVal S_Parameter_Enable As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_setCorrectionFrequency", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SetCorrectionFrequency(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Frequency As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getCorrectionFrequency", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetCorrectionFrequency(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Frequency As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_setCorrectionFrequencyStep", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SetCorrectionFrequencyStep(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Frequency_Step As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getCorrectionFrequencyStep", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetCorrectionFrequencyStep(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Frequency_Step As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_setCorrectionFrequencySpacing", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SetCorrectionFrequencySpacing(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Frequency_Spacing As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getCorrectionFrequencySpacing", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetCorrectionFrequencySpacing(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Frequency_Spacing As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_corr_setOffset", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function corr_setOffset(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal offset As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_corr_getOffset", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function corr_getOffset(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Offset As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_corr_setOffsetEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function corr_setOffsetEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Offset_State As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_corr_getOffsetEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function corr_getOffsetEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Offset_State As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_corr_setSParamDeviceEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function corr_setSParamDeviceEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal S_Parameter_Enable As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_corr_getSParamDeviceEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function corr_getSParamDeviceEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef S_Parameter_Correction As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_corr_setSParamDevice", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function corr_setSParamDevice(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal S_Parameter As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_corr_getSParamDevice", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function corr_getSParamDevice(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef S_Parameter As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_corr_getSParamDevList", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function corr_getSParamDevList(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Buffer_Size As Integer, ByVal S_Parameter_Device_List As System.Text.StringBuilder) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_configureSourceGammaCorr", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function chan_configureSourceGammaCorr(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Source_Gamma_Correction As UShort, ByVal Magnitude As Double, ByVal Phase As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_setSourceGammaMagnitude", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SetSourceGammaMagnitude(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Magnitude As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getSourceGammaMagnitude", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetSourceGammaMagnitude(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Magnitude As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_setSourceGammaPhase", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SetSourceGammaPhase(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Phase As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getSourceGammaPhase", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetSourceGammaPhase(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Phase As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_setSourceGammaCorrEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SetSourceGammaCorrEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Source_Gamma_Correction As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getSourceGammaCorrEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetSourceGammaCorrEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Source_Gamma_Correction As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_configureReflectGammaCorr", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function chan_configureReflectGammaCorr(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Magnitude As Double, ByVal Phase As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_setReflectionGammaMagn", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SetReflectionGammaMagn(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Magnitude As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getReflectionGammaMagn", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetReflectionGammaMagn(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Magnitude As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_setReflectionGammaPhase", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SetReflectionGammaPhase(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Phase As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getReflectionGammaPhase", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetReflectionGammaPhase(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Phase As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_setReflectionGammaUncertainty", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SetReflectionGammaUncertainty(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Uncertainty As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getReflectionGammaUncertainty", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetReflectionGammaUncertainty(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Uncertainty As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_corr_configureDutyCycle", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function corr_configureDutyCycle(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Duty_Cycle_State As UShort, ByVal Duty_Cycle As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_corr_setDutyCycle", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function corr_setDutyCycle(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Duty_Cycle As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_corr_getDutyCycle", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function corr_getDutyCycle(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Duty_Cycle As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_corr_setDutyCycleEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function corr_setDutyCycleEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Duty_Cycle_State As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_corr_getDutyCycleEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function corr_getDutyCycleEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Duty_Cycle_State As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_setContAvAperture", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SetContAvAperture(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal ContAv_Aperture As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getContAvAperture", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetContAvAperture(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef ContAv_Aperture As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_setContAvSmoothingEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SetContAvSmoothingEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal ContAv_Smoothing As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getContAvSmoothingEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetContAvSmoothingEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef ContAv_Smoothing As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_setContAvBufferedEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SetContAvBufferedEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal ContAv_Buffered_Mode As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getContAvBufferedEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetContAvBufferedEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef ContAv_Buffered_Mode As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_setContAvBufferSize", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SetContAvBufferSize(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Buffer_Size As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getContAvBufferSize", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetContAvBufferSize(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Buffer_Size As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getContAvBufferCount", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetContAvBufferCount(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Buffer_Count As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getContAvBufferInfo", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetContAvBufferInfo(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal infoType As String, ByVal arraySize As Integer, ByVal Info As System.Text.StringBuilder) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_setBurstDropoutTolerance", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SetBurstDropoutTolerance(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Drop_out_Tolerance As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getBurstDropoutTolerance", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetBurstDropoutTolerance(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Drop_out_Tolerance As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_setBurstChopperEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SetBurstChopperEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal BurstAv_Chopper As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getBurstChopperEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetBurstChopperEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef BurstAv_Chopper As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_stat_confTimegate", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function stat_confTimegate(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal offset As Double, ByVal time As Double, ByVal midambleOffset As Double, ByVal midambleLength As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_stat_confScale", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function stat_confScale(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal referenceLevel As Double, ByVal range As Double, ByVal points As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_stat_setOffsetTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function stat_setOffsetTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal offset As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_stat_getOffsetTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function stat_getOffsetTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Offset As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_stat_setTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function stat_setTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal time As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_stat_getTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function stat_getTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Time As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_stat_setMidOffset", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function stat_setMidOffset(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal offset As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_stat_getMidOffset", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function stat_getMidOffset(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Offset As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_stat_setMidLength", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function stat_setMidLength(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal length As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_stat_getMidLength", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function stat_getMidLength(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Length As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_stat_setScaleRefLevel", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function stat_setScaleRefLevel(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal referenceLevel As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_stat_getScaleRefLevel", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function stat_getScaleRefLevel(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Reference_Level As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_stat_setScaleRange", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function stat_setScaleRange(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal range As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_stat_getScaleRange", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function stat_getScaleRange(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Range As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_stat_setScalePoints", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function stat_setScalePoints(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal points As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_stat_getScalePoints", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function stat_getScalePoints(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Points As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_stat_getScaleWidth", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function stat_getScaleWidth(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Width As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_tslot_configureTimeSlot", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function tslot_configureTimeSlot(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal timeSlotCount As Integer, ByVal width As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_tslot_setTimeSlotCount", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function tslot_setTimeSlotCount(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal timeSlotCount As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_tslot_getTimeSlotCount", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function tslot_getTimeSlotCount(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Time_Slot_Count As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_tslot_setTimeSlotWidth", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function tslot_setTimeSlotWidth(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal width As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_tslot_getTimeSlotWidth", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function tslot_getTimeSlotWidth(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Width As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_tslot_setTimeSlotMidOffset", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function tslot_setTimeSlotMidOffset(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal offset As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_tslot_getTimeSlotMidOffset", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function tslot_getTimeSlotMidOffset(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Offset As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_tslot_setTimeSlotMidLength", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function tslot_setTimeSlotMidLength(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal length As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_tslot_getTimeSlotMidLength", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function tslot_getTimeSlotMidLength(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Length As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_tslot_setTimeSlotChopperEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function tslot_setTimeSlotChopperEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Time_Slot_Chopper As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_tslot_getTimeSlotChopperEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function tslot_getTimeSlotChopperEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Time_Slot_Chopper As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_configureScope", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_configureScope(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal scopePoints As Integer, ByVal scopeTime As Double, ByVal offsetTime As Double, ByVal Realtime As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_fastZero", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_fastZero(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_setAverageEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_setAverageEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Scope_Averaging As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_getAverageEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_getAverageEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Scope_Averaging As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_setAverageCount", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_setAverageCount(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal count As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_getAverageCount", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_getAverageCount(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Count As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_setAverageTerminalControl", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_setAverageTerminalControl(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal terminalControl As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_getAverageTerminalControl", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_getAverageTerminalControl(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Terminal_Control As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_setOffsetTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_setOffsetTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal offsetTime As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_getOffsetTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_getOffsetTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Offset_Time As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_setPoints", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_setPoints(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal scopePoints As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_getPoints", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_getPoints(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Scope_Points As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_setRealtimeEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_setRealtimeEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Realtime As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_getRealtimeEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_getRealtimeEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Realtime As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_setTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_setTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal scopeTime As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_getTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_getTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Scope_Time As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_setAutoEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_setAutoEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Auto_Enabled As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_getAutoEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_getAutoEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Auto_Enabled As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_setAutoMaxMeasuringTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_setAutoMaxMeasuringTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal upperTimeLimit As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_getAutoMaxMeasuringTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_getAutoMaxMeasuringTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Upper_Time_Limit As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_setAutoNoiseSignalRatio", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_setAutoNoiseSignalRatio(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal maximumNoiseRatio As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_getAutoNoiseSignalRatio", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_getAutoNoiseSignalRatio(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef maximumNoiseRatio As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_setAutoResolution", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_setAutoResolution(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Resolution As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_getAutoResolution", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_getAutoResolution(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Resolution As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_setAutoType", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_setAutoType(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal method As AutoCountType) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_getAutoType", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_getAutoType(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef method As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_setEquivalentSampling", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_setEquivalentSampling(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Scope_Equivalent_Sampling As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_getEquivalentSampling", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function scope_getEquivalentSampling(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Scope_Equivalent_Sampling As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_meas_setMeasEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Scope_Meas_SetMeasEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Trace_Measurements As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_meas_getMeasEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Scope_Meas_GetMeasEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Trace_Measurements As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_meas_setMeasAlgorithm", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Scope_Meas_SetMeasAlgorithm(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal algorithm As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_meas_getMeasAlgorithm", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Scope_Meas_GetMeasAlgorithm(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef algorithm As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_meas_setLevelThresholds", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Scope_Meas_SetLevelThresholds(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal durationRef As Double, ByVal transitionLowRef As Double, ByVal transitionHighRef As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_meas_getLevelThresholds", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Scope_Meas_GetLevelThresholds(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef durationRef As Double, ByRef transitionLowRef As Double, ByRef transitionHighRef As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_meas_setTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Scope_Meas_SetTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal measTime As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_meas_getTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Scope_Meas_GetTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef measTime As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_meas_setOffsetTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Scope_Meas_SetOffsetTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal offsetTime As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_meas_getOffsetTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Scope_Meas_GetOffsetTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Offset_Time As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_meas_getPulseTimes", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Scope_Meas_GetPulseTimes(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Duty_Cycle As Double, ByRef Pulse_Duration As Double, ByRef Pulse_Period As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_meas_getPulseTransition", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Scope_Meas_GetPulseTransition(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Slope As Integer, ByRef Duration As Double, ByRef Occurence As Double, ByRef Overshoot As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_meas_getPulsePower", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Scope_Meas_GetPulsePower(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Average As Double, ByRef Min_Peak As Double, ByRef Max_Peak As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_meas_getPulseLevels", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Scope_Meas_GetPulseLevels(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Top_Level As Double, ByRef Base_Level As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_meas_getPulseReferenceLevels", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Scope_Meas_GetPulseReferenceLevels(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Low_Ref_Level As Double, ByRef High_Ref_Level As Double, ByRef Duration_Ref_Level As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_meas_setEquivalentSampling", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Scope_Meas_SetEquivalentSampling(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal scopeMeasEquivSampling As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_meas_getEquivalentSampling", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Scope_Meas_GetEquivalentSampling(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef scopeMeasEquivSampling As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_scope_meas_getSamplePeriod", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Scope_Meas_GetSamplePeriod(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Sample_Period As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_configureInternal", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_ConfigureInternal(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal triggerLevel As Double, ByVal triggerSlope As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_configureExternal", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_ConfigureExternal(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal triggerDelay As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_immediate", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_Immediate(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_setAutoDelayEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_SetAutoDelayEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal autoDelay As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_getAutoDelayEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_GetAutoDelayEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef autoDelay As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_setAutoTriggerEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_SetAutoTriggerEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal autoTrigger As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_getAutoTriggerEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_GetAutoTriggerEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef autoTrigger As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_setCount", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_SetCount(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal triggerCount As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_getCount", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_GetCount(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef triggerCount As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_setDelay", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_SetDelay(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal triggerDelay As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_getDelay", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_GetDelay(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef triggerDelay As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_setHoldoff", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_SetHoldoff(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal triggerHoldoff As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_getHoldoff", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_GetHoldoff(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef triggerHoldoff As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_setHysteresis", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_SetHysteresis(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal triggerHysteresis As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_getHysteresis", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_GetHysteresis(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef triggerHysteresis As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_setLevel", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_SetLevel(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal triggerLevel As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_getLevel", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_GetLevel(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef triggerLevel As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_setSlope", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_SetSlope(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal triggerSlope As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_getSlope", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_GetSlope(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef triggerSlope As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_setSource", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_SetSource(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal triggerSource As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_getSource", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_GetSource(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef triggerSource As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_setDropoutTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_SetDropoutTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal dropoutTime As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_getDropoutTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_GetDropoutTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef dropoutTime As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_setMasterState", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_SetMasterState(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal State As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_getMasterState", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_GetMasterState(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef State As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_setSyncState", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_SetSyncState(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal State As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_getSyncState", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_GetSyncState(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef State As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_info", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_Info(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal infoType As String, ByVal arraySize As Integer, ByVal Info As System.Text.StringBuilder) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_infoHeader", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_InfoHeader(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal parameterNumber As Integer, ByVal arraySize As Integer, ByVal Header As System.Text.StringBuilder) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_infosCount", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_InfosCount(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Count As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_fw_version_check", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function fw_version_check(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal Buffer_Size As Integer, ByVal Firmware_Current As System.Text.StringBuilder, ByVal Firmware_Required_Minimum As System.Text.StringBuilder, ByRef Firmware_Okay As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_system_setStatusUpdateTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function System_SetStatusUpdateTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal statusUpdateTime As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_system_getStatusUpdateTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function System_GetStatusUpdateTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef statusUpdateTime As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_system_setResultUpdateTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function System_SetResultUpdateTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal resultUpdateTime As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_system_getResultUpdateTime", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function System_GetResultUpdateTime(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef resultUpdateTime As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_calib_test", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Calib_Test(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Calib_Test_2 As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_calib_getTestDeviation", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Calib_GetTestDeviation(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Test_Deviation As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_calib_getTestReference", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Calib_GetTestReference(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Test_Reference As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_abort", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_Abort(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_initiate", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_Initiate(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_setInitContinuousEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SetInitContinuousEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Continuous_Initiate As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getInitContinuousEnabled", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetInitContinuousEnabled(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Continuous_Initiate As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_reset", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_Reset(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_setSamplingFrequency", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SetSamplingFrequency(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal samplingFrequency As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getSamplingFrequency", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetSamplingFrequency(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef samplingFrequency As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_zero", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_Zero(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_zeroAdvanced", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_ZeroAdvanced(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Zeroing As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_isZeroComplete", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_IsZeroComplete(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Zeroing_Complete As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_isMeasurementComplete", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_IsMeasurementComplete(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Measurement_Complete As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_selfTest", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SelfTest(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Result As System.Text.StringBuilder) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_setAuxiliary", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_SetAuxiliary(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal auxiliaryValue As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_chan_getAuxiliary", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Chan_GetAuxiliary(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef auxiliaryValue As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_meass_readMeasurement", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Meass_ReadMeasurement(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal timeoutMs As Integer, ByRef Measurement As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_meass_fetchMeasurement", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Meass_FetchMeasurement(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Measurement As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_meass_readBufferMeasurement", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Meass_ReadBufferMeasurement(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Maximum_Time__ms_ As Integer, ByVal Buffer_Size As Integer, <[In], Out> Measurement_Array As Double(), ByRef Read_Count As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_meass_fetchBufferMeasurement", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Meass_FetchBufferMeasurement(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal arraySize As Integer, <[In], Out> Measurement_Array As Double(), ByRef Read_Count As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_meass_sendSoftwareTrigger", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Meass_SendSoftwareTrigger(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_meass_readMeasurementAux", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Meass_ReadMeasurementAux(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal timeoutMs As Integer, ByRef Measurement As Double, ByRef Aux_1 As Double, ByRef Aux_2 As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_meass_fetchMeasurementAux", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Meass_FetchMeasurementAux(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal timeoutMs As Integer, ByRef Measurement As Double, ByRef Aux_1 As Double, ByRef Aux_2 As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_meass_readBufferMeasurementAux", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Meass_ReadBufferMeasurementAux(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Maximum_Time__ms_ As Integer, ByVal Buffer_Size As Integer, <[In], Out> Measurement_Array As Double(), <[In], Out> Aux_1_Array As Double(), <[In], Out> Aux_2_Array As Double(), ByRef Read_Count As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_meass_fetchBufferMeasurementAux", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Meass_FetchBufferMeasurementAux(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Maximum_Time__ms_ As Integer, ByVal Buffer_Size As Integer, <[In], Out> Measurement_Array As Double(), <[In], Out> Aux_1_Array As Double(), <[In], Out> Aux_2_Array As Double(), ByRef Read_Count As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_status_preset", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function status_preset(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_status_checkCondition", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Status_CheckCondition(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal statusClass As Integer, ByVal Mask As UInteger, ByRef State As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_status_catchEvent", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Status_CatchEvent(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal statusClass As Integer, ByVal Mask As UInteger, ByVal Direction As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_status_checkEvent", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Status_CheckEvent(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal statusClass As Integer, ByVal Mask As UInteger, ByVal Reset_Mask As UInteger, ByRef Events As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_status_enableEventNotification", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Status_EnableEventNotification(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal statusClass As Integer, ByVal Mask As UInteger) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_status_disableEventNotification", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Status_DisableEventNotification(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal statusClass As Integer, ByVal Mask As UInteger) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_status_driverOpenState", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Status_DriverOpenState(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByRef Driver_State As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_status_registerWindowMessage", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Status_RegisterWindowMessage(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal windowHandle As UInteger, ByVal messageID As UInteger) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_service_getDetectorTemperature", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Service_GetDetectorTemperature(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Temperature As Double) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_service_startSimulation", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Service_StartSimulation(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Block_Count As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_service_setSimulationValues", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Service_SetSimulationValues(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Value_Count As Integer(), ByVal Values As Double()) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_service_stopSimulation", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Service_StopSimulation(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_errorCheckState", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function ErrorCheckState(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal State_Checking As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_reset", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function reset(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_self_test", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function self_test(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByRef Self_Test_Result As Short, ByVal Self_Test_Message As System.Text.StringBuilder) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_error_query", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Error_query(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByRef Error_Code As Integer, ByVal Error_Message As System.Text.StringBuilder) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_revision_query", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function revision_query(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal Instrument_Driver_Revision As System.Text.StringBuilder, ByVal Firmware_Revision As System.Text.StringBuilder) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_GetSensorCount", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function GetSensorCount(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByRef Sensor_Count As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_GetSensorInfo", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function GetSensorInfo(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Sensor_Name As System.Text.StringBuilder, ByVal Sensor_Type As System.Text.StringBuilder, ByVal Sensor_Serial As System.Text.StringBuilder) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_setSensorName", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function setSensorName(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Name As String) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_getSensorName", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function getSensorName(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Name As System.Text.StringBuilder, ByVal Max_Length As UInteger) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_setLedMode", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function setLedMode(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Mode As UInteger) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_getLedMode", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function getLedMode(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Mode As UInteger) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_setLedColor", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function setLedColor(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Color As UInteger) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_getLedColor", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function getLedColor(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Color As UInteger) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_setMasterPort", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_SetMasterPort(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Port As UInteger) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_getMasterPort", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_GetMasterPort(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Port As UInteger) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_setSyncPort", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_SetSyncPort(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByVal Port As UInteger) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_trigger_getSyncPort", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Trigger_GetSyncPort(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal channel As Integer, ByRef Port As UInteger) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_getUsageMap", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function getUsageMap(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal Map As System.Text.StringBuilder, ByVal Max_Len As UInteger, ByRef Ret_Len As UInteger) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_GetDeviceStatusZ5", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function GetDeviceStatusZ5(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByRef Availability As Integer) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_GetDeviceInfoZ5", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function GetDeviceInfoZ5(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal Port As Integer, ByVal Sensor_Name As System.Text.StringBuilder, ByVal Sensor_Type As System.Text.StringBuilder, ByVal Sensor_Serial As System.Text.StringBuilder, ByRef Connected As UShort) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_close", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Close(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef) As Integer
            End Function

            <DllImport(LibPath, EntryPoint:="rsnrpz_error_message", CallingConvention:=CallingConvention.StdCall)>
            Public Shared Function Error_message(ByVal Instrument_Handle As System.Runtime.InteropServices.HandleRef, ByVal Status_Code As Integer, ByVal Message As System.Text.StringBuilder) As Integer
            End Function

            ''' <returns>
            ''' Returns the status code of this operation. 
            ''' The status code either indicates success or describes an error or warning condition. 
            ''' You examine the status code from each call to an instrument driver function to determine if an error occurred. 
            ''' To obtain a text description of the status code, call the rsnrpz_error_message function.
            '''           
            ''' The general meaning of the status code is as follows:
            ''' 
            ''' Value                  Meaning
            ''' -------------------------------
            ''' 0                      Success
            ''' Positive Values        Warnings
            ''' Negative Values        Errors
            ''' 
            ''' This driver defines the following status codes:
            '''           
            ''' Status    Description
            ''' -------------------------------------------------
            ''' 3FFF0085  Unknown status code - VI_WARN_UNKNOWN_STATUS
            '''            
            ''' This instrument driver also returns errors and warnings defined by other sources. 
            ''' The following table defines the ranges of additional status codes that this driver can return. 
            ''' The table lists the different include files that contain the defined constants for the particular status codes:
            '''            
            ''' Numeric Range (in Hex)   Status Code Types    
            ''' -------------------------------------------------
            ''' 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
            ''' BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
            ''' </returns>
            Public Shared Function TestForError(ByVal handle As System.Runtime.InteropServices.HandleRef, ByVal status As Integer) As Integer
                If (status < 0) Then
                    Native.ThrowError(handle, status)
                End If
                Return status
            End Function

            Private Shared Function ThrowError(ByVal handle As System.Runtime.InteropServices.HandleRef, ByVal code As Integer) As Integer
                Dim msg As New System.Text.StringBuilder(256)
                Native.Error_message(handle, code, msg)
                Throw New System.Runtime.InteropServices.ExternalException(msg.ToString(), code)
            End Function

        End Class '/Native

#End Region '/NATIVE

#Region "ENUMS"

        Public Enum SensorMode As Integer
            ''' <summary>
            ''' ContAv (default value): After a trigger event, the power is integrated over a time interval (Averaging).
            ''' </summary>
            SensorModeContav = 0
            ''' <summary>
            ''' Buffered ContAv: The same as ContAv except the buffered mode is switched On.
            ''' </summary>
            SensorModeBufContav = 1
            ''' <summary>
            ''' Timeslot: The power is measured simultaneously in a number of timeslots (up to 26).
            ''' </summary>
            SensorModeTimeslot = 2
            ''' <summary>
            ''' Burst: SENS:POW:BURS:DTOL defines the time interval during which a signal drop below the trigger level is not interpreted as the end of the burst. 
            ''' In the BurstAv mode, the set trigger source is ignored.
            ''' </summary>
            SensorModeBurst = 3
            ''' <summary>
            ''' Scope: A sequence of measurements is performed. The individual measured values are determined as in the ContAv mode.
            ''' </summary>
            SensorModeScope = 4
            ''' <summary>
            ''' CCDF NRP-Z51 supports only RSNRPZ_SENSOR_MODE_CONTAV and RSNRPZ_SENSOR_MODE_BUF_CONTAV.
            ''' </summary>
            SensorModeCcdf = 6
            ''' <summary>
            ''' PDF.
            ''' </summary>
            SensorModePdf = 7
        End Enum

        Public Enum AutoCountType As Integer
            AutoCountTypeResolution = 0
            AutoCountTypeNsr = 1
        End Enum

        Public Enum TerminalControl As Integer
            TerminalControlMoving = 0
            TerminalControlRepeat = 1
        End Enum

        Public Enum Spacing As Integer
            ''' <summary>
            ''' Linear increment of correction frequency (spacing is added).
            ''' </summary>
            Linear = 0
            ''' <summary>
            ''' Logarithmic increment of corrcetion frequency (spacing is multiplied).
            ''' </summary>
            Logarithmic = 1
        End Enum

        Public Enum ScopeMeasAlg As Integer
            ''' <summary>
            ''' The Histogram Algorithm computes the pulse levels analysing the Histogram of the trace data. 
            ''' Toplevel and Baselevel are the bins with the maximum number of hits in the upper and the lower half of the histogram.
            ''' If  the signal has too much noise that there is no maximum bin, the algorithm returns the min and max peak sample values as base and top level.
            ''' </summary>
            Histogram = 0
            ''' <summary>
            ''' The Integration Algorithm tries to find the pulse top power by fitting a reference rectangle pulse into the pulse 
            ''' by doing the integration of the pulse power and the according voltages. This algorithm should be used if the energy content of the complete pulse
            ''' (including rising and falling edges) is needed and not only the most probable top level.
            ''' </summary>
            Integration = 1
        End Enum

        Public Enum Slope As Integer
            SlopePositive = 0
            SlopeNegative = 1
        End Enum

        Public Enum TriggerSource As Integer
            ''' <summary>
            ''' (1) Bus: The trigger event is initiated by TRIG:IMM or *TRG. In this case, the setting for Trigger Slope is meaningless.
            ''' </summary>
            Bus = 0
            ''' <summary>
            ''' (2) External: Triggering is performed with an external signal applied to the trigger connector. 
            ''' The Trigger Slope setting determines whether the rising or the falling edge of the signal is to be used for triggering. 
            ''' Waiting for a trigger event can be skipped by Immediate Trigger.
            ''' </summary>
            External = 1
            ''' <summary>
            ''' (3) Hold: A measurement can only be triggered when Immediate Trigger is executed.
            ''' </summary>
            Hold = 2
            ''' <summary>
            ''' (4) Immediate: The sensor does not remain in the WAIT_FOR_TRG state but immediately changes to the MEASURING state.
            ''' </summary>
            Immediate = 3
            ''' <summary>
            ''' (5) Internal: The sensor determines the trigger time by means of the signal to be measured. 
            ''' When this signal exceeds the power set by Trigger Level, the measurement is started after the time set by Trigger Delay. 
            ''' Similar to External Trigger, waiting for a trigger event can also be skipped by Immediate Trigger.
            ''' </summary>
            Internal = 4
        End Enum

        Public Enum SamplingFrequency As Integer
            SamplingFrequency1 = 1
            SamplingFrequency2 = 2
        End Enum

        Public Enum Zeroing As Integer
            ''' <summary>
            ''' 
            ''' </summary>
            Once = 3
            ''' <summary>
            ''' (1) Low Frequencies - Does zeroing only for low frequencies (&lt; 500 MHZ)
            ''' </summary>
            ZeroLfr = 4
            ''' <summary>
            ''' (2) High Frequencies - Does zeroing for higher Frequencies (&gt;= 500 MHz).
            ''' </summary>
            ZeroUfr = 5
        End Enum

        Public Enum Aux As Integer
            ''' <summary>
            ''' Only the average power of the associated samples.
            ''' </summary>
            AuxNone = 0
            ''' <summary>
            ''' Provides the maximum and minimum as well.
            ''' </summary>
            AuxMinmax = 1
            ''' <summary>
            ''' Provides the maximum and a random sample.
            ''' </summary>
            AuxRndmax = 2
        End Enum

        Public Enum StatClass As Integer
            ''' <summary>
            ''' Sensor Connected .
            ''' </summary>
            StatClassDConn = 1
            ''' <summary>
            ''' Sensor Error.
            ''' </summary>
            StatClassDErr = 2
            ''' <summary>
            ''' Operation Calibrating Status Register.
            ''' </summary>
            StatClassOCal = 3
            ''' <summary>
            ''' Operation Measuring Status Register.
            ''' </summary>
            StatClassOMeas = 4
            ''' <summary>
            ''' Operation Trigger Status Register.
            ''' </summary>
            StatClassOTrigger = 5
            ''' <summary>
            ''' Operation Sense Status Register.
            ''' </summary>
            StatClassOSense = 6
            ''' <summary>
            ''' Operation Lower Limit Fail Status Register.
            ''' </summary>
            StatClassOLower = 7
            ''' <summary>
            ''' Operation Upper Limit Fail Status Register.
            ''' </summary>
            StatClassOUpper = 8
            ''' <summary>
            ''' Power Part of Questionable Power Status Register.
            ''' </summary>
            StatClassQPower = 9
            ''' <summary>
            ''' Questionable Window Status Register.
            ''' </summary>
            StatClassQWindow = 10
            ''' <summary>
            ''' Questionable Calibration Status Register.
            ''' </summary>
            StatClassQCal = 11
            ''' <summary>
            ''' Zero Part of Questionable Power Status Register.
            ''' </summary>
            StatClassQZer = 12
        End Enum

        Public Enum Direction As Integer
            ''' <summary>
            ''' None Transition.
            ''' </summary>
            DirectionNone = 0
            ''' <summary>
            ''' Positive Transition  (Default Value).
            ''' </summary>
            DirectionPtr = 1
            ''' <summary>
            ''' Negative Transition.
            ''' </summary>
            DirectionNtr = 2
            ''' <summary>
            ''' Both Transition.
            ''' </summary>
            DirectionBoth = 3
        End Enum

        Public Enum Ledmode As UInteger
            ''' <summary>
            ''' User Settings - LED is controlled by user-settings
            ''' </summary>
            LedmodeUser = 1
            ''' <summary>
            ''' Sensor Functions - LED is controlled by the sensor-functions
            ''' </summary>
            LedmodeSensor = 2
        End Enum

        Public Enum PortExt As UInteger
            ''' <summary>
            ''' RSNRPZ_PORT_EXT1 (1) - IO Signal
            ''' </summary>
            PortExt1 = 1
            ''' <summary>
            ''' RSNRPZ_PORT_EXT2 (5) - TRIG 2 IO Signal
            ''' </summary>
            PortExt2 = 5
        End Enum

        Public Enum Z5Port As Integer
            A = 0 'RSNRPZ_Z5_PORT_A
            B = 1 'RSNRPZ_Z5_PORT_B
            C = 2 'RSNRPZ_Z5_PORT_C
            D = 3 'RSNRPZ_Z5_PORT_D
        End Enum

        Public Enum InfoType As Integer
            None
            Manufacturer
            Type
            StockNumber
            Serial
            HWVersion
            HWVariant
            SWBuild
            Technology
            Function_
            MinPower
            MaxPower
            MinFreq
            MaxFreq
            Resolution
            Impedance
            Coupling
            CalAbs
            CalRefl
            CalSPara
            CalMisc
            CalTemp
            CalLin
            SpdMnemonic
        End Enum

        <Flags()>
        Public Enum Sensors As UInteger
            RSNRPZ_SENSOR_01 = &H2
            RSNRPZ_SENSOR_02 = &H4
            RSNRPZ_SENSOR_03 = &H8
            RSNRPZ_SENSOR_04 = &H10
            RSNRPZ_SENSOR_05 = &H20
            RSNRPZ_SENSOR_06 = &H40
            RSNRPZ_SENSOR_07 = &H80
            RSNRPZ_SENSOR_08 = &H100
            RSNRPZ_SENSOR_09 = &H200
            RSNRPZ_SENSOR_10 = &H400
            RSNRPZ_SENSOR_11 = &H800
            RSNRPZ_SENSOR_12 = &H1000
            RSNRPZ_SENSOR_13 = &H2000
            RSNRPZ_SENSOR_14 = &H4000
            RSNRPZ_SENSOR_15 = &H8000
            RSNRPZ_SENSOR_16 = &H10000
            RSNRPZ_SENSOR_17 = &H20000
            RSNRPZ_SENSOR_18 = &H40000
            RSNRPZ_SENSOR_19 = &H80000
            RSNRPZ_SENSOR_20 = &H100000
            RSNRPZ_SENSOR_21 = &H200000
            RSNRPZ_SENSOR_22 = &H400000
            RSNRPZ_SENSOR_23 = &H800000
            RSNRPZ_SENSOR_24 = &H1000000
            RSNRPZ_SENSOR_25 = &H2000000
            RSNRPZ_SENSOR_26 = &H4000000
            RSNRPZ_SENSOR_27 = &H8000000
            RSNRPZ_SENSOR_28 = &H10000000
            RSNRPZ_SENSOR_29 = &H20000000
            RSNRPZ_SENSOR_30 = &H40000000
            RSNRPZ_SENSOR_31 = &H80000000UI
            RSNRPZ_ALL_SENSORS =
                RSNRPZ_SENSOR_01 Or RSNRPZ_SENSOR_02 Or RSNRPZ_SENSOR_03 Or RSNRPZ_SENSOR_04 _
                Or RSNRPZ_SENSOR_05 Or RSNRPZ_SENSOR_06 Or RSNRPZ_SENSOR_07 Or RSNRPZ_SENSOR_08 _
                Or RSNRPZ_SENSOR_09 Or RSNRPZ_SENSOR_10 Or RSNRPZ_SENSOR_11 Or RSNRPZ_SENSOR_12 _
                Or RSNRPZ_SENSOR_13 Or RSNRPZ_SENSOR_14 Or RSNRPZ_SENSOR_15 Or RSNRPZ_SENSOR_16 _
                Or RSNRPZ_SENSOR_17 Or RSNRPZ_SENSOR_18 Or RSNRPZ_SENSOR_19 Or RSNRPZ_SENSOR_20 _
                Or RSNRPZ_SENSOR_21 Or RSNRPZ_SENSOR_22 Or RSNRPZ_SENSOR_23 Or RSNRPZ_SENSOR_24 _
                Or RSNRPZ_SENSOR_25 Or RSNRPZ_SENSOR_26 Or RSNRPZ_SENSOR_27 Or RSNRPZ_SENSOR_28 _
                Or RSNRPZ_SENSOR_29 Or RSNRPZ_SENSOR_30 Or RSNRPZ_SENSOR_31
            'RSNRPZ_ALL_SENSORS    'all       0xFFFFFFFE
        End Enum

        Public Enum MeasMaxTime As Integer
            RSNRPZ_VAL_MAX_TIME_IMMEDIATE = 0
            RSNRPZ_VAL_MAX_TIME_INFINITE = &HFFFFFFFF
        End Enum

        Public Enum Resolution
            Res1 = 1
            Res2 = 2
            Res3 = 3
            Res4 = 4
        End Enum

#End Region '/ENUMS

#Region "DICTS"


        Public InfoTypes As New Dictionary(Of InfoType, String) From {
            {InfoType.None, ""},
            {InfoType.Manufacturer, "Manufacturer"},
            {InfoType.Type, "Type"},
            {InfoType.StockNumber, "Stock Number"},
            {InfoType.Serial, "Serial"},
            {InfoType.HWVersion, "HWVersion"},
            {InfoType.HWVariant, "HWVariant"},
            {InfoType.SWBuild, "SW Build"},
            {InfoType.Technology, "Technology"},
            {InfoType.Function_, "Function"},
            {InfoType.MinPower, "MinPower"},
            {InfoType.MaxPower, "MaxPower"},
            {InfoType.MinFreq, "MinFreq"},
            {InfoType.MaxFreq, "MaxFreq"},
            {InfoType.Resolution, "Resolution"},
            {InfoType.Impedance, "Impedance"},
            {InfoType.Coupling, "Coupling"},
            {InfoType.CalAbs, "Cal. Abs."},
            {InfoType.CalRefl, "Cal. Refl."},
            {InfoType.CalSPara, "Cal. S-Para."},
            {InfoType.CalMisc, "Cal. Misc."},
            {InfoType.CalTemp, "Cal. Temp."},
            {InfoType.CalLin, "Cal. Lin."},
            {InfoType.SpdMnemonic, "SPD Mnemonic"}
        }

#End Region '/DICTS

#Region "STRUCTS"

        Public Structure SensorInfo
            ''' <summary>
            ''' Defines the channel number. Valid Range: 1 to max available channels. Default Value: 1.
            ''' </summary>
            Public Channel As Integer
            ''' <summary>
            ''' Returns selected sensor's name. The array has to have allocated at least 128 elements.
            ''' </summary>
            Public Sensor_Name As System.Text.StringBuilder
            ''' <summary>
            ''' Returns selected sensor's type. The array has to have allocated at least 128 elements.
            ''' </summary>
            Public Sensor_Type As System.Text.StringBuilder
            ''' <summary>
            ''' Returns selected sensor's serial number. The array has to have allocated at least 128 elements.
            ''' </summary>
            Public Sensor_Serial As System.Text.StringBuilder
        End Structure

        Public Structure LevelThresholds
            ''' <summary>
            ''' Defines duration reference level used to calculate pulse duration and pulse period.
            ''' Valid Range: 0..100%.
            ''' Default Value: 50%.
            ''' </summary>
            Public DurationRef As Double
            ''' <summary>
            ''' Defines transition low level used to calculate the pulse transition's rise time and its occurrences.
            ''' Valid Range: 0..100%.
            ''' Default Value: 10%.
            ''' </summary>
            Public TransitionLowRef As Double
            ''' <summary>
            ''' Defines transition high level used to calculate the pulse transition's fall time and its occurrences.
            ''' Valid Range: 0..100%.
            ''' Default Value: 90%.
            ''' </summary>
            Public TransitionHighRef As Double
        End Structure

        Public Structure PulseTimes
            ''' <summary>
            ''' Returns duty cycle value. Duty Cycle = (pulse duration / pulse period) * 100
            ''' </summary>
            Public DutyCycle As Double
            ''' <summary>
            ''' Returns pulse duration value. Pulse Duration is the time between the positive and the negative transition of one pulse.
            ''' </summary>
            Public PulseDuration As Double
            ''' <summary>
            ''' Returns pulse period value. Pulse Period is the time between two consecutive transitions of the same polarity.
            ''' </summary>
            Public PulsePeriod As Double
        End Structure

        Public Structure PulseTransition
            ''' <summary>
            ''' Returns transition duration value. The positive transition duration is measured from the point when the trace crosses the low reference level 
            ''' until it reaches the high reference level. Negative transition is vice versa.
            ''' </summary>
            Public Duration As Double
            ''' <summary>
            ''' Returns transition occurence value. The positive transition occurrence is the absolut time of the trace when it crosses the medial reference level.
            ''' </summary>
            Public Occurence As Double
            ''' <summary>
            ''' Returns overshoot value. The overshoot measures the height of the local maximum (minimum) following a rising (falling) transition. 
            ''' Overshoot is calculated in percent of the pulse amplitude (top level - base level).
            ''' Overshoot (pos) = 100 * (local maximum - top level) / (top level - base level)
            ''' Overshoot (neg) = 100 * (base level - local minimum) / (top level - base level)
            ''' </summary>
            Public Overshoot As Double
        End Structure

        Public Structure PulsePower
            ''' <summary>
            ''' Returns average power value.
            ''' </summary>
            Public Average As Double
            ''' <summary>
            ''' Returns min peak power value.
            ''' </summary>
            Public MinPeak As Double
            ''' <summary>
            ''' Returns max peak power value.
            ''' </summary>
            Public MaxPeak As Double
        End Structure

        Public Structure PulseLevels
            ''' <summary>
            ''' Top level value, in watt.
            ''' </summary>
            Public TopLevel As Double
            ''' <summary>
            ''' Base level value, in watt.
            ''' </summary>
            Public BaseLevel As Double
        End Structure

        Public Structure PulseReferenceLevels
            ''' <summary>
            ''' Absolute power at the 10% amplitude level.
            ''' </summary>
            Public LowRefLevel As Double
            ''' <summary>
            ''' Absolute power at the 90% amplitude level.
            ''' </summary>
            Public HighRefLevel As Double
            ''' <summary>
            ''' Absolute power at the 50% amplitude level.
            ''' </summary>
            Public DurationRefLevel As Double
        End Structure

        Public Structure FwStructure
            ''' <summary>
            ''' Size of the character arrays which return current and required firmware version (both char arrays should be same size and MUST be at least 16 chars each).
            ''' Valid Range: &gt;15.
            ''' Default Value: 256.
            ''' </summary>
            Public BufferSize As Integer
            ''' <summary>
            ''' Character array for returning the firmware version of the sensor.
            ''' Notes: The array must contain at least 16 elements ViChar[16].
            ''' </summary>
            Public FirmwareCurrent As System.Text.StringBuilder
            ''' <summary>
            ''' Character array for returning the required miminum firmware version.
            ''' Notes: The array must contain at least 16 elements ViChar[16].
            ''' </summary>
            Public FirmwareRequiredMinimum As System.Text.StringBuilder
            ''' <summary>
            ''' Returns 1 (TRUE) if sensor firmware is actual enough, 0 (FALSE) if firmware is out-dated. 
            ''' This parameter can be set to NULL if you are not interested in the result of the firmware version check.
            ''' </summary>
            Public FirmwareOkay As Boolean
        End Structure

        Public Structure AuxMeasurement
            ''' <summary>
            ''' Returns single measurement.
            ''' </summary>
            Public Measurement As Double
            ''' <summary>
            ''' First Auxiliary value.
            ''' </summary>
            Public Aux1 As Double
            ''' <summary>
            ''' Second Auxiliary value.
            ''' </summary>
            Public Aux2 As Double
        End Structure

        Public Structure RevisionInfo
            ''' <summary>
            ''' Instrument Driver Software Revision.
            ''' Notes: The array must contain at least 256 elements ViChar[256].
            ''' </summary>
            Public InstrumentDriverRevision As System.Text.StringBuilder
            ''' <summary>
            ''' Instrument Firmware Revision.
            ''' Notes: Because instrument does not support firmware revision the array is set to empty string or ignored if used VI_NULL.
            ''' </summary>
            Public FirmwareRevision As System.Text.StringBuilder
        End Structure

        Public Structure ErrorInfo
            ''' <summary>
            ''' Returns the error code read from the instrument's error queue.
            ''' </summary>
            Public ErrorCode As Integer
            ''' <summary>
            ''' Returns the error message string read from the instrument's error message queue.
            ''' The array must contain at least 256 elements ViChar[256].
            ''' </summary>
            Public ErrorMessage As System.Text.StringBuilder
        End Structure

        Public Structure SelfTestResult
            ''' <summary>
            ''' Value returned from the instrument self test. 
            ''' Zero means success. For any other code, see the device's operator's manual.
            ''' </summary>
            Public Result As Short
            ''' <summary>
            ''' String returned from the self test. 
            ''' See the device's operation manual for an explanation of the string's contents.
            ''' The array must contain at least 256 elements ViChar[256].
            ''' </summary>
            Public Message As System.Text.StringBuilder
        End Structure

        Public Structure UsageMap
            ''' <summary>
            ''' Returns a list of used sensors and their corresponding applications.
            ''' </summary>
            Public Map As System.Text.StringBuilder
            ''' <summary>
            ''' How big the full list would be.
            ''' </summary>
            Public RetLen As UInteger
        End Structure

#End Region '/STRUCTS

    End Class '/RsNrpz

End Namespace