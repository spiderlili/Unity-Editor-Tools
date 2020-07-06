using UnityEngine;

public class TimePropertyDrawerDemo : MonoBehaviour
{
    [Time]
    public int TimeMinutes = 3600;
    [Time(true)]
    public int TimeHours = 3600;
    [Time]
    public float TimeError = 3600;
}
