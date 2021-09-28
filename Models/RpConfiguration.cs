using System;
using ReportPortal.Client.Models;

namespace ReportPortalNUnitLog4netClient.Models
{
    public class RpConfiguration
    {
        public Uri Uri { get; set; }
        public string Project { get; set; }
        public string Uuid { get; set; }
        public string LaunchName { get; set; }
        public LaunchMode Mode { get; set; } = LaunchMode.Debug;
        public bool IsWriteLaunchData { get; set; }
        public bool IsEnabled { get; set; }
        public string IssueUri { get; set; }
        public string TestManagementSystemUri { get; set; }
    }
}
