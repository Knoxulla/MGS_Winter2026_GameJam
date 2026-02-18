using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Button playBTN;

    private void Start()
    {
        playBTN.onClick.AddListener(PlayGame);
    }
    private void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
