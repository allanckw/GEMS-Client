using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Gems.UIWPF.Helper
{
    public class IdleHelper
    {
        static Timer timer1;

        [DllImport("user32.dll")]
        public static extern Boolean GetLastInputInfo(ref tagLASTINPUTINFO plii);

        public struct tagLASTINPUTINFO
        {
            public uint cbSize;
            public Int32 dwTime;
        }

        private static void timer1_Tick(object sender, EventArgs e)
        {
            tagLASTINPUTINFO LastInput = new tagLASTINPUTINFO();
            Int32 IdleTime;
            LastInput.cbSize = (uint)Marshal.SizeOf(LastInput);
            LastInput.dwTime = 0;

            if (GetLastInputInfo(ref LastInput))
            {
                IdleTime = System.Environment.TickCount - LastInput.dwTime;
                //label1.Text = IdleTime + "ms";
                if (IdleTime > (60000*10))
                {
                    timer1.Stop();
                    MessageBox.Show("You have been idle for 10 mins, please Refresh Your Data before continue!","Idle",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    //stopIdleTimer();
                }
            }
        }

        public static void startIdleTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
        }

        public static void stopIdleTimer()
        {
            if (timer1 != null)
                timer1.Stop();
        }
    }
}
