using UnityEngine;
using System;

public struct Option<T> 
{
    public enum State{ exist, notExist}

    private T value;
    public State state;

    public void set( T val){
        value = val;
        state = State.exist;
    }
    public T get(){
        return value;
    }
    public T consume(){
        state = State.notExist;
        return value;
    }
    public bool exist(){
        return state == State.exist;
    }
    public bool empty(){
        return state == State.notExist;
    }
    public void reset(){
        state = State.notExist;
    }
}
public static class misc {
    public delegate void foreachFunc<T>(T array);
    public static void forAll<T>( T[] array, foreachFunc<T> func) {
        for( uint i = 0; i < array.Length; i++ ){
            func( array[i]);
        }
    }
    public delegate void foreachFuncRef<T>( ref T array);
    public static void forAll<T>( T[] array, foreachFuncRef<T> func) {
        for( uint i = 0; i < array.Length; i++ ){
            func( ref array[i]);
        }
    }
    public delegate void foreachFuncI<T>(T array, uint index);
    public static void forAll<T>( T[] array, foreachFuncI<T> func) {
        for( uint i = 0; i < array.Length; i++ ){
            func( array[i], i);
        }
    }

    public delegate void foreachFuncRefI<T>(ref T array, uint index);
    public static void forAll<T>( T[] array, foreachFuncRefI<T> func) {
        for( uint i = 0; i < array.Length; i++ ){
            func( ref array[i], i);
        }
    }

    public delegate void CheckTimerFunc();
    public class CheckTimer{
        public CheckTimer( CheckTimerFunc func, float period){
            this.period = period;
            this.func = func;
            nextTime = Time.time + period;
        }
        public void checkTime(){
            if( Time.time >= nextTime){
                func();
                nextTime += period;
            }
        }
        CheckTimerFunc func;
        float nextTime;
        float period;
    }
    public static string stackStrace(){
        System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
        
       return t.ToString();
    }
}





