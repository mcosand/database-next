namespace Kcsara.Database.Website.Identity
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Microsoft.AspNet.Http;
  using Microsoft.AspNet.Identity;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.OptionsModel;

  public class SarUserManager : UserManager<ApplicationUser>
  {
    private readonly ApplicationDbContext _db;
    public SarUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<SarUserManager> logger, IHttpContextAccessor contextAccessor)
      : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger, contextAccessor)
    {
      _db = services.GetService<ApplicationDbContext>();
    }

    public PendingUser FindPendingUserEmail(string email)
    {
      return _db.PendingUsers.FirstOrDefault(f => f.Email == email);
    }

    internal void UpdatePending(PendingUser pending)
    {
      //if (pending.Id == 0) _db.PendingUsers.Add(pending);
      _db.PendingUsers.Attach(pending);
 //     _db.Entry(pending).State = (pending.Id <= 0) ? Microsoft.Data.Entity.EntityState.Added : Microsoft.Data.Entity.EntityState.Modified;
      _db.SaveChanges();
    }
  }
}
