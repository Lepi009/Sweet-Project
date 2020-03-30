using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace Com.LepiStudios.TutorialPhotonYoutube {

    /// <summary>
    /// will be added to any multiplayer scene, does care for Instantiation,...
    /// </summary>
	public class GameSetupController : MonoBehaviourPunCallbacks, IOnEventCallback
	{
        #region Serialized Fields

        [Tooltip("The prefab of the player")]
        [SerializeField]
        private GameObject PlayerPrefab;

        #endregion

        private EventCache cache;

        #region MonoBehaviour Callbacks

        ///<summary> MonoBehaviour method called on GameObject by Unity during initialization phase. </summary>
        void Awake()
		{
            if (!PhotonNetwork.IsConnectedAndReady)
                PhotonNetwork.ConnectUsingSettings();
            else
                CreatePlayer();

            cache = FindObjectOfType<EventCache>();
            if(cache != null)
            {
                EventData[] events = cache.toArray();
                foreach(EventData e in events) {
                    OnEvent(e);
                    cache.cache.Remove(e);
                }
            }
		}

        #endregion

        #region Pun Callbacks

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinRandomRoom();
            PhotonNetwork.NickName = "Lepi009";
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to connect:  " + returnCode + "\nCreating a room");
            PhotonNetwork.CreateRoom("One", new RoomOptions());
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to connect:  " + returnCode);
            Debug.Break();
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            Debug.Log("Joined room");
            CreatePlayer();
        }

        #endregion

        public void CreatePlayer()
        {
            GameObject player = Instantiate(PlayerPrefab);
            PhotonView photonView = player.GetComponent<PhotonView>();

            if (PhotonNetwork.AllocateViewID(photonView))
            {
                object[] data = new object[] { photonView.ViewID };

                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others, CachingOption = EventCaching.AddToRoomCache };

                SendOptions sendOptions = new SendOptions { Reliability = true };

                PhotonNetwork.RaiseEvent(1, data, raiseEventOptions, sendOptions);
            }
            else
            {
                Debug.LogError("Failed to allocate a ViewID");
                Destroy(player);
            }            
        }

        public void OnEvent(EventData photonEvent)
        {
            if(photonEvent.Code == 1)
            {
                object[] data = (object[])photonEvent.CustomData;

                GameObject player = Instantiate(PlayerPrefab);
                PhotonView photonView = player.GetComponent<PhotonView>();
                photonView.ViewID = (int)data[0];
            }
        }

        #region RPC

        /*[PunRPC]
        void RPC_SpawnPlayer(int playerPhotonViewID)
        {
            GameObject player = Instantiate(PlayerPrefab);

            PhotonView photonViewPlayer = player.GetComponent<PhotonView>();
            photonViewPlayer.ViewID = playerPhotonViewID;
            player.name = "Other: " + photonViewPlayer.Owner.NickName;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creating the player, so instantiation
        /// </summary>
        private void CreatePlayer()
        {
            GameObject player = Instantiate(PlayerPrefab);
            player.name = "Me: " + PhotonNetwork.NickName;

            PhotonView photonView = player.GetComponent<PhotonView>();

            if(PhotonNetwork.AllocateViewID(photonView))
            {
                photonView.RPC(nameof(RPC_SpawnPlayer), RpcTarget.OthersBuffered, photonView.ViewID);
            } 
            else
            {
                Debug.LogError("Failed to allocate a ViewID");
                Destroy(player);
            }
        }*/

		#endregion
	}

}
