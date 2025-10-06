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
    public event Action PlayerOpenMenu;
    public void onPlayerOpenMenu()
    {
        if (PlayerOpenMenu != null)
        {
            PlayerOpenMenu();
        }
    }
    public event Action PlayerCloseMenu;
    public void onPlayerCloseMenu()
    {
        if (PlayerCloseMenu != null)
        {
            PlayerCloseMenu();
        }
    }
    public event Action PlayerOpenDebugMenu;
    public void onPlayerOpenDebugMenu()
    {
        if (PlayerOpenDebugMenu != null)
        {
            PlayerOpenDebugMenu();
        }
    }
    public event Action PlayerCloseDebugMenu;
    public void onPlayerCloseDebugMenu()
    {
        if (PlayerCloseDebugMenu != null)
        {
            PlayerCloseDebugMenu();
        }
    }
}
