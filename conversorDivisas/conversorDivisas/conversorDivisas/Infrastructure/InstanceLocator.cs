using conversorDivisas.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace conversorDivisas.Infrastructure
{
    class InstanceLocator
    {
        public MainViewModel Main { get; set; }
        public InstanceLocator()
        {
            Main = new MainViewModel();
        }
    }
}
