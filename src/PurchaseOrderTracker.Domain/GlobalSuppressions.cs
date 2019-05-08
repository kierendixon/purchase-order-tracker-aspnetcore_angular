// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Not applicable", Scope = "type", Target = "~T:PurchaseOrderTracker.Domain.Exceptions.PurchaseOrderTrackerException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "Not applicable", Scope = "member", Target = "~M:PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.PurchaseOrderStatus.Fire(PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.PurchaseOrderStatus.Trigger)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "Not applicable", Scope = "member", Target = "~M:PurchaseOrderTracker.Domain.Models.ShipmentAggregate.ShipmentStatus.Fire(PurchaseOrderTracker.Domain.Models.ShipmentAggregate.ShipmentStatus.Trigger)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "Not applicable", Scope = "type", Target = "~T:PurchaseOrderTracker.Domain.Exceptions.PurchaseOrderTrackerException")]

// Third-party code (Entity.cs)
// https://github.com/dotnet/roslyn/issues/3705
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:Braces must not be omitted", Justification = "<Pending>", Scope = "member", Target = "~M:PurchaseOrderTracker.Domain.Models.Entity.Equals(System.Object)~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:Braces must not be omitted", Justification = "<Pending>", Scope = "member", Target = "~M:PurchaseOrderTracker.Domain.Models.Entity.GetHashCode~System.Int32")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Bug", "S2328:\"GetHashCode\" should not reference mutable fields", Justification = "<Pending>", Scope = "member", Target = "~M:PurchaseOrderTracker.Domain.Models.Entity.GetHashCode~System.Int32")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements must appear in the correct order", Justification = "<Pending>", Scope = "member", Target = "~M:PurchaseOrderTracker.Domain.Models.Entity.op_Equality(PurchaseOrderTracker.Domain.Models.Entity,PurchaseOrderTracker.Domain.Models.Entity)~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Code Smell", "S3875:\"operator==\" should not be overloaded on reference types", Justification = "<Pending>", Scope = "member", Target = "~M:PurchaseOrderTracker.Domain.Models.Entity.op_Equality(PurchaseOrderTracker.Domain.Models.Entity,PurchaseOrderTracker.Domain.Models.Entity)~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1513:Closing brace must be followed by blank line", Justification = "<Pending>", Scope = "member", Target = "~M:PurchaseOrderTracker.Domain.Models.Entity.GetHashCode~System.Int32")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S3249:Classes directly extending \"object\" should not call \"base\" in \"GetHashCode\" or \"Equals\"", Justification = "<Pending>", Scope = "member", Target = "~M:PurchaseOrderTracker.Domain.Models.Entity.GetHashCode~System.Int32")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1125:Boolean literals should not be redundant", Justification = "<Pending>", Scope = "member", Target = "~M:PurchaseOrderTracker.Domain.Models.Entity.op_Equality(PurchaseOrderTracker.Domain.Models.Entity,PurchaseOrderTracker.Domain.Models.Entity)~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:Braces must not be omitted", Justification = "<Pending>", Scope = "member", Target = "~M:PurchaseOrderTracker.Domain.Models.Entity.op_Equality(PurchaseOrderTracker.Domain.Models.Entity,PurchaseOrderTracker.Domain.Models.Entity)~System.Boolean")]

// Generated code (GlobalSuppressions.cs)
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1515:Single-line comment must be preceded by blank line", Justification = "<Pending>")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1512:Single-line comments must not be followed by blank line", Justification = "<Pending>")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1404:Code analysis suppression must have justification", Justification = "<Pending>")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:Braces must not be omitted", Justification = "<Pending>", Scope = "member", Target = "~M:PurchaseOrderTracker.Domain.Models.ValueObject.Equals(System.Object)~System.Boolean")]

// Third-party code (ValueObject.cs)
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:Braces must not be omitted", Justification = "<Pending>", Scope = "member", Target = "~M:PurchaseOrderTracker.Domain.Models.ValueObject.EqualOperator(PurchaseOrderTracker.Domain.Models.ValueObject,PurchaseOrderTracker.Domain.Models.ValueObject)~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements must be ordered by access", Justification = "<Pending>", Scope = "member", Target = "~M:PurchaseOrderTracker.Domain.Models.ValueObject.Equals(System.Object)~System.Boolean")]
