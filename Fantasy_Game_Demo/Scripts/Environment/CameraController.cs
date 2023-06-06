using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    float xTransformAmt = 10;
    float yTransformAmt = 3;
    [SerializeField] public float distanceFromPlayer = 5f;

    //Fields used for direction indicator manipulation
    public GameObject windIndicator;
    //Array containing Vector3 objs representing the checkpoints at which to display indicators
    private Vector3[] indicatorStartStopPositions = {
        new Vector3(10,0,0),
        new Vector3(100,0,0),
        new Vector3(200,0,0)
    };

    private Vector3 indicatorOffDistance = new Vector3(20, 0, 0);   //object used to set distance after checkpoint to turn off indicator
    bool passedCheckpoint;  //Value indicating whether the player has passed a checkpoint activating a direction indicator
    int lastCheckpointIndex = 0;

    private Vector3[] checkpoint_positions;

    void Start()
    {
        //Statement used to set distance for camera compnent, 
        //positive number correlates with increase in distance of camera from player
        GetComponent<Camera>().orthographicSize = distanceFromPlayer;

        //Wind direction indicator img object reference is initialized
        windIndicator = transform.GetChild(0).gameObject;
        //Get children of camera here for all indicators

        //Locations of each of 3 checkpoints found and recorded 
        checkpoint_positions = new Vector3[] {
            new Vector3(GameObject.FindGameObjectWithTag("CHECK_1").transform.position.x, 0, 0),
            new Vector3(GameObject.FindGameObjectWithTag("CHECK_2").transform.position.x, 0, 0),
            new Vector3(GameObject.FindGameObjectWithTag("CHECK_3").transform.position.x, 0, 0)
        };
    }

    void Update()
    {
        //Change the camera position based on player position for camera follow affect
        transform.position = new Vector3(player.position.x + xTransformAmt, player.position.y + yTransformAmt, transform.position.z);

        //Check whether a the player has passed a checkpoint/ a direction indicator should be played
        startStopIndicator(player.position);
    }

    //Method controls the when to start or stop the direction indicators based on player checkpoints
    //The checkpoints are stored as an array of Vector3 objects indicating their positions
    private void startStopIndicator(Vector3 currentPos) 
    {
        for (int i = 0; i < indicatorStartStopPositions.Length; i++)
        {
            //Check if the player has moved the distance indicated by the x value of indicatorOffDistance 
            if (currentPos.x >= indicatorStartStopPositions[lastCheckpointIndex].x + indicatorOffDistance.x)
            {
                lastCheckpointIndex++;
                passedCheckpoint = false;
                //Turn the indicator off once the player has moved far enough from the last checkpoint
                windIndicator.SetActive(passedCheckpoint);
                windIndicator.GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
            }
            if(i == lastCheckpointIndex)
            if (currentPos.x >= indicatorStartStopPositions[i].x)    //Player has reached/passed a checkpoint
            {
                passedCheckpoint = true;
                //The direction indicator is displayed to the player
                windIndicator.SetActive(passedCheckpoint);
            }
        }
    }
}
