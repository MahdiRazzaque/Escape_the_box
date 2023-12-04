using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;
    public Vector3 offset;
    public bool useOffsetValues;
    public float rotateSpeed;
    public Transform pivot;
    public float maxViewAngle;
    public float minViewAngle;
    public bool invertY;

    // Camera Zoom
    [SerializeField] private float maxCameraZoom; // Camera rotation max limit blah
    [SerializeField] private float minCameraZoom; // Camera zoom min limit
    [SerializeField] private float camZoom; // Mousewheel input
    [SerializeField] private float camZoomSpeed; // Speed multiplier
    [SerializeField] private float camDistance; // Distance between camera and target

    void Start() {
        if(!useOffsetValues) 
            offset = target.position - transform.position;

        pivot.transform.position = target.transform.position;
        //pivot.transform.parent = target.transform;
        pivot.transform.parent = null;

        Cursor.lockState = CursorLockMode.Locked;
    }

   
    void LateUpdate() {

        pivot.transform.position = target.transform.position;
        
        //Get the X position of mouse and rotate target
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        pivot.Rotate(0, horizontal, 0);

        //Get the Y position of the mouse and rotate the pivot.
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
        

        if(invertY) {
            pivot.Rotate(vertical, 0, 0);
        } else  {
            pivot.Rotate(-vertical, 0, 0);
        }

        //Limit the up/down camera rotation
        if(pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180f) {
            pivot.rotation = Quaternion.Euler(maxViewAngle, pivot.eulerAngles.y, 0);
        }

        if(pivot.rotation.eulerAngles.x > 180 && pivot.rotation.eulerAngles.x < 360 + minViewAngle) {
            pivot.rotation = Quaternion.Euler(360 + minViewAngle, pivot.eulerAngles.y, 0);
        }

        //Move the camera based on the current rotation of the target and the original offset
        float desiredYAngle = pivot.eulerAngles.y;
        float desiredXAngle = pivot.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = target.position - (rotation * offset);     
        
        if(transform.position.y < target.position.y) {
            transform.position = new Vector3(transform.position.x, target.position.y -0.5f, transform.position.z);
        }

        transform.LookAt(target);

        // Zooming in/out
        if (Input.GetAxis("Mouse ScrollWheel") != 0.0f)
        {
            camZoom = 0;
            camZoom += Input.GetAxis("Mouse ScrollWheel") * camZoomSpeed;

            camDistance += camZoom;

            // Limits distance from the target
            camDistance = Mathf.Clamp(camDistance, minCameraZoom, maxCameraZoom);
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, camDistance);
    }
}
