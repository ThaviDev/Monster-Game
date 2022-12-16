using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_ExitLVL : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col){
        if (col.tag == "player"){
            PlayerMotor _player;
            _player = FindObjectOfType<PlayerMotor>();
            if (_player.pistasAmount >= 5){

            }
            if (_player.pistasAmount < 5){

            }
        }
    }
}
