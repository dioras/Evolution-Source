using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool menu;
    public GameObject Player;
    public GameObject CharacterPrefab;
    [Space]
    public int gold;
    public GameObject[] skins;

    public float gameZone;
    [HideInInspector]
    public GameObject[] Enemys;
    public string[] LeaderString;
      public int leaderNum;
    //  [HideInInspector]
      public int playerNum;


    public GameObject[] Effects;
    [Space]
    public GameObject GameFinish;


    private GameObject GameZone;

    private string[] selectedName;
    private bool nicklistsetted;

    [HideInInspector]
    public int totalPlayers;
    private float totalPlDelay;
    [HideInInspector]
    public Text t_totalHerb;
    [HideInInspector]
    public Text t_totalPredator;

    private InfoText infoText;
    private GameObject Canvas;

    [HideInInspector]
    public bool gameFinished;
    // Start is called before the first frame update
    private bool hungryTime;

    //private Metrica metrica;
    [Space]
    public bool ShowLevelSkills;

    public void DoEffect(Vector3 vec, int num){
      Instantiate(Effects[num], vec, Effects[num].transform.rotation);
    }

    void Start()
    {
      // if (GameObject.Find("AppMetrica")){
      //   metrica = GameObject.Find("AppMetrica").GetComponent<Metrica>();
      // }

      if (!menu){
        leaderNum = 20;
        RespawnPlayers();
        GameZone = GameObject.Find("GameZone");
        Canvas = GameObject.Find("Canvas");

        t_totalHerb = GameObject.Find("t_totalHerb").GetComponent<Text>();
        t_totalPredator = GameObject.Find("t_totalPredator").GetComponent<Text>();
        infoText = GameObject.Find("InfoText").GetComponent<InfoText>();



        // if (metrica){
        //    if (PlayerPrefs.GetInt("stageNum") == 0) metrica.level_start(1, "01_grass", 999);
        //    if (PlayerPrefs.GetInt("stageNum") > 0) metrica.level_start(1, "02_sand", 999);
        //  }
      }
    }

    public void FillLeader(string str){
      LeaderString[leaderNum] = str;
      leaderNum -= 1;
    }

    public void SetPlayerLeader(){
      playerNum = leaderNum;
    }

    public void ShowText(string s, int col){
      infoText.ShowText( s, col );
    }

    void ShowTotalPlayers(){
      int totalHerb = 0;
      int totalPred = 0;

      foreach (GameObject g in Enemys){
        if (g){
          if (g.GetComponent<Controll>().predator){
            totalPred += 1;
          }else{
            totalHerb += 1;
          }
        }
      }

      totalPlayers = totalHerb+totalPred;
      t_totalHerb.text = totalHerb + "";
      t_totalPredator.text = totalPred + "";
    }

    void RespawnPlayers(){
      var resps = GameObject.FindGameObjectsWithTag("Respawn");
      int playernum = 0;

      GameObject pl;
      int numresp = Random.Range(0, resps.Length);

      foreach (GameObject g in resps){ // do respawn characters
        if (playernum == numresp){
          Player.transform.position = g.transform.position;
          pl = Player;
        }else{
          pl = Instantiate(CharacterPrefab, g.transform.position, CharacterPrefab.transform.rotation);
          pl.GetComponent<Controll>().Bot = true;
        }

        Destroy(g);
        Enemys[playernum] = pl;
        playernum += 1;
      }

      foreach (GameObject g in Enemys){ // start all characters
        g.GetComponent<Controll>().StartPlayer();
      }

    }

    // Update is called once per frame
    void Update()
    {
      if (!menu){
          gameZone -= Time.deltaTime*0.65f;
          totalPlDelay -= Time.deltaTime;

          if (gameZone < 14){
             gameZone = 14;
             /*
             if (!hungryTime){

               hungryTime = true;
               ShowText("For all animals, it is time for hunger!!!", 3);
               GameObject.Find("s_start_hungry").GetComponent<AudioSource>().Play();
               foreach(GameObject en in Enemys){
                 if (en){
                   if (en.GetComponent<Controll>().Bot){
                     en.GetComponent<Controll>().SetHungry(-8);
                   }
                 }
               }
             }*/

           }
          GameZone.transform.localScale = new Vector3(gameZone,GameZone.transform.localScale.y,gameZone);

          if (totalPlDelay<0){
            totalPlDelay = 1;
            ShowTotalPlayers();

            if (totalPlayers < 2 && !gameFinished){
              playerNum = 1;
              LeaderString[leaderNum] = PlayerPrefs.GetString("Nickname");
              GameEnd(true);
            }
          }
        }
    }

    public void GameEnd(bool win){
      if (gameFinished == false){
        gameFinished = true;
        GameObject.Find("Joystick").active = false;
        GameObject.Find("UI").active = false;

        Instantiate(GameFinish, Canvas.transform);

        if (win){
          infoText.ShowText( "YOU ARE WIN", 2 );
        }else{
          infoText.ShowText( "YOU ARE DEAD", 1 );
        }
        GameObject.Find("s_end_game").GetComponent<AudioSource>().Play();
                  /// 100 ////
        gold = 1000 + ((21-playerNum)*5) * 20;
        int lvl = PlayerPrefs.GetInt("lvl_count");
        if (lvl == 0) gold = 500;
        if (lvl == 1) gold = 500;
        if (lvl == 3) gold = 1000;
        if (lvl == 4 && gold > 1500) gold = 1500;
        if (lvl == 5 && gold > 2500) gold = 2500;


        PlayerPrefs.SetInt("Gold", gold + PlayerPrefs.GetInt("Gold") );
        GameObject.Find("t_gold").GetComponent<Text>().text = gold + "";

        string s_win = "win";
        if (!win) s_win = "lose";

        // if (metrica){
        //    if (PlayerPrefs.GetInt("stageNum") == 0) metrica.level_finish(1, "01_grass", 999, s_win, (21-playerNum)*5, 0);
        //    if (PlayerPrefs.GetInt("stageNum") > 0) metrica.level_finish(1, "02_sand", 999, s_win, (21-playerNum)*5, 0);
        //  }
      }
    }

    public string GenerateNickname(){
      string nicks = "";

      if (!nicklistsetted){
        nicklistsetted = true;
        string names = "Lorder,Golum,Svego,Scratch,Minior,Scooter,TrailBoom,Bomber,Crot,Polos,CretoGard,Creamor,Scremer,Dread!!,Tormant,Grotar,trender,Porter,Potter,Mikel,Dragos,Crang,Creos,Lopas,Kayle,Toodas,Gababa,PilionLorak,Tommy,Dreamer,Josef,Joque,Kinderos,Solan,Sonar,Talos,Tanos,Ketozzzz,Dodo,Foley,Fooooty,Mikilos,Positron,Torman,Vivi,Jios,Pisko,Gurad,Jonny,Xeros,Zooomer,Zombie,velos,pooner,Spil,queqweqwe,asdas,qwee,BloodyKnight,xAngeLx,vlom,Maels,oskar61,wanderer_from,amaze,Z1KkY,Crysler,heletch,shipilov,Chacha,usist,zingo,excurs,capitan_beans,Cashish,LUNTIK,gour,The knyazzz,American_SnIper,NIGHTMARE,007up,Dr.Dizel,RaNDoM,sportik,Su1c1de,Roger,glx506,volandband,pas,Necron,edik_lukoyanov,Synchromesh,SolomA,Dron128,DeatHSoul,DangErXeTER,Psy,Forcas,Morgot,Aspect,Kraken,Bender,Lynch,Big Papa,Mad Dog,Bowser,O’Doyle,Bruise,Psycho,Cannon,Ranger,Clink,Ratchet,Cobra,Reaper,Colt,Rigs,Crank,Ripley,Creep,Roadkill,Daemon,Ronin,Decay,Rubble,Diablo,Sasquatch,Doom,Scar,Dracula,Shiver,Dragon,Skinner,Fender,Skull Crusher,Fester,Slasher,Fisheye,Steelshot,Flack,Surge,Gargoyle,Sythe,Grave,Trip,Gunner,Trooper,Hash,Tweek,Hashtag,Vein,Indominus,Void,Ironclad,Wardon,Killer,Wraith,Knuckles,Zero,Steel,Kevlar,Lightning,Tito,Bullet-Proof,Fire-Bred,Titanium,Hurricane,Ironsides,Iron-Cut,Tempest,Iron Heart,Steel Forge,Pursuit,Steel Foil,Upsurge,Uprising,Overthrow,Breaker,Sabotage,Dissent,Subversion,Rebellion,Insurgent,Loch,Golem,Wendigo,Rex,Hydra,Behemoth,Balrog,Manticore,Gorgon,Basilisk,Minotaur,Leviathan,Cerberus,Mothman,Sylla,Charybdis,Orthros,Baal,Cyclops,Satyr,Azrael,Mariy_Kis,KATUSHA,KinDer,Eva,BoSoranY,AlfabetkA,ANGEL,Äђģę,Abel,Abe,Abie,Abner,Ab,Abbie,Abraham,Abram,Abe,Abie,Bram,Adam,Ad,Addie,Addy,Ade,Adelbert,Adalbert,Ad,Ade,Al,Bert,Bertie,Del,Adrian,Ade,Alan,Allan,Allen,Al,Albert,Al,Bert,Bertie,Alexander,Al,Alex,Alec,Aleck,Lex,Sandy,Sander,Alfred,Al,Alf,Alfie,Fred,Freddie,Freddy,Algernon,Algie,Algy,Alger,Alister,Allister,Alistair,Alastair,Alaster,Al,Alonso,Alonzo,Al,Lon,Lonnie,Lonny,Alphonso,Alfonso,Al,Alf,Alfie,Alonso,Lon,Alva,Alvah,Alvan,Al,Alvin,Alwin,Alwyn,Al,Vin,Vinny,Win,Ambrose,Ambie,Brose,Amos,Andrew,Andy,Drew,Angus,Gus,Anselm,Ansel,Anse,Anthony,Antony,Anton,Tony,Archibald,Arch,Archie,Baldie,Arnold,Arnie,Arthur,Art,Artie,Augustus,August,Augie,Gus,Gussy,Gust,Gustus,Augustine,Augustin,Augie,Austin,Gus,Gussy,Gust,Austin,Avery,Avy,Baldwin,Baldie,Win,Barrett,Barry,Barrie,Bartholomew,Bart,Barty,Bartlett,Bartley,Bat,Batty,Basil,Baz,Basie,Benedict,Ben,Bennie,Benny,Benjamin,Ben,Bennie,Benny,Benjy,Benjie,Bennet,Bennett,Ben,Bennie,Benny,Bernard,Barnard,Bernie,Berney,Barney,Barnie,Bert,Bertie,Berthold,Bert,Bertie,Bertram,Bertrand,Bert,Bertie,Bill,Billy,Billie,Blair,Blake,Boris,Bradford,Brad,Ford,Bradley,Brad,Brady,Brandon,Branden,Brand,Brandy,Brenton,Brent,Bret,Brett,Brian,Bryan,Bryant,Broderick,Brodie,Brody,Brady,Rick,Ricky,Bruce,Bruno,Burton,Burt,Byron,Ron,Ronnie,Ronny,Caleb,Cal,Calvin,Cal,Vin,Vinny,Cameron,Cam,Ron,Ronny,Carey,Cary,Carry,Carl,Karl,Carol,Carrol,Carroll,Casey,Kasey,Caspar,Casper,Cas,Cass,Cassius,Cas,Cass,Cecil,Cis,Cedric,Ced,Rick,Ricky,Charles,Charlie,Charley,Chuck,Chad,Chas,Chester,Chet,Christian,Chris,Christy,Kit,Christopher,Kristopher,Chris,Kris,Cris,Christy,Kit,Kester,Kristof,Toph,Topher,Clarence,Clare,Clair,Clare,Clair,Clark,Clarke,Claude,Claud,Clayton,Clay,Clement,Clem,Clifford,Cliff,Ford,Clinton,Clint,Clive,Clyde,Cody,Colin,Collin,Cole,Conrad,Con,Connie,Conny,Corey,Cory,Cornelius,Connie,Conny,Corny,Corney,Cory,Craig,Curtis,Curt,Cyril,Cy,Cyrus,Cy,Dale,Daniel,Dan,Danny,Darrell,Darrel,Darryl,Daryl,Darry,David,Dave,Davey,Davie,Davy,Dean,Deane,Delbert,Del,Bert,Bertie,Dennis,Denis,Den,Denny,Derek,Derrick,Derry,Rick,Ricky,Desmond,Des,Dexter,Dex,Dominic,Dominick,Domenic,Domenick,Dom,Nick,Nicky,Don,Donnie,Donny,Donald,Don,Donnie,Donny,Donovan,Don,Donnie,Donny,Dorian,Douglas,Douglass,Doug,Doyle,Drew,Duane,Dwayne,Dudley,Dud,Duddy,Duke,Duncan,Dunny,Dunk,Dustin,Dusty,Dwight,Dylan,Dillon,Earl,Earle,Edgar,Ed,Eddie,Eddy,Ned,Edmund,Edmond,Ed,Eddie,Eddy,Ned,Ted,Edward,Ed,Eddie,Eddy,Ned,Ted,Teddy,Edwin,Ed,Eddie,Eddy,Ned,Egbert,Bert,Bertie,Elbert,El,Bert,Bertie,Eldred,El,Elijah,Elias,Eli,Lige,Elliot,Elliott,El,Ellis,El,Elmer,El,Elton,Alton,El,Al,Elvin,Elwin,Elwyn,El,Vin,Vinny,Win,Elvis,El,Elwood,El,Woody,Emery,Emmery,Emory,Em,Emil,Emile,Em,Emmanuel,Emanuel,Immanuel,Manuel,Manny,Mannie,Emmet,Emmett,Em,Eric,Erik,Erick,Rick,Ricky,Ernest,Earnest,Ernie,Errol,Ervin,Erwin,Irvin,Irvine,Irving,Irwin,Erv,Vin,Win,Ethan,Eugene,Gene,Eustace,Stacy,Stacey,Evan,Ev,Everard,Ev,Everett,Ev,Fabian,Fabe,Fab,Felix,Lix,Ferdinand,Ferdie,Fred,Freddie,Fergus,Ferguson,Fergie,Floyd,Floy,Ford,Francis,Frank,Frankie,Franky,Fran,Franklin,Franklyn,Frank,Frankie,Franky,Frederick,Frederic,Fredrick,Fredric,Fred,Freddie,Freddy,Rick,Ricky,Fred,Freddie,Gabriel,Gabe,Gabby,Garrett,Garret,Gary,GarryGeoffrey,Jeffrey,Jeffery,Jeff,George,Georgie,Geordie,Gerald,Gerard,Gerry,Jerry,Gilbert,Gil,Bert,Glenn,Glen,Gordon,Gordy,Don,Graham,Grant,Gregory,Gregor,Greg,Gregg,Griffith,Griffin,Griff,Guy,Harold,Hal,Harry,Harris,Harrison,Harry,Harvey,Harve,Hector,Henry,Harry,Hank,Hal,Herbert,Herb,Bert,Bertie,Herman,Manny,Mannie,Hilary,Hillary,Hill,Hillie,Hilly,Homer,Horace,Horatio,Howard,Howie,Hubert,Hugh,Bert,Bertie,Hube,Hugh,Hughie,Hugo,Humphrey,Humphry,Humph,Ian,Ignatius,Iggy,Nate,Immanuel,Manny,Mannie,Irvin,Irvine,Irving,Irwin,Isaac,Isaak,Ike,Isidore,Isidor,Isadore,Isador,Izzy,Ivor,Jack,Jackie,Jacky,Jacob,Jake,Jay,James,Jim,Jimmy,Jimmie,Jamie,Jem,Jared,Jerry,Jarvis,Jervis,Jerry,Jason,Jay,Jasper,Jay,Jefferson,Jeff,Jeffrey,Jeffery,Geoffrey,Jeff,Jeremy,Jeremiah,Jerry,Jerome,Jerry,Jesse,Jess,Jessie,Jessy,Joel,Joe,John,Jack,Jackie,Jacky,Johnny,Jonathan,Jon,Jonny,Joseph,Joe,Joey,Jo,Jos,Jody,Joshua,Josh,Judson,Jud,Sonny,Julian,Julius,Jule,Jules,Justin,Jus,Just,Karl,Carl,Keith,Kelly,Kelley,Kelvin,Kel,Kelly,Kendall,Ken,Kenny,Kendrick,Ken,Kenny,Rick,Ricky,Kenneth,Ken,Kenny,Kent,Ken,Kenny,Kevin,Kev,Kirk,Kristopher,Kristofer,Kris,Kit,Kester,Kurt,Curt,Kyle,Lambert,Bert,Lamont,Monty,Monte,Lancelot,Launcelot,Lance,Laurence,Lawrence,Lorence,Lorenzo,Larry,Lars,Laurie,Lawrie,Loren,Lauren,Lee,Leigh,Leo,Leon,Lee,Leonard,Leo,Leon,Len,Lenny,Lennie,Leopold,Leo,Poldie,Leroy,Leeroy,Lee,Roy,Leslie,Lesley,Les,Lester,Les,Lewis,Lew,Lewie,Lincoln,Lin,Linc,Lynn,Lindon,Lyndon,Lin,Lynn,Lindsay,Lindsey,Lin,Lynn,Linus,Lionel,Leo,Leon,Llewellyn,Llew,Lyn,Lloyd,Loyd,Loyde,Floyd,Loy,Floy,Logan,Lonnie,Lonny,Louis,Lou,Louie,Lowell,Lovell,Lucian,Lucius,Lu,Luke,Luke,Lucas,Luke,Luther,Loot,Luth,Lyle,Lyall,Lynn,Malcolm,Mal,Malc,Mac,Manuel,Manny,Mannie,Marion,Mark,Marc,Marcus,Mark,Marc,Marshall,Marshal,Martin,Mart,Marty,Marvin,Mervin,Marv,Merv,Matthew,Matt,Mat,Matty,Mattie,Matthias,Matt,Mat,Matty,Mattie,Maurice,Morris,Morry,Morey,Moe,Maximilian,Max,Maxwell,Max,Maynard,Melvin,Mel,Merlin,Merle,Merrill,Merril,Merill,Michael,Mike,Mikey,Mick,Mickey,Micky,Miles,Myles,Milo,Milo,Milton,Milt,Mitchell,Mitch,Monroe,Munroe,Montague,Monty,Monte,Montgomery,Monty,Monte,Morgan,Mo,Mortimer,Mort,Morty,Morton,Mort,Morty,Moses,Mo,Moe,Mose,Moss,Murray,Murry,Nathan,Nathaniel,Nat,Nate,Natty,Neal,Neil,Nelson,Nel,Nell,Nels,Nevill,Nevil,Nevile,Neville,Nev,Newton,Newt,Nicholas,Nicolas,Nick,Nicky,Nicol,Cole,Colin,Nigel,Nige,Noah,Noel,Nowell,Norbert,Bert,Norris,Nor,Norrie,Norman,Norm,Normie,Nor,Norrie,Norton,Nort,Oliver,Ollie,Noll,Nollie,Nolly,Orson,Orville,Orv,Ollie,Osbert,Ossy,Ozzie,Ozzy,Bert,Osborn,Osborne,Ossy,Ozzie,Ozzy,Oscar,Os,Ossy,Osmond,Osmund,Ossy,Ozzie,Ozzy,Oswald,Oswold,Os,Ossy,Oz,Ozzie,Ozzy,Otis,Owen,Patrick,Pat,Paddy,Patsy,Paul,Pauly,Percival,Perceval,Percy,Perce,Perry,Peter,Pete,Petie,Petey,Philip,Phillip,Phil,Pip,Preston,Quentin,Quintin,Quenton,Quinton,Quinn,Quincy,Quincey,Quinn,Ralph,Raff,Rafe,Ralphy,Randall,Randal,Rand,Randy,Randolph,Rand,Randy,Dolph,Raphael,Rafael,Raff,Rafe,Raymond,Raymund,Ray,Reginald,Reg,Reggie,Renny,Rex,Rene,Reuben,Ruben,Rubin,Rube,Ruby,Reynold,Ray,Richard,Dick,Rick,Ricky,Rich,Richie,Rick,Ricky,Robert,Bob,Bobbie,Bobby,Dob,Rob,Robbie,Robby,Robin,Bert,Roderic,Roderick,Rod,Roddy,Rick,Ricky,Rodney,Rod,Roddy,Roger,Rodger,Rod,Roddy,Rodge,Roge,Roland,Rowland,Rolly,Roly,Rowly,Orlando,Rolph,Rolf,Rolfe,Roman,Rom,Romy,Ronald,Ron,Ronnie,Ronny,Ron,Ronnie,Ronny,Roscoe,Ross,Ross,Roy,Rudolph,Rudolf,Rudy,Rolf,Rolph,Dolph,Dolf,Rufus,Rufe,Rupert,Russell,Russel,Russ,Ryan,Samson,Sampson,Sam,Sammy,Samuel,Sam,Sammy,Sanford,Sandy,Ford,Saul,Scott,Scotty,Sean,Shaun,Shawn,Shane,Sebastian,Seb,Bass,Serge,Seth,Seymour,Morey,Sy,Shannon,Shanon,Sheldon,Shelly,Shel,Don,Shelley,Shelly,Shellie,Shel,Sherman,Shelton,Shelly,Shel,Tony,Sidney,Sydney,Sid,Syd,Silas,Si,Sy,Silvester,Sylvester,Syl,Vester,Simeon,Sim,Simie,Simmy,Simon,Si,Sy,Sim,Simie,Simmy,Solomon,Sol,Solly,Sal,Sonny,Son,Spencer,Stacy,Stacey,Stanley,Stan,Stephen,Steven,Stephan,Steffan,Stefan,Steve,Stevie,Steph,Steff,Stef,Stuart,Stewart,Stu,Stew,Terence,Terrence,Terrance,Terry,Thaddeus,Thadeus,Tad,Thad,Theodore,Theodor,Ted,Teddy,Theo,Terry,Thomas,Tom,Tommy,Timothy,Tim,Timmy,Tobias,Toby,Tobi,Tobie,Todd,Tony,Tracy,Tracey,Travis,Trav,Trenton,Trent,Trevor,Trev,Tristram,Tristam,Tristan,Tris,Troy,Tyler,Ty,Tyrone,Tyron,Ty,Ulysses,Uly,Uli,Lyss,Uriah,Urias,Uri,Uria,Valentine,Valentin,Val,Valerian,Valerius,Val,Van,Vance,Van,Vaughan,Vaughn,Vernon,Vern,Verne,Victor,Vic,Vick,Vincent,Vince,Vin,Vinny,Virgil,Vergil,Virge,Wallace,Wallis,Wally,Wallie,Waldo,Walter,Walt,Wally,Wallie,Warren,Wayne,Wesley,Wes,Wendell,Dell,Del,Wilbert,Will,Willie,Willy,Bert,Wilbur,Wilber,Will,Willie,Willy,Wiley,Will,Willie,Willy,Wilfred,Wilfrid,Will,Willie,Willy,Fred,Freddie,Freddy,Willard,Will,Willie,Willy,William,Bill,Billy,Billie,Will,Willie,Willy,Liam,Willis,Bill,Billy,Billie,Will,Willie,Willy,Wilson,Will,Willie,Willy,Winfred,Winfrid,Win,Winnie,Winny,Fred,Freddie,Freddy,Winston,Win,Winnie,Winny,Woodrow,Wood,Woody,Xavier,Zave,Zachary,Zachariah,Zacharias,Zack,Zacky,Zach";

        selectedName = names.Split(char.Parse(","));
      }
      int rnd = Random.Range(0, 5);
      if (rnd == 0)
      {
        rnd = Random.Range(1131, 98340);
        nicks = "Player"+rnd;
      }
      else
      {
        nicks = selectedName[Random.Range(0, selectedName.Length)];
      }

      return nicks;
    }


}
