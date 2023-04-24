using StudentSearch.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentSearch.Models;

public class Student
{
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string ExpectedGraduation { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal GPA { get; set; }

    [Required]
    public string Major { get; set; }

    public virtual ICollection<Comment> Comments { get; set; }
}
