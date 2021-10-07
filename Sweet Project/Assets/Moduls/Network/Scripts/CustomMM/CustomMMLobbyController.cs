using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using Com.LepiStudios.ScriptableObjects;

namespace Com.LepiStudios.Network {

    public class CustomMMLobbyController : MonoBehaviourPunCallbacks
    {

        #region Private Serialization Fields
        [Tooltip("Automatically scene sync (whenever the master client changes the scene, the others will change too")]
        [SerializeField] bool automaticSceneSync = true;

        [Tooltip("The event for sending for chats")]
        [SerializeField] GameEventWithParam eventChat;

        [Header("UI GameObjects")]
        [Tooltip("Button used for joining a Lobby")]
        [SerializeField]
        private GameObject lobbyConnectButton;

        [Tooltip("Panel for displaying lobby")]
        [SerializeField]
        private GameObject lobbyPanel;

        [Tooltip("Panel for displaying the main menu")]
        [SerializeField]
        private GameObject mainPanel;

        [Tooltip("InputField where players can change their name")]
        [SerializeField]
        private InputField playerNameInput;

        [Tooltip("Container for holding all the room listings")]
        [SerializeField]
        private Transform roomsContainer;

        [Tooltip("Prefab for displayer each room in the lobby")]
        [SerializeField]
        private GameObject roomListingPrefab;

        #endregion

        #region Private Fields

        ///<summary>string for saving the room name</summary>
        private string roomName;

        ///<summary>int for saving room size</summary>
        private int roomSize;

        ///<summary>list of all current rooms</summary>
        private List<RoomInfo> roomListings;

        #endregion

        #region PunCallbacks

        /// <summary>
        /// Callback function for when the first connection is established
        /// </summary>
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.AutomaticallySyncScene = automaticSceneSync; //true: whenever masterclient changes the scene, all clients will change too
            lobbyConnectButton.SetActive(true);
            roomListings = new List<RoomInfo>(); //initalizing roomListing

            //check for player name saved to player prefs
            if(PlayerPrefs.HasKey("NickName") && PlayerPrefs.GetString("NickName") != "")
            {
                PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");
            } else
            {
                PhotonNetwork.NickName = "Player" + Random.Range(0, 1000).ToString();
            }

            playerNameInput.text = PhotonNetwork.NickName; //update input field with player name
            playerNameInput.interactable = true;
            eventChat.Raise(new ChatMessage("Connected to Lobby", ChatMessageTypes.Network));
        }

        /// <summary>
        /// once in lobby, this method is called everytime the roomlist changes
        /// </summary>
        /// <param name="roomList"></param>
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            //clear all room listings
            roomListings.Clear();

            for(int i = roomsContainer.childCount-1; i >= 0; i--)
            {
                Destroy(roomsContainer.GetChild(i).gameObject); //destroys every room button inside the room container
            }

            //adds all open rooms
            foreach (RoomInfo room in roomList)
            {
                if (room.PlayerCount > 0) //only if the room has player in it, it will appear
                {
                    roomListings.Add(room);
                    ListRoom(room);
                }
            }
        }

        /// <summary>
        /// Callback function by PUN, called whenever CreateRoom() failed
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            eventChat.Raise(new ChatMessage("Tried to create a room but it failed, maybe there is a room existing with the same name", ChatMessageTypes.Error));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Method updates the player name from the input field, got called by the player name input
        /// </summary>
        /// <param name="nameInput"></param>
        public void PlayerNameUpdate(string nameInput)
        {
            PhotonNetwork.NickName = nameInput;
            PlayerPrefs.SetString("NickName", nameInput);
        }

        /// <summary>
        /// method called by the start button to join a lobby
        /// </summary>
        public void JoinLobbyOnClick()
        {
            mainPanel.SetActive(false);
            lobbyPanel.SetActive(true);
            PhotonNetwork.JoinLobby(); //first tries to join an existing room
        }

        /// <summary>
        /// input function for changing room name. paired to room InputField_Name
        /// </summary>
        /// <param name="nameIn"></param>
        public void OnRoomNameChanged(string nameIn)
        {
            this.roomName = nameIn;
        }

        /// <summary>
        /// input function for changing room size. paired to InputField_Size
        /// </summary>
        /// <param name="sizeIn"></param>
        public void OnRoomSizeChanged(string sizeIn)
        {
            roomSize = int.Parse(sizeIn);
        }

        /// <summary>
        /// button function paired to the create room button
        /// </summary>
        public void CreateRoom()
        {
            eventChat.Raise(new ChatMessage("Creating room", ChatMessageTypes.Network));
            if(roomSize <= 0)
            {
                roomSize = 1;
            }
            RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
            PhotonNetwork.CreateRoom(roomName, roomOps);
        }

        /// <summary>
        /// button function paired to the cancel button, leaves the lobby and changes panels
        /// </summary>
        public void MatchmakingCancel()
        {
            mainPanel.SetActive(true);
            lobbyPanel.SetActive(false);
            PhotonNetwork.LeaveLobby();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// displays new room listing for current room
        /// </summary>
        /// <param name="room"></param>
        private void ListRoom(RoomInfo room)
        {
            if(room.IsOpen && room.IsVisible)
            {
                GameObject tempListing = Instantiate(roomListingPrefab, roomsContainer);
                RoomButton tempButton = tempListing.GetComponent<RoomButton>();
                tempButton.SetRoom(room.Name, room.MaxPlayers, room.PlayerCount);


                object hasStarted;
                if (!room.CustomProperties.TryGetValue("HasStarted", out hasStarted)) return;
                if((bool)hasStarted)
                {
                    tempButton.ChangeColor();
                }
            }
        }

        #endregion
    }

}
