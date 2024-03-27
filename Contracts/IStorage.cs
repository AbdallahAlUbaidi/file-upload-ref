namespace FileUpload.Contracts;

public interface IStorage
{
	public Task<string> SaveAsync(string fileIdentifier, Stream fileStream, string fileExtension);

	public Task<string> SaveAsync(Stream fileStream, string fileExtension);

	public Task<FileStream> GetFileAsync(string fileIdentifier);

	public Task DeleteFileAsync(string fileIdentifier);
}