using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CnControls;

public class Controll : MonoBehaviour
{
    public bool Bot;
    [Space]
    public float Speed;

    [Space]
    public int foodRate;
    public float foodCur;
    public float evoCur;

    [Space]
    public int skillRightButton;
    public int level;
    public bool predator;
    public bool scavenger;
    public bool longNeck;
    public bool biggest;
    public bool tailDropping;
    public bool tracking;
    public bool poison;
    public bool flight;
    public bool camouflage;
    public bool jump;


    [Space]
    public GameObject model;
    public Animator animator;
    public SpriteRenderer s_indicate;
    public GameObject eatTarget;
    public TextMesh t_nickname;
    public TextMesh t_nicknameSdw;
    public TextMesh t_level;
    public GameObject corpse;
    public GameObject arrow;
    public GameObject attention;
    public GameObject i_droptail;
    public GameObject i_poison;
    public GameObject eyeHidden;

    private GameObject ii_tracker;
    private GameObject ii_biggest;
    private GameObject ii_camouflage;
    private GameObject ii_jump;

    private Shkala shkala;
    private float timerFood;
    private GameObject CharacterUI;
    private SkillManager SM;
    [HideInInspector]
    public GameManager GM;
    private bool PlayerStarted;

    private float evoNeed;
    private float evoCoef;
    private float tailDropSpeed;
    private float jumpSpeed;
    [HideInInspector]
    public bool hidden;
    private float raycastTimer;
    [HideInInspector]
    public bool immortal;

    [Space]
    public string nickname;
    // BOT PARAMS ------------------
    public GameObject Target;
    private int targetPrior; // 0 - null, 10 - movePivot, 20 - food, 25 - evo,  30 - player
    private GameObject botPivot;
    private BotPivotsMove BotPivotsMove;
    private GameObject Player;
    private float timerRangePlayer;
    private bool playerTrackerDone;
    private bool botfear;
    private bool doHide;

    private float timerBotMove;

    private AudioSource s_eat;
    private AudioSource s_evoPick;
    private AudioSource s_kill;
    private AudioSource s_levelUp;
    private AudioSource s_bush;
    private Text t_lvl;

    private float atkTimer;
    public GameObject modelPlace;


    private GameObject i_dangeonzone;
    private float dztimer;
    // Start is called before the first frame update
    void Awake(){
      if (!Bot){
        s_eat = GameObject.Find("s_eat").GetComponent<AudioSource>();
        s_evoPick = GameObject.Find("s_evoPick").GetComponent<AudioSource>();
        s_kill = GameObject.Find("s_kill").GetComponent<AudioSource>();
        s_levelUp = GameObject.Find("s_levelUp").GetComponent<AudioSource>();
        s_bush = GameObject.Find("s_bush").GetComponent<AudioSource>();
        t_lvl = GameObject.Find("t_lvl").GetComponent<Text>();
      }
    }

    public void StartPlayer()
    {
      evoNeed = 100;
      evoCoef = 1;
      timerFood = 1;
      SM = GameObject.Find("SkillManager").GetComponent<SkillManager>();
      GM = GameObject.Find("GameManager").GetComponent<GameManager>();


      foodCur = 100;

      if (!Bot){
        t_lvl.text = level+"";
        shkala = GameObject.Find("SHKALA").GetComponent<Shkala>();
        shkala.heroControll = this;
        shkala.ChangeFoodRate(foodRate);
        nickname = PlayerPrefs.GetString("Nickname");
        s_indicate.enabled = false;
        attention.active = false;
        eyeHidden.transform.parent = null;

        i_dangeonzone = GameObject.Find("i_dangeonzone");
          i_dangeonzone.transform.parent.gameObject.active = false;
        ii_tracker = GameObject.Find("ii_tracker");
        ii_biggest = GameObject.Find("ii_biggest");
        ii_camouflage = GameObject.Find("ii_camouflage");
        ii_jump = GameObject.Find("ii_jump");
          ii_tracker.active = false;
          ii_biggest.active = false;
          ii_camouflage.active = false;
          ii_jump.active = false;

        
        Speed += PlayerPrefs.GetInt("curSkin")/25.0f;

        model = Instantiate(GM.skins[PlayerPrefs.GetInt("curSkin")], modelPlace.transform);
        animator = model.GetComponent<Animator>();

      }else{
        Speed += Random.Range(0.01f, 0.64f);
        evoCoef = Random.Range(0.3f, 0.81f);
        BotPivotsMove = GameObject.Find("BotPivotsMove").GetComponent<BotPivotsMove>();
        Player = GM.Player;
        nickname = GM.GenerateNickname();
        //animator.SetBool("Run", true);



        model = Instantiate(GM.skins[Random.Range(0, GM.skins.Length)], modelPlace.transform);
        animator = model.GetComponent<Animator>();
        animator.SetInteger("Move", 2);
      }



      t_nickname.text = nickname;
      t_nicknameSdw.text = nickname;

      GetEvo(100);

      if (CharacterUI) CharacterUI.transform.position = transform.position;
      PlayerStarted = true;
    }



