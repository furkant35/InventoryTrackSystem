using InventoryTrackSystem.Data.Abstract;

namespace InventoryTrackSystem.Business.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository Repository { get; }
    }
}
