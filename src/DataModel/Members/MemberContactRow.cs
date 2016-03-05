/*
 * Copyright 2016 Matthew Cosand
 */
namespace Kcsara.Database.Data
{
  using System;
  using System.ComponentModel.DataAnnotations.Schema;

  [Table("MemberContacts")]
  public class MemberContactRow : ModelObject
  {
    [ForeignKey("TypeId")]
    public virtual MemberContactTypeRow Type { get; set; }
    public Guid TypeId { get; set; }
    public string Value { get; set; }
    public int Priority { get; set; }

    [ForeignKey("MemberId")]
    public virtual MemberRow Member { get; set; }
    public Guid MemberId { get; set; }
  }
}
