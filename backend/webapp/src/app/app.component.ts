import { Component } from '@angular/core';
import { BackendService } from './backend.service';
import { Observable } from 'rxjs/Observable';
import { UserResponse, ApplicationState, MarkRequestState, ListItemObject } from './models';
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

  addList(shareName) {
    console.log('addList');
    console.log(shareName);
    this.backend.addList(shareName);
  }

  deleteList(id) {
    this.backend.deleteList(id);
  }

  editListTitle(list) {
    list.editing = true;
  }

  addItem(listId, title, $event) {
    console.log('add');
    console.log(listId);
    console.log(title.value);
    $event.preventDefault();
    this.backend.addItem(listId, title.value);
  }

  updateListTitle(list, newName) {
    console.log('save');
    console.log(list);
    console.log(newName.value);
    this.backend.renameList(list, newName.value);
  }

  changeItemState(listId, item: ListItemObject, state) {
    item.s = state;
    console.log('change');
    console.log(listId);
    console.log(item);
    console.log(state);
    this.backend.changeItemState(listId, item.n, state);
  }

  isCompleted(item) {
    return item.s === MarkRequestState.Complete;
  }

  isActive(item) {
    return item.s === MarkRequestState.Active;
  }

}
