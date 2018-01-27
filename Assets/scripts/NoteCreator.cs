using System.Collections.Generic;
using UnityEngine;

public class NoteCreator : MonoBehaviour
{
	public void NotifyListenersOfPattern(NotePattern pattern)
	{
		var allListeners = FindObjectsOfType(typeof(NoteListener)) as NoteListener[];
		foreach (var listener in allListeners)
		{
			if (Vector3.Distance(listener.gameObject.transform.position, transform.position) <= listener.listenRadius)
			{
				listener.DoesPatternMatch(pattern);
			}
		}
	}
}
