using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ContactDTOs;
using MultiShop.WebUI.Services;

namespace MultiShop.WebUI.Controllers
{
    public class ContactController : Controller
    {
        private readonly IHttpService _httpService;
        public ContactController(IHttpService httpService)
        {
            _httpService = httpService;
            _httpService.setUrl("CatalogApi");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> Index(CreateContactDto createContactDto)
        {
            createContactDto.SendDate = DateTime.Now;
            createContactDto.IsRead = false;
            var create = await _httpService.Create<CreateContactDto>("Contacts", createContactDto);
            if (create) return RedirectToAction("Index", "Default");
            return View();
        }



    }
}
