using System.Runtime.InteropServices;

namespace Easyyyyy_Reborn.Core
{
    public class NativeMethods
    {
        [DllImport("User32.dll")]
        public static extern bool GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        public static extern void mouse_event(MouseEvent dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public enum MouseEvent
        {
            MOUSEEVENTF_LEFTDOWN = 0x02,
            MOUSEEVENTF_LEFTUP = 0x04,
            MOUSEEVENTF_RIGHTDOWN = 0x08,
            MOUSEEVENTF_RIGHTUP = 0x10,
        }
    }
}
