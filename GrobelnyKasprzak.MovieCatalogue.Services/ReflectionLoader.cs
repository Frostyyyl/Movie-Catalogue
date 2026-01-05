using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GrobelnyKasprzak.MovieCatalogue.Services
{
    public class ReflectionLoader
    {
        private readonly Assembly _daoAssembly;
        public ReflectionLoader()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();

            string? dllName = config["AppSettings:DaoLibrary"];

            if (string.IsNullOrEmpty(dllName))
            {
                throw new Exception("Nie skonfigurowano nazwy biblioteki DAO w appsettings.json");
            }

            string path = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(path, dllName);

            if (File.Exists(fullPath))
            {
                _daoAssembly = Assembly.LoadFrom(fullPath);
            }
            else
            {
                throw new FileNotFoundException($"Nie znaleziono pliku biblioteki: {fullPath}");
            }
        }

        public void Register(IServiceCollection services)
        {
            var moduleType = _daoAssembly.GetTypes()
                .FirstOrDefault(t => typeof(IDaoModule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            if (moduleType == null)
            {
                throw new Exception($"Nie znaleziono implementacji {nameof(IDaoModule)} w bibliotece.");
            }

            var instance = Activator.CreateInstance(moduleType);

            if (instance is IDaoModule module)
            {
                module.RegisterServices(services);
            }
            else
            {
                throw new Exception($"Nie udało się utworzyć instancji {moduleType.Name}.");
            }
        }
    }
}
