using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Databasteknik.Models;

[Table("content")]
public class ContentModel
{
    [Key]
    [Column("content_id")]
    public int ContentId { get; set; }

    [Column("title")]
    public required string Title { get; set; }

    [Column("content_type")]
    public required string ContentType { get; set; }

    [Column("release_year")]
    public DateTime ReleaseYear { get; set; }

    [Column("maturity_rating")]
    public required string MaturityRating { get; set; }

    [Column("description")]
    public required string Description { get; set; }

    [Column("country")]
    public required string Country { get; set; }

    [Column("original_language")]
    public required string OriginalLanguage { get; set; }

    [Column("duration")]
    public int Duration { get; set; }

    [Column("rating")]
    public decimal Rating { get; set; }

    [Column("views")]
    public int Views { get; set; }
}