    public void BotTarget(GameObject g, int prior){
      if (targetPrior < prior){
        Target = g;
        targetPrior =  prior;
        if (botfear){
          timerBotMove = Random.Range(0.8f, 3.1f);
        }else{
          if (prior == 30) doHide = false;
          timerBotMove = Random.Range(4.1f, 9.1f);

          if (Target == Player) timerBotMove = Random.Range(1.5f, 4.5f);

        }
      }
    }

    // Update is called once per frame
    void FixedUpdate(){
      if (PlayerStarted){
            if (!Bot){
                if (Input.GetMouseButton(0)){
                    if (CnInputManager.GetAxis("Horizontal") != 0 || CnInputManager.GetAxis("Vertical") != 0)
                    {
                      animator.SetInteger("Move", 2);
                      MovePlayer(new Vector2(CnInputManager.GetAxis("Horizontal"), CnInputManager.GetAxis("Vertical"))*100);
                      //animator.SetBool("RUN", true);
                    }
                }else{
                  animator.SetInteger("Move", 0);
                }

                if (Input.GetMouseButtonDown(1)){

                //  SM.ApplySkill(skillRightButton,gameObject);
                }
            }

            if (CharacterUI){
                if (!hidden){
                  CharacterUI.transform.position = transform.position;
                  eyeHidden.transform.position = Vector3.up*999;
                }else{
                  CharacterUI.transform.position = Vector3.up*999;
                  eyeHidden.transform.position =  transform.position;
                }
             }
      }
    }

