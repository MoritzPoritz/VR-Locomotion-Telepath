using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.IO;

namespace HAW.HCI.CheckpointSystem
{
    public class CsvWriter : MonoBehaviour
    {
        private TimeManager _timeManager;
        private DateTime _localDate = DateTime.Now;
        private CultureInfo _culture;

        void Start()
        {
            _timeManager = GetComponent<TimeManager>();
            _culture = new CultureInfo("de-DE");
        }

        /// <summary>
        /// Writes a csv-file with the current time in its name.
        /// On Android files are saved under /storage/emulated/0/Android/data/<packagename>/files
        /// on Windows under %userprofile%\AppData\Local\Packages\<productname>\LocalState
        /// </summary>
        public void WriteCSV()
        {
            string path = Application.persistentDataPath;
            string fileName = "Checkpoint_Results_" + _localDate.ToString("HH_mm_ss");
            string fullFileName = path + "/" + fileName + ".csv";

            TextWriter writer = new StreamWriter(fullFileName, false);
            writer.WriteLine("Checkpoint;TimeNeeded;skipped");

            int totalOfSkippedCheckpoints = 0;
            for (int i = 0; i < _timeManager.CheckpointTimes.Length; i++)
            {
                int checkpointNumber = i + 1;
                writer.WriteLine(checkpointNumber.ToString() + ';' + _timeManager.CheckpointTimes[i] + ';' +
                                 _timeManager.SkippedCheckpoints[i]);
                if (_timeManager.SkippedCheckpoints[i] == true)
                {
                    totalOfSkippedCheckpoints++;
                }
            }

            writer.WriteLine("Total Time Elapsed: " + _timeManager.TimeSinceLapStart + ';' + "Completed At: " +
                             _localDate.ToString(_culture) + ';' + "Checkpoints Skipped: " +
                             totalOfSkippedCheckpoints.ToString());
            writer.Close();
        }
    }
}
/*
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.IO;

namespace HAW.HCI.CheckpointSystem
{
    public class CsvWriter : MonoBehaviour
    {
        private TimeManager _timeManager;
        private DateTime _localDate = DateTime.Now;
        private CultureInfo _culture;

        void Start()
        {
            _timeManager = GetComponent<TimeManager>();
            _culture = new CultureInfo("de-DE");
        }

        /// <summary>
        /// Writes a csv-file with the current time in its name.
        /// On Android files are saved under /storage/emulated/0/Android/data/<packagename>/files
        /// on Windows under %userprofile%\AppData\Local\Packages\<productname>\LocalState
        /// </summary>
        public void WriteCSV()
        {
            string path = Application.persistentDataPath;
            string fileName = "Checkpoint_Results_" + _localDate.ToString("HH_mm_ss");
            string fullFileName = path + "/" + fileName + ".csv";

            TextWriter writer = new StreamWriter(fullFileName, false);
            writer.WriteLine("Checkpoint, TimeNeeded, skipped");

            int totalOfSkippedCheckpoints = 0;
            for (int i = 0; i < _timeManager.CheckpointTimes.Length; i++)
            {
                int checkpointNumber = i + 1;
                writer.WriteLine(checkpointNumber.ToString() + ',' + _timeManager.CheckpointTimes[i] + ',' +
                                 _timeManager.SkippedCheckpoints[i]);
                if (_timeManager.SkippedCheckpoints[i] == true)
                {
                    totalOfSkippedCheckpoints++;
                }
            }

            writer.WriteLine("Total Time Elapsed: " + _timeManager.TimeSinceLapStart + ',' + "Completed At: " +
                             _localDate.ToString(_culture) + ',' + "Checkpoints Skipped: " +
                             totalOfSkippedCheckpoints.ToString());
            writer.Close();
        }
    }
}*/