//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.Audio;
using Pathfinding;
using UnityEngine;

public class MG_MonsterChaserManager : MonoBehaviour {

    [Header("Vision")]
    private float visionRadiousIG;// El radio de la vision constante del monstruo perseguidor
    private float visionRadiousConcentrated;// El radio de la vision cuando el monstruo perseguidor está en estado de busqueda
    private float visionRadiousChasing;// El radio de la vision cuando el monstruo perseguidor está en estado de persecucion
    [Header("Speed")]
    private float speedIG;// La velocidad constante del monstruo
    //public float speedChasing;// La velocidad del monstruo perseguidor cuando está en estado de persecucion
    //public float speedWondering;// La velocidad del monstruo perseguidor cuando está en estado de busqueda
    private GameObject _player;// El objeto del jugador en la escena
    [Header("Posicions")]
    private Transform TargetTransform;// La ultima posicion donde el monstruo perseguidor vió al jugador
    [Header("Other")]
    public Collider2D rockDestroyerGMO;// El objeto con la colision para destruir la roca del monstruo perseguidor
    public bool wonderingAccess; // Acceso a poder moverse en estado wondering
    private Animator anim; // el animador del monstruo
    //bool isChasingPlayer = false;
    float timeOnRock; // Medida de tiempo que el monstruo perseguidor utiliza para romper la roca
    float patienceTimeFT; // Medida de tiempo utilizada de varias maneras por el monstruo perseguidor
    public bool playerDetection = false; // Si se detectó al jugador en el area de vision del monstruo perseguidor
    //public AudioSource monsterAudioSource;
    //public AudioClip chasingSound;
    //public AudioClip wonderingSound;
    bool chasePlayerConstant = true; // Si se detectó al jugador en el area de vision del monstruo perseguidor
    float wonderingDirection = 1f;
    bool hasDesicionTaken = false;
    private float animTimer = 0f; // el timer de la direccion de animacion del monstruo (cuantas veces identifica la direccion de la animacion)
    //public GameObject monstVisual; // Visual del monstruo en pantalla
    //string dirAnim; // detecta a que dirección se mueve el objeto
    float animFloatX = 0; // guarda la informacion de la diferencia entre pos_actual y pos_ultima en x
    float animFloatY = 0; // guarda la informacion de la diferencia entre pos_actual y pos_ultima en y
    private AIDestinationSetter destinationSetter;
    private AIPath aiPath_Monster;
    Vector3 lastPos; //la ultima posicion de vector 2 del monstruo (esta variable sirve para detectar si el monstruo se esta moviendo y hacia que direccion)
    //public GameObject chaseMusic;
    //public GameObject ambientMusic;


    //float distanceFromPlayer = Vector2.Distance(playerFollowGMO.transform.position, transform.position);

