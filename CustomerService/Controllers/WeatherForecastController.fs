namespace CustomerService.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open CustomerService

open CustomerService.DataContext
open CustomerService.Models
open System.Transactions;

[<ApiController>]
[<Route("[controller]")>]
type WeatherForecastController private() =
    inherit ControllerBase()

    let summaries = [| "Freezing"; "Bracing"; "Chilly"; "Cool"; "Mild"; "Warm"; "Balmy"; "Hot"; "Sweltering"; "Scorching" |]

    new (logger : ILogger<WeatherForecastController>, context : CustomerContext) as this =
        WeatherForecastController () then
        this._Context <- context
        this._logger <- logger

    [<HttpGet>]
    [<Route("/items")>]
    member this.GetItms() =
        Initialize(this._Context)
        ActionResult<IEnumerable<Customer>>(this._Context.ToDoItems)

    [<HttpPost>]
    [<Route("/items")>]
    member this.Post([<FromBody>] _ToDoItem : Customer) : IActionResult =
        if (base.ModelState.IsValid) then 
            if not( isNull _ToDoItem.Name ) then
                if ( _ToDoItem.Id <> 0 ) then //check if the ID is set
                    upcast base.BadRequest("BAD REQUEST, the ToDoItemID is autoincremented") // the ToDoItem is autoincremented
                else 
                    let newItem = using(new TransactionScope()) ( fun scope -> 
                        this._Context.ToDoItems.Add(_ToDoItem) |> ignore
                        this._Context.SaveChanges() |> ignore
                        let newItem = this._Context.ToDoItems.AsEnumerable().Last()
                        scope.Complete()
                        newItem
                    )
                    upcast base.Ok(newItem)
            else
                upcast base.BadRequest("BAD REQUEST!, the field Initials can not be null")
        else
            upcast base.BadRequest(base.ModelState)

    [<DefaultValue>]
    val mutable _Context : CustomerContext

    [<DefaultValue>]
    val mutable _logger : ILogger<WeatherForecastController>