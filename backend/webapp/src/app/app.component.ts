import { Component } from '@angular/core';
import { BackendService } from './backend.service';
import { Observable } from 'rxjs/Observable';
import { UserResponse, ApplicationState } from './models';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  state$: Observable<ApplicationState>;
  title = 'app';

  constructor(private backend: BackendService) {

  }

  ngOnInit() {
    this.backend.fetch();
    this.state$ = this.backend.getState();
  }

  addList() {
    this.backend.addList();
  }

  editListTitle(list) {
    list.editing = true;
  }

  updateListTitle(list, newName) {
    console.log('save');
    console.log(list);
    console.log(newName.value);
  }

}
