using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeWall : interactable
{
    Mesh mesh;
    void Start()
    {
        var filter = GetComponent<MeshFilter>();
        if( isNull( filter, "filter is null")){
            return;
        }
        mesh = filter.mesh;
       // mesh.UploadMeshData( false );
       // print( "vertices: " + mesh.vertices.Length);
    }
    
    public override void interract(){
        print("interract  ");
    }

    bool isNull( Object obj, string errMsg){
        if( obj == null){
            Debug.LogError(errMsg);
            return true;
        }
        return false;
    }
}
