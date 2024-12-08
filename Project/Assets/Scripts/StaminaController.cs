 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    //https://www.youtube.com/watch?v=Fs2YCoamO_U
    public float playerStamina = 100f;

    public float maxStam = 100f;
    public bool isSprinting = false;


    [Range(0, 50)] [SerializeField] private float staminaDrain = 35f;
    [Range(0, 50)][SerializeField] private float staminaRegen = 30f;

    //private float slowedRun = 2f;
     
    [SerializeField] private Image staminaBar = null;
    [SerializeField] private GameObject sliderCanvasGroup;
    [SerializeField] private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        if (player.currSpeed < 15f)
        { 
            if (playerStamina < maxStam)
            {
                playerStamina = playerStamina + staminaRegen * Time.fixedDeltaTime;
                UpdateStamina(1);

                if (playerStamina >= maxStam)
                {
                    player.currSpeed = 10f;
                    sliderCanvasGroup.SetActive(false);
                }
            }


        }
    }

    public void Sprinting()
    {
                isSprinting = true;
                playerStamina -= (staminaDrain * Time.deltaTime);
                UpdateStamina(1);
        
    }
    public void UpdateStamina(int value)
    {
        staminaBar.fillAmount = playerStamina / 100f;

        if(value == 0)
        {
            sliderCanvasGroup.SetActive(false);
        }
        else
        {
            sliderCanvasGroup.SetActive(true);
        }
    }
}
