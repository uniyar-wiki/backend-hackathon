namespace UniYarWiki.Models
{
    public class TeacherRatings
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int UserId { get; set; }
        public double KnowledgeRating { get; set; }
        public double TeachingSkillRating { get; set; }
        public double CommunicationRating { get; set; }
        public double EasinessRating { get; set; }
    }
}
