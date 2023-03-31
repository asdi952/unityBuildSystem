using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class init : MonoBehaviour
{
    GameObject gbObj;
    GameObject storageHolder;
    BCManager BMSingleton;
    BManager BSingleton;

    void findObj(){
        gbObj = GameObject.Find("gbHolder");
        if( gbObj == null ){
            Debug.LogError("Error: gbHolder not found");
            return;
        }
        storageHolder = GameObject.Find("storageHolder");
        if( storageHolder == null ){
            Debug.LogError("Error: storageHolder not found");
            return;
        }
    }

    void Awake(){
        iBuildType.init();
        findObj();
        
        storageHolder.GetComponent<StorageHolder>().init();
       
        gbObj.GetComponent<Creator>().init();

        gbObj.GetComponent<BCManager>().init();

        gbObj.GetComponent<BManager>().init();



    }
    
}
