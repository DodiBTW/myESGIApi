namespace Utils
open Models
module FormatChecker =
    open System
    let IsValidEmail(email : string) : bool = 
        email.Contains("@") && email.Contains(".")
    let IsValidPassword(password : string) : bool = 
        password.Length >= 8
        && password |> Seq.exists Char.IsDigit
        && password |> Seq.exists Char.IsUpper
        && password |> Seq.exists Char.IsLower
    let CheckUser(user : User): bool = 
        // Test function to check user data type (unused)
        match user.Password with
        | Some password -> IsValidPassword(password)
        | None -> true // Debatable
        && IsValidEmail(user.Email)