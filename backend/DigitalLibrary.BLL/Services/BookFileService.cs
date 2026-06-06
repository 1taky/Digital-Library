using AutoMapper;
using DigitalLibrary.BLL.DTOs.Responses;
using DigitalLibrary.BLL.Exceptions;
using DigitalLibrary.BLL.Interfaces;
using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Enums;
using DigitalLibrary.DAL.Interfaces;

namespace DigitalLibrary.BLL.Services;

public class BookFileService : IBookFileService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    private const string UploadsFolderName = "uploads";

    public BookFileService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BookFileResponseDto> UploadCoverAsync(
        int bookId,
        string fileName,
        string contentType,
        long fileSize,
        Stream fileStream)
    {
        await GetBookOrThrowAsync(bookId);

        ValidateFile(fileName, fileSize);

        if (!contentType.StartsWith("image/"))
        {
            throw new BadRequestException("Обкладинка має бути зображенням.");
        }

        BookFile bookFile = await SaveOrReplaceFileAsync(
            bookId,
            fileName,
            contentType,
            fileSize,
            FileCategory.Cover,
            fileStream);

        return _mapper.Map<BookFileResponseDto>(bookFile);
    }

    public async Task<BookFileResponseDto> UploadEBookAsync(
        int bookId,
        string fileName,
        string contentType,
        long fileSize,
        Stream fileStream)
    {
        Book book = await GetBookOrThrowAsync(bookId);

        bool hasElectronicFormat = book.Formats.Any(format =>
    format.FormatType == BookFormatType.Electronic);

        if (!hasElectronicFormat)
        {
            throw new BadRequestException("PDF/EPUB файл можна завантажити тільки якщо книга має електронний формат.");
        }

        ValidateFile(fileName, fileSize);

        if (!IsAllowedEBookContentType(contentType))
        {
            throw new BadRequestException("Дозволені формати електронної книги: PDF або EPUB.");
        }

        BookFile bookFile = await SaveOrReplaceFileAsync(
            bookId,
            fileName,
            contentType,
            fileSize,
            FileCategory.EBook,
            fileStream);

        return _mapper.Map<BookFileResponseDto>(bookFile);
    }

    public async Task<BookFileResponseDto> UploadAudioAsync(
        int bookId,
        string fileName,
        string contentType,
        long fileSize,
        Stream fileStream)
    {
        Book book = await GetBookOrThrowAsync(bookId);

        bool hasAudioFormat = book.Formats.Any(format =>
    format.FormatType == BookFormatType.Audio);

        if (!hasAudioFormat)
        {
            throw new BadRequestException("Аудіофайл можна завантажити тільки якщо книга має аудіоформат.");
        }

        ValidateFile(fileName, fileSize);

        if (!IsAllowedAudioContentType(contentType))
        {
            throw new BadRequestException("Дозволені формати аудіо: MP3 або M4A.");
        }

        BookFile bookFile = await SaveOrReplaceFileAsync(
            bookId,
            fileName,
            contentType,
            fileSize,
            FileCategory.Audio,
            fileStream);

        return _mapper.Map<BookFileResponseDto>(bookFile);
    }

    public async Task<BookFileResponseDto> GetCoverAsync(int bookId)
    {
        BookFile? file = await _unitOfWork.BookFiles
            .GetByBookIdAndCategoryAsync(bookId, FileCategory.Cover);

        if (file == null)
        {
            throw new NotFoundException("Обкладинку книги не знайдено.");
        }

        return _mapper.Map<BookFileResponseDto>(file);
    }

    public async Task<BookFileResponseDto> GetEBookAsync(int bookId)
    {
        Book book = await GetBookOrThrowAsync(bookId);

        bool hasElectronicFormat = book.Formats.Any(format =>
            format.FormatType == BookFormatType.Electronic);

        if (!hasElectronicFormat)
        {
            throw new BadRequestException("Скачування доступне тільки для книг з електронним форматом.");
        }

        BookFile? file = await _unitOfWork.BookFiles
            .GetByBookIdAndCategoryAsync(bookId, FileCategory.EBook);

        if (file == null)
        {
            throw new NotFoundException("Файл електронної книги не знайдено.");
        }

        return _mapper.Map<BookFileResponseDto>(file);
    }

    public async Task<BookFileResponseDto> GetAudioAsync(int bookId)
    {
        Book book = await GetBookOrThrowAsync(bookId);

        bool hasAudioFormat = book.Formats.Any(format =>
            format.FormatType == BookFormatType.Audio);

        if (!hasAudioFormat)
        {
            throw new BadRequestException("Прослуховування доступне тільки для книг з аудіоформатом.");
        }

        BookFile? file = await _unitOfWork.BookFiles
            .GetByBookIdAndCategoryAsync(bookId, FileCategory.Audio);

        if (file == null)
        {
            throw new NotFoundException("Аудіофайл книги не знайдено.");
        }

        return _mapper.Map<BookFileResponseDto>(file);
    }

    private async Task<BookFile> SaveOrReplaceFileAsync(
        int bookId,
        string originalFileName,
        string contentType,
        long fileSize,
        FileCategory fileCategory,
        Stream fileStream)
    {
        string uploadsDirectory = Path.Combine(
            Directory.GetCurrentDirectory(),
            UploadsFolderName,
            "books",
            bookId.ToString());

        Directory.CreateDirectory(uploadsDirectory);

        string extension = Path.GetExtension(originalFileName);

        string storedFileName =
            $"{fileCategory.ToString().ToLower()}_{Guid.NewGuid()}{extension}";

        string fullPath = Path.Combine(uploadsDirectory, storedFileName);

        await using (FileStream outputStream = new FileStream(fullPath, FileMode.Create))
        {
            await fileStream.CopyToAsync(outputStream);
        }

        string relativePath = Path.Combine(
            UploadsFolderName,
            "books",
            bookId.ToString(),
            storedFileName);

        BookFile? existingFile = await _unitOfWork.BookFiles
            .GetByBookIdAndCategoryAsync(bookId, fileCategory);

        if (existingFile != null)
        {
            DeletePhysicalFileIfExists(existingFile.FilePath);

            existingFile.FileName = originalFileName;
            existingFile.FilePath = relativePath;
            existingFile.ContentType = contentType;
            existingFile.FileSize = fileSize;
            existingFile.UploadedAt = DateTime.UtcNow;

            _unitOfWork.BookFiles.Update(existingFile);
            await _unitOfWork.SaveChangesAsync();

            return existingFile;
        }

        BookFile bookFile = new BookFile
        {
            BookId = bookId,
            FileName = originalFileName,
            FilePath = relativePath,
            ContentType = contentType,
            FileSize = fileSize,
            FileCategory = fileCategory,
            UploadedAt = DateTime.UtcNow
        };

        await _unitOfWork.BookFiles.AddAsync(bookFile);
        await _unitOfWork.SaveChangesAsync();

        return bookFile;
    }

    private async Task<Book> GetBookOrThrowAsync(int bookId)
    {
        Book? book = await _unitOfWork.Books.GetByIdDetailedAsync(bookId);

        if (book == null)
        {
            throw new NotFoundException("Книгу не знайдено.");
        }

        return book;
    }

    private static void ValidateFile(
        string fileName,
        long fileSize)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new BadRequestException("Файл є обов'язковим.");
        }

        if (fileSize <= 0)
        {
            throw new BadRequestException("Файл порожній.");
        }

        const long maxFileSize = 100 * 1024 * 1024;

        if (fileSize > maxFileSize)
        {
            throw new BadRequestException("Файл занадто великий. Максимальний розмір — 100 MB.");
        }
    }

    private static bool IsAllowedEBookContentType(string contentType)
    {
        return contentType == "application/pdf" ||
               contentType == "application/epub+zip";
    }

    private static bool IsAllowedAudioContentType(string contentType)
    {
        return contentType == "audio/mpeg" ||
               contentType == "audio/mp4" ||
               contentType == "audio/x-m4a";
    }

    private static void DeletePhysicalFileIfExists(string relativePath)
    {
        string fullPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            relativePath);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}