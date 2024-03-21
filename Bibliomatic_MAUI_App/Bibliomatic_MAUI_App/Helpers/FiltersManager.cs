namespace Bibliomatic_MAUI_App.Helpers
{
    public enum OrderBy
    {
        Ascending,
        Descending,
    }

    public class FiltersManager
    {
        List<IDataFilter> dataFilters = new List<IDataFilter>();
        private string propertyForOrder;
        private OrderBy orderBy;
        private bool isCountRecords;

        public void AddStringDataFilter(string propertyName, object propertyValue, bool isEqual)
        {
            var stringDataFilter = new StringDataFilter(GetLowerCasePropertyName(propertyName), (string)propertyValue, isEqual);
            dataFilters.Add(stringDataFilter);
        }

        public void AddDefaultDataFilter(string propertyName, object propertyValue, bool isEqual)
        {
            var defaultDataFilter = new DefaultDataFilter(GetLowerCasePropertyName(propertyName), propertyValue, isEqual);
            dataFilters.Add(defaultDataFilter);
        }

        public void AddCollectionDataFilter(string collectionName, string propertyName, object propertyValue, bool notInCollection)
        {
            var collectionDataFilter = new CollectionDataFilter(GetLowerCasePropertyName(collectionName), GetLowerCasePropertyName(propertyName), propertyValue, notInCollection);
            dataFilters.Add(collectionDataFilter);
        }

        public void AddOrderFilter(string propertyForOrder, OrderBy orderBy)
        {
            this.propertyForOrder = GetLowerCasePropertyName(propertyForOrder);
            this.orderBy = orderBy;
        }

        public void AddCountFilter() => isCountRecords = true;

        private string GetLowerCasePropertyName(string propertyName)
        {
            return char.ToLower(propertyName[0]) + propertyName.Substring(1);
        }

        public void ClearCurrentFilters()
        {
            dataFilters.Clear();
            propertyForOrder = string.Empty;
        }

        public string CreateFiltersUrl()
        {
            string filtersUrl = string.Empty;

            if (dataFilters.Count != 0)
            {                
                bool isFirstFilter = true;

                foreach (var filter in dataFilters)
                {
                    string currentFilter = filter.GetFilterUrl();

                    if(!string.IsNullOrEmpty(currentFilter))
                    {
                        if (isFirstFilter) filtersUrl = "&$filter=";
                        if (!isFirstFilter) filtersUrl += " and ";                       

                        filtersUrl += currentFilter;
                        isFirstFilter = false;
                    }
                }
            }

            if (!string.IsNullOrEmpty(propertyForOrder))
            {
                string orderType = orderBy == OrderBy.Ascending ? "asc" : "desc";
                filtersUrl += $"&$orderBy={propertyForOrder} {orderType}";
            }

            if (isCountRecords) filtersUrl += "&$count=true";

            return filtersUrl;
        }
    }
}
