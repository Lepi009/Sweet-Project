using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

namespace Com.LepiStudios.TutorialPhotonYoutube {

	public class CustomMMRoomController:MonoBehaviourPunCallbacks, IOnEventCallback
    {

        #region Private Serialization Fields

        [Tooltip("Scene index for loading multiplayer scene")]
        [SerializeField]
        private int multiplayerSceneIndex;

        [Tooltip("The lobby panel, displaying the lobby UI")]
        [SerializeField]
        private GameObject lobbyPanel;

        [Tooltip("The room panel, displaying the room UI")]
        [SerializeField]
        private GameObject roomPanel;

        [Tooltip("The start button, which starts the game and load the multiplayer scene, only for masterclient")]
        [SerializeField]
        private GameObject startButton;

        [Tooltip("The toggle if the players can join after starting the game")]
        [SerializeField] 
        private GameObject toggleJoinable;

        [Tooltip("The join button, which allows a new player to join the game that already has started")]
        [SerializeField]
        private GameObject joinButton;

        [Tooltip("The transform component to display all the players in the current room")]
        [SerializeField]
        private Transform playersContainer;

        [Tooltip("The prefab to display each player in the room")]
        [SerializeField]
        private GameObject playerListingPrefab;

        [Tooltip("The text component of the title to display the name of the room")]
        [SerializeField]
        private Text roomNameDisplay;

        [Tooltip("The text component of the player count to display how much players are actual in this room and the maximum amount")]
        [SerializeField]
        private Text playerCountDisplay;

        #endregion

        #region Private Fields

        [Tooltip("If players can join the room after the game starts, it should be true")]
        private bool roomOpenAfterStart = false;

        [Tooltip("Var safes if the game has already started, called by buffered event for players who join later")]
        private bool gameHasStarted = false;

        #endregion

        #region PunCallbacks

        /// <summary>
        /// Pun Callback for initializing the UI on joining the room
        /// </summary>
        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                ExitGames.Client.Photon.Hashtable test = new ExitGames.Client.Photon.Hashtable();
                test.Add("HasStarted", false);
                PhotonNetwork.CurrentRoom.SetCustomProperties(test);
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All, CachingOption = EventCaching.AddToRoomCache };
                SendOptions sendOptions = new SendOptions { Reliability = true };
                PhotonNetwork.RaiseEvent(2, false, raiseEventOptions, sendOptions);
            }

            roomPanel.SetActive(true);
            lobbyPanel.SetActive(false);
            roomNameDisplay.text = PhotonNetwork.CurrentRoom.Name; //displays the current room name in the title
            startButton.SetActive(PhotonNetwork.IsMasterClient); //shows the start button if we are the masterclient
            toggleJoinable.SetActive(PhotonNetwork.IsMasterClient);

            ClearPlayerListings();
            ListPlayers();
        }

        /// <summary>
        /// Pun Callback, refreshes the PlayerListings whenever a new player joins the room
        /// </summary>
        /// <param name="newPlayer"></param>
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            ClearPlayerListings();
            ListPlayers();
        }

        /// <summary>
        /// Pun Callback, refreshes the PlayerListings whenever a player leaves, also proofs if we are the master client now
        /// </summary>
        /// <param name="otherPlayer"></param>
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            ClearPlayerListings();
            ListPlayers();
            startButton.SetActive(PhotonNetwork.IsMasterClient);
            toggleJoinable.SetActive(PhotonNetwork.IsMasterClient);

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Button function paired to the start button. Will load all players into the multiplayer scene
        /// </summary>
        public void StartGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = roomOpenAfterStart; //comment out if you want to join player after the game start

                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All, CachingOption = EventCaching.RemoveFromRoomCache };
                SendOptions sendOptions = new SendOptions { Reliability = true };
                PhotonNetwork.RaiseEvent(2, false, raiseEventOptions, sendOptions);

                PhotonNetwork.LoadLevel(multiplayerSceneIndex);

                RaiseEventOptions raiseEventOptions2 = new RaiseEventOptions { Receivers = ReceiverGroup.All, CachingOption = EventCaching.AddToRoomCache };
                SendOptions sendOptions2 = new SendOptions { Reliability = true };
                PhotonNetwork.RaiseEvent(2, true, raiseEventOptions2, sendOptions2);

                ExitGames.Client.Photon.Hashtable test = new ExitGames.Client.Photon.Hashtable();
                test.Add("HasStarted", true);
                PhotonNetwork.CurrentRoom.SetCustomProperties(test);
                PhotonNetwork.CurrentRoom.SetPropertiesListedInLobby(new string[] { "HasStarted" });
            }
        }

        /// <summary>
        /// Button function paired to the back button in the room panel. Will return the player in to the lobby
        /// </summary>
        public void BackOnClick()
        {
            lobbyPanel.SetActive(true);
            roomPanel.SetActive(false);
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby(); //you have to leave and rejoin the lobby, otherwise the playerLisings will not be updated if you were the master client in the lefted room
            StartCoroutine(rejoinLobby());
        }

        public void ChangeToggleJoinableAfterStart(bool newValue)
        {
            roomOpenAfterStart = newValue;
        }

        public void JoinGame()
        {
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);
        }

        #endregion

        #region RPCs

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == 2)
            {
                Debug.Log("Game started? " + (bool)photonEvent.CustomData);
                if ((bool)photonEvent.CustomData)
                {
                    PhotonNetwork.AutomaticallySyncScene = false;
                    joinButton.SetActive(true);
                }
                else
                {
                    PhotonNetwork.AutomaticallySyncScene = true;
                }
            }

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// private method that clears the playersContainer
        /// </summary>
        private void ClearPlayerListings()
        {
            for(int i = playersContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(playersContainer.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// private method that fill up all players and refreshes the player count display
        /// </summary>
        private void ListPlayers()
        {
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                GameObject tempListing = Instantiate(playerListingPrefab, playersContainer);
                Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
                tempText.text = player.NickName;
            }
            float playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            float maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
            float fullPercentage = playerCount / maxPlayers;
            Image buttonColor = startButton.GetComponent<Image>();
            playerCountDisplay.text = string.Format("Player: {0}/{1}", playerCount, maxPlayers);

            if(PhotonNetwork.IsMasterClient)
            {
                if (fullPercentage < 0.5) buttonColor.color = Color.red;
                else if (fullPercentage < 1) buttonColor.color = Color.yellow;
                else buttonColor.color = Color.green;
            }
        }

        IEnumerator rejoinLobby()
        {
            yield return new WaitForSeconds(3);
            PhotonNetwork.JoinLobby();
        }

		#endregion
	}

}
