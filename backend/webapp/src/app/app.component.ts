import { Component } from '@angular/core';
import { BackendService } from './backend.service';
import { Observable } from 'rxjs/Observable';
import { UserResponse } from './models';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  lists$: Observable<UserResponse>;
  title = 'app';

  constructor(private backend: BackendService) {

  }

  ngOnInit() {
    this.lists$ = this.backend.getUser();
  }
}
