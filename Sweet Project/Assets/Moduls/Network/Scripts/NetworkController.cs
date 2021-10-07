using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LepiStudios.myChatConsole;
using Com.LepiStudios.ScriptableObjects;
using Photon.Pun;

namespace Com.LepiStudios.Network {

	public class NetworkController : MonoBehaviourPunCallbacks
	{

        #region Private Serialized Field

        [Tooltip("The event for sending for chats")]
        [SerializeField] GameEventWithParam eventChat;

        #endregion

        #region MonoBehaviour Callbacks

        ///<summary> MonoBehaviour method called on GameObject by Unity during initialization phase. </summary>
        void Start()
		{
            //easiest way to connect to Photon master servers
            PhotonNetwork.ConnectUsingSettings();
		}

        #endregion

        #region PunCallbacks

        /// <summary>
        /// Called by PUN after the connection is established
        /// </summary>
        public override void OnConnectedToMaster()
        {
            eventChat.Raise(new ChatMessage("We are now connected to the " + PhotonNetwork.CloudRegion + " server!", ChatMessageTypes.Network));
        }

        #endregion

    }

}
