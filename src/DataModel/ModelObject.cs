/*
 * Copyright 2016 Matthew Cosand
 */
namespace Kcsara.Database.Data
{
  using System;

  public interface IModelObject
  {
    Guid Id { get; }
    DateTime LastChanged { get; set; }
    string ChangedBy { get; set; }
  }

  public abstract class ModelObject : IModelObject
  {
    public Guid Id { get; set; }
    public DateTime LastChanged { get; set; }
    public string ChangedBy { get; set; }
  }
}
