using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    AudioManager audioManager;

    private readonly InputSource _inputSource = new();
    private int _currentPlantIndex;

    /// <summary>
    /// Add new elements at the end, remove old elements at the front
    /// </summary>
    private readonly List<PlantCharacterController.Breadcrump> _breadcrumps = new();

    [SerializeField, Tooltip("Arrangement of plants to be used for Gameplay.")] private List<PlantCharacterController> _plantPreset = new();
    [SerializeField] private float[] _plantTimes;
    [SerializeField] private Butterflycontroller _butterfly;
    [SerializeField] private UI_InGameScript _ingameUI;
    [SerializeField, Tooltip("zero is not allowd, buggs out queue shift")] private int _delay;
    [SerializeField] private bool _isPlaying = false;
    public bool IsPlaying => _isPlaying;
    public Vector2 SpawnPosition => Vector2.zero;

    private float _timeLeft;

    private void Update()
    {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0f)
        {
            if (CanAdvanceToNextPlant() && _plantPreset[_currentPlantIndex].CanTakeRoot())
            {
                CurrentPlantTakeRoot();
            }
            else
            {
                GameOver();
                _timeLeft = 0f;
            }
        }
        _ingameUI.UpdateTimer(_timeLeft);
    }

    #region gamestate start/stop
    public void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        _timeLeft = _plantTimes[0];
        _ingameUI.StartNextLevel(_timeLeft);

        _butterfly.Setup(this);

        _plantPreset[0].Setup(this);
        _plantPreset[0].gameObject.SetActive(true);

        for (int i = 1; i < _plantPreset.Count; i++)
        {
            PlantCharacterController plant = _plantPreset[i];
            plant.Setup(this);
            plant.gameObject.SetActive(false);
        }

        SetPlantInputsource(_plantPreset[0]);
        _inputSource.Enable();
        _isPlaying = true;
    }

    public void GameOver()
    {
        if (_isPlaying)
        {
            _ingameUI.OpenLooseScreen("you dried");
            _isPlaying = false;
            _inputSource.Disable();
            _plantPreset[_currentPlantIndex].Stop();

            audioManager.SelectBackgroundMusic("Lose");
            audioManager.PlaySFX("GameOver");
        }
    }

    public void GameWon()
    {
        if (_isPlaying)
        {
            _ingameUI.OpenWinScreen("you bloomed");
            _isPlaying = false;
            _inputSource.Disable();
            _plantPreset[_currentPlantIndex].Stop();

            audioManager.SelectBackgroundMusic("Win");
            audioManager.PlaySFX("Victory");
        }
    }
    #endregion gamestate start/stop

    #region Follow up system
    private void SetPlantInputsource(PlantCharacterController characterController)
    {
        _inputSource.BindCharacterController(characterController);
    }

    public bool CanAdvanceToNextPlant()
    {
        return _currentPlantIndex < _plantPreset.Count - 1;
    }

    public void CurrentPlantTakeRoot()
    {
        // top queue element is active plan
        var currentPlant = _plantPreset[_currentPlantIndex];
        _currentPlantIndex++;
        PlantCharacterController nextPlant = _plantPreset[_currentPlantIndex];

        currentPlant.TakeRoot();
        SetPlantInputsource(nextPlant);

        _timeLeft = _plantTimes[_currentPlantIndex];
        _ingameUI.SpawnNextPlant(_timeLeft);

        if (_breadcrumps.Count > _delay + 1)
        {
            // remove the block of newest entries until we hit the reentry point            
            _breadcrumps.RemoveRange(_breadcrumps.Count - _delay, _delay);

            // last entry, we just popped all entries of the "Stack"
            nextPlant.StartFromBreadcrump(_breadcrumps[^1]);
        }
        else
        {
            _breadcrumps.Clear();
            nextPlant.StartFromBreadcrump(
                new PlantCharacterController.Breadcrump(
                    position: SpawnPosition,
                    velocity: Vector2.zero,
                    PlantCharacterController.Characterstates.GroundedLocomotion));
        }
    }

    /// <summary>
    /// Called from physics update, so delta time is fixed.
    /// </summary>
    /// <param name="breadcrump"></param>
    /// <exception cref="NotImplementedException"></exception>
    internal void PushBreadcrump(PlantCharacterController.Breadcrump breadcrump)
    {
        // add at the end
        _breadcrumps.Add(breadcrump);
        // + 1 because we start to count the active plant index at 0. 
        // for the last plant we want the firstelement in the array.
        // For everyone before we want to look deeper inside.
        if (_breadcrumps.Count > _plantPreset.Count * _delay)
        {
            // we add breadcrumps one by one, so a single removal keeps things neat and tidy.
            // remove oldest entry at the front
            _breadcrumps.RemoveAt(0);
        }
    }

    public Vector2 GetButterflyPosition()
    {
        if (_breadcrumps.Count >= _delay)
        {
            // queue peek
            return _breadcrumps[^(_delay - 1)].Position;
        }
        else
        {
            return SpawnPosition;
        }
    }
    #endregion Follow up system

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        int i = 0;
        foreach (var item in _breadcrumps)
        {
            UnityEditor.Handles.Label(item.Position, i.ToString());
            i++;
        }
    }
#endif
}