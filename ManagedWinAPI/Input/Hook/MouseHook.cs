using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MWAPI.Input.Hook
{
    public class MouseHook : GlobalHook
    {


        #region MouseEventType Enum

        private enum MouseEventType
        {
            None,
            MouseDown,
            MouseUp,
            DoubleClick,
            MouseWheel,
            MouseMove
        }

        #endregion

        #region Events

        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseMove;
        public event MouseEventHandler MouseWheel;

        public event EventHandler Click;
        public event EventHandler DoubleClick;

        #endregion

        #region Constructor

        public MouseHook()
        {

            _hookType = WH_MOUSE_LL;

        }

        #endregion


        #region Methods

        protected override int HookCallbackProcedure(int nCode, int wParam, IntPtr lParam)
        {

            if (nCode > -1 && (MouseDown != null || MouseUp != null || MouseMove != null))
            {

                MOUSEINPUT mouseHookStruct =
                    (MOUSEINPUT)Marshal.PtrToStructure(lParam, typeof(MOUSEINPUT));

                
                MouseButtons button = GetButton(wParam);
                MouseEventType eventType = GetEventType(wParam);

                MouseEventArgs e = new MouseEventArgs(
                    button,
                    (eventType == MouseEventType.DoubleClick ? 2 : 1),
                    mouseHookStruct.pt.X,
                    mouseHookStruct.pt.Y,
                    (eventType == MouseEventType.MouseWheel ? (short)((mouseHookStruct.MouseData >> 16) & 0xffff) : 0));

                // Prevent multiple Right Click events (this probably happens for popup menus)
                if (button == MouseButtons.Right && mouseHookStruct.Flags != 0)
                {
                    eventType = MouseEventType.None;
                }

                switch (eventType)
                {
                    case MouseEventType.MouseDown:
                        if (MouseDown != null)
                        {
                            MouseDown(this, e);
                        }
                        break;
                    case MouseEventType.MouseUp:
                        if (Click != null)
                        {
                            Click(this, new EventArgs());
                        }
                        if (MouseUp != null)
                        {
                            MouseUp(this, e);
                        }
                        break;
                    case MouseEventType.DoubleClick:
                        if (DoubleClick != null)
                        {
                            DoubleClick(this, new EventArgs());
                        }
                        break;
                    case MouseEventType.MouseWheel:
                        if (MouseWheel != null)
                        {
                            MouseWheel(this, e);
                        }
                        break;
                    case MouseEventType.MouseMove:
                        if (MouseMove != null)
                        {
                            MouseMove(this, e);
                        }
                        break;
                    default:
                        break;
                }

            }

            return CallNextHookEx(_handleToHook, nCode, wParam, lParam);

        }

        private MouseButtons GetButton(Int32 wParam)
        {

            switch ((MouseState)wParam)
            {

                case MouseState.LBUTTONDOWN:
                case MouseState.LBUTTONUP:
                case MouseState.LBUTTONDBLCLK:
                    return MouseButtons.Left;
                case MouseState.RBUTTONDOWN:
                case MouseState.RBUTTONUP:
                case MouseState.RBUTTONDBLCLK:
                    return MouseButtons.Right;
                case MouseState.MBUTTONDOWN:
                case MouseState.MBUTTONUP:
                case MouseState.MBUTTONDBLCLK:
                    return MouseButtons.Middle;
                default:
                    return MouseButtons.None;

            }

        }

        private MouseEventType GetEventType(Int32 wParam)
        {

            switch ((MouseState)wParam)
            {

                case MouseState.LBUTTONDOWN:
                case MouseState.RBUTTONDOWN:
                case MouseState.MBUTTONDOWN:
                    return MouseEventType.MouseDown;
                case MouseState.LBUTTONUP:
                case MouseState.RBUTTONUP:
                case MouseState.MBUTTONUP:
                    return MouseEventType.MouseUp;
                case MouseState.LBUTTONDBLCLK:
                case MouseState.RBUTTONDBLCLK:
                case MouseState.MBUTTONDBLCLK:
                    return MouseEventType.DoubleClick;
                case MouseState.MOUSEWHEEL:
                    return MouseEventType.MouseWheel;
                case MouseState.MOUSEMOVE:
                    return MouseEventType.MouseMove;
                default:
                    return MouseEventType.None;

            }
        }

        #endregion
    }
}
