using NUnit.Framework;
using Wkhtmltopdf.NetCore.Options;

namespace Wkhtmltopdf.NetCore.Test.Options
{
    public class MarginsTest
    {
        [TestCase("-B 1 -L 2 -R 3 -T 4", 1,2,3,4)]
        [TestCase("-B 1 -L 2 -R 3", 1,2,3,null)]
        [TestCase("-B 1 -L 2", 1,2,null,null)]
        [TestCase("-B 1", 1,null,null,null)]
        [TestCase("", null,null,null,null)]
        public void Converts(string expected, int? bottom, int? left, int? right, int? top)
        {
            var margins = new Margins
            {
                Bottom = bottom,
                Left = left,
                Right = right,
                Top = top,
            };
            
            Assert.AreEqual(expected, margins.ToString());
        }
    }
}