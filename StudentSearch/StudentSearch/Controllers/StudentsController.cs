using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using StudentSearch.Repository.Interfaces;
using StudentSearch.ViewModels;
using StudentSearch.Models;
using static StudentSearch.Helpers.Enums;

namespace StudentSearch.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStudentRepository _studentRepository;

        public StudentsController(UserManager<ApplicationUser> userManager, IStudentRepository studentRepository)
        {
            _userManager = userManager;
            _studentRepository = studentRepository;
        }

        // GET: Students
        public async Task<IActionResult> Index(StudentViewModel studentViewModel)
        {
            if (studentViewModel.SortBy == SortByParameter.FirstNameASC)
            {
                studentViewModel.SortByFirstName = SortByParameter.FirstNameDESC;
            }
            else
            {
                studentViewModel.SortByFirstName = SortByParameter.FirstNameASC;
            }

            // Swap Last Name sort order.
            if (studentViewModel.SortBy == SortByParameter.LastNameASC)
            {
                studentViewModel.SortByLastName = SortByParameter.LastNameDESC;
            }
            else
            {
                studentViewModel.SortByLastName = SortByParameter.LastNameASC;
            }

            // Swap Graduation Date sort order.
            if (studentViewModel.SortBy == SortByParameter.GraduationDateASC)
            {
                studentViewModel.SortByGraduation = SortByParameter.GraduationDateDESC;
            }
            else
            {
                studentViewModel.SortByGraduation = SortByParameter.GraduationDateASC;
            }

            //Swap GPA sort order.
            if (studentViewModel.SortBy == SortByParameter.GPAASC)
            {
                studentViewModel.SortByGPA = SortByParameter.GPADESC;
            }
            else
            {
                studentViewModel.SortByGPA = SortByParameter.GPAASC;
            }

            //Swap Major sort order.
            if (studentViewModel.SortBy == SortByParameter.MajorASC)
            {
                studentViewModel.SortByMajor = SortByParameter.MajorDESC;
            }
            else
            {
                studentViewModel.SortByMajor = SortByParameter.MajorASC;
            }



            studentViewModel.Students = await _studentRepository.GetStudents(studentViewModel.StudentMajor,
                                                                              studentViewModel.GradYear,
                                                                              studentViewModel.FilterBy,
                                                                              studentViewModel.SortBy);

            studentViewModel.Majors = await _studentRepository.GetMajors();
            studentViewModel.GradYears = await _studentRepository.GetGradYears();
            return View(studentViewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id, string StudentMajor, string GradYear, string FilterBy, SortByParameter sortBy)
        {
            StudentViewModel studentViewModel;

            if (id == null)
            {
                return NotFound();
            }

            var student = await _studentRepository.GetStudentByID((int)id);

            if (student == null)
            {
                return NotFound();
            }
            else
            {
                studentViewModel = new(student)
                {
                    StudentMajor = StudentMajor,
                    GradYear = GradYear,
                    FilterBy = FilterBy,
                    SortBy = sortBy
                };
            }

            return View(studentViewModel);
        }

        // GET: Students/Create
        public IActionResult Create(string StudentMajor, string GradYear, string FilterBy, SortByParameter sortBy)
        {
            StudentViewModel studentViewModel = new()
            {
                StudentMajor = StudentMajor,
                GradYear = GradYear,
                FilterBy = FilterBy,
                SortBy = sortBy
            };

            return View(studentViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,ExpectedGraduation,GPA,Major")] StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                await _studentRepository.InsertStudent(studentViewModel);

                return RedirectToAction(nameof(Index), studentViewModel);
            }
            return View(studentViewModel);

        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id, string StudentMajor, string GradYear, string FilterBy, SortByParameter sortBy)
        {
            StudentViewModel studentViewModel;

            if (id == null)
            {
                return NotFound();
            }

            var student = await _studentRepository.GetStudentByID((int)id);

            if (student == null)
            {
                return NotFound();
            }
            else
            {
                studentViewModel = new(student)
                {
                    StudentMajor = StudentMajor,
                    GradYear = GradYear,
                    FilterBy = FilterBy,
                    SortBy = sortBy
                };
            }
            return View(studentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,ExpectedGraduation,GPA,Major")] StudentViewModel studentViewModel)
        {

            if (id != studentViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updatedStudent = await _studentRepository.UpdateStudent(studentViewModel);
                if (updatedStudent == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index), studentViewModel);
            }
            return View(studentViewModel);

        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id, string StudentMajor, string GradYear, string FilterBy, SortByParameter sortBy)
        {
            StudentViewModel studentViewModel;
            if (id == null)
            {
                return NotFound();
            }

            var student = await _studentRepository.GetStudentByID((int)id);
            if (student == null)
            {
                return NotFound();
            }
            else
            {
                studentViewModel = new(student)
                {
                    StudentMajor = StudentMajor,
                    GradYear = GradYear,
                    FilterBy = FilterBy,
                    SortBy = sortBy
                };
            }

            return View(studentViewModel);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, [Bind("Id,FirstName,LastName,ExpectedGraduation,GPA,Major")] StudentViewModel studentViewModel)
        {
            await _studentRepository.DeleteStudent(id);
            return RedirectToAction(nameof(Index), studentViewModel);

        }

        public async Task<IActionResult> CreateComment(int id)
        {
            StudentViewModel studentViewModel = new StudentViewModel(await _studentRepository.GetStudentByID(id));

            return View(studentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment([Bind("Id,FirstName,LastName,ExpectedGraduation,GPA,Major,CommentText")] StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                studentViewModel.CommentEnteredOn = DateTime.Now;
                studentViewModel.CommentEnteredBy = await _userManager.GetUserAsync(User);
                await _studentRepository.InsertComment(studentViewModel);
                return RedirectToAction(nameof(Details), new { studentViewModel.Id });
            }
            return View();
        }

        // GET: Student/DeleteComment/5
        public async Task<IActionResult> DeleteComment(int id, int studentId)
        {
            var Id = studentId;

            await _studentRepository.DeleteCommentByID(id);
            return RedirectToAction(nameof(Details), new { Id });
        }

    }
}
