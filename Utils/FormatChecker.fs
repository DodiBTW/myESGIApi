namespace Utils
open Models
module FormatChecker =
    open System
    open System.Text.RegularExpressions
    let IsValidEmail(email : string) : bool = 
        let regex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
        Regex.IsMatch(email, regex)
    let IsSpecialChar (c : char) : bool = 
        Char.IsSymbol c || Char.IsPunctuation c
    let IsValidPassword(password : string) : bool = 
        password.Length >= 8
        && password |> Seq.exists Char.IsDigit
        && password |> Seq.exists Char.IsUpper
        && password |> Seq.exists Char.IsLower
        && password |> Seq.exists IsSpecialChar
    let CheckUser(user : User): bool = 
        // Test function to check user data type (unused for now)
        match user.Password with
        | Some password -> IsValidPassword(password)
        | None -> true // Debatable
        && IsValidEmail(user.Email)