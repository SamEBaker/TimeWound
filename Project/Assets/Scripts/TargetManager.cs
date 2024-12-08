using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TargetManager : MonoBehaviour
{

    public List<GameObject> targets;
    public int HitTargets = 0;
    public int HitsNeeded;
    public bool DoorOpen = false;
    public UnityEvent Solved;
    public float TimeToSolve = 10f;
    public bool TimerStarted = false;
    public AudioSource a;


    public void Start()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        if(!TimerStarted){
            TimerStarted = true;
            TimeToSolve -= Time.deltaTime;
        }
    }

    public void Update()
    {
        if(TimerStarted)
        {
            if(TimeToSolve > 0)
            {
                TimeToSolve -= Time.deltaTime;
            }
            else
            {
                TimerStarted = false;
                StartCoroutine(ClearAllBuffer());
            }
        }
    }
    public void CheckSolved()
    {
        if(HitTargets == HitsNeeded)
        {
            TimerStarted = false;
            Solved.Invoke(); 
        }

    }
    public void HitTarget()
    {
        HitTargets++;
        CheckSolved();
    }

    public void ClearAll()
    {
        a.Play();
        for(int i = 0; i < targets.Count; i++)
        {
            targets[i].GetComponent<Targets>().Clear();
        }
        HitTargets = 0;
        TimeToSolve = 10f;
    }
    public IEnumerator ClearAllBuffer()
    {
        ClearAll();
        yield return new WaitForSeconds(2f);
        TimerStarted = true;
    }
}
