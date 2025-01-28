using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public int startYear;
    public int startMonth;
    public int startDay;

    public int currentYear;
    public int currentMonth;
    public int currentDay;
    public int currentHour;
    public int currentMinute;

    public float elapsedTime;
    public float timeElapsingSpeed = 10; // 시간의 속도 조절

    private Planet planet;

    // Singletone instance
    public static TimeManager Instance { get; private set;}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        currentYear = startYear;
        currentMonth = startMonth;
        currentDay = startDay;
        currentHour = 0;
        currentMinute = 0;

        elapsedTime = 0;
    }

    
    private void Update()
    {
        elapsedTime += Time.deltaTime * timeElapsingSpeed;

        CheckTime();
        CheckDate();
    }

#region 시간 체크
    private void CheckTime()
    {
        if (elapsedTime >= 60f)
        {
            elapsedTime = 0;
            currentMinute++;
        }
        if (currentMinute >= 60)
        {
            currentMinute = 0;
            currentHour++;
        }
        if (currentHour >= 60)
        {
            currentHour = 0;
            currentDay++;
        }
    }

    private void CheckDate()
    {
        if (currentDay > 30f)
        {
            currentDay = 1;
            currentMonth++;
        }
        if (currentMonth > 12)
        {
            currentMonth = 1;
            currentYear++;
        }
    }
#endregion

    
}
