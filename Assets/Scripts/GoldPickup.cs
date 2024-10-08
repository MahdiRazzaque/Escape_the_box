using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickup : MonoBehaviour {

    public int value;
    public GameObject pickupEffect;

    void Start() {
        
    }

    void Update() {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            FindObjectOfType<GameManager>().AddGold(value);

            var effect = Instantiate(pickupEffect, transform.position, transform.rotation);

            Destroy(gameObject);
            Destroy(effect, 1f);
        }
    }
}
