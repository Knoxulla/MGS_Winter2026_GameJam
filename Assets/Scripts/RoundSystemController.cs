using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class RoundSystemController : MonoBehaviour
{
    public static RoundSystemController Instance { get; private set; }

    public float dmgMulti = 1;
    public float speedMulti = 1;
    public float healthMulti = 1;

    public float addToMulti = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dmgMulti = 1;
        speedMulti = 1;
        healthMulti = 1;
    }

    private void Awake()
    {
        // Singleton Set up
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateStats()
    {
        dmgMulti += addToMulti;
        speedMulti += addToMulti;
        healthMulti += addToMulti;
    }
}
