using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Diagnostics;


namespace hoTools.Utils.RUN
{

    public class Run
    {
        Stopwatch _stopWatch = null;

        #region Constructor
        public Run()
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();

        }
        #endregion
        public string stop()
        {
            _stopWatch.Stop();
            TimeSpan ts = _stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            return String.Format("{0:00}M:{1:00}S.{2:00}MS",
                ts.Minutes, ts.Seconds,
                ts.Milliseconds);
        }

    }
}
