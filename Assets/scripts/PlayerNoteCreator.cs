using System.Collections.Generic;
using UnityEngine;

public class PlayerNoteCreator : NoteCreator
{
	public PlayNotes playNotes;

	public NotePattern currentNotePattern { get; private set; }

	private float firstNoteTime = -1f;

	void Reset()
	{
		if (playNotes == null)
		{
			playNotes = GetComponent<PlayNotes>();
		}
	}

	void Update()
	{
		Note inputNote = Note.NONOTE;

		if (Input.GetButtonDown("NoteA"))
		{
			inputNote = Note.NoteA;
			playNotes.PlayPattern(NotePattern.soloA);
		}
		else if (Input.GetButtonDown("NoteB"))
		{
			inputNote = Note.NoteB;
			playNotes.PlayPattern(NotePattern.soloB);
		}
		else if (Input.GetButtonDown("NoteC"))
		{
			inputNote = Note.NoteC;
			playNotes.PlayPattern(NotePattern.soloC);
		}
		else if (Input.GetButtonDown("NoteD"))
		{
			inputNote = Note.NoteD;
			playNotes.PlayPattern(NotePattern.soloD);
		}
	
		if (inputNote != Note.NONOTE)
		{
			if (currentNotePattern == null)
			{
				currentNotePattern = new NotePattern(new List<NoteStamp>{
						new NoteStamp { note = inputNote, time = 0f } 
						} );
				firstNoteTime = Time.time;
			}	
			else
			{
				currentNotePattern.Pattern.Add( new NoteStamp { note = inputNote, time = Time.time - firstNoteTime } );
			}
		}
		else
		{
			if (currentNotePattern != null)
			{
				// Check if a second has passed since last note, stop recording if so.
				float deadTimePassed = Time.time - (firstNoteTime +
					currentNotePattern.Pattern[currentNotePattern.Pattern.Count-1].time);

				if (currentNotePattern != null && deadTimePassed > NotePattern.patternEndWindow)
				{
					NotifyListenersOfPattern(currentNotePattern);

					firstNoteTime = -1f;

					currentNotePattern = null;
				}
			}
		}
	}
}
