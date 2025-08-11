using System.Reflection;

namespace Borrowing.Application
{
    public class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}
