using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GrobelnyKasprzak.MovieCatalogue.DAOSql;

public class SqlModule : IDaoModule
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddDbContext<MovieCatalogueContext>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IDirectorRepository, DirectorRepository>();
    }
}