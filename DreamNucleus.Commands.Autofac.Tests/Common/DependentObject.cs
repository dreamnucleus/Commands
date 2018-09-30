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
