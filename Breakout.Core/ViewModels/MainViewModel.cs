using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Breakout.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private string _hello = "Hello MvvmCross";
        public string Hello
        {
            get { return _hello; }
            set { _hello = value; RaisePropertyChanged(() => Hello); }
        }
    }
}
