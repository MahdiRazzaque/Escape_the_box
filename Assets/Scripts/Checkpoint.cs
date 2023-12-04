using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public HealthManager healthManager;
    public Renderer checkPointRenderer;

    public Material cpOff;
    public Material cpOn;

    void Start() {
        healthManager = FindObjectOfType<HealthManager>();
    }

    void Update() {
        
    }

    public void CheckpointOn() {
        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();
        foreach (Checkpoint cp in checkpoints) {
            cp.CheckpointOff();
        }
        checkPointRenderer.material = cpOn;
    }

    public void CheckpointOff() {
        checkPointRenderer.material = cpOff;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            healthManager.SetSpawnPoint(transform.position);
            CheckpointOn();
        }
    }
}