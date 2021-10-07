using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.LepiStudios
{
    public enum ChatMessageTypes
    {
        Log,
        Warning,
        Error,
        Network
    }

    public class ChatMessage : UnityEngine.Object
    {
        public string Message { get; private set; }
        public ChatMessageTypes MessageType { get; private set; }

        public ChatMessage(string message, ChatMessageTypes messageType)
        {
            this.Message = message;
            this.MessageType = messageType;
        }



    }

}
