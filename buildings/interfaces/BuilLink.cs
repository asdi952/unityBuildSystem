using UnityEngine;


public class BuildLink{
    public iBuild.AnchorPointer[] pointers = new iBuild.AnchorPointer[2]; 

    public void push0( iBuild.AnchorPointer pointer){
        pointers[0] = pointer;
    }
    public void push1( iBuild.AnchorPointer pointer){
        pointers[1] = pointer;
    }
    public bool attach(){
        if( !isComplete()){
            Debug.LogError("Erro: not complete");
            return false;
        }

        if( pointers[0].build.addLink( this, pointers[0]) == false)
            return false;

        if( pointers[1].build.addLink( this, pointers[1]) == false)
            return false;
        return true;
    }
    public void anchor0To1(){
        var newQ = new Quaternion();
        newQ.SetFromToRotation( pointers[0].anchor.forward, - pointers[1].anchor.forward);
        Transform tMoved = pointers[0].build.transform;
        tMoved.rotation = newQ * tMoved.rotation;
        tMoved.position = ( tMoved.position - pointers[0].anchor.position) + pointers[1].anchor.position;
    }

    public void dattach0(){
        pointers[0].build.removeLink( pointers[0]);
    }
    public void dattach1(){
        pointers[1].build.removeLink( pointers[1]);
    }
    public void dattach(){
        if( !isComplete()){
            Debug.LogError("Erro: Link is not complete");
            return;
        }

        dattach0();
        dattach1();
    }

    bool isComplete(){
        return ( pointers[0] != null && pointers[1] != null);
    }

}

