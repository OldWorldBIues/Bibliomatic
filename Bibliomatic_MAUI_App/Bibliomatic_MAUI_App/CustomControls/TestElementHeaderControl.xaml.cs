namespace Bibliomatic_MAUI_App.CustomControls;

public partial class TestElementHeaderControl : ContentView
{
	public static readonly BindableProperty HeaderProperty = BindableProperty.Create(
		propertyName: nameof(Header),
		returnType: typeof(string),
		declaringType: typeof(TestElementHeaderControl));

	public string Header
	{
		get => (string)GetValue(HeaderProperty);
		set => SetValue(HeaderProperty, value);
	}

	public TestElementHeaderControl()
	{
		InitializeComponent();
	}
}