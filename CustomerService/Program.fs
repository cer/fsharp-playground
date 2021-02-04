namespace CustomerService

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open CustomerService.DataContext
open Microsoft.Extensions.DependencyInjection

module Program =
    let exitCode = 0

    let CreateHostBuilder args =
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(fun webBuilder ->
                webBuilder.UseStartup<Startup>() |> ignore
            )

    [<EntryPoint>]
    let main args =
        Console.Write("Starting\n")
        let host = CreateHostBuilder(args).Build()
        use scope = host.Services.CreateScope()
        
        let services = scope.ServiceProvider
        Console.Write("Starting 2\n")
        let context = services.GetRequiredService<CustomerContext>()
        Initialize(context) |> ignore

        host.Run()
        exitCode
