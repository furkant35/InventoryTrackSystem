using InventoryTrackSystem.Business.Interfaces;
using InventoryTrackSystem.Model.Dtos.Brand;
using InventoryTrackSystem.Model.Dtos.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTrackSystem.WebUI.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBrandService _brandService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger, IBrandService brandService)
        {
            _productService = productService;
            _logger = logger;
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var result = await _productService.GetAllAsync();

            if (!result.IsSuccess)
            {
                ViewBag.ErrorMessage = result.Message;
                return View(new List<ProductGetDto>());
            }

            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var brands = await _brandService.GetAllAsync();
            ViewBag.Brands = brands.Data ?? new List<BrandGetDto>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                var brands = await _brandService.GetAllAsync();
                ViewBag.Brands = brands.Data ?? new List<BrandGetDto>();
                return View(model);
            }

            var result = await _productService.AddAsync(model);
            if (!result.IsSuccess)
            {
                ViewBag.ErrorMessage = result.Message;
                return View(model);
            }

            TempData["Success"] = "Ürün başarıyla eklendi!";
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (!product.IsSuccess || product.Data == null)
            {
                TempData["ErrorMessage"] = "Ürün bulunamadı.";
                return RedirectToAction(nameof(List));
            }

            var brands = await _brandService.GetAllAsync();
            ViewBag.Brands = brands.Data ?? new List<BrandGetDto>();

            return View(product.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var brands = await _brandService.GetAllAsync();
                ViewBag.Brands = brands.Data ?? new List<BrandGetDto>();
                return View(dto);
            }

            var result = await _productService.UpdateAsync(dto);
            if (!result.IsSuccess)
            {
                ViewBag.ErrorMessage = result.Message;
                return View(dto);
            }

            TempData["Success"] = "Ürün başarıyla güncellendi!";
            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _productService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
            }
            else
            {
                TempData["Success"] = "Ürün başarıyla silindi.";
            }

            return RedirectToAction(nameof(List));
        }
    }
}
