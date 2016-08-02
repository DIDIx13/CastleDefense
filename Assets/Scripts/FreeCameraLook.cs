using UnityEngine;
//using UnityEditor;

public class FreeCameraLook : Pivot {

	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float turnSpeed = 1.5f;
	[SerializeField] private float turnsmoothing = .1f;
	[SerializeField] private float tiltMax = 75f;
	[SerializeField] private float tiltMin = 45f;

    public bool isEnabled = true;

    private Vector3 lookPos;
	private float lookAngle;
	private float tiltAngle;

	private const float LookDistance = 100f;

	private float smoothX = 0;
	private float smoothY = 0;
	private float smoothXvelocity = 0;
	private float smoothYvelocity = 0;

    float verticalRotation = 15f;
    float minY = -40f;
    float maxY = 40f;


	protected override void Awake()
	{
		base.Awake();

		cam = GetComponentInChildren<Camera>().transform;
		pivot = cam.parent;
	}
	
	// Update is called once per frame
    protected override void Update ()
	{
		base.Update();

		HandleRotationMovement();
	}

	protected override void Follow (float deltaTime)
	{
        transform.position = Vector3.Lerp(transform.position, target.position, deltaTime * moveSpeed);
	}

	void HandleRotationMovement()
	{
        float x = 0;
        float y = 0;
        if (isEnabled)
        {
            x = Input.GetAxis("Mouse X");
            y = Input.GetAxis("Mouse Y");
        }

        if (turnsmoothing > 0)
        {
            smoothX = Mathf.SmoothDamp(smoothX, x, ref smoothXvelocity, turnsmoothing);
            smoothY = Mathf.SmoothDamp(smoothY, y, ref smoothYvelocity, turnsmoothing);
        }
        else
        {
            smoothX = x;
            smoothY = y;
        }

        lookAngle += smoothX * turnSpeed;

        transform.rotation = Quaternion.Euler(0f, lookAngle, 0);
        target.rotation = Quaternion.Euler(0f, lookAngle, 0);

        tiltAngle -= smoothY * turnSpeed;
        tiltAngle = Mathf.Clamp(tiltAngle, -tiltMin, tiltMax);

        pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
        float rotateX = Input.GetAxis("Mouse X");
	}

}
