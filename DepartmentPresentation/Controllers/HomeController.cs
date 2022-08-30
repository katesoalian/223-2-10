using Core.Models;
using Core.Views;
using DepartmentPresentation.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using System.Diagnostics;

namespace DepartmentPresentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProfessorRepository _professorRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository, IGroupRepository groupRepository, IStudentRepository studentRepository, IProfessorRepository professorRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _professorRepository = professorRepository;
            _groupRepository = groupRepository;
            _studentRepository = studentRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Students = await _studentRepository.GetAllAsync();
            ViewBag.Professors = await _professorRepository.GetAllAsync();
            ViewBag.Groups = await _groupRepository.GetAllAsync();

            return View();
        }

        [HttpGet]
        [ActionName("LogIn")]
        public IActionResult LogIn()
        {
            ViewBag.Message = string.Empty;
            return View("~/Views/Home/LogIn.cshtml");
        }

        [HttpPost]
        [ActionName("LogIn")]
        public IActionResult LogIn(string login, string password)
        {
            List<User> users = _userRepository.GetAllAsync().Result.ToList();

            User? user = users.Find(u => u.Login == login);

            if (user is null || user.Password != password)
            {
                ViewBag.Message = "Не вірний логін або прароль";
                return View("~/Views/Home/LogIn.cshtml");
            }

            return View("~/Views/Home/AdminPanel.cshtml");
        }

        [HttpGet]
        [ActionName("Registration")]
        public IActionResult GetRegistry()
        {
            return View("~/Views/Home/Registration.cshtml");
        }

        [HttpPost]
        [ActionName("Registration")]
        public async Task<IActionResult> Registry(User user, ProfessorView professor)
        {
            try
            {
                await _professorRepository.SaveAsync(new Professor()
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = professor.FirstName,
                    LastName = professor.LastName,
                    Subject = professor.Subject
                });

                await _userRepository.SaveAsync(new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    Login = user.Login,
                    Password = user.Password
                });

                ViewBag.Message = "Користувача зареєстровано";
                return View("~/Views/Home/LogIn.cshtml");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View("~/Views/Home/LogIn.cshtml");
            }            
        }
    }
}