namespace Bibliomatic_MAUI_App.Helpers
{
    public class DefaultDataFilter : IDataFilter
    {
        string propertyName;
        object propertyValue;
        bool isEqual;

        public DefaultDataFilter(string propertyName, object propertyValue, bool isEqual)
        {
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
            this.isEqual = isEqual;
        }

        public string GetFilterUrl()
        {
            if (propertyValue is bool boolValue)
            {
                if (boolValue) 
                {
                    propertyValue = "true";
                }
                else
                {
                    propertyValue = "false";
                }
            }

            if (propertyValue == null) return string.Empty;

            string filterUrl = $"{propertyName} eq {propertyValue}";

            if (isEqual) return filterUrl;

            return $"NOT({filterUrl})";
        }
    }
}
