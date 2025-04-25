using Breezeblocks.PlayerScripts;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    private Transform _target; // assign your player here

    private void Awake()
    {
        _target = FindAnyObjectByType<PlayerBase>().transform;
    }

    void LateUpdate()
    {
        transform.position = _target.position;
    }
}
