 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

//https://gamedevbeginner.com/how-to-make-countdown-timer-in-unity-minutes-seconds/#:~:text=Making%20a%20countdown%20timer%20in%20Unity%20involves%20storing%20a%20float#:~:text=Making%20a%20countdown%20timer%20in%20Unity%20involves%20storing%20a%20float
public class Timer : MonoBehaviour
{
    public UnityEvent GameOver;
    public float timeRemaining;
    public float TimePickupValue;
    public float maxTime = 120;
    public bool IsRunning = false; 
    public TMP_Text timerText1;
    public TMP_Text timerText2;
    public TMP_Text timerText3;
    public TMP_Text timerText4;
    public TMP_Text timerText5Overlay;
    public int GearsRecieved;
    public AudioSource audio;
    public AudioSource BGaudio;
    public AudioSource BOOM;
    public AudioClip RockRumble;
    //public AudioClip WarningBoom;
    public AudioClip WarningTicking;
    public AudioClip addTime;
    

    void Start()
    {
        //IsRunning = true;
    } 
    void Update()
    {
        if (IsRunning == true)
        {
            if (timeRemaining > 0)
            {
                if(timeRemaining <= 60 && timeRemaining > 59.9f)
                {
                    BOOM.Play();
                }
                if (timeRemaining <= 30 && timeRemaining > 29.9f)
                {
                    BOOM.Play();
                }
                if (timeRemaining <= 20 && timeRemaining > 19.9f)
                {
                    BOOM.Play();
                }
                if (timeRemaining <= 10 && timeRemaining > 9.9f)
                {
                    BOOM.Play();
                }

                timeRemaining -= Time.deltaTime;
                Displaytime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                IsRunning = false;
                GameOver.Invoke();
            }
        }
        else
        {
            BGaudio.Stop();
        }

        
    }
    public void StartTime()
    {
        IsRunning = true;
        BGaudio.Play();
        audio.clip = RockRumble;
        audio.Play();
    }

    public void EndTime()
    {
        IsRunning = false;
        BGaudio.Stop();
        //store timetremaining to gamemanager;
    }
    void Displaytime(float timeDisplay)
    {
        timeDisplay += 1;
        float min = Mathf.FloorToInt(timeDisplay / 60);
        float sec = Mathf.FloorToInt(timeDisplay % 60);

        timerText1.text = string.Format("{0:00}:{1:00}", min, sec);
        timerText2.text = string.Format("{0:00}:{1:00}", min, sec);
        timerText3.text = string.Format("{0:00}:{1:00}", min, sec);
        timerText4.text = string.Format("{0:00}:{1:00}", min, sec);
        timerText5Overlay.text = string.Format("{0:00}:{1:00}", min, sec);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Find the player GameObject and get the PlayerController component
            Player playerController = other.GetComponent<Player>();
            if(playerController.Gear != 0)
            {
                playerController.UseItem();
                AddTime();
            }
            else
            {
                //call player hud flashgear
            }
        }
    }

    public void CheatTime()
    {
        timeRemaining = 9000;
    }

    public void AddTime()
    {
        audio.clip = addTime;
        audio.Play();
        timeRemaining += TimePickupValue;
        GearsRecieved += 1;
        //cap timer at 160 add if timer is at full - 10 seconds to not take a gear
        //timeRemaining = Mathf.Min(timeRemaining, maxTime);
    }
}
