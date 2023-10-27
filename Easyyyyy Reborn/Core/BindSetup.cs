using System.Reflection;
using System;
using System.Windows.Input;

namespace Easyyyyy_Reborn.Core
{
    class BindSetup
    {
        public object[] GetData(KeyEventArgs e)
        {
            return new object[] { KeyInterop.VirtualKeyFromKey(e.Key), e.Key.ToString() };
        }

        public object[] GetMouseData(MouseButtonEventArgs e)
        {
            switch (e.ChangedButton)
            {
                case MouseButton.Middle:
                    return new object[] { 0x04, "MMiddle" };
                case MouseButton.XButton1:
                    return new object[] { 0x05, "XBtn1" };
                case MouseButton.XButton2:
                    return new object[] { 0x06, "XBtn2" };
            }

            return new object[] { };
        }
    }
}
