using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuildCollision : iCollision
{
    public override iBuildType.GroupType group{ get{ return iBuildType.GroupType.wallBuild;}}
    
    public static WallBuildCollision createRaw(){
        GameObject prefab = (GameObject)Resources.Load("build/collision/wallBuildCol", typeof(GameObject));
        GameObject obj = (GameObject)Instantiate( prefab);
        if( obj == null){
            Debug.LogError("Error: WallBuildCollision prefab failed to instantiate!");
            return null;
        }
        WallBuildCollision quad = obj.GetComponent<WallBuildCollision>();
        if( quad == null){
            Debug.LogError("Error: WallBuildCollision prefab failed to instantiate!");
            return null;
        }
        return quad;
    }
}
