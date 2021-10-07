using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

namespace Com.LepiStudios.myChatConsole{

	public class ChatNetwork : MonoBehaviourPunCallbacks
	{

        #region Private Serialization Fields

        [SerializeField]
        private GeneralChatController chatController;

        #endregion

        #region PunRPC Callbacks

        /// <summary>
        /// a rpc called to all players. They receive a message from an transmitter
        /// </summary>
        /// <param name="message">the public message to all in this room</param>
        /// <param name="info">the info of the message, contains the sender</param>
        [PunRPC]
        void Rpc_MessageToAll(string message, PhotonMessageInfo info)
        {
            chatController.RecieveMessage(message, info.Sender.NickName, MessageType.Public);
        }

        /// <summary>
        /// RPC called by only one specific player, the player to which this message is to
        /// </summary>
        /// <param name="message">the private message from info.Sender</param>
        /// <param name="info">the info of the message, contains the sender</param>
        [PunRPC]
        void Rpc_MessageToOne(string message, PhotonMessageInfo info)
        {
            chatController.RecieveMessage(message, info.Sender.NickName, MessageType.Private);
        }

        #endregion

        #region Public Methods

        public void SendMessageToAll(string message)
        {
            if (!PhotonNetwork.InRoom)
            {
                chatController.Warning("You are currently not in a room, so you need to join one");
                return;
            }
            photonView.RPC(nameof(Rpc_MessageToAll), RpcTarget.All, message);
        }

        public void SendMessageToOnePlayer(string message, string targetPlayerName)
        {
            if (!PhotonNetwork.InRoom)
            {
                chatController.Warning("You are currently not in a room, so you need to join one");
                return;
            }
            Player[] players = PhotonNetwork.PlayerListOthers;
            int matchingPlayers = 0;
            Player playerToSendMessage = null;
            foreach(Player player in players)
            {
                if (player.NickName.Equals(targetPlayerName))
                {
                    playerToSendMessage = player;
                    matchingPlayers++;
                }
            }
            if (matchingPlayers == 0) chatController.Warning("No player currently in this room with the name " + targetPlayerName);
            else if (matchingPlayers == 1)
            {
                photonView.RPC(nameof(Rpc_MessageToOne), playerToSendMessage, message);
                chatController.Log(message, "Sended to " + playerToSendMessage);
            }
            else chatController.Warning("Error has happend, no message could send to " + targetPlayerName);

            if(PhotonNetwork.NickName.Equals(targetPlayerName))
            {
                chatController.Warning("Ohh dude do not write private messages to yourself, this is very sad, you really need friends, go out and find some");
            }
        }

        #endregion

    }

}
