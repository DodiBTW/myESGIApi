namespace Utils
module Models=
    open System
    type User = {
        FirstName : string; LastName : string;
        Email : string;
        Password : string option;
        Role : string;
        JoinDate : DateTime option;
        ProfilePictureUrl : string option
    }
    type Post = {
        Id : int option
        Title : string
        Description : string option
        AuthorId : int
        ImgUrl : string option
        PostDate : DateTime
    }