    void Update()
    {
      if (PlayerStarted){

          timerFood -= Time.deltaTime*0.4f;
          if (timerFood < 0){
            timerFood = 1;
            foodRate = 0;
            //GetFood(foodRate);
          }

          atkTimer -= Time.deltaTime;
          if (atkTimer < 0){
            //if (atkTimer > -3) animator.SetBool("Attack", false);
          }

          if (tailDropSpeed > 0){
            tailDropSpeed -= Time.deltaTime*4.2f;
            if (tailDropSpeed < 7f)immortal = false;
          }else{

            tailDropSpeed = 0;
          }

          if (jumpSpeed > 0){
            jumpSpeed -= Time.deltaTime*6;
          }else{
            jumpSpeed = 0;
          }

          if (!Bot){
              //if (transform.position.z > GM.gameZone || transform.position.x > GM.gameZone){
              if (Vector3.Distance(transform.position, Vector3.zero) > GM.gameZone){
                if (dztimer == 10) i_dangeonzone.transform.parent.gameObject.active = true;
                dztimer -= Time.deltaTime;
                i_dangeonzone.GetComponent<Image>().fillAmount = dztimer/10;


                if (dztimer < 0){
                    i_dangeonzone.transform.parent.gameObject.active = false;
                   Death(1);
                 }
              }else{
                if (dztimer != 10) i_dangeonzone.transform.parent.gameObject.active = false;
                dztimer = 10;
              }
          }



          /////////////////////////       BOT         //////////////////////////////
          if (Bot){
            if (!Target){
               targetPrior = 0;
               BotTarget( BotPivotsMove.GetPivot(), 10 );
             }

             if (Target){
               timerBotMove -= Time.deltaTime;

               if (botfear){
                MovePlayer(new Vector2(Target.transform.position.x-transform.position.x, Target.transform.position.z-transform.position.z)*-100);
               }else{
                if (!doHide) MovePlayer(new Vector2(Target.transform.position.x-transform.position.x, Target.transform.position.z-transform.position.z)*100);
               }



               if (hidden && foodCur > 65 && doHide == false){
                 foodCur -= Time.deltaTime;
                 if (Target.tag != "Player" || Target.tag != "Food" || Target.tag != "Evo"){
                  // doHide = true;
                   timerBotMove = Random.Range(3, 8);
                 }else{
                   doHide = false;
                 }
               }else{
                 if (!hidden && doHide){
                   doHide = false;
                   timerBotMove = -1;
                 }


                  if (Target.tag == "Player" || Target.tag == "Food" || Target.tag == "Evo"){
                      doHide = false;
                  }
               }

               if (foodCur < 50) doHide = false;

               if (Vector3.Distance(Target.transform.position, transform.position) < 0.5f){
                   Target = null;
               }
               if (timerBotMove < 0){ Target = null; botfear = false; doHide = false;}
             }


             timerRangePlayer -= Time.deltaTime;
             if (timerRangePlayer < 0){
                 timerRangePlayer = 1;

                 if (Player){
                   if (Vector3.Distance(Player.transform.position, transform.position) < 20){ //distance player and bot
                     if (!playerTrackerDone){
                       if (Player.GetComponent<Controll>().tracking){
                          playerTrackerDone = true;
                          arrow.GetComponent<OnArrow>().StartIt(Player, gameObject);
                        }
                     }

                     if (CheckKill(Player)){
                       s_indicate.color = new Color(1, 0.1f, 0, 0.5f);
                       attention.active = true;
                       eatTarget.active = false;
                     }else if (Player.GetComponent<Controll>().CheckKill(gameObject)) {
                       s_indicate.color = new Color(0.3f, 1, 0.3f , 0.6f);
                       attention.active = false;
                       eatTarget.active = true;
                     }else{
                       s_indicate.color = new Color(1, 1, 1, 0.4f);
                       attention.active = false;
                       eatTarget.active = false;
                     }

                   }
                 }

             }


          }

      }



      if (jump){

        raycastTimer -= Time.deltaTime;

        if (raycastTimer < 0){
          raycastTimer = 0.35f;
          RaycastHit hit;
          // Does the ray intersect any objects excluding the player layer
          int layerMask = 1 << 8;
          if (Physics.Raycast(transform.position+(transform.forward*1.3f), transform.TransformDirection(Vector3.forward), out hit, 6.5f, layerMask))
          {
            if (CheckKill(hit.collider.gameObject)) jumpSpeed = 4;
          }
        }
      }
    }

    public bool CheckKill(GameObject trg){
      bool cankill = true;
      Controll controllTrg = trg.GetComponent<Controll>();

      if (predator){
        if (controllTrg.biggest && !biggest) cankill = false; // check for biggest skill
        if (controllTrg.flight && !flight) cankill = false; // check for biggest skill

        if (controllTrg.predator && cankill ){ // check for biggest skill for level after all checks
          if (controllTrg.level >= level){
            cankill = false;
          }
        }


        if (!cankill){
          if (!controllTrg.biggest && biggest) cankill = true;
          //if (!controllTrg.flight && flight) cankill = true;
        }
      }else{
        cankill = false;
      }


      return cankill;
    }

