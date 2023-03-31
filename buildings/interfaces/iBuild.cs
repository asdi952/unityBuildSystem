using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class iBuild : MonoBehaviour
{
   public struct AnchorGroup{
      public Transform[] anchors;
   }
   public class AnchorPointer{ 
      public iBuild build;
      public uint categoryIndex;
      public uint groupIndex;
      public uint anchorIndex;
      public Transform anchor;

      public void initAnchor(){
         anchor = build.anchorGroups[ build.type.aGCategories[categoryIndex].aGroups[groupIndex].anchorGroupIndex].anchors[anchorIndex];
      }
      public iBuildType.GroupType getAttachGroup(){
         return build.type.aGCategories[categoryIndex].aGroups[groupIndex].group;
      }
   }

   public struct GroupLink{
      public Option< BuildLink>[] links;
      public AnchorPointer[] pointerBuffers;
      public bool hasPointer( uint index){
         return pointerBuffers[ index] != null;
      }
      public void resetAll(){
         misc.forAll<Option<BuildLink>>( links, ( ref Option<BuildLink> link) => { link.reset();});
      }
   }
   public struct CategoriesLink{
      public GroupLink[] groupLinks;
   }
   public struct CollisionLink{
      public Option<iCollision>[] collisions;
      public void resetAll(){
         misc.forAll<Option<iCollision>>( collisions, (ref Option<iCollision> col) => col.reset());
      }
   }
   //------------------------------------------------------------------------
   public virtual iBuildType type{ get{ return iBuildType.none;}} 

   public CategoriesLink[] categoriesLinks;
   public CollisionLink[] collisionLinks;
   public AnchorGroup[] anchorGroups;
   BCManager.SubscriptionSheet ColSubscription = null;

   public bool visible = true;
   public iBuild(){
      visible = true;
   }
   //------------------------------------------------------------------------
   public void initStatic(){
      categoriesLinks = new CategoriesLink[ type.aGCategories.Length];
      for( uint i = 0; i < categoriesLinks.Length; i++ ){
         int groupsLen = 0;
         if( type.aGCategories[i] == null){
            categoriesLinks[i].groupLinks = new GroupLink[ 0 ];
            continue;
         }else{
            groupsLen = type.aGCategories[i].aGroups.Length;
            categoriesLinks[i].groupLinks = new GroupLink[ groupsLen ];
         }
         

         switch( type.aGCategories[i].type){
            case iBuildType.AttachGroupCategory.Type.withCollision:
               collisionLinks = new CollisionLink[ groupsLen];
            break;
         }
         for( uint j = 0; j < groupsLen; j++){
            int anchorLen = type.getAnchorLength( i, j);
            categoriesLinks[i].groupLinks[j].links = new  Option<BuildLink>[ anchorLen];
            categoriesLinks[i].groupLinks[j].resetAll();
            categoriesLinks[i].groupLinks[j].pointerBuffers = new AnchorPointer[ anchorLen];
           
            switch( type.aGCategories[i].type){
               case iBuildType.AttachGroupCategory.Type.withCollision:
                  collisionLinks[j].collisions = new Option<iCollision>[anchorLen];
                  collisionLinks[j].resetAll();
               break;
            }
         }
      }
      //array contains anchor Transforms[]
      anchorGroups = new AnchorGroup[ type.anchorGroups.Length];
      for( uint i = 0; i < anchorGroups.Length; i++){
         anchorGroups[i].anchors = new Transform[ type.anchorGroups[i].anchorNames.Length];
         for( int j = 0; j < anchorGroups[i].anchors.Length; j++){
            anchorGroups[i].anchors[j] = transform.Find( type.anchorGroups[i].anchorNames[j]);
            if(  anchorGroups[i].anchors[j] == null){
               Debug.LogError("Error anhcor name not found : " +  type.anchorGroups[i].anchorNames[j]);
            }
         }
      }
   } 

   public void init(){
      initStatic();
      registerCollisions();
      gameObject.layer = LayerMask.NameToLayer("building");
   }
   public void dinit(){
      unregisterCollisions();
   }

   public void registerCollisions(){
      if( ColSubscription == null){
         ColSubscription = BCManager.Instance.registerCollisions(this);
      }
   }
   public void unregisterCollisions(){
      BCManager.Instance.unregisterCollisions( ColSubscription);
      ColSubscription = null;
   }

   public void enableCollision( uint groupIndex){
      GroupLink groupLinks = categoriesLinks[ (int)iBuildType.AttachGroupCategory.Type.withCollision].groupLinks[groupIndex];

      misc.forAll< Option< BuildLink>>( groupLinks.links, (ref Option< BuildLink> link, uint i) =>{
         if( link.empty()){
            print("getting collisons of: " + (int)iBuildType.AttachGroupCategory.Type.withCollision + " " + groupIndex +  " " + i);
            AnchorPointer pointer = getAnchorPointer( groupIndex, i);
            collisionLinks[groupIndex].collisions[i].set( iCollision.getCollision( pointer));
         }else{
            print("fail");
         }
      });
   }

   public void disableCollisions(uint groupIndex){
      misc.forAll< Option< iCollision>>( collisionLinks[ groupIndex].collisions, ( ref Option< iCollision> col, uint i)=>{
         if( col.exist()){
            iCollision.returnCollision( col.consume());
         }
      });
   }

   AnchorPointer getAnchorPointer( uint _groupIndex, uint _anchorIndex){
      AnchorPointer pointer = categoriesLinks[ (int)iBuildType.AttachGroupCategory.Type.withCollision].groupLinks[_groupIndex].pointerBuffers[ _anchorIndex];
      if( pointer == null){
         pointer = new AnchorPointer(){categoryIndex = (int)iBuildType.AttachGroupCategory.Type.withCollision,
            groupIndex = _groupIndex, anchorIndex = _anchorIndex,
            build = this};

         pointer.initAnchor();
         categoriesLinks[ (int)iBuildType.AttachGroupCategory.Type.withCollision].groupLinks[_groupIndex].pointerBuffers[ _anchorIndex] = pointer;
      }
      return pointer;
   }

   public static iBuild getBuild( iBuildType type){
      iBuild build = BManager.Instance.popBuild( type);
      build.activate();
      build.transform.SetParent( null, true);
      return build;
   }
   public static void returnBuild( iBuild build){
      build.deactivate();
      BManager.Instance.pushBuild( build);
   }

   public bool addLink( BuildLink link, AnchorPointer pointer){
      var checkLink = categoriesLinks[ pointer.categoryIndex].groupLinks[ pointer.groupIndex].links[pointer.anchorIndex];
      if( checkLink.exist()){
         Debug.LogError("Error: Link already exists");
         return false;
      }
      print("adding link of: " + pointer.build.name + " " + (int)pointer.categoryIndex + " " + pointer.groupIndex +  " " + pointer.anchorIndex);
      checkLink.set( link);
      switch( type.aGCategories[ pointer.categoryIndex].type){
         case iBuildType.AttachGroupCategory.Type.withCollision:
            print("link with collisionm");
            iCollision col = collisionLinks[ pointer.groupIndex].collisions[ pointer.anchorIndex].consume();
            iCollision.returnCollision( col);
         break;
      }
      return true;
   }
   public bool removeLink( AnchorPointer pointer){
      var checkLink = categoriesLinks[ pointer.categoryIndex].groupLinks[ pointer.groupIndex].links[pointer.anchorIndex];
      if( checkLink.empty()){
         Debug.LogError("Error: Link does not exist");
         return false;
      }
      checkLink.reset();
      switch( type.aGCategories[ pointer.categoryIndex].type){
         case iBuildType.AttachGroupCategory.Type.withCollision:
            iCollision col = iCollision.getCollision( pointer);
            collisionLinks[ pointer.groupIndex].collisions[ pointer.anchorIndex].set( col);
         break;
      }
      return true;
   }
   public bool addBuild( AnchorPointer anchorBuildPointer, AnchorPointer inBuildPointer){
      BuildLink link = new BuildLink();
      
      link.push0( inBuildPointer);
      link.push1( anchorBuildPointer);
      if( !link.attach())
         return false;

      link.anchor0To1();
      return true;
   }

   public void activate(){
      if( visible == false){
         visible = true;
         gameObject.SetActive(true);
         registerCollisions();
      }
   }
   public void deactivate(){
      if( visible == true){
         visible = false;
         gameObject.SetActive( false);
         unregisterCollisions();
      }
   }

   public void show(){
      if( visible == false){
         print("show " + type.name);
         visible = true;
         gameObject.SetActive(true);
      }
   }
   public void hide(){
      if( visible == true){
         print("hider " + type.name + " " + misc.stackStrace());
         visible = false;
         gameObject.SetActive(false);
      }
   }

   public void destroy(){
      dinit();
      Destroy( gameObject);
   }
}
