namespace Utils
module FormatTester = 
    open FsUnit.Xunit
    open Xunit
    open FormatChecker
    open Models
    [<Fact>]
    let ``Check if a valid email is valid`` () = 
        IsValidEmail "email@example.com" |> should equal true
    [<Fact>]
    let ``Check if invalid email is valid`` () = 
        IsValidEmail "email@example." |> should equal false
    [<Fact>]
    let ``Check if valid password is valid`` () = 
        IsValidPassword "Test123@" |> should equal true
    [<Fact>]
    let ``Check if invalid password is invalid`` () = 
        IsValidPassword "132dsq2&"  |> should equal false