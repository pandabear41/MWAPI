﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsAPI.InputSending
{
    /// <summary>
    /// XButton definitions for use in the MouseData property of the <see cref="MOUSEINPUT"/> structure. (See: http://msdn.microsoft.com/en-us/library/ms646273(VS.85).aspx)
    /// </summary>
    public enum XButton : uint
    {
        /// <summary>
        /// Set if the first X button is pressed or released.
        /// </summary>
        XBUTTON1 = 0x0001,

        /// <summary>
        /// Set if the second X button is pressed or released.
        /// </summary>
        XBUTTON2 = 0x0002,
    }
}
