module Models
open System
type User = {
    FirstName : string; LastName : string;
    Email : string;
    Password : string option;
    Role : string;
    JoinDate : DateTime option;
    ProfilePictureUrl : string option
}