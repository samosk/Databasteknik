using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Databasteknik.Models;

[Table("profile")]
public class ProfileModel
{
    [Key]
    [Column("profile_id")]
    public int ProfileId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("profile_name")]
    public required string ProfileName { get; set; }

    [Column("kids_profile")]
    public bool KidsProfile { get; set; } = false;

    [Column("maturity_rating")]
    public required string MaturityRating { get; set; }

    [Column("profile_language")]
    public string ProfileLanguage { get; set; } = "en";

    [Column("date_of_creation")]
    public DateTime DateOfCreation { get; set; }

    // Reference to users
    public virtual required UserModel User { get; set; }
}