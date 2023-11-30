using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {

    public int currentGold;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI healthText;

    void Start() {
        
    }

    void Update() {
        
    }

    public void AddGold(int goldToAdd) {
        currentGold += goldToAdd;
        goldText.text = "Gold: " + currentGold;
    }

    public void UpdateHealth(int currentHealth, int maxHealth, bool dead) {
        if(dead) {
            healthText.text = "Respawning...";
        } else {
            healthText.text = "Health: " + currentHealth + "/" + maxHealth;
        }
        
    }
}
