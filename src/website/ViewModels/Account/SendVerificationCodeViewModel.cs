using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace website.ViewModels.Account
{
  public class SendVerificationCodeViewModel
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string Invite { get; set; }
  }
}
