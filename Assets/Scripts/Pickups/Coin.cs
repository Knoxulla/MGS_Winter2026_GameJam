using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Coin : MonoBehaviour
{
    [Header("Config"), SerializeField, 
    Tooltip("ID based on its ID in itemCatalogue")] 
    private int itemID = 0;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private bool respawnAfterTime;
    [SerializeField] private float respawnTimeSeconds = 8;
    [SerializeField] private int goldGained = 1;

    private CircleCollider2D circleCollider;
    private SpriteRenderer visual;

    private void Awake() 
    {
        circleCollider = GetComponent<CircleCollider2D>();
        visual = GetComponentInChildren<SpriteRenderer>();
    }

    private void CollectCoin() 
    {
        circleCollider.enabled = false;
        visual.gameObject.SetActive(false);
       // Add Currency Here
        StopAllCoroutines();
        if(respawnAfterTime) ItemRespawnManager.Instance.StartRespawn(gameObject, respawnTimeSeconds);
    }

    private void OnTriggerEnter2D(Collider2D otherCollider) 
    {
        if (GameManager.Instance.ContainsLayer(playerLayer, otherCollider.gameObject.layer))
        {
            CollectCoin();
        }
    }
}
