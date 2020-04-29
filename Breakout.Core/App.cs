using MvvmCross.IoC;
using MvvmCross.ViewModels;
using System;

namespace Breakout.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<ViewModels.MainViewModel>();
            // if you want to use a custom AppStart, you should replace the previous line with this one:
            // RegisterCustomAppStart<MyCustomAppStart>();
        }
    }
}
