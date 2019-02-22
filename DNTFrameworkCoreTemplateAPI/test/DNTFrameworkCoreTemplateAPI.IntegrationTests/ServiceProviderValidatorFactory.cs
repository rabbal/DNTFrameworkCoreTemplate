using System;
using FluentValidation;

namespace DNTFrameworkCoreTemplateAPI.IntegrationTests
{
    /// <summary>
    /// Validator factory implementation that uses the asp.net service provider to consruct validators.
    /// </summary>
    public class ServiceProviderValidatorFactory : ValidatorFactoryBase {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderValidatorFactory(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public override IValidator CreateInstance(Type validatorType) {
            return _serviceProvider.GetService(validatorType) as IValidator;
        }
    }
}