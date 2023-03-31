using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interact : MonoBehaviour
{
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {   

        if( Input.GetButtonDown("e")){
            print("pressed");
            RaycastHit hit;
            if( Physics.Raycast( transform.position, transform.forward, out hit, 100)){
                var script = hit.collider.gameObject.GetComponent<interactable>();
                if( script == null )
                    return;
                
                script.interract();
            }
        }
    }
}
