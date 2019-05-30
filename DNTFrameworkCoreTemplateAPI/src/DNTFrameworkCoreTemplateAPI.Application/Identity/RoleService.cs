using System;
using System.Linq;
using AutoMapper;
using DNTFrameworkCore.Application.Models;
using DNTFrameworkCore.Application.Services;
using DNTFrameworkCore.EntityFramework.Application;
using DNTFrameworkCore.EntityFramework.Context;
using DNTFrameworkCore.Eventing;
using DNTFrameworkCoreTemplateAPI.Application.Identity.Models;
using DNTFrameworkCoreTemplateAPI.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace DNTFrameworkCoreTemplateAPI.Application.Identity
{
    public interface IRoleService : ICrudService<long, RoleReadModel, RoleModel>
    {
    }

    public class RoleService : CrudService<Role, long, RoleReadModel, RoleModel>, IRoleService
    {
        private readonly IMapper _mapper;
        public RoleService(IUnitOfWork uow, IEventBus bus, IMapper mapper) : base(uow, bus)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        protected override IQueryable<Role> BuildFindQuery()
        {
            return base.BuildFindQuery()
                .Include(r => r.Permissions);
        }

        protected override IQueryable<RoleReadModel> BuildReadQuery(FilteredPagedQueryModel model)
        {
            return EntitySet.AsNoTracking().Select(r => new RoleReadModel
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description
            });
        }

        protected override void MapToEntity(RoleModel model, Role role)
        {
            _mapper.Map(model, role);
        }

        protected override RoleModel MapToModel(Role role)
        {
            return _mapper.Map<RoleModel>(role);
        }
    }
}