/*
 * Copyright 2016 Matthew Cosand
 */
namespace Kcsara.Database.Data
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  [Table("Members")]
  public class MemberRow : ModelObject
  {
    public MemberRow() : base()
    {
      Contacts = new List<MemberContactRow>();
    }

    [MaxLength(50)]
    public string WorkerNumber { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
    public DateTime? BirthDate { get; set; }

    public Gender Gender { get; set; }

    public string PhotoFile { get; set; }

    public virtual ICollection<MemberContactRow> Contacts { get; set; }

    public bool? IsYouth
    {
      get
      {
        if (BirthDate.HasValue)
        {
          return BirthDate.Value.Date.AddYears(18) >= DateTime.Now.Date;
        }
        return null;
      }
    }
  }
}
