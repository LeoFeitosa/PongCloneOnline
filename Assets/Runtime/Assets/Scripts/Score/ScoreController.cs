using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scorePlayer1;
    [SerializeField] TextMeshProUGUI scorePlayer2;

    public void SetScore(int player)
    {
        if (player == 1)
        {
            scorePlayer1.text = (ConvertStringToInt(scorePlayer1.text) + 1).ToString();
        }
        if (player == 2)
        {
            scorePlayer2.text = (ConvertStringToInt(scorePlayer2.text) + 1).ToString();
        }
    }

    int ConvertStringToInt(string stringNumber)
    {
        int score;
        int.TryParse(stringNumber, out score);
        return score;
    }

}
