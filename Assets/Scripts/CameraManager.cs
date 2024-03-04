using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // this script checks which camera point the camera should teleport to

    [SerializeField] private GameObject player;
    private List<GameObject> cameraPoints;

    private float distancePlayerPoint;
    private float distancePlayerClosest;
    private Vector3 closestPoint;

    private void Start()
    {
        cameraPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("Camera Point"));
        //PlayerInteractions.onUsingDoor += MoveCamera;
        PlayerInteractions.onUsingDoor += MoveCameraWithDelay;
    }

    // this checks which camera point is closest to the player and transports the camera there
    private void MoveCamera()
    {
        foreach (GameObject point in cameraPoints)
        {
            distancePlayerPoint = Vector3.Distance(player.transform.position, point.transform.position);
            distancePlayerClosest = Vector3.Distance(player.transform.position, closestPoint);
            if (distancePlayerPoint < distancePlayerClosest)
            {
                closestPoint = point.transform.position;
            }
        }
        this.gameObject.transform.position = closestPoint;
    }

    private void MoveCameraWithDelay()
    {
        StartCoroutine(CameraDelay());
    }

    private IEnumerator CameraDelay()
    {
        yield return new WaitForSeconds(0.3f);

        foreach (GameObject point in cameraPoints)
        {
            distancePlayerPoint = Vector3.Distance(player.transform.position, point.transform.position);
            distancePlayerClosest = Vector3.Distance(player.transform.position, closestPoint);
            if (distancePlayerPoint < distancePlayerClosest)
            {
                closestPoint = point.transform.position;
            }
        }
        this.gameObject.transform.position = closestPoint;
    }
}
