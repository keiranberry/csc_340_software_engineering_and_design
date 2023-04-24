using StudentSearch.Models;
using System.ComponentModel.DataAnnotations;

namespace StudentSearch.Models
{
    public class Comment
    {
        public Comment()
        {
            Text = string.Empty;
        }

        public int Id { get; set; }

        [Required]
        public DateTime EnteredOn { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public virtual Student Student { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string EnteredBy
        {
            get
            {
                return ApplicationUser == null ? "Not Set" : string.Concat(ApplicationUser.CompanyName);
            }
        }
    }
}
