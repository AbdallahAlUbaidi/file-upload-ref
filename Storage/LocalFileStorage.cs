namespace FileUpload.Storage;

using System.IO;
using System.Threading.Tasks;
using FileUpload.Contracts;


public class LocalStorage : IStorage
{
	private readonly IConfiguration _Config;
	private readonly string UploadDir;

	public LocalStorage(IConfiguration config)
	{
		_Config = config;

		UploadDir = GetFileUploadPath();
	}

	public Task DeleteFileAsync(string fileIdentifier)
	{
		throw new NotImplementedException();
	}

	public Task<FileStream> GetFileAsync(string fileIdentifier)
	{
		throw new NotImplementedException();
	}

	public async Task<string> SaveAsync(string fileIdentifier, Stream fileStream, string fileExtension)
	{
		if (!Directory.Exists(UploadDir))
			Directory.CreateDirectory(UploadDir);

		string filePath = Path.Combine(UploadDir, fileIdentifier) + fileExtension;
		using var file = new FileStream(filePath, FileMode.Create, FileAccess.Write);

		await fileStream.CopyToAsync(file);

		return filePath;
	}

	public Task<string> SaveAsync(Stream fileStream, string fileExtension)
	{
		string fileIdentifier = GenerateUniqueFileIdentifier();

		return SaveAsync(fileIdentifier, fileStream, fileExtension);
	}

	private static string GenerateUniqueFileIdentifier()
	{
		string uniqueName = $"{DateTime.Now:yyyyMMddHHmmssfff}";
		return uniqueName;
	}

	private string GetFileUploadPath()
	{
		string? path = _Config["FileStorage:local:uploadDir"];

		if (path is not null)
			return Path.Combine(path);

		return Path.Combine("./uploads");
	}

}
