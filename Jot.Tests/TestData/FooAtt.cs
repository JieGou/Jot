using Jot.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jot.Tests.TestData
{
    class FooAtt
    {
        [Trackable]
        public int Int { get; set; }
        [Trackable]
        public double Double { get; set; }
        public TimeSpan Timespan { get; set; }
    }
}
