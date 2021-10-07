using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Com.LepiStudios.Network {

	public class RoomButton : MonoBehaviourPunCallbacks
	{

        #region Private Serialization Fields

        [Tooltip("Text component for displaying the room name")]
        [SerializeField]
        private Text nameText;

        [Tooltip("Text component for displaying the room size")]
        [SerializeField]
        private Text sizeText;

        [Tooltip("The image component of the background of the button")]
        [SerializeField]
        private Image backGround;

        #endregion

        #region Private Fields

        ///<summary>string for saving the room name</summary>
        private string roomName;

        ///<summary>int for saving the room size</summary>
        private int roomSize;

        ///<summary>int for saving player count</summary>
        private int playerCount;

        #endregion

        #region Public Fields

        /// <summary>
        /// button function paired the button that is the room listing. Joins the player to this room, the button represents
        /// </summary>
        public void JoinRoomOnClick()
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        /// <summary>
        /// method sets the parameters of the button and defines the UI texts
        /// </summary>
        /// <param name="nameInput">The room name</param>
        /// <param name="sizeInput">The maximum possible amount of players in the room</param>
        /// <param name="countInput">The actual players in this room</param>
        public void SetRoom(string nameInput, int sizeInput, int countInput)
        {
            this.roomName = nameInput;
            this.roomSize = sizeInput;
            this.playerCount = countInput;
            this.nameText.text = nameInput;
            this.sizeText.text = countInput + "/" + sizeInput;
            
            if(countInput >= sizeInput) //you cannot join the room anymore because it is full
            {
                sizeText.color = Color.red;
                GetComponent<Button>().interactable = false;
            }
        }

        public void ChangeColor()
        {
            backGround.color = Color.yellow;
        }

        #endregion
    }

}
