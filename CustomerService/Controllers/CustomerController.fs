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
type CustomerController private() =
    inherit ControllerBase()

    new (logger : ILogger<CustomerController>, context : CustomerContext) as this =
        CustomerController () then
        this._Context <- context
        this._logger <- logger

    [<HttpGet>]
    [<Route("/items")>]
    member this.GetItms() =
        ActionResult<IEnumerable<Customer>>(this._Context.Customers)

    [<HttpPost>]
    [<Route("/items")>]
    member this.Post([<FromBody>] _Customer : Customer) : IActionResult =
        if (base.ModelState.IsValid) then
            if not( isNull _Customer.Name ) then
                if ( _Customer.Id <> 0 ) then //check if the ID is set
                    upcast base.BadRequest("BAD REQUEST, the CustomerID is autoincremented") // the Customer is autoincremented
                else
                    let newItem = using(new TransactionScope()) ( fun scope ->
                        this._Context.Customers.Add(_Customer) |> ignore
                        this._Context.SaveChanges() |> ignore
                        let newItem = this._Context.Customers.AsEnumerable().Last()
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
    val mutable _logger : ILogger<CustomerController>
