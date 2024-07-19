using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    static int hp = 100;
    static int flow = 100;
    static int enemMult = 1;
    public static int GetHp(){
        return hp;
    }
    public static int GetFlow(){
        return flow;
    }
    public static void changeMult(){
        if(flow > 75){
            enemMult = 1;
        }else if(flow > 50){
            enemMult = 2;
        }else if(flow > 25){
            enemMult = 3;
        }else{
            enemMult = 5;
        }
    }
    public static void DecreaseHp(int howMuch){
        hp -= (howMuch * enemMult);
    }
    public static void DecreaseFlow(int howMuch){
        flow -= howMuch;
        changeMult();
    }
}
