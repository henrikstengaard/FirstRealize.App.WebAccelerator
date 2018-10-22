using System.Configuration;

namespace FirstRealize.App.WebAccelerator.Configuration
{
    // https://manyrootsofallevilrants.blogspot.com/2011/07/nested-custom-configuration-collections.html
    public class WebAcceleratorSection : ConfigurationSection
    {
        [ConfigurationProperty("ContentTypes")]
        public ContentTypeElement ContentTypes
        {
            get { return (ContentTypeElement)this["ContentTypes"]; }
            set { this["ContentTypes"] = value; }
        }
    }
}