using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum Note 
{
	NoteA,
	NoteB,
	NoteC,
	NoteD,
	NONOTE
}

[Serializable]
public struct NoteStamp
{
	public NoteStamp(Note note, float time)
	{
		this.note = note;
		this.time = time;
	}
	
	[SerializeField]
	public Note note;

	[SerializeField]
	public float time;
}

// NOTE: Patterns should always start at 0f time.
[Serializable]
public class NotePattern
{
	public static readonly string patternFilePath = "Assets/data/";

	public static readonly float patternEndWindow = 0.8f;

	public static readonly float halfNoteWindow = 0.2f;

	public NotePattern(List<NoteStamp> noteSequence)
	{
		Pattern = noteSequence;
	}
	
	[SerializeField]
	private List<NoteStamp> pattern;
	public List<NoteStamp> Pattern
	{
		get { return pattern; }
	   	private set { pattern = value; }
	}
}
