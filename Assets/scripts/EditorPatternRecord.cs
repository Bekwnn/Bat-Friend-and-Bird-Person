using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class EditorPatternRecord : MonoBehaviour
{
	public static readonly string dataPath = "Assets/data/";

	public InputField fileName;

	public Text recordButtonText;

	public Text lastRecordedTextDisplay;

	public PlayNotes playNotes;

	[HideInInspector]
	public bool recording = false;

	private string lastRecorded;

	private float firstNoteTime = -1f;

	private NotePattern currentNotePattern;

	void Reset()
	{
		if (playNotes == null)
		{
			playNotes = GetComponent<PlayNotes>();
		}
	}

	void Update()
	{
		if (recording)
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

					Debug.Log(deadTimePassed);
					Debug.Log(currentNotePattern.Pattern.Count);

					if (currentNotePattern != null && deadTimePassed > NotePattern.patternEndWindow)
					{
						recording = false;
						recordButtonText.text = "Record";
						firstNoteTime = -1f;

						string json = JsonUtility.ToJson(currentNotePattern);
						currentNotePattern = null;

						// Save currentNotePattern.
						if (fileName.text != null || fileName.text != "")
						{
							try
							{
								File.WriteAllText(dataPath + fileName.text + ".json", json);
								lastRecorded = fileName.text;
								lastRecordedTextDisplay.text = lastRecorded;
							}
							catch (Exception e)
							{
								Debug.Log("Failed to save pattern " + NotePattern.patternFilePath + fileName.text +
										", error: " + e.Message);
							}
						}
					}
				}
			}
		}
	}

	public void RecordingToggle()
	{
		if (currentNotePattern != null) return;

		recording = (recording)? false : true;
		if (recording)
		{
			recordButtonText.text = "Cancel Recording";
		}
		else
		{
			recordButtonText.text = "Record";
		}
	}

	public void PlayBackLast()
	{
		if (lastRecorded == null)
		{
			Debug.Log("No last recorded file on record.");
			return;
		}

		string json = File.ReadAllText(dataPath + lastRecorded + ".json");
		NotePattern nPattern = JsonUtility.FromJson<NotePattern>(json);

		PlayNotes playNotes = GetComponent<PlayNotes>();
		playNotes.PlayPattern(nPattern);
	}

	public void DeleteLast()
	{
		if (lastRecorded == null)
		{
			Debug.Log("No last recorded file on record.");
			return;
		}

		string path = dataPath + lastRecorded + ".json";
		try
		{
			File.Delete(path);
			Debug.Log("Deleted file " + path);
		}
		catch (Exception e)
		{
			Debug.Log("Exception " + e.Message + " in deleting file " + path);
		}
	}
}
