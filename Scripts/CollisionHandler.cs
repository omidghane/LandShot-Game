using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] AudioClip crashDown;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] float levelLoadDelay = 1f;
     [SerializeField] ParticleSystem crashParticle;
    [SerializeField] ParticleSystem successParticle;

    Move myMove;
    AudioSource myAudio;
    public GameObject player1;
    public GameObject player2;
    GameObject myRocket;

    int timesOFLoadScene = 0;
    int currentScene;
    bool isCollisionDisabled = false;
    bool isCrashed = false;
    bool istransitioning = false;
    bool haveFuel = true;

    private void Start() {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        myMove = GetComponent<Move>();
        myAudio = GetComponent<AudioSource>();

        // Debug.Log(SavePlayerInfo.isPlayer1Turn + " player1");
        // Debug.Log(SavePlayerInfo.isPlayer2Turn + " player2");
        applyTurn();
    }

    void applyTurn(){
        if(SavePlayerInfo.isPlayer1Turn){
            player1.transform.SetPositionAndRotation(new Vector3(-12.27f,1.52999997f,0),new Quaternion(0,0,0,0));
            player2.transform.SetPositionAndRotation(new Vector3(12.27f,-49f,0),new Quaternion(0,0,0,0));
            player1.GetComponent<Move>().enabled = true;
            player2.GetComponent<Move>().enabled = false;
            SavePlayerInfo.player2FirstCarsh = true;
        }
        else if(SavePlayerInfo.isPlayer2Turn){
            player2.transform.SetPositionAndRotation(new Vector3(12.27f,1.52999997f,0),new Quaternion(0,0,0,0));
            player1.transform.SetPositionAndRotation(new Vector3(-12.27f,-49f ,0),new Quaternion(0,0,0,0));
            player1.GetComponent<Move>().enabled = false;
            player2.GetComponent<Move>().enabled = true;
            SavePlayerInfo.player1FirstCarsh = true;
        }

        SavePlayerInfo.player1Fuel = 100f;
        SavePlayerInfo.player2Fuel = 100f;
    }

    private void Update()
    {
        if (gameObject.tag.Equals("Player1"))
        {
            haveFuel = SavePlayerInfo.player1HaveFuel;
        }
        else if (gameObject.tag.Equals("Player2"))
        {
            haveFuel = SavePlayerInfo.player2HaveFuel;
        }

        //  if(gameObject.tag.Equals("Player1")){
        //     Debug.Log(SavePlayerInfo.CrashingSoundIsPlayed + " " + Move.currentFuel + " Player1");
        //  }else if(gameObject.tag.Equals("Player2")){
        //     Debug.Log(SavePlayerInfo.CrashingSoundIsPlayed + " " + Move.currentFuel + " Player2");
        //  }

        if (Move.currentFuel < 0 && !SavePlayerInfo.CrashingSoundIsPlayed)
        {
            if (gameObject.tag.Equals("Player1"))
            {
                Debug.Log(SavePlayerInfo.CrashingSoundIsPlayed + " " + Move.currentFuel + " Player1");
            }
            else if (gameObject.tag.Equals("Player2"))
            {
                Debug.Log(SavePlayerInfo.CrashingSoundIsPlayed + " " + Move.currentFuel + " Player2");
            }
            Debug.Log("crashSound played");
            SavePlayerInfo.CrashingSoundIsPlayed = true;
            myAudio.Stop();
            myAudio.PlayOneShot(crashDown);
            //  myMove.enabled = false;
            // applyChangeTurn();
        }

        cheatDebug();
    }

    private void OnCollisionEnter(Collision other) {
        string RocketsTag = gameObject.tag;
        string landTag = other.gameObject.tag;
        if(!istransitioning && !isCollisionDisabled)
        {
            if(landTag.Equals(RocketsTag)){
                if(Move.currentFuel < 0 && !SavePlayerInfo.justChangedTurn){
                    SavePlayerInfo.justChangedTurn = false;
                    istransitioning = true;
                    crashIntoSomething();
                }
                Debug.Log("MyLand");
            }
            else if(landTag.Equals("Player1") || landTag.Equals("Player2")){
                istransitioning = true; 
                landing();
            }
            else if(!landTag.Equals("Neutral")){
                
                istransitioning = true;
                crashIntoSomething();
                // changeTurn = true;
            }
        }
        
    }

    bool playerTurn(){
        if(SavePlayerInfo.isPlayer1Turn && gameObject.tag.Equals("Player1")){
            return true;
        }
        else if(SavePlayerInfo.isPlayer2Turn && gameObject.tag.Equals("Player2")){
            return true;
        }
        else{
            return false;
        }
    }

    void crashIntoSomething(){
        myAudio.Stop();
        myAudio.PlayOneShot(crashSound);
        myMove.enabled = false;
        crashParticle.Play();

        // applyChangeTurn();

    //    SceneManager.MergeScenes(SceneManager.GetSceneByBuildIndex(2),SceneManager.GetSceneByBuildIndex(3));
        // Invoke("reloadScene",levelLoadDelay);
        Invoke("applyChangeTurn",levelLoadDelay);
        
    }

    void applyChangeTurn(){
        if(gameObject.tag.Equals("Player1")){
            gameObject.transform.SetPositionAndRotation(new Vector3(-12.27f,-49f,0),new Quaternion(0,0,0,0));
            player2.transform.SetPositionAndRotation(new Vector3(12.27f,1.52999997f,0),new Quaternion(0,0,0,0));
            player2.GetComponent<Move>().enabled = true;

            Debug.Log("player1 changed turn");  
            if(!SavePlayerInfo.player2FirstCarsh){
                SavePlayerInfo.player2Fuel += 50;
                if(SavePlayerInfo.player2Fuel > 100){
                    SavePlayerInfo.player2Fuel = 100;
                }
            }
            SavePlayerInfo.player2FirstCarsh = false;
            
            determineTurn(false,true);

            SavePlayerInfo.player2HaveFuel = true;
            

            // GameObject newPlayer = Instantiate(player2,new Vector3(10.5299997f,1.52999997f,0),new Quaternion(0,0,0,0));
            // gameObject.SetActive(false);
        }
        else if(gameObject.tag.Equals("Player2")){
            gameObject.transform.SetPositionAndRotation(new Vector3(12.27f,-49f,0),new Quaternion(0,0,0,0));
            player1.transform.SetPositionAndRotation(new Vector3(-12.27f,1.52999997f,0),new Quaternion(0,0,0,0));
            player1.GetComponent<Move>().enabled = true;
            
            if(!SavePlayerInfo.player1FirstCarsh){
                SavePlayerInfo.player1Fuel += 50;
                if(SavePlayerInfo.player1Fuel > 100){
                    SavePlayerInfo.player1Fuel = 100;
                }
            }
            SavePlayerInfo.player1FirstCarsh = false;

            determineTurn(true,false);

            SavePlayerInfo.player1HaveFuel = true;
            

            // Debug.Log(SavePlayerInfo.player1Fuel + " player1Fuels");
            // GameObject newPlayer = Instantiate(player1,new Vector3(10.5299997f,1.52999997f,0),new Quaternion(0,0,0,0));
            // gameObject.SetActive(false);
        }
       Invoke("crashSoundFalse",1f);
        istransitioning = false;
    }

    void determineTurn(bool player1Turn,bool player2Turn){
        SavePlayerInfo.isPlayer1Turn = player1Turn;
        SavePlayerInfo.isPlayer2Turn = player2Turn;
    }

    void crashSoundFalse(){
        SavePlayerInfo.CrashingSoundIsPlayed = false;
    }

    void landing()
    {
        myAudio.Stop();
        myAudio.PlayOneShot(clip: successSound);
        myMove.enabled = false;
        successParticle.Play();

        giveScore();

        timesOFLoadScene++;
        if(timesOFLoadScene == 2 ){
            Invoke("loadNextScene",time: levelLoadDelay);

            if(gameObject.tag.Equals("Player1")){
                determineTurn(false,true);
            }
            else if(gameObject.tag.Equals("Player2")){
               determineTurn(true,false);
            }

            timesOFLoadScene = 0;
        }
        else{
            Invoke("applyChangeTurn",levelLoadDelay);

            // giveGift();
        }
    }

    void giveGift(){

    }

    void giveScore(){
        if(gameObject.tag.Equals("Player1")){
            // Debug.Log("player2 choosed");
            SavePlayerInfo.player1Score++;
           
            // Debug.Log(SavePlayersScore.player1Score);
        }
        else if(gameObject.tag.Equals("Player2")){
            SavePlayerInfo.player2Score++;
            
            // Debug.Log(SavePlayersScore.player2Score);
        }
        SavePlayerInfo.justChangedTurn = true;
    }

    void loadNextScene()
    {
        int nextSceneIndex = currentScene + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("finished");
            UIManager.instance.showWinner();
            nextSceneIndex = 0;
            Invoke("loadNextScene",time: 10F);
        }
        else{
        SceneManager.LoadScene(nextSceneIndex);
        }
    }

    void reloadScene()
    {
        SceneManager.LoadScene(currentScene);
    }

        private void cheatDebug()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            loadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            isCollisionDisabled = !isCollisionDisabled;
        }
    }

}
