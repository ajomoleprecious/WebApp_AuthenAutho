using Microsoft.AspNetCore.Authorization;

namespace WebApp_UnderTheHood.Authorization
{
    // This is a custom requirement class that inherits from IAuthorizationRequirement
    public class HRManagerProbationRequirement : IAuthorizationRequirement
    {
        // This constructor takes in the number of months of probation
        public HRManagerProbationRequirement(int probationMonths)
        {
            ProbationMonths = probationMonths;
        }
        // This property is used to store the number of days of probation
        public int ProbationMonths { get; }
    }
    // This is a custom handler class that inherits from AuthorizationHandler<TRequirement>
    // This class is used to check if the user has been employed for the required number of months
    public class HRManagerProbationRequirementHandler : AuthorizationHandler<HRManagerProbationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRManagerProbationRequirement requirement)
        {
           if(!context.User.HasClaim(claim => claim.Type == "EmploymentDate"))
            {
                return Task.CompletedTask; // This means that the user is not an employee
            }

           if(DateTime.TryParse(context.User.FindFirst(claim => claim.Type == "EmploymentDate")?.Value, out DateTime employmentDate))
            {
                var period = DateTime.Now - employmentDate;
                if(period.TotalDays > 30 * requirement.ProbationMonths)
                {
                    context.Succeed(requirement); // This means that the user has been employed for the required number of months
                }
            }
           return Task.CompletedTask; // This means that the user has not been employed for the required number of months
        }
    }
}
