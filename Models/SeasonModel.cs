using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Databasteknik.Models;

[Table("season")]
public class SeasonModel
{
    [Key]
    [Column("season_id")]
    public int SeasonId { get; set; }

    [Column("content_id")]
    public int ContentId { get; set; }

    [Column("season_number")]
    public int SeasonNumber { get; set; }

    [Column("number_of_episodes")]
    public int NumberOfEpisodes { get; set; }

    [Column("title")]
    public required string Title { get; set; }

    [Column("release_year")]
    public DateTime ReleaseYear { get; set; }

    public virtual required ContentModel Content { get; set; }
}