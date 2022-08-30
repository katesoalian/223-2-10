using Core.Models;
using Core.Views;
using DepartmentPresentation.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace DepartmentPresentation.Controllers
{
    public class GroupController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IProfessorRepository _professorRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IStudentRepository _studentRepository;

        public GroupController(ILogger<HomeController> logger, IGroupRepository groupRepository, IProfessorRepository professorRepository, IStudentRepository studentRepository)
        {
            _logger = logger;
            _groupRepository = groupRepository;
            _professorRepository = professorRepository;
            _studentRepository = studentRepository;
        }

        [HttpGet]
        [ActionName("GroupManagement")]
        public IActionResult GetGroupsPage()
        {
            ViewBag.Message = "";
            ViewBag.Groups = _groupRepository.GetAllAsync().Result.ToList();
            return View("~/Views/Home/Department Management/Groups/GroupManagement.cshtml");
        }

        [HttpGet]
        [ActionName("FindGroup")]
        public IActionResult FindGroupPage(string search)
        {
            if (search is null)
                search = string.Empty;

            ViewBag.Groups = _groupRepository.GetAllAsync()
                                .Result
                                .ToList()
                                .FindAll(group => group.Course.ToString().Contains(search)
                                || group.Professor.FirstName.Contains(search)
                                || group.Professor.LastName.Contains(search));
            return View("~/Views/Home/Department Management/Groups/GroupManagement.cshtml");
        }

        [HttpGet]
        [ActionName("AddGroup")]
        public async Task<IActionResult> GetAddGroupPage()
        {
            ViewBag.Professors = await _professorRepository.GetAllAsync();

            return View("~/Views/Home/Department Management/Groups/AddGroup.cshtml");
        }

        [HttpPost]
        [ActionName("AddGroup")]
        public async Task<IActionResult> AddGroup(GroupView group)
        {
            try
            {
                await _groupRepository.SaveAsync(new Group()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = group.Name,
                    Course = group.Course,
                    ProfessorId = group.ProfessorId
                });

                return RedirectToAction("GroupManagement");
            }
            catch(Exception)
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpGet]
        [ActionName("UpdateGroup")]
        public async Task<IActionResult> GetUpdateGroupPage(string id)
        {
            Group? group = await _groupRepository.GetByIdAsync(id);

            ViewBag.Professors = await _professorRepository.GetAllAsync();
            ViewBag.Group = group;

            return View("~/Views/Home/Department Management/Groups/UpdateGroup.cshtml");
        }

        [HttpPost]
        [ActionName("UpdateGroup")]
        public async Task<IActionResult> UpdateGroupPage(Group group)
        {
            await _groupRepository.UpdateAsync(group);

            ViewBag.Group = group;

            return RedirectToAction("GroupManagement");
        }

        [HttpPost]
        [ActionName("DeleteGroup")]
        public async Task<IActionResult> DeleteGroup(string id)
        {            
            try
            {
                Group? group = await _groupRepository.GetByIdAsync(id);

                if(group is null)
                    return RedirectToAction("GroupManagement");

                foreach (var student in group.Students)
                {
                    student.GroupId = null;
                    await _studentRepository.UpdateAsync(student);
                }

                await _groupRepository.DeleteAsync(id);

                return RedirectToAction("GroupManagement");
            }
            catch (Exception)
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}
