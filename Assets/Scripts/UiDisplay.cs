using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiDisplay : MonoBehaviour
{
    public Mag player;
    public EnemyController enemy;
    public TextMeshProUGUI healthText;
    private bool childFound = false;

    void Start(){
        foreach (Transform child in transform){
            if (child.name == "text_Health")
            {
                healthText = child.GetComponent<TextMeshProUGUI>();
                childFound = true;
            }
            else{
                childFound = false;
            }
        }

    }


    void Update(){
        if(childFound && player != null){
            healthText.text = "HP : " + player.GetCurrentHealth().ToString();
        }
        else if(childFound && enemy != null){
            healthText.text = enemy.GetCurrentHealth().ToString();
        }
        
    }
    
}
