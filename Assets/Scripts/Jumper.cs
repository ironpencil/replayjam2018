using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    public Transform target;
	public BallColor followColor;
	public PlayerColorState colorState;
	public List<Transform> players;
	public Transform ball;
    public ParticleSystemWrangler particleWrangler;

	private PlayerParticleController ppc;

	public JumperConfig config;

	void Start()
	{
		transform.parent = ball;
	}

    void Update()
    {
		if (target != null) {
			float step = config.speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, target.position, step);
			if (Vector3.Distance(transform.position, target.position) < step) {
				transform.parent = target;
				ppc = target.gameObject.GetComponentInChildren<PlayerParticleController>();
				if (ppc != null) {
					Debug.Log("Enabling " + followColor + " from jumper");
					ppc.EnableColor(followColor);
				}
				target = null;
                particleWrangler.Stop();
			}
		}
    }

	public void ChangeTarget()
	{
		if (ppc != null) {
			Debug.Log("Disabling color from jumper");
			ppc.DisableColor(followColor);
		}
        particleWrangler.Play();
		transform.parent = null;
		int owner = colorState.GetBallOwner(followColor);
		if (owner > -1) {
			target = players[owner];
		} else {
			target = ball;
		}
	}
}
