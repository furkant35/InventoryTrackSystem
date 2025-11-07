using Business.Services.Base;
using InventoryTrackSystem.Business.Interfaces;
using InventoryTrackSystem.Business.UnitOfWork;
using InventoryTrackSystem.Core.Common;
using InventoryTrackSystem.Core.Enums;
using InventoryTrackSystem.Model.Concrete;
using InventoryTrackSystem.Model.Dtos.Brand;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventoryTrackSystem.Business.Services
{
    public class BrandService
      : CrudServiceBase<Brand, long, BrandCreateDto, BrandUpdateDto, BrandGetDto>,
        IBrandService
    {
        public BrandService(IUnitOfWork uow, IMapper mapper, TypeAdapterConfig config)
            : base(uow, mapper, config) { }

        protected override long ReadKey(Brand e) => e.Id;
        protected override Expression<Func<Brand, bool>> KeyPredicate(long id) => b => b.Id == id;

        protected override async Task<Brand?> ResolveEntityForUpdateAsync(BrandUpdateDto dto)
        {
            if (dto.Id <= 0) return null;
            var entity = await _unitOfWork.Repository.GetByIdAsync<Brand>(
                asNoTracking: false,
                id: dto.Id
            );

            if (entity != null) return entity;
            else return null;
        }

        public async Task<ResponseModel<List<BrandGetDto>>> GetAllAsync(bool asNoTracking = false, CancellationToken cancellationToken = default)
        {

            var list = await _unitOfWork.Repository.GetMultipleAsync<Brand>(asNoTracking, cancellationToken: cancellationToken);

            var dtoList = _mapper.Map<List<BrandGetDto>>(list);

            return new ResponseModel<List<BrandGetDto>>
            {
                Data = dtoList,
                IsSuccess = list != null && list.Count > 0,
                StatusCode = (list != null && list.Count > 0) ? StatusCode.Ok : StatusCode.NotFound,
                Message = (list != null && list.Count > 0) ? "Kayıtlar listelendi" : "Kayıt bulunamadı"
            };
        }

        public async Task<ResponseModel<BrandGetDto>> GetByIdAsync(object id, bool asNoTracking = false, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.Repository.GetByIdAsync<Brand>(asNoTracking, id, cancellationToken);
            var configurationGetDto = _mapper.Map<BrandGetDto>(entity);
            return new ResponseModel<BrandGetDto>
            {
                Data = configurationGetDto,
                IsSuccess = entity != null,
                StatusCode = entity != null ? StatusCode.Ok : StatusCode.NotFound,
                Message = entity != null ? "Kayıtlar listelendi" : "Kayıt bulunamadı"
            };
        }

        public async Task<ResponseModel<BrandGetDto>> AddAsync(BrandCreateDto entity)
        {
            var configuration = _mapper.Map<Brand>(entity);
            await _unitOfWork.Repository.AddAsync(configuration);
            //var result = await SaveAsync(configuration);
            var result = await SaveAsync();
            var configurationGetDto = _mapper.Map<BrandGetDto>(configuration);
            return new ResponseModel<BrandGetDto>
            {
                Data = configurationGetDto,
                IsSuccess = result.IsSuccess,
                StatusCode = result.StatusCode,
                Message = result.Message
            };
        }

        public async Task<ResponseModel> DeleteAsync(object id)
        {
            var isThere = await _unitOfWork.Repository.AnyAsync<Brand>(b => b.Id == (long)id);
            if (!isThere)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    StatusCode = StatusCode.NotFound,
                    Message = "Kayıt bulunamadı."
                };
            }

            await _unitOfWork.Repository.HardDeleteAsync<Brand>(id);

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
