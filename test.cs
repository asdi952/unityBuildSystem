using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Option<int>[] arr;

    void Start(){
        arr = new Option<int>[10];

        misc.forAll<Option<int>>( arr, ( ref Option<int> a, uint i)=>{
            a.set( (int)i);
            print(a.get());
        });
    }

    void Update(){
        if( Input.GetMouseButtonDown(0)){
            print();
        }
    }
    void print(){
        print( arr.Length);
        // for( int i = 0; i < arr.Length; i++){
        //     print( arr[i].get());
        // }
        misc.forAll<Option<int>>( arr, ( ref Option<int> a, uint i)=>{
            print(a.get());
        });
    }
}
