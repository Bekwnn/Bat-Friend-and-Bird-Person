using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class PlayPatternIdle : MonoBehaviour
{
	public enum IdlePatternState
	{
		PlayingNotes,
		ShuttingUp,
		PausedBetween,
		NONE
	}

	public float pauseBetweenRepeats = 1.5f;

	public float volume = 0.5f;

	public PlayerNoteCreator[] shutUpWhenOtherSpeaking;

	public float shutUpWhenOtherSpeakingRadius = 0f;

	public float pauseBeforeResume = 1f;

	// Updated list created from candidates in the shutUpWhenOtherSpeaking array.
	private List<PlayerNoteCreator> speakingNear;

	private IdlePatternState curState;

	private float curTimer;

	public string patternFile;

	private NotePattern pattern;

	private int curNote;

	public PlayNotes playNotes;

	void Reset()
	{
		if (playNotes == null)
		{
			playNotes = GetComponent<PlayNotes>();
		}
	}

	void Awake()
	{
		speakingNear = new List<PlayerNoteCreator>();
		curState = IdlePatternState.PausedBetween;

		try
		{
			string json = File.ReadAllText(NotePattern.patternFilePath + patternFile + ".json");
			pattern = JsonUtility.FromJson<NotePattern>(json);
		}
		catch (Exception e)
		{
			Debug.Log("Failed to open " + NotePattern.patternFilePath + patternFile + ".json" + ", error: " + e.Message);
		}
	}

	void Update()
	{	
		curTimer += Time.deltaTime;

		speakingNear.Clear();
		foreach (var speaker in shutUpWhenOtherSpeaking)
		{
			if (speaker.currentNotePattern != null && 
					Vector3.Distance(speaker.transform.position, transform.position) < shutUpWhenOtherSpeakingRadius)
			{
				speakingNear.Add(speaker);
			}
		}

		if (speakingNear.Count > 0)
		{
			curState = IdlePatternState.ShuttingUp;
			playNotes.StopPlaying();
			curTimer = 0f;
		}
		

		switch (curState)
		{
		case IdlePatternState.PlayingNotes:
			if (playNotes.currentPattern == null)
			{
				curState = IdlePatternState.PausedBetween;
				curTimer = 0f;
			}
			break;
		case IdlePatternState.PausedBetween:
			if (curTimer > pauseBetweenRepeats)
			{
				if (playNotes.currentPattern == null)
				{
					playNotes.PlayPattern(pattern, volume);
					curState = IdlePatternState.PlayingNotes;
				}
			}
			break;
		case IdlePatternState.ShuttingUp:
			if (speakingNear.Count == 0 && curTimer > pauseBeforeResume)
			{
				if (playNotes.currentPattern == null)
				{
					playNotes.PlayPattern(pattern, volume);
					curState = IdlePatternState.PlayingNotes;
				}
			}
			break;
		default:
			break;
		}
	}
}
