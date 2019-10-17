using System;
using System.Collections.Generic;
using System.Text;

namespace Jot.Configuration.Attributes
{
    public class TrackingKeyAttribute
    {
        public bool IncludeType { get; }

        public object Namespace { get; }

        public TrackingKeyAttribute(object @namespace = null, bool includeType = false)
        {
            IncludeType = includeType;
            Namespace = @namespace;
        }
    }
}
