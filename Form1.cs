using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Input;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Timer = System.Windows.Forms.Timer;
using System.Reflection.Emit;

namespace textnet
{
    public partial class Form1 : Form
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;

        private static LowLevelKeyboardProc _proc;
        private static IntPtr _hookID = IntPtr.Zero;

        private Stopwatch stopwatch = new Stopwatch();

        public Form1()
        {
            InitializeComponent();
            _proc = HookCallback;
            _hookID = SetHook(_proc);
            lblTxt.Text = "";

            // Set up a timer
            Timer clearLabelTimer = new Timer();
            clearLabelTimer.Interval = 2500; // 2.5 seconds in milliseconds
            clearLabelTimer.Tick += ClearLabelTimer_Tick;
            clearLabelTimer.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnhookWindowsHookEx(_hookID);
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private void ClearLabelTimer_Tick(object sender, EventArgs e)
        {
            // Check if 2.5 seconds have elapsed since the last keypress
            if (stopwatch.ElapsedMilliseconds > 1800)
            {
                // Clear the label or perform any action needed
                lblTxt.Text = "";
                Console.WriteLine("Label cleared due to inactivity.");
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            int textNum = lblTxt.Text.Length;
            if (textNum >= 75)
            {
                lblTxt.Text = "";
            }
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;
                if (key.ToString() == "LControlKey" || key.ToString() == "RControlKey")
                {
                    lblTxt.Text += "[︽]";
                }
                else if (key.ToString() == "LShiftKey" || key.ToString() == "RShiftKey")
                {
                    lblTxt.Text += "[⇧]";
                }
                else if(key.ToString() == "LWin" || key.ToString() == "RWin")
                {
                    lblTxt.Text += "[⌘]";
                }

                else if(key.ToString() == "Back")
                {
                    lblTxt.Text += "[⌫]";
                }
                else if(key.ToString() == "Return")
                {
                    lblTxt.Text += "[➠]";
                }
                else if(key.ToString() == "Oem1")
                {
                    lblTxt.Text += ";";
                }
                else if(key.ToString() == "Oem5")
                {
                    lblTxt.Text += "\\";
                }
                else if(key.ToString() == "Oem6")
                {
                    lblTxt.Text += "]";
                }
                else if(key.ToString() == "Oem7")
                {
                    lblTxt.Text += "'";
                }
                else if(key.ToString() == "OemOpenBrackets")
                {
                    lblTxt.Text += "[";
                }
                else if(key.ToString() == "OemQuestion")
                {
                    lblTxt.Text += "/";
                }
                else if(key.ToString() == "Oemcomma")
                {
                    lblTxt.Text += ",";
                }
                else if(key.ToString() == "Oemtilde")
                {
                    lblTxt.Text += "`";
                }
                else if(key.ToString() == "Oemperiod")
                {
                    lblTxt.Text += ".";
                }
                else if(key.ToString() == "OemMinus")
                {
                    lblTxt.Text += "-";
                }
                else if(key.ToString() == "Oemplus")
                {
                    lblTxt.Text += "+";
                }
                else if(key.ToString() == "Capital")
                {
                    lblTxt.Text += "[Caps]";
                }
                else if(key.ToString() == "Tab")
                {
                    lblTxt.Text += "[⇄]";
                }
                else if(key.ToString() == "Win")
                {
                    lblTxt.Text += "[⇄]";
                }
                else if(key.ToString() == "Escape")
                {
                    lblTxt.Text += "[Esc]";
                }
                else if(key.ToString() == "PrintScreen")
                {
                    lblTxt.Text += "[PrtSc]";
                }
                else if(key.ToString() == "Scroll")
                {
                    lblTxt.Text += "[Scrl]";
                }
                else if(key.ToString() == "Insert")
                {
                    lblTxt.Text += "⏸";
                }
                else if(key.ToString() == "Home")
                {
                    lblTxt.Text += "[🏚]";
                }
                else if(key.ToString() == "PageUp")
                {
                    lblTxt.Text += "[PgUp]";
                }
                else if(key.ToString() == "Delete")
                {
                    lblTxt.Text += "[Del]";
                }
                else if(key.ToString() == "Next")
                {
                    lblTxt.Text += "[PgDn]";
                }
                else if(key.ToString() == "End")
                {
                    lblTxt.Text += "[⌀]";
                }
                else if (key.ToString() == "Up")
                {
                    lblTxt.Text += "⇧";
                }
                else if (key.ToString() == "Right")
                {
                    lblTxt.Text += "⇨";
                }
                else if (key.ToString() == "Down")
                {
                    lblTxt.Text += "⇩";
                }
                else if (key.ToString() == "Left")
                {
                    lblTxt.Text += "⇦";
                }
                else if (key.ToString() == "D0")
                {
                    lblTxt.Text += "0";
                }
                else if (key.ToString() == "D1")
                {
                    lblTxt.Text += "1";
                }
                else if (key.ToString() == "D2")
                {
                    lblTxt.Text += "2";
                }
                else if (key.ToString() == "D3")
                {
                    lblTxt.Text += "3";
                }
                else if (key.ToString() == "D4")
                {
                    lblTxt.Text += "4";
                }
                else if (key.ToString() == "D5")
                {
                    lblTxt.Text += "5";
                }
                else if (key.ToString() == "D6")
                {
                    lblTxt.Text += "6";
                }
                else if (key.ToString() == "D7")
                {
                    lblTxt.Text += "7";
                }
                else if (key.ToString() == "D8")
                {
                    lblTxt.Text += "8";
                }
                else if (key.ToString() == "D9")
                {
                    lblTxt.Text += "9";
                }
                else if (key.ToString() == "Space")
                {
                    lblTxt.Text += "[_]";
                }
                else
                {
                    lblTxt.Text += key.ToString().ToLower();
                }
                stopwatch.Restart();
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn,
            IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
