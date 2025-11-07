using Business.Services.Base;
using InventoryTrackSystem.Business.Interfaces;
using InventoryTrackSystem.Business.UnitOfWork;
using InventoryTrackSystem.Core.Common;
using InventoryTrackSystem.Core.Enums;
using InventoryTrackSystem.Model.Concrete;
using InventoryTrackSystem.Model.Dtos.Product;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventoryTrackSystem.Business.Services
{
    public class ProductService
      : CrudServiceBase<Product, long, ProductCreateDto, ProductUpdateDto, ProductGetDto>,
        IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, TypeAdapterConfig config)
            : base(unitOfWork, mapper, config) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        protected override long ReadKey(Product e) => e.Id;
        protected override Expression<Func<Product, bool>> KeyPredicate(long id) => x => x.Id == id;

        protected override Task<Product?> ResolveEntityForUpdateAsync(ProductUpdateDto dto)
            => _unitOfWork.Repository.GetSingleAsync<Product>(false, x => x.Id == dto.Id,
                   q => q.Include(p => p.Brand));

        public async Task<ResponseModel<List<ProductGetDto>>> GetAllAsync(bool asNoTracking = false, CancellationToken cancellationToken = default)
        {

            var list = await _unitOfWork.Repository
           .GetMultipleAsync<Product>(
               asNoTracking: true,
               includeExpression: q => q.Include(p => p.Brand),
               cancellationToken: cancellationToken
           );


            var dtoList = _mapper.Map<List<ProductGetDto>>(list);

            return new ResponseModel<List<ProductGetDto>>
            {
                Data = dtoList,
                IsSuccess = list != null && list.Count > 0,
                StatusCode = (list != null && list.Count > 0) ? StatusCode.Ok : StatusCode.NotFound,
                Message = (list != null && list.Count > 0) ? "Kayıtlar listelendi" : "Kayıt bulunamadı"
            };
        }

        public async Task<ResponseModel<ProductGetDto>> GetByIdAsync(object id, bool asNoTracking = false, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.Repository.GetByIdAsync<Product>(asNoTracking, id, cancellationToken);
            var configurationGetDto = _mapper.Map<ProductGetDto>(entity);
            return new ResponseModel<ProductGetDto>
            {
                Data = configurationGetDto,
                IsSuccess = entity != null,
                StatusCode = entity != null ? StatusCode.Ok : StatusCode.NotFound,
                Message = entity != null ? "Kayıtlar listelendi" : "Kayıt bulunamadı"
            };
        }

        public async Task<ResponseModel<ProductGetDto>> AddAsync(ProductCreateDto entity)
        {
            var configuration = _mapper.Map<Product>(entity);
            await _unitOfWork.Repository.AddAsync(configuration);
            //var result = await SaveAsync(configuration);
            var result = await SaveAsync();
            var configurationGetDto = _mapper.Map<ProductGetDto>(configuration);
            return new ResponseModel<ProductGetDto>
            {
                Data = configurationGetDto,
                IsSuccess = result.IsSuccess,
                StatusCode = result.StatusCode,
                Message = result.Message
            };
        }

        public async Task<ResponseModel> DeleteAsync(object id)
        {
            var isThere = await _unitOfWork.Repository.AnyAsync<Product>(b => b.Id == (long)id);
            if (!isThere)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    StatusCode = StatusCode.NotFound,
                    Message = "Kayıt bulunamadı"
                };
            }
            await _unitOfWork.Repository.HardDeleteAsync<Product>(id);
            var result = await SaveAsync();
            return new ResponseModel
            {
                IsSuccess = result.IsSuccess,
                StatusCode = result.StatusCode,
                Message = result.Message
            };
        }
    }
}
