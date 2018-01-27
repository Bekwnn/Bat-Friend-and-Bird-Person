using System.Collections.Generic;
using UnityEngine;

public class PlayerNoteCreator : NoteCreator
{
	public AudioClip noteAClip;

	public AudioClip noteBClip;

	public AudioClip noteCClip;

	public AudioClip noteDClip;

	public AudioSource audioSource;

	private NotePattern currentNotePattern;

	private float firstNoteTime = -1f;

	void Reset()
	{
		if (audioSource == null)
		{
			audioSource = GetComponent<AudioSource>();
		}
	}

	void Update()
	{
		Note inputNote = Note.NONOTE;

		if (Input.GetButtonDown("NoteA"))
		{
			inputNote = Note.NoteA;
			audioSource.PlayOneShot(noteAClip);
		}
		else if (Input.GetButtonDown("NoteB"))
		{
			inputNote = Note.NoteB;
			audioSource.PlayOneShot(noteBClip);
		}
		else if (Input.GetButtonDown("NoteC"))
		{
			inputNote = Note.NoteC;
			audioSource.PlayOneShot(noteCClip);
		}
		else if (Input.GetButtonDown("NoteD"))
		{
			inputNote = Note.NoteD;
			audioSource.PlayOneShot(noteDClip);
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
					firstNoteTime = -1f;

					currentNotePattern = null;
				}
			}
		}
	}
}
