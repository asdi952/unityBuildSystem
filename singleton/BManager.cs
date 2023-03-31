using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BManager : MonoBehaviour
{
    public struct BuildType{
        public Stack< iBuild> stack;
        public void init(){
            stack = new Stack< iBuild>();
        }
    } 
    //-----------------------------------------------
    public static BManager Instance;

    public BuildType[] buildTypes = new BuildType[ iBuildType.allTypes.Length];
    //################################################

    public void init(){
        if( Instance == null){
            Instance = this;
        }
        
        misc.forAll<BuildType>( buildTypes, ( type, i) =>{
            buildTypes[i].init(); 
        });
    }

    public iBuild popBuild( iBuildType type ){
        iBuild build;
    
        if(!buildTypes[ type.index].stack.TryPop( out build)){

            build = Creator.Instance.createBuild( type);
            if( build == null){
                Debug.LogError("Error: Can't create build of type " + type.name);
                return null;
            }
            build.init();
        }
        return build;
    }

    public void pushBuild( iBuild build ){
        buildTypes[ build.type.index].stack.Push( build );
        StorageHolder.Instance.pushObject( build);
    }


}
