export interface Post{
    Id:Number;
    Title:String;
    Author:String;
    Body:String;
    CreationTime:Date;
}

export interface PostDetail{
    Id:Number;
    Title:String;
    Author:String;
    Body:String;
    CreationTime:Date;
    PrevTitle:String;
    PrevId:Number;
    NextTitle:String;
    NextId:Number;
}

export interface Response<T>
{
    Msg:String;
    Success:Boolean;
    Timestamp:Date;
    Content: T;
}