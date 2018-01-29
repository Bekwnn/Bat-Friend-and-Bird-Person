using System.Collections.Generic;
using UnityEngine;

public class TurnGreenOnSuccessEvent : MonoBehaviour
{
	public PlayNotes playNotes;

	void Reset()
	{
		if (playNotes == null)
		{
			playNotes = GetComponent<PlayNotes>();
		}
	}

	public void Execute(NotePattern p)
	{
		Renderer rend = GetComponent<Renderer>();
		rend.material.SetColor("_Color", Color.green);
		PlayPatternIdle idlePattern = GetComponent<PlayPatternIdle>();
		idlePattern.enabled = false;
		playNotes.PlayPattern(p);
	}
}
