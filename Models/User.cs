using System.ComponentModel.DataAnnotations;

namespace UniYarWiki.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string HashPwd { get; set; }

        public string First { get; set; }

        public string Last { get; set; }

        public int CourseNum { get; set; }

        public string Group { get; set; }

        public string Avatar { get; set; }

        public bool IsVerified { get; set; }
    }
}