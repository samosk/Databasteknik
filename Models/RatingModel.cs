using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Databasteknik.Models;

[Table("rating")]
public class Rating
{
    [Key]
    [Column("rating_id")]
    public int RatingId { get; set; }

    [Column("profile_id")]
    public int ProfileId { get; set; }

    [Column("content_id")]
    public int ContentId { get; set; }

    [Column("rating_value")]
    public required string RatingValue { get; set; }

    public virtual required ProfileModel Profile { get; set; }
    public virtual required ContentModel Content { get; set; }
}