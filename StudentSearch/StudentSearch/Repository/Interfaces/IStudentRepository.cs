using StudentSearch.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentSearch.ViewModels;
using static StudentSearch.Helpers.Enums;

namespace StudentSearch.Repository.Interfaces
{
    public interface IStudentRepository : IDisposable
    {
        Task<SelectList> GetMajors();
        Task<SelectList> GetGradYears();
        Task<IList<Student>> GetStudents(string filterByMajor, string filterByGrad, string SearchString, SortByParameter sortBy);
        Task<Student> GetStudentByID(int studentId);
        Task<Student> InsertStudent(StudentViewModel studentViewModel);
        Task DeleteStudent(int studentID);
        Task<Student> UpdateStudent(StudentViewModel studentViewModel);
        Task<Comment> InsertComment(StudentViewModel studentViewModel);
        Task DeleteCommentByID(int commentId);

    }

}
