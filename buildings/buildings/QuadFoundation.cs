using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadFoundation : iBuild
{
    public static uint InstanceCount = 0;
    
    public override iBuildType type{ get{ return iBuildType.quadFoundation;}} 

    public static uint getInstanceCount(){
        uint count = InstanceCount;
        InstanceCount ++;
        return count;
    }
    
    public static QuadFoundation createRaw(){
        GameObject prefab = (GameObject)Resources.Load("build/quadFoundation", typeof(GameObject));
        var script = Instantiate( prefab).GetComponent<QuadFoundation>();
        if( script == null){
            Debug.LogError( "QuadFoundation prefab is not found in prefabs/Quad.");
            return null;
        }
        script.name = "QuadFoundation_" + getInstanceCount().ToString();
        print("visible: " + script.visible);
        script.visible = true; 
        return script;
    }

    public static QuadFoundation create(){
        var script = createRaw();
        script.init();
        return script;
    }
}
