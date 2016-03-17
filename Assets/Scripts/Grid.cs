using System.Collections;
using UnityEngine;

public class Grid
{
	string[] 					_names = {"aaa", "bbb", "ccc", "ddd", "eee", "fff", "ggg", "hhh", "iii", "jjj"};
	string[] 					_colors = {"red", "green", "blue"};
	const string				_error = "empty";
	const int					_minId = 1;
	const int					_maxId = 100;

	public readonly int 		id;
	public readonly string 		name;
	public readonly string 		color;

	public Grid()
	{
		id = Random.Range(_minId, _maxId + 1);
		//id = test;
		//test++;

		if (_names != null && _names.Length > 0)
		{
			name = _names[Random.Range(0, _names.Length)];
		}
		else
		{
			name = _error;
		}

		if (_colors != null && _colors.Length > 0)
		{
			color = _colors[Random.Range(0, _colors.Length)];
		}
		else
		{
			color = _error;
		}
	}

	//static int test = 0;
}
