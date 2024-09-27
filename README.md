# DateTimeManager
[![Unity 2021.3+](https://img.shields.io/badge/unity-2021.3.13+-blue)](https://unity3d.com/get-unity/download)
[![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/tayfagames/DateTimeManager/blob/master/LICENSE)


A Unity asset for managing date and time (like Football Manager, Mount &amp; Blade)

## Installation

Simply import the .unitypackage file in the [Releases](https://github.com/tayfagames/datetimemanager/releases/) page to your project.

## Getting Started

Create a new empty object and add DateTimeManager and JobQueue components to it. Or you can use the prefab in the prefabs folder.

![Component setup](https://i.imgur.com/6dCa02c.png)

Manager requires three parameters to work, `Progress Mode`, `Start Date` and `Start On Awake`. For progress mode there 4 are choices: `Minute`, `Hour`, `Day` and `Manual`. 

When the manager is in `Manual` mode you need to progress time manually through code with `ProgressTime` function like this:

```csharp
public enum ProgressMode
{
    Minute,
    Hour,
    Day,
    Manual
}

dateTimeManager.ProgressTime(ProgressMode.Day);
```

Alternatively you can progress to certain `DateTime` like this:

```csharp
DateTime newDate = new DateTime(2023, 05, 29, 22, 27, 00);
dateTimeManager.ProgressTime(newDate);
```

While this way progresses time to a date, most of the events will not trigger depending on the new date.

For `Start Date` DateTimeManager uses a specific string format for expressing DateTime which is `DD.MM.YYYY HH:MM:SS`

If `Start On Awake` is true, DateTimeManager will start time progression when the object is starts running.

---

DateTimeManager's default speed modifier is 1. You can speed up or slow down time progression with modifying the speed modifier. You can also stop time progression with setting the speed modifier to zero.

```csharp
// Increasing speed
dateTimeManager.IncreaseSpeedModifier(); // Increases speed modifier by 1
dateTimeManager.IncreaseSpeedModifier(2f); // Increases speed modifier by 2

// Decreasing speed
dateTimeManager.DecreaseSpeedModifier(); // Decreases speed modifier by 1
dateTimeManager.DecreaseSpeedModifier(2f); // Decreases speed modifier by 2
```

Note: Speed modifier can't be negative.

---

You can stop and resume time progression like this:

```csharp
# Resume
dateTimeManager.ResumeTime();

# Stop
dateTimeManager.StopTime();
```

---

DateTimeManager throws triggers based on time progression. An important note is DateTimeManager triggers the minimum `ProgressMode` events. For example if `ProgressMode` is `Hour`, `Minute` events will not trigger. Additionally to `ProgressMode` triggers, DateTimeManager has events for month and year progression events. 

```csharp
using TayfaGames.DateTimeManager;

public class Demo : MonoBehaviour
{
    DateTimeManager dateTimeManager;

    private void Start()
    {
        dateTimeManager = FindObjectOfType<DateTimeManager>();
    }

    private void OnEnable()
    {
        // Subscribing to the time events
        DateTimeManager.OnMinutePass += HandleMinutePass;
        DateTimeManager.OnHourPass += HandleHourPass;
        DateTimeManager.OnDayPass += HandleDayPass;
        DateTimeManager.OnMonthPass += HandleMonthPass;
        DateTimeManager.OnYearPass += HandleYearPass;
    }

    private void OnDisable()
    {
        DateTimeManager.OnMinutePass -= HandleMinutePass;
        DateTimeManager.OnHourPass -= HandleHourPass;
        DateTimeManager.OnDayPass -= HandleDayPass;
        DateTimeManager.OnMonthPass -= HandleMonthPass;
        DateTimeManager.OnYearPass -= HandleYearPass;
    }

    // These functions will trigger when enough time passes
    private void HandleMinutePass(DateTime dateTime)
    {
        Debug.Log(dateTime.ToString() + " - Minute passed event");
    }

    private void HandleHourPass(DateTime dateTime)
    {
        Debug.Log(dateTime.ToString() + " - Hour passed event");
    }

    private void HandleDayPass(DateTime dateTime)
    {
        Debug.Log(dateTime.ToString() + " - Day passed event");
    }

    private void HandleMonthPass(DateTime dateTime)
    {
        Debug.Log(dateTime.ToString() + " - Month passed event");
    }

    private void HandleYearPass(DateTime dateTime)
    {
        Debug.Log(dateTime.ToString() + " - Year passed event");
    }
}
```

## Job Queue

You can also register functions to queue to execute on certain time.

```csharp
// Function to register
public void GrowPlant()
{
    growthStage += 1;
}

// Register function to the queue
// GrowPlant function will execute after an hour passes in game
dateTimeManager.QueueJob(GrowPlant,currentDateTime + dateTimeManager.Timedelta(hours: 1));
```

## License

Distributed under the MIT License. See `LICENSE` for more information.
