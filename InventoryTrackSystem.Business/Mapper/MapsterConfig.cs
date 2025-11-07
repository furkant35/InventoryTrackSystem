using InventoryTrackSystem.Model.Concrete;
using InventoryTrackSystem.Model.Dtos.Brand;
using InventoryTrackSystem.Model.Dtos.Product;
using InventoryTrackSystem.Model.Dtos.User;
using Mapster;

namespace InventoryTrackSystem.Business.Mapper
{
    public class MapsterConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.Default.MaxDepth(2);

            // ---------------- USER ----------------

            config.NewConfig<UserCreateDto, User>()
                  .Ignore(dest => dest.Id)
                  .Map(dest => dest.Name, src => src.Name.Trim())
                  .Map(dest => dest.Surname, src => src.Surname.Trim())
                  .Map(dest => dest.Email, src => src.Email.Trim().ToLower())
                  .Map(dest => dest.Phone, src => src.Phone)
                  .Map(dest => dest.IsActive, src => true);

            config.NewConfig<UserUpdateDto, User>()
                  .IgnoreNullValues(true)
                  .Map(dest => dest.Name, src => src.Name)
                  .Map(dest => dest.Surname, src => src.Surname)
                  .Map(dest => dest.Email, src => src.Email)
                  .Map(dest => dest.Phone, src => src.Phone);

            config.NewConfig<User, UserGetDto>()
                  .Map(dest => dest.Id, src => src.Id)
                  .Map(dest => dest.Name, src => src.Name)
                  .Map(dest => dest.Surname, src => src.Surname)
                  .Map(dest => dest.Email, src => src.Email)
                  .Map(dest => dest.Phone, src => src.Phone)
                  .Map(dest => dest.IsActive, src => src.IsActive)
                    .Map(dest => dest.CreatedDate, src => src.CreatedDate)
                    .Map(dest => dest.UpdatedDate, src => src.UpdatedDate);


            config.NewConfig<ProductCreateDto,Product>()
                .Ignore(dest => dest.Id)
                    .Map(dest => dest.Name, src => src.Name.Trim())
                    .Map(dest => dest.Description, src => src.Description.Trim())
                    .Map(dest => dest.Code, src => src.Code.Trim())
                    .Map(dest => dest.Price, src => src.Price)
                    .Map(dest => dest.Stock, src => src.Stock)
                    .Map(dest => dest.BrandId, src => src.BrandId);

            config.NewConfig<ProductUpdateDto, Product>()
                .IgnoreNullValues(true)
                    .Map(dest => dest.Name, src => src.Name)
                    .Map(dest => dest.Description, src => src.Description)
                    .Map(dest => dest.Code, src => src.Code)
                    .Map(dest => dest.Price, src => src.Price)
                    .Map(dest => dest.Stock, src => src.Stock)
                    .Map(dest => dest.BrandId, src => src.BrandId);


            config.NewConfig<Product, ProductGetDto>()
                .Map(dest => dest.Id, src => src.Id)
                    .Map(dest => dest.Name, src => src.Name)
                    .Map(dest => dest.Description, src => src.Description)
                    .Map(dest => dest.Code, src => src.Code)
                    .Map(dest => dest.Price, src => src.Price)
                    .Map(dest => dest.Stock, src => src.Stock)
                    .Map(dest => dest.BrandId, src => src.BrandId)
                   .Map(dest => dest.BrandName, src => src.Brand != null ? src.Brand.Name : string.Empty)
                    .Map(dest => dest.CreatedDate, src => src.CreatedDate)
                    .Map(dest => dest.UpdatedDate, src => src.UpdatedDate);

            config.NewConfig<BrandCreateDto, Brand>()
                .Ignore(dest => dest.Id)
                    .Map(dest => dest.Name, src => src.Name.Trim())
                    .Map(dest => dest.Desc, src => src.Desc.Trim());

            config.NewConfig<BrandUpdateDto, Brand>()
                .IgnoreNullValues(true)
                    .Map(dest => dest.Name, src => src.Name)
                    .Map(dest => dest.Desc, src => src.Desc);

            config.NewConfig<Brand , BrandGetDto>()
                .Map(dest => dest.Id, src => src.Id)
                    .Map(dest => dest.Name, src => src.Name)
                    .Map(dest => dest.Desc, src => src.Desc)
                    .Map(dest => dest.CreatedDate, src => src.CreatedDate)
                    .Map(dest => dest.UpdatedDate, src => src.UpdatedDate);

        }
    }
}
