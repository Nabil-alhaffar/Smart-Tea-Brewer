//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("TeaPot1.Components.CircleDateTime.CircleTime.xaml", "Components/CircleDateTime/CircleTime.xaml", typeof(global::TeaPot1.Components.CircleDateTime.CircleTime))]

namespace TeaPot1.Components.CircleDateTime {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Components\\CircleDateTime\\CircleTime.xaml")]
    public partial class CircleTime : global::Tizen.Wearable.CircularUI.Forms.CirclePage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Label TeaTimerheader;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Tizen.Wearable.CircularUI.Forms.CircleDateTimeSelector timeSelector;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(CircleTime));
            TeaTimerheader = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Label>(this, "TeaTimerheader");
            timeSelector = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Tizen.Wearable.CircularUI.Forms.CircleDateTimeSelector>(this, "timeSelector");
        }
    }
}
