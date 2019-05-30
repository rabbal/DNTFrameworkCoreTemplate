using System;
using System.Collections.Generic;
using DNTFrameworkCore.MultiTenancy;
using DNTFrameworkCore.Runtime;

namespace DNTFrameworkCoreTemplateAPI.IntegrationTests.Stubs
{
    public class StubUserSession : IUserSession
    {
        public StubUserSession(long userId)
        {
            UserId = userId;
        }
        public IDisposable Use(long? tenantId, long? userId)
        {
            throw new NotImplementedException();
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public bool IsGranted(string permission)
        {
            throw new NotImplementedException();
        }

        public bool IsAuthenticated { get; }
        public long? UserId { get; }
        public string UserName { get; }
        public IReadOnlyList<string> Permissions { get; }
        public IReadOnlyList<string> Roles { get; }
        public string UserDisplayName { get; }
        public string UserBrowserName { get; }
        public string UserIP { get; }
        public long? TenantId { get; }
        public MultiTenancySides MultiTenancySide { get; }
        public long? ImpersonatorUserId { get; }
        public long? ImpersonatorTenantId { get; }
        public long? BranchId { get; }
    }
}