    void Start () {
        //_player = GameObject.FindObjectOfType<PlayerMotor>();
        // Muchos de estos objetos que encuentra este codigo deberían de ser llamados y utilizados en el codigo general del monstruo ---
        _player = GameObject.Find("Player");
        TargetTransform = GameObject.Find("Mons Target").transform;
        rockDestroyerGMO = GetComponentInChildren<Collider2D>();
        aiPath_Monster = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        anim = GetComponentInChildren<Animator>();
        visionRadiousIG = visionRadiousConcentrated;
        timeOnRock = 5;
        wonderingAccess = true;
    }
	void Update () 
    {//-- BEGGIN UPDATE
        print (animTimer);
        //-- Linea del monstruo a jugador ---
        Debug.DrawLine(transform.position, _player.transform.position, Color.green);
        //-- El maximo tiempo del timer de la animacion del monstruo (animTimer)
        float animTimerMax = .3f; 
        //-- Play a sonidos del monstruo (Audio Manager del monstruo)
        FindObjectOfType<AudioManager>().Play("ChaserChasingSound");// Poner el sonido de la persecucion del monstruo
        //-- Acceder acciones del monstruo
        MonsterTypeChaser();
        //-- La velocidad maxima del pathfinding es la velocidad general del monstruo
        aiPath_Monster.maxSpeed = speedIG;
        //-- El tiempo de roca siempre está disminuyendo si es mayor a 0
        if (timeOnRock >= 0){
        timeOnRock -= Time.deltaTime;
        }
        //----- DIRECCION DE ANIMACION
        /*-- Anim timer cuenta un pequeño tiempo para cambiar la animacion de una a otra, se toma la posicion actual del monstruo y se resta por la
        anterior que tenía para determinar a que direccion este se está dirigiendo*/
        animTimer -= Time.deltaTime;
        if (animTimer <= 0f){
            animFloatX = this.gameObject.transform.position.x - lastPos.x;// Resta la posicion actual con la ultima posicion en X
            animFloatY = this.gameObject.transform.position.y - lastPos.y;// Resta la posicion actual con la ultima posicion en Y
            //print ("X: " + animFloatX + " Y: " + animFloatY);
            if (Mathf.Abs(animFloatX) >= Mathf.Abs(animFloatY) && Mathf.Abs(animFloatX) > .005f){ // Si el valor absoluto de X y es más grande que 00.5
                if (animFloatX > 0){
                    anim.SetInteger("movementDirection", 2);//Derecha
                }
                else{
                    anim.SetInteger("movementDirection", 1);//Izquierda
                }
            }
            else if (Mathf.Abs(animFloatX) < Mathf.Abs(animFloatY) && Mathf.Abs(animFloatY) > .005f){ // Si el valor absoluto de Y y es más grande que 00.5
                if (animFloatY > 0){
                    anim.SetInteger("movementDirection", 3);//Arriba
                }
                else{
                    anim.SetInteger("movementDirection", 4);//Abajo
                }
            } 
            else {
                anim.SetInteger("movementDirection", 0);//Quieto
            }
            lastPos = this.gameObject.transform.position;// La posicion actual se vuelve en la ultima posicion para la ultima contada
            animTimer = animTimerMax; // << El animTimer vuelve a contar
        }
    }//-- END UPDATE

    void MonsterTypeChaser(){
        float distanceFromPlayer = Vector2.Distance(_player.transform.position, transform.position);
        destinationSetter.target = TargetTransform;
        //-- Si se localiza al jugador, cambia el estado a PERSEGUIR
        if (distanceFromPlayer < visionRadiousIG && _player.GetComponent<PlayerMotor>().isOnBush == false)
        {
            //-- Se detecto al jugador
            playerDetection = true;
            chasePlayerConstant = true;
            //-- La posicion de busqueda ahora es exactamente en la posicion del jugador
            TargetTransform.position = _player.transform.position;
            patienceTimeFT = Random.Range(4,7);
        }
        //-- Estado de Persecucion del Monstruo
        if (playerDetection == true){
            MonsterTypeChaser_ChasingPlayer();
            //-- Musica de Persecucion
            //chaseMusic.SetActive(true);
            //ambientMusic.SetActive(false);
        }
        //-- Estado de Busqueda del Monstruo
        if (playerDetection == false){
            // Supongo que esto temporal para el primer nivel
            if (wonderingAccess == false){
                MonsterTypeChaser_Linear();
            }
            if (wonderingAccess == true){
                MonsterTypeChaser_Wondering();
            }
            //-- Musica de Ambiente
            //chaseMusic.SetActive(false);
            //ambientMusic.SetActive(true);
        }

        // >> Monster Destroy Rock State ----------------------------------------
        if (timeOnRock > 0) {
            this.GetComponent<Collider2D>().enabled = false;
            //timeOnRock -= Time.deltaTime;
            speedIG = 0f;
            //-- El monstruo detiene su camino para destruir la roca
            aiPath_Monster.enabled = false;
            //-- El monstruo activa el Col Destructor de Rocas cuando llega a cierto tiempo
            if (timeOnRock <= .3f){
                //-- Activar el colisionador de rocas
                rockDestroyerGMO.enabled = true;
                //-- Mover el objeto un milimetro de nada para que funcione...
                if (timeOnRock > .2f){
                transform.Translate(.001f,.001f,0f);
                }
                //-- Activar Colisionador Destructor de Roca
                rockDestroyerGMO.enabled = true;
            }
        }
        if (timeOnRock <= 0)
        {
            //-- Reactivar el colisionador del monstruo
            this.GetComponent<Collider2D>().enabled = true;
            //-- Desactivar el Col Destructor de Rocas
            rockDestroyerGMO.enabled = false;
            //-- Continuar el camino
            aiPath_Monster.enabled = true;
            // aquí estaría mejor si una variable declarara SI el monstruo puede moverse, no que determine la velocidad
        }
    }

