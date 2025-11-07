using InventoryTrackSystem.Business.UnitOfWork;
using InventoryTrackSystem.Core.Common;
using InventoryTrackSystem.Core.Enums;
using InventoryTrackSystem.Core.Utilities.Constants;

namespace InventoryTrackSystem.Business.Abstractions
{
    public abstract class BaseService
    {
        private readonly IUnitOfWork _unitOfWork;

        protected BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected async Task<ResponseModel<TEntity>> SaveAsync<TEntity>(TEntity entity) where TEntity : class
        {
            var result = new ResponseModel<TEntity>();
            try
            {
                if (await _unitOfWork.Repository.CompleteAsync() > 0)
                    result = new ResponseModel<TEntity>(true, StatusCode.Ok, entity);
                result.Message = Messages.DataSavedSuccessfully;
            }
            catch (Exception ex)
            {
                result.Message = ex.GetBaseException().Message;
            }
            return result;
        }


        protected async Task<ResponseModel> SaveAsync()
        {
            var result = new ResponseModel();
            try
            {
                if (await _unitOfWork.Repository.CompleteAsync() > 0)
                    result.IsSuccess = true;
                result.StatusCode = StatusCode.Ok;
                result.Message = Messages.DataSavedSuccessfully;
            }
            catch (Exception ex)
            {
                result.Message = ex.GetBaseException().Message;
            }
            return result;
        }
    }
}
