using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SampleApp.Models
{
    public class Organisation
    {
        public int Id { get; set; }
        public string? OrganisationName { get; set; }
        public string? Location { get; set; }
        public string? OrganisationImageBase64 { get; set; }
        [JsonIgnore]
        public byte[]? OrganisationByteImage { get; set; }

        [NotMapped]
        [JsonIgnore]
        public IFormFile? OrganisationImage { get; set; }
    }
}
