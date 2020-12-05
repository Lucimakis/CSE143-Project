using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Shakes the camera 
public class CameraShake : MonoBehaviour
{
    // Takes a float duration and float magnitude
    // Shakes the camera for the duration with a severity based on the magnitude;
    public IEnumerator screenShake(float time, float magnitude)
    {
        Vector3 start = transform.position; // starting position of the camera
        while (time > 0)
        {
            float x = Random.Range(-1f, 1f) * magnitude; // Generates a random number between -magnitude and +magnitude
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.position = new Vector3(x, y, start.z); // Moves the camera to the randomly generated position
            time -= Time.deltaTime; // Counts down the duration
            yield return null;
        }
        transform.position = start; 
    }
}
