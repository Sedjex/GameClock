using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class ClockReality : MonoBehaviour
{
    float secondsNew = 60f, minutesNew = 60f, hoursNew = 12f;
    int interval = 1;
    float nextTime = 1;

    [SerializeField] Text seconds;
    [SerializeField] Text minutes;
    [SerializeField] Text hours;

    private Text days;

    DateTime time = DateTime.Now;
    DateTime timeStart = DateTime.Now;

    public Button TimeNow;
    public Button TimeDay;

    public Button TimeDelta;

    public Dropdown Changing;

    public Sprite day;
    public Sprite night;

    public GameObject panel;

    void Start()
    {
        DateTime currentDate = DateTime.Now;

        long temp = Convert.ToInt64(PlayerPrefs.GetString("sysString"));

        DateTime oldDate = DateTime.FromBinary(temp);
        print("oldDate: " + oldDate);

        TimeSpan difference = currentDate.Subtract(oldDate);
        print("Difference: " + difference);

        PlayerPrefs.SetString("sysString", currentDate.ToBinary().ToString());

        Changing.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(Changing);
        });
    }

    public void DropdownValueChanged(Dropdown change)
    {
        int[] min = new int[] { -4320, -2880, -1440, 1440, 2880, 4320, -30, -20, -10, 10, 20, 30 };
        PlusMinutes(min[change.value]);
    }

    public void PressTimeNow()
    {
        print(time.Hour + " :" + time.Minute + " :" + time.Second);
    }

    public void ChangeBG()
    {
        if (time.Hour >= 5 && time.Hour <= 19)
        {
            panel.GetComponent<Image>().sprite = day;
            TimeNow.GetComponent<Image>().color = new Color(0, 0, 0);
            TimeDay.GetComponent<Image>().color = new Color(0, 0, 0);
            TimeDelta.GetComponent<Image>().color = new Color(0, 0, 0);
            Changing.GetComponent<Image>().color = new Color(0, 0, 0);
        }
        else
        {
            panel.GetComponent<Image>().sprite = night;
        }
    }

    public void PressTimesDay()
    {
        if (time.Hour >= 5 && time.Hour <= 10)
        {
            print("Morning");
        }
        else if (time.Hour > 10 && time.Hour <= 19)
        {
            print("Day");
        }
        else
        {
            print("Night");
        }
    }

    public void DeltaTime()
    {
        TimeSpan delta = DateTime.Now.Subtract(timeStart);
        double days = Math.Floor(delta.TotalDays);
        double minutes = Math.Floor(delta.TotalMinutes);
        double seconds = Math.Floor(delta.TotalSeconds);

        print(days + " Days " + minutes + " Minutes " + seconds + " Seconds ");
    }

    public void PlusMinutes(int count)
    {
        time = time.AddMinutes(count);
        UpdateClock();
    }

    void UpdateClock()
    {
        seconds.text = ("" + (time.Second * (60f / secondsNew))).PadLeft(2, '0');
        minutes.text = ("" + (time.Minute * (60f / minutesNew))).PadLeft(2, '0') + " :";
        hours.text = ("" + (time.Hour * (12f / hoursNew))).PadLeft(2, '0') + " :";
    }

    void Update()
    {
        if (Time.time >= nextTime)
        {
            time = time.AddSeconds(1);
            UpdateClock();
            ChangeBG();
            nextTime += interval;
        }

    }

}

