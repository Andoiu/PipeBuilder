using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class PipeController : MonoBehaviour
{
    public GameObject nextPipe;
    public GameObject[] pipeTypes; //Should be input in order (pipe_long, pipe_medium, pipe_short, pipe_left, pipe_right, pipe_split, pipe_fan)
    public List<GameObject> currentSelection = new List<GameObject>();
    public List<GameObject> pipeHistory = new List<GameObject>();
    public GameObject cam;
    public VentilationTaskGoal ventGoal;
    public GameObject[] introUI;
    public GameObject[] endUI;
    private int index = 1;
    private int bendDir = 0;
    private float jumpLength;
    private bool completed = false;
    private bool firstPipe = true;

    /*
    / Start with medium length pipe selected
    */
    void Awake() {
        ventGoal = GameObject.Find("goal_1").GetComponent<VentilationTaskGoal>();
        introUI = GameObject.FindGameObjectsWithTag("IntroUI");
        endUI = GameObject.FindGameObjectsWithTag("EndingUI");
        showObjs(endUI, false);

        foreach (GameObject pipe in pipeTypes) {
            currentSelection.Add(GameObject.Instantiate(pipe, transform.position, transform.rotation));
            pipe.SetActive(false);
        }
        nextPipe = currentSelection[1];
        nextPipe.SetActive(true);
        updateCamera();
    }

    /*
    / Key listeners
    */
    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !completed) {
            cyclePipes(0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)  && !completed) {
            cyclePipes(1);
        }
        if (Input.GetKeyDown(KeyCode.Return) && !completed) {
            bool success = buildPipe();
            if (firstPipe && success) {
                showObjs(introUI, false);
                firstPipe = false;
            }
            if (!completed && success) {
                newPipe();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return) && completed) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

     

    /*
    / Create new pipes
    */
    private void newPipe() {

        foreach (GameObject pipe in pipeTypes) {
            currentSelection.Add(GameObject.Instantiate(pipe, pipeHistory.Last().transform.position, pipeHistory.Last().transform.rotation));
        }

        foreach (GameObject pipe in currentSelection) {
            pipe.SetActive(false);
            pipe.transform.Translate(jumpLength, 0, 0);
            if (bendDir == 1) {
                pipe.transform.Rotate(0, -90, 0);
                pipe.transform.Translate(-0.18f, 0, 0);
            }
            else if (bendDir == 2) {
                pipe.transform.Rotate(0, 90, 0);
                pipe.transform.Translate(-0.18f, 0, 0);
            }
        }
        nextPipe = currentSelection[1];
        index = 1;
        bendDir = 0;
        nextPipe.SetActive(true);
    }
    
    /*
    / Cycle through pipes, either traversing left or right
    */
    private void cyclePipes(int button) {
        nextPipe.SetActive(false);
        if (button == 1) {
            index++;
        }
        else {
            index--;
        }
        if (index > 6) {
            index = 0;
        }
        else if (index < 0) {
            index = 6;
        }
        nextPipe = currentSelection[index];
        nextPipe.SetActive(true);
        //Debug.Log("Index: " + index);
    }

    /*
    / Build selected pipe, returns if successul
    */
    private bool buildPipe() {

        if (nextPipe.GetComponent<Identifier>().pipeType == Identifier.PipeType.Outlet && !ventGoal.bounds.Contains(nextPipe.transform.position)) {
            Debug.Log("Not allowed to build here");
            return false;
        }

        ChangeMaterial pipeToChange = nextPipe.GetComponentInChildren(typeof(ChangeMaterial)) as ChangeMaterial;
        pipeToChange.ChangeMat();
        GameObject builtPipe = nextPipe;
        pipeHistory.Add(builtPipe);

        foreach (GameObject pipe in currentSelection) {
            if (!pipe.activeSelf) {
                Destroy(pipe);
            }
        }
        currentSelection.Clear();
        PipeSound.Instance.PlayPipeConnectSound(builtPipe.transform);
        updateCamera();

        // Determine jump length based on built pipe type
        switch (builtPipe.GetComponent<Identifier>().pipeType) {
            case Identifier.PipeType.Long:
                jumpLength = -1.25f;
                break;
            case Identifier.PipeType.Medium:
                jumpLength = -0.88f;
                //Debug.Log("medium");
                break;
            case Identifier.PipeType.Short:
                jumpLength = -0.50f;
                //Debug.Log("medium");
                break;
            case Identifier.PipeType.Left:
                jumpLength = -0.18f;
                bendDir = 1;
                //Debug.Log("left");
                break;
            case Identifier.PipeType.Right:
                jumpLength = -0.18f;
                bendDir = 2;
                //Debug.Log("right");
                break;
            case Identifier.PipeType.T:
                jumpLength = -0.25f;
                //Debug.Log("t");
                break;
            case Identifier.PipeType.Outlet:
                jumpLength = -0.30f;
                if (!ventGoal.getComplete() && ventGoal.bounds.Contains(builtPipe.transform.position)) {
                    ventGoal.setComplete(true);
                    completed = true;
                    showObjs(endUI, true);
                }
                break;
        }
        return true;
    }

    /*
    / Show or hide a list of objects
    */ 
    private void showObjs(GameObject[] objs, bool visible) {
        foreach (GameObject obj in objs) {
            obj.SetActive(visible);
        }
    }
    
    /*/
    / Updates the camera position to a new pipe
    */
    private void updateCamera() {
        cam.GetComponent<TopCamera>().updateCam(nextPipe.transform.position);
    }
}