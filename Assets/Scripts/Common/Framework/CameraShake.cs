using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

    Vector3 originalCameraPosition;    

    public Camera mainCamera;

    public Vector3 cameraResetPosition = new Vector3(0, 0, -10);

    public float shakeMagnitude = 0.0f;
    public float shakeSpeed = 1.0f;

    public float minimumShake = 0.001f;

    private float seed = 0.0f;
    private bool shakeEnded = true;

    public void Update()
    {

        if (shakeMagnitude > minimumShake)
        {
            if (shakeEnded) //get a new random perlin seed
            {
                NewSeed();
                shakeEnded = false;
            }

            //Calculate the noise parameter starting randomly and going as fast as speed allows
            seed += shakeSpeed;

            // map value to [-1, 1]
            //float x = Random.value * 2.0f - 1.0f;
            //float y = Random.value * 2.0f - 1.0f;
            float x = Mathf.Clamp(Mathf.PerlinNoise(seed, 0), 0.0f, 1.0f) * 2.0f - 1.0f;
            float y = Mathf.Clamp(Mathf.PerlinNoise(0, seed), 0.0f, 1.0f) * 2.0f - 1.0f;
            x *= shakeMagnitude * Time.timeScale;
            y *= shakeMagnitude * Time.timeScale;

            mainCamera.transform.localPosition = new Vector3(x, y, mainCamera.transform.localPosition.z);

        }
        else
        {
            shakeEnded = true;
            mainCamera.transform.localPosition = cameraResetPosition;
        }

        //    float elapsed = 0.0f;


        //    Vector3 originalCamPos = Camera.main.transform.localPosition;

        //    originalCamPos.z = Camera.main.transform.localPosition.z;

        //    while (elapsed < duration)
        //    {

        //        elapsed += Time.deltaTime;

        //        float percentComplete = elapsed / duration;
        //        float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

        //        // Calculate the noise parameter starting randomly and going as fast as speed allows
        //        float alpha = randomStart + speed * percentComplete;

        //        // map value to [-1, 1]
        //        //float x = Random.value * 2.0f - 1.0f;
        //        //float y = Random.value * 2.0f - 1.0f;
        //        float x = Mathf.Clamp(Mathf.PerlinNoise(alpha, 0), 0.0f, 1.0f) * 2.0f - 1.0f;
        //        float y = Mathf.Clamp(Mathf.PerlinNoise(0, alpha), 0.0f, 1.0f) * 2.0f - 1.0f;
        //        x *= magnitude * damper;
        //        y *= magnitude * damper;

        //        mainCamera.transform.localPosition = new Vector3(x, y, originalCamPos.z);

        //        yield return null;
        //    }

    }

    private void NewSeed()
    {
        seed = Random.Range(-1000.0f, 1000.0f);
    }

    public float testMagnitude = 0.0f;
    public float testSustain = 0.0f;
    public float testDecay = 0.0f;

    [ContextMenu("Shake It Up!")]
    public void DoShake()
    {
        ShakeCamera(testMagnitude, testSustain, testDecay);
    }

    public void ShakeCamera(float magnitude, float sustainTime, float decayTime)
    {
        StartCoroutine(AddShake(magnitude, sustainTime, decayTime));
    }

    IEnumerator AddShake(float magnitude, float sustainTime, float decayTime)
    {
        shakeMagnitude += magnitude;
        
        if (sustainTime > 0) {
            yield return new WaitForSeconds(sustainTime);
        }
        
        float elapsed = 0.0f;
        float newMagnitude = magnitude;

        while (elapsed < decayTime)
        {            
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / decayTime;

            //remove last update's magnitude value
            shakeMagnitude = Mathf.Max(0, shakeMagnitude - newMagnitude);

            newMagnitude = Mathf.Lerp(magnitude, 0.0f, percentComplete);

            shakeMagnitude += newMagnitude;

            yield return null;
        }

        shakeMagnitude = Mathf.Max(0, shakeMagnitude - newMagnitude);
        
    }

    //public void ShakeCamera(float magnitude, float speed, float duration)
    //{
    //    StartCoroutine(Shake(magnitude, speed, duration));
    //}

    //IEnumerator Shake(float magnitude, float speed, float duration)
    //{
    //    float randomStart = Random.Range(-1000.0f, 1000.0f);
    //    float elapsed = 0.0f;


    //    Vector3 originalCamPos = Camera.main.transform.localPosition;

    //    originalCamPos.z = Camera.main.transform.localPosition.z;

    //    while (elapsed < duration)
    //    {

    //        elapsed += Time.deltaTime;

    //        float percentComplete = elapsed / duration;
    //        float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

    //        // Calculate the noise parameter starting randomly and going as fast as speed allows
    //        float alpha = randomStart + speed * percentComplete;

    //        // map value to [-1, 1]
    //        //float x = Random.value * 2.0f - 1.0f;
    //        //float y = Random.value * 2.0f - 1.0f;
    //        float x = Mathf.Clamp(Mathf.PerlinNoise(alpha, 0), 0.0f, 1.0f) * 2.0f - 1.0f;
    //        float y = Mathf.Clamp(Mathf.PerlinNoise(0, alpha), 0.0f, 1.0f) * 2.0f - 1.0f;
    //        x *= magnitude * damper;
    //        y *= magnitude * damper;

    //        mainCamera.transform.localPosition = new Vector3(x, y, originalCamPos.z);

    //        yield return null;
    //    }

    //    Camera.main.transform.localPosition = originalCamPos;
    //}
}
