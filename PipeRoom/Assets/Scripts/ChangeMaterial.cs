using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{

    public Material[] material;
    private Renderer render;

    void Start() {
        //Debug.Log("mat ready!");
        render = GetComponent<Renderer>();
        render.enabled = true;
        render.sharedMaterial = material[0];
    }

    public void ChangeMat() {
        render.sharedMaterial = material[1];
    }
}
