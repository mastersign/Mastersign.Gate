using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.Gate
{
    public static class Extensions
    {
        public static string RecursiveMessage(this Exception exc)
        {
            // Merge error messages from exception and inner exceptions
            var msg = string.Empty;
#if DEBUG
            msg += exc.ToString();
#else
            msg += exc2.Message;
#endif
            while (exc.InnerException != null)
            {
                exc = exc.InnerException;
#if DEBUG
                msg += Environment.NewLine + Environment.NewLine + exc.ToString();
#else
                msg += Environment.NewLine + exc2.Message;
#endif
            }
            return msg;
        }
    }
}
