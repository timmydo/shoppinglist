import { Component } from '@angular/core';
import { BackendService } from './backend.service';
import { Observable } from 'rxjs/Observable';
import { UserResponse, ApplicationState, MarkRequestState } from './models';
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

  deleteList(id) {
    this.backend.deleteList(id);
  }

  editListTitle(list) {
    list.editing = true;
  }

  addItem(listId, title) {
    console.log('add');
    console.log(listId);
    console.log(title.value);
    this.backend.addItem(listId, title.value);
  }

  updateListTitle(list, newName) {
    console.log('save');
    console.log(list);
    console.log(newName.value);
    this.backend.renameList(list, newName.value);
  }

  changeItemState(listId, itemName, state) {
    console.log('change');
    console.log(listId);
    console.log(itemName);
    console.log(state);
  }

  isCompleted(item) {
    return item.s === MarkRequestState.Complete;
  }

  isActive(item) {
    return item.s === MarkRequestState.Active;
  }

}
