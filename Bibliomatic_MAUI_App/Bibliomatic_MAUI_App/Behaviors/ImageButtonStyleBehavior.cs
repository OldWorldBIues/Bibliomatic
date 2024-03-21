using CommunityToolkit.Maui.Behaviors;

namespace Bibliomatic_MAUI_App.Behaviors
{
    public class ImageButtonStyleBehavior : Behavior<ImageButton>
    {
        public static readonly BindableProperty AttachBehaviorProperty = BindableProperty.CreateAttached(
           propertyName: "AttachBehavior",
           returnType: typeof(object),
           declaringType: typeof(ImageButtonStyleBehavior),
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
            var imageButton = view as ImageButton;

            if (imageButton == null)
                return;

            IconTintColorBehavior attachBehavior = newValue as IconTintColorBehavior;
            imageButton.Behaviors.Add(attachBehavior);
        }
    }
}
