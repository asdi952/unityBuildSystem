using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageHolder : MonoBehaviour
{
   public static StorageHolder Instance;

   public void init(){
        if( Instance == null){
            Instance = this;
        }
   }

   public void pushObject( MonoBehaviour mono){
        mono.transform.SetParent( transform, true);
   }
}