    public void GetEvo(int cnt){
      evoCur += (int)(cnt*evoCoef);
      if (evoCur >= 100){
         evoCur -= 100;
         // SHOW SKILLS
         if (!Bot){
           s_levelUp.Play();
           if (GM.ShowLevelSkills){
             GM.DoEffect(transform.position+Vector3.up, 1);
             SM.ShowSkills();
           }
         }else{
           if (GM.ShowLevelSkills){
             int snum = (int)SM.SkillRandom(gameObject).x;
             SM.ApplySkill(snum,gameObject);
           }
         }

         level += 1;
         t_level.text = level +"";


         if (!Bot){
           t_lvl.text = level+"";
           Camera.main.GetComponent<CameraMovement>().SetFieldView(0.2f,1);
         }
         model.GetComponent<OnModelEffect>().DoBig();
       }else{
          if (!Bot) s_evoPick.Play();
       }


      if (!Bot) shkala.SetEvo(evoCur);
    }

    public void GetFood(float cnt){
      foodCur += cnt;
      if (foodCur > 100) foodCur = 100;

      if (foodCur <= 0){
        foodCur = 0; // DEAD c GOLODA
        Death(1);
      }

      if (!Bot){
         shkala.SetFood(foodCur);
         if (cnt>0)s_eat.Play();
       }
    }

    public void SetCharacterUI(GameObject g){
      CharacterUI = g;
    }



    public void MovePlayer( Vector2 move){

                float x = move.x;
                float y = move.y;
                float coef = 1/x;
                if (x > 1 || x < -1){
                  x = x*Mathf.Abs(coef);
                  y = y*Mathf.Abs(coef);

                }
                if (y > 1 || y < -1){
                  coef = 1/y;
                  x = x*Mathf.Abs(coef);
                  y = y*Mathf.Abs(coef);
                }



                move = new Vector2(x,y);
              //  if (Mathf.Abs(x) > Mathf.Abs(y)) animator.speed = Mathf.Abs(x);
              //  if (Mathf.Abs(y) > Mathf.Abs(x)) animator.speed = Mathf.Abs(y);

          //rb.velocity = new Vector3(move.x * speed * Time.deltaTime * 10, rb.velocity.y, move.y * speed * Time.deltaTime * 10);
          float angle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg;

          float CurAngle = Mathf.LerpAngle(transform.eulerAngles.y, angle, Time.deltaTime*10);
          transform.eulerAngles = new Vector3(0, CurAngle, 0);
          transform.Translate(Vector3.forward * (Speed+tailDropSpeed+jumpSpeed) * Time.deltaTime);
    }

    void GrassHide(bool hide){
      GM.DoEffect(transform.position+Vector3.up, 0);
      if (hide){
        if (!Bot){
           eyeHidden.active = true;
           model.GetComponent<OnModelEffect>().effect_hidden(true, predator);
           s_bush.Play();
         }
        //GetComponent<SphereCollider>().enabled = false;
        hidden = true;
        if (model && Bot) model.transform.localPosition = Vector3.up*999;
      }else{
        if (!Bot){
          eyeHidden.active = false;
          model.GetComponent<OnModelEffect>().effect_hidden(false, predator);
          s_bush.Play();
        }
        //GetComponent<SphereCollider>().enabled = true;
        hidden = false;
        if (model && Bot) model.transform.localPosition = Vector3.zero;
      }
    }



    public void BotFear(){
      botfear = true;
    }

    public void s_predator(){
      predator = true;
      SetHungry(-1);

      if (!Bot){
        shkala.ChangeFoodIcons(predator, scavenger);
        Camera.main.cullingMask = ~(1 << 9);
      }

      t_nickname.color = new Color(1, 0.2f, 0.2f);

      model.GetComponent<OnModelEffect>().effect_predator(true);
    }

    public void s_scavenger(){
      scavenger = true;
      if (!Bot) shkala.ChangeFoodIcons(predator, scavenger);
    //  t_nickname.color = new Color(0.65f, 1, 0.6f);
    }

    public void s_herbivore(){
      predator = false;
    //  SetHungry(1);

      if (!Bot){
         shkala.ChangeFoodIcons(predator, scavenger);
         Camera.main.cullingMask = ~(1 << 10);
       }
      t_nickname.color = new Color(0.65f, 1, 0.6f);

      model.GetComponent<OnModelEffect>().effect_predator(false);
    }

    public void s_fat(){
      foodRate +=1;
      if (foodRate > -1) foodRate = -1;

      //SetHungry(0);
    }

