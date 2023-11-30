using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using JetBrains.Annotations;

public class HealthManager : MonoBehaviour {

    public int maxHealth;
    public int currentHealth;

    public playerController thePlayer;

    public float invincibilityTime;
    private float invincibilityCounter;

    public Renderer playerRenderer;
    private float flashCounter;
    public float flashTime;

    private bool isRespawning;
    private Vector3 respawnPoint;
    public float respawnTime;

    void Start() {
        currentHealth = maxHealth;
        UpdateHealth();

        respawnPoint = thePlayer.transform.position;        
    }

    void Update() {
        if (invincibilityCounter > 0) {
            invincibilityCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;

            if(flashCounter <= 0) {
                playerRenderer.enabled = !playerRenderer.enabled;
                flashCounter = flashTime;
            }
        }

        if (invincibilityCounter <= 0) {
            playerRenderer.enabled = true;
        }
        
    }
    
    public void HurtPlayer(int damage, Vector3 direction) {
        if(invincibilityCounter <= 0) {
            currentHealth -= damage;
            UpdateHealth();

            if (currentHealth <= 0) {
                Respawn();
            } else {
                thePlayer.knockback(direction);

                invincibilityCounter = invincibilityTime;

                playerRenderer.enabled = false;

                flashCounter = flashTime;
            }
        }
    }

    public void HealPlayer(int heal) {
        currentHealth += heal;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        UpdateHealth();
    }

    public void Respawn() {
        if(!isRespawning) {
            StartCoroutine("RespawnCo");
        }
    }

    public IEnumerator RespawnCo() {
        isRespawning = true;
        FindObjectOfType<GameManager>().UpdateHealth(currentHealth, maxHealth, true);
        thePlayer.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        isRespawning = false;

        thePlayer.gameObject.SetActive(true);

        CharacterController charController = thePlayer.GetComponent<CharacterController>();
        charController.enabled = false;
        thePlayer.transform.position = respawnPoint;
        charController.enabled = true;

        currentHealth = maxHealth;
        UpdateHealth();
        invincibilityCounter = invincibilityTime;
        playerRenderer.enabled = false;
        flashCounter = flashTime;
        playerRenderer.enabled = true;
    }

    public void UpdateHealth() {
        FindObjectOfType<GameManager>().UpdateHealth(currentHealth, maxHealth, false);
    }
}


