using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
namespace FileSystem
{
    // Credit to:
    // Patrick Bacon: https://spin.atomicobject.com/2010/07/08/consolidate-multiple-filesystemwatcher-events/
    // delegate matches event
    public delegate void FileSystemEvent(String path);

    /// <summary>
    /// Interface FileMonitor to debounce / consolidate multiple FileSystemWatcher Events
    /// </summary>
    public interface IFileMonitor
    {
        // the event which is fired after consolidating / debouncing all events related to one save
        event FileSystemEvent Change;
        void Start();
    }

    public class FileMonitor : IFileMonitor
    {
        readonly FileSystemWatcher _fileSystemWatcher = new FileSystemWatcher();
        readonly Dictionary<string, DateTime> _pendingEvents = new Dictionary<string, DateTime>();
        readonly Timer _timer;
        bool _timerStarted = false;

        /// <summary>
        /// Parameterize FileSystemWatcher
        /// </summary>
        /// <param name="filePath"></param>
        public FileMonitor(string filePath)
        {
            _fileSystemWatcher.Path = Path.GetDirectoryName(filePath);
            _fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            _fileSystemWatcher.IncludeSubdirectories = false;
            _fileSystemWatcher.Changed += new FileSystemEventHandler(OnChange);

            _timer = new Timer(OnTimeout, null, Timeout.Infinite, Timeout.Infinite);

        }
        public event FileSystemEvent Change;

        /// <summary>
        /// Start Watching the file
        /// </summary>
        public void Start()
        {
            _fileSystemWatcher.EnableRaisingEvents = true;
        }
        /// <summary>
        /// Stop watching the file
        /// </summary>
        public void Stop()
        {
            _fileSystemWatcher.EnableRaisingEvents = false;
        }


        /// <summary>
        /// OnChange Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnChange(object sender, FileSystemEventArgs e)
        {
            // Don't want other threads messing with the pending events right now 
            lock (_pendingEvents)
            {
                // Save a timestamp for the most recent event for this path
                _pendingEvents[e.FullPath] = DateTime.Now;
                // Start a timer if not already started 
                if (!_timerStarted)
                {
                    // Start time + Interval in ms (after 100ms for 100ms)
                    _timer.Change(100, 100);
                    _timerStarted = true;
                }
            }
        }


        /// <summary>
        /// Timeout
        /// </summary>
        /// <param name="state"></param>
        void OnTimeout(object state)
        {
            List<string> paths;

            // Don't want other threads messing with the pending events right now 
            lock (_pendingEvents)
            { // Get a list of all paths that should have events thrown 
                paths = FindReadyPaths(_pendingEvents); 
                // Remove paths that are going to be used now 
                paths.ForEach(delegate(string path) { _pendingEvents.Remove(path); }); 
                // Stop the timer if there are no more events pending 
                if (_pendingEvents.Count == 0)
                {
                    _timer.Change(Timeout.Infinite, Timeout.Infinite);
                    _timerStarted = false;
                }
                // Fire an event for each path that has changed paths
                paths.ForEach(delegate (string path) 
                {
                     FireEvent(path);
                });
            
            }
        }

        List<string> FindReadyPaths(Dictionary<string, DateTime> events)
        {
            List<string> results = new List<string>();
            DateTime now = DateTime.Now;
            foreach (KeyValuePair<string, DateTime> entry in events)
            {
                // If the path has not received a new event in the last 75ms 
                // an event for the path should be fired 
                double diff = now.Subtract(entry.Value).TotalMilliseconds;
                if (diff >= 75)
                {
                    results.Add(entry.Key);
                }
            }
            return results;
        }

        void FireEvent(string path)
        {
            FileSystemEvent evt = Change;
            if (evt != null)
            {
                evt(path);
            }
        }
    }
}
   