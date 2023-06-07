using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
    private HealthMngr healthMngr;

    public Image HealthBar;

    private void Start()
    {
        //take the component by name in the scene
        HealthBar = GameObject.Find("HealthBar").GetComponent<Image>();

        //used for take reference of health
        healthMngr = FindAnyObjectByType<HealthMngr>();
    }
    private void Update()
    {
        HealthBar.fillAmount = healthMngr.CurrentHealth / 100f;
    }

}
