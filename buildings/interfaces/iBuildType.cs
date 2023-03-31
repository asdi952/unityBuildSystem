using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class iBuildType
{
    public enum GroupType{ None, caveBlock, quadBaseBuild, triBaseBuild, wallBuild, quadfloorBuild, triFloorBuild, roofBuild, middleBuild}

    //#################################################
    public class AnchorGroup{
        public string[] anchorNames;
        public static AnchorGroup[] createArray(params AnchorGroup[] anchorGroups){
            misc.forAll<AnchorGroup>( anchorGroups, ( group, i)=>{
                group.index = i;
            });
            return anchorGroups;
        }
        public static AnchorGroup createUnit(params string[] labels){
            return new AnchorGroup(){ anchorNames = labels };
        }

        public uint index{private set; get;}
    }
    //#################################################
    public class AttachGroup{ 
        public enum Interaction{ OnlyOne, Always, Never};

        public Interaction interaction = Interaction.Never;
        public GroupType group; 
        public uint anchorGroupIndex;

        public static AttachGroup[] createArray( params AttachGroup[] groups ){
            misc.forAll<AttachGroup>( groups, ( group, i) =>{
                group.index = i;
            });
            return groups;
        }
        public uint index{ private set; get;}
    }
    //#################################################
    public class AttachGroupCategory{ 
        public enum Type{ withCollision, withoutCollision}
        public static AttachGroupCategory[] createArray( params AttachGroupCategory[] categories){
            AttachGroupCategory[] newCategories = new AttachGroupCategory[ Type.GetNames(typeof(Type)).Length];
            misc.forAll<AttachGroupCategory>(categories, ( cat, i)=>{
                newCategories[ (int)cat.type] = cat;
            });
            return newCategories;
        }
        public static AttachGroupCategory createUnit( AttachGroupCategory.Type _type, params AttachGroup[] categories){
            return new AttachGroupCategory(){ 
                type = _type,
                aGroups = categories,
            };
        
        }
        public Type type;
        public AttachGroup[] aGroups;
    }
    
    //#################################################
    public class AttachReference{ 
        public uint categoryIndex;
        public uint attachGroupIndex;
        public AttachGroup attachGroup;

        public void getAttachGroup( iBuildType type){
            attachGroup = type.aGCategories[ categoryIndex].aGroups[attachGroupIndex];
        }
        public static AttachReference[] createArray( params AttachReference[] refs){
            misc.forAll<AttachReference>( refs, ( _ref, i)=>{
                _ref.index = i;
            });
            return refs;
        }
        public static AttachReference createUnit( uint categorieIndex, uint attachGroupIndex){
            var aux = new AttachReference(){
                categoryIndex = categorieIndex,
                attachGroupIndex = attachGroupIndex
            };
            return aux;
        }
        public uint index{ private set; get;}
    }

     
    public delegate iBuild CreateRaw();
    //##########################################################
    //-----------------BUILD DESCRIPTION----------------------------------------------
    public static iBuildType none = new iBuildType(){ name = "none", group = GroupType.None,};

    public static iBuildType quadFoundation = new iBuildType(){ name = "quadFoundation", group = GroupType.quadBaseBuild,
        createRaw = QuadFoundation.createRaw,
        anchorGroups = AnchorGroup.createArray(
            AnchorGroup.createUnit("a0", "a1", "a2", "a3")
        ),
        inputAttachGroups = AttachReference.createArray(
            AttachReference.createUnit((int)AttachGroupCategory.Type.withCollision, 0)
        ),
        aGCategories = AttachGroupCategory.createArray(
            AttachGroupCategory.createUnit(
                AttachGroupCategory.Type.withCollision,
                AttachGroup.createArray(
                    new AttachGroup(){ group = iBuildType.GroupType.quadBaseBuild, anchorGroupIndex = 0},
                    new AttachGroup(){ group = iBuildType.GroupType.triBaseBuild, anchorGroupIndex = 0},
                    new AttachGroup(){ group = iBuildType.GroupType.wallBuild, anchorGroupIndex = 0}
                    //new AttachGroup(){ group = iBuildType.GroupType.middleBuild, anchorGroupIndex = 1}
                )
            )
        )
    };
    public static iBuildType fullWall = new iBuildType(){ name = "fullWall", group = GroupType.wallBuild,
        createRaw = FullWall.createRaw,
        anchorGroups = AnchorGroup.createArray(
            AnchorGroup.createUnit("i0"),
            AnchorGroup.createUnit("o0", "o1")
        ),
        inputAttachGroups = AttachReference.createArray(
            AttachReference.createUnit((int)AttachGroupCategory.Type.withoutCollision, 0),
            AttachReference.createUnit((int)AttachGroupCategory.Type.withoutCollision, 1),
            AttachReference.createUnit((int)AttachGroupCategory.Type.withoutCollision, 2)
        ),
        aGCategories = AttachGroupCategory.createArray(
            AttachGroupCategory.createUnit(
                AttachGroupCategory.Type.withoutCollision,
                AttachGroup.createArray(
                    new AttachGroup(){ group = iBuildType.GroupType.quadBaseBuild, anchorGroupIndex = 0},
                    new AttachGroup(){ group = iBuildType.GroupType.triBaseBuild, anchorGroupIndex = 0},
                    new AttachGroup(){ group = iBuildType.GroupType.wallBuild, anchorGroupIndex = 0}
                    //new AttachGroup(){ group = iBuildType.GroupType.middleBuild, anchorGroupIndex = 1}
                )
            ),
            AttachGroupCategory.createUnit(
                AttachGroupCategory.Type.withCollision,
                AttachGroup.createArray(
                    new AttachGroup(){ group = iBuildType.GroupType.wallBuild, anchorGroupIndex = 1},
                    new AttachGroup(){ group = iBuildType.GroupType.quadfloorBuild, anchorGroupIndex = 1},
                    new AttachGroup(){ group = iBuildType.GroupType.triFloorBuild, anchorGroupIndex = 1}
                )
            )
        )
    };
    public static iBuildType caveThHole = new iBuildType(){ name = "caveThHole", group = GroupType.caveBlock,
        createRaw = QuadFoundation.createRaw,
        anchorGroups = AnchorGroup.createArray(
            AnchorGroup.createUnit("a0", "a1", "a2", "a3")
        ),
        inputAttachGroups = AttachReference.createArray(
            AttachReference.createUnit((int)AttachGroupCategory.Type.withCollision, 0)
        ),
        aGCategories = AttachGroupCategory.createArray(
            AttachGroupCategory.createUnit(
                AttachGroupCategory.Type.withCollision,
                AttachGroup.createArray(
                    new AttachGroup(){ group = iBuildType.GroupType.caveBlock, anchorGroupIndex = 0},
                    new AttachGroup(){ group = iBuildType.GroupType.caveBlock, anchorGroupIndex = 0},
                    new AttachGroup(){ group = iBuildType.GroupType.caveBlock, anchorGroupIndex = 0}
                )
            )
        )
    };
    //##########################################################
    public AnchorGroup[] anchorGroups; 
    public AttachReference[] inputAttachGroups;
    public string name;
    public uint index;
    public GroupType group;
    public AttachGroupCategory[] aGCategories;
    public CreateRaw createRaw;
    public string modelPath;

    //--------------------------------------------------------
    public AttachReference getInputGroup( iBuildType.GroupType group){
        for( int i = 0; i < inputAttachGroups.Length; i++){
            var input = inputAttachGroups[i];
            if( input.attachGroup.group == group){
                return input;
            }
        }
        return null;
    }
    public int getAnchorLength( uint aCatI, uint aGroupI){
        return anchorGroups[ aGCategories[ aCatI].aGroups[ aGroupI].anchorGroupIndex].anchorNames.Length;
    }
    //##########################################################

    public static iBuildType[] allTypes = new iBuildType[]{
        quadFoundation,
        fullWall,
    };
    //------------------------------------------------------------
    public static void SetResourcesPaths(){
        caveThHole.modelPath = "cave/cave/cave_ThHole";
    }

    //------------------------------------------------------------
    public static void init(){
        for( uint i = 0; i < allTypes.Length; i++ ){
            iBuildType type = allTypes[i];
            type.index = i;

            for( uint j = 0; j < type.inputAttachGroups.Length; j++){
                type.inputAttachGroups[j].getAttachGroup( type);
            }
        }
        SetResourcesPaths();
    }
    public static iCollision createCollison( iBuildType.GroupType group){
        switch( group){
            case iBuildType.GroupType.quadBaseBuild:
                return QuadBaseCollision.createRaw();
            case iBuildType.GroupType.wallBuild:
                return WallBuildCollision.createRaw();
            case iBuildType.GroupType.caveBlock: 
                return QuadBaseCollision.createRaw();
            case iBuildType.GroupType.None: 
                Debug.LogError("Error: iBuildType.GroupType.None is not a valid group type");
                return null;
        }
        return null;
    }
    
}

