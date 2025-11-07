using InventoryTrackSystem.Core.Common;
using InventoryTrackSystem.Model.Dtos.Brand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryTrackSystem.Business.Interfaces
{
    public interface IBrandService
    {
        Task<ResponseModel<List<BrandGetDto>>> GetAllAsync(bool asNoTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseModel<BrandGetDto>> GetByIdAsync(object id, bool asNoTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseModel<BrandGetDto>> AddAsync(BrandCreateDto entity);
        Task<ResponseModel<BrandGetDto>> UpdateAsync(BrandUpdateDto entity);
        Task<ResponseModel> DeleteAsync(object id);
    }
}
