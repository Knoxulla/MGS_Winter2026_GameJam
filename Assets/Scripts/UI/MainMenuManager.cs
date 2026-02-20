using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Button playBTN;
    [SerializeField] Button optionsBTN;
    [SerializeField] Button creditsBTN;
    [SerializeField] Button exitBTN;

    private void Start()
    {
        playBTN.onClick.AddListener(PlayGame);
        optionsBTN.onClick.AddListener(OpenOptions);
        creditsBTN.onClick.AddListener(OpenCredits);
        exitBTN.onClick.AddListener(ExitGame);
    }
    private void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void OpenCredits()
    {
        SceneManager.LoadScene("CreditScene");

    }

    private void OpenOptions()
    {
        SceneManager.LoadScene("Options", LoadSceneMode.Additive);
       
    }

    private void ExitGame()
    {
       Application.Quit();
    }
}
