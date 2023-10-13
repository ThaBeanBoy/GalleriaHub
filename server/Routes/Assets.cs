using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using server.Routes;

using static server.Routes.Assets.ProductAssets;

namespace server.Routes;
public static class AssetsEndpoints
{
    public static string RouterPrefix = "/assets";

    public static RouteGroupBuilder AssetEndpoints(this RouteGroupBuilder group)
    {
        group.MapGroup($"{Assets.ProductAssets.RouterPrefix}").ProductAssetEndpoints();

        return group;
    }
}