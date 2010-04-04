using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MWAPI.Input.Hook
{
    public class KeyboardHook : GlobalHook
    {

        #region Events

        public event KeyEventHandler KeyDown;
        public event KeyEventHandler KeyUp;
        public event KeyPressEventHandler KeyPress;

        #endregion


        public KeyboardHook()
        {

            _hookType = WH_KEYBOARD_LL;

        }


        #region Methods

        protected override int HookCallbackProcedure(int nCode, int wParam, IntPtr lParam)
        {

            bool handled = false;

            if (nCode > -1 && (KeyDown != null || KeyUp != null || KeyPress != null))
            {

                KEYBDINPUT keyboardHookStruct =
                    (KEYBDINPUT)Marshal.PtrToStructure(lParam, typeof(KEYBDINPUT));

                // Is Control being held down?
                bool control = ((GetKeyState((int)VirtualKeyCode.LCONTROL) & 0x80) != 0) ||
                               ((GetKeyState((int)VirtualKeyCode.RCONTROL) & 0x80) != 0);

                // Is Shift being held down?
                bool shift = ((GetKeyState((int)VirtualKeyCode.LSHIFT) & 0x80) != 0) ||
                             ((GetKeyState((int)VirtualKeyCode.RSHIFT) & 0x80) != 0);

                // Is Alt being held down?
                bool alt = ((GetKeyState((int)VirtualKeyCode.LALT) & 0x80) != 0) ||
                           ((GetKeyState((int)VirtualKeyCode.RALT) & 0x80) != 0);

                // Is CapsLock on?
                bool capslock = (GetKeyState((int)VirtualKeyCode.CAPITAL) != 0);

                // Create event using keycode and control/shift/alt values found above
                KeyEventArgs e = new KeyEventArgs(
                    (Keys)(
                        keyboardHookStruct.Vk |
                        (control ? (int)Keys.Control : 0) |
                        (shift ? (int)Keys.Shift : 0) |
                        (alt ? (int)Keys.Alt : 0)
                        ));

                // Handle KeyDown and KeyUp events
                switch (wParam)
                {

                    case WM_KEYDOWN:
                    case WM_SYSKEYDOWN:
                        if (KeyDown != null)
                        {
                            KeyDown(this, e);
                            handled = handled || e.Handled;
                        }
                        break;
                    case WM_KEYUP:
                    case WM_SYSKEYUP:
                        if (KeyUp != null)
                        {
                            KeyUp(this, e);
                            handled = handled || e.Handled;
                        }
                        break;

                }

                // Handle KeyPress event
                if (wParam == WM_KEYDOWN &&
                   !handled &&
                   !e.SuppressKeyPress &&
                    KeyPress != null)
                {

                    byte[] keyState = new byte[256];
                    byte[] inBuffer = new byte[2];
                    GetKeyboardState(keyState);

                    if (ToAscii(keyboardHookStruct.Vk,
                              keyboardHookStruct.Scan,
                              keyState,
                              inBuffer,
                              (int)keyboardHookStruct.Flags) == 1)
                    {

                        char key = (char)inBuffer[0];
                        if ((capslock ^ shift) && Char.IsLetter(key))
                            key = Char.ToUpper(key);
                        KeyPressEventArgs e2 = new KeyPressEventArgs(key);
                        KeyPress(this, e2);
                        handled = handled || e.Handled;

                    }

                }

            }

            if (handled)
            {
                return 1;
            }
            else
            {
                return CallNextHookEx(_handleToHook, nCode, wParam, lParam);
            }

        }

        #endregion
    }
}
