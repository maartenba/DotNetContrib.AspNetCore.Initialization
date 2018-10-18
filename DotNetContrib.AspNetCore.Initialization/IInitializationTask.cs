using System.Threading.Tasks;

namespace DotNetContrib.AspNetCore.Initialization
{
    public interface IInitializationTask
    {
        Task InitializeAsync(InitializationContext context);
    }
}