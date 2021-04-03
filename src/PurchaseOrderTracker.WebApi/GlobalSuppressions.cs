// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "Irellevant for Swagger docs", Scope = "member", Target = "~F:PurchaseOrderTracker.WebApi.StartupExtensions.ApplicationBuilderExtensions.SwaggerExtensions.SwaggerSpecUrl")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Doesn't adhere to ASP.Net Core semantics", Scope = "member", Target = "~M:PurchaseOrderTracker.WebApi.Startup.Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder,AutoMapper.IConfigurationProvider)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:Statement must not use unnecessary parenthesis", Justification = "Parenthesis is necessary. Bug in rule?", Scope = "member", Target = "~M:PurchaseOrderTracker.WebApi.Features.Account.Refresh.CommandHandler.TryGetPrincipalFromToken(System.String)~System.Security.Claims.ClaimsPrincipal")]

