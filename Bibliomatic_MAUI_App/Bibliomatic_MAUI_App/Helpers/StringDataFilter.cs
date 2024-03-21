namespace Bibliomatic_MAUI_App.Helpers
{
    public class StringDataFilter : IDataFilter
    {
        string propertyName;
        string propertyValue;
        bool isEqual;

        public StringDataFilter(string propertyName, string propertyValue, bool isEqual)
        {
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
            this.isEqual = isEqual;
        }

        public string GetFilterUrl()
        {
            if (string.IsNullOrEmpty(propertyValue)) return string.Empty;

            string filterUrl = $"contains(tolower({propertyName}),tolower('{propertyValue}'))";

            if (isEqual) return filterUrl;

            return $"NOT({filterUrl})";
        }
    }
}
