﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WindowManager : MonoBehaviour {
	[HideInInspector]
	public GenericWindow[] windows;
	public Windows currentWindowID;
	public Windows defaultWindowID;
	public static GameControl gameControl;

	// Hide Animation
	public GameObject layout;
	private LayoutManager tl;

	void Awake(){
		if (GameObject.Find ("GameControl") != null) {
			gameControl = GameObject.Find ("GameControl").GetComponent<GameControl> ();
			gameControl.windowManager = this;
		}

		// GenericWindow
		GenericWindow.manager = this;
		GenericWindow.gameControl = gameControl;

		// Hide Animation
		if (layout != null) {
			tl = layout.GetComponent<LayoutManager> ();
		}
	}

	public GenericWindow GetWindow(int value){
		return windows [value];
	}

	private void ToggleVisability(int value){
		for (var i = 0; i < windows.Length; i++) {
			var window = windows [i];

			if (i == (int)currentWindowID - 1) {
				window.Open ();
			} else if (window.gameObject.activeSelf) {
				window.Close ();
			}
		}
	}

	public GenericWindow Open(int value){
		if(value < 0 || value > windows.Length)
			return null;

		currentWindowID = (Windows)value;
		ToggleVisability ((int)currentWindowID);

		// Translate Langyage and Icon
		gameControl.translateLanguage = true;
		return GetWindow ((int)currentWindowID - 1);
	}

	void Start(){
		Open ((int)defaultWindowID);
	}

	public GameObject GetActiveWindow(){
		for (var i = 0; i < windows.Length; i++)
			if (windows [i].gameObject.activeSelf)
				return windows [i].gameObject;

		return null;
	}

	public int GetActiveWindowIndex(){
		for (var i = 0; i < windows.Length; i++)
			if (windows [i].gameObject.activeSelf)
				return i;

		return -1;
	}

	void Update(){
		if (layout != null) {
			int aw = GetActiveWindowIndex ();

			// Top
			if (aw > 0 && tl.hide == false) {
				tl.hide = true;
				tl.topLayoutHidden.SetActive (false);
			} else if(aw == 0 && tl.hide == true) {
				tl.hide = false;
				tl.topLayoutHidden.SetActive (true);
			}

			// Bottom
			if ((aw == 1 || aw == 7 || aw == 8 || aw == 9 || aw == 12) && tl.hideBottom == false) {
				tl.hideBottom = true;
				tl.bottomLayoutHidden.SetActive (false);
			} else if(aw == 0 && tl.hideBottom == true) {
				tl.hideBottom = false;
				tl.bottomLayoutHidden.SetActive (true);
			}
		}

		// Back Button
		if(Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex == 0){
			if (GetActiveWindowIndex () == 0)
				Application.Quit ();
			else if(GetActiveWindowIndex () != 1)
				Open (0);
		}
	}
}