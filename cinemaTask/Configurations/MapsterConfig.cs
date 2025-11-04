using cinemaTask.Models;
using cinemaTask.ViewModels;
using Mapster;

namespace cinemaTask.Configurations
{
    public static class MapsterConfig
    {
        public static void RegisterMapsterConfig(this IServiceCollection services)
        {
            TypeAdapterConfig<ApplicationUser, ApplicationUserVM>
                    .NewConfig()
                    .Map(d => d.FullName, s => $"{s.Name} ")
                    .TwoWays();
        }
    }
}

