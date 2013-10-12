using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xunit.gui.wpf.Models
{
    public enum ResultStatus
    {
        NotExecuted = 0,
        Skipped     = 1,
        Executing   = 2,
        Failed      = 3,
        Passed      = 4
    }
}
