namespace Bibliomatic_MAUI_App.CustomControls;

public partial class TestElementExpanderControl : ContentView
{
    public static readonly BindableProperty ContentPaddingProperty = BindableProperty.Create(
        propertyName: nameof(ContentPadding),
        returnType: typeof(double),
        declaringType: typeof(TestElementExpanderControl));

    public double ContentPadding
    {
        get => (double)GetValue(ContentPaddingProperty);
        set => SetValue(ContentPaddingProperty, value);
    }

    public static readonly BindableProperty ElementExpandedProperty = BindableProperty.Create(
        propertyName: nameof(ElementExpanded),
        returnType: typeof(bool),
        defaultValue: false,
        declaringType: typeof(TestElementExpanderControl));

    public bool ElementExpanded
    {
        get => (bool)GetValue(ElementExpandedProperty);
        set => SetValue(ElementExpandedProperty, value);
    }

    public static readonly BindableProperty ExpanderHeaderProperty = BindableProperty.Create(
        propertyName: nameof(ExpanderHeader),
        returnType: typeof(string),
        declaringType: typeof(TestElementExpanderControl));

    public string ExpanderHeader
    {
        get => (string)GetValue(ExpanderHeaderProperty);
        set => SetValue(ExpanderHeaderProperty, value);
    }

    public static readonly BindableProperty ContentBorderVisibleProperty = BindableProperty.Create(
        propertyName: nameof(ContentBorderVisible),
        returnType: typeof(bool),
        declaringType: typeof(TestElementExpanderControl));

    public bool ContentBorderVisible
    {
        get => (bool)GetValue(ContentBorderVisibleProperty);
        set => SetValue(ContentBorderVisibleProperty, value);
    }

    public static readonly BindableProperty ExpanderImageSourceProperty = BindableProperty.Create(
        propertyName: nameof(ExpanderImageSource),
        returnType: typeof(ImageSource),
        declaringType: typeof(TestElementExpanderControl));

    public ImageSource ExpanderImageSource
    {
        get => (ImageSource)GetValue(ExpanderImageSourceProperty);
        set => SetValue(ExpanderImageSourceProperty, value);
    }

    public TestElementExpanderControl()
	{
		InitializeComponent();
	}
}