﻿using UnityEngine;

public class EatingBugsController : MonoBehaviour
{
	public int[] BugsOnLvl;

	private InputController _inputController;
	private EatingModel _eatingModel;
	private MovingUpObjects _movingUpObjects;
	private StaminaSlider _staminaSlider;
	private AnimatorsModel _animatorsModel;
	private ParticlesController _particlesController;
	private SoundController _soundController;
	private Coins _coins;
	private Boss _bossModel;
	private int _currentLvl = 0;
	private bool _nothingWasEaten = true;
	private bool _bitingEnded = true;

	private void Awake()
	{
		_eatingModel = FindObjectOfType<EatingModel>();
		_movingUpObjects = FindObjectOfType<MovingUpObjects>();
		_inputController = FindObjectOfType<InputController>();
		_staminaSlider = FindObjectOfType<StaminaSlider>();
		_animatorsModel = FindObjectOfType<AnimatorsModel>();
		_particlesController = FindObjectOfType<ParticlesController>();
		_soundController = FindObjectOfType<SoundController>();
		_bossModel = FindObjectOfType<Boss>();
		_coins = FindObjectOfType<Coins>();
	}
	private void Start()
	{
		BugsOnLvl = _eatingModel.BugsOnLvls;
	}
	private void Update()
	{
		if (_inputController.InputStarted && !_eatingModel.IsBiting && _eatingModel.СanBiteAgain && _eatingModel.СanBiteAtAll)
		{
			_nothingWasEaten = true;
			_eatingModel.IsBiting = true;
			_eatingModel.СanBiteAgain = false;
			_animatorsModel.MakeBiteAnimation();
			_bitingEnded = false;
			if (_bossModel.IsBossFightNow)
			{	
				if(_eatingModel.SpeedOfBiting != _eatingModel.SpeedOfBitingForBossBattle)
				{
					_eatingModel.SpeedOfBiting = _eatingModel.SpeedOfBitingForBossBattle;
				}
				_soundController.PlayEatSomethingSound();
				_bossModel.BossGetDamage();
				_particlesController.PlayBossParticles();
				_coins.AddCoin();
			}
		}
		if (!_inputController.InputStarted && !_eatingModel.СanBiteAgain)
		{
			_eatingModel.СanBiteAgain = true;
		}
		if (_eatingModel.BiteWasMade && !_bitingEnded)
		{
			ReduceStamina();
		}
	}
	private void FixedUpdate()
	{
		if (_eatingModel.IsBiting)
		{
			_eatingModel.MakeBite();
		}
	}
	public void EatBug(GameObject BugObject)
	{
		_coins.AddCoin();
		Destroy(BugObject);
		_nothingWasEaten = false;
		_soundController.PlayEatSomethingSound();
		if (BugsOnLvl.Length - _currentLvl == 9)
		{
			_movingUpObjects.NeedToMoveOnlyCharacter = true;
		}
		if (BugsOnLvl.Length - _currentLvl == 1)
		{
			_movingUpObjects.MovingUpAmount = 2f;
		}
		BugsOnLvl[_currentLvl]--;
		if (BugsOnLvl[_currentLvl] <= 0)
		{
			_currentLvl++;
			_movingUpObjects.MoveObjectsUp();
		}
		if (BugsOnLvl.Length - _currentLvl == 0)
		{
			_bossModel.NeedToStartBossBattle = true;
		}
		_staminaSlider.IncreaseStaminaByNum(_eatingModel.EnergyBySingleBug);
	}
	public void EatSomething()
	{
		_soundController.PlayEatSomethingSound();
		_nothingWasEaten = false;
	}
	private void ReduceStamina()
	{
		if (_nothingWasEaten)
		{
			if (!_bossModel.IsBossFightNow)
			{
				_soundController.PlayHitTreeSound();
			}
			if (!_bossModel.IsBossFightNow)
			{
				_staminaSlider.ReduceStaminaByNum(_eatingModel.EnergyMissClick);
			}
			_nothingWasEaten = false;
		}
		_bitingEnded = true;
	}
}
