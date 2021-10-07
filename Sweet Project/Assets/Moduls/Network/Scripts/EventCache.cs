using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class EventCache : MonoBehaviourPun, IOnEventCallback
{
    public LinkedList<EventData> cache = new LinkedList<EventData>();

    [Tooltip("Event ids to cache")]
    [SerializeField] List<int> eventsToCache;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        if(eventsToCache.Contains(photonEvent.Code))
        {
            cache.AddLast(photonEvent);
        }
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
