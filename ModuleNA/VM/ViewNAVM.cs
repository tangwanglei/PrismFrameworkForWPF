﻿using App.Infrastucture;
using Prism.Commands;
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
    public class ViewNAVM : AppBase
    {
        private string _textValue = "This is ViewNA" + DateTime.Now.ToString();
        public string TextValue
        {
            get { return _textValue; }
            set
            {
                _textValue = value;
                RaisePropertyChanged("TextValue");
            }
        }

        public DelegateCommand CloseView { get; private set; }

        public ViewNAVM(IRegionManager _regionManager, IEventAggregator _eventAggregator) :
            base(_regionManager, _eventAggregator)
        {
            _textValue += "  " + this.GetHashCode().ToString();
            CloseView = new DelegateCommand(DoCloseView);
        }

        private void DoCloseView()
        {
            this.ViewClose("ContentRegion");
        }

    }
}
