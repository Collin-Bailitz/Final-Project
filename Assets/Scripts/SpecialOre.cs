using UnityEngine;
using UnityEngine.SceneManagement;

public class SpecialOre : MonoBehaviour
{
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
{
    Debug.Log("Triggered by: " + other.name + " tag: " + other.tag);

    if (!other.CompareTag(playerTag))
    {
        return;
    }

    Debug.Log("You found the special ore! You win!");
}
}
