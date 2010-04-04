using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MWAPI.Input.Hook
{
    public enum MouseState : uint
    {
        MOUSEMOVE = 0x200,
        LBUTTONDOWN = 0x201,
        RBUTTONDOWN = 0x204,
        MBUTTONDOWN = 0x207,
        LBUTTONUP = 0x202,
        RBUTTONUP = 0x205,
        MBUTTONUP = 0x208,
        LBUTTONDBLCLK = 0x203,
        RBUTTONDBLCLK = 0x206,
        MBUTTONDBLCLK = 0x209,
        MOUSEWHEEL = 0x020A,
    }
}
