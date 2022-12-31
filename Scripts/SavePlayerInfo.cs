using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlayerInfo : MonoBehaviour
{
    public static int player1Score = 0;
    public static int player2Score = 0;
    public static bool justChangedTurn = false;
    public static bool isPlayer1Turn = true;
    public static bool isPlayer2Turn = false;
    public static float player1Fuel = 200f;
    public static float player2Fuel = 200f;
    public static bool player1HaveFuel = true;
    public static bool player2HaveFuel = true;
    public static bool player1FirstCarsh = false;
    public static bool player2FirstCarsh = true;

    public static bool CrashingSoundIsPlayed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
