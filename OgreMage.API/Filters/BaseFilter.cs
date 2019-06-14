namespace OgreMage.API.Filters
{
    /// <summary>
    /// Filtro padrão da aplicação.
    /// </summary>
    public class BaseFilter
    {
        private int _page = 1;
        public int Page
        {
            get => _page;
            set => _page = value <= 0 ? 1 : value;
        }

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value <= 0 ? 1 : value >= 100 ? 100 : value;
        }
    }
}