    void MonsterTypeChaser_ChasingPlayer()
    {
        //monsterAudioSource.clip = chasingSound;
        //FindObjectOfType<AudioManager>().Play("ChaserChasingSound");
        float RelativePosTargetX = this.gameObject.transform.position.x - TargetTransform.transform.position.x;
        float RelativePosTargetY = this.gameObject.transform.position.y - TargetTransform.transform.position.y;
        visionRadiousIG = visionRadiousChasing;
        float distanceFromPlayer = Vector2.Distance(_player.transform.position, transform.position);
        if (chasePlayerConstant == true){
            if (distanceFromPlayer < visionRadiousIG){
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, speedIG * Time.deltaTime);
            }
        }
        if (chasePlayerConstant == false){
            transform.position = Vector2.MoveTowards(transform.position, TargetTransform.transform.position, speedIG * Time.deltaTime);
            // deja de perseguir al jugador e ir al ultimo punto donde se le vio
            print("No lo veo, a donde se fue?");
            if (patienceTimeFT <= 0){
                print ("No pues no está aquí, ya debería irme");
                playerDetection = false;
            }
            //if (transform.position == TargetTransform.transform.position){ // Si la posicion del Monstruo es la misma que el target
            if (Mathf.Abs(RelativePosTargetX) < 0.05 && Mathf.Abs(RelativePosTargetY) < 0.05){
                // espera 5 segundos
                patienceTimeFT -= Time.deltaTime/2;
            }
            //print(patienceTimeFT);
        }
        if (distanceFromPlayer > visionRadiousIG || _player.GetComponent<PlayerMotor>().isOnBush == true) // Si el jugador deja de estar en la vision o se esconde, chasePlayerConstant es falso
            chasePlayerConstant = false;
        //if (transform.position == TargetTransform.transform.position)
            //patienceTimeFT -= Time.deltaTime;

    }
    void MonsterTypeChaser_Wondering(){
        //FindObjectOfType<AudioManager>().Play("ChaserWonderingSound");
        //El radio de vision se hace más grande
        visionRadiousIG = visionRadiousConcentrated;
        //monsterAudioSource.clip = wonderingSound;
        //Empieza a tomar una decision de a donde ir
        if (hasDesicionTaken == false){
            //Decide tomar una posicion aleatoria
            wonderingDirection = Random.Range(1,6);
            //Decide dirigirse a esta en un tiempo aleatorio
            patienceTimeFT = Random.Range(5,20);

            if (wonderingDirection == 1){
                TargetTransform.position = new Vector3 (_player.transform.position.x, _player.transform.position.y + 100, 0f);
            }
            if (wonderingDirection == 2){
                TargetTransform.position = new Vector3 (_player.transform.position.x, _player.transform.position.y - 100, 0f);
            }
            if (wonderingDirection == 3){
                TargetTransform.position = new Vector3 (_player.transform.position.x + 100, _player.transform.position.y, 0f);
            }
            if (wonderingDirection == 4){
                TargetTransform.position = new Vector3 (_player.transform.position.x - 100, _player.transform.position.y, 0f);
            }
            if (wonderingDirection == 5){
                TargetTransform.position = new Vector3 (_player.transform.position.x + patienceTimeFT - 5f, _player.transform.position.y - patienceTimeFT + 5f , 0f);
            }
            //Se a tomado a donde ir y en que tiempo
            hasDesicionTaken = true;
        }
        //Empezar a dirigirse a esta direccion
        if (hasDesicionTaken == true){
            transform.position = Vector2.MoveTowards(transform.position, TargetTransform.position, speedIG * Time.deltaTime);
            //Cuando se acabe el tiempo de paciencia, el monstruo dejara de perseguir y reiniciará el ciclo
            patienceTimeFT -= Time.deltaTime;
            if (patienceTimeFT <= 0)
            hasDesicionTaken = false;
        }
    }
    void MonsterTypeChaser_Linear(){
        TargetTransform.position = new Vector3(24f,62f,0f);
        destinationSetter.target = TargetTransform;
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Rock" && timeOnRock < 5 || col.gameObject.tag == "Big Rock" && timeOnRock < 5)
        {
            //-- Activar Contador de destruccon de roca cuando el monstruo toque una
            timeOnRock = 2f;
        }
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Bush")
        {
            print("Monstruo Toco Arbusto");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, visionRadiousIG);
    }


}
