using System;

namespace UniYarWiki.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }

    }
}
