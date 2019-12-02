using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatorManager : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyPrefab;
    [SerializeField]
    private int numbers;
    [SerializeField]
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Tester, will delete later
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MakeEnemy();
        }
    }

    public void MakeEnemy()
    {
        StartCoroutine(InstantiateEnemy());
    }

    IEnumerator InstantiateEnemy()
    {
        for (int i = 0; i < numbers; i++)
        {
            Object.Instantiate(EnemyPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(time);
        }
    }
}
