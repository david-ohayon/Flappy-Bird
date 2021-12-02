using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour
{

    public delegate void PlayerDelegate();
	public static event PlayerDelegate OnPlayerDied;
	public static event PlayerDelegate OnPlayerScored;

	public float tapForce = 10f;
	public float tiltSmooth = 5f;
	public Vector3 startPos;

	public AudioSource tapSound;
	public AudioSource scoreSound;
	public AudioSource dieSound;

	private Rigidbody2D rigidBody;
	private Quaternion downRotation;
	private Quaternion forwardRotation;

	private GameManager game;
	// TrailRenderer trail;

	private void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		downRotation = Quaternion.Euler(0, 0, -90);
		forwardRotation = Quaternion.Euler(0, 0, 40);
		game = GameManager.Instance;
		rigidBody.simulated = false;
		// trail = GetComponent<TrailRenderer>();
		// trail.sortingOrder = 20; 
	}

	private void OnEnable()
	{
		GameManager.OnGameStarted += OnGameStarted;
		GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
	}

	private void OnDisable()
	{
		GameManager.OnGameStarted -= OnGameStarted;
		GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
	}

	private void OnGameStarted()
	{
		rigidBody.velocity = Vector3.zero;
		rigidBody.simulated = true;
	}

	private void OnGameOverConfirmed()
	{
		transform.localPosition = startPos;
		transform.rotation = Quaternion.identity;
	}

	private void Update()
	{
		if (game.GameOver) return;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			transform.rotation = forwardRotation;
			rigidBody.velocity = Vector2.zero;
			rigidBody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
			tapSound.Play();
		}

		transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
	}

	private void OnTriggerEnter2D(Collider2D _collision)
	{
		if (_collision.gameObject.CompareTag("ScoreZone"))
		{
			OnPlayerScored();
			scoreSound.Play();
		}
		if (_collision.gameObject.CompareTag("DeadZone"))
		{
			rigidBody.simulated = false;
			OnPlayerDied();
			dieSound.Play();
		}
	}
}