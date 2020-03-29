using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.LepiStudios.myChatConsole
{

	public class ChatInputController : MonoBehaviour
	{

        #region Private Serialization Fields

        [Tooltip("The Inputfield for the chat")]
        [SerializeField]
        private InputField input;

        [Tooltip("Reference to the instance of the ChatOutPutController")]
        [SerializeField]
        private ChatOutputController output;

        [Tooltip("The general chat controller")]
        [SerializeField]
        private GeneralChatController chatController;

        [Tooltip("Reference to the chat network script")]
        [SerializeField]
        private ChatNetwork chatNetwork;

        #endregion

        #region Public Methods

        /// <summary>
        /// sends the text from the inputfield to the output
        /// </summary>
        public void SendText()
        {
            string inputText = input.text;
            if (inputText == "") return;
            input.text = "";

            focusOnInputfield();
            if (proofInput(inputText)) return; //if this is a command, it should not be sended to the output
            //output.EingabeInputField(inputText);
            chatController.RecieveMessage(inputText, "You", MessageType.Local);
        }

        /// <summary>
        /// make it possible to send the text of the inputfield by pressing enter, must be invoked by OnEndEdit() from the inputfield
        /// </summary>
        public void OnEnterSend()
        {
            if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) SendText();
        }

        /// <summary>
        /// writes a text inside the input field without sending it
        /// </summary>
        /// <param name="text"></param>
        public void WriteInInputField(string text)
        {
            input.text = text;
        }

        /// <summary>
        /// method focus on the inputfield 
        /// </summary>
        public void focusOnInputfield()
        {
            input.ActivateInputField();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// method that proofs if the written text is a command and run them
        /// </summary>
        /// <param name="text">the text that should be proofen</param>
        private bool proofInput(string text)
        {
            if (!text.StartsWith("@")) return false; //@ steht für einen befehl, ohne @ ist es eine normale Nachrich

            if(text.StartsWith("@a"))
            {
                chatNetwork.SendMessageToAll(text.Substring(2));
            }
            else if(text.StartsWith("@p"))
            {
                string[] messageParts = text.Split(new char[] { ' ' }, 3);
                if (messageParts.Length < 3)
                {
                    chatController.Warning("There are few arguments for this command");
                    return true;
                }

                string otherPlayerName = messageParts[1];
                chatNetwork.SendMessageToOnePlayer(messageParts[2], otherPlayerName);
            }
            else if(text.StartsWith("@c"))
            {
                output.ResetText();
            } else if(text.StartsWith("@h"))
            {
                chatController.Log(string.Format("@a <i>text</i>: sends a message to all inside this room\n\t@p <i>NickName text</i>: sends a private message to the person\n\t@c: clears the chat"), "Here is a list of all general commands");
            }
            else
            {
                //an unknown command
                chatController.Warning(string.Format("The command {0} is unknown.", text.Substring(0, 2)));
            }
            return true;
        }

        #endregion
    }

}
