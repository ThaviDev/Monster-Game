using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Pathfinding;
using UnityEngine.UI;
using UnityEngine;

public class MG_GameManager : MonoBehaviour {
    private int linDestiny;
    public int colMessageNum;
    private GameObject player;
    private GameObject monster;
    public PlayerMotor playerSCRPT;
    public MG_SceneManager sceneManager;
    public Text message;
    private float messageTime;
    private bool canDissapearMessage;
    private MG_MonsterChaserManager monsChaserScrpt;
    private AIPath aiPath_Monster;
    public Collider2D[] linealColliders;
    public Collider2D[] colMessages;
    void Awake(){
        //DontDestroyOnLoad(this.gameObject);
    }
    void Start () {
        linDestiny = 0;
        monsChaserScrpt = FindObjectOfType<MG_MonsterChaserManager>();
        aiPath_Monster = FindObjectOfType<AIPath>();
        player = FindObjectOfType<PlayerMotor>().gameObject;
        monster = FindObjectOfType<MG_MonsterMotor>().gameObject;
        //Message_StartGame1();
        ColidersMessage();
        sceneManager = FindObjectOfType<MG_SceneManager>();
        monster.SetActive(false);
        linealColliders[0].enabled = false;
        linealColliders[1].enabled = false;
	}
	
	void Update () {
		if (messageTime >= 1){
			canDissapearMessage = true;
		}
		if (canDissapearMessage == true){
			messageTime -= Time.deltaTime;
		}
        if (messageTime <= 0){
			canDissapearMessage = false;
            message.text = "";
		}
        switch (linDestiny)
        {
            case 0:
            linealColliders[0].enabled = true;
            break;
            case 1:
            linealColliders[0].enabled = false;
            linealColliders[1].enabled = true;
            monster.SetActive(true);
            monsChaserScrpt.enabled = true;
            aiPath_Monster.enabled = true;
            monsChaserScrpt.wonderingAccess = false;
            break;
            case 2:
            monster.transform.position = new Vector3(-22f, 240f,0f);
            linealColliders[1].enabled = false;
            monsChaserScrpt.enabled = false;
            aiPath_Monster.enabled = false;
            monsChaserScrpt.enabled = true;
            aiPath_Monster.enabled = true;
            monsChaserScrpt.wonderingAccess = true;
            linDestiny = 3;
            break;
            case 3:
            print ("parte lineal completada");
            break;
            default:
            break;
        }
	}
    void OnTriggerEnter2D(Collider2D col){
        if (col.tag == "Player" || col.tag == "Monster"){
            //linealColliders[linealSeries].enabled = false; // Se desactiva la colision actual
            linDestiny += 1; // Se suma 1 la serie lineal
            print ("Player Touched collider");
        }
    }

    public void ColidersMessage(){
        // Tutorial, avanza cuando colisionas con ColMessages
        switch (colMessageNum)
        {
            case 0:
            messageTime = 12;
            message.text = "Mover: WASD, apunta linterna con Mouse";
            break;
            case 1:
            messageTime = 12;
            message.text = "Apunta a una roca cerca y tomala con click derecho";
            break;
            case 2:
            messageTime = 12;
            message.text = "Este puede ser un buen lugar para esconderse";
            break;
            case 3:
            messageTime = 12;
            message.text = "Puedes correr con Shift izquierdo";
            break;
            default:
            break;
        }
    }
    public void ExitFunctionGameplay(){
        print ("Jugador Toco Salida");
        if (playerSCRPT.pistasAmount < 5){
            Message_5Pistas();
        }
        if (playerSCRPT.pistasAmount >= 5){
            sceneManager.ButtonPlayerWon();
        }
    }
    public void ExitGame(){
        //Encuentra el script de continuar el juego y lo resumea
        MG_PauseMenu pm = FindObjectOfType<MG_PauseMenu>();
        pm.Resume();
        //Entra a la escena del menu principal
        sceneManager.ButtonSalirToMenu();
    }
    public void MonsterReachedPlayer(){
        sceneManager.ButtonPlayerLost();
        //sceneManager.PlayerLost();
    }
    public void Message_5Pistas(){
        messageTime = 10;
        message.text = "Necesitas 5 pistas para escapar";
    }
}
