using StudentSearch.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static StudentSearch.Helpers.Enums;


namespace StudentSearch.ViewModels
{
    public class StudentViewModel
    {
        public StudentViewModel()
        {
        }

        public StudentViewModel(Student student)
        {
            Id = student.Id;
            FirstName = student.FirstName;
            LastName = student.LastName;
            ExpectedGraduation = student.ExpectedGraduation;
            GPA = student.GPA;
            Major = student.Major;
            Comments = student.Comments;
        }

        //Student Attributes
        public int Id { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$"), StringLength(30)]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$"), StringLength(30)]
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        [Required]
        [Display(Name = "Expected Graduation")]
        public string ExpectedGraduation { get; set; }

        [Range(0, 4), Column(TypeName = "decimal(18, 2)")]
        [Required]
        public decimal GPA { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$"), StringLength(30)]
        [Required]
        public string Major { get; set; }

        [DisplayName("Comment")]
        public string CommentText { get; set; }

        [DisplayName("Date")]
        public DateTime CommentEnteredOn { get; set; }

        [DisplayName("User")]
        public ApplicationUser CommentEnteredBy { get; set; }
        public string FullName
        {
            get { return string.Concat(FirstName, " ", LastName); }
        }



        //Other Attributes
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public SelectList Majors { get; set; }
        public SelectList GradYears { get; set; }
        public string StudentMajor { get; set; }
        public string GradYear { get; set; }

        public string FilterBy { get; set; }

        public SortByParameter SortBy { get; set; }
        public SortByParameter SortByFirstName { get; set; }

        public SortByParameter SortByLastName { get; set; }
        public SortByParameter SortByGraduation { get; set; }
        public SortByParameter SortByGPA { get; set; }
        public SortByParameter SortByMajor { get; set; }
    }
}
