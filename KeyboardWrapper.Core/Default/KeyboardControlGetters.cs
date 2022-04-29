using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardWrapper.Core.Default
{
    internal partial class KeyboardControl
    {
        public static string GetAvailableKeys()
        {
            return String.Join(',', Enum.GetNames<Keys>());
        }
    }
}
