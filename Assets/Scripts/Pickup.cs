using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public void Picked()
    {
        Debug.Log("Picked up!");
        Destroy(gameObject);
    }
}
