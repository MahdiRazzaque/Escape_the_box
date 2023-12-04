using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

    public int maxHealth;
    public int currentHealth;

    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public playerController thePlayer;

    public float invincibilityTime;
    private float invincibilityCounter;

    public Renderer playerRenderer;
    private float flashCounter;
    public float flashTime;

    private bool isRespawning;
    private Vector3 respawnPoint;
    public float respawnTime;

    public GameObject deathEffect;

    public Image blackScreen;
    private bool isFadingToBlack;
    private bool isFadingFromBlack;
    public float fadeSpeed;
    public float fadeTime;

    void Start() {
        currentHealth = maxHealth;
        numOfHearts = maxHealth;
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

        if(isFadingToBlack) {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            if(blackScreen.color.a == 1f) {
                isFadingToBlack = false;
            }
        }

        if(isFadingFromBlack) {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if(blackScreen.color.a == 0f) {
                isFadingFromBlack = false;
            }
        }

        //Hearts manager


        
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
        thePlayer.gameObject.SetActive(false);
        Instantiate(deathEffect, thePlayer.transform.position, thePlayer.transform.rotation);
        
        yield return new WaitForSeconds(respawnTime);

        isFadingToBlack = true;

        yield return new WaitForSeconds(fadeTime);
        
        isFadingToBlack = false;
        isFadingFromBlack = true;

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
        if(currentHealth > numOfHearts) {
            numOfHearts = currentHealth;
        }

        for(int i = 0; i < hearts.Length; i++) {
            if(i < currentHealth) {   
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }
            if(i < numOfHearts) {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }
    }

    public void SetSpawnPoint(Vector3 newPosition) {
        respawnPoint = newPosition;  
    }
}


