using System;
using System.Linq;
using System.Collections.Generic;
using Knowledgebase.UnitOfWork;
using Knowledgebase.Models.AppUser;

namespace Knowledgebase.Application.Services
{
    public class AppUserService : _ServiceBase
    {
        private readonly IRepository<Entities.AppUser> _usersRepository;
        public AppUserService(IServiceProvider serviceProvider, IUnitOfWork uow)
            : base(serviceProvider)
        {
            _usersRepository = uow.GetRepository<Entities.AppUser>();
        }

        public ICollection<AppUserBrief> GetAll()
        {
            return _usersRepository.GetAll()
                .Select(x => new AppUserBrief
                {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.Picture,
                }).ToList();
        }

        public AppUserDetails GetDetails(Guid id)
        {
            return _usersRepository.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new AppUserDetails
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    CreatedAt = x.CreatedAt,
                    IsOwner = x.IsOwner,
                    Picture = x.Picture,
                }).FirstOrDefault();
        }

        public Guid Create(AppUserCreate input)
        {
            var model = new Entities.AppUser
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                ExternalId = input.ExternalId,
                Picture = input.Picture,
                Name = input.Name,
                Email = input.Email,
                IsOwner = true,
            };
            _usersRepository.Insert(model);
            return model.Id;
        }
    }
}
