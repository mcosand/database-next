namespace Kcsara.Database.Data
{
  using System.ComponentModel.DataAnnotations.Schema;

  [Table("MemberContactTypes")]
  public class MemberContactTypeRow : ModelObject
  {
    public string CategoryLabel { get; set; }
    public string SubTypeLabel { get; set; }
    public string RegEx { get; set; }
  }
}
