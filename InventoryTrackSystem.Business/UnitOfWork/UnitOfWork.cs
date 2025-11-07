using InventoryTrackSystem.Data.Abstract;

namespace InventoryTrackSystem.Business.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IRepository repository)
        {
            Repository = repository;
        }
        public IRepository Repository { get; }

    }
}
