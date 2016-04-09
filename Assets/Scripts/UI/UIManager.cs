/*
MIT License

Copyright (c) [2016] [Digvijay Patel https://github.com/digzou]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public delegate void OnStateChangeHandler ();

public class UIManager : MonoBehaviour
{
	static UIManager _instance;

	public event OnStateChangeHandler OnStateChange;

	public enum State
	{
		MainMenu ,
		CharacterSelect
	};

	State currState;

	Transform currentActiveCharacter;

	public GameObject mainMenu;
	public GameObject characterSelect;

	//Creating a singleton for UI manager.
	public static UIManager instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<UIManager> ();
				
				//Tell unity not to destroy this object when loading a new scene!
				if (_instance != null)
					DontDestroyOnLoad (_instance.gameObject);
			}
				return _instance;
			}
	}
	
	void Awake ()
	{
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (this);
		} else {
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if (this != _instance)
				Destroy (this.gameObject);
		}
	}

	void Start ()
	{
		spawnInitCharacter ();

		OnStateChange += HandleStateChange;

		//Set initial ui state
		HandleStateChange ();
	}

	void spawnInitCharacter(){
		GameObject go = Instantiate (Resources.Load ("Prefabs/Players/Cube3")) as GameObject;
		currentActiveCharacter = go.transform;
		currentActiveCharacter.SetParent(transform);
		currentActiveCharacter.localScale = Vector3.one;
		currentActiveCharacter.localPosition = Vector3.zero;
		currentActiveCharacter.localRotation = Quaternion.identity;
		currentActiveCharacter.name = "Character 3";
	}

	void HandleStateChange ()
	{
		mainMenu.SetActive (CurrState == State.MainMenu);
		characterSelect.SetActive (CurrState == State.CharacterSelect);
	}

	public State CurrState {
		get {
			return currState;
		}
		set {
			currState = value;
			
			if (OnStateChange != null)
				OnStateChange ();
		}
	}

	public Transform CurrentActiveCharacter {
		get {
			return currentActiveCharacter;
		}
		set {
			if(currentActiveCharacter !=null)
				Destroy(currentActiveCharacter.gameObject);

			currentActiveCharacter = value;
		}
	}
}
