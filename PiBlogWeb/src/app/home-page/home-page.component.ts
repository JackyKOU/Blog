import { Component, OnInit } from '@angular/core';
import {Post,PostsQueryParameters} from '../Shared/interface';
import {PostsService} from '../posts.service'
@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {
  posts:Post[];
  postParameter:PostsQueryParameters = {Page :1,Limit : 10};

  constructor(
    private postService:PostsService
    ) { }

  ngOnInit(): void {
    this.postService.GetPagedPosts(this.postParameter).subscribe(
      resp=>{
        console.log(resp);
      }
    );
  }

}
