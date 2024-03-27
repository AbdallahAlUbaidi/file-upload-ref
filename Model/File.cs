using System.ComponentModel.DataAnnotations;

namespace Model;

public class File
{
	Guid Id { get; set; }
	[Required]
	string? Name { get; set; }
	string? Extension { get; set; }

}