namespace UniYarWiki.Models
{
    public class UserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public int CourseNum { get; set; }
        public string Group { get; set; }
        public string Avatar { get; set; }
    }
}