using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.Switch.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Controls;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.InputSystem.Switch
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    //[InputControlLayout(stateType = typeof(SwitchJoyConState), isGenericTypeOfDevice = true)]
    [InputControlLayout(displayName = "Switch Joycon")]
    public class SwitchJoyCon : Gamepad, IInputStateCallbackReceiver, IEventPreProcessor
    {
#if !UNITY_EDITOR
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
#endif
        static void Init()
        {
            InputSystem.RegisterLayout<SwitchJoyCon>();
        }

        static SwitchJoyCon()
        {
            Init();
        }


        public bool GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
        {
            return false;
        }
        private int m_HandshakeStepIndex;
        private double m_HandshakeTimer;

        public new static SwitchJoyCon current { get; private set; }

        public override void MakeCurrent()
        {
            base.MakeCurrent();
            current = this;
        }
        protected override void OnRemoved()
        {
            base.OnRemoved();
            if (current == this)
                current = null;
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
        }


        private void HandshakeRestart()
        {
            // Delay first command issue until some time into the future
            m_HandshakeStepIndex = -1;
            m_HandshakeTimer = InputRuntime.s_Instance.currentTime;
        }


        private void HandshakeTick()
        {
            const double handshakeRestartTimeout = 2.0;
            const double handshakeNextStepTimeout = 0.1;

            var currentTime = InputRuntime.s_Instance.currentTime;

            // There were no events for last few seconds, restart handshake
            if (currentTime >= m_LastUpdateTimeInternal + handshakeRestartTimeout &&
                currentTime >= m_HandshakeTimer + handshakeRestartTimeout)
                m_HandshakeStepIndex = 0;
            // If handshake is complete, ignore the tick.
            else if (m_HandshakeStepIndex + 1 >= s_HandshakeSequence.Length)
                return;
            // If we timeout, proceed to next step after some time is elapsed.
            else if (currentTime > m_HandshakeTimer + handshakeNextStepTimeout)
                m_HandshakeStepIndex++;
            // If we haven't timed out on handshake step, skip the tick.
            else
                return;

            m_HandshakeTimer = currentTime;

            var command = s_HandshakeSequence[m_HandshakeStepIndex];

            var commandBt = SwitchMagicOutputHIDBluetooth.Create(command);
            if (ExecuteCommand(ref commandBt) > 0)
                return;

            //var commandUsb = SwitchMagicOutputHIDUSB.Create(command);
            //ExecuteCommand(ref commandUsb);
        }

        public void OnNextUpdate()
        {
            HandshakeTick();
        }
        private static readonly SwitchMagicOutputReport.CommandIdType[] s_HandshakeSequence = new[]
        {
            SwitchMagicOutputReport.CommandIdType.Status,
            SwitchMagicOutputReport.CommandIdType.Handshake,
            SwitchMagicOutputReport.CommandIdType.Highspeed,
            SwitchMagicOutputReport.CommandIdType.Handshake,
            SwitchMagicOutputReport.CommandIdType.ForceUSB
        };
        protected override void OnAdded()
        {
            base.OnAdded();

            HandshakeRestart();
        }
        public virtual void OnStateEvent(InputEventPtr eventPtr)
        {
            throw new System.NotImplementedException();
        }

        public unsafe bool PreProcessEvent(InputEventPtr eventPtr)
        {
            if (eventPtr.type == DeltaStateEvent.Type)
                // if someone queued delta state SPVS directly, just use as-is
                // otherwise skip all delta state events
                return DeltaStateEvent.FromUnchecked(eventPtr)->stateFormat == SwitchJoyConLHIDInputState.Format;

            // use all other non-state/non-delta-state events
            if (eventPtr.type != StateEvent.Type)
                return true;

            var stateEvent = StateEvent.FromUnchecked(eventPtr);
            var size = stateEvent->stateSizeInBytes;

            if (stateEvent->stateFormat == SwitchJoyConLHIDInputState.Format)
                return true; // if someone queued SPVS directly, just use as-is

            if (stateEvent->stateFormat != SwitchHIDGenericInputReport.Format || size < sizeof(SwitchHIDGenericInputReport))
                return false; // skip unrecognized state events otherwise they will corrupt control states

            var genericReport = (SwitchHIDGenericInputReport*)stateEvent->state;
            //if (genericReport->reportId == SwitchSimpleInputReport.ExpectedReportId && size >= SwitchSimpleInputReport.kSize)
            //{
            //    var data = ((SwitchSimpleInputReport*)stateEvent->state)->ToHIDInputReport();
            //    *((SwitchJoyConLHIDInputState*)stateEvent->state) = data;
            //    stateEvent->stateFormat = SwitchJoyConLHIDInputState.Format;
            //    return true;
            //}
            //else 
            return HandleFullReport(stateEvent, size, genericReport);
        }

        protected virtual unsafe bool HandleFullReport(StateEvent* stateEvent, uint size, SwitchHIDGenericInputReport* genericReport)
        {
            return false;
        }

        [StructLayout(LayoutKind.Explicit)]
        protected struct SwitchHIDGenericInputReport
        {
            public static FourCC Format => new FourCC('H', 'I', 'D');

            [FieldOffset(0)] public byte reportId;
        }

        [StructLayout(LayoutKind.Explicit, Size = kSize)]
        internal struct SwitchMagicOutputReport
        {
            public const int kSize = 49;

            public const byte ExpectedReplyInputReportId = 0x81;

            [FieldOffset(0)] public byte reportType;
            [FieldOffset(1)] public byte commandId;

            internal enum ReportType
            {
                Magic = 0x80
            }

            public enum CommandIdType
            {
                Status = 0x01,
                Handshake = 0x02,
                Highspeed = 0x03,
                ForceUSB = 0x04,
            }
        }
        [StructLayout(LayoutKind.Explicit, Size = kSize)]
        internal struct SwitchMagicOutputHIDBluetooth : IInputDeviceCommandInfo
        {
            public static FourCC Type => new FourCC('H', 'I', 'D', 'O');
            public FourCC typeStatic => Type;

            public const int kSize = InputDeviceCommand.kBaseCommandSize + 49;

            [FieldOffset(0)] public InputDeviceCommand baseCommand;
            [FieldOffset(InputDeviceCommand.kBaseCommandSize + 0)] public SwitchMagicOutputReport report;

            public static SwitchMagicOutputHIDBluetooth Create(SwitchMagicOutputReport.CommandIdType type)
            {
                return new SwitchMagicOutputHIDBluetooth
                {
                    baseCommand = new InputDeviceCommand(Type, kSize),
                    report = new SwitchMagicOutputReport
                    {
                        reportType = (byte)SwitchMagicOutputReport.ReportType.Magic,
                        commandId = (byte)type
                    }
                };
            }
        }

    }
}