using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleNA
{
    public class ViewNAVM : BindableBase, INavigationAware
    {
        private string _textValue = "This is ViewNA";
        public string TextValue
        {
            get { return _textValue; }
            set
            {
                _textValue = value;
                RaisePropertyChanged("TextValue");
            }
        }

        public ViewNAVM()
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var _journal = navigationContext.NavigationService.Journal;
            //_journal.GoBack();
            //_journal.GoForward();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
            //throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }
    }
}
