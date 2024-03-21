using CommunityToolkit.Maui.Behaviors;

namespace Bibliomatic_MAUI_App.Behaviors
{
    public class TestRadioButtonStyleBehavior : Behavior<RadioButton>
    {
        public static readonly BindableProperty AttachBehaviorProperty = BindableProperty.CreateAttached(
           propertyName: "AttachBehavior",
           returnType: typeof(object),
           declaringType: typeof(TestRadioButtonStyleBehavior),
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
            RadioButton radioButton = view as RadioButton;
            if (radioButton == null)
            {
                return;
            }

            EventToCommandBehavior attachBehavior = newValue as EventToCommandBehavior;
            radioButton.Behaviors.Add(attachBehavior);
        }
    }
}
