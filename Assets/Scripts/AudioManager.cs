using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [field: SerializeField]
    public AudioCatalogue AudioCatalogueSO { get; private set; }

    [Space(20)]

    #region VolumeSliders

    [Header("VOLUMES")]
    [Range(0f, 1f), SerializeField]
    private float mASVol;
    [Range(0f, 1f), SerializeField]
    public float MASVol
    {
        get { return mASVol; }
        set
        {

            mASVol = Mathf.Clamp01(value);

            if (mASVol <= 0f) MASIsMuted = true;
            else MASIsMuted = false;
        }
    }
    public bool MASIsMuted { get; private set; }

    [Space(10)]

    [Header("VOLUMES")]
    [Range(0f, 1f), SerializeField]
    private float musVol;
    [Range(0f, 1f), SerializeField]
    public float MusVol
    {
        get { return musVol; }
        set
        {
            musVol = Mathf.Clamp01(value);
            if (musVol <= 0f) MusIsMuted = true;
            else MusIsMuted = false;
        }
    }
    public bool MusIsMuted { get; private set; }

    [Space(10)]

    [Header("VOLUMES")]
    [Range(0f, 1f), SerializeField]
    private float sfxVol;
    [Range(0f, 1f), SerializeField]
    public float SFXVol
    {
        get { return sfxVol; }
        set
        {

            sfxVol = Mathf.Clamp01(value);

            if (sfxVol <= 0f) SFXIsMuted = true;
            else SFXIsMuted = false;
        }
    }
    public bool SFXIsMuted { get; private set; }

    [Space(10)]

    [Header("VOLUMES")]
    [Range(0f, 1f), SerializeField]
    private float playerSFXVol;
    [Range(0f, 1f)]
    public float PlayerSFXVol
    {
        get { return playerSFXVol; }
        set
        {
            playerSFXVol = Mathf.Clamp01(value);

          

            if (playerSFXVol <= 0f) PlayerIsMuted = true;
            else PlayerIsMuted = false;
        }
    }
    public bool PlayerIsMuted { get; private set; }

    [Header("VOLUMES")]
    [Range(0f, 1f), SerializeField]
    private float enemySFXVol;
    [Range(0f, 1f)]
    public float EnemySFXVol
    {
        get { return enemySFXVol; }
        set
        {
            enemySFXVol = Mathf.Clamp01(value);

            if (enemySFXVol <= 0f) EnemyIsMuted = true;
            else EnemyIsMuted = false;
        }
    }
    public bool EnemyIsMuted { get; private set; }

    [Header("VOLUMES")]
    [Range(0f, 1f), SerializeField]
    private float environmentSFXVol;
    [Range(0f, 1f)]
    public float EnvironmentSFXVol
    {
        get { return environmentSFXVol; }
        set
        {
            environmentSFXVol = Mathf.Clamp01(value);

            if (environmentSFXVol <= 0f) EnvironmentIsMuted = true;
            else EnvironmentIsMuted = false;
        }
    }
    public bool EnvironmentIsMuted { get; private set; }

    [Header("VOLUMES")]
    [Range(0f, 1f), SerializeField]
    private float UISFXvol;
    [Range(0f, 1f)]
    public float UISFXVol
    {
        get { return UISFXvol; }
        set
        {
            UISFXvol = Mathf.Clamp01(value);


            if (UISFXvol <= 0f) UIIsMuted = true;
            else UIIsMuted = false;
        }
    }
    public bool UIIsMuted { get; private set; }

    [Space(10)]

    [Header("VOLUMES")]
    [Range(0f, 1f), SerializeField]
    private float ambvol;
    [Range(0f, 1f)]
    public float AmbVol
    {
        get { return ambvol; }
        set
        {
            ambvol = Mathf.Clamp01(value);



            if (ambvol <= 0f) AmbIsMuted = true;
            else AmbIsMuted = false;
        }
    }
    public bool AmbIsMuted { get; private set; }

    #endregion

    public bool MusicIsPlaying { get; private set; }

    public string MUSICPARAM_PLAYERHP
    {
        get
        {
            return "PlayerHP";
        }
        private set
        {
            MUSICPARAM_PLAYERHP = value;
        }
    }

    public string MUSICPARAM_INCOMBAT
    {
        get
        {
            return "InCombat";
        }
        private set
        {
            MUSICPARAM_INCOMBAT = value;
        }
    }

    public string MUSICPARAM_CURRENTFAITH
    {
        get
        {
            return "PlayerFaith";
        }
        private set
        {
            MUSICPARAM_CURRENTFAITH = value;
        }
    }

    public string MUSICPARAM_CURRENCYCOMBO
    {
        get
        {
            return "CurrencyCombo";
        }
        private set
        {
            MUSICPARAM_CURRENCYCOMBO = value;
        }
    }

    #region Setup

    private new void Awake()
    {
        base.Awake();
        
    }

    private void Start()
    {
        setInitMusicValues();
    }

    void setInitMusicValues()
    {

        // First check if save file exists.
        // For now, defaults all values to 1
        SetVolume(0, 1);
        SetVolume(1, 1);
        SetVolume(2, 1);
        SetVolume(3, 1);
        SetVolume(4, 1);
        SetVolume(5, 1);
        SetVolume(6, 1);


        //if (MUS_Variables.musVariables[0] != null)
        //    MUS_Variables.setIsPlaying_Val(false);
        //if (MUS_Variables.musVariables[1] != null)
        //    MUS_Variables.setIsAction_Val(false);
        //MUS_Variables.setCurrentSanity_Val(1); BeatTracker sets the current sanity
    }

    private void Update()
    {

    }

    #endregion


    // You honestly don't need this, as all volume change logic is done through get/set in the actual values themselves
    public void SetVolume(int MixerID, float incomingVol)
    {
        switch (MixerID)
        {
            case 0:
                MASVol = incomingVol;
                break;


            case 1:
                MusVol = incomingVol;
                break;

            case 2:
                SFXVol = incomingVol;
                break;


            case 3:
                PlayerSFXVol = incomingVol;
                break;

            case 4:
                EnemySFXVol = incomingVol;
                break;

            case 5:
                EnvironmentSFXVol = incomingVol;
                break;

            case 6:
                UISFXVol = incomingVol;
                break;


            case 7:
                AmbVol = incomingVol;
                break;


            default:
                MASVol = incomingVol;
                break;

        }
    }

    public void PlayMusic(int songID)
    {
        switch (songID)
        {
            case 0:
                //InitializeMUS(_FModEvents.MUS_Sounds.PracLevel);
                break;
        }
    }

}