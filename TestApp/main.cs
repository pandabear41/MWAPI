using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MWAPI.Input.Sending;
using MWAPI.Input;

namespace TestApp
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (KeyboardSender.IsKeyDownAsync(VirtualKeyCode.END))
            {
                KeyboardSender.HardwareModifiedKeyStroke(ScanCode.LSHIFT, ScanCode.FIVE);
            }
        }

        private void gos_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            keyboardhook keybdh = new keyboardhook();
            keybdh.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mousehook keybdh = new mousehook();
            keybdh.Show();
        }
    }
}
