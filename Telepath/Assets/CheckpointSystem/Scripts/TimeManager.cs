using System.Text;
using UnityEngine;

namespace HAW.HCI.CheckpointSystem
{
    /// <summary>
    /// Behaviour that keeps track of the time
    /// </summary>
    public class TimeManager : MonoBehaviour
    {

        #region Serialized Fields

        /// <summary> checkpoints of the track </summary>
        [Tooltip("Checkpoints of the track")]
        [SerializeField]
        private Checkpoint[] checkpoints = null;

        [Tooltip("The Player")]
        [SerializeField]
        private Transform player;

        #endregion

        #region Constants

        /// <summary> How many seconds are in a minute </summary>
        private const float SECONDS_IN_MINUTE = 60.0f;

        /// <summary> How many milliseconds are in a second </summary>
        private const float MILLISECONDS_IN_SECOND = 1000.0f;

        #endregion

        #region Private Attributes

        /// <summary> The time since the lap started </summary>
        private float timeSinceLapStart = 0.0f;

        private float timeAtLastCheckpoint = 0.0f;

        /// <summary> the currently active checkpoint </summary>
        private int currentCheckpoint = 0;

        /// <summary> Flag whether the timer started </summary>
        private bool timerStarted = false;

        /// <summary> Saves the time for each checkpoint </summary>
        private float[] checkpointTimes = null;

        /// <summary> Saves which Checkpoints have been skipped </summary>
        private bool[] skippedCheckpoints = null;

        private CsvWriter _csvWriter;

        #endregion

        #region Accessors

        public float TimeSinceLapStart => timeSinceLapStart;

        public float[] CheckpointTimes => checkpointTimes;

        public bool[] SkippedCheckpoints => skippedCheckpoints;

        #endregion



        #region MonoBehaviour implementation

        /// <summary>
        /// Activates the first and deactivates all other checkpoints
        /// </summary>
        private void Start()
        {
            Debug.Assert(checkpoints.Length >= 2);

            checkpoints[0].gameObject.SetActive(true);
            for (int i = 1; i < checkpoints.Length; i++)
            {
                checkpoints[i].gameObject.SetActive(false);
            }

            checkpointTimes = new float[checkpoints.Length];
            skippedCheckpoints = new bool[checkpoints.Length];
            _csvWriter = GetComponent<CsvWriter>();
        }

