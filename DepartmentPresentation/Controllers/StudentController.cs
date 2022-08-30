using Core.Models;
using Core.Views;
using DepartmentPresentation.Models;
using Logic;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace DepartmentPresentation.Controllers
{
    public class StudentController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IStudentRepository _studentRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IPointsService _pointsService;

        public StudentController(ILogger<HomeController> logger, IStudentRepository studentRepository, IGroupRepository groupRepository, IPointsService pointsService)
        {
            _logger = logger;
            _studentRepository = studentRepository;
            _groupRepository = groupRepository;
            _pointsService = pointsService;
        }

        [HttpGet]
        [ActionName("StudentManagement")]
        public IActionResult GetStudentsPage()
        {
            ViewBag.Message = "";
            ViewBag.Students = _studentRepository.GetAllAsync().Result.ToList();
            return View("~/Views/Home/Department Management/Students/StudentManagement.cshtml");
        }

        [HttpGet]
        [ActionName("FindStudent")]
        public IActionResult FindStudentPage(string search)
        {
            if (search is null)
                search = string.Empty;

            ViewBag.Students = _studentRepository.GetAllAsync()
                                .Result
                                .ToList()
                                .FindAll(student => student.FirstName.Contains(search)
                                || student.LastName.Contains(search));
            return View("~/Views/Home/Department Management/Students/StudentManagement.cshtml");
        }

        [HttpGet]
        [ActionName("AddStudent")]
        public async Task<IActionResult> GetAddStudentPage()
        {
            ViewBag.Groups = await _groupRepository.GetAllAsync();

            return View("~/Views/Home/Department Management/Students/AddStudent.cshtml");
        }

        [HttpPost]
        [ActionName("AddStudent")]
        public async Task<IActionResult> AddStudent(StudentView student)
        {
            try
            {
                await _studentRepository.SaveAsync(new Student()
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    GroupId = student.GroupId
                });

                return RedirectToAction("StudentManagement");
            }
            catch(Exception)
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpGet]
        [ActionName("UpdateStudent")]
        public async Task<IActionResult> GetUpdateStudentPage(string id)
        {
            Student? student = await _studentRepository.GetByIdAsync(id);

            ViewBag.Groups = await _groupRepository.GetAllAsync();
            ViewBag.Student = student;

            return View("~/Views/Home/Department Management/Students/UpdateStudent.cshtml");
        }

        [HttpPost]
        [ActionName("UpdateStudent")]
        public async Task<IActionResult> UpdateStudentPage(Student student)
        {
            await _studentRepository.UpdateAsync(student);

            ViewBag.Student = student;

            return RedirectToAction("StudentManagement");
        }

        [HttpPost]
        [ActionName("DeleteStudent")]
        public async Task<IActionResult> DeleteStudent(string id)
        {            
            try
            {
                await _studentRepository.DeleteAsync(id);

                return RedirectToAction("StudentManagement");
            }
            catch (Exception)
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpGet]
        [ActionName("JournalStudent")]
        public async Task<IActionResult> GetJournalStudentPage(string id)
        {
            Student? student = await _studentRepository.GetByIdAsync(id);

            if(student is null)
                return RedirectToAction("StudentManagement");

            ViewBag.Points = await _pointsService.GetPointsByStudentIdAsync(id);
            ViewBag.Student = student;

            return View("~/Views/Home/Department Management/Students/JournalStudent.cshtml");
        }

        [HttpPost]
        [ActionName("AddPointStudent")]
        public async Task<IActionResult> AddPointStudentPage(string id, PointsView points)
        {
            await _pointsService.AddPointToStudentAsync(id,
            new Points() { Id = Guid.NewGuid().ToString(), Subject = points.Subject, Point = points.Point });

            return await GetJournalStudentPage(id);
        }

        [HttpPost]
        [ActionName("DeleteStudentPoint")]
        public async Task<IActionResult> DeleteStudentPoint(string pointId, string studentId)
        {
            List<Points?>? points = await _pointsService.GetPointsByStudentIdAsync(studentId);

            if(points is null)
                return RedirectToAction("JournalStudent");

            Points? point = points.Find(p => p.Id == pointId);

            if (point is null)
                return RedirectToAction("JournalStudent");

            Student_Point? student_Point = point.StudentPoints.ToList().Find(p => p.PointId == pointId);

            if (student_Point is null)
                return RedirectToAction("JournalStudent");

            await _pointsService.DeletePointFromStudentAsync(student_Point.Id);

            return await GetJournalStudentPage(studentId);
        }
    }
}
