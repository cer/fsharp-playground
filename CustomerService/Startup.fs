namespace CustomerService

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy;
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open CustomerService.DataContext
open Microsoft.EntityFrameworkCore; // Needed this to get UseSqlServer to exist

type Startup private () =
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration

    member this.ConfigureServices(services: IServiceCollection) =
        services.AddControllers() |> ignore
        services.AddDbContext<CustomerContext>(fun options -> options.UseSqlServer("Data Source=localhost;Initial Catalog=eventuate;User Id=sa;Password=App@Passw0rd").EnableSensitiveDataLogging(true) |> ignore) |> ignore // options.UseInMemoryDatabase("DB_ToDo")

    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore

        app.UseHttpsRedirection() |> ignore
        app.UseRouting() |> ignore

        app.UseAuthorization() |> ignore

        app.UseEndpoints(fun endpoints ->
            endpoints.MapControllers() |> ignore
            ) |> ignore

    member val Configuration : IConfiguration = null with get, set
