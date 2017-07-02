using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace HybridScriptingConnectEA
{

    /// <summary>
    /// Example to read the ROT (Object Running Table, COM Objects that are currently in use).
    /// 
    /// Every EA Instance has an entry in this table. This Class shows how to read the object table.
    /// The process id is used to identify the correct ROT entry.
    /// 
    /// </summary>
    public class ROT
    {
        void Main()
        {
            List<EAAppInstance> l = GetEAInstances();
            foreach (EAAppInstance el in l)
            {
                EA.App eaApp = el.EaApp;
                EA.Repository rep = eaApp.Repository;
                string name = rep.ConnectionString;
            }
        }



        [DllImport("ole32.dll")]
        public static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);

        [DllImport("ole32.dll")]
        public static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        public static List<EAAppInstance> GetEAInstances()
        {
            List<EAAppInstance> result = new List<EAAppInstance>();

            Hashtable runningObjects = GetRunningObjectTable();

            IDictionaryEnumerator rotEnumerator = runningObjects.GetEnumerator();
            while (rotEnumerator.MoveNext())
            {
                string candidateName = (string) rotEnumerator.Key;

                if (!candidateName.ToUpper().Contains("EA"))
                    continue;

                EA.App eaApplication = rotEnumerator.Value as EA.App;

                if (eaApplication == null)
                    continue;

                // Remember the process Id so we can Show that window to the user
                int processId = int.Parse(candidateName.Split(':')[1]);

                result.Add(new EAAppInstance(eaApplication, processId));
            }

            return result;
        }

        [STAThread]
        public static Hashtable GetRunningObjectTable()
        {
            Hashtable result = new Hashtable();

            IntPtr numFetched = new IntPtr();
            ;
            IRunningObjectTable runningObjectTable;
            IEnumMoniker monikerEnumerator;
            IMoniker[] monikers = new IMoniker[1];

            GetRunningObjectTable(0, out runningObjectTable);
            runningObjectTable.EnumRunning(out monikerEnumerator);
            monikerEnumerator.Reset();

            while (monikerEnumerator.Next(1, monikers, numFetched) == 0)
            {
                IBindCtx ctx;
                CreateBindCtx(0, out ctx);

                string runningObjectName;
                monikers[0].GetDisplayName(ctx, null, out runningObjectName);

                object runningObjectVal;
                runningObjectTable.GetObject(monikers[0], out runningObjectVal);

                result[runningObjectName] = runningObjectVal;
            }

            return result;
        }
    }

    /// <summary>
    /// An EA Instance with it's process ID
    /// </summary>
    public class EAAppInstance
    {
        public EA.App EaApp { get; }
        public int ProcessId { get; }
        public EAAppInstance(EA.App eaApp, int processId)
        {
        EaApp = eaApp;
        ProcessId = processId;
    }

    }
}
