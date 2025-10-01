using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    private void Awake()
    {
        current = this;
    }

    public event Action RoomRewardInteract;
    public void onRoomRewardInteract()
    {
        if (RoomRewardInteract != null)
        {
            RoomRewardInteract();
        }
    }
    public event Action RoomRewardClose;
    public void onRoomRewardClose()
    {
        if (RoomRewardClose != null)
        {
            RoomRewardClose();
        }
    }
}
