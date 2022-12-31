using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTurnIdentifier : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    
    void Start()
    {
        if(!SceneManager.GetActiveScene().name.Equals("Over"))
        {
            Debug.Log("getInstance");
            if(SavePlayerInfo.isPlayer1Turn){
                GameObject newPlayer = Instantiate(player1,new Vector3(-12.27f,1.52999997f,0),new Quaternion(0,0,0,0));
            }
            else if(SavePlayerInfo.isPlayer2Turn){
                GameObject newPlayer = Instantiate(player2,new Vector3(12.27f,1.52999997f,0),new Quaternion(0,0,0,0));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Turn = CollisionHandler.changeTurn
        // if(){
        //     // Debug.Log(gameObject.active);
        //     gameObject.GetComponent<Move>().enabled = false;
        // }
    }
}
