using App.Infrastucture;
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
    public class ViewNBVM : AppBase
    {
        private string _textValue = "This is ViewNB" + DateTime.Now.ToString();
        public string TextValue
        {
            get { return _textValue; }
            set
            {
                _textValue = value;
                RaisePropertyChanged("TextValue");
            }
        }

        public ViewNBVM(IRegionManager _regionManager, IEventAggregator _eventAggregator) :
            base(_regionManager, _eventAggregator)
        {
            _textValue += "  " + this.GetHashCode().ToString();
        }
    }
}
