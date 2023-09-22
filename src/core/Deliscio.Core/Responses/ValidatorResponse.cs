using System.ComponentModel.DataAnnotations;

namespace Deliscio.Core.Responses;

public class ValidatorResponse
{
    public bool IsValid { get; set; }

    public List<ValidationResult> ValidationResults { get; set; }
}