using App.Infrastucture;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Threading;
using System.Windows;

namespace WindowsApp.ViewModel
{
    public class MainWindowViewModel : AppBaseViewModel
    {
        private string _title = "Prism Unity Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                SetProperty(ref _isEnabled, value);
                ExecuteDelegateCommand.RaiseCanExecuteChanged();
            }
        }

        private Visibility _CreateVisibility = Visibility.Visible;
        public Visibility CreateVisibility
        {
            get { return _CreateVisibility; }
            set
            {
                SetProperty(ref _CreateVisibility, value);
            }
        }

        private Visibility _ViewVisibility = Visibility.Hidden;
        public Visibility ViewVisibility
        {
            get { return _ViewVisibility; }
            set
            {
                SetProperty(ref _ViewVisibility, value);
            }
        }

        private string _updateText;
        public string UpdateText
        {
            get { return _updateText; }
            set { SetProperty(ref _updateText, value); }
        }

        public DelegateCommand ExecuteDelegateCommand { get; private set; }

        public DelegateCommand<string> ExecuteGenericDelegateCommand { get; private set; }

        public DelegateCommand DelegateCommandObservesProperty { get; private set; }

        public DelegateCommand DelegateCommandObservesCanExecute { get; private set; }

        public DelegateCommand<string> NavigateCommand { get; private set; }


        public DelegateCommand<string> JournalAwareCommand { get; private set; }

        public MainWindowViewModel(IRegionManager _regionManager, IEventAggregator _eventAggregator) :
            base(_regionManager, _eventAggregator)
        {
            //ExecuteDelegateCommand = new DelegateCommand(Execute, CanExecute);

            //DelegateCommandObservesProperty = new DelegateCommand(Execute, CanExecute)
            //    .ObservesProperty(() => IsEnabled);

            //DelegateCommandObservesCanExecute = new DelegateCommand(Execute)
            //    .ObservesCanExecute(() => IsEnabled);

            //ExecuteGenericDelegateCommand = new DelegateCommand<string>(ExecuteGeneric)
            //    .ObservesCanExecute(() => IsEnabled);

            //_regionManager = regionManager;

            NavigateCommand = new DelegateCommand<string>(Navigate);

            JournalAwareCommand = new DelegateCommand<string>(JournalAware);
        }

        private void Execute()
        {
            UpdateText = $"Updated: {DateTime.Now}";
        }

        private void ExecuteGeneric(string parameter)
        {
            UpdateText = parameter;
        }

        private bool CanExecute()
        {
            return IsEnabled;
        }

        private void Navigate(string navigatePath)
        {
            if ("Close" == navigatePath)
            {
                RegionManager.Regions["ContentRegion"].RemoveAll();
            }
            else if (navigatePath != null)
                RegionManager.RequestNavigate("ContentRegion", navigatePath);
        }

        private void JournalAware(string goBackAndgoForward)
        {
            if (goBackAndgoForward == "GoBack")
                GoBack();
            if (goBackAndgoForward == "GoForward")
                GoForward();
        }

        public override void App_ButtonCommand(string command)
        {
            base.Async(
            () =>
                {
                    Thread.Sleep(3000);
                    return true;
                },
            () =>
                {
                    RequestNavigate(AppRegions.ContentRegion, "HomePage");
                    return true;
                });
        }
    }
}
