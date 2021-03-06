﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System;

[Serializable]
public class NotePatternEvent : UnityEvent<NotePattern>
{

}

public class NoteListener : MonoBehaviour
{
	public string patternFile;

	public float listenRadius = 5f;

	public NotePatternEvent onMatchingNotes;

	public NotePattern PatternListeningFor { get; set; }

	void Awake()
	{
		if (patternFile != null && patternFile != "")
		{
			try
			{
				string patternJson = File.ReadAllText(NotePattern.patternFilePath + patternFile + ".json");
				PatternListeningFor = JsonUtility.FromJson<NotePattern>(patternJson);
			}
			catch (Exception e)
			{
				Debug.Log("Failed to open " + NotePattern.patternFilePath + patternFile + ".json" + ", error: " + e.Message);
			}	
		}
	}

	public bool DoesPatternMatch(NotePattern pattern)
	{
		if (PatternListeningFor == null || pattern.Pattern.Count != PatternListeningFor.Pattern.Count)
		{
			return false;
		}

		bool matches = true;
		for (int i = 0; i < PatternListeningFor.Pattern.Count; ++i)
		{
			// If the note doesn't match or the timing is wrong, reject match and break.
			if (pattern.Pattern[i].note != PatternListeningFor.Pattern[i].note ||
					Mathf.Abs(pattern.Pattern[i].time - PatternListeningFor.Pattern[i].time) > NotePattern.halfNoteWindow)
			{
				matches = false;
				break;
			}
		}

		if (onMatchingNotes != null && matches)
		{
			onMatchingNotes.Invoke(pattern);
		}

		return matches;
	}
}
