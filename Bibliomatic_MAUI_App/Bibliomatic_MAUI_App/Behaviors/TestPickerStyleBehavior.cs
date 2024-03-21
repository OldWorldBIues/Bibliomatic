using CommunityToolkit.Maui.Behaviors;

namespace Bibliomatic_MAUI_App.Behaviors
{
    public class TestPickerStyleBehavior : Behavior<Picker>
    {
        public static readonly BindableProperty AttachBehaviorProperty = BindableProperty.CreateAttached(
           propertyName: "AttachBehavior",
           returnType: typeof(object),
           declaringType: typeof(TestPickerStyleBehavior),
           defaultValue: null,
           propertyChanged: OnAttachBehaviorChanged);

        public static object GetAttachBehavior(BindableObject view)
        {
            return (object)view.GetValue(AttachBehaviorProperty);
        }

        public static void SetAttachBehavior(BindableObject view, object value)
        {
            view.SetValue(AttachBehaviorProperty, value);
        }

        static void OnAttachBehaviorChanged(BindableObject view, object oldValue, object newValue)
        {
            Picker picker = view as Picker;
            if (picker == null)
            {
                return;
            }

            EventToCommandBehavior attachBehavior = newValue as EventToCommandBehavior;
            picker.Behaviors.Add(attachBehavior);
        }
    }
}
