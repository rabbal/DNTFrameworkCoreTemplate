using System.Threading.Tasks;
using DNTFrameworkCore.Domain;
using DNTFrameworkCore.Eventing;
using DNTFrameworkCore.Functional;

namespace DNTFrameworkCoreTemplateAPI.IntegrationTests.Stubs
{
    public class StubEventBus : IEventBus
    {

        public Task<Result> TriggerAsync(IBusinessEvent businessEvent)
        {
            return Task.FromResult(Result.Ok());
        }

        public Task TriggerAsync(IDomainEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}