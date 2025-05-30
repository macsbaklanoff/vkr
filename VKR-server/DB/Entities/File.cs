using System.ComponentModel.DataAnnotations.Schema;

namespace VKR_server.DB.Entities
{
    [Table("files")]
    public class File
    {
        [Column("file_id")]
        public int FileId { get; set; }
        [Column("file_name")]
        public string FileName { get; set; }

        [Column("academic_subject")]
        public string AcademicSubject { get; set; }
        [Column("topic_work")]
        public string TopicWork { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public User User { get; set; }

        public Estimation? Estimation { get; set; }
    }
}
