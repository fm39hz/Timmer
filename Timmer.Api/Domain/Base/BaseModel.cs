namespace Timmer.Api.Domain.Base;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// The base for every Entity in this project
/// </summary>
public abstract record BaseModel : IModel {
	[Column("id")]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public Guid Id { get; init; }
}
