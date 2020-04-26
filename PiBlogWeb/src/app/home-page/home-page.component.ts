import { Component, OnInit } from '@angular/core';
import {Post,PostsQueryParameters,Response,PaginatedList} from '../Shared/interface';
import {PostsService} from '../posts.service'
@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {
  posts:Post[];
  postParameter:PostsQueryParameters = {page :1,limit : 10};

  constructor(
    private postService:PostsService
    ) { }

  ngOnInit(): void {
    this.postService.GetPagedPosts(this.postParameter).subscribe(
      (resp:Response<PaginatedList<Post>>) =>{
        if(!resp.success)
        {
          console.error(resp.msg);
        }else{
          this.posts = resp.content.dataList;
          console.log(resp.content.pageCount);
          console.log(resp.content.pageSize);
        }

      }
    );
  }

}
