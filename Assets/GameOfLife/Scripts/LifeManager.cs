using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class LifeManager : SingletonMonoBehaviour<LifeManager> {
	private int tileCount = 0;

	public bool[][] tiles;

	public Object TilePrefab = null;

	public Object BorderPrefab = null;

	public int GridSize = 4;

	public Transform TileParent = null;

	public Transform BorderParent = null;

	public int Step = 1;

	public bool Paused = false;

	#region EVENTS
	public UnityEvent OnStep;

	public UnityEvent UpdateTileState;
	#endregion

	// Use this for initialization
	void Start () {
		tiles = new bool[GridSize][];
		for (int i = 0; i < tiles.Length; ++i) {
			tiles[i] = new bool[GridSize];
		}		
		
		tileCount = GridSize * GridSize;
		Camera.main.transform.position = new Vector3 (0, 0, -GridSize);

		StartCoroutine (PopulateTiles ());
	}

	public void SetTileState(int x, int y, bool alive) {
		tiles [y] [x] = alive;
	}

	public bool GetTileState(int x, int y) {
		return tiles [y] [x];
	}

	public void ExecuteStep () {
		if (OnStep != null) {
			OnStep.Invoke ();
		}

		if (UpdateTileState != null) {
			UpdateTileState.Invoke ();
		}

		Step++;
	}

	private IEnumerator PopulateTiles () {
		Vector2 coords = new Vector2 (-tiles.Length / 2f, -tiles.Length / 2f);
		int step = tileCount / 100;
		int currentStep = 0;

		for (int y = -1; y < tiles.Length + 1; ++y) {
			int length = y < 0 || y >= tiles.Length ? tiles[0].Length : tiles[y].Length;
			for (int x = -1; x < length; ++x) {
				GameObject tile = null;
				Vector3 tileCoord =  new Vector3(coords.x + x, coords.y + y, 0);
				if (y < 0 || x < 0 || y >= tiles.Length || x >= length - 1) {
					tile = Instantiate(BorderPrefab, tileCoord, Quaternion.identity) as GameObject;
					tile.transform.SetParent (BorderParent);
				} else {
					tile = Instantiate(TilePrefab, tileCoord, Quaternion.identity) as GameObject;
					tile.transform.SetParent(TileParent);

					LifeTile lt = tile.GetComponent<LifeTile> ();
					lt.SetCoords (x, y);

					currentStep++;
					if (currentStep > step) {
						currentStep = 0;
						yield return new WaitForFixedUpdate ();
					}
				}
			}
		}

		StartCoroutine (IncrementStep ());

		yield break;
	}

	private IEnumerator IncrementStep () {
		while (true) {
			yield return null;

			while (Paused) {
				yield return null;
			}			
			
			ExecuteStep ();
		}
	}
}
