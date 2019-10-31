using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace WindowsApp
{
    public class MainWindowVM : BindableBase
    {
        private IRegionManager _regionManager;

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

        public MainWindowVM(IRegionManager regionManager)
        {
            //ExecuteDelegateCommand = new DelegateCommand(Execute, CanExecute);

            //DelegateCommandObservesProperty = new DelegateCommand(Execute, CanExecute)
            //    .ObservesProperty(() => IsEnabled);

            //DelegateCommandObservesCanExecute = new DelegateCommand(Execute)
            //    .ObservesCanExecute(() => IsEnabled);

            //ExecuteGenericDelegateCommand = new DelegateCommand<string>(ExecuteGeneric)
            //    .ObservesCanExecute(() => IsEnabled);

            _regionManager = regionManager;

            NavigateCommand = new DelegateCommand<string>(Navigate);
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
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }
    }
}
