using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mesh : MonoBehaviour
{
    // Start is called before the first frame update

    void removeTri(){
         rend = gameObject.GetComponent<MeshFilter>();
        Mesh mesh = rend.mesh;

        List<int> holder = new List<int>( mesh.triangles);
        holder.RemoveRange( 0, 3);
        mesh.triangles = holder.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
    MeshFilter rend;
    void Start()
    {
        removeTri();
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetButtonDown("e")){
            print("pressed E");
            removeTri();
        }
    }
}
