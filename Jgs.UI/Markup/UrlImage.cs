using Jgs.UI.Extensions;
using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace Jgs.UI.Markup
{
    [MarkupExtensionReturnType(typeof(ImageSource))]
    public class UrlImage : MarkupExtension
    {
        public UrlImage()
           : this(null)
        {
        }

        public UrlImage(string imageLink)
        {
            ImageLink = imageLink;
        }

        public string ImageLink { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ImageLink?.UrlAsImageSource();
        }
    }
}
