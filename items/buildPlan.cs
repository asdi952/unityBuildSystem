using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildPlan : MonoBehaviour
{
    public iBuild[] allGhost;
    public iBuild curGhost = null;
    bool ghostVisible = true;


    void initGhost(){
        allGhost = new iBuild[ iBuildType.allTypes.Length];
        misc.forAll<iBuildType>( iBuildType.allTypes, ( type, i) => {
            iBuild aux = type.createRaw();
            aux.name = "ghost_" + type.name;
            hideGhost( aux);
            allGhost[ i] = aux;

            Destroy( aux.GetComponent<BoxCollider>());
        });
        changeCurGhost( iBuildType.quadFoundation);
    }
    void hideGhost( iBuild ghost){
        print("hideGhost " + ghost.type.name + " " + ghost.visible);
        ghost.hide();
        StorageHolder.Instance.pushObject( ghost);
    }
    void showGhost( iBuild ghost){
        print("showGhost " + ghost.type.name);
        ghost.transform.parent = null;
        ghost.show();
    }
    public void changeCurGhost( iBuildType type){
        print("change curGhost");
        if( curGhost != null){
            hideGhost( curGhost);
        }
        curGhost = allGhost[ type.index ];
        showGhost( curGhost);
        BCManager.Instance.changeCollision( type.group);
    } 

    void Awake()
    {
        initGhost();
    }

    void changeGhostBuild(){
        if( Input.GetButtonDown("one")){
            changeCurGhost( iBuildType.quadFoundation);
        }else if( Input.GetButtonDown("two")){
            changeCurGhost( iBuildType.fullWall);
        }
    }

    void Update()
    {
        RaycastHit hit;
        iBuild build = null;
        iCollision buildCol = null;

        changeGhostBuild();

        ghostVisible = false;
        int mask = LayerMask.GetMask("building") | LayerMask.GetMask("buildingCollision") | LayerMask.GetMask("terrain");
        if( Physics.Raycast( transform.position, transform.forward, out hit, 500, mask)){
            GameObject obj = hit.collider.gameObject;
            if( obj.layer == LayerMask.NameToLayer("building")){
                build = obj.GetComponent<iBuild>();
            }else if( obj.layer == LayerMask.NameToLayer("buildingCollision")){
                buildCol = obj.GetComponent<iCollision>();
            }else if( obj.layer == LayerMask.NameToLayer("terrain")){
                curGhost.transform.position = hit.point;
                curGhost.transform.rotation = Quaternion.LookRotation( Vector3.Cross( hit.normal, transform.right), Vector3.up);
                ghostVisible = true; // this varible gets reseted to false at begin of loop
            }else{
                Debug.Log("Unknown layer");
                return;
            }
            if(Input.GetMouseButtonDown(0)){
                if( build != null){

                }else if( buildCol != null){
                    iBuild nBuild = iBuild.getBuild( curGhost.type);
                    if( nBuild == null) 
                        return;

                    // what build attachment should i attach to?
                    iBuildType.AttachReference aGroup = nBuild.type.getInputGroup( buildCol.getParent().type.group);
                    if( aGroup == null){
                        Debug.LogError("Error: group not set in attachReference");
                        return;
                    }
                    print( "aGroup:  " + aGroup.categoryIndex + " " + aGroup.attachGroupIndex);

                    iBuild.AnchorPointer aPointer = new iBuild.AnchorPointer(){ build = nBuild, 
                        categoryIndex = aGroup.categoryIndex, groupIndex = aGroup.attachGroupIndex, anchorIndex = 0};
                    aPointer.initAnchor();
                    if( !buildCol.placeBuild( aPointer)){
                        iBuild.returnBuild( aPointer.build);
                    }
                }else{
                    iBuild nBuild = iBuild.getBuild( curGhost.type);
                    if( nBuild == null) 
                        return;
                    
                    nBuild.transform.rotation = Quaternion.LookRotation( Vector3.Cross( hit.normal, transform.right), Vector3.up);
                    nBuild.transform.position = hit.point;
                }
            }
        }
        if( ghostVisible == true){
            curGhost.show();
        }else{
            curGhost.hide();
        }
    }
}
