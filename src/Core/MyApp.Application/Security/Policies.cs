using System.Collections.Generic;

namespace MyApp.Application.Security;

public static class PolicyConstants
{
    public const string PolicyClaimType = "policy";
}

public static class Policies
{
    private const string Prefix = "Policies.";


    public static class Users
    {
        private const string Base = Prefix + "Users.";
        public const string Create = Base + "Create";
        public const string Update = Base + "Update";
        public const string Delete = Base + "Delete";
        public const string View = Base + "View";
        public const string List = Base + "List";
    }

    public static class Tenants
    {
        private const string Base = Prefix + "Tenants.";
        public const string Create = Base + "Create";
        public const string Update = Base + "Update";
        public const string Delete = Base + "Delete";
        public const string View = Base + "View";
        public const string List = Base + "List";
    }

}
