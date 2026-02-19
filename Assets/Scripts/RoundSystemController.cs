using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class RoundSystemController : MonoBehaviour
{
    public static RoundSystemController Instance { get; private set; }

    public float dmgMulti = 1;
    public float speedMulti = 1;
    public float healthMulti = 1;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        dmgMulti += 1;
        speedMulti += 1;
        healthMulti += 1;
    }
}
