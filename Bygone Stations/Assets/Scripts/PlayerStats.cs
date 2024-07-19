using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    static int hp = 100;
    static int guage = 100;
    static int enemMult = 1;
    public static int GetHp(){
        return hp;
    }
    public static int GetGuage(){
        return guage;
    }
    public static void changeMult(){
        if(guage > 75){
            enemMult = 1;
        }else if(guage > 50){
            enemMult = 2;
        }else if(guage > 25){
            enemMult = 3;
        }else{
            enemMult = 5;
        }
    }
    public static void DecreaseHp(int howMuch){
        hp -= (howMuch * enemMult);
    }
    public static void DecreaseGuage(int howMuch){
        guage -= howMuch;
        changeMult();
    }
}
