import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';


import { AppComponent } from './app.component';
import { AuthService } from './auth.service';
import { BackendService } from './backend.service';
import { MessageService } from './message.service';


@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [ AuthService, BackendService, MessageService],
  bootstrap: [AppComponent]
})
export class AppModule { }
