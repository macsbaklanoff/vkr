using System.ComponentModel.DataAnnotations.Schema;

namespace VKR_server.DB.Entities
{
    [Table("files")]
    public class File
    {
        public int FileId { get; set; }
        [Column("file_name")]
        public string FileName { get; set; }
        [Column("name_job")]
        public string NameJob { get; set; }
        [Column("academic_subject")]
        public string AcademicSubject { get; set; }
        [Column("topic_work")]
        public string TopicWork { get; set; }

    }
}
