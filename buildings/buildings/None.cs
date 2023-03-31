using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class None : iBuild
{
    public override iBuildType type{ get{ return iBuildType.none;}} 


    public static iBuild createRaw(){
        Debug.LogError("none is not supposed to be instantiated");
        return null;
    }
    public static iBuild create(){
        return createRaw();
    } 
}
