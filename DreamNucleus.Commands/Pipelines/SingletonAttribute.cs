using System;

namespace DreamNucleus.Commands.Pipelines
{
    /// <summary>
    /// This will ensure the pipeline is only run once per top level command execution
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SingletonAttribute : Attribute
    {
    }
}
