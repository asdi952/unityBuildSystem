using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class iCollision : MonoBehaviour
{
    [SerializeField] iBuild.AnchorPointer parentPointer;
    [SerializeField] public virtual iBuildType.GroupType group{ get{ return iBuildType.GroupType.None;}}

    public bool visible = true;
    [SerializeField]public Transform anchor;

    public void init(){
        anchor = transform.Find("a0");
        if( anchor == null){
            Debug.LogError("Can't find Anchor a0");
            return;
        }
        gameObject.layer = LayerMask.NameToLayer("buildingCollision");
    }

    public void parentTo( iBuild.AnchorPointer pointer){
        parentPointer = pointer;
        transform.SetParent( pointer.build.transform, true);
    }
    public void unparent(){
        //transform.parent = null;
    }
    public iBuild getParent(){
        return parentPointer.build;
    }
    public void anchorToParent(){
        Quaternion rot = Quaternion.FromToRotation(anchor.forward, parentPointer.anchor.forward);
        transform.rotation = rot * transform.rotation;
        transform.rotation = Quaternion.LookRotation( transform.forward, parentPointer.build.transform.up);
        
        transform.position = ( transform.position - anchor.position) + parentPointer.anchor.position;
    }
    public bool placeBuild( iBuild.AnchorPointer pointer){
        return parentPointer.build.addBuild( parentPointer, pointer);
    }
    
   public static iCollision getCollision( iBuild.AnchorPointer pointer){
        iCollision col = BCManager.Instance.popCollision( pointer.getAttachGroup());
        col.parentTo( pointer);
        col.anchorToParent();
        col.activate();
        return col;
   }
   public static void returnCollision( iCollision col){
        col.deactivate();
        col.unparent();
        BCManager.Instance.pushCollision( col);
   }

    public void activate(){
        if( visible == false){
            visible = true;
            gameObject.SetActive( true);
        }
    }
    public void deactivate(){
        if( visible == true){
            visible = false;
            gameObject.SetActive( false);
        }
    }
}
