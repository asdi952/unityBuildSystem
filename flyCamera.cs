using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyCamera : MonoBehaviour
{
    public float frontSpeed = 800;
    public float sideSpeed = 400;
    public float upSpeed = 400;
    public float sprintSpeed = 2;
    Transform cam;
    Transform parent;
    // Start is called before the first frame update
    Rigidbody rBody;
    
    void Start()
    {
        //parent = transform.parent.transform;
        //rBody = parent.GetComponent<Rigidbody>();
        cam = transform;
        if( cam == null){
            Debug.LogError("Error: cant find camera");
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    float yaw = 0;
    float pitch = 0;
    public float sen = 1.5F;
    void Update()
    {   
        yaw += Input.GetAxisRaw("Mouse X") * sen;
        pitch += -Input.GetAxisRaw("Mouse Y") * sen;

        cam.rotation = Quaternion.identity;
        cam.Rotate(Vector3.up,yaw);
        cam.Rotate(Vector3.right,pitch);

        float speed = Input.GetAxis("speed") == 1 ? sprintSpeed : 1;
        
        Vector3 posDir = new Vector3(0,0,0);
        float frontAxis = Input.GetAxis("frontAxis");
        posDir += cam.forward * frontAxis * frontSpeed * speed * Time.deltaTime;

        float sideAxis = Input.GetAxis("sideAxis");
        posDir += cam.right * sideAxis * sideSpeed * speed * Time.deltaTime;

        float upAxis = Input.GetAxis("upAxis");
        posDir += cam.up * upAxis * upSpeed * speed * Time.deltaTime;
       
        transform.position += posDir;
    }
}
