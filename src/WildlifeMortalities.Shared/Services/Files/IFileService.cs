namespace WildlifeMortalities.Shared.Services.Files;
internal interface IFileService
{
    Task<(FileType, byte[])> Get(string reportHumanReadableId);
    Task Save(string reportHumanReadableId, FileType fileType, byte[] file, string fileName);
}
