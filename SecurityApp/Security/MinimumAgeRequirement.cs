using Microsoft.AspNetCore.Authorization;

namespace SecurityApp.Security
{
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public MinimumAgeRequirement(int age)
        {
            Age = age;
        }

        public int Age { get; }
    }
}
