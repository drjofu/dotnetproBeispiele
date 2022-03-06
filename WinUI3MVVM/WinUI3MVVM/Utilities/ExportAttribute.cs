namespace WinUI3MVVM.Utilities;

/// <summary>
/// For convenience only. Classes with [Export] annotation are loaded for dependency injection
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class ExportAttribute : Attribute
{
    public bool AsSingleton { get; set; }
}


