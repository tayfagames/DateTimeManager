# DateTimeManager
[![Unity 2021.3+](https://img.shields.io/badge/unity-2021.3.13+-blue)](https://unity3d.com/get-unity/download)
[![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/tayfagames/DateTimeManager/blob/master/LICENSE)


A Unity asset for managing date and time (like Football Manager, Mount &amp; Blade)

## Installation

Copy the repo to your project folder and you're good to go!

## Getting Started

Create a new empty object and add DateTimeManager component to it.

![Component setup](https://i.imgur.com/pAbKxIO.png)

Manager requires two parameters to work, `Progress Mode` and `Start Date`. For progress mode there 4 are choices: `Minute`, `Hour`, `Day` and `Manual`. 

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
---

For `Start Date` DateTimeManager uses a specific string format for expressing DateTime which is `DD.MM.YYYY HH:MM:SS`

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
