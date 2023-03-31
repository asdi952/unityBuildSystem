using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullWall : iBuild
{
public static uint InstanceCount = 0;
    
    public override iBuildType type{ get{ return iBuildType.fullWall;}} 

    public static uint getInstanceCount(){
        uint count = InstanceCount;
        InstanceCount ++;
        return count;
    }
    
    public static FullWall createRaw(){
        GameObject prefab = (GameObject)Resources.Load("build/fullWall", typeof(GameObject));
        var script = Instantiate( prefab).GetComponent<FullWall>();
        if( script == null){
            Debug.LogError( "QuadFoundation prefab is not found in prefabs/Quad.");
            return null;
        }
        script.name = "Fullwall_" + getInstanceCount().ToString();

        return script;
    }

    public static FullWall create(){
        var script = createRaw();
        script.init();
        return script;
    }
}
