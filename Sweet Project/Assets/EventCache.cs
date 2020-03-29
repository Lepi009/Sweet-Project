using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class EventCache : MonoBehaviourPun, IOnEventCallback
{
    public LinkedList<EventData> cache = new LinkedList<EventData>();

    public int eventCacheCount = 0;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code == 1)
        {
            cache.AddLast(photonEvent);
        }
    }
    private void Update()
    {
        eventCacheCount = cache.Count;
    }

    public EventData[] toArray()
    {
        EventData[] result = new EventData[cache.Count];
        LinkedListNode<EventData> pointer = cache.First;

        for(int i = 0; i < cache.Count; i++)
        {
            result[i] = pointer.Value;
            pointer = pointer.Next;
        }

        return result;
    }
    


}