        /// <summary>
        /// Updates the time since lap start
        /// </summary>
        private void FixedUpdate()
        {
            if (timerStarted)
            {
                timeSinceLapStart += Time.fixedDeltaTime;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Is called when a checkpoint gets triggered
        /// takes the time of the current sector and starts the next one
        /// </summary>
        public void OnTrigger(bool skip, GameObject triggeredCheckpoint)
        {
            checkpoints[currentCheckpoint].gameObject.SetActive(false);
            if (skip)
            {
                checkpoints[currentCheckpoint].gameObject.SetActive(true);
                float yPos = checkpoints[currentCheckpoint].transform.position.y -
                             checkpoints[currentCheckpoint].GetComponent<Collider>().bounds.size.y / 2f;
                checkpoints[currentCheckpoint].gameObject.SetActive(false);
                triggeredCheckpoint.SetActive(false);
                player.position = new Vector3(checkpoints[currentCheckpoint].transform.position.x, yPos, checkpoints[currentCheckpoint].transform.position.z);
                skippedCheckpoints[currentCheckpoint] = true;
            }


            currentCheckpoint = (currentCheckpoint + 1) % checkpoints.Length;
            switch (currentCheckpoint)
            {
                case 0:
                    timerStarted = false;
                    checkpointTimes[checkpointTimes.Length - 1] = timeSinceLapStart - timeAtLastCheckpoint;

                    Debug.Log("Needed Time:");
                    Debug.Log(FormatTime(timeSinceLapStart));

                    _csvWriter.WriteCSV();

                   // UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);

                    break;
                case 1:
                    checkpoints[currentCheckpoint].gameObject.SetActive(true);
                    timerStarted = true;
                    checkpointTimes[0] = 0.0f;
                    Debug.Log("Timer has started");
                    break;
                default:
                    checkpoints[currentCheckpoint].gameObject.SetActive(true);
                    checkpointTimes[currentCheckpoint - 1] = timeSinceLapStart - timeAtLastCheckpoint;
                    timeAtLastCheckpoint = timeSinceLapStart;
                    Debug.Log("Reached another Checkpoint");
                    break;
            }
        }

        /// <summary>
        /// Creates a string for a time in format: "*MM:SS:mmm"
        /// </summary>
        /// <param name="time"> time in seconds </param>
        /// <returns>time in format: "*MM:SS:mmm"</returns>
        public static string FormatTime(float time)
        {
            StringBuilder timeString = new StringBuilder();
            int minutes = (int)(time / SECONDS_IN_MINUTE);
            if (minutes < 10)
            {
                timeString.Append('0');
            }
            timeString.Append(minutes);
            timeString.Append(':');

            int seconds = (int)(time % SECONDS_IN_MINUTE);
            if (seconds < 10)
            {
                timeString.Append('0');
            }
            timeString.Append(seconds);
            timeString.Append(':');

            int milliseconds = (int)(time * MILLISECONDS_IN_SECOND % MILLISECONDS_IN_SECOND);
            if (milliseconds < 100)
            {
                timeString.Append('0');
            }
            if (milliseconds < 10)
            {
                timeString.Append('0');
            }
            timeString.Append(milliseconds);

            return timeString.ToString();
        }

        #endregion

    }
}
/*
namespace HAW.HCI.CheckpointSystem
{

    /// <summary>
    /// Behaviour that keeps track of the time
    /// </summary>
    public class TimeManager : MonoBehaviour
    {

        #region Serialized Fields

        /// <summary> checkpoints of the track </summary>
        [Tooltip("Checkpoints of the track")]
        [SerializeField]
        private Checkpoint[] checkpoints = null;

        [Tooltip("The Player")]
        [SerializeField]
        private Transform player;

        #endregion

        #region Constants

        /// <summary> How many seconds are in a minute </summary>
        private const float SECONDS_IN_MINUTE = 60.0f;

        /// <summary> How many milliseconds are in a second </summary>
        private const float MILLISECONDS_IN_SECOND = 1000.0f;

        #endregion

        #region Private Attributes

        /// <summary> The time since the lap started </summary>
        private float timeSinceLapStart = 0.0f;

        /// <summary> the currently active checkpoint </summary>
        private int currentCheckpoint = 0;

        /// <summary> Flag whether the timer started </summary>
        private bool timerStarted = false;
        
        /// <summary> Saves the time for each checkpoint </summary>
        private float[] checkpointTimes = null;

        /// <summary> Saves which Checkpoints have been skipped </summary>
        private bool[] skippedCheckpoints = null;

        private CsvWriter _csvWriter;

        #endregion
        
        #region Accessors

        public float TimeSinceLapStart => timeSinceLapStart;
        
        public float[] CheckpointTimes => checkpointTimes;
        
        public bool[] SkippedCheckpoints => skippedCheckpoints;
        
        #endregion

        
        
        #region MonoBehaviour implementation

        /// <summary>
        /// Activates the first and deactivates all other checkpoints
        /// </summary>
        private void Start()
        {
            Debug.Assert(checkpoints.Length >= 2);

            checkpoints[0].gameObject.SetActive(true);
            for (int i = 1; i < checkpoints.Length; i++)
            {
                checkpoints[i].gameObject.SetActive(false);
            }

            checkpointTimes = new float[checkpoints.Length];
            skippedCheckpoints = new bool[checkpoints.Length];
            _csvWriter = GetComponent<CsvWriter>();
        }

        /// <summary>
        /// Updates the time since lap start
        /// </summary>
        private void FixedUpdate()
        {
            if (timerStarted)
            {
                timeSinceLapStart += Time.fixedDeltaTime;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Is called when a checkpoint gets triggered
        /// takes the time of the current sector and starts the next one
        /// </summary>
        public void OnTrigger(bool skip, GameObject triggeredCheckpoint)
        {
            checkpoints[currentCheckpoint].gameObject.SetActive(false);
            if (skip)
            {
                checkpoints[currentCheckpoint].gameObject.SetActive(true);
                float yPos = checkpoints[currentCheckpoint].transform.position.y -
                             checkpoints[currentCheckpoint].GetComponent<Collider>().bounds.size.y / 2f;
                checkpoints[currentCheckpoint].gameObject.SetActive(false);
                triggeredCheckpoint.SetActive(false);
                player.position = new Vector3(checkpoints[currentCheckpoint].transform.position.x, yPos, checkpoints[currentCheckpoint].transform.position.z);
                skippedCheckpoints[currentCheckpoint] = true;
            }

            
            currentCheckpoint = (currentCheckpoint + 1) % checkpoints.Length;
            switch (currentCheckpoint)
            {
                case 0:
                    timerStarted = false;
                    checkpointTimes[checkpointTimes.Length - 1] = timeSinceLapStart - checkpointTimes[checkpointTimes.Length - 2];

                    Debug.Log("Needed Time:");
                    Debug.Log(FormatTime(timeSinceLapStart));
                    
                    _csvWriter.WriteCSV();
                    
                    break;
                case 1:
                    checkpoints[currentCheckpoint].gameObject.SetActive(true);
                    timerStarted = true;
                    checkpointTimes[0] = 0.0f;
                    Debug.Log("Timer has started");
                    break;
                default:
                    checkpoints[currentCheckpoint].gameObject.SetActive(true);
                    checkpointTimes[currentCheckpoint - 1] = timeSinceLapStart - checkpointTimes[currentCheckpoint - 2];
                    Debug.Log("Reached another Checkpoint");
                    break;
            }
        }

        /// <summary>
        /// Creates a string for a time in format: "*MM:SS:mmm"
        /// </summary>
        /// <param name="time"> time in seconds </param>
        /// <returns>time in format: "*MM:SS:mmm"</returns>
        public static string FormatTime(float time)
        {
            StringBuilder timeString = new StringBuilder();
            int minutes = (int)(time / SECONDS_IN_MINUTE);
            if (minutes < 10)
            {
                timeString.Append('0');
            }
            timeString.Append(minutes);
            timeString.Append(':');

            int seconds = (int)(time % SECONDS_IN_MINUTE);
            if (seconds < 10)
            {
                timeString.Append('0');
            }
            timeString.Append(seconds);
            timeString.Append(':');

            int milliseconds = (int)(time * MILLISECONDS_IN_SECOND % MILLISECONDS_IN_SECOND);
            if (milliseconds < 100)
            {
                timeString.Append('0');
            }
            if (milliseconds < 10)
            {
                timeString.Append('0');
            }
            timeString.Append(milliseconds);

            return timeString.ToString();
        }

        #endregion

    }
}*/