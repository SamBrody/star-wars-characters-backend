namespace StarWars.Characters.Presentation.Utils.Paging;

public class PageInfo {
    /// <summary>
    /// Количество элементов
    /// </summary>
    public int Items { get; init; }
    
    /// <summary>
    /// Номер страницы
    /// </summary>
    public int Page { get; init; }
    
    /// <summary>
    /// Количество элементов на одной странице
    /// </summary>
    public int PerPage { get; init; }
    
    /// <summary>
    /// Количество страниц
    /// </summary>
    public long Pages { get; init; }

    public static PageInfo Create(int items, int page, int perPage) {
        return new() { Items = items, Page = page, PerPage = perPage, Pages = items / perPage };
    }
}