using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Com.LepiStudios.myChatConsole {

	public class ChatOutputController : MonoBehaviour
	{

        #region Private Serialization Fields

        [Tooltip("Text component which is the output")]
        [SerializeField]
        private Text outputText;

        [Tooltip("Text component of the teaser chat")]
        [SerializeField]
        private Text teaserText;

        [Tooltip("General Chat Controller")]
        [SerializeField]
        private GeneralChatController generalChatController;

        #endregion

        #region Private Fields

        /// <summary>var to save the chat content</summary>
        private string chatContent = "";

        /// <summary>var to save the teaser chat content</summary>
        private string teaserChatContent = "";

        /// <summary>var to save how long a message after receiving should be shown, can be set inside the general chat controller</summary>
        private float showMessageTime = 3F;

        #endregion

        #region MonoBehaviour Callbacks

        ///<summary> MonoBehaviour method called on GameObject by Unity during early initialization phase. </summary>
        void Awake()
		{
            ResetText(); //in the very beginning, the chat is gonna resetted	
		}

        /// <summary>
        /// MonoBehaviour method called on GameObjects if they change from disabled to enabled, in this case when the chat is becoming visible
        /// </summary>
        private void Start()
        {
            UpdateShowView(); //chat output has to be updated
            showMessageTime = generalChatController.showMessageTime;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// public method which resets the output text to ""
        /// </summary>
        public void ResetText()
        {
            chatContent = "";
            teaserChatContent = "";
            UpdateShowView();
        }

        /// <summary>
        /// adds the referred param to the output chat
        /// </summary>
        /// <param name="newLine"></param>
        public void AddText(string message)
        { 
            chatContent += message;

            AddTeaserContent(message);

            UpdateShowView();

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// updates the text component to display the newest messages
        /// </summary>
        private void UpdateShowView()
        {
            outputText.text = chatContent;
            if(teaserText.enabled)
            {
                teaserText.text = teaserChatContent;
            }
        }

        /// <summary>
        /// adds the message to the teaser chat, only if the teaser chat is shown, start a timer to delete the teaser
        /// </summary>
        /// <param name="message"></param>
        private void AddTeaserContent(string message)
        {
            teaserChatContent += message;
            StartCoroutine(TeaserRemoveMessage(message));
        }

        /// <summary>
        /// coroutine to delete the message in the teaser content text after a specific time
        /// </summary>
        /// <param name="message">the message that is showed</param>
        /// <returns></returns>
        IEnumerator TeaserRemoveMessage(string message)
        {
            yield return new WaitForSeconds(showMessageTime);
            if(teaserChatContent != "") teaserChatContent = teaserChatContent.Remove(0, message.Length); //if the chat was cleared but there are some coroutines still running, it cannot delete them and it throws an ArgumentOutOfRangeException
            UpdateShowView();
        }

        #endregion
    }

}
