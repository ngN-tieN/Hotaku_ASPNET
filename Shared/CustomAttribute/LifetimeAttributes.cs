namespace Hotaku.Shared.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ScopedAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TransientAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class SingletonAttribute : Attribute
    {
    }
}
