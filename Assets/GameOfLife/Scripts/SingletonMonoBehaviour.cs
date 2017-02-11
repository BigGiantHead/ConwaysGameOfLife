using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T: MonoBehaviour {
	protected static T instance = null;

	public static T Instance {
		get {
			return instance;
		}
	}

	protected virtual void Awake() {
		if (instance != null) {
			Destroy (instance.gameObject);
		}

		instance = this as T;
	}
}
