using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision");
        if (other.gameObject.CompareTag("Player"))
        {
            GlobalEventManager.CollectCoin();
            this.gameObject.SetActive(false);
        }
    }
}
