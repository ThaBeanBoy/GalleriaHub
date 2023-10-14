using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routes;

public static class Sales
{
    public static string RouterPrefix = "/sales";

    public static RouteGroupBuilder CartEndpoints(this RouteGroupBuilder group)
    {
        return group;
    }
}