using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour {

    public float magnitude = 1.0f;
    public float sustainTime = 0.2f;
    public float decayTime = 0.2f;

    public bool shakeOnStart = false;
    public bool shakeOnCollision = false;
    public LayerMask collisionMask;

    public CameraShake shake;
    // Use this for initialization
    void Start() {

        if (shake == null)
        {
            shake = Camera.main.GetComponent<CameraShake>();
        }

        if (shakeOnStart)
        {
            Shake();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Shake()
    {
        if (shake != null)
        {
            shake.ShakeCamera(magnitude, sustainTime, decayTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (shakeOnCollision)
        {
            int layer = collision.gameObject.layer;
            if (collisionMask == (collisionMask | (1 << layer)))
            {
                Shake();
            }
        }
    }
}
