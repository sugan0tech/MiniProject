using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrimonyApiService.Entities;

public class Preference
{
    [Key] public int PreferenceId { get; set; }
    public string MotherTongue { get; set; }
    public string Religion { get; set; }
    public string Education { get; set; }
    public string Occupation { get; set; }
    public Tuple<int, int> HeightRange { get; set; }
    public Tuple<int, int> AgeRange { get; set; }
    [ForeignKey("PreferenceForProfileId")] public int PreferenceForId { get; set; }
    public Profile PreferenceFor { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; }
}