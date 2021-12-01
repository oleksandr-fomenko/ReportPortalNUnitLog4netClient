using ReportPortal.Client.Models;

namespace ReportPortalNUnitLog4netClient.Models
{
    public class ImageAttachment : Attach
    {
        public ImageAttachment()
        {
        }

        public ImageAttachment(string name, byte[] data)
        {
            Name = name;
            MimeType = "image/png";
            Data = data;
        }
    }
}
