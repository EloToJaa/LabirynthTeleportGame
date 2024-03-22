using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform closePosition;
    public Transform openPosition;
    public Transform door;

    public bool open = false;

    private void Start()
    {
        if(open)
        {
            door.position = openPosition.position;
        }
        else
        {
            door.position = closePosition.position;
        }
    }
}
