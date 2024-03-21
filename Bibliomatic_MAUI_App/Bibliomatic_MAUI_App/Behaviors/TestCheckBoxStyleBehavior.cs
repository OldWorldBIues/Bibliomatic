using CommunityToolkit.Maui.Behaviors;

namespace Bibliomatic_MAUI_App.Behaviors
{
    public class TestCheckBoxStyleBehavior : Behavior<CheckBox>
    {
        public static readonly BindableProperty AttachBehaviorProperty = BindableProperty.CreateAttached(
           propertyName: "AttachBehavior",
           returnType: typeof(object),
           declaringType: typeof(TestCheckBoxStyleBehavior),
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
            CheckBox checkBox = view as CheckBox;
            if (checkBox == null)
            {
                return;
            }

            EventToCommandBehavior attachBehavior = newValue as EventToCommandBehavior;
            checkBox.Behaviors.Add(attachBehavior);
        }
    }
}
