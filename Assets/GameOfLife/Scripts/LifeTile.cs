using UnityEngine;
using System.Collections;

public class LifeTile : MonoBehaviour {
	//not recommended in Unity(or anywhere)
	private static Vector2[] neighbourIndices = new Vector2[8] {
		new Vector2(-1, -1),
		new Vector2(-1, 0),
		new Vector2(-1, 1),
		new Vector2(0, -1),
		new Vector2(0, 1),
		new Vector2(1, -1),
		new Vector2(1, 0),
		new Vector2(1, 1)
	};

	private int x = -1;

	private int y = -1;

	private AudioSource boingSound = null;

	[SerializeField]
	private Renderer myRenderer;

	private bool alive = false;

	public bool Alive {
		get {
			return alive;
		}
	}

	// Use this for initialization
	void Start () {
		alive = Random.value > 0.8f ? true : false;
		myRenderer.enabled = Alive;
		boingSound = GameObject.Find ("BoingSound").GetComponent<AudioSource>();

		LifeManager.Instance.OnStep.AddListener (TileStep);
		LifeManager.Instance.UpdateTileState.AddListener (UpdateTileState);

		LifeManager.Instance.SetTileState(x, y, Alive);
	}

	void OnMouseDown () {
		boingSound.Play ();
		alive = !alive;		
		myRenderer.enabled = Alive;
		LifeManager.Instance.SetTileState(x, y, Alive);
	}

	public void SetCoords(int x, int y) {
		this.x = x;
		this.y = y;
	}

	private void CheckIfAlive () {
		Vector2 neighbourIndex = Vector2.zero;
		int aliveNeighbourCount = 0;

		for (int i = 0; i < neighbourIndices.Length; ++i) {
			neighbourIndex = neighbourIndices[i];
			neighbourIndex.x += x;
			neighbourIndex.y += y;

			if (neighbourIndex.x < 0 || neighbourIndex.x >= LifeManager.Instance.GridSize || neighbourIndex.y < 0 || neighbourIndex.y >= LifeManager.Instance.GridSize)
				continue;

			if (LifeManager.Instance.GetTileState((int)neighbourIndex.x, (int)neighbourIndex.y)) {
				aliveNeighbourCount++;
			}
		}

		switch (aliveNeighbourCount) {
			case 0:
			case 1:
				alive = false;
				break;
			case 2:
			if (alive) 
				alive = true;
				break;
			case 3:
			alive = true;
				break;
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			default:
			alive = false;	
				break;
		}
	}

	private void TileStep () {				
		CheckIfAlive ();
		
		myRenderer.enabled = Alive;
	}

	private void UpdateTileState() {
		LifeManager.Instance.SetTileState(x, y, Alive);
	}
}
