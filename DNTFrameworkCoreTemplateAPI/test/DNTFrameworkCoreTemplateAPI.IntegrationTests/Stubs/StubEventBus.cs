using System.Threading.Tasks;
using DNTFrameworkCore.Eventing;
using DNTFrameworkCore.Functional;

namespace DNTFrameworkCoreTemplateAPI.IntegrationTests.Stubs
{
    public class StubEventBus : IEventBus
    {
        public Task<Result> TriggerAsync<T>(T @event) where T : IBusinessEvent
        {
            return Task.FromResult(Result.Ok());
        }
    }
}