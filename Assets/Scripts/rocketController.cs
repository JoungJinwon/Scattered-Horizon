using UnityEngine;

public class rocketController : MonoBehaviour
{
    public float rocketSpeed = 10f;

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.forward * rocketSpeed * Time.deltaTime);
    }
}
