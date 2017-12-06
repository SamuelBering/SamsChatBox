import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms/src/directives/ng_form';
import { ChatService } from '../services/chat.service';

@Component({
  selector: 'app-root',
  templateUrl: './root.component.html',
  styleUrls: ['./root.component.scss']
})
export class RootComponent implements OnInit {

  errors: string;
  isRequesting: boolean;
  submitted: boolean = false;

  constructor(private chatService: ChatService) { }

  ngOnInit() {
  }

  onSubmitNewRoom(f: NgForm) {
    if (f.valid) {
      let roomName = f.value['roomName'];
      let id = this.createRoom(roomName);
    }
  }

  createRoom(title: string) {
    this.submitted = true;
    this.isRequesting = true;
    this.errors = '';
    this.chatService.createRoom(title)
      .finally(() => this.isRequesting = false)
      .subscribe(
      result => {
        let id = result.id;

        // if (result) {
        //   this.router.navigate(['/login'], { queryParams: { brandNew: true, email: value.email } });
        // }
      },
      errors => this.errors = errors);

  }

}
