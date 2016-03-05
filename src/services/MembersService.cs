/*
 * Copyright 2016 Matthew Cosand
 */
namespace Kcsara.Database.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Data.Entity;
  using System.Text;
  using System.Threading.Tasks;
  using Kcsara.Database.Data;
  using Kcsara.Database.Model.Members;
  using Microsoft.Extensions.Logging;

  public interface IMembersService
  {
    Task<List<MemberSummary>> ByEmail(string email);
  }

  public class MembersService : IMembersService
  {
    private readonly Func<IDataContext> _dbFactory;
    //private readonly ICurrentPrincipalProvider principalProvider;
    private readonly ILogger _log;

    public MembersService(Func<IDataContext> dbFactory, ILogger<MembersService> log)
    {
      _dbFactory = dbFactory;
      //this.principalProvider = principalProvider;
      _log = log;
    }

    public async Task<List<MemberSummary>> ByEmail(string email)
    {
      using (var db = _dbFactory())
      {
        return await SummariesWithUnits(db.Members.Where(f => f.Contacts.Any(g => g.Value == email))).ToListAsync();
      }
    }


    private IQueryable<MemberSummary> SummariesWithUnits(IQueryable<MemberRow> query)
    {
      DateTime cutoff = DateTime.Now;
      //return query
      //  .Select(f => new {
      //    Member = f,
      //    Units = f.Memberships
      //              .Where(g => (g.EndTime == null || g.EndTime > cutoff) && g.Status.IsActive)
      //              .Select(g => g.Unit)
      //              .Select(g => new NameIdPair { Id = g.Id, Name = g.DisplayName }).Distinct()
      //  })
      //  .AsEnumerable()
      //  .Select(f => new MemberSummary
      //  {
      //    Name = f.Member.FullName,
      //    WorkerNumber = f.Member.DEM,
      //    Id = f.Member.Id,
      //    Units = f.Units.ToArray(),
      //    Photo = f.Member.PhotoFile
      //  });
      return query
        .Select(f => new
        {
          Member = f
        })
        .Select(f => new MemberSummary
        {
          Id = f.Member.Id,
          WorkerNumber = f.Member.WorkerNumber,
          Name = f.Member.FirstName + " " + f.Member.LastName,
          Photo = f.Member.PhotoFile
        });
    }
  }
}
