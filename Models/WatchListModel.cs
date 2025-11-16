using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Databasteknik.Models;

[Table("watchlist")]
public class WatchList
{
    [Key]
    [Column("watch_list_id")]
    public int WatchListId { get; set; }

    [Column("profile_id")]
    public int ProfileId { get; set; }

    [Column("content_id")]
    public int ContentId { get; set; }

    public virtual required ProfileModel Profile { get; set; }
    public virtual required ContentModel Content { get; set; }
}