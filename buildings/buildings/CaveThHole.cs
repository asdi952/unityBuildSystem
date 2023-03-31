using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thHoleCave : iBuild
{
    public override iBuildType type{ get{ return iBuildType.quadFoundation;}} 

    public static thHoleCave createRaw(){
        GameObject prefab = (GameObject)Resources.Load("cave/cave/cave_ThHole", typeof(GameObject));
        var script = Instantiate( prefab).GetComponent<thHoleCave>();
        if( script == null){
            Debug.LogError( "thHoleCave prefab is not found in prefabs/Quad.");
            return null;
        }
        return script;
    }

    public static thHoleCave create(){
        var script = createRaw();
        script.init();
        return script;
    }
}
