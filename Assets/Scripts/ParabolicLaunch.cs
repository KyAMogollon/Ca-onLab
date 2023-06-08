using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicLaunch : MonoBehaviour
{
    [SerializeField] private Rigidbody ball;
	[SerializeField] private Transform target;

	[SerializeField] private float max_H = 25;
	[SerializeField] private float custom_Gravity = -18;

	public bool debugPath;

	void Update() {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Launch ();
		}

		if (debugPath) {
			DrawPath ();
		}
	}

	void Launch() {
		Physics.gravity = Vector3.up * custom_Gravity;
		ball.useGravity = true;
		ball.velocity = CalculateLaunchData().initialVelocity;
	}

	LaunchData CalculateLaunchData() {
		float displacementY = target.position.y - ball.position.y;
		Vector3 displacementXZ = new Vector3 (target.position.x - ball.position.x, 0, target.position.z - ball.position.z);
		float time = Mathf.Sqrt(-2*max_H/custom_Gravity) + Mathf.Sqrt(2*(displacementY - max_H)/custom_Gravity);
		Vector3 velocityY = Vector3.up * Mathf.Sqrt (-2 * custom_Gravity * max_H);
		Vector3 velocityXZ = displacementXZ / time;

		return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(custom_Gravity), time);
	}

	void DrawPath() {
		LaunchData launchData = CalculateLaunchData ();
		Vector3 previousDrawPoint = ball.position;

		int resolution = 30;
		for (int i = 1; i <= resolution; i++) {
			float simulationTime = i / (float)resolution * launchData.timeToTarget;
			Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up *custom_Gravity * simulationTime * simulationTime / 2f;
			Vector3 drawPoint = ball.position + displacement;
			Debug.DrawLine (previousDrawPoint, drawPoint, Color.green);
			previousDrawPoint = drawPoint;
		}
	}

	struct LaunchData {
		public readonly Vector3 initialVelocity;
		public readonly float timeToTarget;

		public LaunchData (Vector3 initialVelocity, float timeToTarget)
		{
			this.initialVelocity = initialVelocity;
			this.timeToTarget = timeToTarget;
		}
		
	}
}
