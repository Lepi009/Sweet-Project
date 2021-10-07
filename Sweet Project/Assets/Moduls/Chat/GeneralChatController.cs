using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Com.LepiStudios.ScriptableObjects;

namespace Com.LepiStudios.myChatConsole
{

    /// <summary>
    /// This controller is the main interface to the "outside", so there are the settings how you can change your chat
    /// </summary>
	public class GeneralChatController : MonoBehaviour
	{
        #region Public Fields

        public UnityEvent OnChat;

        public UnityEvent OnDisableChat;

        [Tooltip("Key with which you can visible/invisible the chat")]
        public KeyCode key = KeyCode.T;

        [Tooltip("Time in seconds, how long the last messages should be shown")]
        public float showMessageTime = 3F;

        #endregion

        #region Private Serialization Fields

        [Tooltip("Chat is scene-independent")]
        [SerializeField]
        private bool dontDestroyOnLoad = true;
        
        [Tooltip("The output controller")]
        [SerializeField]
        private ChatOutputController output;

        [Tooltip("The input controller")]
        [SerializeField]
        private ChatInputController input;

        [Tooltip("The panel which inherits the chat system")]
        [SerializeField]
        private GameObject chatPanel;

        [Tooltip("The panel which inherits the teaser chat")]
        [SerializeField]
        private GameObject teaserPanel;

        [Tooltip("Font")]
        [SerializeField]
        private Font font;

        [Tooltip("Color of private messages")]
        [SerializeField]
        private Color privateMessageColor;

        [Tooltip("Color of public messages")]
        [SerializeField]
        private Color publicMessageColor;

        [Tooltip("Color of warning messages")]
        [SerializeField]
        private Color warningMessageColor;

        #endregion

        #region Private Fields

        /// <summary>var which saves the status of the visibility of the chat</summary>
        bool chatIsVisible;

        /// <summary>var saves the status whether the player is Writing or not </summary>
        bool isWritting = false;


        #endregion

        #region MonoBehaviour Callbacks

        ///<summary> MonoBehaviour method called on GameObject by Unity during initialization phase. </summary>
        void Start()
		{
            OnChat = new UnityEvent();
            OnDisableChat = new UnityEvent();

            GetAllInputFields(new Scene(), LoadSceneMode.Additive);
            SceneManager.sceneLoaded += GetAllInputFields;

            if (dontDestroyOnLoad) DontDestroyOnLoad(this.gameObject);

            //changes the fonts of all text components of the chat panel to the given
            Text[] textComponents = chatPanel.GetComponentsInChildren<Text>();
            foreach(Text textComponent in textComponents)
            {
                textComponent.font = font;
            }

            ShowChat(false); //in the beginning you cannot see the chat
		}

        void GetAllInputFields(Scene aSchene, LoadSceneMode aMode)
        {
            InputField[] gos = Resources.FindObjectsOfTypeAll<InputField>();

            foreach(InputField o in gos)
            {
                o.onValueChanged.AddListener(OnWritting);
                o.onEndEdit.AddListener(EndWritting);
            }
        }

		///<summary> MonoBehaviour method called on GameObject by Unity every frame. </summary>
		void Update()
		{
            if (Input.GetKeyDown(key) && !isWritting) //only if the player is not writing, you can change the visibility with the corresponding key
            {
                ShowChat(!chatIsVisible); //toggles the visibility
            }
            
		}

		#endregion

		#region Public Methods

        /// <summary>
        /// this method formats a message from Unity as a status update and send it to the chat output
        /// </summary>
        /// <param name="message">The message we want to display in the chat</param>
        public void Log(string message)
        {
            string format = MessageFormat(message, "", MessageType.Log);
            output.AddText(format);
        }

        /// <summary>
        /// this method formats a message from Unity as a status update and send it to the chat output
        /// </summary>
        /// <param name="message">The message we want to display in the chat</param>
        /// <param title="title>The title of the message</param>
        public void Log(string message, string title)
        {
            string format = MessageFormat(message, "", MessageType.Log, title);
            output.AddText(format);
        }

        public void Warning(string message)
        {
            string format = MessageFormat(message, "", MessageType.Warning);
            output.AddText(format);
        }

        /// <summary>
        /// This method sends a formated message to the chat output, but the message is from a transmitter (player)
        /// </summary>
        /// <param name="message">the message we want to display in the chat</param>
        /// <param name="visibility">the visibility of the message, private and public</param>
        public void RecieveMessage(string message, string transmitter, MessageType messageType)
        {
            output.AddText(MessageFormat(message, transmitter, messageType));
        }


