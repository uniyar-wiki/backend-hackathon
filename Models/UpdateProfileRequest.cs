namespace UniYarWiki.Models
{
    public class UpdateProfileRequest
    {
        public string token { get; set; }
        public string first { get; set; }
        public string last { get; set; }
        public int course_num{ get; set; }
        public string group { get; set; }
        public string avatar { get; set; }
    }
}
