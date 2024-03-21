using CommunityToolkit.Maui.Behaviors;

namespace Bibliomatic_MAUI_App.Behaviors
{
    public class ImageStyleBehavior : Behavior<Image>
    {
        public static readonly BindableProperty AttachBehaviorProperty = BindableProperty.CreateAttached(
            propertyName: "AttachBehavior",
            returnType: typeof(object),
            declaringType: typeof(ImageStyleBehavior),
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
            var image = view as Image;

            if (image == null)
                return;

            IconTintColorBehavior attachBehavior = newValue as IconTintColorBehavior;
            image.Behaviors.Add(attachBehavior);
        }
    }
}
