
using TMPro;
using UnityEngine;
using System.Collections;

public class PlayerScore : MonoBehaviour
{
    public int currentScore;
    public int highscore;


    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI HighScoreText;
    [SerializeField] private TextMeshProUGUI scoreAddedText;
    private Coroutine scoreCoroutine;
    private Coroutine animateScoreCoroutine;
    void Start()
    {
        highscore = PlayerPrefs.GetInt("HighScore",0);
        scoreText.text = "0000000";
        HighScoreText.text = highscore.ToString("D7");
    }
    public void addscore(int score){
        int previousScore = currentScore;
        currentScore += score;
        if(currentScore > highscore){

            highscore = currentScore;
            PlayerPrefs.SetInt("HighScore",highscore);
            PlayerPrefs.Save();
        }
        updateUI(score,previousScore);
    }

    void updateUI(int scoreadded,int previousScore){
        

        if (animateScoreCoroutine != null)
        {
            StopCoroutine(animateScoreCoroutine);
        }

        animateScoreCoroutine = StartCoroutine(AnimateScore(previousScore, currentScore));
        
        HighScoreText.text = highscore.ToString("D7");

        if (scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine);
        }
        scoreCoroutine = StartCoroutine(ShowScoreAdded(scoreadded));
    }

    IEnumerator ShowScoreAdded(int scoreAdded)
    {
        scoreAddedText.text = $"+{scoreAdded}"; 
        scoreAddedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        scoreAddedText.gameObject.SetActive(false);
    }

    IEnumerator AnimateScore(int start, int end)
    {
        float duration = 0.5f; // 0.5 seconds for smooth transition
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            int displayedScore = Mathf.RoundToInt(Mathf.Lerp(start, end, progress));
            scoreText.text = displayedScore.ToString("D7");
            yield return null;
        }

        // Ensure the final value is set correctly
        scoreText.text = end.ToString("D7");
    }
}
