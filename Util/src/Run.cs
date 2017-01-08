using System;
using System.Diagnostics;

// ReSharper disable once CheckNamespace
namespace hoTools.Utils.RUN
{

    public class Run
    {
        readonly Stopwatch _stopWatch;

        #region Constructor
        public Run()
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();

        }
        #endregion
        public string Stop()
        {
            _stopWatch.Stop();
            TimeSpan ts = _stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            return $"{ts.Minutes:00}M:{ts.Seconds:00}S.{ts.Milliseconds:00}MS";
        }

    }
}
