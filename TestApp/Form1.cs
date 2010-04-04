using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsAPI.InputSending;

namespace TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (KeyboardHook.IsKeyDownAsync(VirtualKeyCode.END))
            {
                KeyboardHook.HardwareModifiedKeyStroke(ScanCode.LSHIFT, ScanCode.FIVE);
            }
        }

        private void gos_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }
    }
}
