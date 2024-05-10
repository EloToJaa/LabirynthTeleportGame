using UnityEngine;

public class WinPlace : MonoBehaviour
{
    private float alfa = 0.0f;

    public float Resizer()
    {
        float value = Mathf.Sin(alfa);
        alfa += (1.5f * Time.deltaTime);
        return value + 2f;
    }

    private void FixedUpdate()
    {
        float scale = Resizer();
        transform.localScale = new Vector3(scale, 4.5f, scale);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.WinGame();
        }
    }
}
