/*
 * Copyright 2016 Matthew Cosand
 */
namespace Kcsara.Database.Model.Members
{
  using System;

  public class MemberSummary
  {
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string WorkerNumber { get; set; }

    public string Photo { get; set; }

 //   public NameIdPair[] Units { get; set; }
  }
}
