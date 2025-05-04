using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour
{
    private InputSource _inputSource = new();
    private List<PlantCharacterController> _plantInstances = new();
    private int _currentPlantIndex;

    /// <summary>
    /// Add new elements at the end, remove old elements at the front
    /// </summary>
    private List<PlantCharacterController.Breadcrump> _breadcrumps = new();

    // zero is not allowd, buggs out queue shift.
    [SerializeField] private int _delay;

    [SerializeField] private bool _isPlaying = false;
    public bool IsPlaying => _isPlaying;

    public Vector2 SpawnPosition => Vector2.zero;

    #region Singleton
    private static App s_instance;

    public static App Instance
    {
        get
        {
            if (s_instance == null)
            {
                throw new System.NullReferenceException("Application singleton not set");
            }
            return s_instance;
        }
    }

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            _inputSource.Enable();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion 

    public void StartGame(PlantCombination plantCombination)
    {
        if (!_isPlaying)
        {
            _isPlaying = true;

            for (int i = 0; i < _plantInstances.Count; i++)
            {
                Destroy(_plantInstances[i].gameObject);
            }

            _plantInstances.Clear();
            _breadcrumps.Clear();
            foreach (var plant in plantCombination.Plants)
            {
                PlantCharacterController plantInstance = Instantiate(plant);
                _plantInstances.Add(plantInstance);
                plantInstance.gameObject.SetActive(false);
                plantInstance.name += _plantInstances.Count;
            }
            _plantInstances[0].gameObject.SetActive(true);
            SetPlantInputsource(_plantInstances[0]);
        }
    }

    public void StopGame()
    {
        _isPlaying = false;
    }

    private void SetPlantInputsource(PlantCharacterController characterController)
    {
        _inputSource.BindCharacterController(characterController);
    }

    public bool CanAdvanceToNextPlant()
    {
        // -2 because we can advance to the last entry in the list.
        return _currentPlantIndex < _plantInstances.Count - 2;
    }

    public void CurrentPlantTakeRoot()
    {
        // top queue element is active plan
        var currentPlant = _plantInstances[_currentPlantIndex];
        _currentPlantIndex++;
        PlantCharacterController nextPlant = _plantInstances[_currentPlantIndex];

        currentPlant.TakeRoot();
        SetPlantInputsource(nextPlant);

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
        if (_breadcrumps.Count > _plantInstances.Count * _delay)
        {
            // we add breadcrumps one by one, so a single removal keeps things neat and tidy.
            // remove oldest entry at the front
            _breadcrumps.RemoveAt(0);
        }
    }

    public Vector2 GetButterflyPosition()
    {
        if (_breadcrumps.Count > _delay + 1)
        {
            // queue peek
            return _breadcrumps[^_delay].Position;
        }
        else
        {
            return SpawnPosition;
        }
    }

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
