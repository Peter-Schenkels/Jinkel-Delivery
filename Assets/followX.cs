using UnityEngine;
using System.Collections;

public class followX : MonoBehaviour
{

	//offset from the viewport center to fix damping
	public float m_DampTime = 10f;
	Transform m_Target;
	public float m_XOffset = 0;
	public float m_YOffset = 0;
	public GameObject scooter;
	public GameObject player;
	movement_scooter scooterStats;

	public float cameraViewPlayer = 5;
	public float cameraViewScooter = 7;
	float targetCameraView;

	public Camera camera;

	private float margin = 0.1f;

	void Start()
	{
		scooterStats = scooter.GetComponent<movement_scooter>();
	}

	void Update()
	{
		if(scooterStats.PlayerOnScooter)
		{
			m_Target = scooter.transform;
			targetCameraView = cameraViewScooter;
		}
		else
		{
			m_Target = player.transform;
			targetCameraView = cameraViewPlayer;
		}
		if (m_Target)
		{
			float targetX = m_Target.position.x + m_XOffset;
			float targetY = m_Target.position.y + m_YOffset;

			if (Mathf.Abs(transform.position.x - targetX) > margin)
				targetX = Mathf.Lerp(transform.position.x, targetX, 1 / m_DampTime * Time.deltaTime);

			if (Mathf.Abs(transform.position.y - targetY) > margin)
				targetY = Mathf.Lerp(transform.position.y, targetY, m_DampTime * Time.deltaTime);

			transform.position = new Vector3(targetX, targetY, transform.position.z);
		}

		float targetCamera = targetCameraView;

		if (Mathf.Abs(camera.orthographicSize - targetCamera) > margin)
		{
			targetCamera = Mathf.Lerp(camera.orthographicSize, targetCamera, 1 / m_DampTime * Time.deltaTime);
		}

		camera.orthographicSize = targetCamera;
	}
}