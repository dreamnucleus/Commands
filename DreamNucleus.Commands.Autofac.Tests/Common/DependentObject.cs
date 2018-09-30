using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Autofac.Tests.Common
{
    public class DependentObject
    {
        public DependencyObject DependencyObject { get; }

        public DependentObject(DependencyObject dependencyObject)
        {
            DependencyObject = dependencyObject;
        }
    }
}
