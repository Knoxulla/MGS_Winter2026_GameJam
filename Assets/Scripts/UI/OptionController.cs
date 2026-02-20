using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    [SerializeField] AudioMixer myMixer;

    [Header("Sliders")]
    [SerializeField] Slider masterVolSlider;
    [SerializeField] Slider SFXVolSlider;
    [SerializeField] Slider musicVolSlider;

    [Header("In Game Button Set")]
    [SerializeField] GameObject inGameBTN;
    [SerializeField, Tooltip("Resume BTN")] Button closeOptionsBTN_Game;// resume
   // [SerializeField] Button mainMenuBTN_Game;
    [Header("Main Menu Button Set")]
    [SerializeField] GameObject mainMenuBTN;
    [SerializeField] Button closeOptionsBTN_MainMenu;

    private void Start()
    {
        if (PlayerPrefs.HasKey("masterVol"))
        {
            LoadMasterVolume();
        }
        else
        {
            SetMasterVol();
        }

        if (PlayerPrefs.HasKey("musicVol"))
        {
            LoadMusicVolume();
        }
        else
        {
            SetMusicVol();
        }

        if (PlayerPrefs.HasKey("sfxVol"))
        {
            LoadSFXVolume();
        }
        else
        {
            SetMusicVol();
        }

        inGameBTN.SetActive(true);
        //mainMenuBTN.SetActive(false);

       // closeOptionsBTN_MainMenu.onClick.AddListener(CloseOptions);
        closeOptionsBTN_Game.onClick.AddListener(CloseOptions);
       // mainMenuBTN_Game.onClick.AddListener(ToMainMenu);
       // ChangeBackBTNVis();
        PauseGame();
    }

    private void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //private void ChangeBackBTNVis()
    //{
    //    if (SceneManager.GetActiveScene().name.Equals("GameScene"))
    //    {
    //        mainMenuBTN.SetActive(false);
    //        inGameBTN.SetActive(true);
    //    }

    //    else if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
    //    {
    //        inGameBTN.SetActive(false);
    //        mainMenuBTN.SetActive(true);
    //    }
    //}

    public void SetMasterVol()
    {
        float volume = masterVolSlider.value;
        myMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVol", volume);
    }

    private void LoadMasterVolume()
    {
        masterVolSlider.value = PlayerPrefs.GetFloat("masterVol");

        SetMasterVol();
    }

    public void SetMusicVol()
    {
        float volume = musicVolSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVol", volume);
    }

    private void LoadMusicVolume()
    {
        musicVolSlider.value = PlayerPrefs.GetFloat("musicVol");

        SetMusicVol();
    }

    public void SetSFXVol()
    {
        float volume = SFXVolSlider.value;
        myMixer.SetFloat("Sound Effects", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVol", volume);
    }

    private void LoadSFXVolume()
    {
        SFXVolSlider.value = PlayerPrefs.GetFloat("sfxVol");

        SetSFXVol();
    }

    private void CloseOptions()
    {
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("Options");
       
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }
}
