using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LepiStudios.myChatConsole;

using Photon.Pun;

namespace Com.LepiStudios.TutorialPhotonYoutube {

	public class NetworkController : MonoBehaviourPunCallbacks
	{
        private GeneralChatController chat;

		#region MonoBehaviour Callbacks

		///<summary> MonoBehaviour method called on GameObject by Unity during initialization phase. </summary>
		void Start()
		{
            chat = GameObject.FindGameObjectWithTag("Chat").GetComponent<GeneralChatController>(); //gets the controller script from the chat	
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
            chat.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " server!");
        }

        #endregion

    }

}
