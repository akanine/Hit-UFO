using UnityEngine;
using System.Collections;
using Com.HitGame;

public class UserInterface : MonoBehaviour
{
    void Start()
    {

    }

    float beginButtonX = Screen.width / 2 - Screen.width / 8;
    float beginButtonY = Screen.height / 2 - Screen.width / 16;
    float beginButtonW = Screen.width / 4;
    float beginButtonH = Screen.height / 8;
    int hit = 0;

    void OnGUI()
    {
        GUIStyle Mystyle = new GUIStyle();
        Mystyle.fontSize = 20;
        Mystyle.normal.background = null;
        GUI.skin.button.fontSize = 30;
        if (hit == 0)
        {
            if (GUI.Button(new Rect(beginButtonX, beginButtonY, beginButtonW, beginButtonH), "Start"))
            {
                myFactory.GetInstance().Round = 1;
                hit = 1;
            }
        }

        string round = "Round: " + myFactory.GetInstance().Round.ToString();
        GUI.Label(new Rect(10, 0, beginButtonW, beginButtonH), round, Mystyle);
 
        string score = "Score: " + myFactory.GetInstance().Score.ToString();
        GUI.Label(new Rect(10, beginButtonH, beginButtonW, beginButtonH), score, Mystyle);
  
        string state;
        if (myFactory.GetInstance().GameState == 1)
        {
            state = "State: Ready";
            GUI.Label(new Rect(10, beginButtonH * 2, beginButtonW, beginButtonH * 2), state, Mystyle);
        }
        else if (myFactory.GetInstance().GameState == 2)
        {
            state = "State: Playing";
            GUI.Label(new Rect(10, beginButtonH * 2, beginButtonW, beginButtonH * 2), state, Mystyle);
        }
        else if (myFactory.GetInstance().GameState == 3)
        {
            state = "State: GameOver !";
            GUI.Label(new Rect(10, beginButtonH * 2, beginButtonW, beginButtonH * 2), state, Mystyle);
        }
        string Lose = "Lose: " + myFactory.GetInstance().LoseNum.ToString();
        GUI.Label(new Rect(10, beginButtonH * 3, beginButtonW, beginButtonH * 3), Lose, Mystyle);

    }

}
