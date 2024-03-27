using FileUpload.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FileUpload.Controllers;

[ApiController]
[Route("/api/v1/files")]
public class FileController(IStorage storage) : ControllerBase
{

	private readonly IStorage _Storage = storage;

	private readonly List<string> AllowedExtensions = ["pdf"];

	[HttpPost]
	[Route("upload")]
	public async Task<IActionResult> Upload(IFormFile file)
	{

		if (file is null || file.Length == 0)
			return BadRequest(new { message = "No file or empty file sent" });

		var fileExtension = Path.GetExtension(file.FileName);

		if (!IsFileExtensionSupported(fileExtension))
			return BadRequest(new
			{
				message = "File extension is not supported",
				supportedFileExtensions = AllowedExtensions
			});

		await _Storage.SaveAsync(file.OpenReadStream(), fileExtension);

		return Ok();
	}

	private bool IsFileExtensionSupported(string fileExtension)
	{
		string sanitizedFileExtension = fileExtension.TrimStart('.').ToLower();

		return AllowedExtensions.Contains(sanitizedFileExtension);
	}
}