using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mastersign.Gate
{
    static class ProcessTerminator
    {
        // https://codetitans.pl/blog/post/sending-ctrl-c-signal-to-another-application-on-windows

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handler, bool add);

        // Delegate type to be used as the Handler Routine for SCCH
        delegate bool ConsoleCtrlDelegate(CtrlTypes type);

        // Enumerated type for the control messages sent to the handler routine
        enum CtrlTypes : uint
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            // CTRL_LOGOFF_EVENT = 5,
            // CTRL_SHUTDOWN_EVENT = 6,
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GenerateConsoleCtrlEvent(CtrlTypes dwCtrlEvent, uint dwProcessGroupId);

        public static async Task Stop(this Process process)
        {
            // It's impossible to be attached to 2 consoles at the same time,
            // so release the current one.
            FreeConsole();

            // This does not require the console window to be visible.
            if (AttachConsole((uint)process.Id))
            {
                // Disable Ctrl-C handling for our program
                SetConsoleCtrlHandler(null, true);
                GenerateConsoleCtrlEvent(CtrlTypes.CTRL_C_EVENT, 0);

                // Must wait here. If we don't and re-enable Ctrl-C
                // handling below too fast, we might terminate ourselves.
                await Task.Delay(2000);

                FreeConsole();

                // Re-enable Ctrl-C handling or any subsequently started
                // programs will inherit the disabled state.
                SetConsoleCtrlHandler(null, false);
            }
        }
    }
}