    public void s_swift(){
      Speed += 1f;
      if (Speed > 6.7f) Speed = 6.7f;
    //  SetHungry(-1);
    }

    public void s_evolution(){
      evoCoef += 0.05f;
      if (evoCoef > 3) evoCoef = 3;
    }

    public void s_longNeck(){
      longNeck = true;
      if (!Bot) Camera.main.GetComponent<CameraMovement>().SetFieldView(5,1);
    //  SetHungry(-1);

      model.GetComponent<OnModelEffect>().effect_longNeck();
    }

    public void s_biggest(){
      biggest = true;
      if (!Bot) ii_biggest.active = true;
      //SetHungry(-1);

      if (!Bot){
         Camera.main.GetComponent<CameraMovement>().SetFieldView(3,1);
       }

      model.GetComponent<OnModelEffect>().effect_biggest();
    }

    public void s_droptail(){
      tailDropping = true;
      i_droptail.active = true;
     model.GetComponent<OnModelEffect>().effect_tailDrop(true);
    }

    public void s_tracking(){
      tracking = true;
      if (!Bot) ii_tracker.active = true;
    }

    public void s_poison(){
      poison = true;
      //SetHungry(-1);
      i_poison.active = true;

     model.GetComponent<OnModelEffect>().effect_poison();
    }

    public void s_flight(){
      flight = true;
      //SetHungry(-1);

     model.GetComponent<OnModelEffect>().effect_flight();
    }

    public void s_camouflage(){
      camouflage = true;
      if (!Bot) ii_camouflage.active = true;
    }

    public void s_jump(){
      jump = true;
      //SetHungry(-3);

     model.GetComponent<OnModelEffect>().effect_jump();
     if (!Bot) ii_jump.active = true;
    }


    public void SetHungry(int cnt){
      //foodRate +=cnt;
      if (!Bot) shkala.ChangeFoodRate(foodRate);
    }

    public void Death(int types){ // 0 - enemy kill, 1 - hungry
      if (tailDropping && types == 0){
        tailDropping = false;
        tailDropSpeed = 8;
        //transform.Translate(Vector3.forward*0.15f);
        immortal = true;

       model.GetComponent<OnModelEffect>().effect_tailDrop(false);
       i_droptail.active = false;
        //GetComponent<SphereCollider>().enabled = false;
      }else{

        model.transform.parent = null;
        //animator.SetTrigger("Death");
        animator.SetInteger("Idle", -1);
        Destroy(model, 0.65f);


        PlayerStarted = false;

        Instantiate(corpse, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);

        CharacterUI.transform.position = Vector3.up * 999;

        if (!Bot){
          s_kill.Play();
          GM.SetPlayerLeader();
         }
        GM.FillLeader(nickname);

        if (!Bot) GM.GameEnd(false);
      }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player"){
          if (CheckKill(collision.collider.gameObject)){
            Controll contollEnemy = collision.collider.GetComponent<Controll>();
            if (!contollEnemy.immortal){
              if (!contollEnemy.tailDropping){
                 //GetFood(100);
               }else{
                 if (!Bot) GM.ShowText("ENEMY DROPPED TAIL", 3);
               }
              if (contollEnemy.poison){
                foodRate -=4;
                if (!Bot){
                    s_kill.Play();
                   shkala.ChangeFoodRate(foodRate);
                   GM.ShowText("YOU ARE POISONED", 2);
                 }
              }
              contollEnemy.Death(0);
              if (!Bot){
                 s_kill.Play();
                 GetEvo(25);
               }
              //animator.SetBool("Attack", true);
              atkTimer = 1;
              GM.DoEffect(collision.collider.transform.position+Vector3.up*2, 2);
            }
          }
        }





    }

    void OnTriggerStay(Collider collision)
    {
      if (camouflage && !hidden){
          if (collision.tag == "Grass"){
            GrassHide(true);
          }
      }
    }

    void OnTriggerExit(Collider collision)
    {
      if (camouflage){
        if (collision.tag == "Grass"){
          GrassHide(false);
        }
      }
    }
}
