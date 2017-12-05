import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms/src/directives/ng_form';

@Component({
  selector: 'app-root',
  templateUrl: './root.component.html',
  styleUrls: ['./root.component.scss']
})
export class RootComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  onSubmitNewRoom(f: NgForm) {
    let roomName = f.value['roomName'];

  }

}
