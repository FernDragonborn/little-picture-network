using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test_project_Inforce_backend.Models;
public class Like
{
    [Key]
    public uint LikeId { get; set; }

    [Required]
    [ForeignKey("Photo")]
    public Guid PhotoId { get; set; }

    [Required]
    [ForeignKey("User")]
    public Guid UserId { get; set; }


    public virtual Photo Photo { get; set; }

    public virtual User User { get; set; }

    /// <summary>
    /// 0 - dislike, 1 - LikeState
    /// </summary>
    [Required]
    public bool LikeState { get; set; }
}