using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kcsara.Database.Website.Identity
{
  [Table("PendingUsers")]
  public class PendingUser
  {    
    public int Id { get; set; }

    public Guid? MemberId { get; set; }

    public string Email { get; set; }
    public string VerifyCode { get; set; }

    public string InvitationCode { get; set; }
    public string InvitedByUser { get; set; }
    public string InviteComment { get; set; }
  }
}
