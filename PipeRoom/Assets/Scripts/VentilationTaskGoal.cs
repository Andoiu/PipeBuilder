using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentilationTaskGoal : MonoBehaviour
{
    public Bounds bounds;
    public AudioSource audioS;
    public bool complete = false;

    void Start() {
        bounds = gameObject.GetComponent<MeshCollider>().bounds;
        audioS = gameObject.GetComponent<AudioSource>();
    }
    
    public void setComplete(bool value) {
        complete = value;
        if (value) {
            audioS.PlayOneShot(audioS.clip, 1.0f);
            Debug.Log("Goal Completed! " + gameObject.name);
        }
    }

    public bool getComplete() {
        return complete;
    }
}