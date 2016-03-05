/*
 * Copyright 2016 Matthew Cosand
 */
namespace Kcsara.Database.Model
{
  using System;

  public class NameIdPair
  {
    public NameIdPair() { }
    public NameIdPair(Guid id, string name)
    {
      Id = id;
      Name = name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
  }
}
