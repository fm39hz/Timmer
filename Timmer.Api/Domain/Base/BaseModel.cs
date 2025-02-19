namespace Timmer.Api.Domain.Base;

using System.ComponentModel.DataAnnotations.Schema;

public record BaseModel {
	[Column("id")]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public Guid Id { get; init; }
}
