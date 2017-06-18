/**********************************************
 *               Extreme AltF4                *
 *            (C) 2017 Marcel Bulla           *
 * https://github.com/markellus/Extreme-AltF4 *
 *  See file LICENSE for license information  *
 **********************************************/

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static AltF4.NativeMethods;

namespace AltF4
{
    internal class AltF4Handler
    {
        public static AltF4Handler Get()
        {
            return _instance;
        }

        private static AltF4Handler _instance = new AltF4Handler();

        private static SafeList<Keys> _listKeys;

        private static IntPtr _hookID = IntPtr.Zero;

        private bool _fired;

        private LowLevelKeyboardProc _callback;

        public event EventHandler OnAltF4;

        private AltF4Handler()
        {
            _listKeys = new SafeList<Keys>();
            _callback = HookCallback;
            _hookID = SetHook(_callback);
            _fired = false;
        }

        ~AltF4Handler()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                        GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }

            int vkCode = Marshal.ReadInt32(lParam);
            Keys code = (Keys)vkCode;

            if (KeyDown(wParam))
            {
                _listKeys.SafeAdd(code);
            }
            else if (KeyUp(wParam))
            {
                _listKeys.SafeRemove(code);
            }

            if (_listKeys.Contains(Keys.F4) &&
                (_listKeys.Contains(Keys.LMenu) || _listKeys.Contains(Keys.Alt)))
            {
                if(!_fired)
                {
                    _fired = true;
                    OnAltF4?.Invoke(this, EventArgs.Empty);
                }
                return (IntPtr)1;
            }
            else
            {
                _fired = false;
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private bool KeyDown(IntPtr wParam)
        {
            return (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN);
        }

        private bool KeyUp(IntPtr wParam)
        {
            return (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP);
        }
    }
}
