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
        void Stop();
        void Update(string fileName);
    }
    /// <summary>
    /// Monitors a file for changes. It uses the FileSystemWatcher and debounce the events to make sure only one event is fired for a change of a file.
    /// Note:
    /// If a file is used in different contexts than FileMonitor will fire for each context.
    /// </summary>
    public class FileMonitor : IFileMonitor
    {
        const int EventObserveStartAfterMs = 100; // start checking after 200ms
        const int EventObserveNextIntervalMs = 100;    // check every 100ms
        const int TimeWitoutEventMs = 75;            // if 100ms without event than fire 

        readonly FileSystemWatcher _fileSystemWatcher = new FileSystemWatcher();

        /// <summary>
        /// The pending files. All file which are waiting for time in which no further event occurs
        /// </summary>
        readonly Dictionary<string, DateTime> _pendingEvents = new Dictionary<string, DateTime>();

        readonly Timer _timer;
        bool _timerStarted;
        string _filePath;

        /// <summary>
        /// Parameterize FileSystemWatcher
        /// </summary>
        /// <param name="filePath"></param>
        public FileMonitor(string filePath)
        {
            _filePath = filePath;
            _fileSystemWatcher.Path = Path.GetDirectoryName(filePath);
            _fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            _fileSystemWatcher.IncludeSubdirectories = false;
            _fileSystemWatcher.Changed += OnChange;
            _fileSystemWatcher.Error += OnFileError;

            // timer to debounce multiple events for the same file
            _timer = new Timer(OnTimeout, null, Timeout.Infinite, Timeout.Infinite);


        }
        /// <summary>
        /// Handle error from FileSystemWatcher
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        void OnFileError (object o, ErrorEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show($"Error file: '{_filePath}'\r\n{e.GetException().Message}");
        }
        // The event to fire
        public event FileSystemEvent Change;

        /// <summary>
        /// Start Watching the file
        /// </summary>
        public void Start()
        {
            try
            {
                _fileSystemWatcher.EnableRaisingEvents = true;
            } catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show($"File: {_filePath}'\r\n{e}", @"Invalid monitoring of file");
            }
        }
        /// <summary>
        /// Stop watching the file
        /// </summary>
        public void Stop()
        {
            _fileSystemWatcher.EnableRaisingEvents = false;
        }

        /// <summary>
        /// Update the fileName to watch for changes
        /// </summary>
        /// <param name="filePath"></param>
        public void Update(string filePath)
        {
            _filePath = filePath;
            _fileSystemWatcher.Path = Path.GetDirectoryName(filePath);
            _fileSystemWatcher.Filter = Path.GetFileName(filePath);
            _fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            _fileSystemWatcher.EnableRaisingEvents = true;

        }

        /// <summary>
        /// OnChange Event from System FileSystemWatcher after file has changed. It enters all events with the last event time in the pending event list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnChange(object sender, FileSystemEventArgs e)
        {
            // Don't want other threads messing with the pending events right now 
            lock (_pendingEvents)
            {
                // Save a timestamp for the most recent event for this filePath
                _pendingEvents[e.FullPath] = DateTime.Now;
                // Start a timer if not already started 
                if (!_timerStarted)
                {
                    // Start time + Interval in ms (after 100ms for 100ms)
                    _timer.Change(EventObserveStartAfterMs, EventObserveNextIntervalMs);
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
            // Don't want other threads messing with the pending events right now 
            lock (_pendingEvents)
            {
                if (! _timerStarted) return;
                // Get a list of all paths that are ready to fire
                var paths = FindReadyPaths(_pendingEvents);
                
                paths.ForEach(
                        (path) =>
                        {
                            _pendingEvents.Remove(path);
                        }
                ); 
                // Stop the timer if there are no more events pending 
                if (_pendingEvents.Count == 0)
                {
                    _timer.Change(Timeout.Infinite, Timeout.Infinite);
                    _timerStarted = false;

                }
                // Fire an event for each path that has changed paths
                paths.ForEach(
                    FireEvent
                 
                );
            
            }
        }

        /// <summary>
        /// Return list of files which haven't received an change event for TIME_WITOUT_EVENT_MS.
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        List<string> FindReadyPaths(Dictionary<string, DateTime> events)
        {
            List<string> results = new List<string>();
            DateTime now = DateTime.Now;
            foreach (KeyValuePair<string, DateTime> entry in events)
            {
                // If the path has not received a new event in the last 75ms 
                // an event for the path should be fired 
                double diff = now.Subtract(entry.Value).TotalMilliseconds;
                if (diff >= TimeWitoutEventMs)  results.Add(entry.Key);
            }
            return results;
        }

        void FireEvent(string path)
        {
            FileSystemEvent evt = Change;
            evt(path);
               
        }
    }
}
   