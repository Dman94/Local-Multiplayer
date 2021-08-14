using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    int p1Score = 0;
    int p2Score = 0;

    [SerializeField] Text P1ScoreText;
    [SerializeField] Text P2ScoreText;

    // Start is called before the first frame update
   
     void OnEnable()
    {
        PlayerLogic.onPLayerDeath += onScoreUpdate;
    }
     void OnDisable()
    {
        PlayerLogic.onPLayerDeath -= onScoreUpdate;
    }

    void onScoreUpdate(int playerNum)
    {
        if(playerNum == 1)
        {
            ++p1Score;
            P1ScoreText.text = "" + p1Score;
        }
        else if (playerNum == 2)
        {
            ++p2Score;
            P2ScoreText.text = "" + p2Score;
        }
    }

    // Update is called once per frame
    
}
