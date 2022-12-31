using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TMPro.TextMeshProUGUI player1ScoreText;
    public TMPro.TextMeshProUGUI player2ScoreText;
    public Slider player1Slider;
    public Slider player2Slider;
    public TMPro.TextMeshProUGUI winnerText;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        showPLayersScore();

        player1Slider.value = SavePlayerInfo.player1Fuel / 100f;
        player2Slider.value = SavePlayerInfo.player2Fuel / 100f;
    }

    void showPLayersScore(){
        player1ScoreText.gameObject.SetActive(true);
        player1ScoreText.text = "Blue Point: " + SavePlayerInfo.player1Score;

        player2ScoreText.gameObject.SetActive(true);
        player2ScoreText.text = "Red Point: " + SavePlayerInfo.player2Score;
    }

    public void showWinner(){
        winnerText.gameObject.SetActive(true);
        int player1Score = SavePlayerInfo.player1Score;
        int player2Score = SavePlayerInfo.player2Score;
        if(player1Score > player2Score){
            winnerText.text = "Blue";
        }else
        {
            winnerText.text = "Red";
        }
    }
}
