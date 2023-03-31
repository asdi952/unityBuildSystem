using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
  public static Creator Instance;

  public void init(){
    if( Instance == null){
      Instance = this;
    }
  }

  public iCollision createCollision( iBuildType.GroupType group){
    return iBuildType.createCollison( group);
  }

  public iBuild createBuild( iBuildType type){
    return type.createRaw();
  }
   
}
