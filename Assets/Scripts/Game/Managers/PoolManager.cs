using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager _instance;
    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Pool Manager Is null");
            return _instance;
        }      
    }
    [SerializeField] private GameObject _laserContainer;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private List<GameObject> _laserPool;
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _laserPool = GenerateBullets(1);
    }


    List<GameObject> GenerateBullets(int amountOfBullets)
    {
        for(int i = 0; i < amountOfBullets; i++)
        {
            GameObject laser = Instantiate(_laserPrefab);
            laser.transform.parent = _laserContainer.transform;
            laser.SetActive(false);
            _laserPool.Add(laser);
        }

        return _laserPool;
    }

    public void RequestLaser(Vector3 requestedPos)
    {
        bool foundLaser = false;
        foreach(var laser in _laserPool)
        {
            if(laser.activeInHierarchy == false)
            {
                laser.transform.position = requestedPos;
                laser.SetActive(true);
                foundLaser = true;
                break;
            }
        }
        if (foundLaser == true)
            return;
        GenerateBullets(1);
        RequestLaser(requestedPos);
    }
}
