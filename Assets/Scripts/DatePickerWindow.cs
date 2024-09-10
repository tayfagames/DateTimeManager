using UnityEditor;
using UnityEngine;
using System;
using System.Globalization;

public class DatePickerWindow : EditorWindow
{
    private Action<DateTime> onDateSelected;

    private static int hour;
    private static int minute;
    private static int second;
    private int selectedDay;
    private static DateTime lastDateTime = DateTime.Now;
    public static DateTime selectedDate = lastDateTime;

    public static void Show(Action<DateTime> onDateSelected, string dateTimeString)
    {
        var window = GetWindow<DatePickerWindow>(true, "Select Date");
        window.minSize = new Vector2(300, 300);
        window.onDateSelected = onDateSelected;
        window.ShowPopup();
        lastDateTime = DateTime.ParseExact(dateTimeString, "MM.dd.yyyy HH:mm:ss", CultureInfo.InvariantCulture); ;
        selectedDate = lastDateTime;
        hour = lastDateTime.Hour;
        minute = lastDateTime.Minute;
        second = lastDateTime.Second;
    }

    private void OnGUI()
    {
        GUILayout.Label($"{selectedDate:MMMM yyyy}", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<1y", GUILayout.Width(75)))
        {
            selectedDate = selectedDate.AddMonths(-12);
        }
        if (GUILayout.Button("<1m", GUILayout.Width(75))) 
        {
            selectedDate = selectedDate.AddMonths(-1);
        }
        if (GUILayout.Button("1m>", GUILayout.Width(75))) 
        {
            selectedDate = selectedDate.AddMonths(1);
        }
        if (GUILayout.Button("1y>", GUILayout.Width(75)))
        {
            selectedDate = selectedDate.AddMonths(12);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<100y", GUILayout.Width(75)))
        {
            selectedDate = selectedDate.AddMonths(-1200);
        }
        if (GUILayout.Button("<10y", GUILayout.Width(75)))
        {
            selectedDate = selectedDate.AddMonths(-120);
        }
        if (GUILayout.Button("10y>", GUILayout.Width(75)))
        {
            selectedDate = selectedDate.AddMonths(120);
        }
        if (GUILayout.Button("100y>", GUILayout.Width(75)))
        {
            selectedDate = selectedDate.AddMonths(1200);
        }
        GUILayout.EndHorizontal();

        string[] dayLabels = new string[] { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };
        GUILayout.BeginHorizontal();
        foreach (var dayLabel in dayLabels)
        {
            GUILayout.Label(dayLabel, GUILayout.Width(40));
        }
        GUILayout.EndHorizontal();

        var firstDayOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
        int startDay = (int)firstDayOfMonth.DayOfWeek;

        int daysInMonth = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);

        GUILayout.BeginHorizontal();
        for (int i = 0; i < startDay; i++)
        {
            GUILayout.Button("", GUILayout.Width(40));
        }

        for (int day = 1; day <= daysInMonth; day++)
        {
            if ((day + startDay - 1) % 7 == 0)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }

            GUIStyle selectedStyle = new GUIStyle(GUI.skin.button);
            if (day == selectedDay)
            {
                selectedStyle.normal.textColor = Color.white;
                selectedStyle.normal.background = MakeTexture(2, 2, Color.blue);
            }

            if (day == lastDateTime.Day)
            {
                selectedDay = day;
                selectedStyle.normal.textColor = Color.white;
                selectedStyle.normal.background = MakeTexture(2, 2, Color.blue);
            }

            if (GUILayout.Button(day.ToString(), selectedStyle, GUILayout.Width(40)))
            {
                selectedDay = day;
                lastDateTime = new DateTime(selectedDate.Year, selectedDate.Month, day);
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Hour:", GUILayout.Width(100));
        hour = EditorGUILayout.IntSlider(hour, 0, 23);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Minute:", GUILayout.Width(100));
        minute = EditorGUILayout.IntSlider(minute, 0, 59);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Second:", GUILayout.Width(100));
        second = EditorGUILayout.IntSlider(second, 0, 59);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("OK", GUILayout.Width(300)))
        {
            selectedDate = new DateTime(selectedDate.Year, selectedDate.Month, selectedDay, hour, minute, second);
            onDateSelected?.Invoke(selectedDate);
            lastDateTime = selectedDate;
            Close();
        }
    }

    private Texture2D MakeTexture(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}
