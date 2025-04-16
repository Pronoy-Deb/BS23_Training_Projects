﻿using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.Suppliers.Models
{
    public record SuppliersSearchModel : BaseSearchModel
    {
        public string SearchName { get; init; }
        public string SearchEmail { get; init; }
        public bool? Active { get; internal set; }

    }
}