        /// <summary>
        /// this method changes the visibility of the chat
        /// </summary>
        /// <param name="visible">true if the chat should be visible, false if the chat should be invisible</param>
        public void ShowChat(bool visible)
        {
            if (visible) OnChat.Invoke();
            else OnDisableChat.Invoke();

            chatPanel.SetActive(visible);
            teaserPanel.SetActive(!visible);
            chatIsVisible = visible;
            if (visible) input.focusOnInputfield();
        }

        /// <summary>
        /// this method changes the visibility of the chat but you can also write something inside the input field without sending it
        /// </summary>
        /// <param name="visible">true if the chat should be visible, false if the chat should be invisible</param>
        /// <param name="messageInput">the text that should be witten inside the inputfield without sending it</param>
        public void ShowChat(bool visible, string messageInput)
        {
            ShowChat(visible);
            input.WriteInInputField(messageInput);
        }

        /// <summary>
        /// method called by the OnChangeValue() method from the inputfield, so that the isWritting var can be changed to true
        /// </summary>
        public void OnWritting(string str)
        {
            isWritting = true;
        }

        /// <summary>
        /// method called by the OnEndEdit() method from the inputfield, so that we know the player is not writing anymore
        /// </summary>
        public void EndWritting(string str)
        {
            isWritting = false;
        }

        #endregion

        #region Private Methods


        /// <summary>
        /// This method formats the string and returns a string in this specific format
        /// </summary>
        /// <param name="message">The message we want to format</param>
        /// <param name="transmitter">The transmitter</param>
        /// <returns></returns>
        private string MessageFormat(string message, string transmitter, MessageType messageType)
        {
            Color colorMessage;
            switch(messageType)
            {
                case MessageType.Private: colorMessage = privateMessageColor; break;
                case MessageType.Public: colorMessage = publicMessageColor; break;
                case MessageType.Local: colorMessage = Color.black; break;
                case MessageType.Log: return string.Format("\n[{0}]  <Color=Yellow>Status Update</Color>\n\t{1}", System.DateTime.Now.ToString("HH:mm"), message);
                case MessageType.Warning: return string.Format("\n[{0}]  <Color=#{2}><b>Warning</b></Color>\n\t{1}", System.DateTime.Now.ToString("HH:mm"), message, ColorUtility.ToHtmlStringRGB(warningMessageColor));
                default: throw new System.NotImplementedException(); //combine the Log() method with this, so only this method carries about the format and colors
            }
            return string.Format("\n[{0}] <color=#{4}> From:{1}  <{3}>\n\t\"{2}\" </color>", System.DateTime.Now.ToString("HH:mm"), transmitter, message, messageType, ColorUtility.ToHtmlStringRGB(colorMessage));
        }

        /// <summary>
        /// This method formats the string and returns a string in this specific format
        /// </summary>
        /// <param name="message">The message we want to format</param>
        /// <param name="transmitter">The transmitter</param>
        /// <param name="title">the title of the message</param>
        /// <returns></returns>
        private string MessageFormat(string message, string transmitter, MessageType messageType, string title)
        {
            Color colorMessage;
            switch (messageType)
            {
                case MessageType.Private: colorMessage = privateMessageColor; break;
                case MessageType.Public: colorMessage = publicMessageColor; break;
                case MessageType.Local: colorMessage = Color.black; break;
                case MessageType.Log: return string.Format("\n[{0}]  <Color=Yellow>{2}</Color>\n\t{1}", System.DateTime.Now.ToString("HH:mm"), message, title);
                default: throw new System.NotImplementedException(); //combine the Log() method with this, so only this method carries about the format and colors
            }
            return string.Format("\n[{0}] <color=#{4}> From:{1}  <{3}>\n\t\"{2}\" </color>", System.DateTime.Now.ToString("HH:mm"), transmitter, message, messageType, ColorUtility.ToHtmlStringRGB(colorMessage));
        }


        #endregion

        #region Event Callbacks

        public void OnMessageDisplay(GameEventListenerWithParam paramListener)
        {
            Object param = (Object)paramListener.parameter;
            if(param is ChatMessage)
            {
                ChatMessage message = param as ChatMessage;
                
            }
        }

        #endregion

    }

}
