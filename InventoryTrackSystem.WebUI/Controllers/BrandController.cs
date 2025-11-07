using InventoryTrackSystem.Business.Interfaces;
using InventoryTrackSystem.Model.Dtos.Brand;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTrackSystem.WebUI.Controllers
{
    [Authorize]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly ILogger<BrandController> _logger;

        public BrandController(ILogger<BrandController> logger, IBrandService brandService)
        {
            _logger = logger;
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var result = await _brandService.GetAllAsync();

            if (!result.IsSuccess)
            {
                ViewBag.ErrorMessage = result.Message;
                return View(new List<BrandGetDto>());
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
        public async Task<IActionResult> Create(BrandCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                var brands = await _brandService.GetAllAsync();
                ViewBag.Brands = brands.Data ?? new List<BrandGetDto>();
                return View(model);
            }

            var result = await _brandService.AddAsync(model);
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
            var result = await _brandService.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("List");
            }

            var dto = new BrandUpdateDto
            {
                Id = result.Data.Id,
                Name = result.Data.Name,
                Desc = result.Data.Desc,
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _brandService.UpdateAsync(dto);

            TempData[result.IsSuccess ? "Success" : "ErrorMessage"] = result.Message;
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _brandService.DeleteAsync(id);

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
