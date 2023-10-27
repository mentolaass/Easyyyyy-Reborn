using Easyyyyy_Reborn.Core;
using Easyyyyy_Reborn.Models;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Easyyyyy_Reborn.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        #pragma warning disable
        private int _CurrentClicks;
        public int CurrentClicks
        {
            get => _CurrentClicks;
            set
            {
                _CurrentClicks = value;
                RaisePropertyChanged(nameof(CurrentClicks));
            }
        }

        private int _CountClicksPerSecond = App.ApplicationConfiguration.CountClicksPerSecond;
        public int CountClicksPerSecond
        {
            get => _CountClicksPerSecond;
            set
            {
                if (value > 0)
                {
                    _CountClicksPerSecond = value;
                    RaisePropertyChanged(nameof(CountClicksPerSecond));

                    // update config
                    App.ApplicationConfiguration.CountClicksPerSecond = value;
                    App.ImplementUpdateConfiguration();
                }
            }
        }

        private bool _IsLeftClick = App.ApplicationConfiguration.IsLeftClick;
        public bool IsLeftClick
        {
            get => _IsLeftClick;
            set
            {
                _IsLeftClick = value;
                RaisePropertyChanged(nameof(IsLeftClick));

                // update config
                App.ApplicationConfiguration.IsLeftClick = value;
                App.ImplementUpdateConfiguration();
            }
        }

        private bool _IsWorking;
        public bool IsWorking
        {
            get => _IsWorking;
            set
            {
                _IsWorking = value;
                RaisePropertyChanged(nameof(IsWorking));

                // change text status
                Status = value ? "ON" : "OFF";
            }
        }

        private bool _IsStateBinding;
        public bool IsStateBinding
        {
            get => _IsStateBinding;
            set
            {
                _IsStateBinding = value;
                RaisePropertyChanged(nameof(IsStateBinding));
            }
        }

        private bool _IsRandom = App.ApplicationConfiguration.IsEnabledRandom;
        public bool IsRandom
        {
            get => _IsRandom;
            set
            {
                _IsRandom = value;
                RaisePropertyChanged(nameof(IsRandom));

                // update config
                App.ApplicationConfiguration.IsEnabledRandom = value;
                App.ImplementUpdateConfiguration();
            }
        }

        private bool _IsDefaultClick = App.ApplicationConfiguration.IsDefaultClicks;
        public bool IsDefaultClick
        {
            get => _IsDefaultClick;
            set
            {
                _IsDefaultClick = value;
                RaisePropertyChanged(nameof(IsDefaultClick));

                // update config
                App.ApplicationConfiguration.IsDefaultClicks = value;
                App.ImplementUpdateConfiguration();
            }
        }

        private bool _IsToggleMode = App.ApplicationConfiguration.IsToggleMode;
        public bool IsToggleMode
        {
            get => _IsToggleMode;
            set
            {
                _IsToggleMode = value;
                RaisePropertyChanged(nameof(IsToggleMode));

                // update config
                App.ApplicationConfiguration.IsToggleMode = value;
                App.ImplementUpdateConfiguration();
            }
        }

        private string _Status;
        public string Status
        {
            get => _Status;
            set
            {
                _Status = value;
                RaisePropertyChanged(nameof(Status));
            }
        }

        private string _BindKey = App.ApplicationConfiguration.BindKey;
        public string BindKey
        {
            get => _BindKey;
            set
            {
                _BindKey = value;
                RaisePropertyChanged(nameof(BindKey));

                // update config
                App.ApplicationConfiguration.BindKey = value;
                App.ImplementUpdateConfiguration();
            }
        }

        private int _IntBindKey = App.ApplicationConfiguration.IntBindKey;
        public int IntBindKey
        {
            get => _IntBindKey;
            set
            {
                _IntBindKey = value;
                RaisePropertyChanged(nameof(IntBindKey));

                // update config
                App.ApplicationConfiguration.IntBindKey = value;
                App.ImplementUpdateConfiguration();
            }
        }

        #pragma warning enable
        private MouseCore MouseCoreManager = new MouseCore();

        #region COMMANDS   
        public RelayCommand GotoGithub
        {
            get => new RelayCommand((obj) =>
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = Constants.GithubRepository,
                    UseShellExecute = true
                });
            });
        }

        public RelayCommand DragMainWindow
        {
            get => new RelayCommand((obj) =>
            {
                if (Application.Current.MainWindow != null && obj is MouseButtonEventArgs)
                {
                    var Args = (MouseButtonEventArgs)obj;

                    if (Args.LeftButton == MouseButtonState.Pressed)
                    {
                        Application.Current.MainWindow.DragMove();
                    }
                }
            });
        }

        public RelayCommand CloseMainWindow
        {
            get => new RelayCommand((obj) =>
            {
                if (Application.Current.MainWindow != null)
                {
                    Application.Current.MainWindow.Close();
                }
            });
        }

        public RelayCommand OnClosedApplication
        {
            get => new RelayCommand((obj) =>
            {
                App.ApplicationIsWorking = false;
            });
        }

        public RelayCommand SetupBindKey
        {
            get => new RelayCommand((obj) =>
            {
                new Thread(() =>
                {
                    if (obj is MouseButtonEventArgs && IsStateBinding)
                    {
                        var Data = new BindSetup().GetMouseData((MouseButtonEventArgs)obj);

                        if (Data.Length != 0)
                        {
                            BindKey = (string)Data[1];
                            IntBindKey = (int)Data[0];

                            IsStateBinding = false;

                            Thread.Sleep(500);
                        }
                    }
                    else if (obj is KeyEventArgs && IsStateBinding)
                    {
                        var Data = new BindSetup().GetData((KeyEventArgs)obj);

                        BindKey = (string)Data[1];
                        IntBindKey = (int)Data[0];

                        IsStateBinding = false;

                        Thread.Sleep(500);
                    }
                }).Start();
            });
        }

        public RelayCommand StartSetupBindKey
        {
            get => new RelayCommand((obj) =>
            {
                IsStateBinding = !IsStateBinding;
            });
        }

        public RelayCommand ToggleLeftClickParameter
        {
            get => new RelayCommand((obj) =>
            {
                IsLeftClick = !IsLeftClick;
            });
        }

        public RelayCommand ToggleDefaultClickParameter
        {
            get => new RelayCommand((obj) =>
            {
                IsDefaultClick = !IsDefaultClick;
            });
        }

        public RelayCommand ToggleRandomClicksParameter
        {
            get => new RelayCommand((obj) =>
            {
                IsRandom = !IsRandom;
            });
        }

        public RelayCommand ToggleClickModeParameter
        {
            get => new RelayCommand((obj) =>
            {
                IsToggleMode = !IsToggleMode;
            });
        }

        #endregion COMMANDS

        public MainViewModel()
        {
            MouseCoreManager.StartLoops();

            // create timers
            var UpdaterCountClicksPerSecond = new System.Timers.Timer(1000);
            UpdaterCountClicksPerSecond.Elapsed += UpdaterCountClicksPerSecond_Elapsed;
            UpdaterCountClicksPerSecond.Start();

            var UpdaterStateEasyyyy = new System.Timers.Timer(1);
            UpdaterStateEasyyyy.Elapsed += UpdaterStateEasyyyy_Elapsed;
            UpdaterStateEasyyyy.Start();

            new Thread(UpdaterToggleMode).Start();
        }

        private void UpdaterToggleMode()
        {
            new Thread(() =>
            {
                for (; ; )
                {
                    if (App.ApplicationConfiguration.IsToggleMode && NativeMethods.GetAsyncKeyState(IntBindKey))
                    {
                        App.GlobalIsWorking = !App.GlobalIsWorking;
                        Thread.Sleep(250);
                    }

                    if (!App.ApplicationIsWorking) break;

                    Thread.Sleep(5);
                }
            }).Start();
        }

        private void UpdaterStateEasyyyy_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            IsWorking = App.GlobalIsWorking;
        }

        private void UpdaterCountClicksPerSecond_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            CurrentClicks = App.TotalClicks;
            App.TotalClicks = 0;
        }
    }
}
