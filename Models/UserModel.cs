using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Databasteknik.Models;

[Table("users")]
public class UserModel
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("email")]
    public required string Email { get; set; }

    [Column("password_hash")]
    public required string PasswordHash { get; set; }

    [Column("first_name")]
    public required string FirstName { get; set; }

    [Column("last_name")]
    public required string LastName { get; set; }

    [Column("date_of_birth")]
    public DateTime DateOfBirth { get; set; }

    [Column("date_of_creation")]
    public DateTime DateOfCreation { get; set; }

    [Column("subscription_tier")]
    public required string SubscriptionTier { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    //Reference to profile
    public virtual required ProfileModel Profiles { get; set; }
}


