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
    member this.Post([<FromBody>] _ToDoItem : Customer) =
        if (base.ModelState.IsValid) then 
            if not( isNull _ToDoItem.Name ) then
                if ( _ToDoItem.Id <> 0 ) then //check if the ID is set
                    ActionResult<IActionResult>(base.BadRequest("BAD REQUEST, the ToDoItemID is autoincremented")) // the ToDoItem is autoincremented
                else 
                        Initialize(this._Context)
                        this._Context.ToDoItems.Add(_ToDoItem) |> ignore
                        this._Context.SaveChanges() |> ignore
                        ActionResult<IActionResult>(base.Ok(this._Context.ToDoItems.Last()))
            else
                ActionResult<IActionResult>(base.BadRequest("BAD REQUEST!, the field Initials can not be null"))                    
        else
            ActionResult<IActionResult>(base.BadRequest(base.ModelState))

    [<DefaultValue>]
    val mutable _Context : CustomerContext

    [<DefaultValue>]
    val mutable _logger : ILogger<WeatherForecastController>