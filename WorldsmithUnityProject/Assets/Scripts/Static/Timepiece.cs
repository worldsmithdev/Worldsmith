using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timepiece : MonoBehaviour
{

    public Timepiece timepiece; //Nasty, nasty singleton. For good reasons, I promise.

    [SerializeField]
    float secondsPerDay, daysPerWeek, weeksPerMonth, monthsPerYear; //Allows for creating custom calendars by adjusting these ratios
    [SerializeField]
    bool IsTurnBased = false;
    //False starts the timepiece in real-time mode which tracks delta time when unpaused
    //True starts the timepiece in turn based mode which converts a turn into a progressed time unit (months, years, etc) when the EndTurn() function is called
    [SerializeField]
    string[] dayNames, weekNames, monthNames, yearNames;

    //Subscribe a function to any of these delegates so it fires any time that time changes
    public delegate void OnTimeChange();
    public OnTimeChange onDayChange;
    public OnTimeChange onWeekChange;
    public OnTimeChange onMonthChange;
    public OnTimeChange onYearChange;

    //Only for use with custom date units
    public delegate void CustomDateChange(CustomTimeStrings unitChanged);
    public CustomDateChange customDateChanged;


    //INTERNALS
    bool isPaused = true;
    float day, week, month, year;

    [System.Serializable]
    public struct Date
    {
        public int day, week, month, year;
        public CustomTimeStrings[] customStrings; //For defining custom calendars, time lengths, time units, etc
    }

    [System.Serializable]
    public class CustomTimeStrings
    { //Serializable lass so all data can be customized by the inspector.
        //Create anything here tied to any date or time unit. e.g. "Century of the Fruitbat" with date: d:1 w:1 m:1 y:400
        public string[] customNames;
        public bool tiedToDate = true; //Assume most people need a standard date for their custom string
        public Date date; //Set this date in the inspector if you are tying this string to a date
        public bool isTimeUnit; //Will set to true if false and tiedToData is false. Enables tracking this string as a date unit
        public float secondsPerUnit; //After this many seconds passes, the currentAmount increments
        public bool isTurnBased; //If using the turn based setting, the timer will use the turnsPerUnit field
        public float turnsPerUnit;
        //INTERNALS
        float currentAmount; //Tracks how many of the units there are at this current time.
    }

    // Start is called before the first frame update
    void Start()
    {
        if (timepiece != null) //Lazy singleton instantiation, because I'm a GREAT programmer ;D
            Destroy(this);
        timepiece = this;

        onDayChange = new OnTimeChange(DebugDelegate);
        onWeekChange = new OnTimeChange(DebugDelegate);
        onMonthChange = new OnTimeChange(DebugDelegate);
        onYearChange = new OnTimeChange(DebugDelegate);
        //customDateChanged should be instantiated if you want custom time units

    }

    void DebugDelegate()
    {
    }

    // Update is called once per frame
    void Update()
    {
        AdvanceTime(Time.deltaTime);
    }

    void AdvanceTime(float secondsElapsed)
    {
        float elapsed = secondsPerDay / secondsElapsed;
        day += elapsed;
        FireDelegate(onDayChange, elapsed);
        if (day > daysPerWeek)
        {
            elapsed = daysPerWeek / day;
            week += elapsed;
            day = 1;
            FireDelegate(onWeekChange, elapsed);
        }

        if (week > weeksPerMonth)
        {
            elapsed = weeksPerMonth / week;
            month += elapsed;
            week = 1;
            FireDelegate(onMonthChange, elapsed);
        }
        if (month > monthsPerYear)
        {
            elapsed = monthsPerYear / month;
            year += elapsed;
            month = 1;
            FireDelegate(onYearChange, elapsed);
        }

    }

    void FireDelegate(OnTimeChange del, float elapsed)
    {
        int rounded = (int)elapsed;
        for (int i = 0; i < rounded; i++)
        {
            del.Invoke();
        }
    }


}