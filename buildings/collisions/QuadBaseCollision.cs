using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadBaseCollision : iCollision
{
    public override iBuildType.GroupType group{ get{ return iBuildType.GroupType.quadBaseBuild;}}
    
    public static QuadBaseCollision createRaw(){
        GameObject prefab = (GameObject)Resources.Load("build/collision/baseCol", typeof(GameObject));
        GameObject obj = (GameObject)Instantiate( prefab);
        if( obj == null){
            Debug.LogError("Error: QuadBaseCollision prefab failed to instantiate!");
            return null;
        }
        QuadBaseCollision quad = obj.GetComponent<QuadBaseCollision>();
        if( quad == null){
            Debug.LogError("Error: QuadBaseCollision prefab failed to instantiate!");
            return null;
        }
        return quad;
    }
}
