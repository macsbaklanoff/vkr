using System.ComponentModel.DataAnnotations.Schema;

namespace VKR_server.DB.Entities
{
    [Table("estimations")]
    public class Estimation
    {
        [Column("estimation_id")]
        public int EstimationId { get; set; }
        [Column("est_content")]
        public int EstContent { get; set; }
        [Column("est_relevance")]
        public int EstRelevance { get; set; }
        [Column("est_stylistic")]
        public int EstStylistic { get; set; }
        [Column("est_recommendations")]
        public string EstRecommedations { get; set; }
        public File File { get; set; }
    }
}
