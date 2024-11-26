using UnityEngine;

public class PlayerEchoTrail : MonoBehaviour
{
    [SerializeField]
    private GameObject _echo;
    [SerializeField]
    private float _spawnRate;

    private float _defaultSpawnRate;

    private void Awake()
    {
        _defaultSpawnRate = _spawnRate;
    }

    private void Update()
    {
        if(_spawnRate <= 0)
        {
            var echo = Instantiate(_echo, transform.position, Quaternion.identity);
            echo.name = "tempTrail";
            _spawnRate = _defaultSpawnRate;
            _echo = echo;
        }

        else
        {
            _spawnRate -= Time.deltaTime;
        }
    }
}
