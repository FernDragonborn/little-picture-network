using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test_project_Inforce_backend.Models;

public class Album
{
    [Key]
    public Guid AlbumId { get; set; }

    [Required]
    [ForeignKey("UserId")]
    public virtual User User { get; set; }


    public List<Photo> Photos { get; set; }

    public string Title { get; set; }
}