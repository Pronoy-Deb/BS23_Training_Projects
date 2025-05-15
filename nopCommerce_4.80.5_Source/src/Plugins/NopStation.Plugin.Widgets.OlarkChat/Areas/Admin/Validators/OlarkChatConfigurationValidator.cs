using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using NopStation.Plugin.Widgets.OlarkChat.Areas.Admin.Models;

namespace NopStation.Plugin.Widgets.OlarkChat.Areas.Admin.Validators;

public class OlarkChatConfigurationValidator : BaseNopValidator<OlarkChatConfigurationModel>
{
    public OlarkChatConfigurationValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.SiteId)
            .NotEmpty()
            .WithMessage(localizationService
                .GetResourceAsync("Admin.NopStation.OlarkChat.Fields.SiteId.Required").Result);
    }
}