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
            if (textNum == 50)
            {
                lblTxt.Text += Environment.NewLine;
            }
            else if (textNum >= 100)
            {
                lblTxt.Text = "";
            }
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;
                Dictionary<string, string> keyMappings = new Dictionary<string, string>
                {
                    {"LControlKey", "︽"},
                    {"RControlKey", "︽"},
                    {"LShiftKey", "⇧"},
                    {"RShiftKey", "⇧"},
                    {"LWin", "⌘"},
                    {"RWin", "⌘"},
                    {"Back", "⌫"},
                    {"Apps", "❆"},
                    {"Return", "➠"},
                    {"Oem1", ";"},
                    {"Oem5", "\\"},
                    {"Oem6", "]"},
                    {"Oem7", "'"},
                    {"OemOpenBrackets", "["},
                    {"OemQuestion", "/"},
                    {"Oemcomma", ","},
                    {"Oemtilde", "`"},
                    {"OemPeriod", "."},
                    {"OemMinus", "-"},
                    {"Oemplus", "+"},
                    {"Capital", "[Caps]"},
                    {"Tab", "[⇄]"},
                    {"Win", "[⇄]"},
                    {"Escape", "[Esc]"},
                    {"PrintScreen", "[PrtSc]"},
                    {"Scroll", "[Scrl]"},
                    {"Insert", "[Ins]"},
                    {"Home", "🏚"},
                    {"Pause", "⏸"},
                    {"PageUp", "[PgUp]"},
                    {"Delete", "[Del]"},
                    {"Next", "[PgDn]"},
                    {"End", "[⌀]"},
                    {"Up", "⇧"},
                    {"Right", "⇨"},
                    {"Down", "⇩"},
                    {"Left", "⇦"},
                    {"Space", "_"},
                // Add more keys and their mappings here
                };

                string keyString = key.ToString();
                if (keyMappings.ContainsKey(keyString))
                {
                    lblTxt.Text += keyMappings[keyString];
                }
                else if (keyString.Length == 2 && keyString[0] == 'D' && char.IsDigit(keyString[1]))
                {
                    lblTxt.Text += keyString[1];
                }
                else
                {
                    lblTxt.Text += keyString.ToLower();
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
