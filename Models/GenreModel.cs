using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("genre")]
public class Genre
{
    [Key]
    [Column("genre_id")]
    public int GenreId { get; set; }

    [Column("genre_name")]
    public required string GenreName { get; set; }

    [Column("description")]
    public required string Description { get; set; }
}