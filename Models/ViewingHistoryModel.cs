using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Databasteknik.Models;

[Table("viewing_history")]
public class ViewingHistoryModel
{
    [Key]
    [Column("viewing_id")]
    public int ViewingId { get; set; }

    [Column("profile_id")]
    public int ProfileId { get; set; }

    [Column("content_id")]
    public int ContentId { get; set; }

    [Column("episode_id")]
    public int EpisodeId { get; set; }

    [Column("watch_date")]
    public DateTime WatchDate { get; set; }

    [Column("watched_duration")]
    public int WatchedDuration { get; set; }

    [Column("completion")]
    public decimal Completion { get; set; }

    [Column("device_type")]
    public required string DeviceType { get; set; }

    public virtual required ProfileModel Profile { get; set; }
    public virtual required ContentModel Content { get; set; }
    public virtual required EpisodeModel Episode { get; set; }
}