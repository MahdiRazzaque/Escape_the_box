using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {

    public int currentGold;
    public TextMeshProUGUI goldText;

    void Start() {
        
    }

    void Update() {
        
    }

    public void AddGold(int goldToAdd) {
        currentGold += goldToAdd;
        goldText.text = "Gold: " + currentGold;
    }
}
