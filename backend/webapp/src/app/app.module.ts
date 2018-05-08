import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule, MatCheckboxModule, MatToolbarModule, MatListModule, MatCardModule, MatInputModule } from '@angular/material';
import { AppComponent } from './app.component';
import { AuthService } from './auth.service';
import { BackendService } from './backend.service';
import { MessageService } from './message.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from './token.interceptor';
import { HttpClientModule } from '@angular/common/http';
import { CallbackPipe } from './pipes';

@NgModule({
  declarations: [
    AppComponent, CallbackPipe
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatButtonModule, MatCheckboxModule, MatToolbarModule, MatListModule, MatCardModule, MatInputModule
  ],
  providers: [AuthService, BackendService, MessageService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
