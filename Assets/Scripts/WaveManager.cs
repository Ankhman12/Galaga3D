using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    private int wave;
    private List<BoidSpawner> boidSpawners;
    [SerializeField] GameObject boidSpawner;
    // Start is called before the first frame update
    void Start()
    {
        wave = 1;
        GameObject.Find("WaveText").GetComponent<TMP_Text>().text = "Wave: 1";
        boidSpawners = new List<BoidSpawner>();
        int x = Random.Range(-100, 100);
        int y = Random.Range(-100, 100);
        int z = Random.Range(-100, 100);
        Vector3 pos = new Vector3(x, y, z);
        boidSpawners.Add(Instantiate(boidSpawner, pos, Quaternion.identity).GetComponent<BoidSpawner>());
    }

    // Update is called once per frame
    void Update()
    {
        foreach(BoidSpawner b in boidSpawners)
        {
            if (b.waspsCount > 0 || b.beetleCount > 0)
            {
                return;
            }
        }
        foreach (BoidSpawner b in boidSpawners)
        {
            Destroy(b.gameObject);
        }
        wave = wave + 1;
        GameObject.Find("WaveText").GetComponent<TMP_Text>().text = "Wave: " + wave;
        for (int i = 0; i < wave / 2; i++)
        {
            int x = Random.Range(-100, 100);
            int y = Random.Range(-100, 100);
            int z = Random.Range(-100, 100);
            Vector3 pos = new Vector3(x, y, z);
            boidSpawners.Add(Instantiate(boidSpawner, pos, Quaternion.identity).GetComponent<BoidSpawner>());
        }
    }
}
