using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Com.LepiStudios.Network {

    /// <summary>
    /// class that is the component of the player button inside the room. You can click on the player to show the profile. this script make this possible
    /// </summary>
	public class PlayerButton : MonoBehaviour
	{
        #region Private Serialization Fields

        [Tooltip("The prefab of the player profile")]
        [SerializeField]
        private GameObject profilePrefab;

        [Tooltip("The text component that shows the name")]
        [SerializeField]
        private Text nameDisplay;

		#endregion

        #region Public Fields

        /// <summary>
        /// button function called whenever the player presses the player button. Show the player's profile
        /// </summary>
        public void ShowProfileOnClick()
        {
            if (profilePrefab == null) return;
            GameObject temp = Instantiate(profilePrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
            PlayerProfile profile = temp.GetComponent<PlayerProfile>();
            profile.SetPlayerProfile(nameDisplay.text);
        }

        #endregion

    }

}
