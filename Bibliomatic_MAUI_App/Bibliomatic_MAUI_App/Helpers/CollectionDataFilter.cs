namespace Bibliomatic_MAUI_App.Helpers
{
    public class CollectionDataFilter : IDataFilter
    {
        string collectionName;
        string propertyName;
        object propertyValue;
        bool notInCollection;

        public CollectionDataFilter(string collectionName, string propertyName, object propertyValue, bool notInCollection)
        {
            this.collectionName = collectionName;
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
            this.notInCollection = notInCollection;
        }

        public string GetFilterUrl()
        {
            string filterUrl;

            if (propertyValue is string)
            {
                filterUrl = $"v:contains(tolower(v/{propertyName}),tolower('{propertyValue}'))";                
            }
            else
            {
                var defaultFilter = new DefaultDataFilter(propertyName, propertyValue, true);
                filterUrl = $"v:v/{defaultFilter.GetFilterUrl()}";
            }

            if (notInCollection)
            {
                return $"NOT({collectionName}/any({filterUrl}))";
            }

            return $"{collectionName}/any({filterUrl})";
        }
    }
}
