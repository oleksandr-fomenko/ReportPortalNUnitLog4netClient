using System;
using System.Collections.Generic;
using ReportPortalNUnitLog4netClient.Models.Api;

namespace ReportPortalNUnitLog4netClient.Factory
{
    public static class RpFilterFactory
    {
        public static Filter ByLaunchName(string launchName, params Condition[] additionalConditions)
        {
            var filter = new Filter
            {
                Conditions = new List<Condition>
                {
                    new Condition
                    {
                        Value = launchName,
                        ConditionType = "cnt",
                        FilteringField = "name"
                    }
                },
                Type = "launch",
                Orders = new[] 
                {
                    new Order
                    {
                        IsAsc = false,
                        SortingColumn = "startTime"
                    },
                    new Order
                    {
                        IsAsc = false,
                        SortingColumn = "number"
                    }
                },
                Name = $"{launchName}_{DateTime.Now:hhmmssff}",
                Share = true
            };
            if (additionalConditions != null)
            {
                filter.Conditions.AddRange(additionalConditions);
            }
            return filter;
        }
    }
}
