import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import{ HttpClient} from '@angular/common/http';
import {PostsQueryParameters,Response,Post,PostDetail} from './Shared/interface';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class PostsService {
  postsQueryUrl = `${environment.apiUrlBase}/blog/query`;
  constructor(private http:HttpClient) { }

  public GetPagedPosts(args?:any|PostsQueryParameters){
    return this.http.get(
      this.postsQueryUrl,
      {
        params:args
      }
    );
  }
}
