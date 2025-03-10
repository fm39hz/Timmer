using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

internal sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer {
	private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider = authenticationSchemeProvider;

	public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken) {
		var authenticationSchemes = await _authenticationSchemeProvider.GetAllSchemesAsync();

		if (authenticationSchemes.Any(static authScheme => authScheme.Name == "Bearer")) {
			var requirements = new Dictionary<string, OpenApiSecurityScheme> {
				["Bearer"] = new OpenApiSecurityScheme {
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					In = ParameterLocation.Header,
					BearerFormat = "JWT"
				}
			};

			document.Components ??= new OpenApiComponents();
			document.Components.SecuritySchemes = requirements;

			foreach (var operation in document.Paths.Values.SelectMany(static path => path.Operations)) {
				operation.Value.Security.Add(new OpenApiSecurityRequirement {
					[new OpenApiSecurityScheme {
						Reference = new OpenApiReference {
							Id = "Bearer",
							Type = ReferenceType.SecurityScheme
						}
					}] = Array.Empty<string>()
				});
			}
		}
	}
}
