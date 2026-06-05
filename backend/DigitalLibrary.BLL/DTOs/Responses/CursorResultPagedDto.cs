namespace DigitalLibrary.BLL.DTOs.Responses;

public class CursorPagedResultDto<T>
{
    public List<T> Items { get; set; } = new();

    public int? NextCursor { get; set; }

    public bool HasMore { get; set; }
}