using UnityEngine;

internal class RubyController : MonoBehaviour
{
    void Update()
    {
        Vector2 position = transform.position;
        position.x = position.x + 0.1f;
        transform.position = position;
    }
}
