using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopCamera : MonoBehaviour
{
    public float height = 7;
    public float moveUp = 0;
    public float moveSide = 0;
    private GameObject currentPipe;
    private Vector3 newCamPos;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        handleCamera();
    }

    void Update()
    {
        handleCamera();
    }

    private void handleCamera() {

        Vector3 worldPos = (Vector3.forward * moveUp) + (Vector3.left * -moveSide) + (Vector3.up * height);
        Vector3 flatPos = newCamPos;
        flatPos.y = 0;
        Vector3 finalPos = flatPos + worldPos;
        transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref velocity, 1.0f);
    }

    public void updateCam(Vector3 pipePos) {
        newCamPos = pipePos;
    }
}