using Microsoft.Extensions.Options;

namespace WildlifeMortalities.Shared.Services.Files;

public class FileServiceOptions
{
    public string RootDirectory { get; set; } = string.Empty;
}

public class FileService : IFileService
{
    private readonly FileServiceOptions _options;
    private const string PdfDirectory = "PDFs";
    private const string ImageDirectory = "Images";

    public FileService(IOptions<FileServiceOptions> options)
    {
        _options = options.Value;
        if (string.IsNullOrWhiteSpace(_options.RootDirectory))
        {
            throw new Exception("The RootDirectory value in appsettings is empty");
        }
    }

    public async Task Save(
        string reportHumanReadableId,
        FileType fileType,
        byte[] file,
        string fileName
    )
    {
        var subdirectory = fileType == FileType.Pdf ? PdfDirectory : ImageDirectory;
        var directory = Path.Combine(_options.RootDirectory, reportHumanReadableId, subdirectory);
        Directory.CreateDirectory(directory);
        var path = Path.Combine(directory, fileName);
        var version = 1;
        if (fileType == FileType.Pdf)
        {
            while (File.Exists(path))
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                var fileExtension = Path.GetExtension(fileName);
                path = Path.Combine(
                    directory,
                    $"{fileNameWithoutExtension}-{version++}.{fileExtension}"
                );
            }
        }

        await File.WriteAllBytesAsync(path, file);
    }

    public async Task<(FileType, byte[])> Get(string reportHumanReadableId)
    {
        throw new NotImplementedException();
    }
}
