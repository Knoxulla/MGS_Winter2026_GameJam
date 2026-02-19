using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemRespawnManager : MonoBehaviour
{
    public static ItemRespawnManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartRespawn(GameObject item, float delay)
    {
        StartCoroutine(RespawnCoroutine(item, delay));
    }

    private IEnumerator RespawnCoroutine(GameObject item, float delay)
    {
        yield return new WaitForSeconds(delay);
        item.SetActive(true);
    }
}
