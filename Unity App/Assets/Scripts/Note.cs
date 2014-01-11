using UnityEngine;
using System.Collections;

public class Note
{		
	public enum Which
	{
		A = 0,
		B = 1,
		C = 2,
		D = 3,
		E = 4,
		F = 5,
		NONE = -1
	}
	
	public int start;
	public int length;
	
	public Note(int start, int length)
	{
		this.start = start;
		this.length = length;
	}
}