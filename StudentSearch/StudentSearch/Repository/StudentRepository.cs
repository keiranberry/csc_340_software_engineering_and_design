using StudentSearch.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentSearch.Data;
using StudentSearch.Helpers;
using StudentSearch.Repository.Interfaces;
using StudentSearch.ViewModels;
using static StudentSearch.Helpers.Enums;

namespace StudentSearch.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private StudentSearchContext _context;

        public StudentRepository(StudentSearchContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Cleanup.
            _context = null;
        }

        public async Task<IList<Student>> GetStudents(string filterByMajor, string filterByGrad, string searchString, Enums.SortByParameter sortBy)
        {
            var students = from s in _context.Student
                           select s;

            // First, filter the list of students.
            if (string.IsNullOrEmpty(filterByMajor) == false)
            {
                students = students.Where(s => s.Major.Contains(filterByMajor));
            }

            if (string.IsNullOrEmpty(filterByGrad) == false)
            {
                students = students.Where(s => s.ExpectedGraduation.Contains(filterByGrad));
            }

            if (string.IsNullOrEmpty(searchString) == false)
            {
                students = students.Where(s => (s.FirstName + " " + s.LastName).Contains(searchString));
            }

            // Next, sort the list of students.
            switch (sortBy)
            {
                case SortByParameter.FirstNameDESC:
                    students = students.OrderByDescending(o => o.FirstName).ThenByDescending(o => o.LastName);
                    break;
                case SortByParameter.LastNameASC:
                    students = students.OrderBy(o => o.LastName).ThenBy(o => o.FirstName);
                    break;
                case SortByParameter.LastNameDESC:
                    students = students.OrderByDescending(o => o.LastName).ThenByDescending(o => o.FirstName);
                    break;
                case SortByParameter.GraduationDateASC:
                    students = students.OrderBy(o => o.ExpectedGraduation).ThenBy(o => o.FirstName).ThenBy(o => o.LastName);
                    break;
                case SortByParameter.GraduationDateDESC:
                    students = students.OrderByDescending(o => o.ExpectedGraduation).ThenByDescending(o => o.FirstName).ThenByDescending(o => o.LastName);
                    break;
                case SortByParameter.GPAASC:
                    students = students.OrderBy(o => o.GPA).ThenBy(o => o.FirstName).ThenBy(o => o.LastName);
                    break;
                case SortByParameter.GPADESC:
                    students = students.OrderByDescending(o => o.GPA).ThenByDescending(o => o.FirstName).ThenByDescending(o => o.LastName);
                    break;
                case SortByParameter.MajorASC:
                    students = students.OrderBy(o => o.Major).ThenBy(o => o.FirstName).ThenBy(o => o.LastName);
                    break;
                case SortByParameter.MajorDESC:
                    students = students.OrderByDescending(o => o.Major).ThenByDescending(o => o.FirstName).ThenByDescending(o => o.LastName);
                    break;
                default:
                    students = students.OrderBy(o => o.FirstName).ThenBy(o => o.LastName).ThenBy(o => o.ExpectedGraduation);
                    break;
            }

            return await students.ToListAsync();
        }

        public async Task<SelectList> GetMajors()
        {
            IQueryable<string> majorQuery = from m in _context.Student
                                            orderby m.Major
                                            select m.Major;

            return new SelectList(await majorQuery.Distinct().ToListAsync());
        }

        public async Task<SelectList> GetGradYears()
        {
            IQueryable<string> gradQuery = from g in _context.Student
                                           orderby g.ExpectedGraduation
                                           select g.ExpectedGraduation;

            return new SelectList(await gradQuery.Distinct().ToListAsync());
        }

        public async Task<Student> GetStudentByID(int studentId)
        {
            return await _context.Student
                .Include(c => c.Comments.OrderByDescending(o => o.EnteredOn))
                .ThenInclude(u => u.ApplicationUser)
                .FirstOrDefaultAsync(s => s.Id == studentId);

        }

        public async Task<Student> InsertStudent(StudentViewModel studentViewModel)
        {
            Student student = new()
            {
                FirstName = studentViewModel.FirstName.Trim(),
                LastName = studentViewModel.LastName.Trim(),
                ExpectedGraduation = studentViewModel.ExpectedGraduation.Trim(),
                Major = studentViewModel.Major.Trim(),
                GPA = studentViewModel.GPA
            };

            _context.Add(student);
            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<Student> UpdateStudent(StudentViewModel studentViewModel)
        {
            Student student;

            try
            {
                student = await _context.Student.FindAsync(studentViewModel.Id);

                student.FirstName = studentViewModel.FirstName.Trim();
                student.LastName = studentViewModel.LastName.Trim();
                student.ExpectedGraduation = studentViewModel.ExpectedGraduation;
                student.Major = studentViewModel.Major.Trim();
                student.GPA = studentViewModel.GPA;

                _context.Update(student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(studentViewModel.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return student;
        }

        public async Task DeleteStudent(int studentID)
        {
            var student = _context.Student.Include(c => c.Comments).SingleOrDefault(s => s.Id == studentID);

            _context.Comment.RemoveRange(student.Comments);


            _context.Student.Remove(student);

            await _context.SaveChangesAsync();
        }

        public async Task<Comment> InsertComment(StudentViewModel studentViewModel)
        {
            Student student = await _context.Student.FindAsync(studentViewModel.Id);

            Comment comment = new()
            {
                Student = student,
                ApplicationUser = studentViewModel.CommentEnteredBy,
                EnteredOn = studentViewModel.CommentEnteredOn,
                Text = studentViewModel.CommentText
            };

            _context.Comment.Add(comment);

            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task DeleteCommentByID(int commentId)
        {
            Comment comment = await _context.Comment.SingleOrDefaultAsync(c => c.Id == commentId);

            _context.Comment.Remove(comment);

            await _context.SaveChangesAsync();
        }


        private bool StudentExists(int id)
        {
            return (_context.Student?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
