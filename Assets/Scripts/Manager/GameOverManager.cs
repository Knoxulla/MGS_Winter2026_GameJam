using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] Button goToCrd;
    [SerializeField] Button replay;

    private void Start()
    {
        Time.timeScale = 0f;

        replay.onClick.AddListener(Replay);
        goToCrd.onClick.AddListener(GoToCredits);
    }

    private void GoToCredits()
    {
        SceneManager.LoadScene("CreditScene");
    }

    private void Replay()
    {
        SceneManager.LoadScene("GameScene");
    }
}
