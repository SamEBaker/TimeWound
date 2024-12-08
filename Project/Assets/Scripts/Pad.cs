 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Pad : MonoBehaviour
{
    public string Mykey;
    public bool PressedDown;
    public PressurePlateManager pm;
    public MeshRenderer mr;
    public Material material;
    public Material pressed;
    public AudioClip click;
    public AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        mr.material = material;
    }
     
    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.name == Mykey)
        {
            Debug.Log("I am pressed");
            audio.Play();
            mr.material = pressed;
            pm.Addpressed();
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        if ( collision.gameObject.name == Mykey)
        {
            Debug.Log("Block Upressed");
            pm.Removepressed();
            mr.material = material;
        }
    }
}
