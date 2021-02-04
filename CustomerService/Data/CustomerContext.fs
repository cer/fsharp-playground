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

        //returns if the Item exists
        member this.CustomerExist (id:int) = this.Customers.Any(fun x -> x.Id = id)

        //Returns the Item with the given id
        member this.GetCustomer (id:int) = this.Customers.Find(id)

    let Initialize (context : CustomerContext) =
        //context.Database.EnsureDeleted() |> ignore //Deletes the database
        context.Database.EnsureCreated() |> ignore //check if the database is created, if not then creates it
        //default Items for testing
        let tdItems : Customer[] =
            [|
                { Id = 0; Name = "Do the software";  }
                { Id = 0; Name = "Create the Article";   }
                { Id = 0; Name = "Upload to the internet";  }
            |]

        if not(context.Customers.Any()) then
                context.Customers.AddRange(tdItems) |> ignore
                context.SaveChanges() |> ignore
