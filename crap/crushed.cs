using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crushed : interactable
{

    Animator animator;
    void Start(){
        
        animator = transform.GetComponent<Animator>();
        if( animator == null ){
            Debug.LogError( "Animator not found" );
            return;
        }
    }
    bool play = false;
    // Start is called before the first frame update
    public override void interract()
    {
        print("interacted");
        if( play == false){
            play = true;
            animator.Rebind();
            animator.Update(0f);
            animator.StartPlayback();
        }else{
            play = false;
            animator.StopPlayback();
        }

    }
}
