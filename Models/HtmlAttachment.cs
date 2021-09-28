using System.Text;
using ReportPortal.Client.Models;

namespace ReportPortalNUnitLog4netClient.Models
{
    public class HtmlAttachment : Attach
    {
        public HtmlAttachment(string name, string html)
        {
            Name = name;
            MimeType = "text/html";
            Data = Encoding.ASCII.GetBytes(html);
        }
    }
}
