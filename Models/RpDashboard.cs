using System.Collections.Generic;
using ReportPortalNUnitLog4netClient.Models.Api;

namespace ReportPortalNUnitLog4netClient.Models
{
    public class RpDashboard
    {
        public Filter Filter { get; set; }
        public Dashboard Dashboard { get; set; }
        public List<Widget> Widgets { get; set; }

        public RpDashboard(Filter filter, Dashboard dashboard, List<Widget> widgets)
        {
            Filter = filter;
            Dashboard = dashboard;
            Widgets = widgets;
        }
    }
}
