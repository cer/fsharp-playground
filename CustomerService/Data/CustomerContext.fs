namespace CustomerService

open CustomerService.Models
open Microsoft.EntityFrameworkCore
open System.Linq
open CustomerService.Models

module DataContext =

    type CustomerContext(options : DbContextOptions<CustomerContext>) =
        inherit DbContext(options)

        [<DefaultValue>]
        val mutable Customers : DbSet<Customer>
        member public this._Customers      with    get()   = this.Customers
                                           and     set value  = this.Customers <- value

        member this.CustomerExist (id:int) = this.Customers.Any(fun x -> x.Id = id)

        member this.GetCustomer (id:int) : Customer option = let c = this.Customers.Find(id)
                                                             if box c <> null then Some(c) else None

    let Initialize (context : CustomerContext) =
        context.Database.EnsureCreated() |> ignore
