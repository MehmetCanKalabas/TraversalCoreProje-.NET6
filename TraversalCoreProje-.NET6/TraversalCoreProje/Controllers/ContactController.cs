using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using DTOLayer.DTOs.ContactDTOs;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.Controllers
{
    [AllowAnonymous]
    public class ContactController : Controller
    {
        private readonly IContactUsService _IContactUsService;
        private readonly IMapper _mapper;

        public ContactController(IContactUsService ıContactUsService, IMapper mapper)
        {
            _IContactUsService = ıContactUsService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(SendMessageDto p)
        {
            if (ModelState.IsValid)
            {
                _IContactUsService.TAdd(new ContactUs()
                {
                    MessageBody = p.MessageBody,
                    Mail = p.Mail,
                    MessageStatus = true,
                    Name = p.Name,
                    Subject = p.Subject,
                    MessageDate = Convert.ToDateTime(DateTime.Now.ToShortDateString())
                });
                return RedirectToAction("Index","Default");
            }
            return View();
        }
    }
}
