using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemHolder : MonoBehaviour
{

    public Transform frame;
    public Vector3 offset;
    public float spacing = 0.1f;
    public Transform Cam;

    public int slotNumber = 6;

    float maxSize;
    List<Transform>slots = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        var size = frame.GetChild(0).GetComponent<MeshFilter>().sharedMesh.bounds.size;
        maxSize = Mathf.Max(size.x,size.y,size.z);

        init();
    }
    void init(){
        Vector3 curOffset = Cam.TransformDirection(offset);
        Vector3 right = Cam.right;
        //Quaternion rot = Cam.rotation;
        Quaternion rot = Quaternion.LookRotation( -Cam.transform.forward, Cam.transform.up);

        curOffset -= right * (maxSize + spacing) *( slotNumber -1) * 0.5F;

        for(int i = 0 ; i < slotNumber; i++){
            var holder = Instantiate(frame,Cam.position +curOffset,rot);

            holder.parent = Cam;
            slots.Add(holder);
            curOffset += right * (maxSize + spacing);        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
