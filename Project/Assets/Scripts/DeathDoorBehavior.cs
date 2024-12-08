using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDoorBehavior : MonoBehaviour
{
    public int PlayerIDoor;
    public GameObject deadplayer;
    public GameObject spawnpoint;
    public GameManager gm;
    public AudioSource a;
     
    public void Unlock()
    {
        deadplayer.transform.position = spawnpoint.transform.position;
    }
    public void CheckUnlock()
    {
        if(gm.TotalGold >= 50)
        {
            Unlock();
            gm.SpendGold(50);
        }
        else{
            a.Play();
        }

    }
}
 