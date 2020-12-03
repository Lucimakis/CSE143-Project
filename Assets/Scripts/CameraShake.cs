using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator screenShake(float time, float magnitude)
    {
        Vector3 start = transform.position;
        while (time > 0)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.position = new Vector3(x, y, start.z);
            time -= Time.deltaTime;
            yield return null;
        }
        transform.position = start;
    }
}
