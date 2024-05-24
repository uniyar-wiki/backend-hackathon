using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniYarWiki.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string AlmaMater { get; set; }
        public string Degree { get; set; }
        public List<string> Positions { get; set; }
        public string Biography { get; set; }
        public double KnowledgeRating { get; set; }
        public double TeachingSkillRating { get; set; }
        public double CommunicationRating { get; set; }
        public double EasinessRating { get; set; }
        public double OverallRating { get; set; }
    }
}

