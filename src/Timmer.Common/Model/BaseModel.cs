namespace Timmer.Common.Model;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
///     The base for every Entity in this project
/// </summary>
public abstract record BaseModel : IModel {
	protected BaseModel(BaseModel model) {
		Id = model.Id;
	}

	[Column("id")]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public Guid Id { get; init; }
}
