using System.Threading.Tasks;
using DNTFrameworkCore.Eventing;
using DNTFrameworkCore.Functional;

namespace DNTFrameworkCoreTemplateAPI.IntegrationTests.Stubs
{
    public class StubEventBus : IEventBus
    {
        public Task<Result> TriggerAsync<T>(T domainEvent) where T : IDomainEvent
        {
            return Task.FromResult(Result.Ok());
        }
    }
}