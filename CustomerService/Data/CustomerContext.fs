namespace CustomerService

open CustomerService.Models
open Microsoft.EntityFrameworkCore
open System.Linq
open CustomerService.Models

module DataContext =

    type CustomerContext(options : DbContextOptions<CustomerContext>) = 
        inherit DbContext(options)
    
        [<DefaultValue>]
        val mutable ToDoItems : DbSet<Customer>
        member public this._ToDoItemsX     with    get()   = this.ToDoItems 
                                           and     set value  = this.ToDoItems <- value 

        //returns if the Item exists 
        member this.ToDoItemExist (id:int) = this.ToDoItems.Any(fun x -> x.Id = id)

        //Returns the Item with the given id
        member this.GetToDoItem (id:int) = this.ToDoItems.Find(id)

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

        if not(context.ToDoItems.Any()) then
                context.ToDoItems.AddRange(tdItems) |> ignore
                context.SaveChanges() |> ignore  