using System;

namespace Wkhtmltopdf.NetCore.Options
{
    public class OptionFlagAttribute : Attribute
    {
        public string Name { get; private set; }

        public OptionFlagAttribute(string name)
        {
            Name = name;
        }
    }
}
