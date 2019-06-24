using DNTFrameworkCore.Domain;
using DNTFrameworkCore.EFCore.Context.Hooks;
using DNTFrameworkCoreTemplateAPI.Common.PersianToolkit;

namespace DNTFrameworkCoreTemplateAPI.Infrastructure.Hooks
{
    public class PreInsertApplyCorrectYeKeHook : PreInsertHook<IEntity>
    {
        protected override void Hook(IEntity entity, HookEntityMetadata metadata)
        {
            entity.ApplyCorrectYeKeToProperties();
        }
    }

    public class PreUpdateApplyCorrectYeKeHook : PreUpdateHook<IEntity>
    {
        protected override void Hook(IEntity entity, HookEntityMetadata metadata)
        {
            entity.ApplyCorrectYeKeToProperties();
        }
    }
}