using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Yarn.Unity;

public class PuzzleRooms : MonoBehaviour
{
    public GameObject player;
    public GameObject[] rooms;
    public GameObject[] interactables;

    // Drag in same referred StatUI as the other scripts in the Inspector
    public StatUI statUI;

    public void OnInteract(InputAction.CallbackContext context){
        if(context.performed){
            if(PlayerStats.touchingRoom){
                for(int i=0; i<rooms.Length; ++i){
                    if(rooms[i] == PlayerStats.touchingWhat){
                        if(i%2==0){
                            player.transform.position = rooms[i+1].transform.position;
                        }else{
                            player.transform.position = rooms[i-1].transform.position;
                        }
                    }
                }
            }
            else if (PlayerStats.touchingBench) {
                PlayerStats.RestoreHp(); 
                Debug.Log("Restored HP"); 
                statUI.ChangeHP(PlayerStats.GetHp());
            } 
            else if(PlayerStats.touchingInteractable){
                // For other interactable items 
            }
        }
    }
}
