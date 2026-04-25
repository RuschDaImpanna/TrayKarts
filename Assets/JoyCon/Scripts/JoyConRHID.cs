#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_WSA || PACKAGE_DOCS_GENERATION
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Switch.LowLevel;
using UnityEngine.InputSystem.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace UnityEngine.InputSystem.Switch.LowLevel
{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_WSA

    [StructLayout(LayoutKind.Explicit, Size = 7)]
    internal struct SwitchJoyConRHIDInputState : IInputStateTypeInfo
    {
        public static FourCC Format = new FourCC('M', 'J', 'C', 'R'); // Momos joycon Right

        public FourCC format => Format;

        [InputControl(name = "leftStick", layout = "Stick", format = "VC2B")]
        [InputControl(name = "leftStick/x", offset = 1, format = "BYTE", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5,invert=false")]
        [InputControl(name = "leftStick/right", offset = 0, format = "BYTE", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=1,invert=false")]
        [InputControl(name = "leftStick/left", offset = 0, format = "BYTE", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5,clamp=1,clampMin=0,clampMax=0.5,invert")]
        [InputControl(name = "leftStick/y", offset = 0, format = "BYTE", parameters = "invert=false,normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5")]
        [InputControl(name = "leftStick/down", offset = 1, format = "BYTE", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5,clamp=1,clampMin=0.15,clampMax=0.5,invert")]
        [InputControl(name = "leftStick/up", offset = 1, format = "BYTE", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=0.85,invert=false")]
        [FieldOffset(0)] public byte stickX;
        [FieldOffset(1)] public byte stickY;

        //unused
        [InputControl(name = "rightStick", layout = "Stick", format = "VC2B")]
        [InputControl(name = "rightStick/x", offset = 0, format = "BYTE", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5")]
        [InputControl(name = "rightStick/left", offset = 0, format = "BYTE", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5,clamp=1,clampMin=0,clampMax=0.5,invert")]
        [InputControl(name = "rightStick/right", offset = 0, format = "BYTE", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=1")]
        [InputControl(name = "rightStick/y", offset = 1, format = "BYTE", parameters = "invert,normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5")]
        [InputControl(name = "rightStick/up", offset = 1, format = "BYTE", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5,clamp=1,clampMin=0.15,clampMax=0.5,invert")]
        [InputControl(name = "rightStick/down", offset = 1, format = "BYTE", parameters = "normalize,normalizeMin=0.15,normalizeMax=0.85,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=0.85,invert=false")]
        [FieldOffset(2)] public byte rightStickX;
        [FieldOffset(3)] public byte rightStickY;

        [InputControl(name = "buttonNorth", layout = "Button", displayName = "up button", shortDisplayName = "UB", bit = (int)Button.North)]
        [InputControl(name = "buttonEast", layout = "Button", displayName = "right button", shortDisplayName = "RB", bit = (int)Button.East, usages = new[] { "Back", "Cancel" })]
        [InputControl(name = "buttonWest", layout = "Button", displayName = "left button", shortDisplayName = "LB", bit = (int)Button.West, usage = "SecondaryAction")]
        [InputControl(name = "buttonSouth", layout = "Button", displayName = "down button", shortDisplayName = "DB", bit = (int)Button.South, usages = new[] { "PrimaryAction", "Submit" })]
        [InputControl(name = "leftShoulder", displayName = "SL", shortDisplayName = "SL", bit = (uint)Button.L)]
        [InputControl(name = "rightShoulder", displayName = "SR", shortDisplayName = "SR", bit = (uint)Button.R)]
        [InputControl(name = "leftStickPress", displayName = "Left Stick", bit = (uint)Button.StickL)]
        [InputControl(name = "leftTrigger", displayName = "ZR", shortDisplayName = "ZL", format = "BIT", bit = (uint)Button.ZL)]
        [InputControl(name = "rightTrigger", displayName = "R", shortDisplayName = "L", format = "BIT", bit = (uint)Button.ZR)]
        [InputControl(name = "start", displayName = "Plus", bit = (uint)Button.Plus, usage = "Menu")]
        //unused
        [InputControl(name = "dpad", format = "BIT", bit = 0, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = (int)Button.Up)]
        [InputControl(name = "dpad/right", bit = (int)Button.Right)]
        [InputControl(name = "dpad/down", bit = (int)Button.Down)]
        [InputControl(name = "dpad/left", bit = (int)Button.Left)]
        [InputControl(name = "rightStickPress", displayName = "Right Stick", bit = (uint)Button.StickR)]
        [InputControl(name = "select", displayName = "Minus", bit = (uint)Button.Minus)]
        [FieldOffset(4)] public ushort buttons1;

        public enum Button
        {
            Up = 0,
            Right = 1,
            Down = 2,
            Left = 3,

            West = 4,
            North = 5,
            South = 6,
            East = 7,

            L = 8,
            R = 9,

            StickL = 10,

            ZL = 12,
            ZR = 13,
            Plus = 14,


            //unused
            StickR = 11,
            Minus = 15,

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SwitchJoyConRHIDInputState WithButton(Button button, bool value = true)
        {
            Set(button, value);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(Button button, bool state)
        {
            Debug.Assert((int)button < 18, $"Expected button < 18");
            if ((int)button < 16)
            {
                var bit = (ushort)(1U << (int)button);
                if (state)
                    buttons1 = (ushort)(buttons1 | bit);
                else
                    buttons1 &= (ushort)~bit;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Press(Button button)
        {
            Set(button, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Release(Button button)
        {
            Set(button, false);
        }
    }
#endif
}
namespace UnityEngine.InputSystem.Switch
{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_WSA || PACKAGE_DOCS_GENERATION
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    [InputControlLayout(stateType = typeof(SwitchJoyConRHIDInputState), displayName = "Switch JoyCon Right")]
    public class SwitchJoyConRHID : SwitchJoyCon
    {
#if !UNITY_EDITOR
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
#endif
        static void Init()
        {
            InputSystem.RegisterLayout<SwitchJoyConRHID>(
                matches: new InputDeviceMatcher()
                    .WithInterface("HID")
                    .WithCapability("vendorId", 0x057e) // Nintendo
                    .WithCapability("productId", 0x2007) // Joycon Right
                    );
        }

        static SwitchJoyConRHID()
        {
            Init();
        }

        // filter out three lower bits as jitter noise
        internal const byte JitterMaskLow = 0b01111000;
        internal const byte JitterMaskHigh = 0b10000111;

        public override unsafe void OnStateEvent(InputEventPtr eventPtr)
        {
            if (eventPtr.type == StateEvent.Type && eventPtr.stateFormat == SwitchJoyConRHIDInputState.Format)
            {
                var currentState = (SwitchJoyConRHIDInputState*)((byte*)currentStatePtr + m_StateBlock.byteOffset);
                var newState = (SwitchJoyConRHIDInputState*)StateEvent.FromUnchecked(eventPtr)->state;

                var actuated =
                     // we need to make device current if axes are outside of deadzone specifying hardware jitter of sticks around zero point
                     newState->stickX < JitterMaskLow
                                             || newState->stickX > JitterMaskHigh
                    || newState->stickY < JitterMaskLow
                                             || newState->stickY > JitterMaskHigh
                    || newState->buttons1 != currentState->buttons1;


                if (!actuated)
                    InputSystem.s_Manager.DontMakeCurrentlyUpdatingDeviceCurrent();
            }

            InputState.Change(this, eventPtr);
        }
        protected override unsafe bool HandleFullReport(StateEvent* stateEvent, uint size, SwitchHIDGenericInputReport* genericReport)
        {
            if (genericReport->reportId == SwitchFullInputReport.ExpectedReportId && size >= SwitchFullInputReport.kSize)
            {
                var data = ((SwitchFullInputReport*)stateEvent->state)->ToHIDInputReport();
                *((SwitchJoyConRHIDInputState*)stateEvent->state) = data;
                stateEvent->stateFormat = SwitchJoyConRHIDInputState.Format;
                return true;
            }
            return false; // skip unrecognized reportId
        }


        [StructLayout(LayoutKind.Explicit, Size = kSize)]
        private struct SwitchFullInputReport
        {
            public const int kSize = 25;
            public const byte ExpectedReportId = 0x30;

            [FieldOffset(0)] public byte reportId;
            [FieldOffset(2)] public byte buttons1;
            [FieldOffset(3)] public byte buttons0;
            [FieldOffset(4)] public byte buttons2;
            [FieldOffset(9)] public byte left0;
            [FieldOffset(10)] public byte left1;
            [FieldOffset(11)] public byte left2;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public SwitchJoyConRHIDInputState ToHIDInputReport()
            {
                ////TODO: calibration curve

                var leftXRaw = (uint)(left0 | ((left1 & 0x0F) << 8));
                var leftYRaw = (uint)(((left1 & 0xF0) >> 4) | (left2 << 4));

                var leftXByte = (byte)(0xff - (byte)NumberHelpers.RemapUIntBitsToNormalizeFloatToUIntBits(leftXRaw, 12, 8));
                var leftYByte = (byte)NumberHelpers.RemapUIntBitsToNormalizeFloatToUIntBits(leftYRaw, 12, 8);

                var state = new SwitchJoyConRHIDInputState
                {
                    stickX = leftXByte,
                    stickY = leftYByte,
                    rightStickX = 0x80,
                    rightStickY = 0x80,
                };
                state.Set(SwitchJoyConRHIDInputState.Button.East, (buttons0 & 0x02) != 0);
                state.Set(SwitchJoyConRHIDInputState.Button.West, (buttons0 & 0x04) != 0);
                state.Set(SwitchJoyConRHIDInputState.Button.North, (buttons0 & 0x01) != 0);
                state.Set(SwitchJoyConRHIDInputState.Button.South, (buttons0 & 0x08) != 0);
                state.Set(SwitchJoyConRHIDInputState.Button.R, (buttons0 & 0x10) != 0);
                state.Set(SwitchJoyConRHIDInputState.Button.L, (buttons0 & 0x20) != 0);
                state.Set(SwitchJoyConRHIDInputState.Button.StickL, (buttons2 & 0x04) != 0);
                state.Set(SwitchJoyConRHIDInputState.Button.ZL, (buttons0 & 0x80) != 0);
                state.Set(SwitchJoyConRHIDInputState.Button.ZR, (buttons0 & 0x40) != 0);
                state.Set(SwitchJoyConRHIDInputState.Button.Plus, (buttons2 & 0x02) != 0);

                //Unused
                state.Set(SwitchJoyConRHIDInputState.Button.Down, false);
                state.Set(SwitchJoyConRHIDInputState.Button.Up, false);
                state.Set(SwitchJoyConRHIDInputState.Button.Right, false);
                state.Set(SwitchJoyConRHIDInputState.Button.Left, false);
                state.Set(SwitchJoyConRHIDInputState.Button.StickR, false);
                state.Set(SwitchJoyConRHIDInputState.Button.Minus, false);



                return state;
            }
        }
    }
#endif
}
#endif