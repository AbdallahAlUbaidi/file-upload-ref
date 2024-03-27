using FileUpload.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FileUpload.Controllers;

[ApiController]
[Route("/api/v1/images")]
public class ImageController(IStorage storage) : ControllerBase
{

	private readonly IStorage _Storage = storage;
	private readonly List<string> AllowedExtensions = ["jpg", "jpeg", "png", "gif", "bmp", "svg"];

	private readonly long MaxImageSize = 1024 * 1024 * 2;

	[HttpPost]
	[Route("upload")]
	public async Task<IActionResult> Upload(IFormFile image)
	{

		if (IsImageEmpty(image))
			return BadRequest(new { message = "Image file is empty" });

		if (IsImageTooBig(image))
			return BadRequest(new { message = $"Image file is too big, max image size is {MaxImageSize / 1024 * 1024} MegaBytes" });

		string fileExtension = Path.GetExtension(image.FileName);

		if (!IsFileExtensionSupported(fileExtension))
			return BadRequest(new
			{
				message = "File extension is not supported",
				supportedFileExtensions = AllowedExtensions
			});

		await _Storage.SaveAsync(image.OpenReadStream(), fileExtension);

		return Ok();
	}

	private bool IsFileExtensionSupported(string fileExtension)
	{
		string sanitizedFileExtension = fileExtension.TrimStart('.').ToLower();

		return AllowedExtensions.Contains(sanitizedFileExtension);
	}

	private static bool IsImageEmpty(IFormFile image)
	{
		return image is null || image.Length == 0;
	}

	private bool IsImageTooBig(IFormFile image)
	{
		long size = image.Length;

		return size > MaxImageSize;
	}
}


public record Dimensions(int Width, int Height);