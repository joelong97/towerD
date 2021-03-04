using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wavespawner : MonoBehaviour{
    
    public Transform enemyPrefab;
    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;
    public Text waveCountDownText;
    public Text roundTracker;
    public Text WaveTracker;
    private int waveNum = 0;
    private int rounds = 0;
    public Button start;
    

    void Start()
    {
        roundTracker.text = "Round :" + Mathf.Round(rounds).ToString();
        start.onClick.AddListener(TaskOnClick);
        
    }
    
    void UpdateTimer ()
    {   
        waveCountDownText.text = "Next wave in:" + Mathf.Round(countdown).ToString();
        if (waveNum >=10){
            Debug.Log("round over");
            waveNum = 0;
            CancelInvoke ("UpdateTimer");
        }
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            waveCountDownText.text = "";
            CancelInvoke ("UpdateTimer");

        }
        countdown -= Time.deltaTime * 2f;
        
        
    }

    void update() {
        roundTracker.text = "Round :" + Mathf.Round(rounds).ToString();
        Debug.Log("hi");
    }
    
    IEnumerator SpawnWave ()
    {
        waveNum++;
        WaveTracker.text = "Waves : " + Mathf.Round(waveNum).ToString();
        if (waveNum >= 10)
        {
            yield break;
        }
        for (int i = 0; i < waveNum; i++){
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }

        InvokeRepeating("UpdateTimer", 0f, Time.deltaTime);

        
        
        
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    void TaskOnClick(){
        rounds++;
        
        roundTracker.text = "Round :" + Mathf.Round(rounds).ToString();
        InvokeRepeating("UpdateTimer", 0f, Time.deltaTime);
        
        
    }
}
