namespace DigitalLibrary.API.Models.Responses;

public class CursorPagedResultModel<T>
{
    public List<T> Items { get; set; } = new();

    public int? NextCursor { get; set; }

    public bool HasMore { get; set; }
}