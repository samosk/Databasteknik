
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Databasteknik.Models;

[Table("episode")]
public class EpisodeModel
{
    [Key]
    [Column("episode_id")]
    public int EpisodeId { get; set; }

    [Column("season_id")]
    public int SeasonId { get; set; }

    [Column("episode_number")]
    public int EpisodeNumber { get; set; }

    [Column("title")]
    public required string Title { get; set; }

    [Column("description")]
    public required string Description { get; set; }

    [Column("duration")]
    public int Duration { get; set; }

    [Column("release_date")]
    public DateTime ReleaseDate { get; set; }

    public virtual required SeasonModel Season { get; set; }
}