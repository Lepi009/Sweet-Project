using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Com.LepiStudios.myChatConsole;

namespace Com.LepiStudios.TutorialPhotonYoutube {

    /// <summary>
    /// class that is the component of the playerprofilePrefab. Sets the player profile texts and actions.
    /// </summary>
	public class PlayerProfile : MonoBehaviour
	{

        #region Private Serialization Fields

        [Tooltip("The text component to display the player name")]
        [SerializeField]
        private Text playerNameDisplay;

        #endregion

        #region Private Fields

        ///<summary>var that saves the player name of this specific player profile</summary>
        private string playerName;

        #endregion

        #region Public Fields

        /// <summary>
        /// method that is like a constructor, sets all parameter for the profile
        /// </summary>
        /// <param name="playerName">the player name that belongs to this profile</param>
        public void SetPlayerProfile(string playerName)
        {
            playerNameDisplay.text = playerName;
            this.playerName = playerName;
        }

        /// <summary>
        /// button function called to text the player on this profile
        /// </summary>
        public void SendMessage()
        {
            GeneralChatController chatController = GameObject.FindGameObjectWithTag("Chat").GetComponent<GeneralChatController>(); //gets the chat controller
            chatController.ShowChat(true, "@p " + playerName + " "); //writes the command inside the chat
        }

        /// <summary>
        /// button function to destroy this profile
        /// </summary>
        public void DestroyProfile()
        {
            Destroy(gameObject);
        }

        #endregion

    }

}
