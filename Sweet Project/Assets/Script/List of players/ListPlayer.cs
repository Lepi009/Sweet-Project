using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class ListPlayer : MonoBehaviourPunCallbacks
{
    #region Public Fields

    [Tooltip("The text component of the quest describtion")]
    public Text questDescribtion;

    #endregion

    #region Serialized Fields

    [Tooltip("The key which the list can be showed")]
    [SerializeField]
    private KeyCode key;

    [Tooltip("The prefab for showing the player")]
    [SerializeField]
    private GameObject playerButton;

    [Tooltip("The container for display")]
    [SerializeField]
    private Transform displayContainer;


    #endregion

    #region Private Fields

    private Player[] playerList;

    private GameObject playerListPanel;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playerListPanel = this.gameObject.transform.GetChild(0).gameObject;
        HideList();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(key) && !playerListPanel.activeSelf)
        {
            ShowList();
        } 
        else if(Input.GetKeyDown(key) && playerListPanel.activeSelf)
        {
            HideList();
        }
    }

    #region Private Methods

    void ShowList()
    {
        playerListPanel.SetActive(true);
        RefreshList();
    }

    void HideList()
    {
        playerListPanel.SetActive(false);

    }

    void RefreshList()
    {
        ClearPlayerListings();
        playerList = PhotonNetwork.PlayerList;

        foreach (Player player in playerList)
        {
            GameObject go = Instantiate(playerButton, displayContainer);
            Text tempText = go.transform.GetChild(0).GetComponent<Text>();
            tempText.text = player.NickName;
        }
    }

    /// <summary>
    /// private method that clears the playersContainer
    /// </summary>
    private void ClearPlayerListings()
    {
        for (int i = displayContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(displayContainer.GetChild(i).gameObject);
        }
    }

    #endregion

    #region Photon Callbacks

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerList = PhotonNetwork.PlayerList;
        if (playerListPanel.activeSelf)
            RefreshList();

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        playerList = PhotonNetwork.PlayerList;
        if (playerListPanel.activeSelf)
            RefreshList();
    }

    #endregion

}
