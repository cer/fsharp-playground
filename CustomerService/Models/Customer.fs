namespace CustomerService

open System.ComponentModel.DataAnnotations

module Models =
  
    [<CLIMutable>]
    type Customer =
        {
            Id : int
            [<Required>]
            Name : string
        }