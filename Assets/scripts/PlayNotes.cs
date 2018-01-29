using System.Collections.Generic;
using UnityEngine;

public class PlayNotes : MonoBehaviour
{
	public AudioClip noteAClip;

	public AudioClip noteBClip;

	public AudioClip noteCClip;

	public AudioClip noteDClip;

	public GameObject noteParticle;

	public AudioSource audioSource;

	public NotePattern currentPattern { get; private set; }

	private float curVolume;

	private float firstNoteTime = -1f;

	private int currentNoteIdx = -1;

	void Reset()
	{
		if (audioSource == null)
		{
			audioSource = GetComponent<AudioSource>();
		}
	}

	void Update()
	{
		if (audioSource != null && currentPattern != null)
		{
			if (Time.time - firstNoteTime > currentPattern.Pattern[currentNoteIdx].time)
			{
				AudioClip clip = null;
				switch (currentPattern.Pattern[currentNoteIdx].note)
				{
				case Note.NoteA:
					clip = noteAClip;
					break;
				case Note.NoteB:
					clip = noteBClip;
					break;
				case Note.NoteC:
					clip = noteCClip;
					break;
				case Note.NoteD:
					clip = noteDClip;
					break;
				case Note.NONOTE:
				default:
					break;
				}

				if (clip != null)
				{
					audioSource.PlayOneShot(clip, curVolume);
					if (noteParticle != null)
					{
						Instantiate(noteParticle, transform); //parent to this
					}
				}

				currentNoteIdx++;
				if (currentNoteIdx >= currentPattern.Pattern.Count)
				{
					StopPlaying();
				}
			}
		}
	}

	public void PlayPattern(NotePattern pattern)
	{
		PlayPattern(pattern, 1f);
	}

	public void PlayPattern(NotePattern pattern, float volume)
	{
		currentPattern = pattern;
		firstNoteTime = Time.time;
		currentNoteIdx = 0;
		curVolume = volume;
	}

	public void StopPlaying()
	{
		currentPattern = null;
		firstNoteTime = -1f;
		currentNoteIdx = -1;
	}
}
