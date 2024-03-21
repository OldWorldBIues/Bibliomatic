using CommunityToolkit.Maui.Behaviors;

namespace Bibliomatic_MAUI_App.Behaviors
{
    public class TestEditorStyleBehavior : Behavior<Editor>
    {
        public static readonly BindableProperty AttachBehaviorProperty = BindableProperty.CreateAttached(
            propertyName: "AttachBehavior", 
            returnType:typeof(object), 
            declaringType: typeof(TestEditorStyleBehavior), 
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
            var editor = view as Editor;

            if (editor == null)            
                return;            

            EventToCommandBehavior attachBehavior = newValue as EventToCommandBehavior;
            editor.Behaviors.Add(attachBehavior);
        }
    }

    
}
