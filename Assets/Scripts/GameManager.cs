using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    /*
     * To avoid having multiple singleton scripts as much as possible,
     * Managers in the GameManager would not be singletons, and would
     * instead be children of the GameManager.
     * 
     * Referencing said managers would reference the GameManager first.
     * 
     * Only put managers here that persist through scenes.
     * 
     */


    public enum GameState
    {
        TitleScreen,
        InGame,
        Paused
    }


    [field: SerializeField]
    public GameState Current_GameState { get; private set; }

    public AudioManager _AudioManager;


    public EnemyCatalogue EnemyCatalogue;
    public ItemCatalogue ItemCatalogue;

    [field: Space(10)]

    [field: SerializeField]
    public string GameScene { get; private set; }

    [field: SerializeField]
    public string MenuScene { get; private set; }

    [field: SerializeField]
    public string CreditScene { get; private set; }


    
    public override void Awake()
    {
        base.Awake();

        GetManagersFromChildren();

        DontDestroyOnLoad(this);

        checkGameState();


    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneUpdate;
    }

    public void SyncSceneNames(string incomingGame, string incomingMenu, string incomingCredit)
    {
        GameScene = incomingGame;
        MenuScene = incomingMenu;
        CreditScene = incomingCredit;
    }

    void SceneUpdate(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "GameScene":
                UpdateGameState(GameState.InGame);
                break;

            //case "MenuScene":
            //    UpdateGameState(GameState.Paused); 
            //    break;

            case "TitleScene":
                UpdateGameState(GameState.TitleScreen);
                break;

            //default:
            //    UpdateGameState(GameState.InGame);
            //    break;
        }
    }

    void GetManagersFromChildren()
    {
        //if (TickManager == null)
        //    TickManager = GetComponentInChildren<TickManager>();

        if (_AudioManager == null)
            _AudioManager = GetComponentInChildren<AudioManager>();


       
    }




    public void UpdateGameState(GameState newState)
    {
        if (Current_GameState == newState) return;
        Current_GameState = newState;

        checkGameState();
    }

    void checkGameState()
    {
        switch (Current_GameState)
        {
            case GameState.TitleScreen:
                
                break;

            case GameState.InGame:
               
                break;

            case GameState.Paused:
               
                break;

        }
    }

    #region utility
    public virtual bool ContainsLayer(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
    #endregion
}
