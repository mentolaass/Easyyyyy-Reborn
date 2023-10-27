using System;
using System.Threading;

namespace Easyyyyy_Reborn.Core
{
    class MouseCore : NativeMethods
    {
        # region Executable Methods
        private void ExecuteCommonClick(bool ClickIsLeftButton, bool ClickIsDouble)
        {
            App.TotalClicks += ClickIsDouble ? 2 : 1;

            for (int x = 0; x != (ClickIsDouble ? 2 : 1); x++)
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
            bool IsEnabledRandom = App.ApplicationConfiguration == null ? false : App.ApplicationConfiguration.IsEnabledRandom;
            bool IsLeftClick = App.ApplicationConfiguration == null ? false : App.ApplicationConfiguration.IsLeftClick;
            bool IsDefaultClicks = App.ApplicationConfiguration == null ? true : !App.ApplicationConfiguration.IsDefaultClicks;
            int CountClicksPerSecond = App.ApplicationConfiguration == null ? 7 : App.ApplicationConfiguration.CountClicksPerSecond;

            ExecuteCommonClick(IsLeftClick, IsDefaultClicks);

            Thread.Sleep(IsEnabledRandom ? 1000 / new Random().Next(CountClicksPerSecond - ((CountClicksPerSecond / 100) * 20), CountClicksPerSecond) : 1000 / CountClicksPerSecond);
        }

        private void LoopHandleKeyState()
        {
            for (; ; )
            {
                bool IsToggleMode = App.ApplicationConfiguration == null ? false : App.ApplicationConfiguration.IsToggleMode;
                int IntegerBindKey = App.ApplicationConfiguration == null ? 0 : App.ApplicationConfiguration.IntBindKey;

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

                Thread.Sleep(1);
            }
        }
    }
}
