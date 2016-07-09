using System;
using System.Diagnostics;


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
            return string.Format("{0:00}M:{1:00}S.{2:00}MS",
                ts.Minutes, ts.Seconds,
                ts.Milliseconds);
        }

    }
}
