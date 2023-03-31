using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BCManager : MonoBehaviour
{
    public class BuildColRef{
        public iBuild build;
        public uint groupIndex;
    }
    public class SubscriptionSheet{
        public struct Unit{
            public LinkedListNode<BuildColRef> node;
            public iBuildType.GroupType group;
        }
        public Unit[] units;
        public SubscriptionSheet(int size){
            units = new Unit[size];
        }
    }
    struct CollisionsStacks{
        public Stack< iCollision> stack;
        public void init(){
            stack = new Stack< iCollision>();
        }
    }
    struct BuildSubscription{
        public LinkedList< BuildColRef> list;
        public void init(){
            list = new LinkedList< BuildColRef>();
        }
    }

    public static BCManager Instance;

    BuildSubscription[] buildSub = new BuildSubscription[ iBuildType.GroupType.GetNames(typeof(iBuildType.GroupType)).Length];
    CollisionsStacks[] colStacks = new CollisionsStacks[ iBuildType.GroupType.GetNames(typeof(iBuildType.GroupType)).Length];

    public iBuildType.GroupType curColGroup = iBuildType.GroupType.None;

    public void init(){
        if( Instance == null){
            Instance = this;

            misc.forAll<CollisionsStacks>( colStacks, ( ref CollisionsStacks stack, uint i)=>{
                colStacks[i].init();
            });
            misc.forAll<BuildSubscription>( buildSub, ( ref BuildSubscription list, uint i)=>{
                buildSub[i].init();
            });
        }
    }

    public iCollision popCollision( iBuildType.GroupType group ){
        iCollision col;
        if( !colStacks[ (int)group].stack.TryPop( out col)){
            col = Creator.Instance.createCollision( group);
            if( col == null){
                return null;
            }
            col.init();
        }
        return col;
    }
    
    public void pushCollision( iCollision collision ){
        colStacks[ (int)collision.group ].stack.Push( collision );
        StorageHolder.Instance.pushObject( collision);
    }
    //----------------------------
    public SubscriptionSheet registerCollisions( iBuild build){
        iBuildType.AttachGroup[] attachGroups = build.type.aGCategories[ (int)iBuildType.AttachGroupCategory.Type.withCollision].aGroups;
        SubscriptionSheet sheet = new SubscriptionSheet( attachGroups.Length);

        misc.forAll<iBuildType.AttachGroup>( attachGroups, (groups, i) =>{
            LinkedList< BuildColRef> list = buildSub[(int)groups.group].list;
            BuildColRef buildRef = new BuildColRef(){ build = build, groupIndex = i}; 
            list.AddLast( buildRef);
            sheet.units[ i].node = list.Last;
            sheet.units[ i].group = groups.group;
            
            if( curColGroup == groups.group){
                build.enableCollision( i);
            }
        });
        return sheet;
    }

    public void unregisterCollisions( SubscriptionSheet sheet){
        if( sheet == null){
            Debug.LogError("Error: SubShett is null");
            return;
        }

        misc.forAll<SubscriptionSheet.Unit>(sheet.units, ( ref SubscriptionSheet.Unit unit, uint i)=>{
            LinkedList< BuildColRef> list = buildSub[(int)unit.group].list;
            if( unit.group == curColGroup){
                unit.node.Value.build.disableCollisions( unit.node.Value.groupIndex);
            }
            list.Remove(unit.node); 
        });
    }

    public void changeCollision( iBuildType.GroupType group){
        if( curColGroup == group)
            return;

        LinkedList< BuildColRef> list = buildSub[ (int)curColGroup].list;
        for(LinkedListNode< BuildColRef> node = list.First; node != null; node=node.Next){
            node.Value.build.disableCollisions( node.Value.groupIndex);
        }

        curColGroup = group;
        list = buildSub[ (int)curColGroup].list;
        for(LinkedListNode< BuildColRef> node = list.First; node != null; node=node.Next){
            node.Value.build.enableCollision( node.Value.groupIndex);
        }
    }
}