export interface Post{
    id:Number;
    title:String;
    author:String;
    body:String;
    summary:String;
    creationTime:Date;
}

export interface PostDetail{
    id:Number;
    title:String;
    author:String;
    body:String;
    creationTime:Date;
    prevTitle:String;
    prevId:Number;
    nextTitle:String;
    nextId:Number;
}

export interface Response<T>
{
    msg:String;
    success:Boolean;
    timestamp:Date;
    content: T;
}

export interface PostsQueryParameters{
    page:Number;
    limit:Number;
}

export interface PaginatedList<T>
{
    pageSize:Number;
    pageIndex:Number;
    totalItemCount:Number;
    pageCount:Number;
    hasPrevious:Boolean;
    hasNext:Boolean;
    dataList:Array<T>
}