using UnityEngine;

public class Parallaxer : MonoBehaviour
{
    private class PoolObject
	{
		public Transform transform;
		public bool inUse;
		public PoolObject(Transform _t) { transform = _t; }
		public void Use() { inUse = true; }
		public void Dispose() { inUse = false; }
	}

	[System.Serializable]
	public struct YSpawnRange
	{
		public float minY;
		public float maxY;
	}

	public GameObject Prefab;
	public int poolSize;
	public float shiftSpeed;
	public float spawnRate;

	public YSpawnRange ySpawnRange;
	public Vector3 defaultSpawnPos;
	public bool spawnImmediate;
	public Vector3 immediateSpawnPos;
	public Vector2 targetAspectRatio;

	private float spawnTimer;
	private PoolObject[] poolObjects;
	private float targetAspect;
	private GameManager game;

	private void Awake()
	{
		Configure();
	}

	private void Start()
	{
		game = GameManager.Instance;
	}

	private void OnEnable()
	{
		GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
	}

	private void OnDisable()
	{
		GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
	}

	private void OnGameOverConfirmed()
	{
		for (int i = 0; i < poolObjects.Length; i++)
		{
			poolObjects[i].Dispose();
			poolObjects[i].transform.position = Vector3.one * 1000;
		}
		Configure();
	}

	private void Update()
	{
		if (game.GameOver) return;

		Shift();
		spawnTimer += Time.deltaTime;
		if (spawnTimer > spawnRate)
		{
			Spawn();
			spawnTimer = 0;
		}
	}

	private void Configure()
	{
        // spawning pool objects
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;
		poolObjects = new PoolObject[poolSize];
		for (int i = 0; i < poolObjects.Length; i++)
		{
			GameObject go = Instantiate(Prefab) as GameObject;
			Transform t = go.transform;
			t.SetParent(transform);
			t.position = Vector3.one * 1000;
			poolObjects[i] = new PoolObject(t);
		}

		if (spawnImmediate)
		{
			SpawnImmediate();
		}
	}

	private void Spawn()
	{
		// moving pool objects into place
		Transform t = GetPoolObject();
		if (t == null) return;
		Vector3 pos = Vector3.zero;
		pos.y = Random.Range(ySpawnRange.minY, ySpawnRange.maxY);
		pos.x = (defaultSpawnPos.x * Camera.main.aspect) / targetAspect;
		t.position = pos;
	}

	private void SpawnImmediate()
	{
		Transform t = GetPoolObject();
		if (t == null) return;
		Vector3 pos = Vector3.zero;
		pos.y = Random.Range(ySpawnRange.minY, ySpawnRange.maxY);
		pos.x = (immediateSpawnPos.x * Camera.main.aspect) / targetAspect;
		t.position = pos;
		Spawn();
	}

	private void Shift()
	{
		// loop through pool objects 
		// moving them
		// discarding them as they go off screen
		for (int i = 0; i < poolObjects.Length; i++)
		{
			poolObjects[i].transform.position += -Vector3.right * shiftSpeed * Time.deltaTime;
			CheckDisposeObject(poolObjects[i]);
		}
	}

	private void CheckDisposeObject(PoolObject _poolObject)
	{
		// place objects off screen
		if (_poolObject.transform.position.x < (-defaultSpawnPos.x * Camera.main.aspect) / targetAspect)
		{
			_poolObject.Dispose();
			_poolObject.transform.position = Vector3.one * 1000;
		}
	}

	private Transform GetPoolObject()
	{
		//retrieving first available pool object
		for (int i = 0; i < poolObjects.Length; i++)
		{
			if (!poolObjects[i].inUse)
			{
				poolObjects[i].Use();
				return poolObjects[i].transform;
			}
		}
		return null;
	}
}