using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject[] BugPrefabs;
	public BugsPreset[] LvlPresets;
	public GameObject[] SticksPrefab;
	public float StartYPostion;
	public float SpaceBetweenLines;
	public int AmountOfPresets;
	public bool NeedToSpawnPreset;
	public Vector3[] PositonOfBugs;
	public float[] RotationOfBugs;

	private int[] _bugsOnLvl;
	private int _linesPastForSticks;
	private int _linesWrited;
	private int _nonBugsOnLine;

	public int[] SpawnBugsFromPreset()
	{
		_bugsOnLvl = new int[0];
		for (int i = 0; i < AmountOfPresets; i++)
		{
			int SelectedPresetNum = UnityEngine.Random.Range(0, LvlPresets.Length);
			BugsPreset Preset = LvlPresets[SelectedPresetNum];
			Debug.Log("Spawn Preset with name :" + Preset.name);
			int[] Temp = new int[Preset.PartsOfPreset.Length];
			for (int j = 0; j < Preset.PartsOfPreset.Length; j++)
			{
				_nonBugsOnLine = 0;
				for (int k = 0; k < Preset.PartsOfPreset[j].SingleBugPreset.Length; k++)
				{
					if (Preset.PartsOfPreset[j].SingleBugPreset[k].NumberOfBug == 3 || Preset.PartsOfPreset[j].SingleBugPreset[k].NumberOfBug == 2)
					{
						_nonBugsOnLine++;
					}
				}
				for (int k = 0; k < Preset.PartsOfPreset[j].SingleBugPreset.Length; k++)
				{
					Temp[j] = Preset.PartsOfPreset[j].SingleBugPreset.Length - _nonBugsOnLine;
					Vector3 PositionOfBug = PositonOfBugs[Preset.PartsOfPreset[j].SingleBugPreset[k].PositionOfBug];
					PositionOfBug.y = StartYPostion;
					GameObject Bug = Instantiate(BugPrefabs[Preset.PartsOfPreset[j].SingleBugPreset[k].NumberOfBug], PositionOfBug, Quaternion.identity);
					Bug.transform.Rotate(new Vector3(0, RotationOfBugs[Preset.PartsOfPreset[j].SingleBugPreset[k].PositionOfBug], 0));
				}
				_linesPastForSticks++;
				if (_linesPastForSticks == 3)
				{
					SpawnStick();
					_linesPastForSticks = 0;
				}
				StartYPostion += SpaceBetweenLines;
			}


			int[] BugsWrited = new int[_bugsOnLvl.Length];
			for (int j = 0; j < _bugsOnLvl.Length; j++)
			{
				BugsWrited[j] = _bugsOnLvl[j];
			}
			_bugsOnLvl = new int[BugsWrited.Length + Temp.Length];
			for (int j = 0; j < BugsWrited.Length; j++)
			{
				_bugsOnLvl[j] = BugsWrited[j];
			}
			for (int j = 0; j < Temp.Length; j++)
			{
				_bugsOnLvl[_linesWrited] = Temp[j];
				_linesWrited++;
			}
		}
		return _bugsOnLvl;
	}

	private void SpawnStick()
	{
		Vector3 PositionOfStick = PositonOfBugs[2];
		PositionOfStick.y = StartYPostion;
		Instantiate(SticksPrefab[UnityEngine.Random.Range(0, SticksPrefab.Length)], PositionOfStick, Quaternion.identity);
	}
}
