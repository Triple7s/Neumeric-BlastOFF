using UnityEngine;

public class S_TurnTerrainMenu : MonoBehaviour
{
    [SerializeField] private Transform _terrainTransform;
    [SerializeField] [Range(-1, 1)] private int _turnX;
    [SerializeField] [Range(-1, 1)] private int _turnY;
    [SerializeField] [Range(-1, 1)] private int _turnZ;
    [SerializeField] private float _turnSpeed = 10f;

    private void Update()
    {
        if (!_terrainTransform) return;
        float turnAmount =  _turnSpeed * Time.deltaTime;
        _terrainTransform.Rotate(_turnX * turnAmount, _turnY * turnAmount, _turnZ * turnAmount);
    }
}
