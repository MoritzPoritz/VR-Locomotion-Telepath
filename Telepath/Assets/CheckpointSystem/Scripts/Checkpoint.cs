using UnityEngine;

namespace HAW.HCI.CheckpointSystem
{
    /// <summary>
    /// Checkpoint to determine timing
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Checkpoint : MonoBehaviour
    {

        #region Serialized Fields

        /// <summary> TimeManager of the scene </summary>
        [Tooltip("TimeManager of the scene")]
        [SerializeField]
        private TimeManager timeManager;

        [Tooltip("Whether the Checkpoint is to skip to the next one")]
        [SerializeField]
        private bool skip;

        #endregion

        #region Trigger implementation

        /// <summary>
        /// Is called when the player enters this trigger
        /// Calls the OnTrigger() Method of the TimeManager
        /// </summary>
        /// <param name="other"> collider of the car </param>
        private void OnTriggerEnter(Collider other)
        {
            if (skip && other.gameObject.GetComponentInParent<TelepathDrawing>() != null)
            {
                
                other.gameObject.GetComponentInParent<TelepathDrawing>().ResetLine();
            }
            timeManager.OnTrigger(skip, this.gameObject);
            
        }

        #endregion

    }
}