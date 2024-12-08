using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Unity.Collections.AllocatorManager;

public class CheckButton : MonoBehaviour
{
    public UnityEvent PuzzleSolved;
    public int[] correctOrder;
    public int[] pressedOrder;
    public List<ButtonBehavior> buttons;
    public int index = 0;
    public int correct = 0;
    public MeshRenderer mr;
    public Material norm;
    public Material pressed;
    public AudioClip lose;
    public AudioClip click;
    public AudioSource audio;

    public void Start()
    {
        pressedOrder = new int[6];
        mr.material = norm;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            audio.clip = click;
            audio.Play();
            mr.material = pressed;
            Check();
        }
    }
    public void Check()
    {
        for (int i = 0; i < pressedOrder.Length; i++)
        {
            if (pressedOrder[i] == correctOrder[i])
            {
                correct++;
            }
            else
            {
                ClearAll();
            }
        }
        if (correct >= 6)
        {
            PuzzleSolved.Invoke();
        }
    }
    public void AddtoOrder(int ButtonID)
    {
        if(index <= 5)
        {
            pressedOrder[index] = ButtonID;
            index++;
        }
        else
        {
            ClearAll();
        }

    }
    public void ClearAll()
    {
        audio.clip = lose;
        audio.Play();
        Debug.Log("Clearing All");
        pressedOrder = new int[6];
        for (int i = 0;i < pressedOrder.Length; i++)
        {
            buttons[i].resetMaterial();
        }
        index = 0;
        correct = 0;
        mr.material = norm;
    }
}
