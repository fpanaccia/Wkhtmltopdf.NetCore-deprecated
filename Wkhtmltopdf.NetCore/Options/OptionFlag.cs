using System;

namespace Wkhtmltopdf.NetCore.Options
{
    public class OptionFlag : Attribute
    {
        public string Name { get; private set; }

        public OptionFlag(string name)
        {
            Name = name;
        }
    }
}
