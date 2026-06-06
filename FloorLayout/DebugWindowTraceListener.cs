using System.Diagnostics;

namespace FloorLayout
{
    /// <summary>
    /// TraceListener that redirects System.Diagnostics.Debug output to DebugWindow
    /// </summary>
    public class DebugWindowTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            // Don't write partial messages, wait for WriteLine
        }

        public override void WriteLine(string message)
        {
            DebugWindow.WriteLine(message);
        }
    }
}
