using System;
using System.Threading;
using System.Windows;

namespace Easyyyyy_Reborn.Core
{
    class MouseCore : NativeMethods
    {
        # region Executable Methods
        private void ExecuteCommonClick(bool ClickIsLeftButton, bool ClickIsDouble)
        {
            int ExecuteClicks = 0;
            if (ClickIsDouble) ExecuteClicks = 2;
            else ExecuteClicks = 1;

            App.TotalClicks += ExecuteClicks;

            for (int x = 0; x != ExecuteClicks; x++)
            {
                if (ClickIsLeftButton)
                {
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN | MouseEvent.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

                    // continue to next iteraction
                    continue;
                }

                mouse_event(MouseEvent.MOUSEEVENTF_RIGHTDOWN | MouseEvent.MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            }
        }

        public void StartLoops()
        {
            new Thread(LoopHandleKeyState).Start();
        }
        #endregion Executable Methods

        private void ExecuteBindedClick()
        {
            bool IsEnabledRandom = App.ApplicationConfiguration.IsEnabledRandom;
            bool IsLeftClick = App.ApplicationConfiguration.IsLeftClick;
            bool IsDefaultClicks = !App.ApplicationConfiguration.IsDefaultClicks;
            int CountClicksPerSecond = App.ApplicationConfiguration.CountClicksPerSecond;

            ExecuteCommonClick(IsLeftClick, IsDefaultClicks);

            int toWait = 0;
            if (IsEnabledRandom)
                toWait = 1000 / new Random().Next(CountClicksPerSecond - (CountClicksPerSecond / 100 * 50), CountClicksPerSecond);
            else 
                toWait = 1000 / CountClicksPerSecond;

            Thread.Sleep(toWait);
        }

        private void LoopHandleKeyState()
        {
            var spin = new SpinWait();

            for (; ; )
            {
                bool IsToggleMode = App.ApplicationConfiguration.IsToggleMode;
                int IntegerBindKey = App.ApplicationConfiguration.IntBindKey;

                // HOLD MODE
                if (IntegerBindKey != 0 && !IsToggleMode)
                {
                    if (GetAsyncKeyState(IntegerBindKey))
                    {
                        ExecuteBindedClick();
                        if (!App.GlobalIsWorking) App.GlobalIsWorking = true;
                    }
                    else App.GlobalIsWorking = false;
                }// TOGGLE MODE
                else if (IntegerBindKey != 0 && IsToggleMode)
                {
                    if (App.GlobalIsWorking)
                    {
                        ExecuteBindedClick();
                    }
                }

                if (!App.ApplicationIsWorking) break;

                spin.SpinOnce();
            }
        }
    }
}
