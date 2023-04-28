namespace WildlifeMortalities.Data.Entities.Authorizations;

public sealed class AuthorizationResult
{
    private AuthorizationResult(
        Authorization authorization,
        IEnumerable<ViolationResult> violations,
        bool isApplicable
    )
    {
        Authorization = authorization;
        Violations = violations;
        IsApplicable = isApplicable;
    }

    private AuthorizationResult(Authorization authorization)
        : this(authorization, Array.Empty<ViolationResult>()) { }

    private AuthorizationResult(
        Authorization authorization,
        IEnumerable<ViolationResult> violations
    )
        : this(authorization, violations, true) { }

    public Authorization Authorization { get; }
    public IEnumerable<ViolationResult> Violations { get; }
    public bool HasViolations => Violations.Any();
    public bool IsApplicable { get; }

    public static AuthorizationResult IsLegal(Authorization authorization) => new(authorization);

    public static AuthorizationResult NotApplicable(Authorization authorization) =>
        new(authorization, Array.Empty<ViolationResult>(), false);

    public static AuthorizationResult IsIllegal(
        Authorization authorization,
        IEnumerable<ViolationResult> violations
    ) => new(authorization, violations);
}
