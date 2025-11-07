using Microsoft.Extensions.DependencyInjection;

namespace InventoryTrackSystem.Core.Utilities.IoC
{
    public interface ICoreModule
    {
        void Load(IServiceCollection collection);
    }
}
