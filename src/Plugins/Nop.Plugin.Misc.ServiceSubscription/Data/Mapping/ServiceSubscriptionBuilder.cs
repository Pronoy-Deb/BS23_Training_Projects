using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.ServiceSubscription.Domain;

namespace Nop.Plugin.Misc.ServiceSubscription.Data.Mapping
{
    public class ServiceSubscriptionBuilder : NopEntityBuilder<Subscription>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {

        }
    }
}