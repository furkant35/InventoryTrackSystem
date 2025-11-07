using InventoryTrackSystem.Core.Common;
using InventoryTrackSystem.Model.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryTrackSystem.Business.Interfaces
{
    public interface IProductService
    {
        Task<ResponseModel<List<ProductGetDto>>> GetAllAsync(bool asNoTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseModel<ProductGetDto>> GetByIdAsync(object id, bool asNoTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseModel<ProductGetDto>> AddAsync(ProductCreateDto entity);
        Task<ResponseModel<ProductGetDto>> UpdateAsync(ProductUpdateDto entity);
        Task<ResponseModel> DeleteAsync(object id);
    }
}
