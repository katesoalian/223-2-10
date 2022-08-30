using Core.Models;
using Core.Views;
using DepartmentPresentation.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace DepartmentPresentation.Controllers
{
    public class ProfessorController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IProfessorRepository _professorRepository;

        public ProfessorController(ILogger<HomeController> logger, IProfessorRepository professorRepository)
        {
            _logger = logger;
            _professorRepository = professorRepository;
        }

        [HttpGet]
        [ActionName("ProfessorManagement")]
        public IActionResult GetProfessorsPage()
        {
            ViewBag.Message = "";
            ViewBag.Professors = _professorRepository.GetAllAsync().Result.ToList();
            return View("~/Views/Home/Department Management/Professors/ProfessorManagement.cshtml");
        }

        [HttpGet]
        [ActionName("FindProfessor")]
        public IActionResult FindProfessorPage(string search)
        {
            if (search == null)
                search = string.Empty;

            ViewBag.Professors = _professorRepository.GetAllAsync()
                                .Result
                                .ToList()
                                .FindAll(professor => professor.FirstName.Contains(search)
                                || professor.LastName.Contains(search)
                                || professor.Subject.Contains(search));
            return View("~/Views/Home/Department Management/Professors/ProfessorManagement.cshtml");
        }

        [HttpGet]
        [ActionName("AddProfessor")]
        public IActionResult GetAddProfessorPage()
        {
            return View("~/Views/Home/Department Management/Professors/AddProfessor.cshtml");
        }

        [HttpPost]
        [ActionName("AddProfessor")]
        public async Task<IActionResult> AddProfessor(ProfessorView professor)
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

                return RedirectToAction("ProfessorManagement");
            }
            catch(Exception)
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpGet]
        [ActionName("UpdateProfessor")]
        public async Task<IActionResult> GetUpdateProfessorPage(string id)
        {
            Professor? professor = await _professorRepository.GetByIdAsync(id);

            ViewBag.Professor = professor;

            return View("~/Views/Home/Department Management/Professors/UpdateProfessor.cshtml");
        }

        [HttpPost]
        [ActionName("UpdateProfessor")]
        public async Task<IActionResult> UpdateProfessorPage(Professor professor)
        {
            await _professorRepository.UpdateAsync(professor);

            ViewBag.Professor = professor;

            return RedirectToAction("ProfessorManagement");
        }

        [HttpPost]
        [ActionName("DeleteProfessor")]
        public async Task<IActionResult> DeleteProfessor(string id)
        {            
            try
            {
                await _professorRepository.DeleteAsync(id);

                return RedirectToAction("ProfessorManagement");
            }
            catch (Exception)
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}
