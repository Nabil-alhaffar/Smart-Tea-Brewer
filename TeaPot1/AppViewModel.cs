using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Tizen.Wearable.CircularUI;
using TeaPot1.Components;
using TeaPot1.Components.CircleDateTime;

namespace TeaPot1
{
    public class AppViewModel : ContentPage
    {
        public AppViewModel()
        {
            Components = new ObservableCollection<Component>();
            Components.Add(new Component { Title = "Tea Timer", Class = typeof(CircleTime) });
            Components.Add(new Component { Title = "Heart Rate Sensor", Class = typeof(HeartRateSensor) });
            Components.Add(new Component { Title = "Sleep Sensor ", Class = typeof(SleepSensor) });
            Components.Add(new Component { Title = "Tea Brewer", Class = typeof(TeaBrewer) });
        }
        public IList<Component> Components { get; private set; }
        public IList<Component> DateTimeComponents { get; private set; }
    }
}